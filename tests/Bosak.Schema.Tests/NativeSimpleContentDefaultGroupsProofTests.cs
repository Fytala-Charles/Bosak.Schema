// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 28 November 2026
// PURPOSE              : Proves original simple-content G/A authority with default groups, budgets and lifetimes.
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
namespace Bosak.Schema.Tests.SimpleContentDefaultGroups;
/// <summary>Verifies original governing complex and actual content types with default groups.</summary>
public sealed class NativeSimpleContentDefaultGroupsProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private const string G="<xs:attributeGroup name='G'><xs:attribute name='g' type='xs:boolean' use='required'/></xs:attributeGroup>";
    private static string S(string type="string",bool named=false,string group=G,string tail="")=>"<xs:schema xmlns:xs='"+X+"' defaultAttributes='G'>"+group+(named?"<xs:complexType name='T'>":"<xs:element name='a'><xs:complexType>")+"<xs:simpleContent><xs:extension base='xs:"+type+"'>"+tail+"</xs:extension></xs:simpleContent></xs:complexType>"+(named?"<xs:element name='a' type='T'/><xs:element name='b' type='T'/>":"</xs:element>")+"</xs:schema>";
    private static NativeSchemaSessionOptions Options=>new(256,256,1024,65536,2,256,16,256,1024);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private static NativeDecimalSchema Compile(string source)=>NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;
    /// <summary>Governing complex G, actual simple A and original attribute type remain distinct and authoritative.</summary>
    /// <param name="type">Existing actual content type.</param><param name="text">Lexical value.</param><param name="named">Owner naming.</param>
    [Theory][InlineData("string","text",false)][InlineData("string","text",true)][InlineData("decimal","1.5",false)][InlineData("float","-0",true)][InlineData("date","2000-01-02Z",false)][InlineData("duration","P1D",true)]
    public void Authority(string type,string text,bool named)
    {using var f=new Fixture(S(type,named),"<a g='false'>"+text+"</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.True(fact.NativeType.IsComplex);Assert.Equal(named?"T":null,fact.NativeType.Name);Assert.Equal(type,fact.NativeType.ContentType!.Name);var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);var atom=Assert.Single(typed.AtomicItems);Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.NotSame(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Provider.TypeComponent(fact.NativeType.ContentType),atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);var attr=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","g"))!;Assert.True(attr.NativeType.IsBoolean);Assert.False(f.Read(attr.Node).BooleanValue);if(type=="float")Assert.True(BitConverter.SingleToInt32Bits((float)atom.Value.DoubleValue)<0);}
    /// <summary>Named aliases, canonical references and explicit/default repeats retain original declarations.</summary>
    [Fact]public void Identity()
    {var p=Compile(S(named:true,tail:"<xs:attributeGroup ref='G'/>"));var a=p.Roots.First();var b=p.Roots.Skip(1).First();Assert.Same(a.Root.Type,b.Root.Type);Assert.Same(Assert.Single(a.AttributeUses),Assert.Single(b.AttributeUses));var group="<xs:attribute name='g' type='xs:boolean'/><xs:attributeGroup name='G'><xs:attribute ref='g' use='required'/></xs:attributeGroup>";var c=Compile(S(group:group));Assert.Same(Assert.Single(c.GlobalAttributeDeclarations),Assert.Single(c.AttributeUses).CanonicalDeclaration);}
    /// <summary>Compound content retains actual original list items and union member authorities.</summary>
    /// <param name="union">Compound selector.</param>
    [Theory][InlineData(false)][InlineData(true)]public void Compound(bool union)
    {var declaration="<xs:simpleType name='A'>"+(union?"<xs:union memberTypes='xs:decimal xs:string'/>":"<xs:list itemType='xs:string'/>")+"</xs:simpleType>";var source=S().Replace("base='xs:string'","base='A'").Replace(G,declaration+G);using var f=new Fixture(source,union?"<a g='false'>1.5</a>":"<a g='false'>one two</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal("A",fact.NativeType.ContentType!.Name);var typed=fact.ReadTypedValue(default).Value;Assert.Equal(union?1:2,typed.AtomicItems.Count());foreach(var atom in typed.AtomicItems){Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.NotSame(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);}Assert.Equal(union?XdmValueKind.Decimal:XdmValueKind.String,typed.AtomicItems.First().Value.Kind);}
    /// <summary>Complete failures expose no facts; recovery retains cancellation, ownership, detached values and expiry.</summary>
    [Fact]public void Lifetime()
    {var source=S();using var bad=new Fixture(source,"<a>text</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.Empty(bad.Assessment.Facts);Assert.False(bad.Input.IsAssessed);using var owner=NativeSchemaSession.Create(source,Options).Value!;Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<a g='bad'>text</a>",Context).Outcome);var doc=owner.Assess("<a g='false'>text</a>",Context).Value!;var detached=doc.ReadRootScalar();using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>doc.ExportXml(cancel.Token));doc.Dispose();Assert.Throws<ObjectDisposedException>(()=>doc.ReadRootItems());Assert.Equal("text",detached.StringValue);using var recovered=owner.Assess("<a g='false'>again</a>",Context).Value!;using var f=new Fixture(source,"<a g='false'>text</a>",atoms:16);using var other=new Fixture(source,"<a g='false'>text</a>",atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","g"))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Attribute([],"","g"))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Declaration!,foreign.Declaration!,default));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));}
    /// <summary>Effective defaults and explicit attributes share the existing use ceiling.</summary>
    [Fact]public void Capacity()
    {var group="<xs:attributeGroup name='G'>"+string.Concat(Enumerable.Range(0,16).Select(i=>"<xs:attribute name='g"+i+"' type='xs:boolean'/>"))+"</xs:attributeGroup>";Assert.Equal(16,Compile(S(group:group)).AttributeUses.Length);Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(group:group,tail:"<xs:attribute name='extra' type='xs:boolean'/>"),Options).Outcome);}
    /// <summary>Integer/Boolean and wildcard/restriction boundaries remain independent of default groups.</summary>
    [Fact]public void Boundaries()
    {foreach(var type in new[]{"integer","int","boolean"})Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(type),Options).Outcome);Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(tail:"<xs:anyAttribute namespace='##local' processContents='strict'/>"),Options).Outcome);var legacy=NativeDecimalSession.Create(S(),new(256,256,1,32,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,legacy.Outcome);Assert.Null(legacy.Value);var optOut=S().Replace("<xs:complexType>","<xs:complexType defaultAttributesApply='false'>");using var owner=NativeSchemaSession.Create(optOut,Options).Value!;Assert.Equal(NativeSchemaOutcome.Unsupported,owner.Assess("<a g='false'>text</a>",Context).Outcome);using var valid=owner.Assess("<a>text</a>",Context).Value!;Assert.Equal(0,valid.AttributeCount);}
}
