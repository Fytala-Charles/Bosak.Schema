// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Assembles bounded same-namespace includes from an explicit closed text catalog.
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
using System.Text;
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
 internal static NativeResult<string> AssembleIncludes(string rootDocumentUri,IReadOnlyDictionary<string,string> schemaDocuments,CancellationToken token)
 {
  try
  {
   token.ThrowIfCancellationRequested();Require(schemaDocuments.Count is >=1 and <=8,"Closed schema catalog capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
   Uri Identifier(string text)
   {
    Require(text.Length<=1024 && Uri.TryCreate(text,UriKind.Absolute,out var candidate) && candidate.Fragment.Length==0,"Absolute fragment-free schema identifier required.",NativeDecimalUnsupportedReason.DeclarationShape);
    return new Uri(text,UriKind.Absolute);
   }
   var rootId=Identifier(rootDocumentUri).AbsoluteUri;var catalog=new Dictionary<string,string>(StringComparer.Ordinal);var total=0;
   foreach(var pair in schemaDocuments)
   {
    token.ThrowIfCancellationRequested();ArgumentNullException.ThrowIfNull(pair.Key);ArgumentNullException.ThrowIfNull(pair.Value);var id=Identifier(pair.Key).AbsoluteUri;
    Require(pair.Value.Length<=32768,"Schema document character ceiling exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);var bytes=Utf8.GetByteCount(pair.Value);Require(bytes<=32768,"Schema document byte ceiling exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);total+=bytes;Require(total<=65536 && catalog.Count<8,"Closed schema catalog byte/count ceiling exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);Require(catalog.TryAdd(id,pair.Value),"Duplicate canonical schema document identifier.",NativeDecimalUnsupportedReason.DeclarationShape);
   }
   var seen=new HashSet<string>(StringComparer.Ordinal);XElement? aggregate=null;string? target=null;string? elementForm=null;string? attributeForm=null;string? assertionNamespace=null;
   void Visit(string id)
   {
    token.ThrowIfCancellationRequested();if(!seen.Add(id))return;Require(catalog.TryGetValue(id,out var text),"Schema document is absent from the closed catalog.",NativeDecimalUnsupportedReason.DeclarationShape);
    var source=Parse(text!,32768,token).Root!;Require(source.Name==Xs+"schema","Schema root namespace is unsupported.",NativeDecimalUnsupportedReason.DeclarationShape);Attributes(source,"version","xpathDefaultNamespace","targetNamespace","elementFormDefault","attributeFormDefault");Require(source.Attribute("version") is null || (string?)source.Attribute("version")=="1.1","Only schema version1.1 is admitted.",NativeDecimalUnsupportedReason.DeclarationShape);
    var ns=TargetNamespace(source,true);var ef=Form(source,"elementFormDefault","unqualified");var af=Form(source,"attributeFormDefault","unqualified");var ax=AssertionNamespace(source,ns,"");
    if(aggregate is null){aggregate=new XElement(source.Name,source.Attributes());target=ns;elementForm=ef;attributeForm=af;assertionNamespace=ax;}
    else
    {
     Require(ns.Length!=0 || target!.Length==0,"Chameleon include conversion is outside this profile.",NativeDecimalUnsupportedReason.DeclarationShape);Check(ns==target,"Included targetNamespace differs from the including schema.");Require(ef==elementForm && af==attributeForm && ax==assertionNamespace,"Per-source form/assertion contexts differ.",NativeDecimalUnsupportedReason.DeclarationShape);
    }
    var members=Elements(source);var includes=new List<string>();
    foreach(var member in members)
    {
     token.ThrowIfCancellationRequested();if(member.Name==Xs+"include")
     {
      Attributes(member,"schemaLocation");Check(member.Attribute("schemaLocation") is not null,"Include requires schemaLocation.");Require(Elements(member).Count==0,"Include metadata is outside this profile.",NativeDecimalUnsupportedReason.DeclarationShape);var location=Collapse((string)member.Attribute("schemaLocation")!);Require(location.Length is >0 and <=1024 && Uri.TryCreate(new Uri(id,UriKind.Absolute),location,out var resolved) && resolved.IsAbsoluteUri && resolved.Fragment.Length==0,"Include URI is outside the closed catalog profile.",NativeDecimalUnsupportedReason.DeclarationShape);includes.Add(Identifier(new Uri(new Uri(id),location).AbsoluteUri).AbsoluteUri);continue;
     }
     var component=new XElement(member);foreach(var binding in source.Attributes().Where(a=>a.IsNamespaceDeclaration))if(component.Attribute(binding.Name) is null)component.Add(new XAttribute(binding));aggregate!.Add(component);
    }
    foreach(var include in includes)Visit(include);
   }
   Visit(rootId);token.ThrowIfCancellationRequested();var assembled=aggregate!.ToString(SaveOptions.DisableFormatting);Require(assembled.Length<=32768 && Utf8.GetByteCount(assembled)<=32768,"Assembled schema ceiling exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);return NativeResult<string>.Valid(assembled);
  }
  catch(NativeProfileException error){return NativeResult<string>.Failure(error.Outcome,error.Message,error.UnsupportedReason);}
  catch(EncoderFallbackException){return NativeResult<string>.Failure(NativeOutcome.Invalid,"Invalid Unicode schema catalog text.");}
 }
}
