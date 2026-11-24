// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Verifies contextual integer assertions through the public owned facades.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 21-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Validates contextual integer assertions through both public owned facades.</summary>
public sealed class NativeContextualAssertionConsumerTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,1024,65536,1,32,1,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private static string Text(NativeScalarValue v)=>v.IntegerValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
    private static string ExpectedText(string xml,ImmutableArray<int> path)
    {var node=XElement.Parse(xml);foreach(var index in path)node=node.Elements().ElementAt(index);return System.Numerics.BigInteger.Parse(node.Value).ToString(System.Globalization.CultureInfo.InvariantCulture);}
    /// <summary>Gets all concrete proposal payloads for both public facades.</summary>
    /// <returns>Identity, payload and facade.</returns>
    public static IEnumerable<object[]> Cases()
    {using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","ContextualAssertions","cases.json")));foreach(var row in d.RootElement.GetProperty("rows").EnumerateArray())foreach(var data in new[]{false,true})yield return [row.GetProperty("id").GetString()!,row.GetRawText(),data];}
    /// <summary>Executes concrete proposal validity/refusal through the actual public owned facades.</summary>
    /// <param name="id">Proposal identity.</param>
    /// <param name="payload">Full fixture and bounded expected outcomes.</param>
    /// <param name="data">Selects original-node data.</param>
    [Theory][MemberData(nameof(Cases))]
    public void ProposedPayloads_UseNativeAssessment(string id,string payload,bool data)
    {
        Assert.StartsWith("CA-",id);using var d=JsonDocument.Parse(payload);var r=d.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compiled=r.GetProperty("expectedCompile").GetString();var assessed=r.GetProperty("expectedAssessment").GetString();var reason=r.GetProperty("unsupportedReason").GetString();var path=r.GetProperty("path").EnumerateArray().Select(x=>x.GetInt32()).ToImmutableArray();
        if(data)
        {
            var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(compiled,c.Outcome.ToString());if(c.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(c.Value);Assert.Equal("BSK-NATIVE-SCHEMA-"+compiled!.ToUpperInvariant(),Assert.Single(c.Diagnostics).Code);if(reason is not null)Assert.Equal(reason,c.Diagnostics[0].UnsupportedReason.ToString());return;}
            using var s=c.Value!;var a=s.Assess(xml,Context);Assert.Equal(assessed,a.Outcome.ToString());if(a.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(a.Value);if(reason is not null)Assert.Equal(reason,Assert.Single(a.Diagnostics).UnsupportedReason.ToString());return;}
            using var document=a.Value!;var value=document.ReadElement(path);Assert.Equal(NativeScalarKind.Integer,value.Kind);var text=Text(value);Assert.Equal(ExpectedText(xml,path),text);var exported=document.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));document.Dispose();using var reopened=s.Assess(exported,Context).Value!;Assert.Equal(text,Text(reopened.ReadElement(path)));
        }
        else
        {
            var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(compiled,c.Outcome.ToString());if(c.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(c.Value);Assert.Equal("BSK-NATIVE-SCHEMA-"+compiled!.ToUpperInvariant(),Assert.Single(c.Diagnostics).Code);if(reason is not null)Assert.Equal(reason,c.Diagnostics[0].UnsupportedReason.ToString());return;}
            using var s=c.Value!;var a=s.Assess(xml,Context);Assert.Equal(assessed,a.Outcome.ToString());if(a.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(a.Value);if(reason is not null)Assert.Equal(reason,Assert.Single(a.Diagnostics).UnsupportedReason.ToString());return;}
            using var document=a.Value!;var value=document.HasRootScalar?document.ReadRootScalar():document.ReadScalar(0);Assert.Equal(NativeScalarKind.Integer,value.Kind);var text=Text(value);Assert.Equal(ExpectedText(xml,path),text);var exported=document.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));document.Dispose();using var reopened=s.Assess(exported,Context).Value!;Assert.Equal(text,Text(reopened.HasRootScalar?reopened.ReadRootScalar():reopened.ReadScalar(0)));
        }
    }
}
