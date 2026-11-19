// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 15 November 2026
// PURPOSE              : Verifies bounded native decimal structure, assertions, state and refusal limits.
// SPECIAL NOTES        : Backend qualification only; session adapter and full XSD 1.1 remain pending.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 15-11-2026     | Implement bounded native decimal backend                                                 |
//                      | Charles Korthout | 0.2   | 19-11-2026     | Qualify deliberate namespace and zero-assertion admissions                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Text;
using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Verifies bounded native decimal structure, assertions, state and refusal limits.</summary>
public sealed class NativeDecimalBackendTests
{
    private static NativeNumericLimits Limits => new(256, 256);
    private const string Declarations = """<xs:element name="amount" type="xs:decimal" minOccurs="0" maxOccurs="2"/><xs:element name="tax" type="xs:decimal"/>""";

    /// <summary>Checks the authored golden document and original present-only component state.</summary>
    [Fact]
    public void Assess_GoldenOptionalRepeatedDocument_PreservesOriginalStateAndOwnedExports()
    {
        var schema = Compile(File.ReadAllText(Data("native-decimal.xsd")));
        var result = schema.Assess(File.ReadAllText(Data("native-decimal.xml")));
        Assert.Equal(NativeOutcome.Valid, result.Outcome);
        var assessed = Assert.IsType<NativeAssessedDocument>(result.Value);
        Assert.Equal(4, assessed.Elements.Length);
        Assert.Same(schema.Root, assessed.Elements[0].Declaration);
        Assert.Null(schema.Root.Type.Name);
        Assert.True(schema.Root.Type.HasNoTypedValue);
        Assert.Null(assessed.Elements[0].TypedValue);
        Assert.All(assessed.Elements, state => Assert.False(state.IsNilled));
        Assert.Same(schema.Children[0], assessed.Elements[1].Declaration);
        Assert.Same(schema.Children[0], assessed.Elements[2].Declaration);
        Assert.Equal("12.5", assessed.Elements[1].TypedValue!.Value.ToInvariantString(256));
        var exported = assessed.CreateDocumentCopy();
        var expected = XDocument.Load(Data("native-decimal.expected.xml"));
        Assert.True(XNode.DeepEquals(expected.Root, exported.Root));
        exported.Root!.RemoveAll();
        Assert.True(XNode.DeepEquals(expected.Root, assessed.CreateDocumentCopy().Root));
        Assert.NotSame(schema.Root.Type, Compile(File.ReadAllText(Data("native-decimal.xsd"))).Root.Type);
    }

    /// <summary>Checks structural and lexical violations never publish assessed state.</summary>
    /// <param name="xml">The invalid instance.</param>
    [Theory]
    [InlineData("<wrong><tax>1</tax></wrong>")]
    [InlineData("<order/>")]
    [InlineData("<order><tax>1</tax><amount>2</amount></order>")]
    [InlineData("<order><amount>1</amount><amount>2</amount><amount>3</amount><tax>1</tax></order>")]
    [InlineData("<order><tax>1</tax><tax>2</tax></order>")]
    [InlineData("<order><other>1</other><tax>1</tax></order>")]
    [InlineData("<order>mixed<tax>1</tax></order>")]
    [InlineData("<order><tax><nested/></tax></order>")]
    [InlineData("<order><tax>1e2</tax></order>")]
    [InlineData("<order><tax>NaN</tax></order>")]
    [InlineData("<order><tax>INF</tax></order>")]
    [InlineData("<order><tax>1 2</tax></order>")]
    [InlineData("<order><tax/></order>")]
    [InlineData("<order><tax>1</order>")]
    [InlineData("<order xmlns='urn:foreign'><tax>1</tax></order>")]
    public void Assess_InvalidStructureOrLexicalValue_ReturnsInvalidWithoutDocument(string xml) =>
        Failure(Compile(Schema()).Assess(xml), NativeOutcome.Invalid);

