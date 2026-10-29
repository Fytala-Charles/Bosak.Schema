// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 29 October 2026
// PURPOSE              : Verifies activation-free schema access and legacy API compatibility.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 29-10-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Text;
using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Checks the Apache licensing boundary independently of the old credential infrastructure.</summary>
public sealed class ApacheLicenseTests
{
    /// <summary>Verifies all public factories work without a credential.</summary>
    [Fact]
    public void Factories_WithoutActivation_AreAvailable()
    {
        Assert.NotNull(SchemaServices.CreateSchemaCompiler());
        Assert.NotNull(SchemaServices.CreateValidationService());
        Assert.True(SchemaAwareXslt.CreateCompiler().SchemaAware);
    }

    /// <summary>Verifies legacy activation calls do not parse or change credential state.</summary>
    [Fact]
    public void LegacyActivation_RepeatedIgnoredValues_DoesNotRestrictSchema()
    {
        var marker = License.Current;
        License.Activate("legacy-value-is-ignored");
        License.Activate(null!);
        Assert.True(License.IsActivated);
        Assert.Same(marker, License.Current);
        Assert.Equal("Apache-2.0 (activation not required)", marker!.Licensee);
        Assert.True(marker.IsUsable);
        Assert.NotNull(SchemaServices.CreateSchemaCompiler());
    }

    /// <summary>Verifies invalid configuration still fails without a licensing prerequisite.</summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void EmptySchemaDirectory_ThrowsArgumentException(string directory)
    {
        Assert.Throws<ArgumentException>(() => SchemaAwareXslt.CreateCompiler(directory));
    }

    /// <summary>Verifies both successful and failing XSD assertion assessment through free factories.</summary>
    /// <param name="quantity">Candidate quantity.</param>
    /// <param name="expectedValid">Whether the assertion should pass.</param>
    [Theory]
    [InlineData(3, true)]
    [InlineData(0, false)]
    public void AssertionValidation_WithoutActivation_PreservesResults(int quantity, bool expectedValid)
    {
        const string schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='order'><xs:complexType><xs:sequence><xs:element name='qty' type='xs:integer'/></xs:sequence><xs:assert test='qty gt 0'/></xs:complexType></xs:element></xs:schema>";
        using var bytes = new MemoryStream(Encoding.UTF8.GetBytes(schema));
        var compiled = SchemaServices.CreateSchemaCompiler().Compile(bytes, new Uri("urn:test:apache"), SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));
        var result = SchemaServices.CreateValidationService().Validate(compiled, XElement.Parse($"<order><qty>{quantity}</qty></order>"));
        Assert.Equal(expectedValid, result.IsValid);
    }

    /// <summary>Verifies the public assembly contains no internal key issuer.</summary>
    [Fact]
    public void PublicAssembly_DoesNotContainSigningOrBillingInfrastructure()
    {
        var assembly = typeof(SchemaServices).Assembly;
        Assert.Null(assembly.GetType("Bosak.Schema.LicenseKeyIssuer"));
        Assert.Null(assembly.GetType("Bosak.Schema.LicenseKey"));
        Assert.DoesNotContain(assembly.GetReferencedAssemblies(), reference => reference.Name!.Contains("Licensing.Pipeline", StringComparison.Ordinal));
    }
}
