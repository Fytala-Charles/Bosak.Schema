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
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>Exposes detached scalar values and XML from one owned native assessment.</summary>
/// <remarks>Owned by its creating session. Exported XML and returned values carry no transferable schema authority.</remarks>
public sealed class NativeSchemaDocument : IDisposable
{
    private readonly NativeDecimalSessionCore.Document document;
    internal NativeSchemaDocument(NativeDecimalSessionCore.Document document) => this.document = document;

    /// <summary>Gets the count of present string/decimal leaves; absent optional children have no index.</summary>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Another session operation is admitted.</exception>
    public int ScalarCount => document.DecimalCount;

    /// <summary>Reads one present leaf through the issued scalar route and returns its detached typed value.</summary>
    /// <param name="leafIndex">Zero-based present-leaf index in instance order.</param>
    /// <param name="cancellationToken">Cancellation checked before and during the read.</param>
    /// <returns>A detached string, Boolean or exact decimal usable after document and session disposal.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside the present leaves.</exception>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Overlapping work or a value outside explicit query limits is refused.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested.</exception>
    public NativeScalarValue ReadScalar(int leafIndex, CancellationToken cancellationToken = default) =>
        document.ReadScalar(leafIndex, cancellationToken);

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
