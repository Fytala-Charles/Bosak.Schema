// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies exact calendar contracts through both public facades.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 25-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Xml.Linq;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.NativeCalendars;
/// <summary>Checks fixed facets, inherited boundaries and shared pattern work.</summary>
public sealed class NativeCalendarFacetReviewTests
{
    private const string Start="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>";
    private const string Literal="2000-01-01T00:00:00Z";
    private static string Schema(string facets,string inherited="")=>Start+inherited+"<xs:simpleType name='N'><xs:restriction base='"+(inherited.Length==0?"xs:dateTime":"B")+"'>"+facets+"</xs:restriction></xs:simpleType><xs:element name='r' type='N'/></xs:schema>";
    /// <summary>Checks both successful matching and budget/cancellation refusal.</summary>
    [Fact]
    public void CAL088_SharedWorkBudget()
    {
        var compiled=NativeDecimalSchema.Compile(Schema("<xs:pattern value='2000.*'/>"),new(256,256),maxStringCharacters:256);Assert.Equal(NativeOutcome.Valid,compiled.Outcome);
        var facets=compiled.Value!.Root.Type.CalendarFacets!;var value=XdmExactCalendarValue.Parse(Literal,XdmExactCalendarKind.DateTime,256,256);
        var shared=new NativePatternBudget(default,10000);Assert.True(facets.Accepts(value,Literal,shared));Assert.True(shared.Consumed>0);
        var refused=Assert.Throws<NativeProfileException>(()=>facets.Accepts(value,Literal,new NativePatternBudget(default,1)));Assert.Equal(NativeOutcome.Unsupported,refused.Outcome);Assert.Equal(NativeDecimalUnsupportedReason.RepresentationCapacity,refused.UnsupportedReason);
        using var cancellation=new CancellationTokenSource();cancellation.Cancel();Assert.ThrowsAny<OperationCanceledException>(()=>facets.Accepts(value,Literal,new NativePatternBudget(cancellation.Token)));
    }
    /// <summary>Checks immutable fixed constraints and inherited exclusive endpoint exceptions.</summary>
    /// <param name="baseFacet">Base facet markup.</param><param name="derived">Derived facet markup.</param><param name="expected">Expected compilation result.</param>
    [Theory]
    [InlineData("<xs:explicitTimezone value='optional' fixed='true'/>","<xs:explicitTimezone value='required'/>" ,"Invalid")]
    [InlineData("<xs:minInclusive value='2000-01-01T00:00:00Z' fixed='true'/>","<xs:minInclusive value='2000-01-01T01:00:00+01:00'/>" ,"Valid")]
    [InlineData("<xs:minInclusive value='2000-01-01T00:00:00Z' fixed='true'/>","<xs:minInclusive value='2001-01-01T00:00:00Z'/>" ,"Invalid")]
    [InlineData("<xs:minExclusive value='2000-01-01T00:00:00Z'/>","<xs:minExclusive value='2000-01-01T00:00:00Z'/>" ,"Valid")]
    [InlineData("<xs:minExclusive value='2000-01-01T00:00:00Z'/>","<xs:minInclusive value='2000-01-01T00:00:00Z'/>" ,"Invalid")]
    [InlineData("<xs:pattern value='2000.*'/>","<xs:minInclusive value='2000-01-01T00:00:00Z'/>" ,"Unsupported")]
    public void Inherited(string baseFacet,string derived,string expected)
    {
        var baseType="<xs:simpleType name='B'><xs:restriction base='xs:dateTime'>"+baseFacet+"</xs:restriction></xs:simpleType>";
        Assert.Equal(Enum.Parse<NativeOutcome>(expected),NativeDecimalSchema.Compile(Schema(derived,baseType),new(256,256),maxStringCharacters:256).Outcome);
    }
    /// <summary>Checks pattern literal whitespace is preserved while instance whitespace collapses.</summary>
    [Fact]
    public void PatternLiteralWhitespace()
    {
        var exact=NativeDecimalSchema.Compile(Schema("<xs:pattern value='2000.*'/>"),new(256,256),maxStringCharacters:256).Value!;Assert.Equal(NativeOutcome.Valid,exact.Assess("<r>  "+Literal+" </r>").Outcome);
        var spaced=NativeDecimalSchema.Compile(Schema("<xs:pattern value=' 2000.* '/>"),new(256,256),maxStringCharacters:256).Value!;Assert.Equal(NativeOutcome.Invalid,spaced.Assess("<r>"+Literal+"</r>").Outcome);
    }
    /// <summary>Checks signed-zero year mapping without losing original pattern text.</summary>
    /// <param name="kind">Calendar family.</param>
    [Theory][InlineData("dateTime")][InlineData("dateTimeStamp")]
    public void SignedZeroYear(string kind)
    {
        var token="0000-02-29T00:00:00Z";
        var schema=Start+"<xs:simpleType name='N'><xs:restriction base='xs:"+kind+"'><xs:enumeration value='"+token+"'/><xs:pattern value='-0000.*'/></xs:restriction></xs:simpleType><xs:element name='r' type='N'/></xs:schema>";
        var compiled=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:256);Assert.Equal(NativeOutcome.Valid,compiled.Outcome);
        var signed=compiled.Value!.Assess("<r>-"+token+"</r>");Assert.Equal(NativeOutcome.Valid,signed.Outcome);Assert.Equal(0,signed.Value!.Elements[0].CalendarValue!.Value.Year);
        Assert.Equal(NativeOutcome.Invalid,compiled.Value.Assess("<r>"+token+"</r>").Outcome);
    }
}
