// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 17 November 2026
// PURPOSE              : Attempts every owned cleanup action after logical revocation.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 17-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>Attempts every owned cleanup action after logical revocation.</summary>
internal static class NativeCleanup
{
    internal static void Release(params IDisposable?[] resources)
    {
        var failures = 0;
        foreach (var resource in resources)
        {
            try { resource?.Dispose(); }
            catch (Exception) { failures++; }
        }
        if (failures != 0)
            throw new InvalidOperationException($"Native cleanup failed for {failures} resource(s); all owned resources were attempted and access remains revoked.");
    }
}
