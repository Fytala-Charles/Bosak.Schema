// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 28 November 2026
// PURPOSE              : Proves original default-group use identity, effective budgets and owned lifetimes.
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
//                      | Charles Korthout | 0.2 | 28-11-2026 | Reconcile scoped opt-out validity; preserve original inputs and names. |
// ===========================================================================================================================================================
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.DefaultAttributeGroups;
/// <summary>Verifies original default attribute uses without instance augmentation.</summary>
public sealed class NativeDefaultAttributeGroupsProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private const string Group="<xs:attributeGroup name='G'><xs:attribute name='g' type='xs:boolean' use='required'/></xs:attributeGroup>";
    private static string S(string body,string group=Group)=>"<xs:schema xmlns:xs='"+X+"' defaultAttributes='G'>"+group+body+"</xs:schema>";
    private static string Root(string flag="",string extra="")=>"<xs:element name='a'><xs:complexType"+flag+">"+extra+"</xs:complexType></xs:element>";
    private static string Two=>Root(" defaultAttributesApply='false'","<xs:sequence><xs:element name='v'><xs:complexType/></xs:element><xs:element name='w'><xs:complexType/></xs:element></xs:sequence>");
    private static NativeSchemaSessionOptions Options=>new(256,256,1024,65536,2,256,16,256,1024);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private static NativeDecimalSchema Compile(string source)=>NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;
    /// <summary>Actual defaulted attribute uses retain Boolean values and original provenance.</summary>
    /// <param name="shape">Owner representation.</param><param name="value">Original Boolean value.</param>
    [Theory][InlineData("root",false)][InlineData("root",true)][InlineData("named",false)][InlineData("named",true)][InlineData("local",false)][InlineData("local",true)]
    public void Authority(string shape,bool value)
    {
        var body=shape=="named"?"<xs:complexType name='T'/><xs:element name='a' type='T'/>":shape=="local"?Root(" defaultAttributesApply='false'","<xs:sequence><xs:element name='v'><xs:complexType/></xs:element></xs:sequence>"):Root();var flag=value?"true":"false";var xml=shape=="local"?"<a><v g='"+flag+"'/></a>":"<a g='"+flag+"'/>";using var f=new Fixture(S(body),xml,atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var node=f.Attribute(shape=="local"?[0]:[],"","g");var fact=f.Input.FactsFor((NativeSessionNode)node)!;Assert.Same(fact.NativeDeclaration,fact.NativeDeclaration.CanonicalDeclaration);Assert.True(fact.NativeDeclaration.Type.IsBoolean);Assert.Equal(1,fact.NativeDeclaration.MinOccurs);Assert.Equal(value,f.Read(node).BooleanValue);var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);var atom=Assert.Single(typed.AtomicItems);Assert.Equal(XdmValueKind.Boolean,atom.Value.Kind);Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);
    }
    /// <summary>Two original owners and nodes share the actual group-local declaration.</summary>
    [Fact]public void Sharing()
    {var source=S(Two);var c=Compile(source);Assert.Empty(c.AttributeUses);Assert.Same(c.Children[0].Container!.AttributeUses[0],c.Children[1].Container!.AttributeUses[0]);Assert.Empty(c.GlobalAttributeDeclarations);using var f=new Fixture(source,"<a><v g='false'/><w g='true'/></a>",atoms:16);var a=f.Input.FactsFor((NativeSessionNode)f.Attribute([0],"","g"))!;var b=f.Input.FactsFor((NativeSessionNode)f.Attribute([1],"","g"))!;Assert.NotSame(a.Node,b.Node);Assert.Same(a.NativeDeclaration,b.NativeDeclaration);Assert.Same(a.Declaration,b.Declaration);Assert.False(f.Read(a.Node).BooleanValue);Assert.True(f.Read(b.Node).BooleanValue);}
    /// <summary>Defaulted global references retain their canonical global source declaration.</summary>
    [Fact]public void GlobalReference()
    {var group="<xs:attribute name='g' type='xs:boolean'/><xs:attributeGroup name='G'><xs:attribute ref='g' use='required'/></xs:attributeGroup>";var c=Compile(S(Two,group));var use=c.Children[0].Container!.AttributeUses[0];Assert.Same(use,c.Children[1].Container!.AttributeUses[0]);Assert.NotSame(use,use.CanonicalDeclaration);Assert.Same(Assert.Single(c.GlobalAttributeDeclarations),use.CanonicalDeclaration);}
    /// <summary>Set union deduplicates original shared uses and rejects distinct same-name declarations.</summary>
    [Fact]public void SetUnion()
    {var c=Compile(S(Root(extra:"<xs:attributeGroup ref='G'/><xs:attributeGroup ref='G'/>")));Assert.Single(c.AttributeUses);Assert.Equal(NativeSchemaOutcome.Invalid,NativeSchemaSession.Create(S(Root(extra:"<xs:attribute name='g' type='xs:boolean'/>")),Options).Outcome);}
    /// <summary>Schema QName and attribute-form bindings are independent of local prefix shadowing.</summary>
    [Fact]public void NamespaceAuthority()
    {var source=S(Root(extra:"")).Replace("defaultAttributes='G'","targetNamespace='urn:s' xmlns:t='urn:s' attributeFormDefault='qualified' defaultAttributes='t:G'").Replace("<xs:complexType>","<xs:complexType xmlns:t='urn:shadow'>");using var f=new Fixture(source,"<t:a xmlns:t='urn:s' t:g='false'/>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"urn:s","g"))!;Assert.Equal("urn:s",fact.NativeDeclaration.NamespaceUri);Assert.False(f.Read(fact.Node).BooleanValue);}
    /// <summary>Missing required uses publish no facts; scoped zero-use opt-outs reject ordinary extra attributes.</summary>
    [Fact]public void NoFactsAndOptOut()
    {using var bad=new Fixture(S(Root()),"<a/>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.Empty(bad.Assessment.Facts);Assert.False(bad.Input.IsAssessed);using var owner=NativeSchemaSession.Create(S(Root(" defaultAttributesApply='false'")),Options).Value!;Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<a g='false'/>",Context).Outcome);using var valid=owner.Assess("<a/>",Context).Value!;Assert.Equal(0,valid.AttributeCount);}
    /// <summary>Recovery, cancellation, detached values, foreign ownership and expiry retain original rules.</summary>
    [Fact]public void Lifetime()
    {var source=S(Root());using var owner=NativeSchemaSession.Create(source,Options).Value!;Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<a/>",Context).Outcome);var doc=owner.Assess("<a g='false'/>",Context).Value!;var detached=doc.ReadAttribute(0).Value;using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>doc.ReadAttribute(0,cancel.Token));doc.Dispose();Assert.Throws<ObjectDisposedException>(()=>doc.ReadAttribute(0));Assert.False(detached.BooleanValue);using var recovered=owner.Assess("<a g='true'/>",Context).Value!;Assert.True(recovered.ReadAttribute(0).Value.BooleanValue);using var f=new Fixture(source,"<a g='false'/>",atoms:16);using var other=new Fixture(source,"<a g='false'/>",atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","g"))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Attribute([],"","g"))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Declaration!,foreign.Declaration!,default));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));}
    /// <summary>Explicit and default uses share one effective sixteen-use ceiling.</summary>
    [Fact]public void EffectiveCapacity()
    {var group="<xs:attributeGroup name='G'>"+string.Concat(Enumerable.Range(0,16).Select(i=>"<xs:attribute name='g"+i+"' type='xs:boolean'/>"))+"</xs:attributeGroup>";Assert.Equal(16,Compile(S(Root(),group)).AttributeUses.Length);Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(Root(extra:"<xs:attribute name='extra' type='xs:boolean'/>"),group),Options).Outcome);}
    /// <summary>Repeated nested owners count effective default uses before aggregate admission.</summary>
    [Fact]public void AggregateCapacity()
    {var group="<xs:attributeGroup name='G'>"+string.Concat(Enumerable.Range(0,16).Select(i=>"<xs:attribute name='g"+i+"' type='xs:boolean'/>"))+"</xs:attributeGroup>";var body=Root(" defaultAttributesApply='false'","<xs:sequence><xs:element name='v' maxOccurs='4'><xs:complexType/></xs:element></xs:sequence>");Assert.NotNull(Compile(S(body,group)));Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(body.Replace("defaultAttributesApply='false'","defaultAttributesApply='true'"),group),Options).Outcome);Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(body.Replace("maxOccurs='4'","maxOccurs='5'"),group),Options).Outcome);}
    /// <summary>Without a schema default group, a valid Boolean flag has no use effect.</summary>
    [Fact]public void FlagWithoutDefault()
    {foreach(var flag in new[]{"true","false","1","0"}){var source=S(Root(" defaultAttributesApply='"+flag+"'")).Replace(" defaultAttributes='G'","");Assert.Empty(Compile(source).AttributeUses);}Assert.Equal(NativeSchemaOutcome.Invalid,NativeSchemaSession.Create(S(Root(" defaultAttributesApply='yes'")),Options).Outcome);}
    /// <summary>Unresolved defaults, unused conflicts and cycles are checked eagerly.</summary>
    [Fact]public void EagerDefinitions()
    {Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(Root(" defaultAttributesApply='false'")).Replace("defaultAttributes='G'","defaultAttributes='Missing'"),Options).Outcome);Assert.Equal(NativeSchemaOutcome.Invalid,NativeSchemaSession.Create(S(Root(" defaultAttributesApply='false'")+"<xs:complexType name='Unused'><xs:attribute name='g' type='xs:boolean'/></xs:complexType>"),Options).Outcome);Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(Root(),"<xs:attributeGroup name='G'><xs:attributeGroup ref='G'/></xs:attributeGroup>"),Options).Outcome);}
    /// <summary>Missing value constraints require augmentation, while provided values and XML remain original.</summary>
    [Fact]public void NoAugmentation()
    {var source=S(Root(),"<xs:attributeGroup name='G'><xs:attribute name='g' type='xs:boolean' default='false'/></xs:attributeGroup>");using var owner=NativeSchemaSession.Create(source,Options).Value!;var absent=owner.Assess("<a/>",Context);Assert.Equal(NativeSchemaOutcome.Unsupported,absent.Outcome);Assert.Null(absent.Value);using var doc=owner.Assess("<a g='0'/>",Context).Value!;Assert.False(doc.ReadAttribute(0).Value.BooleanValue);Assert.Equal("0",System.Xml.Linq.XElement.Parse(doc.ExportXml()).Attribute("g")!.Value);}
    /// <summary>Active defaults retain the first owner-closure exclusions; explicit false preserves earlier content.</summary>
    [Fact]public void OwnerScope()
    {var body=Root(extra:"<xs:simpleContent><xs:extension base='xs:string'/></xs:simpleContent>");Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(body),Options).Outcome);using var owner=NativeSchemaSession.Create(S(body.Replace("<xs:complexType>","<xs:complexType defaultAttributesApply='false'>")),Options).Value!;using var doc=owner.Assess("<a>text</a>",Context).Value!;Assert.Equal("text",doc.ReadRootScalar().StringValue);}
    /// <summary>The legacy decimal-only facade does not acquire default-group support.</summary>
    [Fact]public void LegacyBoundary()
    {var attempt=NativeDecimalSession.Create(S(Root()),new(256,256,1,32,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,attempt.Outcome);Assert.Null(attempt.Value);}
}
