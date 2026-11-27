// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Shares direct restriction parsing and bounds original local anonymous definitions.
// SPECIAL NOTES        : Internal bounded profile; anonymous identities never acquire invented QNames.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 20-11-2026     | Admit direct opt-in integer restrictions with exact original type identity                 |
//                      | Charles Korthout | 0.3   | 21-11-2026     | Adopt bounded instance selection with original declaration and governing type identity |
//                      | Charles Korthout | 0.4   | 22-11-2026     | Prototype bounded string patterns and per-assessment accounting |
//                      | Charles Korthout | 0.5   | 22-11-2026     | Adopt qualified bounded native string patterns |
//                      | Charles Korthout | 0.6   | 22-11-2026     | Probe simple scalar roots in isolated source copies only |
//                      | Charles Korthout | 0.7   | 22-11-2026     | Adopt qualified native simple scalar root assessment |
//                      | Charles Korthout | 0.8   | 22-11-2026     | Probe derived integer builtins in isolated source copies |
//                      | Charles Korthout | 0.9   | 22-11-2026     | Adopt qualified bounded builtin integer family |
//                      | Charles Korthout | 0.10   | 23-11-2026     | Adopt qualified bounded native choice |
//                      | Charles Korthout | 0.11   | 23-11-2026     | Adopt qualified bounded root all groups |
//                      | Charles Korthout | 0.12   | 24-11-2026     | Probe integer enumeration in isolated source copies |
//                      | Charles Korthout | 0.13   | 24-11-2026     | Adopt qualified bounded integer enumeration |
//                      | Charles Korthout | 0.14   | 24-11-2026     | Probe revised bounded date transport in isolated source copy |
//                      | Charles Korthout | 0.15   | 24-11-2026     | Adopt qualified bounded native date transport |
//                      | Charles Korthout | 0.16   | 24-11-2026     | Probe bounded date enumeration in isolated source only                                      |
//                      | Charles Korthout | 0.17   | 24-11-2026     | Adopt qualified bounded date enumeration and internal value equality |
//                      | Charles Korthout | 0.18   | 24-11-2026     | Admit restrictions of named date types with original base identity |
//                      | Charles Korthout | 0.19   | 24-11-2026     | Use shared pattern compilation for direct and named date restrictions |
//                      | Charles Korthout | 0.20   | 24-11-2026     | Admit direct Boolean patterns and fixed-collapse whitespace |
//                      | Charles Korthout | 0.21   | 24-11-2026     | Resolve named Boolean bases with actual derivation identity |
//                      | Charles Korthout | 0.22   | 24-11-2026     | Keep decimal enumeration opt-in while preserving the legacy grammar |
//                      | Charles Korthout | 0.23   | 24-11-2026     | Admit closed named decimal bases with explicit native inherited-facet validation |
//                      | Charles Korthout | 0.24   | 25-11-2026     | Preserve simple nillability and actual typed-empty values |
//                      | Charles Korthout | 0.25   | 25-11-2026     | Admit bounded exact native calendar values and original typed transport |
//                      | Charles Korthout | 0.26   | 25-11-2026     | Add bounded binary and namespace-sensitive QName transport |
//                      | Charles Korthout | 0.27   | 25-11-2026     | Preserve bounded list/union types and ordered selected member facts |
//                      | Charles Korthout | 0.28   | 26-11-2026     | Support bounded text datatypes and inherited normalization facets |
//                      | Charles Korthout | 0.29   | 26-11-2026     | Preserve native URI and IEEE floating-point datatype values |
//                      | Charles Korthout | 0.30   | 26-11-2026     | Preserve bounded exact native duration values and provenance |
//                      | Charles Korthout | 0.31   | 26-11-2026     | Add bounded native Gregorian fragments with original typed provenance |
//                      | Charles Korthout | 0.32 | 26-11-2026     | Admit bounded empty groups and original anonymous attribute types |
//                      | Charles Korthout | 0.33 | 26-11-2026     | Compile opt-in numeric lexical patterns without changing legacy numeric facets |
//                      | Charles Korthout | 0.34 | 26-11-2026     | Compile implicit and explicit anyType roots with actual mixed terminal facts |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.35 | 27-11-2026     | Enforce bounded simple-type final derivation controls.                                   |
// ===========================================================================================================================================================

