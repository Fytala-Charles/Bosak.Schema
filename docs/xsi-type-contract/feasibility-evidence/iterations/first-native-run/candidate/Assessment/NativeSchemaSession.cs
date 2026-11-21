// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Owns bounded scalar schema sessions with root attributes and decimal simple content.
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
//                      | Charles Korthout | 0.4   | 19-11-2026     | Document the adopted opt-in local root attribute profile                                  |
//                      | Charles Korthout | 0.5   | 20-11-2026     | Document opt-in decimal simple-content root assessment                                   |
//                      | Charles Korthout | 0.6   | 20-11-2026     | Document bounded nested element-only sequences with existing scalar reads                |
//                      | Charles Korthout | 0.7   | 20-11-2026     | Document nested container attributes and preserved root/scalar access semantics          |
//                      | Charles Korthout | 0.8   | 20-11-2026     | Document decimal simple content on root and nested scalar-bearing complex elements       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>Owns finite scalar documents with nested sequences, attributes and root or nested decimal simple content.</summary>
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
