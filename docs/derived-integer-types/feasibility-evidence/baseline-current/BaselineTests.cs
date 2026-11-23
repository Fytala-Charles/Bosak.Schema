// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Verifies isolated derived integer values and original type evidence.
// SPECIAL NOTES        : Test-only prototype; no production facade or synthetic XML validation.
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


using Bosak.Schema;
using System.Text.Json;
using Xunit;
namespace Baseline;
/// <summary>Measures unchanged-production refusal of the twelve new type names.</summary>
public sealed class Tests
{
    /// <summary>Gets builtin names and both facade selectors.</summary>
    /// <returns>The explicit controls.</returns>
    public static IEnumerable<object[]> Rows()
    {using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"builtin-types.json")));foreach(var r in d.RootElement.GetProperty("types").EnumerateArray())foreach(bool data in new[]{false,true})yield return[r.GetProperty("name").GetString()!,data];}
    /// <summary>Confirms the unchanged product refuses new type names.</summary>
    /// <param name="type">Builtin name.</param><param name="data">Selects original-node facade.</param>
    [Theory][MemberData(nameof(Rows))]
    public void ExistingRefusal(string type,bool data)
    {var schema=$"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:{type}'/></xs:schema>";var opts=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);if(data){var c=NativeSchemaNodeDataSession.Create(schema,opts);Assert.Equal(NativeSchemaOutcome.Unsupported,c.Outcome);Assert.Null(c.Value);}else{var c=NativeSchemaSession.Create(schema,opts);Assert.Equal(NativeSchemaOutcome.Unsupported,c.Outcome);Assert.Null(c.Value);}}
}
