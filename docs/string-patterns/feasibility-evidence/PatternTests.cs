// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Verifies isolated bounded pattern matching and native original string assessment.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
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

using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace PatternProbe;
/// <summary>Measures proposed pattern schemas and instances through actual native public facades.</summary>
public sealed class PatternTests
{
    private static NativeSchemaSessionOptions Options => new(256,256,1024,65536,1,32,1,256,256);
    private static NativeSchemaAssessmentContext Context => new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Gets original proposal payloads for both public facades.</summary>
    /// <returns>Identity, JSON and facade selector.</returns>
    public static IEnumerable<object[]> Cases()
    { using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"case-plan.json")));foreach(var row in d.RootElement.GetProperty("rows").EnumerateArray())foreach(bool data in new[]{false,true})yield return [row.GetProperty("id").GetString()!,row.GetRawText(),data]; }
    /// <summary>Executes original planned compile/assessment and exact original scalar behavior.</summary>
    /// <param name="id">Stable authored identity.</param><param name="json">Original payload.</param><param name="data">Selects data facade.</param>
    [Theory][MemberData(nameof(Cases))]
    public void ProposedPayloads(string id,string json,bool data)
    {
        Assert.StartsWith("SP-",id);using var doc=JsonDocument.Parse(json);var r=doc.RootElement;string schema=r.GetProperty("schema").GetString()!,xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("expectedCompile").GetString();var assessment=r.GetProperty("expectedAssessment").GetString();bool attribute=r.GetProperty("attribute").ValueKind!=JsonValueKind.Null;var expected=r.GetProperty("expectedLexical").GetString();
        if(data)
        {
            var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(compile,c.Outcome.ToString());if(c.Value is not {} s){Assert.Null(c.Value);return;}using(s){var a=s.Assess(xml,Context);Assert.Equal(assessment,a.Outcome.ToString());if(a.Value is not {} d){Assert.Null(a.Value);return;}using(d){var value=attribute?d.ReadAttribute([],"","a"):d.ReadElement([0]);Assert.Equal(NativeScalarKind.String,value.Kind);Assert.Equal(expected,value.StringValue);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}}
        }
        else
        {
            var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(compile,c.Outcome.ToString());if(c.Value is not {} s){Assert.Null(c.Value);return;}using(s){var a=s.Assess(xml,Context);Assert.Equal(assessment,a.Outcome.ToString());if(a.Value is not {} d){Assert.Null(a.Value);return;}using(d){var value=attribute?d.ReadAttribute(0).Value:d.ReadScalar(0);Assert.Equal(NativeScalarKind.String,value.Kind);Assert.Equal(expected,value.StringValue);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}}
        }
    }
    /// <summary>Nullable epsilon loops terminate and supplementary characters remain single scalars.</summary>
    [Theory][InlineData("()*","",true)][InlineData("(a|aa)*b","aaaaab",true)][InlineData("(a|aa)*b","aaaaa",false)][InlineData(".","\U00010400",true)][InlineData(".","e\u0301",false)]
    public void PureAutomaton(string pattern,string input,bool expected)
    {var compiled=NativePattern.Compile(pattern,new(default));Assert.Equal(expected,compiled.Match(input,new(default)));}
}
