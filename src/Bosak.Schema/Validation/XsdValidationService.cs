// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : REQ-009 S2/S4a/S4b validation service: constrained compile, CTA applicability, assertion evaluation.
// SPECIAL NOTES        : Part of the REQ-009 XSD 1.1 staged support (ADR-005). The W3C-constrained
//                        assertion environment is enforced add-side: XPath 2.0 compatibility, captured schema
//                        namespace bindings, and a lexical pre-screen denying external-resource/environment
//                        functions (the core has no public static-context restriction hook — recorded in the
//                        S2 phase report). S4b: Validate computes the governing type per candidate element
//                        (xsi:type pre-emption, then xs:alternative CTA in document order, then the declared
//                        type) and evaluates only the assertions owned by that governing type or its anonymous
//                        owner element; complex-type derivation checks need the full component model (S5).
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S2)                                                                    |
//                      | Charles Korthout | 0.2   | 27-10-2026     | Typed candidate view: declared built-in simple types annotated on a private clone       |
//                      | Charles Korthout | 0.3   | 27-10-2026     | ValidateValue: xs:assertion facets with atomic context item + $value (REQ-009 S3)        |
//                      | Charles Korthout | 0.4   | 27-10-2026     | ValidateAlternativeTests: CTA test compile checks sharing the plan cache (REQ-009 S4a)   |
//                      | Charles Korthout | 0.5   | 27-10-2026     | Governing-type CTA applicability + assertion filtering + typed view v2 (REQ-009 S4b)     |
//                      | Charles Korthout | 0.6   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Bosak.XPath.Api;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Parser;
using Bosak.XPath.Providers.Xml;
using Bosak.XPath.Runtime.Vm;

namespace Bosak.Schema;

/// <summary>
/// Add-on validation service contract (REQ-009 S2): evaluates the recorded XSD 1.1 assertion
/// components of a compiled schema against a candidate element and reports explicit findings.
/// </summary>
/// <remarks>
/// S2 evaluates every recorded <c>xs:assert</c> (<see cref="AssertionContext.ComplexTypeAssert"/>) against
/// the supplied candidate element; S3 evaluates the <c>xs:assertion</c> facets
/// (<see cref="AssertionContext.SimpleTypeAssertion"/>) of a named simple type against a lexical value
/// via <see cref="ValidateValue"/>. The W3C-constrained assertion environment is enforced add-side
/// (XPath 2.0 compatibility, captured namespace bindings, forbidden-function pre-screen) per ADR-005
/// decision 3; facets additionally receive the typed value as context item and through <c>$value</c>.
/// REQ-009 S4a adds <see cref="ValidateAlternativeTests"/> — static compilation checking of the recorded
/// <c>xs:alternative</c> test expressions under the same constrained environment (CTA applicability
/// evaluation itself is S4b).
/// </remarks>
public interface IXsdValidationService
{
    /// <summary>
    /// Evaluates the <c>xs:assert</c> assertions applicable to <paramref name="candidate"/> and returns
    /// the aggregated outcome. REQ-009 S4b governing-type semantics: an explicit <c>xsi:type</c> pre-empts;
    /// otherwise the owning declaration's <c>xs:alternative</c> tests run in document order (first match
    /// selects its type, the test-less default alternative catches the remainder, and no match without a
    /// default is an explicit <c>BSK-XSD-015</c>); otherwise the declared type governs. Only assertions
    /// owned by the governing named type — or, for anonymous inline complex types, owned by the candidate's
    /// element declaration — are evaluated.
    /// </summary>
    /// <param name="schema">A schema produced by <see cref="IXsdSchemaCompiler"/> against <see cref="XsdLanguageVersion.Xsd11"/>.</param>
    /// <param name="candidate">The element to validate. Its child content is what the assertion expressions see.</param>
    /// <returns>A <see cref="SchemaValidationResult"/>; invalid when any applicable assertion is violated, refused, or errors, or when CTA fails.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> or <paramref name="candidate"/> is null.</exception>
    SchemaValidationResult Validate(ICompiledSchema schema, XElement candidate);

