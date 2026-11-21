// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 September 2026
// PURPOSE              : Exception raised when a Bosak.Schema license key fails validation.
// SPECIAL NOTES        : Thrown by License.Activate only; the package stays inert when it is thrown.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-09-2026     | Creation                                                                                 |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>Retains the historical exception type for binary compatibility; Apache factories do not throw it.</summary>
public sealed class LicenseValidationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LicenseValidationException"/> class.
    /// </summary>
    public LicenseValidationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LicenseValidationException"/> class
    /// with a descriptive message.
    /// </summary>
    /// <param name="message">The reason the license key was rejected.</param>
    public LicenseValidationException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LicenseValidationException"/> class
    /// with a descriptive message and an inner exception.
    /// </summary>
    /// <param name="message">The reason the license key was rejected.</param>
    /// <param name="innerException">The underlying parsing or cryptographic failure.</param>
    public LicenseValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
