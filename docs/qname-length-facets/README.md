# QName length-facet compatibility

Development0.49 qualifies bounded QName length/minLength/maxLength component compatibility. QName values are not measured by prefix spelling, namespace/local-name strings or binary octets; original component inheritance/fixed consistency remains enforced. All30,298 source tests(2,159 internal/28,139 public) and28,139 overlapping exact-package checks pass. All29,913 prior names and original fixture bytes remain;185 authored contracts x2+15native=385new checks. Ten public expectations are archived/reconciled. Pilot21; structural70/59; datatype584/14; zeroerrors. API55/663 and nine signed Core1.86.0 payloads are unchanged. Unpublished; full XSD1.1/PSVI/C45 acceptance remains pending.

Four historical fixtures now compile and assess Valid with their original QName values; all ten facade/timezone assertions are reconciled against archived original harnesses. Original schemas/XML/IDs/names remain unchanged. The six original upstream cases are distinct from authored variants.

- [Contract](CONTRACT.md), [immutable adoption](ADOPTION.json), [ADR057](../ADR-057-qname-length-facets.md)
- [Guide](GUIDE.md), [contracts](CONTRACTS.json), [tests](TEST-RESULTS.json), [qualification](QUALIFICATION.json)
- [API](API-REVIEW.json), [historical reconciliation](HISTORICAL-RECONCILIATION.json), [upstream samples](UPSTREAM-TRIALS.json)
- [Corpus delta](CORPUS-DELTA.json), [current matrix](CURRENT-MATRIX.json), [review](REVIEW.json), [beta readiness](BETA-READINESS.md)

Run `python docs/qname-length-facets/validate.py` offline. Samples do not establish a normative specification denominator.
