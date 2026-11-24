// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Compiles and enforces bounded decimal value-space facets.
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
//                      | Charles Korthout | 0.2   | 21-11-2026     | Adopt bounded complex restriction with original and atomic component identities |
//                      | Charles Korthout | 0.3   | 24-11-2026     | Admit bounded opt-in decimal enumeration with exact normalized value keys |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Globalization;
using System.Numerics;
using System.Xml.Linq;
using Bosak.XPath.Core.Xdm;

namespace Bosak.Schema;

/// <summary>Compiles and enforces bounded decimal value-space facets.</summary>
internal sealed class NativeDecimalFacets(
    XdmExactDecimal? lower, bool lowerExclusive, XdmExactDecimal? upper, bool upperExclusive,
    int? totalDigits, int? fractionDigits, NativeDecimalFacets? parent = null, ImmutableHashSet<(BigInteger Coefficient, int Scale)>? enumeration = null)
{
    internal bool HasEnumeration => enumeration is not null || parent?.HasEnumeration == true;
    internal XdmExactDecimal? LowerBound => lower ?? parent?.LowerBound;
    internal XdmExactDecimal? UpperBound => upper ?? parent?.UpperBound;
    internal bool LowerExclusive => lower is not null ? lowerExclusive : parent?.LowerExclusive ?? false;
    internal bool UpperExclusive => upper is not null ? upperExclusive : parent?.UpperExclusive ?? false;
    internal int? TotalLimit => totalDigits ?? parent?.TotalLimit;
    internal int? FractionLimit => fractionDigits ?? parent?.FractionLimit;

    internal static NativeDecimalFacets CompileDerived(XElement restriction, NativeDecimalFacets? inherited, NativeNumericLimits limits)
    {
        NativeDecimalFacets own;
        try { own = Compile(restriction, limits); }
        catch (NativeProfileException error) when (error.Outcome == NativeOutcome.Unsupported &&
            error.UnsupportedReason is NativeDecimalUnsupportedReason.ValueType or NativeDecimalUnsupportedReason.DeclarationShape)
        { throw new NativeProfileException(NativeOutcome.Invalid, "Invalid bounded decimal restriction facets."); }
        static void Valid(bool condition)
        { if (!condition) throw new NativeProfileException(NativeOutcome.Invalid, "Illegal inherited decimal facet restriction."); }
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
        var effectiveTotal = own.TotalLimit ?? inherited?.TotalLimit;
        var effectiveFraction = own.FractionLimit ?? inherited?.FractionLimit;
        Valid(effectiveTotal is null || effectiveFraction is null || effectiveFraction <= effectiveTotal);
        return new(own.LowerBound, own.LowerExclusive, own.UpperBound, own.UpperExclusive,
            own.TotalLimit, own.FractionLimit, inherited);
    }

    internal static NativeDecimalFacets Compile(XElement restriction, NativeNumericLimits limits, bool allowEnumeration = false)
    {
        XdmExactDecimal? lower=null, upper=null;
        bool lowerExclusive=false, upperExclusive=false;
        int? total=null, fraction=null;
        var seen=new HashSet<string>(StringComparer.Ordinal);
        var entries = ImmutableHashSet.CreateBuilder<(BigInteger Coefficient, int Scale)>();
        int sourceEntries = 0;
        foreach(var facet in NativeDecimalSchema.Elements(restriction))
        {
            NativeDecimalSchema.Require(facet.Name.NamespaceName=="http://www.w3.org/2001/XMLSchema" &&
                (facet.Name.LocalName is "minInclusive" or "minExclusive" or "maxInclusive" or "maxExclusive" or "totalDigits" or "fractionDigits" || allowEnumeration && facet.Name.LocalName == "enumeration"),
                "Unsupported decimal facet.", NativeDecimalUnsupportedReason.ValueType);
            var kind=facet.Name.LocalName;
            NativeDecimalSchema.Require(kind == "enumeration" || seen.Add(kind), "Duplicate facet.", NativeDecimalUnsupportedReason.DeclarationShape);
            NativeDecimalSchema.Attributes(facet,"value");
            NativeDecimalSchema.Require(NativeDecimalSchema.Elements(facet).Count==0, "Facet children are unsupported.", NativeDecimalUnsupportedReason.DeclarationShape);
            var supplied=(string?)facet.Attribute("value");
            if (kind == "enumeration")
            {
                NativeDecimalSchema.Require(++sourceEntries <= 16, "Decimal enumeration source capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
                if (supplied is null) throw new NativeProfileException(NativeOutcome.Invalid, "Enumeration value required.");
                NativeDecimalSchema.Require(supplied.Length <= 256, "Decimal enumeration lexical capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
                var normalized = string.Join(" ", supplied.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries));
                var member = NativeDecimalSchema.ParseDecimal(normalized, limits, NativeOutcome.Invalid);
                entries.Add((member.Coefficient, member.Scale));
                continue;
            }
            NativeDecimalSchema.Require(supplied is not null, "Facet value required.", NativeDecimalUnsupportedReason.ValueType);
            NativeDecimalSchema.Require(supplied!.Length<=256,"Facet lexical capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
            var lexical=string.Join(" ",supplied.Split([' ','\t','\r','\n'],StringSplitOptions.RemoveEmptyEntries));
            if(kind is "totalDigits" or "fractionDigits")
            {
                var digits=lexical.StartsWith('+') || lexical.StartsWith('-') ? lexical[1..] : lexical;
                NativeDecimalSchema.Require(digits.Length!=0 && digits.All(c=>c is >= '0' and <= '9'),"Invalid integer facet.",NativeDecimalUnsupportedReason.ValueType);
                var value=BigInteger.Parse(lexical,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture);
                NativeDecimalSchema.Require(value>=0 && (kind!="totalDigits" || value>0),"Invalid integer facet value.",NativeDecimalUnsupportedReason.ValueType);
                NativeDecimalSchema.Require(value<=256,"Integer facet capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
                if(kind=="totalDigits") total=(int)value; else fraction=(int)value;
            }
            else
            {
                XdmExactDecimal value;
                try { value=NativeDecimalSchema.ParseDecimal(lexical,limits,NativeOutcome.Unsupported); }
                catch(NativeProfileException ex) when(ex.UnsupportedReason==NativeDecimalUnsupportedReason.Unspecified)
                { throw new NativeProfileException(NativeOutcome.Unsupported,"Invalid decimal facet value.",NativeDecimalUnsupportedReason.ValueType); }
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
        NativeDecimalSchema.Require(total is null || fraction is null || fraction<=total,"Inconsistent digit facets.",NativeDecimalUnsupportedReason.ValueType);
        if(lower is { } lo && upper is { } hi)
        {
            var compare=Compare(lo,hi);
            NativeDecimalSchema.Require(compare<0 || compare==0 && lowerExclusive==upperExclusive,
                "Inconsistent range facets.",NativeDecimalUnsupportedReason.ValueType);
        }
        return new(lower,lowerExclusive,upper,upperExclusive,total,fraction, enumeration: sourceEntries == 0 ? null : entries.ToImmutable());
    }

    internal bool Accepts(XdmExactDecimal value)
    {
        if (parent is not null && !parent.Accepts(value)) return false;
        if (enumeration is not null && !enumeration.Contains((value.Coefficient, value.Scale))) return false;
        if(lower is { } lo) { var c=Compare(value,lo); if(c<0 || c==0 && lowerExclusive) return false; }
        if(upper is { } hi) { var c=Compare(value,hi); if(c>0 || c==0 && upperExclusive) return false; }
        if(fractionDigits is { } f && value.Scale>f) return false;
        var digits=BigInteger.Abs(value.Coefficient).ToString(CultureInfo.InvariantCulture).Length;
        return totalDigits is not { } t || Math.Max(digits,value.Scale)<=t;
    }

    private static int Compare(XdmExactDecimal a,XdmExactDecimal b)
    {
        var scale=Math.Max(a.Scale,b.Scale);
        return (a.Coefficient*BigInteger.Pow(10,scale-a.Scale)).CompareTo(b.Coefficient*BigInteger.Pow(10,scale-b.Scale));
    }
}
