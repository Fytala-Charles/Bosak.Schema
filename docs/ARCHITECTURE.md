<div align="center">
  <img src="../assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Architecture">
  <h1>Bosak.Schema Architecture</h1>
  <p>Modular Apache schema services with explicit resource and engine boundaries.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](PUBLICATION.json). No NuGet release or commercial infrastructure publication.

# Bosak.Schema Architecture

> Modular Apache schema services with explicit resource and engine boundaries.


## 1. Current Architecture

This Schema 0.8 feature candidate references unpublished Bosak XPath API/providers, XSLT and XQuery 1.34.1 through the explicit local source override; the original public checkout retains its published 1.26.3 pins. The core engine does not reference this package. Keeping schema services in a separate assembly avoids a dependency cycle; changing the license does not require merging them into Core.

## 2. Components

| Component | Responsibility and limit |
|---|---|
| `Compilation` | Explicit-version schema descriptor compilation and bounded resolved imports/includes |
| `Model` | Staged XSD 1.1 assertions, alternatives, declarations, simple facets, open-content/default-attribute metadata |
| `Validation` | Assertion/facet evaluation and supported governing-type applicability through Bosak XPath |
| `Assessment` | Native v1–v4 finite structural/decimal/assertion assessment, bounded count grammar and original occurrence/declaration state (0.8 candidate) |
| `SchemaServices` | Activation-free factories for compiler and validation services |
| `SchemaAwareXslt` | Opt-in helper for the existing schema-aware XSLT path |
| Legacy license DTO/shim types | API compatibility only; no credential implementation or authority |

The separate private licensing repository retains key issuers, payments, delivery/deployment services and historical commercial code. None is referenced by the public solution.

## 3. Identity and Integration Limits

The current compiled root-document digest is a legacy identifier, not proof of a complete schema-component universe. Full compatible-universe/PSVI session integration remains a separate design and provider qualification task. No new core session API is accepted by this licensing change.

Schema language version, host/XDM profile, component authority and source/snapshot identity must remain distinct in future integration. Preserve original PSVI across compatible schemas; QName or digest equality alone must not authenticate a component authority.

## 4. Key Design Decisions

| Decision | Status |
|---|---|
| [ADR-007: Apache/public schema split](ADR-007-apache-schema-publication.md) | Accepted by owner on 29 October 2026 |
| Preserve modular package and core dependency direction | Retained |
| Require no activation for standards processing | Implemented |
| Full XSD 1.1 provider/session integration | Pending; no conformance promotion |
| [ADR-008: bounded native assessment](ADR-008-bounded-native-assessment.md) | Implemented candidate; public API freeze pending |

## 5. Validation and Extensibility

The publication baseline has 114 tests. The 0.5 native assessment candidate passes 191 tests (114 existing plus 77 new) against published 1.26.3 and current core source; those overlapping scopes are not additive. Existing 114-case test binaries pass with only the Schema assembly replaced. Existing assertion, facet, alternative, resource-policy and typed-XSLT regression bodies are retained; commercial credential tests remain with the private infrastructure. New tests cover activation-free factories, valid/invalid assertions, ignored legacy activation, invalid directory input and absence of issuer/credential implementations.

No new engine implementation, baseline/filter change, storage migration or consumer adoption is included. Registry reclamation, full PSVI/component completeness and real consumer ABI qualification remain future acceptance gates.

## 6. Native Assessment Candidate

[ADR-008](ADR-008-bounded-native-assessment.md) separates complete admitted-profile assessment from descriptor assertion success. Compilation captures a closed single-schema model and finite XPath 2.0 assertion plans. Native validation checks structure and exact decimal values before assertions; successful results alone own snapshots and original declaration/type state. Unsupported coverage cannot fall back to XmlSchemaSet or expose a partially assessed document.

Anonymous root authority is retained in the native result, not invented from a QName or digest. Native copies preserve original PSVI; full typed XML projection is refused. Explicit decimal-child projections use the actual built-in type with the legacy provider, without claiming local-declaration interchange. The [profile guide](DOCUMENT-ASSESSMENT.md) documents limits, ownership, cancellation and the next root bridge. No core/session interface, normal engine dependency or global conformance admission changes.

*Last updated: 2026-10-29*

## Whole-document bridge candidate — 29 October 2026

