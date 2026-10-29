// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : REQ-009 S4a tests: xs:alternative recording with owner/static context, CTA set diagnostics, alternative test compile checks.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation. Rules pinned to W3C
//                        XML Schema 1.1 Part 1 §3.4.2.4 (see docs/XSD11_S4_DESIGN_PREP.md); exercises the public
//                        XsdValidationService directly (the license gate is covered in CompatibilityTests).
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S4a)                                                                   |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>
/// Tests for the REQ-009 S4a slice: <c>xs:alternative</c> components are recorded with their owner
/// element declaration, captured static context (namespace bindings + <c>xpathDefaultNamespace</c>),
/// and resolved type name; malformed alternative sets emit the explicit <c>BSK-XSD-013/014</c>
/// diagnostics; <see cref="XsdValidationService.ValidateAlternativeTests"/> statically compiles every
/// non-default alternative test under the W3C-constrained XPath 2.0 environment (CTA applicability
/// evaluation itself is S4b).
/// </summary>
public class AlternativeCompileTests
{
    private static readonly Uri BaseUri = new("urn:test:root");

    private const string OrderAlternativesSchema = """
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders" targetNamespace="urn:orders" elementFormDefault="qualified" xpathDefaultNamespace="##targetNamespace">
          <xs:element name="order">
            <xs:alternative test="@kind = 'priority'" type="t:priorityOrder"/>
            <xs:alternative type="t:regularOrder"/>
          </xs:element>
        </xs:schema>
        """;

    [Fact]
    public void Compile_Alternative_RecordsOwnerStaticContextAndResolvedType()
    {
        var compiled = Compile(OrderAlternativesSchema);

        Assert.Empty(compiled.Diagnostics);
        Assert.Equal(2, compiled.Alternatives.Count);

        var priority = compiled.Alternatives[0];
        Assert.Equal("@kind = 'priority'", priority.TestExpression);
        Assert.Equal("t:priorityOrder", priority.TypeName);
        Assert.Equal(XName.Get("priorityOrder", "urn:orders"), priority.ResolvedTypeName);
        Assert.Equal("order", priority.OwnerElementName);
        Assert.Equal(XName.Get("order", "urn:orders"), priority.OwnerElementResolvedName);
        Assert.Equal("urn:orders", priority.NamespaceBindings?["t"]);
        Assert.Equal("urn:orders", priority.XPathDefaultNamespace);

        var regular = compiled.Alternatives[1];
        Assert.Null(regular.TestExpression);
        Assert.Equal(XName.Get("regularOrder", "urn:orders"), regular.ResolvedTypeName);
        Assert.Equal("order", regular.OwnerElementName);
    }

    [Fact]
    public void Compile_AlternativeUnprefixedType_ResolvesToTargetNamespace()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" targetNamespace="urn:orders">
              <xs:element name="order">
                <xs:alternative test="@kind = 'priority'" type="priorityOrder"/>
              </xs:element>
            </xs:schema>
            """;

        var compiled = Compile(schema);

        Assert.Empty(compiled.Diagnostics);
        var alternative = Assert.Single(compiled.Alternatives);
        Assert.Equal("priorityOrder", alternative.TypeName);
        Assert.Equal(XName.Get("priorityOrder", "urn:orders"), alternative.ResolvedTypeName);
    }

    [Fact]
    public void Compile_LocalOwnerElement_UnprefixedNameStaysUnqualifiedByDefault()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" targetNamespace="urn:orders">
              <xs:element name="order">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name="line">
                      <xs:alternative test="@qty gt 0" type="xs:string"/>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:schema>
            """;

        var compiled = Compile(schema);

