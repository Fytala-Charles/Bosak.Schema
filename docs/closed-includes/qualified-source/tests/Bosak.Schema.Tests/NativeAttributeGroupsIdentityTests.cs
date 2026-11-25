// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies attribute-group component reuse, set union and original facts.
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
namespace Bosak.Schema.Tests.AttributeGroups;
/// <summary>Verifies actual attribute-use reuse and distinct component conflicts.</summary>
public sealed class NativeAttributeGroupsIdentityTests
{
 private const string Group="<xs:attributeGroup name='G'><xs:attribute name='a' type='xs:decimal' use='required'/></xs:attributeGroup>";
 private const string Two="<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='one'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attributeGroup ref='G'/></xs:complexType></xs:element><xs:element name='two'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attributeGroup ref='G'/></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element>";
 private const string Xml="<r><one a='1.2300'><v>1</v></one><two a='2.34'><v>1</v></two></r>";
 private static string Schema(string group=Group)=>"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+group+Two+"</xs:schema>";
 /// <summary>The actual group use and its local declaration are reused across owners.</summary>
 [Fact]public void Uses(){var c=NativeDecimalSchema.Compile(Schema(),new(256,256),maxStringCharacters:1024).Value!;var a=c.Children[0].Container!.AttributeUses[0];var b=c.Children[1].Container!.AttributeUses[0];Assert.Same(a,b);Assert.Same(a,a.CanonicalDeclaration);Assert.Equal(1,a.MinOccurs);Assert.Empty(c.GlobalAttributeDeclarations);}
 /// <summary>Distinct original nodes retain a shared actual group-local declaration.</summary>
 [Fact]public void Facts(){using var f=new Fixture(Schema(),Xml);var a=f.Input.FactsFor((NativeSessionNode)f.Attribute([0],"","a"))!;var b=f.Input.FactsFor((NativeSessionNode)f.Attribute([1],"","a"))!;Assert.NotSame(a.Node,b.Node);Assert.Same(a.NativeDeclaration,b.NativeDeclaration);Assert.Same(a.Declaration,b.Declaration);Assert.Equal("1.23",f.Read(a.Node).DecimalValue.ToInvariantString(256));Assert.Equal("2.34",f.Read(b.Node).DecimalValue.ToInvariantString(256));Assert.Equal(SchemaResultKind.Unsupported,f.Provider.ResolveAttributeDeclaration(new("","a"),default).Kind);}
 /// <summary>Global references inside groups retain the actual global declaration handle.</summary>
 [Fact]public void Global(){var group="<xs:attribute name='a' type='xs:decimal'/><xs:attributeGroup name='G'><xs:attribute ref='a' use='required'/></xs:attributeGroup>";using var f=new Fixture(Schema(group),Xml);var a=f.Input.FactsFor((NativeSessionNode)f.Attribute([0],"","a"))!;var b=f.Input.FactsFor((NativeSessionNode)f.Attribute([1],"","a"))!;Assert.Same(a.Declaration,b.Declaration);Assert.Same(a.Declaration,f.Provider.ResolveAttributeDeclaration(new("","a"),default).Value);}
 /// <summary>Repeated references to one group do not duplicate attribute facts.</summary>
 [Fact]public void Repeated(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+Group+"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attributeGroup ref='G'/><xs:attributeGroup ref='G'/></xs:complexType></xs:element></xs:schema>";using var f=new Fixture(schema,"<r a='1.23'><v>1</v></r>");Assert.Single(f.Assessment.AttributeFacts);Assert.Equal("1.23",f.Read(f.Attribute([],"","a")).DecimalValue.ToInvariantString(256));}
 /// <summary>Different use components remain conflicting even when their global declaration is shared.</summary>
 [Fact]public void DistinctConflict(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:attribute name='a' type='xs:decimal'/><xs:attributeGroup name='G'><xs:attribute ref='a'/></xs:attributeGroup><xs:attributeGroup name='H'><xs:attribute ref='a'/></xs:attributeGroup><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attributeGroup ref='G'/><xs:attributeGroup ref='H'/></xs:complexType></xs:element></xs:schema>";var c=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:1024);Assert.Equal(NativeOutcome.Invalid,c.Outcome);Assert.Null(c.Value);}
 /// <summary>XSD1.1 circularity is outside this profile, never Invalid solely for being circular.</summary>
 [Fact]public void CyclesUnsupported(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:attributeGroup name='G'><xs:attribute name='a' type='xs:decimal'/><xs:attributeGroup ref='G'/></xs:attributeGroup><xs:element name='r' type='xs:decimal'/></xs:schema>";var c=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:1024);Assert.Equal(NativeOutcome.Unsupported,c.Outcome);Assert.Null(c.Value);}
}
