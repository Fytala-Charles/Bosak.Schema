# Lax attribute wildcards on empty owners

Development 0.28 qualifies lax attribute wildcards on empty complex owners. Known globals retain their declaration/type constraints. Undeclared attributes at an empty root carry no declaration, anySimpleType node types and untypedAtomic values; partial roots project to canonical anyType while original declared schema components remain. All 23,485 source tests (1,790 internal / 21,695 public) and 21,695 overlapping package checks pass. All 23,313 prior names remain, with 172 checks added. Pilot21; structural44/85; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads are unchanged. Unpublished; broader projection, full XSD1.1, PSVI and C45 acceptance remain separate.

The79 authored contracts run through both facades, with14 native proofs of original schema versus assessed type, declaration absence, actual types, immutable aliases, bounds and lifetime. Four historical public expectations are reconciled; original payload bytes and all prior test names remain.

- [Original contract](CONTRACT.md) and [immutable adoption](ADOPTION.json)
- [Consumer guide](GUIDE.md), [contracts](CONTRACTS.json), [test results](TEST-RESULTS.json) and [qualification](QUALIFICATION.json)
- [API review](API-REVIEW.json), [historical reconciliation](HISTORICAL-RECONCILIATION.json), [setup corrections](TEST-SETUP-CORRECTIONS.json) and [corpus deltas](CORPUS-DELTA.json)
- [Current matrix](CURRENT-MATRIX.json), [review](REVIEW.json) and [beta readiness](BETA-READINESS.md)

Run `python docs/lax-empty-attributes/validate.py` offline. Package checks overlap source; samples are not a specification coverage denominator.
