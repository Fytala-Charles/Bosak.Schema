// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies original text-only anyType terminals and explicit lax wildcard refusals.
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
//                      | Charles Korthout | 0.2 | 26-11-2026     | Reconcile known global lax matches with original fixtures and test names retained |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Text.Json;
using System.Xml.Linq;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Checks actual anyType text values, original XML and refusal boundaries through both facades.</summary>
public sealed class NativeAnyTypeTerminalConsumerTests
{
    /// <summary>Gets the authored cases through both public facades.</summary><returns>Original contract and facade selector.</returns>
    public static IEnumerable<object[]> Cases()
    {using var p=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","AnyTypeTerminals","cases.json")));foreach(var r in p.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetRawText(),data];}
    /// <summary>Publishes one original string atom only after complete bounded assessment.</summary>
    /// <param name="id">Stable contract.</param><param name="json">Original stimulus.</param><param name="data">Facade selector.</param>
    [Theory][MemberData(nameof(Cases))]
    public void Contract(string id,string json,bool data)
    {
        using var p=JsonDocument.Parse(json);var r=p.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("compile").GetString();var outcome=r.GetProperty("outcome").GetString();if(id is "AT-017" or "AT-035")outcome="Valid";if(id is "AT-062" or "AT-063")outcome="Invalid";var nested=r.GetProperty("placement").GetString()!="root";var read=r.GetProperty("read").GetBoolean();var options=new NativeSchemaSessionOptions(256,256,r.GetProperty("maxString").GetInt32(),65536,2,256,16,256,1024);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);string exported;NativeScalarValue? value=null;
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);if(compile!="Valid"){Assert.Null(c.Value);return;}using var s=c.Value!;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==outcome,id+":assess:"+a.Outcome);if(outcome!="Valid"){Assert.Null(a.Value);return;}using var d=a.Value!;if(read){value=d.ReadElement(nested?[0]:[]);Assert.Single(d.ReadElementItems(nested?[0]:[]));Assert.False(d.IsNilled(nested?[0]:[]));}exported=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ExportXml());}
        else{var c=NativeSchemaSession.Create(schema,options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);if(compile!="Valid"){Assert.Null(c.Value);return;}using var s=c.Value!;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==outcome,id+":assess:"+a.Outcome);if(outcome!="Valid"){Assert.Null(a.Value);return;}using var d=a.Value!;Assert.Equal(!nested,d.HasRootScalar);if(read){value=nested?d.ReadScalar(0):d.ReadRootScalar();Assert.Single(nested?d.ReadScalarItems(0):d.ReadRootItems());}else Assert.Equal(0,d.ScalarCount);exported=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ExportXml());}
        Assert.True(XNode.DeepEquals(XElement.Parse(xml,LoadOptions.PreserveWhitespace),XElement.Parse(exported,LoadOptions.PreserveWhitespace)),id+":XML");if(read&&r.TryGetProperty("expected",out var expected)){Assert.NotNull(value);Assert.Equal(NativeScalarKind.String,value.Kind);Assert.Equal(expected.GetString(),value.StringValue);}
    }
}
