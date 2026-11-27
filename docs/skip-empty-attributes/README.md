# Skip attribute wildcards on empty owners

Development 0.29 qualifies skip attribute wildcards on empty complex owners. Skipped extras have no node declaration, anySimpleType node types and original untypedAtomic values, even when globals exist. Roots with skipped extras project to canonical anyType; no-extra roots retain original empty typing. Explicit uses retain validation. All 23,649 source tests (1,802 internal /21,847 public) and21,847 overlapping package checks pass. All23,485 prior names remain;164 checks are added. Pilot21; structural47/82; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads are unchanged. Unpublished; broader projection, full XSD1.1, PSVI and C45 acceptance remain separate.

The76 authored contracts run through both facades, with12 native proofs for absent node declarations, skipped global constraints, original versus projected type evidence, explicit precedence, identity boundaries, immutable aliases, capacity and lifetime. Six historical public expectations are reconciled; original fixtures and all prior names remain.

- [Original contract](CONTRACT.md) and [immutable adoption](ADOPTION.json)
- [Consumer guide](GUIDE.md), [contracts](CONTRACTS.json), [test results](TEST-RESULTS.json) and [qualification](QUALIFICATION.json)
- [API review](API-REVIEW.json), [historical reconciliation](HISTORICAL-RECONCILIATION.json), [setup checks](TEST-SETUP-CORRECTIONS.json) and [corpus deltas](CORPUS-DELTA.json)
- [Current matrix](CURRENT-MATRIX.json), [review](REVIEW.json) and [beta readiness](BETA-READINESS.md)

Run `python docs/skip-empty-attributes/validate.py` offline. Package checks overlap source; samples are not an overall specification denominator.
