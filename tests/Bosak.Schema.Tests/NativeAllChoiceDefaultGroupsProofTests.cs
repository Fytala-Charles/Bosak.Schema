// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 28 November 2026
// PURPOSE              : Proves original all/choice plans with default-group facts, budgets and lifetimes.
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
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.AllChoiceDefaultGroups;
/// <summary>Verifies original all/choice types and default-group uses without model changes.</summary>
public sealed class NativeAllChoiceDefaultGroupsProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private const string G="<xs:attributeGroup name='G'><xs:attribute name='g' type='xs:boolean' use='required'/></xs:attributeGroup>";
    private static string S(string tag,bool named=false,string group=G,string tail="")=>"<xs:schema xmlns:xs='"+X+"' defaultAttributes='G'>"+group+(named?"<xs:complexType name='T'>":"<xs:element name='a'><xs:complexType>")+"<xs:"+tag+"><xs:element name='v' type='xs:string'/></xs:"+tag+">"+tail+"</xs:complexType>"+(named?"<xs:element name='a' type='T'/><xs:element name='b' type='T'/>":"</xs:element>")+"</xs:schema>";
    private static string Xml=>"<a g='false'><v>text</v></a>";
    private static NativeSchemaSessionOptions Options=>new(256,256,1024,65536,2,256,16,256,1024);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private static NativeDecimalSchema Compile(string source)=>NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;
    /// <summary>Actual owner and attribute facts retain original declared types and assessment origin.</summary>
    /// <param name="tag">Compositor.</param><param name="named">Owner naming selector.</param>
    [Theory][InlineData("all",false)][InlineData("all",true)][InlineData("choice",false)][InlineData("choice",true)]
    public void Authority(string tag,bool named)
    {using var f=new Fixture(S(tag,named),Xml,atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var root=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.True(root.NativeType.IsComplex);Assert.Equal(named?"T":null,root.NativeType.Name);var fact=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","g"))!;Assert.Same(fact.NativeDeclaration,fact.NativeDeclaration.CanonicalDeclaration);Assert.True(fact.NativeType.IsBoolean);Assert.False(f.Read(fact.Node).BooleanValue);var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);var atom=Assert.Single(typed.AtomicItems);Assert.Equal(XdmValueKind.Boolean,atom.Value.Kind);Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);Assert.Equal("text",f.Read(f.Element([0])).StringValue);}
    /// <summary>Named aliases and explicit/default group references share actual original uses.</summary>
    /// <param name="tag">Compositor.</param>
    [Theory][InlineData("all")][InlineData("choice")]
    public void GroupIdentity(string tag)
    {var c=Compile(S(tag,true,tail:"<xs:attributeGroup ref='G'/>"));var a=c.Roots.First();var b=c.Roots.Skip(1).First();Assert.Same(a.Root.Type,b.Root.Type);Assert.Same(Assert.Single(a.AttributeUses),Assert.Single(b.AttributeUses));var global="<xs:attribute name='g' type='xs:boolean'/><xs:attributeGroup name='G'><xs:attribute ref='g' use='required'/></xs:attributeGroup>";var g=Compile(S(tag,true,global));Assert.Same(Assert.Single(g.GlobalAttributeDeclarations),g.AttributeUses[0].CanonicalDeclaration);Assert.NotSame(g.AttributeUses[0],g.AttributeUses[0].CanonicalDeclaration);}
    /// <summary>Original compositor rules and physical member facts remain authoritative.</summary>
    /// <param name="tag">Compositor.</param>
    [Theory][InlineData("all")][InlineData("choice")]
    public void Models(string tag)
    {var source=S(tag).Replace("<xs:element name='v' type='xs:string'/>","<xs:element name='v' type='xs:string'/><xs:element name='w' type='xs:string'/>");var valid=tag=="all"?"<a g='false'><w>second</w><v>first</v></a>":Xml;using var f=new Fixture(source,valid,atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Equal(tag=="all"?"second":"text",f.Read(f.Element([0])).StringValue);var invalid=tag=="all"?Xml:"<a g='false'><v>one</v><w>two</w></a>";using var bad=new Fixture(source,invalid,atoms:16);Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.Empty(bad.Assessment.Facts);Assert.False(bad.Input.IsAssessed);}
    /// <summary>Default and explicit uses share existing effective and model capacities.</summary>
    /// <param name="tag">Compositor.</param>
    [Theory][InlineData("all")][InlineData("choice")]
    public void Capacity(string tag)
    {var group="<xs:attributeGroup name='G'>"+string.Concat(Enumerable.Range(0,16).Select(i=>"<xs:attribute name='g"+i+"' type='xs:boolean'/>"))+"</xs:attributeGroup>";Assert.Equal(16,Compile(S(tag,group:group)).AttributeUses.Length);Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(tag,group:group,tail:"<xs:attribute name='extra' type='xs:boolean'/>"),Options).Outcome);var members=string.Concat(Enumerable.Range(0,17).Select(i=>"<xs:element name='v"+i+"' type='xs:string'/>"));Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(tag).Replace("<xs:element name='v' type='xs:string'/>",members),Options).Outcome);}
    /// <summary>No-facts failures preserve recovery, cancellation, detached values, ownership and expiry.</summary>
    [Fact]public void Lifetime()
    {foreach(var tag in new[]{"all","choice"}){var source=S(tag);using var owner=NativeSchemaSession.Create(source,Options).Value!;Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<a><v>text</v></a>",Context).Outcome);var doc=owner.Assess(Xml,Context).Value!;var detached=doc.ReadAttribute(0).Value;using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>doc.ExportXml(cancel.Token));doc.Dispose();Assert.Throws<ObjectDisposedException>(()=>doc.ReadAttribute(0));Assert.False(detached.BooleanValue);using var recovered=owner.Assess(Xml,Context).Value!;using var f=new Fixture(source,Xml,atoms:16);using var other=new Fixture(source,Xml,atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","g"))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Attribute([],"","g"))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Declaration!,foreign.Declaration!,default));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));}}
    /// <summary>Source schema namespace bindings do not change with owner-local prefix shadowing.</summary>
    [Fact]public void NamespaceAuthority()
    {foreach(var tag in new[]{"all","choice"}){var source=S(tag).Replace("defaultAttributes='G'","targetNamespace='urn:s' xmlns:t='urn:s' attributeFormDefault='qualified' defaultAttributes='t:G'").Replace("<xs:complexType>","<xs:complexType xmlns:t='urn:shadow'>");using var f=new Fixture(source,"<t:a xmlns:t='urn:s' t:g='false'><v>text</v></t:a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"urn:s","g"))!;Assert.Equal("urn:s",fact.NativeDeclaration.NamespaceUri);Assert.False(f.Read(fact.Node).BooleanValue);}}
    /// <summary>Active wildcard/assertion and legacy routes retain their documented exclusions.</summary>
    [Fact]public void OtherScopes()
    {foreach(var tag in new[]{"all","choice"}){Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(tag,tail:"<xs:anyAttribute namespace='##local' processContents='strict'/>"),Options).Outcome);Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(tag,tail:"<xs:assert test='true()'/>"),Options).Outcome);var legacy=NativeDecimalSession.Create(S(tag),new(256,256,1,32,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,legacy.Outcome);Assert.Null(legacy.Value);}}
    /// <summary>The separate opt-out closed-attribute capability is not broadened to all/choice.</summary>
    [Fact]public void OptOutBoundary()
    {foreach(var tag in new[]{"all","choice"}){using var owner=NativeSchemaSession.Create(S(tag).Replace("<xs:complexType>","<xs:complexType defaultAttributesApply='false'>"),Options).Value!;Assert.Equal(NativeSchemaOutcome.Unsupported,owner.Assess(Xml,Context).Outcome);using var good=owner.Assess("<a><v>text</v></a>",Context).Value!;Assert.Equal(0,good.AttributeCount);}}
}
