// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Checks complex simple-content roots with distinct original decimal atomic provenance.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.XPath.Api;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Qualifies actual native complex simple-content identity independently of decimal atomic types.</summary>
public sealed class NativeSimpleContentCoreTests
{
    private static string Schema(string baseType="xs:decimal")=>"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:t='urn:price' targetNamespace='urn:price'><xs:simpleType name='Money'><xs:restriction base='xs:decimal'><xs:minInclusive value='-100'/></xs:restriction></xs:simpleType><xs:complexType name='PriceType'><xs:simpleContent><xs:extension base='"+baseType+"'><xs:attribute name='currency' type='xs:string' use='required'/></xs:extension></xs:simpleContent></xs:complexType><xs:element name='price' type='t:PriceType'/><xs:element name='quote' type='t:PriceType'/></xs:schema>";
    private static string Xml(string lexical,string root="price")=>"<p:"+root+" xmlns:p='urn:price' currency='EUR'>"+lexical+"</p:"+root+">";
    private static NativeSchemaProvider Provider(string type="xs:decimal")=>new(NativeDecimalSchema.Compile(Schema(type),new(256,256),maxStringCharacters:256).Value!);
    private static SchemaQuerySession Session(NativeSchemaProvider p)=>SchemaQuerySession.CompileXPath("$value",new(2,8,1,256,256),assessor:p).Value;
    private static NativeSchemaProvider.Assessment Assess(SchemaQuerySession s,NativeSchemaInput input)=>(NativeSchemaProvider.Assessment)s.Advisor!.Assess(new(input,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);
    private sealed record Episode:ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }
    /// <summary>Governing complex annotation and actual atomic type remain distinct through native read/evaluation.</summary>
    /// <param name="baseType">Content decimal type QName.</param>
    /// <param name="lexical">Original XML text.</param>
    /// <param name="expected">Exact canonical decimal.</param>
    /// <param name="normalized">Schema-normalized lexical string value.</param>
    [Theory][InlineData("xs:decimal","12.50","12.5","12.50")][InlineData("xs:decimal"," \t+0012.50&#xD; ","12.5","+0012.50")][InlineData("xs:decimal","-0.000","0","-0.000")][InlineData("xs:decimal","12345678901234567890123456789012345678901234567890","12345678901234567890123456789012345678901234567890","12345678901234567890123456789012345678901234567890")][InlineData("t:Money","12.50","12.5","12.50")]
    public void NativeSimpleRoot_RetainsComplexAndAtomicOriginalTypes(string baseType,string lexical,string expected,string normalized)
    {
        var p=Provider(baseType);using var s=Session(p);using var input=NativeSchemaInput.Capture(Xml(lexical)).Value!;var result=Assess(s,input);Assert.Equal(SchemaAssessmentStatus.Valid,result.Status);
        var root=result.ElementFacts[0].Node;var complex=p.ResolveType(new("urn:price","PriceType"),default).Value;var actual=baseType.StartsWith("t:",StringComparison.Ordinal)?p.ResolveType(new("urn:price","Money"),default).Value:p.DecimalType;
        Assert.True(root.IsComplexType);Assert.False(root.HasNoTypedValue);Assert.Equal(("urn:price","PriceType"),root.SchemaTypeAnnotation!.Value);Assert.Equal(("urn:price","price"),root.SchemaElementDeclaration!.Value);Assert.Null(root.SchemaAttributeDeclaration);
        Assert.Same(complex,result.ElementFacts[0].Type);Assert.NotSame(complex,actual);Assert.Single(result.ElementFacts);Assert.Single(result.AttributeFacts);Assert.Equal(expected,root.TypedValue.ExactDecimalValue.ToInvariantString(512));
        Assert.Equal(normalized,root.StringValue);Assert.Equal(normalized,XPath31Expression.Compile("string(.)").Evaluate(root).StringValue);Assert.Equal(expected,XPath31Expression.Compile("data(.)").Evaluate(root).ExactDecimalValue.ToInvariantString(512));
        using var snapshot=s.OpenSnapshot(input,input,result.Origin).Value;var issued=result.ElementFacts[0].ReadTypedValue(default).Value;Assert.Same(complex,issued.GoverningType);
        var detached=new NativeSessionRunner(s).Run([snapshot],op=>
        {
            var read=op.ReadTypedValue(snapshot,issued);Assert.Equal(SchemaResultKind.Supported,read.Kind);var v=op.Evaluate(new EvaluationContext().WithVariable("value",read.Value));Assert.True(SchemaAtomics.TryGetProvenance(v,out var evidence));
            Assert.Same(complex,evidence!.DeclaredType);Assert.Same(actual,evidence.ActualType);Assert.Same(result.Origin,evidence.OriginalAssessment);return v.ExactDecimalValue;
        });input.Dispose();Assert.Equal(expected,detached.ToInvariantString(512));Assert.Throws<ObjectDisposedException>(()=>root.TypedValue);
    }
    /// <summary>Root declarations share the named complex type but preserve independent original facts.</summary>
    [Fact]
    public void SharedNamedComplexRoot_PreservesComponentsWithoutReusingFacts()
    {
        var p=Provider();using var s=Session(p);using var a=NativeSchemaInput.Capture(Xml("1")).Value!;using var b=NativeSchemaInput.Capture(Xml("2","quote")).Value!;var x=Assess(s,a);var y=Assess(s,b);
        Assert.Same(x.ElementFacts[0].Type,y.ElementFacts[0].Type);Assert.NotSame(x.ElementFacts[0].Declaration,y.ElementFacts[0].Declaration);Assert.NotSame(x.ElementFacts[0],y.ElementFacts[0]);Assert.Same(x.AttributeFacts[0].Declaration,y.AttributeFacts[0].Declaration);
    }
    /// <summary>No decimal root or attribute fact is published on invalid lexical/content/facet or capacity failures.</summary>
    /// <param name="kind">Failure variant.</param>
    [Theory][InlineData(0)][InlineData(1)][InlineData(2)][InlineData(3)]
    public void FailedSimpleContent_PublishesNothing(int kind)
    {
        var p=Provider("t:Money");using var s=Session(p);var lexical=kind switch {0=>"bad",1=>"<n>1</n>",2=>"-101",_=>new string('9',257)};using var input=NativeSchemaInput.Capture(Xml(lexical)).Value!;var r=Assess(s,input);
        Assert.Equal(kind==3?SchemaAssessmentStatus.Unsupported:SchemaAssessmentStatus.Invalid,r.Status);Assert.Empty(r.Facts);Assert.Empty(r.AttributeFacts);Assert.Empty(r.ElementFacts);Assert.False(input.IsAssessed);Assert.Null(r.Evidence);Assert.Null(r.ValidatedRoot);
    }
    /// <summary>Foreign complex root declarations/types/origins/facts never authenticate equal XML names.</summary>
    [Fact]
    public void ForeignSimpleRoot_RemainsGuarded()
    {
        var p=Provider();var q=Provider();using var a=Session(p);using var b=Session(q);using var x=NativeSchemaInput.Capture(Xml("1")).Value!;using var y=NativeSchemaInput.Capture(Xml("1")).Value!;var ax=Assess(a,x);var by=Assess(b,y);
        Assert.False(p.Owns(by.ElementFacts[0].Type));Assert.False(p.Owns(by.ElementFacts[0].Declaration!));Assert.False(a.Advisor!.OwnsNode(ax.ElementFacts[0].Node,by.ElementFacts[0],x,ax.Origin));Assert.False(a.Advisor.OwnsAssessment(by.Origin));
    }
    /// <summary>Root reads and availability checks use the same sequential gate as attribute/leaf operations.</summary>
    [Fact]
    public void RootReads_RefuseOverlapAndRecover()
    {
        using var s=NativeDecimalSessionCore.CreateMixed(Schema(),new(256,256,256,65536,1,8,1,256,256)).Value!;using var d=s.Assess(Xml("12.50"),DateTimeOffset.UnixEpoch,0).Value!;
        s.Work(()=>{Assert.Throws<NotSupportedException>(()=>d.HasRootScalar);Assert.Throws<NotSupportedException>(()=>d.ReadRootScalar());Assert.Throws<NotSupportedException>(()=>d.ReadAttribute(0));return 0;},default);Assert.True(d.HasRootScalar);Assert.Equal("12.5",d.ReadRootScalar().DecimalValue.ToInvariantString(256));
    }
}
