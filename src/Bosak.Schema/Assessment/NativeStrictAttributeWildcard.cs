// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Compiles and validates bounded strict attribute wildcards on empty complex owners.
// SPECIAL NOTES        : Internal bounded strict-only wildcard profile; original global declarations retain authority.
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

internal sealed record NativeStrictAttributeWildcard(string Mode, string TargetNamespace, ImmutableHashSet<string> Namespaces)
{
    internal bool Allows(string ns) => Mode == "##any" || (Mode == "##other" ? ns.Length != 0 && ns != TargetNamespace : Namespaces.Contains(ns));
}

internal sealed partial class NativeDecimalSchema
{
    private static NativeStrictAttributeWildcard CompileStrictAttributeWildcard(XElement source, string targetNamespace)
    {
        Attributes(source, "namespace", "processContents");
        Require(Elements(source).Count == 0, "Wildcard children are outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
        Require(Collapse((string?)source.Attribute("processContents") ?? "strict") == "strict", "Only strict attribute wildcards are admitted.", NativeDecimalUnsupportedReason.DeclarationShape);
        var text = Collapse((string?)source.Attribute("namespace") ?? "##any");
        var tokens = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Require(tokens.Length is >= 1 and <= 8 && tokens.All(t => t.Length <= 256), "Attribute wildcard namespace capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        if (text is "##any" or "##other") return new(text, targetNamespace, ImmutableHashSet<string>.Empty);
        var namespaces = ImmutableHashSet.CreateBuilder<string>(StringComparer.Ordinal);
        foreach (var token in tokens)
        {
            if (token == "##targetNamespace") namespaces.Add(targetNamespace);
            else if (token == "##local") namespaces.Add("");
            else
            {
                Require(!token.StartsWith("##", StringComparison.Ordinal) && Uri.TryCreate(token, UriKind.Absolute, out _), "Namespace token is outside the bounded wildcard profile.", NativeDecimalUnsupportedReason.DeclarationShape);
                namespaces.Add(token);
            }
        }
        return new("enumeration", targetNamespace, namespaces.ToImmutable());
    }

    private void AssessStrictAttributeExtras(XElement element, NativeDecimalSchema plan,
        ImmutableArray<NativeElementState>.Builder attributes, ImmutableArray<int> path, int ownerIndex,
        CancellationToken token, NativePatternBudget patternBudget)
    {
        var extras = element.Attributes().Where(a => !IsAdmittedInstanceAttribute(a) && !plan.AttributeUses.Any(u => u.ExpandedName == a.Name)).ToArray();
        foreach (var attribute in extras)
        {
            token.ThrowIfCancellationRequested();
            Check(plan.Root.Type.AttributeWildcard is { } wildcard && wildcard.Allows(attribute.Name.NamespaceName), "Attribute does not match the strict wildcard.");
            Check(GlobalAttributeDeclarations.Any(d => d.ExpandedName == attribute.Name), "Strict wildcard attribute has no global declaration.");
        }
        foreach (var declaration in GlobalAttributeDeclarations)
        {
            token.ThrowIfCancellationRequested();
            if (extras.FirstOrDefault(a => a.Name == declaration.ExpandedName) is { } attribute)
                attributes.Add(AssessAttributeValue(attribute.Value, declaration, patternBudget, element) with { OwnerElementPath = path, OwnerElementIndex = ownerIndex });
        }
    }
}
