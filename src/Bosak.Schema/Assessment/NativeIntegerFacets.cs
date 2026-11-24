// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Compiles and enforces bounded exact integer restriction facets.
// SPECIAL NOTES        : BigInteger value-space checks; inherited fractionDigits zero remains fixed.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 21-11-2026     | Adopt bounded integer simple content with actual component identity |
//                      | Charles Korthout | 0.3   | 24-11-2026     | Probe integer enumeration in isolated source copies |
//                      | Charles Korthout | 0.4   | 24-11-2026     | Keep invalid-value reporting inside the facet compiler |
//                      | Charles Korthout | 0.5   | 24-11-2026     | Adopt qualified bounded integer enumeration |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Globalization;
using System.Numerics;
using System.Xml.Linq;

namespace Bosak.Schema;

/// <summary>Compiles and enforces bounded exact integer restriction facets.</summary>
internal sealed class NativeIntegerFacets(BigInteger? lower, bool lowerExclusive, BigInteger? upper, bool upperExclusive, int? totalDigits, NativeIntegerFacets? parent = null, ImmutableHashSet<BigInteger>? enumeration = null)
{
    internal ImmutableHashSet<BigInteger>? Enumeration => enumeration;
    internal BigInteger? LowerBound => lower ?? parent?.LowerBound;
    internal BigInteger? UpperBound => upper ?? parent?.UpperBound;
    internal bool LowerExclusive => lower is not null ? lowerExclusive : parent?.LowerExclusive ?? false;
    internal bool UpperExclusive => upper is not null ? upperExclusive : parent?.UpperExclusive ?? false;
    internal int? TotalLimit => totalDigits ?? parent?.TotalLimit;
    internal int? FractionLimit => 0;

    internal static NativeIntegerFacets CompileDerived(XElement restriction, NativeIntegerFacets? inherited, NativeNumericLimits limits)
    {
        NativeIntegerFacets own;
        try { own = Compile(restriction, limits); }
        catch (NativeProfileException error) when (error.Outcome == NativeOutcome.Unsupported &&
            error.UnsupportedReason is NativeDecimalUnsupportedReason.ValueType or NativeDecimalUnsupportedReason.DeclarationShape)
        { throw new NativeProfileException(NativeOutcome.Invalid, "Invalid bounded integer restriction facets."); }
        static void Valid(bool condition)
        { if (!condition) throw new NativeProfileException(NativeOutcome.Invalid, "Illegal inherited integer facet restriction."); }
        if (inherited is not null)
        {
            if (own.UpperBound is { } hi)
            {
                if (inherited.UpperBound is { } bhi)
                {
                    var c = Compare(hi, bhi);
                    Valid(own.UpperExclusive || !inherited.UpperExclusive ? c <= 0 : c < 0);
                }
                if (inherited.LowerBound is { } blo)
                {
                    var c = Compare(hi, blo);
                    Valid(own.UpperExclusive || inherited.LowerExclusive ? c > 0 : c >= 0);
                }
            }
            if (own.LowerBound is { } lo)
            {
                if (inherited.LowerBound is { } blo)
                {
                    var c = Compare(lo, blo);
                    Valid(!own.LowerExclusive && inherited.LowerExclusive ? c > 0 : c >= 0);
                }
                if (inherited.UpperBound is { } bhi)
                {
                    var c = Compare(lo, bhi);
                    Valid(own.LowerExclusive || inherited.UpperExclusive ? c < 0 : c <= 0);
                }
            }
            if (own.TotalLimit is { } total && inherited.TotalLimit is { } baseTotal) Valid(total <= baseTotal);
            if (own.FractionLimit is { } fraction && inherited.FractionLimit is { } baseFraction) Valid(fraction <= baseFraction);
        }
        if (own.Enumeration is { } entries && inherited is not null)
            foreach (var value in entries) Valid(inherited.Accepts(value));
        var effectiveTotal = own.TotalLimit ?? inherited?.TotalLimit;
        var effectiveFraction = own.FractionLimit ?? inherited?.FractionLimit;
        Valid(effectiveTotal is null || effectiveFraction is null || effectiveFraction <= effectiveTotal);
        return new(own.LowerBound, own.LowerExclusive, own.UpperBound, own.UpperExclusive,
            own.TotalLimit, inherited, own.Enumeration);
    }

    private static int Compare(BigInteger a, BigInteger b) => a.CompareTo(b);

