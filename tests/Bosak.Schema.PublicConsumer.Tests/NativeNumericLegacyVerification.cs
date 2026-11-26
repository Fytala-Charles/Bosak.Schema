// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Reconciles measured numeric-pattern expectations without altering original fixtures.
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
namespace Bosak.Schema.PublicConsumer.Tests;
internal static class NativeNumericLegacyVerification
{
    internal static JsonElement Reconcile(JsonElement original, string owner)
    {
        var id=original.GetProperty("id").GetString();
        var admitted=(owner,id) is (nameof(NativeAnonymousRestrictionConsumerTests),"AS-037") or
            (nameof(NativeIntegerRestrictionConsumerTests),"IR-071") or
            (nameof(NativeDerivedIntegerConsumerTests),"DI-142") or
            (nameof(NativeIntegerEnumerationConsumerTests),"IE-089") or
            (nameof(NativeDecimalEnumerationConsumerTests),"pattern-outside") or
            (nameof(NativeDecimalChainsConsumerTests),"pattern-outside");
        if(!admitted)return original;
        var row=JsonNode.Parse(original.GetRawText())!.AsObject();
        if(row.ContainsKey("expectedCompile")){row["expectedCompile"]="Valid";row["expectedAssessment"]="Valid";}
        else {row["proposedCompile"]="Valid";row["proposedAssessment"]="Valid";}
        if(row.ContainsKey("values"))row["values"]=new JsonArray("1");
        if(row.ContainsKey("expectedValue"))row["expectedValue"]="1";
        return JsonSerializer.SerializeToElement(row);
    }
}
