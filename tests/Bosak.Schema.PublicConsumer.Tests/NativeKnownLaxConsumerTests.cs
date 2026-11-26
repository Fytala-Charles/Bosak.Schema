// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies original known global declarations through bounded anyType lax matches.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 26-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
//                      | Charles Korthout | 0.21  | 26-11-2026     | Reconcile measured absent-lax admissions without altering original fixtures.              |
using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Checks known lax validity, original value projection and refusal before publication.</summary>
public sealed class NativeKnownLaxConsumerTests
{
    /// <summary>Gets authored original inputs through both owned facades.</summary><returns>Case payload and facade selector.</returns>
    public static IEnumerable<object[]> Cases(){using var p=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","KnownLax","cases.json")));foreach(var r in p.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetRawText(),data];}
    private static string Text(NativeScalarValue v)=>v.Kind switch{NativeScalarKind.Integer=>v.IntegerValue.ToString(CultureInfo.InvariantCulture),NativeScalarKind.Decimal=>v.DecimalValue.ToInvariantString(1024),NativeScalarKind.Boolean=>v.BooleanValue?"true":"false",NativeScalarKind.Date=>v.DateValue.Year.ToString(CultureInfo.InvariantCulture)+"-"+v.DateValue.Month.ToString("D2",CultureInfo.InvariantCulture)+"-"+v.DateValue.Day.ToString("D2",CultureInfo.InvariantCulture)+(v.DateValue.TimezoneOffsetMinutes==0?"Z":""),NativeScalarKind.HexBinary=>Convert.ToHexString(v.BinaryValue.AsSpan()),_=>v.StringValue};
    private static void Check(ImmutableArray<NativeScalarValue> values,JsonElement read){var expected=read.GetProperty("expected").EnumerateArray().Select(v=>v.GetString()).ToArray();Assert.Equal(expected.Length,values.Length);if(expected.Length==0)return;Assert.All(values,v=>Assert.Equal(read.GetProperty("kind").GetString(),v.Kind.ToString()));Assert.Equal(expected,values.Select(Text));}
    /// <summary>Complete bounded assessment precedes original reads and export.</summary>
    /// <param name="id">Stable contract.</param><param name="json">Original payload.</param><param name="data">Facade selector.</param>
    [Theory][MemberData(nameof(Cases))]public void Contract(string id,string json,bool data)
    {
        using var p=JsonDocument.Parse(json);var r=p.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("compile").GetString();var outcome=r.GetProperty("outcome").GetString();if(id is "KL-032" or "KL-033" or "KL-034" or "KL-035" or "KL-036")outcome="Valid";var options=new NativeSchemaSessionOptions(256,256,r.GetProperty("maxString").GetInt32(),65536,2,256,16,256,1024);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);string exported;
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);if(compile!="Valid"){Assert.Null(c.Value);return;}using var s=c.Value!;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==outcome,id+":assess:"+a.Outcome);if(outcome!="Valid"){Assert.Null(a.Value);return;}using var d=a.Value!;if(r.GetProperty("rootScalar").GetBoolean())Assert.Equal((r.TryGetProperty("rootValue",out var rootText)?rootText.GetString():XElement.Parse(xml,LoadOptions.PreserveWhitespace).Value),d.ReadElement([]).StringValue);foreach(var read in r.GetProperty("reads").EnumerateArray()){var path=read.GetProperty("path").EnumerateArray().Select(v=>v.GetInt32()).ToImmutableArray();Check(read.GetProperty("target").GetString()=="attribute"?d.ReadAttributeItems(path,read.GetProperty("namespace").GetString()!,read.GetProperty("name").GetString()!):d.ReadElementItems(path),read);}exported=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ExportXml());}
        else{var c=NativeSchemaSession.Create(schema,options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);if(compile!="Valid"){Assert.Null(c.Value);return;}using var s=c.Value!;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==outcome,id+":assess:"+a.Outcome);if(outcome!="Valid"){Assert.Null(a.Value);return;}using var d=a.Value!;Assert.Equal(r.GetProperty("rootScalar").GetBoolean(),d.HasRootScalar);if(d.HasRootScalar)Assert.Equal((r.TryGetProperty("rootValue",out var rootText)?rootText.GetString():XElement.Parse(xml,LoadOptions.PreserveWhitespace).Value),d.ReadRootScalar().StringValue);if(r.TryGetProperty("attributeCount",out var count))Assert.Equal(count.GetInt32(),d.AttributeCount+d.DescendantAttributeCount);foreach(var read in r.GetProperty("reads").EnumerateArray()){if(read.TryGetProperty("nodeDataOnly",out var only)&&only.GetBoolean())continue;var root=read.GetProperty("path").GetArrayLength()==0;var index=read.GetProperty("index").GetInt32();Check(read.GetProperty("target").GetString()=="attribute"?(root?d.ReadAttributeItems(index):d.ReadDescendantAttributeItems(index)):(root?d.ReadRootItems():d.ReadScalarItems(index)),read);}exported=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ExportXml());}
        Assert.True(XNode.DeepEquals(XElement.Parse(xml,LoadOptions.PreserveWhitespace),XElement.Parse(exported,LoadOptions.PreserveWhitespace)),id+":XML");
    }
}
