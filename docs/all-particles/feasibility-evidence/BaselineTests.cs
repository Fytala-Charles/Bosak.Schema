// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 November 2026
// PURPOSE              : Measures original production refusal without changing authored payloads.
// SPECIAL NOTES        : Test-only prototype; no production facade or synthetic XML validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 23-11-2026     | Align test responsibility header with its primary type |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


using System.Text.Json;
using Bosak.Schema;
using Xunit;
namespace AllBaseline;
/// <summary>Measures original production refusal without changing authored payloads.</summary>
public sealed class BaselineTests
{
    /// <summary>Gets both facades for every proposed original schema.</summary><returns>Public control inputs.</returns>
    public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"case-plan.json")));foreach(var r in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetProperty("schema").GetString()!,data];}
    /// <summary>Unmodified production refuses all-group schemas.</summary><param name="id">Original ID.</param><param name="schema">Original schema.</param><param name="data">Facade selector.</param>
    [Theory][MemberData(nameof(Cases))]
    public void ProductionRefusesAll(string id,string schema,bool data)
    {Assert.NotEmpty(id);var options=new NativeSchemaSessionOptions(256,256,16384,65536,1,256,1,256,256);if(data){var r=NativeSchemaNodeDataSession.Create(schema,options);Assert.Equal(NativeSchemaOutcome.Unsupported,r.Outcome);Assert.Null(r.Value);}else{var r=NativeSchemaSession.Create(schema,options);Assert.Equal(NativeSchemaOutcome.Unsupported,r.Outcome);Assert.Null(r.Value);}}
}
