// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Validates provided attribute constraints while preserving canonical declaration uses.
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
//                      | Charles Korthout | 0.3   | 25-11-2026     | Add bounded binary and namespace-sensitive QName transport |
//                      | Charles Korthout | 0.4   | 25-11-2026     | Transport ordered compound items with original member provenance |
//                      | Charles Korthout | 0.5   | 26-11-2026     | Preserve native URI and IEEE floating-point datatype values |
//                      | Charles Korthout | 0.6   | 26-11-2026     | Preserve bounded exact native duration values and provenance |
//                      | Charles Korthout | 0.7   | 26-11-2026     | Add bounded native Gregorian fragments with original typed provenance |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
 private static void AttributeConstraintRepresentation(XElement source)
 {
  var use=Collapse((string?)source.Attribute("use")??"optional");Check(source.Attribute("default") is null || use=="optional","Explicit default requires optional attribute use.");Check(source.Attribute("fixed") is null || use!="prohibited","Explicit fixed cannot use prohibited.");
 }
 private static NativeValueConstraint? CompileAttributeConstraint(XElement source,NativeType type,NativeDeclaration? global=null)
 {
  AttributeConstraintRepresentation(source);var constraint=CompileValueConstraint(source,type);
  if(global?.ValueConstraint is { IsFixed:true } declared && constraint is not null)Check(constraint.IsFixed && ConstraintIdentical(declared,constraint),"A use-local override of global fixed must be fixed and identical.");return constraint;
 }
 private static bool ConstraintIdentical(NativeValueConstraint expected,NativeValueConstraint other)
 {
  if(expected.Gregorian is { } gregorian && other.Gregorian is { } otherGregorian)return gregorian.IdenticalValue(otherGregorian);
  if(expected.Duration is { } duration && other.Duration is { } otherDuration)return duration.SameValue(otherDuration);
  if(expected.Uri is { } uri && other.Uri is { } otherUri)return string.Equals(uri,otherUri,StringComparison.Ordinal);
  if(expected.Floating is { } floating && other.Floating is { } otherFloating)return floating.SameSchemaValue(otherFloating);
  if(expected.Compound is { } compound && other.Compound is { } otherCompound)return NativeCompoundEquality.Equal(compound,otherCompound);
  if(expected.Interchange is { } interchange && other.Interchange is { } otherInterchange)return interchange.ValueEquals(otherInterchange);
  if(expected.Calendar is { } calendar && other.Calendar is { } otherCalendar)return calendar.Equals(otherCalendar);
  if(expected.Date is { } date && other.Date is { } otherDate)return date.Year==otherDate.Year && date.Month==otherDate.Month && date.Day==otherDate.Day && date.HasTimezone==otherDate.HasTimezone && date.TimezoneOffsetMinutes==otherDate.TimezoneOffsetMinutes;
  if(expected.Decimal is { } a && other.Decimal is { } b)return a.Coefficient==b.Coefficient && a.Scale==b.Scale;
  if(expected.Text is { } x && other.Text is { } y)return string.Equals(x,y,StringComparison.Ordinal);
  if(expected.Boolean is { } logical && other.Boolean is { } otherLogical)return logical==otherLogical;
  if(expected.Integer is { } integer && other.Integer is { } otherInteger)return integer==otherInteger;
  return false;
 }
 private static NativeValueConstraint? EffectiveAttributeConstraint(NativeDeclaration use)=>use.ValueConstraint ?? use.CanonicalDeclaration.ValueConstraint;
 private static void RequireUnconstrainedAttributeAbsence(NativeDeclaration use)=>Require(EffectiveAttributeConstraint(use) is null,"Default/fixed attribute application requires augmented original attribute nodes.",NativeDecimalUnsupportedReason.DeclarationShape);
 private NativeElementState AssessAttributeValue(string text,NativeDeclaration use,NativePatternBudget budget,XElement? scope = null)
 {
  var value=AssessScalar(text,use,budget,scope);if(EffectiveAttributeConstraint(use) is { IsFixed:true } constraint)Check(ConstraintEquals(constraint,value),"Provided attribute differs from its effective fixed value.");return value;
 }
}
