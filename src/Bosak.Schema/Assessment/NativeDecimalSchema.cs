// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 15 November 2026
// PURPOSE              : Compiles and assesses the internal finite native decimal schema profile.
// SPECIAL NOTES        : Internal finite native profile; no Core adapter or full XSD 1.1 conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 15-11-2026     | Implement bounded native decimal backend                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Bosak.XPath.Core.Xdm;

namespace Bosak.Schema;

/// <summary>Compiles and assesses the internal finite native decimal schema profile.</summary>
internal sealed class NativeDecimalSchema
{
    private static readonly XNamespace Xs = "http://www.w3.org/2001/XMLSchema";
    private static readonly UTF8Encoding Utf8 = new(false, true);
    private static readonly Regex DecimalLexical = new(@"^[+-]?(?:[0-9]+(?:\.[0-9]*)?|\.[0-9]+)$", RegexOptions.CultureInvariant, TimeSpan.FromSeconds(1));
    private readonly ImmutableArray<NativeDecimalAssertion> assertions;
    private readonly NativeNumericLimits limits;

    private NativeDecimalSchema(NativeDeclaration root, ImmutableArray<NativeDeclaration> children,
        ImmutableArray<NativeDecimalAssertion> assertions, NativeNumericLimits limits)
    {
        Root = root;
        Children = children;
        this.assertions = assertions;
        this.limits = limits;
    }

    internal NativeDeclaration Root { get; }
    internal ImmutableArray<NativeDeclaration> Children { get; }

    internal static NativeResult<NativeDecimalSchema> Compile(string schema, NativeNumericLimits limits, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(schema);
        if (limits.MaxDigits <= 0 || limits.MaxScale < 0) throw new ArgumentOutOfRangeException(nameof(limits));
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var document = Parse(schema, 32768, cancellationToken);
            var root = document.Root!;
            Require(root.Name == Xs + "schema", "Schema root or namespace is unsupported.");
            Attributes(root, "version", "xpathDefaultNamespace");
            Require(root.Attribute("version") is null || (string?)root.Attribute("version") == "1.1", "Only schema version 1.1 is admitted.");
            LocalNamespace(root);
            var global = Single(root, "element");
            Attributes(global, "name");
            var rootName = Name(global);
            var complex = Single(global, "complexType");
            Attributes(complex);
            var members = Elements(complex);
            Require(members.Count is >= 2 and <= 9 && members[0].Name == Xs + "sequence" &&
                members.Skip(1).All(e => e.Name == Xs + "assert"), "Expected sequence then one to eight assertions.");
            var sequence = members[0];
            Attributes(sequence);
            var declarations = Elements(sequence);
            Require(declarations.Count is >= 1 and <= 16, "Expected one to sixteen decimal declarations.");
            var children = ImmutableArray.CreateBuilder<NativeDeclaration>();
            var names = new HashSet<string>(StringComparer.Ordinal);
            var maximum = 0;
            var decimalType = new NativeType("decimal", false);
            foreach (var declaration in declarations)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Require(declaration.Name == Xs + "element", "Only local decimal elements are supported.");
                Attributes(declaration, "name", "type", "minOccurs", "maxOccurs");
                Require(Elements(declaration).Count == 0, "Local declarations must be empty.");
                var name = Name(declaration);
                Require(names.Add(name), "Local declaration names must be unique.");
                Require(IsDecimalType(declaration), "The local type must resolve to xs:decimal.");
                var min = Occurs(declaration, "minOccurs", 1);
                var max = Occurs(declaration, "maxOccurs", 1);
                Require(min is 0 or 1 && max is >= 1 and <= 16 && min <= max, "Occurrence limits are outside the profile.");
                maximum += max;
                Require(maximum <= 16, "Aggregate maximum exceeds sixteen.");
                children.Add(new NativeDeclaration(name, decimalType, min, max));
            }

            var plans = ImmutableArray.CreateBuilder<NativeDecimalAssertion>();
            foreach (var assertion in members.Skip(1))
            {
                cancellationToken.ThrowIfCancellationRequested();
                Attributes(assertion, "test", "xpathDefaultNamespace");
                LocalNamespace(assertion);
                Require(Elements(assertion).Count == 0, "Assertion children are unsupported.");
                var expression = (string?)assertion.Attribute("test");
                Require(expression is not null, "Assertion test is required.");
                plans.Add(NativeDecimalAssertion.Compile(expression!, names, limits));
            }

