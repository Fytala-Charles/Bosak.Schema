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
        Dictionary<XName, NativeType> simple, NativeNumericLimits limits, int maxStringCharacters, CancellationToken cancellationToken, Dictionary<XElement, NativeType> anonymousSimple)
    {
        Require(!schema.Descendants(Xs + "assert").Any(), "Nested schemas must be assertion-free.", NativeDecimalUnsupportedReason.AssertionGrammar);
        var definitions = types.Where(t => t.Name == Xs + "complexType")
            .ToDictionary(t => XName.Get(Name(t), targetNamespace));
        var named = new Dictionary<XName, NativeDecimalSchema>();
        var visiting = new HashSet<XName>();

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
            var members = Elements(complex);
            if (members.Count == 1 && members[0].Name == Xs + "simpleContent")
                return CompileComplex(complex, root, targetNamespace, defaultForm, attributeForm, assertionNamespace,
                    decimalType, stringType, booleanType, integerType, simple, limits, cancellationToken, anonymousSimple);
            Require(members.Count >= 1 && members[0].Name == Xs + "sequence" && members.Skip(1).All(e => e.Name == Xs + "attribute"),
                "Expected an element-only sequence and root attributes.", NativeDecimalUnsupportedReason.DeclarationShape);
            var sequence = members[0];
            Attributes(sequence);
            var locals = Elements(sequence);
            Require(locals.Count is >= 1 and <= 16, "Expected one to sixteen local declarations.", NativeDecimalUnsupportedReason.DeclarationShape);
            var children = ImmutableArray.CreateBuilder<NativeDeclaration>();
            var names = new HashSet<XName>();
            foreach (var local in locals)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Require(local.Name == Xs + "element", "Only local element particles are admitted.", NativeDecimalUnsupportedReason.DeclarationShape);
                Attributes(local, "name", "type", "minOccurs", "maxOccurs", "form");
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
                children.Add(new NativeDeclaration(name, type, min, max, ns) { Container = container });
            }
            var attributes = CompileAttributes(members.Skip(1).ToArray(), targetNamespace, attributeForm,
                decimalType, stringType, booleanType, integerType, simple, cancellationToken);
            var plan = new NativeDecimalSchema(root, children.ToImmutable(), [], limits) { AttributeUses = attributes, NestedProfile = true };
            NestedBudget(plan, cancellationToken);
            return plan;
        }

        // Eager resolution includes unused definitions and catches optional cycles.
        foreach (var name in definitions.Keys) Resolve(name);
        var roots = ImmutableDictionary.CreateBuilder<XName, NativeDecimalSchema>();
        NativeDecimalSchema? first = null;
        foreach (var element in declarations)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Attributes(element, "name", "type");
            var name = Name(element);
            var key = XName.Get(name, targetNamespace);
            Require(!roots.ContainsKey(key), "Duplicate root declaration.", NativeDecimalUnsupportedReason.DeclarationShape);
            NativeDecimalSchema plan;
            if (element.Attribute("type") is not null)
            {
                Require(Elements(element).Count == 0, "Inline and named types cannot coexist.", NativeDecimalUnsupportedReason.DeclarationShape);
                var shared = Resolve(ResolveTypeName(element));
                plan = new NativeDecimalSchema(new NativeDeclaration(name, shared.Root.Type, 1, 1, targetNamespace), shared.Children, shared.assertions, limits)
                    { AttributeUses = shared.AttributeUses, NestedProfile = shared.NestedProfile };
            }
            else
            {
                var complex = Single(element, "complexType"); Attributes(complex);
                plan = Complex(complex, new NativeDeclaration(name, new NativeType(null, true), 1, 1, targetNamespace));
            }
            NestedBudget(plan, cancellationToken);
            roots.Add(key, plan); first ??= plan;
        }
        first!.roots = roots.ToImmutable();
        first.NamedTypes = simple.Values.Concat(named.Values.Select(p => p.Root.Type)).ToImmutableArray();
        first.DecimalType = decimalType; first.StringType = stringType; first.BooleanType = booleanType; first.IntegerType = integerType;
        first.maxStringCharacters = maxStringCharacters;
        first.PrepareInstanceTypes();
        return first;
    }

    private static (int Nodes, int Scalars, int Depth, int Attributes) NestedBudget(NativeDecimalSchema plan, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (plan.Root.Type.ContentType is not null) return (1, 1, 1, plan.AttributeUses.Length);
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
        ImmutableArray<int> ownerPath, CancellationToken cancellationToken, bool isRoot = false)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var effectiveType = ResolveInstanceType(element, declaration.Type);
        if (!isRoot)
        {
            if (plan.AttributeUses.IsEmpty) InstanceAttributes(element);
            else
            {
                Require(element.Attributes().All(a => IsAdmittedInstanceAttribute(a) || a.Name.NamespaceName is not "http://www.w3.org/2001/XMLSchema-instance" and not "http://www.w3.org/XML/1998/namespace"),
                    "Special attribute requests are unsupported.", NativeDecimalUnsupportedReason.Attributes);
                foreach (var use in plan.AttributeUses)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var attribute = element.Attribute(use.ExpandedName);
                    Check(attribute is not null || use.MinOccurs == 0, "Required descendant attribute is missing.");
                    if (attribute is not null) attributes.Add(AssessScalar(attribute.Value, use) with { OwnerElementPath = ownerPath, OwnerElementIndex = states.Count });
                }
                Check(element.Attributes().All(a => IsAdmittedInstanceAttribute(a) || plan.AttributeUses.Any(u => u.ExpandedName == a.Name)), "Undeclared descendant attribute.");
            }
        }
        if (plan.Root.Type.ContentType is { } contentType)
        {
            Check(!element.HasElements, "Simple-content terminal cannot contain child elements.");
            Require(element.Nodes().All(n => n is XText), "Simple-content comments or processing instructions are unsupported.");
            var atomicDeclaration = new NativeDeclaration(declaration.Name, contentType, 1, 1, declaration.NamespaceUri);
            states.Add(AssessScalar(element.Value, atomicDeclaration) with { Declaration = declaration, EffectiveType = effectiveType, NormalizedStringValue = Collapse(element.Value) });
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
                if (child.Container is { } nested) AssessContainer(node, child, nested, states, attributes, childPath, cancellationToken);
                else
                {
                    InstanceAttributes(node);
                    Check(!node.HasElements, "Scalar leaf contains nested elements.");
                    Require(node.Nodes().All(n => n is XText), "Scalar comments or processing instructions are unsupported.");
                    states.Add(AssessElementScalar(node, child));
                }
                Check(++count <= child.MaxOccurs, "Maximum occurrence exceeded.");
            }
            Check(count >= child.MinOccurs, "Required child is missing or out of order.");
        }
        Check(index == nodes.Length, "Unexpected child or sequence order.");
    }
}
