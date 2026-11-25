// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies named date chain component identity and effective selection.
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
namespace Bosak.Schema.Tests.DateChains;
/// <summary>Checks original components and effective types for named date chains.</summary>
public sealed class NativeDateChainsIdentityTests
{
 private const string Types="<xs:simpleType name='B'><xs:restriction base='A'><xs:maxExclusive value='2000-01-03Z'/></xs:restriction></xs:simpleType><xs:simpleType name='A'><xs:restriction base='xs:date'><xs:minInclusive value='2000-01-01Z'/></xs:restriction></xs:simpleType>";
 /// <summary>Complex governing type, atomic restriction and base remain distinct components.</summary>
 [Fact]public void Components(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+Types+"<xs:complexType name='C'><xs:simpleContent><xs:extension base='B'/></xs:simpleContent></xs:complexType><xs:element name='r' type='C'/></xs:schema>";using var f=new Fixture(schema,"<r>2000-01-02Z</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("C",fact.NativeType.Name);var atomic=fact.NativeType.ContentType!;Assert.Equal("B",atomic.Name);Assert.Equal("A",atomic.DerivationBase!.Name);Assert.NotSame(atomic,atomic.DerivationBase);Assert.Equal(2,f.Read(f.Input.Root).DateValue.Day);}
 /// <summary>Selected derived type enforces inherited bounds while keeping original declaration.</summary>
 [Fact]public void Selection(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+Types+"<xs:element name='r' type='xs:date'/></xs:schema>";using var f=new Fixture(schema,"<r xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='B'>2000-01-02Z</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("B",fact.NativeType.Name);Assert.Equal("date",fact.NativeDeclaration.Type.Name);Assert.Equal(2,f.Read(f.Input.Root).DateValue.Day);using var bad=new Fixture(schema,"<r xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='B'>2000-01-04Z</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);}
 /// <summary>Anonymous aliases retain no global name while inheriting the original named base.</summary>
 [Fact]public void Anonymous(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+Types+"<xs:element name='r'><xs:simpleType><xs:restriction base='B'/></xs:simpleType></xs:element></xs:schema>";using var f=new Fixture(schema,"<r>2000-01-02Z</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Null(fact.NativeType.Name);Assert.Equal("B",fact.NativeType.DerivationBase!.Name);var detached=f.Read(f.Input.Root).DateValue;f.Input.Dispose();Assert.Equal(2,detached.Day);}
}
