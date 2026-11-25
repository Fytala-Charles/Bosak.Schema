// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Owns bounded native assessment and fixed original-node atomization.
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
//                      | Charles Korthout | 0.2   | 25-11-2026     | Add explicit closed-catalog include compilation overload |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>Owns bounded native assessment and fixed original-node atomization.</summary>
/// <remarks>String input only. Operations on one session must be sequential; overlapping operations throw
/// <see cref="NotSupportedException"/>. Disposal waits for admitted synchronous work. No callbacks, raw
/// provider authority, general query syntax or cross-universe handoff are exposed.</remarks>
public sealed class NativeSchemaNodeDataSession : IDisposable
{
    private readonly NativeNodeDataCore core;
    private NativeSchemaNodeDataSession(NativeNodeDataCore core) => this.core = core;

    /// <summary>Compiles matching-context includes from an explicit closed schema-text catalog.</summary>
    /// <param name="rootDocumentUri">Absolute fragment-free identifier of the root catalog document.</param>
    /// <param name="schemaDocuments">Closed schema texts keyed by absolute identifiers; no external resolution occurs.</param>
    /// <param name="options">Required immutable numerical and admission limits.</param>
    /// <param name="cancellationToken">Cancellation checked during catalog copying, assembly and compilation.</param>
    /// <returns>An owned session only for Valid; other outcomes publish no session.</returns>
    /// <exception cref="ArgumentNullException">An argument or catalog text is null.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested.</exception>
    public static NativeSchemaResult<NativeSchemaNodeDataSession> Create(string rootDocumentUri,
        IReadOnlyDictionary<string,string> schemaDocuments, NativeSchemaSessionOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(rootDocumentUri);
        ArgumentNullException.ThrowIfNull(schemaDocuments);
        ArgumentNullException.ThrowIfNull(options);
        var assembled = NativeDecimalSchema.AssembleIncludes(rootDocumentUri, schemaDocuments, cancellationToken);
        return assembled.Outcome == NativeOutcome.Valid
            ? Create(assembled.Value!, options, cancellationToken)
            : NativeSchemaResult<NativeSchemaNodeDataSession>.Refused(assembled.Outcome, "SCHEMA", assembled.UnsupportedReason);
    }

    /// <summary>Compiles the finite scalar schema and opens a separate owned node-data session.</summary>
    /// <param name="schema">Complete XML schema text; the finite profile limits it to 32 KiB UTF-8.</param>
    /// <param name="options">Required immutable numerical and admission limits, with no implicit defaults.</param>
    /// <param name="cancellationToken">Cancellation checked before and during compilation.</param>
    /// <returns>An owned session only for Valid; otherwise one bounded stage diagnostic.</returns>
    /// <exception cref="ArgumentNullException">Schema or options is null.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested.</exception>
    public static NativeSchemaResult<NativeSchemaNodeDataSession> Create(string schema, NativeSchemaSessionOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(schema);
        cancellationToken.ThrowIfCancellationRequested();
        var result = NativeNodeDataCore.Create(schema, options, cancellationToken);
        return result.Outcome == NativeOutcome.Valid
            ? NativeSchemaResult<NativeSchemaNodeDataSession>.Success(new(result.Value!))
            : NativeSchemaResult<NativeSchemaNodeDataSession>.Refused(result.Outcome, "SCHEMA", result.UnsupportedReason);
    }

    /// <summary>Completely assesses owned instance text before admitting a document.</summary>
    /// <param name="instance">Complete instance text, limited to 64 KiB UTF-8 by the finite profile.</param>
    /// <param name="context">Required explicit assessment date and timezone.</param>
    /// <param name="cancellationToken">Cancellation checked before and during assessment.</param>
    /// <returns>An owned document only for Valid; Invalid or Unsupported publishes no document.</returns>
    /// <exception cref="ArgumentNullException">Instance or context is null.</exception>
    /// <exception cref="ObjectDisposedException">The session is closed.</exception>
    /// <exception cref="NotSupportedException">Another operation is admitted on this session.</exception>
    /// <exception cref="InvalidOperationException">Admission capacity is exhausted or cleanup fails.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested.</exception>
    public NativeSchemaResult<NativeSchemaNodeDataDocument> Assess(string instance, NativeSchemaAssessmentContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(instance);
        var result = core.Assess(instance, context.CurrentDateTime, context.ImplicitTimezoneOffsetMinutes, cancellationToken);
        return result.Outcome == NativeOutcome.Valid
            ? NativeSchemaResult<NativeSchemaNodeDataDocument>.Success(NativeSchemaNodeDataDocument.Own(result.Value!))
            : NativeSchemaResult<NativeSchemaNodeDataDocument>.Refused(result.Outcome, "INSTANCE", result.UnsupportedReason);
    }

    /// <summary>Revokes this session and all documents, then attempts every owned cleanup action once.</summary>
    /// <remarks>Waits for admitted synchronous work. Repeated disposal is harmless. If cleanup fails,
    /// access stays revoked; disposal does not retry or certify successful physical cleanup.</remarks>
    /// <exception cref="InvalidOperationException">One or more cleanup actions failed after all were attempted.</exception>
    public void Dispose() => core.Dispose();
}
