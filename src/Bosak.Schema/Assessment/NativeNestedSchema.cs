// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Compiles and assesses bounded acyclic nested element-only sequences.
// SPECIAL NOTES        : Internal finite native profile; no Core adapter or full XSD 1.1 conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 20-11-2026     | Adopt nested local attributes with bounded total uses and original owner identity        |
//                      | Charles Korthout | 0.3   | 20-11-2026     | Adopt nested decimal simple content as original complex scalar-bearing terminals         |
//                      | Charles Korthout | 0.4   | 20-11-2026     | Preserve unfetched location hints separately from descendant application attributes      |
//                      | Charles Korthout | 0.5   | 20-11-2026     | Share original anonymous simple definitions and the eager combined local type budget      |
//                      | Charles Korthout | 0.6   | 21-11-2026     | Adopt bounded instance selection with original declaration and governing type identity |
//                      | Charles Korthout | 0.7   | 21-11-2026     | Adopt bounded complex restriction with original and atomic component identities |
//                      | Charles Korthout | 0.8   | 21-11-2026     | Adopt bounded contextual and inherited integer assertions |
//                      | Charles Korthout | 0.9   | 21-11-2026     | Probe empty complex typing in isolated copies only |
//                      | Charles Korthout | 0.10   | 22-11-2026     | Probe bounded structural assertions in isolated copies |
//                      | Charles Korthout | 0.11   | 22-11-2026     | Adopt qualified bounded structural and empty-content semantics |
//                      | Charles Korthout | 0.12   | 22-11-2026     | Prototype bounded string patterns and per-assessment accounting |
//                      | Charles Korthout | 0.13   | 22-11-2026     | Adopt qualified bounded native string patterns |
//                      | Charles Korthout | 0.14   | 22-11-2026     | Probe simple scalar roots in isolated source copies only |
//                      | Charles Korthout | 0.15   | 22-11-2026     | Adopt qualified native simple scalar root assessment |
//                      | Charles Korthout | 0.16   | 23-11-2026     | Adopt qualified bounded native choice |
//                      | Charles Korthout | 0.17   | 23-11-2026     | Adopt qualified bounded root all groups |
//                      | Charles Korthout | 0.18   | 24-11-2026     | Probe bounded date simple content in isolated source copy |
//                      | Charles Korthout | 0.19   | 24-11-2026     | Adopt qualified bounded date simple-content extension |
//                      | Charles Korthout | 0.20   | 24-11-2026     | Preserve caller character capacity for nested date restrictions |
//                      | Charles Korthout | 0.21   | 24-11-2026     | Share date/string pattern compilation across nested date restrictions |
//                      | Charles Korthout | 0.22   | 25-11-2026     | Recognize admitted attribute-group references before owner assertions |
//                      | Charles Korthout | 0.23   | 25-11-2026     | Preserve nested structural assertion exclusion for attribute groups |
//                      | Charles Korthout | 0.24   | 25-11-2026     | Resolve cached closed global elements with canonical particle uses and cycle budgets |
//                      | Charles Korthout | 0.25   | 25-11-2026     | Share actual particles across closed named sequence groups |
//                      | Charles Korthout | 0.26   | 25-11-2026     | Preserve simple nillability and actual typed-empty values |
//                      | Charles Korthout | 0.27   | 25-11-2026     | Validate scalar element constraints against provided original values |
//                      | Charles Korthout | 0.28   | 25-11-2026     | Validate provided effective attribute constraints and fixed override identity |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml.Linq;

namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    internal bool NestedProfile { get; private set; }

    private static bool HasNestedDeclarations(XElement schema, XElement[] types, string targetNamespace)
    {
        if (schema.Elements(Xs + "group").Any()) return true;
        if (schema.Descendants(Xs + "element").Any(e => e.Attribute("ref") is not null &&
            (e.Parent?.Name == Xs + "sequence" || e.Parent?.Name == Xs + "all" || e.Parent?.Name == Xs + "choice"))) return true;
        var names = types.Where(t => t.Name == Xs + "complexType")
            .Select(t => XName.Get(Name(t), targetNamespace)).ToHashSet();
        foreach (var local in schema.Descendants(Xs + "element").Where(e => e.Parent?.Name == Xs + "sequence"))
        {
            if (local.Elements(Xs + "complexType").Any()) return true;
            if (local.Attribute("type") is not null && names.Contains(ResolveTypeName(local))) return true;
        }
        return false;
    }

    private static NativeDecimalSchema CompileNestedSchema(XElement schema, XElement[] types, XElement[] declarations,
        string targetNamespace, string defaultForm, string attributeForm, string assertionNamespace,
        NativeType decimalType, NativeType stringType, NativeType booleanType, NativeType integerType,
        Dictionary<XName, NativeType> simple, NativeNumericLimits limits, int maxStringCharacters, CancellationToken cancellationToken, Dictionary<XElement, NativeType> anonymousSimple, NativePatternCompilation patternCompilation)
    {
        // Structural assertions are parsed eagerly on each complex owner below.
        var definitions = types.Where(t => t.Name == Xs + "complexType")
            .ToDictionary(t => XName.Get(Name(t), targetNamespace));
        var named = new Dictionary<XName, NativeDecimalSchema>();
        var visiting = new HashSet<XName>();

        var modelGroups = CompileModelGroups(schema, targetNamespace, cancellationToken);
        var particles = new Dictionary<XElement, NativeDeclaration>();
        var compilingParticles = new HashSet<XElement>();
        var elementDefinitions = new Dictionary<XName, XElement>();
        foreach (var declaration in declarations)
        {
            var key = XName.Get(Name(declaration), targetNamespace);
            Require(elementDefinitions.TryAdd(key, declaration), "Duplicate root declaration.", NativeDecimalUnsupportedReason.DeclarationShape);
        }
        var roots = new Dictionary<XName, NativeDecimalSchema>();
        var visitingElements = new HashSet<XName>();
        NativeDecimalSchema ResolveElement(XName key)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (roots.TryGetValue(key, out var existing)) return existing;
            Require(elementDefinitions.TryGetValue(key, out var element), "Unresolved closed global element.", NativeDecimalUnsupportedReason.DeclarationShape);
            Require(visitingElements.Add(key), "Recursive global element graph is outside the finite profile.", NativeDecimalUnsupportedReason.DeclarationShape);
            Attributes(element!, "name", "type", "nillable", "default", "fixed");
            var name = Name(element!);
            NativeDecimalSchema plan;
            var scalarRoot = TryCompileSimpleRoot(element!, targetNamespace, decimalType, stringType, booleanType, integerType, simple, anonymousSimple, limits);
            Require(scalarRoot is not null || !ReadNillable(element!), "Complex nillability is outside the simple-valued profile.", NativeDecimalUnsupportedReason.DeclarationShape);
            Require(scalarRoot is not null || !HasValueConstraint(element!), "Complex value constraints are outside the provided-content profile.", NativeDecimalUnsupportedReason.DeclarationShape);
            if (scalarRoot is not null) plan = scalarRoot;
            else if (element!.Attribute("type") is not null)
            {
                Require(Elements(element).Count == 0, "Inline and named types cannot coexist.", NativeDecimalUnsupportedReason.DeclarationShape);
                var shared = Resolve(ResolveTypeName(element));
                plan = new NativeDecimalSchema(new NativeDeclaration(name, shared.Root.Type, 1, 1, targetNamespace), shared.Children, shared.assertions, limits)
                    { AttributeUses = shared.AttributeUses, integerAssertions = shared.integerAssertions, structuralAssertions = shared.structuralAssertions, NestedProfile = shared.NestedProfile, ChoiceMinimum = shared.ChoiceMinimum, AllMinimum = shared.AllMinimum };
            }
            else
            {
                var complex = Single(element!, "complexType"); Attributes(complex);
                plan = Complex(complex, new NativeDeclaration(name, new NativeType(null, true), 1, 1, targetNamespace));
            }
            NestedBudget(plan, cancellationToken); visitingElements.Remove(key); roots.Add(key, plan); return plan;
        }
        schema.AddAnnotation(new NativeElementDeclarations(ResolveElement));

        NativeDecimalSchema Resolve(XName name)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (named.TryGetValue(name, out var existing)) return existing;
            Require(definitions.TryGetValue(name, out var definition), "Unresolved complex type.", NativeDecimalUnsupportedReason.ValueType);
            Require(visiting.Add(name), "Recursive complex types are outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
            Attributes(definition!, "name");
            var plan = Complex(definition!, new NativeDeclaration(name.LocalName, new NativeType(name.LocalName, true, targetNamespace), 1, 1, targetNamespace));
            named.Add(name, plan);
            visiting.Remove(name);
            return plan;
        }

        NativeDecimalSchema Complex(XElement complex, NativeDeclaration root)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (TryCompileEmpty(complex, root, limits, targetNamespace, assertionNamespace, cancellationToken) is { } empty) return empty;
            if (TryCompileAll(complex, root, targetNamespace, defaultForm, attributeForm, assertionNamespace,
                decimalType, stringType, booleanType, integerType, simple, anonymousSimple, limits, cancellationToken) is { } all) return all;
            if (TryCompileChoice(complex, root, targetNamespace, defaultForm, attributeForm, assertionNamespace,
                decimalType, stringType, booleanType, integerType, simple, anonymousSimple, limits, cancellationToken) is { } choice) return choice;
            var members = Elements(complex);
            if (members.Count == 1 && members[0].Name == Xs + "simpleContent")
                return CompileComplex(complex, root, targetNamespace, defaultForm, attributeForm, assertionNamespace,
                    decimalType, stringType, booleanType, integerType, simple, limits, cancellationToken, anonymousSimple, requested => definitions.ContainsKey(requested) ? Resolve(requested) : null, nestedContext: true, maxStringCharacters: maxStringCharacters, patternCompilation: patternCompilation);
            Require(members.Count >= 1 && (members[0].Name == Xs + "sequence" || members[0].Name == Xs + "group") && members.Skip(1).TakeWhile(IsAttributeUseMember).Count() + members.Skip(1).SkipWhile(IsAttributeUseMember).Count(e => e.Name == Xs + "assert") == members.Count - 1,
                "Expected an element-only sequence and root attributes.", NativeDecimalUnsupportedReason.DeclarationShape);
            var sequence = members[0];
            var locals = modelGroups.Expand(sequence).ToArray();
            var children = CompileParticles(locals);
            Require(!members.Any(a => a.Name == Xs + "assert") || !members.Any(IsAttributeUseMember), "Structural assertions with application attributes are excluded.", NativeDecimalUnsupportedReason.Attributes);
            var attributes = CompileAttributes(members.Skip(1).TakeWhile(IsAttributeUseMember).ToArray(), targetNamespace, attributeForm,
                decimalType, stringType, booleanType, integerType, simple, cancellationToken);
            var plan = new NativeDecimalSchema(root, children, [], limits) { AttributeUses = attributes, NestedProfile = true, structuralAssertions = CompileStructuralAssertions(members.Skip(1).SkipWhile(IsAttributeUseMember), targetNamespace, assertionNamespace, cancellationToken) };
            NestedBudget(plan, cancellationToken);
            return plan;
        }

        ImmutableArray<NativeDeclaration> CompileParticles(XElement[] locals)
        {
            Require(locals.Length is >= 1 and <= 16, "Expected one to sixteen local declarations.", NativeDecimalUnsupportedReason.DeclarationShape);
            var children = ImmutableArray.CreateBuilder<NativeDeclaration>();
            var names = new HashSet<XName>();
            foreach (var local in locals)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Require(local.Name == Xs + "element", "Only local element particles are admitted.", NativeDecimalUnsupportedReason.DeclarationShape);
                if (particles.TryGetValue(local, out var sharedParticle))
                {
                    Require(names.Add(sharedParticle.ExpandedName), "Duplicate expanded sequence particle names are outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
                    children.Add(sharedParticle); continue;
                }
                Require(compilingParticles.Add(local), "Recursive element graphs through model groups are outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
                if (CompileElementReference(local, allowComplex: true) is { } reference)
                {
                    Require(names.Add(reference.ExpandedName), "Duplicate expanded sequence particle names are outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
                    compilingParticles.Remove(local); particles.Add(local, reference); children.Add(reference); continue;
                }
                Attributes(local, "name", "type", "minOccurs", "maxOccurs", "form", "nillable", "default", "fixed");
                var name = Name(local);
                var ns = Form(local, "form", defaultForm) == "qualified" ? targetNamespace : "";
                Require(names.Add(XName.Get(name, ns)), "Duplicate expanded local name.", NativeDecimalUnsupportedReason.DeclarationShape);
                var min = Occurs(local, "minOccurs", 1);
                var max = Occurs(local, "maxOccurs", 1);
                Require(min is 0 or 1 && max is >= 1 and <= 16 && min <= max, "Occurrence outside the finite profile.", NativeDecimalUnsupportedReason.DeclarationShape);
                var inline = Elements(local);
                NativeDecimalSchema? container = null;
                NativeType type;
                if (local.Attribute("type") is not null)
                {
                    Require(inline.Count == 0, "Inline and named types cannot coexist.", NativeDecimalUnsupportedReason.DeclarationShape);
                    var typeName = ResolveTypeName(local);
                    if (definitions.ContainsKey(typeName)) { container = Resolve(typeName); type = container.Root.Type; }
                    else type = ResolveScalarType(local, decimalType, stringType, booleanType, integerType, simple);
                }
                else if (inline.Count == 1 && anonymousSimple.TryGetValue(inline[0], out var anonymousType))
                    type = anonymousType;
                else
                {
                    Require(inline.Count == 1 && inline[0].Name == Xs + "complexType", "Expected an inline complex type.", NativeDecimalUnsupportedReason.DeclarationShape);
                    Attributes(inline[0]);
                    container = Complex(inline[0], new NativeDeclaration(name, new NativeType(null, true), 1, 1, ns));
                    type = container.Root.Type;
                }
                Require(container?.AllMinimum is null, "Nested all containers are outside the first phase.", NativeDecimalUnsupportedReason.DeclarationShape);
                Require(container?.ChoiceMinimum is null, "Nested choice containers are outside the first phase.", NativeDecimalUnsupportedReason.DeclarationShape);
                var particle = ElementDeclaration(local, name, type, min, max, ns, container);
                compilingParticles.Remove(local); particles.Add(local, particle); children.Add(particle);
            }
            return children.ToImmutable();
        }

        // Eager resolution includes unused definitions and catches optional cycles.
        foreach (var name in definitions.Keys) Resolve(name);
        foreach (var name in modelGroups.Names) CompileParticles(modelGroups.Resolve(name).ToArray());
        NativeDecimalSchema? first = null;
        foreach (var element in declarations)
        {
            var plan = ResolveElement(XName.Get(Name(element), targetNamespace));
            first ??= plan;
        }
        first!.roots = roots.ToImmutableDictionary();
        first.NamedTypes = simple.Values.Concat(named.Values.Select(p => p.Root.Type)).ToImmutableArray();
        first.DecimalType = decimalType; first.StringType = stringType; first.BooleanType = booleanType; first.IntegerType = integerType;
        first.maxStringCharacters = maxStringCharacters;
        first.PrepareInstanceTypes();
        return first;
    }

    private static (int Nodes, int Scalars, int Depth, int Attributes) NestedBudget(NativeDecimalSchema plan, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!plan.Root.Type.IsComplex || plan.Root.Type.ContentType is not null) return (1, 1, 1, plan.AttributeUses.Length);
        if (plan.AllMinimum is not null)
        {
            var maximum = plan.Children.Sum(c => c.MaxOccurs);
            return (1 + maximum, maximum, 2, plan.AttributeUses.Length);
        }
        if (plan.ChoiceMinimum is not null)
        {
            var maximum = plan.Children.Max(c => c.MaxOccurs);
            return (1 + maximum, maximum, 2, plan.AttributeUses.Length);
        }
        var nodes = 1; var scalars = 0; var depth = 1; var attributes = plan.AttributeUses.Length;
        foreach (var child in plan.Children)
        {
            var budget = child.Container is { } nested ? NestedBudget(nested, cancellationToken) : (Nodes: 1, Scalars: 1, Depth: 1, Attributes: 0);
            // Every child budget was bounded before multiplication; each maximum is at most16.
            nodes += child.MaxOccurs * budget.Nodes;
            scalars += child.MaxOccurs * budget.Scalars;
            depth = Math.Max(depth, 1 + budget.Depth);
            attributes += child.MaxOccurs * budget.Attributes;
            Require(nodes <= 64 && scalars <= 16 && depth <= 4 && attributes <= 64, "Expanded nested representation ceiling exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        }
        return (nodes, scalars, depth, attributes);
    }

    private void AssessContainer(XElement element, NativeDeclaration declaration, NativeDecimalSchema plan,
        ImmutableArray<NativeElementState>.Builder states, ImmutableArray<NativeElementState>.Builder attributes,
        ImmutableArray<int> ownerPath, CancellationToken cancellationToken, NativePatternBudget patternBudget, bool isRoot = false)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var effectiveType = ResolveInstanceType(element, declaration.Type);
        _ = NilledState(element, declaration, effectiveType);
        if (!isRoot)
        {
            if (plan.AttributeUses.IsEmpty && plan.Root.Type.ContentType?.IsDate != true) InstanceAttributes(element);
            else
            {
                Require(element.Attributes().All(a => IsAdmittedInstanceAttribute(a) || a.Name.NamespaceName is not "http://www.w3.org/2001/XMLSchema-instance" and not "http://www.w3.org/XML/1998/namespace"),
                    "Special attribute requests are unsupported.", NativeDecimalUnsupportedReason.Attributes);
                foreach (var use in plan.AttributeUses)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var attribute = element.Attribute(use.ExpandedName);
                    Check(attribute is not null || use.MinOccurs == 0, "Required descendant attribute is missing.");
                    if (attribute is not null) attributes.Add(AssessAttributeValue(attribute.Value, use, patternBudget) with { OwnerElementPath = ownerPath, OwnerElementIndex = states.Count });
                    else RequireUnconstrainedAttributeAbsence(use);
                }
                Check(element.Attributes().All(a => IsAdmittedInstanceAttribute(a) || plan.AttributeUses.Any(u => u.ExpandedName == a.Name)), "Undeclared descendant attribute.");
            }
        }
        if (plan.Root.Type.ContentType is { } contentType)
        {
            Check(!element.HasElements, "Simple-content terminal cannot contain child elements.");
            Require(element.Nodes().All(n => n is XText), "Simple-content comments or processing instructions are unsupported.");
            var atomicDeclaration = new NativeDeclaration(declaration.Name, contentType, 1, 1, declaration.NamespaceUri);
            var scalarState = AssessScalar(element.Value, atomicDeclaration, patternBudget) with { Declaration = declaration, EffectiveType = effectiveType, NormalizedStringValue = Collapse(element.Value) };
            plan.AssessIntegerAssertions(element, scalarState, cancellationToken);
            states.Add(scalarState);
            cancellationToken.ThrowIfCancellationRequested();
            return;
        }
        if (effectiveType.EmptyContent)
        {
            Require(element.Nodes().All(n => n is XElement or XText), "Empty-content comments or processing instructions are unsupported.");
            Check(!element.Nodes().Any(), "Empty content cannot contain character or element children.");
            plan.AssessStructuralAssertions(element, cancellationToken);
            states.Add(new NativeElementState(declaration, null) { EffectiveType = effectiveType });
            cancellationToken.ThrowIfCancellationRequested();
            return;
        }
        foreach (var text in element.Nodes().OfType<XText>()) Check(IsWhitespace(text.Value), "Element-only container contains non-whitespace text.");
        Require(element.Nodes().All(n => n is XElement or XText), "Container comments or processing instructions are unsupported.");
        states.Add(new NativeElementState(declaration, null) { EffectiveType = effectiveType });
        var nodes = element.Elements().ToArray(); var index = 0;
        foreach (var child in plan.Children)
        {
            var count = 0;
            while (index < nodes.Length && nodes[index].Name == child.ExpandedName)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var childPath = ownerPath.Add(index);
                var node = nodes[index++];
                if (child.Container is { } nested) AssessContainer(node, child, nested, states, attributes, childPath, cancellationToken, patternBudget);
                else
                {
                    InstanceAttributes(node);
                    Check(!node.HasElements, "Scalar leaf contains nested elements.");
                    Require(node.Nodes().All(n => n is XText), "Scalar comments or processing instructions are unsupported.");
                    states.Add(AssessElementScalar(node, child, patternBudget));
                }
                Check(++count <= child.MaxOccurs, "Maximum occurrence exceeded.");
            }
            Check(count >= child.MinOccurs, "Required child is missing or out of order.");
        }
        Check(index == nodes.Length, "Unexpected child or sequence order.");
        plan.AssessStructuralAssertions(element, cancellationToken);
    }
}
