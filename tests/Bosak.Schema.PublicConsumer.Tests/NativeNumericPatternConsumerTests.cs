// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies collapsed lexical patterns independently from exact numeric values.
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
using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Checks numeric lexical validity, exact values and original XML through both facades.</summary>
public sealed class NativeNumericPatternConsumerTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,2,256,16,256,1024);
    /// <summary>Gets all authored numeric contracts through both facades.</summary><returns>Original contract and facade selector.</returns>
    public static IEnumerable<object[]> Cases()
    {using var p=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","NumericPatterns","cases.json")));foreach(var r in p.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetRawText(),data];}
    /// <summary>Validates original collapsed spelling and returns complete actual typed values only after success.</summary>
    /// <param name="id">Stable case ID.</param><param name="json">Original stimulus.</param><param name="data">Facade selector.</param>
    [Theory][MemberData(nameof(Cases))]
    public void Contract(string id,string json,bool data)
    {
        using var p=JsonDocument.Parse(json);var r=p.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("compile").GetString();var outcome=r.GetProperty("outcome").GetString();var place=r.GetProperty("placement").GetString();var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);ImmutableArray<NativeScalarValue> values=[];string exported;
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);if(compile!="Valid"){Assert.Null(c.Value);return;}using var s=c.Value!;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==outcome,id+":assess:"+a.Outcome);if(outcome!="Valid"){Assert.Null(a.Value);return;}using var d=a.Value!;values=place=="attribute"?d.ReadAttributeItems([],"","a"):d.ReadElementItems(place=="leaf"?[0]:[]);exported=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ExportXml());}
        else{var c=NativeSchemaSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);if(compile!="Valid"){Assert.Null(c.Value);return;}using var s=c.Value!;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==outcome,id+":assess:"+a.Outcome);if(outcome!="Valid"){Assert.Null(a.Value);return;}using var d=a.Value!;values=place=="attribute"?d.ReadAttributeItems(0):place=="leaf"?d.ReadScalarItems(0):d.ReadRootItems();exported=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ExportXml());}
        Assert.True(XNode.DeepEquals(XElement.Parse(xml,LoadOptions.PreserveWhitespace),XElement.Parse(exported,LoadOptions.PreserveWhitespace)),id+":XML");
        if(r.TryGetProperty("kind",out var kind)){Assert.All(values,v=>Assert.Equal(kind.GetString(),v.Kind.ToString()));var expected=r.GetProperty("expected").GetString();Assert.Equal(expected,string.Join("|",values.Select(v=>v.Kind==NativeScalarKind.Integer?v.IntegerValue.ToString(CultureInfo.InvariantCulture):v.Kind==NativeScalarKind.Decimal?v.DecimalValue.ToInvariantString(1024):v.StringValue)));}
    }
}
