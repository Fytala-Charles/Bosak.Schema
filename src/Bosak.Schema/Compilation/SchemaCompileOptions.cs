// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : Compile options and compiler contract for the REQ-009 S1 compile pipeline.
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

using System.Xml;

namespace Bosak.Schema;

/// <summary>
/// Options for compiling a schema document.
/// </summary>
/// <param name="Version">The explicit XSD language version to compile against. Never inferred from
/// document content alone: 1.0 compiles reject 1.1-only constructs with explicit diagnostics.</param>
/// <param name="Resolver">Resolver for <c>xs:import</c>/<c>xs:include</c> schemaLocation hints, or null
/// to compile the single supplied document without resolving references (referenced imports then
/// produce <see cref="SchemaDiagnosticCodes.ResolutionUnavailable"/> diagnostics).</param>
/// <param name="MaxImportDepth">Maximum import/include chain depth (bounded, default 32).</param>
public sealed record SchemaCompileOptions(
    XsdLanguageVersion Version,
    ISchemaResourceResolver? Resolver = null,
    int MaxImportDepth = 32)
{
    /// <summary>Default options for the given version: no resolver, default import depth.</summary>
    public static SchemaCompileOptions Default(XsdLanguageVersion version)
    {
        return new SchemaCompileOptions(version);
    }
}

/// <summary>
/// Compiles XSD schema documents into immutable <see cref="ICompiledSchema"/> models (REQ-009 S1).
/// </summary>
public interface IXsdSchemaCompiler
{
    /// <summary>
    /// Compiles the schema document read from <paramref name="reader"/>.
    /// </summary>
    /// <param name="reader">Reader positioned on the schema document. The caller controls its settings;
    /// use the <see cref="Compile(Stream, Uri, SchemaCompileOptions)"/> overload for a hardened reader
    /// (DTD prohibited, no embedded resolution).</param>
    /// <param name="baseUri">Absolute base URI used to resolve relative import/include hints.</param>
    /// <param name="options">Compile options (version, resolver, import depth bound).</param>
    /// <returns>The immutable compiled schema; observations are reported via <see cref="ICompiledSchema.Diagnostics"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="reader"/>, <paramref name="baseUri"/>, or <paramref name="options"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="baseUri"/> is not absolute.</exception>
    ICompiledSchema Compile(XmlReader reader, Uri baseUri, SchemaCompileOptions options);

    /// <summary>
    /// Compiles the schema document read from <paramref name="stream"/> with hardened reader settings
    /// (DTD processing prohibited, no embedded XML resolution) — the recommended entry point.
    /// </summary>
    /// <param name="stream">Stream positioned at the start of the schema document.</param>
    /// <param name="baseUri">Absolute base URI used to resolve relative import/include hints.</param>
    /// <param name="options">Compile options (version, resolver, import depth bound).</param>
    /// <returns>The immutable compiled schema; observations are reported via <see cref="ICompiledSchema.Diagnostics"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/>, <paramref name="baseUri"/>, or <paramref name="options"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="baseUri"/> is not absolute.</exception>
    /// <exception cref="XmlException">The document is not well-formed XML or declares a DTD (DTD is prohibited).</exception>
    ICompiledSchema Compile(Stream stream, Uri baseUri, SchemaCompileOptions options);
}
