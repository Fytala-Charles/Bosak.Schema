// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies exact duration provenance, partial order and finite budgets.
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
using System.Numerics;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Fixture=Bosak.Schema.Tests.NodeData.Fixture;
using Xunit;
namespace Bosak.Schema.Tests;
/// <summary>Verifies actual exact duration facts and REC partial ordering.</summary>
public sealed class NativeDurationProofTests
{
    private static NativeDurationValue Value(string text,NativeDurationKind kind=NativeDurationKind.Duration)=>NativeDurationValue.Parse(text,kind,new(256,256),256);
    /// <summary>Checks real member proof, exact payload, foreign refusal and revocation.</summary>
    /// <param name="kind">Primitive or derived builtin.</param><param name="named">Alias selector.</param><param name="union">Union selector.</param>
    [Theory]
    [InlineData("duration",false,false)][InlineData("duration",true,false)][InlineData("duration",false,true)]
    [InlineData("yearMonthDuration",false,false)][InlineData("yearMonthDuration",true,false)][InlineData("yearMonthDuration",false,true)]
    [InlineData("dayTimeDuration",false,false)][InlineData("dayTimeDuration",true,false)][InlineData("dayTimeDuration",false,true)]
    public void Provenance(string kind,bool named,bool union)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='N'><xs:restriction base='xs:"+kind+"'/></xs:simpleType><xs:simpleType name='U'><xs:union memberTypes='N xs:string'/></xs:simpleType><xs:element name='r' type='"+(union?"U":named?"N":"xs:"+kind)+"'/></xs:schema>";
        var lexical=kind=="yearMonthDuration"?"P123456789012345678901234567890M":"PT0.123456789012345678901234567890123456789S";var xml="<r>"+lexical+"</r>";
        using var f=new Fixture(schema,xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Same(f.Input.Root,fact.Node);
        var typed=fact.ReadTypedValue(default).Value;Assert.True(typed.TryGetSingleAtomic(out var cell));Assert.Equal(XdmValueKind.Duration,cell.Value.Kind);Assert.Equal(lexical,cell.Value.DurationValue);Assert.Equal(kind,cell.Value.SchemaTypeName);
        Assert.Same(f.Provider.TypeComponent(union?fact.NativeType.UnionMembers[0]:fact.NativeType),cell.TypeEvidence.ActualType);
        f.Run(op=>{var atom=f.Evaluate(op,f.Input.Root);Assert.True(SchemaAtomics.TryGetProvenance(atom,out var proof));Assert.Same(cell.TypeEvidence.ActualType,proof!.ActualType);Assert.Same(f.Assessment.Origin,proof.OriginalAssessment);return true;});
        using var other=new Fixture(schema,xml);Assert.False(other.Session.Advisor!.OwnsTypedValue(typed,other.Input,null));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>typed.TryGetSingleAtomic(out _));
    }
    /// <summary>Checks component identity across derived subsets and scalar/list values.</summary>
    [Fact]
    public void Equality()
    {
        NativeElementState State(string kind,string text)
        {
            var schema=NativeDecimalSchema.Compile("<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:"+kind+"'/></xs:schema>",new(256,256),maxStringCharacters:256).Value!;return schema.Assess("<r>"+text+"</r>").Value!.Elements[0];
        }
        var value=State("duration","P1Y");var derived=State("yearMonthDuration","P12M");Assert.True(NativeCompoundEquality.Equal(value,derived));Assert.True(NativeCompoundEquality.Equal(value,derived with { Items=[derived] }));
        Assert.True(NativeCompoundEquality.Equal(State("yearMonthDuration","P0M"),State("dayTimeDuration","PT0S")));Assert.False(NativeCompoundEquality.Equal(State("duration","P1M"),State("duration","P30D")));
        Assert.False(NativeCompoundEquality.Equal(State("duration","PT1S"),State("decimal","1")));Assert.False(NativeCompoundEquality.Equal(State("duration","P1D"),State("string","P1D")));
        Assert.True(Value("PT0.10S").SameValue(Value("PT0.1S")));Assert.True(Value("-P0D").SameValue(Value("PT0S")));
    }
    /// <summary>Checks independently specified reference ordering examples and exact fractions.</summary>
    /// <param name="left">Left lexical value.</param><param name="right">Right lexical value.</param><param name="expected">Expected order or incomparable.</param>
    [Theory]
    [InlineData("P1M","P27D",1)][InlineData("P1M","P28D",null)][InlineData("P1M","P29D",null)][InlineData("P1M","P30D",null)][InlineData("P1M","P31D",null)][InlineData("P1M","P32D",-1)]
    [InlineData("P1Y","P364D",1)][InlineData("P1Y","P365D",null)][InlineData("P1Y","P366D",null)][InlineData("P1Y","P367D",-1)]
    [InlineData("-P1M","-P27D",-1)][InlineData("-P1M","-P32D",1)][InlineData("P12M","P1Y",0)][InlineData("P1D","PT24H",0)]
    [InlineData("PT0.000000000000000000000000001S","PT0.000000000000000000000000002S",-1)]
    [InlineData("P1000000000000000000000Y","P999999999999999999999Y",1)]
    public void PartialOrder(string left,string right,int? expected)=>Assert.Equal(expected,Value(left).Compare(Value(right)));
    /// <summary>Cross-checks arbitrary-precision day arithmetic against independent bounded .NET dates.</summary>
    [Fact]
    public void GregorianOracle()
    {
        var dates=new[]{new DateTime(1696,9,1),new DateTime(1697,2,1),new DateTime(1903,3,1),new DateTime(1903,7,1)};
        for(int months=-12000;months<=12000;months+=137)
        {
            var left=Value((months<0?"-":"")+"P"+Math.Abs(months)+"M");var right=Value((months<0?"-":"")+"P"+(Math.Abs(months)*30)+"D");
            int? expected=null;bool mismatch=false;foreach(var date in dates){var order=date.AddMonths(months).CompareTo(date.AddDays(months*30));if(expected is not null&&order!=expected)mismatch=true;expected=order;}
            Assert.Equal(mismatch?null:expected,left.Compare(right));
        }
    }
    /// <summary>Refuses conflicting coincident-projection equality rather than conflating components.</summary>
    [Fact]
    public void GregorianCycleBoundary()
    {
        var left=Value("P400Y");var right=Value("P146097D");Assert.False(left.SameValue(right));Assert.Equal(NativeOutcome.Unsupported,Assert.Throws<NativeProfileException>(()=>left.Compare(right)).Outcome);
    }
    /// <summary>Checks primitive subset ancestry, canonical type sharing and anonymous complex identity.</summary>
    /// <param name="kind">Builtin family.</param>
    [Theory][InlineData("duration")][InlineData("yearMonthDuration")][InlineData("dayTimeDuration")]
    public void Canonical(string kind)
    {
        var lexical=kind=="yearMonthDuration"?"P1M":"P1D";
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='T'><xs:restriction base='xs:"+kind+"'/></xs:simpleType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='T'/><xs:element name='b' type='T'/><xs:element name='c'><xs:simpleType><xs:restriction base='T'/></xs:simpleType></xs:element></xs:sequence></xs:complexType></xs:element></xs:schema>";
        using var f=new Fixture(schema,"<r><a>"+lexical+"</a><b>"+lexical+"</b><c>"+lexical+"</c></r>");var children=new List<NativeSessionNode>();foreach(var child in f.Input.Root.Children(XdmNodeKind.Element))children.Add((NativeSessionNode)child.NodeValue);
        var a=f.Input.FactsFor(children[0])!;var b=f.Input.FactsFor(children[1])!;var c=f.Input.FactsFor(children[2])!;Assert.Same(a.Type,b.Type);Assert.NotSame(a.Type,c.Type);Assert.Same(a.NativeType,c.NativeType.DerivationBase);
        if(kind!="duration")Assert.Equal("duration",a.NativeType.DerivationBase!.DerivationBase!.Name);
        using var complex=new Fixture("<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:simpleContent><xs:extension base='xs:"+kind+"'/></xs:simpleContent></xs:complexType></xs:element></xs:schema>","<r>"+lexical+"</r>");var fact=complex.Input.FactsFor((NativeSessionNode)complex.Input.Root)!;Assert.True(fact.ReadTypedValue(default).Value.TryGetSingleAtomic(out var item));Assert.NotSame(fact.Type,item.TypeEvidence.ActualType);Assert.Equal(kind,fact.NativeType.ContentType!.Name);
    }
    /// <summary>Checks matching work, cancellation and invalid assessments publish no facts.</summary>
    [Fact]
    public void BudgetsAndInvalidFacts()
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='T'><xs:restriction base='xs:duration'><xs:pattern value='P[0-9]+D'/></xs:restriction></xs:simpleType><xs:element name='r' type='T'/></xs:schema>";var plan=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:256).Value!;var facets=plan.Root.Type.DurationFacets!;
        Assert.Equal(NativeOutcome.Unsupported,Assert.Throws<NativeProfileException>(()=>facets.Accepts(Value("P111D"),"P111D",new(default,1))).Outcome);using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.ThrowsAny<OperationCanceledException>(()=>facets.Accepts(Value("P1D"),"P1D",new(cancel.Token)));Assert.True(facets.Accepts(Value("P1D"),"P1D",new(default)));
        using var f=new Fixture(schema,"<r>bad</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Input.FactsFor((NativeSessionNode)f.Input.Root));
    }
}
