// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 29 October 2026
// PURPOSE              : Qualifies original whole-document component state, atomization, copies and explicit refusal.
// SPECIAL NOTES        : Bounded native XSD 1.1 assessment; decimal-sequence-assertions-v1 profile.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 29-10-2026     | Creation (REQ-009 bounded document assessment, 0.5 development line)                      |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Xml.Schema;
using Bosak.XPath.Api;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Providers.Xml;
using Bosak.XPath.Runtime.Vm;
using Bosak.XQuery.Api;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Tests the real native backend's optional 1.34 XML-provider bridge.</summary>
public sealed class WholeDocumentBridgeTests
{
    private const string Schema = """
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
          <xs:element name="order"><xs:complexType><xs:sequence>
            <xs:element name="amount" type="xs:decimal"/><xs:element name="shipped" type="xs:decimal"/>
          </xs:sequence><xs:assert test="amount gt 0 and shipped le amount"/></xs:complexType></xs:element>
        </xs:schema>
        """;
    private static AssessedDocument Assess() => SchemaServices.CreateDocumentAssessor(XDocument.Parse(Schema))
        .Assess(XDocument.Parse("<order><amount>12.50</amount><shipped>2</shipped></order>")).Document!;
    private static EvaluationContext Context(XDocument xml) => new EvaluationContext().WithFocus(XdmValue.FromNode(new XDocumentNode(xml)),1,1);
    private static XdmValue Query(XDocument xml,string text) => XPath31Expression.Compile(text).Evaluate(Context(xml));

    /// <summary>Whole-document queries read actual decimals while retaining the original anonymous root state.</summary>
    [Fact]
    public void WholeDocument_ExposesOriginalComponentsAndCorrectTypes()
    {
        var assessed=Assess();var xml=assessed.CreateDocumentCopy(true);var root=new XDocumentNode(xml.Root!);
        Assert.Same(assessed,root.ElementAssessment!.Owner);
        Assert.Same(assessed.Elements[0],root.ElementAssessment.State);
        Assert.True(root.HasNoTypedValue);Assert.True(root.IsComplexType);
        Assert.Equal((string.Empty,string.Empty),root.SchemaTypeAnnotation);
        Assert.True(Query(xml,"/order instance of element(*,xs:anyType)").BooleanValue);
        Assert.False(Query(xml,"/order instance of element(*,xs:untyped)").BooleanValue);
        Assert.True(Query(xml,"data(/order/amount) instance of xs:decimal").BooleanValue);
        Assert.Equal(15m,Query(xml,"data(/order/amount) * 1.2").DecimalValue);
        Assert.Same(assessed.Elements[1],XdmElementAssessment.Get(xml.Root!.Element("amount")!)!.State);
    }

    /// <summary>Element-only root atomization fails through functions, comparisons and direct node access.</summary>
    /// <param name="expression">An atomizing operation.</param>
    [Theory]
    [InlineData("data(/order)")]
    [InlineData("/order = '12.502'")]
    [InlineData("number(/order)")]
    public void RootAtomization_RaisesFoty0012(string expression)
    {
        var xml=Assess().CreateDocumentCopy(true);
        var error=Assert.ThrowsAny<Exception>(()=>Query(xml,expression));Assert.Contains("FOTY0012",error.ToString());
        Assert.Contains("FOTY0012",Assert.Throws<InvalidOperationException>(()=>new XDocumentNode(xml.Root!).TypedValue).Message);
    }

    /// <summary>Real XSLT preserve copies carry original state and strip copies are untyped.</summary>
    [Fact]
    public void XsltPreserveAndStrip_RetainOrRemoveRootAndDescendantAssessment()
    {
        var assessed=Assess();var input=assessed.CreateDocumentCopy(true);
        const string style="""
            <xsl:stylesheet version="3.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:err="http://www.w3.org/2005/xqt-errors" exclude-result-prefixes="xs err">
              <xsl:template name="main">
                <xsl:variable name="p" as="element()"><xsl:copy-of select="/order" validation="preserve"/></xsl:variable>
                <xsl:variable name="s" as="element()"><xsl:copy-of select="/order" validation="strip"/></xsl:variable>
                <result typed="{data($p/amount) instance of xs:decimal}" stripped="{data($s/amount) instance of xs:untypedAtomic}" root-stripped="{$s instance of element(*,xs:untyped)}">
                  <xsl:try><xsl:value-of select="$p"/><xsl:catch><error code="{$err:code}"/></xsl:catch></xsl:try>
                </result>
              </xsl:template>
            </xsl:stylesheet>
            """;
        var compiler=SchemaAwareXslt.CreateCompiler();compiler.SchemaSet=new XmlSchemaSet();
        var result=XElement.Parse(compiler.Compile(style).TransformToString(new XDocumentNode(input),initialTemplate:"main"));
        Assert.Equal("true",(string?)result.Attribute("typed"));Assert.Equal("true",(string?)result.Attribute("stripped"));
        Assert.Equal("true",(string?)result.Attribute("root-stripped"));Assert.Contains("FOTY0012",(string?)result.Element("error")!.Attribute("code"));
        Assert.Same(assessed,XdmElementAssessment.Get(input.Root!)!.Owner);
    }

