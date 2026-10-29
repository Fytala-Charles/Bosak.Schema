// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : REQ-009 S5a open content tests: xs:openContent / xs:defaultOpenContent recording and composition-rule diagnostics.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation. Rules pinned to the
//                        XSD 1.1 schema-for-schemas and W3C XML Schema 1.1 Part 1 §3.4.2 (see docs/ADR-005).
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S5a)                                                                   |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>
/// Component tests for the REQ-009 S5a open content compile support: recording of
/// <c>xs:openContent</c> / <c>xs:defaultOpenContent</c> components into the immutable compiled
/// model, and the explicit XSD 1.1 composition-rule diagnostics (<c>BSK-XSD-011</c>) for
/// misplaced, duplicated, misordered, or malformed open content declarations.
/// </summary>
public class OpenContentTests
{
    private static readonly Uri BaseUri = new("urn:test:root");

    private static ICompiledSchema Compile(string document, XsdLanguageVersion version = XsdLanguageVersion.Xsd11)
    {
        return new SchemaCompiler().Compile(SchemaComponentTests.ToStream(document), BaseUri, SchemaCompileOptions.Default(version));
    }

    [Fact]
    public void Compile_OpenContentSuffixMode_RecordsSuffixWithWildcardDefaults()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:openContent mode="suffix">
                  <xs:any/>
                </xs:openContent>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Empty(compiled.Diagnostics);
        var openContent = Assert.Single(compiled.OpenContents);
        Assert.Equal(OpenContentMode.Suffix, openContent.Mode);
        Assert.Equal("##any", Assert.Single(openContent.Wildcard!.NamespaceConstraint));
        Assert.Equal(WildcardProcessContents.Strict, openContent.Wildcard.ProcessContents);
    }

    [Fact]
    public void Compile_OpenContentOnComplexContentExtension_RecordsDefinition()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:complexContent>
                  <xs:extension base="xs:anyType">
                    <xs:openContent>
                      <xs:any namespace="urn:extra" processContents="skip"/>
                    </xs:openContent>
                    <xs:sequence>
                      <xs:element name="a" type="xs:string"/>
                    </xs:sequence>
                  </xs:extension>
                </xs:complexContent>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Empty(compiled.Diagnostics);
        var openContent = Assert.Single(compiled.OpenContents);
        Assert.Equal(OpenContentMode.Interleave, openContent.Mode);
        Assert.Equal("urn:extra", Assert.Single(openContent.Wildcard!.NamespaceConstraint));
        Assert.Equal(WildcardProcessContents.Skip, openContent.Wildcard.ProcessContents);
    }

    [Fact]
    public void Compile_OpenContentOnComplexContentRestriction_RecordsDefinition()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:complexContent>
                  <xs:restriction base="xs:anyType">
                    <xs:openContent>
                      <xs:any namespace="##local"/>
                    </xs:openContent>
                    <xs:sequence>
                      <xs:element name="a" type="xs:string"/>
                    </xs:sequence>
                  </xs:restriction>
                </xs:complexContent>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Empty(compiled.Diagnostics);
        var openContent = Assert.Single(compiled.OpenContents);
        Assert.Equal("##local", Assert.Single(openContent.Wildcard!.NamespaceConstraint));
    }

    [Fact]
    public void Compile_OpenContentModeNone_RecordsDefinitionWithoutWildcard()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:openContent mode="none"/>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Empty(compiled.Diagnostics);
        var openContent = Assert.Single(compiled.OpenContents);
        Assert.Equal(OpenContentMode.None, openContent.Mode);
        Assert.Null(openContent.Wildcard);
    }

    [Fact]
    public void Compile_DefaultOpenContent_RecordsAppliesToEmptyAndMode()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:defaultOpenContent appliesToEmpty="true" mode="suffix">
                <xs:any namespace="##other" processContents="lax"/>
              </xs:defaultOpenContent>
              <xs:element name="a" type="xs:string"/>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Empty(compiled.Diagnostics);
        var defaultOpenContent = Assert.Single(compiled.DefaultOpenContents);
        Assert.True(defaultOpenContent.AppliesToEmpty);
        Assert.Equal(OpenContentMode.Suffix, defaultOpenContent.Mode);
        Assert.Equal("##other", Assert.Single(defaultOpenContent.Wildcard!.NamespaceConstraint));
        Assert.Equal(WildcardProcessContents.Lax, defaultOpenContent.Wildcard.ProcessContents);
        Assert.Empty(compiled.OpenContents);
    }

    [Fact]
    public void Compile_OpenContentUnderXsd10_EmitsConstructRequiresXsd11ForBothConstructs()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:defaultOpenContent>
                <xs:any/>
              </xs:defaultOpenContent>
              <xs:complexType name="t">
                <xs:openContent>
                  <xs:any/>
                </xs:openContent>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document, XsdLanguageVersion.Xsd10);

        Assert.Equal(2, compiled.Diagnostics.Count(d => d.Code == SchemaDiagnosticCodes.ConstructRequiresXsd11));
        Assert.Empty(compiled.OpenContents);
        Assert.Empty(compiled.DefaultOpenContents);
    }

    [Fact]
    public void Compile_OpenContentOnSimpleContent_EmitsInvalidOpenContent()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:simpleContent>
                  <xs:extension base="xs:string">
                    <xs:openContent>
                      <xs:any/>
                    </xs:openContent>
                  </xs:extension>
                </xs:simpleContent>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidOpenContent);
        Assert.Empty(compiled.OpenContents);
    }

    [Fact]
    public void Compile_OpenContentAfterParticle_EmitsInvalidOpenContent()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
                <xs:openContent>
                  <xs:any/>
                </xs:openContent>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidOpenContent);
        Assert.Empty(compiled.OpenContents);
    }

    [Fact]
    public void Compile_DuplicateOpenContent_EmitsInvalidOpenContent()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:openContent>
                  <xs:any/>
                </xs:openContent>
                <xs:openContent>
                  <xs:any namespace="urn:extra"/>
                </xs:openContent>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidOpenContent);
        var openContent = Assert.Single(compiled.OpenContents);
        Assert.Equal("##any", Assert.Single(openContent.Wildcard!.NamespaceConstraint));
    }

    [Fact]
    public void Compile_OpenContentModeNoneWithAny_EmitsInvalidOpenContent()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:openContent mode="none">
                  <xs:any/>
                </xs:openContent>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidOpenContent);
        Assert.Empty(compiled.OpenContents);
    }

    [Fact]
    public void Compile_OpenContentWithoutAny_EmitsInvalidOpenContent()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:openContent mode="suffix"/>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidOpenContent);
        Assert.Empty(compiled.OpenContents);
    }

    [Fact]
    public void Compile_OpenContentInvalidMode_EmitsInvalidOpenContent()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:openContent mode="middle">
                  <xs:any/>
                </xs:openContent>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidOpenContent);
        Assert.Empty(compiled.OpenContents);
    }

    [Fact]
    public void Compile_DefaultOpenContentModeNone_EmitsInvalidOpenContent()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:defaultOpenContent mode="none">
                <xs:any/>
              </xs:defaultOpenContent>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidOpenContent);
        Assert.Empty(compiled.DefaultOpenContents);
    }

    [Fact]
    public void Compile_DefaultOpenContentWithoutAny_EmitsInvalidOpenContent()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:defaultOpenContent/>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidOpenContent);
        Assert.Empty(compiled.DefaultOpenContents);
    }

    [Fact]
    public void Compile_DefaultOpenContentAfterComponent_EmitsInvalidOpenContent()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:element name="a" type="xs:string"/>
              <xs:defaultOpenContent>
                <xs:any/>
              </xs:defaultOpenContent>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidOpenContent);
        Assert.Empty(compiled.DefaultOpenContents);
    }

    [Fact]
    public void Compile_DefaultOpenContentInsideComplexType_EmitsInvalidOpenContent()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:defaultOpenContent>
                  <xs:any/>
                </xs:defaultOpenContent>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidOpenContent);
        Assert.Empty(compiled.DefaultOpenContents);
    }

    [Fact]
    public void Compile_OpenContentInvalidWildcardValues_EmitInvalidOpenContent()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="badProcessContents">
                <xs:openContent>
                  <xs:any processContents="maybe"/>
                </xs:openContent>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
              <xs:complexType name="badNamespace">
                <xs:openContent>
                  <xs:any namespace="##bogus"/>
                </xs:openContent>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Equal(2, compiled.Diagnostics.Count(d => d.Code == SchemaDiagnosticCodes.InvalidOpenContent));
        Assert.Empty(compiled.OpenContents);
    }
}
