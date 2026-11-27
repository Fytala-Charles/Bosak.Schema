// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Proves anonymous integer/Boolean base identity, inherited facets and actual scalar authority.
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
namespace Bosak.Schema.Tests.AnonymousIntegralBases;
/// <summary>Checks original anonymous integer/Boolean bases and bounded value authority.</summary>
public sealed class NativeAnonymousIntegralBasesProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Base(string kind="integer",string facets="")=>"<xs:simpleType><xs:restriction base='xs:"+kind+"'>"+facets+"</xs:restriction></xs:simpleType>";
    private static string Restrict(string source,string facets="")=>"<xs:simpleType><xs:restriction>"+source+facets+"</xs:restriction></xs:simpleType>";
    private static string Schema(string definition,bool named=false,string extra="")=>"<xs:schema xmlns:xs='"+X+"'>"+(named?"<xs:simpleType name='T'>"+definition["<xs:simpleType>".Length..]:"")+"<xs:element name='a'"+(named?" type='T'/>":">"+definition+"</xs:element>")+extra+"</xs:schema>";
    /// <summary>Named and unnamed outer types retain their original base and actual scalar.</summary>
    /// <param name="named">Whether the outer type is named.</param><param name="kind">Primitive family.</param>
    [Theory][InlineData(false,"integer")][InlineData(true,"integer")][InlineData(false,"boolean")][InlineData(true,"boolean")]
    public void TypeAuthority(bool named,string kind)
    {
        using var f=new Fixture(Schema(Restrict(Base(kind)),named),kind=="integer"?"<a>+001</a>":"<a>1</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var derived=fact.NativeType;var original=derived.DerivationBase!;Assert.NotNull(original);Assert.Null(original.Name);Assert.Null(derived.BaseName);Assert.Equal(named?"T":null,derived.Name);Assert.NotSame(derived,original);Assert.True(f.Provider.Owns(f.Provider.TypeComponent(original)));var typed=fact.ReadTypedValue(default).Value;Assert.False(typed.IsList);Assert.Same(fact.Type,typed.GoverningType);var atom=Assert.Single(typed.AtomicItems);Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);if(kind=="boolean")Assert.True(atom.Value.BooleanValue);else Assert.Equal("1",atom.Value.IntegerValue.ToString());
    }
    /// <summary>References share one actual source base while independent definitions remain distinct.</summary>
    /// <param name="kind">Atomic family.</param>
    [Theory][InlineData("integer")][InlineData("boolean")]public void SharedAndDistinct(string kind)
    {
        var definition=Restrict(Base(kind));var c=NativeDecimalSchema.Compile(Schema(definition,true,"<xs:element name='b' type='T'/>"),new(256,256),maxStringCharacters:1024).Value!;Assert.Same(c.Roots.First().Root.Type,c.Roots.Skip(1).First().Root.Type);var d=NativeDecimalSchema.Compile(Schema(definition,false,"<xs:element name='b'>"+definition+"</xs:element>"),new(256,256),maxStringCharacters:1024).Value!;Assert.NotSame(d.Roots.First().Root.Type.DerivationBase,d.Roots.Skip(1).First().Root.Type.DerivationBase);
    }
    /// <summary>Original integer facets retain exact inherited bounds, enumeration and lexical pattern ancestry.</summary>
    [Fact]public void IntegerAncestry()
    {
        var c=NativeDecimalSchema.Compile(Schema(Restrict(Base("unsignedLong","<xs:maxInclusive value='18446744073709551615'/>"),"<xs:minInclusive value='1'/><xs:enumeration value='18446744073709551615'/>")),new(256,256),maxStringCharacters:1024).Value!;Assert.NotNull(c.Root.Type.DerivationBase!.IntegerFacets);Assert.Equal(NativeOutcome.Valid,c.Assess("<a>18446744073709551615</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>18446744073709551616</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>0</a>").Outcome);var p=NativeDecimalSchema.Compile(Schema(Restrict(Base("integer","<xs:pattern value='1.*'/>"),"<xs:pattern value='.*2'/>")),new(256,256),maxStringCharacters:1024).Value!;Assert.NotNull(p.Root.Type.DerivationBase!.NumericPatterns);Assert.Equal(NativeOutcome.Valid,p.Assess("<a>12</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,p.Assess("<a>13</a>").Outcome);Assert.Null(c.Root.Type.Name);Assert.DoesNotContain(c.NamedTypes,t=>ReferenceEquals(t,c.Root.Type.DerivationBase));
    }

    /// <summary>Boolean ancestry intersects parent and child groups while preserving fixed collapse whitespace.</summary>
    [Fact]public void BooleanAncestry()
    {
        var c=NativeDecimalSchema.Compile(Schema(Restrict(Base("boolean","<xs:pattern value='true'/><xs:pattern value='1'/>"),"<xs:pattern value='t.*'/><xs:whiteSpace value='collapse'/>")),new(256,256),maxStringCharacters:1024).Value!;Assert.NotNull(c.Root.Type.DerivationBase!.BooleanFacets);Assert.Equal(NativeOutcome.Valid,c.Assess("<a>  true  </a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>1</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(Schema(Restrict(Base("boolean"),"<xs:whiteSpace value='preserve'/>")),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(Schema(Restrict(Base("boolean"),"<xs:enumeration value='true'/>")),new(256,256),maxStringCharacters:1024).Outcome);
    }

    /// <summary>Forward references and final prohibitions traverse anonymous integer/Boolean bases.</summary>
    [Fact]public void ForwardAndFinal()
    {
        var source=Schema(Restrict("<xs:simpleType><xs:restriction base='B'/></xs:simpleType>"),true,"<xs:simpleType name='B'><xs:restriction base='xs:boolean'/></xs:simpleType>");var c=NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;Assert.Same(c.NamedTypes.Single(t=>t.Name=="B"),c.Root.Type.DerivationBase!.DerivationBase);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(source.Replace("name='B'","name='B' final='restriction'"),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Scalar source sites retain the existing recursive and shared inline ceilings.</summary>
    /// <param name="kind">Atomic family.</param>
    [Theory][InlineData("integer")][InlineData("boolean")]public void Budgets(string kind)
    {
        var definition=Base(kind);for(var i=0;i<4;i++)definition=Restrict(definition);Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(definition),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(definition)),new(256,256),maxStringCharacters:1024).Outcome);var members=Enumerable.Repeat(Restrict(Base(kind)),8).ToArray();string Union()=>"<xs:simpleType><xs:union>"+string.Concat(members)+"</xs:union></xs:simpleType>";Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);members[0]=Restrict(members[0]);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);
    }

    /// <summary>Both families recover after invalid input and detached values survive document expiry.</summary>
    /// <param name="kind">Atomic family.</param>
    [Theory][InlineData("integer")][InlineData("boolean")]public void RecoveryAndLifetime(string kind)
    {
        using var s=NativeSchemaSession.Create(Schema(Restrict(Base(kind))),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<a>bad</a>",context).Outcome);using var d=s.Assess("<a>1</a>",context).Value!;var values=d.ReadRootItems();Assert.Single(values);d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootItems());if(kind=="boolean")Assert.True(values[0].BooleanValue);else Assert.Equal("1",values[0].IntegerValue.ToString());using var again=s.Assess("<a>0</a>",context).Value!;Assert.Single(again.ReadRootItems());
    }

    /// <summary>Anonymous scalar base handles retain ownership, cancellation and expiry checks.</summary>
    /// <param name="kind">Atomic family.</param>
    [Theory][InlineData("integer")][InlineData("boolean")]public void Ownership(string kind)
    {
        using var f=new Fixture(Schema(Restrict(Base(kind))),"<a>1</a>",atoms:16);using var other=new Fixture(Schema(Restrict(Base(kind))),"<a>1</a>",atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var handle=f.Provider.TypeComponent(fact.NativeType.DerivationBase!);Assert.True(f.Provider.Owns(handle));Assert.False(other.Provider.Owns(handle));var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>fact.ReadTypedValue(cancel.Token));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.Declaration);
    }
}
