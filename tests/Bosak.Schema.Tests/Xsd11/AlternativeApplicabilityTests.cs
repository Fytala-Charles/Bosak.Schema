// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : REQ-009 S4b tests: CTA governing-type computation, assertion applicability filtering, typed view v2.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation. Semantics pinned to W3C
//                        XML Schema 1.1 Part 1 §3.4.2.4/§3.4.6 (see docs/XSD11_S4_DESIGN_PREP.md §4); exercises the
//                        public XsdValidationService directly (the license gate is covered in CompatibilityTests).
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S4b)                                                                   |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>
/// Tests for the REQ-009 S4b slice: governing-type computation (<c>xsi:type</c> pre-emption, then
/// <c>xs:alternative</c> tests in document order, then the declared type), assertion applicability
/// filtering by owning named type or anonymous owner element (<c>BSK-XSD-015</c> when no alternative
/// matches and no default exists), and the typed view v2 (user-defined simple types resolved through
/// their recorded restriction chains to the built-in base).
/// </summary>
public class AlternativeApplicabilityTests
{
    private static readonly Uri BaseUri = new("urn:test:root");

    private const string CtaSchema = """
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders" targetNamespace="urn:orders" elementFormDefault="qualified" xpathDefaultNamespace="##targetNamespace">
          <xs:element name="order">
            <xs:alternative test="@kind = 'priority'" type="t:priorityOrder"/>
            <xs:alternative type="t:regularOrder"/>
          </xs:element>
          <xs:complexType name="priorityOrder">
            <xs:sequence><xs:element name="amount" type="xs:decimal"/></xs:sequence>
            <xs:assert test="@kind = 'priority'"/>
          </xs:complexType>
          <xs:complexType name="regularOrder">
            <xs:sequence><xs:element name="amount" type="xs:decimal"/></xs:sequence>
            <xs:assert test="@kind = 'regular'"/>
          </xs:complexType>
        </xs:schema>
        """;

    [Fact]
    public void Validate_CtaFirstMatch_SelectsOwningTypeAndRunsOnlyItsAssertions()
    {
        var result = Validate(CtaSchema, """
            <t:order xmlns:t="urn:orders" kind="priority">
              <t:amount>10</t:amount>
            </t:order>
            """);

        Assert.True(result.IsValid, string.Join(" | ", result.Diagnostics.Select(d => d.ToString())));
        Assert.Empty(result.Diagnostics);
    }

