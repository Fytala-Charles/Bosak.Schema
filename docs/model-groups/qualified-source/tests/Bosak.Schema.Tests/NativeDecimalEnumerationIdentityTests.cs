// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies decimal enumeration identity, exact values and invalid facts.
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
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.DecimalEnumeration;
/// <summary>Verifies actual decimal enumeration components and exact typed values.</summary>
public sealed class NativeDecimalEnumerationIdentityTests
{
 private const string Types="<xs:simpleType name='D'><xs:restriction base='xs:decimal'><xs:enumeration value='1.23'/></xs:restriction></xs:simpleType>";
 /// <summary>Named enumeration retains original normalized exact decimal facts and lifetime.</summary>
 [Fact]public void Components(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+Types+"<xs:element name='r' type='D'/></xs:schema>";using var f=new Fixture(schema,"<r>+001.2300</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("D",fact.NativeType.Name);Assert.True(fact.NativeType.Facets!.HasEnumeration);var value=f.Read(f.Input.Root).DecimalValue;Assert.Equal(123,value.Coefficient);Assert.Equal(2,value.Scale);f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));Assert.Equal("1.23",value.ToInvariantString(256));}
 /// <summary>Extension governing and atomic enumeration components stay distinct.</summary>
 [Fact]public void Extension(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+Types+"<xs:complexType name='G'><xs:simpleContent><xs:extension base='D'/></xs:simpleContent></xs:complexType><xs:element name='r' type='G'/></xs:schema>";using var f=new Fixture(schema,"<r>1.230</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("G",fact.NativeType.Name);Assert.Equal("D",fact.NativeType.ContentType!.Name);Assert.True(fact.NativeType.ContentType.Facets!.HasEnumeration);Assert.NotSame(fact.NativeType,fact.NativeType.ContentType);Assert.Equal("1.23",f.Read(f.Input.Root).DecimalValue.ToInvariantString(256));}
 /// <summary>Failed membership exposes no successful facts or evidence.</summary>
 [Fact]public void InvalidFacts(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+Types+"<xs:element name='r' type='D'/></xs:schema>";using var f=new Fixture(schema,"<r>1.24</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);}
 /// <summary>Signed zero and equivalent decimal spellings have identical membership keys.</summary>
 [Fact]public void ExactKeys(){var r=XElement.Parse("<xs:restriction xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:enumeration value='-0.00'/><xs:enumeration value='+001.2300'/></xs:restriction>");var limits=new NativeNumericLimits(256,256);var facets=NativeDecimalFacets.Compile(r,limits,allowEnumeration:true);Assert.True(facets.Accepts(NativeDecimalSchema.ParseDecimal("+0.0",limits,NativeOutcome.Invalid)));Assert.True(facets.Accepts(NativeDecimalSchema.ParseDecimal("1.230",limits,NativeOutcome.Invalid)));Assert.False(facets.Accepts(NativeDecimalSchema.ParseDecimal("1.2301",limits,NativeOutcome.Invalid)));}
}
