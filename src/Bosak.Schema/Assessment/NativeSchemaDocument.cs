// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Exposes detached typed scalar values and bounded XML from owned assessment.
// SPECIAL NOTES        : Bounded opt-in scalar profile; existing decimal facade remains unchanged.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 19-11-2026     | Add bounded opt-in Boolean values while preserving decimal and string contracts             |
//                      | Charles Korthout | 0.3   | 19-11-2026     | Document bounded exact integer leaf support                                                |
//                      | Charles Korthout | 0.4   | 19-11-2026     | Expose separate root attribute reads without changing existing leaf indexes              |
//                      | Charles Korthout | 0.5   | 20-11-2026     | Expose independent root scalar availability and original simple-content typed reads      |
//                      | Charles Korthout | 0.6   | 20-11-2026     | Document terminal-only document-order scalar reads for nested sequences                  |
//                      | Charles Korthout | 0.7   | 20-11-2026     | Expose separate descendant attribute reads without changing root or scalar indexes       |
//                      | Charles Korthout | 0.8   | 20-11-2026     | Document scalar-bearing complex terminals in the unchanged descendant read projection    |
//                      | Charles Korthout | 0.9   | 22-11-2026     | Document simple scalar root reads without widening the API |
//                      | Charles Korthout | 0.10   | 25-11-2026     | Preserve simple nillability and actual typed-empty values |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>Exposes detached scalar values and XML from one owned native assessment.</summary>
/// <remarks>Owned by its creating session. Exported XML and returned values carry no transferable schema authority.</remarks>
public sealed class NativeSchemaDocument : IDisposable
{
    private readonly NativeDecimalSessionCore.Document document;
    internal NativeSchemaDocument(NativeDecimalSessionCore.Document document) => this.document = document;

    /// <summary>Gets the count of present scalar leaves; absent optional children have no index.</summary>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Another session operation is admitted.</exception>
    public int ScalarCount => document.DecimalCount;

    /// <summary>Reads one scalar-bearing descendant through original facts and returns its detached typed value.</summary>
    /// <param name="leafIndex">Zero-based terminal-leaf index in document order; element-only containers and attributes are excluded.</param>
    /// <param name="cancellationToken">Cancellation checked before and during the read.</param>
    /// <returns>A detached scalar or Empty typed sequence usable after document and session disposal.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside the present leaves.</exception>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Overlapping work or a value outside explicit query limits is refused.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested.</exception>
    public NativeScalarValue ReadScalar(int leafIndex, CancellationToken cancellationToken = default) =>
        document.ReadScalar(leafIndex, cancellationToken);

    /// <summary>Gets whether the actual assessed root is nilled.</summary>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Another session operation is admitted.</exception>
    public bool IsRootNilled => document.IsRootNilled;

    /// <summary>Gets whether the assessed root has an admitted scalar typed value.</summary>
    /// <remarks>False for nilled, empty-complex and existing element-only roots; independent of leaf and attribute counts.</remarks>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Another session operation is admitted.</exception>
    public bool HasRootScalar => document.HasRootScalar;

    /// <summary>Reads an admitted simple or simple-content root through its original typed facts.</summary>
    /// <param name="cancellationToken">Cancellation checked before and during the read.</param>
    /// <returns>A detached root scalar or supported Empty typed sequence usable after owner disposal.</returns>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">The root has no typed scalar (FOTY0012), overlapping work is admitted, or query limits are exceeded.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested.</exception>
    public NativeScalarValue ReadRootScalar(CancellationToken cancellationToken = default) => document.ReadRootScalar(cancellationToken);

    /// <summary>Gets the count of present declared root attributes, omitting absent optional uses.</summary>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Another session operation is admitted.</exception>
    public int AttributeCount => document.AttributeCount;

    /// <summary>Reads a present root attribute in schema declaration order through original typed facts.</summary>
    /// <param name="attributeIndex">Zero-based present root attribute index; independent of leaf indexes.</param>
    /// <param name="cancellationToken">Cancellation checked before and during the read.</param>
    /// <returns>A detached expanded name and scalar usable after document and session disposal.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside the present root attributes.</exception>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Overlapping work or a value outside explicit query limits is refused.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested.</exception>
    public NativeAttributeValue ReadAttribute(int attributeIndex, CancellationToken cancellationToken = default) =>
        document.ReadAttribute(attributeIndex, cancellationToken);

    /// <summary>Gets the count of present declared attributes on descendant element-only containers, excluding root attributes.</summary>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Another session operation is admitted.</exception>
    public int DescendantAttributeCount => document.DescendantAttributeCount;

    /// <summary>Reads a descendant attribute in owner preorder then schema declaration order.</summary>
    /// <param name="attributeIndex">Zero-based present descendant attribute index; independent of root and scalar indexes.</param>
    /// <param name="cancellationToken">Cancellation checked before and during the read.</param>
    /// <returns>A detached owner path, expanded name and scalar usable after document and session disposal.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside the present descendant attributes.</exception>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Overlapping work or a value outside explicit query/cache limits is refused.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested.</exception>
    public NativeAttributeValue ReadDescendantAttribute(int attributeIndex, CancellationToken cancellationToken = default) =>
        document.ReadDescendantAttribute(attributeIndex, cancellationToken);

    /// <summary>Exports detached XML retaining scalar lexical spelling, without PSVI or authority.</summary>
    /// <param name="cancellationToken">Cancellation checked before and during bounded serialization.</param>
    /// <returns>Detached XML text; not a canonical XSD serialization or byte-preserving source copy.</returns>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Another operation is admitted or output capacity is exceeded.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested during the operation.</exception>
    public string ExportXml(CancellationToken cancellationToken = default) => document.ExportXml(cancellationToken);

    /// <summary>Revokes this document and frees its admission, waiting for admitted synchronous session work.</summary>
    /// <remarks>Repeated disposal is harmless; the parent session remains usable.</remarks>
    /// <exception cref="InvalidOperationException">Cleanup failed after every owned cleanup action was attempted.</exception>
    public void Dispose() => document.Dispose();
}
