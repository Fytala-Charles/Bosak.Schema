// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Proves anonymous date base identity, inherited facets and actual scalar authority.
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
namespace Bosak.Schema.Tests.AnonymousDateBases;
/// <summary>Checks original anonymous date bases and bounded value authority.</summary>
public sealed class NativeAnonymousDateBasesProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Base(string kind="date",string facets="")=>"<xs:simpleType><xs:restriction base='xs:"+kind+"'>"+facets+"</xs:restriction></xs:simpleType>";
    private static string Restrict(string source,string facets="")=>"<xs:simpleType><xs:restriction>"+source+facets+"</xs:restriction></xs:simpleType>";
    private static string Schema(string definition,bool named=false,string extra="")=>"<xs:schema xmlns:xs='"+X+"'>"+(named?"<xs:simpleType name='T'>"+definition["<xs:simpleType>".Length..]:"")+"<xs:element name='a'"+(named?" type='T'/>":">"+definition+"</xs:element>")+extra+"</xs:schema>";
    /// <summary>Named and unnamed outer types retain their original base and actual scalar.</summary>
    /// <param name="named">Whether the outer type is named.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void TypeAuthority(bool named)
    {
        using var f=new Fixture(Schema(Restrict(Base()),named),"<a>2000-01-02Z</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var derived=fact.NativeType;var original=derived.DerivationBase!;Assert.NotNull(original);Assert.Null(original.Name);Assert.Null(derived.BaseName);Assert.Equal(named?"T":null,derived.Name);Assert.NotSame(derived,original);Assert.True(f.Provider.Owns(f.Provider.TypeComponent(original)));var typed=fact.ReadTypedValue(default).Value;Assert.False(typed.IsList);Assert.Same(fact.Type,typed.GoverningType);var atom=Assert.Single(typed.AtomicItems);Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);
    }
    /// <summary>References share one actual source base while independent definitions remain distinct.</summary>
    [Fact]public void SharedAndDistinct()
    {
        var definition=Restrict(Base());var c=NativeDecimalSchema.Compile(Schema(definition,true,"<xs:element name='b' type='T'/>"),new(256,256),maxStringCharacters:1024).Value!;Assert.Same(c.Roots.First().Root.Type,c.Roots.Skip(1).First().Root.Type);var d=NativeDecimalSchema.Compile(Schema(definition,false,"<xs:element name='b'>"+definition+"</xs:element>"),new(256,256),maxStringCharacters:1024).Value!;Assert.NotSame(d.Roots.First().Root.Type.DerivationBase,d.Roots.Skip(1).First().Root.Type.DerivationBase);
    }
    /// <summary>Inherited date bounds preserve partial order and exact source ancestry.</summary>
    [Fact]public void BoundAncestry()
    {
        var c=NativeDecimalSchema.Compile(Schema(Restrict(Base("date","<xs:minInclusive value='2000-01-02Z'/>"),"<xs:maxInclusive value='2000-01-04Z'/>")),new(256,256),maxStringCharacters:1024).Value!;Assert.NotNull(c.Root.Type.DerivationBase!.DateFacets);Assert.Equal(NativeOutcome.Valid,c.Assess("<a>2000-01-03Z</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>2000-01-01Z</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>2000-01-02</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>2000-01-05Z</a>").Outcome);
    }

    /// <summary>Inherited timezone restrictions retain required and prohibited semantics.</summary>
    [Fact]public void TimezoneAncestry()
    {
        var c=NativeDecimalSchema.Compile(Schema(Restrict(Base("date","<xs:explicitTimezone value='required'/>"))),new(256,256),maxStringCharacters:1024).Value!;Assert.Equal(NativeOutcome.Valid,c.Assess("<a>2000-01-02Z</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>2000-01-02</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(Schema(Restrict(Base("date","<xs:explicitTimezone value='required'/>"),"<xs:explicitTimezone value='optional'/>")),new(256,256),maxStringCharacters:1024).Outcome);
    }

    /// <summary>Forward references and final prohibitions traverse anonymous date bases.</summary>
    [Fact]public void ForwardAndFinal()
    {
        var source=Schema(Restrict("<xs:simpleType><xs:restriction base='B'/></xs:simpleType>"),true,"<xs:simpleType name='B'><xs:restriction base='xs:date'/></xs:simpleType>");var c=NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;Assert.Same(c.NamedTypes.Single(t=>t.Name=="B"),c.Root.Type.DerivationBase!.DerivationBase);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(source.Replace("name='B'","name='B' final='restriction'"),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Scalar source sites retain the existing recursive and shared inline ceilings.</summary>
    [Fact]public void Budgets()
    {
        var definition=Base();for(var i=0;i<4;i++)definition=Restrict(definition);Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(definition),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(definition)),new(256,256),maxStringCharacters:1024).Outcome);var members=Enumerable.Repeat(Restrict(Base()),8).ToArray();string Union()=>"<xs:simpleType><xs:union>"+string.Concat(members)+"</xs:union></xs:simpleType>";Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);members[0]=Restrict(members[0]);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Enumeration equality normalizes date instants without rewriting original detached components.</summary>
    [Fact]public void EnumerationEquality()
    {
        var definition=Restrict(Base("date","<xs:enumeration value='2000-01-02+14:00'/>"));using var owner=NativeSchemaSession.Create(Schema(definition),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);using var d=owner.Assess("<a>2000-01-01-10:00</a>",context).Value!;var value=Assert.Single(d.ReadRootItems()).DateValue;Assert.Equal(2000,value.Year);Assert.Equal(1,value.Month);Assert.Equal(1,value.Day);Assert.Equal(-600,value.TimezoneOffsetMinutes);Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<a>2000-01-02</a>",context).Outcome);
    }

    /// <summary>Invalid input recovers and detached date components survive document expiry.</summary>
    [Fact]public void RecoveryAndLifetime()
    {
        using var owner=NativeSchemaSession.Create(Schema(Restrict(Base())),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<a>bad</a>",context).Outcome);using var d=owner.Assess("<a>0000-02-29Z</a>",context).Value!;var value=Assert.Single(d.ReadRootItems()).DateValue;d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootItems());Assert.Equal(0,value.Year);Assert.Equal(2,value.Month);Assert.Equal(29,value.Day);Assert.Equal(0,value.TimezoneOffsetMinutes);using var again=owner.Assess("<a>2000-01-02</a>",context).Value!;Assert.Null(Assert.Single(again.ReadRootItems()).DateValue.TimezoneOffsetMinutes);
    }

    /// <summary>Anonymous scalar base handles retain ownership, cancellation and expiry checks.</summary>
    [Fact]public void Ownership()
    {
        using var f=new Fixture(Schema(Restrict(Base())),"<a>2000-01-02Z</a>",atoms:16);using var other=new Fixture(Schema(Restrict(Base())),"<a>2000-01-02Z</a>",atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var handle=f.Provider.TypeComponent(fact.NativeType.DerivationBase!);Assert.True(f.Provider.Owns(handle));Assert.False(other.Provider.Owns(handle));var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>fact.ReadTypedValue(cancel.Token));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.Declaration);
    }
    /// <summary>Nested anonymous bases remain original distinct objects with inherited facets.</summary>
    [Fact]public void BaseChain()
    {
        var definition=Restrict(Restrict(Base("date","<xs:enumeration value='2000-01-02Z'/>")));var c=NativeDecimalSchema.Compile(Schema(definition),new(256,256),maxStringCharacters:1024).Value!;var t=c.Root.Type;Assert.Null(t.Name);Assert.Null(t.DerivationBase!.Name);Assert.Null(t.DerivationBase.DerivationBase!.Name);Assert.NotSame(t.DerivationBase,t.DerivationBase.DerivationBase);Assert.DoesNotContain(c.NamedTypes,n=>ReferenceEquals(n,t.DerivationBase));Assert.Equal(NativeOutcome.Valid,c.Assess("<a>2000-01-02Z</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>2000-01-03Z</a>").Outcome);
    }
    /// <summary>Original date pattern groups retain inherited intersection and bounded value restrictions.</summary>
    [Fact]public void PatternAncestry()
    {
        var definition=Restrict(Base("date","<xs:pattern value='2000.*'/>"),"<xs:pattern value='.*Z'/>");var c=NativeDecimalSchema.Compile(Schema(definition),new(256,256),maxStringCharacters:1024).Value!;Assert.True(c.Root.Type.DateFacets!.HasPatterns);Assert.True(c.Root.Type.DerivationBase!.DateFacets!.HasPatterns);Assert.Equal(NativeOutcome.Valid,c.Assess("<a>2000-01-02Z</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>2000-01-02</a>").Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(Base("date","<xs:pattern value='2000.*'/>"),"<xs:minInclusive value='2000-01-01Z'/>")),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Year capacity refusal precedes publication and leaves the owner usable.</summary>
    [Fact]public void RepresentationCapacity()
    {
        using var owner=NativeSchemaSession.Create(Schema(Restrict(Base())),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);var failed=owner.Assess("<a>-9223372036854775808-01-01</a>",context);Assert.Equal(NativeSchemaOutcome.Unsupported,failed.Outcome);Assert.Null(failed.Value);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,failed.Diagnostics[0].UnsupportedReason);using var d=owner.Assess("<a>-9223372036854775807-01-01</a>",context).Value!;Assert.Equal(-long.MaxValue,Assert.Single(d.ReadRootItems()).DateValue.Year);
    }
    /// <summary>A nilled original date declaration retains its governing type and typed-empty value.</summary>
    [Fact]public void NilledAuthority()
    {
        var schema=Schema(Restrict(Base())).Replace("<xs:element name='a'>","<xs:element name='a' nillable='true'>");using var f=new Fixture(schema,"<a xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true'/>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.True(fact.NativeType.IsDate);Assert.NotNull(fact.NativeType.DerivationBase);var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);Assert.Empty(typed.AtomicItems);
    }

}
