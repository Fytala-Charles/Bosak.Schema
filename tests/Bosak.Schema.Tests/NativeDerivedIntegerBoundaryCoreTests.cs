// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Verifies isolated derived integer values and original type evidence.
// SPECIAL NOTES        : Test-only prototype; no production facade or synthetic XML validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 22-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 24-11-2026     | Adopt measured enumeration expectations and upstream validity |
//                      | Charles Korthout | 0.3 | 26-11-2026     | Reconcile measured original numeric pattern outcomes without changing inputs |
//                      | Charles Korthout | 0.4 | 28-11-2026     | Reconcile named integer admission; preserve original inputs and test names |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================



using System.Collections.Immutable;
using System.Globalization;
using System.Numerics;
using System.Text.Json;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.DerivedIntegers;
/// <summary>Verifies inherited facets, graph selection, lifetime and original upstream controls.</summary>
public sealed class NativeDerivedIntegerBoundaryCoreTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string S(string content,string ns="")=>$"<xs:schema xmlns:xs='{X}'{ns}>{content}</xs:schema>";
    private static string Root(string type="byte")=>$"<xs:element name='r' type='xs:{type}'/>";
    private static NativeSchemaSessionOptions Options=>new(256,256,1024,65536,1,256,1,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Source Type Budget.</summary>
    /// <param name="count">The count control input.</param>
    /// <param name="expected">The expected control input.</param>
    [Theory][InlineData(16,"Valid")][InlineData(17,"Unsupported")]
    public void SourceTypeBudget(int count,string expected)
    {var definitions=string.Concat(Enumerable.Range(0,count).Select(i=>$"<xs:simpleType name='N{i}'><xs:restriction base='xs:int'/></xs:simpleType>"));var c=NativeSchemaSession.Create(S(definitions+Root("int")),Options);Assert.Equal(expected,c.Outcome.ToString());c.Value?.Dispose();}
    /// <summary>Namespace And Anonymous.</summary>
    /// <param name="anonymous">The anonymous control input.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void NamespaceAndAnonymous(bool anonymous)
    {var definition=anonymous?"":"<xs:simpleType name='N'><xs:restriction base='xs:int'/></xs:simpleType>";var root=anonymous?"<xs:element name='r'><xs:simpleType><xs:restriction base='xs:int'/></xs:simpleType></xs:element>":"<xs:element name='r' type='t:N'/>";using var f=new Fixture(S(definition+root," targetNamespace='urn:i' xmlns:t='urn:i'"),"<t:r xmlns:t='urn:i'>7</t:r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("urn:i",fact.NativeDeclaration.NamespaceUri);Assert.Equal(anonymous?null:"N",fact.NativeType.Name);Assert.Equal(anonymous?"":"urn:i",fact.NativeType.NamespaceUri);Assert.Equal("int",fact.NativeType.DerivationBase!.Name);Assert.Equal(X,fact.NativeType.DerivationBase.NamespaceUri);}
    /// <summary>Selected Original Governing Type.</summary>
    /// <param name="selected">The selected control input.</param>
    /// <param name="number">The number control input.</param>
    [Theory][InlineData("int","7")][InlineData("unsignedLong","18446744073709551615")]
    public void SelectedOriginalGoverningType(string selected,string number)
    {using var f=new Fixture(S(Root("integer")),$"<r xmlns:xs='{X}' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='xs:{selected}'>{number}</r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("integer",fact.NativeDeclaration.Type.Name);Assert.Equal(selected,fact.NativeType.Name);Assert.NotSame(fact.NativeDeclaration.Type,fact.NativeType);f.Run(op=>{var v=f.Evaluate(op,f.Input.Root);Assert.Equal(BigInteger.Parse(number,CultureInfo.InvariantCulture),v.BigIntegerValue);Assert.True(SchemaAtomics.TryGetProvenance(v,out var e));Assert.Same(fact.Type,e!.DeclaredType);Assert.Same(fact.Type,e.ActualType);return 0;});}
    /// <summary>Complex Integer Restriction.</summary>
    /// <param name="number">The number control input.</param>
    /// <param name="expected">The expected control input.</param>
    [Theory][InlineData("9","Valid")][InlineData("0","Invalid")][InlineData("10","Invalid")]
    public void ComplexIntegerRestriction(string number,string expected)
    {var schema=S("<xs:complexType name='B'><xs:simpleContent><xs:extension base='xs:int'><xs:assert test='$value gt 0'/></xs:extension></xs:simpleContent></xs:complexType><xs:complexType name='D'><xs:simpleContent><xs:restriction base='B'><xs:maxInclusive value='10'/><xs:assert test='$value lt 10'/></xs:restriction></xs:simpleContent></xs:complexType><xs:element name='r' type='D'/>");using var f=new Fixture(schema,$"<r>{number}</r>");Assert.Equal(expected,f.Assessment.Status.ToString());if(expected!="Valid"){Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);return;}var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("D",fact.NativeType.Name);Assert.Null(fact.NativeType.ContentType!.Name);Assert.Equal("int",fact.NativeType.ContentType.DerivationBase!.Name);f.Run(op=>{var v=f.Evaluate(op,f.Input.Root);Assert.True(SchemaAtomics.TryGetProvenance(v,out var e));Assert.Same(fact.Type,e!.DeclaredType);Assert.Same(f.Provider.TypeComponent(fact.NativeType.ContentType),e.ActualType);Assert.NotSame(e.DeclaredType,e.ActualType);return 0;});}
    /// <summary>Failure Recovery Cancellation And Detached Value.</summary>
    /// <param name="data">The data control input.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void FailureRecoveryCancellationAndDetachedValue(bool data)
    {var schema=S(Root());using var cancelled=new CancellationTokenSource();cancelled.Cancel();NativeScalarValue detached;
        if(data){Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaNodeDataSession.Create(schema,Options,cancelled.Token));using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>s.Assess("<r>7</r>",Context,cancelled.Token));Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r>128</r>",Context).Outcome);var d=s.Assess("<r>7</r>",Context).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadElement([],cancelled.Token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ExportXml(cancelled.Token));detached=d.ReadElement([]);var xml=d.ExportXml();Assert.Throws<InvalidOperationException>(()=>s.Assess("<r>7</r>",Context));d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadElement([]));using var copy=s.Assess(xml,Context).Value!;Assert.Equal(7,copy.ReadElement([]).IntegerValue);s.Dispose();Assert.Throws<ObjectDisposedException>(()=>copy.ReadElement([]));}
        else{Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaSession.Create(schema,Options,cancelled.Token));using var s=NativeSchemaSession.Create(schema,Options).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>s.Assess("<r>7</r>",Context,cancelled.Token));Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r>128</r>",Context).Outcome);var d=s.Assess("<r>7</r>",Context).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadRootScalar(cancelled.Token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ExportXml(cancelled.Token));detached=d.ReadRootScalar();var xml=d.ExportXml();Assert.Throws<InvalidOperationException>(()=>s.Assess("<r>7</r>",Context));d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootScalar());using var copy=s.Assess(xml,Context).Value!;Assert.Equal(7,copy.ReadRootScalar().IntegerValue);s.Dispose();Assert.Throws<ObjectDisposedException>(()=>copy.ReadRootScalar());}
        Assert.Equal(7,detached.IntegerValue);
    }
    /// <summary>Invalid No Facts And Foreign Authority.</summary>
    [Fact]
    public void InvalidNoFactsAndForeignAuthority()
    {using(var bad=new Fixture(S(Root()),"<r>128</r>")){Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);Assert.Empty(bad.Assessment.Facts);Assert.Null(bad.Assessment.Evidence);}using var a=new Fixture(S(Root()),"<r>7</r>");using var b=new Fixture(S(Root()),"<r>7</r>");var fact=a.Input.FactsFor((NativeSessionNode)a.Input.Root)!;Assert.False(b.Session.Advisor!.OwnsNode(a.Input.Root,fact,b.Input,b.Assessment.Origin));Assert.Throws<ArgumentException>(()=>b.Run(op=>b.Evaluate(op,a.Input.Root)));Assert.False(b.Session.Advisor.OwnsTypedValue(fact.ReadTypedValue(default).Value,b.Input,b.Assessment.Origin));Assert.Throws<ArgumentException>(()=>b.Provider.GetComponentClosure(fact.Type,default));Assert.Equal(SchemaResultKind.Unsupported,a.Provider.GetComponentClosure(fact.Type,default).Kind);a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));}
    /// <summary>Excluded Facet Grammar.</summary>
    /// <param name="facet">The facet control input.</param>
    /// <param name="attributes">The attributes control input.</param>
    [Theory][InlineData("enumeration","value='1'")][InlineData("pattern","value='.*'")][InlineData("whiteSpace","value='preserve'")][InlineData("fractionDigits","value='0' fixed='true'")]
    public void ExcludedFacetGrammar(string facet,string attributes)
    {var schema=S($"<xs:simpleType name='N'><xs:restriction base='xs:int'><xs:{facet} {attributes}/></xs:restriction></xs:simpleType>"+Root());var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(facet is "enumeration" or "pattern"?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Unsupported,c.Outcome);if(facet is "enumeration" or "pattern")Assert.NotNull(c.Value);else Assert.Null(c.Value);c.Value?.Dispose();}
    /// <summary>Preserves the historical test name while qualifying named integer chains.</summary>
    [Fact]
    public void UserChainsStayExcluded()
    {var schema=S("<xs:simpleType name='N'><xs:restriction base='xs:int'/></xs:simpleType><xs:simpleType name='D'><xs:restriction base='N'/></xs:simpleType>"+Root());var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var session=c.Value!;}
    /// <summary>Builtins.</summary>
    /// <returns>The explicit controls.</returns>
    public static IEnumerable<object[]> Builtins(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData/DerivedIntegers/builtin-types.json")));foreach(var row in d.RootElement.GetProperty("types").EnumerateArray())yield return[row.GetProperty("name").GetString()!];}
    /// <summary>Legacy Grammar.</summary>
    /// <param name="type">The type control input.</param>
    [Theory][MemberData(nameof(Builtins))]
    public void LegacyGrammar(string type){var c=NativeDecimalSession.Create(S(Root(type)),new(256,256,1,32,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,c.Outcome);Assert.Null(c.Value);}
    /// <summary>Originals.</summary>
    /// <returns>The explicit controls.</returns>
    public static IEnumerable<object[]> Originals(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData/DerivedIntegers/originals.json")));foreach(var row in d.RootElement.EnumerateArray())foreach(bool data in new[]{false,true})yield return[row.GetProperty("id").GetString()!,row.GetRawText(),data];}
    /// <summary>Original Public Fixtures.</summary>
    /// <param name="id">The id control input.</param>
    /// <param name="json">The json control input.</param>
    /// <param name="data">The data control input.</param>
    [Theory][MemberData(nameof(Originals))]
    public void OriginalPublicFixtures(string id,string json,bool data)
    {Assert.StartsWith("MS-DataTypes",id);using var d=JsonDocument.Parse(json);var row=d.RootElement;var schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,row.GetProperty("schema").GetString()!));var xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,row.GetProperty("instance").GetString()!));var expected=row.GetProperty("expectedCompile").GetString();var expectedAssessment=row.GetProperty("upstreamExpected").GetString()=="valid"?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Invalid;if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(expected,c.Outcome.ToString());if(c.Value is{}s){using(s)Assert.Equal(expectedAssessment,s.Assess(xml,Context).Outcome);}}else{var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(expected,c.Outcome.ToString());if(c.Value is{}s){using(s)Assert.Equal(expectedAssessment,s.Assess(xml,Context).Outcome);}}}
}
