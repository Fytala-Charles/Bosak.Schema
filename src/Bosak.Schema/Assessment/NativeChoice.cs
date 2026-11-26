// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 November 2026
// PURPOSE              : Compiles and assesses bounded direct choice models with physical-order facts.
// SPECIAL NOTES        : Bounded direct scalar alternatives; preserve original physical node and declaration identity.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-11-2026     | Adopt qualified bounded native choice |
//                      | Charles Korthout | 0.2   | 25-11-2026     | Recognize admitted attribute-group references before owner assertions |
//                      | Charles Korthout | 0.3   | 25-11-2026     | Admit closed scalar global references with existing compositor occurrence limits |
//                      | Charles Korthout | 0.4   | 25-11-2026     | Preserve simple nillability and actual typed-empty values |
//                      | Charles Korthout | 0.5   | 25-11-2026     | Validate scalar element constraints against provided original values |
//                      | Charles Korthout | 0.6 | 26-11-2026     | Preserve original comment and PI nodes while validating content contributions |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml.Linq;
namespace Bosak.Schema;
internal sealed partial class NativeDecimalSchema
{
    private int? ChoiceMinimum { get; init; }
    private static NativeDecimalSchema? TryCompileChoice(XElement complex, NativeDeclaration root,
        string targetNamespace, string defaultForm, string attributeForm, string assertionNamespace,
        NativeType decimalType, NativeType stringType, NativeType? booleanType, NativeType? integerType,
        Dictionary<XName, NativeType> simple, Dictionary<XElement, NativeType> anonymousSimple,
        NativeNumericLimits limits, CancellationToken token)
    {
        var members = Elements(complex);
        if (members.Count == 0 || members[0].Name != Xs + "choice") return null;
        token.ThrowIfCancellationRequested();var group = members[0];Attributes(group, "minOccurs", "maxOccurs");
        var minimum = Occurs(group, "minOccurs", 1);var maximum = Occurs(group, "maxOccurs", 1);
        Require(minimum is 0 or 1 && maximum == 1, "Outer choice occurrence is outside the first phase.", NativeDecimalUnsupportedReason.DeclarationShape);
        var declarations = Elements(group);
        Require(declarations.Count is >= 1 and <= 16, "Choice source alternative capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var children = ImmutableArray.CreateBuilder<NativeDeclaration>();var names = new HashSet<XName>();
        foreach(var declaration in declarations)
        {
            token.ThrowIfCancellationRequested();
            Require(declaration.Name == Xs + "element", "Choice requires direct scalar element alternatives.", NativeDecimalUnsupportedReason.DeclarationShape);
            if (CompileElementReference(declaration, allowComplex: false) is { } reference)
            {
                Check(names.Add(reference.ExpandedName), "Competing referenced element particles.");
                Require(reference.MinOccurs == 1 && reference.MaxOccurs is >= 1 and <= 16, "Referenced member occurrence outside compositor profile.", NativeDecimalUnsupportedReason.DeclarationShape);
                children.Add(reference); continue;
            }
            Attributes(declaration, "name", "type", "minOccurs", "maxOccurs", "form", "nillable", "default", "fixed");
            var name = Name(declaration);var ns = Form(declaration, "form", defaultForm) == "qualified" ? targetNamespace : "";
            Check(names.Add(XName.Get(name, ns)), "Competing direct choice element particles.");
            var min = Occurs(declaration, "minOccurs", 1);var max = Occurs(declaration, "maxOccurs", 1);
            Require(min == 1 && max >= 1, "Nullable alternatives are outside the first phase.", NativeDecimalUnsupportedReason.DeclarationShape);
            Require(max <= 16, "Choice branch capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            var inline = Elements(declaration);NativeType type;
            if (inline.Count == 0) type = ResolveScalarType(declaration, decimalType, stringType, booleanType, integerType, simple);
            else
            {
                Require(inline.Count == 1 && declaration.Attribute("type") is null && anonymousSimple.ContainsKey(inline[0]),
                    "Only admitted anonymous scalar choice alternatives are supported.", NativeDecimalUnsupportedReason.DeclarationShape);
                type = anonymousSimple[inline[0]];
            }
            children.Add(ElementDeclaration(declaration, name, type, min, max, ns));
        }
        var attributes = members.Skip(1).TakeWhile(IsAttributeUseMember).ToArray();
        var assertions = members.Skip(1 + attributes.Length).ToArray();
        Require(assertions.All(e => e.Name == Xs + "assert" && IsStructuralAssertion(e)), "Choice child-value assertion grammar is excluded.", NativeDecimalUnsupportedReason.AssertionGrammar);
        Require(assertions.Length <= 8, "Choice assertion capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        Require(assertions.Length == 0 || attributes.Length == 0, "Structural assertions with application attributes are excluded.", NativeDecimalUnsupportedReason.Attributes);
        return new NativeDecimalSchema(root, children.ToImmutable(), [], limits)
        {
            ChoiceMinimum = minimum,
            AttributeUses = CompileAttributes(attributes, targetNamespace, attributeForm, decimalType, stringType, booleanType, integerType, simple, token),
            structuralAssertions = CompileStructuralAssertions(assertions, targetNamespace, assertionNamespace, token)
        };
    }
    private void AssessChoice(XElement owner, NativeDecimalSchema plan, ImmutableArray<NativeElementState>.Builder states,
        NativePatternBudget budget, CancellationToken token)
    {
        foreach(var text in owner.Nodes().OfType<XText>()) Check(IsWhitespace(text.Value), "Element-only choice contains non-whitespace text.");
        Require(owner.Nodes().All(n => n is XElement or XText or XComment or XProcessingInstruction), "Choice container comments or processing instructions are excluded.");
        var nodes = owner.Elements().ToArray();
        if (nodes.Length == 0) { Check(plan.ChoiceMinimum == 0, "Required choice is empty.");return; }
        var selected = plan.Children.FirstOrDefault(c => c.ExpandedName == nodes[0].Name);
        Check(selected is not null, "No choice alternative matches the original expanded name.");
        Check(nodes.Length >= selected!.MinOccurs && nodes.Length <= selected.MaxOccurs, "Choice selected occurrence violation.");
        foreach(var node in nodes)
        {
            token.ThrowIfCancellationRequested();Check(node.Name == selected.ExpandedName, "Choice mixes alternatives.");
            InstanceAttributes(node);Check(!node.HasElements, "Scalar choice alternative contains elements.");
            Require(node.Nodes().All(n => n is XText or XComment or XProcessingInstruction), "Scalar choice comments or processing instructions are excluded.");
            states.Add(AssessElementScalar(node, selected, budget));
        }
    }
}
