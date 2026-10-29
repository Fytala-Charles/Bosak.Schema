// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 September 2026
// PURPOSE              : Retains activation-free compatibility members for legacy schema callers.
// SPECIAL NOTES        : Legacy compatibility only; schema functionality requires no credential or activation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-09-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 05-10-2026     | Added GracePeriod, trial/Community acceptance, test clock seam (REQ-002/003/004)         |
//                      | Charles Korthout | 0.3   | 29-10-2026     | Document the legacy activation API as optional compatibility behavior                    |

//                      | Charles Korthout | 0.4   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>Provides activation-free compatibility with legacy Bosak.Schema callers.</summary>
/// <remarks>No credential is parsed, stored, issued or used to authorize any product.</remarks>
public static class License
{
    private static readonly ActivatedLicense ApacheCompatibility = new("Apache-2.0 (activation not required)", LicenseTier.Dev, DateTimeOffset.UnixEpoch, DateTimeOffset.MaxValue);

    /// <summary>Gets the historical grace duration for compatibility; it does not restrict schema access.</summary>
    public static TimeSpan GracePeriod { get; } = TimeSpan.FromDays(14);

    /// <summary>Gets a value that is always true; Apache schema services need no activation.</summary>
    public static bool IsActivated => true;

    /// <summary>Gets a perpetual Apache compatibility marker, not a commercial entitlement.</summary>
    public static ActivatedLicense? Current => ApacheCompatibility;

    /// <summary>Accepts legacy calls without validating or retaining the supplied value.</summary>
    /// <param name="licenseKey">Ignored compatibility parameter; no credential is required.</param>
    public static void Activate(string licenseKey)
    {
        _ = licenseKey;
    }

    internal static Func<DateTimeOffset> Clock { get; set; } = () => DateTimeOffset.UtcNow;
}
