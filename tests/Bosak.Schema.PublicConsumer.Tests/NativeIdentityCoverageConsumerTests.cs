// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies bounded validation-root identity and reference contracts.
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

/// <summary>Checks lexical, graph, value and capacity contracts through both public facades.</summary>
public sealed class NativeIdentityCoverageConsumerTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,2,256,16,256,1024);
    /// <summary>Gets fixed original stimuli through both owned facades.</summary><returns>Identity, payload and selector.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var p=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","IdentityCoverage","cases.json")));
        foreach(var r in p.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetRawText(),data];
    }
    /// <summary>Checks complete validity before publication, exact detached strings and original XML.</summary>
    /// <param name="id">Stable identity.</param><param name="json">Original input.</param><param name="data">Original-node facade selector.</param>
    [Theory][MemberData(nameof(Cases))]
    public void Contract(string id,string json,bool data)
    {
        using var p=JsonDocument.Parse(json);var r=p.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;
        var compile=r.GetProperty("compile").GetString();var outcome=r.GetProperty("outcome").GetString();var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);
        bool Flag(string key)=>r.TryGetProperty(key,out var v)&&v.GetBoolean();
        var path=r.TryGetProperty("path",out var a)?a.EnumerateArray().Select(v=>v.GetInt32()).ToImmutableArray():[];
        ImmutableArray<NativeScalarValue> values=[];string exported;
        if(data)
        {
            var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome+":"+JsonSerializer.Serialize(c.Diagnostics));if(compile!="Valid"){Assert.Null(c.Value);return;}using var s=c.Value;if(s is null){Assert.NotEqual("Valid",compile);return;}
            var aresult=s.Assess(xml,context);Assert.True(aresult.Outcome.ToString()==outcome,id+":assess:"+aresult.Outcome+":"+JsonSerializer.Serialize(aresult.Diagnostics));if(outcome!="Valid"){Assert.Null(aresult.Value);return;}using var d=aresult.Value;if(d is null){Assert.NotEqual("Valid",outcome);return;}
            if(Flag("empty")){if(Flag("list"))Assert.Throws<NotSupportedException>(()=>d.ReadElement([]));else Assert.Equal(NativeScalarKind.Empty,d.ReadElement([]).Kind);Assert.Empty(d.ReadElementItems([]));Assert.True(d.IsNilled([]));}
            if(r.TryGetProperty("read",out var read))values=read.GetString()=="attribute"?d.ReadAttributeItems(path,"",r.GetProperty("name").GetString()!):d.ReadElementItems(path);
            exported=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ExportXml());
        }
        else
        {
            var c=NativeSchemaSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+JsonSerializer.Serialize(c.Diagnostics));if(compile!="Valid"){Assert.Null(c.Value);return;}using var s=c.Value;if(s is null){Assert.NotEqual("Valid",compile);return;}
            var aresult=s.Assess(xml,context);Assert.True(aresult.Outcome.ToString()==outcome,id+":assess:"+aresult.Outcome+":"+JsonSerializer.Serialize(aresult.Diagnostics));if(outcome!="Valid"){Assert.Null(aresult.Value);return;}using var d=aresult.Value;if(d is null){Assert.NotEqual("Valid",outcome);return;}
            if(Flag("empty")){if(Flag("list"))Assert.Throws<NotSupportedException>(()=>d.ReadRootScalar());else Assert.Equal(NativeScalarKind.Empty,d.ReadRootScalar().Kind);Assert.Empty(d.ReadRootItems());Assert.True(d.IsRootNilled);}
            if(r.TryGetProperty("read",out var read))values=read.GetString()=="attribute"?d.ReadAttributeItems(r.GetProperty("index").GetInt32()):Flag("root")?d.ReadRootItems():d.ReadScalarItems(0);
            exported=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ExportXml());
        }
        Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));
        if(r.TryGetProperty("expected",out var expected)){Assert.All(values,v=>Assert.Equal(NativeScalarKind.String,v.Kind));Assert.Equal(expected.GetString(),string.Join("|",values.Select(v=>v.StringValue)));}
    }
}
