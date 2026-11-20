// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Checks actual root attribute nodes, original facts, provenance and guarded lifetimes.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Globalization;
using Bosak.XPath.Api;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Qualifies actual native root attributes rather than synthetic metadata.</summary>
public sealed class NativeAttributeCoreTests
{
    private static string Schema(string type="xs:string")=>"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:t='urn:a' targetNamespace='urn:a' elementFormDefault='qualified'><xs:simpleType name='Label'><xs:restriction base='xs:string'><xs:length value='1'/></xs:restriction></xs:simpleType><xs:simpleType name='Money'><xs:restriction base='xs:decimal'><xs:minInclusive value='0'/></xs:restriction></xs:simpleType><xs:complexType name='Order'><xs:sequence><xs:element name='amount' type='xs:decimal' minOccurs='0'/></xs:sequence><xs:attribute name='value' type='"+type+"' form='qualified' use='required'/></xs:complexType><xs:element name='r' type='t:Order'/><xs:element name='quote' type='t:Order'/></xs:schema>";
    private static string Xml(string value,string root="r")=>"<"+root+" xmlns='urn:a' xmlns:p='urn:a' p:value='"+value+"'><amount>1</amount></"+root+">";
    private static NativeSchemaProvider Provider(string type="xs:string")=>new(NativeDecimalSchema.Compile(Schema(type),new(256,256),maxStringCharacters:256).Value!);
    private static SchemaQuerySession Session(NativeSchemaProvider p)=>SchemaQuerySession.CompileXPath("$value",new(2,8,1,256,256),assessor:p).Value;
    private static NativeSchemaProvider.Assessment Assess(SchemaQuerySession s,NativeSchemaInput input)=>(NativeSchemaProvider.Assessment)s.Advisor!.Assess(new(input,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);
    private sealed record Episode:ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }
    private static string Text(XdmValue v)=>v.Kind switch
    {
        XdmValueKind.String=>v.StringValue,XdmValueKind.Boolean=>v.BooleanValue?"true":"false",XdmValueKind.Integer=>v.BigIntegerValue.ToString(CultureInfo.InvariantCulture),_=>v.ExactDecimalValue.ToInvariantString(256)
    };
    /// <summary>Actual attribute facts, declaration/type identities and atomization keep original exact values.</summary>
    /// <param name="type">Declared primitive/built-in/named type QName.</param>
    /// <param name="lexical">Attribute XML lexical text.</param>
    /// <param name="expected">Normalized exact typed value.</param>
    [Theory][InlineData("xs:string"," a&#xD;b "," a\rb ")][InlineData("xs:boolean","0","false")][InlineData("xs:boolean","true","true")][InlineData("xs:integer","9223372036854775808","9223372036854775808")][InlineData("xs:decimal","12.50","12.5")][InlineData("t:Label","&#x1F600;","\U0001F600")][InlineData("t:Money","12.50","12.5")]
    public void NativeAttribute_RetainsActualNodeAndOriginalTypedEvidence(string type,string lexical,string expected)
    {
        var p=Provider(type);using var s=Session(p);using var input=NativeSchemaInput.Capture(Xml(lexical)).Value!;var result=Assess(s,input);Assert.Equal(SchemaAssessmentStatus.Valid,result.Status);
        Assert.Equal(2,result.ElementFacts.Length);Assert.Single(result.AttributeFacts);Assert.Equal(3,result.Facts.Length);var fact=result.AttributeFacts[0];var node=fact.Node;
        Assert.Equal(XdmNodeKind.Attribute,node.NodeKind);Assert.Same(node,input.RootAttributeNode(System.Xml.Linq.XName.Get("value","urn:a")));Assert.True(input.OwnsNode(node));
        Assert.Equal(("urn:a","value"),node.SchemaAttributeDeclaration!.Value);Assert.Null(node.SchemaElementDeclaration);Assert.False(node.HasNoTypedValue);
        var local=type.Split(':')[1];var ns=type.StartsWith("t:",StringComparison.Ordinal)?"urn:a":"http://www.w3.org/2001/XMLSchema";
        var declared=p.ResolveType(new(ns,local),default).Value;Assert.Same(declared,fact.Type);Assert.NotSame(fact.Type,fact.Declaration);Assert.Equal((ns,local),node.SchemaTypeAnnotation!.Value);
        Assert.Equal(SchemaResultKind.Unsupported,p.ResolveAttributeDeclaration(new("urn:a","value"),default).Kind);
        Assert.Equal(expected,Text(node.TypedValue));Assert.Equal(expected,Text(XPath31Expression.Compile("data(.)").Evaluate(node)));
        using var snapshot=s.OpenSnapshot(input,input,result.Origin).Value;var detached=new NativeSessionRunner(s).Run([snapshot],op=>
        {
            var read=op.ReadTypedValue(snapshot,fact.ReadTypedValue(default).Value);Assert.Equal(SchemaResultKind.Supported,read.Kind);var actual=op.Evaluate(new EvaluationContext().WithVariable("value",read.Value));
            Assert.True(SchemaAtomics.TryGetProvenance(actual,out var evidence));Assert.Same(declared,evidence!.DeclaredType);Assert.Same(declared,evidence.ActualType);Assert.Same(result.Origin,evidence.OriginalAssessment);
            Assert.Equal("FOTY0012",result.ElementFacts[0].ReadTypedValue(default).DiagnosticCode);return Text(actual);
        });
        input.Dispose();Assert.Equal(expected,detached);Assert.Throws<ObjectDisposedException>(()=>node.TypedValue);Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));
    }
    /// <summary>Named complex references reuse local attribute declarations, while facts stay input-specific.</summary>
    [Fact]
    public void NamedComplexRoots_ReuseLocalDeclarationWithoutSharingNodeFacts()
    {
        var p=Provider();using var s=Session(p);using var a=NativeSchemaInput.Capture(Xml("a")).Value!;using var b=NativeSchemaInput.Capture(Xml("b","quote")).Value!;
        var x=Assess(s,a);var y=Assess(s,b);Assert.Same(x.AttributeFacts[0].Declaration,y.AttributeFacts[0].Declaration);Assert.Same(x.AttributeFacts[0].Type,y.AttributeFacts[0].Type);Assert.NotSame(x.AttributeFacts[0],y.AttributeFacts[0]);Assert.NotSame(x.AttributeFacts[0].Node,y.AttributeFacts[0].Node);
    }
    /// <summary>Invalid required/lexical/leaf/undeclared cases and capacity refusal never publish partial facts.</summary>
    [Theory][InlineData(0)][InlineData(1)][InlineData(2)][InlineData(3)][InlineData(4)]
    public void FailedAttributeAssessment_PublishesNoOriginalFacts(int kind)
    {
        var p=Provider("xs:boolean");using var s=Session(p);var xml=kind switch {0=>Xml("bad"),1=>Xml("0").Replace("p:value='0'",""),2=>Xml("0").Replace("<amount>1</amount>","<amount>bad</amount>"),3=>Xml("0").Replace("p:value='0'","p:value='0' extra='x'"),_=>Xml(new string(' ',256)+"0")};
        using var input=NativeSchemaInput.Capture(xml).Value!;var result=Assess(s,input);Assert.Equal(kind==4?SchemaAssessmentStatus.Unsupported:SchemaAssessmentStatus.Invalid,result.Status);Assert.Empty(result.Facts);Assert.Empty(result.ElementFacts);Assert.Empty(result.AttributeFacts);Assert.Null(result.ValidatedRoot);Assert.Null(result.Evidence);Assert.False(input.IsAssessed);
    }
    /// <summary>Foreign attribute nodes, facts, origin and declaration/type handles remain untrusted.</summary>
    [Fact]
    public void ForeignAttributeFacts_DoNotAuthenticateEqualNames()
    {
        var p=Provider();var q=Provider();using var a=Session(p);using var b=Session(q);using var x=NativeSchemaInput.Capture(Xml("x")).Value!;using var y=NativeSchemaInput.Capture(Xml("x")).Value!;
        var ax=Assess(a,x);var by=Assess(b,y);var local=ax.AttributeFacts[0];var foreign=by.AttributeFacts[0];Assert.False(p.Owns(foreign.Type));Assert.False(p.Owns(foreign.Declaration!));Assert.False(a.Advisor!.OwnsNode(local.Node,foreign,x,ax.Origin));Assert.False(a.Advisor.OwnsAssessment(by.Origin));
    }
    /// <summary>Separate attribute entry points participate in the same sequential admission gate.</summary>
    [Fact]
    public void AttributeReads_RefuseOverlapAndRecover()
    {
        using var s=NativeDecimalSessionCore.CreateMixed(Schema(),new(256,256,256,65536,1,8,1,256,256)).Value!;using var d=s.Assess(Xml("x"),DateTimeOffset.UnixEpoch,0).Value!;
        s.Work(()=>{Assert.Throws<NotSupportedException>(()=>d.AttributeCount);Assert.Throws<NotSupportedException>(()=>d.ReadAttribute(0));Assert.Throws<NotSupportedException>(()=>d.ReadScalar(0));return 0;},default);
        Assert.Equal("x",d.ReadAttribute(0).Value.StringValue);Assert.Equal(1,d.DecimalCount);
    }
}
