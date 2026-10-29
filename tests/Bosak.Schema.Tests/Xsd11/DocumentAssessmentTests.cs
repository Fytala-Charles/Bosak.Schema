// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 29 October 2026
// PURPOSE              : Tests native structural assessment, exact values, retained snapshots and real engine projections.
// SPECIAL NOTES        : Bounded native XSD 1.1 assessment; decimal-sequence-assertions-v1 profile.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 29-10-2026     | Creation (REQ-009 bounded document assessment, 0.5 development line)                      |
//                      | Charles Korthout | 0.2   | 29-10-2026     | Add boundary controls and explicit legacy annotation context                           |
//                      | Charles Korthout | 0.3   | 29-10-2026     | Check exact assertion constants and absent root normalized value                        |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.4   | 29-10-2026     | Add bounded native document assessment bridge and original-state copy handling           |
// ===========================================================================================================================================================

using System.Xml.Linq;
using System.Xml.Schema;
using Bosak.XPath.Api;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Providers.Xml;
using Bosak.XPath.Runtime.Vm;
using Bosak.Xslt.Api;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Qualifies the finite native XSD 1.1 document assessment and its explicit legacy projection.</summary>
public sealed class DocumentAssessmentTests
{
    private const string SchemaText = """
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.1">
          <xs:element name="order"><xs:complexType>
            <xs:sequence>
              <xs:element name="amount" type="xs:decimal"/>
              <xs:element name="shipped" type="xs:decimal"/>
            </xs:sequence>
            <xs:assert test="amount gt 0 and (shipped le amount or shipped eq 0)"/>
          </xs:complexType></xs:element>
        </xs:schema>
        """;
    private const string Instance = "<order><amount>12.50</amount><shipped>2</shipped></order>";
    private static XsdDocumentAssessor Compile(string? xml = null) => SchemaServices.CreateDocumentAssessor(XDocument.Parse(xml ?? SchemaText));
    private static AssessedDocument Valid(XsdDocumentAssessor? assessor = null, string? xml = null)
    {
        var schema = assessor ?? Compile();
        Assert.True(schema.IsSupported, string.Join(";", schema.Diagnostics));
        var result = schema.Assess(XDocument.Parse(xml ?? Instance));
        Assert.Equal(SchemaAssessmentOutcome.Valid, result.Outcome);
        Assert.Empty(result.Diagnostics);
        return Assert.IsType<AssessedDocument>(result.Document);
    }

    /// <summary>Structural validation and assertions yield complete original state for the admitted shape.</summary>
    [Fact]
    public void ValidDocument_RetainsRootAndChildPsvi()
    {
        var document = Valid();
        Assert.Equal(3, document.Elements.Count);
        var root = document.Elements[0];
        Assert.Same(document.Schema.RootDeclaration, root.Declaration);
        Assert.True(root.Declaration.Type.IsComplex);
        Assert.Null(root.Declaration.Type.Name);
        Assert.True(root.HasNoTypedValue);
        Assert.Null(root.TypedValue);
        Assert.Null(root.NormalizedValue);
        Assert.Equal(XName.Get("decimal", XmlSchema.Namespace), document.Elements[1].Declaration.Type.Name);
        Assert.Equal(12.5m, document.Elements[1].TypedValue!.Value.DecimalValue);
        Assert.Equal(XsdLanguageVersion.Xsd11, document.Schema.LanguageVersion);
    }

