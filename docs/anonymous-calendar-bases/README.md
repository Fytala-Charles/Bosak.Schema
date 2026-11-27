# Anonymous dateTime and time restriction bases

Development 0.41 qualifies bounded inline anonymous xs:dateTime and xs:time restriction bases. Original unnamed base objects, direct derivation links and exact original facet sources preserve fractional seconds, enumeration equality, partial-order bounds, fixed/timezone/whitespace facets and lexical pattern ancestry. All 26,198 source tests (1,961 internal / 24,237 public) and 24,237 overlapping exact-package checks pass. All 25,834 prior names and original test inputs remain; four public expectations are explicitly reconciled and 364 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; dateTimeStamp, other atomic/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate.

The 171 authored contracts run through both public facades, with 22 additional native proofs. All prior case names and original fixture bytes remain. Four public compile/assessment expectations are reconciled explicitly against archived assertions. Two positive enumeration fixtures were corrected to use their enumerated whole-second values; initial failures are retained in the execution archive.

- [Contract](CONTRACT.md), [immutable adoption](ADOPTION.json) and [ADR049](../ADR-049-anonymous-calendar-bases.md)
- [Consumer guide](GUIDE.md), [contracts](CONTRACTS.json), [test results](TEST-RESULTS.json) and [qualification](QUALIFICATION.json)
- [API review](API-REVIEW.json), [historical reconciliation](HISTORICAL-RECONCILIATION.json), [test setup corrections](TEST-SETUP-CORRECTIONS.json) and [corpus deltas](CORPUS-DELTA.json)
- [Current matrix](CURRENT-MATRIX.json), [review](REVIEW.json) and [beta readiness](BETA-READINESS.md)

Run `python docs/anonymous-calendar-bases/validate.py` offline. Package checks overlap source. Samples are not a specification denominator.
