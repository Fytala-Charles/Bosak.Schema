// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies actual shared model-group declarations and original node facts.
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
namespace Bosak.Schema.Tests.ModelGroups;
/// <summary>Verifies shared model-group declarations and actual original-node facts.</summary>
public sealed class NativeModelGroupsIdentityTests
{
 private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='one'><xs:complexType><xs:group ref='G'/></xs:complexType></xs:element><xs:element name='two'><xs:complexType><xs:group ref='G'/></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element><xs:group name='G'><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence></xs:group></xs:schema>";
 private const string Xml="<r><one><v>1.2300</v></one><two><v>2.34</v></two></r>";
 /// <summary>Two owners retain the exact same actual group particle and declaration object.</summary>
 [Fact]public void SharedParticles(){var c=NativeDecimalSchema.Compile(Schema,new(256,256),maxStringCharacters:1024).Value!;Assert.Same(c.Children[0].Container!.Children[0],c.Children[1].Container!.Children[0]);}
 /// <summary>Actual facts retain shared declaration handles and distinct original nodes and values.</summary>
 [Fact]public void CanonicalFacts(){using var f=new Fixture(Schema,Xml);var a=f.Input.FactsFor((NativeSessionNode)f.Element([0,0]))!;var b=f.Input.FactsFor((NativeSessionNode)f.Element([1,0]))!;Assert.NotSame(a.Node,b.Node);Assert.Same(a.NativeDeclaration,b.NativeDeclaration);Assert.Same(a.Declaration!,b.Declaration!);Assert.True(f.Provider.IsSameComponent(a.Declaration!,b.Declaration!,default).Value);Assert.Equal("1.23",f.Read(a.Node).DecimalValue.ToInvariantString(256));Assert.Equal("2.34",f.Read(b.Node).DecimalValue.ToInvariantString(256));}
 /// <summary>An anonymous group-local restriction remains one actual type definition across owners.</summary>
 [Fact]public void AnonymousType(){var schema=Schema.Replace("<xs:element name='v' type='xs:decimal'/>","<xs:element name='v'><xs:simpleType><xs:restriction base='xs:decimal'><xs:enumeration value='1.23'/></xs:restriction></xs:simpleType></xs:element>");using var f=new Fixture(schema,Xml.Replace("2.34","1.23"));var a=f.Input.FactsFor((NativeSessionNode)f.Element([0,0]))!;var b=f.Input.FactsFor((NativeSessionNode)f.Element([1,0]))!;Assert.Same(a.NativeDeclaration.Type,b.NativeDeclaration.Type);Assert.Null(a.NativeDeclaration.Type.Name);}
 /// <summary>A global element reference inside the group retains the global lookup declaration.</summary>
 [Fact]public void GlobalReference(){var schema=Schema.Replace("<xs:element name='v' type='xs:decimal'/>","<xs:element ref='v'/>").Replace("</xs:schema>","<xs:element name='v' type='xs:decimal'/></xs:schema>");using var f=new Fixture(schema,Xml);var a=f.Input.FactsFor((NativeSessionNode)f.Element([0,0]))!;var b=f.Input.FactsFor((NativeSessionNode)f.Element([1,0]))!;Assert.Same(a.Declaration,b.Declaration);Assert.Same(a.Declaration,f.Provider.ResolveElementDeclaration(new("","v"),default).Value);}
 /// <summary>Foreign group-derived handles are refused and facts expire with the original input.</summary>
 [Fact]public void AuthorityAndLifetime(){using var a=new Fixture(Schema,Xml);using var b=new Fixture(Schema,Xml);var x=a.Input.FactsFor((NativeSessionNode)a.Element([0,0]))!;var y=b.Input.FactsFor((NativeSessionNode)b.Element([0,0]))!;Assert.False(a.Provider.Owns(y.Declaration!));Assert.Throws<ArgumentException>(()=>a.Provider.IsSameComponent(x.Declaration!,y.Declaration!,default));var detached=a.Read(x.Node).DecimalValue;a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>x.ReadTypedValue(default));Assert.Equal("1.23",detached.ToInvariantString(256));}
 /// <summary>A failed group member assessment issues no successful facts or evidence.</summary>
 [Fact]public void InvalidHasNoFacts(){using var f=new Fixture(Schema,Xml.Replace("2.34","invalid"));Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);}
}
