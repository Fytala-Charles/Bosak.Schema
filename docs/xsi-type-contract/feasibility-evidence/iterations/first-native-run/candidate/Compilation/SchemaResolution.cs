// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : Resolver contract and policy-based secure implementation for REQ-009 schema resolution.
// SPECIAL NOTES        : Part of the REQ-009 XSD 1.1 staged support (ADR-005); S1 slice. Security posture:
//                        default deny network and DTD, optional URI-prefix allowlist, bounded size — every
//                        denial throws with a stable code (testable controls).
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

namespace Bosak.Schema;

/// <summary>
/// Opens schema documents referenced by <c>xs:import</c> / <c>xs:include</c> schemaLocation hints.
/// </summary>
/// <remarks>
/// Implementations enforce their own security posture. The default implementation returned by
/// <see cref="SchemaServices"/> is <see cref="SecureSchemaResolver"/> with
/// <see cref="SchemaResolutionPolicy.LocalFiles"/>: local files only, no network, no DTD.
/// </remarks>
public interface ISchemaResourceResolver
{
    /// <summary>
    /// Opens the resolved schema document.
    /// </summary>
    /// <param name="resolvedUri">Absolute URI of the schema document to open.</param>
    /// <returns>A readable stream positioned at the start of the document.</returns>
    /// <exception cref="SchemaResolutionException">The URI is denied by policy or exceeds a bounded limit.</exception>
    Stream Open(Uri resolvedUri);
}

/// <summary>
/// Security policy for <see cref="ISchemaResourceResolver"/> implementations: which URI schemes are
/// allowed, whether DTDs are tolerated, an optional allowlist of URI prefixes, and a per-document
/// byte cap.
/// </summary>
/// <param name="AllowLocalFiles">Allow <c>file</c> URIs. Default-deny posture keeps this off unless set.</param>
/// <param name="AllowNetwork">Allow <c>http</c>/<c>https</c> URIs. Off in every shipped default.</param>
/// <param name="AllowDtd">Allow documents declaring a DTD. Off in every shipped default.</param>
/// <param name="MaxDocumentBytes">Per-document byte cap enforced while reading.</param>
/// <param name="AllowedUriPrefixes">Optional allowlist; when non-null, URIs must start with one of these prefixes.</param>
public sealed record SchemaResolutionPolicy(
    bool AllowLocalFiles,
    bool AllowNetwork,
    bool AllowDtd,
    long MaxDocumentBytes,
    IReadOnlyList<string>? AllowedUriPrefixes = null)
{
    /// <summary>Local files allowed; network and DTD denied; 10 MiB per-document cap; no allowlist.</summary>
    public static SchemaResolutionPolicy LocalFiles { get; } = new(true, false, false, 10 * 1024 * 1024, null);

    /// <summary>Deny every resolution (default-deny posture for untrusted input).</summary>
    public static SchemaResolutionPolicy DenyAll { get; } = new(false, false, false, 0, null);
}

/// <summary>
/// Thrown when schema resolution is denied by policy or exceeds a bounded limit.
/// </summary>
public sealed class SchemaResolutionException : InvalidOperationException
{
    /// <summary>
    /// Creates the exception with a stable machine-readable code and a human-readable reason.
    /// </summary>
    /// <param name="code">Stable code (see <see cref="SchemaDiagnosticCodes"/>).</param>
    /// <param name="message">Human-readable description of the denial.</param>
    public SchemaResolutionException(string code, string message)
        : base(message)
    {
        Code = code;
    }

    /// <summary>Stable machine-readable denial code.</summary>
    public string Code { get; }
}

/// <summary>
/// Policy-enforcing <see cref="ISchemaResourceResolver"/>: default deny network and DTD, optional
/// URI-prefix allowlist, per-document byte cap. Every control is explicit and testable.
/// </summary>
public sealed class SecureSchemaResolver : ISchemaResourceResolver
{
    private readonly SchemaResolutionPolicy _policy;

    /// <summary>
    /// Creates a resolver enforcing <paramref name="policy"/>.
    /// </summary>
    /// <param name="policy">The policy to enforce; use <see cref="SchemaResolutionPolicy.DenyAll"/> for untrusted input.</param>
    /// <exception cref="ArgumentNullException"><paramref name="policy"/> is null.</exception>
    public SecureSchemaResolver(SchemaResolutionPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(policy);
        _policy = policy;
    }

    /// <inheritdoc/>
    /// <exception cref="SchemaResolutionException">The URI is denied by scheme, allowlist, or DTD policy, or exceeds the byte cap.</exception>
    public Stream Open(Uri resolvedUri)
    {
        ArgumentNullException.ThrowIfNull(resolvedUri);

        if (!resolvedUri.IsAbsoluteUri)
        {
            throw new SchemaResolutionException(
                SchemaDiagnosticCodes.ResolutionDenied,
                $"Schema resolution denied: '{resolvedUri}' is not an absolute URI.");
        }

        if (_policy.AllowedUriPrefixes is { Count: > 0 } prefixes &&
            !prefixes.Any(p => resolvedUri.AbsoluteUri.StartsWith(p, StringComparison.Ordinal)))
        {
            throw new SchemaResolutionException(
                SchemaDiagnosticCodes.ResolutionDenied,
                $"Schema resolution denied: '{resolvedUri}' is not in the allowed URI prefix list.");
        }

        var scheme = resolvedUri.Scheme;
        var schemeAllowed = scheme.Equals(Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase)
            ? _policy.AllowLocalFiles
            : scheme is "http" or "https"
                ? _policy.AllowNetwork
                : false;
        if (!schemeAllowed)
        {
            throw new SchemaResolutionException(
                SchemaDiagnosticCodes.ResolutionDenied,
                $"Schema resolution denied: scheme '{scheme}' of '{resolvedUri}' is not allowed by policy.");
        }

        using var source = OpenSource(resolvedUri);
        var buffer = new MemoryStream();
        var cap = _policy.MaxDocumentBytes;
        var chunk = new byte[81920];
        long total = 0;
        int read;
        while ((read = source.Read(chunk, 0, chunk.Length)) > 0)
        {
            total += read;
            if (total > cap)
            {
                throw new SchemaResolutionException(
                    SchemaDiagnosticCodes.ResolutionDenied,
                    $"Schema resolution denied: '{resolvedUri}' exceeds the per-document limit of {cap} bytes.");
            }

            buffer.Write(chunk, 0, read);
        }

        if (!_policy.AllowDtd && ContainsDoctype(buffer.GetBuffer(), (int)buffer.Length))
        {
            throw new SchemaResolutionException(
                SchemaDiagnosticCodes.ResolutionDenied,
                $"Schema resolution denied: '{resolvedUri}' declares a DTD, which is prohibited by policy.");
        }

        buffer.Position = 0;
        return buffer;
    }

    private static Stream OpenSource(Uri uri)
    {
        if (uri.Scheme.Equals(Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase))
        {
            return File.OpenRead(uri.LocalPath);
        }

        throw new SchemaResolutionException(
            SchemaDiagnosticCodes.ResolutionDenied,
            $"Schema resolution denied: no fetcher is available for scheme '{uri.Scheme}'.");
    }

    private static bool ContainsDoctype(byte[] buffer, int length)
    {
        // The prolog precedes the root element, so scanning the first few KiB is sufficient.
        var window = Math.Min(length, 8192);
        var prolog = System.Text.Encoding.UTF8.GetString(buffer, 0, window);
        return prolog.Contains("<!DOCTYPE", StringComparison.Ordinal);
    }
}
