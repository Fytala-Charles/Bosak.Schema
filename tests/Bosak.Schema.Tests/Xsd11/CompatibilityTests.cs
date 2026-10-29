// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : Tests for activation-free schema compilation and validation compatibility.
// SPECIAL NOTES        : Unit tests verifying the license gate; shares the Activation state collection and reset helper.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S1)                                                                    |
//                      | Charles Korthout | 0.2   | 27-10-2026     | Added CreateValidationService gate tests (REQ-009 S2)                                    |
//                      | Charles Korthout | 0.3   | 29-10-2026     | Verify schema services are available without credential activation                       |

//                      | Charles Korthout | 0.4   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Verifies schema services work without activation and preserve their compilation/validation behavior.</summary>
public class CompatibilityTests
{
    [Fact]
    public void CreateSchemaCompiler_BeforeActivation_ReturnsCompiler()
    {
        Assert.NotNull(SchemaServices.CreateSchemaCompiler());
    }

    [Fact]
    public void Factories_BeforeActivation_AreAllAvailable()
    {
        Assert.NotNull(SchemaServices.CreateSchemaCompiler());
        Assert.NotNull(SchemaServices.CreateValidationService());
        Assert.True(SchemaAwareXslt.CreateCompiler().SchemaAware);
    }

    [Fact]
    public void CreateSchemaCompiler_WithoutActivation_ReturnsWorkingCompiler()
    {

        var compiler = SchemaServices.CreateSchemaCompiler();

        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" targetNamespace="urn:test">
              <xs:element name="a" type="xs:string"/>
            </xs:schema>
            """;
        var compiled = compiler.Compile(SchemaComponentTests.ToStream(document), new Uri("urn:test:root"), SchemaCompileOptions.Default(XsdLanguageVersion.Xsd10));

        Assert.Empty(compiled.Diagnostics);
        Assert.Equal(XsdLanguageVersion.Xsd10, compiled.LanguageVersion);
        Assert.StartsWith("xsd-", compiled.SchemaId, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateValidationService_BeforeActivation_ReturnsService()
    {
        Assert.NotNull(SchemaServices.CreateValidationService());
    }

    [Fact]
    public void CreateValidationService_WithoutActivation_ReturnsWorkingService()
    {

        var validation = SchemaServices.CreateValidationService();

        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:element name="order">
                <xs:complexType>
                  <xs:sequence><xs:element name="qty" type="xs:integer"/></xs:sequence>
                  <xs:assert test="qty gt 0"/>
                </xs:complexType>
              </xs:element>
            </xs:schema>
            """;
        var compiled = new SchemaCompiler().Compile(SchemaComponentTests.ToStream(document), new Uri("urn:test:root"), SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));
        var result = validation.Validate(compiled, System.Xml.Linq.XElement.Parse("<order><qty>3</qty></order>"));

        Assert.True(result.IsValid);
    }
}
