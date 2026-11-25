// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 17 November 2026
// PURPOSE              : Tests compile-diagnostic preconditions and explicit legacy validation scope.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 17-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Text;
using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Tests compile-diagnostic preconditions and explicit legacy validation scope.</summary>
public sealed class CompileDiagnosticValidationTests
{
    private const string Clean = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='Positive'><xs:restriction base='xs:decimal'/></xs:simpleType><xs:element name='order' type='xs:decimal'/></xs:schema>";

    /// <summary>Checks every service entry point returns original compiler findings instead of success.</summary>
    [Theory]
    [InlineData(0)] [InlineData(1)] [InlineData(2)]
    public void InvalidRoot_AllEntryPoints_ReturnCompileFindings(int route)
    {
        var schema = Compile("<not-schema/>");
        var expected = Assert.Single(schema.Diagnostics);
        var result = Call(new XsdValidationService(), route, schema);
        Assert.False(result.IsValid);
        Assert.Same(expected, Assert.Single(result.Diagnostics));
        Assert.Equal(SchemaDiagnosticCodes.RootNotSchema, result.Diagnostics[0].Code);
    }

    /// <summary>Checks compile failure is decided before typed focus, type lookup or test traversal.</summary>
    [Theory]
    [InlineData(0)] [InlineData(1)] [InlineData(2)]
    public void CompileFailure_SkipsOtherModelCollections(int route)
    {
        var schema = Assert.IsType<CompiledSchema>(Compile("<not-schema/>")) with
        {
            ElementDeclarations = null!, Assertions = null!, Alternatives = null!, SimpleTypes = null!,
        };
        var result = Call(new XsdValidationService(), route, schema);
        Assert.False(result.IsValid);
        Assert.Equal(schema.Diagnostics, result.Diagnostics);
    }

    /// <summary>Checks findings retain order and duplicates while detaching from caller-owned mutable lists.</summary>
    [Theory]
    [InlineData(0)] [InlineData(1)] [InlineData(2)]
    public void CallerDiagnostics_AreCopiedOnceWithoutTranslation(int route)
    {
        var a = new SchemaDiagnostic("APP-FIRST", "Detailed caller finding", "/first");
        var b = new SchemaDiagnostic(SchemaDiagnosticCodes.UnsupportedConstruct, "Unsupported finding", "/second");
        var findings = new List<SchemaDiagnostic> { a, b, a };
        var schema = Assert.IsType<CompiledSchema>(Compile(Clean)) with { Diagnostics = findings };
        var result = Call(new XsdValidationService(), route, schema);
        findings.Clear();
        Assert.False(result.IsValid);
        Assert.Equal(new[] { a, b, a }, result.Diagnostics);
        Assert.Throws<NotSupportedException>(() => ((IList<SchemaDiagnostic>)result.Diagnostics).Clear());
    }

    /// <summary>Checks clean assertion-free models retain the exact limited legacy success meaning.</summary>
    [Theory]
    [InlineData(0)] [InlineData(1)] [InlineData(2)]
    public void CleanModel_NoApplicableTests_RetainsScopedSuccess(int route)
    {
        var schema = Compile(Clean);
        Assert.Empty(schema.Diagnostics);
        var result = Call(new XsdValidationService(), route, schema);
        Assert.True(result.IsValid);
        Assert.Empty(result.Diagnostics);
    }

    /// <summary>Checks unsupported language-mode findings block validation without new diagnostic codes.</summary>
    [Theory]
    [InlineData(0)] [InlineData(1)] [InlineData(2)]
    public void Xsd10_RequiresXsd11Finding_BlocksEveryRoute(int route)
    {
        const string xml = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='root'><xs:complexType><xs:assert test='true()'/></xs:complexType></xs:element></xs:schema>";
        var schema = Compile(xml, new SchemaCompileOptions(XsdLanguageVersion.Xsd10));
        Assert.Contains(schema.Diagnostics, d => d.Code == SchemaDiagnosticCodes.ConstructRequiresXsd11);
        var result = Call(new XsdValidationService(), route, schema);
        Assert.False(result.IsValid);
        Assert.Equal(schema.Diagnostics, result.Diagnostics);
    }

