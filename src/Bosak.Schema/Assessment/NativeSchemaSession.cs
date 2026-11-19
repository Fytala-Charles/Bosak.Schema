// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Owns synchronous opt-in string and decimal schema assessment.
// SPECIAL NOTES        : Bounded opt-in string/decimal profile; existing decimal facade remains unchanged.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>Owns a synchronous session for the finite native string/decimal schema profile.</summary>
/// <remarks>String input only. Operations on one session must be sequential; overlapping operations throw
/// <see cref="NotSupportedException"/>. Disposal waits for admitted synchronous work. No callbacks, raw
/// provider authority, general query syntax or cross-universe handoff are exposed.</remarks>
public sealed class NativeSchemaSession : IDisposable
{
    private readonly NativeDecimalSessionCore core;
    private NativeSchemaSession(NativeDecimalSessionCore core) => this.core = core;

    /// <summary>Compiles the finite scalar schema and opens an owned scalar session.</summary>
    /// <param name="schema">Complete XML schema text; the finite profile limits it to 32 KiB UTF-8.</param>
    /// <param name="options">Required immutable numerical and admission limits, with no implicit defaults.</param>
    /// <param name="cancellationToken">Cancellation checked before and during compilation.</param>
    /// <returns>An owned session only for Valid; otherwise one bounded stage diagnostic.</returns>
    /// <exception cref="ArgumentNullException">Schema or options is null.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested.</exception>
    public static NativeSchemaResult<NativeSchemaSession> Create(string schema, NativeSchemaSessionOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(schema);
        cancellationToken.ThrowIfCancellationRequested();
        var result = NativeDecimalSessionCore.CreateMixed(schema, options, cancellationToken);
        return result.Outcome == NativeOutcome.Valid
            ? NativeSchemaResult<NativeSchemaSession>.Success(new(result.Value!))
            : NativeSchemaResult<NativeSchemaSession>.Refused(result.Outcome, "SCHEMA", result.UnsupportedReason);
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
    public NativeSchemaResult<NativeSchemaDocument> Assess(string instance, NativeSchemaAssessmentContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(instance);
        var result = core.Assess(instance, context.CurrentDateTime, context.ImplicitTimezoneOffsetMinutes, cancellationToken);
        return result.Outcome == NativeOutcome.Valid
            ? NativeSchemaResult<NativeSchemaDocument>.Success(new(result.Value!))
            : NativeSchemaResult<NativeSchemaDocument>.Refused(result.Outcome, "INSTANCE", result.UnsupportedReason);
    }

    /// <summary>Revokes this session and all documents, then attempts every owned cleanup action once.</summary>
    /// <remarks>Waits for admitted synchronous work. Repeated disposal is harmless. If cleanup fails,
    /// access stays revoked; disposal does not retry or certify successful physical cleanup.</remarks>
    /// <exception cref="InvalidOperationException">One or more cleanup actions failed after all were attempted.</exception>
    public void Dispose() => core.Dispose();
}
