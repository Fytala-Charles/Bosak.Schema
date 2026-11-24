// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies named decimal inheritance and exact base-value constraints.
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
namespace Bosak.Schema.Tests.DecimalChains;
/// <summary>Verifies actual decimal ancestry and inherited exact value constraints.</summary>
public sealed class NativeDecimalChainsIdentityTests
{
 private const string Prefix="<xs:restriction xmlns:xs='http://www.w3.org/2001/XMLSchema'>";
 private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='A'><xs:restriction base='xs:decimal'><xs:enumeration value='1.23'/><xs:enumeration value='2.34'/></xs:restriction></xs:simpleType><xs:simpleType name='D'><xs:restriction base='A'><xs:enumeration value='1.2300'/></xs:restriction></xs:simpleType><xs:element name='r' type='D'/></xs:schema>";
 /// <summary>Actual named bases and original normalized exact values are retained.</summary>
 [Fact]public void Components(){using var f=new Fixture(Schema,"<r>+001.2300</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("D",fact.NativeType.Name);Assert.Equal("A",fact.NativeType.DerivationBase!.Name);Assert.True(fact.NativeType.Facets!.HasEnumeration);Assert.True(fact.NativeType.DerivationBase.Facets!.HasEnumeration);var value=f.Read(f.Input.Root).DecimalValue;Assert.Equal(123,value.Coefficient);Assert.Equal(2,value.Scale);f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));Assert.Equal("1.23",value.ToInvariantString(256));}
 /// <summary>A base-valid value excluded by the child issues no successful facts.</summary>
 [Fact]public void InvalidFacts(){using var f=new Fixture(Schema,"<r>2.34</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);}
 /// <summary>Every local enumeration declaration must be datatype-valid for its base.</summary>
 [Fact]public void BaseMembers(){var limits=new NativeNumericLimits(256,256);var a=NativeDecimalFacets.Compile(XElement.Parse(Prefix+"<xs:enumeration value='1'/></xs:restriction>"),limits,allowEnumeration:true);var e=Assert.Throws<NativeProfileException>(()=>NativeDecimalFacets.CompileDerived(XElement.Parse(Prefix+"<xs:enumeration value='1'/><xs:enumeration value='2'/></xs:restriction>"),a,limits,nativeDerivation:true));Assert.Equal(NativeOutcome.Invalid,e.Outcome);}
 /// <summary>Repeating an inherited exclusive endpoint uses the REC exception without reopening it.</summary>
 [Fact]public void ExclusiveEndpoint(){var limits=new NativeNumericLimits(256,256);var a=NativeDecimalFacets.Compile(XElement.Parse(Prefix+"<xs:maxExclusive value='100'/><xs:totalDigits value='1'/></xs:restriction>"),limits);var d=NativeDecimalFacets.CompileDerived(XElement.Parse(Prefix+"<xs:maxExclusive value='100'/></xs:restriction>"),a,limits,nativeDerivation:true);Assert.True(d.Accepts(NativeDecimalSchema.ParseDecimal("1",limits,NativeOutcome.Invalid)));var e=Assert.Throws<NativeProfileException>(()=>NativeDecimalFacets.CompileDerived(XElement.Parse(Prefix+"<xs:maxInclusive value='100'/></xs:restriction>"),a,limits,nativeDerivation:true));Assert.Equal(NativeOutcome.Invalid,e.Outcome);}
}
