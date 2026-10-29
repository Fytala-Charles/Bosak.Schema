// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : REQ-009 S5b default attributes tests: xs:schema defaultAttributes + complexType defaultAttributesApply recording.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation. Rules pinned to the
//                        XSD 1.1 schema-for-schemas and W3C XML Schema 1.1 Part 1 §3.4.2.4 (see docs/ADR-005).
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S5b)                                                                   |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>
/// Component tests for the REQ-009 S5b default attributes compile support: recording of
/// <c>xs:schema defaultAttributes="QName"</c> references and <c>xs:complexType
/// defaultAttributesApply="false"</c> opt-outs into the immutable compiled model, with the
/// explicit malformed-value diagnostic (<c>BSK-XSD-012</c>) and the XSD 1.0 rejection
/// (<c>BSK-XSD-002</c>).
/// </summary>
public class DefaultAttributesTests
{
    private static readonly Uri BaseUri = new("urn:test:root");

    private static ICompiledSchema Compile(string document, XsdLanguageVersion version = XsdLanguageVersion.Xsd11)
    {
        return new SchemaCompiler().Compile(SchemaComponentTests.ToStream(document), BaseUri, SchemaCompileOptions.Default(version));
    }

    [Fact]
    public void Compile_DefaultAttributesOnSchema_RecordsResolvedReference()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:test" targetNamespace="urn:test" defaultAttributes="t:commonAttrs">
              <xs:attributeGroup name="commonAttrs">
                <xs:attribute name="id" type="xs:string"/>
              </xs:attributeGroup>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Empty(compiled.Diagnostics);
        var reference = Assert.Single(compiled.DefaultAttributeReferences);
        Assert.Equal("urn:test", reference.AttributeGroupNamespace);
        Assert.Equal("commonAttrs", reference.AttributeGroupLocalName);
    }

    [Fact]
    public void Compile_DefaultAttributesUnprefixed_RecordsEmptyNamespace()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" defaultAttributes="commonAttrs">
              <xs:complexType name="t">
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Empty(compiled.Diagnostics);
        var reference = Assert.Single(compiled.DefaultAttributeReferences);
        Assert.Equal(string.Empty, reference.AttributeGroupNamespace);
        Assert.Equal("commonAttrs", reference.AttributeGroupLocalName);
    }

    [Fact]
    public void Compile_DefaultAttributesApplyFalse_RecordsOptOutWithTypeName()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:test" targetNamespace="urn:test" defaultAttributes="t:commonAttrs">
              <xs:complexType name="closed" defaultAttributesApply="false">
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Empty(compiled.Diagnostics);
        var optOut = Assert.Single(compiled.DefaultAttributeOptOuts);
        Assert.Equal("closed", optOut.ComplexTypeName);
    }

    [Fact]
    public void Compile_DefaultAttributesApplyFalseOnAnonymousType_RecordsOptOutWithoutTypeName()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:element name="a">
                <xs:complexType defaultAttributesApply="false">
                  <xs:sequence>
                    <xs:element name="b" type="xs:string"/>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Empty(compiled.Diagnostics);
        var optOut = Assert.Single(compiled.DefaultAttributeOptOuts);
        Assert.Null(optOut.ComplexTypeName);
        Assert.NotNull(optOut.NodePath);
    }

    [Fact]
    public void Compile_DefaultAttributesApplyTrue_RecordsNothing()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="open" defaultAttributesApply="true">
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Empty(compiled.Diagnostics);
        Assert.Empty(compiled.DefaultAttributeOptOuts);
        Assert.Empty(compiled.DefaultAttributeReferences);
    }

    [Fact]
    public void Compile_DefaultAttributesUnderXsd10_EmitsConstructRequiresXsd11ForBothConstructs()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:test" targetNamespace="urn:test" defaultAttributes="t:commonAttrs">
              <xs:complexType name="t" defaultAttributesApply="false">
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document, XsdLanguageVersion.Xsd10);

        Assert.Equal(2, compiled.Diagnostics.Count(d => d.Code == SchemaDiagnosticCodes.ConstructRequiresXsd11));
        Assert.Empty(compiled.DefaultAttributeReferences);
        Assert.Empty(compiled.DefaultAttributeOptOuts);
    }

    [Fact]
    public void Compile_DefaultAttributesApplyInvalidBoolean_EmitsInvalidDefaultAttributes()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:complexType name="t" defaultAttributesApply="maybe">
                <xs:sequence>
                  <xs:element name="a" type="xs:string"/>
                </xs:sequence>
              </xs:complexType>
            </xs:schema>
            """;

        var compiled = Compile(document);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.InvalidDefaultAttributes);
        Assert.Empty(compiled.DefaultAttributeOptOuts);
    }
}
