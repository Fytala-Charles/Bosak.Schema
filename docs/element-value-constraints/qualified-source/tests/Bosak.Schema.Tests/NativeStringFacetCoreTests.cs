// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Checks named string node facts, atomization and original type provenance.
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

using Bosak.XPath.Api;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Checks original named string component identity and actual string atoms through Core.</summary>
public sealed class NativeStringFacetCoreTests
{
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:t='urn:s' targetNamespace='urn:s' elementFormDefault='qualified'><xs:simpleType name='Label'><xs:restriction base='xs:string'><xs:length value='1'/></xs:restriction></xs:simpleType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='s' type='t:Label' maxOccurs='2'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
    private static NativeSchemaProvider Provider()=>new(NativeDecimalSchema.Compile(Schema,new(256,256),maxStringCharacters:2).Value!);
    private static SchemaQuerySession Session(NativeSchemaProvider p)=>SchemaQuerySession.CompileXPath("$value",new(1,4,1,256,256),assessor:p).Value;
    private static NativeSchemaProvider.Assessment Assess(SchemaQuerySession s,NativeSchemaInput i)=>(NativeSchemaProvider.Assessment)s.Advisor!.Assess(new(i,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);
    private sealed record Episode:ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }
    /// <summary>Named string atomics retain exact text and declared/actual named provenance.</summary>
    /// <param name="lexical">XML text for the string leaf.</param>
    /// <param name="expected">Exact parsed string.</param>
    [Theory][InlineData("a","a")][InlineData("é","é")][InlineData("😀","😀")][InlineData(" "," ")][InlineData("&#xD;","\r")]
    public void NamedStringNode_RetainsAtomizationAndOriginalNamedProvenance(string lexical,string expected)
    {
        var p=Provider();using var s=Session(p);using var input=NativeSchemaInput.Capture("<r xmlns='urn:s'><s>"+lexical+"</s></r>").Value!;
        var result=Assess(s,input);Assert.Equal(SchemaAssessmentStatus.Valid,result.Status);
        var named=p.ResolveType(new("urn:s","Label"),default).Value;
        Assert.NotSame(p.ResolveType(new("http://www.w3.org/2001/XMLSchema","string"),default).Value,named);
        Assert.Same(named,result.Facts[1].Type);var node=input.ElementNodes[1];Assert.Equal(("urn:s","Label"),node.SchemaTypeAnnotation!.Value);Assert.False(node.HasNoTypedValue);
        Assert.Equal(expected,node.TypedValue.StringValue);Assert.Equal(expected,XPath31Expression.Compile("data(.)").Evaluate(node).StringValue);
        using var snapshot=s.OpenSnapshot(input,input,result.Origin).Value;var issued=result.Facts[1].ReadTypedValue(default).Value;
        var detached=new NativeSessionRunner(s).Run([snapshot],op=>
        {
            var scalar=op.ReadTypedValue(snapshot,issued).Value;var evaluated=op.Evaluate(new EvaluationContext().WithVariable("value",scalar));
            Assert.True(SchemaAtomics.TryGetProvenance(evaluated,out var evidence));Assert.Same(named,evidence!.DeclaredType);Assert.Same(named,evidence.ActualType);Assert.Same(result.Origin,evidence.OriginalAssessment);
            Assert.Equal(expected,evaluated.StringValue);Assert.Equal("FOTY0012",result.Facts[0].ReadTypedValue(default).DiagnosticCode);return evaluated.StringValue;
        });
        input.Dispose();Assert.Equal(expected,detached);Assert.Throws<ObjectDisposedException>(()=>node.TypedValue);
    }
    /// <summary>Repeated references share one named type while keeping independent node facts.</summary>
    [Fact]
    public void RepeatedNamedString_UsesOneOriginalTypeComponent()
    {
        var p=Provider();using var s=Session(p);using var input=NativeSchemaInput.Capture("<r xmlns='urn:s'><s>a</s><s>b</s></r>").Value!;var result=Assess(s,input);
        Assert.Equal(SchemaAssessmentStatus.Valid,result.Status);Assert.Same(result.Facts[1].Type,result.Facts[2].Type);Assert.NotSame(result.Facts[1],result.Facts[2]);
        Assert.Equal("a",input.ElementNodes[1].TypedValue.StringValue);Assert.Equal("b",input.ElementNodes[2].TypedValue.StringValue);
    }
    /// <summary>Facet invalidity and representation refusal issue no partial facts.</summary>
    [Fact]
    public void StringFacetFailure_ProducesNoPartialFacts()
    {
        foreach(var value in new[]{"ab","abc"})
        {
            using var s=Session(Provider());using var input=NativeSchemaInput.Capture("<r xmlns='urn:s'><s>"+value+"</s></r>").Value!;var result=Assess(s,input);
            Assert.Equal(value=="ab"?SchemaAssessmentStatus.Invalid:SchemaAssessmentStatus.Unsupported,result.Status);Assert.Null(result.Evidence);Assert.Null(result.ValidatedRoot);Assert.Empty(result.Facts);Assert.False(input.IsAssessed);
        }
    }
    /// <summary>Same named type spelling does not authenticate foreign facts or disposed nodes.</summary>
    [Fact]
    public void ForeignNamedStringFacts_AndDisposedValuesRemainGuarded()
    {
        var p=Provider();var q=Provider();using var a=Session(p);using var b=Session(q);
        using var x=NativeSchemaInput.Capture("<r xmlns='urn:s'><s>a</s></r>").Value!;using var y=NativeSchemaInput.Capture("<r xmlns='urn:s'><s>a</s></r>").Value!;
        var ax=Assess(a,x);var by=Assess(b,y);Assert.False(p.Owns(by.Facts[1].Type));Assert.False(a.Advisor!.OwnsNode(x.ElementNodes[1],by.Facts[1],x,ax.Origin));Assert.False(a.Advisor.OwnsAssessment(by.Origin));x.Dispose();Assert.Throws<ObjectDisposedException>(()=>ax.Facts[1].ReadTypedValue(default));
    }
}