    /// <summary>Checks unsupported instance constructs are not silently ignored.</summary>
    /// <param name="xml">The unsupported instance.</param>
    [Theory]
    [InlineData("<order flag='yes'><tax>1</tax></order>")]
    [InlineData("<order><tax flag='yes'>1</tax></order>")]
    [InlineData("<order xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'><tax xsi:nil='true'/></order>")]
    [InlineData("<order><tax>1<!--comment--></tax></order>")]
    [InlineData("<order><tax><?leaf test?>1</tax></order>")]
    [InlineData("<!DOCTYPE order [<!ENTITY value '1'>]><order><tax>&value;</tax></order>")]
    [InlineData("<?xml version='1.1'?><order><tax>1</tax></order>")]
    public void Assess_UnsupportedInstance_ReturnsUnsupportedWithoutDocument(string xml) =>
        Failure(Compile(Schema()).Assess(xml), NativeOutcome.Unsupported);

    /// <summary>Checks optional absence, repeated count and Unicode NCName handling.</summary>
    /// <param name="expression">The supported assertion.</param>
    /// <param name="xml">The valid instance.</param>
    [Theory]
    [InlineData("empty(amount) and exists(tax)", "<order><tax>1</tax></order>")]
    [InlineData("count(amount) eq 2 and tax ge 0", "<order><amount>1</amount><amount>2</amount><tax>0</tax></order>")]
    [InlineData("tax eq +.5", "<order><tax><![CDATA[ +.50 ]]></tax></order>")]
    [InlineData("tax eq -0.50", "<order><tax>-.5</tax></order>")]
    [InlineData("tax eq 0", "<order><tax>-0.000</tax></order>")]
    [InlineData("tax ne 1", "<order><tax>2</tax></order>")]
    [InlineData("tax lt 2", "<order><tax>1</tax></order>")]
    [InlineData("tax le 1", "<order><tax>1</tax></order>")]
    [InlineData("tax gt -2", "<order><tax>-1</tax></order>")]
    [InlineData("tax eq 9999999999999999999999999999999999999999.1", "<order><tax>9999999999999999999999999999999999999999.10</tax></order>")]
    [InlineData("(empty(amount) or count(amount) eq 2) and tax eq 1", "<order><!--root--><?root ok?><tax>1</tax></order>")]
    [InlineData("empty(amount) or amount eq 1", "<order><tax>1</tax></order>")]
    [InlineData("count(amount) eq 2 or amount eq 1", "<order><amount>1</amount><amount>2</amount><tax>1</tax></order>")]
    public void Assess_SupportedAssertionAndExactDecimals_ReturnsValid(string expression, string xml)
    {
        var result = Compile(Schema(expression)).Assess(xml);
        Assert.Equal(NativeOutcome.Valid, result.Outcome);
        Assert.NotNull(result.Value);
    }

    /// <summary>Checks false, empty and repeated scalar assertion results.</summary>
    /// <param name="expression">The supported failing assertion.</param>
    /// <param name="xml">The instance.</param>
    [Theory]
    [InlineData("tax gt 0", "<order><tax>-1</tax></order>")]
    [InlineData("amount ne 0", "<order><tax>1</tax></order>")]
    [InlineData("amount eq 1", "<order><amount>1</amount><amount>1</amount><tax>1</tax></order>")]
    [InlineData("amount eq tax", "<order><amount>1</amount><amount>2</amount><tax>1</tax></order>")]
    [InlineData("amount eq 1 or count(amount) eq 2", "<order><amount>1</amount><amount>2</amount><tax>1</tax></order>")]
    [InlineData("exists(amount) and tax gt 0", "<order><tax>1</tax></order>")]
    public void Assess_FalseOrTypeErrorAssertion_ReturnsInvalidWithoutDocument(string expression, string xml) =>
        Failure(Compile(Schema(expression)).Assess(xml), NativeOutcome.Invalid);

