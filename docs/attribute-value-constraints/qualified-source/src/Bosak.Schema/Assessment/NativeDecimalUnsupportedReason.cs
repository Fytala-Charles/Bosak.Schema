// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Classifies detected Unsupported refusals without caller input or authority.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>Classifies one detected Unsupported refusal; not an exhaustive compatibility report.</summary>
/// <remarks>Consumers must tolerate Unspecified and future values. Categories do not expand the native grammar.</remarks>
public enum NativeDecimalUnsupportedReason
{
    /// <summary>No category is supplied; also used for Invalid diagnostics.</summary>
    Unspecified = 0,
    /// <summary>An existing check refused schema namespace settings.</summary>
    Namespace = 1,
    /// <summary>An existing check refused the schema declaration or content shape.</summary>
    DeclarationShape = 2,
    /// <summary>A local declaration does not use the admitted decimal type.</summary>
    ValueType = 3,
    /// <summary>An existing check refused schema or instance attributes.</summary>
    Attributes = 4,
    /// <summary>An assertion lies outside the finite expression grammar.</summary>
    AssertionGrammar = 5,
    /// <summary>An explicit byte, node, lexical, numeric or assertion capacity was exceeded.</summary>
    RepresentationCapacity = 6
}