            return NativeResult<NativeDecimalSchema>.Valid(new NativeDecimalSchema(
                new NativeDeclaration(rootName, new NativeType(null, true), 1, 1),
                children.ToImmutable(), plans.ToImmutable(), limits));
        }
        catch (NativeProfileException ex)
        {
            return NativeResult<NativeDecimalSchema>.Failure(ex.Outcome, ex.Message);
        }
    }

    internal NativeResult<NativeAssessedDocument> Assess(string instance, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(instance);
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var document = Parse(instance, 65536, cancellationToken);
            var root = document.Root!;
            InstanceAttributes(root);
            Check(root.Name == XName.Get(Root.Name), "Unexpected root element.");
            foreach (var text in root.Nodes().OfType<XText>()) Check(IsWhitespace(text.Value), "Element-only root contains non-whitespace text.");
            var values = new Dictionary<string, List<XdmExactDecimal>>(StringComparer.Ordinal);
            var elements = ImmutableArray.CreateBuilder<NativeElementState>();
            elements.Add(new NativeElementState(Root, null));
            var nodes = root.Elements().ToArray();
            var index = 0;
            foreach (var declaration in Children)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var present = new List<XdmExactDecimal>();
                while (index < nodes.Length && nodes[index].Name == XName.Get(declaration.Name))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var leaf = nodes[index++];
                    InstanceAttributes(leaf);
                    Check(!leaf.HasElements, "Decimal leaf contains nested elements.");
                    Require(leaf.Nodes().All(n => n is XText), "Decimal leaf comments or processing instructions are unsupported.");
                    Require(leaf.Value.Length <= 256, "Decimal lexical length exceeds 256 characters.");
                    var value = ParseDecimal(Collapse(leaf.Value), limits, NativeOutcome.Invalid);
                    present.Add(value);
                    Check(present.Count <= declaration.MaxOccurs, "Maximum occurrence exceeded.");
                    elements.Add(new NativeElementState(declaration, value));
                }

                Check(present.Count >= declaration.MinOccurs, "Required child is missing or out of order.");
                values.Add(declaration.Name, present);
            }

            Check(index == nodes.Length, "Unexpected child or sequence order.");
            foreach (var assertion in assertions)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Check(assertion.Evaluate(values), "Assertion evaluated false.");
            }

            cancellationToken.ThrowIfCancellationRequested();
            return NativeResult<NativeAssessedDocument>.Valid(new NativeAssessedDocument(document, elements.ToImmutable()));
        }
        catch (NativeProfileException ex)
        {
            return NativeResult<NativeAssessedDocument>.Failure(ex.Outcome, ex.Message);
        }
    }

    internal static XdmExactDecimal ParseDecimal(string lexical, NativeNumericLimits limits, NativeOutcome lexicalFailure)
    {
        Require(lexical.Length <= 256, "Decimal lexical length exceeds 256 characters.");
        if (!DecimalLexical.IsMatch(lexical)) throw new NativeProfileException(lexicalFailure, "Invalid decimal lexical form.");
        var digits = lexical.Count(c => c is >= '0' and <= '9');
        var point = lexical.IndexOf('.');
        var scale = point < 0 ? 0 : lexical.Length - point - 1;
        Require(digits <= limits.MaxDigits && scale <= limits.MaxScale, "Decimal capacity exceeded before normalization.");
        var coefficient = BigInteger.Parse(lexical.Replace(".", "", StringComparison.Ordinal), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
        return new XdmExactDecimal(coefficient, scale, limits.MaxDigits, limits.MaxScale);
    }

    private static XDocument Parse(string input, int bytes, CancellationToken cancellationToken)
    {
        Require(input.Length <= bytes, "Input length exceeds the byte ceiling.");
        try
        {
            Require(Utf8.GetByteCount(input) <= bytes, "UTF-8 input exceeds the byte ceiling.");
            using var text = new StringReader(input);
            using var reader = XmlReader.Create(text, new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null,
                MaxCharactersInDocument = bytes,
                MaxCharactersFromEntities = 1
            });
            var document = XDocument.Load(reader, LoadOptions.PreserveWhitespace);
            cancellationToken.ThrowIfCancellationRequested();
            Require(document.Declaration is null || document.Declaration.Version == "1.0", "Only XML 1.0 is supported.");
            Require(document.DescendantNodes().Take(257).Count() <= 256, "Descendant node ceiling exceeded.");
            return document;
        }
        catch (EncoderFallbackException)
        {
            throw new NativeProfileException(NativeOutcome.Invalid, "Invalid Unicode input.");
        }
        catch (XmlException)
        {
            var refused = input.Contains("<!DOCTYPE", StringComparison.Ordinal) ||
                Regex.IsMatch(input, """^\s*<\?xml\s+version\s*=\s*['"]1\.[1-9]""", RegexOptions.CultureInvariant, TimeSpan.FromSeconds(1));
            throw new NativeProfileException(refused ? NativeOutcome.Unsupported : NativeOutcome.Invalid, "XML parsing refused the document.");
        }
    }

    private static List<XElement> Elements(XElement element)
    {
        Require(element.Nodes().All(n => n is XElement or XComment or XProcessingInstruction || n is XText text && IsWhitespace(text.Value)),
            "Schema text content is unsupported.");
        return element.Elements().ToList();
    }

    private static XElement Single(XElement element, string name)
    {
        var children = Elements(element);
        Require(children.Count == 1 && children[0].Name == Xs + name, "Unexpected schema child structure.");
        return children[0];
    }

    private static void Attributes(XElement element, params string[] permitted) =>
        Require(element.Attributes().All(a => a.IsNamespaceDeclaration || a.Name.NamespaceName.Length == 0 &&
            permitted.Contains(a.Name.LocalName, StringComparer.Ordinal)), "Unsupported schema attribute.");

    private static void InstanceAttributes(XElement element) =>
        Require(element.Attributes().All(a => a.IsNamespaceDeclaration), "Instance application attributes and xsi requests are unsupported.");

    private static void LocalNamespace(XElement element) =>
        Require(element.Attribute("xpathDefaultNamespace") is null || (string?)element.Attribute("xpathDefaultNamespace") == "##local",
            "Only the local XPath default namespace is supported.");

    private static string Name(XElement element)
    {
        var name = (string?)element.Attribute("name");
        Require(!string.IsNullOrEmpty(name), "Declaration name is required.");
        try { XmlConvert.VerifyNCName(name!); }
        catch (XmlException) { throw new NativeProfileException(NativeOutcome.Unsupported, "Declaration name is not an NCName."); }
        return name!;
    }

    private static bool IsDecimalType(XElement element)
    {
        var type = ((string?)element.Attribute("type"))?.Trim(' ', '\t', '\r', '\n');
        if (type is null) return false;
        var parts = type.Split(':');
        if (parts.Length > 2 || parts[^1] != "decimal") return false;
        if (parts.Length == 1) return element.GetDefaultNamespace() == Xs;
        try { XmlConvert.VerifyNCName(parts[0]); }
        catch (XmlException) { return false; }
        return element.GetNamespaceOfPrefix(parts[0]) == Xs;
    }

    private static int Occurs(XElement element, string name, int fallback)
    {
        var attribute = element.Attribute(name);
        if (attribute is null) return fallback;
        Require(int.TryParse(attribute.Value, NumberStyles.None, CultureInfo.InvariantCulture, out var result), "Occurrence is not a bounded integer.");
        return result;
    }

    private static bool IsWhitespace(string text) => text.All(c => c is ' ' or '\t' or '\r' or '\n');
    private static string Collapse(string text) => string.Join(" ", text.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries));
    internal static void Require(bool condition, string message)
    {
        if (!condition) throw new NativeProfileException(NativeOutcome.Unsupported, message);
    }
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new NativeProfileException(NativeOutcome.Invalid, message);
    }
}

