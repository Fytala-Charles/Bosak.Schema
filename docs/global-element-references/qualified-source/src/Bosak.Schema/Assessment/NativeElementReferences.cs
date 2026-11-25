// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Compiles closed global element references with canonical declaration identity.
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
using System.Xml;
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed class NativeElementDeclarations(Func<XName,NativeDecimalSchema> resolve)
{
 internal NativeDecimalSchema Resolve(XName name)=>resolve(name);
 internal static NativeElementDeclarations? For(XElement source)=>source.AncestorsAndSelf()
  .Select(e=>e.Annotation<NativeElementDeclarations>()).FirstOrDefault(c=>c is not null);
}

internal sealed partial class NativeDecimalSchema
{
 private static NativeDeclaration? CompileElementReference(XElement source,bool allowComplex)
 {
  if(source.Attribute("ref") is null)return null;
  Check(source.Attribute("name") is null && source.Attribute("type") is null && source.Attribute("form") is null,
   "Element reference cannot redefine name, type or form.");
  Attributes(source,"ref","minOccurs","maxOccurs");Require(Elements(source).Count==0,"Element reference children are unsupported.",NativeDecimalUnsupportedReason.DeclarationShape);
  var raw=(string?)source.Attribute("ref") ?? "";Require(raw.Length<=256,"Element reference QName capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
  var text=Collapse(raw);var parts=text.Split(':');Check(parts.Length is 1 or 2 && parts.All(p=>p.Length!=0),"Invalid element reference QName.");
  try{foreach(var part in parts)XmlConvert.VerifyNCName(part);}catch(Exception e) when(e is XmlException or ArgumentException){throw new NativeProfileException(NativeOutcome.Invalid,"Invalid element reference QName.");}
  var ns=parts.Length==1?source.GetDefaultNamespace().NamespaceName:source.GetNamespaceOfPrefix(parts[0])?.NamespaceName;Check(ns is not null,"Unbound element reference prefix.");
  var context=NativeElementDeclarations.For(source);Require(context is not null,"Element reference context unavailable.",NativeDecimalUnsupportedReason.DeclarationShape);
  var plan=context!.Resolve(XName.Get(parts[^1],ns!));var global=plan.Root;
  Require(!global.Type.IsComplex || allowComplex,"Complex references are outside flat scalar compositors.",NativeDecimalUnsupportedReason.DeclarationShape);
  Require(!global.Type.IsComplex || plan.AllMinimum is null && plan.ChoiceMinimum is null,"Nested all/choice global containers are outside the profile.",NativeDecimalUnsupportedReason.DeclarationShape);
  var min=Occurs(source,"minOccurs",1);var max=Occurs(source,"maxOccurs",1);
  Require(min is 0 or 1 && max is >=1 and <=16 && min<=max,"Element reference occurrence outside the finite profile.",NativeDecimalUnsupportedReason.DeclarationShape);
  return new NativeDeclaration(global.Name,global.Type,min,max,global.NamespaceUri,referencedDeclaration:global)
   {Container=global.Type.IsComplex?plan:null};
 }
}