using System.Xml.Linq;

namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    private static NativeType CompileRestriction(XElement definition, string? name, string namespaceUri,
        NativeNumericLimits limits, int? maxStringCharacters, NativePatternCompilation patternCompilation, Dictionary<XName, NativeType> simple, int inlineDepth = 0)
    {
        Require(inlineDepth <= 4, "Inline compound depth exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var members = Elements(definition);
        if (maxStringCharacters is not null && members.Count == 1 && members[0].Name is var variety && (variety == Xs + "list" || variety == Xs + "union"))
            return CompileCompound(definition, name, namespaceUri, limits, maxStringCharacters.Value, patternCompilation, simple, inlineDepth);
        Attributes(definition, name is null ? [] : maxStringCharacters is null ? ["name"] : ["name", "final"]);
        var restriction = Single(definition, "restriction");
        Attributes(restriction, "base");
        var baseName = ResolveTypeName(restriction, "base");
        if (maxStringCharacters is not null && simple.TryGetValue(baseName, out var numericBase) &&
            (baseName == Xs + "decimal" || numericBase.IsInteger || numericBase.Facets is not null) &&
            (numericBase.NumericPatterns is not null || Elements(restriction).Any(f => f.Name == Xs + "pattern")))
            return CompileNumericPatternRestriction(restriction, name, namespaceUri, baseName, numericBase, limits, patternCompilation);
        if (maxStringCharacters is not null && simple.TryGetValue(baseName, out var newBase))
        {
            if (newBase.GregorianKind is { } gregorianKind) return new NativeType(name, false, namespaceUri, gregorianKind: gregorianKind, baseName: baseName, derivationBase: newBase,
                gregorianFacets: NativeGregorianFacets.Compile(restriction, gregorianKind, maxStringCharacters.Value, patternCompilation, newBase.GregorianFacets));
            if (newBase.DurationKind is { } durationKind) return new NativeType(name, false, namespaceUri, durationKind: durationKind, baseName: baseName, derivationBase: newBase,
                durationFacets: NativeDurationFacets.Compile(restriction, durationKind, limits, maxStringCharacters.Value, patternCompilation, newBase.DurationFacets));
            if (newBase.IsUri) return new NativeType(name, false, namespaceUri, isUri: true, textKind: NativeTextKind.Token, baseName: baseName, derivationBase: newBase,
                textFacets: NativeTextFacets.Compile(restriction, newBase, maxStringCharacters.Value, patternCompilation));
            if (newBase.FloatingKind is { } kind) return new NativeType(name, false, namespaceUri, floatingKind: kind, baseName: baseName, derivationBase: newBase,
                floatingFacets: NativeFloatingFacets.Compile(restriction, kind, maxStringCharacters.Value, patternCompilation, newBase.FloatingFacets));
        }
        if (maxStringCharacters is not null && simple.TryGetValue(baseName, out var compoundBase) && compoundBase.IsCompound)
            return new NativeType(name, false, namespaceUri, baseName: baseName, derivationBase: compoundBase, listItemType: compoundBase.ListItemType, unionMembers: compoundBase.UnionMembers,
                compoundFacets: NativeCompoundFacets.Compile(restriction, compoundBase, limits, maxStringCharacters.Value, patternCompilation));
        if (maxStringCharacters is not null && simple.TryGetValue(baseName, out var interchangeBase) && interchangeBase.InterchangeKind is { } interchangeKind)
            return new NativeType(name, false, namespaceUri, baseName: baseName, derivationBase: interchangeBase,
                interchangeKind: interchangeKind, interchangeFacets: NativeInterchangeFacets.Compile(restriction, interchangeKind, maxStringCharacters.Value, patternCompilation, interchangeBase.InterchangeFacets));
        if (maxStringCharacters is not null && simple.TryGetValue(baseName, out var calendarBase) && calendarBase.CalendarKind is { } calendarKind)
            return new NativeType(name, false, namespaceUri, baseName: baseName, derivationBase: calendarBase,
                calendarKind: calendarKind, calendarFacets: NativeCalendarFacets.Compile(restriction, calendarKind, limits, maxStringCharacters.Value, patternCompilation, calendarBase.CalendarFacets));
        if (maxStringCharacters is not null && baseName == Xs + "date")
        {
            var dateFacets = NativeDateFacets.Compile(restriction, maxStringCharacters.Value, patternCompilation);
            return new NativeType(name, false, namespaceUri, baseName: baseName, isDate: true, dateFacets: dateFacets);
        }
        if (maxStringCharacters is not null && simple.TryGetValue(baseName, out var dateBase) && dateBase.IsDate)
            return new NativeType(name, false, namespaceUri, baseName: baseName, derivationBase: dateBase,
                isDate: true, dateFacets: NativeDateFacets.CompileDerived(restriction, maxStringCharacters.Value, dateBase.DateFacets, patternCompilation));
        if (maxStringCharacters is not null && baseName == Xs + "boolean")
            return new NativeType(name, false, namespaceUri, isBoolean: true, baseName: baseName,
                booleanFacets: NativeBooleanFacets.Compile(restriction, patternCompilation));
        if (maxStringCharacters is not null && simple.TryGetValue(baseName, out var booleanBase) && booleanBase.IsBoolean)
            return new NativeType(name, false, namespaceUri, isBoolean: true, baseName: baseName, derivationBase: booleanBase,
                booleanFacets: NativeBooleanFacets.CompileDerived(restriction, patternCompilation, booleanBase.BooleanFacets));
        if (baseName == Xs + "decimal")
            return new NativeType(name, false, namespaceUri, NativeDecimalFacets.Compile(restriction, limits, allowEnumeration: maxStringCharacters is not null), baseName: baseName);
        if (maxStringCharacters is not null && simple.TryGetValue(baseName, out var decimalBase) && decimalBase.Facets is not null)
        {
            foreach (var facet in Elements(restriction))
            {
                Require(facet.Name.Namespace == Xs && facet.Name.LocalName is "enumeration" or "minInclusive" or "minExclusive" or "maxInclusive" or "maxExclusive" or "totalDigits" or "fractionDigits",
                    "Excluded named decimal facet syntax.", NativeDecimalUnsupportedReason.ValueType);
                Attributes(facet, "value");
                Require(Elements(facet).Count == 0, "Named decimal facet children are unsupported.", NativeDecimalUnsupportedReason.DeclarationShape);
            }
            return new NativeType(name, false, namespaceUri,
                facets: NativeDecimalFacets.CompileDerived(restriction, decimalBase.Facets, limits, nativeDerivation: true),
                baseName: baseName, derivationBase: decimalBase);
        }
        if (maxStringCharacters is not null && simple.TryGetValue(baseName, out var textBase) && textBase.IsString)
            return new NativeType(name, false, namespaceUri, isString: true, textKind: textBase.TextKind,
                textFacets: NativeTextFacets.Compile(restriction, textBase, maxStringCharacters.Value, patternCompilation), baseName: baseName, derivationBase: textBase);
        if (maxStringCharacters is not null && baseName == Xs + "integer")
            return new NativeType(name, false, namespaceUri, isInteger: true,
                integerFacets: NativeIntegerFacets.Compile(restriction, limits), baseName: baseName);
        if (maxStringCharacters is not null && baseName.Namespace == Xs && simple.TryGetValue(baseName, out var inherited) && inherited.IsInteger)
        {
            foreach (var facet in Elements(restriction))
            {
                Require(facet.Name.Namespace == Xs && facet.Name.LocalName is "minInclusive" or "minExclusive" or "maxInclusive" or "maxExclusive" or "totalDigits" or "fractionDigits" or "enumeration",
                    "Excluded integer facet syntax.", NativeDecimalUnsupportedReason.ValueType);
                Attributes(facet, "value");
                Require(Elements(facet).Count == 0, "Facet children are unsupported.", NativeDecimalUnsupportedReason.DeclarationShape);
            }
            return new NativeType(name, false, namespaceUri, isInteger: true,
                integerFacets: NativeIntegerFacets.CompileDerived(restriction, inherited.IntegerFacets, limits),
                baseName: baseName, derivationBase: inherited);
        }
        throw new NativeProfileException(NativeOutcome.Unsupported, "Only admitted direct scalar restrictions are supported.", NativeDecimalUnsupportedReason.ValueType);
    }

    private static Dictionary<XElement, NativeType> CompileAnonymousRestrictions(XElement schema,
        NativeNumericLimits limits, int? maxStringCharacters, CancellationToken cancellationToken, NativePatternCompilation patternCompilation, Dictionary<XName, NativeType> simple)
    {
        var result = new Dictionary<XElement, NativeType>();
        if (maxStringCharacters is null) return result;
        // Count source sites, including unused named definitions; shared references do not consume the budget twice.
        var definitions = schema.Descendants().Where(e =>
            e.Name == Xs + "simpleType" && e.Parent?.Name == Xs + "attribute" ||
            (e.Name == Xs + "simpleType" || e.Name == Xs + "complexType") &&
            e.Parent?.Name == Xs + "element" && (e.Parent.Parent?.Name == Xs + "sequence" || e.Parent.Parent?.Name == Xs + "choice" || e.Parent.Parent?.Name == Xs + "all" || e.Name == Xs + "simpleType" && e.Parent.Parent?.Name == Xs + "schema")).ToArray();
        Require(definitions.Length <= 16, "Local anonymous type ceiling exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        foreach (var definition in definitions.Where(e => e.Name == Xs + "simpleType"))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Require(definition.Parent!.Attribute("type") is null, "Inline and named types cannot coexist.", NativeDecimalUnsupportedReason.DeclarationShape);
            var type = CompileRestriction(definition, null, "", limits, maxStringCharacters, patternCompilation, simple);
            result.Add(definition, type);
            if (definition.Parent.Name == Xs + "attribute") definition.AddAnnotation(type);
        }
        return result;
    }
    private static NativeDecimalSchema? TryCompileSimpleRoot(XElement declaration, string targetNamespace,
        NativeType decimalType, NativeType? stringType, NativeType? booleanType, NativeType? integerType,
        Dictionary<XName, NativeType> simple, Dictionary<XElement, NativeType> anonymousSimple, NativeNumericLimits limits)
    {
        if (stringType is null) return null; // Preserve the legacy facade grammar.
        if (IsAnyTypeElement(declaration)) return CompileAnyTypeTerminal(declaration, Name(declaration), 1, 1, targetNamespace, limits);
        NativeType? type = null;
        if (declaration.Attribute("type") is not null)
        {
            Require(Elements(declaration).Count == 0, "Inline and named types cannot coexist.", NativeDecimalUnsupportedReason.DeclarationShape);
            var name = ResolveTypeName(declaration);
            type = name == Xs + "decimal" ? decimalType : name == Xs + "string" ? stringType :
                name == Xs + "boolean" ? booleanType : name == Xs + "integer" ? integerType : simple.GetValueOrDefault(name);
        }
        else
        {
            var members = Elements(declaration);
            if (members.Count == 1 && members[0].Name == Xs + "simpleType") type = anonymousSimple[members[0]];
        }
        return type is null ? null : new NativeDecimalSchema(ElementDeclaration(declaration, Name(declaration), type, 1, 1, targetNamespace), [], [], limits);
    }

}
