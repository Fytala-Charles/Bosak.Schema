// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Preserves bounded simple nillability and actual empty typed-value facts.
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
 private static bool NilBoolean(string raw)
 {
  Require(raw.Length<=256,"Nillability Boolean source capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);var value=Collapse(raw);Check(value is "true" or "false" or "1" or "0","Invalid nillability Boolean.");return value is "true" or "1";
 }
 private static bool ReadNillable(XElement source)=>source.Attribute("nillable") is { } flag && NilBoolean(flag.Value);
 private static NativeDeclaration ElementDeclaration(XElement source,string name,NativeType type,int min,int max,string ns)
 {
  var nil=ReadNillable(source);Require(!nil || !type.IsComplex,"Complex nillability is outside the simple-valued profile.",NativeDecimalUnsupportedReason.DeclarationShape);return new NativeDeclaration(name,type,min,max,ns){IsNillable=nil};
 }
 private static NativeElementState? NilledState(XElement element,NativeDeclaration declaration,NativeType selected)
 {
  if(element.Attribute(Xsi+"nil") is not { } request)return null;
  Check(declaration.CanonicalDeclaration.IsNillable,"Any xsi:nil attribute requires a nillable declaration.");if(!NilBoolean(request.Value))return null;
  Require(!selected.IsComplex,"Complex nillability is outside the simple-valued profile.",NativeDecimalUnsupportedReason.DeclarationShape);
  Require(element.Nodes().All(n=>n is XText or XElement),"Nilled comments or processing instructions are outside the profile.",NativeDecimalUnsupportedReason.DeclarationShape);Check(!element.Nodes().Any(),"Nilled elements cannot contain text or element children.");return new NativeElementState(declaration,null){EffectiveType=selected,IsNilled=true};
 }
}
