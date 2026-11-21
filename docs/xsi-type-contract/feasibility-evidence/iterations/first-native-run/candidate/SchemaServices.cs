// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : Activation-free factory for the staged XSD 1.1 compiled schema pipeline.
// SPECIAL NOTES        : S1 of REQ-009; the returned compiler never touches the core XmlSchemaSet seam (XSD 1.0 only).
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S1)                                                                    |
//                      | Charles Korthout | 0.2   | 27-10-2026     | Added CreateValidationService (REQ-009 S2 assertion evaluation)                          |
//                      | Charles Korthout | 0.3   | 29-10-2026     | Make schema compilation and validation factories activation-free                         |

//                      | Charles Korthout | 0.4   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>
/// Creates the REQ-009 compiled XSD pipeline entry points: an <see cref="IXsdSchemaCompiler"/>
/// (S1) that compiles XSD 1.0/1.1 documents into the immutable <see cref="ICompiledSchema"/> model
/// without flowing through the core engine's XSD 1.0-only <c>XmlSchemaSet</c> seam, and an
/// <see cref="IXsdValidationService"/> (S2) that evaluates the recorded assertions against
/// candidate elements (ADR-005).
/// </summary>
/// <remarks>Available under Apache-2.0; no license activation is required.</remarks>
public static class SchemaServices
{
    /// <summary>
    /// Creates an <see cref="IXsdSchemaCompiler"/> that parses XSD documents with hardened
    /// <see cref="System.Xml.XmlReader"/> settings, records XSD 1.1 assertion/alternative
    /// components, computes the stable interpretation digest, and resolves imports/includes
    /// only through the caller-supplied <see cref="ISchemaResourceResolver"/> policy.
    /// </summary>
    /// <returns>A schema compiler honoring <see cref="SchemaCompileOptions"/>.</returns>
    public static IXsdSchemaCompiler CreateSchemaCompiler()
    {
        return new SchemaCompiler();
    }

    /// <summary>
    /// Creates an <see cref="IXsdValidationService"/> that evaluates the recorded <c>xs:assert</c>
    /// components of a compiled schema against candidate elements, enforcing the W3C-constrained
    /// XPath 2.0 assertion environment (captured namespace bindings, forbidden-function pre-screen)
    /// and reporting explicit findings — violated assertions, refused expressions, evaluation errors.
    /// </summary>
    /// <returns>An assertion validation service with per-service compiled-assertion caching.</returns>
    public static IXsdValidationService CreateValidationService()
    {
        return new XsdValidationService();
    }
}
