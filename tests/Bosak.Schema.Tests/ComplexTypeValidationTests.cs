// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 02 October 2026
// PURPOSE              : v0.2.0 slice: complex-type validation, instance of, and treat as through the seam.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 02-10-2026     | Creation                                                                                 |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Xml.Linq;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Providers.Xml;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>
/// Proves the v0.2.0 milestone: complex-type awareness (<c>instance of</c>, <c>treat as</c>,
/// PSVI construction via <c>validation</c> / <c>@type</c>) works through the add-on's public
/// surface without activation, with spec-correct error codes on the failure paths.
/// </summary>
public class ComplexTypeValidationTests
{
    private const string OrdersSource = @"<order xmlns='urn:fytala:bosak:schema:test' id='A-1'><item>ABC-1234</item></order>";
    private const string NoteSource = @"<note xmlns='urn:fytala:bosak:schema:test'>Hello <emph>world</emph>!</note>";

    private static string SchemaDirectory()
    {
        return Path.Combine(AppContext.BaseDirectory, "TestData", "Schemas");
    }

    private static string Run(string templateBody, string sourceXml)
    {

        var stylesheet = $@"<xsl:stylesheet version='3.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform' xmlns:t='urn:fytala:bosak:schema:test' xmlns:err='http://www.w3.org/2005/xqt-errors' exclude-result-prefixes='t err'>
    <xsl:import-schema namespace='urn:fytala:bosak:schema:test' schema-location='orders.xsd'/>
    <xsl:template name='main'>{templateBody}</xsl:template>
</xsl:stylesheet>";
        var compiler = SchemaAwareXslt.CreateCompiler(SchemaDirectory());
        var executable = compiler.Compile(stylesheet, "file:///complex-type.xsl");
        var source = (IXdmNode)new XDocumentNode(XDocument.Parse(sourceXml));
        return executable.TransformToString(source, initialTemplate: "main");
    }

    [Fact]
    public void InstanceOf_ValidatedSourceNode_MatchesNamedComplexType()
    {
        var result = Run(
            @"<out><xsl:variable name='v'><xsl:copy-of select='/t:order' validation='strict'/></xsl:variable><xsl:value-of select=""$v/* instance of element(*, t:OrderType), $v/*/@id, count($v/*/t:item)"" separator='|'/></out>",
            OrdersSource);

        Assert.Contains("<out>true|A-1|1</out>", result, StringComparison.Ordinal);
    }

    [Fact]
    public void InstanceOf_PlainSourceNode_DoesNotMatchNamedComplexType()
    {
        var result = Run(
            @"<out><xsl:value-of select=""/t:order instance of element(*, t:OrderType)""/></out>",
            OrdersSource);

        Assert.Contains("<out>false</out>", result, StringComparison.Ordinal);
    }

    [Fact]
    public void TreatAs_ValidatedNode_SucceedsAndExposesTypedContent()
    {
        var result = Run(
            @"<out><xsl:variable name='v'><xsl:copy-of select='/t:order' validation='strict'/></xsl:variable><xsl:value-of select=""($v/* treat as element(*, t:OrderType))/@id""/></out>",
            OrdersSource);

        Assert.Contains("<out>A-1</out>", result, StringComparison.Ordinal);
    }

    [Fact]
    public void TreatAs_PlainNode_RaisesXpdy0050()
    {
        var result = Run(
            @"<out><xsl:try select=""/t:order treat as element(*, t:OrderType)"">
    <xsl:catch select=""$err:code""/>
</xsl:try></out>",
            OrdersSource);

        Assert.Contains("<out>err:XPDY0050</out>", result, StringComparison.Ordinal);
    }

    [Fact]
    public void TreatAs_ValidatedNodeOfOtherComplexType_RaisesXpdy0050()
    {
        var result = Run(
            @"<out><xsl:variable name='p'><xsl:copy-of select='/t:price' validation='strict'/></xsl:variable><xsl:try select=""$p/* treat as element(*, t:OrderType)"">
    <xsl:catch select=""$err:code""/>
</xsl:try></out>",
            @"<price xmlns='urn:fytala:bosak:schema:test' currency='EUR'>12.5</price>");

        Assert.Contains("<out>err:XPDY0050</out>", result, StringComparison.Ordinal);
    }

