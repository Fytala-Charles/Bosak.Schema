// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 17 November 2026
// PURPOSE              : Defines explicit immutable limits for native decimal sessions.
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

using Bosak.XPath.Api.Schema;

namespace Bosak.Schema;

/// <summary>Defines explicit immutable limits for native decimal sessions.</summary>
/// <remarks>Counts and scales are representation/admission limits, not time or total-memory budgets.
/// Independent finite XML, grammar and lexical ceilings still apply.</remarks>
public sealed class NativeDecimalSessionOptions
{
    /// <summary>Creates options with no unlimited sentinels or implicit workload defaults.</summary>
    /// <param name="maxNativeCoefficientDigits">Positive coefficient digit limit before normalization.</param>
    /// <param name="maxNativeScale">Nonnegative native decimal scale limit before normalization.</param>
    /// <param name="maxOpenDocuments">Positive number of simultaneously admitted documents.</param>
    /// <param name="maxCachedScalarValuesPerDocument">Positive Core typed-value cache capacity per document.</param>
    /// <param name="maxTypedResultAtoms">Positive typed-result atom capacity.</param>
    /// <param name="maxQueryCoefficientDigits">Positive query coefficient digit capacity.</param>
    /// <param name="maxQueryScale">Nonnegative query scale capacity.</param>
    /// <exception cref="ArgumentOutOfRangeException">A positive limit is nonpositive or a scale is negative.</exception>
    public NativeDecimalSessionOptions(int maxNativeCoefficientDigits, int maxNativeScale, int maxOpenDocuments,
        int maxCachedScalarValuesPerDocument, int maxTypedResultAtoms, int maxQueryCoefficientDigits, int maxQueryScale)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxNativeCoefficientDigits);
        ArgumentOutOfRangeException.ThrowIfNegative(maxNativeScale);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxOpenDocuments);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxCachedScalarValuesPerDocument);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxTypedResultAtoms);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxQueryCoefficientDigits);
        ArgumentOutOfRangeException.ThrowIfNegative(maxQueryScale);
        MaxNativeCoefficientDigits = maxNativeCoefficientDigits;
        MaxNativeScale = maxNativeScale;
        MaxOpenDocuments = maxOpenDocuments;
        MaxCachedScalarValuesPerDocument = maxCachedScalarValuesPerDocument;
        MaxTypedResultAtoms = maxTypedResultAtoms;
        MaxQueryCoefficientDigits = maxQueryCoefficientDigits;
        MaxQueryScale = maxQueryScale;
    }

    /// <summary>Gets the native coefficient digit ceiling before normalization.</summary>
    public int MaxNativeCoefficientDigits { get; }
    /// <summary>Gets the native scale ceiling before normalization.</summary>
    public int MaxNativeScale { get; }
    /// <summary>Gets the simultaneously admitted document ceiling.</summary>
    public int MaxOpenDocuments { get; }
    /// <summary>Gets the Core typed-value cache capacity per document.</summary>
    public int MaxCachedScalarValuesPerDocument { get; }
    /// <summary>Gets the typed-result atom capacity.</summary>
    public int MaxTypedResultAtoms { get; }
    /// <summary>Gets the query coefficient digit capacity.</summary>
    public int MaxQueryCoefficientDigits { get; }
    /// <summary>Gets the query scale capacity.</summary>
    public int MaxQueryScale { get; }

    internal NativeNumericLimits NumericLimits => new(MaxNativeCoefficientDigits, MaxNativeScale);
    internal SchemaQueryLimits QueryLimits => new(MaxOpenDocuments, MaxCachedScalarValuesPerDocument,
        MaxTypedResultAtoms, MaxQueryCoefficientDigits, MaxQueryScale);
}
