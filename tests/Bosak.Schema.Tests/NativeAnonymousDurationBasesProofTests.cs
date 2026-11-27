// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Proves original anonymous duration base identity, exact values and facet authority.
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
namespace Bosak.Schema.Tests.AnonymousDurationBases;
/// <summary>Checks original unnamed duration bases and bounded exact value authority.</summary>
public sealed class NativeAnonymousDurationBasesProofTests
{
    private const string Fraction="123456789012345678901234567890123";
    private static readonly string[] Kinds=["duration","yearMonthDuration","dayTimeDuration"];
    private static string Good(string kind)=>kind switch{"yearMonthDuration"=>"P1Y","dayTimeDuration"=>"PT60."+Fraction+"S",_=>"P1Y1DT0."+Fraction+"S"};
    private static string Alias(string kind)=>kind switch{"yearMonthDuration"=>"P12M","dayTimeDuration"=>"PT1M0."+Fraction+"S",_=>"P12MT86400."+Fraction+"S"};
    private static string Before(string kind)=>kind switch{"yearMonthDuration"=>"P11M","dayTimeDuration"=>"PT59S",_=>"PT0S"};
    private static string After(string kind)=>kind switch{"yearMonthDuration"=>"P13M","dayTimeDuration"=>"PT61S",_=>"P2Y1DT0."+Fraction+"S"};
    private static string Base(string kind="duration",string facets="")=>"<xs:simpleType><xs:restriction base='xs:"+kind+"'>"+facets+"</xs:restriction></xs:simpleType>";
    private static string Restrict(string source,string facets="")=>"<xs:simpleType><xs:restriction>"+source+facets+"</xs:restriction></xs:simpleType>";
    private static string Schema(string definition,bool named=false,string extra="")=>"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+(named?"<xs:simpleType name='T'>"+definition["<xs:simpleType>".Length..]:"")+"<xs:element name='a'"+(named?" type='T'/>":">"+definition+"</xs:element>")+extra+"</xs:schema>";
    private static NativeDecimalSchema Compile(string source)=>NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;
    private static NativeDurationKind Kind(string kind)=>Enum.Parse<NativeDurationKind>(char.ToUpperInvariant(kind[0])+kind[1..]);
    /// <summary>Named and unnamed outer types retain their original base and exact scalar provenance.</summary>
    /// <param name="kind">Duration family.</param><param name="named">Outer naming selector.</param>
    [Theory][InlineData("duration",false)][InlineData("duration",true)][InlineData("yearMonthDuration",false)][InlineData("yearMonthDuration",true)][InlineData("dayTimeDuration",false)][InlineData("dayTimeDuration",true)]
    public void TypeAuthority(string kind,bool named)
    {
        using var f=new Fixture(Schema(Restrict(Base(kind)),named),"<a>"+Good(kind)+"</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var t=fact.NativeType;Assert.Null(t.BaseName);Assert.Equal(named?"T":null,t.Name);Assert.NotNull(t.DerivationBase);Assert.Null(t.DerivationBase!.Name);Assert.NotSame(t,t.DerivationBase);Assert.Equal(Kind(kind),t.DurationKind);Assert.Equal(t.DurationKind,t.DerivationBase.DurationKind);Assert.True(f.Provider.Owns(f.Provider.TypeComponent(t.DerivationBase)));var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);var atom=Assert.Single(typed.AtomicItems);Assert.Equal(XdmValueKind.Duration,atom.Value.Kind);Assert.Equal(kind,atom.Value.SchemaTypeName);Assert.Equal(Good(kind),atom.Value.DurationValue);Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);
    }
    /// <summary>Each family preserves inherited exact bounds, fixed equality and pattern exclusions.</summary>
    /// <param name="kind">Duration family.</param>
    [Theory][InlineData("duration")][InlineData("yearMonthDuration")][InlineData("dayTimeDuration")]
    public void FacetAncestry(string kind)
    {
        var good=Good(kind);var original=Base(kind,"<xs:minInclusive value='"+good+"' fixed='true'/>");var c=Compile(Schema(Restrict(original,"<xs:maxInclusive value='"+After(kind)+"'/>")));Assert.NotNull(c.Root.Type.DerivationBase!.DurationFacets);Assert.NotSame(c.Root.Type.DurationFacets,c.Root.Type.DerivationBase.DurationFacets);Assert.Equal(NativeOutcome.Valid,c.Assess("<a>"+good+"</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>"+Before(kind)+"</a>").Outcome);Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(Restrict(original,"<xs:minInclusive value='"+Alias(kind)+"'/>")),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(Schema(Restrict(original,"<xs:minInclusive value='"+After(kind)+"'/>")),new(256,256),maxStringCharacters:1024).Outcome);
        var prefix=kind=="dayTimeDuration"?"PT60.*":"P1Y.*";var patterned=Base(kind,"<xs:pattern value='"+prefix+"'/>");var p=Compile(Schema(Restrict(patterned,"<xs:pattern value='"+good+"'/>")));Assert.Equal(NativeOutcome.Valid,p.Assess("<a>"+good+"</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,p.Assess("<a>"+Alias(kind)+"</a>").Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(patterned,"<xs:enumeration value='"+good+"'/>")),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Detached exact components, recovery, ownership, cancellation and expiry retain original contracts.</summary>
    /// <param name="kind">Duration family.</param>
    [Theory][InlineData("duration")][InlineData("yearMonthDuration")][InlineData("dayTimeDuration")]
    public void ComponentsAndLifetime(string kind)
    {
        var source=Schema(Restrict(Base(kind)));var xml="<a>"+Good(kind)+"</a>";using var owner=NativeSchemaSession.Create(source,new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<a>bad</a>",context).Outcome);using var d=owner.Assess(xml,context).Value!;var value=Assert.Single(d.ReadRootItems());d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootItems());Assert.Equal(Enum.Parse<NativeScalarKind>(char.ToUpperInvariant(kind[0])+kind[1..]),value.Kind);Assert.Equal(Good(kind),value.DurationValue);Assert.Equal(kind=="dayTimeDuration"?"0":"12",value.DurationMonths.ToString());Assert.Equal(kind=="yearMonthDuration"?"0":(kind=="duration"?"86400.":"60.")+Fraction,value.DurationSeconds.ToInvariantString(1024));using var again=owner.Assess("<a>"+Alias(kind)+"</a>",context).Value!;Assert.Single(again.ReadRootItems());
        using var f=new Fixture(source,xml,atoms:16);using var other=new Fixture(source,xml,atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var handle=f.Provider.TypeComponent(fact.NativeType.DerivationBase!);Assert.True(f.Provider.Owns(handle));Assert.False(other.Provider.Owns(handle));var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>fact.ReadTypedValue(cancel.Token));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.Declaration);
    }
    /// <summary>Forward named dependencies and final prohibitions retain source edges for all duration families.</summary>
    [Fact]public void ForwardAndFinal()
    {
        foreach(var kind in Kinds){var source=Schema(Restrict("<xs:simpleType><xs:restriction base='B'/></xs:simpleType>"),true,"<xs:simpleType name='B'><xs:restriction base='xs:"+kind+"'/></xs:simpleType>");var c=Compile(source);Assert.Same(c.NamedTypes.Single(t=>t.Name=="B"),c.Root.Type.DerivationBase!.DerivationBase);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(source.Replace("name='B'","name='B' final='restriction'"),new(256,256),maxStringCharacters:1024).Outcome);}
    }
    /// <summary>Shared references and separate source definitions retain original unnamed bases.</summary>
    [Fact]public void SharedAndDistinct()
    {
        foreach(var kind in Kinds){var definition=Restrict(Base(kind));var c=Compile(Schema(definition,true,"<xs:element name='b' type='T'/>"));Assert.Same(c.Roots.First().Root.Type,c.Roots.Skip(1).First().Root.Type);var d=Compile(Schema(definition,false,"<xs:element name='b'>"+definition+"</xs:element>"));Assert.NotSame(d.Roots.First().Root.Type.DerivationBase,d.Roots.Skip(1).First().Root.Type.DerivationBase);Assert.DoesNotContain(d.NamedTypes,t=>ReferenceEquals(t,d.Root.Type.DerivationBase));}
    }
    /// <summary>Inline depth and shared source-site capacities retain their finite ceilings.</summary>
    [Fact]public void Budgets()
    {
        var definition=Base();for(var i=0;i<4;i++)definition=Restrict(definition);Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(definition),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(definition)),new(256,256),maxStringCharacters:1024).Outcome);var members=Enumerable.Repeat(Restrict(Base()),8).ToArray();string Union()=>"<xs:simpleType><xs:union>"+string.Concat(members)+"</xs:union></xs:simpleType>";Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);members[0]=Restrict(members[0]);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Months and seconds retain exact values beyond legacy decimal precision.</summary>
    /// <param name="kind">Duration family.</param>
    [Theory][InlineData("duration")][InlineData("yearMonthDuration")][InlineData("dayTimeDuration")]
    public void ExactPrecision(string kind)
    {
        const string digits="123456789012345678901234567890123456789";var lexical=kind=="yearMonthDuration"?"P"+digits+"M":"PT0."+digits+"S";using var owner=NativeSchemaSession.Create(Schema(Restrict(Base(kind))),new(256,256,1024,65536,2,256,16,256,1024)).Value!;using var d=owner.Assess("<a>"+lexical+"</a>",new(DateTimeOffset.UnixEpoch,0)).Value!;var value=Assert.Single(d.ReadRootItems());Assert.Equal(kind=="yearMonthDuration"?digits:"0",value.DurationMonths.ToString());Assert.Equal(kind=="yearMonthDuration"?"0":"0."+digits,value.DurationSeconds.ToInvariantString(1024));Assert.Equal(lexical,value.DurationValue);
    }
    /// <summary>General-duration partial order and conservative guards refuse values without issuing facts.</summary>
    [Fact]public void PartialOrderAndEmptyRange()
    {
        var source=Schema(Restrict(Base(),"<xs:minInclusive value='P1M'/>"));using var f=new Fixture(source,"<a>P30D</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Input.FactsFor((NativeSessionNode)f.Element([])));Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(Base(),"<xs:minInclusive value='P1M'/><xs:maxInclusive value='P30D'/>")),new(256,256),maxStringCharacters:1024).Outcome);var guarded=Compile(Schema(Restrict(Base(),"<xs:minInclusive value='P400Y'/>")));Assert.Equal(NativeOutcome.Unsupported,guarded.Assess("<a>P146097D</a>").Outcome);var empty=Compile(Schema(Restrict(Base(),"<xs:minExclusive value='P1M'/><xs:maxExclusive value='P1M'/>")));Assert.Equal(NativeOutcome.Invalid,empty.Assess("<a>P1M</a>").Outcome);
    }
    /// <summary>Nilled declarations retain original governing duration types and typed-empty values.</summary>
    [Fact]public void NilledAuthority()
    {
        foreach(var kind in Kinds){var source=Schema(Restrict(Base(kind))).Replace("<xs:element name='a'>","<xs:element name='a' nillable='true'>");using var f=new Fixture(source,"<a xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true'/>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal(Kind(kind),fact.NativeType.DurationKind);Assert.NotNull(fact.NativeType.DerivationBase);var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);Assert.Empty(typed.AtomicItems);}
    }
    /// <summary>Supplied and aggregate numeric limits precede publication and leave the owner usable.</summary>
    /// <param name="kind">Duration family.</param>
    [Theory][InlineData("duration")][InlineData("yearMonthDuration")][InlineData("dayTimeDuration")]
    public void NumericBudgets(string kind)
    {
        using var owner=NativeSchemaSession.Create(Schema(Restrict(Base(kind))),new(2,2,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);var bad=kind=="yearMonthDuration"?"P99Y":"P99D";var result=owner.Assess("<a>"+bad+"</a>",context);Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,result.Diagnostics[0].UnsupportedReason);using var d=owner.Assess("<a>"+(kind=="yearMonthDuration"?"P1Y":"PT1S")+"</a>",context).Value!;Assert.Single(d.ReadRootItems());
    }
}
