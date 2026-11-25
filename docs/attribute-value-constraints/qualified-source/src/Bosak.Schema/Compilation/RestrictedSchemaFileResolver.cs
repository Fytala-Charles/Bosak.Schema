// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 17 November 2026
// PURPOSE              : Resolves bounded XML schema files within an explicit trusted directory.
// SPECIAL NOTES        : Requires stable host-owned paths; does not provide an atomic filesystem sandbox.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 17-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Xml;

namespace Bosak.Schema;

/// <summary>Resolves bounded XML schema files within an explicit trusted directory.</summary>
/// <remarks>Rejects reparse points along the complete path, but checks and file opening are not atomic.
/// The host must prevent concurrent path/link replacement and supply ordinary files in a trusted filesystem.
/// Returned bytes are detached and read-only. This is not an operating-system sandbox.</remarks>
public sealed class RestrictedSchemaFileResolver : ISchemaResourceResolver
{
    private readonly string root;
    private readonly int maxDocumentBytes;

    /// <summary>Creates an opt-in resolver with explicit directory and per-document byte limits.</summary>
    /// <param name="schemaDirectory">An absolute existing directory without reparse-point ancestors.</param>
    /// <param name="maxDocumentBytes">Positive maximum bytes per acquired XML document.</param>
    /// <exception cref="ArgumentException">The directory is null, blank, or not fully qualified.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The byte limit is not positive.</exception>
    /// <exception cref="SchemaResolutionException">The directory is unavailable, nonlocal, or uses a reparse point.</exception>
    public RestrictedSchemaFileResolver(string schemaDirectory, int maxDocumentBytes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(schemaDirectory);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxDocumentBytes);
        if (!Path.IsPathFullyQualified(schemaDirectory))
            throw new ArgumentException("An absolute schema directory is required.", nameof(schemaDirectory));
        root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(schemaDirectory));
        this.maxDocumentBytes = maxDocumentBytes;
        if (new Uri(root).IsUnc) throw Denied();
        try { CheckDirectories(root); }
        catch (IOException) { throw Unavailable(); }
        catch (UnauthorizedAccessException) { throw Unavailable(); }
    }

    /// <summary>Reads a contained XML document after checking path, byte and XML policies.</summary>
    /// <param name="resolvedUri">Absolute local file URI without query, fragment or credentials.</param>
    /// <returns>A detached read-only stream positioned at zero; the caller owns disposal.</returns>
    /// <exception cref="ArgumentNullException">The URI is null.</exception>
    /// <exception cref="SchemaResolutionException">The path, XML or size is denied, or the file is unavailable.</exception>
    public Stream Open(Uri resolvedUri) => new MemoryStream(ReadBytes(resolvedUri), writable: false);

    internal byte[] ReadBytes(Uri resolvedUri)
    {
        ArgumentNullException.ThrowIfNull(resolvedUri);
        if (!resolvedUri.IsAbsoluteUri || !resolvedUri.IsFile || resolvedUri.IsUnc ||
            !string.IsNullOrEmpty(resolvedUri.Host) || !string.IsNullOrEmpty(resolvedUri.UserInfo) ||
            !string.IsNullOrEmpty(resolvedUri.Query) || !string.IsNullOrEmpty(resolvedUri.Fragment)) throw Denied();
        try
        {
            // Uri.LocalPath already removes some Windows trailing-dot/space spellings. Refuse
            // those aliases from the original spelling before asking Path for a canonical path.
            if (OperatingSystem.IsWindows() && Uri.UnescapeDataString(resolvedUri.OriginalString)
                .Split('\\', '/').Any(part => part is not "." and not ".." && (part.EndsWith('.') || part.EndsWith(' ')))) throw Denied();
            var path = Path.GetFullPath(resolvedUri.LocalPath);
            var prefix = Path.EndsInDirectorySeparator(root) ? root : root + Path.DirectorySeparatorChar;
            // Require the captured root's exact spelling, including on Windows directories with
            // case-sensitive lookup enabled. A platform-insensitive relative path is not sufficient.
            if (!path.StartsWith(prefix, StringComparison.Ordinal)) throw Denied();
            var relative = Path.GetRelativePath(root, path);
            if (Path.IsPathRooted(relative) || relative == "." || relative == ".." ||
                relative.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal) ||
                relative.StartsWith(".." + Path.AltDirectorySeparatorChar, StringComparison.Ordinal)) throw Denied();
            if (OperatingSystem.IsWindows() && relative.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                .Any(part => part.Contains(':') || part.EndsWith('.') || part.EndsWith(' '))) throw Denied();
            CheckDirectories(Path.GetDirectoryName(path)!);
            var attributes = File.GetAttributes(path);
            if ((attributes & (FileAttributes.ReparsePoint | FileAttributes.Directory | FileAttributes.Device)) != 0) throw Denied();
            using var source = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var buffer = new MemoryStream();
            var chunk = new byte[8192];
            while (true)
            {
                var count = source.Read(chunk, 0, (int)Math.Min(chunk.Length, (long)maxDocumentBytes - buffer.Length + 1));
                if (count == 0) break;
                if (buffer.Length + count > maxDocumentBytes) throw Denied();
                buffer.Write(chunk, 0, count);
            }
            buffer.Position = 0;
            using (var reader = XmlReader.Create(buffer, new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null,
                MaxCharactersInDocument = maxDocumentBytes,
            }))
            {
                while (reader.Read()) { }
            }
            return buffer.ToArray();
        }
        catch (XmlException) { throw Denied(); }
        catch (ArgumentException) { throw Denied(); }
        catch (IOException) { throw Unavailable(); }
        catch (UnauthorizedAccessException) { throw Unavailable(); }
    }

    private static void CheckDirectories(string directory)
    {
        for (var current = new DirectoryInfo(directory); current is not null; current = current.Parent)
        {
            var attributes = File.GetAttributes(current.FullName);
            if ((attributes & FileAttributes.ReparsePoint) != 0 || (attributes & FileAttributes.Directory) == 0)
                throw Denied();
        }
    }

    private static SchemaResolutionException Denied() =>
        new(SchemaDiagnosticCodes.ResolutionDenied, "Restricted schema file resolution denied.");
    private static SchemaResolutionException Unavailable() =>
        new(SchemaDiagnosticCodes.ResolutionUnavailable, "Restricted schema file is unavailable.");
}
