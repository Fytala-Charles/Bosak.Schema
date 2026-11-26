// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies bounded dates through the public consumer API.
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
//                      | Charles Korthout | 0.4   | 24-11-2026     | Reconcile measured date extension in isolated test copy |
//                      | Charles Korthout | 0.5   | 24-11-2026     | Adopt measured date extension factory expectation |
//                      | Charles Korthout | 0.6   | 24-11-2026     | Reconcile measured date-enumeration outcomes in isolated tests only                         |
//                      | Charles Korthout | 0.7   | 24-11-2026     | Adopt measured date enumeration expectation reconciliation |
//                      | Charles Korthout | 0.8   | 24-11-2026     | Reconcile measured date-bound and timezone outcomes |
//                      | Charles Korthout | 0.3   | 24-11-2026     | Reconcile measured date pattern admission without changing original schema |
//                      | Charles Korthout | 0.9   | 25-11-2026     | Reconcile measured native calendar outcomes with original stimuli |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.10   | 26-11-2026     | Reconcile measured duration admission; preserve original inputs |
//                      | Charles Korthout | 0.11   | 26-11-2026     | Reconcile measured Gregorian admission with original inputs retained |
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;


using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Verifies bounded date validation and transport.</summary>
public sealed class NativeDateConsumerTests
{
 private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,1,256,1,256,256);
 private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,120);
 private const string Start="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>";
 private const string Simple=Start+"<xs:element name='r' type='xs:date'/></xs:schema>";
 /// <summary>Gets revised planned payloads for both public facades.</summary><returns>Cases.</returns>
 public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","Dates","case-plan.json")));foreach(var r in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetProperty("schema").GetString()!,r.GetProperty("instance").GetString()!,r.GetProperty("proposedAssessment").GetString()!,data];}
 /// <summary>Assesses original planned payloads without replacing invalid inputs.</summary><param name="id">ID.</param><param name="schema">Schema.</param><param name="xml">Instance.</param><param name="expected">Outcome.</param><param name="data">Facade.</param>
 [Theory][MemberData(nameof(Cases))]
 public void Planned(string id,string schema,string xml,string expected,bool data)
 {
  Assert.NotEmpty(id);var want=Enum.Parse<NativeSchemaOutcome>(expected);
  if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;var a=s.Assess(xml,Context);Assert.Equal(want,a.Outcome);using var d=a.Value;if(d is not null){Assert.Equal(NativeScalarKind.Date,d.ReadElement([]).Kind);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}}
  else{var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;var a=s.Assess(xml,Context);Assert.Equal(want,a.Outcome);using var d=a.Value;if(d is not null){Assert.True(d.HasRootScalar);Assert.Equal(NativeScalarKind.Date,d.ReadRootScalar().Kind);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}}
 }
 /// <summary>Gets native placement schemas.</summary><returns>Schemas.</returns>
 private static IEnumerable<object[]> RawPlacements(){yield return[Simple,"<r>0000-02-29</r>",0];yield return[Start+"<xs:simpleType name='N'><xs:restriction base='xs:date'/></xs:simpleType><xs:element name='r' type='N'/></xs:schema>","<r>0000-02-29</r>",0];yield return[Start+"<xs:element name='r'><xs:simpleType><xs:restriction base='xs:date'/></xs:simpleType></xs:element></xs:schema>","<r>0000-02-29</r>",0];yield return[Start+"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='d' type='xs:date'/></xs:sequence><xs:attribute name='a' type='xs:date'/></xs:complexType></xs:element></xs:schema>","<r a='-0004-02-29Z'><d>0000-02-29</d></r>",1];yield return[Start+"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='g'><xs:complexType><xs:sequence><xs:element name='d' type='xs:date'/></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:schema>","<r><g><d>0000-02-29</d></g></r>",2];}
 /// <summary>Gets uniquely labelled actual-native placements.</summary><returns>Cases.</returns>
 public static IEnumerable<object[]> Placements(){int i=0;foreach(var r in RawPlacements())yield return[r[0],r[1],r[2],"DP-"+(++i).ToString("D2")];}
 /// <summary>Checks public lifetime, cancellation and wrong accessors.</summary><param name="data">Facade.</param>
 [Theory][InlineData(false)][InlineData(true)]
 public void PublicLifetime(bool data)
 {
  using var cancel=new CancellationTokenSource();cancel.Cancel();NativeScalarValue detached;
  if(data){Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaNodeDataSession.Create(Simple,Options,cancel.Token));using var s=NativeSchemaNodeDataSession.Create(Simple,Options).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>s.Assess("<r>0000-02-29</r>",Context,cancel.Token));Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r>1900-02-29</r>",Context).Outcome);using var d=s.Assess("<r>0000-02-29</r>",Context).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadElement([],cancel.Token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ExportXml(cancel.Token));detached=d.ReadElement([]);var xml=d.ExportXml();Assert.Throws<InvalidOperationException>(()=>s.Assess(xml,Context));d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadElement([]));using var reopened=s.Assess(xml,Context).Value!;s.Dispose();Assert.Throws<ObjectDisposedException>(()=>reopened.ReadElement([]));}
  else{Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaSession.Create(Simple,Options,cancel.Token));using var s=NativeSchemaSession.Create(Simple,Options).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>s.Assess("<r>0000-02-29</r>",Context,cancel.Token));Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r>1900-02-29</r>",Context).Outcome);using var d=s.Assess("<r>0000-02-29</r>",Context).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadRootScalar(cancel.Token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ExportXml(cancel.Token));detached=d.ReadRootScalar();var xml=d.ExportXml();Assert.Throws<InvalidOperationException>(()=>s.Assess(xml,Context));d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootScalar());using var reopened=s.Assess(xml,Context).Value!;s.Dispose();Assert.Throws<ObjectDisposedException>(()=>reopened.ReadRootScalar());}
  Assert.Equal(0,detached.DateValue.Year);Assert.Null(detached.DateValue.TimezoneOffsetMinutes);Assert.Throws<InvalidOperationException>(()=>detached.IntegerValue);Assert.Throws<InvalidOperationException>(()=>detached.StringValue);Assert.Throws<InvalidOperationException>(()=>detached.DecimalValue);Assert.Throws<InvalidOperationException>(()=>detached.BooleanValue);Assert.Empty(typeof(NativeDateValue).GetConstructors());Assert.Equal(5,(int)NativeScalarKind.Date);
 }
 /// <summary>Unsupported temporal facets and legacy grammar remain refused.</summary>
 [Fact]public void Exclusions(){foreach(var facet in new[]{"enumeration","minInclusive","maxExclusive","pattern","explicitTimezone"}){var schema=Start+"<xs:simpleType name='Unused'><xs:restriction base='xs:date'><xs:"+facet+" value='2000-01-01'/></xs:restriction></xs:simpleType><xs:element name='r' type='xs:string'/></xs:schema>";if(facet is "enumeration" or "minInclusive" or "maxExclusive" or "pattern"){var a=NativeSchemaSession.Create(schema,Options);var b=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);Assert.Equal(NativeSchemaOutcome.Valid,b.Outcome);a.Value!.Dispose();b.Value!.Dispose();}else if(facet=="explicitTimezone"){Assert.Equal(NativeSchemaOutcome.Invalid,NativeSchemaSession.Create(schema,Options).Outcome);Assert.Equal(NativeSchemaOutcome.Invalid,NativeSchemaNodeDataSession.Create(schema,Options).Outcome);}else{Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(schema,Options).Outcome);Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaNodeDataSession.Create(schema,Options).Outcome);}}Assert.Equal(NativeDecimalOutcome.Unsupported,NativeDecimalSession.Create(Simple,new(256,256,1,32,1,256,256)).Outcome);}

 /// <summary>Gets placements through both public facades.</summary><returns>Cases.</returns>
 public static IEnumerable<object[]> PublicPlacements(){foreach(var r in Placements())foreach(bool data in new[]{false,true})yield return[r[0],r[1],r[2],data,r[3]];}
 /// <summary>Checks descendant paths, aliases, attributes and public export.</summary><param name="schema">Schema.</param><param name="xml">XML.</param><param name="depth">Depth.</param><param name="data">Facade.</param><param name="placementId">Stable placement ID.</param>
 [Theory][MemberData(nameof(PublicPlacements))]
 public void PublicPlacement(string schema,string xml,int depth,bool data,string placementId)
 {
  Assert.NotEmpty(placementId);NativeDateValue date;
  if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;using var d=s.Assess(xml,Context).Value!;date=d.ReadElement(Enumerable.Repeat(0,depth).ToImmutableArray()).DateValue;if(depth==1){var a=d.ReadAttribute([],"","a").DateValue;Assert.Equal(-4,a.Year);Assert.Equal(0,a.TimezoneOffsetMinutes);}Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}
  else{using var s=NativeSchemaSession.Create(schema,Options).Value!;using var d=s.Assess(xml,Context).Value!;date=(depth==0?d.ReadRootScalar():d.ReadScalar(0)).DateValue;if(depth==1){var a=d.ReadAttribute(0).Value.DateValue;Assert.Equal(-4,a.Year);Assert.Equal(0,a.TimezoneOffsetMinutes);}Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}
  Assert.Equal(0,date.Year);Assert.Equal(2,date.Month);Assert.Equal(29,date.Day);Assert.Null(date.TimezoneOffsetMinutes);
 }
 /// <summary>Checks explicit date timezone and extreme negative year reads.</summary><param name="data">Facade.</param>
 [Theory][InlineData(false)][InlineData(true)]
 public void ExtremeAndZones(bool data)
 {
  foreach(var value in new[]{"-9223372036854775807-01-01+14:00","9223372036854775807-01-01-14:00","10000-01-01-00:00"})
  {
   NativeDateValue date;var xml="<r>"+value+"</r>";
   if(data){using var s=NativeSchemaNodeDataSession.Create(Simple,Options).Value!;using var d=s.Assess(xml,Context).Value!;date=d.ReadElement([]).DateValue;}
   else{using var s=NativeSchemaSession.Create(Simple,Options).Value!;using var d=s.Assess(xml,Context).Value!;date=d.ReadRootScalar().DateValue;}
   Assert.Equal(value[0]=='-'?-long.MaxValue:value.StartsWith("10000",StringComparison.Ordinal)?10000:long.MaxValue,date.Year);Assert.Equal(value[0]=='-'?840:value.StartsWith("10000",StringComparison.Ordinal)?0:-840,date.TimezoneOffsetMinutes);
  }
 }
 /// <summary>Excluded calendar types and complex date content remain refused.</summary>
 [Fact]public void DeferredTypes()
 {
  foreach(var name in new[]{"time","dateTime","dateTimeStamp","gYear","duration"})foreach(bool data in new[]{false,true}){var schema=Simple.Replace("xs:date'","xs:"+name+"'",StringComparison.Ordinal);if(name is "time" or "dateTime" or "dateTimeStamp" or "duration" or "gYear"){if(data){var compiled=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,compiled.Outcome);compiled.Value!.Dispose();}else{var compiled=NativeSchemaSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,compiled.Outcome);compiled.Value!.Dispose();}}else Assert.Equal(NativeSchemaOutcome.Unsupported,data?NativeSchemaNodeDataSession.Create(schema,Options).Outcome:NativeSchemaSession.Create(schema,Options).Outcome);}
  var complex=Start+"<xs:element name='r'><xs:complexType><xs:simpleContent><xs:extension base='xs:date'/></xs:simpleContent></xs:complexType></xs:element></xs:schema>";var admitted=NativeSchemaSession.Create(complex,Options);Assert.Equal(NativeSchemaOutcome.Valid,admitted.Outcome);admitted.Value?.Dispose();
 }

 /// <summary>Checks public selected alias and capacity diagnostics.</summary><param name="data">Facade.</param>
 [Theory][InlineData(false)][InlineData(true)]
 public void SelectedAndCapacity(bool data)
 {
  var schema=Start+"<xs:simpleType name='N'><xs:restriction base='xs:date'/></xs:simpleType><xs:element name='r' type='xs:date'/></xs:schema>";var xml="<r xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='N'>0000-02-29</r>";
  if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;using(var d=s.Assess(xml,Context).Value!)Assert.Equal(0,d.ReadElement([]).DateValue.Year);var a=s.Assess("<r>-9223372036854775808-01-01</r>",Context);Assert.Equal(NativeSchemaOutcome.Unsupported,a.Outcome);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,Assert.Single(a.Diagnostics).UnsupportedReason);}
  else{using var s=NativeSchemaSession.Create(schema,Options).Value!;using(var d=s.Assess(xml,Context).Value!)Assert.Equal(0,d.ReadRootScalar().DateValue.Year);var a=s.Assess("<r>-9223372036854775808-01-01</r>",Context);Assert.Equal(NativeSchemaOutcome.Unsupported,a.Outcome);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,Assert.Single(a.Diagnostics).UnsupportedReason);}
 }
}
