// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : REQ-009 S3 tests: xs:assertion facet evaluation against named simple types.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation. Exercises the
//                        public XsdValidationService.ValidateValue directly (the license gate is covered
//                        in CompatibilityTests).
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S3)                                                                    |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>
/// Tests for the REQ-009 S3 <c>xs:assertion</c> slice: facet evaluation against named simple
/// types with the typed value as context item and through <c>$value</c>.
/// </summary>
public class AssertionFacetTests
{
    private static readonly Uri BaseUri = new("urn:test:root");

    private const string TypesSchema = """
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:types" targetNamespace="urn:types">
          <xs:simpleType name="sku">
            <xs:restriction base="xs:string">
              <xs:assertion test="matches($value, '[A-Z]{3}-[0-9]{4}')"/>
            </xs:restriction>
          </xs:simpleType>
          <xs:simpleType name="positiveCount">
            <xs:restriction base="xs:integer">
              <xs:assertion test="$value gt 0"/>
            </xs:restriction>
          </xs:simpleType>
          <xs:simpleType name="smallCount">
            <xs:restriction base="t:positiveCount">
              <xs:assertion test="$value le 10"/>
            </xs:restriction>
          </xs:simpleType>
        </xs:schema>
        """;

    [Fact]
    public void ValidateValue_MatchingValue_ReturnsValid()
    {
        var result = ValidateValue(TypesSchema, "sku", "ABC-1234");

        Assert.True(result.IsValid, string.Join(" | ", result.Diagnostics.Select(d => d.ToString())));
    }

    [Fact]
    public void ValidateValue_ViolatingValue_ReturnsAssertionViolated()
    {
        var result = ValidateValue(TypesSchema, "sku", "abc-12");

        Assert.False(result.IsValid);
        var diagnostic = Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionViolated);
        Assert.Contains("matches($value", diagnostic.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateValue_NumericBase_ParsesValuePerBuiltInBase()
    {
        // $value is typed as xs:integer via the restriction base, so the comparison is numeric.
        var negative = ValidateValue(TypesSchema, "positiveCount", "-3");
        var positive = ValidateValue(TypesSchema, "positiveCount", "42");

        Assert.False(negative.IsValid);
        Assert.Single(negative.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionViolated);
        Assert.True(positive.IsValid);
    }

    [Fact]
    public void ValidateValue_UserTypeBaseChain_ResolvesThroughIntermediateType()
    {
        // t:smallCount restricts t:positiveCount (which restricts xs:integer): both facets apply,
        // and the value is still parsed as xs:integer through the chain.
        var withinBounds = ValidateValue(TypesSchema, "smallCount", "7");
        var overBound = ValidateValue(TypesSchema, "smallCount", "11");
        var negative = ValidateValue(TypesSchema, "smallCount", "-2");

        Assert.True(withinBounds.IsValid, string.Join(" | ", withinBounds.Diagnostics.Select(d => d.ToString())));
        Assert.Single(overBound.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionViolated);
        Assert.Single(negative.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionViolated);
    }

    [Fact]
    public void ValidateValue_ContextItemIsTheTypedValue()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:types" targetNamespace="urn:types">
              <xs:simpleType name="len2">
                <xs:restriction base="xs:string">
                  <xs:assertion test="string-length(.) ge 2"/>
                </xs:restriction>
              </xs:simpleType>
            </xs:schema>
            """;

        var shortValue = ValidateValue(schema, "len2", "x");
        var longValue = ValidateValue(schema, "len2", "xyz");

        Assert.False(shortValue.IsValid);
        Assert.True(longValue.IsValid);
    }

    [Fact]
    public void ValidateValue_UnknownSimpleType_ReturnsSimpleTypeNotFound()
    {
        var compiled = Compile(TypesSchema);
        var result = new XsdValidationService().ValidateValue(compiled, XName.Get("missing", "urn:types"), "x");

        Assert.False(result.IsValid);
        var diagnostic = Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.SimpleTypeNotFound);
        Assert.Contains("missing", diagnostic.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateValue_RefusedAndErroringFacets_ProduceExplicitDiagnostics()
    {
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:simpleType name="bad">
                <xs:restriction base="xs:string">
                  <xs:assertion test="doc('file:///etc/passwd') ne ''"/>
                  <xs:assertion test="$value gt"/>
                  <xs:assertion test="xs:integer($value) gt 0"/>
                </xs:restriction>
              </xs:simpleType>
            </xs:schema>
            """;

        var compiled = Compile(schema);
        var result = new XsdValidationService().ValidateValue(compiled, XName.Get("bad"), "abc");

        Assert.False(result.IsValid);
        Assert.Equal(2, result.Diagnostics.Count(d => d.Code == SchemaDiagnosticCodes.AssertionRefused));
        Assert.Single(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionEvaluationError);
    }

    [Fact]
    public void Compile_RecordsSimpleTypeDeclarationsWithOwnedFacets()
    {
        var compiled = Compile(TypesSchema);

        var sku = Assert.Single(compiled.SimpleTypes, t => t.TypeLocalName == "sku");
        Assert.Equal("urn:types", sku.TypeNamespace);
        Assert.Equal("http://www.w3.org/2001/XMLSchema", sku.BaseTypeNamespace);
        Assert.Equal("string", sku.BaseTypeLocalName);
        var facet = Assert.Single(sku.Assertions);
        Assert.Equal(AssertionContext.SimpleTypeAssertion, facet.Context);
        Assert.Contains("simpleType", facet.NodePath, StringComparison.Ordinal);

        // Facets are shared instances in the top-level assertion list.
        Assert.Contains(compiled.Assertions, a => ReferenceEquals(a, facet));
    }

    [Fact]
    public void Validate_ElementCandidate_IgnoresSimpleTypeFacets()
    {
        // A schema with only assertion facets validates an element candidate as valid (nothing applies).
        var compiled = Compile(TypesSchema);
        var result = new XsdValidationService().Validate(compiled, XElement.Parse("<any/>"));

        Assert.True(result.IsValid, string.Join(" | ", result.Diagnostics.Select(d => d.ToString())));
    }

    private static SchemaValidationResult ValidateValue(string schema, string typeLocalName, string lexicalValue)
    {
        var compiled = Compile(schema);
        return new XsdValidationService().ValidateValue(compiled, XName.Get(typeLocalName, "urn:types"), lexicalValue);
    }

    private static ICompiledSchema Compile(string schema)
    {
        return new SchemaCompiler().Compile(SchemaComponentTests.ToStream(schema), BaseUri, SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));
    }
}
