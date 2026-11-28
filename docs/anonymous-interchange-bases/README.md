# Anonymous binary and QName restriction bases

Development 0.45 qualifies bounded inline anonymous hexBinary, base64Binary and QName restriction bases. Original unnamed objects, direct base links and facet namespace scopes preserve exact octets/length, expanded-name QName equality and fixed/enumeration/whitespace/pattern ancestry. All 28,107 source tests (2,044 internal / 26,063 public) and 26,063 overlapping exact-package checks pass. All 27,708 prior names and original test inputs remain; 26 public expectations are explicitly reconciled and 399 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; other atomic/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate.

The 190 authored contracts run through both public facades, with 19 additional native proofs. All prior names and original fixture bytes remain. Thirteen prior authored interchange refusal fixtures are explicitly reconciled for both facades: compilation is now Valid; twelve original instance texts remain Invalid, while the original QName text P1M is Valid as an unqualified name. Original assertions are archived.

- [Contract](CONTRACT.md), [immutable adoption](ADOPTION.json) and [ADR053](../ADR-053-anonymous-interchange-bases.md)
- [Consumer guide](GUIDE.md), [contracts](CONTRACTS.json), [test results](TEST-RESULTS.json) and [qualification](QUALIFICATION.json)
- [API review](API-REVIEW.json), [historical reconciliation](HISTORICAL-RECONCILIATION.json), [setup checks](TEST-SETUP-CORRECTIONS.json) and [corpus deltas](CORPUS-DELTA.json)
- [Current matrix](CURRENT-MATRIX.json), [review](REVIEW.json) and [beta readiness](BETA-READINESS.md)

Run `python docs/anonymous-interchange-bases/validate.py` offline. Package checks overlap source. Samples are not a specification denominator.
