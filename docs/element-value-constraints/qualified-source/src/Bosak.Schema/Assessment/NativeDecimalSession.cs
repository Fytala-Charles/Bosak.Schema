// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 17 November 2026
// PURPOSE              : Owns a synchronous session for the finite native decimal schema profile.
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
//                      | Charles Korthout | 0.2   | 19-11-2026     | Carry bounded Unsupported reasons without changing refusal outcomes                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>Owns a synchronous session for the finite native decimal schema profile.</summary>
/// <remarks>String input only. Operations on one session must be sequential; overlapping operations throw
/// <see cref="NotSupportedException"/>. Disposal waits for admitted synchronous work. No callbacks, raw
/// provider authority, general query syntax or cross-universe handoff are exposed.</remarks>
public sealed class NativeDecimalSession : IDisposable
{
    private readonly NativeDecimalSessionCore core;
    private NativeDecimalSession(NativeDecimalSessionCore core) => this.core = core;

    /// <summary>Compiles the finite schema and opens an owned scalar session.</summary>
    /// <param name="schema">Complete XML schema text; the finite profile limits it to 32 KiB UTF-8.</param>
    /// <param name="options">Required immutable numerical and admission limits, with no implicit defaults.</param>
    /// <param name="cancellationToken">Cancellation checked before and during compilation.</param>
    /// <returns>An owned session only for Valid; otherwise one bounded stage diagnostic.</returns>
    /// <exception cref="ArgumentNullException">Schema or options is null.</exception>
    /// <exception cref="OperationCanceledException">Cancellation is requested.</exception>
    public static NativeDecimalResult<NativeDecimalSession> Create(string schema, NativeDecimalSessionOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(schema);
        cancellationToken.ThrowIfCancellationRequested();
        var result = NativeDecimalSessionCore.Create(schema, options.NumericLimits, options.QueryLimits, cancellationToken);
        return result.Outcome == NativeOutcome.Valid
            ? NativeDecimalResult<NativeDecimalSession>.Success(new(result.Value!))
            : NativeDecimalResult<NativeDecimalSession>.Refused(result.Outcome, "SCHEMA", result.UnsupportedReason);
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
    public NativeDecimalResult<NativeDecimalDocument> Assess(string instance, NativeDecimalAssessmentContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(instance);
        var result = core.Assess(instance, context.CurrentDateTime, context.ImplicitTimezoneOffsetMinutes, cancellationToken);
        return result.Outcome == NativeOutcome.Valid
            ? NativeDecimalResult<NativeDecimalDocument>.Success(new(result.Value!))
            : NativeDecimalResult<NativeDecimalDocument>.Refused(result.Outcome, "INSTANCE", result.UnsupportedReason);
    }

    /// <summary>Revokes this session and all documents, then attempts every owned cleanup action once.</summary>
    /// <remarks>Waits for admitted synchronous work. Repeated disposal is harmless. If cleanup fails,
    /// access stays revoked; disposal does not retry or certify successful physical cleanup.</remarks>
    /// <exception cref="InvalidOperationException">One or more cleanup actions failed after all were attempted.</exception>
    public void Dispose() => core.Dispose();
}
