// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : Default attributes component definitions for the REQ-009 compiled schema model (REQ-009 S5b).
// SPECIAL NOTES        : Part of the REQ-009 XSD 1.1 staged support (ADR-005); S5b records the components —
//                        computing the effective attribute uses per complex type (W3C XML Schema 1.1 Part 1
//                        §3.4.2.4) is structural work for the engine/structural-validator phase.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S5b: xs:schema defaultAttributes + complexType defaultAttributesApply) |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>
/// An XSD 1.1 <c>xs:schema defaultAttributes="QName"</c> attribute: a reference to a global attribute
/// group whose attribute uses are applied by default to every complex type in that schema document
/// (W3C XML Schema 1.1 Part 1 §3.4.2.4). Recorded per schema document of the compiled set.
/// </summary>
/// <param name="AttributeGroupNamespace">Resolved namespace URI of the referenced attribute group name
/// (an unprefixed QName has no namespace, per QName attribute semantics).</param>
/// <param name="AttributeGroupLocalName">Local name of the referenced attribute group.</param>
/// <param name="NodePath">Path of the originating node inside the schema document.</param>
public sealed record DefaultAttributesReference(
    string AttributeGroupNamespace,
    string AttributeGroupLocalName,
    string? NodePath);

/// <summary>
/// A complex type opting out of the schema's default attributes via
/// <c>defaultAttributesApply="false"</c> (W3C XML Schema 1.1 Part 1 §3.4.2.4). Anonymous local
/// complex types have a null <paramref name="ComplexTypeName"/>; their <paramref name="NodePath"/>
/// identifies them.
/// </summary>
/// <param name="ComplexTypeName">The owning <c>xs:complexType</c>'s <c>name</c> attribute, or null
/// for an anonymous local complex type.</param>
/// <param name="NodePath">Path of the originating node inside the schema document.</param>
public sealed record DefaultAttributesOptOut(
    string? ComplexTypeName,
    string? NodePath);
