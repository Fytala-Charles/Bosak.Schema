# Exact native duration values

The 0.12 development candidate adds bounded `duration`, `yearMonthDuration` and `dayTimeDuration` to both owned facades on signed Core 1.86.0. These source APIs are not in published Schema beta.1. Select an exact reviewed development package.

```csharp
using Bosak.Schema;
var options = new NativeSchemaSessionOptions(256,256,256,65536,1,256,16,256,256);
const string schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:duration'/></xs:schema>";
using var session = NativeSchemaSession.Create(schema,options).Value!;
using var document = session.Assess("<r>-P1Y13MT0.123456789012345678901234567890123S</r>",new(DateTimeOffset.UnixEpoch,0)).Value!;
var value = document.ReadRootScalar();
Console.WriteLine(value.DurationMonths);
Console.WriteLine(value.DurationSeconds.ToInvariantString(512));
```

Expected output: `-25` and `-0.123456789012345678901234567890123`. `DurationMonths` is a detached BigInteger; `DurationSeconds` is a detached Core XdmExactDecimal. The independent signed components never pass through TimeSpan, DateTime or System.Decimal. `DurationValue` returns the collapsed original family-valid lexical text. This is not a canonical XSD serializer. Original exported XML remains unchanged; detached values survive document/session disposal. Unrelated scalar getters throw InvalidOperationException. Both derived families have distinct NativeScalarKind values and share these three getters. Lists/unions use the existing immutable item-read methods and preserve actual selected-member kinds.

XSD duration values contain months and seconds. Years become months; days/hours/minutes become exact seconds. Numeric fields and fractions are not rounded. YearMonthDuration excludes day/time lexical fields even when zero; dayTimeDuration excludes year/month lexical fields even when zero. XML whitespace collapses. A leading minus applies to both components; negative zero equals zero. Equivalent spellings such as P1Y/P12M and P1D/PT24H have the same value. Derived durations are subsets of duration, so equality across duration families uses components, including shared zero; strings and decimal numbers remain different schema value spaces.

Duration ordering uses the four dated REC reference dates with exact arbitrary-precision Gregorian calculations. For example P1M exceeds P27D, is incomparable to P28D through P31D, and precedes P32D. Runtime values incomparable to an admitted bound are Invalid. The two derived families are totally ordered. Incomparable local endpoint pairs remain Unsupported. Distinct component pairs with identical projections at all four reference dates (for example P400Y/P146097D) also remain Unsupported for ordering; they are never silently treated as component-equal. The dated REC formal grammar and accompanying regex disagree about PT.5S/PT1.S; those ambiguous forms remain Unsupported. See [adoption](ADOPTION.json), [ordering review adoption](ORDER-REVIEW-ADOPTION.json) and [semantic review](FACET-REVIEW.md).

Supplied raw instance and numeric facet text is capped at256 UTF-16 units and MaxStringCharacters before collapse. Total supplied numeric digits, including leading zeros across all fields, use MaxNativeCoefficientDigits; seconds fraction scale uses MaxNativeScale. Aggregated months and scaled seconds coefficients must also fit before normalization. These are explicit representation capacities, not claims that larger XSD values are invalid. Existing XML/schema/graph bounds,16 enum entries,8 patterns, shared work/cancellation, query item and cache limits apply. Enumeration/bounds over patterned ancestors, arbitrary regex/assertions, excluded complex restriction grammar and automatic default augmentation remain Unsupported.

Core duration text transport is qualified only for original owned facts and these reads. Legacy duration arithmetic/comparison functions, general XPath/XQuery/XSLT schema validation, map keys, full PSVI/C45 and a public query factory are separate. No registry publication or complete XSD1.1 claim follows from this batch.
