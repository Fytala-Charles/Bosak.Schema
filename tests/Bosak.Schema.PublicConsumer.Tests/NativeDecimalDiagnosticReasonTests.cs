// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Tests public detected-reason categories, privacy and unchanged outcome boundaries.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.Schema;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Exercises additive reason categories through the public session API only.</summary>
public sealed class NativeDecimalDiagnosticReasonTests
{
    private const string Schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='xs:decimal'/></xs:sequence><xs:assert test='a gt 0'/></xs:complexType></xs:element></xs:schema>";
    private static NativeDecimalSessionOptions Options => new(256, 256, 1, 4, 1, 256, 256);
    private static NativeDecimalAssessmentContext Context => new(new DateTimeOffset(2026, 11, 19, 0, 0, 0, TimeSpan.Zero), 0);

    /// <summary>Detects categories at existing schema refusal checks, preserving stage codes and privacy.</summary>
    /// <param name="change">The authored schema mutation.</param>
    /// <param name="reason">Expected detected reason.</param>
    [Theory]
    [InlineData("namespace", NativeDecimalUnsupportedReason.Namespace)]
    [InlineData("shape", NativeDecimalUnsupportedReason.DeclarationShape)]
    [InlineData("type", NativeDecimalUnsupportedReason.ValueType)]
    [InlineData("attribute", NativeDecimalUnsupportedReason.Attributes)]
    [InlineData("assertion", NativeDecimalUnsupportedReason.AssertionGrammar)]
    [InlineData("assertion-length", NativeDecimalUnsupportedReason.RepresentationCapacity)]
    public void Create_UnsupportedSchema_PreservesStageAndReportsDetectedReason(string change, NativeDecimalUnsupportedReason reason)
    {
        string input = change switch
        {
            "namespace" => Schema.Replace("<xs:schema ", "<xs:schema targetNamespace='urn:PRIVATE-MARKER' "),
            "shape" => Schema.Replace("<xs:assert test='a gt 0'/>", ""),
            "type" => Schema.Replace("xs:decimal", "xs:string"),
            "attribute" => Schema.Replace("<xs:schema ", "<xs:schema secret='PRIVATE-MARKER' "),
            "assertion" => Schema.Replace("a gt 0", "sum(a) gt 0"),
            _ => Schema.Replace("a gt 0", new string(' ', 513))
        };
        Refused(NativeDecimalSession.Create(input, Options), NativeDecimalOutcome.Unsupported, "SCHEMA", reason);
    }

    /// <summary>Transports instance categories through the real provider while preserving Invalid classification.</summary>
    /// <param name="input">Authored instance text.</param>
    /// <param name="outcome">Expected unchanged assessment outcome.</param>
    /// <param name="reason">Expected detected reason.</param>
    [Theory]
    [InlineData("<r secret='PRIVATE-MARKER'><a>1</a></r>", NativeDecimalOutcome.Unsupported, NativeDecimalUnsupportedReason.Attributes)]
    [InlineData("<r><a secret='PRIVATE-MARKER'>1</a></r>", NativeDecimalOutcome.Unsupported, NativeDecimalUnsupportedReason.Attributes)]
    [InlineData("<r><a>0</a></r>", NativeDecimalOutcome.Invalid, NativeDecimalUnsupportedReason.Unspecified)]
    [InlineData("<r><a>bad</a></r>", NativeDecimalOutcome.Invalid, NativeDecimalUnsupportedReason.Unspecified)]
    [InlineData("<r xmlns='urn:PRIVATE-MARKER'><a>1</a></r>", NativeDecimalOutcome.Invalid, NativeDecimalUnsupportedReason.Unspecified)]
    [InlineData("<r", NativeDecimalOutcome.Invalid, NativeDecimalUnsupportedReason.Unspecified)]
    [InlineData("<!DOCTYPE r SYSTEM 'https://PRIVATE-MARKER.invalid'><r/>", NativeDecimalOutcome.Unsupported, NativeDecimalUnsupportedReason.Unspecified)]
    public void Assess_RefusedInstance_ReportsOnlyTheDetectedCategory(string input, NativeDecimalOutcome outcome, NativeDecimalUnsupportedReason reason)
    {
        using var session = NativeDecimalSession.Create(Schema, Options).Value!;
        Refused(session.Assess(input, Context), outcome, "INSTANCE", reason);
        using var recovered = session.Assess("<r><a>1</a></r>", Context).Value!;
        Assert.Equal(1, recovered.DecimalCount);
    }