The unpublished core 1.34.0 provider bridge now carries original anonymous-root and decimal-child state through whole-document exports and preserve/strip copies. Root atomization raises FOTY0012; stale content and legacy reassessment refuse explicitly. Use `-p:BosakSourceRoot=D:/Development/Bosak` for builds/tests/examples because candidate package dependencies are unpublished. See [the current guide](DOCUMENT-ASSESSMENT.md). The earlier 191-case receipts describe the historical projection-only checkpoint; fresh bridge qualification is recorded separately in the core `docs/schema-document-bridge` dossier. Broader coverage, consumer adoption and API freeze remain pending. No commit/push/tag/publication.

## Reviewed whole-document bridge — 29 October 2026

The optional provider attachment API now checks a replacement completely before removing the previous binding. Failed replacement retains original state and stale-content refusal; failed initial attachment leaves the node unregistered. The trusted host must supply immutable state and the correct assessed value. This API carries that state without performing schema validation. The finite native profile is unchanged; optional children and general API freeze remain pending. Current core candidate is unpublished 1.34.0; use the BosakSourceRoot override. New review qualification lives in the core `docs/schema-document-bridge/api-review` dossier, separately from historical assessment receipts.

Post-review verification: **24,209 core tests pass in one complete solution run**, **204 Schema tests pass**, and all six focused provider attachment cases pass. Release build: zero warnings/errors. QT3 **31,397/0/424**, qt4 **8,941/0/1,918** (230 gated sets, 11 experimental passes), with exact skip mappings unchanged. The full-document example retains original root state and passes typed-value and preserve/strip checks. Prior sealed assessment, bridge and integration receipts remain unchanged.

## Optional decimal child scope — 29 October 2026

The [new adoption record](OPTIONAL-DECIMAL-ADOPTION-2026-10-29.md) and [implementation plan](OPTIONAL-DECIMAL-PLAN.md) define the next finite extension. It is not implemented. Keep occurrence bounds on particles, actual element states only for present nodes, and use effective Boolean conversion for empty assertion results. Maximum occurrence stays one; no broader grammar/session admission. Proposed implementation version is Schema 0.6; this documentation step keeps 0.5 and core 1.34.0 unchanged. Fresh baseline: 204 Schema tests pass. Core solution/gates inherit the source-identical `a163ff11` checkpoint. No REQ completion or publication.

Scope refinement: a thirteenth isolated observation exposes ignorable-whitespace removal being refused for an empty element-only assessed root. Supporting all-optional-absent documents therefore needs a bounded provider binding correction. The future implementation proposes core patch 1.34.1 plus fresh complete core tests and exact conformance gates; current versions and production source stay unchanged. Both the initial twelve and extended thirteen-observation receipts are preserved in the optional-child dossier; they overlap.

## Optional decimal scope delivery — 29 October 2026

See the [synchronized checkpoint](OPTIONAL-DECIMAL-CHECKPOINT.md) for current delivery identity and remote target. Documentation is prepared for the user-authorized commit/push; optional-child semantics are not implemented. Current versions/pins and production source remain unchanged. Thirteen observations and 204 passing Schema baseline cases support the scope; the core suite/gates are inherited. Next: implement the bounded plan and its core binding correction with fresh required qualification.

## Optional decimal implementation candidate — 29 October 2026

Current behavior: [Schema 0.6 optional assessment](OPTIONAL-DECIMAL-IMPLEMENTATION.md), with core 1.34.1 and required local source override. Local zero-or-one particles, ordered matching, assertion EBV and present-only original state are implemented; required-only v1 remains and optional schemas use v2. Empty element-only root whitespace normalization is corrected while real/decimal mutation refusal remains. The complete Schema suite passes 260 cases, including 56 new optional cases; the full-document example retains later declaration identity. Earlier planned-scope/version notices are historical. Broader REQ-009/general session/API freeze and consumer acceptance remain pending. Original public checkout and product pins are unchanged; implementation is uncommitted and unpublished.

Fresh final qualification: **24,214 complete core tests pass** (including 8,196 XSLT and 212 provider cases), **260 complete Schema tests pass** (56 new optional cases), normal QT3 **31,397/0/424**, qt4 **8,941/0/1,918**, 230 gated sets and 11 experimental passes. Exact skip names/reasons match the sealed integration checkpoint. Core build has zero errors and five warnings in unchanged Standard test/XSLT conformance sources; no new warning source is introduced. The optional-gap whole-document example and strict Schema documentation check pass. Original 45-case before evidence is 9 passed/36 failed; all 45 now pass alongside eleven added boundary cases. Five new core cases cover the root binding correction.

