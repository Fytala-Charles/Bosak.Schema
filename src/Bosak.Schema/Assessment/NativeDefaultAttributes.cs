// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 28 November 2026
// PURPOSE              : Applies original default attribute-group uses to bounded source complex owners.
// SPECIAL NOTES        : Internal source-bound compilation context; no instance augmentation or synthetic declarations.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 28-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.2 | 28-11-2026 | Qualify scoped closed attributes on default-group opt-out owners. |
//                      | Charles Korthout | 0.3 | 28-11-2026 | Admit original default-group uses on existing bounded all/choice owners. |
//                      | Charles Korthout | 0.4 | 28-11-2026 | Qualify original default groups on existing noninteger simple-content extensions. |
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml;
using System.Xml.Linq;

namespace Bosak.Schema;

internal sealed class NativeDefaultAttributeUses(ImmutableArray<NativeDeclaration> uses)
{
    internal ImmutableArray<NativeDeclaration> Uses { get; } = uses;
}

internal sealed partial class NativeDecimalSchema
{
    private bool ClosedDefaultAttributeOptOut { get; set; }

    private static void PrepareDefaultAttributeGroup(XElement schema, CancellationToken token)
    {
        if (schema.Attribute("defaultAttributes") is not { } supplied) return;
        token.ThrowIfCancellationRequested();
        Require(supplied.Value.Length <= 256, "Default group QName capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var text = Collapse(supplied.Value); var parts = text.Split(':');
        Check(parts.Length is 1 or 2 && parts.All(p => p.Length != 0), "Invalid default attribute-group QName.");
        try { foreach (var part in parts) XmlConvert.VerifyNCName(part); }
        catch (Exception e) when (e is XmlException or ArgumentException)
        { throw new NativeProfileException(NativeOutcome.Invalid, "Invalid default attribute-group QName."); }
        var ns = parts.Length == 1 ? schema.GetDefaultNamespace().NamespaceName : schema.GetNamespaceOfPrefix(parts[0])?.NamespaceName;
        Check(ns is not null, "Unbound default attribute-group prefix.");
        var context = NativeAttributeGroups.For(schema);
        Require(context is not null, "Default attribute-group context unavailable.", NativeDecimalUnsupportedReason.Attributes);
        schema.AddAnnotation(new NativeDefaultAttributeUses(context!.Resolve(XName.Get(parts[^1], ns!))));
    }

    private static bool ReadDefaultAttributesApply(XElement complex)
    {
        if (complex.Attribute("defaultAttributesApply") is not { } supplied) return true;
        Require(supplied.Value.Length <= 256, "Default application Boolean capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var flag = Collapse(supplied.Value);
        Check(flag is "true" or "1" or "false" or "0", "Invalid defaultAttributesApply Boolean.");
        return flag is "true" or "1";
    }

    private static NativeDecimalSchema ApplyDefaultAttributeUses(XElement complex, NativeDecimalSchema plan,
        bool apply, CancellationToken token)
    {
        var context = complex.AncestorsAndSelf().Select(e => e.Annotation<NativeDefaultAttributeUses>()).FirstOrDefault(c => c is not null);
        if (context is null) return plan;
        var members = Elements(complex);
        if (!apply)
        {
            plan.ClosedDefaultAttributeOptOut = plan.AttributeUses.IsEmpty && plan.AttributeWildcard is null &&
                !plan.Root.Type.MixedContent && plan.Root.Type.ContentType is null &&
                plan.AllMinimum is null && plan.ChoiceMinimum is null &&
                !members.Any(e => e.Name == Xs + "assert" || e.Name == Xs + "simpleContent" || e.Name == Xs + "complexContent");
            return plan;
        }

        var simpleExtension = plan.Root.Type.ContentType is { IsInteger: false, IsBoolean: false } &&
            members.Count == 1 && members[0].Name == Xs + "simpleContent" &&
            Elements(members[0]) is { Count: 1 } content && content[0].Name == Xs + "extension" &&
            !Elements(content[0]).Any(e => e.Name == Xs + "assert");
        Require(!plan.Root.Type.MixedContent && plan.CompiledAttributeWildcard is null &&
            (simpleExtension || (plan.Root.Type.ContentType is null &&
                !members.Any(e => e.Name == Xs + "assert" || e.Name == Xs + "simpleContent" || e.Name == Xs + "complexContent"))),
            "Default groups require bounded element-only owners or existing noninteger simple-content extensions without assertions/wildcards.", NativeDecimalUnsupportedReason.DeclarationShape);
        var uses = plan.AttributeUses.ToBuilder();
        var names = uses.ToDictionary(d => d.ExpandedName);
        foreach (var use in context.Uses)
        {
            token.ThrowIfCancellationRequested();
            if (names.TryGetValue(use.ExpandedName, out var previous))
            { Check(ReferenceEquals(previous, use), "Distinct default and explicit attribute uses have the same expanded name."); continue; }
            Require(uses.Count < 16, "Effective default attribute-use capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            names.Add(use.ExpandedName, use); uses.Add(use);
        }
        plan.AttributeUses = uses.ToImmutable();
        return plan;
    }

    private static NativeDecimalSchema CompileComplex(XElement complex, NativeDeclaration root,
        string targetNamespace, string defaultForm, string attributeForm, string assertionNamespace, NativeType decimalType,
        NativeType? stringType, NativeType? booleanType, NativeType? integerType, Dictionary<XName, NativeType> simple,
        NativeNumericLimits limits, CancellationToken cancellationToken, Dictionary<XElement, NativeType> anonymousSimple,
        Func<XName, NativeDecimalSchema?>? complexResolver = null, bool nestedContext = false,
        int? maxStringCharacters = null, NativePatternCompilation? patternCompilation = null)
    {
        var apply = stringType is not null && ReadDefaultAttributesApply(complex);
        var plan = CompileComplexCore(complex, root, targetNamespace, defaultForm, attributeForm, assertionNamespace,
            decimalType, stringType, booleanType, integerType, simple, limits, cancellationToken, anonymousSimple,
            complexResolver, nestedContext, maxStringCharacters, patternCompilation);
        return stringType is null ? plan : ApplyDefaultAttributeUses(complex, plan, apply, cancellationToken);
    }
}
