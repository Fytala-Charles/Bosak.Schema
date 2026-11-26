// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies mixed original aggregate strings and independently typed children.
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
//                      | Charles Korthout | 0.2 | 26-11-2026     | Reconcile original markup admissions; preserve source payloads |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Verifies mixed sequence content through both owned public facades.</summary>
public sealed class NativeMixedSequencesConsumerTests
{
 private static NativeSchemaSessionOptions Options=>new(256,256,1024,65536,2,256,1,256,1024);
 /// <summary>Gets original authored nil/empty cases on both facades and three context zones.</summary><returns>Payloads.</returns>
 public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","MixedSequences","case-plan.json")));foreach(var r in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})foreach(int zone in new[]{-840,0,840})yield return[r.GetProperty("id").GetString()!,r.GetRawText(),data,zone];}
 private static void Value(NativeScalarValue value,JsonElement row)
 {
  Assert.Equal(row.GetProperty("kind").GetString(),value.Kind.ToString());
  if(value.Kind==NativeScalarKind.Empty){Assert.Throws<InvalidOperationException>(()=>value.StringValue);Assert.Throws<InvalidOperationException>(()=>value.DecimalValue);Assert.Throws<InvalidOperationException>(()=>value.BooleanValue);Assert.Throws<InvalidOperationException>(()=>value.IntegerValue);Assert.Throws<InvalidOperationException>(()=>value.DateValue);}
  else if(value.Kind==NativeScalarKind.String)Assert.Equal(row.GetProperty("value").GetString(),value.StringValue);
  else if(value.Kind==NativeScalarKind.Integer)Assert.Equal(row.GetProperty("value").GetString(),value.IntegerValue.ToString(CultureInfo.InvariantCulture));
  else if(value.Kind==NativeScalarKind.Boolean)Assert.Equal(row.GetProperty("value").GetString()=="true",value.BooleanValue);
  else if(value.Kind==NativeScalarKind.Date){var e=row.GetProperty("date");Assert.Equal(e.GetProperty("year").GetInt64(),value.DateValue.Year);Assert.Equal(e.GetProperty("month").GetInt32(),value.DateValue.Month);Assert.Equal(e.GetProperty("day").GetInt32(),value.DateValue.Day);Assert.Equal(e.GetProperty("timezone").GetInt32(),value.DateValue.TimezoneOffsetMinutes);}
  else Assert.Equal(row.GetProperty("value").GetString(),value.DecimalValue.ToInvariantString(1024));
 }
 /// <summary>Checks actual compile/assessment outcomes, flags, values and original XML.</summary><param name="id">Case.</param><param name="json">Payload.</param><param name="data">Facade.</param><param name="zone">Context timezone.</param>
 [Theory][MemberData(nameof(Cases))]public void Contract(string id,string json,bool data,int zone)
 {
  using var d=JsonDocument.Parse(json);var r=d.RootElement;r=NativeMarkupLegacyVerification.Reconcile(r,nameof(NativeMixedSequencesConsumerTests));var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("proposedCompile").GetString();var outcome=r.GetProperty("proposedAssessment").GetString();var values=r.GetProperty("values").EnumerateArray().ToArray();var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,zone);var catalog=r.TryGetProperty("documents",out var inventory)?inventory.EnumerateObject().ToDictionary(p=>p.Name,p=>p.Value.GetString()!,StringComparer.Ordinal):null;
  if(data){var c=catalog is null?NativeSchemaNodeDataSession.Create(schema,Options):NativeSchemaNodeDataSession.Create(r.GetProperty("rootUri").GetString()!,catalog,Options);Assert.True(compile==c.Outcome.ToString(),id+":compile:"+c.Outcome+":"+JsonSerializer.Serialize(c.Diagnostics));using var s=c.Value;if(s is null)return;var a=s.Assess(xml,context);Assert.True(outcome==a.Outcome.ToString(),id+":assessment:"+a.Outcome);using var document=a.Value;if(document is null)return;Assert.Equal(r.GetProperty("isRootNilled").GetBoolean(),document.IsNilled([]));foreach(var v in values){var path=v.GetProperty("path").EnumerateArray().Select(x=>x.GetInt32()).ToImmutableArray();Assert.Equal(v.GetProperty("nilled").GetBoolean(),document.IsNilled(path));Value(document.ReadElement(path),v);}Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(document.ExportXml())));}
  else{var c=catalog is null?NativeSchemaSession.Create(schema,Options):NativeSchemaSession.Create(r.GetProperty("rootUri").GetString()!,catalog,Options);Assert.True(compile==c.Outcome.ToString(),id+":compile:"+c.Outcome+":"+JsonSerializer.Serialize(c.Diagnostics));using var s=c.Value;if(s is null)return;var a=s.Assess(xml,context);Assert.True(outcome==a.Outcome.ToString(),id+":assessment:"+a.Outcome);using var document=a.Value;if(document is null)return;Assert.Equal(r.GetProperty("hasRootScalar").GetBoolean(),document.HasRootScalar);Assert.Equal(r.GetProperty("isRootNilled").GetBoolean(),document.IsRootNilled);Assert.Equal(r.GetProperty("scalarCount").GetInt32(),document.ScalarCount);var scalarIndex=0;foreach(var v in values)Value(v.GetProperty("path").GetArrayLength()==0?document.ReadRootScalar():document.ReadScalar(scalarIndex++),v);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(document.ExportXml())));}
 }
}
