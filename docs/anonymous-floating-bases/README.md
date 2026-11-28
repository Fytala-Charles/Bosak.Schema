# Anonymous floating restriction bases

Development 0.47 qualifies bounded inline anonymous float and double restriction bases. Original unnamed objects, direct base links and facet sources preserve IEEE family mapping/rounding, special values, value/lexical facet ancestry and bounded lifetime rules. All 29,374 source tests (2,111 internal / 27,263 public) and 27,263 overlapping exact-package checks pass. All 28,931 prior names and original test inputs remain; four public expectations are explicitly reconciled and 443 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; broader simple-type/complex-content derivation and full XSD 1.1/PSVI/C45 acceptance remain separate.

The 206 authored contracts run through both public facades, with 31 additional native proofs. All prior names and original fixture bytes remain. Two prior authored floating refusal fixtures are explicitly reconciled for both facades: compilation and original 01.20 instances are now Valid, retaining the exact promoted float value 1.2000000476837158 and double value 1.2. Original assertions are archived.

- [Contract](CONTRACT.md), [immutable adoption](ADOPTION.json) and [ADR055](../ADR-055-anonymous-floating-bases.md)
- [Consumer guide](GUIDE.md), [contracts](CONTRACTS.json), [test results](TEST-RESULTS.json) and [qualification](QUALIFICATION.json)
- [API review](API-REVIEW.json), [historical reconciliation](HISTORICAL-RECONCILIATION.json), [setup checks](TEST-SETUP-CORRECTIONS.json) and [corpus deltas](CORPUS-DELTA.json)
- [Current matrix](CURRENT-MATRIX.json), [review](REVIEW.json) and [beta readiness](BETA-READINESS.md)

Run `python docs/anonymous-floating-bases/validate.py` offline. Package checks overlap source. Samples are not a specification denominator.