    /// <summary>Checks complete assertion parsing and explicit function/operand refusal.</summary>
    /// <param name="expression">The unsupported expression.</param>
    [Theory]
    [InlineData("")]
    [InlineData("tax gt 0 trailing")]
    [InlineData("tax gt 0 !")]
    [InlineData("unknown eq 1")]
    [InlineData("count(unknown) eq 0")]
    [InlineData("exists(tax, amount)")]
    [InlineData("not(exists(tax))")]
    [InlineData("tax + 1 gt 0")]
    [InlineData("tax = 1")]
    [InlineData("tax eq 1e0")]
    [InlineData("exists(tax")]
    [InlineData("$value eq 1")]
    [InlineData("child::tax eq 1")]
    [InlineData("tax eq 1 OR tax eq 2")]
    public void Compile_UnsupportedAssertion_RefusesBeforeAssessment(string expression) =>
        Failure(NativeDecimalSchema.Compile(Schema(expression), Limits), NativeOutcome.Unsupported);

    /// <summary>Checks schema constructs outside the complete finite grammar.</summary>
    /// <param name="before">Text in the supported schema.</param>
    /// <param name="after">Unsupported replacement.</param>
    [Theory]
    [InlineData("<xs:schema", "<xs:schema attributeFormDefault='qualified'")]
    [InlineData("<xs:complexType>", "<xs:complexType mixed='true'>")]
    [InlineData("name=\"tax\" type=\"xs:decimal\"", "name=\"tax\" type=\"xs:string\"")]
    [InlineData("name=\"tax\" type=\"xs:decimal\"", "name=\"tax\" type=\"xs:decimal\" nillable=\"true\"")]
    [InlineData("maxOccurs=\"2\"", "maxOccurs=\"unbounded\"")]
    [InlineData("minOccurs=\"0\"", "minOccurs=\"2\"")]
    [InlineData("name=\"tax\"", "name=\"amount\"")]
    [InlineData("</xs:sequence>", "<xs:any/></xs:sequence>")]
    [InlineData("<xs:element name=\"order\">", "<xs:include schemaLocation='https://example.invalid/x.xsd'/><xs:element name=\"order\">")]
    [InlineData("name=\"order\"", "name=\"p:order\"")]
    public void Compile_UnsupportedSchemaConstruct_Refuses(string before, string after) =>
        Failure(NativeDecimalSchema.Compile(Schema().Replace(before, after, StringComparison.Ordinal), Limits), NativeOutcome.Unsupported);

    /// <summary>Checks numeric capacities before leading/trailing-zero normalization.</summary>
    [Fact]
    public void Assess_NumericCapacityBoundaries_RefusesWithoutRoundingOrNormalizationBypass()
    {
        var schema = Compile(Schema("exists(tax)"), new NativeNumericLimits(3, 2));
        Assert.Equal(NativeOutcome.Valid, schema.Assess("<order><tax>1.23</tax></order>").Outcome);
        Failure(schema.Assess("<order><tax>01.23</tax></order>"), NativeOutcome.Unsupported);
        Failure(schema.Assess("<order><tax>1.000</tax></order>"), NativeOutcome.Unsupported);
        Failure(schema.Assess("<order><tax>9999</tax></order>"), NativeOutcome.Unsupported);
        Failure(NativeDecimalSchema.Compile(Schema("tax eq 1.000"), new NativeNumericLimits(10, 2)), NativeOutcome.Unsupported);
        Failure(Compile(Schema()).Assess("<order><tax>" + new string('0', 257) + "</tax></order>"), NativeOutcome.Unsupported);
    }

    /// <summary>Checks exact UTF-8 byte ceilings, including multibyte comments.</summary>
    [Fact]
    public void CompileAndAssess_Utf8ByteBoundaries_CountWholeInput()
    {
        var schemaText = PadToBytes(Schema(), 32768);
        Assert.Equal(NativeOutcome.Valid, NativeDecimalSchema.Compile(schemaText, Limits).Outcome);
        Failure(NativeDecimalSchema.Compile(schemaText + " ", Limits), NativeOutcome.Unsupported);
        var schema = Compile(Schema());
        var instance = PadToBytes("<order><tax>1</tax></order>", 65536);
        Assert.Equal(NativeOutcome.Valid, schema.Assess(instance).Outcome);
        Failure(schema.Assess(instance + " "), NativeOutcome.Unsupported);
        Failure(schema.Assess("<order><!--" + new string('\u00e9', 32768) + "--><tax>1</tax></order>"), NativeOutcome.Unsupported);
    }

