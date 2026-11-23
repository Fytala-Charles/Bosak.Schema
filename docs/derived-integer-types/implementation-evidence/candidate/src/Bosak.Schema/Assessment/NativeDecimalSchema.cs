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
//                      | Charles Korthout | 0.9   | 19-11-2026     | Add bounded exact integer leaves with original typed provenance                           |
//                      | Charles Korthout | 1.0   | 19-11-2026     | Add bounded named string restrictions with Unicode length and exact enumeration facets      |
//                      | Charles Korthout | 1.1   | 19-11-2026     | Adopt opt-in local root attributes without changing leaf indexes or legacy profiles         |
//                      | Charles Korthout | 1.2   | 20-11-2026     | Adopt bounded decimal simple-content roots with original complex type identity             |
//                      | Charles Korthout | 1.3   | 20-11-2026     | Adopt bounded nested sequences and preserve complete original element assessment         |
//                      | Charles Korthout | 1.4   | 20-11-2026     | Capture original nested attribute owner paths and indexes without changing scalar order  |
//                      | Charles Korthout | 1.5   | 20-11-2026     | Admit unfetched location hints only under the explicitly supplied mixed scalar schema    |
//                      | Charles Korthout | 1.6   | 20-11-2026     | Reuse direct facets for original anonymous local decimal and string definitions          |
//                      | Charles Korthout | 1.7   | 20-11-2026     | Enforce exact integer facets on original element and attribute scalar facts                 |
//                      | Charles Korthout | 1.8   | 20-11-2026     | Assess bounded string assertions from original immediate scalar child values             |
//                      | Charles Korthout | 1.9   | 21-11-2026     | Adopt bounded instance selection with original declaration and governing type identity |
//                      | Charles Korthout | 1.10  | 21-11-2026     | Adopt mixed relative target namespaces and form defaults independently of attribute uses     |
//                      | Charles Korthout | 1.11  | 21-11-2026     | Adopt bounded complex restriction with original and atomic component identities |
//                      | Charles Korthout | 1.12  | 21-11-2026     | Adopt bounded integer simple content with actual component identity |
//                      | Charles Korthout | 1.13  | 21-11-2026     | Adopt bounded contextual and inherited integer assertions |
//                      | Charles Korthout | 1.14   | 21-11-2026     | Probe empty complex typing in isolated copies only |
//                      | Charles Korthout | 1.15   | 22-11-2026     | Probe bounded structural assertions in isolated copies |
//                      | Charles Korthout | 1.16   | 22-11-2026     | Adopt qualified bounded structural and empty-content semantics |
//                      | Charles Korthout | 1.17   | 22-11-2026     | Prototype bounded string patterns and per-assessment accounting |
//                      | Charles Korthout | 1.18   | 22-11-2026     | Adopt qualified bounded native string patterns |
//                      | Charles Korthout | 1.19   | 22-11-2026     | Probe simple scalar roots in isolated source copies only |
//                      | Charles Korthout | 1.20   | 22-11-2026     | Adopt qualified native simple scalar root assessment |
//                      | Charles Korthout | 1.21   | 22-11-2026     | Probe derived integer builtins in isolated source copies |
//                      | Charles Korthout | 1.22   | 22-11-2026     | Adopt qualified bounded builtin integer family |
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
internal sealed partial class NativeDecimalSchema
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
    internal NativeType? IntegerType { get; private set; }
    private int? maxStringCharacters;
    internal NativeDeclaration Root { get; }
    internal ImmutableArray<NativeDeclaration> Children { get; }
    internal ImmutableArray<NativeDeclaration> AttributeUses { get; private set; } = [];

    internal static NativeResult<NativeDecimalSchema> Compile(string schema, NativeNumericLimits limits, CancellationToken cancellationToken = default, int? maxStringCharacters = null)
    {
        ArgumentNullException.ThrowIfNull(schema);
        if (limits.MaxDigits <= 0 || limits.MaxScale < 0) throw new ArgumentOutOfRangeException(nameof(limits));
        if (maxStringCharacters is <= 0 or > 65536) throw new ArgumentOutOfRangeException(nameof(maxStringCharacters));
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var patternCompilation = new NativePatternCompilation(cancellationToken);
            var document = Parse(schema, 32768, cancellationToken);
            var root = document.Root!;
            Require(root.Name == Xs + "schema", "Schema root or namespace is unsupported.", NativeDecimalUnsupportedReason.DeclarationShape);
            var mixedProfile = maxStringCharacters is not null;
            if (mixedProfile) Attributes(root, "version", "xpathDefaultNamespace", "targetNamespace", "elementFormDefault", "attributeFormDefault");
            else Attributes(root, "version", "xpathDefaultNamespace", "targetNamespace", "elementFormDefault");
            var attributeForm = mixedProfile ? Form(root, "attributeFormDefault", "unqualified") : "unqualified";
            Require(root.Attribute("version") is null || (string?)root.Attribute("version") == "1.1", "Only schema version 1.1 is admitted.");
            var targetNamespace = TargetNamespace(root, mixedProfile);
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
            var integerType = maxStringCharacters is null ? null : new NativeType("integer", false, Xs.NamespaceName, isInteger: true);
            var named = new Dictionary<XName, NativeDecimalSchema>();
            var simple = CreateBuiltinIntegerTypes(integerType);
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
                var restricted = CompileRestriction(type, name, targetNamespace, limits, maxStringCharacters, patternCompilation, simple);
                simple.Add(XName.Get(name, targetNamespace), restricted);
            }
            var anonymousSimple = CompileAnonymousRestrictions(root, limits, maxStringCharacters, cancellationToken, patternCompilation, simple);
            if (stringType is not null && HasNestedDeclarations(root, types, targetNamespace))
                return NativeResult<NativeDecimalSchema>.Valid(CompileNestedSchema(root, types, declarations, targetNamespace,
                    defaultForm, attributeForm, assertionNamespace, decimalType, stringType, booleanType!, integerType!, simple,
                    limits, maxStringCharacters!.Value, cancellationToken, anonymousSimple));
            // Compile all complex types eagerly, including unused declarations.
            var complexDefinitions = types.Where(t => t.Name == Xs + "complexType")
                .ToDictionary(t => XName.Get(Name(t), targetNamespace));
            var resolvingComplex = new HashSet<XName>();
            NativeDecimalSchema ResolveComplex(XName key)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (named.TryGetValue(key, out var known)) return known;
                Require(resolvingComplex.Add(key), "Complex dependency cycle is outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
                var type = complexDefinitions[key];
                Attributes(type, "name");
                var name = Name(type);
                var identity = new NativeType(name, true, targetNamespace);
                var plan = CompileComplex(type, new NativeDeclaration(name, identity, 1, 1, targetNamespace),
                    targetNamespace, defaultForm, attributeForm, assertionNamespace, decimalType, stringType, booleanType, integerType,
                    simple, limits, cancellationToken, anonymousSimple,
                    requested => complexDefinitions.ContainsKey(requested) ? ResolveComplex(requested) : null);
                resolvingComplex.Remove(key);
                named.Add(key, plan);
                return plan;
            }
            foreach (var key in complexDefinitions.Keys) ResolveComplex(key);
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
                var scalarRoot = TryCompileSimpleRoot(declaration, targetNamespace, decimalType, stringType, booleanType, integerType, simple, anonymousSimple, limits);
                if (scalarRoot is not null) plan = scalarRoot;
                else if (declaration.Attribute("type") is not null)
                {
                    Require(Elements(declaration).Count == 0, "Inline and named types cannot coexist.", NativeDecimalUnsupportedReason.DeclarationShape);
                    Require(named.TryGetValue(ResolveTypeName(declaration), out var shared), "Unresolved root complex type.", NativeDecimalUnsupportedReason.ValueType);
                    plan = new NativeDecimalSchema(new NativeDeclaration(name, shared!.Root.Type, 1, 1, targetNamespace), shared.Children, shared.assertions, limits) { AttributeUses = shared.AttributeUses, integerAssertions = shared.integerAssertions, structuralAssertions = shared.structuralAssertions, NestedProfile = shared.NestedProfile };
                }
                else
                {
                    var complex = Single(declaration, "complexType");
                    Attributes(complex);
                    plan = CompileComplex(complex, new NativeDeclaration(name, new NativeType(null, true), 1, 1, targetNamespace),
                        targetNamespace, defaultForm, attributeForm, assertionNamespace, decimalType, stringType, booleanType, integerType, simple, limits, cancellationToken, anonymousSimple, requested => complexDefinitions.ContainsKey(requested) ? ResolveComplex(requested) : null);
                }
                compiled.Add(key, plan);
                first ??= plan;
            }
            first!.roots = compiled.ToImmutable();
            first.NamedTypes = simple.Values.Concat(named.Values.Select(p => p.Root.Type)).ToImmutableArray();
            first.DecimalType = decimalType;
            first.StringType = stringType;
            first.BooleanType = booleanType;
            first.IntegerType = integerType;
            first.maxStringCharacters = maxStringCharacters;
            first.PrepareInstanceTypes();
            return NativeResult<NativeDecimalSchema>.Valid(first);
        }
        catch (NativeProfileException ex)
        {
            return NativeResult<NativeDecimalSchema>.Failure(ex.Outcome, ex.Message, ex.UnsupportedReason);
        }
    }

    private static NativeDecimalSchema? TryCompileEmpty(XElement complex, NativeDeclaration root, NativeNumericLimits limits, string targetNamespace, string assertionNamespace, CancellationToken cancellationToken)
    {
        var members = Elements(complex);
        var hasSequence = members.Count != 0 && members[0].Name == Xs + "sequence";
        if (hasSequence && Elements(members[0]).Count != 0) return null;
        var assertions = members.Skip(hasSequence ? 1 : 0).ToArray();
        if (assertions.Any(a => a.Name != Xs + "assert")) return null;
        if (hasSequence) Attributes(members[0]);
        var type = new NativeType(root.Type.Name, false, root.Type.NamespaceUri) { EmptyContent = true };
        return new NativeDecimalSchema(new NativeDeclaration(root.Name, type, root.MinOccurs, root.MaxOccurs, root.NamespaceUri), [], [], limits) { NestedProfile = true, structuralAssertions = CompileStructuralAssertions(assertions, targetNamespace, assertionNamespace, cancellationToken) };
    }

    private static NativeDecimalSchema CompileComplex(XElement complex, NativeDeclaration root,
        string targetNamespace, string defaultForm, string attributeForm, string assertionNamespace, NativeType decimalType, NativeType? stringType, NativeType? booleanType, NativeType? integerType,
        Dictionary<XName, NativeType> simple, NativeNumericLimits limits, CancellationToken cancellationToken, Dictionary<XElement, NativeType> anonymousSimple, Func<XName, NativeDecimalSchema?>? complexResolver = null, bool nestedContext = false)
    {
        var members = Elements(complex);
        if (stringType is not null && TryCompileEmpty(complex, root, limits, targetNamespace, assertionNamespace, cancellationToken) is { } empty) return empty;
        if (stringType is not null && members.Count == 1 && members[0].Name == Xs + "simpleContent")
            return CompileSimpleContent(members[0], root, targetNamespace, attributeForm, decimalType, stringType, booleanType, integerType, simple, limits, cancellationToken, complexResolver, assertionNamespace, nestedContext);
        var attributeMembers = members.Skip(1).TakeWhile(e => e.Name == Xs + "attribute").ToArray();
        var assertionMembers = members.Skip(1 + attributeMembers.Length).ToArray();
        if (assertionMembers.Any(IsStructuralAssertion))
        {
            Require(stringType is not null, "Structural assertions require the opt-in profile.", NativeDecimalUnsupportedReason.AssertionGrammar);
            Require(attributeMembers.Length == 0, "Structural assertions with application attributes are excluded.", NativeDecimalUnsupportedReason.Attributes);
            Require(assertionMembers.Length <= 8, "Structural assertion capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        }
        Require(members.Count >= 1 && members[0].Name == Xs + "sequence" && assertionMembers.Length <= 8 &&
            assertionMembers.All(e => e.Name == Xs + "assert") && (stringType is not null || attributeMembers.Length == 0),
            "Expected sequence, admitted attributes then zero to eight assertions.", NativeDecimalUnsupportedReason.DeclarationShape);
        Require(attributeMembers.Length <= 16, "Root attribute use ceiling exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
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
            var inline = Elements(declaration);
            var name = Name(declaration);
            var namespaceUri = Form(declaration, "form", defaultForm) == "qualified" ? targetNamespace : "";
            var expandedName = XName.Get(name, namespaceUri).ToString();
            Require(names.Add(expandedName), "Local declaration names must be unique.", NativeDecimalUnsupportedReason.DeclarationShape);
            NativeType localType;
            if (inline.Count == 0) localType = ResolveScalarType(declaration, decimalType, stringType, booleanType, integerType, simple);
            else
            {
                Require(inline.Count == 1 && declaration.Attribute("type") is null && anonymousSimple.ContainsKey(inline[0]),
                    "Expected one admitted anonymous simple type.", NativeDecimalUnsupportedReason.DeclarationShape);
                localType = anonymousSimple[inline[0]];
            }
            var min = Occurs(declaration, "minOccurs", 1);
            var max = Occurs(declaration, "maxOccurs", 1);
            Require(min is 0 or 1 && max is >= 1 and <= 16 && min <= max, "Occurrence limits are outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
            maximum += max;
            Require(maximum <= 16, "Aggregate maximum exceeds sixteen.", NativeDecimalUnsupportedReason.DeclarationShape);
            children.Add(new NativeDeclaration(name, localType, min, max, namespaceUri));
        }

        var attributeUses = CompileAttributes(attributeMembers, targetNamespace, attributeForm, decimalType, stringType, booleanType, integerType, simple, cancellationToken);

        var assertionNames = children.Where(c => !c.Type.IsBoolean && !c.Type.IsInteger).Select(c => c.ExpandedName.ToString()).ToHashSet(StringComparer.Ordinal);
        var stringNames = stringType is null ? null : children.Where(c => c.Type.IsString).Select(c => c.ExpandedName.ToString()).ToHashSet(StringComparer.Ordinal);
        var structuralPlans = CompileStructuralAssertions(assertionMembers.Where(IsStructuralAssertion), targetNamespace, assertionNamespace, cancellationToken);
        var plans = ImmutableArray.CreateBuilder<NativeDecimalAssertion>();
        foreach (var assertion in assertionMembers.Where(a => !IsStructuralAssertion(a)))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Attributes(assertion, "test", "xpathDefaultNamespace");
            var defaultNamespace = AssertionNamespace(assertion, targetNamespace, assertionNamespace);
            Require(Elements(assertion).Count == 0, "Assertion children are unsupported.", NativeDecimalUnsupportedReason.AssertionGrammar);
            var expression = (string?)assertion.Attribute("test");
            Require(expression is not null, "Assertion test is required.", NativeDecimalUnsupportedReason.AssertionGrammar);
            plans.Add(NativeDecimalAssertion.Compile(expression!, assertionNames, limits,
                name => ResolveAssertionName(assertion, name, defaultNamespace, assertionNames), stringNames));
        }

        return new NativeDecimalSchema(root, children.ToImmutable(), plans.ToImmutable(), limits) { AttributeUses = attributeUses, structuralAssertions = structuralPlans };
    }

    private static ImmutableArray<NativeDeclaration> CompileAttributes(IReadOnlyList<XElement> attributeMembers,
        string targetNamespace, string attributeForm, NativeType decimalType, NativeType? stringType, NativeType? booleanType,
        NativeType? integerType, Dictionary<XName, NativeType> simple, CancellationToken cancellationToken)
    {
        Require(attributeMembers.All(e => e.Name == Xs + "attribute"), "Only local attributes are admitted here.", NativeDecimalUnsupportedReason.DeclarationShape);
        Require(attributeMembers.Count <= 16, "Root attribute use ceiling exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var attributeUses = ImmutableArray.CreateBuilder<NativeDeclaration>();
        var attributeNames = new HashSet<XName>();
        foreach (var attribute in attributeMembers)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Attributes(attribute, "name", "type", "use", "form");
            Require(Elements(attribute).Count == 0, "Attribute children are unsupported.", NativeDecimalUnsupportedReason.DeclarationShape);
            var name = Name(attribute, NativeDecimalUnsupportedReason.DeclarationShape);
            var ns = Form(attribute, "form", attributeForm) == "qualified" ? targetNamespace : "";
            Require(name != "xmlns" && ns is not "http://www.w3.org/2000/xmlns/" and not "http://www.w3.org/2001/XMLSchema-instance" and not "http://www.w3.org/XML/1998/namespace",
                "Reserved attribute declaration.", NativeDecimalUnsupportedReason.Attributes);
            Require(attributeNames.Add(XName.Get(name, ns)), "Duplicate expanded attribute declaration.", NativeDecimalUnsupportedReason.DeclarationShape);
            var use = Collapse((string?)attribute.Attribute("use") ?? "optional");
            Require(use is "optional" or "required", "Unsupported attribute use.", NativeDecimalUnsupportedReason.DeclarationShape);
            var type = ResolveScalarType(attribute, decimalType, stringType, booleanType, integerType, simple);
            attributeUses.Add(new NativeDeclaration(name, type, use == "required" ? 1 : 0, 1, ns, isAttribute: true));
        }

        return attributeUses.ToImmutable();
    }

    private static NativeDecimalSchema CompileSimpleContent(XElement content, NativeDeclaration root,
        string targetNamespace, string attributeForm, NativeType decimalType, NativeType stringType, NativeType? booleanType,
        NativeType? integerType, Dictionary<XName, NativeType> simple, NativeNumericLimits limits, CancellationToken cancellationToken, Func<XName, NativeDecimalSchema?>? complexResolver, string assertionNamespace, bool nestedContext)
    {
        Attributes(content);
        var contentMembers = Elements(content);
        if (contentMembers.Count == 1 && contentMembers[0].Name == Xs + "restriction")
            return CompileComplexRestriction(contentMembers[0], root, targetNamespace, simple, limits, complexResolver, cancellationToken, assertionNamespace);
        var extension = Single(content, "extension");
        Attributes(extension, "base");
        var baseName = ResolveTypeName(extension, "base");
        NativeType? baseType = baseName == Xs + "decimal" ? decimalType : baseName == Xs + "integer" ? integerType : simple.GetValueOrDefault(baseName);
        if (nestedContext && baseType?.IsInteger != true && Elements(extension).Any(e => e.Name == Xs + "assert"))
            Require(false, "Noninteger nested assertions are outside the profile.", NativeDecimalUnsupportedReason.AssertionGrammar);
        Require(baseType is not null && !baseType.IsString && !baseType.IsBoolean,
            "Only direct admitted decimal simple-content bases are supported.", NativeDecimalUnsupportedReason.ValueType);
        if (baseType!.IsInteger)
        {
            var children = Elements(extension);
            Require(!children.Any(e => e.Name == Xs + "attribute" || e.Name == Xs + "attributeGroup" || e.Name == Xs + "anyAttribute"), "Integer extension attributes are outside the profile.", NativeDecimalUnsupportedReason.Attributes);
            Require(children.All(e => e.Name == Xs + "assert"), "Integer extension children are outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
            var integerTypePlan = new NativeType(root.Type.Name, false, root.Type.NamespaceUri, contentType: baseType);
            return new NativeDecimalSchema(new NativeDeclaration(root.Name, integerTypePlan, root.MinOccurs, root.MaxOccurs, root.NamespaceUri), [], [], limits)
            { integerAssertions = CompileIntegerAssertions(children, targetNamespace, assertionNamespace, limits, cancellationToken) };
        }
        var uses = CompileAttributes(Elements(extension), targetNamespace, attributeForm, decimalType, stringType, booleanType, integerType, simple, cancellationToken);
        var type = new NativeType(root.Type.Name, false, root.Type.NamespaceUri, contentType: baseType);
        return new NativeDecimalSchema(new NativeDeclaration(root.Name, type, root.MinOccurs, root.MaxOccurs, root.NamespaceUri), [], [], limits) { AttributeUses = uses };
    }

    private static NativeType ResolveScalarType(XElement declaration, NativeType decimalType, NativeType? stringType,
        NativeType? booleanType, NativeType? integerType, Dictionary<XName, NativeType> simple)
    {
        if (IsDecimalType(declaration)) return decimalType;
        var name = ResolveTypeName(declaration);
        if (stringType is not null && name == Xs + "string") return stringType;
        if (booleanType is not null && name == Xs + "boolean") return booleanType;
        if (integerType is not null && name == Xs + "integer") return integerType;
        Require(simple.TryGetValue(name, out var resolved), "The local type must resolve to an admitted scalar type.", NativeDecimalUnsupportedReason.ValueType);
        return resolved!;
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
            var patternBudget = new NativePatternBudget(cancellationToken);
            var document = Parse(instance, 65536, cancellationToken);
            var root = document.Root!;
            var found = roots.TryGetValue(root.Name, out var selected);
            var rootType = found ? ResolveInstanceType(root, selected!.Root.Type) : null;
            var attributeStates = ImmutableArray.CreateBuilder<NativeElementState>();
            if (found && selected!.AttributeUses.Length != 0)
            {
                Require(root.Attributes().All(a => IsAdmittedInstanceAttribute(a) || a.Name.NamespaceName is not "http://www.w3.org/2001/XMLSchema-instance" and not "http://www.w3.org/XML/1998/namespace"),
                    "Special attribute requests are unsupported.", NativeDecimalUnsupportedReason.Attributes);
                foreach (var use in selected.AttributeUses)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var attribute = root.Attribute(use.ExpandedName);
                    Check(attribute is not null || use.MinOccurs == 0, "Required root attribute is missing.");
                    if (attribute is not null) attributeStates.Add(AssessScalar(attribute.Value, use, patternBudget));
                }
                Check(root.Attributes().All(a => IsAdmittedInstanceAttribute(a) || selected.AttributeUses.Any(u => u.ExpandedName == a.Name)), "Undeclared root attribute.");
            }
            else InstanceAttributes(root);
            Check(found, "Unexpected root element.");
            if (!selected!.Root.Type.IsComplex)
            {
                Check(!root.HasElements, "Simple root cannot contain child elements.");
                Require(root.Nodes().All(n => n is XText), "Simple-root comments or processing instructions are unsupported.");
                var scalar = AssessElementScalar(root, selected.Root, patternBudget) with
                { NormalizedStringValue = rootType!.IsString ? root.Value : Collapse(root.Value) };
                cancellationToken.ThrowIfCancellationRequested();
                return NativeResult<NativeAssessedDocument>.Valid(new NativeAssessedDocument(document, [scalar], []));
            }
            if (selected.Root.Type.ContentType is { } contentType)
            {
                Check(!root.HasElements, "Simple-content root cannot contain child elements.");
                Require(root.Nodes().All(n => n is XText), "Simple-content comments or processing instructions are unsupported.");
                var scalarDeclaration = new NativeDeclaration(selected.Root.Name, contentType, 1, 1, selected.Root.NamespaceUri);
                var state = AssessScalar(root.Value, scalarDeclaration, patternBudget) with { Declaration = selected.Root, EffectiveType = rootType, NormalizedStringValue = Collapse(root.Value) };
                selected.AssessIntegerAssertions(root, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return NativeResult<NativeAssessedDocument>.Valid(new NativeAssessedDocument(document, [state], attributeStates.ToImmutable()));
            }
            if (selected.NestedProfile)
            {
                var nestedElements = ImmutableArray.CreateBuilder<NativeElementState>();
                AssessContainer(root, selected.Root, selected, nestedElements, attributeStates, [], cancellationToken, patternBudget, isRoot: true);
                cancellationToken.ThrowIfCancellationRequested();
                return NativeResult<NativeAssessedDocument>.Valid(new NativeAssessedDocument(document, nestedElements.ToImmutable(), attributeStates.ToImmutable()));
            }
            foreach (var text in root.Nodes().OfType<XText>()) Check(IsWhitespace(text.Value), "Element-only root contains non-whitespace text.");
            var values = new Dictionary<string, List<XdmExactDecimal>>(StringComparer.Ordinal);
            var strings = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            var elements = ImmutableArray.CreateBuilder<NativeElementState>();
            elements.Add(new NativeElementState(selected!.Root, null) { EffectiveType = rootType });
            var nodes = root.Elements().ToArray();
            var index = 0;
            foreach (var declaration in selected!.Children)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var present = new List<XdmExactDecimal>();
                var textValues = new List<string>();
                var occurrences = 0;
                while (index < nodes.Length && nodes[index].Name == declaration.ExpandedName)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var leaf = nodes[index++];
                    InstanceAttributes(leaf);
                    Check(!leaf.HasElements, "Decimal leaf contains nested elements.");
                    Require(leaf.Nodes().All(n => n is XText), "Decimal leaf comments or processing instructions are unsupported.");
                    var state = AssessElementScalar(leaf, declaration, patternBudget);
                    if (state.TypedValue is { } numeric) present.Add(numeric);
                    if (state.StringValue is { } textValue) textValues.Add(textValue);
                    elements.Add(state);
                    Check(++occurrences <= declaration.MaxOccurs, "Maximum occurrence exceeded.");
                }

                Check(occurrences >= declaration.MinOccurs, "Required child is missing or out of order.");
                values.Add(declaration.ExpandedName.ToString(), present);
                if (declaration.Type.IsString) strings.Add(declaration.ExpandedName.ToString(), textValues);
            }

            Check(index == nodes.Length, "Unexpected child or sequence order.");
            selected!.AssessStructuralAssertions(root, cancellationToken);
            foreach (var assertion in selected.assertions)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Check(assertion.Evaluate(values, strings), "Assertion evaluated false.");
            }

            cancellationToken.ThrowIfCancellationRequested();
            return NativeResult<NativeAssessedDocument>.Valid(new NativeAssessedDocument(document, elements.ToImmutable(), attributeStates.ToImmutable()));
        }
        catch (NativeProfileException ex)
        {
            return NativeResult<NativeAssessedDocument>.Failure(ex.Outcome, ex.Message, ex.UnsupportedReason);
        }
    }

    private NativeElementState AssessScalar(string text, NativeDeclaration declaration, NativePatternBudget patternBudget)
    {
        if (declaration.Type.IsString)
        {
            Require(text.Length <= maxStringCharacters!.Value, "String capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            Check(declaration.Type.StringFacets?.Accepts(text, patternBudget) ?? true, "String facet violation.");
            return new NativeElementState(declaration, null, text);
        }
        else if (declaration.Type.IsBoolean)
        {
            Require(text.Length <= 256, "Boolean lexical capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            var lexical = Collapse(text);
            Check(lexical is "true" or "false" or "1" or "0", "Invalid Boolean lexical form.");
            return new NativeElementState(declaration, null, BooleanValue: lexical is "true" or "1");
        }
        else if (declaration.Type.IsInteger)
        {
            var value = NativeIntegerFacets.ParseValue(text, limits, NativeOutcome.Invalid);
            Check(declaration.Type.IntegerFacets?.Accepts(value) ?? true, "Integer facet violation.");
            return new NativeElementState(declaration, null, IntegerValue: value);
        }
        else
        {
            Require(text.Length <= 256, "Decimal lexical length exceeds 256 characters.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            var value = ParseDecimal(Collapse(text), limits, NativeOutcome.Invalid);
            Check(declaration.Type.Facets?.Accepts(value) ?? true, "Decimal facet violation.");
            return new NativeElementState(declaration, value);
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

    private bool IsNamespaceOrLocationHint(XAttribute attribute) => attribute.IsNamespaceDeclaration ||
        maxStringCharacters is not null && attribute.Name.NamespaceName == "http://www.w3.org/2001/XMLSchema-instance" &&
        attribute.Name.LocalName is "schemaLocation" or "noNamespaceSchemaLocation";

    private void InstanceAttributes(XElement element) =>
        Require(element.Attributes().All(IsAdmittedInstanceAttribute), "Instance application attributes and xsi requests are unsupported.", NativeDecimalUnsupportedReason.Attributes);

    private static string TargetNamespace(XElement schema, bool allowRelative)
    {
        var attribute = schema.Attribute("targetNamespace");
        if (attribute is null) return "";
        var value = Collapse(attribute.Value);
        Require(value.Length != 0 && value is not "http://www.w3.org/2001/XMLSchema" and
            not "http://www.w3.org/2001/XMLSchema-instance" and not "http://www.w3.org/XML/1998/namespace" and
            not "http://www.w3.org/2000/xmlns/", "Target namespace is outside the admitted profile.", NativeDecimalUnsupportedReason.Namespace);
        return NamespaceUri(value, allowRelative);
    }

    private static string NamespaceUri(string value, bool allowRelative = false)
    {
        Require(!value.Any(char.IsWhiteSpace) && Uri.TryCreate(value, allowRelative ? UriKind.RelativeOrAbsolute : UriKind.Absolute, out _),
            allowRelative ? "Namespace reference is outside the admitted lexical profile." :
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
internal sealed class NativeType(string? name, bool hasNoTypedValue, string namespaceUri = "", NativeDecimalFacets? facets = null, bool isString = false, bool isBoolean = false, bool isInteger = false, NativeStringFacets? stringFacets = null, NativeType? contentType = null, NativeIntegerFacets? integerFacets = null, XName? baseName = null, NativeType? derivationBase = null, bool isComplexRestriction = false)
{
    internal NativeType? DerivationBase { get; } = derivationBase;
    internal bool IsComplexRestriction { get; } = isComplexRestriction;
    internal XName? BaseName { get; } = baseName;
    internal NativeIntegerFacets? IntegerFacets { get; } = integerFacets;
    internal NativeType? ContentType { get; } = contentType;
    internal bool EmptyContent { get; init; }
    internal bool IsComplex => EmptyContent || HasNoTypedValue || ContentType is not null;
    internal NativeStringFacets? StringFacets { get; } = stringFacets;
    internal bool IsInteger { get; } = isInteger;
    internal bool IsBoolean { get; } = isBoolean;
    internal bool IsString { get; } = isString;
    internal NativeDecimalFacets? Facets { get; } = facets;
    internal string? Name { get; } = name;
    internal bool HasNoTypedValue { get; } = hasNoTypedValue;
    internal string NamespaceUri { get; } = namespaceUri;
}
internal sealed class NativeDeclaration(string name, NativeType type, int minOccurs, int maxOccurs, string namespaceUri = "", bool isAttribute = false)
{
    internal NativeDecimalSchema? Container { get; init; }
    internal bool IsAttribute { get; } = isAttribute;
    internal string Name { get; } = name;
    internal string NamespaceUri { get; } = namespaceUri;
    internal XName ExpandedName { get; } = XName.Get(name, namespaceUri);
    internal NativeType Type { get; } = type;
    internal int MinOccurs { get; } = minOccurs;
    internal int MaxOccurs { get; } = maxOccurs;
}
internal sealed record NativeElementState(NativeDeclaration Declaration, XdmExactDecimal? TypedValue, string? StringValue = null, bool? BooleanValue = null, BigInteger? IntegerValue = null, string? NormalizedStringValue = null)
{
    internal NativeType? EffectiveType { get; init; }
    internal NativeType GoverningType => EffectiveType ?? Declaration.Type;
    internal ImmutableArray<int> OwnerElementPath { get; init; } = [];
    internal int OwnerElementIndex { get; init; }
    internal bool IsNilled => false;
}
internal sealed class NativeAssessedDocument(XDocument document, ImmutableArray<NativeElementState> elements, ImmutableArray<NativeElementState> attributes = default)
{
    private readonly XDocument document = document;
    internal ImmutableArray<NativeElementState> Elements { get; } = elements;
    internal ImmutableArray<NativeElementState> Attributes { get; } = attributes.IsDefault ? [] : attributes;
    internal XDocument CreateDocumentCopy() => new(document);
}
internal sealed class NativeProfileException(NativeOutcome outcome, string message,
    NativeDecimalUnsupportedReason reason = NativeDecimalUnsupportedReason.Unspecified) : Exception(message)
{
    internal NativeOutcome Outcome { get; } = outcome;
    internal NativeDecimalUnsupportedReason UnsupportedReason { get; } = reason;
}
