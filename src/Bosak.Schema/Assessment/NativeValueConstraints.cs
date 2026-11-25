// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Compiles scalar element value constraints and compares provided original values.
// SPECIAL NOTES        : Private compilation context; no ambient state or public authority factories.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 25-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 25-11-2026     | Admit bounded exact native calendar values and original typed transport |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Numerics;
using System.Xml.Linq;
using Bosak.XPath.Core.Xdm;
namespace Bosak.Schema;

internal sealed record NativeValueConstraint(bool IsFixed,XdmExactDecimal? Decimal,string? Text,bool? Boolean,BigInteger? Integer,XPathDateTime? Date,XdmExactCalendarValue? Calendar);
internal sealed class NativeValueConstraintCompilation(NativeNumericLimits limits,int maxStringCharacters,CancellationToken token)
{
 internal NativeNumericLimits Limits=>limits;
 internal int MaxStringCharacters=>maxStringCharacters;
 internal CancellationToken Token=>token;
 internal NativePatternBudget PatternBudget {get;}=new(token);
 internal static NativeValueConstraintCompilation? For(XElement source)=>source.AncestorsAndSelf().Select(e=>e.Annotation<NativeValueConstraintCompilation>()).FirstOrDefault(x=>x is not null);
}

internal sealed partial class NativeDecimalSchema
{
 private static bool HasValueConstraint(XElement source)=>source.Attribute("default") is not null || source.Attribute("fixed") is not null;
 private static NativeValueConstraint? CompileValueConstraint(XElement source,NativeType type)
 {
  var fixedValue=source.Attribute("fixed");var defaultValue=source.Attribute("default");Check(fixedValue is null || defaultValue is null,"Default and fixed are mutually exclusive.");if(fixedValue is null && defaultValue is null)return null;
  Require(!type.IsComplex,"Complex value constraints are outside the provided-content profile.",NativeDecimalUnsupportedReason.DeclarationShape);var text=(fixedValue??defaultValue)!.Value;Require(text.Length<=256,"Value constraint literal capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
  var context=NativeValueConstraintCompilation.For(source);Require(context is not null,"Value constraint compile context unavailable.",NativeDecimalUnsupportedReason.DeclarationShape);context!.Token.ThrowIfCancellationRequested();
  // This scratch declaration is only a pure literal parser input, never registered or issued as a component.
  var scratch=new NativeDeclaration("literal",type,1,1);var parser=new NativeDecimalSchema(scratch,[],[],context.Limits){maxStringCharacters=context.MaxStringCharacters};var value=parser.AssessScalar(text,scratch,context.PatternBudget);
  return new(fixedValue is not null,value.TypedValue,value.StringValue,value.BooleanValue,value.IntegerValue,value.DateValue,value.CalendarValue);
 }
 private static void RequireProvidedElementValue(XElement element,NativeDeclaration declaration)
 {
  if(declaration.CanonicalDeclaration.ValueConstraint is null)return;
  Require(element.Nodes().Any(n=>n is XText or XElement),"Default/fixed application requires an augmented-node provenance contract.",NativeDecimalUnsupportedReason.DeclarationShape);
 }
 private static bool ConstraintEquals(NativeValueConstraint expected,NativeElementState actual)
 {
  if(expected.Decimal is { } a && actual.TypedValue is { } b)return a.Coefficient==b.Coefficient && a.Scale==b.Scale;
  if(expected.Text is { } x && actual.StringValue is { } y)return string.Equals(x,y,StringComparison.Ordinal);
  if(expected.Boolean is { } logical && actual.BooleanValue is { } otherLogical)return logical==otherLogical;
  if(expected.Integer is { } integer && actual.IntegerValue is { } otherInteger)return integer==otherInteger;
  if(expected.Calendar is { } calendar && actual.CalendarValue is { } otherCalendar)return XdmExactCalendarComparison.SameKey(calendar,otherCalendar);
  if(expected.Date is { } date && actual.DateValue is { } otherDate)return NativeDateFacets.Key(date)==NativeDateFacets.Key(otherDate);
  return false;
 }
 private static void CheckFixedValue(NativeDeclaration declaration,NativeElementState actual)
 {
  if(declaration.CanonicalDeclaration.ValueConstraint is { IsFixed:true } constraint)Check(ConstraintEquals(constraint,actual),"Provided value differs from the fixed value.");
 }
}
