// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 17 November 2026
// PURPOSE              : Represents bounded native assessment outcomes without exposing provider authority.
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

using System.Collections.Immutable;

namespace Bosak.Schema;

/// <summary>Classifies a completed operation in the finite native decimal profile.</summary>
public enum NativeDecimalOutcome
{
    /// <summary>The complete admitted profile succeeded and owns a value.</summary>
    Valid,
    /// <summary>Malformed XML or a supported structural, lexical or assertion violation was found.</summary>
    Invalid,
    /// <summary>Coverage or representation lies outside the finite profile.</summary>
    Unsupported
}

/// <summary>Contains a stable bounded stage code and fixed message with no input text or exception graph.</summary>
public sealed class NativeDecimalDiagnostic
{
    internal NativeDecimalDiagnostic(string code, string message, NativeDecimalUnsupportedReason unsupportedReason)
    { Code = code; Message = message; UnsupportedReason = unsupportedReason; }
    /// <summary>Gets the stable BSK-NATIVE-{SCHEMA|INSTANCE}-{INVALID|UNSUPPORTED} code.</summary>
    public string Code { get; }
    /// <summary>Gets a fixed stage message of at most 160 characters, without caller input.</summary>
    public string Message { get; }
    /// <summary>Gets one detected Unsupported category, or Unspecified for unmapped refusals and Invalid.</summary>
    /// <remarks>Not exhaustive; callers must tolerate Unspecified and future categories.</remarks>
    public NativeDecimalUnsupportedReason UnsupportedReason { get; }
}

/// <summary>Contains an owned value only for Valid, or one immutable bounded refusal diagnostic.</summary>
/// <typeparam name="T">The owned session or document type.</typeparam>
/// <remarks>Results cannot be constructed by callers. Cancellation, argument, capacity and lifecycle
/// failures throw instead of masquerading as Invalid input. Refusals deliberately summarize one stage;
/// they are not an exhaustive diagnostic report.</remarks>
public sealed class NativeDecimalResult<T> where T : class
{
    private NativeDecimalResult(NativeDecimalOutcome outcome, T? value, ImmutableArray<NativeDecimalDiagnostic> diagnostics)
    { Outcome = outcome; Value = value; Diagnostics = diagnostics; }
    /// <summary>Gets the complete finite-profile outcome.</summary>
    public NativeDecimalOutcome Outcome { get; }
    /// <summary>Gets the owned value for Valid, or null for a refusal.</summary>
    public T? Value { get; }
    /// <summary>Gets zero diagnostics for Valid, or exactly one stage diagnostic for a refusal.</summary>
    public ImmutableArray<NativeDecimalDiagnostic> Diagnostics { get; }
    /// <summary>Gets whether the public diagnostic collection was truncated; always false for this one-stage contract.</summary>
    public bool DiagnosticsTruncated => false;

    internal static NativeDecimalResult<T> Success(T value) => new(NativeDecimalOutcome.Valid, value, []);
    internal static NativeDecimalResult<T> Refused(NativeOutcome outcome, string stage, NativeDecimalUnsupportedReason reason = NativeDecimalUnsupportedReason.Unspecified)
    {
        var invalid = outcome == NativeOutcome.Invalid;
        return new(invalid ? NativeDecimalOutcome.Invalid : NativeDecimalOutcome.Unsupported, null,
            [new($"BSK-NATIVE-{stage}-{(invalid ? "INVALID" : "UNSUPPORTED")}",
                invalid ? "The input is malformed or violates the supported native decimal profile."
                    : "The input or representation is outside the supported native decimal profile.",
                invalid ? NativeDecimalUnsupportedReason.Unspecified : reason)]);
    }
}
