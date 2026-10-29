// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : Immutable compiled schema model produced by the REQ-009 S1 compile pipeline.
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
//                      | Charles Korthout | 0.2   | 27-10-2026     | ICompiledSchema exposes Assertions/Alternatives (REQ-009 S2 validation service)          |
//                      | Charles Korthout | 0.3   | 27-10-2026     | Added ElementDeclarations (typed-view building for REQ-009 S2)                           |
//                      | Charles Korthout | 0.4   | 27-10-2026     | Added SimpleTypes (xs:assertion facet evaluation for REQ-009 S3)                         |

//                      | Charles Korthout | 0.5   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>
/// The result of compiling one schema document (plus its resolved import/include closure)
/// against an explicit <see cref="XsdLanguageVersion"/>.
/// </summary>
public interface ICompiledSchema
{
    /// <summary>Deterministic schema identifier, derived from <see cref="Sha256Digest"/>.</summary>
    string SchemaId { get; }

    /// <summary>SHA-256 digest of the canonical (formatting-insensitive) schema document content.</summary>
    string Sha256Digest { get; }

    /// <summary>The language version the schema was compiled against.</summary>
    XsdLanguageVersion LanguageVersion { get; }

    /// <summary>Compiled XSD 1.1 assertion components (empty for <see cref="XsdLanguageVersion.Xsd10"/> compiles).</summary>
    IReadOnlyList<AssertionDefinition> Assertions { get; }

    /// <summary>Compiled XSD 1.1 conditional type assignment components (empty for <see cref="XsdLanguageVersion.Xsd10"/> compiles).</summary>
    IReadOnlyList<TypeAlternativeDefinition> Alternatives { get; }

    /// <summary>Element declarations carrying an explicit <c>type</c> attribute (typed-view building for assertion evaluation).</summary>
    IReadOnlyList<ElementDeclaration> ElementDeclarations { get; }

    /// <summary>Named simple-type restrictions carrying <c>xs:assertion</c> facets (facet evaluation).</summary>
    IReadOnlyList<SimpleTypeDeclaration> SimpleTypes { get; }

    /// <summary>Explicit XSD 1.1 open content components (<c>xs:openContent</c> on complex types or on
    /// complex content derivation; empty for <see cref="XsdLanguageVersion.Xsd10"/> compiles). Structural
    /// enforcement is a later REQ-009 phase.</summary>
    IReadOnlyList<OpenContentDefinition> OpenContents { get; }

    /// <summary>XSD 1.1 <c>xs:defaultOpenContent</c> components of the resolved schema documents, at most
    /// one per document (empty for <see cref="XsdLanguageVersion.Xsd10"/> compiles).</summary>
    IReadOnlyList<OpenContentDefinition> DefaultOpenContents { get; }

    /// <summary>XSD 1.1 default attribute group references (<c>xs:schema defaultAttributes="QName"</c>;
    /// at most one per schema document; empty for <see cref="XsdLanguageVersion.Xsd10"/> compiles).
    /// Referential integrity and per-type attribute-use computation are later phases.</summary>
    IReadOnlyList<DefaultAttributesReference> DefaultAttributeReferences { get; }

    /// <summary>Complex types opting out of the schema's default attributes via
    /// <c>defaultAttributesApply="false"</c> (empty for <see cref="XsdLanguageVersion.Xsd10"/> compiles).</summary>
    IReadOnlyList<DefaultAttributesOptOut> DefaultAttributeOptOuts { get; }

    /// <summary>Explicit diagnostics: rejections, unsupported constructs, resolution failures. Never silent.</summary>
    IReadOnlyList<SchemaDiagnostic> Diagnostics { get; }
}

/// <summary>
/// Immutable default implementation of <see cref="ICompiledSchema"/> (REQ-009 S1).
/// </summary>
/// <param name="SchemaId">Deterministic schema identifier, derived from <paramref name="Sha256Digest"/>.</param>
/// <param name="Sha256Digest">SHA-256 digest of the canonical schema document content.</param>
/// <param name="LanguageVersion">The language version the schema was compiled against.</param>
/// <param name="TargetNamespace">The <c>targetNamespace</c> of the root schema document, or null.</param>
/// <param name="Assertions">Compiled XSD 1.1 assertion components (empty for <see cref="XsdLanguageVersion.Xsd10"/> compiles).</param>
/// <param name="Alternatives">Compiled XSD 1.1 conditional type assignment components (empty for <see cref="XsdLanguageVersion.Xsd10"/> compiles).</param>
/// <param name="ElementDeclarations">Element declarations carrying an explicit <c>type</c> attribute; used to type candidate content before assertion evaluation.</param>
/// <param name="SimpleTypes">Named simple-type restrictions carrying <c>xs:assertion</c> facets.</param>
/// <param name="OpenContents">Explicit XSD 1.1 open content components (<c>xs:openContent</c>).</param>
/// <param name="DefaultOpenContents"><c>xs:defaultOpenContent</c> components of the resolved schema documents.</param>
/// <param name="DefaultAttributeReferences">XSD 1.1 default attribute group references (<c>xs:schema defaultAttributes</c>).</param>
/// <param name="DefaultAttributeOptOuts">Complex types with <c>defaultAttributesApply="false"</c>.</param>
/// <param name="Diagnostics">Explicit compile diagnostics; unsupported constructs are reported here, never silently accepted.</param>
public sealed record CompiledSchema(
    string SchemaId,
    string Sha256Digest,
    XsdLanguageVersion LanguageVersion,
    string? TargetNamespace,
    IReadOnlyList<AssertionDefinition> Assertions,
    IReadOnlyList<TypeAlternativeDefinition> Alternatives,
    IReadOnlyList<ElementDeclaration> ElementDeclarations,
    IReadOnlyList<SimpleTypeDeclaration> SimpleTypes,
    IReadOnlyList<OpenContentDefinition> OpenContents,
    IReadOnlyList<OpenContentDefinition> DefaultOpenContents,
    IReadOnlyList<DefaultAttributesReference> DefaultAttributeReferences,
    IReadOnlyList<DefaultAttributesOptOut> DefaultAttributeOptOuts,
    IReadOnlyList<SchemaDiagnostic> Diagnostics) : ICompiledSchema;
