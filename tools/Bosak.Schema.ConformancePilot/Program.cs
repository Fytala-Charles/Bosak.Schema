// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Measures native outcomes for hash-verified unchanged upstream XSD fixtures.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Reflection;
using System.Text;
using System.Text.Json;
using Bosak.XPath.Api.Schema;

namespace Bosak.Schema.ConformancePilot;

/// <summary>Reports actual native outcomes only; the parent validates source metadata and classifies results.</summary>
internal static class Program
{
    private static readonly UTF8Encoding Utf8 = new(false, true);
    private static int Main(string[] args)
    {
        try
        {
            if(args.Length!=2)throw new ArgumentException("Expected request and output JSON paths.");
            using var input=JsonDocument.Parse(File.ReadAllText(args[0],Utf8));var rows=new List<object>();
            foreach(var row in input.RootElement.EnumerateArray())
            {
                var id=row.GetProperty("id").GetString()!;
                try
                {
                    var schema=File.ReadAllText(row.GetProperty("schema").GetString()!,Utf8);
                    var created=NativeSchemaSession.Create(schema,new(256,256,65536,524288,1,256,1,256,256));
                    if(created.Outcome!=NativeSchemaOutcome.Valid)
                    {
                        rows.Add(new{id,compile=created.Outcome.ToString(),assessment=(string?)null,reason=created.Diagnostics[0].UnsupportedReason.ToString(),error=(string?)null});continue;
                    }
                    using var session=created.Value!;var xml=File.ReadAllText(row.GetProperty("instance").GetString()!,Utf8);
                    var assessed=session.Assess(xml,new(DateTimeOffset.UnixEpoch,0));using var document=assessed.Value;
                    rows.Add(new{id,compile=created.Outcome.ToString(),assessment=assessed.Outcome.ToString(),reason=assessed.Diagnostics.IsEmpty?null:assessed.Diagnostics[0].UnsupportedReason.ToString(),error=(string?)null});
                }
                catch(Exception error){rows.Add(new{id,compile=(string?)null,assessment=(string?)null,reason=(string?)null,error=error.GetType().Name});}
            }
            var report=new{schemaAssembly=typeof(NativeSchemaSession).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion,coreAssembly=typeof(SchemaQuerySession).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion,options=new{nativeDigits=256,nativeScale=256,stringCharacters=65536,exportCharacters=524288,openDocuments=1,cache=256,resultAtoms=1,queryDigits=256,queryScale=256},rows};
            File.WriteAllText(args[1],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}),Utf8);return 0;
        }
        catch(Exception error){Console.Error.WriteLine("Caught pilot harness error: "+error.GetType().Name);return 3;}
    }
}
