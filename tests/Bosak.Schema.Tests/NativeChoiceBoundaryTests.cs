// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 November 2026
// PURPOSE              : Verifies isolated choice outcomes, physical original facts and omission semantics.
// SPECIAL NOTES        : Test-only prototype; no production facade or synthetic XML validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 23-11-2026     | Adopt18 measured all compile expectation changes |
//                      | Charles Korthout | 0.3   | 24-11-2026     | Adopt measured enumeration expectations and upstream validity |
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
namespace Bosak.Schema.Tests.NodeData;
/// <summary>Verifies bounded choice identity, capacity, authority and lifetime.</summary>
public sealed class NativeChoiceBoundaryTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(string model,string extra="",string suffix="")=>$"<xs:schema xmlns:xs='{X}'>{extra}<xs:element name='r'><xs:complexType>{model}{suffix}</xs:complexType></xs:element></xs:schema>";
    private static string Basic=>"<xs:choice minOccurs='0'><xs:element name='a' type='xs:string'/><xs:element name='b' type='xs:int'/></xs:choice>";
    private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,1,256,1,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Worst Alternative Budget.</summary>
    /// <param name="nested">The nested control input.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void WorstAlternativeBudget(bool nested)
    {string members=string.Concat(Enumerable.Range(0,16).Select(i=>$"<xs:element name='n{i}' type='xs:string' maxOccurs='16'/>"));var extra=nested?"<xs:element name='other'><xs:complexType><xs:sequence><xs:element name='box'><xs:complexType><xs:sequence><xs:element name='v' type='xs:int'/></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element>":"";using var s=NativeSchemaSession.Create(Schema("<xs:choice>"+members+"</xs:choice>",extra),Options).Value!;using var d=s.Assess("<r>"+string.Concat(Enumerable.Repeat("<n15>x</n15>",16))+"</r>",Context).Value!;Assert.Equal(16,d.ScalarCount);Assert.Equal("x",d.ReadScalar(15).StringValue);}
    /// <summary>Branch Count Boundary.</summary>
    /// <param name="count">The count control input.</param>
    /// <param name="expected">The expected control input.</param>
    [Theory][InlineData(16,"Valid")][InlineData(17,"Unsupported")]
    public void BranchCountBoundary(int count,string expected)
    {var c=NativeSchemaSession.Create(Schema("<xs:choice>"+string.Concat(Enumerable.Range(0,count).Select(i=>$"<xs:element name='n{i}' type='xs:string'/>"))+"</xs:choice>"),Options);Assert.Equal(expected,c.Outcome.ToString());c.Value?.Dispose();}
    /// <summary>Anonymous Selected Facet.</summary>
    /// <param name="pattern">The pattern control input.</param>
    [Theory][InlineData("a+")][InlineData("[a-z]+")]
    public void AnonymousSelectedFacet(string pattern)
    {var model="<xs:choice><xs:element name='a' type='xs:int'/><xs:element name='b'><xs:simpleType><xs:restriction base='xs:string'><xs:pattern value='"+pattern+"'/></xs:restriction></xs:simpleType></xs:element></xs:choice>";using var f=new Fixture(Schema(model),"<r><b>aaa</b></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var node=f.Element([0]);var fact=f.Input.FactsFor((NativeSessionNode)node)!;Assert.Equal("b",fact.NativeDeclaration.Name);Assert.Null(fact.NativeType.Name);Assert.Null(node.SchemaTypeAnnotation);Assert.Equal("aaa",f.Read(node).StringValue);}
    /// <summary>Eager Excluded Unselected Type.</summary>
    /// <param name="facet">The facet control input.</param>
    /// <param name="attr">The attr control input.</param>
    [Theory][InlineData("enumeration","value='x'")][InlineData("pattern","value='\\p{L}'")]
    public void EagerExcludedUnselectedType(string facet,string attr)
    {var unused=$"<xs:simpleType name='N'><xs:restriction base='xs:integer'><xs:{facet} {attr}/></xs:restriction></xs:simpleType>";var c=NativeSchemaSession.Create(Schema(Basic,unused),Options);Assert.Equal(facet=="enumeration"?NativeSchemaOutcome.Invalid:NativeSchemaOutcome.Unsupported,c.Outcome);Assert.Null(c.Value);}
    /// <summary>Eager Malformed Unused Pattern.</summary>
    [Fact]
    public void EagerMalformedUnusedPattern()
    {var unused="<xs:simpleType name='N'><xs:restriction base='xs:string'><xs:pattern value='('/></xs:restriction></xs:simpleType>";Assert.Equal(NativeSchemaOutcome.Invalid,NativeSchemaSession.Create(Schema(Basic,unused),Options).Outcome);}
    /// <summary>Root Attributes And Physical Owner.</summary>
    /// <param name="named">The named control input.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void RootAttributesAndPhysicalOwner(bool named)
    {var suffix="<xs:attribute name='id' type='xs:unsignedLong' use='required'/>";var schema=Schema(Basic,suffix:suffix);if(named)schema=$"<xs:schema xmlns:xs='{X}'><xs:complexType name='C'>{Basic}{suffix}</xs:complexType><xs:element name='r' type='C'/></xs:schema>";using var f=new Fixture(schema,"<r id='18446744073709551615'><b>7</b></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var leaf=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;Assert.Equal("b",leaf.NativeDeclaration.Name);var attribute=f.Attribute([],"","id");var fact=f.Input.FactsFor((NativeSessionNode)attribute)!;Assert.True(fact.OwnerElementPath.IsEmpty);Assert.Equal("unsignedLong",fact.NativeType.Name);Assert.Equal("18446744073709551615",f.Read(attribute).IntegerValue.ToString(System.Globalization.CultureInfo.InvariantCulture));}
    /// <summary>Structural Namespace Assertion.</summary>
    /// <param name="valid">The valid control input.</param>
    [Theory][InlineData(true)][InlineData(false)]
    public void StructuralNamespaceAssertion(bool valid)
    {var expected=valid?"":"urn:wrong";var suffix="<xs:assert test=\"namespace-uri(.) eq '"+expected+"'\"/>";using var f=new Fixture(Schema(Basic,suffix:suffix),"<r><b>7</b></r>");Assert.Equal(valid?SchemaAssessmentStatus.Valid:SchemaAssessmentStatus.Invalid,f.Assessment.Status);if(!valid){Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);}}
    /// <summary>Cancellation Recovery And Lifetime.</summary>
    /// <param name="data">The data control input.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void CancellationRecoveryAndLifetime(bool data)
    {using var c=new CancellationTokenSource();c.Cancel();NativeScalarValue detached;var schema=Schema(Basic);
        if(data){Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaNodeDataSession.Create(schema,Options,c.Token));using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>s.Assess("<r><b>7</b></r>",Context,c.Token));Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r><b>bad</b></r>",Context).Outcome);var d=s.Assess("<r><b>7</b></r>",Context).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadElement([0],c.Token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ExportXml(c.Token));detached=d.ReadElement([0]);Assert.Throws<InvalidOperationException>(()=>s.Assess("<r/>",Context));var xml=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadElement([0]));using var reopened=s.Assess(xml,Context).Value!;Assert.Equal(7,reopened.ReadElement([0]).IntegerValue);s.Dispose();Assert.Throws<ObjectDisposedException>(()=>reopened.ReadElement([0]));}
        else{Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaSession.Create(schema,Options,c.Token));using var s=NativeSchemaSession.Create(schema,Options).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>s.Assess("<r><b>7</b></r>",Context,c.Token));Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r><b>bad</b></r>",Context).Outcome);var d=s.Assess("<r><b>7</b></r>",Context).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadScalar(0,c.Token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ExportXml(c.Token));detached=d.ReadScalar(0);Assert.Throws<InvalidOperationException>(()=>s.Assess("<r/>",Context));var xml=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadScalar(0));using var reopened=s.Assess(xml,Context).Value!;Assert.Equal(7,reopened.ReadScalar(0).IntegerValue);s.Dispose();Assert.Throws<ObjectDisposedException>(()=>reopened.ReadScalar(0));}
        Assert.Equal(7,detached.IntegerValue);
    }
    /// <summary>Invalid No Facts And Foreign Authority.</summary>
    [Fact]
    public void InvalidNoFactsAndForeignAuthority()
    {using(var bad=new Fixture(Schema(Basic),"<r><a>x</a><b>7</b></r>")){Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);Assert.Empty(bad.Assessment.Facts);Assert.Null(bad.Assessment.Evidence);}using var a=new Fixture(Schema(Basic),"<r><b>7</b></r>");using var b=new Fixture(Schema(Basic),"<r><b>7</b></r>");var node=a.Element([0]);var fact=a.Input.FactsFor((NativeSessionNode)node)!;Assert.False(b.Session.Advisor!.OwnsNode(node,fact,b.Input,b.Assessment.Origin));Assert.Throws<ArgumentException>(()=>b.Run(op=>b.Evaluate(op,node)));Assert.False(b.Session.Advisor.OwnsTypedValue(fact.ReadTypedValue(default).Value,b.Input,b.Assessment.Origin));a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));}
    /// <summary>Shared Pattern Budget Refusal And Recovery.</summary>
    [Fact]
    public void SharedPatternBudgetRefusalAndRecovery()
    {var wide="("+string.Join("|",Enumerable.Repeat("a",62))+")*";var extra="<xs:simpleType name='W'><xs:restriction base='xs:string'><xs:pattern value='"+wide+"'/></xs:restriction></xs:simpleType>";var model="<xs:choice><xs:element name='a' type='W' maxOccurs='2'/><xs:element name='b' type='xs:int'/></xs:choice>";using var s=NativeSchemaSession.Create(Schema(model,extra),Options).Value!;string value=new('a',8000);var a=s.Assess("<r><a>"+value+"</a><a>"+value+"</a></r>",Context);Assert.Equal(NativeSchemaOutcome.Unsupported,a.Outcome);Assert.Null(a.Value);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,Assert.Single(a.Diagnostics).UnsupportedReason);using var d=s.Assess("<r><b>7</b></r>",Context).Value!;Assert.Equal(7,d.ReadScalar(0).IntegerValue);}
    /// <summary>Legacy Choice Refusal.</summary>
    /// <param name="named">The named control input.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void LegacyChoiceRefusal(bool named)
    {var schema=named?$"<xs:schema xmlns:xs='{X}'><xs:complexType name='C'>{Basic}</xs:complexType><xs:element name='r' type='C'/></xs:schema>":Schema(Basic);Assert.Equal(NativeDecimalOutcome.Unsupported,NativeDecimalSession.Create(schema,new(256,256,1,32,1,256,256)).Outcome);}
    /// <summary>All Plans.</summary>
    /// <returns>The explicit controls.</returns>
    public static IEnumerable<object[]> AllPlans(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"case-plan.json")));foreach(var r in d.RootElement.GetProperty("cases").EnumerateArray().Where(x=>x.GetProperty("phase").GetString()=="all-second"))yield return[r.GetProperty("id").GetString()!,r.GetProperty("schema").GetString()!];}
    /// <summary>Checks original all-phase stimuli against explicitly adopted compile outcomes.</summary>
    /// <remarks>The historical method name and inputs are retained to preserve case identity.</remarks>
    /// <param name="id">The id control input.</param>
    /// <param name="schema">The schema control input.</param>
    [Theory][MemberData(nameof(AllPlans))]
    public void AllRemainsExcluded(string id,string schema)
    {Assert.StartsWith("PT-",id);using var plan=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"case-plan.json")));var expected=plan.RootElement.GetProperty("cases").EnumerateArray().Single(r=>r.GetProperty("phase").GetString()=="all-second"&&r.GetProperty("id").GetString()==id).GetProperty("expectedCompile").GetString();var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(expected,c.Outcome.ToString());if(expected=="Valid")Assert.NotNull(c.Value);else Assert.Null(c.Value);c.Value?.Dispose();}
    /// <summary>Gets exact original co-blocked upstream schemas for both facades.</summary>
    /// <returns>Original identifiers, schemas and facade selector.</returns>
    public static IEnumerable<object[]> Originals(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"originals.json")));foreach(var r in d.RootElement.EnumerateArray())foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetProperty("schema").GetString()!,data];}
    /// <summary>Probes unchanged originals without rewriting implicit anyType or unsupported grammar.</summary>
    /// <param name="id">Original upstream identity.</param><param name="file">Exact schema payload.</param><param name="data">Selects facade.</param>
    [Theory][MemberData(nameof(Originals))]
    public void OriginalCoBlocker(string id,string file,bool data)
    {Assert.NotEmpty(id);var schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,file));if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Unsupported,c.Outcome);Assert.Null(c.Value);}else{var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Unsupported,c.Outcome);Assert.Null(c.Value);}}
}
