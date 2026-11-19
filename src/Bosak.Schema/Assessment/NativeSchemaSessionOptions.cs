// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Defines explicit immutable limits for opt-in native scalar sessions.
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
//                      | Charles Korthout | 0.3   | 19-11-2026     | Add bounded exact integer leaves with original typed provenance                           |
//                      | Charles Korthout | 0.4   | 19-11-2026     | Document shared leaf and enumeration UTF16 representation capacity                         |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.XPath.Api.Schema;

namespace Bosak.Schema;

/// <summary>Defines explicit immutable representation and admission limits for scalar sessions.</summary>
/// <remarks>No unlimited/default sentinels, hard deadlines or total-memory SLA are implied.</remarks>
public sealed class NativeSchemaSessionOptions
{
    /// <summary>Creates explicit native, string, export and Core admission limits.</summary>
    /// <param name="maxNativeCoefficientDigits">Native supplied decimal coefficient or integer digits before normalization.</param>
    /// <param name="maxNativeScale">Native supplied decimal scale before normalization.</param>
    /// <param name="maxStringCharacters">Parsed leaf and enumeration string capacity in UTF-16 code units.</param>
    /// <param name="maxExportCharacters">Serialized XML capacity in UTF-16 code units.</param>
    /// <param name="maxOpenDocuments">Simultaneously admitted document count.</param>
    /// <param name="maxCachedScalarValuesPerDocument">Core scalar cache capacity per document.</param>
    /// <param name="maxTypedResultAtoms">Core typed-result atom capacity.</param>
    /// <param name="maxQueryCoefficientDigits">Core query coefficient digit capacity.</param>
    /// <param name="maxQueryScale">Core query decimal scale capacity.</param>
    /// <exception cref="ArgumentOutOfRangeException">A limit is outside its documented finite range.</exception>
    public NativeSchemaSessionOptions(int maxNativeCoefficientDigits, int maxNativeScale, int maxStringCharacters, int maxExportCharacters, int maxOpenDocuments, int maxCachedScalarValuesPerDocument, int maxTypedResultAtoms, int maxQueryCoefficientDigits, int maxQueryScale)
    {
        if (maxNativeCoefficientDigits < 1 || maxNativeCoefficientDigits > 256) throw new ArgumentOutOfRangeException(nameof(maxNativeCoefficientDigits));
        MaxNativeCoefficientDigits = maxNativeCoefficientDigits;
        if (maxNativeScale < 0 || maxNativeScale > 256) throw new ArgumentOutOfRangeException(nameof(maxNativeScale));
        MaxNativeScale = maxNativeScale;
        if (maxStringCharacters < 1 || maxStringCharacters > 65536) throw new ArgumentOutOfRangeException(nameof(maxStringCharacters));
        MaxStringCharacters = maxStringCharacters;
        if (maxExportCharacters < 1 || maxExportCharacters > 524288) throw new ArgumentOutOfRangeException(nameof(maxExportCharacters));
        MaxExportCharacters = maxExportCharacters;
        if (maxOpenDocuments < 1) throw new ArgumentOutOfRangeException(nameof(maxOpenDocuments));
        MaxOpenDocuments = maxOpenDocuments;
        if (maxCachedScalarValuesPerDocument < 1) throw new ArgumentOutOfRangeException(nameof(maxCachedScalarValuesPerDocument));
        MaxCachedScalarValuesPerDocument = maxCachedScalarValuesPerDocument;
        if (maxTypedResultAtoms < 1) throw new ArgumentOutOfRangeException(nameof(maxTypedResultAtoms));
        MaxTypedResultAtoms = maxTypedResultAtoms;
        if (maxQueryCoefficientDigits < 1) throw new ArgumentOutOfRangeException(nameof(maxQueryCoefficientDigits));
        MaxQueryCoefficientDigits = maxQueryCoefficientDigits;
        if (maxQueryScale < 0) throw new ArgumentOutOfRangeException(nameof(maxQueryScale));
        MaxQueryScale = maxQueryScale;
    }

    /// <summary>Gets native supplied decimal coefficient or integer digits before normalization.</summary>
    public int MaxNativeCoefficientDigits { get; }
    /// <summary>Gets native supplied decimal scale before normalization.</summary>
    public int MaxNativeScale { get; }
    /// <summary>Gets parsed leaf and enumeration string capacity in UTF-16 code units.</summary>
    public int MaxStringCharacters { get; }
    /// <summary>Gets serialized XML capacity in UTF-16 code units.</summary>
    public int MaxExportCharacters { get; }
    /// <summary>Gets simultaneously admitted document count.</summary>
    public int MaxOpenDocuments { get; }
    /// <summary>Gets core scalar cache capacity per document.</summary>
    public int MaxCachedScalarValuesPerDocument { get; }
    /// <summary>Gets core typed-result atom capacity.</summary>
    public int MaxTypedResultAtoms { get; }
    /// <summary>Gets core query coefficient digit capacity.</summary>
    public int MaxQueryCoefficientDigits { get; }
    /// <summary>Gets core query decimal scale capacity.</summary>
    public int MaxQueryScale { get; }

    internal NativeNumericLimits NumericLimits => new(MaxNativeCoefficientDigits, MaxNativeScale);
    internal SchemaQueryLimits QueryLimits => new(MaxOpenDocuments, MaxCachedScalarValuesPerDocument, MaxTypedResultAtoms, MaxQueryCoefficientDigits, MaxQueryScale);
}
