// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies bounded direct decimal enumeration.
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
//                      | Charles Korthout | 0.2   | 24-11-2026     | Reconcile six measured named decimal chain admissions |
//                      | Charles Korthout | 0.3   | 25-11-2026     | Reconcile measured complex enum validity including invalid base bounds |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Text.Json;
using System.Collections.Immutable;
using System.Xml.Linq;
using Bosak.Schema;

using Bosak.XPath.Core.Xdm;

using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Verifies direct decimal enumeration with actual inputs.</summary>
public sealed class NativeDecimalEnumerationConsumerTests
{
 private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,2,256,1,256,1024);
 /// <summary>Gets authored cases on both facades and three timezones.</summary><returns>Unmodified payloads.</returns>
 public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","DecimalEnumeration","case-plan.json")));foreach(var row in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})foreach(int zone in new[]{-840,0,840})yield return[row.GetProperty("id").GetString()!,row.GetRawText(),data,zone];}
 /// <summary>Executes authored proposals through real native sessions.</summary><param name="id">Case.</param><param name="json">Payload.</param><param name="data">Facade.</param><param name="zone">Implicit timezone.</param>
 [Theory][MemberData(nameof(Cases))]public void Contract(string id,string json,bool data,int zone)
 {
  using var j=JsonDocument.Parse(json);var r=j.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("proposedCompile").GetString();var assessment=r.GetProperty("proposedAssessment").GetString();if(id=="named-chain-separate"){compile="Valid";assessment="Valid";}if(id=="complex-base-enum-separate"){compile="Invalid";assessment=null;}if(id=="complex-local-enum-separate"){compile="Valid";assessment="Valid";}var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,zone);
  if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);using var s=c.Value;if(s is null)return;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==assessment,id+":assessment:"+a.Outcome);using var d=a.Value;if(d is not null){Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));var path=r.GetProperty("site").GetString() switch{"terminal"=>ImmutableArray.Create(0),"nested"=>ImmutableArray.Create(0,0),_=>ImmutableArray<int>.Empty};var v=r.GetProperty("site").GetString()=="attribute"?d.ReadAttribute([],"","d"):d.ReadElement(path);Assert.Equal(NativeScalarKind.Decimal,v.Kind);Assert.Equal(r.GetProperty("expectedDecimal").GetString(),v.DecimalValue.ToInvariantString(1024));}}
  else{var c=NativeSchemaSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);using var s=c.Value;if(s is null)return;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==assessment,id+":assessment:"+a.Outcome);using var d=a.Value;if(d is not null){Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));var site=r.GetProperty("site").GetString();var v=site=="attribute"?d.ReadAttribute(0).Value:site is "terminal" or "nested"?d.ReadScalar(0):d.ReadRootScalar();Assert.Equal(NativeScalarKind.Decimal,v.Kind);Assert.Equal(r.GetProperty("expectedDecimal").GetString(),v.DecimalValue.ToInvariantString(1024));}}
 }


 /// <summary>Numeric capacity is checked before normalization and a refusal does not poison the session.</summary><param name="data">Facade.</param>
 [Theory][InlineData(false)][InlineData(true)]public void NumericCapacity(bool data)
 {
  var start="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='D'><xs:restriction base='xs:decimal'><xs:enumeration value='";var end="'/></xs:restriction></xs:simpleType><xs:element name='r' type='D'/></xs:schema>";var options=new NativeSchemaSessionOptions(2,2,32,65536,1,32,1,32,2);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);
  if(data){var c=NativeSchemaNodeDataSession.Create(start+"1.00"+end,options);Assert.Equal(NativeSchemaOutcome.Unsupported,c.Outcome);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,Assert.Single(c.Diagnostics).UnsupportedReason);using var session=NativeSchemaNodeDataSession.Create(start+"1.0"+end,options).Value!;Assert.Equal(NativeSchemaOutcome.Unsupported,session.Assess("<r>1.00</r>",context).Outcome);using var d=session.Assess("<r>1.0</r>",context).Value!;Assert.Equal("1",d.ReadElement([]).DecimalValue.ToInvariantString(32));}
  else{var c=NativeSchemaSession.Create(start+"1.00"+end,options);Assert.Equal(NativeSchemaOutcome.Unsupported,c.Outcome);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,Assert.Single(c.Diagnostics).UnsupportedReason);using var session=NativeSchemaSession.Create(start+"1.0"+end,options).Value!;Assert.Equal(NativeSchemaOutcome.Unsupported,session.Assess("<r>1.00</r>",context).Outcome);using var d=session.Assess("<r>1.0</r>",context).Value!;Assert.Equal("1",d.ReadRootScalar().DecimalValue.ToInvariantString(32));}
 }
 /// <summary>String text limits do not replace numeric capacities.</summary><param name="data">Facade.</param>
 [Theory][InlineData(false)][InlineData(true)]public void IndependentStringLimit(bool data)
 {
  var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='D'><xs:restriction base='xs:decimal'><xs:enumeration value='1.23'/></xs:restriction></xs:simpleType><xs:element name='r' type='D'/></xs:schema>";var options=new NativeSchemaSessionOptions(256,256,1,65536,1,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);
  if(data){using var session=NativeSchemaNodeDataSession.Create(schema,options).Value!;using var d=session.Assess("<r>1.23</r>",context).Value!;Assert.NotNull(d);}else{using var session=NativeSchemaSession.Create(schema,options).Value!;using var d=session.Assess("<r>1.23</r>",context).Value!;Assert.NotNull(d);}
 }
 /// <summary>The legacy decimal-only facade retains its prior grammar.</summary>
 [Fact]public void LegacyRefusal(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='D'><xs:restriction base='xs:decimal'><xs:enumeration value='1.23'/></xs:restriction></xs:simpleType><xs:element name='r' type='D'/></xs:schema>";Assert.Equal(NativeDecimalOutcome.Unsupported,NativeDecimalSession.Create(schema,new(256,256,1,32,1,256,256)).Outcome);}
}
