<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Roadmap">
  <h1>Bosak.Schema Roadmap</h1>
  <p>An open schema foundation with staged validation and integration.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](docs/PUBLICATION.json). No NuGet release or commercial infrastructure publication.

# Bosak.Schema Roadmap

> An open schema foundation with staged validation and integration.


## 1. Current Milestone

The public 0.4 baseline makes schema services Apache-2.0 and activation-free. The unpublished 0.5 candidate implements [native bounded document assessment](docs/DOCUMENT-ASSESSMENT.md) with original declaration/type state and explicit typed-value projection; complete XSD 1.1 and anonymous-root interchange remain incomplete. Public source contains the schema implementation/tests only; the original commercial infrastructure and full Git history remain private. No NuGet release/tag is included in this migration.

## 2. Next Milestones

1. Qualify the smallest anonymous-root/whole-document bridge from the real native assessment result, preserving original type/declaration state and no-typed-value/copy behavior.
2. Qualify complete component/universe identity, compatible-schema handoff, PSVI/atomic flow and bounded resource ownership.
3. Implement or explicitly refuse remaining XSD 1.1 structural/assessment coverage.
4. Build/test real consumers and unchanged binaries, then run the full engine/conformance gates before API freeze or release.

## 3. Validation and Limits

The public schema suite passes 114 cases. Existing core package pins stay at 1.26.3. Neither current-draft XDM certification nor full XSD 1.1 support follows from the Apache transition. Billing/key-generation, other product licensing, Folio, XmlDb storage and Braid workflows remain outside this source change.

*Last updated: 2026-10-29*

## Whole-document bridge candidate — 29 October 2026

The unpublished core 1.34.0 provider bridge now carries original anonymous-root and decimal-child state through whole-document exports and preserve/strip copies. Root atomization raises FOTY0012; stale content and legacy reassessment refuse explicitly. Use `-p:BosakSourceRoot=D:/Development/Bosak` for builds/tests/examples because candidate package dependencies are unpublished. See [the current guide](docs/DOCUMENT-ASSESSMENT.md). The earlier 191-case receipts describe the historical projection-only checkpoint; fresh bridge qualification is recorded separately in the core `docs/schema-document-bridge` dossier. Broader coverage, consumer adoption and API freeze remain pending. No commit/push/tag/publication.

## Reviewed whole-document bridge — 29 October 2026

The optional provider attachment API now checks a replacement completely before removing the previous binding. Failed replacement retains original state and stale-content refusal; failed initial attachment leaves the node unregistered. The trusted host must supply immutable state and the correct assessed value. This API carries that state without performing schema validation. The finite native profile is unchanged; optional children and general API freeze remain pending. Current core candidate is unpublished 1.34.0; use the BosakSourceRoot override. New review qualification lives in the core `docs/schema-document-bridge/api-review` dossier, separately from historical assessment receipts.

## Optional decimal child scope — 29 October 2026

The [new adoption record](docs/OPTIONAL-DECIMAL-ADOPTION-2026-10-29.md) and [implementation plan](docs/OPTIONAL-DECIMAL-PLAN.md) define the next finite extension. It is not implemented. Keep occurrence bounds on particles, actual element states only for present nodes, and use effective Boolean conversion for empty assertion results. Maximum occurrence stays one; no broader grammar/session admission. Proposed implementation version is Schema 0.6; this documentation step keeps 0.5 and core 1.34.0 unchanged. Fresh baseline: 204 Schema tests pass. Core solution/gates inherit the source-identical `a163ff11` checkpoint. No REQ completion or publication.

Scope refinement: a thirteenth isolated observation exposes ignorable-whitespace removal being refused for an empty element-only assessed root. Supporting all-optional-absent documents therefore needs a bounded provider binding correction. The future implementation proposes core patch 1.34.1 plus fresh complete core tests and exact conformance gates; current versions and production source stay unchanged. Both the initial twelve and extended thirteen-observation receipts are preserved in the optional-child dossier; they overlap.

## Optional decimal scope delivery — 29 October 2026

