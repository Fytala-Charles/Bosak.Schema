// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies actual Gregorian facts and independent REC timeline arithmetic.
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
/// <summary>Verifies exact native Gregorian provenance, completion and partial order.</summary>
public sealed class NativeGregorianProofTests
{
    private static NativeGregorianValue Value(string text,string kind)=>NativeGregorianValue.Parse(text,NativeGregorianValue.KindFor(kind),256);
    private static string Lexical(string kind)=>kind switch {"gYear"=>"2000","gYearMonth"=>"2000-02","gMonth"=>"--02","gMonthDay"=>"--02-29",_=>"---31"};
    /// <summary>Checks real atomic/member proof, exact annotated payload, foreign refusal and revocation.</summary>
    /// <param name="kind">Gregorian primitive.</param><param name="named">Alias selector.</param><param name="union">Union selector.</param>
    [Theory]
    [InlineData("gYear",false,false)][InlineData("gYear",true,false)][InlineData("gYear",false,true)]
    [InlineData("gYearMonth",false,false)][InlineData("gYearMonth",true,false)][InlineData("gYearMonth",false,true)]
    [InlineData("gMonth",false,false)][InlineData("gMonth",true,false)][InlineData("gMonth",false,true)]
    [InlineData("gMonthDay",false,false)][InlineData("gMonthDay",true,false)][InlineData("gMonthDay",false,true)]
    [InlineData("gDay",false,false)][InlineData("gDay",true,false)][InlineData("gDay",false,true)]
    public void Provenance(string kind,bool named,bool union)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='N'><xs:restriction base='xs:"+kind+"'/></xs:simpleType><xs:simpleType name='U'><xs:union memberTypes='N xs:string'/></xs:simpleType><xs:element name='r' type='"+(union?"U":named?"N":"xs:"+kind)+"'/></xs:schema>";
        var lexical=Lexical(kind)+"-00:00";var xml="<r>"+lexical+"</r>";using var f=new Fixture(schema,xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Same(f.Input.Root,fact.Node);
        var typed=fact.ReadTypedValue(default).Value;Assert.True(typed.TryGetSingleAtomic(out var cell));Assert.Equal(XdmValueKind.String,cell.Value.Kind);Assert.Equal(lexical,cell.Value.StringValue);Assert.Equal(kind,cell.Value.SchemaTypeName);Assert.Same(f.Provider.TypeComponent(union?fact.NativeType.UnionMembers[0]:fact.NativeType),cell.TypeEvidence.ActualType);
        f.Run(op=>{var atom=f.Evaluate(op,f.Input.Root);Assert.True(SchemaAtomics.TryGetProvenance(atom,out var proof));Assert.Same(cell.TypeEvidence.ActualType,proof!.ActualType);Assert.Same(f.Assessment.Origin,proof.OriginalAssessment);return true;});
        using var other=new Fixture(schema,xml);Assert.False(other.Session.Advisor!.OwnsTypedValue(typed,other.Input,null));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>typed.TryGetSingleAtomic(out _));
    }
    /// <summary>Checks each missing-field completion against an independent full Gregorian date.</summary>
    /// <param name="kind">Primitive.</param><param name="text">Input.</param><param name="year">Completion year.</param><param name="month">Completion month.</param><param name="day">Completion day.</param>
    [Theory][InlineData("gYear","2000",2000,12,31)][InlineData("gYearMonth","2000-02",2000,2,29)][InlineData("gYearMonth","1900-02",1900,2,28)][InlineData("gMonth","--02",1972,2,29)][InlineData("gMonthDay","--02-29",1972,2,29)][InlineData("gDay","---01",1972,12,1)]
    public void ReferenceCompletion(string kind,string text,int year,int month,int day)
    {
        var expected=new BigInteger(new DateTime(year,month,day).Ticks/TimeSpan.TicksPerMinute);Assert.Equal(expected,Value(text,kind).Point(0));
    }
    /// <summary>Cross-checks bounded timeline arithmetic and unknown-zone intervals independently.</summary>
    [Fact]
    public void GregorianOracle()
    {
        foreach(int year in new[]{1,4,100,400,1696,1700,1900,1972,2000,2100,2400,9999})foreach(int month in Enumerable.Range(1,12))foreach(int zone in new[]{-840,-60,0,60,840})
        {
            var text=year.ToString("D4",System.Globalization.CultureInfo.InvariantCulture)+"-"+month.ToString("D2",System.Globalization.CultureInfo.InvariantCulture);var value=Value(text,"gYearMonth");var expected=new BigInteger(new DateTime(year,month,DateTime.DaysInMonth(year,month)).Ticks/TimeSpan.TicksPerMinute)-zone;Assert.Equal(expected,value.Point(zone));
        }
        Assert.Null(Value("---02","gDay").Compare(Value("---02Z","gDay")));Assert.Equal(1,Value("---03","gDay").Compare(Value("---02Z","gDay")));Assert.Equal(-1,Value("---01","gDay").Compare(Value("---02Z","gDay")));
    }
    /// <summary>Checks astronomical year zero and very large signed years without DateTime conversion.</summary>
    /// <param name="left">Cycle start.</param><param name="right">Cycle end.</param><param name="kind">Primitive.</param>
    [Theory][InlineData("0000","0400","gYear")][InlineData("-0400","0000","gYear")][InlineData("-9223372036854775807","-9223372036854775407","gYear")][InlineData("9223372036854775407-02","9223372036854775807-02","gYearMonth")]
    public void ExtendedYearCycle(string left,string right,string kind)=>Assert.Equal(new BigInteger(146097)*1440,Value(right,kind).Point(0)-Value(left,kind).Point(0));
    /// <summary>Separates equality, identity, primitive spaces and singleton-list equality.</summary>
    [Fact]
    public void Equality()
    {
        NativeElementState State(string kind,string text){var schema=NativeDecimalSchema.Compile("<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:"+kind+"'/></xs:schema>",new(256,256),maxStringCharacters:256).Value!;return schema.Assess("<r>"+text+"</r>").Value!.Elements[0];}
        var left=State("gDay","---01-12:00");var right=State("gDay","---02+12:00");Assert.True(NativeCompoundEquality.Equal(left,right));Assert.False(left.GregorianValue!.Value.IdenticalValue(right.GregorianValue!.Value));Assert.True(NativeCompoundEquality.Equal(left,right with { Items=[right] }));
        Assert.False(NativeCompoundEquality.Equal(State("gYear","2000"),State("gYearMonth","2000-12")));Assert.False(NativeCompoundEquality.Equal(State("gYear","2000"),State("string","2000")));Assert.False(NativeCompoundEquality.Equal(State("gYear","2000"),State("integer","2000")));Assert.False(NativeCompoundEquality.Equal(State("gYear","2000Z"),State("gYear","2000")));
        Assert.True(Value("-0000","gYear").SameValue(Value("0000","gYear")));Assert.True(Value("--02+00:00","gMonth").IdenticalValue(Value("--02Z","gMonth")));
    }
    /// <summary>Checks canonical shared types, anonymous definitions and complex governing/atomic separation.</summary>
    /// <param name="kind">Primitive.</param>
    [Theory][InlineData("gYear")][InlineData("gYearMonth")][InlineData("gMonth")][InlineData("gMonthDay")][InlineData("gDay")]
    public void Canonical(string kind)
    {
        var lexical=Lexical(kind);var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='T'><xs:restriction base='xs:"+kind+"'/></xs:simpleType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='T'/><xs:element name='b' type='T'/><xs:element name='c'><xs:simpleType><xs:restriction base='T'/></xs:simpleType></xs:element></xs:sequence></xs:complexType></xs:element></xs:schema>";
        using var f=new Fixture(schema,"<r><a>"+lexical+"</a><b>"+lexical+"</b><c>"+lexical+"</c></r>");var children=new List<NativeSessionNode>();foreach(var item in f.Input.Root.Children(XdmNodeKind.Element))children.Add((NativeSessionNode)item.NodeValue);var a=f.Input.FactsFor(children[0])!;var b=f.Input.FactsFor(children[1])!;var c=f.Input.FactsFor(children[2])!;Assert.Same(a.Type,b.Type);Assert.NotSame(a.Type,c.Type);Assert.Same(a.NativeType,c.NativeType.DerivationBase);
        using var complex=new Fixture("<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:simpleContent><xs:extension base='xs:"+kind+"'/></xs:simpleContent></xs:complexType></xs:element></xs:schema>","<r>"+lexical+"</r>");var fact=complex.Input.FactsFor((NativeSessionNode)complex.Input.Root)!;Assert.True(fact.ReadTypedValue(default).Value.TryGetSingleAtomic(out var atomic));Assert.NotSame(fact.Type,atomic.TypeEvidence.ActualType);Assert.Equal(kind,fact.NativeType.ContentType!.Name);
    }
    /// <summary>Checks finite work/cancellation and that invalid or empty-range assessments issue no facts.</summary>
    [Fact]
    public void BudgetAndInvalidFacts()
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='T'><xs:restriction base='xs:gYear'><xs:pattern value='[0-9]+'/></xs:restriction></xs:simpleType><xs:element name='r' type='T'/></xs:schema>";var plan=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:256).Value!;var facets=plan.Root.Type.GregorianFacets!;
        Assert.Equal(NativeOutcome.Unsupported,Assert.Throws<NativeProfileException>(()=>facets.Accepts(Value("2000","gYear"),"2000",new(default,1))).Outcome);using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.ThrowsAny<OperationCanceledException>(()=>facets.Accepts(Value("2000","gYear"),"2000",new(cancel.Token)));Assert.True(facets.Accepts(Value("2000","gYear"),"2000",new(default)));
        using var f=new Fixture(schema,"<r>bad</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Input.FactsFor((NativeSessionNode)f.Input.Root));
        using var empty=new Fixture(schema.Replace("<xs:pattern value='[0-9]+'/>","<xs:minExclusive value='2000'/><xs:maxExclusive value='2000'/>"),"<r>2000</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,empty.Assessment.Status);Assert.Empty(empty.Assessment.Facts);
    }
}
