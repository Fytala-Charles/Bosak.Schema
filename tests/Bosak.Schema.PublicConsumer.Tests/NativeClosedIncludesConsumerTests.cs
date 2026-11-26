// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies explicit closed-catalog schema inclusion through both owned facades.
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
/// <summary>Verifies closed schema includes through both owned facades.</summary>
public sealed class NativeClosedIncludesConsumerTests
{
 private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,2,256,1,256,1024);
 /// <summary>Gets reference cases across facades and context timezones.</summary><returns>Authored payloads.</returns>
 public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","ClosedIncludes","case-plan.json")));foreach(var r in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})foreach(int zone in new[]{-840,0,840})yield return[r.GetProperty("id").GetString()!,r.GetRawText(),data,zone];}
 private static void Value(NativeScalarValue value,JsonElement row)
 {
  Assert.Equal(row.GetProperty("kind").GetString(),value.Kind.ToString());var expected=row.GetProperty("value").GetString();
  switch(value.Kind){case NativeScalarKind.Decimal:Assert.Equal(expected,value.DecimalValue.ToInvariantString(1024));break;case NativeScalarKind.String:Assert.Equal(expected,value.StringValue);break;case NativeScalarKind.Boolean:Assert.Equal(expected=="true",value.BooleanValue);break;case NativeScalarKind.Integer:Assert.Equal(expected,value.IntegerValue.ToString(CultureInfo.InvariantCulture));break;case NativeScalarKind.Date:Assert.Equal(2000,value.DateValue.Year);Assert.Equal(1,value.DateValue.Month);Assert.Equal(2,value.DateValue.Day);Assert.Equal(0,value.DateValue.TimezoneOffsetMinutes);break;default:Assert.Fail("Unexpected scalar kind.");break;}
 }
 /// <summary>Checks compilation, assessment and original scalar values.</summary><param name="id">Case.</param><param name="json">Payload.</param><param name="data">Facade.</param><param name="zone">Context timezone.</param>
 [Theory][MemberData(nameof(Cases))]public void Contract(string id,string json,bool data,int zone)
 {
  using var d=JsonDocument.Parse(json);var r=d.RootElement;r=NativeMarkupLegacyVerification.Reconcile(r,nameof(NativeClosedIncludesConsumerTests));var rootUri=r.GetProperty("rootUri").GetString()!;var catalog=r.GetProperty("documents").EnumerateObject().ToDictionary(p=>p.Name,p=>p.Value.GetString()!,StringComparer.Ordinal);var xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("proposedCompile").GetString();var outcome=r.GetProperty("proposedAssessment").GetString();var values=r.GetProperty("values").EnumerateArray().ToArray();var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,zone);
  if(data){var c=NativeSchemaNodeDataSession.Create(rootUri,catalog,Options);Assert.True(compile==c.Outcome.ToString(),id+":compile:"+c.Outcome+":"+JsonSerializer.Serialize(c.Diagnostics));using var s=c.Value;if(s is null)return;var a=s.Assess(xml,context);Assert.True(outcome==a.Outcome.ToString(),id+":assessment:"+a.Outcome);using var document=a.Value;if(document is null)return;foreach(var v in values)Value(document.ReadElement(v.GetProperty("path").EnumerateArray().Select(x=>x.GetInt32()).ToImmutableArray()),v);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(document.ExportXml())));}
  else{var c=NativeSchemaSession.Create(rootUri,catalog,Options);Assert.True(compile==c.Outcome.ToString(),id+":compile:"+c.Outcome+":"+JsonSerializer.Serialize(c.Diagnostics));using var s=c.Value;if(s is null)return;var a=s.Assess(xml,context);Assert.True(outcome==a.Outcome.ToString(),id+":assessment:"+a.Outcome);using var document=a.Value;if(document is null)return;var root=r.GetProperty("hasRootScalar").GetBoolean();Assert.Equal(root,document.HasRootScalar);Assert.Equal(root?0:values.Length,document.ScalarCount);for(int i=0;i<values.Length;i++)Value(root?document.ReadRootScalar():document.ReadScalar(i),values[i]);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(document.ExportXml())));}
 }
 /// <summary>Catalog texts are owned after creation and the legacy overload has no resolver.</summary><param name="data">Facade.</param>
 [Theory][InlineData(false)][InlineData(true)]public void CatalogOwnership(bool data)
 {
  var root="https://catalog.test/root.xsd";var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:include schemaLocation='types.xsd'/><xs:element name='r' type='D'/></xs:schema>";var catalog=new Dictionary<string,string>{{root,schema},{"https://catalog.test/types.xsd","<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='D'><xs:restriction base='xs:decimal'/></xs:simpleType></xs:schema>"}};var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);
  if(data){Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaNodeDataSession.Create(schema,Options).Outcome);using var session=NativeSchemaNodeDataSession.Create(root,catalog,Options).Value!;catalog.Clear();using var document=session.Assess("<r>1.23</r>",context).Value!;Assert.Equal("1.23",document.ReadElement([]).DecimalValue.ToInvariantString(256));}
  else{Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(schema,Options).Outcome);using var session=NativeSchemaSession.Create(root,catalog,Options).Value!;catalog.Clear();using var document=session.Assess("<r>1.23</r>",context).Value!;Assert.Equal("1.23",document.ReadRootScalar().DecimalValue.ToInvariantString(256));}
 }
 /// <summary>Pre-cancelled assembly publishes no session.</summary><param name="data">Facade.</param>
 [Theory][InlineData(false)][InlineData(true)]public void Cancellation(bool data){using var cancel=new CancellationTokenSource();cancel.Cancel();var catalog=new Dictionary<string,string>();if(data)Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaNodeDataSession.Create("https://catalog.test/root.xsd",catalog,Options,cancel.Token));else Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaSession.Create("https://catalog.test/root.xsd",catalog,Options,cancel.Token));}
 /// <summary>Required catalog API arguments retain explicit null refusal.</summary><param name="data">Facade.</param>
 [Theory][InlineData(false)][InlineData(true)]public void NullArguments(bool data){if(data){Assert.Throws<ArgumentNullException>(()=>NativeSchemaNodeDataSession.Create(null!,new Dictionary<string,string>(),Options));Assert.Throws<ArgumentNullException>(()=>NativeSchemaNodeDataSession.Create("https://catalog.test/root.xsd",null!,Options));}else{Assert.Throws<ArgumentNullException>(()=>NativeSchemaSession.Create(null!,new Dictionary<string,string>(),Options));Assert.Throws<ArgumentNullException>(()=>NativeSchemaSession.Create("https://catalog.test/root.xsd",null!,Options));}}

}