    /// <summary>Supported structural and lexical errors are invalid, not assertion-only successes.</summary>
    /// <param name="xml">The invalid instance.</param>
    [Theory]
    [InlineData("<wrong><amount>1</amount><shipped>0</shipped></wrong>")]
    [InlineData("<order><amount>1</amount></order>")]
    [InlineData("<order><shipped>0</shipped><amount>1</amount></order>")]
    [InlineData("<order><amount>1</amount><shipped>0</shipped><extra/></order>")]
    [InlineData("<order><amount>1</amount><amount>2</amount></order>")]
    [InlineData("<order attr='x'><amount>1</amount><shipped>0</shipped></order>")]
    [InlineData("<order>text<amount>1</amount><shipped>0</shipped></order>")]
    [InlineData("<order><amount><extra/></amount><shipped>0</shipped></order>")]
    [InlineData("<order><amount bad='x'>1</amount><shipped>0</shipped></order>")]
    [InlineData("<order><amount>not-a-decimal</amount><shipped>0</shipped></order>")]
    [InlineData("<order><amount>1e2</amount><shipped>0</shipped></order>")]
    [InlineData("<order><amount>INF</amount><shipped>0</shipped></order>")]
    [InlineData("<order><amount>NaN</amount><shipped>0</shipped></order>")]
    [InlineData("<order><amount>1 2</amount><shipped>0</shipped></order>")]
    [InlineData("<order><amount>1 </amount><shipped>0</shipped></order>")]
    public void InvalidInstance_NeverExposesAssessedDocument(string xml)
    {
        var result = Compile().Assess(XDocument.Parse(xml));
        Assert.Equal(SchemaAssessmentOutcome.Invalid, result.Outcome);
        Assert.Contains(result.Diagnostics, d => d.Code == "BSK-XSD-017");
        Assert.Null(result.Document);
    }

    /// <summary>Assertion failure is distinguished from lexical and structural errors.</summary>
    [Fact]
    public void AssertionFailure_IsInvalidAndDoesNotEscapeTypedDocument()
    {
        var result = Compile().Assess(XDocument.Parse("<order><amount>1</amount><shipped>2</shipped></order>"));
        Assert.Equal(SchemaAssessmentOutcome.Invalid, result.Outcome);
        Assert.Contains(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.AssertionViolated);
        Assert.Null(result.Document);
    }

    /// <summary>Valid decimal values outside exact CLR representation are refused without rounding.</summary>
    /// <param name="lexical">The valid but unrepresentable decimal.</param>
    [Theory]
    [InlineData("79228162514264337593543950336")]
    [InlineData("0.00000000000000000000000000001")]
    [InlineData("1.00000000000000000000000000001")]
    [InlineData("0.12345678901234567890123456789")]
    public void UnrepresentableDecimal_IsUnsupported(string lexical)
    {
        var result = Compile().Assess(XDocument.Parse($"<order><amount>{lexical}</amount><shipped>0</shipped></order>"));
        Assert.Equal(SchemaAssessmentOutcome.Unsupported, result.Outcome);
        Assert.Null(result.Document);
    }

    /// <summary>Exact lexical boundaries, XML whitespace and decimal normalization are retained.</summary>
    /// <param name="lexical">A supported positive decimal.</param>
    /// <param name="expected">Its exact CLR value.</param>
    [Theory]
    [InlineData("+0012.500", "12.5")]
    [InlineData(".50", "0.5")]
    [InlineData("12.", "12")]
    [InlineData(" 12.50 ", "12.5")]
    [InlineData("79228162514264337593543950335", "79228162514264337593543950335")]
    [InlineData("0.0000000000000000000000000001", "0.0000000000000000000000000001")]
    public void ExactDecimals_KeepTheirValues(string lexical, string expected)
    {
        var doc = Valid(xml: $"<order><amount>{lexical}</amount><shipped>0</shipped></order>");
        Assert.Equal(decimal.Parse(expected, System.Globalization.CultureInfo.InvariantCulture), doc.Elements[1].TypedValue!.Value.DecimalValue);
    }

    /// <summary>Unsupported schema requests cannot be compiled as a partially supported profile.</summary>
    /// <param name="before">Supported source text to replace.</param>
    /// <param name="after">Unsupported replacement.</param>
    [Theory]
    [InlineData("version=\"1.1\"", "targetNamespace=\"urn:test\"")]
    [InlineData("<xs:element name=\"order\">", "<xs:element name=\"order\" nillable=\"true\">")]
    [InlineData("<xs:complexType>", "<xs:complexType mixed=\"true\">")]
    [InlineData("<xs:sequence>", "<xs:sequence minOccurs=\"0\">")]
    [InlineData("type=\"xs:decimal\"", "type=\"xs:string\"")]
    [InlineData("name=\"shipped\"", "name=\"amount\"")]
    [InlineData("<xs:element name=\"amount\" type=\"xs:decimal\"/>", "<xs:element name=\"amount\" type=\"xs:decimal\" maxOccurs=\"unbounded\"/>")]
    [InlineData("<xs:sequence>", "<xs:sequence><xs:any/>")]
    [InlineData("<xs:element name=\"order\">", "<xs:import namespace=\"urn:other\" schemaLocation=\"https://example.invalid/schema\"/><xs:element name=\"order\">")]
    public void UnsupportedSchema_RefusesBeforeAssessingInstances(string before, string after)
    {
        var schema = Compile(SchemaText.Replace(before, after, StringComparison.Ordinal));
        Assert.False(schema.IsSupported);
        Assert.Null(schema.RootDeclaration);
        var result = schema.Assess(XDocument.Parse(Instance));
        Assert.Equal(SchemaAssessmentOutcome.Unsupported, result.Outcome);
        Assert.Null(result.Document);
    }

