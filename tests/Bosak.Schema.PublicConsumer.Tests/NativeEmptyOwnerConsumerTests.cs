// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies bounded original attributes on empty complex owners.
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
using System.Text.Json;
using System.Xml.Linq;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Checks empty owner values, attributes, failure publication and original XML through both facades.</summary>
public sealed class NativeEmptyOwnerConsumerTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,2,256,16,256,1024);
    /// <summary>Gets all authored contracts through both public facades.</summary><returns>Original contract and facade selector.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var p=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","EmptyOwners","cases.json")));
        foreach(var r in p.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetRawText(),data];
    }
    /// <summary>Validates the complete document before returning empty element and original attribute facts.</summary>
    /// <param name="id">Stable contract ID.</param><param name="json">Original stimulus.</param><param name="data">Original-node facade selector.</param>
    [Theory][MemberData(nameof(Cases))]
    public void Contract(string id,string json,bool data)
    {
        using var p=JsonDocument.Parse(json);var r=p.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("compile").GetString();var outcome=r.GetProperty("outcome").GetString();var nested=r.GetProperty("nested").GetBoolean();ImmutableArray<int> path=nested?[0]:[];var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);ImmutableArray<NativeScalarValue> values=[];string exported;
        if(data)
        {
            var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);if(compile!="Valid"){Assert.Null(c.Value);return;}using var s=c.Value!;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==outcome,id+":assess:"+a.Outcome);if(outcome!="Valid"){Assert.Null(a.Value);return;}using var d=a.Value!;
            Assert.Equal(NativeScalarKind.Empty,d.ReadElement(path).Kind);Assert.Empty(d.ReadElementItems(path));Assert.False(d.IsNilled(path));if(nested)Assert.Throws<NotSupportedException>(()=>d.ReadElement([]));
            if(r.TryGetProperty("read",out var read))values=d.ReadAttributeItems(path,"",read.GetString()!);
            exported=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadElement(path));
        }
        else
        {
            var c=NativeSchemaSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome);if(compile!="Valid"){Assert.Null(c.Value);return;}using var s=c.Value!;var a=s.Assess(xml,context);Assert.True(a.Outcome.ToString()==outcome,id+":assess:"+a.Outcome);if(outcome!="Valid"){Assert.Null(a.Value);return;}using var d=a.Value!;
            Assert.False(d.HasRootScalar);Assert.Equal(0,d.ScalarCount);if(nested)Assert.Throws<NotSupportedException>(()=>d.ReadRootScalar());else {Assert.Equal(NativeScalarKind.Empty,d.ReadRootScalar().Kind);Assert.Empty(d.ReadRootItems());}
            if(r.TryGetProperty("read",out _))values=nested?d.ReadDescendantAttributeItems(0):d.ReadAttributeItems(0);
            exported=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ExportXml());
        }
        Assert.True(XNode.DeepEquals(XElement.Parse(xml,LoadOptions.PreserveWhitespace),XElement.Parse(exported,LoadOptions.PreserveWhitespace)),id+":original XML");
        if(r.TryGetProperty("expected",out var expected)){Assert.All(values,v=>Assert.Equal(NativeScalarKind.String,v.Kind));Assert.Equal(expected.GetString(),string.Join("|",values.Select(v=>v.StringValue)));}
    }
}
