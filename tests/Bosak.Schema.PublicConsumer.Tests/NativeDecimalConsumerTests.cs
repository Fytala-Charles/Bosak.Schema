// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 17 November 2026
// PURPOSE              : Qualifies the public native decimal contract without friend-assembly access.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.17; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 17-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 17-11-2026     | Qualify restricted schema resolution from the external public consumer                   |
//                      | Charles Korthout | 0.3   | 17-11-2026     | Qualify compiler-owned snapshots without changing caller-created record compatibility      |
//                      | Charles Korthout | 0.4   | 17-11-2026     | Qualify compile-diagnostic preconditions and assertion-only legacy success                  |
//                      | Charles Korthout | 0.5   | 24-11-2026     | Qualify the adopted Core 1.81.17 development baseline                                      |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Bosak.XPath.Core.Xdm;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Qualifies the public native decimal contract without friend-assembly access.</summary>
public sealed class NativeDecimalConsumerTests
{
    private static string Schema => File.ReadAllText(Data("native-decimal.xsd"));
    private static string Instance => File.ReadAllText(Data("native-decimal.xml"));
    private static NativeDecimalSessionOptions Options => new(256, 256, 1, 4, 1, 256, 256);
    private static NativeDecimalAssessmentContext Context => new(new DateTimeOffset(2026, 11, 17, 12, 0, 0, TimeSpan.Zero), 60);

    /// <summary>Runs the real public scalar workflow and checks detached XML and exact values after disposal.</summary>
    [Fact]
    public void PublicWorkflow_GoldenInput_ReturnsDetachedValuesAndXml()
    {
        var result = NativeDecimalSession.Create(Schema, Options);
        Assert.Equal(NativeDecimalOutcome.Valid, result.Outcome);
        Assert.Empty(result.Diagnostics);
        Assert.False(result.DiagnosticsTruncated);
        var session = Assert.IsType<NativeDecimalSession>(result.Value);
        var assessed = session.Assess(Instance, Context);
        Assert.Equal(NativeDecimalOutcome.Valid, assessed.Outcome);
        var document = Assert.IsType<NativeDecimalDocument>(assessed.Value);
        Assert.Equal(3, document.DecimalCount);
        XdmExactDecimal value = document.ReadDecimal(0);
        Assert.Equal("12.5", value.ToInvariantString(256));
        Assert.Equal("2", document.ReadDecimal(1).ToInvariantString(256));
        Assert.Equal("1", document.ReadDecimal(2).ToInvariantString(256));
        var xml = document.ExportXml();
        Assert.True(XNode.DeepEquals(XDocument.Load(Data("native-decimal.expected.xml")).Root, XDocument.Parse(xml).Root));
        XDocument.Parse(xml).Root!.RemoveAll();
        Assert.Equal(3, XDocument.Parse(document.ExportXml()).Root!.Elements().Count());
        session.Dispose();
        document.Dispose();
        session.Dispose();
        Assert.Equal("12.5", value.ToInvariantString(256));
        Assert.Contains("12.50", xml);
        Assert.Throws<ObjectDisposedException>(() => document.DecimalCount);
        Assert.Throws<ObjectDisposedException>(() => document.ReadDecimal(0));
        Assert.Throws<ObjectDisposedException>(() => document.ExportXml());
        Assert.Throws<ObjectDisposedException>(() => session.Assess(Instance, Context));
    }

