// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Compiles bounded Boolean lexical pattern and whitespace restrictions.
// SPECIAL NOTES        : Finite native Boolean profile; no Core or public API changes.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 24-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 24-11-2026     | Apply inherited Boolean groups without recompiling parent automata |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed class NativeBooleanFacets(ImmutableArray<NativePattern> patterns, NativeBooleanFacets? parent = null)
{
 internal static NativeBooleanFacets Compile(XElement restriction, NativePatternCompilation compilation) => CompileDerived(restriction, compilation, null);
 internal static NativeBooleanFacets CompileDerived(XElement restriction, NativePatternCompilation compilation, NativeBooleanFacets? parent)
 {
  var facets=NativeDecimalSchema.Elements(restriction);var patterns=ImmutableArray.CreateBuilder<NativePattern>();bool whitespace=false;
  NativePattern.Capacity(facets.Count(e=>e.Name.LocalName=="pattern")<=8);
  foreach(var facet in facets)
  {
   compilation.Check();
   if(facet.Name.NamespaceName=="http://www.w3.org/2001/XMLSchema" && facet.Name.LocalName is "enumeration" or "length" or "minLength" or "maxLength" or "totalDigits" or "fractionDigits" or "minInclusive" or "minExclusive" or "maxInclusive" or "maxExclusive" or "explicitTimezone")Invalid("Facet is not applicable to Boolean.");
   NativeDecimalSchema.Require(facet.Name.NamespaceName=="http://www.w3.org/2001/XMLSchema" && facet.Name.LocalName is "pattern" or "whiteSpace", "Boolean facet is outside the profile.", NativeDecimalUnsupportedReason.ValueType);
   NativeDecimalSchema.Attributes(facet,"value");NativeDecimalSchema.Require(NativeDecimalSchema.Elements(facet).Count==0,"Boolean facet children unsupported.",NativeDecimalUnsupportedReason.DeclarationShape);
   var text=(string?)facet.Attribute("value");if(text is null)Invalid("Boolean facet value required.");
   if(facet.Name.LocalName=="pattern")patterns.Add(NativePattern.Compile(text!,compilation));
   else{if(whitespace)Invalid("Duplicate Boolean whitespace facet.");whitespace=true;var collapsed=string.Join(" ",text!.Split([' ','\t','\r','\n'],StringSplitOptions.RemoveEmptyEntries));if(collapsed!="collapse")Invalid("Boolean whitespace is fixed to collapse.");}
  }
  return new(patterns.ToImmutable(), parent);
 }
 internal bool Accepts(string lexical,NativePatternBudget budget)=>(parent is null || parent.Accepts(lexical,budget)) && (patterns.IsDefaultOrEmpty || patterns.Any(p=>p.Match(lexical,budget)));
 private static void Invalid(string message)=>throw new NativeProfileException(NativeOutcome.Invalid,message);
}
