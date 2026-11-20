// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Tests root attributes through the public facade while preserving leaf indexes.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Checks complete root attribute assessment, detached reads and independent leaf indexing.</summary>
public sealed class NativeAttributeConsumerTests
{
    /// <summary>Gets authored case IDs, schemas, instances and exact expectations.</summary>
    /// <returns>Case ID and serialized payload pairs.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var input=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","Attributes","m2e1-cases.json")));
        foreach(var row in input.RootElement.EnumerateArray())yield return [row.GetProperty("id").GetString()!,row.GetRawText()];
    }
    private static string Text(NativeScalarValue value)=>value.Kind switch
    {
        NativeScalarKind.String=>value.StringValue,
        NativeScalarKind.Decimal=>value.DecimalValue.ToInvariantString(256),
        NativeScalarKind.Boolean=>value.BooleanValue?"true":"false",
        NativeScalarKind.Integer=>value.IntegerValue.ToString(CultureInfo.InvariantCulture),
        _=>throw new NotSupportedException()
    };
    /// <summary>Checks all outcomes, declaration-order attribute indexing, original leaf indexes and export/lifetime.</summary>
    /// <param name="id">Stable authored identity.</param>
    /// <param name="payload">Schema, instance and bounded expectations.</param>
    [Theory][MemberData(nameof(Cases))]
    public void RootAttributes_ValidateCompletelyWithoutChangingLeafIndexes(string id,string payload)
    {
        Assert.StartsWith("M2E1-",id);using var data=JsonDocument.Parse(payload);var row=data.RootElement;
        var options=new NativeSchemaSessionOptions(row.GetProperty("maxNativeCoefficientDigits").GetInt32(),row.GetProperty("maxNativeScale").GetInt32(),row.GetProperty("maxStringCharacters").GetInt32(),65536,1,64,1,256,256);
        var created=NativeSchemaSession.Create(row.GetProperty("schema").GetString()!,options);Assert.Equal(row.GetProperty("expectedCompile").GetString(),created.Outcome.ToString());
        if(created.Outcome!=NativeSchemaOutcome.Valid)
        {
            Assert.Null(created.Value);Assert.Equal("BSK-NATIVE-SCHEMA-UNSUPPORTED",Assert.Single(created.Diagnostics).Code);Assert.Equal(row.GetProperty("unsupportedReason").GetString(),created.Diagnostics[0].UnsupportedReason.ToString());return;
        }
        using var session=created.Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);var xml=row.GetProperty("instance").GetString()!;
        var assessed=session.Assess(xml,context);Assert.Equal(row.GetProperty("expectedAssessment").GetString(),assessed.Outcome.ToString());
        if(assessed.Outcome!=NativeSchemaOutcome.Valid)
        {
            Assert.Null(assessed.Value);Assert.Single(assessed.Diagnostics);Assert.Equal("BSK-NATIVE-INSTANCE-"+assessed.Outcome.ToString().ToUpperInvariant(),assessed.Diagnostics[0].Code);
            if(assessed.Outcome==NativeSchemaOutcome.Unsupported)Assert.Equal(row.GetProperty("unsupportedReason").GetString(),assessed.Diagnostics[0].UnsupportedReason.ToString());return;
        }
        using var document=assessed.Value!;var expected=row.GetProperty("attributes").EnumerateArray().ToArray();var leaves=row.GetProperty("leaves").EnumerateArray().ToArray();
        Assert.Equal(expected.Length,document.AttributeCount);Assert.Equal(leaves.Length,document.ScalarCount);var detached=new List<NativeAttributeValue>();
        for(var i=0;i<leaves.Length;i++){var leaf=document.ReadScalar(i);Assert.Equal(leaves[i].GetProperty("kind").GetString(),leaf.Kind.ToString());Assert.Equal(leaves[i].GetProperty("value").GetString(),Text(leaf));}
        for(var i=0;i<expected.Length;i++)
        {
            var a=document.ReadAttribute(i);detached.Add(a);Assert.Equal(expected[i].GetProperty("namespaceUri").GetString(),a.NamespaceUri);Assert.Equal(expected[i].GetProperty("localName").GetString(),a.LocalName);
            Assert.Equal(expected[i].GetProperty("kind").GetString(),a.Value.Kind.ToString());Assert.Equal(expected[i].GetProperty("value").GetString(),Text(a.Value));
            if(a.Value.Kind!=NativeScalarKind.String)Assert.Throws<InvalidOperationException>(()=>a.Value.StringValue);
            if(a.Value.Kind!=NativeScalarKind.Decimal)Assert.Throws<InvalidOperationException>(()=>a.Value.DecimalValue);
            if(a.Value.Kind!=NativeScalarKind.Boolean)Assert.Throws<InvalidOperationException>(()=>a.Value.BooleanValue);
            if(a.Value.Kind!=NativeScalarKind.Integer)Assert.Throws<InvalidOperationException>(()=>a.Value.IntegerValue);
        }
        Assert.Throws<ArgumentOutOfRangeException>(()=>document.ReadAttribute(-1));Assert.Throws<ArgumentOutOfRangeException>(()=>document.ReadAttribute(expected.Length));
        var exported=document.ExportXml();Assert.True(XNode.DeepEquals(XDocument.Parse(xml,LoadOptions.PreserveWhitespace).Root,XDocument.Parse(exported,LoadOptions.PreserveWhitespace).Root));
        document.Dispose();Assert.Throws<ObjectDisposedException>(()=>document.AttributeCount);using var reopened=session.Assess(exported,context).Value!;
        Assert.Equal(detached.Count,reopened.AttributeCount);Assert.Equal(leaves.Length,reopened.ScalarCount);
        for(var i=0;i<detached.Count;i++)Assert.Equal(Text(detached[i].Value),Text(reopened.ReadAttribute(i).Value));
        session.Dispose();Assert.Throws<ObjectDisposedException>(()=>reopened.ReadAttribute(0));foreach(var a in detached)Assert.NotNull(Text(a.Value));
    }

    private const string Simple="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='n' type='xs:decimal' minOccurs='0' maxOccurs='2'/></xs:sequence><xs:attribute name='code' type='xs:integer' use='required'/></xs:complexType></xs:element></xs:schema>";
    private static NativeSchemaSessionOptions Options(int queryDigits=256,int export=65536,int cache=8)=>new(256,256,256,export,1,cache,1,queryDigits,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Legacy decimal entry and producer-only detached value rules remain intact.</summary>
    [Fact]
    public void LegacyAndProducerOnlyAttributeValue_RemainBounded()
    {
        var old=NativeDecimalSession.Create(Simple,new(256,256,1,8,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,old.Outcome);Assert.Null(old.Value);
        Assert.Empty(typeof(NativeAttributeValue).GetConstructors());Assert.All(typeof(NativeAttributeValue).GetProperties(),p=>Assert.False(p.CanWrite));
        using var s=NativeSchemaSession.Create(Simple,Options()).Value!;using var d=s.Assess("<r code='0'/>",Context).Value!;Assert.Equal(0,d.ScalarCount);Assert.Equal(1,d.AttributeCount);Assert.Equal("0",Text(d.ReadAttribute(0).Value));Assert.Throws<ArgumentOutOfRangeException>(()=>d.ReadScalar(0));
    }
    /// <summary>Attributes do not occupy repeated leaf indexes or share their read positions.</summary>
    [Fact]
    public void RepeatedLeaves_KeepTheirOwnIndexes()
    {
        using var s=NativeSchemaSession.Create(Simple,Options()).Value!;using var d=s.Assess("<r code='7'><n>1</n><n>2</n></r>",Context).Value!;
        Assert.Equal(2,d.ScalarCount);Assert.Equal(1,d.AttributeCount);Assert.Equal("1",Text(d.ReadScalar(0)));Assert.Equal("2",Text(d.ReadScalar(1)));Assert.Equal("7",Text(d.ReadAttribute(0).Value));
    }
    /// <summary>Attribute numeric read refusal does not poison export, leaf reads or later admission.</summary>
    [Fact]
    public void QueryCapacity_RecoversAfterAttributeReadRefusal()
    {
        using var s=NativeSchemaSession.Create(Simple,Options(queryDigits:2)).Value!;using var d=s.Assess("<r code='123'><n>1</n></r>",Context).Value!;
        Assert.Throws<NotSupportedException>(()=>d.ReadAttribute(0));Assert.Equal("1",Text(d.ReadScalar(0)));Assert.Contains("123",d.ExportXml());d.Dispose();
        using var next=s.Assess("<r code='1'/>",Context).Value!;Assert.Equal("1",Text(next.ReadAttribute(0).Value));
    }
    /// <summary>Precancelled reads leave attribute documents usable and do not consume admission.</summary>
    [Fact]
    public void Cancellation_LeavesAttributeDocumentUsable()
    {
        using var s=NativeSchemaSession.Create(Simple,Options()).Value!;using var d=s.Assess("<r code='1'/>",Context).Value!;using var cts=new CancellationTokenSource();cts.Cancel();
        Assert.Throws<OperationCanceledException>(()=>d.ReadAttribute(0,cts.Token));Assert.Equal(1,d.AttributeCount);Assert.Equal("1",Text(d.ReadAttribute(0).Value));
    }
    /// <summary>Output capacity failure leaves attributes accessible under the same owner gate.</summary>
    [Fact]
    public void BoundedExportRefusal_DoesNotPoisonAttributes()
    {
        using var s=NativeSchemaSession.Create(Simple,Options(export:1)).Value!;using var d=s.Assess("<r code='1'/>",Context).Value!;
        Assert.Throws<NotSupportedException>(()=>d.ExportXml());Assert.Equal("1",Text(d.ReadAttribute(0).Value));
    }
    /// <summary>Existing roots without attribute uses retain their supported leaf-only profile and refusals.</summary>
    [Fact]
    public void AttributeFreeRoot_RetainsPriorRefusalAndZeroCount()
    {
        var plain=Simple.Replace("<xs:attribute name='code' type='xs:integer' use='required'/>","");using var s=NativeSchemaSession.Create(plain,Options()).Value!;
        using var d=s.Assess("<r><n>1</n></r>",Context).Value!;Assert.Equal(0,d.AttributeCount);Assert.Equal(1,d.ScalarCount);d.Dispose();
        var bad=s.Assess("<r code='1'/>",Context);Assert.Equal(NativeSchemaOutcome.Unsupported,bad.Outcome);Assert.Equal(NativeSchemaUnsupportedReason.Attributes,Assert.Single(bad.Diagnostics).UnsupportedReason);
    }
    /// <summary>Leaf and attribute values consume the same finite snapshot scalar cache and release it on close.</summary>
    [Fact]
    public void SharedCacheCapacity_CountsAttributeAndLeafValues()
    {
        using var s=NativeSchemaSession.Create(Simple,Options(cache:1)).Value!;using var d=s.Assess("<r code='7'><n>1</n></r>",Context).Value!;
        Assert.Equal("1",Text(d.ReadScalar(0)));Assert.Throws<NotSupportedException>(()=>d.ReadAttribute(0));Assert.Equal("1",Text(d.ReadScalar(0)));Assert.Contains("code",d.ExportXml());d.Dispose();
        using var next=s.Assess("<r code='7'/>",Context).Value!;Assert.Equal("7",Text(next.ReadAttribute(0).Value));
    }

}
