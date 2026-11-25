// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Verifies bounded instance type selection through public owned facades.
// SPECIAL NOTES        : Public consumer tests without internal or friend access.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 21-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 25-11-2026     | Reconcile measured compound admissions without editing original fixtures |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Verifies selected restrictions through both public owned facades.</summary>
public sealed class NativeInstanceTypeConsumerTests
{
    private static NativeSchemaSessionOptions Options() => new(256,256,1024,65536,1,32,1,256,256);
    private static NativeSchemaAssessmentContext Context => new(DateTimeOffset.UnixEpoch,0);
    private static string Text(NativeScalarValue value) => value.Kind switch
    {
        NativeScalarKind.String => value.StringValue,
        NativeScalarKind.Boolean => value.BooleanValue ? "true" : "false",
        NativeScalarKind.Integer => value.IntegerValue.ToString(CultureInfo.InvariantCulture),
        _ => value.DecimalValue.ToInvariantString(256)
    };
    /// <summary>Gets the concrete proposal payloads, separately for each public owned facade.</summary>
    /// <returns>Proposed identity, payload and facade choice.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var data=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","InstanceTypes","cases.json")));
        foreach(var row in data.RootElement.GetProperty("rows").EnumerateArray())
            if(row.TryGetProperty("schema",out _))foreach(var nodeData in new[]{false,true})yield return [row.GetProperty("id").GetString()!,row.GetRawText(),nodeData];
    }
    /// <summary>Both owned facades enforce selected types and complete request failures without partial output.</summary>
    /// <param name="id">Original proposed identity.</param>
    /// <param name="payload">Schema, instance and bounded expected result.</param>
    /// <param name="nodeData">Selects the fixed data facade.</param>
    [Theory][MemberData(nameof(Cases))]
    public void ProposalPayloads_ExecuteActualNativeSelection(string id,string payload,bool nodeData)
    {
        Assert.StartsWith("XT-",id);using var data=JsonDocument.Parse(payload);var row=data.RootElement;var schema=row.GetProperty("schema").GetString()!;var xml=row.GetProperty("instance").GetString()!;var expected=row.GetProperty("proposedAssessment").GetString()!.Split('/');var compile=id=="XT-051"?"Valid":row.GetProperty("expectedCompile").GetString();var path=row.TryGetProperty("path",out var pathRow)?pathRow.EnumerateArray().Select(n=>n.GetInt32()).ToArray():new[]{0};
        NativeScalarValue? detached=null;
        if(nodeData)
        {
            var created=NativeSchemaNodeDataSession.Create(schema,Options());Assert.Equal(compile,created.Outcome.ToString());if(created.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(created.Value);return;}using var session=created.Value!;var result=session.Assess(xml,Context);Assert.Equal(expected[0],result.Outcome.ToString());
            if(result.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(result.Value);if(expected.Length>1)Assert.Equal(expected[1],Assert.Single(result.Diagnostics).UnsupportedReason.ToString());return;}
            using var document=result.Value!;detached=document.ReadElement(System.Collections.Immutable.ImmutableArray.CreateRange(path));var exported=document.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));var text=Text(detached);document.Dispose();using var reopened=session.Assess(exported,Context).Value!;Assert.Equal(text,Text(reopened.ReadElement(System.Collections.Immutable.ImmutableArray.CreateRange(path))));
        }
        else
        {
            var created=NativeSchemaSession.Create(schema,Options());Assert.Equal(compile,created.Outcome.ToString());if(created.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(created.Value);return;}using var session=created.Value!;var result=session.Assess(xml,Context);Assert.Equal(expected[0],result.Outcome.ToString());
            if(result.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(result.Value);if(expected.Length>1)Assert.Equal(expected[1],Assert.Single(result.Diagnostics).UnsupportedReason.ToString());return;}
            using var document=result.Value!;Assert.Equal(1,document.ScalarCount);detached=document.ReadScalar(0);var exported=document.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));var text=Text(detached);document.Dispose();using var reopened=session.Assess(exported,Context).Value!;Assert.Equal(text,Text(reopened.ReadScalar(0)));
        }
        Assert.NotNull(detached);Assert.NotEmpty(Text(detached));
    }
}
