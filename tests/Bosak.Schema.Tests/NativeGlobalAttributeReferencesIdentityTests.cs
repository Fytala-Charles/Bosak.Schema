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
namespace Bosak.Schema.Tests.GlobalAttributeReferences;
/// <summary>Verifies global declaration identity separately from per-owner attribute uses.</summary>
public sealed class NativeGlobalAttributeReferencesIdentityTests
{
 private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:attribute name='a' type='xs:decimal'/><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='one'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attribute ref='a' use='required'/></xs:complexType></xs:element><xs:element name='two'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attribute ref='a'/></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:schema>";
 private const string Xml="<r><one a='1.2300'><v>1</v></one><two a='2.34'><v>1</v></two></r>";
 /// <summary>Required and optional wrappers retain separate constraints and one canonical declaration.</summary>
 [Fact]public void Uses(){var c=NativeDecimalSchema.Compile(Schema,new(256,256),maxStringCharacters:1024).Value!;Assert.Single(c.GlobalAttributeDeclarations);var a=c.Children[0].Container!.AttributeUses[0];var b=c.Children[1].Container!.AttributeUses[0];Assert.Equal(1,a.MinOccurs);Assert.Equal(0,b.MinOccurs);Assert.NotSame(a,b);Assert.Same(a.CanonicalDeclaration,b.CanonicalDeclaration);Assert.Same(c.GlobalAttributeDeclarations[0],a.CanonicalDeclaration);}
 /// <summary>Distinct original nodes share only the actual global declaration handle.</summary>
 [Fact]public void CanonicalFacts(){using var f=new Fixture(Schema,Xml);var a=f.Input.FactsFor((NativeSessionNode)f.Attribute([0],"","a"))!;var b=f.Input.FactsFor((NativeSessionNode)f.Attribute([1],"","a"))!;Assert.NotSame(a.Node,b.Node);Assert.Same(a.NativeDeclaration,b.NativeDeclaration);Assert.Same(a.Declaration!,b.Declaration!);Assert.Same(a.Declaration!,f.Provider.ResolveAttributeDeclaration(new("","a"),default).Value);Assert.True(f.Provider.IsSameComponent(a.Declaration!,b.Declaration!,default).Value);Assert.Equal("1.23",f.Read(a.Node).DecimalValue.ToInvariantString(256));Assert.Equal("2.34",f.Read(b.Node).DecimalValue.ToInvariantString(256));}
 /// <summary>A local declaration remains distinct even when its expanded name matches an unused global.</summary>
 [Fact]public void LocalIsDistinct(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:attribute name='a' type='xs:decimal'/><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attribute name='a' type='xs:decimal'/></xs:complexType></xs:element></xs:schema>";using var f=new Fixture(schema,"<r a='1'><v>1</v></r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","a"))!;Assert.NotSame(fact.Declaration,f.Provider.ResolveAttributeDeclaration(new("","a"),default).Value);}
 /// <summary>Unused globals resolve and a missing closed name is NotFound.</summary>
 [Fact]public void Lookup(){using var f=new Fixture(Schema,Xml);Assert.Equal(SchemaResultKind.Supported,f.Provider.ResolveAttributeDeclaration(new("","a"),default).Kind);Assert.Equal(SchemaResultKind.NotFound,f.Provider.ResolveAttributeDeclaration(new("","missing"),default).Kind);}
 /// <summary>Foreign authorities are refused and issued facts expire with their input.</summary>
 [Fact]public void AuthorityAndLifetime(){using var a=new Fixture(Schema,Xml);using var b=new Fixture(Schema,Xml);var x=a.Provider.ResolveAttributeDeclaration(new("","a"),default).Value;var y=b.Provider.ResolveAttributeDeclaration(new("","a"),default).Value;Assert.False(a.Provider.Owns(y));Assert.Throws<ArgumentException>(()=>a.Provider.IsSameComponent(x,y,default));var fact=a.Input.FactsFor((NativeSessionNode)a.Attribute([0],"","a"))!;var detached=a.Read(fact.Node).DecimalValue;a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));Assert.Equal("1.23",detached.ToInvariantString(256));}
 /// <summary>Concurrent compilations with the same QName keep their distinct scalar definitions.</summary><returns>Completion.</returns>
 [Fact]public async Task ContextIsolation(){await Task.WhenAll(Enumerable.Range(1,16).Select(i=>Task.Run(()=>{var text=i.ToString(System.Globalization.CultureInfo.InvariantCulture);var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='D'><xs:restriction base='xs:decimal'><xs:enumeration value='"+text+"'/></xs:restriction></xs:simpleType><xs:attribute name='a' type='D'/><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attribute ref='a' use='required'/></xs:complexType></xs:element></xs:schema>";using var session=NativeSchemaSession.Create(schema,new(256,256,1024,65536,1,32,1,256,256)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);using var document=session.Assess("<r a='"+text+"'><v>1</v></r>",context).Value!;Assert.Equal(text,document.ReadAttribute(0).Value.DecimalValue.ToInvariantString(256));})));}
}