    /// <summary>Preserves input-order detection rather than searching ahead for a preferred category.</summary>
    /// <param name="attributes">Authored attributes in the intended input order.</param>
    /// <param name="reason">Expected first detected reason.</param>
    [Theory]
    [InlineData("secret='PRIVATE-MARKER' targetNamespace='urn:PRIVATE-MARKER'", NativeDecimalUnsupportedReason.Attributes)]
    [InlineData("targetNamespace='urn:PRIVATE-MARKER' secret='PRIVATE-MARKER'", NativeDecimalUnsupportedReason.Namespace)]
    public void Create_MultipleUnsupportedAttributes_UsesFirstDetectedRefusal(string attributes, NativeDecimalUnsupportedReason reason) =>
        Refused(NativeDecimalSession.Create(Schema.Replace("<xs:schema ", "<xs:schema " + attributes + " "), Options), NativeDecimalOutcome.Unsupported, "SCHEMA", reason);

    /// <summary>Separates explicit numeric limits from missing assertion grammar.</summary>
    [Fact]
    public void NumericCapacity_SchemaAndInstance_PreservesCapacityReason()
    {
        var limits = new NativeDecimalSessionOptions(2, 2, 1, 4, 1, 2, 2);
        Refused(NativeDecimalSession.Create(Schema.Replace("a gt 0", "a gt 1000"), limits), NativeDecimalOutcome.Unsupported, "SCHEMA", NativeDecimalUnsupportedReason.RepresentationCapacity);
        using var session = NativeDecimalSession.Create(Schema, limits).Value!;
        Refused(session.Assess("<r><a>1000</a></r>", Context), NativeDecimalOutcome.Unsupported, "INSTANCE", NativeDecimalUnsupportedReason.RepresentationCapacity);
    }

    /// <summary>Classifies capture and lexical ceilings without bypassing normal cleanup.</summary>
    [Fact]
    public void InputCapacity_ByteAndLexicalLimits_PreserveCapacityReason()
    {
        Refused(NativeDecimalSession.Create(new string(' ', 32769), Options), NativeDecimalOutcome.Unsupported, "SCHEMA", NativeDecimalUnsupportedReason.RepresentationCapacity);
        using var session = NativeDecimalSession.Create(Schema, Options).Value!;
        Refused(session.Assess(new string(' ', 65537), Context), NativeDecimalOutcome.Unsupported, "INSTANCE", NativeDecimalUnsupportedReason.RepresentationCapacity);
        Refused(session.Assess("<r><a>" + new string('1', 257) + "</a></r>", Context), NativeDecimalOutcome.Unsupported, "INSTANCE", NativeDecimalUnsupportedReason.RepresentationCapacity);
        using var valid = session.Assess("<r><a>1</a></r>", Context).Value!;
        Assert.Equal("1", valid.ReadDecimal(0).ToInvariantString(256));
    }

    /// <summary>Replays complete public legacy schemas as native refusals without claiming to enumerate every gap.</summary>
    /// <param name="file">Bundled public example filename.</param>
    [Theory]
    [InlineData("orders.xsd")]
    [InlineData("sku.xsd")]
    public void PublicLegacyExamples_AreStillUnsupportedWithNamespaceReason(string file)
    {
        var input = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "TestData", "Schemas", file));
        Refused(NativeDecimalSession.Create(input, Options), NativeDecimalOutcome.Unsupported, "SCHEMA", NativeDecimalUnsupportedReason.Namespace);
    }

    /// <summary>Valid results remain diagnostic-free and enum values remain explicitly versioned.</summary>
    [Fact]
    public void ValidWorkflow_HasNoDiagnosticAndKeepsDetachedOutput()
    {
        var result = NativeDecimalSession.Create(Schema, Options);
        Assert.Empty(result.Diagnostics);
        using var session = result.Value!;
        var assessed = session.Assess("<r><a>1</a></r>", Context);
        Assert.Empty(assessed.Diagnostics);
        var document = assessed.Value!;
        string detached = document.ExportXml();
        document.Dispose();
        Assert.Contains("<a>1</a>", detached);
        Assert.Equal(0, (int)NativeDecimalUnsupportedReason.Unspecified);
        Assert.Equal(6, (int)NativeDecimalUnsupportedReason.RepresentationCapacity);
        Assert.Empty(typeof(NativeDecimalDiagnostic).GetConstructors());
    }

    private static void Refused<T>(NativeDecimalResult<T> result, NativeDecimalOutcome outcome, string stage, NativeDecimalUnsupportedReason reason) where T : class
    {
        Assert.Equal(outcome, result.Outcome);
        Assert.Null(result.Value);
        Assert.False(result.DiagnosticsTruncated);
        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("BSK-NATIVE-" + stage + "-" + outcome.ToString().ToUpperInvariant(), diagnostic.Code);
        Assert.Equal(reason, diagnostic.UnsupportedReason);
        Assert.Equal(outcome == NativeDecimalOutcome.Invalid ? "The input is malformed or violates the supported native decimal profile." : "The input or representation is outside the supported native decimal profile.", diagnostic.Message);
        Assert.DoesNotContain("PRIVATE-MARKER", diagnostic.Message);
        Assert.InRange(diagnostic.Message.Length, 1, 160);
    }
}
