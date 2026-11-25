// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Resolves bounded attribute groups by actual component-preserving set union.
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
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed class NativeAttributeGroups(ImmutableDictionary<XName,XElement> definitions,
 Func<XElement,ImmutableArray<NativeDeclaration>> compile,CancellationToken token)
{
 private readonly Dictionary<XName,ImmutableArray<NativeDeclaration>> resolved=[];
 private readonly HashSet<XName> visiting=[];
 internal static NativeAttributeGroups? For(XElement source)=>source.AncestorsAndSelf()
  .Select(e=>e.Annotation<NativeAttributeGroups>()).FirstOrDefault(c=>c is not null);
 internal ImmutableArray<NativeDeclaration> Resolve(XName name)
 {
  token.ThrowIfCancellationRequested();if(resolved.TryGetValue(name,out var uses))return uses;
  NativeDecimalSchema.Require(definitions.TryGetValue(name,out var definition),"Unresolved closed attribute group.",NativeDecimalUnsupportedReason.Attributes);
  NativeDecimalSchema.Require(visiting.Add(name),"Circular attribute groups require fixed-point support outside this profile.",NativeDecimalUnsupportedReason.DeclarationShape);
  var result=compile(definition!);visiting.Remove(name);resolved.Add(name,result);return result;
 }
}

internal sealed partial class NativeDecimalSchema
{
 private static bool IsAttributeUseMember(XElement element)=>element.Name==Xs+"attribute" || element.Name==Xs+"attributeGroup";
 private static void CompileAttributeGroups(XElement schema,string targetNamespace,string attributeForm,
  NativeType decimalType,NativeType? stringType,NativeType? booleanType,NativeType? integerType,
  Dictionary<XName,NativeType> simple,CancellationToken token)
 {
  var definitions=Elements(schema).Where(e=>e.Name==Xs+"attributeGroup").ToArray();
  Require(definitions.Length<=16,"Attribute group definition capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
  var map=ImmutableDictionary.CreateBuilder<XName,XElement>();
  foreach(var definition in definitions)
  {
   token.ThrowIfCancellationRequested();Attributes(definition,"name");var name=Name(definition,NativeDecimalUnsupportedReason.DeclarationShape);
   Require(name.Length<=256,"Attribute group name capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
   var key=XName.Get(name,targetNamespace);Check(!map.ContainsKey(key),"Duplicate global attribute group.");map.Add(key,definition);
  }
  var context=new NativeAttributeGroups(map.ToImmutable(),definition=>CompileAttributes(Elements(definition),targetNamespace,
   attributeForm,decimalType,stringType,booleanType,integerType,simple,token),token);
  schema.AddAnnotation(context);foreach(var key in map.Keys)context.Resolve(key);
 }
}
