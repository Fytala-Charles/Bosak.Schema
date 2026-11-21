// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 September 2026
// PURPOSE              : License tiers for Bosak.Schema activation (per-developer or per-deployment).
// SPECIAL NOTES        : Legacy tier shapes mirroring the pre-Apache commercial model (subscription
//                        vs runtime license); retained as Apache-2.0 API compatibility only.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-09-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 05-10-2026     | Added Trial and Community tiers (REQ-002, REQ-003)                                       |
//                      | Charles Korthout | 0.3   | 05-10-2026     | Added Team tier (REQ-005 open question 1: tid "team", up to 10 named developers)         |

//                      | Charles Korthout | 0.4   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      | Charles Korthout | 0.5   | 30-10-2026     | Reworded SPECIAL NOTES: dropped private COMMERCIAL.md reference                           |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>Retains legacy tier values for API compatibility; these labels authorize no schema features.</summary>
public enum LicenseTier
{
    /// <summary>Legacy developer label; also used by the Apache compatibility marker.</summary>
    Dev,
    /// <summary>Legacy runtime label.</summary>
    Runtime,
    /// <summary>Legacy trial label; Apache schema services have no trial restriction.</summary>
    Trial,
    /// <summary>Legacy community label.</summary>
    Community,
    /// <summary>Legacy team label.</summary>
    Team,
}
