// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 September 2026
// PURPOSE              : Activation-free entry points for schema-aware XSLT compilation.
// SPECIAL NOTES        : Opt-in compiler configuration; the ordinary Bosak core defaults remain unchanged.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-09-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 05-10-2026     | Runtime expiry/grace check at the gate (REQ-002/004)                                   |
//                      | Charles Korthout | 0.3   | 27-10-2026     | Gate extracted to LicenseGate; delegates to shared guard (REQ-009)                       |
//                      | Charles Korthout | 0.4   | 29-10-2026     | Remove the commercial activation gate from schema-aware compilation                      |

//                      | Charles Korthout | 0.5   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      | Charles Korthout | 0.6   | 17-11-2026     | Add opt-in restricted schema acquisition through the published Core policy               |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.Xslt.Api;
using Bosak.XPath.Runtime.Resources;

namespace Bosak.Schema;

/// <summary>
/// Creates schema-aware <see cref="XsltCompiler"/> instances without license activation.
/// </summary>
/// <remarks>Schema-aware compilation is explicitly selected; ordinary compiler defaults are unchanged.</remarks>
public static class SchemaAwareXslt
{
    /// <summary>
    /// Creates an <see cref="XsltCompiler"/> with schema-aware compilation enabled
    /// (<see cref="XsltCompiler.SchemaAware"/>), so stylesheets containing
    /// <c>xsl:import-schema</c>, <c>validation</c>, and <c>@type</c> compile instead of
    /// raising XTSE1650/XTSE1660. Schemas are located by the declaration's
    /// <c>schema-location</c> hints relative to each module's base URI.
    /// </summary>
    /// <returns>A schema-aware compiler.</returns>
    public static XsltCompiler CreateCompiler()
    {
        return new XsltCompiler { SchemaAware = true };
    }

    /// <summary>
    /// Creates an <see cref="XsltCompiler"/> with schema-aware compilation enabled and a
    /// schema resolver that serves <c>xsl:import-schema</c> <c>schema-location</c> hints from
    /// files in <paramref name="schemaDirectory"/>. Hints that do not resolve to a file fall
    /// back to the core's default URI resolution.
    /// </summary>
    /// <param name="schemaDirectory">Directory containing the XSD files referenced by schema-location hints.</param>
    /// <returns>A schema-aware compiler with a file-system schema resolver.</returns>
    /// <exception cref="ArgumentException"><paramref name="schemaDirectory"/> is null, empty, or whitespace.</exception>
    public static XsltCompiler CreateCompiler(string schemaDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(schemaDirectory);

        return new XsltCompiler
        {
            SchemaAware = true,
            SchemaResolver = (_, hints) =>
            {
                foreach (var hint in hints)
                {
                    var candidate = Path.Combine(schemaDirectory, hint);
                    if (File.Exists(candidate))
                    {
                        return File.OpenRead(candidate);
                    }
                }

                return null;
            },
        };
    }

    /// <summary>Creates a schema-aware compiler that restricts all schema acquisition to an explicit directory.</summary>
    /// <param name="schemaDirectory">Absolute existing trusted directory without reparse-point ancestors.</param>
    /// <param name="maxDocumentBytes">Positive per-schema XML byte limit.</param>
    /// <returns>A compiler with a controlled policy approving only contained schema documents.</returns>
    /// <remarks>Supply an absolute stylesheet base URI for relative schema hints. Nested schema references use
    /// the same policy. Other acquisition routes are denied, without fallback. The host must retain this policy
    /// and prevent concurrent filesystem mutation; this is not an operating-system sandbox.</remarks>
    /// <exception cref="ArgumentException">The directory is null, blank or not fully qualified.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The byte limit is not positive.</exception>
    /// <exception cref="SchemaResolutionException">The directory cannot satisfy the restricted policy.</exception>
    public static XsltCompiler CreateRestrictedCompiler(string schemaDirectory, int maxDocumentBytes)
    {
        var resolver = new RestrictedSchemaFileResolver(schemaDirectory, maxDocumentBytes);
        return new XsltCompiler { SchemaAware = true, ResourcePolicy = new ControlledResourcePolicy(new SchemaAuthority(resolver)) };
    }

    private sealed class SchemaAuthority(RestrictedSchemaFileResolver resolver) : IControlledResourceAuthority
    {
        /// <summary>Approves only schema-route bytes that satisfy the restricted resolver.</summary>
        /// <param name="request">The engine-resolved acquisition request.</param>
        /// <returns>Approved schema bytes or a fixed denial without alternate resolution.</returns>
        public ControlledResourceResponse Authorize(ControlledResourceRequest request)
        {
            if (request.Route != ControlledResourceRoute.Schema || !Uri.TryCreate(request.Uri, UriKind.Absolute, out var uri))
                return ControlledResourceResponse.Deny("Only restricted schema acquisition is permitted.");
            try { return ControlledResourceResponse.Approve(resolver.ReadBytes(uri), uri.AbsoluteUri); }
            catch (SchemaResolutionException) { return ControlledResourceResponse.Deny("Restricted schema acquisition refused."); }
        }
    }

}