    /// <summary>
    /// Statically checks the recorded <c>xs:alternative</c> test expressions of <paramref name="schema"/>:
    /// every non-default alternative's <c>test</c> is compiled under the same W3C-constrained XPath 2.0
    /// environment as assertions (forbidden-function pre-screen, XPath 2.0 compatibility, captured
    /// namespace bindings, resolved <c>xpathDefaultNamespace</c>), and the compiled plans are cached on
    /// this service instance for the S4b applicability evaluation. This method does not evaluate CTA
    /// applicability against any candidate element.
    /// </summary>
    /// <param name="schema">A schema produced by <see cref="IXsdSchemaCompiler"/> against <see cref="XsdLanguageVersion.Xsd11"/>.</param>
    /// <returns>A <see cref="SchemaValidationResult"/>; invalid when any alternative test is refused by the constrained environment.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
    SchemaValidationResult ValidateAlternativeTests(ICompiledSchema schema);

    /// <summary>
    /// Evaluates the <c>xs:assertion</c> facets of a named simple type against a lexical value and
    /// returns the aggregated outcome. The value is parsed per the type's built-in restriction
    /// base chain; each facet sees it as the context item and through the <c>$value</c> variable.
    /// </summary>
    /// <param name="schema">A schema produced by <see cref="IXsdSchemaCompiler"/> against <see cref="XsdLanguageVersion.Xsd11"/>.</param>
    /// <param name="simpleType">The expanded name of a named simple type declared by <paramref name="schema"/>.</param>
    /// <param name="lexicalValue">The lexical representation of the value to validate.</param>
    /// <returns>A <see cref="SchemaValidationResult"/>; invalid when any facet is violated, refused, or errors.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/>, <paramref name="simpleType"/>, or <paramref name="lexicalValue"/> is null.</exception>
    SchemaValidationResult ValidateValue(ICompiledSchema schema, XName simpleType, string lexicalValue);
}

/// <summary>
/// Default <see cref="IXsdValidationService"/> (REQ-009 S2). Compiles assertion test expressions through
/// the engine's public XPath compiler under the W3C-constrained environment, caches compiled assertions
/// keyed by (schema id, expression, static namespace context), and evaluates them over XDM views of the
/// candidate element.
/// </summary>
public sealed partial class XsdValidationService : IXsdValidationService
{
    private readonly ConcurrentDictionary<AssertionCacheKey, AssertionEvaluationPlan> _plans = new();

    /// <inheritdoc/>
    public SchemaValidationResult Validate(ICompiledSchema schema, XElement candidate)
    {
        ArgumentNullException.ThrowIfNull(schema);
        ArgumentNullException.ThrowIfNull(candidate);

        var contextItem = BuildContextItem(schema, candidate);
        var focus = XdmValue.FromNode(contextItem);
        var context = CreateFocusContext(focus);
        var diagnostics = new List<SchemaDiagnostic>();

        var governingType = DetermineGoverningType(schema, candidate, context, diagnostics);
        foreach (var assertion in schema.Assertions)
        {
            if (assertion.Context != AssertionContext.ComplexTypeAssert)
            {
                continue; // xs:assertion facets are validated via ValidateValue
            }

            if (!AppliesTo(assertion, candidate, governingType))
            {
                continue; // S4b: only assertions owned by the governing type (or its anonymous owner element) run
            }

            EvaluateAssertion(assertion, context, diagnostics);
        }

        return new SchemaValidationResult(diagnostics.Count == 0, diagnostics);
    }

    private XName? DetermineGoverningType(
        ICompiledSchema schema,
        XElement candidate,
        EvaluationContext context,
        List<SchemaDiagnostic> diagnostics)
    {
        // XSD 1.1 Part 1 §3.4.6: an explicit xsi:type is assessed before alternatives. Derivation
        // constraints between xsi:type and the alternative-selected type require the full component
        // model and stay unvalidated until S5; an unresolvable xsi:type prefix is treated as absent.
        var xsiType = candidate.Attribute(XName.Get("type", XsiNamespace));
        if (xsiType is not null && TryResolveInstanceQName(candidate, xsiType.Value, out var instanceType))
        {
            return instanceType;
        }

        var alternatives = schema.Alternatives
            .Where(a => a.OwnerElementResolvedName == candidate.Name)
            .ToList();
        if (alternatives.Count > 0)
        {
            foreach (var alternative in alternatives)
            {
                if (alternative.TestExpression is null)
                {
                    return alternative.ResolvedTypeName; // default alternative catches the remainder
                }

                var before = diagnostics.Count;
                var matches = EvaluateAlternativeTest(alternative, context, diagnostics);
                if (diagnostics.Count > before)
                {
                    return null; // refusal or evaluation error: the governing type cannot be determined
                }

                if (matches)
                {
                    return alternative.ResolvedTypeName;
                }
            }

            diagnostics.Add(new SchemaDiagnostic(
                SchemaDiagnosticCodes.AlternativeNotApplicable,
                $"No xs:alternative test matched the element '{candidate.Name}' and the declaration has no default alternative.",
                null));
            return null;
        }

        var declaration = schema.ElementDeclarations.FirstOrDefault(d =>
            d.ElementNamespace == candidate.Name.NamespaceName && d.ElementLocalName == candidate.Name.LocalName);
        return declaration is null
            ? null
            : XName.Get(declaration.TypeLocalName, declaration.TypeNamespace);
    }

