// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Compiles finite floating restrictions with exact family-specific comparisons.
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
//                      | Charles Korthout | 0.2   | 26-11-2026     | Correct equal exclusive endpoints as an admitted empty value space |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed class NativeFloatingFacets
{
    private NativeFloatingFacets? parent;
    private readonly List<NativeFloatingValue> entries=[];
    private NativeFloatingValue? lower,upper;
    private bool lowerExclusive,upperExclusive;
    private readonly Dictionary<string,NativeFloatingValue> fixedBounds=new(StringComparer.Ordinal);
    private ImmutableArray<NativePattern> patterns=[];
    private bool HasPatterns=>!patterns.IsEmpty||parent?.HasPatterns==true;
    internal static NativeFloatingFacets Compile(XElement restriction,NativeFloatingKind kind,int maxCharacters,NativePatternCompilation compilation,NativeFloatingFacets? parent)
    {
        var result=new NativeFloatingFacets{parent=parent};var seen=new HashSet<string>(StringComparer.Ordinal);var patterns=ImmutableArray.CreateBuilder<NativePattern>();
        foreach(var facet in NativeDecimalSchema.Elements(restriction))
        {
            compilation.Check();var name=facet.Name.LocalName;
            NativeDecimalSchema.Require(facet.Name.NamespaceName=="http://www.w3.org/2001/XMLSchema"&&name is "enumeration" or "pattern" or "whiteSpace" or "minInclusive" or "minExclusive" or "maxInclusive" or "maxExclusive","Excluded floating facet.",NativeDecimalUnsupportedReason.ValueType);
            NativeDecimalSchema.Attributes(facet,"value","fixed");NativeDecimalSchema.Require(NativeDecimalSchema.Elements(facet).Count==0,"Facet children excluded.",NativeDecimalUnsupportedReason.DeclarationShape);
            if(name is not ("enumeration" or "pattern")&&!seen.Add(name))Invalid("Duplicate floating facet.");
            var text=(string?)facet.Attribute("value");if(text is null)Invalid("Facet value required.");
            NativeDecimalSchema.Require(text!.Length<=maxCharacters,"Floating facet literal capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
            var isFixed=false;
            if(facet.Attribute("fixed") is {} fixedFlag)
            {
                if(name is "enumeration" or "pattern")Invalid("Fixed flag inapplicable to pattern/enumeration.");
                var flag=NativeCalendarParser.Collapse(fixedFlag.Value);if(flag is not ("true" or "false" or "1" or "0"))Invalid("Invalid fixed flag.");isFixed=flag is "true" or "1";
            }
            if(name=="pattern") { NativePattern.Capacity(patterns.Count<8);patterns.Add(NativePattern.Compile(text,compilation));continue; }
            text=NativeCalendarParser.Collapse(text);
            if(name=="whiteSpace") { if(text!="collapse")Invalid("Floating whitespace is fixed to collapse.");continue; }
            var value=NativeFloatingValue.Parse(facet.Attribute("value")!.Value,kind,maxCharacters);
            if(name=="enumeration")
            {
                NativeDecimalSchema.Require(result.entries.Count<16,"Floating enumeration capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
                NativeDecimalSchema.Require(parent?.HasPatterns!=true,"Enumeration over patterned floating ancestors awaits qualification.",NativeDecimalUnsupportedReason.ValueType);
                if(parent is not null&&!parent.ValuesAccept(value))Invalid("Enumeration outside base value space.");result.entries.Add(value);continue;
            }
            NativeDecimalSchema.Require(!double.IsNaN(value.Number),"NaN bound endpoints await qualification.",NativeDecimalUnsupportedReason.ValueType);
            NativeDecimalSchema.Require(parent?.HasPatterns!=true,"Bounds over patterned floating ancestors await qualification.",NativeDecimalUnsupportedReason.ValueType);
            if(parent is not null&&!parent.ValuesAccept(value,name))Invalid("Floating bound outside base value space.");
            for(var ancestor=parent;ancestor is not null;ancestor=ancestor.parent)
            {
                if(name=="minExclusive"&&ancestor.upper is {} inheritedUpper&&value.Number>=inheritedUpper.Number)
                    Invalid("Exclusive minimum must precede inherited maximum.");
                if(name=="maxExclusive"&&ancestor.lower is {} inheritedLower&&value.Number<=inheritedLower.Number)
                    Invalid("Exclusive maximum must follow inherited minimum.");
            }
            for(var ancestor=parent;ancestor is not null;ancestor=ancestor.parent)if(ancestor.fixedBounds.TryGetValue(name,out var required)&&!value.SameSchemaValue(required))Invalid("Fixed floating bound changed.");
            if(isFixed)result.fixedBounds.Add(name,value);
            if(name.StartsWith("min",StringComparison.Ordinal)){result.lower=value;result.lowerExclusive=name=="minExclusive";}
            else{result.upper=value;result.upperExclusive=name=="maxExclusive";}
        }
        if(seen.Contains("minInclusive")&&seen.Contains("minExclusive")||seen.Contains("maxInclusive")&&seen.Contains("maxExclusive"))Invalid("Conflicting floating bound kinds.");
        if(result.lower is {} lo&&result.upper is {} hi&&(lo.Number>hi.Number||lo.Number==hi.Number&&result.lowerExclusive!=result.upperExclusive))Invalid("Inconsistent floating endpoints.");
        result.patterns=patterns.ToImmutable();return result;
    }
    internal bool Accepts(NativeFloatingValue value,string lexical,NativePatternBudget budget)=>ValuesAccept(value)&&PatternsAccept(lexical,budget);
    private bool PatternsAccept(string lexical,NativePatternBudget budget)=>(parent?.PatternsAccept(lexical,budget)??true)&&(patterns.IsEmpty||patterns.Any(p=>p.Match(lexical,budget)));
    private bool ValuesAccept(NativeFloatingValue value,string? endpointException=null)
    {
        if(parent is not null&&!parent.ValuesAccept(value,endpointException))return false;
        if(entries.Count!=0&&!entries.Any(value.SameSchemaValue))return false;
        if((lower is not null||upper is not null)&&double.IsNaN(value.Number))return false;
        if(lower is {} lo&&(value.Number<lo.Number||value.Number==lo.Number&&lowerExclusive&&endpointException!="minExclusive"))return false;
        if(upper is {} hi&&(value.Number>hi.Number||value.Number==hi.Number&&upperExclusive&&endpointException!="maxExclusive"))return false;
        return true;
    }
    private static void Invalid(string text)=>throw new NativeProfileException(NativeOutcome.Invalid,text);
}
