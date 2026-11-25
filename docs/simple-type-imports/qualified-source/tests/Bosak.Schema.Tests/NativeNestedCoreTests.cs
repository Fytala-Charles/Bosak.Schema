// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Checks nested sequences, terminal scalar projections and original typed facts.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
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

/// <summary>Checks actual native nested fact alignment, declaration identity and lifetime.</summary>
public sealed class NativeNestedCoreTests
{
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:complexType name='Line'><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence></xs:complexType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='Line'/><xs:element name='b' type='Line'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
    private const string Xml="<r><a><v>12.50</v></a><b><v>3.75</v></b></r>";
    private static NativeSchemaProvider Provider()=>new(NativeDecimalSchema.Compile(Schema,new(256,256),maxStringCharacters:256).Value!);
    private static SchemaQuerySession Session(NativeSchemaProvider p)=>SchemaQuerySession.CompileXPath("$value",new(2,16,1,256,256),assessor:p).Value;
    private static NativeSchemaProvider.Assessment Assess(SchemaQuerySession s,NativeSchemaInput i)=>(NativeSchemaProvider.Assessment)s.Advisor!.Assess(new(i,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);
    private sealed record Episode:ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }
    /// <summary>All nodes align to preorder facts, including containers; terminal facts are a distinct projection.</summary>
    [Fact]
    public void CompleteFacts_AlignWithOriginalNodesAndLocalDeclarations()
    {
        var p=Provider();using var s=Session(p);using var input=NativeSchemaInput.Capture(Xml).Value!;var r=Assess(s,input);Assert.Equal(SchemaAssessmentStatus.Valid,r.Status);
        Assert.Equal(5,r.ElementFacts.Length);Assert.Equal(2,r.ScalarFacts.Length);Assert.Equal(5,r.Facts.Length);
        for(var i=0;i<5;i++){Assert.Same(input.ElementNodes[i],r.ElementFacts[i].Node);Assert.Same(r.Origin,r.ElementFacts[i].Origin);}
        Assert.Same(r.ElementFacts[2],r.ScalarFacts[0]);Assert.Same(r.ElementFacts[4],r.ScalarFacts[1]);
        Assert.Same(r.ElementFacts[1].Type,r.ElementFacts[3].Type);Assert.NotSame(r.ElementFacts[1].Declaration,r.ElementFacts[3].Declaration);Assert.Same(r.ElementFacts[2].Declaration,r.ElementFacts[4].Declaration);
        Assert.Null(r.ElementFacts[1].Node.SchemaElementDeclaration);Assert.Null(r.ElementFacts[3].Node.SchemaElementDeclaration);Assert.Equal(("","r"),r.ElementFacts[0].Node.SchemaElementDeclaration!.Value);
        Assert.Same(input.Root,r.ElementFacts[1].Node.Parent);Assert.Same(r.ElementFacts[1].Node,r.ElementFacts[2].Node.Parent);
        Assert.Equal("12.503.75",input.Root.StringValue);Assert.Equal("12.503.75",XPath31Expression.Compile("string(.)").Evaluate(input.Root).StringValue);
        foreach(var index in new[]{0,1,3}){var f=r.ElementFacts[index];Assert.True(f.Node.IsComplexType);Assert.True(f.HasNoTypedValue);Assert.Equal("FOTY0012",f.ReadTypedValue(default).DiagnosticCode);Assert.Contains("FOTY0012",Assert.Throws<InvalidOperationException>(()=>XPath31Expression.Compile("data(.)").Evaluate(f.Node)).Message);}
    }
    /// <summary>Each terminal travels through original public typed identity with exact decimal provenance.</summary>
    [Fact]
    public void OriginalTerminalFacts_AtomizeAndRetainProvenance()
    {
        var p=Provider();using var s=Session(p);using var input=NativeSchemaInput.Capture(Xml).Value!;var r=Assess(s,input);using var snapshot=s.OpenSnapshot(input,input,r.Origin).Value;
        var expected=new[]{"12.5","3.75"};
        for(var i=0;i<2;i++)
        {
            var f=r.ScalarFacts[i];Assert.Equal(expected[i],XPath31Expression.Compile("data(.)").Evaluate(f.Node).ExactDecimalValue.ToInvariantString(256));
            var actual=new NativeSessionRunner(s).Run([snapshot],op=>
            {
                var read=op.ReadTypedValue(snapshot,f.ReadTypedValue(default).Value);Assert.Equal(SchemaResultKind.Supported,read.Kind);var v=op.Evaluate(new EvaluationContext().WithVariable("value",read.Value));Assert.True(SchemaAtomics.TryGetProvenance(v,out var evidence));Assert.Same(f.Type,evidence!.DeclaredType);Assert.Same(p.DecimalType,evidence.ActualType);Assert.Same(r.Origin,evidence.OriginalAssessment);return v.ExactDecimalValue;
            });Assert.Equal(expected[i],actual.ToInvariantString(256));
        }
    }
    /// <summary>Deep failures do not publish any original assessment facts.</summary>
    /// <param name="xml">Invalid or excluded nested instance.</param>
    [Theory][InlineData("<r><a><v>1</v></a><b><v>bad</v></b></r>")][InlineData("<r><a><v>1</v></a><b/></r>")][InlineData("<r><a><v>1</v></a><b x='x'><v>2</v></b></r>")]
    public void DeepFailure_DoesNotPublishFacts(string xml)
    {
        var p=Provider();using var s=Session(p);using var input=NativeSchemaInput.Capture(xml).Value!;var r=Assess(s,input);Assert.NotEqual(SchemaAssessmentStatus.Valid,r.Status);Assert.Null(r.Evidence);Assert.Empty(r.ElementFacts);Assert.False(input.IsAssessed);
    }
    /// <summary>The same owner gate protects nested scalar counts and reads.</summary>
    [Fact]
    public void Overlap_RefusesAndRecovers()
    {
        using var s=NativeDecimalSessionCore.CreateMixed(Schema,new(256,256,256,65536,1,8,1,256,256)).Value!;using var d=s.Assess(Xml,DateTimeOffset.UnixEpoch,0).Value!;
        s.Work(()=>{Assert.Throws<NotSupportedException>(()=>d.ReadScalar(0));Assert.Throws<NotSupportedException>(()=>d.DecimalCount);return 0;},default);Assert.Equal(2,d.DecimalCount);Assert.Equal("12.5",d.ReadScalar(0).DecimalValue.ToInvariantString(256));
    }
    /// <summary>Original nested values cannot be admitted through another authority or survive input revocation.</summary>
    [Fact]
    public void ForeignAndRevokedNestedFacts_AreRefused()
    {
        var a=Provider();var b=Provider();using var sa=Session(a);using var sb=Session(b);using var ia=NativeSchemaInput.Capture(Xml).Value!;using var ib=NativeSchemaInput.Capture(Xml).Value!;
        var ar=Assess(sa,ia);var br=Assess(sb,ib);using var snapshot=sb.OpenSnapshot(ib,ib,br.Origin).Value;var issued=ar.ScalarFacts[0].ReadTypedValue(default).Value;
        new NativeSessionRunner(sb).Run([snapshot],op=>{Assert.Throws<ArgumentException>(()=>op.ReadTypedValue(snapshot,issued));return 0;});
        ia.Dispose();Assert.Throws<ObjectDisposedException>(()=>ar.ScalarFacts[0].ReadTypedValue(default));Assert.Throws<ObjectDisposedException>(()=>ar.ScalarFacts[0].Node.StringValue);
    }
}
