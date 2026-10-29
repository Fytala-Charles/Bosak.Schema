// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : REQ-009 S2 tests: xs:assert evaluation against candidate elements via the add-on validation service.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation. Exercises the public
//                        XsdValidationService directly (the license gate is covered in CompatibilityTests).
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S2)                                                                    |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>
/// Tests for the REQ-009 S2 assertion evaluation slice: the W3C-constrained compile
/// (XPath 2.0, captured namespace bindings, forbidden-function pre-screen) and the
/// validate-a-candidate-element semantics of <see cref="XsdValidationService"/>.
/// </summary>
public class AssertionEvaluationTests
{
    private static readonly Uri BaseUri = new("urn:test:root");

    private const string OrderSchema = """
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders" targetNamespace="urn:orders" elementFormDefault="qualified" xpathDefaultNamespace="##targetNamespace">
          <xs:element name="order">
            <xs:complexType>
              <xs:sequence>
                <xs:element name="orderedQuantity" type="xs:decimal"/>
                <xs:element name="shippedQuantity" type="xs:decimal"/>
              </xs:sequence>
              <xs:assert test="shippedQuantity le orderedQuantity"/>
              <xs:assert test="orderedQuantity gt 0"/>
            </xs:complexType>
          </xs:element>
        </xs:schema>
        """;

    [Fact]
    public void Validate_SatisfyingInstance_ReturnsValidWithoutDiagnostics()
    {
        var result = Validate(OrderSchema, """
            <t:order xmlns:t="urn:orders">
              <t:orderedQuantity>10.5</t:orderedQuantity>
              <t:shippedQuantity>2.5</t:shippedQuantity>
            </t:order>
            """);

        Assert.True(result.IsValid, string.Join(" | ", result.Diagnostics.Select(d => d.ToString())));
        Assert.Empty(result.Diagnostics);
    }

    [Fact]
    public void Validate_CompiledAssertions_CaptureNamespaceBindingsAndDefaultNamespace()
    {
        var compiled = Compile(OrderSchema);

        var assertion = Assert.Single(compiled.Assertions, a => a.TestExpression == "shippedQuantity le orderedQuantity");
        Assert.Equal("urn:orders", assertion.NamespaceBindings?["t"]);
        Assert.Equal("urn:orders", assertion.XPathDefaultNamespace);
    }

    [Fact]
    public void Validate_ViolatingInstance_ReturnsAssertionViolated()
    {
        var result = Validate(OrderSchema, """
            <t:order xmlns:t="urn:orders">
              <t:orderedQuantity>10</t:orderedQuantity>
              <t:shippedQuantity>20</t:shippedQuantity>
            </t:order>
            """);

        Assert.False(result.IsValid);
        var diagnostic = Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionViolated);
        Assert.Contains("shippedQuantity le orderedQuantity", diagnostic.Message, StringComparison.Ordinal);
        Assert.Contains("assert", diagnostic.NodePath, StringComparison.Ordinal);
    }

    [Fact]
    public void Validate_DecimalComparison_IsNumericNotLexicographic()
    {
        // 1.10 <= 1.9 is true numerically but false lexicographically.
        var result = Validate(OrderSchema, """
            <t:order xmlns:t="urn:orders">
              <t:orderedQuantity>1.9</t:orderedQuantity>
              <t:shippedQuantity>1.10</t:shippedQuantity>
            </t:order>
            """);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_PrefixedExpressionWithRootBindings_ResolvesPrefix()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders" targetNamespace="urn:orders" elementFormDefault="qualified">
              <xs:element name="order">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name="orderedQuantity" type="xs:decimal"/>
                    <xs:element name="shippedQuantity" type="xs:decimal"/>
                  </xs:sequence>
                  <xs:assert test="t:shippedQuantity le t:orderedQuantity"/>
                </xs:complexType>
              </xs:element>
            </xs:schema>
            """;

        var satisfied = Validate(schema, """
            <t:order xmlns:t="urn:orders">
              <t:orderedQuantity>10</t:orderedQuantity>
              <t:shippedQuantity>2</t:shippedQuantity>
            </t:order>
            """);
        var violated = Validate(schema, """
            <t:order xmlns:t="urn:orders">
              <t:orderedQuantity>2</t:orderedQuantity>
              <t:shippedQuantity>10</t:shippedQuantity>
            </t:order>
            """);

        Assert.True(satisfied.IsValid);
        Assert.Single(violated.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionViolated);
    }

    [Fact]
    public void Validate_MultipleAssertions_ReportsEachFailingAssertion()
    {
        var result = Validate(OrderSchema, """
            <t:order xmlns:t="urn:orders">
              <t:orderedQuantity>-1</t:orderedQuantity>
              <t:shippedQuantity>20</t:shippedQuantity>
            </t:order>
            """);

        Assert.False(result.IsValid);
        Assert.Equal(2, result.Diagnostics.Count(d => d.Code == SchemaDiagnosticCodes.AssertionViolated));
    }

    [Fact]
    public void Validate_InvalidXPath_RefusesWithAssertionRefused()
    {
        var result = Validate(AssertOnlySchema("test=\"shippedQuantity le\""), "<order><shippedQuantity>1</shippedQuantity></order>");

        Assert.False(result.IsValid);
        var diagnostic = Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionRefused);
        Assert.Contains("XPST0003", diagnostic.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Validate_ForbiddenExternalResourceFunction_RefusesWithAssertionRefused()
    {
        var result = Validate(AssertOnlySchema("test=\"fn:doc('file:///etc/passwd')/a\""), "<order/>");

        Assert.False(result.IsValid);
        Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionRefused);
    }

    [Fact]
    public void Validate_XPath31OnlyOperator_RefusedUnderXPath20Compatibility()
    {
        var result = Validate(AssertOnlySchema("test=\"'a' || 'b\""), "<order/>");

        Assert.False(result.IsValid);
        Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionRefused);
    }

    [Fact]
    public void Validate_RuntimeConversionError_ReportsAssertionEvaluationError()
    {
        var result = Validate(AssertOnlySchema("test=\"xs:integer(shippedQuantity) gt 0\""), """
            <order><shippedQuantity>abc</shippedQuantity></order>
            """);

        Assert.False(result.IsValid);
        var diagnostic = Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionEvaluationError);
        Assert.Contains("xs:integer(shippedQuantity) gt 0", diagnostic.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Validate_MissingTestAttribute_RefusesWithAssertionRefused()
    {
        var compiled = Compile("""
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:element name="order">
                <xs:complexType>
                  <xs:sequence><xs:element name="a" type="xs:string"/></xs:sequence>
                  <xs:assert/>
                </xs:complexType>
              </xs:element>
            </xs:schema>
            """);

        var result = new XsdValidationService().Validate(compiled, XElement.Parse("<order><a>x</a></order>"));

        Assert.False(result.IsValid);
        Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionRefused);
    }

    private static SchemaValidationResult Validate(string schema, string candidateXml)
    {
        var compiled = Compile(schema);
        var candidate = XElement.Parse(candidateXml);
        return new XsdValidationService().Validate(compiled, candidate);
    }

    private static ICompiledSchema Compile(string schema)
    {
        return new SchemaCompiler().Compile(SchemaComponentTests.ToStream(schema), BaseUri, SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));
    }

    private static string AssertOnlySchema(string assertAttributes) =>
        $"""
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
          <xs:element name="order">
            <xs:complexType>
              <xs:sequence><xs:element name="shippedQuantity" type="xs:string"/></xs:sequence>
              <xs:assert {assertAttributes}/>
            </xs:complexType>
          </xs:element>
        </xs:schema>
        """;
}