    private bool EvaluateAlternativeTest(
        TypeAlternativeDefinition alternative,
        EvaluationContext context,
        List<SchemaDiagnostic> diagnostics)
    {
        var plan = GetPlan(
            "Alternative test",
            alternative.TestExpression!,
            alternative.NamespaceBindings,
            alternative.XPathDefaultNamespace,
            alternative.NodePath);
        if (plan.Refusal is not null)
        {
            diagnostics.Add(plan.Refusal with { NodePath = alternative.NodePath });
            return false;
        }

        try
        {
            return plan.Expression!.Evaluate(context).GetEffectiveBooleanValue();
        }
        catch (XPathErrorException ex)
        {
            diagnostics.Add(new SchemaDiagnostic(
                SchemaDiagnosticCodes.AssertionEvaluationError,
                $"Alternative test '{alternative.TestExpression}' raised {ex.CodeLocalName}: {ex.Message}",
                alternative.NodePath));
            return false;
        }
        catch (InvalidOperationException ex)
        {
            // The engine surfaces some function errors (e.g. FORG0001 on a failed cast) as
            // InvalidOperationException rather than XPathErrorException; both are evaluation errors.
            diagnostics.Add(new SchemaDiagnostic(
                SchemaDiagnosticCodes.AssertionEvaluationError,
                $"Alternative test '{alternative.TestExpression}' failed to evaluate: {ex.Message}",
                alternative.NodePath));
            return false;
        }
    }

    private static bool AppliesTo(AssertionDefinition assertion, XElement candidate, XName? governingType)
    {
        if (assertion.OwningTypeName is { } owningType)
        {
            return owningType == governingType;
        }

        // Anonymous inline complex type: the owning element declaration identifies the assertion.
        return assertion.OwningElementName is null || assertion.OwningElementName == candidate.Name;
    }

    private static bool TryResolveInstanceQName(XElement element, string lexical, out XName resolved)
    {
        var colon = lexical.IndexOf(':');
        if (colon < 0)
        {
            resolved = XName.Get(lexical); // unprefixed instance QName: no namespace
            return true;
        }

        var prefix = lexical[..colon];
        foreach (var scope in element.AncestorsAndSelf())
        {
            var attribute = scope.Attributes().FirstOrDefault(a =>
                a.IsNamespaceDeclaration && a.Name.LocalName == prefix);
            if (attribute is not null)
            {
                resolved = XName.Get(lexical[(colon + 1)..], attribute.Value);
                return true;
            }
        }

        resolved = XName.Get(lexical);
        return false; // unresolvable prefix
    }

    /// <inheritdoc/>
    public SchemaValidationResult ValidateValue(ICompiledSchema schema, XName simpleType, string lexicalValue)
    {
        ArgumentNullException.ThrowIfNull(schema);
        ArgumentNullException.ThrowIfNull(simpleType);
        ArgumentNullException.ThrowIfNull(lexicalValue);

        var declaration = schema.SimpleTypes.FirstOrDefault(s =>
            s.TypeNamespace == simpleType.NamespaceName && s.TypeLocalName == simpleType.LocalName);
        if (declaration is null)
        {
            return new SchemaValidationResult(
                IsValid: false,
                Diagnostics: [new SchemaDiagnostic(
                    SchemaDiagnosticCodes.SimpleTypeNotFound,
                    $"The simple type '{simpleType}' was not found in the compiled schema (or declares no xs:restriction with a base).",
                    null)]);
        }

        var (facets, builtInBase) = ResolveFacetChain(schema, declaration);
        var value = ParseAtomicValue(lexicalValue, builtInBase);
        var context = CreateValueContext(value);
        var diagnostics = new List<SchemaDiagnostic>();
        foreach (var facet in facets)
        {
            EvaluateAssertion(facet, context, diagnostics);
        }

        return new SchemaValidationResult(diagnostics.Count == 0, diagnostics);
    }

