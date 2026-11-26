// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Compiles finite compound facets and exact native value equality.
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
//                      | Charles Korthout | 0.2   | 26-11-2026     | Support bounded text datatypes and inherited normalization facets |
//                      | Charles Korthout | 0.3   | 26-11-2026     | Preserve native URI and IEEE floating-point datatype values |
//                      | Charles Korthout | 0.4   | 26-11-2026     | Preserve bounded exact native duration values and provenance |
//                      | Charles Korthout | 0.5   | 26-11-2026     | Add bounded native Gregorian fragments with original typed provenance |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Globalization;
using System.Numerics;
using System.Xml.Linq;
using Bosak.XPath.Core.Xdm;

namespace Bosak.Schema;

internal sealed class NativeCompoundFacets(NativeCompoundFacets? parent, int? length, int? minimum, int? maximum,
    ImmutableArray<NativeElementState> enumeration, ImmutableArray<NativePattern> patterns)
{
    private NativeCompoundFacets? Parent => parent;
    private readonly Dictionary<string,int> fixedLengths = new(StringComparer.Ordinal);
    private int Minimum => length ?? minimum ?? parent?.Minimum ?? 0;
    private int Maximum => length ?? maximum ?? parent?.Maximum ?? int.MaxValue;
    private bool HasPatterns => !patterns.IsEmpty || parent?.HasPatterns == true;
    internal static NativeCompoundFacets Compile(XElement restriction, NativeType baseType, NativeNumericLimits limits,
        int maxCharacters, NativePatternCompilation compilation)
    {
        int? length=null, minimum=null, maximum=null;
        var seen=new HashSet<string>(StringComparer.Ordinal);
        var fixedValues=new Dictionary<string,int>(StringComparer.Ordinal);
        var entries=ImmutableArray.CreateBuilder<NativeElementState>();
        var patterns=ImmutableArray.CreateBuilder<NativePattern>();
        foreach(var facet in NativeDecimalSchema.Elements(restriction))
        {
            compilation.Check(); var name=facet.Name.LocalName;
            NativeDecimalSchema.Require(facet.Name.NamespaceName=="http://www.w3.org/2001/XMLSchema" && name is "length" or "minLength" or "maxLength" or "pattern" or "enumeration" or "whiteSpace", "Excluded compound facet.", NativeDecimalUnsupportedReason.ValueType);
            NativeDecimalSchema.Attributes(facet,"value","fixed");
            NativeDecimalSchema.Require(NativeDecimalSchema.Elements(facet).Count==0,"Compound facet children excluded.",NativeDecimalUnsupportedReason.DeclarationShape);
            if(name is not ("pattern" or "enumeration") && !seen.Add(name)) Invalid("Duplicate compound facet.");
            var text=(string?)facet.Attribute("value");if(text is null) Invalid("Compound facet value required.");
            NativeDecimalSchema.Require(text!.Length<=maxCharacters,"Compound facet literal capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
            var isFixed=false;
            if(facet.Attribute("fixed") is { } flag)
            {
                if(name is "pattern" or "enumeration") Invalid("Fixed is inapplicable to this facet.");
                var lexical=NativeCalendarParser.Collapse(flag.Value);if(lexical is not ("true" or "1" or "false" or "0")) Invalid("Invalid fixed Boolean.");isFixed=lexical is "true" or "1";
            }
            if(name=="pattern") { NativePattern.Capacity(patterns.Count<8);patterns.Add(NativePattern.Compile(text,compilation)); }
            else if(name=="enumeration")
            {
                NativeDecimalSchema.Require(entries.Count<16,"Compound enumeration capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
                NativeDecimalSchema.Require(baseType.CompoundFacets?.HasPatterns!=true,"Enumeration over patterned compound ancestors awaits qualification.",NativeDecimalUnsupportedReason.ValueType);
                entries.Add(NativeDecimalSchema.ParseCompoundLiteral(text,facet,baseType,limits,maxCharacters,compilation.MatchingBudget));
            }
            else
            {
                if(baseType.IsUnion) Invalid("Facet is inapplicable to union variety.");
                var lexical=NativeCalendarParser.Collapse(text);
                if(name=="whiteSpace") { if(lexical!="collapse") Invalid("List whitespace is fixed to collapse.");continue; }
                NativeDecimalSchema.Require(lexical.Length<=256,"List length lexical capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
                var offset=lexical.StartsWith('+')||lexical.StartsWith('-')?1:0;
                if(lexical.Length<=offset || lexical.Skip(offset).Any(c=>c is <'0' or >'9')) Invalid("Invalid list length facet.");
                var number=BigInteger.Parse(lexical,CultureInfo.InvariantCulture);if(number<0) Invalid("Negative list length facet.");
                NativeDecimalSchema.Require(number<=65536,"List length facet capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
                var value=(int)number;
                if(name=="length")length=value;else if(name=="minLength")minimum=value;else maximum=value;
                for(var ancestor=baseType.CompoundFacets;ancestor is not null;ancestor=ancestor.Parent)
                    if(ancestor.fixedLengths.TryGetValue(name,out var required)&&required!=value)Invalid("Fixed list length changed.");
                if(isFixed)fixedValues.Add(name,value);
            }
        }
        if(length is not null && (minimum is not null || maximum is not null) || minimum>maximum)Invalid("Inconsistent list length facets.");
        var parent=baseType.CompoundFacets;
        if(parent is not null && (length is { } exact && (exact<parent.Minimum||exact>parent.Maximum) || minimum is { } lo&&lo<parent.Minimum || maximum is { } hi&&hi>parent.Maximum))Invalid("List length widens base.");
        var result=new NativeCompoundFacets(parent,length,minimum,maximum,entries.ToImmutable(),patterns.ToImmutable());
        if(result.Minimum>result.Maximum)Invalid("Empty list length interval.");
        foreach(var pair in fixedValues)result.fixedLengths.Add(pair.Key,pair.Value);
        return result;
    }

    internal bool Accepts(NativeElementState value,string text,NativePatternBudget budget)
    {
        budget.CompoundAttempt();
        var actual=value.ActualAtomicType??value.GoverningType;
        var lexical=value.IsListValue || !actual.IsString ? NativeCalendarParser.Collapse(text):NativeTextRules.Normalize(text, actual.TextWhitespace);
        return (parent?.Accepts(value,text,budget)??true) &&
            (length is null || value.Items.Length==length) && (minimum is null || value.Items.Length>=minimum) &&
            (maximum is null || value.Items.Length<=maximum) &&
            (enumeration.IsEmpty || enumeration.Any(e=>NativeCompoundEquality.Equal(e,value))) &&
            (patterns.IsEmpty || patterns.Any(p=>p.Match(lexical,budget)));
    }
    private static void Invalid(string message)=>throw new NativeProfileException(NativeOutcome.Invalid,message);
}

internal static class NativeCompoundEquality
{
    internal static bool Equal(NativeElementState a,NativeElementState b)
    {
        if(a.IsListValue || b.IsListValue)
        {
            if(!a.IsListValue)return b.Items.Length==1&&Equal(a,b.Items[0]);
            if(!b.IsListValue)return a.Items.Length==1&&Equal(a.Items[0],b);
            return a.Items.Length==b.Items.Length&&a.Items.Zip(b.Items).All(pair=>Equal(pair.First,pair.Second));
        }
        if(a.GregorianValue is { } gregorian && b.GregorianValue is { } otherGregorian)return gregorian.SameValue(otherGregorian);
        if(a.DurationValue is { } duration && b.DurationValue is { } otherDuration)return duration.SameValue(otherDuration);
        if(a.FloatingValue is { } floating && b.FloatingValue is { } otherFloating)return floating.SameSchemaValue(otherFloating);
        if(a.UriValue is { } uri && b.UriValue is { } otherUri)return string.Equals(uri,otherUri,StringComparison.Ordinal);
        if((a.IntegerValue is not null||a.TypedValue is not null)&&(b.IntegerValue is not null||b.TypedValue is not null))
        {
            var ac=a.IntegerValue??a.TypedValue!.Value.Coefficient;var bc=b.IntegerValue??b.TypedValue!.Value.Coefficient;
            var sa=a.TypedValue?.Scale??0;var sb=b.TypedValue?.Scale??0;
            return ac*BigInteger.Pow(10,sb)==bc*BigInteger.Pow(10,sa);
        }
        if(a.StringValue is { } text && b.StringValue is { } otherText)return text==otherText;
        if(a.BooleanValue is { } logical && b.BooleanValue is { } otherLogical)return logical==otherLogical;
        if(a.InterchangeValue is { } interchange && b.InterchangeValue is { } otherInterchange)return interchange.ValueEquals(otherInterchange);
        if(a.CalendarValue is { } calendar && b.CalendarValue is { } otherCalendar)return XdmExactCalendarComparison.SameKey(calendar,otherCalendar);
        if(a.DateValue is { } date && b.DateValue is { } otherDate)return NativeDateFacets.Key(date)==NativeDateFacets.Key(otherDate);
        return false;
    }
}