    /// <summary>Checks finite assertion length, token, nesting and declaration ceilings.</summary>
    [Fact]
    public void Compile_GrammarBounds_RejectsBeyondEachCeiling()
    {
        var nested = new string('(', 16) + "tax eq 1" + new string(')', 16);
        Assert.Equal(NativeOutcome.Valid, NativeDecimalSchema.Compile(Schema(nested), Limits).Outcome);
        Failure(NativeDecimalSchema.Compile(Schema("(" + nested + ")"), Limits), NativeOutcome.Unsupported);
        var expression = "tax eq 1" + new string(' ', 504);
        Assert.Equal(512, expression.Length);
        Assert.Equal(NativeOutcome.Valid, NativeDecimalSchema.Compile(Schema(expression), Limits).Outcome);
        Failure(NativeDecimalSchema.Compile(Schema(expression + " "), Limits), NativeOutcome.Unsupported);
        var tokens = "exists(tax) or " + string.Join(" or ", Enumerable.Repeat("tax eq 1", 31));
        Assert.Equal(NativeOutcome.Valid, NativeDecimalSchema.Compile(Schema(tokens), Limits).Outcome);
        var oneMoreToken = "exists(tax) or exists(tax) or " + string.Join(" or ", Enumerable.Repeat("tax eq 1", 30));
        Failure(NativeDecimalSchema.Compile(Schema(oneMoreToken), Limits), NativeOutcome.Unsupported);
        Failure(NativeDecimalSchema.Compile(Schema().Replace("maxOccurs=\"2\"", "maxOccurs=\"16\"", StringComparison.Ordinal), Limits), NativeOutcome.Unsupported);
        var nodes = "<order>" + string.Concat(Enumerable.Repeat("<!--x-->", 253)) + "<tax>1</tax></order>";
        Assert.Equal(NativeOutcome.Valid, Compile(Schema()).Assess(nodes).Outcome);
        Failure(Compile(Schema()).Assess(nodes.Replace("</order>", "<!--one-more--></order>", StringComparison.Ordinal)), NativeOutcome.Unsupported);
    }

