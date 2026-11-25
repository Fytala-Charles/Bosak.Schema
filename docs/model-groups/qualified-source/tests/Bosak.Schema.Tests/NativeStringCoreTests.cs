// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Qualifies actual mixed Schema node facts, atomization and bounded XML export.
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

using System.Xml;
using Bosak.XPath.Api;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Uses the real mixed backend/provider through actual Core node and scalar operations.</summary>
public sealed class NativeStringCoreTests
{
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='s' type='xs:string'/><xs:element name='n' type='xs:decimal'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
    private const string Xml="<r><s> kept </s><n>12.50</n></r>";
    private static NativeSchemaProvider Provider()=>new(NativeDecimalSchema.Compile(Schema,new(256,256),maxStringCharacters:256).Value!);
    private static SchemaQuerySession Session(NativeSchemaProvider p)=>SchemaQuerySession.CompileXPath("$value",new(1,4,1,256,256),assessor:p).Value;
    private static NativeSchemaProvider.Assessment Assess(SchemaQuerySession s,NativeSchemaInput i)=>(NativeSchemaProvider.Assessment)s.Advisor!.Assess(new(i,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);
    private sealed record Episode:ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }

    /// <summary>Actual string node annotations, atomization and issued scalar evaluation retain original facts.</summary>
    [Fact]
    public void ActualMixedFacts_RetainStringIdentityAndRootFoty0012()
    {
        var p=Provider();using var s=Session(p);using var input=NativeSchemaInput.Capture(Xml).Value!;
        var result=Assess(s,input);Assert.Equal(SchemaAssessmentStatus.Valid,result.Status);
        var stringType=p.ResolveType(new("http://www.w3.org/2001/XMLSchema","string"),default).Value;
        Assert.NotSame(p.DecimalType,stringType);Assert.Same(stringType,result.Facts[1].Type);
        var node=input.ElementNodes[1];Assert.Equal(("http://www.w3.org/2001/XMLSchema","string"),node.SchemaTypeAnnotation!.Value);
        Assert.False(node.HasNoTypedValue);Assert.Equal(" kept ",node.TypedValue.StringValue);
        Assert.True(XPath31Expression.Compile("data(.) eq ' kept '").Evaluate(node).GetEffectiveBooleanValue());
        using var snapshot=s.OpenSnapshot(input,input,result.Origin).Value;
        var issued=result.Facts[1].ReadTypedValue(default).Value;
        var detached=new NativeSessionRunner(s).Run([snapshot],op=>
        {
            var read=op.ReadTypedValue(snapshot,issued).Value;var value=op.Evaluate(new EvaluationContext().WithVariable("value",read));
            Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(stringType,evidence!.DeclaredType);Assert.Same(stringType,evidence.ActualType);Assert.Same(result.Origin,evidence.OriginalAssessment);
            var error=Assert.Throws<InvalidOperationException>(()=>XPath31Expression.Compile("data(.)").Evaluate(op.GetRoot(snapshot)).GetEffectiveBooleanValue());Assert.StartsWith("FOTY0012:",error.Message,StringComparison.Ordinal);
            return value.StringValue;
        });
        input.Dispose();Assert.Equal(" kept ",detached);Assert.Throws<ObjectDisposedException>(()=>node.TypedValue);
    }

    /// <summary>Equal mixed schemas do not grant foreign type, facts, source or origin authority.</summary>
    [Fact]
    public void ForeignMixedFacts_AndDisposedTypedValuesRemainGuarded()
    {
        var p=Provider();var q=Provider();using var a=Session(p);using var b=Session(q);
        using var x=NativeSchemaInput.Capture(Xml).Value!;using var y=NativeSchemaInput.Capture(Xml).Value!;
        var ax=Assess(a,x);var by=Assess(b,y);Assert.False(p.Owns(by.Facts[1].Type));
        Assert.False(a.Advisor!.OwnsNode(x.ElementNodes[1],by.Facts[1],x,ax.Origin));Assert.False(a.Advisor.OwnsAssessment(by.Origin));
        x.Dispose();Assert.Throws<ObjectDisposedException>(()=>ax.Facts[1].ReadTypedValue(default));
    }

    /// <summary>String capacity failure publishes no original evidence and can release the captured input.</summary>
    [Fact]
    public void StringCapacity_ProducesNoPartialOwnedFacts()
    {
        var compiled=NativeDecimalSchema.Compile(Schema,new(256,256),maxStringCharacters:1).Value!;
        var p=new NativeSchemaProvider(compiled);using var s=Session(p);using var input=NativeSchemaInput.Capture(Xml).Value!;
        var result=Assess(s,input);Assert.Equal(SchemaAssessmentStatus.Unsupported,result.Status);Assert.Null(result.Evidence);Assert.Null(result.ValidatedRoot);Assert.Empty(result.Facts);Assert.False(input.IsAssessed);
    }

    /// <summary>Bounded writer checks escaped XML output before appending and never returns a failed partial buffer.</summary>
    [Fact]
    public void BoundedWriter_EscapingOverflowAndCancellation_AreTerminal()
    {
        using var output=new NativeBoundedTextWriter(8,default);
        using(var xml=XmlWriter.Create(output,new XmlWriterSettings{OmitXmlDeclaration=true,ConformanceLevel=ConformanceLevel.Fragment}))
        {
            Assert.Throws<NotSupportedException>(()=>{xml.WriteString("<<<<");xml.Flush();});
        }
        Assert.Throws<NotSupportedException>(()=>output.GetText());
        using var cts=new CancellationTokenSource();using var canceled=new NativeBoundedTextWriter(8,cts.Token);canceled.Write("ok");cts.Cancel();
        Assert.Throws<OperationCanceledException>(()=>canceled.Write('x'));Assert.Throws<OperationCanceledException>(()=>canceled.GetText());
    }
}
