// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Checks nested attribute owner facts, detached paths and independent indexes.
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

using System.Xml.Linq;
using Bosak.XPath.Api;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Checks actual native nested attributes against original captured owners and facts.</summary>
public sealed class NativeNestedAttributeCoreTests
{
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:complexType name='Line'><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attribute name='currency' type='xs:string' use='required'/></xs:complexType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='line' type='Line' maxOccurs='2'/></xs:sequence><xs:attribute name='id' type='xs:string' use='required'/></xs:complexType></xs:element></xs:schema>";
    private const string Xml="<r id='ROOT'><line currency='EUR'><v>12.50</v></line><line currency='USD'><v>3.75</v></line></r>";
    private static NativeSchemaProvider Provider()=>new(NativeDecimalSchema.Compile(Schema,new(256,256),maxStringCharacters:1024).Value!);
    private static SchemaQuerySession Session(NativeSchemaProvider p)=>SchemaQuerySession.CompileXPath("$value",new(2,16,1,256,256),assessor:p).Value;
    private static NativeSchemaProvider.Assessment Assess(SchemaQuerySession s,NativeSchemaInput input)=>(NativeSchemaProvider.Assessment)s.Advisor!.Assess(new(input,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);
    private sealed record Episode:ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }
    /// <summary>Equal names and shared uses do not merge original attribute nodes across repeated owners.</summary>
    [Fact]
    public void OriginalOwners_RetainDistinctNodesAndSharedUseIdentity()
    {
        var p=Provider();using var s=Session(p);using var input=NativeSchemaInput.Capture(Xml).Value!;var r=Assess(s,input);Assert.Equal(SchemaAssessmentStatus.Valid,r.Status);Assert.Equal(5,r.ElementFacts.Length);Assert.Equal(2,r.ScalarFacts.Length);Assert.Single(r.AttributeFacts);Assert.Equal(2,r.DescendantAttributeFacts.Length);Assert.Equal(8,r.Facts.Length);
        var a=r.DescendantAttributeFacts[0];var b=r.DescendantAttributeFacts[1];Assert.NotSame(a.Node,b.Node);Assert.Same(a.Declaration,b.Declaration);Assert.Same(a.Type,b.Type);Assert.Same(input.ElementNodes[1],a.Node.Parent);Assert.Same(input.ElementNodes[3],b.Node.Parent);Assert.Equal(new[]{0},a.OwnerElementPath);Assert.Equal(new[]{1},b.OwnerElementPath);
        Assert.False(s.Advisor!.OwnsNode(b.Node,a,input,r.Origin));Assert.True(s.Advisor.OwnsNode(a.Node,a,input,r.Origin));Assert.Equal("EUR",XPath31Expression.Compile("data(.)").Evaluate(a.Node).StringValue);Assert.Equal("USD",XPath31Expression.Compile("data(.)").Evaluate(b.Node).StringValue);
        using var snapshot=s.OpenSnapshot(input,input,r.Origin).Value;
        foreach(var fact in r.DescendantAttributeFacts)
        {
            var value=new NativeSessionRunner(s).Run([snapshot],op=>
            {
                var read=op.ReadTypedValue(snapshot,fact.ReadTypedValue(default).Value);Assert.Equal(SchemaResultKind.Supported,read.Kind);var v=op.Evaluate(new EvaluationContext().WithVariable("value",read.Value));Assert.True(SchemaAtomics.TryGetProvenance(v,out var evidence));Assert.Same(fact.Type,evidence!.DeclaredType);Assert.Same(fact.Type,evidence.ActualType);Assert.Same(r.Origin,evidence.OriginalAssessment);return v.StringValue;
            });Assert.Equal(fact.Node.StringValue,value);
        }
    }
    /// <summary>Path, owner index and name must all match the captured original node.</summary>
    [Fact]
    public void CapturedOwnerMapping_RejectsMismatchedLocatorOrName()
    {
        using var input=NativeSchemaInput.Capture(Xml).Value!;Assert.Same(input.ElementAttributeNode(1,[0],XName.Get("line"),XName.Get("currency")),input.ElementAttributeNode(1,[0],XName.Get("line"),XName.Get("currency")));
        Assert.Throws<InvalidOperationException>(()=>input.ElementAttributeNode(3,[0],XName.Get("line"),XName.Get("currency")));Assert.Throws<InvalidOperationException>(()=>input.ElementAttributeNode(1,[0],XName.Get("wrong"),XName.Get("currency")));Assert.Throws<InvalidOperationException>(()=>input.ElementAttributeNode(1,[9],XName.Get("line"),XName.Get("currency")));
    }
    /// <summary>Deep required/lexical/unknown/special failures never publish any original facts.</summary>
    /// <param name="attributes">Attributes supplied on the second owner.</param>
    [Theory][InlineData("")][InlineData("currency='USD' extra='X'")][InlineData("currency='USD' xml:lang='en'")]
    public void DeepAttributeFailure_PublishesNoFacts(string attributes)
    {
        var p=Provider();using var s=Session(p);using var input=NativeSchemaInput.Capture(Xml.Replace("currency='USD'",attributes,StringComparison.Ordinal)).Value!;var r=Assess(s,input);Assert.NotEqual(SchemaAssessmentStatus.Valid,r.Status);Assert.False(input.IsAssessed);Assert.Null(r.Evidence);Assert.Empty(r.Facts);
    }
    /// <summary>Authentic issued attributes from another input/authority cannot enter a foreign snapshot.</summary>
    [Fact]
    public void ForeignAndRevokedNestedAttributes_AreRefused()
    {
        var a=Provider();var b=Provider();using var sa=Session(a);using var sb=Session(b);using var ia=NativeSchemaInput.Capture(Xml).Value!;using var ib=NativeSchemaInput.Capture(Xml).Value!;var ar=Assess(sa,ia);var br=Assess(sb,ib);using var snap=sb.OpenSnapshot(ib,ib,br.Origin).Value;var issued=ar.DescendantAttributeFacts[0].ReadTypedValue(default).Value;
        new NativeSessionRunner(sb).Run([snap],op=>{Assert.Throws<ArgumentException>(()=>op.ReadTypedValue(snap,issued));return 0;});ia.Dispose();Assert.Throws<ObjectDisposedException>(()=>ar.DescendantAttributeFacts[0].ReadTypedValue(default));
    }
    /// <summary>The same owner gate protects descendant counts and reads.</summary>
    [Fact]
    public void Overlap_RefusesAndRecoversOwnerAwareReads()
    {
        using var s=NativeDecimalSessionCore.CreateMixed(Schema,new(256,256,1024,65536,1,16,1,256,256)).Value!;using var d=s.Assess(Xml,DateTimeOffset.UnixEpoch,0).Value!;
        s.Work(()=>{Assert.Throws<NotSupportedException>(()=>d.DescendantAttributeCount);Assert.Throws<NotSupportedException>(()=>d.ReadDescendantAttribute(0));return 0;},default);Assert.Equal(2,d.DescendantAttributeCount);Assert.Equal("EUR",d.ReadDescendantAttribute(0).Value.StringValue);
    }
}