## Optional decimal review checkpoint — 29 October 2026

Current delivery: [reviewed optional implementation](OPTIONAL-DECIMAL-REVIEW.md), Schema 0.6/core 1.34.1. No further production fix was needed; source matches tested qualification. The user authorized checkpoint commit/push; original uncommitted notices are capture-time history. Next: scope repetitions separately before implementation. Full schema/session/API freeze and product adoption remain pending.

## Bounded repeated decimal scope — 29 October 2026

The [repetition adoption](REPEATED-DECIMAL-ADOPTION-2026-10-29.md) and [implementation plan](REPEATED-DECIMAL-PLAN.md) define the next slice. Repetitions are not implemented. The reviewed optional source is checkpointed at Schema `773fd69` / core `5e277c6a`; current Schema 0.6/core 1.34.1 remain unchanged. Sixteen diagnostic observations cover current refusal and real-engine comparison cardinality, not native repetition acceptance. Proposed Schema 0.7/v3 keeps grammar and public overloads unchanged, bounds total occurrences, preserves original state, handles known cardinality errors and refuses ambiguous singular projection. Broader API/session acceptance remains pending.

## Bounded repeated decimal candidate — 29 October 2026

Current behavior: [Schema 0.7/v3 repetition implementation](REPEATED-DECIMAL-IMPLEMENTATION.md). Local minimum zero/one, maximum and total budget up to sixteen, contiguous ordered states, shared original declarations, narrow XPTY0004 assertion failure and ambiguous singular projection refusal are implemented. Required-only v1 and optional v2 remain; broader grammar is refused. Fresh complete Schema tests: 334 passes (74 new). Build/example pass; unchanged core 1.34.1 full/gate evidence is inherited after source checks. Earlier planned-scope/version notices describe earlier capture. General schema/session acceptance and product adoption remain pending. Source changes are uncommitted and unpublished.

Final repeated-candidate qualification: **334 complete Schema cases pass**, including **74 new repetition/error-boundary cases**. The original 57-case before cohort (17 passed/40 failed) now all passes; seventeen further boundary/safety cases also pass. The prior 260-case suite retains 259 unchanged passing expectations and one explicitly adopted maxOccurs-two coverage/profile update. Schema source-override Release build: **zero warnings/errors**. The repeated-document XPath/XSLT example and strict Schema docs check pass. Core source/tests/harness/fixture references and build configuration are unchanged from the reviewed source, so **24,214 core passes**, QT3 **31,397/0/424** and qt4 **8,941/0/1,918** (230 sets) are inherited, not fresh full runs. Exact raw source/receipt identities and all earlier sealed dossiers are preserved.

## Repetition implementation review checkpoint — 29 October 2026

Current delivery: [reviewed Schema 0.7 implementation](REPEATED-DECIMAL-REVIEW.md), with unchanged core 1.34.1. No further production fix was needed; tested source identities and 334-case qualification match. Review/checkpoint is user-authorized. Count assertion support will be a separate adoption/plan; it is not implemented now. Earlier uncommitted notices describe original capture. General API/session acceptance and product adoption remain pending.

## Count assertion scope — 29 October 2026

The [count adoption](COUNT-ASSERTION-ADOPTION-2026-10-29.md), [source locks](COUNT-ASSERTION-SOURCES.json) and [plan](COUNT-ASSERTION-PLAN.md) define the next slice. Unprefixed count(DeclaredChildName) is proposed; no count grammar is implemented yet. Prefixed calls/aliases and wider functions remain Unsupported, and bare count child names retain selection semantics. Current Schema 0.7/core 1.34.1 source is unchanged from reviewed qualification. Schema 0.8/v4 is a proposed future target. Native structure/exact-value validation, original state and copy semantics remain mandatory before assertions. No API freeze or release.

## Count assertion candidate — 29 October 2026

Current behavior: [Schema 0.8/v4 count admission](COUNT-ASSERTION-IMPLEMENTATION.md), with unchanged core 1.34.1. Unprefixed count(DeclaredChildName) is admitted in the bounded comparison/logical grammar; bare count names and no-count v1/v2/v3 profiles are preserved. Native structure/exact values still precede assertions. Complete Schema tests: 397 passes (63 new); build/example/docs pass. One historical count refusal is explicitly adopted, with 333 other prior expectations unchanged. Core full/gate qualification is inherited after source/configuration checks, not rerun. Earlier scope-only notices are capture-time history. General API/session/namespace/function coverage and consumer adoption remain pending. Current changes are uncommitted and unpublished.

