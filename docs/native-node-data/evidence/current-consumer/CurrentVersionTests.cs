// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Verifies the explicitly overridden published Core package version.
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

using Xunit;
using Bosak.XPath.Api.Schema;
using System.Reflection;

/// <summary>Identifies the overridden current public Core lane.</summary>
public sealed class CurrentVersionTests
{
    /// <summary>Requires the recorded published1.81.17 package to be loaded.</summary>
    [Fact]
    public void CurrentCore_IsRecordedVersion()
    {
        Assert.Equal(new Version(1,81,17,0),typeof(SchemaQuerySession).Assembly.GetName().Version);
        Assert.StartsWith("1.81.17+",typeof(SchemaQuerySession).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion);
    }
}
