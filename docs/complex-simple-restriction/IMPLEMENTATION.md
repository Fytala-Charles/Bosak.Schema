# Decimal complex simple-content restriction implementation

21 November 2026. Development candidate based on
7ce8ada7ca78a77a13e07151b199639fa757317a. [Production adoption](ADOPTION.md) was
recorded after signed-Core feasibility and before source edits. Original proposal
and feasibility dossiers remain sealed historical evidence. This is an uncommitted
bounded implementation, not a package release or full XSD/PSVI claim.

The implementation retains G->complex B and anonymous C1->actual C0 as separate
immutable compiled identities. Facet restriction validity is checked before
instance assessment; parent/local numeric constraints are then enforced together.
Same/fresh context behavior, unsupported complex substitutions and original fact/
provenance/lifetime guards use the existing APIs. SC-058 has one recorded reason
reconciliation; its original schema/instance/ID and Unsupported outcome remain.

Full source qualification passes2,319cases(651implementation/1,668public), including
70new internal and106new public cases. Package-only minimum/current, retained
callers, signed dependency identity, API and frozen-corpus checks also pass. No commit/push, tag/controller or publication requested.

## Measured qualification

| Lane | Result |
|---|---|
| Full source | 2,319 passed: 651 implementation / 1,668 public |
| Package-only Core1.81.2 | 1,668 passed |
| Package-only Core1.81.17 | 1,668 passed, with the existing test-only loaded-version expectation overlay |
| Current-package native metadata/static controls | 70 passed; overlaps source |
| Retained callers, only Schema.dll replaced | 124 / 23 / 33 passed |
| Public API | 54 types / 401 rows, zero delta |
| Signed dependency/source/archive/runtime checks | 18 Core signatures and published beta.1 baseline verified |
| Unchanged frozen corpus | 17 matches / 4 Unsupported / zero mismatch or harness errors |

All 2,143 prior stable source IDs remain, after the single recorded SC-058 refusal
reason change. The new 176 cases are 70 internal (including48normative bound
triplets and22native/ownership/resource controls) plus106public (53payloads per
facade). These overlapping lanes are not an additive conformance total.

The only upstream classification change is
MS-ComplexType2006-07-15/ctD017/ctD017.v, now a measured match. All original metadata,
expectations, notices and42corpus files remain unchanged. Three assert006 cases
and the namespace-uri nested assertion case remain Unsupported/AssertionGrammar;
that reason is a detected gate, not a complete inventory of missing features.

[Exact results](implementation-evidence/RESULTS.json),
[case identity comparison](implementation-evidence/STABLE-CASES.json),
[pilot delta](implementation-evidence/PILOT-DELTA.json),
[package identities](implementation-evidence/IDENTITY-FINAL.json),
[API comparison](implementation-evidence/SURFACE.json) and
[authored admission](ADMISSIONS.json) retain their individual scope. Raw TRX,
signed-package verification logs, project inputs, source snapshots/hashes and
candidate driver request/response/report are preserved with a separate seal.

Local0.6.0-alpha.0.26 is unsigned qualification only. It includes the reconciled
consumer README; no registry publication or tag is implied. Existing development
Core pins, published beta.1 and release controllers remain unchanged.

## Review and limits

Self-review covers immutable G/B/C0/C1 identity, compile-time facet validity versus
runtime conjunction, all resolver call sites/eager unused definitions, bounded
cycles/chains, original data/typed-identity evidence, budgets, query rollback,
legacy refusal ordering and anonymous content registration. No blocking findings.
Actual-native signed-Core proof precedes production adoption; no Core workaround,
new public API or authority factory was introduced. Existing complex xsi:type
substitution remains deferred after recognizing genuine ancestry.

This first slice deliberately excludes attributes/assertions on either complex
type, longer complex restriction chains, other scalar families, inline simpleType,
composition and broader grammar. No full-XSD/PSVI/C45/performance acceptance.

Next: review and deliver this candidate on an explicit Git request. Then assess
integer simple-content mapping separately from contextual/inherited assertion
semantics, using original inputs and fresh bounded contracts rather than treating
four refused fixtures as four isolated patches.
