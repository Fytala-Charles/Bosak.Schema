// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies actual foreign named type identities and original native facts.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
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
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.SimpleTypeImports;
/// <summary>Verifies original facts and actual imported type namespaces.</summary>
public sealed class NativeSimpleTypeImportsIdentityTests
{
 private const string Root="https://catalog.test/root.xsd";
 private const string Main="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:f='urn:lib'><xs:import namespace='urn:lib' schemaLocation='lib.xsd'/><xs:element name='r' type='f:D'/></xs:schema>";
 private const string Library="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:lib' xmlns:f='urn:lib'><xs:simpleType name='D'><xs:restriction base='f:E'><xs:enumeration value='1.23'/></xs:restriction></xs:simpleType><xs:simpleType name='E'><xs:restriction base='xs:decimal'/></xs:simpleType></xs:schema>";
 private static NativeSchemaAssembly Assembly(string? root=null){var result=NativeDecimalSchema.AssembleCatalog(Root,new Dictionary<string,string>{{Root,root??Main},{"https://catalog.test/lib.xsd",Library}},default);Assert.Equal(NativeOutcome.Valid,result.Outcome);return result.Value!;}
 private static Fixture Open(NativeSchemaAssembly assembly,string xml="<r>1.2300</r>")=>new(assembly.Text,xml,importedTypeNamespaces:assembly.SimpleTypeNamespaces);
 /// <summary>The actual governing imported type carries its defining namespace.</summary>
 [Fact]public void TypeIdentity(){using var f=Open(Assembly());var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal("D",fact.NativeType.Name);Assert.Equal("urn:lib",fact.NativeType.NamespaceUri);Assert.Equal("1.23",f.Read(fact.Node).DecimalValue.ToInvariantString(256));Assert.Same(fact.Type,f.Provider.ResolveType(new("urn:lib","D"),default).Value);}
 /// <summary>An imported base is the actual named foreign component rather than a root-namespace alias.</summary>
 [Fact]public void Derivation(){using var f=Open(Assembly());var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal("E",fact.NativeType.DerivationBase!.Name);Assert.Equal("urn:lib",fact.NativeType.DerivationBase.NamespaceUri);Assert.Equal(SchemaResultKind.Supported,f.Provider.ResolveType(new("urn:lib","E"),default).Kind);}
 /// <summary>Same local names in distinct namespaces remain separate type handles.</summary>
 [Fact]public void DistinctNamespaces(){var schema=Main.Replace("<xs:element name='r' type='f:D'/>","<xs:simpleType name='D'><xs:restriction base='xs:decimal'/></xs:simpleType><xs:element name='r' type='f:D'/>");using var f=Open(Assembly(schema));var local=f.Provider.ResolveType(new("","D"),default).Value;var foreign=f.Provider.ResolveType(new("urn:lib","D"),default).Value;Assert.NotSame(local,foreign);Assert.False(f.Provider.IsSameComponent(local,foreign,default).Value);}
 /// <summary>Imported type handles remain scoped to the actual compilation authority and input lifetime.</summary>
 [Fact]public void AuthorityAndLifetime(){var assembly=Assembly();using var a=Open(assembly);using var b=Open(assembly);var x=a.Input.FactsFor((NativeSessionNode)a.Element([]))!;var y=b.Input.FactsFor((NativeSessionNode)b.Element([]))!;Assert.False(a.Provider.Owns(y.Type));Assert.Throws<ArgumentException>(()=>a.Provider.IsSameComponent(x.Type,y.Type,default));var detached=a.Read(x.Node).DecimalValue;a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>x.ReadTypedValue(default));Assert.Equal("1.23",detached.ToInvariantString(256));}
 /// <summary>Failed imported membership issues no successful typed facts or evidence.</summary>
 [Fact]public void InvalidHasNoFacts(){using var f=Open(Assembly(),"<r>1.24</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);}
 /// <summary>Plain XML cannot manufacture the private imported-type projection.</summary>
 [Fact]public void ProjectionIsPrivate(){var assembly=Assembly();Assert.NotEmpty(assembly.SimpleTypeNamespaces);var withoutMetadata=NativeDecimalSchema.Compile(assembly.Text,new(256,256),maxStringCharacters:1024);Assert.Equal(NativeOutcome.Unsupported,withoutMetadata.Outcome);var withMetadata=NativeDecimalSchema.Compile(assembly.Text,new(256,256),maxStringCharacters:1024,importedTypeNamespaces:assembly.SimpleTypeNamespaces);Assert.Equal(NativeOutcome.Valid,withMetadata.Outcome);}
}
