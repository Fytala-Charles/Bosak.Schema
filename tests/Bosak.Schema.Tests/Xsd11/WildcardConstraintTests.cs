// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 30 October 2026
// PURPOSE              : REQ-009 S5c wildcard constraint tests: notNamespace / notQName recording and representation-rule diagnostics.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation. Rules pinned to W3C XML
//                        Schema 1.1 Part 1 §3.10.2 (wildcard representation), §3.10.3 (Wildcard Representation OK)
//                        and §3.10.2.2 (wildcard component mapping); semantic §3.10.6.1 item-4 checking is a later phase.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 30-10-2026     | Creation (REQ-009 S5c)                                                                   |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>
/// Component tests for the REQ-009 S5c wildcard negative constraints: recording of
/// <c>notNamespace</c> / <c>notQName</c> on open content wildcards into the immutable compiled
/// model, and the explicit representation-rule diagnostics (<c>BSK-XSD-016</c>) for the
/// §3.10.3 mutual exclusion and §3.10.2 token-space violations. Content-model and attribute
/// wildcards stay explicit <c>BSK-XSD-003</c> unsupported diagnostics (no component is recorded
/// for them yet).
/// </summary>
public class WildcardConstraintTests
{
    private static readonly Uri BaseUri = new("urn:test:root");

    private static ICompiledSchema Compile(string document, XsdLanguageVersion version = XsdLanguageVersion.Xsd11)
    {
        return new SchemaCompiler().Compile(SchemaComponentTests.ToStream(document), BaseUri, SchemaCompileOptions.Default(version));
    }

    [Fact]
    public void Compile_OpenContentAnyWithNotNamespace_RecordsTokensAndEmptyPositiveConstraint()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" targetNamespace="urn:t">
              <xs:complexType name="t">
                <xs:openContent>
                  <xs:any notNamespace="##targetNamespace ##local"/>
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
        Assert.Empty(openContent.Wildcard!.NamespaceConstraint);
        Assert.Equal(["##targetNamespace", "##local"], openContent.Wildcard.NotNamespace);
        Assert.Empty(openContent.Wildcard.NotQName);
        Assert.Equal(WildcardProcessContents.Strict, openContent.Wildcard.ProcessContents);
    }

    [Fact]
    public void Compile_OpenContentAnyWithNotQName_RecordsKeywordAndQNameTokens()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
              <xs:complexType name="t">
                <xs:openContent>
                  <xs:any namespace="##other" processContents="lax" notQName="##defined ##definedSibling xsl:comment unprefixed"/>
                </xs:openContent>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Empty(compiled.Diagnostics);
        var wildcard = Assert.Single(compiled.OpenContents).Wildcard!;
        Assert.Equal("##other", Assert.Single(wildcard.NamespaceConstraint));
        Assert.Equal(WildcardProcessContents.Lax, wildcard.ProcessContents);
        Assert.Equal(["##defined", "##definedSibling", "xsl:comment", "unprefixed"], wildcard.NotQName);
        Assert.Empty(wildcard.NotNamespace);
    }

    [Fact]
    public void Compile_DefaultOpenContentWithNotNamespace_RecordsTokens()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:defaultOpenContent>
                <xs:any notNamespace="urn:disallowed ##local" processContents="skip"/>
              </xs:defaultOpenContent>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Empty(compiled.Diagnostics);
        var wildcard = Assert.Single(compiled.DefaultOpenContents).Wildcard!;
        Assert.Empty(wildcard.NamespaceConstraint);
        Assert.Equal(["urn:disallowed", "##local"], wildcard.NotNamespace);
        Assert.Equal(WildcardProcessContents.Skip, wildcard.ProcessContents);
    }

    [Fact]
    public void Compile_OpenContentAnyWithoutNegativeConstraints_RecordsEmptyLists()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
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

        var compiled = Compile(document);

        Assert.Empty(compiled.Diagnostics);
        var wildcard = Assert.Single(compiled.OpenContents).Wildcard!;
        Assert.Equal("##any", Assert.Single(wildcard.NamespaceConstraint));
        Assert.Empty(wildcard.NotNamespace);
        Assert.Empty(wildcard.NotQName);
    }

    [Fact]
    public void Compile_AnyWithNamespaceAndNotNamespace_EmitsInvalidWildcardAndRecordsNothing()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:openContent>
                  <xs:any namespace="##other" notNamespace="##local"/>
                </xs:openContent>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        var diagnostic = Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidWildcard);
        Assert.Contains("namespace", diagnostic.Message, StringComparison.Ordinal);
        Assert.Contains("notNamespace", diagnostic.Message, StringComparison.Ordinal);
        Assert.Empty(compiled.OpenContents);
    }

    [Fact]
    public void Compile_AnyWithDisallowedNotNamespaceToken_EmitsInvalidWildcard()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:openContent>
                  <xs:any notNamespace="##other"/>
                </xs:openContent>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidWildcard);
        Assert.Empty(compiled.OpenContents);
    }

    [Fact]
    public void Compile_AnyWithUnknownNotQNameToken_EmitsInvalidWildcard()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:openContent>
                  <xs:any notQName="##bogus"/>
                </xs:openContent>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        var diagnostic = Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidWildcard);
        Assert.Contains("##bogus", diagnostic.Message, StringComparison.Ordinal);
        Assert.Empty(compiled.OpenContents);
    }

    [Fact]
    public void Compile_AnyWithMalformedNotQNameToken_EmitsInvalidWildcard()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:openContent>
                  <xs:any notQName=":leading-colon"/>
                </xs:openContent>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidWildcard);
        Assert.Empty(compiled.OpenContents);
    }

    [Fact]
    public void Compile_ContentModelAnyWithNotQName_EmitsUnsupportedConstruct()
    {
        // Content-model wildcards have no recorded component yet: the negative constraints stay an
        // explicit BSK-XSD-003 rather than being silently accepted.
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:sequence>
                  <xs:any notQName="##defined"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.UnsupportedConstruct);
        Assert.Empty(compiled.OpenContents);
    }

    [Fact]
    public void Compile_Xsd10OpenContentAnyWithNotNamespace_RequiresXsd11Only()
    {
        // Under XSD 1.0 the parent xs:openContent is rejected with BSK-XSD-002; the recorded-wildcard
        // exemption must not suppress that, nor emit wildcard-specific diagnostics for the child.
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t">
                <xs:openContent>
                  <xs:any notNamespace="##local"/>
                </xs:openContent>
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document, XsdLanguageVersion.Xsd10);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.ConstructRequiresXsd11);
        Assert.DoesNotContain(compiled.Diagnostics, d => d.Code is SchemaDiagnosticCodes.InvalidWildcard or SchemaDiagnosticCodes.UnsupportedConstruct);
    }
}
