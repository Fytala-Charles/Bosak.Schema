// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 28 November 2026
// PURPOSE              : Proves scoped opt-out attribute validity without publishing unsupported facts.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 28-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.DefaultOptOutAttributes;
/// <summary>Verifies scoped closed attributes with original source authority and lifetimes.</summary>
public sealed class NativeDefaultOptOutAttributesProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private const string G="<xs:attributeGroup name='G'><xs:attribute name='g' type='xs:boolean' use='required'/></xs:attributeGroup>";
    private static string S(string body)=>"<xs:schema xmlns:xs='"+X+"' defaultAttributes='G'>"+G+body+"</xs:schema>";
    private static string Root(string content="")=>"<xs:element name='a'><xs:complexType defaultAttributesApply='false'>"+content+"</xs:complexType></xs:element>";
    private static NativeSchemaSessionOptions Options=>new(256,256,1024,65536,2,256,16,256,1024);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Supported owners reject ordinary attributes before any facts are published.</summary>
    /// <param name="shape">Original source owner representation.</param>
    [Theory][InlineData("root")][InlineData("named")][InlineData("local")]
    public void AuthorityAndNoFacts(string shape)
    {
        var body=shape=="named"?"<xs:complexType name='T' defaultAttributesApply='0'/><xs:element name='a' type='T'/>":shape=="local"?Root("<xs:sequence><xs:element name='v'><xs:complexType defaultAttributesApply='false'/></xs:element></xs:sequence>"):Root();var xml=shape=="local"?"<a><v/></a>":"<a/>";var invalid=shape=="local"?"<a><v g='false'/></a>":"<a g='false'/>";
        using(var bad=new Fixture(S(body),invalid,atoms:16)){Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.Empty(bad.Assessment.Facts);Assert.False(bad.Input.IsAssessed);Assert.Null(bad.Assessment.Evidence);}using var f=new Fixture(S(body),xml,atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element(shape=="local"?[0]:[]))!;Assert.True(fact.NativeType.IsComplex);Assert.Equal(shape=="named"?"T":null,fact.NativeType.Name);Assert.True(f.Provider.Owns(fact.Type));Assert.False(fact.NativeDeclaration.IsAttribute);
    }
    /// <summary>Named root aliases retain one original type and the same closure.</summary>
    [Fact]public void NamedAliases()
    {var source=S("<xs:complexType name='T' defaultAttributesApply='false'/><xs:element name='a' type='T'/><xs:element name='b' type='T'/>");var c=NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;Assert.Same(c.Roots.First().Root.Type,c.Roots.Skip(1).First().Root.Type);foreach(var name in new[]{"a","b"}){Assert.Equal(NativeOutcome.Valid,c.Assess("<"+name+"/>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<"+name+" g='false'/>").Outcome);}}
    /// <summary>Global references preserve canonical declarations without projecting ignored uses.</summary>
    [Fact]public void GlobalReferences()
    {var source=S("<xs:complexType name='T' defaultAttributesApply='false'/><xs:element name='v' type='T'/>"+Root("<xs:sequence><xs:element ref='v'/></xs:sequence>"));using var good=new Fixture(source,"<a><v/></a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,good.Assessment.Status);var fact=good.Input.FactsFor((NativeSessionNode)good.Element([0]))!;Assert.Equal("v",fact.NativeDeclaration.CanonicalDeclaration.Name);using var bad=new Fixture(source,"<a><v g='false'/></a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.Empty(bad.Assessment.Facts);}
    /// <summary>Location hints stay original and are not source acquisition or attribute uses.</summary>
    [Fact]public void Hints()
    {var source=S(Root());const string xml="<a xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:noNamespaceSchemaLocation='https://example.invalid/never-fetch.xsd'/>";using var owner=NativeSchemaSession.Create(source,Options).Value!;using var doc=owner.Assess(xml,Context).Value!;Assert.Equal(0,doc.AttributeCount);Assert.Equal("https://example.invalid/never-fetch.xsd",System.Xml.Linq.XElement.Parse(doc.ExportXml()).Attribute(System.Xml.Linq.XName.Get("noNamespaceSchemaLocation","http://www.w3.org/2001/XMLSchema-instance"))!.Value);Assert.Equal(NativeSchemaOutcome.Unsupported,owner.Assess("<a xml:lang='en'/>",Context).Outcome);}
    /// <summary>Invalid recovery, cancellation, detached values, foreign handles and expiry retain their rules.</summary>
    [Fact]public void Lifetime()
    {var source=S(Root("<xs:sequence><xs:element name='v' type='xs:string'/></xs:sequence>"));using var owner=NativeSchemaSession.Create(source,Options).Value!;Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<a g='false'><v>text</v></a>",Context).Outcome);var doc=owner.Assess("<a><v>text</v></a>",Context).Value!;var detached=doc.ReadScalar(0);using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>doc.ExportXml(cancel.Token));doc.Dispose();Assert.Throws<ObjectDisposedException>(()=>doc.ReadScalar(0));Assert.Equal("text",detached.StringValue);using var recovered=owner.Assess("<a><v>again</v></a>",Context).Value!;Assert.Equal("again",recovered.ReadScalar(0).StringValue);using var f=new Fixture(source,"<a><v>text</v></a>",atoms:16);using var other=new Fixture(source,"<a><v>text</v></a>",atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Type,foreign.Type,default));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.Declaration);}
    /// <summary>Other profiles and the legacy decimal facade keep their boundaries.</summary>
    [Fact]public void OtherRoutes()
    {using var noDefault=NativeSchemaSession.Create(S(Root()).Replace(" defaultAttributes='G'",""),Options).Value!;Assert.Equal(NativeSchemaOutcome.Unsupported,noDefault.Assess("<a g='false'/>",Context).Outcome);var legacy=NativeDecimalSession.Create(S(Root()),new(256,256,1,32,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,legacy.Outcome);Assert.Null(legacy.Value);}
    /// <summary>Raw document and eager definition capacities still apply before publication.</summary>
    [Fact]public void Capacity()
    {using var owner=NativeSchemaSession.Create(S(Root()),Options).Value!;var refused=owner.Assess("<a g='"+new string('x',65537)+"'/>",Context);Assert.Equal(NativeSchemaOutcome.Unsupported,refused.Outcome);Assert.Null(refused.Value);using var recovered=owner.Assess("<a/>",Context).Value!;Assert.Equal(0,recovered.AttributeCount);Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(Root()).Replace("defaultAttributes='G'","defaultAttributes='Missing'"),Options).Outcome);}
}
