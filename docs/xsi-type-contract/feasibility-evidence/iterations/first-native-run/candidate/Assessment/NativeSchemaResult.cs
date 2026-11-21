// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Returns producer-owned scalar outcomes with bounded sanitized diagnostics.
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

using System.Collections.Immutable;

namespace Bosak.Schema;

/// <summary>Classifies a completed operation in the finite native scalar profile.</summary>
public enum NativeSchemaOutcome
{
    /// <summary>The complete admitted profile succeeded and owns a value.</summary>
    Valid = 0,
    /// <summary>Malformed XML or a supported structural, lexical or assertion violation was found.</summary>
    Invalid = 1,
    /// <summary>Coverage or representation lies outside the finite profile.</summary>
    Unsupported = 2
}

/// <summary>Contains a stable bounded stage code and fixed message with no input text or exception graph.</summary>
public sealed class NativeSchemaDiagnostic
{
    internal NativeSchemaDiagnostic(string code, string message, NativeSchemaUnsupportedReason unsupportedReason)
    { Code = code; Message = message; UnsupportedReason = unsupportedReason; }
    /// <summary>Gets the stable BSK-NATIVE-{SCHEMA|INSTANCE}-{INVALID|UNSUPPORTED} code.</summary>
    public string Code { get; }
    /// <summary>Gets a fixed stage message of at most 160 characters, without caller input.</summary>
    public string Message { get; }
    /// <summary>Gets one detected Unsupported category, or Unspecified for unmapped refusals and Invalid.</summary>
    /// <remarks>Not exhaustive; callers must tolerate Unspecified and future categories.</remarks>
    public NativeSchemaUnsupportedReason UnsupportedReason { get; }
}

/// <summary>Contains an owned value only for Valid, or one immutable bounded refusal diagnostic.</summary>
/// <typeparam name="T">The owned session or document type.</typeparam>
/// <remarks>Results cannot be constructed by callers. Cancellation, argument, capacity and lifecycle
/// failures throw instead of masquerading as Invalid input. Refusals deliberately summarize one stage;
/// they are not an exhaustive diagnostic report.</remarks>
public sealed class NativeSchemaResult<T> where T : class
{
    private NativeSchemaResult(NativeSchemaOutcome outcome, T? value, ImmutableArray<NativeSchemaDiagnostic> diagnostics)
    { Outcome = outcome; Value = value; Diagnostics = diagnostics; }
    /// <summary>Gets the complete finite-profile outcome.</summary>
    public NativeSchemaOutcome Outcome { get; }
    /// <summary>Gets the owned value for Valid, or null for a refusal.</summary>
    public T? Value { get; }
    /// <summary>Gets zero diagnostics for Valid, or exactly one stage diagnostic for a refusal.</summary>
    public ImmutableArray<NativeSchemaDiagnostic> Diagnostics { get; }
    /// <summary>Gets whether the public diagnostic collection was truncated; always false for this one-stage contract.</summary>
    public bool DiagnosticsTruncated => false;

    internal static NativeSchemaResult<T> Success(T value) => new(NativeSchemaOutcome.Valid, value, []);
    internal static NativeSchemaResult<T> Refused(NativeOutcome outcome, string stage, NativeDecimalUnsupportedReason reason = NativeDecimalUnsupportedReason.Unspecified)
    {
        var invalid = outcome == NativeOutcome.Invalid;
        return new(invalid ? NativeSchemaOutcome.Invalid : NativeSchemaOutcome.Unsupported, null,
            [new($"BSK-NATIVE-{stage}-{(invalid ? "INVALID" : "UNSUPPORTED")}",
                invalid ? "The input is malformed or violates the supported native scalar profile."
                    : "The input or representation is outside the supported native scalar profile.",
                invalid ? NativeSchemaUnsupportedReason.Unspecified : (NativeSchemaUnsupportedReason)(int)reason)]);
    }
}
