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
//                      | Charles Korthout | 0.2   | 15-11-2026     | Share hardened bounded input capture with the internal Core adapter                       |
//                      | Charles Korthout | 0.3   | 19-11-2026     | Carry bounded Unsupported reasons without changing refusal outcomes                       |
//                      | Charles Korthout | 0.4   | 19-11-2026     | Adopt bounded expanded names and optional native assertions                              |
//                      | Charles Korthout | 0.5   | 19-11-2026     | Adopt bounded named complex types and shared expanded component identity                  |
//                      | Charles Korthout | 0.6   | 19-11-2026     | Adopt named decimal restrictions and original named scalar type identity                  |
//                      | Charles Korthout | 0.7   | 19-11-2026     | Share explicit mixed scalar profile with preserved decimal compatibility                  |
//                      | Charles Korthout | 0.8   | 19-11-2026     | Add bounded opt-in Boolean values while preserving decimal and string contracts             |
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

    private ImmutableDictionary<XName, NativeDecimalSchema> roots = ImmutableDictionary<XName, NativeDecimalSchema>.Empty;
    internal IEnumerable<NativeDecimalSchema> Roots => roots.Values;
    internal ImmutableArray<NativeType> NamedTypes { get; private set; } = [];
    internal NativeType DecimalType { get; private set; } = null!;
    internal NativeType? StringType { get; private set; }
    internal NativeType? BooleanType { get; private set; }
    private int? maxStringCharacters;
    internal NativeDeclaration Root { get; }
    internal ImmutableArray<NativeDeclaration> Children { get; }

    internal static NativeResult<NativeDecimalSchema> Compile(string schema, NativeNumericLimits limits, CancellationToken cancellationToken = default, int? maxStringCharacters = null)
    {
        ArgumentNullException.ThrowIfNull(schema);
        if (limits.MaxDigits <= 0 || limits.MaxScale < 0) throw new ArgumentOutOfRangeException(nameof(limits));
        if (maxStringCharacters is <= 0 or > 65536) throw new ArgumentOutOfRangeException(nameof(maxStringCharacters));
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var document = Parse(schema, 32768, cancellationToken);
            var root = document.Root!;
            Require(root.Name == Xs + "schema", "Schema root or namespace is unsupported.", NativeDecimalUnsupportedReason.DeclarationShape);
            Attributes(root, "version", "xpathDefaultNamespace", "targetNamespace", "elementFormDefault");
            Require(root.Attribute("version") is null || (string?)root.Attribute("version") == "1.1", "Only schema version 1.1 is admitted.");
            var targetNamespace = TargetNamespace(root);
            var defaultForm = Form(root, "elementFormDefault", "unqualified");
            var assertionNamespace = AssertionNamespace(root, targetNamespace, "");
            var globals = Elements(root);
            Require(globals.All(e => e.Name == Xs + "element" || e.Name == Xs + "complexType" || e.Name == Xs + "simpleType"),
                "Only global roots and named complex types are supported.", NativeDecimalUnsupportedReason.DeclarationShape);
            var types = globals.Where(e => e.Name == Xs + "complexType" || e.Name == Xs + "simpleType").ToArray();
            var declarations = globals.Where(e => e.Name == Xs + "element").ToArray();
            Require(types.Length <= 16 && declarations.Length <= 16, "Global component ceiling exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            Require(declarations.Length != 0, "At least one root declaration is required.", NativeDecimalUnsupportedReason.DeclarationShape);
            var decimalType = new NativeType("decimal", false, Xs.NamespaceName);
            var stringType = maxStringCharacters is null ? null : new NativeType("string", false, Xs.NamespaceName, isString: true);
            var booleanType = maxStringCharacters is null ? null : new NativeType("boolean", false, Xs.NamespaceName, isBoolean: true);
            var named = new Dictionary<XName, NativeDecimalSchema>();
            var simple = new Dictionary<XName, NativeType>();
            var typeNames = new HashSet<XName>();
            foreach (var type in types)
            {
                Require(typeNames.Add(XName.Get(Name(type, NativeDecimalUnsupportedReason.DeclarationShape), targetNamespace)),
                    "Duplicate named type.", NativeDecimalUnsupportedReason.DeclarationShape);
            }
            // Direct primitive restrictions have no forward dependency on user-defined types.
            foreach (var type in types.Where(t => t.Name == Xs + "simpleType"))
            {
                cancellationToken.ThrowIfCancellationRequested();
                Attributes(type, "name");
                var name = Name(type);
                var restriction = Single(type, "restriction");
                Attributes(restriction, "base");
                Require(ResolveTypeName(restriction, "base") == Xs + "decimal", "Only direct decimal restrictions are admitted.", NativeDecimalUnsupportedReason.ValueType);
                simple.Add(XName.Get(name, targetNamespace), new NativeType(name, false, targetNamespace,
                    NativeDecimalFacets.Compile(restriction, limits)));
            }
            // Compile all complex types eagerly, including unused declarations.
            foreach (var type in types.Where(t => t.Name == Xs + "complexType"))
            {
                cancellationToken.ThrowIfCancellationRequested();
                Attributes(type, "name");
                var name = Name(type);
                var key = XName.Get(name, targetNamespace);
                var identity = new NativeType(name, true, targetNamespace);
                named.Add(key, CompileComplex(type, new NativeDeclaration(name, identity, 1, 1, targetNamespace),
                    targetNamespace, defaultForm, assertionNamespace, decimalType, stringType, booleanType, simple, limits, cancellationToken));
            }
            var compiled = ImmutableDictionary.CreateBuilder<XName, NativeDecimalSchema>();
            NativeDecimalSchema? first = null;
            foreach (var declaration in declarations)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Attributes(declaration, "name", "type");
                var name = Name(declaration);
                var key = XName.Get(name, targetNamespace);
                Require(!compiled.ContainsKey(key), "Duplicate root declaration.", NativeDecimalUnsupportedReason.DeclarationShape);
                NativeDecimalSchema plan;
                if (declaration.Attribute("type") is not null)
                {
                    Require(Elements(declaration).Count == 0, "Inline and named types cannot coexist.", NativeDecimalUnsupportedReason.DeclarationShape);
                    Require(named.TryGetValue(ResolveTypeName(declaration), out var shared), "Unresolved root complex type.", NativeDecimalUnsupportedReason.ValueType);
                    plan = new NativeDecimalSchema(new NativeDeclaration(name, shared!.Root.Type, 1, 1, targetNamespace), shared.Children, shared.assertions, limits);
                }
                else
                {
                    var complex = Single(declaration, "complexType");
                    Attributes(complex);
                    plan = CompileComplex(complex, new NativeDeclaration(name, new NativeType(null, true), 1, 1, targetNamespace),
                        targetNamespace, defaultForm, assertionNamespace, decimalType, stringType, booleanType, simple, limits, cancellationToken);
                }
                compiled.Add(key, plan);
                first ??= plan;
            }
            first!.roots = compiled.ToImmutable();
            first.NamedTypes = simple.Values.Concat(named.Values.Select(p => p.Root.Type)).ToImmutableArray();
            first.DecimalType = decimalType;
            first.StringType = stringType;
            first.BooleanType = booleanType;
            first.maxStringCharacters = maxStringCharacters;
            return NativeResult<NativeDecimalSchema>.Valid(first);
        }
        catch (NativeProfileException ex)
        {
            return NativeResult<NativeDecimalSchema>.Failure(ex.Outcome, ex.Message, ex.UnsupportedReason);
        }
    }

    private static NativeDecimalSchema CompileComplex(XElement complex, NativeDeclaration root,
        string targetNamespace, string defaultForm, string assertionNamespace, NativeType decimalType, NativeType? stringType, NativeType? booleanType,
        Dictionary<XName, NativeType> simple, NativeNumericLimits limits, CancellationToken cancellationToken)
    {
        var members = Elements(complex);
        Require(members.Count is >= 1 and <= 9 && members[0].Name == Xs + "sequence" &&
            members.Skip(1).All(e => e.Name == Xs + "assert"), "Expected sequence then zero to eight assertions.", NativeDecimalUnsupportedReason.DeclarationShape);
        var sequence = members[0];
        Attributes(sequence);
        var declarations = Elements(sequence);
        Require(declarations.Count is >= 1 and <= 16, "Expected one to sixteen decimal declarations.", NativeDecimalUnsupportedReason.DeclarationShape);
        var children = ImmutableArray.CreateBuilder<NativeDeclaration>();
        var names = new HashSet<string>(StringComparer.Ordinal);
        var maximum = 0;
        foreach (var declaration in declarations)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Require(declaration.Name == Xs + "element", "Only local decimal elements are supported.", NativeDecimalUnsupportedReason.DeclarationShape);
            Attributes(declaration, "name", "type", "minOccurs", "maxOccurs", "form");
            Require(Elements(declaration).Count == 0, "Local declarations must be empty.", NativeDecimalUnsupportedReason.DeclarationShape);
            var name = Name(declaration);
            var namespaceUri = Form(declaration, "form", defaultForm) == "qualified" ? targetNamespace : "";
            var expandedName = XName.Get(name, namespaceUri).ToString();
            Require(names.Add(expandedName), "Local declaration names must be unique.", NativeDecimalUnsupportedReason.DeclarationShape);
            NativeType localType;
            if (IsDecimalType(declaration)) localType = decimalType;
            else if (stringType is not null && ResolveTypeName(declaration) == Xs + "string") localType = stringType;
            else if (booleanType is not null && ResolveTypeName(declaration) == Xs + "boolean") localType = booleanType;
            else
            {
                Require(simple.TryGetValue(ResolveTypeName(declaration), out var resolved), "The local type must resolve to an admitted decimal type.", NativeDecimalUnsupportedReason.ValueType);
                localType = resolved!;
            }
            var min = Occurs(declaration, "minOccurs", 1);
            var max = Occurs(declaration, "maxOccurs", 1);
            Require(min is 0 or 1 && max is >= 1 and <= 16 && min <= max, "Occurrence limits are outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
            maximum += max;
            Require(maximum <= 16, "Aggregate maximum exceeds sixteen.", NativeDecimalUnsupportedReason.DeclarationShape);
            children.Add(new NativeDeclaration(name, localType, min, max, namespaceUri));
        }

        var assertionNames = children.Where(c => !c.Type.IsString && !c.Type.IsBoolean).Select(c => c.ExpandedName.ToString()).ToHashSet(StringComparer.Ordinal);
        var plans = ImmutableArray.CreateBuilder<NativeDecimalAssertion>();
        foreach (var assertion in members.Skip(1))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Attributes(assertion, "test", "xpathDefaultNamespace");
            var defaultNamespace = AssertionNamespace(assertion, targetNamespace, assertionNamespace);
            Require(Elements(assertion).Count == 0, "Assertion children are unsupported.", NativeDecimalUnsupportedReason.AssertionGrammar);
            var expression = (string?)assertion.Attribute("test");
            Require(expression is not null, "Assertion test is required.", NativeDecimalUnsupportedReason.AssertionGrammar);
            plans.Add(NativeDecimalAssertion.Compile(expression!, assertionNames, limits,
                name => ResolveAssertionName(assertion, name, defaultNamespace, assertionNames)));
        }

        return new NativeDecimalSchema(root, children.ToImmutable(), plans.ToImmutable(), limits);
    }

    private static XName ResolveTypeName(XElement element, string attribute = "type")
    {
        var token = Collapse((string?)element.Attribute(attribute) ?? "");
        var parts = token.Split(':');
        Require(parts.Length is 1 or 2 && parts.All(p => p.Length != 0), "Malformed type QName.", NativeDecimalUnsupportedReason.ValueType);
        try { foreach (var part in parts) XmlConvert.VerifyNCName(part); }
        catch (XmlException) { throw new NativeProfileException(NativeOutcome.Unsupported, "Malformed type QName.", NativeDecimalUnsupportedReason.ValueType); }
        var ns = parts.Length == 1 ? element.GetDefaultNamespace().NamespaceName : element.GetNamespaceOfPrefix(parts[0])?.NamespaceName;
        Require(ns is not null, "Undefined type prefix.", NativeDecimalUnsupportedReason.ValueType);
        return XName.Get(parts[^1], ns!);
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
            Check(roots.TryGetValue(root.Name, out var selected), "Unexpected root element.");
            foreach (var text in root.Nodes().OfType<XText>()) Check(IsWhitespace(text.Value), "Element-only root contains non-whitespace text.");
            var values = new Dictionary<string, List<XdmExactDecimal>>(StringComparer.Ordinal);
            var elements = ImmutableArray.CreateBuilder<NativeElementState>();
            elements.Add(new NativeElementState(selected!.Root, null));
            var nodes = root.Elements().ToArray();
            var index = 0;
            foreach (var declaration in selected!.Children)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var present = new List<XdmExactDecimal>();
                var occurrences = 0;
                while (index < nodes.Length && nodes[index].Name == declaration.ExpandedName)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var leaf = nodes[index++];
                    InstanceAttributes(leaf);
                    Check(!leaf.HasElements, "Decimal leaf contains nested elements.");
                    Require(leaf.Nodes().All(n => n is XText), "Decimal leaf comments or processing instructions are unsupported.");
                    if (declaration.Type.IsString)
                    {
                        Require(leaf.Value.Length <= maxStringCharacters!.Value, "String capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
                        elements.Add(new NativeElementState(declaration, null, leaf.Value));
                    }
                    else if (declaration.Type.IsBoolean)
                    {
                        Require(leaf.Value.Length <= 256, "Boolean lexical capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
                        var lexical = Collapse(leaf.Value);
                        Check(lexical is "true" or "false" or "1" or "0", "Invalid Boolean lexical form.");
                        elements.Add(new NativeElementState(declaration, null, BooleanValue: lexical is "true" or "1"));
                    }
                    else
                    {
                        Require(leaf.Value.Length <= 256, "Decimal lexical length exceeds 256 characters.", NativeDecimalUnsupportedReason.RepresentationCapacity);
                        var value = ParseDecimal(Collapse(leaf.Value), limits, NativeOutcome.Invalid);
                        Check(declaration.Type.Facets?.Accepts(value) ?? true, "Decimal facet violation.");
                        present.Add(value);
                        elements.Add(new NativeElementState(declaration, value));
                    }
                    Check(++occurrences <= declaration.MaxOccurs, "Maximum occurrence exceeded.");
                }

                Check(occurrences >= declaration.MinOccurs, "Required child is missing or out of order.");
                values.Add(declaration.ExpandedName.ToString(), present);
            }

            Check(index == nodes.Length, "Unexpected child or sequence order.");
            foreach (var assertion in selected!.assertions)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Check(assertion.Evaluate(values), "Assertion evaluated false.");
            }

            cancellationToken.ThrowIfCancellationRequested();
            return NativeResult<NativeAssessedDocument>.Valid(new NativeAssessedDocument(document, elements.ToImmutable()));
        }
        catch (NativeProfileException ex)
        {
            return NativeResult<NativeAssessedDocument>.Failure(ex.Outcome, ex.Message, ex.UnsupportedReason);
        }
    }

    internal static XdmExactDecimal ParseDecimal(string lexical, NativeNumericLimits limits, NativeOutcome lexicalFailure)
    {
        Require(lexical.Length <= 256, "Decimal lexical length exceeds 256 characters.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        if (!DecimalLexical.IsMatch(lexical)) throw new NativeProfileException(lexicalFailure, "Invalid decimal lexical form.");
        var digits = lexical.Count(c => c is >= '0' and <= '9');
        var point = lexical.IndexOf('.');
        var scale = point < 0 ? 0 : lexical.Length - point - 1;
        Require(digits <= limits.MaxDigits && scale <= limits.MaxScale, "Decimal capacity exceeded before normalization.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var coefficient = BigInteger.Parse(lexical.Replace(".", "", StringComparison.Ordinal), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
        return new XdmExactDecimal(coefficient, scale, limits.MaxDigits, limits.MaxScale);
    }

    internal static XDocument Parse(string input, int bytes, CancellationToken cancellationToken)
    {
        Require(input.Length <= bytes, "Input length exceeds the byte ceiling.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        try
        {
            Require(Utf8.GetByteCount(input) <= bytes, "UTF-8 input exceeds the byte ceiling.", NativeDecimalUnsupportedReason.RepresentationCapacity);
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
            Require(document.DescendantNodes().Take(257).Count() <= 256, "Descendant node ceiling exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
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

    internal static List<XElement> Elements(XElement element)
    {
        Require(element.Nodes().All(n => n is XElement or XComment or XProcessingInstruction || n is XText text && IsWhitespace(text.Value)),
            "Schema text content is unsupported.", NativeDecimalUnsupportedReason.DeclarationShape);
        return element.Elements().ToList();
    }

    private static XElement Single(XElement element, string name)
    {
        var children = Elements(element);
        Require(children.Count == 1 && children[0].Name == Xs + name, "Unexpected schema child structure.", NativeDecimalUnsupportedReason.DeclarationShape);
        return children[0];
    }

    internal static void Attributes(XElement element, params string[] permitted)
    {
        var refused = element.Attributes().FirstOrDefault(a => !a.IsNamespaceDeclaration &&
            !(a.Name.NamespaceName.Length == 0 && permitted.Contains(a.Name.LocalName, StringComparer.Ordinal)));
        Require(refused is null, "Unsupported schema attribute.",
            refused is not null && refused.Name.NamespaceName.Length == 0 &&
                refused.Name.LocalName is "targetNamespace" or "elementFormDefault" or "attributeFormDefault"
                ? NativeDecimalUnsupportedReason.Namespace : NativeDecimalUnsupportedReason.Attributes);
    }

    private static void InstanceAttributes(XElement element) =>
        Require(element.Attributes().All(a => a.IsNamespaceDeclaration), "Instance application attributes and xsi requests are unsupported.", NativeDecimalUnsupportedReason.Attributes);

    private static string TargetNamespace(XElement schema)
    {
        var attribute = schema.Attribute("targetNamespace");
        if (attribute is null) return "";
        var value = Collapse(attribute.Value);
        Require(value.Length != 0 && value is not "http://www.w3.org/2001/XMLSchema" and
            not "http://www.w3.org/2001/XMLSchema-instance" and not "http://www.w3.org/XML/1998/namespace" and
            not "http://www.w3.org/2000/xmlns/", "Target namespace is outside the admitted profile.", NativeDecimalUnsupportedReason.Namespace);
        return NamespaceUri(value);
    }

    private static string NamespaceUri(string value)
    {
        Require(!value.Any(char.IsWhiteSpace) && Uri.TryCreate(value, UriKind.Absolute, out _),
            "Only absolute namespace URIs without whitespace are admitted.", NativeDecimalUnsupportedReason.Namespace);
        return value; // Compare supplied strings, not Uri normalization or resource identities.
    }

    private static string Form(XElement element, string attribute, string fallback)
    {
        var value = element.Attribute(attribute);
        if (value is null) return fallback;
        var form = Collapse(value.Value);
        Require(form is "qualified" or "unqualified", "Element form is outside the admitted profile.", NativeDecimalUnsupportedReason.Namespace);
        return form;
    }

    private static string AssertionNamespace(XElement element, string targetNamespace, string inherited)
    {
        var value = element.Attribute("xpathDefaultNamespace");
        if (value is null) return inherited;
        var setting = Collapse(value.Value);
        return setting switch
        {
            "##local" => "",
            "##targetNamespace" => targetNamespace,
            "##defaultNamespace" => element.GetDefaultNamespace().NamespaceName,
            _ => NamespaceUri(setting)
        };
    }

    private static string? ResolveAssertionName(XElement assertion, string token, string defaultNamespace, HashSet<string> names)
    {
        var parts = token.Split(':');
        if (parts.Length is < 1 or > 2 || parts.Any(part => part.Length == 0)) return null;
        try { foreach (var part in parts) XmlConvert.VerifyNCName(part); }
        catch (XmlException) { return null; }
        var ns = parts.Length == 1 ? defaultNamespace : assertion.GetNamespaceOfPrefix(parts[0])?.NamespaceName;
        if (ns is null) return null;
        var name = XName.Get(parts[^1], ns).ToString();
        return names.Contains(name) ? name : null;
    }

    private static string Name(XElement element, NativeDecimalUnsupportedReason invalidReason = NativeDecimalUnsupportedReason.Unspecified)
    {
        var name = (string?)element.Attribute("name");
        Require(!string.IsNullOrEmpty(name), "Declaration name is required.", NativeDecimalUnsupportedReason.DeclarationShape);
        try { XmlConvert.VerifyNCName(name!); }
        catch (XmlException) { throw new NativeProfileException(NativeOutcome.Unsupported, "Declaration name is not an NCName.", invalidReason); }
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
        Require(int.TryParse(attribute.Value, NumberStyles.None, CultureInfo.InvariantCulture, out var result), "Occurrence is not a bounded integer.", NativeDecimalUnsupportedReason.DeclarationShape);
        return result;
    }

    private static bool IsWhitespace(string text) => text.All(c => c is ' ' or '\t' or '\r' or '\n');
    private static string Collapse(string text) => string.Join(" ", text.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries));
    internal static void Require(bool condition, string message,
        NativeDecimalUnsupportedReason reason = NativeDecimalUnsupportedReason.Unspecified)
    {
        if (!condition) throw new NativeProfileException(NativeOutcome.Unsupported, message, reason);
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
    internal NativeDecimalUnsupportedReason UnsupportedReason { get; init; }
    internal static NativeResult<T> Failure(NativeOutcome outcome, string diagnostic,
        NativeDecimalUnsupportedReason reason = NativeDecimalUnsupportedReason.Unspecified) =>
        new(outcome, null, diagnostic) { UnsupportedReason = outcome == NativeOutcome.Unsupported ? reason : NativeDecimalUnsupportedReason.Unspecified };
}

// Identity is deliberate: independently compiled equal schemas do not share components.
internal sealed class NativeType(string? name, bool hasNoTypedValue, string namespaceUri = "", NativeDecimalFacets? facets = null, bool isString = false, bool isBoolean = false)
{
    internal bool IsBoolean { get; } = isBoolean;
    internal bool IsString { get; } = isString;
    internal NativeDecimalFacets? Facets { get; } = facets;
    internal string? Name { get; } = name;
    internal bool HasNoTypedValue { get; } = hasNoTypedValue;
    internal string NamespaceUri { get; } = namespaceUri;
}
internal sealed class NativeDeclaration(string name, NativeType type, int minOccurs, int maxOccurs, string namespaceUri = "")
{
    internal string Name { get; } = name;
    internal string NamespaceUri { get; } = namespaceUri;
    internal XName ExpandedName { get; } = XName.Get(name, namespaceUri);
    internal NativeType Type { get; } = type;
    internal int MinOccurs { get; } = minOccurs;
    internal int MaxOccurs { get; } = maxOccurs;
}
internal sealed record NativeElementState(NativeDeclaration Declaration, XdmExactDecimal? TypedValue, string? StringValue = null, bool? BooleanValue = null)
{
    internal bool IsNilled => false;
}
internal sealed class NativeAssessedDocument(XDocument document, ImmutableArray<NativeElementState> elements)
{
    private readonly XDocument document = document;
    internal ImmutableArray<NativeElementState> Elements { get; } = elements;
    internal XDocument CreateDocumentCopy() => new(document);
}
internal sealed class NativeProfileException(NativeOutcome outcome, string message,
    NativeDecimalUnsupportedReason reason = NativeDecimalUnsupportedReason.Unspecified) : Exception(message)
{
    internal NativeOutcome Outcome { get; } = outcome;
    internal NativeDecimalUnsupportedReason UnsupportedReason { get; } = reason;
}
