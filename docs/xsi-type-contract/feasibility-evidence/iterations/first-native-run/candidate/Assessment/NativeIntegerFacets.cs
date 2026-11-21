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
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Globalization;
using System.Numerics;
using System.Xml.Linq;

namespace Bosak.Schema;

/// <summary>Compiles and enforces bounded exact integer restriction facets.</summary>
internal sealed class NativeIntegerFacets(BigInteger? lower, bool lowerExclusive, BigInteger? upper, bool upperExclusive, int? totalDigits)
{
    internal static NativeIntegerFacets Compile(XElement restriction, NativeNumericLimits limits)
    {
        BigInteger? lower=null,upper=null;
        bool lowerExclusive=false,upperExclusive=false;
        int? total=null;
        var seen=new HashSet<string>(StringComparer.Ordinal);
        foreach(var facet in NativeDecimalSchema.Elements(restriction))
        {
            NativeDecimalSchema.Require(facet.Name.NamespaceName=="http://www.w3.org/2001/XMLSchema" &&
                facet.Name.LocalName is "minInclusive" or "minExclusive" or "maxInclusive" or "maxExclusive" or "totalDigits" or "fractionDigits",
                "Unsupported integer facet.",NativeDecimalUnsupportedReason.ValueType);
            var kind=facet.Name.LocalName;
            NativeDecimalSchema.Require(seen.Add(kind),"Duplicate facet.",NativeDecimalUnsupportedReason.DeclarationShape);
            NativeDecimalSchema.Attributes(facet,"value");
            NativeDecimalSchema.Require(NativeDecimalSchema.Elements(facet).Count==0,"Facet children are unsupported.",NativeDecimalUnsupportedReason.DeclarationShape);
            var supplied=(string?)facet.Attribute("value");
            NativeDecimalSchema.Require(supplied is not null,"Facet value required.",NativeDecimalUnsupportedReason.ValueType);
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
        return new(lower,lowerExclusive,upper,upperExclusive,total);
    }

    internal bool Accepts(BigInteger value)
    {
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
