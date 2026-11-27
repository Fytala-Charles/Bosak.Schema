// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Compiles and assesses bounded native list and union types.
// SPECIAL NOTES        : Part of bounded native datatype validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 25-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.2 | 27-11-2026     | Enforce bounded simple-type final derivation controls.                                   |
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml.Linq;

namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    private static XName CompoundName(XElement source, string text)
    {
        var name = NativeInterchangeParser.Parse(text, NativeInterchangeKind.QName, 256, source).Name;
        return XName.Get(name.LocalName, name.NamespaceUri);
    }

    private static IEnumerable<XName> CompoundDependencies(XElement definition)
    {
        foreach (var e in definition.Descendants())
        {
            if (e.Name == Xs + "restriction" && e.Attribute("base") is not null) yield return ResolveTypeName(e, "base");
            if (e.Name == Xs + "list" && e.Attribute("itemType") is { } item) yield return CompoundName(e, item.Value);
            if (e.Name == Xs + "union" && e.Attribute("memberTypes") is { } members)
            {
                Require(members.Value.Length <= 256, "MemberTypes lexical capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
                foreach (var token in members.Value.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries)) yield return CompoundName(e, token);
            }
        }
    }

    private static NativeType CompileCompound(XElement definition, string? name, string ns, NativeNumericLimits limits,
        int maxCharacters, NativePatternCompilation compilation, Dictionary<XName, NativeType> simple, int depth)
    {
        Attributes(definition, name is null ? [] : ["name", "final"]);
        var construction = Elements(definition).Single();
        var list = construction.Name == Xs + "list";
        Attributes(construction, list ? ["itemType"] : ["memberTypes"]);
        var inline = Elements(construction);
        Require(inline.All(e => e.Name == Xs + "simpleType"), "Compound inline grammar excluded.", NativeDecimalUnsupportedReason.DeclarationShape);
        NativeType Resolve(string token)
        {
            var key = CompoundName(construction, token);
            Require(simple.TryGetValue(key, out var type), "Compound member outside closed admitted types.", NativeDecimalUnsupportedReason.ValueType);
            return type!;
        }
        NativeType Inline(XElement e) => CompileRestriction(e, null, "", limits, maxCharacters, compilation, simple, depth + 1);
        if (list)
        {
            var item = construction.Attribute("itemType");
            Check(item is null ? inline.Count == 1 : inline.Count == 0, "List requires exactly one named or inline item type.");
            var type = item is null ? Inline(inline[0]) : Resolve(item.Value);
            var visited = new HashSet<NativeType>(ReferenceEqualityComparer.Instance);
            bool ContainsList(NativeType t)
            {
                compilation.Check();
                return visited.Add(t) && (t.ListItemType is not null || t.IsUnion && t.UnionMembers.Any(ContainsList));
            }
            Check(!ContainsList(type), "A list item cannot contain a list member.");
            return new NativeType(name, false, ns, listItemType: type);
        }
        var tokens = ((string?)construction.Attribute("memberTypes") ?? "").Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        Require(((string?)construction.Attribute("memberTypes") ?? "").Length <= 256 && tokens.Length + inline.Count <= 8,
            "Union member capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var members = tokens.Select(Resolve).Concat(inline.Select(Inline)).ToImmutableArray();
        return new NativeType(name, false, ns, unionMembers: members);
    }

    private NativeElementState AssessCompound(string text, NativeDeclaration declaration, NativePatternBudget budget, XElement? scope)
    {
        budget.CompoundAttempt();
        Require(text.Length <= 256 && text.Length <= maxStringCharacters, "Compound lexical capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        NativeElementState value;
        var type = declaration.Type;
        if (type.ListItemType is { } item)
        {
            var tokens = text.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            Require(tokens.Length <= 16, "List item capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            var items = ImmutableArray.CreateBuilder<NativeElementState>();
            foreach (var token in tokens)
            {
                budget.CompoundAttempt();
                items.Add(AssessScalar(token, new NativeDeclaration("item", item, 1, 1), budget, scope));
            }
            value = new NativeElementState(declaration, null) { Items = items.ToImmutable() };
        }
        else
        {
            NativeElementState? selected = null;
            foreach (var member in type.UnionMembers)
            {
                budget.CompoundAttempt();
                try
                {
                    var parsed = AssessScalar(text, new NativeDeclaration("member", member, 1, 1), budget, scope);
                    selected = parsed with { Declaration = declaration, ActualAtomicType = parsed.ActualAtomicType ?? member };
                    break;
                }
                catch (NativeProfileException e) when (e.Outcome == NativeOutcome.Invalid) { }
            }
            Check(selected is not null, "No union member accepts the value.");
            value = selected!;
        }
        Check(type.CompoundFacets?.Accepts(value, text, budget) ?? true, "Compound facet violation.");
        return value;
    }

    internal static NativeElementState ParseCompoundLiteral(string text, XElement scope, NativeType type, NativeNumericLimits limits, int maxCharacters, NativePatternBudget budget)
    {
        var declaration = new NativeDeclaration("literal", type, 1, 1);
        var parser = new NativeDecimalSchema(declaration, [], [], limits) { maxStringCharacters = maxCharacters };
        return parser.AssessScalar(text, declaration, budget, scope);
    }
}
