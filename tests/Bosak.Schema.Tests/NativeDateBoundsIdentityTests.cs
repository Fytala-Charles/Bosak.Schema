// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies date-bound partial ordering and original-node identity.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 24-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 24-11-2026     | Adopt qualified date enumeration controls |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.DateBounds;
/// <summary>Verifies exact partial ordering and bounded date facts.</summary>
public sealed class NativeDateBoundsIdentityTests
{
 /// <summary>Partial ordering preserves indeterminacy and symmetry.</summary><param name="a">First.</param><param name="b">Second.</param><param name="expected">Expected comparison.</param>
 [Theory]
 [InlineData("2000-01-01","2000-01-01Z",null)]
 [InlineData("2000-01-01","2000-01-02Z",-1)]
 [InlineData("2000-01-02","2000-01-01Z",1)]
 [InlineData("2000-01-01+13:00","1999-12-31-11:00",0)]
 [InlineData("0000-01-01+13:00","-0001-12-31-11:00",0)]
 [InlineData("9223372036854775807-12-31Z","2000-01-01",1)]
 [InlineData("-9223372036854775807-01-01Z","2000-01-01",-1)]
 [InlineData("2000-01-01","2000-01-01+14:00",null)]
 public void PartialOrder(string a,string b,int? expected)
 {
  var x=NativeDateParser.Parse(a);var y=NativeDateParser.Parse(b);Assert.Equal(expected,NativeDateFacets.Compare(x,y));Assert.Equal(expected is null?null:-expected,NativeDateFacets.Compare(y,x));
 }
 private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='D'><xs:restriction base='xs:date'><xs:minInclusive value='2000-01-01Z'/><xs:maxExclusive value='2000-01-03Z'/><xs:explicitTimezone value='required'/></xs:restriction></xs:simpleType><xs:complexType name='C'><xs:simpleContent><xs:extension base='D'/></xs:simpleContent></xs:complexType><xs:element name='r' type='C'/></xs:schema>";
 /// <summary>Original governing and atomic types retain bounded date facts.</summary>
 [Fact]public void OriginalFacts(){using var f=new Fixture(Schema,"<r>2000-01-02Z</r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("C",fact.NativeType.Name);Assert.Equal("D",fact.NativeType.ContentType!.Name);var value=f.Read(f.Input.Root).DateValue;Assert.Equal(2,value.Day);Assert.Equal(0,value.TimezoneOffsetMinutes);f.Input.Dispose();Assert.Equal(2,value.Day);}
 /// <summary>Invalid bounded dates produce no successful partial state.</summary>
 [Fact]public void InvalidFacts(){using var f=new Fixture(Schema,"<r>2000-01-04Z</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);}
}
