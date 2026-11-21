// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : Simple type declaration records for the REQ-009 compiled schema model (facet evaluation).
// SPECIAL NOTES        : Part of the REQ-009 XSD 1.1 staged support (ADR-005); S3 slice. Records named
//                        xs:simpleType restrictions carrying xs:assertion facets so the validation service
//                        can evaluate a lexical value against a named type's facets with $value bound.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S3)                                                                    |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>
/// A named <c>xs:simpleType</c> restriction carrying <c>xs:assertion</c> facets, recorded by the
/// REQ-009 compile pipeline. The validation service resolves a lexical value against the
/// restriction's built-in base chain and evaluates these facets with <c>$value</c> bound to the
/// typed value (REQ-009 S3).
/// </summary>
/// <param name="TypeNamespace">Resolved namespace URI of the declared type name (named simple types
/// live in the schema's target namespace; unprefixed type names resolve there per the XSD rule).</param>
/// <param name="TypeLocalName">Local name of the declared type.</param>
/// <param name="BaseTypeNamespace">Resolved namespace URI of the <c>xs:restriction base</c>, or null
/// when the simple type has no restriction parent recorded.</param>
/// <param name="BaseTypeLocalName">Local name of the <c>xs:restriction base</c>, or null.</param>
/// <param name="Assertions">The <c>xs:assertion</c> facet definitions owned by this type (same
/// instances as <see cref="CompiledSchema.Assertions"/>).</param>
/// <param name="NodePath">Path of the originating node inside the schema document.</param>
public sealed record SimpleTypeDeclaration(
    string TypeNamespace,
    string TypeLocalName,
    string? BaseTypeNamespace,
    string? BaseTypeLocalName,
    IReadOnlyList<AssertionDefinition> Assertions,
    string? NodePath);