    private static (IReadOnlyList<AssertionDefinition> Facets, XmlSchemaType? BuiltInBase) ResolveFacetChain(
        ICompiledSchema schema, SimpleTypeDeclaration declaration)
    {
        // W3C XSD 1.1: assertion facets are inherited through restriction — a derived simple type
        // enforces its own facets and all facets of its base-type chain, derived first. The value
        // is parsed per the ultimate built-in base found along the same walk.
        const int maxHops = 16;
        var facets = new List<AssertionDefinition>();
        var visited = new HashSet<(string Ns, string Local)>();
        var current = declaration;
        XmlSchemaType? builtInBase = null;
        for (var hop = 0; hop < maxHops; hop++)
        {
            facets.AddRange(current.Assertions);
            if (builtInBase is null)
            {
                if (current.BaseTypeNamespace is not null &&
                    BuiltInSimpleType(current.BaseTypeNamespace, current.BaseTypeLocalName ?? string.Empty) is { } builtIn)
                {
                    builtInBase = builtIn;
                }
            }

            if (current.BaseTypeNamespace is null || current.BaseTypeLocalName is null)
            {
                break;
            }

            var nextBaseNs = current.BaseTypeNamespace;
            var nextBaseLocal = current.BaseTypeLocalName;
            if (!visited.Add((nextBaseNs, nextBaseLocal)))
            {
                break; // restriction cycle
            }

            var nextDeclaration = schema.SimpleTypes.FirstOrDefault(s =>
                s.TypeNamespace == nextBaseNs && s.TypeLocalName == nextBaseLocal);
            if (nextDeclaration is null)
            {
                break; // base declared elsewhere (unresolved import): value stays xs:string
            }

            current = nextDeclaration;
        }

        return (facets, builtInBase);
    }

    private static EvaluationContext CreateFocusContext(XdmValue focus)
    {
        // W3C XSD 1.1: for xs:assert the context item is the element and $value is bound to it.
        return new EvaluationContext().WithFocus(focus, 1, 1).WithVariable("value", focus);
    }

    private static EvaluationContext CreateValueContext(XdmValue value)
    {
        // W3C XSD 1.1: for xs:assertion facets the context item is the typed value and $value is bound to it.
        return new EvaluationContext().WithFocus(value, 1, 1).WithVariable("value", value);
    }

    private static XdmValue ParseAtomicValue(string lexicalValue, XmlSchemaType? builtInType)
    {
        if (builtInType is null)
        {
            return XdmValue.FromString(lexicalValue);
        }

        // Reuse the typed-view machinery: annotate a throwaway wrapper element with the built-in
        // type and read the engine's typed value back (datatype-validated per XSD rules).
        var wrapper = new XElement("value", lexicalValue);
        XdmSchemaAnnotator.Annotate(wrapper, new SimpleTypeSchemaInfo(builtInType));
        return XDocumentProvider.ToXdmNode(wrapper).TypedValue;
    }

    private static IXdmNode BuildContextItem(ICompiledSchema schema, XElement candidate)
    {
        // Typed view: W3C assertions are evaluated against schema-validated content, so leaf values
        // carry their declared datatypes. The candidate is deep-cloned and each element matching a
        // recorded declaration is annotated with the declared built-in simple type — resolved through
        // user-defined simple-type restriction chains (S4b typed view v2) when needed; the caller's
        // tree is never annotated (ADR-005: no mutation of shared nodes).
        var types = new Dictionary<(string Ns, string Local), XmlSchemaType>();
        foreach (var declaration in schema.ElementDeclarations)
        {
            var type = BuiltInSimpleType(declaration.TypeNamespace, declaration.TypeLocalName)
                ?? ResolveUserSimpleType(schema, declaration.TypeNamespace, declaration.TypeLocalName);
            if (type is not null)
            {
                types[(declaration.ElementNamespace, declaration.ElementLocalName)] = type;
            }
        }

        if (types.Count == 0)
        {
            return XDocumentProvider.ToXdmNode(candidate);
        }

        var clone = new XElement(candidate);
        foreach (var element in clone.DescendantsAndSelf())
        {
            if (types.TryGetValue((element.Name.NamespaceName, element.Name.LocalName), out var type))
            {
                XdmSchemaAnnotator.Annotate(element, new SimpleTypeSchemaInfo(type));
            }
        }

        return XDocumentProvider.ToXdmNode(clone);
    }

