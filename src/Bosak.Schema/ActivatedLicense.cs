// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 September 2026
// PURPOSE              : Immutable DTO carrying the claims of an activated Bosak.Schema license.
// SPECIAL NOTES        : Part of the activation boundary; see License and ADR-003 (license key format).
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-09-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 05-10-2026     | Added IsExpired/IsInGrace/IsUsable/DaysUntilExpiry (REQ-002/003/004)                     |

//                      | Charles Korthout | 0.3   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>Retains the legacy credential data shape for compatibility; it does not authorize schema access.</summary>
/// <param name="Licensee">Descriptive legacy label or Apache compatibility marker.</param>
/// <param name="Tier">Legacy tier label, not an entitlement for other products.</param>
/// <param name="IssuedUtc">Informational issue time.</param>
/// <param name="ExpiresUtc">Informational expiry time.</param>
public sealed record ActivatedLicense(string Licensee, LicenseTier Tier, DateTimeOffset IssuedUtc, DateTimeOffset ExpiresUtc)
{
    /// <summary>Gets whether the informational expiry time has been reached.</summary>
    public bool IsExpired => License.Clock() >= ExpiresUtc;

    /// <summary>Gets the historical grace-state calculation; it does not control schema access.</summary>
    public bool IsInGrace => IsExpired && License.Clock() < ExpiresUtc + License.GracePeriod;

    /// <summary>Gets the historical date-state calculation; it grants no commercial entitlement.</summary>
    public bool IsUsable => !IsExpired || IsInGrace;

    /// <summary>Gets informational whole days until expiry, or zero after expiry.</summary>
    public int DaysUntilExpiry => IsExpired
        ? 0
        : (int)Math.Ceiling((ExpiresUtc - License.Clock()).TotalDays);
}
