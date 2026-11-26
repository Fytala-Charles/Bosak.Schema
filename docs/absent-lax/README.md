# Declaration-absent lax assessment

Development 0.21 qualifies declaration-absent lax elements and application attributes beneath uninterrupted anyType ancestor chains. All 22,426 source tests (1,711 internal / 20,715 public) and 20,715 overlapping fresh package consumers pass. All 22,346 prior names remain; 80 controls are added. Pilot 21 matches; structural 41 matches / 88 Unsupported; datatype 582 matches / 16 Unsupported; zero errors. API 55 types / 663 reflection rows and nine signed Core 1.86.0 payloads are unchanged. This candidate is unpublished; broader wildcards, partial-type projection and full XSD 1.1/PSVI/C45 remain separate.

Unknown element declarations and attribute declarations remain null. Node types are xs:anyType and xs:anySimpleType respectively, with original xs:untypedAtomic values. Known descendants and attributes continue to validate against actual global declarations.

- [Original pre-implementation contract](CONTRACT.md), [experimental adoption](EXPERIMENT-ADOPTION.json) and [candidate adoption](ADOPTION.json)
- [Consumer scope](GUIDE.md), [contracts](CONTRACTS.json) and [test evidence](TEST-RESULTS.json)
- [Qualification](QUALIFICATION.json), [API review](API-REVIEW.json), [historical reconciliation](HISTORICAL-RECONCILIATION.json) and [corpus delta](CORPUS-DELTA.json)
- [Current matrix](CURRENT-MATRIX.json), [review](REVIEW.json) and [beta readiness](BETA-READINESS.md)

Run `python docs/absent-lax/validate.py` to verify this sealed checkpoint. Counts measure behavioral tests, not specification features or an overall conformance percentage.
