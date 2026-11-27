// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Compiles admitted anonymous scalar bases without synthetic names or source cloning.
// SPECIAL NOTES        : Original base identity and facet sources remain authoritative.
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
//                      | Charles Korthout | 0.2 | 27-11-2026 | Admit original integer and Boolean base objects with facet ancestry. |
//                      | Charles Korthout | 0.3 | 27-11-2026 | Admit original date base objects with bounded facet ancestry. |
//                      | Charles Korthout | 0.4 | 27-11-2026 | Admit original dateTime/time bases with exact calendar facet ancestry. |
// ===========================================================================================================================================================
using System.Xml.Linq;
using Bosak.XPath.Core.Xdm;
namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    private static NativeType? CompileAnonymousScalarBase(XElement restriction, XElement inlineSource, NativeType baseType,
        string? name, string ns, NativeNumericLimits limits, int maxCharacters, NativePatternCompilation compilation)
    {
        compilation.Check();
        if (baseType.CalendarKind is { } calendarKind && calendarKind is XdmExactCalendarKind.DateTime or XdmExactCalendarKind.Time)
            return new NativeType(name, false, ns, calendarKind: calendarKind, derivationBase: baseType,
                calendarFacets: NativeCalendarFacets.Compile(restriction, calendarKind, limits, maxCharacters, compilation, baseType.CalendarFacets, inlineSource));
        if (baseType.IsDate)
            return new NativeType(name, false, ns, isDate: true, derivationBase: baseType,
                dateFacets: NativeDateFacets.CompileDerived(restriction, maxCharacters, baseType.DateFacets, compilation, inlineSource));
        if (baseType.IsInteger)
        {
            var values = Elements(restriction).Where(f => !ReferenceEquals(f, inlineSource) && f.Name != Xs + "pattern").ToArray();
            var hasPatterns = baseType.NumericPatterns is not null || Elements(restriction).Any(f => f.Name == Xs + "pattern");
            Require(baseType.NumericPatterns is null || !values.Any(f => f.Name.LocalName is "enumeration" or "minInclusive" or "minExclusive" or "maxInclusive" or "maxExclusive"),
                "Value facets over patterned numeric bases await separate adoption.", NativeDecimalUnsupportedReason.ValueType);
            var patterns = hasPatterns ? NativeNumericPatterns.Compile(restriction, baseType.NumericPatterns, compilation) : null;
            var facets = NativeIntegerFacets.CompileDerived(restriction, baseType.IntegerFacets, limits, inlineSource, hasPatterns);
            return new NativeType(name, false, ns, isInteger: true, integerFacets: facets, derivationBase: baseType, numericPatterns: patterns);
        }
        if (baseType.IsBoolean)
            return new NativeType(name, false, ns, isBoolean: true, derivationBase: baseType,
                booleanFacets: NativeBooleanFacets.CompileDerived(restriction, compilation, baseType.BooleanFacets, inlineSource));
        if (!baseType.IsInteger && baseType.Facets is not null)
        {
            var values = Elements(restriction).Where(f => !ReferenceEquals(f, inlineSource) && f.Name != Xs + "pattern").ToArray();
            var hasPatterns = baseType.NumericPatterns is not null || Elements(restriction).Any(f => f.Name == Xs + "pattern");
            Require(!hasPatterns || baseType.NumericPatterns is null || !values.Any(f => f.Name.LocalName is "enumeration" or "minInclusive" or "minExclusive" or "maxInclusive" or "maxExclusive"),
                "Value facets over patterned numeric bases await separate adoption.", NativeDecimalUnsupportedReason.ValueType);
            var patterns = hasPatterns ? NativeNumericPatterns.Compile(restriction, baseType.NumericPatterns, compilation) : null;
            var facets = NativeDecimalFacets.CompileDerived(restriction, baseType.Facets, limits, nativeDerivation: true,
                inlineBase: inlineSource, omitPatterns: hasPatterns);
            return new NativeType(name, false, ns, facets: facets, derivationBase: baseType, numericPatterns: patterns);
        }
        if (baseType.IsString && !baseType.IsUri && baseType.TextKind is NativeTextKind.String or NativeTextKind.NormalizedString or NativeTextKind.Token)
            return new NativeType(name, false, ns, isString: true, textKind: baseType.TextKind, derivationBase: baseType,
                textFacets: NativeTextFacets.Compile(restriction, baseType, maxCharacters, compilation, inlineSource));
        return null;
    }
}