Final count-candidate qualification: **397 complete Schema cases pass**, including **63 new count/grammar/validation-order/copy cases**. The original 57-case cohort (23 passed/34 failed) now all passes; six further profile/optimization-boundary cases pass. The prior 334-case suite retains **333 unchanged passing expectations** plus the explicitly adopted count capability check. Source-override build has **zero warnings/errors**. The actual native count/absence example and strict documentation checks pass. Core source/tests/harness/fixture references and build configuration remain identical, so **24,214 core passes**, QT3 **31,397/0/424**, qt4 **8,941/0/1,918** (230 sets) are inherited, not fresh full runs. All earlier sealed evidence and original refusal source/receipts remain unchanged.


**29 October 2026 review:** Schema 0.8 count assertions reviewed: fresh 397/397 Schema tests pass; core 1.34.1 source and inherited engine gates remain unchanged. Next is the separately scoped presence-predicate implementation; exists/empty remain unsupported in this checkpoint. [Review and next scope](COUNT-ASSERTION-REVIEW.md).


Current presence implementation — 29 October 2026: Schema 0.9/v5 supports bounded unprefixed exists(child)/empty(child) Boolean predicates, retaining count-only v4 and no-call v1–v3. Complete Schema qualification: 451 passes (54 new); build zero warnings/errors; native example and real preserve/strip consumer tests pass. Two prior refusal expectations are explicitly adopted, 395 unchanged. Core 1.34.1 and its source-identical engine qualification remain inherited. Current implementation is uncommitted/unpublished; next is review and checkpoint. Earlier scope-only/presence-unsupported notices are historical capture state. [Implementation and evidence](PRESENCE-ASSERTION-IMPLEMENTATION.md).


**29 October 2026 review:** Schema 0.9/v5 presence predicates reviewed: fresh 451/451 Schema tests pass and all seven tested implementation source identities match. Core 1.34.1 remains unchanged with inherited engine/gate qualification. No further production correction was needed. Earlier uncommitted notices are historical capture state; checkpoint delivery resolves through the review document Git history. [Review](PRESENCE-ASSERTION-REVIEW.md).


Consumer-priority assessment (29 October 2026): qualify the actual XmlDb persistent-node adapter in an isolated source workspace next, then Braid authoring/preview compatibility. No inspected dependency requires more native assertion functions now. Native structural expansion needs a concrete schema/instance use case. Schema 0.9/core 1.34.1 and all prior qualifications remain unchanged; no consumer execution or dependency adoption is claimed. [Assessment and acceptance plan](CONSUMER-COVERAGE-PRIORITIES.md).


Isolated XmlDb qualification (29 October 2026): committed adapter source builds unchanged against current core; 27/27 existing Xdm tests pass on both 1.26.2 and 1.34.1. All 44 bounded diagnostic observations agree, including 26 query pairs, seven persisted kind mappings and nested snapshot disposal. Unrelated consumer dirty work is excluded. No dependency adoption, binary/native/API acceptance or production fix. Next: isolated Braid source compatibility. [Qualification and limits](XMLDB-COMPATIBILITY-QUALIFICATION.md).


Isolated Braid qualification (29 October 2026): all actual consumer source builds against current core without a production fix. Original full suites on 1.26.2/current 1.34.1 each report 1,872 passes/four exact-version failures; separate corrected/adapted identity follow-ups pass all four. No single green unchanged full-suite claim, actual dependency adoption or native desktop acceptance. Next: review/checkpoint the consumer-priority and both compatibility dossiers. [Qualification and limits](BRAID-COMPATIBILITY-QUALIFICATION.md).


Consumer qualification reviewed (29 October 2026): exact sealed receipts and test identities verified; XmlDb 27 passes per version/44 matching observations, Braid 1,872 passes/four version failures plus separate four passing identity follow-ups per version. No further production fix, dependency adoption or binary/native acceptance. Earlier uncommitted notices are historical; delivery resolves through the review document Git history. Next: separately scope unchanged-binary qualification. [Review checkpoint](CONSUMER-QUALIFICATION-REVIEW.md).
