// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies Boolean pattern work and original component identity.
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
namespace Bosak.Schema.Tests.BooleanPatterns;
/// <summary>Verifies Boolean patterns against actual budgets and native facts.</summary>
public sealed class NativeBooleanPatternsIdentityTests
{
 /// <summary>Boolean matching consumes the supplied shared work budget.</summary>
 [Fact]public void Work()
 {
  var c=new NativePatternCompilation(default);var f=NativeBooleanFacets.Compile(XElement.Parse("<xs:restriction xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:pattern value='true'/></xs:restriction>"),c);var b=new NativePatternBudget(default);Assert.True(f.Accepts("true",b));var first=b.Consumed;Assert.True(first>0);Assert.True(f.Accepts("true",b));Assert.Equal(first*2,b.Consumed);Assert.Throws<NativeProfileException>(()=>f.Accepts("true",new NativePatternBudget(default,1)));
 }
 /// <summary>The actual cancellation token is used by matching.</summary>
 [Fact]public void Cancellation()
 {
  var f=NativeBooleanFacets.Compile(XElement.Parse("<xs:restriction xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:pattern value='true'/></xs:restriction>"),new(default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.ThrowsAny<OperationCanceledException>(()=>f.Accepts("true",new(cancel.Token)));
 }
 /// <summary>Named Boolean restrictions retain governing identity and original typed values.</summary>
 [Fact]public void Components()
 {
  var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='D'><xs:restriction base='xs:boolean'><xs:pattern value='1'/></xs:restriction></xs:simpleType><xs:element name='r' type='D'/></xs:schema>";using var f=new Fixture(schema,"<r>1</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("D",fact.NativeType.Name);Assert.True(fact.NativeType.IsBoolean);Assert.Equal("boolean",fact.NativeType.BaseName!.LocalName);Assert.True(f.Read(f.Input.Root).BooleanValue);f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));
 }
}