    /// <summary>Checks explicit numeric capacities and cancellation before work.</summary>
    [Fact]
    public void CompileAndAssess_InvalidLimitsAndCancellation_Throw()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => NativeDecimalSchema.Compile(Schema(), default));
        Assert.Throws<ArgumentOutOfRangeException>(() => NativeDecimalSchema.Compile(Schema(), new NativeNumericLimits(1, -1)));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        Assert.Throws<OperationCanceledException>(() => NativeDecimalSchema.Compile(Schema(), Limits, cancellation.Token));
        Assert.Throws<OperationCanceledException>(() => Compile(Schema()).Assess("<order/>", cancellation.Token));
        Failure(Compile(Schema()).Assess("<order><tax>\ud800</tax></order>"), NativeOutcome.Invalid);
    }

    /// <summary>Checks declaration context resolves actual type namespace authority.</summary>
    [Fact]
    public void Compile_TypeQNameAndKeywordNames_UsesNamespaceContextAndGrammarPosition()
    {
        var correct = Schema().Replace("xs:decimal", "d:decimal", StringComparison.Ordinal)
            .Replace("<xs:sequence>", "<xs:sequence xmlns:d='http://www.w3.org/2001/XMLSchema'>", StringComparison.Ordinal);
        Assert.Equal(NativeOutcome.Valid, NativeDecimalSchema.Compile(correct, Limits).Outcome);
        Failure(NativeDecimalSchema.Compile(correct.Replace("xmlns:d='http://www.w3.org/2001/XMLSchema'", "xmlns:d='urn:foreign'", StringComparison.Ordinal), Limits), NativeOutcome.Unsupported);
        var keywords = """<xs:element name="count" type="xs:decimal"/><xs:element name="and" type="xs:decimal"/>""";
        Assert.Equal(NativeOutcome.Valid, Compile(Schema("count eq 1 and count(and) eq 1", keywords))
            .Assess("<order><count>1</count><and>2</and></order>").Outcome);
        var unicode = """<xs:element name="prijs" type="xs:decimal"/><xs:element name="bedragé" type="xs:decimal"/>""";
        Assert.Equal(NativeOutcome.Valid, Compile(Schema("bedragé eq 2", unicode))
            .Assess("<order><prijs>1</prijs><bedragé>2</bedragé></order>").Outcome);
    }

    /// <summary>Checks an absent optional declaration creates no instance state.</summary>
    [Fact]
    public void Assess_AllOptionalAbsent_PublishesOnlyElementOnlyRootState()
    {
        var declaration = """<xs:element name="amount" type="xs:decimal" minOccurs="0"/>""";
        var schema = Compile(Schema("empty(amount)", declaration));
        var result = schema.Assess("<order> \n </order>");
        Assert.Equal(NativeOutcome.Valid, result.Outcome);
        var state = Assert.Single(result.Value!.Elements);
        Assert.Null(state.TypedValue);
        Assert.True(state.Declaration.Type.HasNoTypedValue);
    }

    /// <summary>Checks declaration, aggregate occurrence, assertion count and schema-node boundaries.</summary>
    [Fact]
    public void Compile_SchemaCountBoundaries_AcceptsLimitsAndRefusesOneBeyond()
    {
        var sixteen = string.Concat(Enumerable.Range(0, 16).Select(i => $"<xs:element name='n{i}' type='xs:decimal'/>"));
        var schema = Compile(Schema("exists(n0)", sixteen));
        var xml = "<order>" + string.Concat(Enumerable.Range(0, 16).Select(i => $"<n{i}>1</n{i}>")) + "</order>";
        Assert.Equal(NativeOutcome.Valid, schema.Assess(xml).Outcome);
        Failure(NativeDecimalSchema.Compile(Schema("exists(n0)", sixteen + "<xs:element name='n16' type='xs:decimal'/>"), Limits), NativeOutcome.Unsupported);
        const string assertion = "<xs:assert test='exists(tax)'/>";
        var eight = Schema().Replace("</xs:complexType>", string.Concat(Enumerable.Repeat(assertion, 7)) + "</xs:complexType>", StringComparison.Ordinal);
        Assert.Equal(NativeOutcome.Valid, NativeDecimalSchema.Compile(eight, Limits).Outcome);
        Failure(NativeDecimalSchema.Compile(eight.Replace("</xs:complexType>", assertion + "</xs:complexType>", StringComparison.Ordinal), Limits), NativeOutcome.Unsupported);
        var noAssertions = XDocument.Parse(Schema());
        noAssertions.Descendants(XName.Get("assert", "http://www.w3.org/2001/XMLSchema")).Remove();
        Assert.Equal(NativeOutcome.Valid, NativeDecimalSchema.Compile(noAssertions.ToString(SaveOptions.DisableFormatting), Limits).Outcome);
        var nodeLimit = Schema().Replace("<xs:sequence>", "<xs:sequence>" + string.Concat(Enumerable.Repeat("<!--x-->", 249)), StringComparison.Ordinal);
        Assert.Equal(NativeOutcome.Valid, NativeDecimalSchema.Compile(nodeLimit, Limits).Outcome);
        Failure(NativeDecimalSchema.Compile(nodeLimit.Replace("<xs:sequence>", "<xs:sequence><!--one-more-->", StringComparison.Ordinal), Limits), NativeOutcome.Unsupported);
    }

    /// <summary>Checks exact lexical maximum and zero-scale decimal lexical variants.</summary>
    [Fact]
    public void Assess_DecimalLexicalBoundaries_AcceptsExactMaximumAndZeroScale()
    {
        var schema = Compile(Schema());
        Assert.Equal(NativeOutcome.Valid, schema.Assess("<order><tax>" + new string('9', 256) + "</tax></order>").Outcome);
        Failure(schema.Assess("<order><tax>" + new string('9', 257) + "</tax></order>"), NativeOutcome.Unsupported);
        var zeroScale = Compile(Schema(), new NativeNumericLimits(3, 0));
        Assert.Equal(NativeOutcome.Valid, zeroScale.Assess("<order><tax>+1.</tax></order>").Outcome);
        Failure(zeroScale.Assess("<order><tax>1.0</tax></order>"), NativeOutcome.Unsupported);
    }

    /// <summary>Checks Boolean precedence and error handling for the evaluated short-circuit branch.</summary>
    [Fact]
    public void Assess_LogicalPrecedenceAndEmptyRepeatedOperands_PreservesXPathRules()
    {
        const string xml = "<order><amount>1</amount><amount>2</amount><tax>1</tax></order>";
        Assert.Equal(NativeOutcome.Valid, Compile(Schema("tax eq 1 or tax eq 2 and amount eq 1")).Assess(xml).Outcome);
        Failure(Compile(Schema("(tax eq 1 or tax eq 2) and amount eq 1")).Assess(xml), NativeOutcome.Invalid);
        Failure(Compile(Schema("tax eq 2 and amount eq 1")).Assess(xml), NativeOutcome.Invalid);
        var declaration = "<xs:element name='amount' type='xs:decimal' maxOccurs='2'/><xs:element name='absent' type='xs:decimal' minOccurs='0'/>";
        Failure(Compile(Schema("absent eq amount", declaration)).Assess("<order><amount>1</amount><amount>2</amount></order>"), NativeOutcome.Invalid);
    }

    private static string Schema(string expression = "exists(tax)", string declarations = Declarations) =>
        "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name=\"order\"><xs:complexType><xs:sequence>" +
        declarations + "</xs:sequence><xs:assert " + new XAttribute("test", expression) + "/></xs:complexType></xs:element></xs:schema>";

    /// <summary>Checks concurrent independent assessments share components and own distinct documents.</summary>
    /// <returns>The completion of all independent assessment operations.</returns>
    [Fact]
    public async Task Assess_ConcurrentIndependentInputs_KeepsSnapshotsAndValuesIndependent()
    {
        var schema = Compile(Schema());
        var results = await Task.WhenAll(Enumerable.Range(1, 16).Select(value => Task.Run(() =>
            schema.Assess($"<order><tax>{value}</tax></order>"))));
        for (var index = 0; index < results.Length; index++)
        {
            Assert.Equal(NativeOutcome.Valid, results[index].Outcome);
            var document = results[index].Value!;
            Assert.Same(schema.Children[1], document.Elements[1].Declaration);
            Assert.Equal((index + 1).ToString(System.Globalization.CultureInfo.InvariantCulture),
                document.Elements[1].TypedValue!.Value.ToInvariantString(256));
            if (index > 0) Assert.NotSame(results[0].Value, document);
        }
    }

    private static NativeDecimalSchema Compile(string schema, NativeNumericLimits? limits = null)
    {
        var result = NativeDecimalSchema.Compile(schema, limits ?? Limits);
        Assert.True(result.Outcome == NativeOutcome.Valid, result.Diagnostic);
        return result.Value!;
    }

    private static void Failure<T>(NativeResult<T> result, NativeOutcome outcome) where T : class
    {
        Assert.Equal(outcome, result.Outcome);
        Assert.Null(result.Value);
        Assert.False(string.IsNullOrEmpty(result.Diagnostic));
    }

    private static string Data(string file) => Path.Combine(AppContext.BaseDirectory, "TestData", "Schemas", file);
    private static string PadToBytes(string xml, int count) => xml + new string(' ', count - Encoding.UTF8.GetByteCount(xml));
}
