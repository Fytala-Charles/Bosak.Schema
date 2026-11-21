// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Verifies exact namespace references and schema form defaults through public facades.
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
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Verifies relative namespace identity and independent form defaults without friend access.</summary>
public sealed class NativeNamespaceFormConsumerTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,1024,65536,1,32,1,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private static string Text(NativeScalarValue v)=>v.Kind switch {NativeScalarKind.String=>v.StringValue,NativeScalarKind.Boolean=>v.BooleanValue?"true":"false",NativeScalarKind.Integer=>v.IntegerValue.ToString(CultureInfo.InvariantCulture),_=>v.DecimalValue.ToInvariantString(256)};
    /// <summary>Gets clause-mapped authored cases for each public owned facade.</summary>
    /// <returns>Stable identity, payload and facade.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var input=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","NamespaceForm","cases.json")));
        foreach(var row in input.RootElement.EnumerateArray())foreach(var data in new[]{false,true})yield return [row.GetProperty("id").GetString()!,row.GetRawText(),data];
    }
    /// <summary>Preserves namespace spelling, validity, detached values and export/reassessment ownership.</summary>
    /// <param name="id">Stable authored identity.</param>
    /// <param name="payload">Schema, instance and expectations.</param>
    /// <param name="data">Selects original-node data.</param>
    [Theory][MemberData(nameof(Cases))]
    public void NamespaceForm_EnforcesExactOwnedBehavior(string id,string payload,bool data)
    {
        Assert.StartsWith("NF-",id);using var input=JsonDocument.Parse(payload);var r=input.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var expectedCompile=r.GetProperty("expectedCompile").GetString();var expected=r.GetProperty("expectedAssessment").GetString();var path=r.GetProperty("path").EnumerateArray().Select(n=>n.GetInt32()).ToImmutableArray();NativeScalarValue? detached=null;
        if(data)
        {
            var created=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(expectedCompile,created.Outcome.ToString());
            if(created.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(created.Value);Assert.Equal(r.GetProperty("reason").GetString(),Assert.Single(created.Diagnostics).UnsupportedReason.ToString());return;}
            using var session=created.Value!;var result=session.Assess(xml,Context);Assert.Equal(expected,result.Outcome.ToString());if(result.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(result.Value);return;}
            using var document=result.Value!;detached=document.ReadElement(path);if(r.GetProperty("attribute").GetBoolean())Assert.Equal("A",document.ReadAttribute([],r.GetProperty("attributeNamespace").GetString()!,"code").StringValue);
            var exported=document.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));document.Dispose();using var again=session.Assess(exported,Context).Value!;Assert.Equal(Text(detached),Text(again.ReadElement(path)));
        }
        else
        {
            var created=NativeSchemaSession.Create(schema,Options);Assert.Equal(expectedCompile,created.Outcome.ToString());
            if(created.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(created.Value);Assert.Equal(r.GetProperty("reason").GetString(),Assert.Single(created.Diagnostics).UnsupportedReason.ToString());return;}
            using var session=created.Value!;var result=session.Assess(xml,Context);Assert.Equal(expected,result.Outcome.ToString());if(result.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(result.Value);return;}
            using var document=result.Value!;Assert.Equal(1,document.ScalarCount);detached=document.ReadScalar(0);Assert.Equal(r.GetProperty("attribute").GetBoolean()?1:0,document.AttributeCount);
            if(document.AttributeCount==1){var attribute=document.ReadAttribute(0);Assert.Equal(r.GetProperty("attributeNamespace").GetString(),attribute.NamespaceUri);Assert.Equal("A",attribute.Value.StringValue);Assert.Empty(attribute.OwnerElementPath);}
            var exported=document.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));document.Dispose();using var again=session.Assess(exported,Context).Value!;Assert.Equal(Text(detached),Text(again.ReadScalar(0)));
        }
        Assert.NotNull(detached);Assert.Equal(r.GetProperty("kind").GetString(),detached.Kind.ToString());Assert.Equal(r.GetProperty("value").GetString(),Text(detached));
    }
}
