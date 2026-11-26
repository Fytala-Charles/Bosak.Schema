// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies bounded named decimal restriction chains.
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
//                      | Charles Korthout | 0.2 | 26-11-2026     | Reconcile original markup admissions; preserve source payloads |
//                      | Charles Korthout | 0.3 | 26-11-2026     | Reconcile measured original numeric pattern outcomes without changing inputs |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Text.Json;
using System.Collections.Immutable;
using System.Xml.Linq;
using Bosak.Schema;

using Bosak.XPath.Core.Xdm;

using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Verifies named decimal derivation with actual inputs.</summary>
public sealed class NativeDecimalChainsConsumerTests
{
 private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,2,256,1,256,1024);
 /// <summary>Gets authored cases on both facades and three timezones.</summary><returns>Unmodified payloads.</returns>
 public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","DecimalChains","case-plan.json")));foreach(var row in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})foreach(int zone in new[]{-840,0,840})yield return[row.GetProperty("id").GetString()!,row.GetRawText(),data,zone];}
 /// <summary>Executes authored proposals through real native sessions.</summary><param name="id">Case.</param><param name="json">Payload.</param><param name="data">Facade.</param><param name="zone">Implicit timezone.</param>
 [Theory][MemberData(nameof(Cases))]public void Contract(string id,string json,bool data,int zone)
 {
  using var j=JsonDocument.Parse(json);var r=j.RootElement;r=NativeNumericLegacyVerification.Reconcile(r,nameof(NativeDecimalChainsConsumerTests));r=NativeMarkupLegacyVerification.Reconcile(r,nameof(NativeDecimalChainsConsumerTests));var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("proposedCompile").GetString();var assessment=r.GetProperty("proposedAssessment").GetString();var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,zone);
  if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);using var s=c.Value;if(s is null)return;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==assessment,id+":assessment:"+a.Outcome);using var d=a.Value;if(d is not null){Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));var path=r.GetProperty("site").GetString() switch{"terminal"=>ImmutableArray.Create(0),"nested"=>ImmutableArray.Create(0,0),_=>ImmutableArray<int>.Empty};var v=r.GetProperty("site").GetString()=="attribute"?d.ReadAttribute([],"","d"):d.ReadElement(path);Assert.Equal(NativeScalarKind.Decimal,v.Kind);Assert.Equal(r.GetProperty("expectedDecimal").GetString(),v.DecimalValue.ToInvariantString(1024));}}
  else{var c=NativeSchemaSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);using var s=c.Value;if(s is null)return;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==assessment,id+":assessment:"+a.Outcome);using var d=a.Value;if(d is not null){Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));var site=r.GetProperty("site").GetString();var v=site=="attribute"?d.ReadAttribute(0).Value:site is "terminal" or "nested"?d.ReadScalar(0):d.ReadRootScalar();Assert.Equal(NativeScalarKind.Decimal,v.Kind);Assert.Equal(r.GetProperty("expectedDecimal").GetString(),v.DecimalValue.ToInvariantString(1024));}}
 }



}
