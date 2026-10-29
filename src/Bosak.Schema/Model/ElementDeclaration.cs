// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : Element declaration records for the REQ-009 compiled schema model (typed-view building).
// SPECIAL NOTES        : Part of the REQ-009 XSD 1.1 staged support (ADR-005); S2 slice. Records xs:element
//                        declarations carrying an explicit type so the validation service can type candidate
//                        content per declaration (PSVI-style annotation on a private clone, never the caller's tree).
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S2)                                                                    |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>
/// An <c>xs:element</c> declaration carrying an explicit <c>type</c> attribute, recorded by the
/// REQ-009 compile pipeline. The validation service uses these declarations to type candidate
/// content: an element whose expanded name matches a declaration is annotated with the declared
/// built-in simple type (when resolvable) before assertions are evaluated (REQ-009 S2).
/// </summary>
/// <param name="ElementNamespace">Resolved namespace URI of the declared element name.</param>
/// <param name="ElementLocalName">Local name of the declared element.</param>
/// <param name="TypeNamespace">Resolved namespace URI of the declared <c>type</c> attribute value.</param>
/// <param name="TypeLocalName">Local name of the declared <c>type</c> attribute value.</param>
/// <param name="NodePath">Path of the originating node inside the schema document.</param>
public sealed record ElementDeclaration(
    string ElementNamespace,
    string ElementLocalName,
    string TypeNamespace,
    string TypeLocalName,
    string? NodePath);
