// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Validates method-specific final controls on global simple type definitions.
// SPECIAL NOTES        : Source-bound derivation edges are checked before any compiled type is published.
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
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    private static void ValidateSimpleTypeFinal(XElement schema, string targetNamespace, CancellationToken token)
    {
        var finals = new Dictionary<XName, HashSet<string>>();
        foreach (var definition in schema.Elements(Xs + "simpleType"))
        {
            token.ThrowIfCancellationRequested();
            if (definition.Attribute("final") is not { } attribute) continue;
            Require(attribute.Value.Length <= 256, "Simple-type final source capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            var text = Collapse(attribute.Value);
            var methods = text == "#all" ? new HashSet<string>(["restriction", "extension", "list", "union"]) : text.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet(StringComparer.Ordinal);
            Check(methods.All(m => m is "restriction" or "extension" or "list" or "union"), "Invalid simple-type final value.");
            var key = XName.Get(Name(definition), DefinitionNamespace(definition, targetNamespace));
            Check(finals.TryAdd(key, methods), "Duplicate named simple type.");
        }
        if (finals.Count == 0) return;
        foreach (var edge in schema.Descendants())
        {
            token.ThrowIfCancellationRequested();
            if (edge.Name == Xs + "restriction" || edge.Name == Xs + "extension")
            {
                if (edge.Attribute("base") is { } source) CheckEdge(edge, source.Value, edge.Name.LocalName);
            }
            else if (edge.Name == Xs + "list" && edge.Attribute("itemType") is { } item) CheckEdge(edge, item.Value, "list");
            else if (edge.Name == Xs + "union" && edge.Attribute("memberTypes") is { } members)
            {
                Require(members.Value.Length <= 256, "Union member source capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
                foreach (var member in members.Value.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries)) CheckEdge(edge, member, "union");
            }
        }
        void CheckEdge(XElement edge, string source, string method)
        {
            var key = CompoundName(edge, source);
            Check(!finals.TryGetValue(key, out var methods) || !methods.Contains(method), "Simple-type final prohibits " + method + " derivation.");
        }
    }
}
