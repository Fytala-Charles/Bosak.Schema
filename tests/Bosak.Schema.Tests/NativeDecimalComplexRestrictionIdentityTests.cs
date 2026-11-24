// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies decimal complex restriction components and original typed facts.
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
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.DecimalComplexRestriction;
/// <summary>Verifies actual governing, atomic and authority identity for decimal complex restriction.</summary>
public sealed class NativeDecimalComplexRestrictionIdentityTests
{
 private const string Types="<xs:simpleType name='A'><xs:restriction base='xs:decimal'><xs:enumeration value='1.23'/><xs:enumeration value='2.34'/></xs:restriction></xs:simpleType><xs:complexType name='C'><xs:simpleContent><xs:extension base='A'/></xs:simpleContent></xs:complexType>";
 private const string Restriction="<xs:complexType name='R'><xs:simpleContent><xs:restriction base='C'><xs:enumeration value='1.2300'/></xs:restriction></xs:simpleContent></xs:complexType>";
 private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+Types+Restriction+"<xs:element name='r' type='R'/></xs:schema>";
 /// <summary>Governing complex and anonymous atomic restriction remain distinct with actual bases.</summary>
 [Fact]public void Components(){using var f=new Fixture(Schema,"<r>+001.23</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;var g=fact.NativeType;Assert.Equal("R",g.Name);Assert.True(g.IsComplexRestriction);Assert.Equal("C",g.DerivationBase!.Name);var atomic=g.ContentType!;Assert.Null(atomic.Name);Assert.NotSame(g,atomic);Assert.Equal("A",atomic.DerivationBase!.Name);Assert.Same(g.DerivationBase.ContentType,atomic.DerivationBase);Assert.True(atomic.Facets!.HasEnumeration);var value=f.Read(f.Input.Root).DecimalValue;Assert.Equal("1.23",value.ToInvariantString(256));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));Assert.Equal("1.23",value.ToInvariantString(256));}
 /// <summary>Anonymous governing and atomic components receive no invented global names.</summary>
 [Fact]public void Anonymous(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+Types+"<xs:element name='r'><xs:complexType><xs:simpleContent><xs:restriction base='C'><xs:enumeration value='1.23'/></xs:restriction></xs:simpleContent></xs:complexType></xs:element></xs:schema>";using var f=new Fixture(schema,"<r>1.23</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Null(fact.NativeType.Name);Assert.Null(fact.NativeType.ContentType!.Name);Assert.Equal("C",fact.NativeType.DerivationBase!.Name);Assert.Equal("A",fact.NativeType.ContentType.DerivationBase!.Name);}
 /// <summary>Failed local membership issues no successful facts or evidence.</summary>
 [Fact]public void InvalidFacts(){using var f=new Fixture(Schema,"<r>2.34</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);}
 /// <summary>Equal schemas in separate sessions never share authority or native component identity.</summary>
 [Fact]public void Authority(){using var a=new Fixture(Schema,"<r>1.23</r>");using var b=new Fixture(Schema,"<r>1.23</r>");var x=a.Input.FactsFor((NativeSessionNode)a.Input.Root)!;var y=b.Input.FactsFor((NativeSessionNode)b.Input.Root)!;Assert.NotSame(x.NativeType,y.NativeType);Assert.False(a.Provider.Owns(y.Type));Assert.Throws<ArgumentException>(()=>a.Provider.IsSameComponent(x.Type,y.Type,default));}
}
