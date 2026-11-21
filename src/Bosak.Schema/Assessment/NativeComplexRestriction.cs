// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Compiles bounded complex simple-content restriction with original component identity.
// SPECIAL NOTES        : Internal finite native profile; no Core adapter or full XSD 1.1 conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 21-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


using System.Xml;
using System.Xml.Linq;

namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    private static NativeDecimalSchema CompileComplexRestriction(XElement restriction, NativeDeclaration root,
        string targetNamespace, Dictionary<XName, NativeType> simple, NativeNumericLimits limits,
        Func<XName, NativeDecimalSchema?>? resolveComplex, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        Attributes(restriction, "base");
        var children = Elements(restriction);
        foreach (var child in children)
        {
            token.ThrowIfCancellationRequested();
            Require(child.Name != Xs + "attribute" && child.Name != Xs + "attributeGroup" && child.Name != Xs + "anyAttribute",
                "Restriction attribute uses are outside the profile.", NativeDecimalUnsupportedReason.Attributes);
            Require(child.Name != Xs + "assert", "Restriction assertions are outside the profile.", NativeDecimalUnsupportedReason.AssertionGrammar);
            Require(child.Name != Xs + "simpleType", "Inline restriction types are outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
            Require(child.Name.Namespace == Xs && child.Name.LocalName is "minInclusive" or "minExclusive" or "maxInclusive" or "maxExclusive" or "totalDigits" or "fractionDigits",
                "Restriction facet is outside the profile.", NativeDecimalUnsupportedReason.ValueType);
            Attributes(child, "value");
            Require(Elements(child).Count == 0, "Facet child grammar is outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
        }
        var lexical = Collapse((string?)restriction.Attribute("base") ?? "");
        var parts = lexical.Split(':');
        Check(parts.Length is 1 or 2 && parts.All(part => part.Length != 0), "Invalid complex base QName.");
        try { foreach (var part in parts) XmlConvert.VerifyNCName(part); }
        catch (Exception error) when (error is XmlException or ArgumentException)
        { throw new NativeProfileException(NativeOutcome.Invalid, "Invalid complex base QName."); }
        var ns = parts.Length == 1 ? restriction.GetDefaultNamespace().NamespaceName : restriction.GetNamespaceOfPrefix(parts[0])?.NamespaceName;
        Check(ns is not null, "Complex base prefix is unbound.");
        var name = XName.Get(parts[^1], ns!);
        if (name.Namespace == Xs)
        {
            Check(KnownBuiltinNames.Contains(name.LocalName), "Complex base is absent from closed schema.");
            throw new NativeProfileException(NativeOutcome.Unsupported, "Complex base kind is outside the profile.", NativeDecimalUnsupportedReason.ValueType);
        }
        Require(!simple.ContainsKey(name), "Simple base kind is outside the complex restriction profile.", NativeDecimalUnsupportedReason.ValueType);
        var basePlan = resolveComplex?.Invoke(name);
        Check(basePlan is not null, "Complex base is absent from closed schema.");
        var baseType = basePlan!.Root.Type;
        Require(!baseType.IsComplexRestriction, "Complex restriction chains are outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
        Require(basePlan.AttributeUses.IsEmpty, "Base attribute uses are outside the restriction profile.", NativeDecimalUnsupportedReason.Attributes);
        Require(basePlan.assertions.IsEmpty, "Base assertions are outside the restriction profile.", NativeDecimalUnsupportedReason.AssertionGrammar);
        var content = baseType.ContentType;
        Require(content is not null && !content.IsComplex && !content.IsString && !content.IsBoolean && !content.IsInteger,
            "Complex base content is outside the decimal profile.", NativeDecimalUnsupportedReason.ValueType);
        var facets = NativeDecimalFacets.CompileDerived(restriction, content!.Facets, limits);
        var atomic = new NativeType(null, false, targetNamespace, facets: facets, derivationBase: content);
        var governing = new NativeType(root.Type.Name, false, targetNamespace, contentType: atomic,
            derivationBase: baseType, isComplexRestriction: true);
        return new NativeDecimalSchema(new NativeDeclaration(root.Name, governing, root.MinOccurs, root.MaxOccurs, root.NamespaceUri), [], [], limits);
    }
}
