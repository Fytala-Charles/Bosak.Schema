// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Verifies isolated empty typed-value representation through original native assessments.
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
//                      | Charles Korthout | 0.2   | 22-11-2026     | Adapt native controls for bounded integer simple content |
//                      | Charles Korthout | 0.3   | 22-11-2026     | Verify isolated contextual assertions and retained native baseline |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Executes the unchanged structural proposal payloads through both native public facades.</summary>
public sealed class NativeStructuralAssertionConsumerTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,1024,65536,1,32,1,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Gets exact original proposal payloads, without rewriting expectations.</summary>
    /// <returns>Proposal identity, JSON and facade selector.</returns>
    public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","StructuralAssertions","cases.json")));foreach(var row in d.RootElement.GetProperty("rows").EnumerateArray())foreach(var data in new[]{false,true})yield return [row.GetProperty("id").GetString()!,row.GetRawText(),data];}
    /// <summary>Measures proposed schema and instance outcomes through native assessment.</summary>
    /// <param name="id">Stable proposal identity.</param><param name="payload">Original payload.</param><param name="data">Data facade selector.</param>
    [Theory][MemberData(nameof(Cases))]
    public void ProposedPayloads(string id,string payload,bool data)
    {
        Assert.StartsWith("SA-",id);using var j=JsonDocument.Parse(payload);var row=j.RootElement;var schema=row.GetProperty("schema").GetString()!;var xml=row.GetProperty("instance").GetString()!;var compiled=row.GetProperty("expectedCompile").GetString();var assessed=row.GetProperty("expectedAssessment").GetString();
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(compiled,c.Outcome.ToString());if(c.Value is not {} s)return;using(s){var a=s.Assess(xml,Context);Assert.Equal(assessed,a.Outcome.ToString());if(a.Value is not {} d)return;using(d)Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}}
        else{var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(compiled,c.Outcome.ToString());if(c.Value is not {} s)return;using(s){var a=s.Assess(xml,Context);Assert.Equal(assessed,a.Outcome.ToString());if(a.Value is not {} d)return;using(d)Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}}
    }
    /// <summary>The established decimal-only facade retains its original profile boundaries.</summary>
    /// <param name="assertion">Existing sequence with a new function instead of an empty type.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void LegacyDecimalProfileRemainsBounded(bool assertion)
    {
        var body=assertion?"<xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:assert test=\"namespace-uri(.)=''\"/>":"";
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType>"+body+"</xs:complexType></xs:element></xs:schema>";
        var result=NativeDecimalSession.Create(schema,new(256,256,1,4,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);
    }
}
