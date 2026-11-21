// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : XSD language version selector for the REQ-009 compiled schema model.
// SPECIAL NOTES        : Part of the REQ-009 XSD 1.1 staged support (ADR-005); S1 slice.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation                                                                                 |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>
/// Selects the W3C XML Schema language version a schema document is compiled against.
/// </summary>
/// <remarks>
/// The version is explicit (never guessed from document content alone): an XSD 1.0
/// compile rejects XSD 1.1-only constructs with explicit diagnostics, and an XSD 1.1
/// compile accepts them. This keeps existing XSD 1.0 behavior bit-identical.
/// </remarks>
public enum XsdLanguageVersion
{
    /// <summary>W3C XML Schema 1.0 semantics; 1.1-only constructs are rejected with diagnostics.</summary>
    Xsd10,

    /// <summary>W3C XML Schema 1.1 semantics; 1.1 constructs are compiled into the model (evaluation lands in later phases).</summary>
    Xsd11,
}
