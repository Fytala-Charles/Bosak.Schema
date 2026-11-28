// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 28 November 2026
// PURPOSE              : Proves QName length component ancestry and original value authority.
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
namespace Bosak.Schema.Tests.QNameLengthFacets;
/// <summary>Verifies QName component consistency without lexical length measurement.</summary>
public sealed class NativeQNameLengthFacetsProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string S(string definitions)=>"<xs:schema xmlns:xs='"+X+"' xmlns:p='urn:one'>"+definitions+"</xs:schema>";
    private static string T(string name,string source,string facets)=>"<xs:simpleType name='"+name+"'><xs:restriction base='"+source+"'>"+facets+"</xs:restriction></xs:simpleType>";
    private static string Root=>"<xs:element name='a' type='T'/>";
    private static string Xml=>"<a xmlns:longprefix='urn:one'>longprefix:item</a>";
    private static NativeSchemaSessionOptions Options=>new(256,256,1024,65536,2,256,16,256,1024);
    /// <summary>Named and anonymous direct bases retain actual QName facts and provenance.</summary>
    /// <param name="facet">Length facet.</param><param name="anonymous">Base representation.</param>
    [Theory][InlineData("length",false)][InlineData("length",true)][InlineData("minLength",false)][InlineData("minLength",true)][InlineData("maxLength",false)][InlineData("maxLength",true)]
    public void Authority(string facet,bool anonymous)
    {
        var b="<xs:simpleType><xs:restriction base='xs:QName'><xs:"+facet+" value='0'/></xs:restriction></xs:simpleType>";
        var source=S(anonymous?"<xs:simpleType name='T'><xs:restriction>"+b+"</xs:restriction></xs:simpleType>"+Root:T("T","B","")+T("B","xs:QName","<xs:"+facet+" value='0'/>")+Root);
        using var f=new Fixture(source,Xml,atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal("T",fact.NativeType.Name);Assert.Equal(anonymous?null:"B",fact.NativeType.DerivationBase!.Name);Assert.Equal(NativeInterchangeKind.QName,fact.NativeType.InterchangeKind);Assert.True(f.Provider.Owns(f.Provider.TypeComponent(fact.NativeType.DerivationBase)));var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);var atom=Assert.Single(typed.AtomicItems);Assert.Equal(XdmValueKind.QName,atom.Value.Kind);Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);
    }
    /// <summary>Intermediate omission retains component bounds and fixed ancestry.</summary>
    /// <param name="facet">Facet.</param><param name="value">Derived value.</param><param name="expected">Compile outcome.</param>
    [Theory][InlineData("length",5,"Valid")][InlineData("length",6,"Invalid")][InlineData("minLength",6,"Invalid")][InlineData("maxLength",4,"Invalid")]
    public void FixedAcrossOmission(string facet,int value,string expected)
    {var source=S(T("B","xs:QName","<xs:"+facet+" value='5' fixed='true'/>")+T("C","B","")+T("T","C","<xs:"+facet+" value='"+value+"'/>")+Root);var c=NativeSchemaSession.Create(source,Options);Assert.Equal(expected,c.Outcome.ToString());c.Value?.Dispose();}
    /// <summary>The special length/min-max rule uses restriction ancestors rather than XML parents.</summary>
    [Fact]public void ComponentAncestorRule()
    {
        var source=S(T("B","xs:QName","<xs:minLength value='1'/><xs:maxLength value='9'/>")+T("C","B","<xs:length value='5'/>")+T("T","C","<xs:minLength value='1'/><xs:maxLength value='9'/>")+Root);using var owner=NativeSchemaSession.Create(source,Options).Value!;using var doc=owner.Assess(Xml,new(DateTimeOffset.UnixEpoch,0)).Value!;Assert.Equal("item",doc.ReadRootScalar().QNameValue.LocalName);Assert.Equal(NativeSchemaOutcome.Invalid,NativeSchemaSession.Create(source.Replace("base='C'><xs:minLength value='1'","base='C'><xs:minLength value='2'"),Options).Outcome);
    }
    /// <summary>Facet and instance namespace scopes remain independent of ignored lengths.</summary>
    [Fact]public void NamespaceAndPattern()
    {var source=S(T("T","xs:QName","<xs:maxLength value='0'/><xs:enumeration xmlns:p='urn:inner' value='p:item'/>")+Root);using var owner=NativeSchemaSession.Create(source,Options).Value!;var ctx=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess(Xml,ctx).Outcome);using var doc=owner.Assess("<a xmlns:q='urn:inner'>q:item</a>",ctx).Value!;Assert.Equal("urn:inner",doc.ReadRootScalar().QNameValue.NamespaceUri);}
    /// <summary>Complete invalid assessment publishes no facts; nil retains typed-empty governing authority.</summary>
    [Fact]public void NoFactsAndNil()
    {var source=S(T("T","xs:QName","<xs:length value='0'/>")+Root);using(var bad=new Fixture(source,"<a>missing:item</a>",atoms:16)){Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.Empty(bad.Assessment.Facts);Assert.False(bad.Input.IsAssessed);}using var nil=new Fixture(source.Replace("name='a'","name='a' nillable='true'"),"<a xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true'/>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,nil.Assessment.Status);var fact=nil.Input.FactsFor((NativeSessionNode)nil.Element([]))!;var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);Assert.Empty(typed.AtomicItems);}
    /// <summary>Recovery, detached values, cancellation and foreign ownership retain their contracts.</summary>
    [Fact]public void Lifetime()
    {var source=S(T("T","xs:QName","<xs:minLength value='65536'/>")+Root);using var owner=NativeSchemaSession.Create(source,Options).Value!;var ctx=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<a>?</a>",ctx).Outcome);var doc=owner.Assess(Xml,ctx).Value!;var detached=doc.ReadRootScalar();using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>doc.ReadRootScalar(cancel.Token));doc.Dispose();Assert.Throws<ObjectDisposedException>(()=>doc.ReadRootScalar());Assert.Equal("urn:one",detached.QNameValue.NamespaceUri);using var recovered=owner.Assess(Xml,ctx).Value!;using var f=new Fixture(source,Xml,atoms:16);using var other=new Fixture(source,Xml,atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Type,foreign.Type,default));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.Declaration);}
    /// <summary>Large facet/lexical capacities refuse without weakening QName grammar or recovery.</summary>
    [Fact]public void Capacity()
    {Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(S(T("T","xs:QName","<xs:length value='65537'/>")+Root),Options).Outcome);using var owner=NativeSchemaSession.Create(S(T("T","xs:QName","<xs:length value='0'/>")+Root),Options).Value!;var ctx=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);var refusal=owner.Assess("<a>"+new string('x',257)+"</a>",ctx);Assert.Equal(NativeSchemaOutcome.Unsupported,refusal.Outcome);Assert.Null(refusal.Value);using var recovered=owner.Assess(Xml,ctx).Value!;Assert.Equal("item",recovered.ReadRootScalar().QNameValue.LocalName);}
}
