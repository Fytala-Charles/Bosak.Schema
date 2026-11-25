// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 17 November 2026
// PURPOSE              : Exposes detached decimal values and XML from one owned native assessment.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
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

using Bosak.XPath.Core.Xdm;

namespace Bosak.Schema;

/// <summary>Exposes detached decimal values and XML from one owned native assessment.</summary>
/// <remarks>Owned by its creating session. Exported XML and returned values carry no transferable schema authority.</remarks>
public sealed class NativeDecimalDocument : IDisposable
{
    private readonly NativeDecimalSessionCore.Document document;
    internal NativeDecimalDocument(NativeDecimalSessionCore.Document document) => this.document = document;

    /// <summary>Gets the count of present decimal leaves; absent optional children have no index.</summary>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Another session operation is admitted.</exception>
    public int DecimalCount => document.DecimalCount;

    /// <summary>Reads one present leaf through the issued scalar route and returns its detached exact value.</summary>
    /// <param name="leafIndex">Zero-based present-leaf index in instance order.</param>
    /// <param name="cancellationToken">Cancellation checked before and during the read.</param>
    /// <returns>A detached exact decimal usable after document and session disposal.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside the present leaves.</exception>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Overlapping work or a value outside explicit query limits is refused.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested.</exception>
    public XdmExactDecimal ReadDecimal(int leafIndex, CancellationToken cancellationToken = default) =>
        document.ReadDecimal(leafIndex, cancellationToken);

    /// <summary>Exports detached XML retaining decimal lexical spelling, without PSVI or authority.</summary>
    /// <param name="cancellationToken">Cancellation checked before serialization; serialization is synchronous.</param>
    /// <returns>Detached XML text; not a canonical XSD serialization or byte-preserving source copy.</returns>
    /// <exception cref="ObjectDisposedException">The document or its session is closed.</exception>
    /// <exception cref="NotSupportedException">Another session operation is admitted.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested before serialization.</exception>
    public string ExportXml(CancellationToken cancellationToken = default) => document.ExportXml(cancellationToken);

    /// <summary>Revokes this document and frees its admission, waiting for admitted synchronous session work.</summary>
    /// <remarks>Repeated disposal is harmless; the parent session remains usable.</remarks>
    /// <exception cref="InvalidOperationException">Cleanup failed after every owned cleanup action was attempted.</exception>
    public void Dispose() => document.Dispose();
}
