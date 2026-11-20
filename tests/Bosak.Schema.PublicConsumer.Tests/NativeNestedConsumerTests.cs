// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Checks nested sequences, terminal scalar projections and original typed facts.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Checks nested assessment and owned scalar reads without friend access.</summary>
public sealed class NativeNestedConsumerTests
{
    private static NativeSchemaSessionOptions Options(int cache=32,int export=65536,int queryDigits=256)=>new(256,256,1024,export,1,cache,1,queryDigits,256);
    private static string Fixture(string name)=>File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","NestedSequences",name));
    private static NativeSchemaAssessmentContext Context()=>new(DateTimeOffset.UnixEpoch,0);
    private static string Text(NativeScalarValue v)=>v.Kind switch {NativeScalarKind.String=>v.StringValue,NativeScalarKind.Decimal=>v.DecimalValue.ToInvariantString(512),NativeScalarKind.Integer=>v.IntegerValue.ToString(CultureInfo.InvariantCulture),_=>v.BooleanValue?"true":"false"};
    /// <summary>Gets authored clause/profile cases.</summary>
    /// <returns>Case identity and exact JSON payload.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var doc=JsonDocument.Parse(Fixture("cases.json"));foreach(var row in doc.RootElement.EnumerateArray())yield return [row.GetProperty("id").GetString()!,row.GetRawText()];
    }
    /// <summary>Assesses the complete tree and reads only original terminal scalar values.</summary>
    /// <param name="id">Authored case ID.</param>
    /// <param name="payload">Complete schema, XML and expected outcomes.</param>
    [Theory][MemberData(nameof(Cases))]
    public void NestedCase_ValidatesAndProjectsTerminals(string id,string payload)
    {
        Assert.StartsWith("NS-",id);using var parsed=JsonDocument.Parse(payload);var r=parsed.RootElement;
        var created=NativeSchemaSession.Create(r.GetProperty("schema").GetString()!,Options());Assert.Equal(r.GetProperty("expectedCompile").GetString(),created.Outcome.ToString());
        if(created.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(created.Value);Assert.Equal("BSK-NATIVE-SCHEMA-UNSUPPORTED",Assert.Single(created.Diagnostics).Code);return;}
        using var session=created.Value!;var xml=r.GetProperty("instance").GetString()!;var result=session.Assess(xml,Context());Assert.Equal(r.GetProperty("expectedAssessment").GetString(),result.Outcome.ToString());
        if(result.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(result.Value);Assert.Equal("BSK-NATIVE-INSTANCE-"+result.Outcome.ToString().ToUpperInvariant(),Assert.Single(result.Diagnostics).Code);return;}
        using var d=result.Value!;var expected=r.GetProperty("values").EnumerateArray().Select(v=>v.GetString()!).ToArray();
        Assert.False(d.HasRootScalar);Assert.Throws<NotSupportedException>(()=>d.ReadRootScalar());Assert.Equal(expected.Length,d.ScalarCount);Assert.Equal(r.GetProperty("attributes").GetInt32(),d.AttributeCount);
        var values=Enumerable.Range(0,d.ScalarCount).Select(i=>d.ReadScalar(i)).ToArray();Assert.Equal(expected,values.Select(Text));
        Assert.Throws<ArgumentOutOfRangeException>(()=>d.ReadScalar(-1));Assert.Throws<ArgumentOutOfRangeException>(()=>d.ReadScalar(d.ScalarCount));
        var exported=d.ExportXml();Assert.True(XNode.DeepEquals(XDocument.Parse(xml,LoadOptions.PreserveWhitespace).Root,XDocument.Parse(exported,LoadOptions.PreserveWhitespace).Root));
        d.Dispose();using var reopened=session.Assess(exported,Context()).Value!;Assert.Equal(expected,Enumerable.Range(0,reopened.ScalarCount).Select(i=>Text(reopened.ReadScalar(i))));session.Dispose();Assert.Equal(expected,values.Select(Text));Assert.Throws<ObjectDisposedException>(()=>reopened.ScalarCount);
    }
    /// <summary>Deep failure publishes no document and returns admission capacity.</summary>
    [Fact]
    public void DeepFailureAndCancellation_RecoverAdmission()
    {
        using var s=NativeSchemaSession.Create(Fixture("nested-order.xsd"),Options()).Value!;var xml=Fixture("nested-order.xml");
        var refused=s.Assess(xml.Replace("3.75","bad",StringComparison.Ordinal),Context());Assert.Equal(NativeSchemaOutcome.Invalid,refused.Outcome);Assert.Null(refused.Value);
        using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>s.Assess(xml,Context(),c.Token));
        using var d=s.Assess(xml,Context()).Value!;Assert.Equal(7,d.ScalarCount);Assert.Equal("ORDER-1",d.ReadAttribute(0).Value.StringValue);
    }
    /// <summary>Nested leaves share existing cache capacity while smaller reads/export remain usable.</summary>
    [Fact]
    public void CacheCapacity_RefusalDoesNotPoisonEarlierReads()
    {
        using var s=NativeSchemaSession.Create(Fixture("nested-order.xsd"),Options(cache:1)).Value!;using var d=s.Assess(Fixture("nested-order.xml"),Context()).Value!;
        Assert.Equal("ABC-0001",d.ReadScalar(0).StringValue);Assert.Throws<NotSupportedException>(()=>d.ReadScalar(1));Assert.Equal("ABC-0001",d.ReadScalar(0).StringValue);Assert.Contains("12.50",d.ExportXml());
    }
    /// <summary>Failed bounded export and cancelled reads leave the admitted document usable.</summary>
    [Fact]
    public void FailedExportAndCancelledRead_KeepScalarUsable()
    {
        using var s=NativeSchemaSession.Create(Fixture("nested-order.xsd"),Options(export:20)).Value!;using var d=s.Assess(Fixture("nested-order.xml"),Context()).Value!;
        Assert.Throws<NotSupportedException>(()=>d.ExportXml());using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>d.ReadScalar(2,c.Token));Assert.Equal("12.5",d.ReadScalar(2).DecimalValue.ToInvariantString(256));
    }
    /// <summary>A nested value can pass representation admission yet refuse a narrower query without poisoning other reads.</summary>
    [Fact]
    public void NarrowQueryCapacity_LeavesExportAndSmallerReadUsable()
    {
        using var session=NativeSchemaSession.Create(Fixture("nested-order.xsd"),Options(queryDigits:4)).Value!;
        var xml=Fixture("nested-order.xml").Replace("<quantity>2</quantity>","<quantity>12345678901234567890123456789012345678901234567890</quantity>",StringComparison.Ordinal);
        using var d=session.Assess(xml,Context()).Value!;Assert.Throws<NotSupportedException>(()=>d.ReadScalar(1));Assert.Equal("12.5",d.ReadScalar(2).DecimalValue.ToInvariantString(256));Assert.Contains("12345678901234567890123456789012345678901234567890",d.ExportXml());
    }
}
