// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies assembled include component identity, ownership and refusal boundaries.
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
namespace Bosak.Schema.Tests.ClosedIncludes;
/// <summary>Verifies assembled declarations through actual native facts.</summary>
public sealed class NativeClosedIncludesIdentityTests
{
 private const string Root="https://catalog.test/root.xsd";
 private const string Main="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:include schemaLocation='types.xsd'/><xs:element name='r'><xs:complexType><xs:sequence><xs:element ref='item' maxOccurs='2'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
 private const string Types="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='item' type='xs:decimal'/></xs:schema>";
 private const string Xml="<r><item>1.2300</item><item>2.34</item></r>";
 private static Dictionary<string,string> Catalog()=>new(){{Root,Main},{"https://catalog.test/types.xsd",Types}};
 private static string Assembly(){var result=NativeDecimalSchema.AssembleIncludes(Root,Catalog(),default);Assert.Equal(NativeOutcome.Valid,result.Outcome);return result.Value!;}
 /// <summary>Included global declarations retain canonical lookup and original node values.</summary>
 [Fact]public void CanonicalFacts(){using var f=new Fixture(Assembly(),Xml);var a=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;var b=f.Input.FactsFor((NativeSessionNode)f.Element([1]))!;Assert.NotSame(a.Node,b.Node);Assert.Same(a.NativeDeclaration,b.NativeDeclaration);Assert.Same(a.Declaration,b.Declaration);Assert.Same(a.Declaration,f.Provider.ResolveElementDeclaration(new("","item"),default).Value);Assert.Equal("1.23",f.Read(a.Node).DecimalValue.ToInvariantString(256));Assert.Equal("2.34",f.Read(b.Node).DecimalValue.ToInvariantString(256));}
 /// <summary>Included group-local declarations remain shared across distinct owners.</summary>
 [Fact]public void GroupIdentity(){var catalog=Catalog();catalog[Root]=Main.Replace("<xs:element ref='item' maxOccurs='2'/>","<xs:element name='one'><xs:complexType><xs:group ref='G'/></xs:complexType></xs:element><xs:element name='two'><xs:complexType><xs:group ref='G'/></xs:complexType></xs:element>");catalog["https://catalog.test/types.xsd"]=Types.Replace("<xs:element name='item' type='xs:decimal'/>","<xs:group name='G'><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence></xs:group>");var assembled=NativeDecimalSchema.AssembleIncludes(Root,catalog,default).Value!;using var f=new Fixture(assembled,"<r><one><v>1.23</v></one><two><v>2.34</v></two></r>");Assert.Same(f.Input.FactsFor((NativeSessionNode)f.Element([0,0]))!.Declaration,f.Input.FactsFor((NativeSessionNode)f.Element([1,0]))!.Declaration);}
 /// <summary>Assembled authorities remain independent and facts expire with their source input.</summary>
 [Fact]public void AuthorityAndLifetime(){var schema=Assembly();using var a=new Fixture(schema,Xml);using var b=new Fixture(schema,Xml);var x=a.Input.FactsFor((NativeSessionNode)a.Element([0]))!;var y=b.Input.FactsFor((NativeSessionNode)b.Element([0]))!;Assert.False(a.Provider.Owns(y.Declaration));Assert.Throws<ArgumentException>(()=>a.Provider.IsSameComponent(x.Declaration,y.Declaration,default));var detached=a.Read(x.Node).DecimalValue;a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>x.ReadTypedValue(default));Assert.Equal("1.23",detached.ToInvariantString(256));}
 /// <summary>An invalid assembled instance issues no successful facts or evidence.</summary>
 [Fact]public void InvalidHasNoFacts(){using var f=new Fixture(Assembly(),"<r><item>invalid</item></r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);}
 /// <summary>Missing closed sources return Unsupported without an assembled schema.</summary>
 [Fact]public void MissingHasNoAssembly(){var result=NativeDecimalSchema.AssembleIncludes(Root,new Dictionary<string,string>{{Root,Main}},default);Assert.Equal(NativeOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);}
 /// <summary>Invalid Unicode inventory text is caught before schema parsing.</summary>
 [Fact]public void InvalidUnicode(){var catalog=Catalog();catalog["https://catalog.test/unused.xsd"]=new string((char)0xD800,1);var result=NativeDecimalSchema.AssembleIncludes(Root,catalog,default);Assert.Equal(NativeOutcome.Invalid,result.Outcome);Assert.Null(result.Value);}
}
