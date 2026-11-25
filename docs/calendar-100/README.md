# Native calendar batch

25 November 2026. All 100 authorized contracts are qualified: ten operational dependency/compatibility contracts and ninety bounded native calendar, capacity, identity, lifetime and integration contracts. This is not 100 new XSD features or a normative coverage denominator. [Ledger](CONTRACTS.json), [contract](CONTRACT.md), [pre-implementation adoption](ADOPTION.json) and [consumer guide](GUIDE.md) define the boundaries.

Signed Core 1.86.0 dependency qualification was delivered separately in [PR #70](https://github.com/Fytala-Charles/Bosak.Schema/pull/70), merged at `2d7ddbca1017df7d2a2db9362657cec3768ca5bf`; exact-main build passed. Native implementation is the additive 0.7 development candidate, with public calendar scalar reads, exact validation/facets and original typed transport.

Fresh source tests: 11,024 (1,508 internal / 9,516 public), including 776 new cases. A fresh package-only public consumer passes all 9,516 overlapping cases; all nine Core DLLs match the signed 1.86.0 payloads. The packaged guide example compiles and prints exact fractional seconds and `Equal`. API review finds 55 unchanged type names, zero removals and five additive reflection rows. [Qualification](QUALIFICATION.json), [API review](API-REVIEW.md) and source-only/raw test evidence are retained.

The frozen upstream pilot remains 21 matches / zero Unsupported or errors. The expanded sample remains 19 matches / 110 Unsupported / zero mismatches or harness errors across 129 cases. No sample-wide gain or full XSD conformance is claimed. [Coverage delta](COVERAGE-DELTA.json) separates the new native families from still-blocked sample shapes and broader gaps. Earlier matrices and sealed packets remain unchanged.

[Legacy reconciliation](LEGACY-RECONCILIATION.json) preserves original fixtures and IDs while updating only measured expectations. [Test corrections](TEST-AUTHORING-CORRECTIONS.json) distinguish newly authored short-pattern mistakes from product changes. Initial diagnostics are preserved separately from final passing runs.

The local validation package is unsigned and is not a release reservation. Published beta.1, its release controller and historical dependency receipts remain unchanged. No new Schema registry package or tag is published by this implementation batch. General C45, complete PSVI, full regex, arbitrary assertions, default augmentation, lists/unions and wider datatype/schema graphs remain separate.

Run `python docs/calendar-100/validate.py` for offline packet integrity, contract/evidence and preserved historical-byte checks. It does not launch .NET, restore packages or publish anything.