    private static XmlSchemaType? ResolveUserSimpleType(ICompiledSchema schema, string typeNamespace, string typeLocalName)
    {
        // Walk the recorded restriction chain (same bounded, cycle-guarded walk as ResolveFacetChain)
        // to the ultimate built-in base, so elements declared against user-defined simple types still
        // surface typed values in the candidate view.
        const int maxHops = 16;
        var visited = new HashSet<(string Ns, string Local)>();
        var current = schema.SimpleTypes.FirstOrDefault(s =>
            s.TypeNamespace == typeNamespace && s.TypeLocalName == typeLocalName);
        for (var hop = 0; hop < maxHops && current is not null; hop++)
        {
            if (current.BaseTypeNamespace is null || current.BaseTypeLocalName is null)
            {
                return null;
            }

            if (BuiltInSimpleType(current.BaseTypeNamespace, current.BaseTypeLocalName) is { } builtIn)
            {
                return builtIn;
            }

            if (!visited.Add((current.BaseTypeNamespace, current.BaseTypeLocalName)))
            {
                return null; // restriction cycle
            }

            var baseNs = current.BaseTypeNamespace;
            var baseLocal = current.BaseTypeLocalName;
            current = schema.SimpleTypes.FirstOrDefault(s =>
                s.TypeNamespace == baseNs && s.TypeLocalName == baseLocal);
        }

        return null;
    }

    private static XmlSchemaType? BuiltInSimpleType(string typeNamespace, string typeLocalName)
    {
        if (typeNamespace != XmlSchemaNamespace || !Enum.TryParse<XmlTypeCode>(typeLocalName, ignoreCase: true, out var code))
        {
            return null; // user-defined types stay untyped in this slice
        }

        return XmlSchemaType.GetBuiltInSimpleType(code);
    }

    private void EvaluateAssertion(AssertionDefinition assertion, EvaluationContext context, List<SchemaDiagnostic> diagnostics)
    {
        var plan = GetPlan(
            "Assertion",
            assertion.TestExpression,
            assertion.NamespaceBindings,
            assertion.XPathDefaultNamespace,
            assertion.NodePath);
        if (plan.Refusal is not null)
        {
            diagnostics.Add(plan.Refusal with { NodePath = assertion.NodePath });
            return;
        }

        bool holds;
        try
        {
            holds = plan.Expression!.Evaluate(context).GetEffectiveBooleanValue();
        }
        catch (XPathErrorException ex)
        {
            diagnostics.Add(new SchemaDiagnostic(
                SchemaDiagnosticCodes.AssertionEvaluationError,
                $"Assertion '{assertion.TestExpression}' raised {ex.CodeLocalName}: {ex.Message}",
                assertion.NodePath));
            return;
        }
        catch (InvalidOperationException ex)
        {
            diagnostics.Add(new SchemaDiagnostic(
                SchemaDiagnosticCodes.AssertionEvaluationError,
                $"Assertion '{assertion.TestExpression}' failed to evaluate: {ex.Message}",
                assertion.NodePath));
            return;
        }

        if (!holds)
        {
            diagnostics.Add(new SchemaDiagnostic(
                SchemaDiagnosticCodes.AssertionViolated,
                $"Assertion '{assertion.TestExpression}' evaluated to false.",
                assertion.NodePath));
        }
    }

    /// <inheritdoc/>
    public SchemaValidationResult ValidateAlternativeTests(ICompiledSchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema);

        var diagnostics = new List<SchemaDiagnostic>();
        foreach (var alternative in schema.Alternatives)
        {
            if (alternative.TestExpression is null)
            {
                continue; // default alternative carries no test expression
            }

            var plan = GetPlan(
                "Alternative test",
                alternative.TestExpression,
                alternative.NamespaceBindings,
                alternative.XPathDefaultNamespace,
                alternative.NodePath);
            if (plan.Refusal is not null)
            {
                diagnostics.Add(plan.Refusal with { NodePath = alternative.NodePath });
            }
        }

