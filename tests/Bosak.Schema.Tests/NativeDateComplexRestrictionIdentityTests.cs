// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies governing and atomic identity for complex date restriction.
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
namespace Bosak.Schema.Tests.DateComplexRestriction;
/// <summary>Checks distinct governing/atomic components and original typed date facts.</summary>
public sealed class NativeDateComplexRestrictionIdentityTests
{
 private const string Types="<xs:simpleType name='A'><xs:restriction base='xs:date'><xs:minInclusive value='2000-01-01Z'/></xs:restriction></xs:simpleType><xs:complexType name='C'><xs:simpleContent><xs:extension base='A'/></xs:simpleContent></xs:complexType><xs:complexType name='R'><xs:simpleContent><xs:restriction base='C'><xs:maxExclusive value='2000-01-03Z'/></xs:restriction></xs:simpleContent></xs:complexType>";
 /// <summary>Original declaration/governing/atomic/base components stay distinct and owned.</summary>
 [Fact]public void Components(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+Types+"<xs:element name='r' type='R'/></xs:schema>";using var f=new Fixture(schema,"<r>2000-01-02Z</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;var governing=fact.NativeType;Assert.Same(fact.NativeDeclaration.Type,governing);Assert.Equal("R",governing.Name);Assert.True(governing.IsComplexRestriction);Assert.Equal("C",governing.DerivationBase!.Name);var atomic=governing.ContentType!;Assert.Null(atomic.Name);Assert.True(atomic.IsDate);Assert.Equal("A",atomic.DerivationBase!.Name);Assert.Same(governing.DerivationBase.ContentType,atomic.DerivationBase);Assert.NotSame(governing,atomic);var detached=f.Read(f.Input.Root).DateValue;Assert.Equal(2,detached.Day);Assert.Equal(0,detached.TimezoneOffsetMinutes);f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));Assert.Equal(2,detached.Day);}
 /// <summary>Anonymous complex/atomic types receive no invented global names.</summary>
 [Fact]public void Anonymous(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+Types+"<xs:element name='r'><xs:complexType><xs:simpleContent><xs:restriction base='C'><xs:maxExclusive value='2000-01-03Z'/></xs:restriction></xs:simpleContent></xs:complexType></xs:element></xs:schema>";using var f=new Fixture(schema,"<r>2000-01-02Z</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Null(fact.NativeType.Name);Assert.Null(fact.NativeType.ContentType!.Name);Assert.Equal("C",fact.NativeType.DerivationBase!.Name);Assert.Equal(2,f.Read(f.Input.Root).DateValue.Day);}
 /// <summary>Invalid restricted values expose no successful partial facts.</summary>
 [Fact]public void InvalidFacts(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+Types+"<xs:element name='r' type='R'/></xs:schema>";using var f=new Fixture(schema,"<r>2000-01-04Z</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);}
}
