// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : REQ-009 S1 component tests: compiled model, digest stability, version-gated diagnostics.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S1)                                                                    |
//                      | Charles Korthout | 0.2   | 27-10-2026     | OpenContent Xsd11 test updated: xs:openContent is now recorded, not unsupported (S5a)   |

//                      | Charles Korthout | 0.3   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>
/// Component tests for the REQ-009 S1 compile pipeline: the immutable compiled model,
/// the stable interpretation digest, and the explicit version-gated diagnostics.
/// </summary>
public class SchemaComponentTests
{
    private static readonly Uri BaseUri = new("urn:test:root");

    private const string Xsd10Document = """
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" targetNamespace="urn:test">
          <xs:element name="a" type="xs:string"/>
        </xs:schema>
        """;

    private const string Xsd11Document = """
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" targetNamespace="urn:test" xmlns:t="urn:test">
          <xs:element name="order">
            <xs:alternative test="@kind = 'priority'" type="t:priorityOrder"/>
            <xs:alternative type="t:regularOrder"/>
          </xs:element>
          <xs:complexType name="priorityOrder">
            <xs:sequence>
              <xs:element name="total" type="xs:decimal"/>
            </xs:sequence>
            <xs:assert test="total gt 0"/>
          </xs:complexType>
          <xs:simpleType name="sku">
            <xs:restriction base="xs:string">
              <xs:assertion test="matches($value, '[A-Z]{3}-[0-9]{4}')"/>
            </xs:restriction>
          </xs:simpleType>
        </xs:schema>
        """;

    [Fact]
    public void Compile_Xsd10Document_ProducesXsd10ModelWithoutDiagnostics()
    {
        var compiled = new SchemaCompiler().Compile(ToStream(Xsd10Document), BaseUri, SchemaCompileOptions.Default(XsdLanguageVersion.Xsd10));

        Assert.Equal(XsdLanguageVersion.Xsd10, compiled.LanguageVersion);
        Assert.Empty(compiled.Diagnostics);
        Assert.Empty(((CompiledSchema)compiled).Assertions);
        Assert.Empty(((CompiledSchema)compiled).Alternatives);
        Assert.Equal("urn:test", ((CompiledSchema)compiled).TargetNamespace);
    }

    [Fact]
    public void Compile_AnyDocument_ProducesStableSchemaIdAndDigest()
    {
        var compiled = new SchemaCompiler().Compile(ToStream(Xsd10Document), BaseUri, SchemaCompileOptions.Default(XsdLanguageVersion.Xsd10));

        Assert.StartsWith("xsd-", compiled.SchemaId, StringComparison.Ordinal);
        Assert.Equal(20, compiled.SchemaId.Length);
        Assert.Equal(64, compiled.Sha256Digest.Length);
        Assert.Equal(compiled.Sha256Digest[..16], compiled.SchemaId["xsd-".Length..]);
    }

    [Fact]
    public void Compile_SameDocumentDifferentFormatting_ProducesIdenticalDigest()
    {
        const string indented = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" targetNamespace="urn:test">
                <xs:element name="a" type="xs:string"/>
            </xs:schema>
            """;
        var compiler = new SchemaCompiler();
        var compact = compiler.Compile(ToStream(Xsd10Document), BaseUri, SchemaCompileOptions.Default(XsdLanguageVersion.Xsd10));
        var spaced = compiler.Compile(ToStream(indented), BaseUri, SchemaCompileOptions.Default(XsdLanguageVersion.Xsd10));

        Assert.Equal(compact.Sha256Digest, spaced.Sha256Digest);
        Assert.Equal(compact.SchemaId, spaced.SchemaId);
    }

    [Fact]
    public void Compile_Xsd11DocumentInXsd11Mode_RecordsAssertionAndAlternativeComponents()
    {
        var compiled = (CompiledSchema)new SchemaCompiler().Compile(ToStream(Xsd11Document), BaseUri, SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));

        Assert.Equal(XsdLanguageVersion.Xsd11, compiled.LanguageVersion);
        Assert.Empty(compiled.Diagnostics);

        var complexAssert = Assert.Single(compiled.Assertions, a => a.Context == AssertionContext.ComplexTypeAssert);
        Assert.Equal("total gt 0", complexAssert.TestExpression);
        Assert.Contains("assert", complexAssert.NodePath, StringComparison.Ordinal);

        var simpleAssertion = Assert.Single(compiled.Assertions, a => a.Context == AssertionContext.SimpleTypeAssertion);
        Assert.Contains("matches($value", simpleAssertion.TestExpression, StringComparison.Ordinal);

        Assert.Equal(2, compiled.Alternatives.Count);
        Assert.Equal("@kind = 'priority'", compiled.Alternatives[0].TestExpression);
        Assert.Equal("t:priorityOrder", compiled.Alternatives[0].TypeName);
        Assert.Null(compiled.Alternatives[1].TestExpression);
        Assert.Equal("t:regularOrder", compiled.Alternatives[1].TypeName);
    }

    [Fact]
    public void Compile_Xsd11DocumentInXsd10Mode_EmitsRequiresXsd11DiagnosticsAndRecordsNothing()
    {
        var compiled = (CompiledSchema)new SchemaCompiler().Compile(ToStream(Xsd11Document), BaseUri, SchemaCompileOptions.Default(XsdLanguageVersion.Xsd10));

        Assert.Equal(XsdLanguageVersion.Xsd10, compiled.LanguageVersion);
        Assert.Empty(compiled.Assertions);
        Assert.Empty(compiled.Alternatives);
        Assert.Equal(4, compiled.Diagnostics.Count);
        Assert.All(compiled.Diagnostics, d => Assert.Equal(SchemaDiagnosticCodes.ConstructRequiresXsd11, d.Code));
    }

    [Fact]
    public void Compile_OpenContentInXsd11Mode_RecordsOpenContentComponent()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:openContent><xs:any namespace="##other"/></xs:openContent>
                <xs:sequence><xs:element name="a" type="xs:string"/></xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = new SchemaCompiler().Compile(ToStream(document), BaseUri, SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));

        Assert.Empty(compiled.Diagnostics);
        var openContent = Assert.Single(compiled.OpenContents);
        Assert.Equal(OpenContentMode.Interleave, openContent.Mode);
        Assert.Equal("##other", Assert.Single(openContent.Wildcard!.NamespaceConstraint));
        Assert.Equal(WildcardProcessContents.Strict, openContent.Wildcard.ProcessContents);
    }

    [Fact]
    public void Compile_OpenContentInXsd10Mode_EmitsRequiresXsd11Diagnostic()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:openContent><xs:any namespace="##other"/></xs:openContent>
                <xs:sequence><xs:element name="a" type="xs:string"/></xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = new SchemaCompiler().Compile(ToStream(document), BaseUri, SchemaCompileOptions.Default(XsdLanguageVersion.Xsd10));

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.ConstructRequiresXsd11);
    }

    [Fact]
    public void Compile_NotASchemaRoot_EmitsRootNotSchemaDiagnostic()
    {
        var compiled = new SchemaCompiler().Compile(ToStream("<notASchema xmlns='urn:other'/>"), BaseUri, SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));

        var diagnostic = Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.RootNotSchema);
        Assert.Null(diagnostic.NodePath);
    }

    internal static MemoryStream ToStream(string document)
    {
        return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(document));
    }
}