    [Fact]
    public void Validate_CtaDefaultAlternative_RunsDefaultTypeAssertions()
    {
        // kind "express" matches no test; the default alternative selects regularOrder whose
        // assertion requires kind = 'regular' — the violation proves the default type governed.
        var result = Validate(CtaSchema, """
            <t:order xmlns:t="urn:orders" kind="express">
              <t:amount>10</t:amount>
            </t:order>
            """);

        Assert.False(result.IsValid);
        Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionViolated);
    }

    [Fact]
    public void Validate_NoAlternativeMatchWithoutDefault_ReportsBskXsd015()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders" targetNamespace="urn:orders">
              <xs:element name="order">
                <xs:alternative test="@kind = 'priority'" type="t:priorityOrder"/>
              </xs:element>
              <xs:complexType name="priorityOrder">
                <xs:sequence><xs:element name="amount" type="xs:decimal"/></xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var result = Validate(schema, """
            <t:order xmlns:t="urn:orders" kind="express">
              <t:amount>10</t:amount>
            </t:order>
            """);

        Assert.False(result.IsValid);
        Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AlternativeNotApplicable);
    }

    [Fact]
    public void Validate_XsiTypePreemptsCta_GoverningTypeFromInstance()
    {
        // xsi:type="t:regularOrder" pre-empts the alternatives: kind="priority" violates the
        // regularOrder assertion even though the priority alternative test matches.
        var result = Validate(CtaSchema, """
            <t:order xmlns:t="urn:orders" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:type="t:regularOrder" kind="priority">
              <t:amount>10</t:amount>
            </t:order>
            """);

        Assert.False(result.IsValid);
        Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionViolated);
    }

    [Fact]
    public void Validate_XsiTypeUnprefixed_ResolvesToNoNamespace()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:element name="order" type="orderType"/>
              <xs:complexType name="orderType">
                <xs:sequence><xs:element name="amount" type="xs:decimal"/></xs:sequence>
                <xs:assert test="true()"/>
              </xs:complexType>
            </xs:schema>
            """;

        // An unprefixed xsi:type names a no-namespace type: it governs but matches no recorded
        // named-type assertion owner, so nothing applies and the result is valid.
        var result = Validate(schema, """
            <order xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:type="orderType">
              <amount>10</amount>
            </order>
            """);

        Assert.True(result.IsValid, string.Join(" | ", result.Diagnostics.Select(d => d.ToString())));
    }

    [Fact]
    public void Validate_NamedDeclaredType_RunsOnlyOwningTypeAssertions()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders" targetNamespace="urn:orders">
              <xs:element name="order" type="t:orderType"/>
              <xs:complexType name="orderType">
                <xs:sequence><xs:element name="amount" type="xs:decimal"/></xs:sequence>
                <xs:assert test="true()"/>
              </xs:complexType>
              <xs:complexType name="otherType">
                <xs:sequence><xs:element name="amount" type="xs:decimal"/></xs:sequence>
                <xs:assert test="false()"/>
              </xs:complexType>
            </xs:schema>
            """;

        // otherType's assertion would always fail; validity proves only orderType's assertion ran.
        var result = Validate(schema, """
            <t:order xmlns:t="urn:orders">
              <t:amount>10</t:amount>
            </t:order>
            """);

        Assert.True(result.IsValid, string.Join(" | ", result.Diagnostics.Select(d => d.ToString())));
    }

    [Fact]
    public void Validate_UnrelatedCandidate_RunsNoNamedTypeAssertions()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders" targetNamespace="urn:orders">
              <xs:element name="order" type="t:orderType"/>
              <xs:complexType name="orderType">
                <xs:sequence><xs:element name="amount" type="xs:decimal"/></xs:sequence>
                <xs:assert test="false()"/>
              </xs:complexType>
            </xs:schema>
            """;

        var result = Validate(schema, """
            <t:invoice xmlns:t="urn:orders">
              <t:amount>10</t:amount>
            </t:invoice>
            """);

        Assert.True(result.IsValid, string.Join(" | ", result.Diagnostics.Select(d => d.ToString())));
    }

    [Fact]
    public void Validate_AnonymousInlineType_AssertionMatchesByOwnerElement()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:element name="order">
                <xs:complexType>
                  <xs:sequence><xs:element name="amount" type="xs:string"/></xs:sequence>
                  <xs:assert test="@ok = 'true'"/>
                </xs:complexType>
              </xs:element>
            </xs:schema>
            """;

        var valid = Validate(schema, """<order ok="true"><amount>x</amount></order>""");
        var violated = Validate(schema, """<order ok="false"><amount>x</amount></order>""");

        Assert.True(valid.IsValid, string.Join(" | ", valid.Diagnostics.Select(d => d.ToString())));
        Assert.Single(violated.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionViolated);
    }

    [Fact]
    public void Validate_CtaEvaluationError_ReportsBskXsd008AndSkipsAssertions()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders" targetNamespace="urn:orders">
              <xs:element name="order">
                <xs:alternative test="xs:integer(@kind) gt 0" type="t:orderType"/>
                <xs:alternative type="t:orderType"/>
              </xs:element>
              <xs:complexType name="orderType">
                <xs:sequence><xs:element name="amount" type="xs:decimal"/></xs:sequence>
                <xs:assert test="false()"/>
              </xs:complexType>
            </xs:schema>
            """;

        var result = Validate(schema, """
            <t:order xmlns:t="urn:orders" kind="not-a-number">
              <t:amount>10</t:amount>
            </t:order>
            """);

        Assert.False(result.IsValid);
        var diagnostic = Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionEvaluationError);
        Assert.Contains("Alternative test", diagnostic.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Validate_UserDefinedSimpleType_TypedViewResolvesRestrictionChain()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders" targetNamespace="urn:orders" elementFormDefault="qualified" xpathDefaultNamespace="##targetNamespace">
              <xs:simpleType name="qty">
                <xs:restriction base="xs:decimal"/>
              </xs:simpleType>
              <xs:element name="order">
                <xs:complexType>
                  <xs:sequence><xs:element name="qty" type="t:qty"/></xs:sequence>
                  <xs:assert test="qty gt 9.9"/>
                </xs:complexType>
              </xs:element>
            </xs:schema>
            """;

        // 10.5 > 9.9 is true numerically but false lexicographically ("1" < "9").
        var typed = Validate(schema, """
            <t:order xmlns:t="urn:orders">
              <t:qty>10.5</t:qty>
            </t:order>
            """);
        var violated = Validate(schema, """
            <t:order xmlns:t="urn:orders">
              <t:qty>0.5</t:qty>
            </t:order>
            """);

        Assert.True(typed.IsValid, string.Join(" | ", typed.Diagnostics.Select(d => d.ToString())));
        Assert.Single(violated.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionViolated);
    }

    [Fact]
    public void Validate_UserDefinedSimpleTypeChain_MultipleHopsToBuiltInBase()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders" targetNamespace="urn:orders" elementFormDefault="qualified" xpathDefaultNamespace="##targetNamespace">
              <xs:simpleType name="qty">
                <xs:restriction base="t:baseQty"/>
              </xs:simpleType>
              <xs:simpleType name="baseQty">
                <xs:restriction base="xs:decimal"/>
              </xs:simpleType>
              <xs:element name="order">
                <xs:complexType>
                  <xs:sequence><xs:element name="qty" type="t:qty"/></xs:sequence>
                  <xs:assert test="qty gt 9.9"/>
                </xs:complexType>
              </xs:element>
            </xs:schema>
            """;

        var typed = Validate(schema, """
            <t:order xmlns:t="urn:orders">
              <t:qty>10.5</t:qty>
            </t:order>
            """);

        Assert.True(typed.IsValid, string.Join(" | ", typed.Diagnostics.Select(d => d.ToString())));
    }

    private static SchemaValidationResult Validate(string schema, string candidateXml)
    {
        var compiled = new SchemaCompiler().Compile(SchemaComponentTests.ToStream(schema), BaseUri, SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));
        return new XsdValidationService().Validate(compiled, XElement.Parse(candidateXml));
    }
}
