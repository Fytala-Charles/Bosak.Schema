// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Proves anonymous scalar base identity, inherited facets and actual scalar authority.
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
namespace Bosak.Schema.Tests.AnonymousScalarBases;
/// <summary>Checks original anonymous scalar bases and bounded value authority.</summary>
public sealed class NativeAnonymousScalarBasesProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Base(string kind="decimal",string facets="")=>"<xs:simpleType><xs:restriction base='xs:"+kind+"'>"+facets+"</xs:restriction></xs:simpleType>";
    private static string Restrict(string source,string facets="")=>"<xs:simpleType><xs:restriction>"+source+facets+"</xs:restriction></xs:simpleType>";
    private static string Schema(string definition,bool named=false,string extra="")=>"<xs:schema xmlns:xs='"+X+"'>"+(named?"<xs:simpleType name='T'>"+definition["<xs:simpleType>".Length..]:"")+"<xs:element name='a'"+(named?" type='T'/>":">"+definition+"</xs:element>")+extra+"</xs:schema>";
    /// <summary>Named and unnamed outer types retain their original base and actual scalar.</summary>
    /// <param name="named">Whether the outer type is named.</param><param name="kind">Primitive family.</param>
    [Theory][InlineData(false,"decimal")][InlineData(true,"decimal")][InlineData(false,"string")][InlineData(true,"string")]
    public void TypeAuthority(bool named,string kind)
    {
        using var f=new Fixture(Schema(Restrict(Base(kind)),named),kind=="decimal"?"<a>01.20</a>":"<a>word</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var derived=fact.NativeType;var original=derived.DerivationBase!;Assert.NotNull(original);Assert.Null(original.Name);Assert.Null(derived.BaseName);Assert.Equal(named?"T":null,derived.Name);Assert.NotSame(derived,original);Assert.True(f.Provider.Owns(f.Provider.TypeComponent(original)));var typed=fact.ReadTypedValue(default).Value;Assert.False(typed.IsList);Assert.Same(fact.Type,typed.GoverningType);var atom=Assert.Single(typed.AtomicItems);Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);
    }
    /// <summary>References share one actual source base while independent definitions remain distinct.</summary>
    [Fact]public void SharedAndDistinct()
    {
        var definition=Restrict(Base());var c=NativeDecimalSchema.Compile(Schema(definition,true,"<xs:element name='b' type='T'/>"),new(256,256),maxStringCharacters:1024).Value!;Assert.Same(c.Roots.First().Root.Type,c.Roots.Skip(1).First().Root.Type);var d=NativeDecimalSchema.Compile(Schema(definition,false,"<xs:element name='b'>"+definition+"</xs:element>"),new(256,256),maxStringCharacters:1024).Value!;Assert.NotSame(d.Roots.First().Root.Type.DerivationBase,d.Roots.Skip(1).First().Root.Type.DerivationBase);
    }
    /// <summary>Decimal facets and pattern ancestry remain attached to original base objects.</summary>
    [Fact]public void DecimalAncestry()
    {
        var c=NativeDecimalSchema.Compile(Schema(Restrict(Base("decimal","<xs:minInclusive value='1'/><xs:maxInclusive value='3'/>"),"<xs:maxInclusive value='2'/>")),new(256,256),maxStringCharacters:1024).Value!;Assert.NotNull(c.Root.Type.DerivationBase!.Facets);Assert.Equal(NativeOutcome.Valid,c.Assess("<a>1.20</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>2.1</a>").Outcome);var p=NativeDecimalSchema.Compile(Schema(Restrict(Base("decimal","<xs:pattern value='1.*'/>"),"<xs:pattern value='.*2'/>")),new(256,256),maxStringCharacters:1024).Value!;Assert.NotNull(p.Root.Type.DerivationBase!.NumericPatterns);Assert.Equal(NativeOutcome.Valid,p.Assess("<a>12</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,p.Assess("<a>13</a>").Outcome);
    }
    /// <summary>Text normalization, inherited length and fixed whitespace retain their existing semantics.</summary>
    [Fact]public void TextAncestry()
    {
        var c=NativeDecimalSchema.Compile(Schema(Restrict(Base("token","<xs:minLength value='1'/>"),"<xs:maxLength value='3'/>")),new(256,256),maxStringCharacters:1024).Value!;Assert.NotNull(c.Root.Type.DerivationBase!.TextFacets);Assert.Equal(NativeOutcome.Valid,c.Assess("<a>  a   b </a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>abcd</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(Schema(Restrict(Base("token"),"<xs:whiteSpace value='preserve'/>")),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Forward references and final prohibitions traverse anonymous scalar bases.</summary>
    [Fact]public void ForwardAndFinal()
    {
        var source=Schema(Restrict("<xs:simpleType><xs:restriction base='B'/></xs:simpleType>"),true,"<xs:simpleType name='B'><xs:restriction base='xs:decimal'/></xs:simpleType>");var c=NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;Assert.Same(c.NamedTypes.Single(t=>t.Name=="B"),c.Root.Type.DerivationBase!.DerivationBase);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(source.Replace("name='B'","name='B' final='restriction'"),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Scalar source sites retain the existing recursive and shared inline ceilings.</summary>
    [Fact]public void Budgets()
    {
        var definition=Base();for(var i=0;i<4;i++)definition=Restrict(definition);Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(definition),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(definition)),new(256,256),maxStringCharacters:1024).Outcome);var members=Enumerable.Repeat(Restrict(Base()),8).ToArray();string Union()=>"<xs:simpleType><xs:union>"+string.Concat(members)+"</xs:union></xs:simpleType>";Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);members[0]=Restrict(members[0]);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>An empty union is a valid type with no values; its list admits the empty sequence.</summary>
    [Fact]public void EmptyUnionCorrection()
    {
        var empty="<xs:simpleType><xs:union/></xs:simpleType>";var c=NativeDecimalSchema.Compile(Schema(Restrict(empty)),new(256,256),maxStringCharacters:1024).Value!;Assert.NotNull(c);Assert.Empty(c.Root.Type.UnionMembers);foreach(var text in new[]{"","1","word"})Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>"+text+"</a>").Outcome);var list="<xs:simpleType><xs:list>"+empty+"</xs:list></xs:simpleType>";var l=NativeDecimalSchema.Compile(Schema(list),new(256,256),maxStringCharacters:1024).Value!;Assert.Equal(NativeOutcome.Valid,l.Assess("<a/>").Outcome);Assert.Equal(NativeOutcome.Invalid,l.Assess("<a>word</a>").Outcome);
    }
    /// <summary>Assessment recovers after invalid values and detached decimals survive session expiry.</summary>
    [Fact]public void RecoveryAndLifetime()
    {
        using var s=NativeSchemaSession.Create(Schema(Restrict(Base(),"<xs:minInclusive value='1'/>")),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<a>0</a>",context).Outcome);using var d=s.Assess("<a>01.20</a>",context).Value!;var values=d.ReadRootItems();Assert.Single(values);d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootItems());Assert.Equal("1.2",values[0].DecimalValue.ToInvariantString(256));
    }
    /// <summary>Anonymous scalar base handles retain ownership, cancellation and expiry checks.</summary>
    [Fact]public void Ownership()
    {
        using var f=new Fixture(Schema(Restrict(Base())),"<a>1</a>",atoms:16);using var other=new Fixture(Schema(Restrict(Base())),"<a>1</a>",atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var handle=f.Provider.TypeComponent(fact.NativeType.DerivationBase!);Assert.True(f.Provider.Owns(handle));Assert.False(other.Provider.Owns(handle));var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>fact.ReadTypedValue(cancel.Token));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.Declaration);
    }
}
