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
//                      | Charles Korthout | 0.2   | 25-11-2026     | Preserve imported named simple-type namespaces in private assembly metadata |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Text;
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed record NativeSchemaAssembly(string Text,ImmutableDictionary<int,string> SimpleTypeNamespaces);
internal sealed record NativeImportedTypeNamespace(string NamespaceUri);

internal sealed partial class NativeDecimalSchema
{
 private static string DefinitionNamespace(XElement source,string fallback)=>source.Annotation<NativeImportedTypeNamespace>()?.NamespaceUri ?? fallback;
 internal static NativeResult<string> AssembleIncludes(string rootDocumentUri,IReadOnlyDictionary<string,string> schemaDocuments,CancellationToken token)
 {
  var result=AssembleCatalogCore(rootDocumentUri,schemaDocuments,token,false);
  return result.Outcome==NativeOutcome.Valid?NativeResult<string>.Valid(result.Value!.Text):NativeResult<string>.Failure(result.Outcome,result.Diagnostic!,result.UnsupportedReason);
 }
 internal static NativeResult<NativeSchemaAssembly> AssembleCatalog(string rootDocumentUri,IReadOnlyDictionary<string,string> schemaDocuments,CancellationToken token)=>AssembleCatalogCore(rootDocumentUri,schemaDocuments,token,true);
 private static NativeResult<NativeSchemaAssembly> AssembleCatalogCore(string rootDocumentUri,IReadOnlyDictionary<string,string> schemaDocuments,CancellationToken token,bool allowImports)
 {
  try
  {
   token.ThrowIfCancellationRequested();Require(schemaDocuments.Count is >=1 and <=8,"Closed schema catalog capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
   Uri Identifier(string text)
   {
    Require(text.Length<=1024 && Uri.TryCreate(text,UriKind.Absolute,out var candidate) && candidate.Fragment.Length==0,"Absolute fragment-free schema identifier required.",NativeDecimalUnsupportedReason.DeclarationShape);return new Uri(text,UriKind.Absolute);
   }
   var rootId=Identifier(rootDocumentUri).AbsoluteUri;var catalog=new Dictionary<string,string>(StringComparer.Ordinal);var total=0;
   foreach(var pair in schemaDocuments)
   {
    token.ThrowIfCancellationRequested();ArgumentNullException.ThrowIfNull(pair.Key);ArgumentNullException.ThrowIfNull(pair.Value);var id=Identifier(pair.Key).AbsoluteUri;Require(pair.Value.Length<=32768,"Schema document character ceiling exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);var bytes=Utf8.GetByteCount(pair.Value);Require(bytes<=32768,"Schema document byte ceiling exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);total+=bytes;Require(total<=65536 && catalog.Count<8,"Closed schema catalog byte/count ceiling exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);Require(catalog.TryAdd(id,pair.Value),"Duplicate canonical schema document identifier.",NativeDecimalUnsupportedReason.DeclarationShape);
   }
   var seen=new HashSet<string>(StringComparer.Ordinal);var parsed=new Dictionary<string,XElement>(StringComparer.Ordinal);var namespaces=ImmutableDictionary.CreateBuilder<int,string>();XElement? aggregate=null;string? target=null;string? elementForm=null;string? attributeForm=null;string? assertionNamespace=null;
   void Visit(string id,string? expectedNamespace,bool simpleOnly,bool importReference)
   {
    token.ThrowIfCancellationRequested();if(!parsed.TryGetValue(id,out var source))
    {
     Require(catalog.TryGetValue(id,out var text),"Schema document is absent from the closed catalog.",NativeDecimalUnsupportedReason.DeclarationShape);source=Parse(text!,32768,token).Root!;Require(source.Name==Xs+"schema","Schema root namespace is unsupported.",NativeDecimalUnsupportedReason.DeclarationShape);Attributes(source,"version","xpathDefaultNamespace","targetNamespace","elementFormDefault","attributeFormDefault");Require(source.Attribute("version") is null || (string?)source.Attribute("version")=="1.1","Only schema version1.1 is admitted.",NativeDecimalUnsupportedReason.DeclarationShape);parsed.Add(id,source);
    }
    var ns=TargetNamespace(source,true);var ef=Form(source,"elementFormDefault","unqualified");var af=Form(source,"attributeFormDefault","unqualified");var ax=AssertionNamespace(source,ns,"");
    if(expectedNamespace is not null){if(!importReference)Require(ns.Length!=0 || expectedNamespace.Length==0,"Chameleon include conversion is outside this profile.",NativeDecimalUnsupportedReason.DeclarationShape);Check(ns==expectedNamespace,"Catalog source targetNamespace differs from its reference.");}
    if(!seen.Add(id))return;
    if(aggregate is null){aggregate=new XElement(source.Name,source.Attributes());target=ns;elementForm=ef;attributeForm=af;assertionNamespace=ax;}
    else if(!simpleOnly)Require(ef==elementForm && af==attributeForm && ax==assertionNamespace,"Per-source form/assertion contexts differ.",NativeDecimalUnsupportedReason.DeclarationShape);
    var members=Elements(source);var references=new List<(string Id,string Namespace,bool SimpleOnly,bool Import)>();
    foreach(var member in members)
    {
     token.ThrowIfCancellationRequested();var include=member.Name==Xs+"include";var import=member.Name==Xs+"import";
     if(include || import && allowImports)
     {
      Attributes(member,include?["schemaLocation"]:["namespace","schemaLocation"]);var expected=ns;
      if(import){Require(member.Attribute("namespace") is not null,"No-namespace imports are outside this profile.",NativeDecimalUnsupportedReason.Namespace);expected=Collapse((string)member.Attribute("namespace")!);Require(expected.Length!=0,"No-namespace imports are outside this profile.",NativeDecimalUnsupportedReason.Namespace);NamespaceUri(expected);Check(expected!=ns,"A schema cannot import its own target namespace.");}
      if(include)Check(member.Attribute("schemaLocation") is not null,"Include requires schemaLocation.");else Require(member.Attribute("schemaLocation") is not null,"Closed import requires a catalog location.",NativeDecimalUnsupportedReason.DeclarationShape);
      Require(Elements(member).Count==0,"Composition metadata is outside this profile.",NativeDecimalUnsupportedReason.DeclarationShape);var location=Collapse((string)member.Attribute("schemaLocation")!);Require(location.Length is >0 and <=1024 && Uri.TryCreate(new Uri(id,UriKind.Absolute),location,out var resolved) && resolved.IsAbsoluteUri && resolved.Fragment.Length==0,"Composition URI is outside the closed catalog profile.",NativeDecimalUnsupportedReason.DeclarationShape);references.Add((Identifier(new Uri(new Uri(id),location).AbsoluteUri).AbsoluteUri,expected,simpleOnly || import,import));continue;
     }
     Require(!simpleOnly || member.Name==Xs+"simpleType","Foreign libraries admit only named simple-type definitions.",NativeDecimalUnsupportedReason.DeclarationShape);
     var component=new XElement(member);foreach(var binding in source.Attributes().Where(a=>a.IsNamespaceDeclaration))if(component.Attribute(binding.Name) is null)component.Add(new XAttribute(binding));if(simpleOnly && ns!=target)namespaces.Add(aggregate!.Elements().Count(),ns);aggregate!.Add(component);
    }
    foreach(var reference in references)Visit(reference.Id,reference.Namespace,reference.SimpleOnly,reference.Import);
   }
   Visit(rootId,null,false,false);token.ThrowIfCancellationRequested();var assembled=aggregate!.ToString(SaveOptions.DisableFormatting);Require(assembled.Length<=32768 && Utf8.GetByteCount(assembled)<=32768,"Assembled schema ceiling exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);return NativeResult<NativeSchemaAssembly>.Valid(new(assembled,namespaces.ToImmutable()));
  }
  catch(NativeProfileException error){return NativeResult<NativeSchemaAssembly>.Failure(error.Outcome,error.Message,error.UnsupportedReason);}
  catch(EncoderFallbackException){return NativeResult<NativeSchemaAssembly>.Failure(NativeOutcome.Invalid,"Invalid Unicode schema catalog text.");}
 }
}
