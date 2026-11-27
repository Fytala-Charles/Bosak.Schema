# Anonymous decimal and string restriction bases

Development 0.38 qualifies bounded inline anonymous decimal and xs:string/normalizedString/token restriction bases. Original unnamed base objects, direct derivation links, facet ancestry and original source contexts remain authoritative. All 25,369 source tests (1,910 internal / 23,459 public) and 23,459 overlapping exact-package checks pass. All 25,171 prior names and original test inputs remain; five public expectations and one native case are explicitly reconciled, and 198 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. The preceding empty-union rejection is corrected: an empty union is a valid type with no values. Unpublished; other atomic/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate.

The 93 authored contracts run through both public facades, with 12 additional native proofs. Prior case names and original fixture bytes are preserved. Five public compile/assessment expectations and one native case are reconciled explicitly against archived original assertions.

- [Contract](CONTRACT.md), [immutable adoption](ADOPTION.json) and [empty-union correction](CORRECTIONS.md)
- [Consumer guide](GUIDE.md), [contracts](CONTRACTS.json), [test results](TEST-RESULTS.json) and [qualification](QUALIFICATION.json)
- [API review](API-REVIEW.json), [historical reconciliation](HISTORICAL-RECONCILIATION.json), [test setup corrections](TEST-SETUP-CORRECTIONS.json) and [corpus deltas](CORPUS-DELTA.json)
- [Current matrix](CURRENT-MATRIX.json), [review](REVIEW.json) and [beta readiness](BETA-READINESS.md)

Run `python docs/anonymous-scalar-bases/validate.py` offline. Package checks overlap source. Samples are not a specification denominator.
