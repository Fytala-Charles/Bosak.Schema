# Bounded simple-content attribute wildcards

Development 0.27 qualifies strict attribute wildcards on existing noninteger simple-content extension owners. Original complex governing components and actual scalar values remain distinct, with independent global attribute validation. All 23,313 source tests (1,776 internal / 21,537 public) and 21,537 overlapping fresh package checks pass. All 23,171 prior test names remain; 142 checks are added. The frozen pilot has 21 matches; structural samples have 43 matches / 86 Unsupported; datatype samples have 582 matches / 16 Unsupported, with zero errors. API 55 types / 663 rows and nine signed Core 1.86.0 payloads are unchanged. Unpublished; full XSD 1.1, PSVI and C45 acceptance remain separate.

The 65 authored contracts run through both facades, supplemented by 12 actual native proofs. Three measured historical public expectations are reconciled without changing original payload bytes or test names. The archived source and initial failed runs preserve that history.

- [Original contract](CONTRACT.md) and [immutable adoption](ADOPTION.json)
- [Consumer guide](GUIDE.md), [contracts](CONTRACTS.json), [test results](TEST-RESULTS.json) and [qualification](QUALIFICATION.json)
- [API review](API-REVIEW.json), [historical reconciliation](HISTORICAL-RECONCILIATION.json), [test setup corrections](TEST-SETUP-CORRECTIONS.json) and [corpus deltas](CORPUS-DELTA.json)
- [Current matrix](CURRENT-MATRIX.json), [review](REVIEW.json) and [beta readiness](BETA-READINESS.md)

Run `python docs/simple-content-wildcards/validate.py` for offline verification. Package checks overlap source coverage; samples are not an overall specification denominator.
