# Anonymous date restriction bases

Development 0.40 qualifies bounded inline anonymous xs:date restriction bases. Original unnamed base objects, direct derivation links and original facet sources preserve existing date equality, enumeration, partial-order bounds, explicitTimezone and lexical pattern ancestry. All 25,834 source tests (1,939 internal / 23,895 public) and 23,895 overlapping exact-package checks pass. All 25,624 prior names and original test inputs remain; four public expectations are explicitly reconciled and 210 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; other calendar/interchange/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate.

The 98 authored contracts run through both public facades, with 14 additional native proofs. All prior case names and original fixture bytes remain. Four public compile/assessment expectations are reconciled explicitly against archived original assertions.

- [Contract](CONTRACT.md), [reviewed adoption](ADOPTION.json), [original pre-implementation receipt](ORIGINAL-ADOPTION.json), [wording review](ADOPTION-REVIEW.json) and [ADR048](../ADR-048-anonymous-date-bases.md)
- [Consumer guide](GUIDE.md), [contracts](CONTRACTS.json), [test results](TEST-RESULTS.json) and [qualification](QUALIFICATION.json)
- [API review](API-REVIEW.json), [historical reconciliation](HISTORICAL-RECONCILIATION.json), [test setup corrections](TEST-SETUP-CORRECTIONS.json) and [corpus deltas](CORPUS-DELTA.json)
- [Current matrix](CURRENT-MATRIX.json), [review](REVIEW.json) and [beta readiness](BETA-READINESS.md)

Run `python docs/anonymous-date-bases/validate.py` offline. Package checks overlap source. Samples are not a specification denominator.
