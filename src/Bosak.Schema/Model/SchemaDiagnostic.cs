// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : Explicit diagnostic record and stable diagnostic codes for the REQ-009 compile pipeline.
// SPECIAL NOTES        : Part of the REQ-009 XSD 1.1 staged support (ADR-005); S1 slice. Diagnostics are explicit
//                        and stable-coded — unsupported constructs are never silently accepted.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 27-10-2026     | Added assertion-evaluation codes BSK-XSD-007..009 (REQ-009 S2)                           |
//                      | Charles Korthout | 0.3   | 27-10-2026     | Added BSK-XSD-010 SimpleTypeNotFound (REQ-009 S3 facet evaluation)                       |
//                      | Charles Korthout | 0.4   | 27-10-2026     | Added BSK-XSD-013/014 alternative diagnostics (REQ-009 S4a)                              |
//                      | Charles Korthout | 0.5   | 27-10-2026     | Added BSK-XSD-015 AlternativeNotApplicable (REQ-009 S4b)                                 |

//                      | Charles Korthout | 0.6   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      | Charles Korthout | 0.7   | 29-10-2026     | Add explicit native assessment coverage and invalid-instance diagnostics                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>
/// A single compile or validation observation: a stable machine-readable code, a human-readable
/// message, and the node path inside the schema document where the observation originated.
/// </summary>
/// <param name="Code">Stable diagnostic code (see <see cref="SchemaDiagnosticCodes"/>).</param>
/// <param name="Message">Human-readable description of the observation.</param>
/// <param name="NodePath">Path of the originating node inside the schema document, or null when not applicable.</param>
public sealed record SchemaDiagnostic(string Code, string Message, string? NodePath)
{
    /// <inheritdoc/>
    public override string ToString()
    {
        return NodePath is null ? $"{Code}: {Message}" : $"{Code} at {NodePath}: {Message}";
    }
}

/// <summary>
/// Stable diagnostic codes emitted by the REQ-009 compile pipeline. Codes are part of the
/// add-on's public contract: consumers may match on them, and the conformance posture
/// (explicit diagnostics, never silent acceptance) depends on their stability.
/// </summary>
public static class SchemaDiagnosticCodes
{
    /// <summary>A native document assessment requests unsupported profile or representation coverage.</summary>
    public const string AssessmentUnsupported = "BSK-XSD-016";

    /// <summary>An instance violates a supported native structural or lexical constraint.</summary>
    public const string AssessmentInvalidInstance = "BSK-XSD-017";

    /// <summary>The document root is not an <c>xs:schema</c> element in the XML Schema namespace.</summary>
    public const string RootNotSchema = "BSK-XSD-001";

    /// <summary>An XSD 1.1-only construct was encountered while compiling against <see cref="XsdLanguageVersion.Xsd10"/>.</summary>
    public const string ConstructRequiresXsd11 = "BSK-XSD-002";

    /// <summary>A recognized XSD 1.1 construct is not yet supported by the current REQ-009 phase.</summary>
    public const string UnsupportedConstruct = "BSK-XSD-003";

    /// <summary>Schema resolution (import/include) was denied by the resolution policy.</summary>
    public const string ResolutionDenied = "BSK-XSD-004";

    /// <summary>Schema resolution exceeded a bounded limit (import depth or resolution count).</summary>
    public const string ResolutionLimit = "BSK-XSD-005";

    /// <summary>A referenced schema document could not be resolved (no resolver supplied or resolution failure).</summary>
    public const string ResolutionUnavailable = "BSK-XSD-006";

    /// <summary>An assertion expression was refused: it uses a forbidden function or fails to compile under the W3C-constrained XPath 2.0 assertion environment.</summary>
    public const string AssertionRefused = "BSK-XSD-007";

    /// <summary>An assertion expression raised an error while being evaluated against the candidate element.</summary>
    public const string AssertionEvaluationError = "BSK-XSD-008";

    /// <summary>An assertion evaluated successfully with an effective boolean value of false — the candidate element violates the assertion.</summary>
    public const string AssertionViolated = "BSK-XSD-009";

    /// <summary>The named simple type was not found in the compiled schema — facet evaluation has nothing to evaluate against.</summary>
    public const string SimpleTypeNotFound = "BSK-XSD-010";

    /// <summary>An <c>xs:openContent</c> or <c>xs:defaultOpenContent</c> element violates the XSD 1.1 composition rules (wrong parent, ordering, duplicates, disallowed mode, or a missing/misplaced <c>xs:any</c> child).</summary>
    public const string InvalidOpenContent = "BSK-XSD-011";

    /// <summary>A default-attributes construct is malformed: the <c>xs:complexType</c> <c>defaultAttributesApply</c> attribute is not a valid boolean.</summary>
    public const string InvalidDefaultAttributes = "BSK-XSD-012";

    /// <summary>An <c>xs:alternative</c> names a <c>type</c> whose QName prefix could not be resolved in the schema document.</summary>
    public const string AlternativeTypeUnresolved = "BSK-XSD-013";

    /// <summary>An element declaration's <c>xs:alternative</c> set violates the CTA rules: more than one default alternative (no <c>test</c>), or a default alternative that is not the final alternative of its declaration.</summary>
    public const string InvalidAlternativeSet = "BSK-XSD-014";

    /// <summary>Conditional type assignment found no matching alternative and the declaration has no default alternative — the element has no governing type.</summary>
    public const string AlternativeNotApplicable = "BSK-XSD-015";
}
