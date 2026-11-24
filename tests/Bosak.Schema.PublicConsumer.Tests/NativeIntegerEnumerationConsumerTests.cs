// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies bounded integer enumeration and original typed-value identity.
// SPECIAL NOTES        : Tests of native validation with original facts; no substitute validator.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 24-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 24-11-2026     | Qualify placement and pin original baseline inputs |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


using System.Collections.Immutable;
using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Checks actual public enumeration outcomes and original native identity.</summary>
public sealed class NativeIntegerEnumerationConsumerTests
{
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private static NativeSchemaSessionOptions Options(JsonElement r)=>new(r.TryGetProperty("options",out var o)?o.GetProperty("nativeDigits").GetInt32():256,256,16384,65536,1,256,1,256,256);
    /// <summary>Gets all proposed payloads through both facades.</summary><returns>Original inputs.</returns>
    public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","IntegerEnumeration","case-plan.json")));foreach(var r in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetRawText(),data];}
    /// <summary>Measures compile and instance outcomes without rewriting payloads.</summary><param name="id">ID.</param><param name="json">Payload.</param><param name="data">Facade.</param>
    [Theory][MemberData(nameof(Cases))]
    public void PlannedPayloads(string id,string json,bool data)
    {
        Assert.StartsWith("IE-",id);using var doc=JsonDocument.Parse(json);var r=doc.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("expectedCompile").GetString();var outcome=r.GetProperty("expectedAssessment").GetString();var root=XElement.Parse(xml);bool attribute=r.TryGetProperty("placement",out var p)&&p.GetString()=="attribute";var children=root.Elements().ToArray();
        void Value(NativeScalarValue v){Assert.Equal(NativeScalarKind.Integer,v.Kind);Assert.Equal(BigInteger.Parse(r.GetProperty("expectedValue").GetString()!,CultureInfo.InvariantCulture),v.IntegerValue);}
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options(r));Assert.True(compile==c.Outcome.ToString(),id+":"+JsonSerializer.Serialize(c.Diagnostics));if(c.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(c.Value);return;}Assert.NotNull(c.Value);using var s=c.Value;var a=s.Assess(xml,Context);Assert.Equal(outcome,a.Outcome.ToString());if(a.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(a.Value);return;}using var d=a.Value!;Value(attribute?d.ReadAttribute([],"","a"):d.ReadElement(children.Length==0?[]:[0]));Assert.True(XNode.DeepEquals(root,XElement.Parse(d.ExportXml())));}
        else{var c=NativeSchemaSession.Create(schema,Options(r));Assert.True(compile==c.Outcome.ToString(),id+":"+JsonSerializer.Serialize(c.Diagnostics));if(c.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(c.Value);return;}Assert.NotNull(c.Value);using var s=c.Value;var a=s.Assess(xml,Context);Assert.Equal(outcome,a.Outcome.ToString());if(a.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(a.Value);return;}using var d=a.Value!;Assert.Equal(!attribute&&children.Length==0,d.HasRootScalar);Assert.Equal(children.Length,d.ScalarCount);Value(attribute?d.ReadAttribute(0).Value:children.Length==0?d.ReadRootScalar():d.ReadScalar(0));Assert.True(XNode.DeepEquals(root,XElement.Parse(d.ExportXml())));}
    }
    /// <summary>The original out-of-profile empty attribute root remains refused.</summary><param name="data">Facade selector.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void OriginalAttributeOnlyRefusal(bool data)
    {
        using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","IntegerEnumeration","original-contract.json")));var r=d.RootElement.GetProperty("cases").EnumerateArray().Single(x=>x.GetProperty("id").GetString()=="IE-073");var schema=r.GetProperty("schema").GetString()!;
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options(r));Assert.Equal(NativeSchemaOutcome.Unsupported,c.Outcome);Assert.Null(c.Value);}else{var c=NativeSchemaSession.Create(schema,Options(r));Assert.Equal(NativeSchemaOutcome.Unsupported,c.Outcome);Assert.Null(c.Value);}
    }

}