internal enum NativeOutcome { Valid, Invalid, Unsupported }
internal readonly record struct NativeNumericLimits(int MaxDigits, int MaxScale);
internal sealed record NativeResult<T>(NativeOutcome Outcome, T? Value, string? Diagnostic) where T : class
{
    internal static NativeResult<T> Valid(T value) => new(NativeOutcome.Valid, value, null);
    internal static NativeResult<T> Failure(NativeOutcome outcome, string diagnostic) => new(outcome, null, diagnostic);
}

// Identity is deliberate: independently compiled equal schemas do not share components.
internal sealed class NativeType(string? name, bool hasNoTypedValue)
{
    internal string? Name { get; } = name;
    internal bool HasNoTypedValue { get; } = hasNoTypedValue;
}
internal sealed class NativeDeclaration(string name, NativeType type, int minOccurs, int maxOccurs)
{
    internal string Name { get; } = name;
    internal NativeType Type { get; } = type;
    internal int MinOccurs { get; } = minOccurs;
    internal int MaxOccurs { get; } = maxOccurs;
}
internal sealed record NativeElementState(NativeDeclaration Declaration, XdmExactDecimal? TypedValue)
{
    internal bool IsNilled => false;
}
internal sealed class NativeAssessedDocument(XDocument document, ImmutableArray<NativeElementState> elements)
{
    private readonly XDocument document = document;
    internal ImmutableArray<NativeElementState> Elements { get; } = elements;
    internal XDocument CreateDocumentCopy() => new(document);
}
internal sealed class NativeProfileException(NativeOutcome outcome, string message) : Exception(message)
{
    internal NativeOutcome Outcome { get; } = outcome;
}
