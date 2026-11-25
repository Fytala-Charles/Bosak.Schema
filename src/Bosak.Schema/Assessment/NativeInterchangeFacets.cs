// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Compiles finite binary and QName facet chains.
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
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Globalization;
using System.Numerics;
using System.Xml.Linq;

namespace Bosak.Schema;

internal sealed class NativeInterchangeFacets(NativeInterchangeFacets? parent, int? length, int? minimum, int? maximum,
    ImmutableArray<NativeInterchangeValue> enumeration, ImmutableArray<NativePattern> patterns)
{
    private readonly Dictionary<string, int> fixedLengths = new(StringComparer.Ordinal);
    private NativeInterchangeFacets? Parent => parent;
    private int EffectiveMinimum => length ?? minimum ?? parent?.EffectiveMinimum ?? 0;
    private int EffectiveMaximum => length ?? maximum ?? parent?.EffectiveMaximum ?? int.MaxValue;
    internal static NativeInterchangeFacets Compile(XElement restriction, NativeInterchangeKind kind, int maxCharacters,
        NativePatternCompilation compilation, NativeInterchangeFacets? parent)
    {
        int? length = null, minimum = null, maximum = null;
        var enumeration = ImmutableArray.CreateBuilder<NativeInterchangeValue>();
        var patterns = ImmutableArray.CreateBuilder<NativePattern>();
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var fixedValues = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var facet in NativeDecimalSchema.Elements(restriction))
        {
            compilation.Check();
            var name = facet.Name.LocalName;
            NativeDecimalSchema.Require(facet.Name.NamespaceName == "http://www.w3.org/2001/XMLSchema" &&
                name is "length" or "minLength" or "maxLength" or "enumeration" or "pattern" or "whiteSpace",
                "Excluded interchange facet.", NativeDecimalUnsupportedReason.ValueType);
            NativeDecimalSchema.Require(name is "enumeration" or "pattern" || seen.Add(name), "Duplicate interchange facet.", NativeDecimalUnsupportedReason.DeclarationShape);
            NativeDecimalSchema.Attributes(facet, "value", "fixed");
            NativeDecimalSchema.Require(NativeDecimalSchema.Elements(facet).Count == 0, "Facet children are excluded.", NativeDecimalUnsupportedReason.DeclarationShape);
            var supplied = (string?)facet.Attribute("value");
            NativeDecimalSchema.Require(supplied is not null, "Facet value required.", NativeDecimalUnsupportedReason.ValueType);
            NativeDecimalSchema.Require(supplied!.Length <= maxCharacters, "Interchange facet literal capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            var isFixed = false;
            if (facet.Attribute("fixed") is { } fixedFlag)
            {
                NativeDecimalSchema.Require(name is not ("pattern" or "enumeration"), "Fixed pattern/enumeration syntax is excluded.", NativeDecimalUnsupportedReason.ValueType);
                var flag = NativeCalendarParser.Collapse(fixedFlag.Value);
                if (flag is not ("true" or "1" or "false" or "0")) throw new NativeProfileException(NativeOutcome.Invalid, "Invalid facet fixed Boolean.");
                isFixed = flag is "true" or "1";
            }
            if (name == "whiteSpace")
            {
                if (NativeCalendarParser.Collapse(supplied!) != "collapse") throw new NativeProfileException(NativeOutcome.Invalid, "Interchange whiteSpace is fixed to collapse.");
            }
            else if (name == "enumeration")
            {
                NativeDecimalSchema.Require(enumeration.Count < 16, "Interchange enumeration capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
                var value = NativeInterchangeParser.Parse(supplied!, kind, maxCharacters, facet);
                // Membership in a patterned base also depends on the lexical mapping; do not infer it from value equality alone.
                NativeDecimalSchema.Require(parent is null || !parent.HasPatterns, "Enumeration over patterned interchange bases awaits qualification.", NativeDecimalUnsupportedReason.ValueType);
                if (parent is not null && !parent.Accepts(value, compilation.MatchingBudget))
                    throw new NativeProfileException(NativeOutcome.Invalid, "Enumeration outside base value space.");
                enumeration.Add(value);
            }
            else if (name == "pattern")
            {
                NativePattern.Capacity(patterns.Count < 8);
                patterns.Add(NativePattern.Compile(supplied!, compilation));
            }
            else
            {
                NativeDecimalSchema.Require(kind != NativeInterchangeKind.QName, "QName length facets await qualification.", NativeDecimalUnsupportedReason.ValueType);
                NativeDecimalSchema.Require(supplied!.Length <= 256, "Length lexical capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
                var lexical = NativeCalendarParser.Collapse(supplied!);
                var start = lexical.StartsWith('+') || lexical.StartsWith('-') ? 1 : 0;
                if (lexical.Length <= start || lexical.Skip(start).Any(c => c is < '0' or > '9')) throw new NativeProfileException(NativeOutcome.Invalid, "Invalid length facet.");
                var value = BigInteger.Parse(lexical, CultureInfo.InvariantCulture);
                if (value < 0) throw new NativeProfileException(NativeOutcome.Invalid, "Negative length facet.");
                NativeDecimalSchema.Require(value <= 65536, "Length facet capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
                if (name == "length") length = (int)value;
                else if (name == "minLength") minimum = (int)value;
                else maximum = (int)value;
                for (var ancestor = parent; ancestor is not null; ancestor = ancestor.Parent)
                    if (ancestor.fixedLengths.TryGetValue(name, out var required) && required != (int)value)
                        throw new NativeProfileException(NativeOutcome.Invalid, "Fixed binary length facet changed.");
                if (isFixed) fixedValues.Add(name, (int)value);
            }
        }
        if (length is not null && (minimum is not null || maximum is not null) || minimum > maximum)
            throw new NativeProfileException(NativeOutcome.Invalid, "Inconsistent length facets.");
        if (parent is not null && (length is { } exact && (exact < parent.EffectiveMinimum || exact > parent.EffectiveMaximum) ||
            minimum is { } lo && lo < parent.EffectiveMinimum || maximum is { } hi && hi > parent.EffectiveMaximum))
            throw new NativeProfileException(NativeOutcome.Invalid, "Binary length facets widen the base.");
        var result = new NativeInterchangeFacets(parent, length, minimum, maximum, enumeration.ToImmutable(), patterns.ToImmutable());
        if (result.EffectiveMinimum > result.EffectiveMaximum) throw new NativeProfileException(NativeOutcome.Invalid, "Empty binary length interval.");
        foreach (var pair in fixedValues) result.fixedLengths.Add(pair.Key, pair.Value);
        return result;
    }

    private bool HasPatterns => !patterns.IsEmpty || parent?.HasPatterns == true;
    internal bool Accepts(NativeInterchangeValue value, NativePatternBudget budget) =>
        (parent?.Accepts(value, budget) ?? true) &&
        (length is null || value.Octets.Length == length) &&
        (minimum is null || value.Octets.Length >= minimum) &&
        (maximum is null || value.Octets.Length <= maximum) &&
        (enumeration.IsEmpty || enumeration.Any(value.ValueEquals)) &&
        (patterns.IsEmpty || patterns.Any(pattern => pattern.Match(value.Lexical, budget)));
}
