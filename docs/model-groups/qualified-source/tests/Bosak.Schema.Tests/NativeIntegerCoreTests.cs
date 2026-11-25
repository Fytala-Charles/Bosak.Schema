// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Qualifies actual integer node facts, atomization, provenance and refusal boundaries.
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
using System.Numerics;
using Bosak.XPath.Api;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Checks actual integer facts and exact atomic values through the published Core APIs.</summary>
public sealed class NativeIntegerCoreTests
{
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='n' type='xs:integer'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
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
    /// <summary>Integers retain exact values and original declared/actual provenance without Int64 narrowing.</summary>
    /// <param name="lexical">Input integer lexical form.</param>
    /// <param name="expected">Exact normalized integer.</param>
    [Theory][InlineData("-0","0")][InlineData("+00012","12")][InlineData("9223372036854775808","9223372036854775808")][InlineData("-9223372036854775809","-9223372036854775809")][InlineData("12345678901234567890123456789012345678901234567890","12345678901234567890123456789012345678901234567890")]
    public void ActualIntegerNode_RetainsExactAtomizationAndOriginalProvenance(string lexical,string expected)
    {
        var exact=BigInteger.Parse(expected,CultureInfo.InvariantCulture);var p=Provider();using var s=Session(p);using var input=NativeSchemaInput.Capture("<r><n>"+lexical+"</n></r>").Value!;
        var result=Assess(s,input);Assert.Equal(SchemaAssessmentStatus.Valid,result.Status);
        var integer=p.ResolveType(new("http://www.w3.org/2001/XMLSchema","integer"),default).Value;
        Assert.NotSame(p.DecimalType,integer);Assert.NotSame(p.ResolveType(new("http://www.w3.org/2001/XMLSchema","boolean"),default).Value,integer);
        Assert.Same(integer,result.Facts[1].Type);var node=input.ElementNodes[1];Assert.Equal(("http://www.w3.org/2001/XMLSchema","integer"),node.SchemaTypeAnnotation!.Value);Assert.False(node.HasNoTypedValue);
        Assert.Equal(exact,node.TypedValue.BigIntegerValue);Assert.Equal(exact,XPath31Expression.Compile("data(.)").Evaluate(node).BigIntegerValue);
        using var snapshot=s.OpenSnapshot(input,input,result.Origin).Value;var issued=result.Facts[1].ReadTypedValue(default).Value;
        var detached=new NativeSessionRunner(s).Run([snapshot],op=>
        {
            var scalar=op.ReadTypedValue(snapshot,issued).Value;var evaluated=op.Evaluate(new EvaluationContext().WithVariable("value",scalar));
            Assert.True(SchemaAtomics.TryGetProvenance(evaluated,out var evidence));Assert.Same(integer,evidence!.DeclaredType);Assert.Same(integer,evidence.ActualType);Assert.Same(result.Origin,evidence.OriginalAssessment);
            Assert.Equal(exact,evaluated.BigIntegerValue);Assert.Equal("FOTY0012",result.Facts[0].ReadTypedValue(default).DiagnosticCode);return evaluated.BigIntegerValue;
        });
        input.Dispose();Assert.Equal(exact,detached);Assert.Throws<ObjectDisposedException>(()=>node.TypedValue);
    }
    /// <summary>Refused/invalid integers issue no partial facts.</summary>
    [Fact]
    public void IntegerFailure_ProducesNoPartialFacts()
    {
        foreach(var value in new[]{"invalid",new string('9',257)})
        {
            using var s=Session(Provider());using var input=NativeSchemaInput.Capture("<r><n>"+value+"</n></r>").Value!;var result=Assess(s,input);
            Assert.Equal(value=="invalid"?SchemaAssessmentStatus.Invalid:SchemaAssessmentStatus.Unsupported,result.Status);Assert.Null(result.Evidence);Assert.Null(result.ValidatedRoot);Assert.Empty(result.Facts);Assert.False(input.IsAssessed);
        }
    }
    /// <summary>Integer component names do not authenticate foreign facts or disposed node reads.</summary>
    [Fact]
    public void ForeignIntegerFacts_AndDisposedValuesRemainGuarded()
    {
        var p=Provider();var q=Provider();using var a=Session(p);using var b=Session(q);
        using var x=NativeSchemaInput.Capture("<r><n>9223372036854775808</n></r>").Value!;using var y=NativeSchemaInput.Capture("<r><n>9223372036854775808</n></r>").Value!;
        var ax=Assess(a,x);var by=Assess(b,y);Assert.False(p.Owns(by.Facts[1].Type));Assert.False(a.Advisor!.OwnsNode(x.ElementNodes[1],by.Facts[1],x,ax.Origin));Assert.False(a.Advisor.OwnsAssessment(by.Origin));x.Dispose();Assert.Throws<ObjectDisposedException>(()=>ax.Facts[1].ReadTypedValue(default));
    }
}
