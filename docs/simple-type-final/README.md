# Global simple-type final controls

Development 0.33 qualifies explicit final controls on global named simple types. Restriction, list, union and simple-content extension edges resolve source-bound names and reject prohibited methods before a compiled schema is published. All 24,313 source tests (1,846 internal /22,467 public) and22,467 overlapping exact-package checks pass. All24,147 prior names and original inputs remain unchanged;166 checks added. Pilot21; structural59/70; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; broader derivation, finalDefault, complex-type final and full XSD1.1/PSVI/C45 remain separate.

The80 authored contracts run through both facades, supplemented by six native proofs. All prior names, expectations and original inputs are preserved.

- [Contract](CONTRACT.md) and [immutable adoption](ADOPTION.json)
- [Consumer guide](GUIDE.md), [contracts](CONTRACTS.json), [test results](TEST-RESULTS.json) and [qualification](QUALIFICATION.json)
- [API review](API-REVIEW.json), [regression preservation](HISTORICAL-RECONCILIATION.json), [setup checks](TEST-SETUP-CORRECTIONS.json) and [corpus deltas](CORPUS-DELTA.json)
- [Current matrix](CURRENT-MATRIX.json), [review](REVIEW.json) and [beta readiness](BETA-READINESS.md)

Run `python docs/simple-type-final/validate.py` offline. Package checks overlap source; samples are not a specification denominator.