        return new SchemaValidationResult(diagnostics.Count == 0, diagnostics);
    }

    private AssertionEvaluationPlan GetPlan(
        string construct,
        string expression,
        IReadOnlyDictionary<string, string>? bindings,
        string? defaultNamespace,
        string? nodePath)
    {
        var key = new AssertionCacheKey(expression, defaultNamespace, BindingsKey(bindings));
        return _plans.GetOrAdd(
            key,
            static (_, state) => CompileConstrained(state.Construct, state.Expression, state.Bindings, state.DefaultNamespace, state.NodePath),
            (Construct: construct, Expression: expression, Bindings: bindings, DefaultNamespace: defaultNamespace, NodePath: nodePath));
    }

    private static AssertionEvaluationPlan CompileConstrained(
        string construct,
        string expression,
        IReadOnlyDictionary<string, string>? bindings,
        string? defaultNamespace,
        string? nodePath)
    {
        if (ForbiddenFunctionCall().IsMatch(expression))
        {
            return AssertionEvaluationPlan.Refused(new SchemaDiagnostic(
                SchemaDiagnosticCodes.AssertionRefused,
                $"{construct} '{expression}' was refused: it calls a function outside the W3C-constrained assertion environment (external resource or environment access is prohibited).",
                nodePath));
        }

        var options = new CompileOptions
        {
            Compatibility = XPathCompatibility.XPath20,
            Namespaces = bindings,
            DefaultElementNamespace = defaultNamespace,
        };

        try
        {
            return AssertionEvaluationPlan.Compiled(XPath31Expression.Compile(expression, options));
        }
        catch (XPathParseException ex)
        {
            return AssertionEvaluationPlan.Refused(new SchemaDiagnostic(
                SchemaDiagnosticCodes.AssertionRefused,
                $"{construct} '{expression}' is not a valid XPath 2.0 expression (XPST0003 at position {ex.Position}): {ex.Message}",
                nodePath));
        }
        catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
        {
            return AssertionEvaluationPlan.Refused(new SchemaDiagnostic(
                SchemaDiagnosticCodes.AssertionRefused,
                $"{construct} '{expression}' was refused by the XPath 2.0 static context: {ex.Message}",
                nodePath));
        }
    }

    private static string BindingsKey(IReadOnlyDictionary<string, string>? bindings)
    {
        if (bindings is null || bindings.Count == 0)
        {
            return string.Empty;
        }

        return string.Join("&", bindings.OrderBy(pair => pair.Key, StringComparer.Ordinal).Select(pair => pair.Key + "=" + pair.Value));
    }

    [GeneratedRegex(@"\b(?:fn:)?(doc|collection|document|json-doc|environment-variable)\s*\(", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ForbiddenFunctionCall();

    private const string XmlSchemaNamespace = "http://www.w3.org/2001/XMLSchema";

    private const string XsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";

    private readonly record struct AssertionCacheKey(string Expression, string? DefaultNamespace, string BindingsKey);

    /// <summary>
    /// Minimal <see cref="IXmlSchemaInfo"/> implementation declaring an element's type as a built-in
    /// simple type. Read by the engine's typed-value surfaces via <c>XObject.GetSchemaInfo()</c>;
    /// carries no declaration identity (there is no System.Xml.Schema component model behind it).
    /// </summary>
    private sealed class SimpleTypeSchemaInfo : IXmlSchemaInfo
    {
        internal SimpleTypeSchemaInfo(XmlSchemaType schemaType)
        {
            SchemaType = schemaType;
        }

        public XmlSchemaValidity Validity => XmlSchemaValidity.Valid;

        public bool IsDefault => false;

        public bool IsNil => false;

        public XmlSchemaSimpleType? MemberType => null;

        public XmlSchemaType SchemaType { get; }

        public XmlSchemaElement? SchemaElement => null;

        public XmlSchemaAttribute? SchemaAttribute => null;

        public XmlNodeType NodeType => XmlNodeType.Element;
    }

    private sealed class AssertionEvaluationPlan
    {
        private AssertionEvaluationPlan(XPath31Expression? expression, SchemaDiagnostic? refusal)
        {
            Expression = expression;
            Refusal = refusal;
        }

        internal XPath31Expression? Expression { get; }

        internal SchemaDiagnostic? Refusal { get; }

        internal static AssertionEvaluationPlan Compiled(XPath31Expression expression)
        {
            return new AssertionEvaluationPlan(expression, null);
        }

        internal static AssertionEvaluationPlan Refused(SchemaDiagnostic refusal)
        {
            return new AssertionEvaluationPlan(null, refusal);
        }
    }
}
