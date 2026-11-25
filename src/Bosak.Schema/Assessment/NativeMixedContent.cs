// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Preserves mixed complex governing types and original untypedAtomic text values.
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
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
 private static bool ReadMixedContent(XElement source)
 {
  if(source.Attribute("mixed") is not { } attribute)return false;Require(attribute.Value.Length<=256,"Mixed Boolean source capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);var value=Collapse(attribute.Value);Check(value is "true" or "false" or "1" or "0","Invalid mixed Boolean.");return value is "true" or "1";
 }
 private static NativeType MixedComplexType(XElement source,string? name,string ns)
 {
  var mixed=ReadMixedContent(source);return new NativeType(name,!mixed,name is null?"":ns,isMixed:mixed);
 }
 private NativeElementState ContainerValueState(XElement element,NativeDeclaration declaration,NativeType effectiveType)
 {
  string? text=null;if(effectiveType.MixedContent){text=element.Value;Require(text.Length<=maxStringCharacters!.Value,"Mixed aggregate string capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);}return new NativeElementState(declaration,null,StringValue:text){EffectiveType=effectiveType};
 }
}
