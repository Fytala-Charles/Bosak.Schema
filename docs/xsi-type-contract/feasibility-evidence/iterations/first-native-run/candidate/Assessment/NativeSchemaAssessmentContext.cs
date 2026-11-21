// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Defines explicit constant context for owned mixed scalar assessment.
// SPECIAL NOTES        : Bounded opt-in string/decimal profile; existing decimal facade remains unchanged.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>Defines an immutable explicit native assessment episode.</summary>
public sealed class NativeSchemaAssessmentContext
{
    /// <summary>Creates an episode without reading the host clock or timezone.</summary>
    /// <param name="currentDateTime">The caller's fixed assessment date and time.</param>
    /// <param name="implicitTimezoneOffsetMinutes">The implicit timezone offset, from -840 to 840 inclusive.</param>
    /// <exception cref="ArgumentOutOfRangeException">The timezone is outside -840..840 minutes.</exception>
    public NativeSchemaAssessmentContext(DateTimeOffset currentDateTime, int implicitTimezoneOffsetMinutes)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(implicitTimezoneOffsetMinutes, -840);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(implicitTimezoneOffsetMinutes, 840);
        CurrentDateTime = currentDateTime;
        ImplicitTimezoneOffsetMinutes = implicitTimezoneOffsetMinutes;
    }
    /// <summary>Gets the explicit fixed date and time.</summary>
    public DateTimeOffset CurrentDateTime { get; }
    /// <summary>Gets the implicit timezone offset in minutes.</summary>
    public int ImplicitTimezoneOffsetMinutes { get; }
}
