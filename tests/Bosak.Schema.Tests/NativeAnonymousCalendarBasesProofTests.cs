// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Proves anonymous dateTime/time base identity and exact inherited calendar authority.
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
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.AnonymousCalendarBases;
/// <summary>Checks original unnamed calendar bases and bounded exact value authority.</summary>
public sealed class NativeAnonymousCalendarBasesProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Value(string kind,string time="12:00:00.123456789Z")=>(kind=="dateTime"?"2000-01-02T":"")+time;
    private static string Base(string kind="dateTime",string facets="")=>"<xs:simpleType><xs:restriction base='xs:"+kind+"'>"+facets+"</xs:restriction></xs:simpleType>";
    private static string Restrict(string source,string facets="")=>"<xs:simpleType><xs:restriction>"+source+facets+"</xs:restriction></xs:simpleType>";
    private static string Schema(string definition,bool named=false,string extra="")=>"<xs:schema xmlns:xs='"+X+"'>"+(named?"<xs:simpleType name='T'>"+definition["<xs:simpleType>".Length..]:"")+"<xs:element name='a'"+(named?" type='T'/>":">"+definition+"</xs:element>")+extra+"</xs:schema>";
    private static NativeDecimalSchema Compile(string source)=>NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;
    /// <summary>Named and unnamed types retain original bases and exact governing scalar provenance.</summary>
    /// <param name="named">Outer naming selector.</param><param name="kind">Calendar family.</param>
    [Theory][InlineData(false,"dateTime")][InlineData(true,"dateTime")][InlineData(false,"time")][InlineData(true,"time")]
    public void TypeAuthority(bool named,string kind)
    {
        using var f=new Fixture(Schema(Restrict(Base(kind)),named),"<a>"+Value(kind)+"</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var t=fact.NativeType;Assert.Null(t.BaseName);Assert.Equal(named?"T":null,t.Name);Assert.NotNull(t.DerivationBase);Assert.Null(t.DerivationBase!.Name);Assert.NotSame(t,t.DerivationBase);Assert.Equal(t.CalendarKind,t.DerivationBase.CalendarKind);Assert.True(f.Provider.Owns(f.Provider.TypeComponent(t.DerivationBase)));var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);var atom=Assert.Single(typed.AtomicItems);Assert.True(atom.Value.IsExactCalendar);Assert.Equal(Value(kind),atom.Value.ExactCalendarValue.ToInvariantString(1024));Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);
    }
    /// <summary>References share one actual base while independently defined source bases stay distinct.</summary>
    [Fact]public void SharedAndDistinct()
    {
        var definition=Restrict(Base());var c=Compile(Schema(definition,true,"<xs:element name='b' type='T'/>"));Assert.Same(c.Roots.First().Root.Type,c.Roots.Skip(1).First().Root.Type);var d=Compile(Schema(definition,false,"<xs:element name='b'>"+definition+"</xs:element>"));Assert.NotSame(d.Roots.First().Root.Type.DerivationBase,d.Roots.Skip(1).First().Root.Type.DerivationBase);Assert.DoesNotContain(d.NamedTypes,t=>ReferenceEquals(t,d.Root.Type.DerivationBase));
    }
    /// <summary>Nested anonymous time bases retain original links and inherited enumeration.</summary>
    [Fact]public void BaseChain()
    {
        var c=Compile(Schema(Restrict(Restrict(Base("time","<xs:enumeration value='12:00:00Z'/>")))));var t=c.Root.Type;Assert.Null(t.Name);Assert.Null(t.DerivationBase!.Name);Assert.Null(t.DerivationBase.DerivationBase!.Name);Assert.NotSame(t.DerivationBase,t.DerivationBase.DerivationBase);Assert.Equal(NativeOutcome.Valid,c.Assess("<a>12:00:00Z</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>13:00:00Z</a>").Outcome);
    }
    /// <summary>Equal calendar instants preserve original offset components and exact fractional seconds.</summary>
    /// <param name="kind">Calendar family.</param>
    [Theory][InlineData("dateTime")][InlineData("time")]public void EnumerationEquality(string kind)
    {
        var source=Schema(Restrict(Base(kind),"<xs:enumeration value='"+Value(kind,"12:00:00.1Z")+"'/>"));using var owner=NativeSchemaSession.Create(source,new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);using var d=owner.Assess("<a>"+Value(kind,"14:00:00.100+02:00")+"</a>",context).Value!;var value=Assert.Single(d.ReadRootItems()).CalendarValue;Assert.Equal(Value(kind,"14:00:00.1+02:00"),value.ToInvariantString(1024));Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<a>"+Value(kind,"12:00:00.1")+"</a>",context).Outcome);
    }
    /// <summary>Fixed bounds compare exact values while inherited partial-order bounds remain enforced.</summary>
    /// <param name="kind">Calendar family.</param>
    [Theory][InlineData("dateTime")][InlineData("time")]public void BoundsAndFixed(string kind)
    {
        var original=Base(kind,"<xs:minInclusive value='"+Value(kind,"12:00:00Z")+"' fixed='true'/>");var c=Compile(Schema(Restrict(original,"<xs:minInclusive value='"+Value(kind,"14:00:00+02:00")+"'/>")));Assert.NotNull(c.Root.Type.DerivationBase!.CalendarFacets);Assert.Equal(NativeOutcome.Valid,c.Assess("<a>"+Value(kind,"13:00:00Z")+"</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>"+Value(kind,"12:00:00")+"</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(Schema(Restrict(original,"<xs:minInclusive value='"+Value(kind,"13:00:00Z")+"'/>")),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Inherited timezone and lexical patterns intersect without changing existing refusal boundaries.</summary>
    /// <param name="kind">Calendar family.</param>
    [Theory][InlineData("dateTime")][InlineData("time")]public void TimezoneAndPatterns(string kind)
    {
        var parent=Base(kind,"<xs:explicitTimezone value='required'/><xs:pattern value='.*Z'/>");var c=Compile(Schema(Restrict(parent,"<xs:pattern value='.*12:.*'/>")));Assert.Equal(NativeOutcome.Valid,c.Assess("<a>"+Value(kind)+"</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>"+Value(kind,"13:00:00Z")+"</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>"+Value(kind,"12:00:00")+"</a>").Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(parent,"<xs:minInclusive value='"+Value(kind,"12:00:00Z")+"'/>")),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Forward named dateTime dependencies and final prohibitions retain original source edges.</summary>
    [Fact]public void ForwardAndFinal()
    {
        var source=Schema(Restrict("<xs:simpleType><xs:restriction base='B'/></xs:simpleType>"),true,"<xs:simpleType name='B'><xs:restriction base='xs:dateTime'/></xs:simpleType>");var c=Compile(source);Assert.Same(c.NamedTypes.Single(t=>t.Name=="B"),c.Root.Type.DerivationBase!.DerivationBase);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(source.Replace("name='B'","name='B' final='restriction'"),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Recursive and shared source-site bounds retain their original finite ceilings.</summary>
    [Fact]public void Budgets()
    {
        var definition=Base("time");for(var i=0;i<4;i++)definition=Restrict(definition);Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(definition),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(definition)),new(256,256),maxStringCharacters:1024).Outcome);var members=Enumerable.Repeat(Restrict(Base("time")),8).ToArray();string Union()=>"<xs:simpleType><xs:union>"+string.Concat(members)+"</xs:union></xs:simpleType>";Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);members[0]=Restrict(members[0]);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Invalid input recovers and detached exact calendars survive document expiry.</summary>
    /// <param name="kind">Calendar family.</param>
    [Theory][InlineData("dateTime")][InlineData("time")]public void RecoveryAndLifetime(string kind)
    {
        using var owner=NativeSchemaSession.Create(Schema(Restrict(Base(kind))),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<a>bad</a>",context).Outcome);using var d=owner.Assess("<a>"+Value(kind)+"</a>",context).Value!;var value=Assert.Single(d.ReadRootItems()).CalendarValue;d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootItems());Assert.Equal(Value(kind),value.ToInvariantString(1024));using var again=owner.Assess("<a>"+Value(kind,"13:00:00Z")+"</a>",context).Value!;Assert.Single(again.ReadRootItems());
    }
    /// <summary>Anonymous base handles retain provider ownership, cancellation and expiry guards.</summary>
    /// <param name="kind">Calendar family.</param>
    [Theory][InlineData("dateTime")][InlineData("time")]public void Ownership(string kind)
    {
        using var f=new Fixture(Schema(Restrict(Base(kind))),"<a>"+Value(kind)+"</a>",atoms:16);using var other=new Fixture(Schema(Restrict(Base(kind))),"<a>"+Value(kind)+"</a>",atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var handle=f.Provider.TypeComponent(fact.NativeType.DerivationBase!);Assert.True(f.Provider.Owns(handle));Assert.False(other.Provider.Owns(handle));var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>fact.ReadTypedValue(cancel.Token));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.Declaration);
    }
    /// <summary>Raw seconds scale capacity is checked before trailing-zero normalization and publication.</summary>
    /// <param name="kind">Calendar family.</param>
    [Theory][InlineData("dateTime")][InlineData("time")]public void RepresentationCapacity(string kind)
    {
        using var owner=NativeSchemaSession.Create(Schema(Restrict(Base(kind))),new(256,2,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);var result=owner.Assess("<a>"+Value(kind,"12:00:00.000Z")+"</a>",context);Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,result.Diagnostics[0].UnsupportedReason);using var good=owner.Assess("<a>"+Value(kind,"12:00:00.12Z")+"</a>",context).Value!;Assert.Equal(Value(kind,"12:00:00.12Z"),Assert.Single(good.ReadRootItems()).CalendarValue.ToInvariantString(1024));
    }
    /// <summary>Nilled calendar declarations retain original governing types and typed-empty values.</summary>
    /// <param name="kind">Calendar family.</param>
    [Theory][InlineData("dateTime")][InlineData("time")]public void NilledAuthority(string kind)
    {
        var schema=Schema(Restrict(Base(kind))).Replace("<xs:element name='a'>","<xs:element name='a' nillable='true'>");using var f=new Fixture(schema,"<a xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true'/>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal(kind=="dateTime"?XdmExactCalendarKind.DateTime:XdmExactCalendarKind.Time,fact.NativeType.CalendarKind);Assert.NotNull(fact.NativeType.DerivationBase);var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);Assert.Empty(typed.AtomicItems);
    }
}
