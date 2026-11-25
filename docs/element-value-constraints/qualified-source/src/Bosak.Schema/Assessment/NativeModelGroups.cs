// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Resolves finite named sequence groups without duplicating source declarations.
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

internal sealed partial class NativeDecimalSchema
{
private sealed class NativeModelGroups(ImmutableDictionary<XName,XElement> definitions,CancellationToken token)
{
 private readonly Dictionary<XName,ImmutableArray<XElement>> resolved=[];
 private readonly HashSet<XName> visiting=[];
 internal IEnumerable<XName> Names=>definitions.Keys;
 internal ImmutableArray<XElement> Resolve(XName name)
 {
  token.ThrowIfCancellationRequested();if(resolved.TryGetValue(name,out var particles))return particles;
  NativeDecimalSchema.Require(definitions.TryGetValue(name,out var definition),"Unresolved closed model group.",NativeDecimalUnsupportedReason.DeclarationShape);
  NativeDecimalSchema.Check(visiting.Add(name),"Circular model groups are invalid.");
  var sequence=NativeDecimalSchema.Elements(definition!);NativeDecimalSchema.Require(sequence.Count==1 && sequence[0].Name==NativeDecimalSchema.Xs+"sequence","Only nonempty sequence model groups are admitted.",NativeDecimalUnsupportedReason.DeclarationShape);
  var result=Expand(sequence[0]);visiting.Remove(name);resolved.Add(name,result);return result;
 }
 internal ImmutableArray<XElement> Expand(XElement source)
 {
  token.ThrowIfCancellationRequested();
  if(source.Name==NativeDecimalSchema.Xs+"group")
  {
   NativeDecimalSchema.Check(source.Attribute("name") is null && source.Attribute("ref") is not null,"Group use requires only a reference.");
   NativeDecimalSchema.Attributes(source,"ref","minOccurs","maxOccurs");NativeDecimalSchema.Require(NativeDecimalSchema.Elements(source).Count==0,"Group reference children are unsupported.",NativeDecimalUnsupportedReason.DeclarationShape);
   NativeDecimalSchema.Require(NativeDecimalSchema.Occurs(source,"minOccurs",1)==1 && NativeDecimalSchema.Occurs(source,"maxOccurs",1)==1,"Group occurrence must be exactly one.",NativeDecimalUnsupportedReason.DeclarationShape);
   return Resolve(NativeDecimalSchema.AttributeReferenceName(source));
  }
  NativeDecimalSchema.Require(source.Name==NativeDecimalSchema.Xs+"sequence","Only sequence group expansion is admitted.",NativeDecimalUnsupportedReason.DeclarationShape);
  NativeDecimalSchema.Attributes(source);var members=NativeDecimalSchema.Elements(source);NativeDecimalSchema.Require(members.Count is >=1 and <=16,"Sequence group source ceiling exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
  var result=ImmutableArray.CreateBuilder<XElement>();foreach(var member in members)
  {
   token.ThrowIfCancellationRequested();if(member.Name==NativeDecimalSchema.Xs+"group")result.AddRange(Expand(member));else{NativeDecimalSchema.Require(member.Name==NativeDecimalSchema.Xs+"element","Only element and group sequence members are admitted.",NativeDecimalUnsupportedReason.DeclarationShape);result.Add(member);}
   NativeDecimalSchema.Require(result.Count<=16,"Expanded sequence particle ceiling exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
  }
  return result.ToImmutable();
 }
}

 private static NativeModelGroups CompileModelGroups(XElement schema,string targetNamespace,CancellationToken token)
 {
  var definitions=Elements(schema).Where(e=>e.Name==Xs+"group").ToArray();Require(definitions.Length<=16,"Model group definition capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
  var map=ImmutableDictionary.CreateBuilder<XName,XElement>();foreach(var definition in definitions)
  {
   token.ThrowIfCancellationRequested();Attributes(definition,"name");var name=Name(definition);Require(name.Length<=256,"Model group name capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);var key=XName.Get(name,targetNamespace);Check(!map.ContainsKey(key),"Duplicate model group definition.");map.Add(key,definition);
  }
  var context=new NativeModelGroups(map.ToImmutable(),token);foreach(var name in context.Names)context.Resolve(name);return context;
 }
}
