// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Verifies isolated simple-root assessment and original scalar provenance.
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
//                      | Charles Korthout | 0.2   | 25-11-2026     | Verify admitted nil metadata and distinct supported empty typed sequences |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


using System.Collections.Immutable;
using System.Numerics;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.SimpleRoots;
/// <summary>Verifies bounded native simple-root behavior and refusal boundaries.</summary>
public sealed class NativeSimpleRootCoreTests
{
    private const string Xs="http://www.w3.org/2001/XMLSchema";
    private static NativeSchemaSessionOptions Options => new(256,256,16384,65536,1,32,1,256,256);
    private static NativeSchemaAssessmentContext Context => new(DateTimeOffset.UnixEpoch,0);
    private static string Schema(string type="string",string body="",string extra="") => $"<xs:schema xmlns:xs='{Xs}'>{extra}<xs:element name='r' type='xs:{type}'/>{body}</xs:schema>";
    /// <summary>Cases.</summary>
    /// <returns>The explicit payload controls.</returns>
    public static IEnumerable<object[]> Cases()
    {using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData/SimpleRoots/cases.json")));foreach(var row in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})yield return [row.GetProperty("id").GetString()!,row.GetRawText(),data];}
    private static void CheckValue(NativeScalarValue value,string family,string text)
    {
        switch(family)
        {case "string":Assert.Equal(NativeScalarKind.String,value.Kind);Assert.Equal(text,value.StringValue);break;
         case "boolean":Assert.Equal(NativeScalarKind.Boolean,value.Kind);Assert.Equal(text.Trim() is "true" or "1",value.BooleanValue);break;
         case "integer":Assert.Equal(NativeScalarKind.Integer,value.Kind);Assert.Equal(BigInteger.Parse(text.Trim(),System.Globalization.CultureInfo.InvariantCulture),value.IntegerValue);break;
         default:Assert.Equal(NativeScalarKind.Decimal,value.Kind);Assert.Equal(decimal.Parse(text.Trim(),System.Globalization.CultureInfo.InvariantCulture),decimal.Parse(value.DecimalValue.ToInvariantString(1024),System.Globalization.CultureInfo.InvariantCulture));break;}
    }
    /// <summary>Identity Rows.</summary>
    /// <returns>The explicit payload controls.</returns>
    public static IEnumerable<object[]> IdentityRows()=>Cases().Where(a=>!(bool)a[2]&&int.Parse(((string)a[0])[3..])<=14&&(string)a[0] is not "SR-008" and not "SR-009");
    /// <summary>Actual Original Identity.</summary>
    /// <param name="id">The id control input.</param>
    /// <param name="json">The json control input.</param>
    /// <param name="data">The data control input.</param>
    [Theory][MemberData(nameof(IdentityRows))]
    public void ActualOriginalIdentity(string id,string json,bool data)
    {
        using var plan=JsonDocument.Parse(json);var r=plan.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;Assert.StartsWith("SR-",id);Assert.False(data);var placement=r.GetProperty("placement").GetString();var family=r.GetProperty("scalarFamily").GetString()!;
        using var f=new Fixture(schema,xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var n=f.Element([]);Assert.Same(f.Input.Root,n);if(placement=="anonymous")Assert.Null(n.SchemaTypeAnnotation);else Assert.Equal((placement=="builtin"?Xs:"",placement=="builtin"?family:"S"),n.SchemaTypeAnnotation!.Value);var fact=f.Input.FactsFor((NativeSessionNode)n)!;
        Assert.Equal("r",fact.NativeDeclaration.Name);Assert.Equal("",fact.NativeDeclaration.NamespaceUri);Assert.False(fact.NativeType.IsComplex);Assert.False(fact.HasNoTypedValue);Assert.False(fact.IsNilled);
        Assert.Equal(placement=="builtin"?family:placement=="named"?"S":null,fact.NativeType.Name);Assert.Equal(placement=="builtin"?Xs:"",fact.NativeType.NamespaceUri);
        var raw=fact.ReadTypedValue(default).Value;Assert.False(raw.IsList);Assert.True(raw.TryGetSingleAtomic(out var atom));Assert.Same(fact.Type,raw.GoverningType);Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);
        Assert.True(f.Session.Advisor!.OwnsNode(n,fact,f.Input,f.Assessment.Origin));Assert.Same(fact.Declaration,f.Provider.ResolveElementDeclaration(new("","r"),default).Value);
        f.Run(op=>{var value=f.Evaluate(op,n);Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(fact.Type,evidence!.DeclaredType);Assert.Same(fact.Type,evidence.ActualType);Assert.Same(f.Assessment.Origin,evidence.OriginalAssessment);return 0;});CheckValue(f.Read(n),family,XElement.Parse(xml).Value);
    }
    /// <summary>Original Normalization.</summary>
    /// <param name="type">The type control input.</param>
    /// <param name="content">The content control input.</param>
    [Theory][InlineData("string","  A  B  ")][InlineData("integer","  -007  ")][InlineData("boolean","  1  ")]
    public void OriginalNormalization(string type,string content)
    {using var f=new Fixture(Schema(type),$"<r>{content}</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal(type=="string"?content:string.Join(" ",content.Split((char[]?)null,StringSplitOptions.RemoveEmptyEntries)),fact.NormalizedStringValue);}
    /// <summary>Empty Complex And Absent Stay Distinct.</summary>
    /// <param name="body">The body control input.</param>
    /// <param name="empty">The empty control input.</param>
    [Theory][InlineData("",true)][InlineData("<xs:sequence><xs:element name='v' type='xs:string' minOccurs='0'/></xs:sequence>",false)]
    public void EmptyComplexAndAbsentStayDistinct(string body,bool empty)
    {string schema=$"<xs:schema xmlns:xs='{Xs}'><xs:element name='r'><xs:complexType>{body}</xs:complexType></xs:element></xs:schema>";using var f=new Fixture(schema,"<r/>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal(!empty,fact.HasNoTypedValue);if(empty){var value=fact.ReadTypedValue(default).Value;Assert.False(value.TryGetSingleAtomic(out _));}if(empty){Assert.Equal(NativeScalarKind.Empty,f.Read(f.Input.Root).Kind);Assert.False(fact.IsNilled);}else Assert.Throws<InvalidOperationException>(()=>f.Read(f.Input.Root));}
    /// <summary>No Facts On Failure.</summary>
    /// <param name="xml">The xml control input.</param>
    /// <param name="expected">The expected control input.</param>
    [Theory][InlineData("<r><bad/></r>","Invalid")][InlineData("<r a='x'>abc</r>","Unsupported")]
    public void NoFactsOnFailure(string xml,string expected)
    {using var f=new Fixture(Schema(),xml);Assert.Equal(expected,f.Assessment.Status.ToString());Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Assessment.Evidence);Assert.Null(f.Snapshot);}
    /// <summary>Failure Recovery And Lifetime.</summary>
    /// <param name="data">The data control input.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void FailureRecoveryAndLifetime(bool data)
    {
        var schema=Schema();NativeScalarValue detached;
        if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r><bad/></r>",Context).Outcome);var d=s.Assess("<r> a </r>",Context).Value!;Assert.Throws<InvalidOperationException>(()=>s.Assess("<r>b</r>",Context));detached=d.ReadElement([]);var exported=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadElement([]));using var reopened=s.Assess(exported,Context).Value!;Assert.Equal(" a ",reopened.ReadElement([]).StringValue);s.Dispose();Assert.Throws<ObjectDisposedException>(()=>reopened.ReadElement([]));}
        else{using var s=NativeSchemaSession.Create(schema,Options).Value!;Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r><bad/></r>",Context).Outcome);var d=s.Assess("<r> a </r>",Context).Value!;Assert.Throws<InvalidOperationException>(()=>s.Assess("<r>b</r>",Context));detached=d.ReadRootScalar();var exported=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootScalar());using var reopened=s.Assess(exported,Context).Value!;Assert.Equal(" a ",reopened.ReadRootScalar().StringValue);s.Dispose();Assert.Throws<ObjectDisposedException>(()=>reopened.ReadRootScalar());}
        Assert.Equal(" a ",detached.StringValue);
    }
    /// <summary>Foreign Authority And Disposal.</summary>
    [Fact]
    public void ForeignAuthorityAndDisposal()
    {using var a=new Fixture(Schema(),"<r>abc</r>");using var b=new Fixture(Schema(),"<r>abc</r>");var fact=a.Input.FactsFor((NativeSessionNode)a.Input.Root)!;Assert.False(b.Session.Advisor!.OwnsNode(a.Input.Root,fact,b.Input,b.Assessment.Origin));Assert.Throws<ArgumentException>(()=>b.Run(op=>b.Evaluate(op,a.Input.Root)));var value=fact.ReadTypedValue(default).Value;Assert.False(b.Session.Advisor.OwnsTypedValue(value,b.Input,b.Assessment.Origin));a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));}
    /// <summary>Cancellation And Recovery.</summary>
    /// <param name="data">The data control input.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void CancellationAndRecovery(bool data)
    {using var c=new CancellationTokenSource();c.Cancel();if(data){Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaNodeDataSession.Create(Schema(),Options,c.Token));using var s=NativeSchemaNodeDataSession.Create(Schema(),Options).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>s.Assess("<r>a</r>",Context,c.Token));using var d=s.Assess("<r>a</r>",Context).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadElement([],c.Token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ExportXml(c.Token));Assert.Equal("a",d.ReadElement([]).StringValue);}else{Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaSession.Create(Schema(),Options,c.Token));using var s=NativeSchemaSession.Create(Schema(),Options).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>s.Assess("<r>a</r>",Context,c.Token));using var d=s.Assess("<r>a</r>",Context).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadRootScalar(c.Token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ExportXml(c.Token));Assert.Equal("a",d.ReadRootScalar().StringValue);}}
    /// <summary>Legacy Still Refuses.</summary>
    /// <param name="type">The type control input.</param>
    [Theory][InlineData("decimal")][InlineData("integer")][InlineData("boolean")][InlineData("string")]
    public void LegacyStillRefuses(string type)
    {var result=NativeDecimalSession.Create(Schema(type),new(256,256,1,32,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);}
    /// <summary>Original Sku Still Refuses.</summary>
    [Fact]
    public void OriginalSkuStillRefuses()
    {var schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData/Schemas/sku.xsd"));Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(schema,Options).Outcome);Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaNodeDataSession.Create(schema,Options).Outcome);}
}
