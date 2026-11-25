// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies bounded date complex simple-content restrictions.
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
//                      | Charles Korthout | 0.3   | 24-11-2026     | Verify date restriction text cannot bypass schema-shape guards |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Text.Json;
using System.Collections.Immutable;
using System.Xml.Linq;
using Bosak.Schema;

using Bosak.XPath.Core.Xdm;

using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Verifies native date enumeration with actual inputs.</summary>
public sealed class NativeDateComplexRestrictionConsumerTests
{
 private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,2,256,1,256,1024);
 /// <summary>Gets authored cases on both facades and three timezones.</summary><returns>Unmodified payloads.</returns>
 public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","DateComplexRestriction","case-plan.json")));foreach(var row in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})foreach(int zone in new[]{-840,0,840})yield return[row.GetProperty("id").GetString()!,row.GetRawText(),data,zone];}
 /// <summary>Executes authored proposals through real native sessions.</summary><param name="id">Case.</param><param name="json">Payload.</param><param name="data">Facade.</param><param name="zone">Implicit timezone.</param>
 [Theory][MemberData(nameof(Cases))]public void Contract(string id,string json,bool data,int zone)
 {
  using var j=JsonDocument.Parse(json);var r=j.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("proposedCompile").GetString();var assessment=r.GetProperty("proposedAssessment").GetString();var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,zone);
  if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);using var s=c.Value;if(s is null)return;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==assessment,id+":assessment:"+a.Outcome);using var d=a.Value;if(d is not null){Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));var path=r.GetProperty("site").GetString() switch{"terminal"=>ImmutableArray.Create(0),"nested"=>ImmutableArray.Create(0,0),_=>ImmutableArray<int>.Empty};var v=r.GetProperty("site").GetString()=="attribute"?d.ReadAttribute([],"","d"):d.ReadElement(path);Assert.Equal(NativeScalarKind.Date,v.Kind);}}
  else{var c=NativeSchemaSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);using var s=c.Value;if(s is null)return;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==assessment,id+":assessment:"+a.Outcome);using var d=a.Value;if(d is not null){Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));var site=r.GetProperty("site").GetString();var v=site=="attribute"?d.ReadAttribute(0).Value:site is "terminal" or "nested"?d.ReadScalar(0):d.ReadRootScalar();Assert.Equal(NativeScalarKind.Date,v.Kind);}}
 }
 /// <summary>Caller facet character limits survive named and nested compilation.</summary><param name="data">Facade.</param><param name="nested">Nested placement.</param>
 [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
 public void FacetCapacity(bool data,bool nested)
 {
  var body=nested?"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='R'/></xs:sequence></xs:complexType></xs:element>":"<xs:element name='r' type='R'/>";
  var start="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:complexType name='C'><xs:simpleContent><xs:extension base='xs:date'/></xs:simpleContent></xs:complexType><xs:complexType name='R'><xs:simpleContent><xs:restriction base='C'><xs:maxInclusive value='";
  var end="'/></xs:restriction></xs:simpleContent></xs:complexType>"+body+"</xs:schema>";var options=new NativeSchemaSessionOptions(256,256,10,65536,1,32,1,256,256);
  if(data){var bad=NativeSchemaNodeDataSession.Create(start+"2000-01-01Z"+end,options);Assert.Equal(NativeSchemaOutcome.Unsupported,bad.Outcome);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,Assert.Single(bad.Diagnostics).UnsupportedReason);using var good=NativeSchemaNodeDataSession.Create(start+"2000-01-01"+end,options).Value!;Assert.NotNull(good);}
  else{var bad=NativeSchemaSession.Create(start+"2000-01-01Z"+end,options);Assert.Equal(NativeSchemaOutcome.Unsupported,bad.Outcome);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,Assert.Single(bad.Diagnostics).UnsupportedReason);using var good=NativeSchemaSession.Create(start+"2000-01-01"+end,options).Value!;Assert.NotNull(good);}
 }

}
