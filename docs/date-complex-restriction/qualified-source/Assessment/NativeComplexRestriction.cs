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
//                      | Charles Korthout | 0.2   | 21-11-2026     | Adopt bounded integer simple content with actual component identity |
//                      | Charles Korthout | 0.3   | 21-11-2026     | Adopt bounded contextual and inherited integer assertions |
//                      | Charles Korthout | 0.4   | 24-11-2026     | Probe integer enumeration in isolated source copies |
//                      | Charles Korthout | 0.5   | 24-11-2026     | Adopt qualified bounded integer enumeration |
//                      | Charles Korthout | 0.6   | 24-11-2026     | Probe bounded date simple content in isolated source copy |
//                      | Charles Korthout | 0.7   | 24-11-2026     | Adopt qualified bounded date simple-content extension |
//                      | Charles Korthout | 0.8   | 24-11-2026     | Admit bounded date complex restriction with distinct atomic content |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


using System.Xml;
using System.Xml.Linq;

namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    private static NativeDecimalSchema CompileComplexRestriction(XElement restriction, NativeDeclaration root,
        string targetNamespace, Dictionary<XName, NativeType> simple, NativeNumericLimits limits,
        Func<XName, NativeDecimalSchema?>? resolveComplex, CancellationToken token, string assertionNamespace, int? maxStringCharacters)
    {
        token.ThrowIfCancellationRequested();
        Attributes(restriction, "base");
        var children = Elements(restriction);
        var localAssertions = children.Where(e => e.Name == Xs + "assert").ToArray();
        Require(!children.SkipWhile(e => e.Name != Xs + "assert").Any(e => e.Name != Xs + "assert"), "Facets must precede assertions.", NativeDecimalUnsupportedReason.DeclarationShape);
        foreach (var child in children.Where(e => e.Name != Xs + "assert"))
        {
            token.ThrowIfCancellationRequested();
            Require(child.Name != Xs + "attribute" && child.Name != Xs + "attributeGroup" && child.Name != Xs + "anyAttribute",
                "Restriction attribute uses are outside the profile.", NativeDecimalUnsupportedReason.Attributes);
            Require(child.Name != Xs + "assert", "Restriction assertions are outside the profile.", NativeDecimalUnsupportedReason.AssertionGrammar);
            Require(child.Name != Xs + "simpleType", "Inline restriction types are outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
            Require(child.Name.Namespace == Xs && child.Name.LocalName is "minInclusive" or "minExclusive" or "maxInclusive" or "maxExclusive" or "totalDigits" or "fractionDigits" or "enumeration" or "explicitTimezone",
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
        Require(content is not null && !content.IsComplex && !content.IsString && !content.IsBoolean,
            "Complex base content is outside the decimal profile.", NativeDecimalUnsupportedReason.ValueType);
        if (content!.IsDate)
        {
            Require(localAssertions.Length == 0 && basePlan.integerAssertions.IsEmpty,
                "Date restriction assertions are outside the profile.", NativeDecimalUnsupportedReason.AssertionGrammar);
            var dateCharacters = maxStringCharacters ?? throw new NativeProfileException(NativeOutcome.Unsupported,
                "Date restriction requires the native scalar profile.", NativeDecimalUnsupportedReason.ValueType);
            var dateFacets = new XElement(restriction.Name, restriction.Attributes(), children);
            var dateAtomic = new NativeType(null, false, targetNamespace, isDate: true,
                dateFacets: NativeDateFacets.CompileDerived(dateFacets, dateCharacters, content.DateFacets), derivationBase: content);
            var dateGoverning = new NativeType(root.Type.Name, false, targetNamespace, contentType: dateAtomic,
                derivationBase: baseType, isComplexRestriction: true);
            return new NativeDecimalSchema(new NativeDeclaration(root.Name, dateGoverning, root.MinOccurs, root.MaxOccurs, root.NamespaceUri), [], [], limits);
        }
        Require(content!.IsInteger || localAssertions.Length == 0 && basePlan.integerAssertions.IsEmpty,
            "Contextual assertions on noninteger content are outside the profile.", NativeDecimalUnsupportedReason.AssertionGrammar);
        Require(content.IsInteger || !children.Any(e=>e.Name==Xs+"enumeration"),
            "Decimal enumeration is outside the integer slice.",NativeDecimalUnsupportedReason.ValueType);
        var facetsOnly = new XElement(restriction.Name, restriction.Attributes(), children.Where(e => e.Name != Xs + "assert"));
        var inheritedAssertions = basePlan.integerAssertions;
        var effectiveAssertions = inheritedAssertions.AddRange(CompileIntegerAssertions(localAssertions, targetNamespace, assertionNamespace, limits, token));
        Require(effectiveAssertions.Length <= 8, "Effective integer assertion capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var atomic = content!.IsInteger
            ? new NativeType(null, false, targetNamespace, isInteger: true,
                integerFacets: NativeIntegerFacets.CompileDerived(facetsOnly, content.IntegerFacets, limits), derivationBase: content)
            : new NativeType(null, false, targetNamespace,
                facets: NativeDecimalFacets.CompileDerived(facetsOnly, content.Facets, limits), derivationBase: content);
        var governing = new NativeType(root.Type.Name, false, targetNamespace, contentType: atomic,
            derivationBase: baseType, isComplexRestriction: true);
        return new NativeDecimalSchema(new NativeDeclaration(root.Name, governing, root.MinOccurs, root.MaxOccurs, root.NamespaceUri), [], [], limits) { integerAssertions = effectiveAssertions };
    }
}