    /// <summary>The finite assertion grammar excludes functions, variables, axes and unknown names.</summary>
    /// <param name="expression">An expression outside the documented profile.</param>
    [Theory]
    [InlineData("doc('https://example.invalid/')")]
    [InlineData("alias:doc('file:///not-allowed')")]
    [InlineData("environment-variable('HOME')")]
    [InlineData("amount + 1 gt 0")]
    [InlineData("$value gt 0")]
    [InlineData("unknown gt 0")]
    [InlineData("../amount gt 0")]
    [InlineData("amount gt 0 trailing")]
    [InlineData("amount gt")]
    [InlineData("amount gt 0 or")]
    [InlineData("(amount gt 0")]
    [InlineData("amount eq 1.00000000000000000000000000001")]
    public void UnsupportedAssertion_IsNotExecuted(string expression)
    {
        var source = XDocument.Parse(SchemaText);
        source.Descendants(XName.Get("assert", XmlSchema.Namespace)).Single().SetAttributeValue("test", expression);
        Assert.False(SchemaServices.CreateDocumentAssessor(source).IsSupported);
    }

    /// <summary>Unsupported instance features are refused explicitly.</summary>
    /// <param name="xml">An instance requesting unsupported coverage.</param>
    [Theory]
    [InlineData("<order xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true'><amount>1</amount><shipped>0</shipped></order>")]
    [InlineData("<order><amount>1<!--keep--></amount><shipped>0</shipped></order>")]
    [InlineData("<order><amount>1<?inside data?></amount><shipped>0</shipped></order>")]
    [InlineData("<!DOCTYPE order [<!ENTITY x '1'>]><order><amount>1</amount><shipped>0</shipped></order>")]
    public void UnsupportedInstance_DoesNotProducePartialSuccess(string xml)
    {
        var result = Compile().Assess(XDocument.Parse(xml));
        Assert.Equal(SchemaAssessmentOutcome.Unsupported, result.Outcome);
        Assert.Null(result.Document);
    }

    /// <summary>Snapshots, declarations and outcome lists cannot be changed through caller-owned XML.</summary>
    [Fact]
    public void CallerMutationAndExportMutation_DoNotChangeOriginalPsvi()
    {
        var source = XDocument.Parse(SchemaText);
        var schema = SchemaServices.CreateDocumentAssessor(source);
        var instance = XDocument.Parse(Instance);
        var original = instance.ToString();
        var result = schema.Assess(instance);
        var document = Assert.IsType<AssessedDocument>(result.Document);
        Assert.Equal(original, instance.ToString());
        Assert.All(instance.Root!.DescendantsAndSelf(), e => Assert.Null(e.GetSchemaInfo()));
        source.Root!.RemoveAll(); instance.Root!.RemoveAll();
        var export = document.CreateTypedElementCopy("amount"); export.Value = "edited";
        Assert.Equal(12.5m, document.Elements[1].TypedValue!.Value.DecimalValue);
        Assert.Equal("12.50", document.CreateTypedElementCopy("amount").Value);
        Assert.Throws<NotSupportedException>(() => ((IList<AssessedElement>)document.Elements).Clear());
        Assert.True(schema.IsSupported);
    }

