// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 September 2026
// PURPOSE              : Alpha vertical slice: xsl:import-schema compiles and resolves through the seam without activation.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-09-2026     | Creation                                                                                 |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Xml.Linq;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Providers.Xml;
using Xunit;

namespace Bosak.Schema.Tests;

public class ImportSchemaSliceTests
{
    private const string SliceStylesheet = @"<xsl:stylesheet version='3.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform' xmlns:t='urn:fytala:bosak:schema:test'>
    <xsl:import-schema namespace='urn:fytala:bosak:schema:test' schema-location='sku.xsd'/>
    <xsl:template name='main'>
        <out><xsl:value-of select=""'ABC-1234' cast as t:sku""/></out>
    </xsl:template>
</xsl:stylesheet>";

    [Fact]
    public void ImportSchema_WithoutActivation_CompilesResolvesSchemaAndCastsUserType()
    {

        var compiler = SchemaAwareXslt.CreateCompiler(SchemaDirectory());

        var executable = compiler.Compile(SliceStylesheet, "file:///slice.xsl");
        var source = (IXdmNode)new XDocumentNode(new XDocument(new XElement("dummy")));
        var result = executable.TransformToString(source, initialTemplate: "main");

        Assert.Contains("<out", result, StringComparison.Ordinal);
        Assert.Contains("ABC-1234", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ImportSchema_OrdinaryCompilerPath_RejectedWithXtse1650()
    {
        // Schema awareness is explicit: an ordinary compiler still rejects import-schema
        // with XTSE1650, independently of the optional package or legacy activation calls.
        var ex = Assert.Throws<InvalidOperationException>(
            () => new Bosak.Xslt.Api.XsltCompiler().Compile(SliceStylesheet));

        Assert.Contains("XTSE1650", ex.Message, StringComparison.Ordinal);
    }

    private static string SchemaDirectory()
    {
        return Path.Combine(AppContext.BaseDirectory, "TestData", "Schemas");
    }
}
