// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Checks scalar-bearing complex terminals, original owner paths and unchanged root reads.
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

/// <summary>Checks adopted nested attributes using only public consumer APIs.</summary>
public sealed class NativeNestedSimpleContentConsumerTests
{
    private static string Fixture(string name)=>File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","NestedSimpleContent",name));
    private static NativeSchemaSessionOptions Options(int cache=128,int export=65536,int digits=256)=>new(256,256,1024,export,1,cache,1,digits,256);
    private static NativeSchemaAssessmentContext Context()=>new(DateTimeOffset.UnixEpoch,0);
    private static string Text(NativeScalarValue v)=>v.Kind switch {NativeScalarKind.String=>v.StringValue,NativeScalarKind.Decimal=>v.DecimalValue.ToInvariantString(512),NativeScalarKind.Integer=>v.IntegerValue.ToString(CultureInfo.InvariantCulture),_=>v.BooleanValue?"true":"false"};
    /// <summary>Gets authored clause/profile-mapped nested attribute cases.</summary>
    /// <returns>Stable case identities and exact schema/instance payloads.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var doc=JsonDocument.Parse(Fixture("cases.json"));foreach(var row in doc.RootElement.EnumerateArray())yield return [row.GetProperty("id").GetString()!,row.GetRawText()];
    }
    /// <summary>Reads exact detached owner paths and values while preserving scalar/root indexing and lexical export.</summary>
    /// <param name="id">Authored case identity.</param>
    /// <param name="payload">Schema, XML and expected outcomes/values.</param>
    [Theory][MemberData(nameof(Cases))]
    public void NestedSimpleContent_AssessOriginalOwnersAndPreserveIndexes(string id,string payload)
    {
        Assert.StartsWith("NC-",id);using var parsed=JsonDocument.Parse(payload);var r=parsed.RootElement;var created=NativeSchemaSession.Create(r.GetProperty("schema").GetString()!,Options());Assert.Equal(r.GetProperty("expectedCompile").GetString(),created.Outcome.ToString());
        if(created.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(created.Value);Assert.Equal("BSK-NATIVE-SCHEMA-UNSUPPORTED",Assert.Single(created.Diagnostics).Code);return;}
        using var s=created.Value!;var xml=r.GetProperty("instance").GetString()!;var assessed=s.Assess(xml,Context());Assert.Equal(r.GetProperty("expectedAssessment").GetString(),assessed.Outcome.ToString());
        if(assessed.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(assessed.Value);Assert.Equal("BSK-NATIVE-INSTANCE-"+assessed.Outcome.ToString().ToUpperInvariant(),Assert.Single(assessed.Diagnostics).Code);return;}
        using var d=assessed.Value!;var scalars=r.GetProperty("values").EnumerateArray().Select(v=>v.GetString()!).ToArray();var attrs=r.GetProperty("attributes").EnumerateArray().ToArray();Assert.Equal(scalars.Length,d.ScalarCount);Assert.Equal(scalars,Enumerable.Range(0,d.ScalarCount).Select(i=>Text(d.ReadScalar(i))));Assert.Equal(r.GetProperty("rootAttributes").GetInt32(),d.AttributeCount);Assert.Equal(attrs.Length,d.DescendantAttributeCount);Assert.False(d.HasRootScalar);
        for(var i=0;i<d.AttributeCount;i++){var a=d.ReadAttribute(i);Assert.False(a.OwnerElementPath.IsDefault);Assert.Empty(a.OwnerElementPath);}
        var values=new List<NativeAttributeValue>();
        for(var i=0;i<attrs.Length;i++)
        {
            var a=d.ReadDescendantAttribute(i);values.Add(a);Assert.False(a.OwnerElementPath.IsDefault);Assert.NotEmpty(a.OwnerElementPath);Assert.Equal(attrs[i].GetProperty("path").EnumerateArray().Select(n=>n.GetInt32()),a.OwnerElementPath);Assert.Equal(attrs[i].GetProperty("namespace").GetString(),a.NamespaceUri);Assert.Equal(attrs[i].GetProperty("name").GetString(),a.LocalName);Assert.Equal(attrs[i].GetProperty("value").GetString(),Text(a.Value));
            var owner=XDocument.Parse(xml).Root!;foreach(var index in a.OwnerElementPath)owner=owner.Elements().ElementAt(index);Assert.NotNull(owner.Attribute(XName.Get(a.LocalName,a.NamespaceUri)));
        }
        Assert.Throws<ArgumentOutOfRangeException>(()=>d.ReadDescendantAttribute(-1));Assert.Throws<ArgumentOutOfRangeException>(()=>d.ReadDescendantAttribute(d.DescendantAttributeCount));
        var exported=d.ExportXml();Assert.True(XNode.DeepEquals(XDocument.Parse(xml,LoadOptions.PreserveWhitespace).Root,XDocument.Parse(exported,LoadOptions.PreserveWhitespace).Root));d.Dispose();using var reopened=s.Assess(exported,Context()).Value!;Assert.Equal(attrs.Length,reopened.DescendantAttributeCount);
        for(var i=0;i<values.Count;i++){Assert.Equal(values[i].OwnerElementPath.ToArray(),reopened.ReadDescendantAttribute(i).OwnerElementPath.ToArray());Assert.Equal(Text(values[i].Value),Text(reopened.ReadDescendantAttribute(i).Value));}
        s.Dispose();Assert.Throws<ObjectDisposedException>(()=>reopened.DescendantAttributeCount);foreach(var a in values){Assert.False(a.OwnerElementPath.IsDefault);Assert.NotNull(Text(a.Value));}
    }
    /// <summary>Missing deep required attributes publish no document or consumed admission.</summary>
    [Fact]
    public void DeepAttributeFailureAndCancellation_RecoverAdmission()
    {
        using var s=NativeSchemaSession.Create(Fixture("nested-order.xsd"),Options()).Value!;var xml=Fixture("nested-order.xml");var bad=s.Assess(xml.Replace(" currency=\"USD\"","",StringComparison.Ordinal),Context());Assert.Equal(NativeSchemaOutcome.Invalid,bad.Outcome);Assert.Null(bad.Value);
        using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>s.Assess(xml,Context(),c.Token));using var d=s.Assess(xml,Context()).Value!;Assert.Equal(2,d.DescendantAttributeCount);Assert.Equal("USD",d.ReadDescendantAttribute(1).Value.StringValue);
    }
    /// <summary>Root, terminal and descendant reads share the same bounded original scalar cache.</summary>
    [Fact]
    public void DescendantCache_RefusesWithoutPoisoningCachedValueOrExport()
    {
        using var s=NativeSchemaSession.Create(Fixture("nested-order.xsd"),Options(cache:1)).Value!;using var d=s.Assess(Fixture("nested-order.xml"),Context()).Value!;
        Assert.Equal("EUR",d.ReadDescendantAttribute(0).Value.StringValue);Assert.Throws<NotSupportedException>(()=>d.ReadAttribute(0));Assert.Throws<NotSupportedException>(()=>d.ReadScalar(0));Assert.Equal("EUR",d.ReadDescendantAttribute(0).Value.StringValue);Assert.Contains("USD",d.ExportXml());
    }
    /// <summary>Cancelled reads and failed bounded export leave owner-aware reads usable.</summary>
    [Fact]
    public void FailedExportAndCancelledRead_DoNotPoisonOwnerFacts()
    {
        using var s=NativeSchemaSession.Create(Fixture("nested-order.xsd"),Options(export:20)).Value!;using var d=s.Assess(Fixture("nested-order.xml"),Context()).Value!;Assert.Throws<NotSupportedException>(()=>d.ExportXml());using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>d.ReadDescendantAttribute(0,c.Token));Assert.Equal("EUR",d.ReadDescendantAttribute(0).Value.StringValue);
    }
    /// <summary>Independent query digit limits can refuse a descendant value without poisoning smaller reads or export.</summary>
    [Fact]
    public void NarrowQueryLimits_KeepOtherOwnerReadsUsable()
    {
        var xml=Fixture("nested-order.xml").Replace("12.50","12345678901234567890123456789012345678901234567890",StringComparison.Ordinal);
        using var session=NativeSchemaSession.Create(Fixture("nested-order.xsd"),Options(digits:4)).Value!;using var d=session.Assess(xml,Context()).Value!;Assert.Throws<NotSupportedException>(()=>d.ReadScalar(2));Assert.Equal("3.75",d.ReadScalar(5).DecimalValue.ToInvariantString(256));Assert.Equal("EUR",d.ReadDescendantAttribute(0).Value.StringValue);Assert.Contains("12345678901234567890123456789012345678901234567890",d.ExportXml());
    }
    /// <summary>One named PriceType supports local scalar reads and an independent global root scalar.</summary>
    [Fact]
    public void SameNamedPriceType_RootReadRemainsSeparate()
    {
        using var session=NativeSchemaSession.Create(Fixture("nested-order.xsd"),Options()).Value!;
        using(var order=session.Assess(Fixture("nested-order.xml"),Context()).Value!){Assert.Equal(7,order.ScalarCount);Assert.False(order.HasRootScalar);Assert.Equal("12.5",order.ReadScalar(2).DecimalValue.ToInvariantString(256));}
        using var root=session.Assess("<p:price xmlns:p='urn:bosak:nested-plan' currency='EUR'>2.50</p:price>",Context()).Value!;Assert.True(root.HasRootScalar);Assert.Equal(0,root.ScalarCount);Assert.Equal("2.5",root.ReadRootScalar().DecimalValue.ToInvariantString(256));Assert.Equal(0,root.DescendantAttributeCount);Assert.Empty(root.ReadAttribute(0).OwnerElementPath);
    }
}
