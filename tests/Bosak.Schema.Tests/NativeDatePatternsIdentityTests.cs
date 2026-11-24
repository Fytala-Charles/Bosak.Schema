// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies bounded date pattern budgets and component identity.
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
using Xunit;
namespace Bosak.Schema.Tests.DatePatterns;
/// <summary>Verifies actual date pattern state, work and identity constraints.</summary>
public sealed class NativeDatePatternsIdentityTests
{
 private const string Facet="<xs:restriction xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:pattern value='2000-01-02Z'/></xs:restriction>";
 /// <summary>A supplied matching budget is shared and cancellation remains effective.</summary>
 [Fact]public void WorkAndCancellation()
 {
  var c=new NativePatternCompilation(default);var f=NativeDateFacets.Compile(XElement.Parse(Facet),10,c)!;var v=NativeDateParser.Parse("2000-01-02Z");var b=new NativePatternBudget(default);Assert.True(f.Accepts(v,"2000-01-02Z",b));var first=b.Consumed;Assert.True(first>0);Assert.True(f.Accepts(v,"2000-01-02Z",b));Assert.Equal(first*2,b.Consumed);
  var low=new NativePatternBudget(default,1);var e=Assert.Throws<NativeProfileException>(()=>f.Accepts(v,"2000-01-02Z",low));Assert.Equal(NativeOutcome.Unsupported,e.Outcome);
  using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.ThrowsAny<OperationCanceledException>(()=>f.Accepts(v,"2000-01-02Z",new NativePatternBudget(cancel.Token)));
 }
 /// <summary>All inherited enumeration declaration checks consume one compilation budget.</summary>
 [Fact]public void CompilationWork()
 {
  var c=new NativePatternCompilation(default);var f=NativeDateFacets.Compile(XElement.Parse(Facet),10,c)!;Assert.Equal(0,c.MatchingBudget.Consumed);var e=XElement.Parse("<xs:restriction xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:enumeration value='2000-01-02Z'/></xs:restriction>");NativeDateFacets.CompileDerived(e,11,f,c);var first=c.MatchingBudget.Consumed;Assert.True(first>0);NativeDateFacets.CompileDerived(e,11,f,c);Assert.Equal(first*2,c.MatchingBudget.Consumed);
 }
 /// <summary>String and date automata reserve one shared schema state capacity.</summary>
 [Fact]public void SharedStates()
 {
  var c=new NativePatternCompilation(default);var s=string.Concat(Enumerable.Repeat("a{15}",8));for(int i=0;i<8;i++)NativePattern.Compile(s,c);Assert.Equal(968,c.States);var e=Assert.Throws<NativeProfileException>(()=>NativeDateFacets.Compile(XElement.Parse(Facet.Replace("2000-01-02Z",s)),10,c));Assert.Equal(NativeOutcome.Unsupported,e.Outcome);
 }
 /// <summary>Patterned components retain their actual base and original date facts.</summary>
 [Fact]public void Components()
 {
  var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='A'><xs:restriction base='xs:date'><xs:pattern value='2000-01-02.*'/></xs:restriction></xs:simpleType><xs:simpleType name='D'><xs:restriction base='A'><xs:pattern value='.*[+]00:00'/></xs:restriction></xs:simpleType><xs:element name='r' type='D'/></xs:schema>";using var f=new Fixture(schema,"<r>2000-01-02+00:00</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("D",fact.NativeType.Name);Assert.Equal("A",fact.NativeType.DerivationBase!.Name);Assert.True(fact.NativeType.IsDate);var value=f.Read(f.Input.Root).DateValue;Assert.Equal(2,value.Day);Assert.Equal(0,value.TimezoneOffsetMinutes);f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));Assert.Equal(2,value.Day);
 }
}
