// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Proves abstract actual complex types and unchanged concrete native authority.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.AbstractComplexTypes;
/// <summary>Checks abstract actual complex types independently from declaration and content identities.</summary>
public sealed class NativeAbstractComplexTypesProofTests
{
    private static string Content(string profile)=>profile=="empty"?"":profile=="simpleContent"?"01.20":profile=="mixed"?" raw <c>01.20</c> end ":"<c>01.20</c>";
    private static string Schema(string profile="empty",bool flag=true,string place="root")
    {
        var content=profile=="empty"?"":profile=="simpleContent"?"<xs:simpleContent><xs:extension base='xs:decimal'/></xs:simpleContent>":"<xs:"+(profile is "all" or "choice"?profile:"sequence")+"><xs:element name='c' type='xs:decimal'/></xs:"+(profile is "all" or "choice"?profile:"sequence")+">";
        var roots="<xs:complexType name='T' abstract='"+(flag?"true":"false")+"'"+(profile=="mixed"?" mixed='true'":"")+">"+content+"</xs:complexType><xs:element name='a' type='T'/><xs:element name='b' type='xs:decimal'/>";
        if(place=="sequence")roots+="<xs:element name='r'><xs:complexType><xs:sequence><xs:element ref='a'/></xs:sequence></xs:complexType></xs:element>";
        if(place=="lax")roots+="<xs:element name='r'/>";
        return "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+roots+"</xs:schema>";
    }
    /// <summary>Every admitted root family rejects abstract actual types and retains concrete component authority.</summary>
    /// <param name="profile">Existing content family.</param>
    [Theory][InlineData("empty")][InlineData("sequence")][InlineData("all")][InlineData("choice")][InlineData("simpleContent")][InlineData("mixed")]
    public void RootAuthority(string profile)
    {
        using var bad=new Fixture(Schema(profile),"<a>"+Content(profile)+"</a>");Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);Assert.Empty(bad.Assessment.Facts);using var good=new Fixture(Schema(profile,false),"<a>"+Content(profile)+"</a>");Assert.Equal(SchemaAssessmentStatus.Valid,good.Assessment.Status);var fact=good.Input.FactsFor((NativeSessionNode)good.Element([]))!;Assert.Same(good.Provider.ResolveElementDeclaration(new("","a"),default).Value,fact.Declaration);Assert.Equal("T",fact.NativeType.Name);Assert.True(good.Provider.Owns(fact.Type));if(profile=="simpleContent")Assert.Equal("1.2",good.Read(good.Element([])).DecimalValue.ToInvariantString(256));if(profile=="mixed")Assert.Equal(" raw 01.20 end ",good.Read(good.Element([])).StringValue);
    }
    /// <summary>Shared abstract types affect every using declaration without poisoning an unrelated concrete type.</summary>
    [Fact]public void SharedType()
    {
        var source=Schema().Replace("type='xs:decimal'","type='T'");var c=NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;var a=c.Roots.Single(p=>p.Root.Name=="a").Root;var b=c.Roots.Single(p=>p.Root.Name=="b").Root;Assert.Same(a.Type,b.Type);Assert.NotSame(a,b);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a/>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<b/>").Outcome);using var concrete=new Fixture(Schema(),"<b>01.20</b>");Assert.Equal(SchemaAssessmentStatus.Valid,concrete.Assessment.Status);Assert.Equal("1.2",concrete.Read(concrete.Element([])).DecimalValue.ToInvariantString(256));
    }
    /// <summary>Canonical references and known lax matches use the same original abstract type authority.</summary>
    /// <param name="place">Existing dispatch path.</param>
    [Theory][InlineData("sequence")][InlineData("lax")]
    public void NestedAuthority(string place)
    {
        using var bad=new Fixture(Schema(place:place),"<r><a/></r>");Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);Assert.Empty(bad.Assessment.Facts);using var good=new Fixture(Schema(flag:false,place:place),"<r><a/></r>");Assert.Equal(SchemaAssessmentStatus.Valid,good.Assessment.Status);var fact=good.Input.FactsFor((NativeSessionNode)good.Element([0]))!;Assert.Same(good.Provider.ResolveElementDeclaration(new("","a"),default).Value,fact.Declaration);Assert.Equal("T",fact.NativeType.Name);
    }
    /// <summary>Concrete derived types do not inherit abstractness; broader instance overrides retain refusal.</summary>
    [Fact]public void ConcreteDerived()
    {
        var derived="<xs:complexType name='D'><xs:simpleContent><xs:restriction base='T'><xs:minInclusive value='1'/></xs:restriction></xs:simpleContent></xs:complexType>";var source=Schema("simpleContent").Replace("</xs:schema>",derived+"</xs:schema>");using var good=new Fixture(source.Replace("name='a' type='T'","name='a' type='D'"),"<a>01.20</a>");Assert.Equal(SchemaAssessmentStatus.Valid,good.Assessment.Status);Assert.Equal("D",good.Input.FactsFor((NativeSessionNode)good.Element([]))!.NativeType.Name);var c=NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;Assert.Equal(NativeOutcome.Unsupported,c.Assess("<a xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='D'>1</a>").Outcome);
    }
    /// <summary>Failed assessment recovers without mutating the frozen type metadata or detached values.</summary>
    [Fact]public void RecoveryAndLifetime()
    {
        using var s=NativeSchemaSession.Create(Schema("simpleContent"),new(256,256,1024,65536,2,256,16,256,1024)).Value!;Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<a>1</a>",new(DateTimeOffset.UnixEpoch,0)).Outcome);using var d=s.Assess("<b>01.20</b>",new(DateTimeOffset.UnixEpoch,0)).Value!;var atom=d.ReadRootScalar();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootScalar());Assert.Equal("1.2",atom.DecimalValue.ToInvariantString(256));Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<a>1</a>",new(DateTimeOffset.UnixEpoch,0)).Outcome);
    }
    /// <summary>Concrete facts retain foreign authority, cancellation and expiry; legacy grammar stays closed.</summary>
    [Fact]public void OwnershipLegacyAndCancellation()
    {
        using var f=new Fixture(Schema(flag:false),"<a/>");using var other=new Fixture(Schema(flag:false),"<a/>");var local=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.False(f.Provider.Owns(foreign.Type));Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(local.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>local.ReadTypedValue(cancel.Token));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>local.Declaration);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(),new(256,256)).Outcome);
    }
}
