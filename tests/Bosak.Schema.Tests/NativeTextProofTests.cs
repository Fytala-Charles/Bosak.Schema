// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Tests actual native text member provenance, XML names and finite facet budgets.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 26-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Fixture=Bosak.Schema.Tests.NodeData.Fixture;
using Xunit;
namespace Bosak.Schema.Tests;

/// <summary>Verifies actual Core text facts and original canonical components.</summary>
public sealed class NativeTextProofTests
{
    /// <summary>Checks normalized values, actual named/builtin member proof and foreign rejection.</summary>
    /// <param name="kind">Builtin text type.</param><param name="text">Input.</param><param name="expected">Normalized value.</param><param name="named">Named alias selector.</param>
    [Theory]
    [InlineData("normalizedString"," a\t b "," a  b ",false)][InlineData("normalizedString"," a\t b "," a  b ",true)]
    [InlineData("token"," a\t b ","a b",false)][InlineData("token"," a\t b ","a b",true)]
    [InlineData("language"," en-US ","en-US",false)][InlineData("language"," en-US ","en-US",true)]
    [InlineData("Name"," x:y ","x:y",false)][InlineData("Name"," x:y ","x:y",true)]
    [InlineData("NCName"," 名 ","名",false)][InlineData("NCName"," 名 ","名",true)]
    [InlineData("NMTOKEN"," 1- ","1-",false)][InlineData("NMTOKEN"," 1- ","1-",true)]
    public void Provenance(string kind,string text,string expected,bool named)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+(named?"<xs:simpleType name='N'><xs:restriction base='xs:"+kind+"'/></xs:simpleType>":"")+"<xs:element name='r' type='"+(named?"N":"xs:"+kind)+"'/></xs:schema>";
        using var f=new Fixture(schema,"<r>"+text+"</r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Same(f.Input.Root,fact.Node);Assert.Equal(expected,f.Input.Root.StringValue);Assert.Equal(expected,f.Read(f.Input.Root).StringValue);
        var typed=fact.ReadTypedValue(default).Value;Assert.False(typed.IsList);Assert.True(typed.TryGetSingleAtomic(out var cell));Assert.Equal(XdmValueKind.String,cell.Value.Kind);Assert.Equal(expected,cell.Value.StringValue);Assert.Same(fact.Type,cell.TypeEvidence.ActualType);
        if(named){Assert.Equal(kind,fact.NativeType.DerivationBase!.Name);Assert.Same(f.Provider.TypeComponent(fact.NativeType.DerivationBase),f.Provider.ResolveType(new("http://www.w3.org/2001/XMLSchema",kind),default).Value);}
        f.Run(op=>{var atom=f.Evaluate(op,f.Input.Root);Assert.True(SchemaAtomics.TryGetProvenance(atom,out var proof));Assert.Same(fact.Type,proof!.ActualType);Assert.Same(f.Assessment.Origin,proof.OriginalAssessment);return true;});
        using var other=new Fixture(schema,"<r>"+text+"</r>");Assert.False(other.Session.Advisor!.OwnsTypedValue(typed,other.Input,null));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>typed.TryGetSingleAtomic(out _));
    }
    /// <summary>Checks builtin nonempty token lists carry original NMTOKEN item proof.</summary>
    [Fact]
    public void NmtokensProof()
    {
        using var f=new Fixture("<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:NMTOKENS'/></xs:schema>","<r>a 1-</r>",atoms:16);
        var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;var typed=fact.ReadTypedValue(default).Value;Assert.True(typed.IsList);Assert.False(typed.TryGetSingleAtomic(out _));Assert.Equal("NMTOKENS",fact.NativeType.Name);Assert.Equal(2,typed.AtomicItems.Count());
        var item=f.Provider.ResolveType(new("http://www.w3.org/2001/XMLSchema","NMTOKEN"),default).Value;Assert.All(typed.AtomicItems,c=>{Assert.Same(fact.Type,c.TypeEvidence.DeclaredType);Assert.Same(item,c.TypeEvidence.ActualType);});
    }
    /// <summary>Checks transitive named text aliases preserve the original base and anonymous identities.</summary>
    [Fact]
    public void AnonymousAndNamedIdentities()
    {
        using var f=new Fixture("<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='B'><xs:restriction base='xs:token'/></xs:simpleType><xs:element name='r'><xs:simpleType><xs:restriction base='B'/></xs:simpleType></xs:element></xs:schema>","<r> a </r>");
        var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Null(fact.NativeType.Name);Assert.Equal("B",fact.NativeType.DerivationBase!.Name);Assert.Same(f.Provider.ResolveType(new("","B"),default).Value,f.Provider.TypeComponent(fact.NativeType.DerivationBase));Assert.Equal("a",f.Read(f.Input.Root).StringValue);
    }
    /// <summary>Checks shared pattern work and cancellation fail without an unhandled diagnostic launch.</summary>
    [Fact]
    public void BudgetAndCancellation()
    {
        var schema=NativeDecimalSchema.Compile("<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='T'><xs:restriction base='xs:token'><xs:pattern value='a+'/></xs:restriction></xs:simpleType><xs:element name='r' type='T'/></xs:schema>",new(256,256),maxStringCharacters:256).Value!;
        var facets=schema.Root.Type.TextFacets!;Assert.Equal(NativeOutcome.Unsupported,Assert.Throws<NativeProfileException>(()=>facets.Accepts("aaa",new(default,1))).Outcome);
        using var c=new CancellationTokenSource();c.Cancel();Assert.ThrowsAny<OperationCanceledException>(()=>facets.Accepts("a",new(c.Token)));Assert.True(facets.Accepts("aaa",new(default)));
    }
    /// <summary>Checks complete invalid text assessment issues no original node facts.</summary>
    [Fact]
    public void InvalidFacts()
    {
        using var f=new Fixture("<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:NCName'/></xs:schema>","<r>a:b</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Snapshot);Assert.Null(f.Input.FactsFor((NativeSessionNode)f.Input.Root));
    }
}
