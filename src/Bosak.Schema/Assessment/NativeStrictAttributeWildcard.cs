// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Compiles and validates bounded strict attribute wildcard constraints.
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
//                      | Charles Korthout | 0.2   | 26-11-2026     | Enforce bounded namespace/QName exclusions and schema consistency.                         |
//                      | Charles Korthout | 0.3   | 26-11-2026     | Retain strict wildcard metadata on finite sequence owners.                               |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml;
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed record NativeStrictAttributeWildcard(string Mode, string TargetNamespace, ImmutableHashSet<string> Namespaces)
{
    internal ImmutableHashSet<XName> DisallowedNames { get; init; } = ImmutableHashSet<XName>.Empty;
    internal bool DisallowDefined { get; init; }
    internal bool Allows(string ns) => Mode == "##any" || (Mode == "##other" ? ns.Length != 0 && ns != TargetNamespace : Mode == "not" ? !Namespaces.Contains(ns) : Namespaces.Contains(ns));
    internal bool AllowsName(XName name, bool defined) => Allows(name.NamespaceName) && !DisallowedNames.Contains(name) && !(DisallowDefined && defined);
}

internal sealed partial class NativeDecimalSchema
{
    private static (XElement[] Uses, NativeStrictAttributeWildcard? Wildcard, XElement[] Assertions) CompileSequenceWildcardTail(XElement complex, string targetNamespace)
    {
        var members = Elements(complex);
        var uses = members.Skip(1).TakeWhile(IsAttributeUseMember).ToArray();
        var rest = members.Skip(1 + uses.Length).ToArray();
        var source = rest.FirstOrDefault()?.Name == Xs + "anyAttribute" ? rest[0] : null;
        var assertions = rest.Skip(source is null ? 0 : 1).ToArray();
        if (source is not null)
            Require(members[0].Name == Xs + "sequence" && Elements(members[0]).Count != 0 && !ReadMixedContent(complex) && assertions.Length == 0,
                "Sequence wildcard requires direct nonempty element-only sequence without assertions.", NativeDecimalUnsupportedReason.DeclarationShape);
        return (uses, source is null ? null : CompileStrictAttributeWildcard(source, targetNamespace), assertions);
    }

    private static NativeStrictAttributeWildcard CompileStrictAttributeWildcard(XElement source, string targetNamespace)
    {
        Attributes(source, "namespace", "notNamespace", "notQName", "processContents");
        Require(Elements(source).Count == 0, "Wildcard children are outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
        Require(Collapse((string?)source.Attribute("processContents") ?? "strict") == "strict", "Only strict attribute wildcards are admitted.", NativeDecimalUnsupportedReason.DeclarationShape);
        Check(source.Attribute("namespace") is null || source.Attribute("notNamespace") is null, "namespace and notNamespace cannot coexist.");
        var negative = source.Attribute("notNamespace") is not null;
        var text = Collapse((string?)source.Attribute(negative ? "notNamespace" : "namespace") ?? "##any");
        var tokens = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (negative) Check(tokens.Length != 0, "A negative namespace set must not be empty.");
        Require(tokens.Length is >= 1 and <= 8 && tokens.All(t => t.Length <= 256), "Attribute wildcard namespace capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        if (!negative && (text is "##any" or "##other")) return CompleteStrictNameExclusions(source, new(text, targetNamespace, ImmutableHashSet<string>.Empty));
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
        return CompleteStrictNameExclusions(source, new(negative ? "not" : "enumeration", targetNamespace, namespaces.ToImmutable()));
    }

    private static NativeStrictAttributeWildcard CompleteStrictNameExclusions(XElement source, NativeStrictAttributeWildcard wildcard)
    {
        var tokens = Collapse((string?)source.Attribute("notQName") ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Require(tokens.Length <= 8 && tokens.All(t => t.Length <= 256), "Attribute wildcard QName capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var names = ImmutableHashSet.CreateBuilder<XName>();
        var defined = false;
        foreach (var token in tokens)
        {
            if (token == "##defined") { defined = true; continue; }
            var parts = token.Split(':');
            Check(parts.Length is 1 or 2 && parts.All(p => p.Length != 0), "Invalid excluded QName.");
            try { foreach (var part in parts) XmlConvert.VerifyNCName(part); }
            catch (XmlException) { throw new NativeProfileException(NativeOutcome.Invalid, "Invalid excluded QName."); }
            var ns = parts.Length == 1 ? source.GetDefaultNamespace().NamespaceName : source.GetNamespaceOfPrefix(parts[0])?.NamespaceName;
            Check(ns is not null, "Unbound excluded QName prefix.");
            Check(wildcard.Allows(ns!), "Excluded QName namespace must be allowed by the wildcard.");
            names.Add(XName.Get(parts[^1], ns!));
        }
        return wildcard with { DisallowedNames = names.ToImmutable(), DisallowDefined = defined };
    }

    private void AssessStrictAttributeExtras(XElement element, NativeDecimalSchema plan,
        ImmutableArray<NativeElementState>.Builder attributes, ImmutableArray<int> path, int ownerIndex,
        CancellationToken token, NativePatternBudget patternBudget)
    {
        var extras = element.Attributes().Where(a => !IsAdmittedInstanceAttribute(a) && !plan.AttributeUses.Any(u => u.ExpandedName == a.Name)).ToArray();
        foreach (var attribute in extras)
        {
            token.ThrowIfCancellationRequested();
            var defined = GlobalAttributeDeclarations.Any(d => d.ExpandedName == attribute.Name);
            Check(plan.AttributeWildcard is { } wildcard && wildcard.AllowsName(attribute.Name, defined), "Attribute does not match the strict wildcard.");
            Check(defined, "Strict wildcard attribute has no global declaration.");
        }
        foreach (var declaration in GlobalAttributeDeclarations)
        {
            token.ThrowIfCancellationRequested();
            if (extras.FirstOrDefault(a => a.Name == declaration.ExpandedName) is { } attribute)
                attributes.Add(AssessAttributeValue(attribute.Value, declaration, patternBudget, element) with { OwnerElementPath = path, OwnerElementIndex = ownerIndex });
        }
    }
}
