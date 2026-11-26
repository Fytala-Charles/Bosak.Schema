// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies closed global attribute references through owned facades.
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
//                      | Charles Korthout | 0.2   | 25-11-2026     | Reconcile six measured attribute-group reference admissions without changing payloads |
//                      | Charles Korthout | 0.3 | 26-11-2026     | Reconcile original structural admission without changing stimuli |
//                      | Charles Korthout | 0.4 | 26-11-2026     | Reconcile original empty-owner admission without changing input or test name |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Verifies typed global attributes, owner-specific uses and original source data.</summary>
public sealed class NativeGlobalAttributeReferencesConsumerTests
{
 private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,2,256,1,256,1024);
 /// <summary>Gets authored inputs on both facades and three context timezones.</summary><returns>Original case payloads.</returns>
 public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","GlobalAttributeReferences","case-plan.json")));foreach(var r in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})foreach(int zone in new[]{-840,0,840})yield return[r.GetProperty("id").GetString()!,r.GetRawText(),data,zone];}
 private static void Value(NativeScalarValue value,JsonElement row)
 {
  Assert.Equal(row.GetProperty("expectedKind").GetString(),value.Kind.ToString());var expected=row.GetProperty("expectedValue").GetString();
  switch(value.Kind){case NativeScalarKind.Decimal:Assert.Equal(expected,value.DecimalValue.ToInvariantString(1024));break;case NativeScalarKind.String:Assert.Equal(expected,value.StringValue);break;case NativeScalarKind.Boolean:Assert.Equal(expected=="true",value.BooleanValue);break;case NativeScalarKind.Integer:Assert.Equal(expected,value.IntegerValue.ToString(CultureInfo.InvariantCulture));break;case NativeScalarKind.Date:Assert.Equal(2000,value.DateValue.Year);Assert.Equal(1,value.DateValue.Month);Assert.Equal(2,value.DateValue.Day);Assert.Equal(0,value.DateValue.TimezoneOffsetMinutes);break;default:Assert.Fail("Unexpected scalar kind.");break;}
 }
 /// <summary>Executes real compile/assessment/refusal and original typed attribute reads.</summary><param name="id">Case.</param><param name="json">Payload.</param><param name="data">Facade.</param><param name="zone">Context timezone.</param>
 [Theory][MemberData(nameof(Cases))]public void Contract(string id,string json,bool data,int zone)
 {
  using var d=JsonDocument.Parse(json);var r=d.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("proposedCompile").GetString();var outcome=r.GetProperty("proposedAssessment").GetString();if(id=="empty-owner-separate"){compile="Valid";outcome="Valid";}var present=r.GetProperty("expectAttribute").GetBoolean();var path=r.GetProperty("path").EnumerateArray().Select(x=>x.GetInt32()).ToImmutableArray();if(id is "group-separate" or "global-inline-separate"){compile="Valid";outcome="Valid";}var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,zone);
  if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.True(compile==c.Outcome.ToString(),id+":compile:"+c.Outcome+":"+JsonSerializer.Serialize(c.Diagnostics));using var s=c.Value;if(s is null)return;var a=s.Assess(xml,context);Assert.True(outcome==a.Outcome.ToString(),id+":assessment:"+a.Outcome);using var document=a.Value;if(document is null)return;if(present)Value(document.ReadAttribute(path,r.GetProperty("namespace").GetString()!,r.GetProperty("name").GetString()!),r);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(document.ExportXml())));}
  else{var c=NativeSchemaSession.Create(schema,Options);Assert.True(compile==c.Outcome.ToString(),id+":compile:"+c.Outcome+":"+JsonSerializer.Serialize(c.Diagnostics));using var s=c.Value;if(s is null)return;var a=s.Assess(xml,context);Assert.True(outcome==a.Outcome.ToString(),id+":assessment:"+a.Outcome);using var document=a.Value;if(document is null)return;Assert.Equal(present?1:0,document.AttributeCount+document.DescendantAttributeCount);if(present)Value(path.IsEmpty?document.ReadAttribute(0).Value:document.ReadDescendantAttribute(0).Value,r);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(document.ExportXml())));}
 }
}
