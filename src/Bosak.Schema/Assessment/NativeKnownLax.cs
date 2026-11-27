// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Assesses bounded anyType wildcard matches against original known global declarations.
// SPECIAL NOTES        : Bounded known and declaration-absent lax facts; no declaration or validation evidence is fabricated.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 26-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.21  | 26-11-2026     | Preserve absent lax declarations and original node identity.                              |
//                      | Charles Korthout | 0.22  | 27-11-2026     | Reject abstract known global matches before container dispatch.                           |
//                      | Charles Korthout | 0.23 | 27-11-2026     | Preserve bounded anyType nil states and independent wildcard attributes.                |
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    private void CheckKnownLaxInput(XDocument document, CancellationToken token)
    {
        var elements = 0; var attributes = 0;
        foreach (var node in document.Root!.DescendantsAndSelf())
        {
            token.ThrowIfCancellationRequested();
            Require(++elements <= 64 && node.Ancestors().Count() < 4, "Dynamic lax element/depth capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            var own = node.Attributes().Count(a => !IsAdmittedInstanceAttribute(a)); attributes += own;
            Require(own <= 16 && attributes <= 64, "Dynamic lax attribute capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        }
    }

    private static void CheckKnownLaxValues(NativeAssessedDocument document, CancellationToken token)
    {
        var values = 0;
        foreach (var state in document.Elements)
        {
            token.ThrowIfCancellationRequested();
            if (!state.GoverningType.HasNoTypedValue && !state.GoverningType.EmptyContent)
                Require(++values <= 16, "Dynamic lax value-state capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        }
    }

    private void AssessKnownLax(XElement element, NativeDeclaration? declaration, NativeType type,
        ImmutableArray<NativeElementState>.Builder states, ImmutableArray<NativeElementState>.Builder attributes,
        ImmutableArray<int> path, CancellationToken token, NativePatternBudget patternBudget, bool allowDeclarationAbsent)
    {
        token.ThrowIfCancellationRequested();
        var ownerIndex = states.Count;
        if (declaration is null)
        {
            Require(element.Value.Length <= maxStringCharacters!.Value, "Lax aggregate string capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            Require(element.Attributes().All(a => a.Name.NamespaceName != "http://www.w3.org/2001/XMLSchema-instance" || IsNamespaceOrLocationHint(a)), "Unknown lax xsi request awaits qualification.", NativeDecimalUnsupportedReason.Attributes);
            states.Add(new NativeElementState(null, null, StringValue: element.Value) { EffectiveType = type, OriginalName = element.Name });
        }
        else states.Add(ContainerValueState(element, declaration, type));
        AssessKnownLaxAttributes(element, attributes, path, ownerIndex, token, patternBudget, allowDeclarationAbsent);
        var index = 0;
        foreach (var child in element.Elements())
        {
            token.ThrowIfCancellationRequested();
            if (!roots.TryGetValue(child.Name, out var plan))
            {
                Require(allowDeclarationAbsent, "Unknown lax element below strict ancestor awaits projection.", NativeDecimalUnsupportedReason.DeclarationShape);
                AssessKnownLax(child, null, type, states, attributes, path.Add(index++), token, patternBudget, true);
                continue;
            }
            CheckConcreteDeclaration(plan!.Root);
            Require(plan.AllMinimum is null && plan.ChoiceMinimum is null, "Lax all/choice child plans await separate qualification.", NativeDecimalUnsupportedReason.DeclarationShape);
            var childPath = path.Add(index++);
            if (plan.Root.Type.IsComplex) AssessContainer(child, plan.Root, plan, states, attributes, childPath, token, patternBudget, allowDeclarationAbsent: allowDeclarationAbsent);
            else
            {
                InstanceAttributes(child);
                Check(!child.HasElements, "Known scalar lax child cannot contain child elements.");
                Require(child.Nodes().All(n => n is XText or XComment or XProcessingInstruction), "Known scalar lax XML node outside profile.", NativeDecimalUnsupportedReason.DeclarationShape);
                states.Add(AssessElementScalar(child, plan.Root, patternBudget));
            }
        }
        token.ThrowIfCancellationRequested();
    }
    private void AssessKnownLaxAttributes(XElement element, ImmutableArray<NativeElementState>.Builder attributes,
        ImmutableArray<int> path, int ownerIndex, CancellationToken token, NativePatternBudget patternBudget, bool allowDeclarationAbsent)
    {
        foreach (var attribute in element.Attributes().Where(a => !IsAdmittedInstanceAttribute(a)))
        {
            token.ThrowIfCancellationRequested();
            Require(GlobalAttributeDeclarations.Any(d => d.ExpandedName == attribute.Name) || allowDeclarationAbsent, "Unknown lax attribute below strict ancestor awaits projection.", NativeDecimalUnsupportedReason.Attributes);
            Require(attribute.Name.NamespaceName is not "http://www.w3.org/XML/1998/namespace" and not "http://www.w3.org/2001/XMLSchema-instance", "Special lax attribute is unsupported.", NativeDecimalUnsupportedReason.Attributes);
        }
        foreach (var global in GlobalAttributeDeclarations)
        {
            token.ThrowIfCancellationRequested();
            if (element.Attribute(global.ExpandedName) is { } attribute)
                attributes.Add(AssessAttributeValue(attribute.Value, global, patternBudget, element) with { OwnerElementPath = path, OwnerElementIndex = ownerIndex });
        }
        foreach (var attribute in element.Attributes().Where(a => !IsAdmittedInstanceAttribute(a) && !GlobalAttributeDeclarations.Any(d => d.ExpandedName == a.Name)))
        {
            token.ThrowIfCancellationRequested();
            Require(attribute.Value.Length <= maxStringCharacters!.Value, "Lax attribute string capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            attributes.Add(new NativeElementState(null, null, StringValue: attribute.Value) { EffectiveType = LaxAttributeType!, ActualAtomicType = UntypedAtomicType!, OriginalName = attribute.Name, OwnerElementPath = path, OwnerElementIndex = ownerIndex });
        }
    }

}
