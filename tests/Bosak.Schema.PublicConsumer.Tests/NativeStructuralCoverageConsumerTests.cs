// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies evidence-selected empty groups and anonymous attribute type contracts.
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
//                      | Charles Korthout | 0.2 | 26-11-2026     | Reconcile original ID admission and retain redacted exclusions |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Checks structural contracts using only the owned public facades.</summary>
public sealed class NativeStructuralCoverageConsumerTests
{
    private static NativeSchemaSessionOptions Options => new(256,256,16384,65536,2,256,16,256,1024);
    /// <summary>Gets fixed authored contracts through both public facades.</summary>
    /// <returns>Stable case identities, original payloads and facade selectors.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","StructuralCoverage","cases.json")));
        foreach(var row in d.RootElement.GetProperty("cases").EnumerateArray())
            foreach(bool data in new[]{false,true}) yield return [row.GetProperty("id").GetString()!,row.GetRawText(),data];
    }
    /// <summary>Verifies compile/assessment outcomes, original XML, empty values and attribute items.</summary>
    /// <param name="id">Stable contract identity.</param><param name="json">Original authored payload.</param><param name="data">Original-node facade selector.</param>
    [Theory][MemberData(nameof(Cases))]
    public void Contract(string id,string json,bool data)
    {
        using var p=JsonDocument.Parse(json);var row=p.RootElement;
        var schema=row.GetProperty("schema").GetString()!;var xml=row.GetProperty("instance").GetString()!;
        var compile=row.GetProperty("compile").GetString();if(id=="ST-098")compile="Valid";var outcome=row.GetProperty("outcome").GetString();
        var path=row.TryGetProperty("path",out var a)?a.EnumerateArray().Select(x=>x.GetInt32()).ToImmutableArray():[];
        bool Flag(string n)=>row.TryGetProperty(n,out var v)&&v.GetBoolean();
        var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);
        ImmutableArray<NativeScalarValue> items=[];string exported;
        if(data)
        {
            var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome+":"+JsonSerializer.Serialize(c.Diagnostics));using var s=c.Value;if(s is null){Assert.NotEqual("Valid",compile);return;}
            var result=s.Assess(xml,context);Assert.True(result.Outcome.ToString()==outcome,id+":assess:"+result.Outcome+":"+JsonSerializer.Serialize(result.Diagnostics));using var d=result.Value;if(d is null){Assert.NotEqual("Valid",outcome);return;}
            if(Flag("empty")){Assert.Equal(NativeScalarKind.Empty,d.ReadElement(path).Kind);Assert.Empty(d.ReadElementItems(path));Assert.False(d.IsNilled(path));}
            if(Flag("attribute"))items=d.ReadAttributeItems(path,"","a");
            if(id=="ST-098")Assert.Equal("x",Assert.Single(d.ReadAttributeItems([],"","a")).StringValue);
            exported=d.ExportXml();
            d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ExportXml());
        }
        else
        {
            var c=NativeSchemaSession.Create(schema,Options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome+":"+JsonSerializer.Serialize(c.Diagnostics));using var s=c.Value;if(s is null){Assert.NotEqual("Valid",compile);return;}
            var result=s.Assess(xml,context);Assert.True(result.Outcome.ToString()==outcome,id+":assess:"+result.Outcome+":"+JsonSerializer.Serialize(result.Diagnostics));using var d=result.Value;if(d is null){Assert.NotEqual("Valid",outcome);return;}
            if(Flag("empty")){Assert.Equal(0,d.ScalarCount);if(path.IsEmpty){Assert.Equal(NativeScalarKind.Empty,d.ReadRootScalar().Kind);Assert.Empty(d.ReadRootItems());}else Assert.False(d.HasRootScalar);}
            if(Flag("attribute"))items=path.IsEmpty?d.ReadAttributeItems(0):d.ReadDescendantAttributeItems(0);
            if(id=="ST-098")Assert.Equal("x",Assert.Single(d.ReadAttributeItems(0)).StringValue);
            exported=d.ExportXml();
            d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ExportXml());
        }
        Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));
        if(Flag("attribute"))
        {
            Assert.All(items,v=>Assert.Equal(row.GetProperty("kind").GetString(),v.Kind.ToString()));
            string Text(NativeScalarValue v)=>v.Kind switch{NativeScalarKind.String=>v.StringValue,NativeScalarKind.Integer=>v.IntegerValue.ToString(CultureInfo.InvariantCulture),NativeScalarKind.Boolean=>v.BooleanValue?"true":"false",_=>throw new InvalidOperationException("Unexpected fixture kind.")};
            Assert.Equal(row.GetProperty("value").GetString(),string.Join("|",items.Select(Text)));
        }
    }
}
