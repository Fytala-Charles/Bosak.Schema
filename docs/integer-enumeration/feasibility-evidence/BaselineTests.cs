// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Records unchanged production refusal for original contract schemas.
// SPECIAL NOTES        : Test-only prototype; no production facade or synthetic XML validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 24-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 24-11-2026     | Qualify placement and pin original baseline inputs |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


using System.Text.Json;
using Bosak.Schema;
using Xunit;
namespace EnumBaseline;
/// <summary>Records unchanged production compile outcomes for proposed schemas.</summary>
public sealed class BaselineTests
{
    /// <summary>Gets exact authored schemas.</summary><returns>Inputs for both facades.</returns>
    public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"original-contract.json")));foreach(var r in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetProperty("schema").GetString()!,data];}
    /// <summary>Unchanged production refuses enumeration-bearing schemas.</summary><param name="id">ID.</param><param name="schema">Schema.</param><param name="data">Facade.</param>
    [Theory][MemberData(nameof(Cases))]
    public void ProductionRefuses(string id,string schema,bool data)
    {Assert.NotEmpty(id);var options=new NativeSchemaSessionOptions(256,256,16384,65536,1,256,1,256,256);if(data){var r=NativeSchemaNodeDataSession.Create(schema,options);Assert.Equal(NativeSchemaOutcome.Unsupported,r.Outcome);Assert.Null(r.Value);}else{var r=NativeSchemaSession.Create(schema,options);Assert.Equal(NativeSchemaOutcome.Unsupported,r.Outcome);Assert.Null(r.Value);}}
}