    internal static NativeIntegerFacets Compile(XElement restriction, NativeNumericLimits limits)
    {
        BigInteger? lower=null,upper=null;
        bool lowerExclusive=false,upperExclusive=false;
        int? total=null;
        var entries=ImmutableHashSet.CreateBuilder<BigInteger>();
        var sourceEntries=0;
        var seen=new HashSet<string>(StringComparer.Ordinal);
        foreach(var facet in NativeDecimalSchema.Elements(restriction))
        {
            NativeDecimalSchema.Require(facet.Name.NamespaceName=="http://www.w3.org/2001/XMLSchema" &&
                facet.Name.LocalName is "minInclusive" or "minExclusive" or "maxInclusive" or "maxExclusive" or "totalDigits" or "fractionDigits" or "enumeration",
                "Unsupported integer facet.",NativeDecimalUnsupportedReason.ValueType);
            var kind=facet.Name.LocalName;
            NativeDecimalSchema.Require(kind=="enumeration" || seen.Add(kind),"Duplicate facet.",NativeDecimalUnsupportedReason.DeclarationShape);
            NativeDecimalSchema.Attributes(facet,"value");
            NativeDecimalSchema.Require(NativeDecimalSchema.Elements(facet).Count==0,"Facet children are unsupported.",NativeDecimalUnsupportedReason.DeclarationShape);
            var supplied=(string?)facet.Attribute("value");
            if(kind!="enumeration") NativeDecimalSchema.Require(supplied is not null,"Facet value required.",NativeDecimalUnsupportedReason.ValueType);
            if (kind=="enumeration")
            {
                NativeDecimalSchema.Require(++sourceEntries<=16,"Integer enumeration source capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
                if (supplied is null) throw new NativeProfileException(NativeOutcome.Invalid,"Enumeration value is required.");
                entries.Add(ParseValue(supplied!,limits,NativeOutcome.Invalid));
                continue;
            }
            if(kind is "totalDigits" or "fractionDigits")
            {
                var value=ParseValue(supplied!,new(256,0),NativeOutcome.Unsupported);
                if(kind=="fractionDigits")
                    NativeDecimalSchema.Require(value.IsZero,"Integer fractionDigits is fixed at zero.",NativeDecimalUnsupportedReason.ValueType);
                else
                {
                    NativeDecimalSchema.Require(value>0,"Invalid totalDigits value.",NativeDecimalUnsupportedReason.ValueType);
                    NativeDecimalSchema.Require(value<=256,"Integer facet capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
                    total=(int)value;
                }
            }
            else
            {
                var value=ParseValue(supplied!,limits,NativeOutcome.Unsupported);
                if(kind.StartsWith("min",StringComparison.Ordinal))
                {
                    NativeDecimalSchema.Require(lower is null,"Conflicting lower facets.",NativeDecimalUnsupportedReason.ValueType);
                    lower=value;lowerExclusive=kind=="minExclusive";
                }
                else
                {
                    NativeDecimalSchema.Require(upper is null,"Conflicting upper facets.",NativeDecimalUnsupportedReason.ValueType);
                    upper=value;upperExclusive=kind=="maxExclusive";
                }
            }
        }
        if(lower is {} lo && upper is {} hi)
        {
            var compare=lo.CompareTo(hi);
            NativeDecimalSchema.Require(compare<0 || compare==0 && lowerExclusive==upperExclusive,
                "Inconsistent integer range facets.",NativeDecimalUnsupportedReason.ValueType);
        }
        return new(lower,lowerExclusive,upper,upperExclusive,total,enumeration:sourceEntries==0?null:entries.ToImmutable());
    }

    internal bool Accepts(BigInteger value)
    {
        if (parent is not null && !parent.Accepts(value)) return false;
        if (enumeration is not null && !enumeration.Contains(value)) return false;
        if(lower is {} lo && (value<lo || lowerExclusive && value==lo))return false;
        if(upper is {} hi && (value>hi || upperExclusive && value==hi))return false;
        return totalDigits is not {} digits || BigInteger.Abs(value).ToString(CultureInfo.InvariantCulture).Length<=digits;
    }

    internal static BigInteger ParseValue(string text, NativeNumericLimits limits, NativeOutcome invalidOutcome)
    {
        NativeDecimalSchema.Require(text.Length<=256,"Integer lexical capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
        var lexical=string.Join(" ",text.Split([' ','\t','\r','\n'],StringSplitOptions.RemoveEmptyEntries));
        var start=lexical.Length>0 && lexical[0] is '+' or '-'?1:0;
        if(lexical.Length<=start || !lexical.Skip(start).All(c=>c is >= '0' and <= '9'))
            throw new NativeProfileException(invalidOutcome,"Invalid integer lexical form.",invalidOutcome==NativeOutcome.Unsupported?NativeDecimalUnsupportedReason.ValueType:NativeDecimalUnsupportedReason.Unspecified);
        NativeDecimalSchema.Require(lexical.Length-start<=limits.MaxDigits,"Integer digit capacity exceeded before normalization.",NativeDecimalUnsupportedReason.RepresentationCapacity);
        return BigInteger.Parse(lexical,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture);
    }
}
