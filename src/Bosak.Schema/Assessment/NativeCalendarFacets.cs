// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Compiles exact native calendar facets under explicit bounds.
// SPECIAL NOTES        : Part of bounded native exact calendar validation.
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
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml.Linq;
using Bosak.XPath.Core.Xdm;
namespace Bosak.Schema;

internal sealed class NativeCalendarFacets
{
    private NativeCalendarFacets? parent;
    private readonly List<XdmExactCalendarValue> entries = [];
    private XdmExactCalendarValue? lower, upper;
    private bool lowerExclusive, upperExclusive;
    private string timezone = "optional";
    private readonly Dictionary<string, (string Text, XdmExactCalendarValue? Value)> fixedFacets = new(StringComparer.Ordinal);
    private ImmutableArray<NativePattern> patterns = [];
    private bool HasPatterns => !patterns.IsEmpty || parent?.HasPatterns == true;

    internal static NativeCalendarFacets Compile(XElement restriction, XdmExactCalendarKind kind, NativeNumericLimits limits,
        int maxCharacters, NativePatternCompilation compilation, NativeCalendarFacets? parent)
    {
        var result = new NativeCalendarFacets { parent = parent, timezone = parent?.timezone ?? (kind == XdmExactCalendarKind.DateTimeStamp ? "required" : "optional") };
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var patternBuilder = ImmutableArray.CreateBuilder<NativePattern>();
        var facets = NativeDecimalSchema.Elements(restriction);
        NativeDecimalSchema.Require(facets.Count(f => f.Name.LocalName == "enumeration") <= 16, "Calendar enumeration capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        NativePattern.Capacity(facets.Count(f => f.Name.LocalName == "pattern") <= 8);
        foreach (var facet in facets)
        {
            compilation.Check();
            var name = facet.Name.LocalName;
            NativeDecimalSchema.Require(facet.Name.NamespaceName == "http://www.w3.org/2001/XMLSchema" && name is "enumeration" or "pattern" or "whiteSpace" or "explicitTimezone" or "minInclusive" or "minExclusive" or "maxInclusive" or "maxExclusive", "Unsupported calendar facet.", NativeDecimalUnsupportedReason.ValueType);
            if (name is "enumeration" or "pattern") NativeDecimalSchema.Attributes(facet, "value");
            else NativeDecimalSchema.Attributes(facet, "value", "fixed");
            NativeDecimalSchema.Require(NativeDecimalSchema.Elements(facet).Count == 0, "Calendar facet children unsupported.", NativeDecimalUnsupportedReason.DeclarationShape);
            if (name is not ("enumeration" or "pattern") && !seen.Add(name)) Invalid("Duplicate calendar facet.");
            var text = (string?)facet.Attribute("value");
            if (text is null) Invalid("Calendar facet value required.");
            NativeDecimalSchema.Require(text!.Length <= maxCharacters, "Calendar facet literal capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            if (name != "pattern") text = NativeCalendarParser.Collapse(text);
            var isFixed = false;
            if (facet.Attribute("fixed") is { } fixedAttribute)
            {
                var fixedText = NativeCalendarParser.Collapse(fixedAttribute.Value);
                if (fixedText is not ("true" or "false" or "1" or "0")) Invalid("Invalid fixed facet flag.");
                isFixed = fixedText is "true" or "1";
            }
            XdmExactCalendarValue? value = null;
            if (name == "pattern") patternBuilder.Add(NativePattern.Compile(text, compilation));
            else if (name == "whiteSpace") { if (text != "collapse") Invalid("Calendar whitespace is fixed to collapse."); }
            else if (name == "explicitTimezone")
            {
                if (text is not ("optional" or "required" or "prohibited")) Invalid("Invalid explicitTimezone value.");
                if (result.timezone != "optional" && text != result.timezone) Invalid("Timezone facet widens the base.");
                result.timezone = text;
            }
            else
            {
                value = NativeCalendarParser.Parse(text, kind, limits, maxCharacters);
                if (name != "enumeration") NativeDecimalSchema.Require(parent?.HasPatterns != true, "Calendar bounds over patterned ancestors require separate value-space qualification.", NativeDecimalUnsupportedReason.ValueType);
                if (parent is not null)
                {
                    if (name == "enumeration") { if (!parent.Accepts(value.Value, text, compilation.MatchingBudget)) Invalid("Enumeration is outside the base type."); }
                    else if (!parent.ValuesAccept(value.Value, name)) Invalid("Calendar bound is outside the base type.");
                }
                if (name == "enumeration") result.entries.Add(value.Value);
                else if (name.StartsWith("min", StringComparison.Ordinal)) { result.lower = value; result.lowerExclusive = name == "minExclusive"; }
                else { result.upper = value; result.upperExclusive = name == "maxExclusive"; }
            }
            for (var ancestor = parent; ancestor is not null; ancestor = ancestor.parent)
                if (ancestor.fixedFacets.TryGetValue(name, out var fixedValue) &&
                    (value is { } supplied && fixedValue.Value is { } required ? !XdmExactCalendarComparison.SameKey(supplied, required) : text != fixedValue.Text)) Invalid("A fixed calendar facet cannot change.");
            if (isFixed) result.fixedFacets.Add(name, (text, value));
        }
        if (seen.Contains("minInclusive") && seen.Contains("minExclusive") || seen.Contains("maxInclusive") && seen.Contains("maxExclusive")) Invalid("Conflicting calendar bound kinds.");
        if (result.lower is { } lo && result.upper is { } hi)
        {
            var order = XdmExactCalendarComparison.CompareXsd(lo, hi);
            if (order == XdmExactCalendarOrder.Greater || order == XdmExactCalendarOrder.Equal && result.lowerExclusive != result.upperExclusive) Invalid("Inconsistent calendar bounds.");
        }
        result.patterns = patternBuilder.ToImmutable();
        return result;
    }

    internal bool Accepts(XdmExactCalendarValue value, string lexical, NativePatternBudget budget) => ValuesAccept(value) && PatternsAccept(lexical, budget);
    private bool PatternsAccept(string lexical, NativePatternBudget budget) => (parent is null || parent.PatternsAccept(lexical, budget)) && (patterns.IsEmpty || patterns.Any(p => p.Match(lexical, budget)));
    private bool ValuesAccept(XdmExactCalendarValue value, string? endpointException = null)
    {
        if (parent is not null && !parent.ValuesAccept(value, endpointException)) return false;
        if (timezone == "required" && !value.HasTimezone || timezone == "prohibited" && value.HasTimezone) return false;
        if (entries.Count != 0 && !entries.Any(e => XdmExactCalendarComparison.SameKey(e, value))) return false;
        if (lower is { } lo)
        {
            var order = XdmExactCalendarComparison.CompareXsd(value, lo);
            if (order is XdmExactCalendarOrder.Less or XdmExactCalendarOrder.Indeterminate || order == XdmExactCalendarOrder.Equal && lowerExclusive && endpointException != "minExclusive") return false;
        }
        if (upper is { } hi)
        {
            var order = XdmExactCalendarComparison.CompareXsd(value, hi);
            if (order is XdmExactCalendarOrder.Greater or XdmExactCalendarOrder.Indeterminate || order == XdmExactCalendarOrder.Equal && upperExclusive && endpointException != "maxExclusive") return false;
        }
        return true;
    }
    private static void Invalid(string message) => throw new NativeProfileException(NativeOutcome.Invalid, message);
}
