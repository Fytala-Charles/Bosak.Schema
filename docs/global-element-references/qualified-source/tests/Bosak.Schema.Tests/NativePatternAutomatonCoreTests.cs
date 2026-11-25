// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Verifies isolated bounded pattern matching and native original string assessment.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 22-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.Schema;
using Xunit;
namespace Bosak.Schema.Tests.Patterns;
/// <summary>Verifies finite pattern automaton matching.</summary>
public sealed class NativePatternAutomatonCoreTests
{
    /// <summary>Nullable epsilon loops terminate and supplementary characters remain single scalars.</summary>
    [Theory][InlineData("()*","",true)][InlineData("(a|aa)*b","aaaaab",true)][InlineData("(a|aa)*b","aaaaa",false)][InlineData(".","\U00010400",true)][InlineData(".","e\u0301",false)]
    public void PureAutomaton(string pattern,string input,bool expected)
    {var compiled=NativePattern.Compile(pattern,new(default));Assert.Equal(expected,compiled.Match(input,new(default)));}
}
