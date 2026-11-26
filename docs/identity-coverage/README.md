# Bounded ID, IDREF and IDREFS checkpoint

Development 0.16 qualifies bounded ID/IDREF/IDREFS validation: owner-aware ID bindings, forward references, actual union/list members and assessment-local resolution. Source 21,730 and fresh package 20,059 overlapping tests pass; all 21,544 prior names remain. The frozen datatype sample improves to 558 matches / 40 Unsupported (+21 admissions), with zero errors. Pilot21 and structural39/90 remain unchanged. Public API55 types/663 rows and signed Core1.86.0 are unchanged. No publication or full XSD1.1/C45 acceptance.

89 authored behavioral contracts through both facades and eight native proof cases qualify lexical boundaries, actual governing/member types, same-owner binding, unresolved/conflicting references, cancellation, recovery, concurrency and expiry. These are contracts, not specification features.

- [Adoption and exact normative revisions](ADOPTION.json)
- [Consumer guide and capacities](GUIDE.md)
- [Contract ledger](CONTRACTS.json)
- [Qualification](QUALIFICATION.json) and [source identities](BUILD-CONTEXT.json)
- [Historical reconciliation](HISTORICAL-RECONCILIATION.json) and [redacted diagnostics review](DIAGNOSTIC-CONTRACT-REVIEW.json)
- [Datatype results](DATATYPE-RESULTS.json), [frozen cohort delta](CORPUS-DELTA.json) and [original invoice trial](APPLICATION-TRIALS.json)
- [Current matrix](CURRENT-MATRIX.json) and [beta readiness](BETA-READINESS.md)

Run `python docs/identity-coverage/validate.py` for offline evidence verification. Package tests repeat source public coverage and must not be added to the source total. No older fixture or sealed packet is rewritten.