    [Fact]
    public void LiteralResultElement_ValidationStrict_ProducesPsviTypedNodeWithAccessibleTypedValue()
    {
        var result = Run(
            @"<out><xsl:variable name='p'><t:price xsl:validation='strict' currency='EUR'>12.5</t:price></xsl:variable><xsl:value-of select=""$p/* instance of element(*, t:PriceType), data($p/*)"" separator='|'/></out>",
            "<dummy/>");

        Assert.Contains("<out>true|12.5</out>", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ConstructedElement_TypeAttribute_ValidatesAgainstNamedComplexType()
    {
        var result = Run(
            @"<out><xsl:variable name='p'><xsl:element name='t:price' type='t:PriceType'><xsl:attribute name='currency' select=""'EUR'""/>12.5</xsl:element></xsl:variable><xsl:value-of select=""$p/* instance of element(*, t:PriceType), data($p/*) + 1"" separator='|'/></out>",
            "<dummy/>");

        // data() is xs:decimal (the type's simple content), so +1 yields 13.5 in decimal arithmetic.
        Assert.Contains("<out>true|13.5</out>", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ConstructedElement_TypeAttribute_ContentViolatingType_RaisesXtte1540()
    {
        var result = Run(
            @"<out><xsl:try><xsl:element name='t:order' type='t:OrderType'><t:item>ABC-1234</t:item></xsl:element><xsl:catch select=""$err:code""/></xsl:try></out>",
            "<dummy/>");

        Assert.Contains("<out>err:XTTE1540</out>", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ConstructedElement_TypeAttribute_ContentViolatingType_ThrowsXsltRuntimeExceptionWithCode()
    {
        const string body = @"<out><xsl:element name='t:order' type='t:OrderType'><t:item>ABC-1234</t:item></xsl:element></out>";

        var ex = Record.Exception(() => Run(body, "<dummy/>"));

        Assert.NotNull(ex);
        Assert.Contains("XTTE1540", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void StrictCopyOf_SourceViolatingComplexType_RaisesXtte1510()
    {
        var result = Run(
            @"<out><xsl:try><xsl:copy-of select='/t:order' validation='strict'/><xsl:catch select=""$err:code""/></xsl:try></out>",
            @"<order xmlns='urn:fytala:bosak:schema:test'><item>ABC-1234</item></order>");

        Assert.Contains("<out>err:XTTE1510</out>", result, StringComparison.Ordinal);
    }

    [Fact]
    public void DataOn_ValidatedElementOnlyContent_RaisesFoty0012()
    {
        var result = Run(
            @"<out><xsl:variable name='v'><xsl:copy-of select='/t:order' validation='strict'/></xsl:variable><xsl:try select=""data($v/*)"">
    <xsl:catch select=""$err:code""/>
</xsl:try></out>",
            OrdersSource);

        Assert.Contains("<out>err:FOTY0012</out>", result, StringComparison.Ordinal);
    }

    [Fact]
    public void InstanceOf_ComplexWithSimpleContentTypedValue_MatchesAndSuppliesDecimalValue()
    {
        var result = Run(
            @"<out><xsl:variable name='p'><t:price xsl:validation='strict' currency='EUR'>12.5</t:price></xsl:variable><xsl:value-of select=""$p/* instance of element(*, t:PriceType), $p/* instance of element(t:price, t:PriceType)"" separator='|'/></out>",
            "<dummy/>");

        Assert.Contains("<out>true|true</out>", result, StringComparison.Ordinal);
    }

    [Fact]
    public void InstanceOf_ValidatedMixedContentNode_MatchesNamedComplexType()
    {
        var result = Run(
            @"<out><xsl:variable name='v'><xsl:copy-of select='/t:note' validation='strict'/></xsl:variable><xsl:value-of select=""$v/* instance of element(*, t:NoteType), string($v/*)"" separator='|'/></out>",
            NoteSource);

        Assert.Contains("<out>true|Hello world!</out>", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ComplexTypeStylesheet_OrdinaryCompilerPath_RejectedWithXtse1650()
    {
        // Deliberately no activation: the same schema-aware stylesheet through an unactivated
        // path must hit the free core's XTSE1650 gate, proving nothing leaks past the license boundary.
        const string stylesheet = @"<xsl:stylesheet version='3.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform' xmlns:t='urn:fytala:bosak:schema:test'>
    <xsl:import-schema namespace='urn:fytala:bosak:schema:test' schema-location='orders.xsd'/>
    <xsl:template name='main'>
        <out><xsl:value-of select=""'A-1'""/></out>
    </xsl:template>
</xsl:stylesheet>";

        var ex = Assert.Throws<InvalidOperationException>(
            () => new Bosak.Xslt.Api.XsltCompiler().Compile(stylesheet));

        Assert.Contains("XTSE1650", ex.Message, StringComparison.Ordinal);
    }
}
