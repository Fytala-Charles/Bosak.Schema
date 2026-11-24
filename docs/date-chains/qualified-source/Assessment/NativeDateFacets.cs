// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Compiles bounded date enumeration and exact XSD date value equality.
// SPECIAL NOTES        : Bounded native date facet validation; no public equality changes.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 24-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 24-11-2026     | Adopt qualified bounded date enumeration and internal value equality |
//                      | Charles Korthout | 0.3   | 24-11-2026     | Guard restriction text through the shared schema parser |
//                      | Charles Korthout | 0.4   | 24-11-2026     | Add bounded date ordering facets and explicit timezone constraints |
//                      | Charles Korthout | 0.5   | 24-11-2026     | Validate inherited date facets and exclusive endpoint exceptions |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Numerics;
using System.Xml.Linq;
using Bosak.XPath.Core.Xdm;

namespace Bosak.Schema;

internal sealed class NativeDateFacets(
    HashSet<(bool Zoned, BigInteger Minutes)>? entries,
    XPathDateTime? lower = null, bool lowerExclusive = false,
    XPathDateTime? upper = null, bool upperExclusive = false,
    string timezone = "optional", NativeDateFacets? parent = null)
{
    private NativeDateFacets? Parent => parent;
    private XPathDateTime? Lower => lower;
    private XPathDateTime? Upper => upper;
    private bool LowerExclusive => lowerExclusive;
    private bool UpperExclusive => upperExclusive;
    private string Timezone => timezone;

    internal static NativeDateFacets? Compile(XElement restriction, int maxCharacters) =>
        CompileDerived(restriction, maxCharacters, null);

    internal static NativeDateFacets? CompileDerived(XElement restriction, int maxCharacters, NativeDateFacets? parent)
    {
        var facets = NativeDecimalSchema.Elements(restriction);
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var facet in facets)
        {
            NativeDecimalSchema.Require(facet.Name.NamespaceName == "http://www.w3.org/2001/XMLSchema" &&
                facet.Name.LocalName is "enumeration" or "minInclusive" or "minExclusive" or "maxInclusive" or "maxExclusive" or "explicitTimezone",
                "Unsupported date facet.", NativeDecimalUnsupportedReason.ValueType);
            NativeDecimalSchema.Attributes(facet, "value");
            NativeDecimalSchema.Require(NativeDecimalSchema.Elements(facet).Count == 0,
                "Date facet children unsupported.", NativeDecimalUnsupportedReason.DeclarationShape);
            if (facet.Name.LocalName != "enumeration" && !seen.Add(facet.Name.LocalName))
                Invalid("Duplicate date facet.");
        }
        NativeDecimalSchema.Require(facets.Count(f => f.Name.LocalName == "enumeration") <= 16,
            "Date enumeration source capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        if (facets.Count == 0) return parent is null ? null : new(null, parent: parent);
        if (seen.Contains("minInclusive") && seen.Contains("minExclusive") ||
            seen.Contains("maxInclusive") && seen.Contains("maxExclusive")) Invalid("Conflicting date bound kinds.");
        HashSet<(bool Zoned, BigInteger Minutes)>? entries = null;
        XPathDateTime? lower = null, upper = null;
        var lowerExclusive = seen.Contains("minExclusive");
        var upperExclusive = seen.Contains("maxExclusive");
        var timezone = "optional";
        foreach (var facet in facets)
        {
            var text = (string?)facet.Attribute("value");
            if (text is null) Invalid("Date facet value required.");
            NativeDecimalSchema.Require(text!.Length <= maxCharacters, "Date facet character capacity exceeded.",
                NativeDecimalUnsupportedReason.RepresentationCapacity);
            var kind = facet.Name.LocalName;
            if (kind == "explicitTimezone")
            {
                timezone = string.Join(" ", text.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries));
                if (timezone is not ("optional" or "required" or "prohibited")) Invalid("Invalid explicitTimezone value.");
                for (var inherited = parent; inherited is not null; inherited = inherited.Parent)
                    if (inherited.Timezone != "optional" && inherited.Timezone != timezone)
                        Invalid("Timezone facet widens the base type.");
                continue;
            }
            var value = NativeDateParser.Parse(text);
            if (parent is not null) ValidateDerivedValue(kind, value, parent);
            if (kind == "enumeration") (entries ??= []).Add(Key(value));
            else if (kind.StartsWith("min", StringComparison.Ordinal)) lower = value;
            else upper = value;
        }
        if (lower is { } lo && upper is { } hi)
        {
            var compare = Compare(lo, hi);
            if (compare > 0 || compare == 0 && lowerExclusive != upperExclusive) Invalid("Inconsistent date bounds.");
        }
        return new(entries, lower, lowerExclusive, upper, upperExclusive, timezone, parent);
    }

    internal bool Accepts(XPathDateTime value)
    {
        if (parent is not null && !parent.Accepts(value)) return false;
        if (timezone == "required" && !value.HasTimezone || timezone == "prohibited" && value.HasTimezone) return false;
        if (entries is not null && !entries.Contains(Key(value))) return false;
        if (lower is { } lo)
        {
            var compare = Compare(value, lo);
            if (compare is null || compare < 0 || lowerExclusive && compare == 0) return false;
        }
        if (upper is { } hi)
        {
            var compare = Compare(value, hi);
            if (compare is null || compare > 0 || upperExclusive && compare == 0) return false;
        }
        return true;
    }

    internal static int? Compare(XPathDateTime a, XPathDateTime b)
    {
        var x = Key(a); var y = Key(b);
        if (x.Zoned == y.Zoned) return x.Minutes.CompareTo(y.Minutes);
        if (!x.Zoned)
        {
            if (x.Minutes + 840 < y.Minutes) return -1;
            if (x.Minutes - 840 > y.Minutes) return 1;
        }
        else
        {
            if (x.Minutes < y.Minutes - 840) return -1;
            if (x.Minutes > y.Minutes + 840) return 1;
        }
        return null;
    }

    private static void ValidateDerivedValue(string kind, XPathDateTime value, NativeDateFacets parent)
    {
        if (kind == "enumeration")
        {
            if (!parent.Accepts(value)) Invalid("Enumeration member is outside the base type.");
            return;
        }
        var minimum = kind.StartsWith("min", StringComparison.Ordinal);
        var exclusive = kind.EndsWith("Exclusive", StringComparison.Ordinal);
        var repeatsExclusiveEndpoint = false;
        for (var inherited = parent; inherited is not null; inherited = inherited.Parent)
        {
            if (inherited.Lower is { } lo)
            {
                var compare = Compare(value, lo);
                if (minimum)
                {
                    if (compare < 0 || !exclusive && inherited.LowerExclusive && compare == 0)
                        Invalid("Lower bound widens the base type.");
                    if (exclusive && inherited.LowerExclusive && Key(value) == Key(lo)) repeatsExclusiveEndpoint = true;
                }
                else if (compare < 0 || (exclusive || inherited.LowerExclusive) && compare == 0)
                    Invalid("Upper bound conflicts with the base lower bound.");
            }
            if (inherited.Upper is { } hi)
            {
                var compare = Compare(value, hi);
                if (!minimum)
                {
                    if (compare > 0 || !exclusive && inherited.UpperExclusive && compare == 0)
                        Invalid("Upper bound widens the base type.");
                    if (exclusive && inherited.UpperExclusive && Key(value) == Key(hi)) repeatsExclusiveEndpoint = true;
                }
                else if (compare > 0 || (exclusive || inherited.UpperExclusive) && compare == 0)
                    Invalid("Lower bound conflicts with the base upper bound.");
            }
        }
        if (!repeatsExclusiveEndpoint && !parent.Accepts(value)) Invalid("Date bound is outside the base type.");
    }

    private static void Invalid(string message) => throw new NativeProfileException(NativeOutcome.Invalid, message);

    internal static (bool Zoned, BigInteger Minutes) Key(XPathDateTime value)
    {
        // March-based Gregorian ordinal. Floor division is essential for negative years.
        BigInteger year = value.Year;
        int month = value.Month;
        if (month <= 2) year--;
        var era = BigInteger.DivRem(year, 400, out var remainder);
        if (remainder.Sign < 0) era--;
        var yoe = year - era * 400;
        int shifted = month + (month > 2 ? -3 : 9);
        var days = era * 146097 + yoe * 365 + yoe / 4 - yoe / 100 + (153 * shifted + 2) / 5 + value.Day - 1;
        return (value.HasTimezone, days * 1440 - (value.HasTimezone ? value.TimezoneOffsetMinutes : 0));
    }
}
