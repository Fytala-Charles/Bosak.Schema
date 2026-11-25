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
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


using System.Collections.Immutable;
using System.Numerics;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Verifies derived builtin integer values through both public native facades.</summary>
public sealed class NativeDerivedIntegerConsumerTests
{
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private static NativeSchemaSessionOptions Options(int digits=256)=>new(digits,256,1024,65536,1,256,1,256,256);
    /// <summary>Gets proposed payloads and facade selectors.</summary><returns>Explicit payload controls.</returns>
    public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData/DerivedIntegers/cases.json")));foreach(var row in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})yield return[row.GetProperty("id").GetString()!,row.GetRawText(),data];}
    /// <summary>Measures proposed XML through both real public facades.</summary>
    /// <param name="id">Case identity.</param><param name="json">Exact original payload.</param><param name="data">Selects original-node data.</param>
    [Theory][MemberData(nameof(Cases))]
    public void ProposedPayloads(string id,string json,bool data)
    {
        Assert.StartsWith("DI-",id);using var doc=JsonDocument.Parse(json);var r=doc.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("expectedCompile").GetString();var assessment=r.GetProperty("expectedAssessment").GetString();var path=r.GetProperty("elementPath").EnumerateArray().Select(x=>x.GetInt32()).ToImmutableArray();var attribute=r.GetProperty("attribute").GetString();var options=r.GetProperty("options").ValueKind==JsonValueKind.Object?Options(r.GetProperty("options").GetProperty("maxDigits").GetInt32()):Options();
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,options);Assert.Equal(compile,c.Outcome.ToString());if(c.Value is not{}s){Assert.Null(c.Value);return;}using(s){var a=s.Assess(xml,Context);Assert.Equal(assessment,a.Outcome.ToString());if(a.Value is not{}d){Assert.Null(a.Value);return;}using(d){Check(attribute is null?d.ReadElement(path):d.ReadAttribute(path,"",attribute),r.GetProperty("expectedValue").GetString()!);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}}}
        else{var c=NativeSchemaSession.Create(schema,options);Assert.Equal(compile,c.Outcome.ToString());if(c.Value is not{}s){Assert.Null(c.Value);return;}using(s){var a=s.Assess(xml,Context);Assert.Equal(assessment,a.Outcome.ToString());if(a.Value is not{}d){Assert.Null(a.Value);return;}using(d){Assert.Equal(attribute is null&&path.IsEmpty,d.HasRootScalar);Assert.Equal(path.IsEmpty?0:1,d.ScalarCount);Check(attribute is not null?d.ReadAttribute(0).Value:path.IsEmpty?d.ReadRootScalar():d.ReadScalar(0),r.GetProperty("expectedValue").GetString()!);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}}}
    }
    private static void Check(NativeScalarValue value,string expected){Assert.Equal(NativeScalarKind.Integer,value.Kind);Assert.Equal(BigInteger.Parse(expected,CultureInfo.InvariantCulture),value.IntegerValue);}
}
