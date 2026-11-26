// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 November 2026
// PURPOSE              : Compiles and assesses bounded direct all groups with physical-order facts.
// SPECIAL NOTES        : Bounded direct scalar members; preserve original physical node and declaration identity.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-11-2026     | Adopt qualified bounded root all groups |
//                      | Charles Korthout | 0.2   | 25-11-2026     | Recognize admitted attribute-group references before owner assertions |
//                      | Charles Korthout | 0.3   | 25-11-2026     | Admit closed scalar global references with existing compositor occurrence limits |
//                      | Charles Korthout | 0.4   | 25-11-2026     | Preserve simple nillability and actual typed-empty values |
//                      | Charles Korthout | 0.5   | 25-11-2026     | Validate scalar element constraints against provided original values |
//                      | Charles Korthout | 0.6 | 26-11-2026     | Preserve original comment and PI nodes while validating content contributions |
//                      | Charles Korthout | 0.7   | 26-11-2026     | Retain strict wildcard plans on bounded all/choice roots.                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml.Linq;
namespace Bosak.Schema;
internal sealed partial class NativeDecimalSchema
{
    private int? AllMinimum { get; init; }
    private static NativeDecimalSchema? TryCompileAll(XElement complex, NativeDeclaration root,
        string targetNamespace, string defaultForm, string attributeForm, string assertionNamespace,
        NativeType decimalType, NativeType stringType, NativeType? booleanType, NativeType? integerType,
        Dictionary<XName, NativeType> simple, Dictionary<XElement, NativeType> anonymousSimple,
        NativeNumericLimits limits, CancellationToken token)
    {
        var members = Elements(complex);
        if (members.Count == 0 || members[0].Name != Xs + "all") return null;
        token.ThrowIfCancellationRequested();var group = members[0];Attributes(group, "minOccurs", "maxOccurs");
        var minimum = Occurs(group, "minOccurs", 1);var maximum = Occurs(group, "maxOccurs", 1);
        Require(minimum is 0 or 1 && maximum == 1, "Outer all occurrence is outside the first phase.", NativeDecimalUnsupportedReason.DeclarationShape);
        var declarations = Elements(group);
        Require(declarations.Count is >= 1 and <= 16, "All source member capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var children = ImmutableArray.CreateBuilder<NativeDeclaration>();var names = new HashSet<XName>();
        foreach(var declaration in declarations)
        {
            token.ThrowIfCancellationRequested();
            Require(declaration.Name == Xs + "element", "All requires direct scalar element members.", NativeDecimalUnsupportedReason.DeclarationShape);
            if (CompileElementReference(declaration, allowComplex: false) is { } reference)
            {
                Check(names.Add(reference.ExpandedName), "Competing referenced element particles.");
                Require(reference.MinOccurs is 0 or 1 && reference.MaxOccurs == 1, "Referenced member occurrence outside compositor profile.", NativeDecimalUnsupportedReason.DeclarationShape);
                children.Add(reference); continue;
            }
            Attributes(declaration, "name", "type", "minOccurs", "maxOccurs", "form", "nillable", "default", "fixed");
            var name = Name(declaration);var ns = Form(declaration, "form", defaultForm) == "qualified" ? targetNamespace : "";
            Check(names.Add(XName.Get(name, ns)), "Competing direct all element particles.");
            var min = Occurs(declaration, "minOccurs", 1);var max = Occurs(declaration, "maxOccurs", 1);
            Require(min is 0 or 1 && max == 1, "All member occurrence is outside the first phase.", NativeDecimalUnsupportedReason.DeclarationShape);
            var inline = Elements(declaration);NativeType type;
            if (inline.Count == 0) type = ResolveScalarType(declaration, decimalType, stringType, booleanType, integerType, simple);
            else
            {
                Require(inline.Count == 1 && declaration.Attribute("type") is null && anonymousSimple.ContainsKey(inline[0]),
                    "Only admitted anonymous scalar all members are supported.", NativeDecimalUnsupportedReason.DeclarationShape);
                type = anonymousSimple[inline[0]];
            }
            children.Add(ElementDeclaration(declaration, name, type, min, max, ns));
        }
        var tail = CompileContentWildcardTail(complex, targetNamespace);
        var attributes = tail.Uses;
        var assertions = tail.Assertions;
        Require(assertions.All(e => e.Name == Xs + "assert" && IsStructuralAssertion(e)), "All child-value assertion grammar is excluded.", NativeDecimalUnsupportedReason.AssertionGrammar);
        Require(assertions.Length <= 8, "All assertion capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        Require(assertions.Length == 0 || attributes.Length == 0, "Structural assertions with application attributes are excluded.", NativeDecimalUnsupportedReason.Attributes);
        return new NativeDecimalSchema(root, children.ToImmutable(), [], limits)
        {
            AllMinimum = minimum,
            CompiledAttributeWildcard = tail.Wildcard,
            AttributeUses = CompileAttributes(attributes, targetNamespace, attributeForm, decimalType, stringType, booleanType, integerType, simple, token),
            structuralAssertions = CompileStructuralAssertions(assertions, targetNamespace, assertionNamespace, token)
        };
    }
    private void AssessAll(XElement owner, NativeDecimalSchema plan, ImmutableArray<NativeElementState>.Builder states,
        NativePatternBudget budget, CancellationToken token)
    {
        foreach(var text in owner.Nodes().OfType<XText>()) Check(IsWhitespace(text.Value), "Element-only all contains non-whitespace text.");
        Require(owner.Nodes().All(n => n is XElement or XText or XComment or XProcessingInstruction), "All container comments or processing instructions are excluded.");
        var nodes = owner.Elements().ToArray();
        if (nodes.Length == 0 && plan.AllMinimum == 0) return;
        var seen = new HashSet<XName>();
        foreach (var node in nodes)
        {
            token.ThrowIfCancellationRequested();
            var selected = plan.Children.FirstOrDefault(c => c.ExpandedName == node.Name);
            Check(selected is not null, "No all member matches the original expanded name.");
            Check(seen.Add(node.Name), "All member occurs more than once.");
            InstanceAttributes(node);
            Check(!node.HasElements, "Scalar all member contains elements.");
            Require(node.Nodes().All(n => n is XText or XComment or XProcessingInstruction), "Scalar all comments or processing instructions are excluded.");
            states.Add(AssessElementScalar(node, selected!, budget));
        }
        foreach (var member in plan.Children)
        {
            token.ThrowIfCancellationRequested();
            Check(member.MinOccurs == 0 || seen.Contains(member.ExpandedName), "Required all member is absent.");
        }
    }
}
