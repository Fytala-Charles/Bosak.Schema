// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Compiles and enforces bounded named string length and enumeration facets.
// SPECIAL NOTES        : Internal finite native profile; no Core adapter or full XSD 1.1 conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 22-11-2026     | Prototype bounded string patterns and per-assessment accounting |
//                      | Charles Korthout | 0.3   | 22-11-2026     | Adopt qualified bounded native string patterns |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Globalization;
using System.Numerics;
using System.Xml.Linq;

namespace Bosak.Schema;

/// <summary>Compiles and enforces bounded named string length and enumeration facets.</summary>
internal sealed class NativeStringFacets(int? length, int? minimum, int? maximum, ImmutableHashSet<string>? enumeration, ImmutableArray<NativePattern> patterns)
{
    internal static NativeStringFacets Compile(XElement restriction, int maxStringCharacters, NativePatternCompilation compilation)
    {
        int? length = null, minimum = null, maximum = null;
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var values = ImmutableHashSet.CreateBuilder<string>(StringComparer.Ordinal);
        var suppliedEnumerations = 0;
        var patterns = ImmutableArray.CreateBuilder<NativePattern>();
        foreach (var facet in NativeDecimalSchema.Elements(restriction))
        {
            compilation.Check();
            NativeDecimalSchema.Require(facet.Name.NamespaceName == "http://www.w3.org/2001/XMLSchema" &&
                facet.Name.LocalName is "length" or "minLength" or "maxLength" or "enumeration" or "pattern",
                "Unsupported string facet.", NativeDecimalUnsupportedReason.ValueType);
            var kind = facet.Name.LocalName;
            NativeDecimalSchema.Require(kind is "enumeration" or "pattern" || seen.Add(kind), "Duplicate string facet.", NativeDecimalUnsupportedReason.DeclarationShape);
            NativeDecimalSchema.Attributes(facet, "value");
            NativeDecimalSchema.Require(NativeDecimalSchema.Elements(facet).Count == 0, "Facet children are unsupported.", NativeDecimalUnsupportedReason.DeclarationShape);
            var supplied = (string?)facet.Attribute("value");
            NativeDecimalSchema.Require(supplied is not null, "Facet value required.", NativeDecimalUnsupportedReason.ValueType);
            if (kind == "pattern")
            {
                NativePattern.Capacity(patterns.Count < 8);
                patterns.Add(NativePattern.Compile(supplied!, compilation));
                continue;
            }
            if (kind == "enumeration")
            {
                NativeDecimalSchema.Require(++suppliedEnumerations <= 16 && supplied!.Length <= maxStringCharacters,
                    "String enumeration capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
                values.Add(supplied!);
                continue;
            }
            NativeDecimalSchema.Require(supplied!.Length <= 256, "String length facet lexical capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            var lexical = string.Join(" ", supplied.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries));
            var start = lexical.Length > 0 && lexical[0] is '+' or '-' ? 1 : 0;
            NativeDecimalSchema.Require(lexical.Length > start && lexical.Skip(start).All(c => c is >= '0' and <= '9'),
                "Invalid string length facet lexical form.", NativeDecimalUnsupportedReason.ValueType);
            var value = BigInteger.Parse(lexical, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
            NativeDecimalSchema.Require(value >= 0, "Negative string length facet.", NativeDecimalUnsupportedReason.ValueType);
            NativeDecimalSchema.Require(value <= 65536, "String length facet capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            if (kind == "length") length = (int)value;
            else if (kind == "minLength") minimum = (int)value;
            else maximum = (int)value;
        }
        NativeDecimalSchema.Require(length is null || minimum is null && maximum is null,
            "Direct string restrictions cannot combine length and bounds.", NativeDecimalUnsupportedReason.ValueType);
        NativeDecimalSchema.Require(minimum is null || maximum is null || minimum <= maximum,
            "Inconsistent string length bounds.", NativeDecimalUnsupportedReason.ValueType);
        return new(length, minimum, maximum, suppliedEnumerations == 0 ? null : values.ToImmutable(), patterns.ToImmutable());
    }

    internal bool Accepts(string value, NativePatternBudget budget)
    {
        var count = 0;
        foreach (var _ in value.EnumerateRunes()) { budget.Tick(); count++; }
        return (length is null || count == length) && (minimum is null || count >= minimum) &&
            (maximum is null || count <= maximum) && (enumeration is null || enumeration.Contains(value)) &&
            (patterns.IsEmpty || patterns.Any(pattern => pattern.Match(value, budget)));
    }
}
