// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Tests decimal simple-content roots through the public owned facade.
// SPECIAL NOTES        : Bounded opt-in root attribute result; no transferable schema authority.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 26-11-2026     | Check measured text admissions against unmodified original fixtures |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Checks independent root decimal reads, attribute indexes, failures and detached lifetime.</summary>
public sealed class NativeSimpleContentConsumerTests
{
    /// <summary>Gets authored simple-content cases.</summary>
    /// <returns>Stable identity and JSON payload pairs.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var input=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","SimpleContent","cases.json")));
        foreach(var row in input.RootElement.EnumerateArray())yield return [row.GetProperty("id").GetString()!,row.GetRawText()];
    }
    private static string Text(NativeScalarValue v)=>v.Kind switch {NativeScalarKind.String=>v.StringValue,NativeScalarKind.Decimal=>v.DecimalValue.ToInvariantString(512),NativeScalarKind.Integer=>v.IntegerValue.ToString(CultureInfo.InvariantCulture),_=>v.BooleanValue?"true":"false"};
    /// <summary>Reads root values independently of leaves and attributes, and proves export/reopen and detached lifetime.</summary>
    /// <param name="id">Authored stable case ID.</param>
    /// <param name="payload">Schema, XML and bounded expectations.</param>
    [Theory][MemberData(nameof(Cases))]
    public void SimpleContent_SeparatesRootAndExistingIndexes(string id,string payload)
    {
        Assert.StartsWith("SC-",id);using var data=JsonDocument.Parse(payload);var r=data.RootElement;
        var options=new NativeSchemaSessionOptions(r.GetProperty("maxNativeCoefficientDigits").GetInt32(),r.GetProperty("maxNativeScale").GetInt32(),r.GetProperty("maxStringCharacters").GetInt32(),65536,1,32,1,256,256);
        if(id=="SC-051"){NativeTextLegacyVerification.Check(r.GetProperty("schema").GetString()!,r.GetProperty("instance").GetString()!,options,NativeSchemaOutcome.Valid,NativeSchemaOutcome.Valid,"12.50",root:true);return;}
        var created=NativeSchemaSession.Create(r.GetProperty("schema").GetString()!,options);Assert.Equal(r.GetProperty("expectedCompile").GetString(),created.Outcome.ToString());
        if(created.Outcome!=NativeSchemaOutcome.Valid)
        {
            Assert.Null(created.Value);Assert.Equal("BSK-NATIVE-SCHEMA-UNSUPPORTED",Assert.Single(created.Diagnostics).Code);Assert.Equal(r.GetProperty("unsupportedReason").GetString(),created.Diagnostics[0].UnsupportedReason.ToString());return;
        }
        using var s=created.Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);var xml=r.GetProperty("instance").GetString()!;
        var assessed=s.Assess(xml,context);Assert.Equal(r.GetProperty("expectedAssessment").GetString(),assessed.Outcome.ToString());
        if(assessed.Outcome!=NativeSchemaOutcome.Valid)
        {
            Assert.Null(assessed.Value);Assert.Equal("BSK-NATIVE-INSTANCE-"+assessed.Outcome.ToString().ToUpperInvariant(),Assert.Single(assessed.Diagnostics).Code);
            if(assessed.Outcome==NativeSchemaOutcome.Unsupported)Assert.Equal(r.GetProperty("unsupportedReason").GetString(),assessed.Diagnostics[0].UnsupportedReason.ToString());return;
        }
        using var d=assessed.Value!;Assert.True(d.HasRootScalar);Assert.Equal(0,d.ScalarCount);Assert.Throws<ArgumentOutOfRangeException>(()=>d.ReadScalar(0));var detached=d.ReadRootScalar();Assert.Equal(NativeScalarKind.Decimal,detached.Kind);Assert.Equal(r.GetProperty("expectedRoot").GetString(),Text(detached));
        Assert.Throws<InvalidOperationException>(()=>detached.StringValue);Assert.Throws<InvalidOperationException>(()=>detached.BooleanValue);Assert.Throws<InvalidOperationException>(()=>detached.IntegerValue);
        var attrs=r.GetProperty("attributes").EnumerateArray().ToArray();Assert.Equal(attrs.Length,d.AttributeCount);
        for(var i=0;i<attrs.Length;i++){var a=d.ReadAttribute(i);Assert.Equal(attrs[i].GetProperty("namespaceUri").GetString(),a.NamespaceUri);Assert.Equal(attrs[i].GetProperty("localName").GetString(),a.LocalName);Assert.Equal(attrs[i].GetProperty("value").GetString(),Text(a.Value));}
        var exported=d.ExportXml();Assert.True(XNode.DeepEquals(XDocument.Parse(xml,LoadOptions.PreserveWhitespace).Root,XDocument.Parse(exported,LoadOptions.PreserveWhitespace).Root));d.Dispose();using var reopened=s.Assess(exported,context).Value!;Assert.Equal(Text(detached),Text(reopened.ReadRootScalar()));Assert.Equal(attrs.Length,reopened.AttributeCount);s.Dispose();Assert.Throws<ObjectDisposedException>(()=>reopened.HasRootScalar);Assert.Throws<ObjectDisposedException>(()=>reopened.ReadRootScalar());Assert.NotNull(Text(detached));
    }
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='price'><xs:complexType><xs:simpleContent><xs:extension base='xs:decimal'><xs:attribute name='currency' type='xs:string' use='required'/></xs:extension></xs:simpleContent></xs:complexType></xs:element></xs:schema>";
    private static NativeSchemaSessionOptions Options(int query=256,int cache=8,int output=65536)=>new(256,256,256,output,1,cache,1,query,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>The exact isolated PriceType from the public fixture now assesses and retains root attributes.</summary>
    [Fact]
    public void OriginalPriceTypeSlice_WorksWithoutAcceptingWholeOrdersSchema()
    {
        var folder=Path.Combine(AppContext.BaseDirectory,"TestData","SimpleContent");using var s=NativeSchemaSession.Create(File.ReadAllText(Path.Combine(folder,"price.xsd")),Options()).Value!;using var d=s.Assess(File.ReadAllText(Path.Combine(folder,"price.xml")),Context).Value!;
        Assert.Equal("12.5",Text(d.ReadRootScalar()));Assert.Equal("EUR",d.ReadAttribute(0).Value.StringValue);Assert.Equal(0,d.ScalarCount);
        var whole=NativeSchemaSession.Create(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","Schemas","orders.xsd")),Options());Assert.Equal(NativeSchemaOutcome.Unsupported,whole.Outcome);Assert.Null(whole.Value);
    }
    /// <summary>Legacy decimal facade remains unchanged and element-only roots still report no root typed scalar.</summary>
    [Fact]
    public void LegacyAndElementOnlyRoot_KeepPriorBoundaries()
    {
        Assert.Equal(NativeDecimalOutcome.Unsupported,NativeDecimalSession.Create(Schema,new(256,256,1,8,1,256,256)).Outcome);
        var old="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='n' type='xs:decimal'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
        using var s=NativeSchemaSession.Create(old,Options()).Value!;using var d=s.Assess("<r><n>12.50</n></r>",Context).Value!;Assert.False(d.HasRootScalar);var ex=Assert.Throws<NotSupportedException>(()=>d.ReadRootScalar());Assert.Equal("FOTY0012",ex.Message);Assert.Equal(1,d.ScalarCount);Assert.Equal("12.5",Text(d.ReadScalar(0)));
    }
    /// <summary>Root read numeric capacity refusal leaves attributes and export usable, and later admission recovers.</summary>
    [Fact]
    public void RootQueryCapacity_RefusesWithoutPoisoningAttributeReads()
    {
        using var s=NativeSchemaSession.Create(Schema,Options(query:2)).Value!;using var d=s.Assess("<price currency='EUR'>123</price>",Context).Value!;
        Assert.True(d.HasRootScalar);Assert.Throws<NotSupportedException>(()=>d.ReadRootScalar());Assert.Equal("EUR",d.ReadAttribute(0).Value.StringValue);Assert.Contains("123",d.ExportXml());d.Dispose();using var next=s.Assess("<price currency='EUR'>1</price>",Context).Value!;Assert.Equal("1",Text(next.ReadRootScalar()));
    }
    /// <summary>Root and attribute data share finite cache capacity, released when the document closes.</summary>
    [Fact]
    public void RootAndAttributeCache_IsSharedAndRecovers()
    {
        using var s=NativeSchemaSession.Create(Schema,Options(cache:1)).Value!;using var d=s.Assess("<price currency='EUR'>12.50</price>",Context).Value!;
        Assert.Equal("12.5",Text(d.ReadRootScalar()));Assert.Throws<NotSupportedException>(()=>d.ReadAttribute(0));Assert.Equal("12.5",Text(d.ReadRootScalar()));Assert.Contains("EUR",d.ExportXml());d.Dispose();using var next=s.Assess("<price currency='EUR'>1</price>",Context).Value!;Assert.Equal("EUR",next.ReadAttribute(0).Value.StringValue);
    }
    /// <summary>Precancelled root reads and bounded export failure do not corrupt the owned document.</summary>
    [Fact]
    public void RootCancellationAndExportFailure_LeaveOwnerUsable()
    {
        using var s=NativeSchemaSession.Create(Schema,Options(output:1)).Value!;using var d=s.Assess("<price currency='EUR'>12.50</price>",Context).Value!;using var cts=new CancellationTokenSource();cts.Cancel();
        Assert.Throws<OperationCanceledException>(()=>d.ReadRootScalar(cts.Token));Assert.Throws<NotSupportedException>(()=>d.ExportXml());Assert.True(d.HasRootScalar);Assert.Equal("12.5",Text(d.ReadRootScalar()));
    }
}
