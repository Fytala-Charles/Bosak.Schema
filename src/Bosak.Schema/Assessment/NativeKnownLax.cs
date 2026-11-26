// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Assesses bounded anyType wildcard matches against original known global declarations.
// SPECIAL NOTES        : Unknown lax declarations remain Unsupported; no declaration or validation evidence is fabricated.
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

    private void AssessKnownLax(XElement element, NativeDeclaration declaration, NativeType type,
        ImmutableArray<NativeElementState>.Builder states, ImmutableArray<NativeElementState>.Builder attributes,
        ImmutableArray<int> path, CancellationToken token, NativePatternBudget patternBudget)
    {
        token.ThrowIfCancellationRequested();
        var ownerIndex = states.Count;
        states.Add(ContainerValueState(element, declaration, type));
        foreach (var attribute in element.Attributes().Where(a => !IsAdmittedInstanceAttribute(a)))
        {
            token.ThrowIfCancellationRequested();
            Require(GlobalAttributeDeclarations.Any(d => d.ExpandedName == attribute.Name), "Unknown lax attribute declaration awaits provenance qualification.", NativeDecimalUnsupportedReason.Attributes);
        }
        foreach (var global in GlobalAttributeDeclarations)
        {
            token.ThrowIfCancellationRequested();
            if (element.Attribute(global.ExpandedName) is { } attribute)
                attributes.Add(AssessAttributeValue(attribute.Value, global, patternBudget, element) with { OwnerElementPath = path, OwnerElementIndex = ownerIndex });
        }
        var index = 0;
        foreach (var child in element.Elements())
        {
            token.ThrowIfCancellationRequested();
            Require(roots.TryGetValue(child.Name, out var plan), "Unknown lax element declaration awaits provenance qualification.", NativeDecimalUnsupportedReason.DeclarationShape);
            Require(plan!.AllMinimum is null && plan.ChoiceMinimum is null, "Lax all/choice child plans await separate qualification.", NativeDecimalUnsupportedReason.DeclarationShape);
            var childPath = path.Add(index++);
            if (plan.Root.Type.IsComplex) AssessContainer(child, plan.Root, plan, states, attributes, childPath, token, patternBudget);
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
}
