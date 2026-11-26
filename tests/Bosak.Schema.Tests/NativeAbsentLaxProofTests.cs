// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies declaration-absent lax node provenance and retained assessment boundaries.
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
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.AbsentLax;
/// <summary>Proves actual absent declarations and owned type evidence.</summary>
public sealed class NativeAbsentLaxProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private const string S="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'/><xs:element name='d' type='xs:decimal'/><xs:attribute name='id' type='xs:ID'/><xs:attribute name='ref' type='xs:IDREF'/></xs:schema>";
    /// <summary>Unknown nodes have no declaration and retain owned types, original values and expiry.</summary>
    /// <param name="attribute">Selects an attribute rather than an element.</param>
    /// <param name="nested">Selects a nested original owner.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void AbsentAuthority(bool attribute,bool nested)
    {
        using var f=new Fixture(S,attribute?(nested?"<r><u z='  x  '/></r>":"<r z='  x  '/>"):(nested?"<r><u><v>  x  </v></u></r>":"<r><u>  x  </u></r>"));
        Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var node=(NativeSessionNode)(attribute?f.Attribute(nested?[0]:[],"","z"):f.Element(nested?[0,0]:[0]));
        var fact=f.Input.FactsFor(node)!;Assert.Null(fact.Declaration);Assert.Null(fact.GoverningDeclaration);Assert.True(f.Provider.Owns(fact.Type));
        Assert.Equal((X,attribute?"anySimpleType":"anyType"),node.SchemaTypeAnnotation);Assert.Null(attribute?node.SchemaAttributeDeclaration:node.SchemaElementDeclaration);
        var item=Assert.Single(fact.ReadTypedValue(default).Value.AtomicItems);Assert.Equal("untypedAtomic",item.Value.SchemaTypeName);Assert.Equal("  x  ",item.Value.StringValue);Assert.Same(fact.Type,item.TypeEvidence.DeclaredType);Assert.Same(f.Provider.ResolveType(new(X,"untypedAtomic"),default).Value,item.TypeEvidence.ActualType);
        Assert.Equal("  x  ",f.Read(node).StringValue);f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.Declaration);Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));
    }
    /// <summary>Known descendants beneath unknown ancestors keep actual declaration handles and validation.</summary>
    [Fact]public void KnownDescendant()
    {
        using var f=new Fixture(S,"<r><u><d>01.20</d></u></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var known=f.Input.FactsFor((NativeSessionNode)f.Element([0,0]))!;Assert.Same(f.Provider.ResolveElementDeclaration(new("","d"),default).Value,known.Declaration);
        using var bad=new Fixture(S,"<r><u><d>bad</d></u></r>");Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);Assert.Empty(bad.Assessment.Facts);
    }
    /// <summary>Known identity attributes below unknown owners retain document-wide resolution.</summary>
    [Fact]public void IdentityBelowUnknown()
    {
        using var good=new Fixture(S,"<r ref='v'><u id='v'/></r>");Assert.Equal(SchemaAssessmentStatus.Valid,good.Assessment.Status);
        using var bad=new Fixture(S,"<r ref='missing'><u id='v'/></r>");Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);
    }
    /// <summary>Untyped lexical id values do not acquire ID binding semantics.</summary>
    [Fact]public void UntypedId()
    {
        var schema=S.Replace("<xs:attribute name='id' type='xs:ID'/>","");using var f=new Fixture(schema,"<r id='v'><u id='v'/></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Null(f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","id"))!.Declaration);
    }
    /// <summary>Unknown values preserve foreign authority and cancellation boundaries.</summary>
    [Fact]public void ForeignAndCancellation()
    {
        using var a=new Fixture(S,"<r><u/></r>");using var b=new Fixture(S,"<r><u/></r>");var fa=a.Input.FactsFor((NativeSessionNode)a.Element([0]))!;var fb=b.Input.FactsFor((NativeSessionNode)b.Element([0]))!;Assert.False(a.Provider.Owns(fb.Type));Assert.Throws<ArgumentException>(()=>a.Provider.IsSameComponent(fa.Type,fb.Type,default));using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>fa.ReadTypedValue(c.Token));
    }
}
