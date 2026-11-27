// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Proves original anonymous Gregorian base identity, facets and component authority.
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
namespace Bosak.Schema.Tests.AnonymousGregorianBases;
/// <summary>Checks original unnamed Gregorian bases and bounded typed-value authority.</summary>
public sealed class NativeAnonymousGregorianBasesProofTests
{
    private static readonly string[] Kinds=["gYear","gYearMonth","gMonth","gMonthDay","gDay"];
    private static string Good(string kind)=>kind switch{"gYear"=>"2000","gYearMonth"=>"2000-02","gMonth"=>"--02","gMonthDay"=>"--02-29",_=>"---15"};
    private static string Before(string kind)=>kind switch{"gYear"=>"1999","gYearMonth"=>"2000-01","gMonth"=>"--01","gMonthDay"=>"--02-28",_=>"---14"};
    private static string After(string kind)=>kind switch{"gYear"=>"2001","gYearMonth"=>"2000-03","gMonth"=>"--03","gMonthDay"=>"--03-01",_=>"---16"};
    private static string Base(string kind="gYear",string facets="")=>"<xs:simpleType><xs:restriction base='xs:"+kind+"'>"+facets+"</xs:restriction></xs:simpleType>";
    private static string Restrict(string source,string facets="")=>"<xs:simpleType><xs:restriction>"+source+facets+"</xs:restriction></xs:simpleType>";
    private static string Schema(string definition,bool named=false,string extra="")=>"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+(named?"<xs:simpleType name='T'>"+definition["<xs:simpleType>".Length..]:"")+"<xs:element name='a'"+(named?" type='T'/>":">"+definition+"</xs:element>")+extra+"</xs:schema>";
    private static NativeDecimalSchema Compile(string source)=>NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;
    /// <summary>Named and unnamed outer types retain their original bases and exact scalar provenance.</summary>
    /// <param name="kind">Gregorian primitive.</param><param name="named">Outer naming selector.</param>
    [Theory][InlineData("gYear",false)][InlineData("gYear",true)][InlineData("gYearMonth",false)][InlineData("gYearMonth",true)][InlineData("gMonth",false)][InlineData("gMonth",true)][InlineData("gMonthDay",false)][InlineData("gMonthDay",true)][InlineData("gDay",false)][InlineData("gDay",true)]
    public void TypeAuthority(string kind,bool named)
    {
        using var f=new Fixture(Schema(Restrict(Base(kind)),named),"<a>"+Good(kind)+"Z</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var t=fact.NativeType;Assert.Null(t.BaseName);Assert.Equal(named?"T":null,t.Name);Assert.NotNull(t.DerivationBase);Assert.Null(t.DerivationBase!.Name);Assert.NotSame(t,t.DerivationBase);Assert.Equal(NativeGregorianValue.KindFor(kind),t.GregorianKind);Assert.Equal(t.GregorianKind,t.DerivationBase.GregorianKind);Assert.True(f.Provider.Owns(f.Provider.TypeComponent(t.DerivationBase)));var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);var atom=Assert.Single(typed.AtomicItems);Assert.Equal(XdmValueKind.String,atom.Value.Kind);Assert.Equal(kind,atom.Value.SchemaTypeName);Assert.Equal(Good(kind)+"Z",atom.Value.StringValue);Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);
    }
    /// <summary>Each family preserves inherited bound/fixed/timezone/pattern behavior and exclusions.</summary>
    /// <param name="kind">Gregorian primitive.</param>
    [Theory][InlineData("gYear")][InlineData("gYearMonth")][InlineData("gMonth")][InlineData("gMonthDay")][InlineData("gDay")]
    public void FacetAncestry(string kind)
    {
        var good=Good(kind);var original=Base(kind,"<xs:minInclusive value='"+good+"Z' fixed='true'/>");var c=Compile(Schema(Restrict(original,"<xs:maxInclusive value='"+After(kind)+"Z'/>")));Assert.NotNull(c.Root.Type.DerivationBase!.GregorianFacets);Assert.NotSame(c.Root.Type.GregorianFacets,c.Root.Type.DerivationBase.GregorianFacets);Assert.Equal(NativeOutcome.Valid,c.Assess("<a>"+good+"Z</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>"+Before(kind)+"Z</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>"+good+"</a>").Outcome);Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(Restrict(original,"<xs:minInclusive value='"+good+"+00:00'/>")),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(Schema(Restrict(original,"<xs:minInclusive value='"+After(kind)+"Z'/>")),new(256,256),maxStringCharacters:1024).Outcome);
        var patterned=Base(kind,"<xs:pattern value='"+good+".*'/>");var p=Compile(Schema(Restrict(patterned,"<xs:pattern value='.*Z'/>")));Assert.Equal(NativeOutcome.Valid,p.Assess("<a>"+good+"Z</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,p.Assess("<a>"+good+"</a>").Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(patterned,"<xs:enumeration value='"+good+"Z'/>")),new(256,256),maxStringCharacters:1024).Outcome);var zoned=Compile(Schema(Restrict(Base(kind,"<xs:explicitTimezone value='required'/>"))));Assert.Equal(NativeOutcome.Valid,zoned.Assess("<a>"+good+"Z</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,zoned.Assess("<a>"+good+"</a>").Outcome);
    }
    /// <summary>Detached components, recovery, foreign ownership, cancellation and expiry retain original contracts.</summary>
    /// <param name="kind">Gregorian primitive.</param>
    [Theory][InlineData("gYear")][InlineData("gYearMonth")][InlineData("gMonth")][InlineData("gMonthDay")][InlineData("gDay")]
    public void ComponentsAndLifetime(string kind)
    {
        var source=Schema(Restrict(Base(kind)));var xml="<a>"+Good(kind)+"Z</a>";using var owner=NativeSchemaSession.Create(source,new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<a>bad</a>",context).Outcome);using var d=owner.Assess(xml,context).Value!;var value=Assert.Single(d.ReadRootItems());d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootItems());Assert.Equal(Enum.Parse<NativeScalarKind>("G"+kind[1..]),value.Kind);Assert.Equal(Good(kind)+"Z",value.GregorianValue);Assert.Equal(kind is "gYear" or "gYearMonth"?2000L:(long?)null,value.GregorianYear);Assert.Equal(kind is "gYearMonth" or "gMonth" or "gMonthDay"?2:(int?)null,value.GregorianMonth);Assert.Equal(kind=="gMonthDay"?29:kind=="gDay"?15:(int?)null,value.GregorianDay);Assert.Equal(0,value.GregorianTimezoneOffsetMinutes);using var again=owner.Assess("<a>"+Good(kind)+"</a>",context).Value!;Assert.Null(Assert.Single(again.ReadRootItems()).GregorianTimezoneOffsetMinutes);
        using var f=new Fixture(source,xml,atoms:16);using var other=new Fixture(source,xml,atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var handle=f.Provider.TypeComponent(fact.NativeType.DerivationBase!);Assert.True(f.Provider.Owns(handle));Assert.False(other.Provider.Owns(handle));var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>fact.ReadTypedValue(cancel.Token));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.Declaration);
    }
    /// <summary>Forward named dependencies and final prohibitions retain original source edges for every family.</summary>
    [Fact]public void ForwardAndFinal()
    {
        foreach(var kind in Kinds){var source=Schema(Restrict("<xs:simpleType><xs:restriction base='B'/></xs:simpleType>"),true,"<xs:simpleType name='B'><xs:restriction base='xs:"+kind+"'/></xs:simpleType>");var c=Compile(source);Assert.Same(c.NamedTypes.Single(t=>t.Name=="B"),c.Root.Type.DerivationBase!.DerivationBase);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(source.Replace("name='B'","name='B' final='restriction'"),new(256,256),maxStringCharacters:1024).Outcome);}
    }
    /// <summary>Shared references and separate source definitions retain their own original unnamed bases.</summary>
    [Fact]public void SharedAndDistinct()
    {
        foreach(var kind in Kinds){var definition=Restrict(Base(kind));var c=Compile(Schema(definition,true,"<xs:element name='b' type='T'/>"));Assert.Same(c.Roots.First().Root.Type,c.Roots.Skip(1).First().Root.Type);var d=Compile(Schema(definition,false,"<xs:element name='b'>"+definition+"</xs:element>"));Assert.NotSame(d.Roots.First().Root.Type.DerivationBase,d.Roots.Skip(1).First().Root.Type.DerivationBase);Assert.DoesNotContain(d.NamedTypes,t=>ReferenceEquals(t,d.Root.Type.DerivationBase));}
    }
    /// <summary>Inline depth and shared source-site capacities retain explicit finite ceilings.</summary>
    [Fact]public void Budgets()
    {
        var definition=Base();for(var i=0;i<4;i++)definition=Restrict(definition);Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(definition),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(definition)),new(256,256),maxStringCharacters:1024).Outcome);var members=Enumerable.Repeat(Restrict(Base()),8).ToArray();string Union()=>"<xs:simpleType><xs:union>"+string.Concat(members)+"</xs:union></xs:simpleType>";Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);members[0]=Restrict(members[0]);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Reference-component equality admits equal instants without inventing absent public components.</summary>
    [Fact]public void ReferenceEquality()
    {
        foreach(var kind in new[]{"gDay","gMonthDay"}){var literal=kind=="gDay"?"---15+14:00":"--03-01+14:00";var equal=kind=="gDay"?"---14-10:00":"--02-29-10:00";using var owner=NativeSchemaSession.Create(Schema(Restrict(Base(kind),"<xs:enumeration value='"+literal+"'/>")),new(256,256,1024,65536,2,256,16,256,1024)).Value!;using var d=owner.Assess("<a>"+equal+"</a>",new(DateTimeOffset.UnixEpoch,0)).Value!;var value=Assert.Single(d.ReadRootItems());Assert.Equal(equal,value.GregorianValue);Assert.Null(value.GregorianYear);Assert.Equal(kind=="gDay"?14:29,value.GregorianDay);Assert.Equal(-600,value.GregorianTimezoneOffsetMinutes);}
    }
    /// <summary>Year capacity refusal precedes facts and leaves the owner usable at the bounded endpoint.</summary>
    /// <param name="kind">Year-bearing primitive.</param>
    [Theory][InlineData("gYear")][InlineData("gYearMonth")]public void YearCapacity(string kind)
    {
        var suffix=kind=="gYearMonth"?"-02":"";using var owner=NativeSchemaSession.Create(Schema(Restrict(Base(kind))),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);var result=owner.Assess("<a>-9223372036854775808"+suffix+"Z</a>",context);Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,result.Diagnostics[0].UnsupportedReason);using var d=owner.Assess("<a>-9223372036854775807"+suffix+"Z</a>",context).Value!;Assert.Equal(-long.MaxValue,Assert.Single(d.ReadRootItems()).GregorianYear);
    }
    /// <summary>Nilled declarations retain original Gregorian governing types and typed-empty values.</summary>
    [Fact]public void NilledAuthority()
    {
        foreach(var kind in Kinds){var source=Schema(Restrict(Base(kind))).Replace("<xs:element name='a'>","<xs:element name='a' nillable='true'>");using var f=new Fixture(source,"<a xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true'/>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal(NativeGregorianValue.KindFor(kind),fact.NativeType.GregorianKind);Assert.NotNull(fact.NativeType.DerivationBase);var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);Assert.Empty(typed.AtomicItems);}
    }
}
