// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Qualifies actual Boolean node facts, atomization, provenance and refusal boundaries.
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

/// <summary>Checks actual native Boolean issuance rather than synthetic transport alone.</summary>
public sealed class NativeBooleanCoreTests
{
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='b' type='xs:boolean'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
    private static NativeSchemaProvider Provider()=>new(NativeDecimalSchema.Compile(Schema,new(256,256),maxStringCharacters:1).Value!);
    private static SchemaQuerySession Session(NativeSchemaProvider p)=>SchemaQuerySession.CompileXPath("$value",new(1,4,1,256,256),assessor:p).Value;
    private static NativeSchemaProvider.Assessment Assess(SchemaQuerySession s,NativeSchemaInput i)=>(NativeSchemaProvider.Assessment)s.Advisor!.Assess(new(i,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);
    private sealed record Episode:ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }

    /// <summary>All legal lexical forms produce actual Boolean node and atomic evidence through Core.</summary>
    /// <param name="lexical">Admitted lexical form.</param>
    /// <param name="expected">Exact Boolean value.</param>
    [Theory][InlineData("true",true)][InlineData("false",false)][InlineData("1",true)][InlineData("0",false)]
    public void ActualBooleanNode_RetainsTypedValueAndOriginalProvenance(string lexical,bool expected)
    {
        var p=Provider();using var s=Session(p);using var input=NativeSchemaInput.Capture("<r><b>"+lexical+"</b></r>").Value!;
        var result=Assess(s,input);Assert.Equal(SchemaAssessmentStatus.Valid,result.Status);
        var boolean=p.ResolveType(new("http://www.w3.org/2001/XMLSchema","boolean"),default).Value;
        Assert.NotSame(p.DecimalType,boolean);Assert.NotSame(p.ResolveType(new("http://www.w3.org/2001/XMLSchema","string"),default).Value,boolean);
        Assert.Same(boolean,result.Facts[1].Type);var node=input.ElementNodes[1];
        Assert.Equal(("http://www.w3.org/2001/XMLSchema","boolean"),node.SchemaTypeAnnotation!.Value);Assert.False(node.HasNoTypedValue);Assert.Equal(expected,node.TypedValue.BooleanValue);
        Assert.True(XPath31Expression.Compile("data(.) eq "+(expected?"true()":"false()")).Evaluate(node).GetEffectiveBooleanValue());
        using var snapshot=s.OpenSnapshot(input,input,result.Origin).Value;var issued=result.Facts[1].ReadTypedValue(default).Value;
        var detached=new NativeSessionRunner(s).Run([snapshot],op=>
        {
            var scalar=op.ReadTypedValue(snapshot,issued).Value;var evaluated=op.Evaluate(new EvaluationContext().WithVariable("value",scalar));
            Assert.True(SchemaAtomics.TryGetProvenance(evaluated,out var evidence));Assert.Same(boolean,evidence!.DeclaredType);Assert.Same(boolean,evidence.ActualType);Assert.Same(result.Origin,evidence.OriginalAssessment);
            Assert.Equal(expected,evaluated.BooleanValue);Assert.Equal("FOTY0012",result.Facts[0].ReadTypedValue(default).DiagnosticCode);
            return evaluated.BooleanValue;
        });
        input.Dispose();Assert.Equal(expected,detached);Assert.Throws<ObjectDisposedException>(()=>node.TypedValue);
    }

    /// <summary>Invalid/capacity refusals expose neither facts nor an original validated root.</summary>
    [Fact]
    public void BooleanFailure_ProducesNoPartialFacts()
    {
        foreach(var value in new[]{"invalid",new string(' ',253)+"true"})
        {
            using var s=Session(Provider());using var input=NativeSchemaInput.Capture("<r><b>"+value+"</b></r>").Value!;var result=Assess(s,input);
            Assert.Equal(value=="invalid"?SchemaAssessmentStatus.Invalid:SchemaAssessmentStatus.Unsupported,result.Status);Assert.Null(result.Evidence);Assert.Null(result.ValidatedRoot);Assert.Empty(result.Facts);Assert.False(input.IsAssessed);
        }
    }

    /// <summary>Identically named Boolean components never authenticate a foreign schema/input.</summary>
    [Fact]
    public void ForeignBooleanFacts_AndDisposedValuesRemainGuarded()
    {
        var p=Provider();var q=Provider();using var a=Session(p);using var b=Session(q);
        using var x=NativeSchemaInput.Capture("<r><b>false</b></r>").Value!;using var y=NativeSchemaInput.Capture("<r><b>false</b></r>").Value!;
        var ax=Assess(a,x);var by=Assess(b,y);Assert.False(p.Owns(by.Facts[1].Type));Assert.False(a.Advisor!.OwnsNode(x.ElementNodes[1],by.Facts[1],x,ax.Origin));Assert.False(a.Advisor.OwnsAssessment(by.Origin));
        x.Dispose();Assert.Throws<ObjectDisposedException>(()=>ax.Facts[1].ReadTypedValue(default));
    }
}