    /// <summary>Checks real denied import findings propagate without any network request or fallback.</summary>
    [Theory]
    [InlineData(0)] [InlineData(1)] [InlineData(2)]
    public void DeniedImport_OriginalResolutionFinding_BlocksEveryRoute(int route)
    {
        const string xml = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:import namespace='urn:blocked' schemaLocation='https://example.invalid/blocked.xsd'/></xs:schema>";
        var schema = Compile(xml, new SchemaCompileOptions(XsdLanguageVersion.Xsd11, new SecureSchemaResolver(SchemaResolutionPolicy.DenyAll)));
        Assert.Equal(SchemaDiagnosticCodes.ResolutionDenied, Assert.Single(schema.Diagnostics).Code);
        var result = Call(new XsdValidationService(), route, schema);
        Assert.False(result.IsValid);
        Assert.Equal(schema.Diagnostics, result.Diagnostics);
    }

    /// <summary>Checks existing null-argument errors still precede the compile-diagnostic result guard.</summary>
    [Fact]
    public void Arguments_InvalidReferences_ThrowBeforeCompileGuard()
    {
        var service = new XsdValidationService();
        var schema = Compile("<not-schema/>");
        Assert.Throws<ArgumentNullException>(() => service.Validate(null!, new XElement("x")));
        Assert.Throws<ArgumentNullException>(() => service.Validate(schema, null!));
        Assert.Throws<ArgumentNullException>(() => service.ValidateAlternativeTests(null!));
        Assert.Throws<ArgumentNullException>(() => service.ValidateValue(null!, XName.Get("x"), "1"));
        Assert.Throws<ArgumentNullException>(() => service.ValidateValue(schema, null!, "1"));
        Assert.Throws<ArgumentNullException>(() => service.ValidateValue(schema, XName.Get("x"), null!));
    }

    /// <summary>Checks compile failure wins over unknown-type or malformed lexical-value handling.</summary>
    [Fact]
    public void CompileFailure_DoesNotParseMalformedValueOrLookupMissingType()
    {
        var result = new XsdValidationService().ValidateValue(Compile("<not-schema/>"), XName.Get("missing"), "NOT-A-DECIMAL");
        Assert.False(result.IsValid);
        Assert.Equal(SchemaDiagnosticCodes.RootNotSchema, Assert.Single(result.Diagnostics).Code);
    }

    /// <summary>Checks the service still evaluates clean supported facets after earlier guarded failures.</summary>
    [Fact]
    public void GuardedFailure_DoesNotPoisonLaterValidOrInvalidFacetChecks()
    {
        var service = new XsdValidationService();
        foreach (var route in new[] { 0, 1, 2 }) Assert.False(Call(service, route, Compile("<not-schema/>")).IsValid);
        var schema = Compile("<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='Positive'><xs:restriction base='xs:decimal'><xs:assertion test='$value gt 0'/></xs:restriction></xs:simpleType></xs:schema>");
        Assert.True(service.ValidateValue(schema, XName.Get("Positive"), "2").IsValid);
        Assert.False(service.ValidateValue(schema, XName.Get("Positive"), "-2").IsValid);
    }

    /// <summary>Checks clean models still use the original missing-simple-type diagnostic.</summary>
    [Fact]
    public void CleanModel_MissingSimpleType_RetainsOriginalDiagnostic()
    {
        var result = new XsdValidationService().ValidateValue(Compile(Clean), XName.Get("missing"), "1");
        Assert.False(result.IsValid);
        Assert.Equal(SchemaDiagnosticCodes.SimpleTypeNotFound, Assert.Single(result.Diagnostics).Code);
    }

    private static SchemaValidationResult Call(XsdValidationService service, int route, ICompiledSchema schema) => route switch
    {
        0 => service.Validate(schema, new XElement("unexpected")),
        1 => service.ValidateValue(schema, XName.Get("Positive"), "2"),
        _ => service.ValidateAlternativeTests(schema),
    };

    private static ICompiledSchema Compile(string xml, SchemaCompileOptions? options = null)
    {
        using var input = new MemoryStream(Encoding.UTF8.GetBytes(xml));
        return new SchemaCompiler().Compile(input, new Uri("file:///guard.xsd"), options ?? SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));
    }
}
