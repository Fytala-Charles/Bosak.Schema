// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies original markup and opaque schema annotation contracts.
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
using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;

internal static class NativeMarkupLegacyVerification
{
    internal static JsonElement Reconcile(JsonElement original,string owner)
    {
        using var data=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","MarkupCoverage","historical-expectations.json")));
        var id=original.GetProperty("id").GetString();
        foreach(var patch in data.RootElement.EnumerateArray())
        {
            if(patch.GetProperty("owner").GetString()!=owner||patch.GetProperty("id").GetString()!=id)continue;
            var row=JsonNode.Parse(original.GetRawText())!;
            Assert.True(JsonNode.DeepEquals(row,JsonNode.Parse(patch.GetProperty("original").GetRawText())),owner+":"+id+":original stimulus/metadata changed");
            foreach(var value in patch.GetProperty("changes").EnumerateObject())row[value.Name]=JsonNode.Parse(value.Value.GetRawText());
            return JsonSerializer.SerializeToElement(row);
        }
        return original;
    }
}
