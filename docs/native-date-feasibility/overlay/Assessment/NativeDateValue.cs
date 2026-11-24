// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Carries detached immutable checked date components.
// SPECIAL NOTES        : Isolated experimental date candidate; not production adoption.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 24-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using Bosak.XPath.Core.Xdm;
namespace Bosak.Schema;
/// <summary>Contains checked date components without schema authority.</summary>
public sealed class NativeDateValue
{
    internal NativeDateValue(XPathDateTime value) { Year=value.Year; Month=value.Month; Day=value.Day; TimezoneOffsetMinutes=value.HasTimezone?value.TimezoneOffsetMinutes:null; }
    /// <summary>Gets the astronomical year.</summary>
    public long Year { get; }
    /// <summary>Gets the month.</summary>
    public int Month { get; }
    /// <summary>Gets the day.</summary>
    public int Day { get; }
    /// <summary>Gets the optional timezone offset; null means absent.</summary>
    public int? TimezoneOffsetMinutes { get; }
}
