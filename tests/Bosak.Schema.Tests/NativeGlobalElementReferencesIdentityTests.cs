// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies canonical global attribute declarations, uses and original origins.
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
//                      | Charles Korthout | 0.21  | 26-11-2026     | Known-declaration controls retain explicit nullable preconditions.                        |
// ===========================================================================================================================================================
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.GlobalElementReferences;
/// <summary>Verifies referenced global declaration identity and original assessment facts.</summary>
public sealed class NativeGlobalElementReferencesIdentityTests
{
 private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element ref='item' minOccurs='0' maxOccurs='2'/></xs:sequence></xs:complexType></xs:element><xs:element name='item' type='xs:decimal'/></xs:schema>";
 private const string Xml="<r><item>1.2300</item><item>2.34</item></r>";
 /// <summary>Particle bounds remain separate from the canonical global declaration.</summary>
 [Fact]public void Uses(){var c=NativeDecimalSchema.Compile(Schema,new(256,256),maxStringCharacters:1024).Value!;Assert.Equal("r",c.Root.Name);var use=Assert.Single(c.Children);Assert.Equal(0,use.MinOccurs);Assert.Equal(2,use.MaxOccurs);Assert.NotSame(use,use.CanonicalDeclaration);Assert.Equal(1,use.CanonicalDeclaration.MinOccurs);Assert.Equal(1,use.CanonicalDeclaration.MaxOccurs);}
 /// <summary>Repeated original nodes share the actual declaration and preserve separate values.</summary>
 [Fact]public void CanonicalFacts(){using var f=new Fixture(Schema,Xml);var a=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;var b=f.Input.FactsFor((NativeSessionNode)f.Element([1]))!;Assert.NotSame(a.Node,b.Node);Assert.Same(a.NativeDeclaration,b.NativeDeclaration);Assert.Same(a.Declaration!,b.Declaration!);Assert.Same(a.Declaration!,f.Provider.ResolveElementDeclaration(new("","item"),default).Value);Assert.True(f.Provider.IsSameComponent(a.Declaration!,b.Declaration!,default).Value);Assert.Equal("1.23",f.Read(a.Node).DecimalValue.ToInvariantString(256));Assert.Equal("2.34",f.Read(b.Node).DecimalValue.ToInvariantString(256));}
 /// <summary>Local and unused global declarations with the same name remain distinct components.</summary>
 [Fact]public void LocalIsDistinct(){using var f=new Fixture(Schema.Replace("ref='item' minOccurs='0' maxOccurs='2'","name='item' type='xs:decimal'"),"<r><item>1</item></r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;Assert.NotSame(fact.Declaration,f.Provider.ResolveElementDeclaration(new("","item"),default).Value);}
 /// <summary>A complex simple-content reference keeps its actual global and governing complex type.</summary>
 [Fact]public void ComplexIdentity(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:complexType name='C'><xs:simpleContent><xs:extension base='xs:decimal'/></xs:simpleContent></xs:complexType><xs:element name='r'><xs:complexType><xs:sequence><xs:element ref='item'/></xs:sequence></xs:complexType></xs:element><xs:element name='item' type='C'/></xs:schema>";using var f=new Fixture(schema,"<r><item>1.23</item></r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;Assert.Same(fact.Declaration,f.Provider.ResolveElementDeclaration(new("","item"),default).Value);Assert.True(fact.NativeDeclaration.Type.IsComplex);Assert.Equal("1.23",f.Read(fact.Node).DecimalValue.ToInvariantString(256));}
 /// <summary>Foreign handles are refused and issued facts expire with their source input.</summary>
 [Fact]public void AuthorityAndLifetime(){using var a=new Fixture(Schema,Xml);using var b=new Fixture(Schema,Xml);var x=a.Provider.ResolveElementDeclaration(new("","item"),default).Value;var y=b.Provider.ResolveElementDeclaration(new("","item"),default).Value;Assert.False(a.Provider.Owns(y));Assert.Throws<ArgumentException>(()=>a.Provider.IsSameComponent(x,y,default));var fact=a.Input.FactsFor((NativeSessionNode)a.Element([0]))!;var detached=a.Read(fact.Node).DecimalValue;a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));Assert.Equal("1.23",detached.ToInvariantString(256));}
 /// <summary>Referenced facet failure issues no successful typed facts or evidence.</summary>
 [Fact]public void InvalidHasNoFacts(){var schema=Schema.Replace("type='xs:decimal'/>","type='D'/>").Replace("<xs:element name='r'>","<xs:simpleType name='D'><xs:restriction base='xs:decimal'><xs:enumeration value='1.23'/></xs:restriction></xs:simpleType><xs:element name='r'>");using var f=new Fixture(schema,"<r><item>1.24</item></r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);}
}
