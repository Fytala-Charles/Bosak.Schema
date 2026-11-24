// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies named Boolean inheritance, budgets and native facts.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 24-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 24-11-2026     | Adopt qualified date enumeration controls |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.BooleanChains;
/// <summary>Verifies inherited Boolean component identity and work accounting.</summary>
public sealed class NativeBooleanChainsIdentityTests
{
 private const string Prefix="<xs:restriction xmlns:xs='http://www.w3.org/2001/XMLSchema'>";
 private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='A'><xs:restriction base='xs:boolean'><xs:pattern value='(true|1)'/></xs:restriction></xs:simpleType><xs:simpleType name='D'><xs:restriction base='A'><xs:pattern value='true'/></xs:restriction></xs:simpleType><xs:element name='r' type='D'/></xs:schema>";
 /// <summary>Actual governing type retains its actual named and primitive base.</summary>
 [Fact]public void Components(){using var f=new Fixture(Schema,"<r>true</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("D",fact.NativeType.Name);Assert.Equal("A",fact.NativeType.DerivationBase!.Name);Assert.True(fact.NativeType.DerivationBase.IsBoolean);Assert.Equal("boolean",fact.NativeType.DerivationBase.BaseName!.LocalName);Assert.True(f.Read(f.Input.Root).BooleanValue);f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));}
 /// <summary>Inherited pattern failure issues no successful facts or evidence.</summary>
 [Fact]public void InvalidFacts(){using var f=new Fixture(Schema,"<r>1</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);}
 /// <summary>Ancestors and local groups share one supplied work budget.</summary>
 [Fact]public void SharedWork(){var c=new NativePatternCompilation(default);var a=NativeBooleanFacets.Compile(XElement.Parse(Prefix+"<xs:pattern value='(true|1)'/></xs:restriction>"),c);var d=NativeBooleanFacets.CompileDerived(XElement.Parse(Prefix+"<xs:pattern value='true'/></xs:restriction>"),c,a);var b=new NativePatternBudget(default);Assert.True(d.Accepts("true",b));var cost=b.Consumed;Assert.True(cost>0);Assert.Throws<NativeProfileException>(()=>d.Accepts("true",new(default,cost-1)));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.ThrowsAny<OperationCanceledException>(()=>d.Accepts("true",new(cancel.Token)));}
 /// <summary>Facet-free aliases reuse inherited automata without reserving duplicate states.</summary>
 [Fact]public void AliasStates(){var c=new NativePatternCompilation(default);var a=NativeBooleanFacets.Compile(XElement.Parse(Prefix+"<xs:pattern value='true'/></xs:restriction>"),c);var states=c.States;for(int i=0;i<14;i++)a=NativeBooleanFacets.CompileDerived(XElement.Parse(Prefix+"</xs:restriction>"),c,a);Assert.Equal(states,c.States);Assert.True(a.Accepts("true",new(default)));}
}