    /// <summary>Legacy structural reassessment cannot silently downgrade owned native coverage.</summary>
    [Fact]
    public void LegacyReassessmentAndDeclarationProjection_RefuseExplicitly()
    {
        var input=Assess().CreateDocumentCopy(true);var root=input.Root!;
        Assert.Throws<NotSupportedException>(()=>XdmSchemaAnnotator.ValidateSubtree(root,new XmlSchemaSet()));
        Assert.Throws<NotSupportedException>(()=>new XDocumentNode(root).SchemaElementDeclaration);
    }

    /// <summary>XML edits cannot reuse cached typed values or the old root certificate.</summary>
    [Fact]
    public void EditingExport_RefusesStaleValuesButLeavesNativeSnapshotIntact()
    {
        var assessed=Assess();var edited=assessed.CreateDocumentCopy(true);edited.Root!.Element("amount")!.Value="99";
        Assert.Throws<NotSupportedException>(()=>Query(edited,"data(/order/amount)"));
        Assert.Throws<NotSupportedException>(()=>new XDocumentNode(edited.Root).HasNoTypedValue);
        Assert.Equal(12.5m,Query(assessed.CreateDocumentCopy(true),"data(/order/amount)").DecimalValue);
        XdmSchemaAnnotator.StripSchemaAnnotations(edited);
        Assert.True(Query(edited,"data(/order/amount) instance of xs:untypedAtomic").BooleanValue);
    }

    /// <summary>Namespace fixup does not invent new component authority or invalidate this namespace-free profile.</summary>
    [Fact]
    public void NamespaceFixupAndWhitespace_KeepTheSameOriginalState()
    {
        var assessed=Assess();var xml=assessed.CreateDocumentCopy(true);
        xml.Root!.SetAttributeValue(XNamespace.Xmlns+"unused","urn:unused");xml.Root.AddFirst(new XText("  "));
        Assert.Same(assessed,XdmElementAssessment.Get(xml.Root)!.Owner);
        Assert.Equal(12.5m,Query(xml,"data(/order/amount)").DecimalValue);
    }

    /// <summary>Node identity and independent snapshots retain the existing provider's identity rules.</summary>
    [Fact]
    public void IdentityAndIndependentExports_AreNotConflatedByEqualXml()
    {
        var assessed=Assess();var first=assessed.CreateDocumentCopy(true);var second=assessed.CreateDocumentCopy(true);
        Assert.True(new XDocumentNode(first.Root!).IsSameNode(new XDocumentNode(first.Root!)));
        Assert.False(new XDocumentNode(first.Root!).IsSameNode(new XDocumentNode(second.Root!)));
        Assert.Same(XdmElementAssessment.Get(first.Root!)!.State,XdmElementAssessment.Get(second.Root!)!.State);
    }

    /// <summary>An escaped XML tree keeps its original owner/state alive after the host drops its reference.</summary>
    [Fact]
    public void EscapedTree_RetainsItsOriginalAssessmentOwner()
    {
        var (tree,owner)=Export();GC.Collect();GC.WaitForPendingFinalizers();GC.Collect();
        Assert.True(owner.IsAlive);Assert.NotNull(XdmElementAssessment.Get(tree.Root!)!.Owner);
        Assert.Equal(12.5m,Query(tree,"data(/order/amount)").DecimalValue);
    }

    /// <summary>XQuery construction mode preserves original components or strips them on private destinations.</summary>
    /// <param name="preserve">The requested construction mode.</param>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void XQueryConstructors_CopyOriginalStateWithoutRevalidation(bool preserve)
    {
        var assessed=Assess();var input=assessed.CreateDocumentCopy(true);
        var source=new XDocumentNode(input.Root!);
        var text="declare construction "+(preserve?"preserve":"strip")+"; declare variable $input external; <copy>{$input}</copy>/*";
        var value=new XQueryCompiler().Compile(text).Evaluate(new XQueryContext().WithVariable("input",XdmValue.FromNode(source)));
        IXdmNode? node=value.IsNode?value.NodeValue:null;
        if(node is null)foreach(var item in XdmSequence.FromSource(value.SequenceValue!)){node=item.NodeValue;break;}
        var result=Assert.IsType<XDocumentNode>(node);
        Assert.Equal(preserve,result.HasNoTypedValue);
        Assert.False(source.IsSameNode(result));
        if(preserve){Assert.Same(assessed,result.ElementAssessment!.Owner);Assert.Same(assessed.Elements[0],result.ElementAssessment.State);}
        else Assert.Null(result.ElementAssessment);
        Assert.Same(assessed,XdmElementAssessment.Get(input.Root!)!.Owner);
    }

    /// <summary>Changed document-level language constraints cannot reuse otherwise unchanged root metadata.</summary>
    [Fact]
    public void AddedDtdAndOversizedEditedContent_AreExplicitlyRefused()
    {
        var dtd=Assess().CreateDocumentCopy(true);dtd.AddFirst(new XDocumentType("order",null,null,null));
        Assert.Throws<NotSupportedException>(()=>Query(dtd,"data(/order/amount)"));
        var huge=Assess().CreateDocumentCopy(true);huge.Root!.Element("amount")!.Value=new string('9',70000);
        Assert.Throws<NotSupportedException>(()=>Query(huge,"data(/order/amount)"));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (XDocument Tree,WeakReference Owner) Export(){var owner=Assess();return(owner.CreateDocumentCopy(true),new WeakReference(owner));}
}