See the [synchronized checkpoint](docs/OPTIONAL-DECIMAL-CHECKPOINT.md) for current delivery identity and remote target. Documentation is prepared for the user-authorized commit/push; optional-child semantics are not implemented. Current versions/pins and production source remain unchanged. Thirteen observations and 204 passing Schema baseline cases support the scope; the core suite/gates are inherited. Next: implement the bounded plan and its core binding correction with fresh required qualification.

## Optional decimal implementation candidate — 29 October 2026

Current behavior: [Schema 0.6 optional assessment](docs/OPTIONAL-DECIMAL-IMPLEMENTATION.md), with core 1.34.1 and required local source override. Local zero-or-one particles, ordered matching, assertion EBV and present-only original state are implemented; required-only v1 remains and optional schemas use v2. Empty element-only root whitespace normalization is corrected while real/decimal mutation refusal remains. The complete Schema suite passes 260 cases, including 56 new optional cases; the full-document example retains later declaration identity. Earlier planned-scope/version notices are historical. Broader REQ-009/general session/API freeze and consumer acceptance remain pending. Original public checkout and product pins are unchanged; implementation is uncommitted and unpublished.

The [optional assessment implementation](docs/OPTIONAL-DECIMAL-REVIEW.md) is reviewed for checkpoint delivery. Bounded repeated-child coverage is the next scope; general schema/session acceptance and product adoption remain future work.

The next [bounded repetition slice](docs/REPEATED-DECIMAL-PLAN.md) is specified but not implemented. Current optional support remains Schema 0.6/core 1.34.1; Schema 0.7/v3 is a future candidate target. General schema/session acceptance and product adoption remain separate.

The [bounded repeated-decimal implementation](docs/REPEATED-DECIMAL-IMPLEMENTATION.md) is a local Schema 0.7 candidate with unchanged core 1.34.1. Review/checkpoint precedes further structural or assertion-grammar expansion; general schema/session acceptance and product adoption remain future work.

The [bounded repetition implementation](docs/REPEATED-DECIMAL-REVIEW.md) is reviewed for checkpoint delivery. A separately adopted fn:count assertion slice is next; aggregation and broader grammar remain outside that first function scope.

The next [count assertion slice](docs/COUNT-ASSERTION-PLAN.md) is adopted/planned but unimplemented. Schema 0.7/core 1.34.1 remain current; Schema 0.8/v4 would mark the future count feature. Other aggregates and wider namespace/function grammar remain separate.

The [bounded count implementation](docs/COUNT-ASSERTION-IMPLEMENTATION.md) is now a local Schema 0.8 candidate with unchanged core 1.34.1. Review/checkpoint comes before wider aggregation, namespaces/functions or general schema/session acceptance.


**29 October 2026 review:** Schema 0.8 count assertions reviewed: fresh 397/397 Schema tests pass; core 1.34.1 source and inherited engine gates remain unchanged. Next is the separately scoped presence-predicate implementation; exists/empty remain unsupported in this checkpoint. [Review and next scope](docs/COUNT-ASSERTION-REVIEW.md).


Current presence implementation — 29 October 2026: Schema 0.9/v5 supports bounded unprefixed exists(child)/empty(child) Boolean predicates, retaining count-only v4 and no-call v1–v3. Complete Schema qualification: 451 passes (54 new); build zero warnings/errors; native example and real preserve/strip consumer tests pass. Two prior refusal expectations are explicitly adopted, 395 unchanged. Core 1.34.1 and its source-identical engine qualification remain inherited. Current implementation is uncommitted/unpublished; next is review and checkpoint. Earlier scope-only/presence-unsupported notices are historical capture state. [Implementation and evidence](docs/PRESENCE-ASSERTION-IMPLEMENTATION.md).


**29 October 2026 review:** Schema 0.9/v5 presence predicates reviewed: fresh 451/451 Schema tests pass and all seven tested implementation source identities match. Core 1.34.1 remains unchanged with inherited engine/gate qualification. No further production correction was needed. Earlier uncommitted notices are historical capture state; checkpoint delivery resolves through the review document Git history. [Review](docs/PRESENCE-ASSERTION-REVIEW.md).