        Assert.Empty(compiled.Diagnostics);
        var alternative = Assert.Single(compiled.Alternatives);
        Assert.Equal("line", alternative.OwnerElementName);
        Assert.Equal(XName.Get("line", string.Empty), alternative.OwnerElementResolvedName);
    }

    [Fact]
    public void Compile_AlternativeUnresolvableTypePrefix_RecordsBskXsd013()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" targetNamespace="urn:orders">
              <xs:element name="order">
                <xs:alternative test="@kind = 'priority'" type="q:missing"/>
              </xs:element>
            </xs:schema>
            """;

        var compiled = Compile(schema);

        var diagnostic = Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AlternativeTypeUnresolved);
        Assert.Contains("q:missing", diagnostic.Message, StringComparison.Ordinal);
        var alternative = Assert.Single(compiled.Alternatives);
        Assert.Equal("q:missing", alternative.TypeName);
        Assert.Null(alternative.ResolvedTypeName);
    }

    [Fact]
    public void Compile_TwoDefaultAlternatives_RecordsBskXsd014()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders" targetNamespace="urn:orders">
              <xs:element name="order">
                <xs:alternative type="t:first"/>
                <xs:alternative type="t:second"/>
              </xs:element>
            </xs:schema>
            """;

        var compiled = Compile(schema);

        var diagnostics = compiled.Diagnostics.Where(d => d.Code == SchemaDiagnosticCodes.InvalidAlternativeSet).ToList();
        Assert.Equal(2, diagnostics.Count);
        Assert.Contains(diagnostics, d => d.Message.Contains("final", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(diagnostics, d => d.Message.Contains("more than one", StringComparison.OrdinalIgnoreCase));
        Assert.Equal(2, compiled.Alternatives.Count);
    }

    [Fact]
    public void Compile_NonFinalDefaultAlternative_RecordsBskXsd014()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" targetNamespace="urn:orders">
              <xs:element name="order">
                <xs:alternative type="t:regularOrder" xmlns:t="urn:orders"/>
                <xs:alternative test="@kind = 'priority'" type="t:priorityOrder" xmlns:t="urn:orders"/>
              </xs:element>
            </xs:schema>
            """;

        var compiled = Compile(schema);

        var diagnostic = Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidAlternativeSet);
        Assert.Contains("final", diagnostic.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Compile_AlternativeUnderXsd10_RejectedWithBskXsd002()
    {
        var compiled = Compile(OrderAlternativesSchema, XsdLanguageVersion.Xsd10);

        Assert.Equal(2, compiled.Diagnostics.Count(d => d.Code == SchemaDiagnosticCodes.ConstructRequiresXsd11));
        Assert.Empty(compiled.Alternatives);
    }

    [Fact]
    public void ValidateAlternativeTests_CompilableTests_ReturnsValid()
    {
        var compiled = Compile(OrderAlternativesSchema);

        var result = new XsdValidationService().ValidateAlternativeTests(compiled);

        Assert.True(result.IsValid, string.Join(" | ", result.Diagnostics.Select(d => d.ToString())));
        Assert.Empty(result.Diagnostics);
    }

    [Fact]
    public void ValidateAlternativeTests_DefaultAlternativeOnly_SkippedAsValid()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" targetNamespace="urn:orders">
              <xs:element name="order">
                <xs:alternative type="t:regularOrder" xmlns:t="urn:orders"/>
              </xs:element>
            </xs:schema>
            """;

        var result = new XsdValidationService().ValidateAlternativeTests(Compile(schema));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateAlternativeTests_NoAlternatives_ReturnsValid()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:element name="order">
                <xs:complexType>
                  <xs:sequence><xs:element name="a" type="xs:string"/></xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:schema>
            """;

        var result = new XsdValidationService().ValidateAlternativeTests(Compile(schema));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateAlternativeTests_ForbiddenExternalResourceFunction_RefusesWithBskXsd007()
    {
        var result = CheckTests(AlternativeSchema("test=\"fn:doc('file:///etc/passwd')/a\""));

        Assert.False(result.IsValid);
        var diagnostic = Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionRefused);
        Assert.Contains("Alternative test", diagnostic.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateAlternativeTests_InvalidXPath_RefusesWithXpst0003()
    {
        var result = CheckTests(AlternativeSchema("test=\"@kind =\""));

        Assert.False(result.IsValid);
        var diagnostic = Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionRefused);
        Assert.Contains("XPST0003", diagnostic.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateAlternativeTests_CapturesStaticContextFromAlternativeLocation()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders" targetNamespace="urn:orders" xpathDefaultNamespace="##targetNamespace">
              <xs:element name="order">
                <xs:alternative test="t:kind = 'priority'" type="t:priorityOrder"/>
              </xs:element>
            </xs:schema>
            """;

        var result = new XsdValidationService().ValidateAlternativeTests(Compile(schema));

        Assert.True(result.IsValid, string.Join(" | ", result.Diagnostics.Select(d => d.ToString())));
    }

    private static SchemaValidationResult CheckTests(string schema)
    {
        return new XsdValidationService().ValidateAlternativeTests(Compile(schema));
    }

    private static ICompiledSchema Compile(string document, XsdLanguageVersion version = XsdLanguageVersion.Xsd11)
    {
        return new SchemaCompiler().Compile(SchemaComponentTests.ToStream(document), BaseUri, SchemaCompileOptions.Default(version));
    }

    private static string AlternativeSchema(string alternativeAttributes) =>
        $"""
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders" targetNamespace="urn:orders">
          <xs:element name="order">
            <xs:alternative {alternativeAttributes} type="t:priorityOrder"/>
          </xs:element>
        </xs:schema>
        """;
}
