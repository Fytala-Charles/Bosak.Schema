// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Exercises isolated date extension outcomes and original-node identities.
// SPECIAL NOTES        : Isolated native experiment; no substitute XML validator.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 24-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 24-11-2026     | Sequence original-node reads and extend native controls |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.DateExtensionExperiment;
/// <summary>Exercises real native date simple content.</summary>
public sealed class ExtensionTests
{
 private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,1,256,1,256,256);
 private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,120);
 private const string Start="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>";
 private const string Schema=Start+"<xs:complexType name='C'><xs:simpleContent><xs:extension base='xs:date'/></xs:simpleContent></xs:complexType><xs:element name='r' type='C'/></xs:schema>";
 /// <summary>Gets both-facade authored contract cases.</summary><returns>Cases.</returns>
 public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"case-plan.json")));foreach(var r in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetRawText(),data];}
 private static IEnumerable<XElement> Dates(XElement root)=>root.HasElements?root.Descendants().Where(e=>!e.HasElements&&e.Value.Length>0&&e.Value!="keep"):new[]{root};
 private static ImmutableArray<int> PathOf(XElement e){var a=new List<int>();while(e.Parent is { } p){a.Insert(0,p.Elements().TakeWhile(n=>!ReferenceEquals(n,e)).Count());e=p;}return a.ToImmutableArray();}
 private static void Value(NativeScalarValue v,XElement e){Assert.Equal(NativeScalarKind.Date,v.Kind);var d=v.DateValue;var text=e.Value.Trim(' ','\t','\r','\n');if(text.StartsWith("0000",StringComparison.Ordinal)){Assert.Equal(0,d.Year);Assert.Equal(2,d.Month);Assert.Equal(29,d.Day);}if(text.StartsWith("10000",StringComparison.Ordinal))Assert.Equal(10000,d.Year);if(!text.EndsWith("Z",StringComparison.Ordinal)&&!text.EndsWith("+14:00",StringComparison.Ordinal)&&!text.EndsWith("-14:00",StringComparison.Ordinal)&&!text.EndsWith("+00:00",StringComparison.Ordinal)&&!text.EndsWith("-00:00",StringComparison.Ordinal))Assert.Null(d.TimezoneOffsetMinutes);}
 /// <summary>Executes original authored payloads through public facades.</summary><param name="id">Case.</param><param name="json">Payload.</param><param name="data">Facade.</param>
 [Theory][MemberData(nameof(Cases))]
 public void Contract(string id,string json,bool data)
 {
  Assert.StartsWith("DSC-",id);using var parsed=JsonDocument.Parse(json);var r=parsed.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compile=Enum.Parse<NativeSchemaOutcome>(r.GetProperty("proposedCompile").GetString()!);
  if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.True(compile==c.Outcome,id+":"+JsonSerializer.Serialize(c.Diagnostics));using var s=c.Value;if(s is null)return;var a=s.Assess(xml,Context);Assert.Equal(r.GetProperty("proposedAssessment").GetString(),a.Outcome.ToString());using var d=a.Value;if(d is null)return;var root=XElement.Parse(xml);foreach(var e in Dates(root))Value(d.ReadElement(PathOf(e)),e);Assert.True(XNode.DeepEquals(root,XElement.Parse(d.ExportXml())));}
  else{var c=NativeSchemaSession.Create(schema,Options);Assert.True(compile==c.Outcome,id+":"+JsonSerializer.Serialize(c.Diagnostics));using var s=c.Value;if(s is null)return;var a=s.Assess(xml,Context);Assert.Equal(r.GetProperty("proposedAssessment").GetString(),a.Outcome.ToString());using var d=a.Value;if(d is null)return;var root=XElement.Parse(xml);Assert.Equal(!root.HasElements,d.HasRootScalar);if(!root.HasElements){Assert.Equal(0,d.ScalarCount);Value(d.ReadRootScalar(),root);}else{var leaves=root.Descendants().Where(e=>!e.HasElements&&!string.IsNullOrEmpty(e.Value)).ToArray();for(int i=0;i<leaves.Length;i++)if(leaves[i].Value!="keep")Value(d.ReadScalar(i),leaves[i]);}Assert.True(XNode.DeepEquals(root,XElement.Parse(d.ExportXml())));}
 }
 /// <summary>Gets uniquely labelled original-node positive controls.</summary><returns>Cases.</returns>
 public static IEnumerable<object[]> Identities(){foreach(var r in Cases())if(!(bool)r[2]){using var d=JsonDocument.Parse((string)r[1]);var j=d.RootElement;if(j.GetProperty("proposedCompile").GetString()=="Valid"&&j.GetProperty("proposedAssessment").GetString()=="Valid")yield return[r[0],r[1]];}}
 /// <summary>Checks actual D/T/G/A and atomic provenance, not synthetic type labels.</summary><param name="id">Case.</param><param name="json">Payload.</param>
 [Theory][MemberData(nameof(Identities))]
 public void ActualIdentity(string id,string json)
 {
  Assert.StartsWith("DSC-",id);using var parsed=JsonDocument.Parse(json);var r=parsed.RootElement;var xml=r.GetProperty("instance").GetString()!;using var f=new Fixture(r.GetProperty("schema").GetString()!,xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var root=XElement.Parse(xml);
  foreach(var e in Dates(root)){var node=f.Element(PathOf(e));var fact=f.Input.FactsFor((NativeSessionNode)node)!;Assert.Equal(e.Name.LocalName,fact.NativeDeclaration.Name);Assert.Equal(e.Name.NamespaceName,fact.NativeDeclaration.NamespaceUri);Assert.True(fact.NativeType.IsComplex);Assert.False(fact.HasNoTypedValue);Assert.NotNull(fact.NativeType.ContentType);Assert.True(fact.NativeType.ContentType!.IsDate);Assert.Same(fact.NativeDeclaration.Type,fact.NativeType);var raw=fact.ReadTypedValue(default).Value;Assert.True(raw.TryGetSingleAtomic(out var a));Assert.Equal(XdmValueKind.Date,a.Value.Kind);var atomic=f.Provider.TypeComponent(fact.NativeType.ContentType);Assert.NotSame(fact.Type,atomic);Assert.Same(atomic,a.TypeEvidence.ActualType);Assert.Same(fact.Type,raw.GoverningType);var detached=f.Read(node);f.Run(op=>{var v=f.Evaluate(op,node);Assert.Equal(XdmValueKind.Date,v.Kind);Assert.True(SchemaAtomics.TryGetProvenance(v,out var evidence));Assert.Same(fact.Type,evidence!.DeclaredType);Assert.Same(atomic,evidence.ActualType);Assert.Same(f.Assessment.Origin,evidence.OriginalAssessment);Assert.Equal(v.DateXPathValue.Year,detached.DateValue.Year);return 0;});}
 }
 /// <summary>Checks cancellation, recovery, reopened value and detached lifetime.</summary><param name="data">Facade.</param>
 [Theory][InlineData(false)][InlineData(true)]public void Lifetime(bool data)
 {
  using var cancel=new CancellationTokenSource();cancel.Cancel();NativeDateValue detached;
  if(data){Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaNodeDataSession.Create(Schema,Options,cancel.Token));using var s=NativeSchemaNodeDataSession.Create(Schema,Options).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>s.Assess("<r>0000-02-29</r>",Context,cancel.Token));Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r>1900-02-29</r>",Context).Outcome);using var d=s.Assess("<r>0000-02-29</r>",Context).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadElement([],cancel.Token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ExportXml(cancel.Token));detached=d.ReadElement([]).DateValue;var xml=d.ExportXml();Assert.Throws<InvalidOperationException>(()=>s.Assess(xml,Context));d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadElement([]));using var again=s.Assess(xml,Context).Value!;s.Dispose();Assert.Throws<ObjectDisposedException>(()=>again.ReadElement([]));}
  else{Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaSession.Create(Schema,Options,cancel.Token));using var s=NativeSchemaSession.Create(Schema,Options).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>s.Assess("<r>0000-02-29</r>",Context,cancel.Token));Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r>1900-02-29</r>",Context).Outcome);using var d=s.Assess("<r>0000-02-29</r>",Context).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadRootScalar(cancel.Token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ExportXml(cancel.Token));detached=d.ReadRootScalar().DateValue;var xml=d.ExportXml();Assert.Throws<InvalidOperationException>(()=>s.Assess(xml,Context));d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootScalar());using var again=s.Assess(xml,Context).Value!;s.Dispose();Assert.Throws<ObjectDisposedException>(()=>again.ReadRootScalar());}
  Assert.Equal(0,detached.Year);Assert.Null(detached.TimezoneOffsetMinutes);
 }
 /// <summary>Invalid ordinary closure and foreign origins expose no partial facts or authority.</summary>
 [Fact]public void FailureAndAuthority(){using(var bad=new Fixture(Schema,"<r extra='x'>0000-02-29</r>")){Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.Empty(bad.Assessment.Facts);Assert.Null(bad.Assessment.Evidence);Assert.False(bad.Input.IsAssessed);}using var a=new Fixture(Schema,"<r>0000-02-29</r>");using var b=new Fixture(Schema,"<r>0000-02-29</r>");var node=a.Input.Root;var fact=a.Input.FactsFor((NativeSessionNode)node)!;Assert.False(b.Session.Advisor!.OwnsNode(node,fact,b.Input,b.Assessment.Origin));Assert.Throws<ArgumentException>(()=>b.Run(op=>b.Evaluate(op,node)));var detached=a.Read(node).DateValue;a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));Assert.Equal(0,detached.Year);}
 /// <summary>Older scalar/decimal empty-use grammar and public enum remain unchanged.</summary>
 [Fact]public void LegacyAndOldClosure(){Assert.Equal(NativeDecimalOutcome.Unsupported,NativeDecimalSession.Create(Schema,new(256,256,1,32,1,256,256)).Outcome);using var s=NativeSchemaSession.Create(Start+"<xs:element name='r' type='xs:date'/></xs:schema>",Options).Value!;Assert.Equal(NativeSchemaOutcome.Unsupported,s.Assess("<r extra='x'>0000-02-29</r>",Context).Outcome);Assert.Equal(5,(int)NativeScalarKind.Date);}

 /// <summary>Gets two unchanged original upstream trials on both facades.</summary><returns>Cases.</returns>
 public static IEnumerable<object[]> Originals(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"originals.json")));foreach(var r in d.RootElement.GetProperty("rows").EnumerateArray())foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetRawText(),data];}
 /// <summary>Preserves original validities and checks actual primitive content identity.</summary><param name="id">Original.</param><param name="json">Record.</param><param name="data">Facade.</param>
 [Theory][MemberData(nameof(Originals))]public void OriginalValidity(string id,string json,bool data)
 {
  Assert.StartsWith("CType/",id);using var p=JsonDocument.Parse(json);var r=p.RootElement;var schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,r.GetProperty("schemaFile").GetString()!));var xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,r.GetProperty("instanceFile").GetString()!));var expected=r.GetProperty("upstreamExpected").GetString()=="valid"?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Invalid;
  if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;var a=s.Assess(xml,Context);Assert.Equal(expected,a.Outcome);using var d=a.Value;if(d is not null){Assert.Equal(NativeScalarKind.Date,d.ReadElement([]).Kind);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}}
  else{using var s=NativeSchemaSession.Create(schema,Options).Value!;var a=s.Assess(xml,Context);Assert.Equal(expected,a.Outcome);using var d=a.Value;if(d is not null){Assert.True(d.HasRootScalar);Assert.Equal(0,d.ScalarCount);Assert.Equal(NativeScalarKind.Date,d.ReadRootScalar().Kind);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}}
  using var f=new Fixture(schema,xml);Assert.Equal(expected==NativeSchemaOutcome.Valid?SchemaAssessmentStatus.Valid:SchemaAssessmentStatus.Invalid,f.Assessment.Status);if(expected==NativeSchemaOutcome.Valid){var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("A",fact.NativeType.Name);Assert.Equal("date",fact.NativeType.ContentType!.Name);Assert.Equal("contentType",fact.NativeDeclaration.NamespaceUri);Assert.Equal(NativeScalarKind.Date,f.Read(f.Input.Root).Kind);}
 }
 /// <summary>Checks typed attributes, shared complex types and original owner paths.</summary>
 [Fact]public void AttributesAndSharedOwners()
 {
  var schema=Start+"<xs:complexType name='C'><xs:simpleContent><xs:extension base='xs:date'><xs:attribute name='label' type='xs:string'/><xs:attribute name='stamp' type='xs:date'/><xs:attribute name='n' type='xs:integer'/><xs:attribute name='b' type='xs:boolean'/></xs:extension></xs:simpleContent></xs:complexType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='left' type='C'/><xs:element name='right' type='C'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
  var xml="<r><left b='true' n='+001' stamp='10000-01-01Z' label='L'>0000-02-29</left><right label='R'>0000-02-29</right></r>";using var f=new Fixture(schema,xml);var left=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;var right=f.Input.FactsFor((NativeSessionNode)f.Element([1]))!;Assert.Same(left.NativeType,right.NativeType);Assert.NotSame(left.NativeDeclaration,right.NativeDeclaration);Assert.Equal("L",f.Read(f.Attribute([0],"","label")).StringValue);Assert.Equal("R",f.Read(f.Attribute([1],"","label")).StringValue);Assert.Equal(10000,f.Read(f.Attribute([0],"","stamp")).DateValue.Year);Assert.Equal(1,f.Read(f.Attribute([0],"","n")).IntegerValue);Assert.True(f.Read(f.Attribute([0],"","b")).BooleanValue);var attribute=f.Input.FactsFor((NativeSessionNode)f.Attribute([1],"","label"))!;Assert.Equal(new[]{1},attribute.OwnerElementPath.ToArray());Assert.Same(right.Declaration,f.Input.FactsFor((NativeSessionNode)f.Element([1]))!.Declaration);
 }

 /// <summary>Checks typed public descendant attribute reads without friend access in the calls.</summary><param name="data">Facade.</param>
 [Theory][InlineData(false)][InlineData(true)]public void PublicAttributeReaders(bool data)
 {
  var schema=Start+"<xs:complexType name='C'><xs:simpleContent><xs:extension base='xs:date'><xs:attribute name='label' type='xs:string'/><xs:attribute name='stamp' type='xs:date'/><xs:attribute name='n' type='xs:integer'/><xs:attribute name='b' type='xs:boolean'/></xs:extension></xs:simpleContent></xs:complexType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='left' type='C'/><xs:element name='right' type='C'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
  var xml="<r><left b='true' n='+001' stamp='10000-01-01Z' label='L'>0000-02-29</left><right label='R'>0000-02-29</right></r>";
  if(data){using var session=NativeSchemaNodeDataSession.Create(schema,Options).Value!;using var d=session.Assess(xml,Context).Value!;Assert.Equal("L",d.ReadAttribute([0],"","label").StringValue);Assert.Equal("R",d.ReadAttribute([1],"","label").StringValue);Assert.Equal(10000,d.ReadAttribute([0],"","stamp").DateValue.Year);Assert.Equal(1,d.ReadAttribute([0],"","n").IntegerValue);Assert.True(d.ReadAttribute([0],"","b").BooleanValue);Assert.Equal(0,d.ReadElement([1]).DateValue.Year);}
  else{using var session=NativeSchemaSession.Create(schema,Options).Value!;using var d=session.Assess(xml,Context).Value!;Assert.False(d.HasRootScalar);Assert.Equal(2,d.ScalarCount);Assert.Equal("L",d.ReadDescendantAttribute(0).Value.StringValue);Assert.Equal(10000,d.ReadDescendantAttribute(1).Value.DateValue.Year);Assert.Equal(1,d.ReadDescendantAttribute(2).Value.IntegerValue);Assert.True(d.ReadDescendantAttribute(3).Value.BooleanValue);Assert.Equal("R",d.ReadDescendantAttribute(4).Value.StringValue);Assert.Equal(0,d.ReadScalar(1).DateValue.Year);}
 }
}