    /// <summary>Checks public refusal status, stable diagnostic bounds and quota recovery.</summary>
    [Theory]
    [InlineData("<order", NativeDecimalOutcome.Invalid, "BSK-NATIVE-INSTANCE-INVALID")]
    [InlineData("<order><tax>1</tax></order>", NativeDecimalOutcome.Invalid, "BSK-NATIVE-INSTANCE-INVALID")]
    [InlineData("<order flag='PRIVATE-CALLER-TEXT'/>", NativeDecimalOutcome.Unsupported, "BSK-NATIVE-INSTANCE-UNSUPPORTED")]
    [InlineData("<!DOCTYPE order SYSTEM 'https://invalid.example/private'><order/>", NativeDecimalOutcome.Unsupported, "BSK-NATIVE-INSTANCE-UNSUPPORTED")]
    public void Assess_RefusedInput_HasNoOwnedValue(string input, NativeDecimalOutcome outcome, string code)
    {
        using var session = Create();
        var result = session.Assess(input, Context);
        Assert.Equal(outcome, result.Outcome);
        Assert.Null(result.Value);
        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal(code, diagnostic.Code);
        Assert.InRange(diagnostic.Message.Length, 1, 160);
        Assert.DoesNotContain("PRIVATE-CALLER-TEXT", diagnostic.Message);
        Assert.DoesNotContain("invalid.example", diagnostic.Message);
        Assert.False(result.DiagnosticsTruncated);
        using var valid = session.Assess(Instance, Context).Value!;
        Assert.Equal(3, valid.DecimalCount);
    }

    /// <summary>Checks schema failures remain distinct from instance failures.</summary>
    [Fact]
    public void Create_RefusedSchema_UsesStableSchemaDiagnostic()
    {
        var malformed = NativeDecimalSession.Create("<xs:schema", Options);
        Assert.Equal(NativeDecimalOutcome.Invalid, malformed.Outcome);
        Assert.Null(malformed.Value);
        Assert.Equal("BSK-NATIVE-SCHEMA-INVALID", Assert.Single(malformed.Diagnostics).Code);
        var unsupported = NativeDecimalSession.Create(Schema.Replace("xs:decimal", "xs:string", StringComparison.Ordinal), Options);
        Assert.Equal(NativeDecimalOutcome.Unsupported, unsupported.Outcome);
        Assert.Null(unsupported.Value);
        Assert.Equal("BSK-NATIVE-SCHEMA-UNSUPPORTED", Assert.Single(unsupported.Diagnostics).Code);
    }

    /// <summary>Checks every invalid option is rejected before any schema parsing.</summary>
    [Theory]
    [InlineData(0, 0)] [InlineData(1, -1)] [InlineData(2, 0)] [InlineData(3, 0)]
    [InlineData(4, 0)] [InlineData(5, 0)] [InlineData(6, -1)]
    public void Options_InvalidBound_ThrowsAtConstruction(int index, int invalid)
    {
        var values = new[] { 256, 256, 1, 4, 1, 256, 256 };
        values[index] = invalid;
        Assert.Throws<ArgumentOutOfRangeException>(() => new NativeDecimalSessionOptions(values[0], values[1], values[2], values[3], values[4], values[5], values[6]));
    }

    /// <summary>Checks explicit context boundaries without consulting host time or timezone.</summary>
    [Theory]
    [InlineData(-841, false)] [InlineData(-840, true)] [InlineData(840, true)] [InlineData(841, false)]
    public void Context_TimezoneBound_IsExplicit(int minutes, bool valid)
    {
        if (!valid) Assert.Throws<ArgumentOutOfRangeException>(() => new NativeDecimalAssessmentContext(Context.CurrentDateTime, minutes));
        else
        {
            var context = new NativeDecimalAssessmentContext(Context.CurrentDateTime, minutes);
            using var session = Create();
            using var document = session.Assess(Instance, context).Value!;
            Assert.Equal(minutes, context.ImplicitTimezoneOffsetMinutes);
            Assert.Equal(Context.CurrentDateTime, context.CurrentDateTime);
            Assert.Equal(3, document.DecimalCount);
        }
    }

