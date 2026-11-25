// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies original attribute facts, effective constraints and fixed override identity.
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
namespace Bosak.Schema.Tests.AttributeValueConstraints;
/// <summary>Verifies effective constraints on actual original attribute facts.</summary>
public sealed class NativeAttributeValueConstraintsIdentityTests
{
 private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attribute name='a' type='xs:decimal' default='1.23'/></xs:complexType></xs:element></xs:schema>";
 private const string Xml="<r a='2.3400'><v>1</v></r>";
 /// <summary>Default metadata preserves the provided original attribute and its different value.</summary>
 [Fact]public void DefaultUsesProvided(){using var f=new Fixture(Schema,Xml);var node=f.Attribute([],"","a");var fact=f.Input.FactsFor((NativeSessionNode)node)!;Assert.Equal("a",fact.NativeDeclaration.Name);Assert.Equal("2.3400",node.StringValue);Assert.Equal("2.34",f.Read(node).DecimalValue.ToInvariantString(256));Assert.False(fact.IsNilled);}
 /// <summary>A use-local fixed over a global default preserves the actual global declaration handle.</summary>
 [Fact]public void CanonicalGlobal(){var schema=Schema.Replace("<xs:element name='r'>","<xs:attribute name='a' type='xs:decimal' default='1.23'/><xs:element name='r'>").Replace("<xs:attribute name='a' type='xs:decimal' default='1.23'/></xs:complexType>","<xs:attribute ref='a' fixed='2.34'/></xs:complexType>");using var f=new Fixture(schema,Xml);var fact=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","a"))!;Assert.Same(fact.Declaration,f.Provider.ResolveAttributeDeclaration(new("","a"),default).Value);Assert.Equal("2.34",f.Read(fact.Node).DecimalValue.ToInvariantString(256));}
 /// <summary>Cached group-local fixed declarations stay shared across owners without duplicating originals.</summary>
 [Fact]public void GroupIdentity(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:attributeGroup name='A'><xs:attribute name='a' type='xs:decimal' fixed='1.23'/></xs:attributeGroup><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='one'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attributeGroup ref='A'/></xs:complexType></xs:element><xs:element name='two'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attributeGroup ref='A'/></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:schema>";using var f=new Fixture(schema,"<r><one a='1.23'><v>1</v></one><two a='+001.2300'><v>1</v></two></r>");var a=f.Input.FactsFor((NativeSessionNode)f.Attribute([0],"","a"))!;var b=f.Input.FactsFor((NativeSessionNode)f.Attribute([1],"","a"))!;Assert.NotSame(a.Node,b.Node);Assert.Same(a.Declaration,b.Declaration);Assert.Equal("1.23",f.Read(b.Node).DecimalValue.ToInvariantString(256));}
 /// <summary>Date fixed instance equality differs from the stricter global-fixed override identity.</summary>
 [Fact]public void DateIdentity(){var schema=Schema.Replace("<xs:element name='r'>","<xs:attribute name='a' type='xs:date' fixed='2000-01-02+14:00'/><xs:element name='r'>").Replace("<xs:attribute name='a' type='xs:decimal' default='1.23'/></xs:complexType>","<xs:attribute ref='a'/></xs:complexType>");using var f=new Fixture(schema,"<r a='2000-01-01-10:00'><v>1</v></r>");Assert.Equal(1,f.Read(f.Attribute([],"","a")).DateValue.Day);var changed=schema.Replace("<xs:attribute ref='a'/>","<xs:attribute ref='a' fixed='2000-01-01-10:00'/>");Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(changed,new(256,256),maxStringCharacters:1024).Outcome);}
 /// <summary>A missing defaulted attribute never publishes earlier successful sibling attribute facts.</summary>
 [Fact]public void MissingHasNoFacts(){var schema=Schema.Replace("<xs:attribute name='a'", "<xs:attribute name='b' type='xs:decimal'/><xs:attribute name='a'");using var f=new Fixture(schema,"<r b='2'><v>1</v></r>");Assert.Equal(SchemaAssessmentStatus.Unsupported,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);Assert.False(f.Input.IsAssessed);Assert.Null(f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","b")));}
 /// <summary>Constraint-bearing attributes retain actual authority and issued lifetime.</summary>
 [Fact]public void AuthorityAndLifetime(){using var a=new Fixture(Schema,Xml);using var b=new Fixture(Schema,Xml);var x=a.Input.FactsFor((NativeSessionNode)a.Attribute([],"","a"))!;var y=b.Input.FactsFor((NativeSessionNode)b.Attribute([],"","a"))!;Assert.False(a.Provider.Owns(y.Declaration));Assert.Throws<ArgumentException>(()=>a.Provider.IsSameComponent(x.Declaration,y.Declaration,default));var detached=a.Read(x.Node).DecimalValue;a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>x.ReadTypedValue(default));Assert.Equal("2.34",detached.ToInvariantString(256));}
 /// <summary>The legacy decimal profile still refuses attribute metadata on otherwise ordinary owners.</summary>
 [Fact]public void Legacy(){var result=NativeDecimalSession.Create(Schema,new(256,256,1,4,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);var control=Schema.Replace("<xs:attribute name='a' type='xs:decimal' default='1.23'/>","");using var valid=NativeDecimalSession.Create(control,new(256,256,1,4,1,256,256)).Value!;}
}
