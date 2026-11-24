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
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Numerics;
using System.Xml.Linq;
using Bosak.XPath.Core.Xdm;
namespace Bosak.Schema;
internal sealed class NativeDateFacets(HashSet<(bool Zoned, BigInteger Minutes)> entries)
{
 internal static NativeDateFacets? Compile(XElement restriction,int maxCharacters)
 {
  var facets=NativeDecimalSchema.Elements(restriction).ToArray();
  foreach(var f in facets)
  {
   NativeDecimalSchema.Require(f.Name==XName.Get("enumeration","http://www.w3.org/2001/XMLSchema"),"Unsupported date facet.",NativeDecimalUnsupportedReason.ValueType);
   NativeDecimalSchema.Attributes(f,"value");
   NativeDecimalSchema.Require(NativeDecimalSchema.Elements(f).Count == 0,"Date facet children unsupported.",NativeDecimalUnsupportedReason.DeclarationShape);
  }
  NativeDecimalSchema.Require(facets.Length<=16,"Date enumeration source capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
  if(facets.Length==0)return null;
  var entries=new HashSet<(bool,BigInteger)>();
  foreach(var f in facets)
  {
   var text=(string?)f.Attribute("value");
   if(text is null)throw new NativeProfileException(NativeOutcome.Invalid,"Enumeration value required.");
   NativeDecimalSchema.Require(text.Length<=maxCharacters,"Date facet character capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
   entries.Add(Key(NativeDateParser.Parse(text)));
  }
  return new(entries);
 }
 internal bool Accepts(XPathDateTime value)=>entries.Contains(Key(value));
 internal static (bool Zoned,BigInteger Minutes) Key(XPathDateTime value)
 {
  // March-based Gregorian ordinal. Floor division is essential for negative years.
  BigInteger year=value.Year;int month=value.Month;
  if(month<=2)year--;
  var era=BigInteger.DivRem(year,400,out var remainder);if(remainder.Sign<0)era--;
  var yoe=year-era*400;
  int shifted=month+(month>2?-3:9);
  var days=era*146097+yoe*365+yoe/4-yoe/100+(153*shifted+2)/5+value.Day-1;
  return (value.HasTimezone,days*1440-(value.HasTimezone?value.TimezoneOffsetMinutes:0));
 }
}
