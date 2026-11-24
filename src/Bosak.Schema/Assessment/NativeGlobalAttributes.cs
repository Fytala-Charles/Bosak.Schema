// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Compiles closed global attributes with private per-schema reference context.
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
using System.Collections.Immutable;
using System.Xml;
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed class NativeAttributeDeclarations(ImmutableDictionary<XName, NativeDeclaration> declarations)
{
 internal ImmutableDictionary<XName, NativeDeclaration> Declarations { get; } = declarations;
 internal ImmutableArray<NativeDeclaration> Values => Declarations.Values.ToImmutableArray();
 internal static NativeAttributeDeclarations? For(XElement source) => source.AncestorsAndSelf()
  .Select(e => e.Annotation<NativeAttributeDeclarations>()).FirstOrDefault(c => c is not null);
}

internal sealed partial class NativeDecimalSchema
{
 private static NativeAttributeDeclarations CompileGlobalAttributeDeclarations(XElement schema, string targetNamespace,
  NativeType decimalType, NativeType? stringType, NativeType? booleanType, NativeType? integerType,
  Dictionary<XName, NativeType> simple, CancellationToken token)
 {
  var definitions=Elements(schema).Where(e=>e.Name==Xs+"attribute").ToArray();
  Require(definitions.Length<=16,"Global attribute declaration capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
  var declarations=ImmutableDictionary.CreateBuilder<XName,NativeDeclaration>();
  foreach(var definition in definitions)
  {
   token.ThrowIfCancellationRequested();Attributes(definition,"name","type");
   Require(Elements(definition).Count==0,"Global attribute children are outside the profile.",NativeDecimalUnsupportedReason.DeclarationShape);
   var name=Name(definition,NativeDecimalUnsupportedReason.DeclarationShape);
   Require(name.Length<=256,"Global attribute name capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
   Require(name!="xmlns" && targetNamespace is not "http://www.w3.org/2000/xmlns/" and not "http://www.w3.org/2001/XMLSchema-instance" and not "http://www.w3.org/XML/1998/namespace",
    "Reserved global attribute declaration.",NativeDecimalUnsupportedReason.Attributes);
   var key=XName.Get(name,targetNamespace);Check(!declarations.ContainsKey(key),"Duplicate global attribute declaration.");
   var type=ResolveScalarType(definition,decimalType,stringType,booleanType,integerType,simple);
   declarations.Add(key,new NativeDeclaration(name,type,0,1,targetNamespace,isAttribute:true));
  }
  var context=new NativeAttributeDeclarations(declarations.ToImmutable());schema.AddAnnotation(context);return context;
 }
 private static XName AttributeReferenceName(XElement reference)
 {
  var raw=(string?)reference.Attribute("ref") ?? "";
  Require(raw.Length<=256,"Attribute reference QName capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
  var text=Collapse(raw);var parts=text.Split(':');Check(parts.Length is 1 or 2 && parts.All(p=>p.Length!=0),"Invalid attribute reference QName.");
  try{foreach(var part in parts)XmlConvert.VerifyNCName(part);}
  catch(Exception e) when(e is XmlException or ArgumentException){throw new NativeProfileException(NativeOutcome.Invalid,"Invalid attribute reference QName.");}
  var ns=parts.Length==1?reference.GetDefaultNamespace().NamespaceName:reference.GetNamespaceOfPrefix(parts[0])?.NamespaceName;
  Check(ns is not null,"Unbound attribute reference prefix.");return XName.Get(parts[^1],ns!);
 }
}
