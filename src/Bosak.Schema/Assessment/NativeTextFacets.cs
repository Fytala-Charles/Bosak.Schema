// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Compiles bounded text restriction chains and fixed whitespace facets.
// SPECIAL NOTES        : Part of bounded native datatype validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 26-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2 | 26-11-2026     | Bound identity facet literals without applying document graph checks |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.3 | 27-11-2026     | Preserve anonymous decimal/text bases and original facet source identities.              |
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Globalization;
using System.Numerics;
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed class NativeTextFacets(NativeTextFacets? parent,int whitespace,bool fixedWhitespace,int? length,int? minimum,int? maximum,
    ImmutableHashSet<string>? enumeration,ImmutableArray<NativePattern> patterns,ImmutableDictionary<string,int> fixedLengths)
{
    internal int Whitespace=>whitespace;
    private NativeTextFacets? Parent=>parent;
    private bool FixedWhitespace=>fixedWhitespace;
    private bool HasLexicalConstraints=>enumeration is not null || !patterns.IsEmpty || parent?.HasLexicalConstraints==true;
    private int Minimum=>Math.Max(length??minimum??0,parent?.Minimum??0);
    private int Maximum=>Math.Min(length??maximum??int.MaxValue,parent?.Maximum??int.MaxValue);
    private int? LengthFacet=>length??parent?.LengthFacet;
    private int? MinFacet=>minimum??parent?.MinFacet;
    private int? MaxFacet=>maximum??parent?.MaxFacet;
    private ImmutableDictionary<string,int> FixedLengths=>fixedLengths;
    internal static NativeTextFacets Compile(XElement restriction,NativeType baseType,int capacity,NativePatternCompilation compilation,XElement? inlineBase=null)
    {
        var parent=baseType.TextFacets;var white=baseType.TextWhitespace;var fixedWhite=parent?.FixedWhitespace??false;
        int? length=null,minimum=null,maximum=null;
        var entries=ImmutableHashSet.CreateBuilder<string>(StringComparer.Ordinal);var enumCount=0;
        var patterns=ImmutableArray.CreateBuilder<NativePattern>();var fixedValues=ImmutableDictionary.CreateBuilder<string,int>(StringComparer.Ordinal);
        var seen=new HashSet<string>(StringComparer.Ordinal);var facets=NativeDecimalSchema.Elements(restriction).Where(f=>!ReferenceEquals(f,inlineBase)).ToArray();
        // Whitespace applies to every literal irrespective of facet source order.
        foreach(var facet in facets.Where(f=>f.Name.LocalName=="whiteSpace"))
        {
            compilation.Check();if(!seen.Add("whiteSpace"))Invalid("Duplicate whiteSpace.");
            var supplied=NativeCalendarParser.Collapse((string?)facet.Attribute("value")??"");
            var mode=supplied switch { "preserve"=>0,"replace"=>1,"collapse"=>2,_=>-1 };
            if(mode<white || mode<0 || fixedWhite && mode!=white)Invalid("Whitespace restriction widens or changes a fixed facet.");
            NativeDecimalSchema.Require(mode==white || parent?.HasLexicalConstraints!=true,"Whitespace strengthening over lexical ancestors awaits qualification.",NativeDecimalUnsupportedReason.ValueType);
            white=mode;fixedWhite|=Flag(facet);
        }
        foreach(var facet in facets)
        {
            compilation.Check();var name=facet.Name.LocalName;
            NativeDecimalSchema.Require(facet.Name.NamespaceName=="http://www.w3.org/2001/XMLSchema" && name is "length" or "minLength" or "maxLength" or "enumeration" or "pattern" or "whiteSpace","Excluded text facet.",NativeDecimalUnsupportedReason.ValueType);
            NativeDecimalSchema.Attributes(facet,"value","fixed");NativeDecimalSchema.Require(NativeDecimalSchema.Elements(facet).Count==0,"Facet children excluded.",NativeDecimalUnsupportedReason.DeclarationShape);
            var supplied=(string?)facet.Attribute("value");if(supplied is null)Invalid("Facet value required.");
            NativeDecimalSchema.Require(supplied!.Length<=capacity,"Text facet literal capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
            if(name=="whiteSpace")continue;
            if(name is not ("enumeration" or "pattern")&&!seen.Add(name))Invalid("Duplicate text length facet.");
            var isFixed=Flag(facet);
            if(name is "pattern" or "enumeration" && facet.Attribute("fixed") is not null)Invalid("Fixed is inapplicable to pattern/enumeration.");
            if(name=="pattern") { NativePattern.Capacity(patterns.Count<8);patterns.Add(NativePattern.Compile(supplied,compilation)); }
            else if(name=="enumeration")
            {
                NativeDecimalSchema.Require(++enumCount<=16,"Text enumeration capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
                NativeDecimalSchema.Require(!NativeDecimalSchema.IsIdentityAtomic(baseType) || supplied.Length<=256,"Identity enumeration literal capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
                var value=NativeTextRules.Normalize(supplied,white);
                if(!NativeTextRules.Accepts(value,baseType.TextKind))Invalid("Enumeration outside lexical family.");
                NativeDecimalSchema.Require(parent is null || !parent.HasPatterns,"Enumeration over patterned text ancestors awaits qualification.",NativeDecimalUnsupportedReason.ValueType);
                if(parent is not null&&!parent.Accepts(value,compilation.MatchingBudget))Invalid("Enumeration outside base value space.");
                entries.Add(value);
            }
            else
            {
                NativeDecimalSchema.Require(supplied.Length<=256,"Length lexical capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
                var lexical=NativeCalendarParser.Collapse(supplied);var start=lexical.StartsWith('+')||lexical.StartsWith('-')?1:0;
                if(lexical.Length<=start||lexical.Skip(start).Any(c=>c is <'0' or >'9'))Invalid("Invalid length lexical value.");
                var number=BigInteger.Parse(lexical,CultureInfo.InvariantCulture);if(number<0)Invalid("Negative text length.");
                NativeDecimalSchema.Require(number<=65536,"Length capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);var value=(int)number;
                if(name=="length")length=value;else if(name=="minLength")minimum=value;else maximum=value;
                for(var ancestor=parent;ancestor is not null;ancestor=ancestor.Parent)if(ancestor.FixedLengths.TryGetValue(name,out var required)&&required!=value)Invalid("Fixed length facet changed.");
                if(isFixed)fixedValues.Add(name,value);
            }
        }
        if(minimum>maximum)Invalid("Inconsistent text length interval.");
        if(parent is not null&&(length is {} exact&&(exact<parent.Minimum||exact>parent.Maximum)||minimum is {} lo&&lo<(parent.MinFacet??0)||maximum is {} hi&&hi>(parent.MaxFacet??int.MaxValue)))Invalid("Length facet widens base.");
        var result=new NativeTextFacets(parent,white,fixedWhite,length,minimum,maximum,enumCount==0?null:entries.ToImmutable(),patterns.ToImmutable(),fixedValues.ToImmutable());
        if(result.LengthFacet is {} exactLength)
        {
            bool AncestorBound(int bound,bool lower)
            {
                for(var ancestor=parent;ancestor is not null;ancestor=ancestor.Parent)
                    if(ancestor.LengthFacet is null && (lower?ancestor.MinFacet:ancestor.MaxFacet)==bound)return true;
                return false;
            }
            if(result.MinFacet is {} min && (min>exactLength || !AncestorBound(min,true)))Invalid("Length/minLength combination lacks a qualifying ancestor.");
            if(result.MaxFacet is {} max && (max<exactLength || !AncestorBound(max,false)))Invalid("Length/maxLength combination lacks a qualifying ancestor.");
        }
        if(result.Minimum>result.Maximum)Invalid("Empty text length interval.");return result;
    }
    private bool HasPatterns=>!patterns.IsEmpty||parent?.HasPatterns==true;
    private static bool Flag(XElement facet)
    {
        if(facet.Attribute("fixed") is not {} flag)return false;
        var text=NativeCalendarParser.Collapse(flag.Value);if(text is not ("true" or "false" or "1" or "0"))Invalid("Invalid fixed flag.");return text is "true" or "1";
    }
    internal bool Accepts(string value,NativePatternBudget budget)
    {
        var count=0;foreach(var _ in value.EnumerateRunes()){budget.Tick();count++;}
        return (parent?.Accepts(value,budget)??true)&&(length is null||count==length)&&(minimum is null||count>=minimum)&&(maximum is null||count<=maximum)&&
            (enumeration is null||enumeration.Contains(value))&&(patterns.IsEmpty||patterns.Any(p=>p.Match(value,budget)));
    }
    private static void Invalid(string text)=>throw new NativeProfileException(NativeOutcome.Invalid,text);
}
