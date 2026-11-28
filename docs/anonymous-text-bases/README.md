# Anonymous URI and text-derived restriction bases

Development 0.46 qualifies bounded inline anonymous anyURI, language, Name, NCName, NMTOKEN, ID and IDREF restriction bases. Original unnamed objects, direct base links and facet sources preserve URI/text normalization, lexical families, Unicode scalar lengths, facet ancestry and validation-root identity checks. All 28,931 source tests (2,080 internal / 26,851 public) and 26,851 overlapping exact-package checks pass. All 28,107 prior names and original test inputs remain; 32 public expectations are explicitly reconciled and 824 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; floating/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate.

The 394 authored contracts run through both public facades, with 36 additional native proofs. All prior names and original fixture bytes remain. Sixteen prior authored URI/text refusal fixtures are explicitly reconciled for both facades: compilation is now Valid; eight original URI texts are Valid, while eight original text lexical inputs remain Invalid. Initial authored identity fixture contexts were corrected to explicit declared binding elements; their original proposals and failing receipts are preserved. Original assertions are archived.

- [Contract](CONTRACT.md), [immutable adoption](ADOPTION.json) and [ADR054](../ADR-054-anonymous-text-bases.md)
- [Consumer guide](GUIDE.md), [contracts](CONTRACTS.json), [test results](TEST-RESULTS.json) and [qualification](QUALIFICATION.json)
- [API review](API-REVIEW.json), [historical reconciliation](HISTORICAL-RECONCILIATION.json), [setup checks](TEST-SETUP-CORRECTIONS.json) and [corpus deltas](CORPUS-DELTA.json)
- [Current matrix](CURRENT-MATRIX.json), [review](REVIEW.json) and [beta readiness](BETA-READINESS.md)

Run `python docs/anonymous-text-bases/validate.py` offline. Package checks overlap source. Samples are not a specification denominator.