    /// <summary>Native copies preserve original PSVI while stripped XML loses all schema annotations.</summary>
    [Fact]
    public void CopyAndStrip_AreExplicitAndKeepOriginalAuthority()
    {
        var document = Valid(); var copy = document.Copy();
        Assert.NotSame(document, copy);
        Assert.Same(document.Schema, copy.Schema);
        Assert.Same(document.Elements[0], copy.Elements[0]);
        Assert.Equal(document.AssessmentIdentity, copy.AssessmentIdentity);
        Assert.NotEqual(document.SnapshotIdentity, copy.SnapshotIdentity);
        Assert.Equal(12.5m, copy.Elements[1].TypedValue!.Value.DecimalValue);
        var stripped = copy.CreateDocumentCopy(preserveTypeAnnotations: false);
        Assert.All(stripped.Root!.DescendantsAndSelf(), e => Assert.Null(e.GetSchemaInfo()));
        var preserved = copy.CreateDocumentCopy(preserveTypeAnnotations: true);
        Assert.Same(copy, XdmElementAssessment.Get(preserved.Root!)!.Owner);
        Assert.Same(copy.Elements[0], XdmElementAssessment.Get(preserved.Root!)!.State);
        Assert.Throws<NotSupportedException>(() => copy.CreateTypedElementCopy("order"));
    }

    /// <summary>Equal schema labels do not conflate independently compiled anonymous component identities.</summary>
    [Fact]
    public void IndependentCompiles_DoNotShareAuthority()
    {
        var first = Compile(); var second = Compile();
        Assert.Equal(first.SchemaDigest, second.SchemaDigest);
        Assert.NotSame(first.RootDeclaration, second.RootDeclaration);
        Assert.NotSame(first.RootDeclaration!.Type, second.RootDeclaration!.Type);
    }

    /// <summary>Real Bosak queries atomize only the genuinely assessed decimal export.</summary>
    [Fact]
    public void TypedLeafProjection_RunsRealXPathAndXsltWithPreserveAndStripCopies()
    {
        var document = Valid();
        var input = new XDocumentNode(document.CreateTypedElementCopy("amount"));
        var expression = XPath31Expression.Compile("data(.) instance of xs:decimal and data(.) eq 12.5");
        Assert.True(expression.Evaluate(new EvaluationContext().WithFocus(XdmValue.FromNode(input), 1, 1)).BooleanValue);
        const string stylesheet = """
            <xsl:stylesheet version="3.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
              <xsl:template name="main">
                <xsl:variable name="preserved" as="element()"><xsl:copy-of select="/*" validation="preserve"/></xsl:variable>
                <xsl:variable name="stripped" as="element()"><xsl:copy-of select="/*" validation="strip"/></xsl:variable>
                <result total="{data(/*) * 1.2}" preserved="{data($preserved) instance of xs:decimal}" stripped="{data($stripped) instance of xs:untypedAtomic}"/>
              </xsl:template>
            </xsl:stylesheet>
            """;
        var compiler = SchemaAwareXslt.CreateCompiler();
        // The legacy copy seam needs an explicit annotation context, even with no imported schema.
        // No XSD 1.1 document is compiled or validated through this empty set.
        compiler.SchemaSet = new XmlSchemaSet();
        var executable = compiler.Compile(stylesheet);
        var transformInput = new XDocumentNode(new XDocument(document.CreateTypedElementCopy("amount")));
        var output = XElement.Parse(executable.TransformToString(transformInput, initialTemplate: "main"));
        Assert.Equal("15", (string?)output.Attribute("total"));
        Assert.Equal("true", (string?)output.Attribute("preserved"));
        Assert.Equal("true", (string?)output.Attribute("stripped"));
    }

    /// <summary>Cancellation produces no partial result, including for an unsupported schema.</summary>
    [Fact]
    public void Cancellation_IsObservedBeforeAssessment()
    {
        using var cancellation = new CancellationTokenSource(); cancellation.Cancel();
        Assert.Throws<OperationCanceledException>(() => Compile().Assess(XDocument.Parse(Instance), cancellation.Token));
        Assert.Throws<OperationCanceledException>(() => Compile("<other/>").Assess(XDocument.Parse(Instance), cancellation.Token));
    }

    /// <summary>One immutable compiled endpoint can assess independent snapshots concurrently.</summary>
    [Fact]
    public async Task ConcurrentAssessment_DoesNotMixValuesOrSnapshotIdentities()
    {
        var schema = Compile();
        var documents = await Task.WhenAll(Enumerable.Range(1, 20).Select(i => Task.Run(() => Valid(schema, $"<order><amount>{i}</amount><shipped>0</shipped></order>"))));
        Assert.Equal(20, documents.Select(d => d.AssessmentIdentity).Distinct().Count());
        Assert.Equal(20, documents.Select(d => d.SnapshotIdentity).Distinct().Count());
        for (var i = 0; i < 20; i++) Assert.Equal((decimal)(i + 1), documents[i].Elements[1].TypedValue!.Value.DecimalValue);
    }

