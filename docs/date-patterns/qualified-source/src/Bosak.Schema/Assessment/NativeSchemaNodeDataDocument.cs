// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Reads detached scalar data from original assessed element and attribute nodes.
// SPECIAL NOTES        : Internal Core 1.81.2 adapter; finite profile only, no public native factory.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;

namespace Bosak.Schema;

/// <summary>Reads detached scalar data from original assessed element and attribute nodes.</summary>
/// <remarks>Owned by its creating session. Paths count element children only; an empty path selects the root.
/// Operations must be sequential. Returned values and XML carry no transferable schema authority.</remarks>
public sealed class NativeSchemaNodeDataDocument : IDisposable
{
    private readonly NativeNodeDataCore.Document document;
    private NativeSchemaNodeDataDocument(NativeNodeDataCore.Document document) => this.document = document;
    internal static NativeSchemaNodeDataDocument Own(NativeNodeDataCore.Document document) => new(document);

    /// <summary>Atomizes one original element and returns its detached scalar typed value.</summary>
    /// <param name="elementPath">Explicit immutable element-child indexes, at most three; empty selects root.</param>
    /// <param name="cancellationToken">Cancellation checked before and during the read.</param>
    /// <returns>One detached string, Boolean, exact integer or exact decimal.</returns>
    /// <exception cref="ArgumentException">The path is default.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The path is too deep, negative or absent.</exception>
    /// <exception cref="ObjectDisposedException">This document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Overlap, query capacity, cache capacity or no typed value (FOTY0012).</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested.</exception>
    public NativeScalarValue ReadElement(ImmutableArray<int> elementPath, CancellationToken cancellationToken = default) =>
        document.ReadElement(elementPath, cancellationToken);

    /// <summary>Atomizes a present original attribute selected by owner path and expanded name.</summary>
    /// <param name="ownerElementPath">Explicit element-child indexes; empty selects the root owner.</param>
    /// <param name="namespaceUri">Exact namespace URI; empty selects no namespace.</param>
    /// <param name="localName">Exact XML NCName.</param>
    /// <param name="cancellationToken">Cancellation checked before and during the read.</param>
    /// <returns>One detached string, Boolean, exact integer or exact decimal.</returns>
    /// <exception cref="ArgumentNullException">Namespace URI or local name is null.</exception>
    /// <exception cref="ArgumentException">Default path, invalid NCName or missing attribute.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The owner path is too deep, negative or absent.</exception>
    /// <exception cref="ObjectDisposedException">This document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Overlap, query/cache capacity or an unassessed attribute.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested.</exception>
    public NativeScalarValue ReadAttribute(ImmutableArray<int> ownerElementPath, string namespaceUri, string localName,
        CancellationToken cancellationToken = default) => document.ReadAttribute(ownerElementPath, namespaceUri, localName, cancellationToken);

    /// <summary>Exports bounded parsed lexical XML without PSVI or transferable authority.</summary>
    /// <param name="cancellationToken">Cancellation checked before and during serialization.</param>
    /// <returns>Detached XML, without a byte-preservation or canonicalization guarantee.</returns>
    /// <exception cref="ObjectDisposedException">This document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Another operation is admitted or export capacity is exceeded.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested.</exception>
    public string ExportXml(CancellationToken cancellationToken = default) => document.ExportXml(cancellationToken);

    /// <summary>Revokes this document, waits for admitted work and attempts each owned cleanup once.</summary>
    /// <remarks>Repeated disposal is harmless; the parent session remains usable.</remarks>
    /// <exception cref="InvalidOperationException">Cleanup failed after all actions were attempted.</exception>
    public void Dispose() => document.Dispose();
}
