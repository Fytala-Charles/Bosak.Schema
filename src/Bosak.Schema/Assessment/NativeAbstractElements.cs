// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Preserves immutable abstract global declaration identities and rejects direct use.
// SPECIAL NOTES        : Abstractness belongs to canonical global declarations, never shared types or local names.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    private ImmutableHashSet<NativeDeclaration> abstractDeclarations = ImmutableHashSet<NativeDeclaration>.Empty;

    private void CompleteAbstractMetadata(XElement schema, CancellationToken token)
    {
        var declarations = ImmutableHashSet.CreateBuilder<NativeDeclaration>();
        var ns = Collapse((string?)schema.Attribute("targetNamespace") ?? "");
        foreach (var element in schema.Elements(Xs + "element"))
        {
            token.ThrowIfCancellationRequested();
            if (element.Attribute("abstract") is not { } flag) continue;
            Require(flag.Value.Length <= 256, "Abstract Boolean source capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            var value = Collapse(flag.Value);
            Check(value is "true" or "false" or "1" or "0", "Invalid abstract Boolean.");
            if (value is "true" or "1") declarations.Add(roots[XName.Get(Name(element), ns)].Root.CanonicalDeclaration);
        }
        abstractDeclarations = declarations.ToImmutable();
    }

    private void CheckConcreteDeclaration(NativeDeclaration declaration) =>
        Check(!abstractDeclarations.Contains(declaration.CanonicalDeclaration), "An abstract global element cannot be used directly.");
}