    /// <summary>Checks null arguments and pre-cancellation do not consume admissions or parse malformed text.</summary>
    [Fact]
    public void ArgumentsAndCancellation_RejectBeforeInputAndLeaveSessionUsable()
    {
        using var canceled = new CancellationTokenSource();
        canceled.Cancel();
        Assert.Throws<ArgumentNullException>(() => NativeDecimalSession.Create("<", null!));
        Assert.Throws<ArgumentNullException>(() => NativeDecimalSession.Create(null!, Options));
        Assert.Throws<OperationCanceledException>(() => NativeDecimalSession.Create("<", Options, canceled.Token));
        using var session = Create();
        Assert.Throws<ArgumentNullException>(() => session.Assess("<", null!));
        Assert.Throws<ArgumentNullException>(() => session.Assess(null!, Context));
        Assert.Throws<OperationCanceledException>(() => session.Assess("<", Context, canceled.Token));
        using var document = session.Assess(Instance, Context).Value!;
        Assert.Throws<OperationCanceledException>(() => document.ReadDecimal(0, canceled.Token));
        Assert.Throws<OperationCanceledException>(() => document.ExportXml(canceled.Token));
        Assert.Throws<ArgumentOutOfRangeException>(() => document.ReadDecimal(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => document.ReadDecimal(3));
        Assert.Equal("1", document.ReadDecimal(2).ToInvariantString(256));
    }

    /// <summary>Checks quota exhaustion is operational and document disposal frees the exact admission.</summary>
    [Fact]
    public void Admission_QuotaExceeded_ThrowsAndDisposeAllowsReuse()
    {
        using var session = Create();
        var first = session.Assess(Instance, Context).Value!;
        Assert.Throws<InvalidOperationException>(() => session.Assess(Instance, Context));
        Assert.Equal("1", first.ReadDecimal(2).ToInvariantString(256));
        first.Dispose();
        first.Dispose();
        using var second = session.Assess(Instance, Context).Value!;
        Assert.Equal("2", second.ReadDecimal(1).ToInvariantString(256));
        Assert.Throws<ObjectDisposedException>(() => first.ReadDecimal(0));
    }

    /// <summary>Checks a session revokes every independent document while other sessions remain usable.</summary>
    [Fact]
    public void Dispose_MultipleDocuments_RevokesOnlyTheOwningSession()
    {
        using var owner = NativeDecimalSession.Create(Schema, new(256, 256, 2, 4, 1, 256, 256)).Value!;
        using var independent = Create();
        var first = owner.Assess(Instance, Context).Value!;
        var second = owner.Assess(Instance, Context).Value!;
        using var third = independent.Assess(Instance, Context).Value!;
        owner.Dispose();
        Assert.Throws<ObjectDisposedException>(() => first.ExportXml());
        Assert.Throws<ObjectDisposedException>(() => second.ReadDecimal(1));
        Assert.Equal("12.5", third.ReadDecimal(0).ToInvariantString(256));
        first.Dispose();
        second.Dispose();
    }

    /// <summary>Checks separate native and query limits refuse precision loss without poisoning later work.</summary>
    [Fact]
    public void NumericLimits_NoRoundingAndNormalizationBypass_RemainExplicit()
    {
        using var native = NativeDecimalSession.Create(Schema, new(2, 1, 1, 4, 1, 256, 256)).Value!;
        Assert.Equal(NativeDecimalOutcome.Unsupported, native.Assess(Instance, Context).Outcome);
        using var query = NativeDecimalSession.Create(Schema, new(256, 256, 1, 4, 1, 1, 0)).Value!;
        using var doc = query.Assess(Instance, Context).Value!;
        Assert.Throws<NotSupportedException>(() => doc.ReadDecimal(0));
        Assert.Equal("1", doc.ReadDecimal(2).ToInvariantString(8));
        var value = doc.ReadDecimal(2);
        Assert.Throws<ArgumentOutOfRangeException>(() => value.ToInvariantString(0));
    }

    /// <summary>Checks absent optional children have no scalar index and zero scale is admitted.</summary>
    [Fact]
    public void OptionalEmptyRoot_HasNoFabricatedScalar()
    {
        const string schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='order'><xs:complexType><xs:sequence><xs:element name='amount' type='xs:decimal' minOccurs='0'/></xs:sequence><xs:assert test='empty(amount)'/></xs:complexType></xs:element></xs:schema>";
        using var session = NativeDecimalSession.Create(schema, new(1, 0, 1, 1, 1, 1, 0)).Value!;
        using var document = session.Assess("<order/>", Context).Value!;
        Assert.Equal(0, document.DecimalCount);
        Assert.Throws<ArgumentOutOfRangeException>(() => document.ReadDecimal(0));
    }

    /// <summary>Verifies this assembly is outside the friend boundary and cannot manufacture owned results.</summary>
    [Fact]
    public void PublicSurface_NoFriendGrantOrPublicFactoriesForAuthority()
    {
        var schemaAssembly = typeof(NativeDecimalSession).Assembly;
        var friends = schemaAssembly.GetCustomAttributes<InternalsVisibleToAttribute>();
        Assert.DoesNotContain(friends, attribute => attribute.AssemblyName == typeof(NativeDecimalConsumerTests).Assembly.GetName().Name);
        Assert.Empty(typeof(NativeDecimalSession).GetConstructors());
        Assert.Empty(typeof(NativeDecimalDocument).GetConstructors());
        Assert.Empty(typeof(NativeDecimalResult<NativeDecimalSession>).GetConstructors());
        Assert.Empty(typeof(NativeDecimalDiagnostic).GetConstructors());
        Assert.All(typeof(NativeDecimalSessionOptions).GetProperties(), property => Assert.Null(property.SetMethod));
        Assert.All(typeof(NativeDecimalAssessmentContext).GetProperties(), property => Assert.Null(property.SetMethod));
        Assert.DoesNotContain(schemaAssembly.GetExportedTypes(), type => type.Name.Contains("NativeSchemaProvider", StringComparison.Ordinal));
        Assert.Equal(new Version(1, 81, 17, 0), typeof(XdmExactDecimal).Assembly.GetName().Version);
    }

    /// <summary>Checks the opt-in resolver returns detached read-only schema bytes outside friend access.</summary>
    [Fact]
    public void RestrictedResolver_ContainedFile_ReturnsReadonlyBytes()
    {
        WithSchemaFiles((root, file) =>
        {
            using var stream = new RestrictedSchemaFileResolver(root, 4096).Open(new Uri(file));
            Assert.False(stream.CanWrite);
            Assert.Contains("urn:restricted", new StreamReader(stream).ReadToEnd(), StringComparison.Ordinal);
        });
    }

    /// <summary>Checks a public resolver refuses a contained-prefix sibling without reading its file.</summary>
    [Fact]
    public void RestrictedResolver_OutsideFile_Refuses()
    {
        WithSchemaFiles((root, file) =>
        {
            var error = Assert.Throws<SchemaResolutionException>(() => new RestrictedSchemaFileResolver(root, 4096).Open(new Uri(Path.Combine(root, "..", "outside.xsd"))));
            Assert.Equal(SchemaDiagnosticCodes.ResolutionDenied, error.Code);
        });
    }

    /// <summary>Checks the public restricted XSLT factory compiles an allowed schema using published Core.</summary>
    [Fact]
    public void RestrictedCompiler_ContainedSchema_Compiles()
    {
        WithSchemaFiles((root, file) =>
        {
            var compiler = SchemaAwareXslt.CreateRestrictedCompiler(root, 4096);
            const string stylesheet = "<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='3.0'><xsl:import-schema namespace='urn:restricted' schema-location='schema.xsd'/><xsl:template name='main'/></xsl:stylesheet>";
            compiler.Compile(stylesheet, new Uri(Path.Combine(root, "main.xsl")).AbsoluteUri);
            Assert.NotEmpty(compiler.ResourcePolicy!.Receipts);
        });
    }

    private static void WithSchemaFiles(Action<string, string> action)
    {
        var root = Path.Combine(Path.GetTempPath(), "bosak-restricted-consumer-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        var file = Path.Combine(root, "schema.xsd");
        try
        {
            File.WriteAllText(file, "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:restricted'><xs:element name='value' type='xs:decimal'/></xs:schema>");
            action(root, file);
        }
        finally { File.Delete(file); Directory.Delete(root, false); }
    }

    /// <summary>Checks compiler-produced outer collections refuse mutable-interface changes publicly.</summary>
    [Fact]
    public void DescriptorSnapshot_OuterCollections_RefuseMutation()
    {
        var model = Descriptor();
        var count = model.ElementDeclarations.Count;
        Assert.NotEqual(0, count);
        Assert.Throws<NotSupportedException>(() => ((IList<ElementDeclaration>)model.ElementDeclarations).Clear());
        Assert.Throws<NotSupportedException>(() => ((IList<SchemaDiagnostic>)model.Diagnostics).Clear());
        Assert.Equal(count, model.ElementDeclarations.Count);
    }

    /// <summary>Checks nested bindings and facet lists cannot alter a compiler-produced model publicly.</summary>
    [Fact]
    public void DescriptorSnapshot_NestedBindingsAndFacets_RefuseMutation()
    {
        var model = Descriptor();
        var simple = Assert.Single(model.SimpleTypes);
        var assertion = Assert.Single(simple.Assertions);
        Assert.Throws<NotSupportedException>(() => ((IList<AssertionDefinition>)simple.Assertions).Clear());
        Assert.Throws<NotSupportedException>(() => ((IDictionary<string, string>)assertion.NamespaceBindings!)["p"] = "urn:changed");
        Assert.Equal("urn:original", assertion.NamespaceBindings!["p"]);
        Assert.Same(Assert.Single(model.Assertions), assertion);
        Assert.True(SchemaServices.CreateValidationService().ValidateValue(model, XName.Get("Positive"), "2").IsValid);
    }

    /// <summary>Checks caller-created record replacements remain compatible and independent of the original.</summary>
    [Fact]
    public void DescriptorSnapshot_RecordCopy_CannotMutateOriginal()
    {
        var model = Assert.IsType<CompiledSchema>(Descriptor());
        var callerList = model.ElementDeclarations.ToList();
        var copy = model with { ElementDeclarations = callerList };
        callerList.Clear();
        Assert.Empty(copy.ElementDeclarations);
        Assert.NotEmpty(model.ElementDeclarations);
    }

    private static ICompiledSchema Descriptor()
    {
        const string schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:p='urn:original'><xs:simpleType name='Positive'><xs:restriction base='xs:decimal'><xs:assertion test='$value gt 0'/></xs:restriction></xs:simpleType><xs:element name='amount' type='xs:decimal'/></xs:schema>";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(schema));
        return SchemaServices.CreateSchemaCompiler().Compile(stream, new Uri("file:///descriptor.xsd"), SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));
    }

    /// <summary>Checks each legacy operation exposes compile findings through the public service contract.</summary>
    [Theory]
    [InlineData(0)] [InlineData(1)] [InlineData(2)]
    public void LegacyValidation_CompileFindings_ReturnFalseBeforeWork(int route)
    {
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("<not-schema/>"));
        var model = SchemaServices.CreateSchemaCompiler().Compile(stream, new Uri("file:///invalid.xsd"), SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));
        var service = SchemaServices.CreateValidationService();
        var result = route switch
        {
            0 => service.Validate(model, new XElement("unknown")),
            1 => service.ValidateValue(model, XName.Get("missing"), "bad lexical value"),
            _ => service.ValidateAlternativeTests(model),
        };
        Assert.False(result.IsValid);
        Assert.Equal(model.Diagnostics, result.Diagnostics);
        Assert.Equal(SchemaDiagnosticCodes.RootNotSchema, Assert.Single(result.Diagnostics).Code);
    }

    /// <summary>Checks a clean assertion-free model does not gain an unsupported structural-validity promise.</summary>
    [Fact]
    public void LegacyValidation_CleanUnmatchedRoot_RemainsAssertionOnly()
    {
        var model = Descriptor();
        var result = SchemaServices.CreateValidationService().Validate(model, new XElement("unknown"));
        Assert.True(result.IsValid);
        Assert.Empty(result.Diagnostics);
    }

    private static NativeDecimalSession Create() => NativeDecimalSession.Create(Schema, Options).Value!;
    private static string Data(string file) => Path.Combine(AppContext.BaseDirectory, "TestData", "Schemas", file);
}
