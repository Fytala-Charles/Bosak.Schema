// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Qualifies isolated bounded date enumeration and exact value equality.
// SPECIAL NOTES        : Isolated experimental source; not production adoption.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 24-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Text.Json;
using System.Collections.Immutable;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.DateEnumerationExperiment;
/// <summary>Exercises isolated date enumeration with actual native inputs.</summary>
public sealed class EnumerationTests
{
 private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,2,256,1,256,1024);
 /// <summary>Gets authored cases on both facades and three timezones.</summary><returns>Unmodified payloads.</returns>
 public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"case-plan.json")));foreach(var row in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})foreach(int zone in new[]{-840,0,840})yield return[row.GetProperty("id").GetString()!,row.GetRawText(),data,zone];}
 /// <summary>Executes authored proposals through real native sessions.</summary><param name="id">Case.</param><param name="json">Payload.</param><param name="data">Facade.</param><param name="zone">Implicit timezone.</param>
 [Theory][MemberData(nameof(Cases))]public void Contract(string id,string json,bool data,int zone)
 {
  using var j=JsonDocument.Parse(json);var r=j.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("proposedCompile").GetString();var assessment=r.GetProperty("proposedAssessment").GetString();var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,zone);
  if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);using var s=c.Value;if(s is null)return;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==assessment,id+":assessment:"+a.Outcome);using var d=a.Value;if(d is not null){Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));var path=r.GetProperty("site").GetString() switch{"terminal"=>ImmutableArray.Create(0),"nested"=>ImmutableArray.Create(0,0),_=>ImmutableArray<int>.Empty};var v=r.GetProperty("site").GetString()=="attribute"?d.ReadAttribute([],"","d"):d.ReadElement(path);Assert.Equal(NativeScalarKind.Date,v.Kind);}}
  else{var c=NativeSchemaSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);using var s=c.Value;if(s is null)return;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==assessment,id+":assessment:"+a.Outcome);using var d=a.Value;if(d is not null){Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));var site=r.GetProperty("site").GetString();var v=site=="attribute"?d.ReadAttribute(0).Value:site is "terminal" or "nested"?d.ReadScalar(0):d.ReadRootScalar();Assert.Equal(NativeScalarKind.Date,v.Kind);}}
 }
 private static string Schema=>"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='D'><xs:restriction base='xs:date'><xs:enumeration value='2000-01-01+13:00'/></xs:restriction></xs:simpleType><xs:element name='r' type='D'/></xs:schema>";
 /// <summary>Preserves actual original-node facts and nonidentical equal values.</summary>
 [Fact]public void OriginalFactsAndLifetime()
 {
  using var f=new Fixture(Schema,"<r>1999-12-31-11:00</r>");using var other=new Fixture(Schema,"<r>2000-01-01+13:00</r>");var node=f.Input.Root;var fact=f.Input.FactsFor((NativeSessionNode)node)!;Assert.Equal("D",fact.NativeType.Name);Assert.Equal("r",fact.NativeDeclaration.Name);var v=f.Read(node);Assert.Equal(1999,v.DateValue.Year);Assert.Equal(12,v.DateValue.Month);Assert.Equal(31,v.DateValue.Day);Assert.Equal(-660,v.DateValue.TimezoneOffsetMinutes);Assert.False(other.Session.Advisor!.OwnsNode(node,fact,other.Input,other.Assessment.Origin));Assert.Throws<ArgumentException>(()=>other.Run(op=>other.Evaluate(op,node)));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));Assert.Equal(-660,v.DateValue.TimezoneOffsetMinutes);
 }
 /// <summary>Equality keys match across offsets without fixed-width overflow.</summary><param name="a">First date.</param><param name="b">Second date.</param><param name="equal">Expected equality.</param>
 [Theory][InlineData("2000-01-01+13:00","1999-12-31-11:00",true)][InlineData("0000-01-01+13:00","-0001-12-31-11:00",true)][InlineData("2000-03-01+14:00","2000-02-29-10:00",true)][InlineData("2000-01-01","2000-01-01Z",false)][InlineData("2000-01-01+14:00","1999-12-31-14:00",false)][InlineData("9223372036854775807-12-31+14:00","9223372036854775807-12-30-10:00",true)][InlineData("-9223372036854775807-01-02+14:00","-9223372036854775807-01-01-10:00",true)]
 public void ExactKeys(string a,string b,bool equal){var x=NativeDateFacets.Key(NativeDateParser.Parse(a));var y=NativeDateFacets.Key(NativeDateParser.Parse(b));Assert.Equal(equal,x==y);Assert.Equal(equal,y==x);}
 /// <summary>Invalid member assessment has no successful partial native state.</summary>
 [Fact]public void InvalidHasNoFacts(){using var f=new Fixture(Schema,"<r>2001-01-01Z</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);Assert.False(f.Input.IsAssessed);}
 /// <summary>Cancellation remains distinct from date membership.</summary>
 [Fact]public void Cancellation(){using var s=NativeSchemaSession.Create(Schema,Options).Value!;using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>s.Assess("<r>1999-12-31-11:00</r>",new(DateTimeOffset.UnixEpoch,0),c.Token));}
 /// <summary>Checks raw facet and instance capacities without changing primitive dates.</summary><param name="length">Raw characters.</param><param name="allowed">Within capacity.</param>
 [Theory][InlineData(1024,true)][InlineData(1025,false)]public void RawCapacity(int length,bool allowed)
 {
  const string date="2000-01-01+13:00";var padded=new string(' ',length-date.Length)+date;
  using var s=NativeSchemaSession.Create(Schema,new(256,256,1024,65536,2,256,1,256,1024)).Value!;var a=s.Assess("<r>"+padded+"</r>",new(DateTimeOffset.UnixEpoch,0));Assert.Equal(allowed?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Unsupported,a.Outcome);a.Value?.Dispose();
  var schema=Schema.Replace("value='"+date+"'","value='"+padded+"'");var c=NativeSchemaSession.Create(schema,new(256,256,1024,65536,2,256,1,256,1024));Assert.Equal(allowed?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Unsupported,c.Outcome);c.Value?.Dispose();
 }
 /// <summary>Checks descendant attribute ownership and restricted complex content.</summary>
 [Fact]public void DescendantAttributes()
 {
  var schema=Schema.Replace("<xs:element name='r' type='D'/>","<xs:complexType name='C'><xs:simpleContent><xs:extension base='D'><xs:attribute name='stamp' type='D' use='required'/></xs:extension></xs:simpleContent></xs:complexType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='C'/></xs:sequence></xs:complexType></xs:element>");
  using var f=new Fixture(schema,"<r><v stamp='1999-12-31-11:00'>2000-01-01+13:00</v></r>");var e=f.Element([0]);var a=f.Attribute([0],"","stamp");Assert.Equal(2000,f.Read(e).DateValue.Year);Assert.Equal(1999,f.Read(a).DateValue.Year);var fact=f.Input.FactsFor((NativeSessionNode)e)!;Assert.Equal("C",fact.NativeType.Name);Assert.Equal("D",fact.NativeType.ContentType!.Name);var attr=f.Input.FactsFor((NativeSessionNode)a)!;Assert.Equal(new[]{0},attr.OwnerElementPath.ToArray());
  using var s=NativeSchemaSession.Create(schema,Options).Value!;var bad=s.Assess("<r><v stamp='2001-01-01'>2000-01-01+13:00</v></r>",new(DateTimeOffset.UnixEpoch,0));Assert.Equal(NativeSchemaOutcome.Invalid,bad.Outcome);
 }

 /// <summary>Runs unchanged upstream inputs on both facades; comments are retained.</summary><param name="index">Original.</param><param name="data">Facade.</param>
 [Theory][InlineData(0,false)][InlineData(0,true)][InlineData(1,false)][InlineData(1,true)]public void Originals(int index,bool data)
 {
  var schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"original-"+index+".xsd"));var xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"original-"+index+".xml"));var expected=index==0?NativeSchemaOutcome.Invalid:NativeSchemaOutcome.Valid;Assert.Contains("<!--",xml);
  if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;var a=s.Assess(xml,new(DateTimeOffset.UnixEpoch,840));Assert.Equal(expected,a.Outcome);using var d=a.Value;if(d is not null)Assert.Equal(1999,d.ReadElement([0]).DateValue.Year);}
  else{using var s=NativeSchemaSession.Create(schema,Options).Value!;var a=s.Assess(xml,new(DateTimeOffset.UnixEpoch,-840));Assert.Equal(expected,a.Outcome);using var d=a.Value;if(d is not null)Assert.Equal(1999,d.ReadScalar(0).DateValue.Year);}
  if(index==1){using var f=new Fixture(schema,xml);var leaf=f.Element([0]);Assert.Equal(1999,f.Read(leaf).DateValue.Year);Assert.Null(f.Input.FactsFor((NativeSessionNode)leaf)!.NativeType.Name);}
 }
 /// <summary>Leaf comment refusal remains separate from container comments.</summary><param name="data">Facade.</param>
 [Theory][InlineData(false)][InlineData(true)]public void LeafComments(bool data)
 {
  const string xml="<r><!--kept-->1999-12-31-11:00</r>";
  if(data){using var s=NativeSchemaNodeDataSession.Create(Schema,Options).Value!;Assert.Equal(NativeSchemaOutcome.Unsupported,s.Assess(xml,new(DateTimeOffset.UnixEpoch,0)).Outcome);}
  else{using var s=NativeSchemaSession.Create(Schema,Options).Value!;Assert.Equal(NativeSchemaOutcome.Unsupported,s.Assess(xml,new(DateTimeOffset.UnixEpoch,0)).Outcome);}
 }

 /// <summary>Schema formatting whitespace does not become unsupported facet markup.</summary>
 [Fact]public void FacetFormattingWhitespace(){var schema=Schema.Replace("value='2000-01-01+13:00'/>","value='2000-01-01+13:00'> "+"\n"+" </xs:enumeration>");using var session=NativeSchemaSession.Create(schema,Options).Value!;Assert.NotNull(session);var a=session.Assess("<r>1999-12-31-11:00</r>",new(DateTimeOffset.UnixEpoch,0));Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);a.Value!.Dispose();}

}
