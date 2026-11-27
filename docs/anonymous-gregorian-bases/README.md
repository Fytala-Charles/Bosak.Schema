# Anonymous Gregorian restriction bases

Development 0.43 qualifies bounded inline anonymous gYear, gYearMonth, gMonth, gMonthDay and gDay restriction bases. Original unnamed objects, direct base links and original facet sources preserve existing Gregorian component/value mapping, timezone equality/partial order and fixed/enumeration/bound/whitespace/pattern ancestry. All 27,209 source tests (2,002 internal / 25,207 public) and 25,207 overlapping exact-package checks pass. All 26,412 prior names and original test inputs remain; six public expectations are explicitly reconciled and 797 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; other atomic/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate.

The 385 authored contracts run through both public facades, with 27 additional native proofs. All prior names and original fixture bytes remain. Three prior authored gYear refusal fixtures are explicitly reconciled for both facades: compilation is now Valid, while their original date/calendar inputs remain Invalid for gYear. Original assertions are archived.

- [Contract](CONTRACT.md), [immutable adoption](ADOPTION.json) and [ADR051](../ADR-051-anonymous-gregorian-bases.md)
- [Consumer guide](GUIDE.md), [contracts](CONTRACTS.json), [test results](TEST-RESULTS.json) and [qualification](QUALIFICATION.json)
- [API review](API-REVIEW.json), [historical reconciliation](HISTORICAL-RECONCILIATION.json), [setup checks](TEST-SETUP-CORRECTIONS.json) and [corpus deltas](CORPUS-DELTA.json)
- [Current matrix](CURRENT-MATRIX.json), [review](REVIEW.json) and [beta readiness](BETA-READINESS.md)

Run `python docs/anonymous-gregorian-bases/validate.py` offline. Package checks overlap source. Samples are not a specification denominator.