    /// <summary>Parenthesized grammar reaches the documented bound and refuses deeper input.</summary>
    [Fact]
    public void AssertionDepthLimit_IsEnforcedBeforeExecution()
    {
        var source = XDocument.Parse(SchemaText);
        var assertion = source.Descendants(XName.Get("assert", XmlSchema.Namespace)).Single();
        assertion.SetAttributeValue("test", new string('(', 16) + "amount gt 0" + new string(')', 16));
        Assert.True(SchemaServices.CreateDocumentAssessor(source).IsSupported);
        assertion.SetAttributeValue("test", new string('(', 17) + "amount gt 0" + new string(')', 17));
        Assert.False(SchemaServices.CreateDocumentAssessor(source).IsSupported);
    }

    /// <summary>Oversized expressions cannot compile or execute unbounded plans.</summary>
    [Fact]
    public void AssertionTokenAndCharacterLimits_RefuseLargePlans()
    {
        var source = XDocument.Parse(SchemaText);
        var assertion = source.Descendants(XName.Get("assert", XmlSchema.Namespace)).Single();
        assertion.SetAttributeValue("test", string.Join(" and ", Enumerable.Repeat("0 eq 0", 32)));
        Assert.True(SchemaServices.CreateDocumentAssessor(source).IsSupported);
        assertion.SetAttributeValue("test", string.Join(" and ", Enumerable.Repeat("0 eq 0", 33)));
        Assert.False(SchemaServices.CreateDocumentAssessor(source).IsSupported);
        assertion.SetAttributeValue("test", new string(' ', 513));
        Assert.False(SchemaServices.CreateDocumentAssessor(source).IsSupported);
    }

    /// <summary>The compiler refuses excessive children or assertions instead of truncating coverage.</summary>
    [Fact]
    public void ComponentBounds_DoNotTruncateTheSchema()
    {
        var source = XDocument.Parse(SchemaText);
        XNamespace xs = XmlSchema.Namespace;
        var sequence = source.Descendants(xs + "sequence").Single();
        sequence.ReplaceNodes(Enumerable.Range(0, 16).Select(i => new XElement(xs + "element", new XAttribute("name", "n" + i), new XAttribute("type", "xs:decimal"))));
        var assertion = source.Descendants(xs + "assert").Single(); assertion.SetAttributeValue("test", "n0 eq n0");
        Assert.True(SchemaServices.CreateDocumentAssessor(source).IsSupported);
        sequence.Add(new XElement(xs + "element", new XAttribute("name", "extra"), new XAttribute("type", "xs:decimal")));
        Assert.False(SchemaServices.CreateDocumentAssessor(source).IsSupported);
        source = XDocument.Parse(SchemaText);
        var type = source.Descendants(xs + "complexType").Single();
        for (var i = 0; i < 7; i++) type.Add(new XElement(xs + "assert", new XAttribute("test", "amount gt 0")));
        Assert.True(SchemaServices.CreateDocumentAssessor(source).IsSupported);
        type.Add(new XElement(xs + "assert", new XAttribute("test", "amount gt 0")));
        Assert.False(SchemaServices.CreateDocumentAssessor(source).IsSupported);
    }

    /// <summary>Large schema/instance payloads and lexical padding produce Unsupported, not rounded success.</summary>
    [Fact]
    public void InputSizeLimits_ProduceNoAssessedDocument()
    {
        var schema = XDocument.Parse(SchemaText); schema.Root!.Add(new XComment(new string('x', 32769)));
        Assert.False(SchemaServices.CreateDocumentAssessor(schema).IsSupported);
        var document = XDocument.Parse(Instance); document.Root!.Add(new XComment(new string('x', 65537)));
        Assert.Equal(SchemaAssessmentOutcome.Unsupported, Compile().Assess(document).Outcome);
        document = XDocument.Parse(Instance); document.Root!.Element("amount")!.Value = new string('0', 257) + "1";
        Assert.Equal(SchemaAssessmentOutcome.Unsupported, Compile().Assess(document).Outcome);
        document = XDocument.Parse(Instance);
        for (var i = 0; i < 257; i++) document.Root!.Add(new XComment("x"));
        Assert.Equal(SchemaAssessmentOutcome.Unsupported, Compile().Assess(document).Outcome);
    }

