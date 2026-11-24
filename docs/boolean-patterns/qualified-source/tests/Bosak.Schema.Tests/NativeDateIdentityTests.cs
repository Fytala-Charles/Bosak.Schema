// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies actual original-node date identities and typed provenance.
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
//                      | Charles Korthout | 0.2   | 24-11-2026     | Expand identity controls and label colliding theory displays |
//                      | Charles Korthout | 0.3   | 24-11-2026     | Adopt qualified date controls with separated public and identity lanes |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.DateExperiment;
/// <summary>Verifies bounded date validation and transport.</summary>
public sealed class NativeDateIdentityTests
{
 private const string Start="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>";
 private const string Simple=Start+"<xs:element name='r' type='xs:date'/></xs:schema>";
 /// <summary>Gets native placement schemas.</summary><returns>Schemas.</returns>
 private static IEnumerable<object[]> RawPlacements(){yield return[Simple,"<r>0000-02-29</r>",0];yield return[Start+"<xs:simpleType name='N'><xs:restriction base='xs:date'/></xs:simpleType><xs:element name='r' type='N'/></xs:schema>","<r>0000-02-29</r>",0];yield return[Start+"<xs:element name='r'><xs:simpleType><xs:restriction base='xs:date'/></xs:simpleType></xs:element></xs:schema>","<r>0000-02-29</r>",0];yield return[Start+"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='d' type='xs:date'/></xs:sequence><xs:attribute name='a' type='xs:date'/></xs:complexType></xs:element></xs:schema>","<r a='-0004-02-29Z'><d>0000-02-29</d></r>",1];yield return[Start+"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='g'><xs:complexType><xs:sequence><xs:element name='d' type='xs:date'/></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:schema>","<r><g><d>0000-02-29</d></g></r>",2];}
 /// <summary>Gets uniquely labelled actual-native placements.</summary><returns>Cases.</returns>
 public static IEnumerable<object[]> Placements(){int i=0;foreach(var r in RawPlacements())yield return[r[0],r[1],r[2],"DP-"+(++i).ToString("D2")];}
 /// <summary>Checks actual Core atoms, facts and foreign authority across compiler routes.</summary><param name="schema">Schema.</param><param name="xml">Instance.</param><param name="depth">Date depth.</param><param name="placementId">Stable placement ID.</param>
 [Theory][MemberData(nameof(Placements))]
 public void ActualFacts(string schema,string xml,int depth,string placementId)
 {
  Assert.NotEmpty(placementId);using var f=new Fixture(schema,xml);var path=Enumerable.Repeat(0,depth).ToImmutableArray();var node=f.Element(path);var fact=f.Input.FactsFor((NativeSessionNode)node)!;Assert.NotNull(fact);Assert.NotNull(fact.Declaration);Assert.NotNull(fact.Type);var typed=fact.ReadTypedValue(default).Value;Assert.True(typed.TryGetSingleAtomic(out var atomic));Assert.Equal(XdmValueKind.Date,atomic.Value.Kind);Assert.Same(fact.Type,typed.GoverningType);Assert.Same(f.Provider.TypeComponent(fact.NativeType),atomic.TypeEvidence.ActualType);Assert.True(f.Provider.Owns(fact.Declaration));Assert.Equal(depth==0?"r":"d",fact.NativeDeclaration.Name);
  var date=f.Run(op=>{var atom=f.Evaluate(op,node);Assert.Equal(XdmValueKind.Date,atom.Kind);Assert.True(SchemaAtomics.TryGetProvenance(atom,out var evidence));Assert.Same(fact.Type,evidence!.DeclaredType);Assert.Same(f.Provider.TypeComponent(fact.NativeType),evidence.ActualType);Assert.Same(f.Assessment.Origin,evidence.OriginalAssessment);Assert.Equal(0,atom.DateXPathValue.Year);Assert.Equal("0000-02-29",atom.ToString());return atom.DateXPathValue;});Assert.False(date.HasTimezone);var detached=f.Read(node).DateValue;Assert.Equal(0,detached.Year);Assert.Null(detached.TimezoneOffsetMinutes);
  if(depth==1){var attribute=f.Attribute([],"","a");var a=f.Read(attribute).DateValue;Assert.Equal(-4,a.Year);Assert.Equal(0,a.TimezoneOffsetMinutes);}
  using var other=new Fixture(schema,xml);Assert.False(other.Session.Advisor!.OwnsNode(node,fact,other.Input,other.Assessment.Origin));Assert.Throws<ArgumentException>(()=>other.Run(op=>other.Evaluate(op,node)));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));Assert.Equal(29,detached.Day);
 }
 /// <summary>Invalid dates produce no partial native facts.</summary>
 [Fact]public void InvalidFacts(){using var f=new Fixture(Simple,"<r>1900-02-29</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Assessment.Evidence);}
 /// <summary>Checks selected date alias declaration, governing and atomic identity.</summary>
 [Fact]public void SelectedIdentity()
 {
  var schema=Start+"<xs:simpleType name='N'><xs:restriction base='xs:date'/></xs:simpleType><xs:element name='r' type='xs:date'/></xs:schema>";var xml="<r xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='N'>0000-02-29</r>";
  using var f=new Fixture(schema,xml);var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("date",fact.NativeDeclaration.Type.Name);Assert.Equal("N",fact.NativeType.Name);Assert.NotSame(fact.NativeDeclaration.Type,fact.NativeType);var typed=fact.ReadTypedValue(default).Value;Assert.True(typed.TryGetSingleAtomic(out var atomic));Assert.Same(f.Provider.TypeComponent(fact.NativeType),atomic.TypeEvidence.ActualType);Assert.Equal(0,f.Read(f.Input.Root).DateValue.Year);
 }
}
