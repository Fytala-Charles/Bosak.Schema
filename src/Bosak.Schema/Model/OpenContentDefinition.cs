// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : Open content component definitions for the REQ-009 compiled schema model (REQ-009 S5a).
// SPECIAL NOTES        : Part of the REQ-009 XSD 1.1 staged support (ADR-005); S5a records the components and
//                        validates the XSD 1.1 composition rules — structural enforcement (W3C XML Schema 1.1
//                        Part 1 §3.4.4.3) and effective-open-content derivation are later-phase engine work.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S5a: xs:openContent / xs:defaultOpenContent compile support)           |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>
/// The <c>mode</c> of an XSD 1.1 open content component (W3C XML Schema 1.1 Part 1 §3.4.2).
/// </summary>
public enum OpenContentMode
{
    /// <summary><c>mode="none"</c>: the type's content is closed — used on derivation to remove inherited open content.</summary>
    None,

    /// <summary><c>mode="interleave"</c> (the default): open-content elements may appear anywhere among the content model's elements.</summary>
    Interleave,

    /// <summary><c>mode="suffix"</c>: open-content elements may appear only after the content model's elements.</summary>
    Suffix,
}

/// <summary>
/// The <c>processContents</c> policy of an open content wildcard (W3C XML Schema 1.1 Part 1 §3.10).
/// </summary>
public enum WildcardProcessContents
{
    /// <summary><c>processContents="strict"</c> (the default): the item must be valid against a found declaration.</summary>
    Strict,

    /// <summary><c>processContents="lax"</c>: the item is validated when a declaration is found, otherwise accepted.</summary>
    Lax,

    /// <summary><c>processContents="skip"</c>: the item is not validated against any declaration.</summary>
    Skip,
}

/// <summary>
/// The wildcard of an XSD 1.1 open content component — the <c>xs:any</c> child of
/// <c>xs:openContent</c> / <c>xs:defaultOpenContent</c>, recorded in raw lexical form.
/// </summary>
/// <param name="NamespaceConstraint">The space-separated <c>namespace</c> attribute tokens
/// (<c>##any</c>, <c>##other</c>, <c>##targetNamespace</c>, <c>##local</c>, or namespace URIs);
/// <c>##any</c> when the attribute is absent.</param>
/// <param name="ProcessContents">The <c>processContents</c> attribute value; <see cref="WildcardProcessContents.Strict"/> when absent.</param>
/// <param name="NodePath">Path of the originating node inside the schema document.</param>
public sealed record OpenContentWildcard(
    IReadOnlyList<string> NamespaceConstraint,
    WildcardProcessContents ProcessContents,
    string? NodePath);

/// <summary>
/// A compiled XSD 1.1 open content component: an explicit <c>xs:openContent</c> on a complex type
/// (or on a complex content restriction/extension), or an <c>xs:defaultOpenContent</c> on a schema
/// document (REQ-009 S5a). Structural enforcement against instance content is a later phase.
/// </summary>
/// <param name="Mode">The open content mode; <see cref="OpenContentMode.None"/> carries no wildcard.</param>
/// <param name="Wildcard">The open content wildcard, or null when <paramref name="Mode"/> is
/// <see cref="OpenContentMode.None"/>.</param>
/// <param name="AppliesToEmpty">For <c>xs:defaultOpenContent</c>: whether the default also applies to
/// complex types with empty content; always false for explicit <c>xs:openContent</c>.</param>
/// <param name="NodePath">Path of the originating node inside the schema document.</param>
public sealed record OpenContentDefinition(
    OpenContentMode Mode,
    OpenContentWildcard? Wildcard,
    bool AppliesToEmpty,
    string? NodePath);