    /// <summary>Empty documents, malformed declaration names and unbound QNames never succeed or crash.</summary>
    /// <param name="type">An unsupported type QName.</param>
    [Theory]
    [InlineData("xs:")]
    [InlineData(":decimal")]
    [InlineData("unbound:decimal")]
    [InlineData("bad?:decimal")]
    [InlineData("xs:xs:decimal")]
    public void MalformedTypeNames_AreExplicitlyUnsupported(string type)
    {
        var source = XDocument.Parse(SchemaText);
        source.Descendants(XName.Get("element", XmlSchema.Namespace)).First(e => (string?)e.Attribute("name") == "amount").SetAttributeValue("type", type);
        Assert.False(SchemaServices.CreateDocumentAssessor(source).IsSupported);
        Assert.False(SchemaServices.CreateDocumentAssessor(new XDocument()).IsSupported);
        Assert.Equal(SchemaAssessmentOutcome.Invalid, Compile().Assess(new XDocument()).Outcome);
    }

    /// <summary>XML text normalization accepts XML whitespace and CDATA without mutating the caller.</summary>
    [Fact]
    public void CdataAndRootComments_KeepTheirIntendedAssessment()
    {
        var source = XDocument.Parse("<order><!--root--><amount><![CDATA[ 12.50 ]]></amount><?root note?><shipped>0</shipped></order>");
        var result = Compile().Assess(source);
        var document = Assert.IsType<AssessedDocument>(result.Document);
        Assert.Equal("12.50", document.Elements[1].NormalizedValue);
        Assert.Contains("root", document.CreateDocumentCopy(false).ToString(), StringComparison.Ordinal);
        Assert.Equal(" 12.50 ", source.Root!.Element("amount")!.Value);
    }

    /// <summary>Valid signed zero and negative decimals are assessed exactly under an admitting assertion.</summary>
    /// <param name="lexical">The exact decimal lexical form.</param>
    [Theory]
    [InlineData("-0")]
    [InlineData("+0.000")]
    [InlineData("-0.5")]
    [InlineData("-79228162514264337593543950335")]
    public void SignedDecimalDomain_IsNotConfusedWithTheDefaultPositiveAssertion(string lexical)
    {
        var source = XDocument.Parse(SchemaText);
        source.Descendants(XName.Get("assert", XmlSchema.Namespace)).Single().SetAttributeValue("test", "amount le 0");
        var schema = SchemaServices.CreateDocumentAssessor(source);
        Assert.Equal(SchemaAssessmentOutcome.Valid, schema.Assess(XDocument.Parse($"<order><amount>{lexical}</amount><shipped>0</shipped></order>")).Outcome);
    }

    /// <summary>Null arguments are rejected before any snapshot, interpretation or projection.</summary>
    [Fact]
    public void NullArguments_AreRejected()
    {
        Assert.Throws<ArgumentNullException>(() => SchemaServices.CreateDocumentAssessor(null!));
        Assert.Throws<ArgumentNullException>(() => Compile().Assess(null!));
        Assert.Throws<ArgumentNullException>(() => Valid().CreateTypedElementCopy(null!));
    }

    /// <summary>Assertion constants preserve exact native values at decimal precision and range boundaries.</summary>
    /// <param name="lexical">An exactly representable decimal used both as input and as an assertion constant.</param>
    [Theory]
    [InlineData("79228162514264337593543950335")]
    [InlineData("-79228162514264337593543950335")]
    [InlineData("0.1234567890123456789012345678")]
    [InlineData("0.0000000000000000000000000001")]
    public void AssertionConstants_AreComparedExactly(string lexical)
    {
        var source = XDocument.Parse(SchemaText);
        source.Descendants(XName.Get("assert", XmlSchema.Namespace)).Single().SetAttributeValue("test", "amount eq " + lexical);
        var schema = SchemaServices.CreateDocumentAssessor(source);
        Assert.True(schema.IsSupported, string.Join(";", schema.Diagnostics));
        var result = schema.Assess(XDocument.Parse($"<order><amount>{lexical}</amount><shipped>0</shipped></order>"));
        Assert.Equal(SchemaAssessmentOutcome.Valid, result.Outcome);
    }
}
