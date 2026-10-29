<div align="center">
  <img src="../assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Feature Requests">
  <h1>Bosak.Schema Feature Requests</h1>
  <p>Current public schema scope, status and acceptance boundaries.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](PUBLICATION.json). No NuGet release or commercial infrastructure publication.

# Bosak.Schema Feature Requests

> Current public schema scope, status and acceptance boundaries.


## 1. Registry

| Request | Status | Scope |
|---|---|---|
| Apache/public transition | Implemented and GitHub publication verified | Schema-only Apache-2.0; activation-free services; private infrastructure excluded |
| REQ-009 staged XSD 1.1 support | In Progress | Compiled descriptors, assertion/facet evaluation, alternative applicability/filtering and selected metadata |
| Core schema-session integration | Pending | Complete component authority, actual PSVI/atomic flow, compatible-schema interchange and real consumer qualification |
| Native document assessment (REQ-009, 0.9 candidate) | Implemented candidate | Finite required/optional/repeated particles, bounded count/presence assertions, exact decimals and original typed occurrence state |
| Complete XSD 1.1 structural validation | Incomplete | One native profile does not establish complete coverage or conformance |

## 2. Implemented Coverage

The public tests cover XSD version selection, schema components, explicit resolution/security, assertion evaluation, simple-type assertion facets, alternative compilation/applicability, open-content/default-attribute descriptor handling and the existing typed-XSLT integration. Support is bounded by those implementations and diagnostics; recorded metadata is not equivalent to structural enforcement.

## 3. Acceptance and Exclusions

The Apache transition passed 114 public schema tests with zero failures/skips. Commercial credential, payments, delivery and key-generation requests remain in the private licensing repository; they are not public schema dependencies or open-source engine features.

The transition does not implement the proposed core session API, change frozen W3C gates, admit skipped tests, certify XmlDb/Braid adoption or release a package. Future semantic additions require immutable specification adoption evidence and happy-path/edge regression tests.

## 4. Native Assessment Acceptance Scope

The [0.5 candidate](DOCUMENT-ASSESSMENT.md) implements `decimal-sequence-assertions-v1` under [ADR-008](ADR-008-bounded-native-assessment.md) and pre-implementation [immutable normative adoption](ASSESSMENT-ADOPTION-2026-10-29.md). All 191 source cases pass, including 77 new happy/error/unsupported/boundary/copy/concurrency cases, on published and current core. The 114 legacy test binaries pass with the new library. Source/API additions are candidate work, not package publication, native product adoption or general API freeze.

The broader REQ-009 remains In Progress. Anonymous-root projection, general component closure, compatible universes, named/anonymous/list/union derivation and broader structural features remain acceptance work. No core REQ-139 completion or frozen skip removal is inferred.

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


Unchanged-binary qualification scoped (29 October 2026): exact retained baseline/candidate assembly hashes and identities are inventoried. Next: frozen baseline controls then engine-only replacement, preserving consumer DLLs, hosts, manifests and original version assertions. No replacement or new consumer execution yet; source qualification is not binary compatibility. Core 1.34.1/Schema 0.9 and actual product pins remain unchanged. [Execution plan](UNCHANGED-BINARY-COMPATIBILITY-PLAN.md).


Unchanged-binary qualification (29 October 2026): baseline controls pass XmlDb 27/Braid 1,876; engine-only replacement passes XmlDb 27 with identical 44 observations and Braid 1,872/four original version failures. All consumer/host/manifest bytes stay frozen; actual candidate loads and preview workers are verified. Additional public-contract probe shows Braid inspector/preview identity stays old pin despite loaded 1.34.1; transparent product adoption is not qualified. No production fix or dependency adoption. Next: review/checkpoint and separately resolve consumer runtime-identity policy before upgrade. [Results and limits](UNCHANGED-BINARY-COMPATIBILITY-RESULTS.md).


Unchanged-binary qualification reviewed (29 October 2026): raw counts/identities, frozen layouts and actual replacement loads verified. XmlDb 27/27 plus identical 44 observations; Braid baseline 1,876 passes, replacement 1,872/four original version failures. Braid advertised runtime identity remains old pin; transparent adoption is not qualified. No production fix/pin/release change. Earlier uncommitted notices are historical; delivery resolves through review Git history. Next: separately scope consumer identity/adoption policy. [Review checkpoint](UNCHANGED-BINARY-COMPATIBILITY-REVIEW.md).


Braid identity/adoption policy scoped (29 October 2026): recommend reviewed rebuild at an explicit pin plus runtime mismatch refusal using an approved artifact manifest, consistent parent/worker identity and preserved historical results. Dynamic automatic replacement is deferred. Next: isolated contract prototype with admission/refusal controls and explicit runtime-verification limits. No Braid/core implementation, actual pin, protocol, storage or release change; scope only. [Policy and acceptance scope](BRAID-ENGINE-IDENTITY-POLICY-SCOPE.md).


Isolated identity-policy prototype (29 October 2026): 47 modeled contract controls pass, six enforcement mutants detected, 18 actual PE/hash observations match. Recorded Braid trace observes eight/nine engine members; complete-runtime evidence therefore refuses. Model flags/envelopes are not an actual runtime/worker guard. No product/pin/core API change. Next: review/checkpoint, then resolve a real lazy-loading/binding admission design in isolation. [Prototype evidence and limits](BRAID-IDENTITY-POLICY-PROTOTYPE.md).


Identity-policy prototype reviewed (29 October 2026): 47 controls/six detected mutants/18 real file observations reconciled; tested source/project/assembly hashes match. All prior evidence preserved. Runtime-binding flags and worker envelopes remain modeled; recorded eight/nine loaded members expose an unresolved lazy-admission boundary. No actual product guard, pin, protocol or core API change. Earlier uncommitted notices are capture history; delivery resolves through review Git history. Next: isolated actual runtime/lazy-binding experiment. [Review checkpoint](BRAID-IDENTITY-POLICY-REVIEW.md).


Actual-runtime admission experiment (29 October 2026): 34 real controls pass with nine independent worker exchanges. Approved snapshots allow eight-member XSLT then deferred ninth-member XQuery binding; actual byte/identity/MVID evidence, drift revocation and worker mismatch rejection are recorded. This is a standalone custom-load-context host, not Braid integration or a product protocol/pin/core API change. Next: review/checkpoint, then consumer-owned boundary/type-identity design. [Runtime evidence and limits](BRAID-ACTUAL-RUNTIME-ADMISSION.md).


Actual-runtime experiment reviewed (29 October 2026): 34 real controls/nine worker exchanges, tested source/assembly hashes and approved snapshot ledgers verified; earlier evidence preserved. Lazy eight-to-nine-member binding works in the standalone host. Actual Braid typed adapters/default loader/protocol are not integrated; no product pin/core API change. Earlier uncommitted notices are historical; delivery resolves through review Git history. Next: consumer-owned integration-boundary design. [Review checkpoint](BRAID-ACTUAL-RUNTIME-REVIEW.md).


Braid integration design (29 October 2026): prefer approved snapshot-backed adapter sessions sharing exact host Application/Core contracts. Real typed casts, opaque ownership and callback/lifecycle behavior remain to prove; default-loader hash prechecks do not establish the same binding. Next: isolated real read-only inspector typed-port proof. No actual Braid implementation, dependency pin, protocol, storage or core API change. [Design and vertical-slice acceptance](BRAID-ENGINE-SESSION-INTEGRATION-DESIGN.md).


Real typed-inspector proof (29 October 2026): 41 controls pass; four encodings match a separate direct baseline across shared Application/Core types, callbacks, source bytes/extents and actual target ownership. Raw port still returns NoChange after unload request; diagnostic lease closure alone is insufficient for a product guard. No edits/product pin/core API change. Next: review/checkpoint, then real exported-port facade admission proof. [Typed proof and lifetime boundary](BRAID-TYPED-INSPECTOR-PROOF.md).


Typed-inspector/integration design reviewed (29 October 2026): 41 controls, four exact direct/isolated projections and private/shared assembly hashes verified; earlier evidence preserved. Raw-port NoChange remains callable after unload request, so outer lease/unload is not revocation. No actual production guard, pin or core API change. Earlier uncommitted notices are historical; delivery resolves through review Git history. Next: isolated guarded exported-port/target facade proof. [Review checkpoint](BRAID-TYPED-INSPECTOR-REVIEW.md).


Guarded-port facade proof (29 October 2026): 54 controls pass over the actual inspector. Supported public result ports/targets are wrapped or removed; closed/foreign/drift admission and controlled in-flight close refuse raw authority publication. Profile is inspection/rename-target/exact NoChange only; broader authoring remains refused. Raw adapter control still reproduces the old lifetime gap. No actual product guard/pin/core API change. Next: review/checkpoint, then consumer-owned facade/lifecycle scope. [Facade evidence and limits](BRAID-GUARDED-PORT-FACADE-PROOF.md).


Guarded-port facade reviewed (29 October 2026): 54 controls, nine result ports/seven target properties and source/assembly/artifact hashes verified; prior evidence preserved. Profile remains inspection/rename-target/exact NoChange only; controlled close is qualified, arbitrary concurrency is not. Raw adapter gap remains unchanged in product. No actual guard/pin/core API change. Earlier uncommitted notices are history; delivery resolves through review Git history. Next: consumer-owned facade/lifecycle contract scope. [Review checkpoint](BRAID-GUARDED-PORT-FACADE-REVIEW.md).


Consumer facade/lifecycle contract scoped (29 October 2026): propose one active engine call per session with immediate overlapping/reentrant refusal, nonblocking close initiation and retained in-flight ownership until completion. Separate engine capability facts from admitted operations; guard every returned authority surface. Post-admission cancellation, callback/drain ordering and broader schedules still need proof. No product/core API/pin/protocol change. Next: isolated deterministic lifecycle proof. [Contract scope and schedule matrix](BRAID-FACADE-LIFECYCLE-CONTRACT-SCOPE.md).


Deterministic facade lifecycle proof (29 October 2026): 65 controls across 13 isolated cases pass, including real callback overlap/reentry, self-drain refusal, post-admission cancellation, caller-abandoned waits and incomplete drain. All leases balance; one underlying release request per case, not GC proof. Engine facts/admission profile are separate. Arbitrary concurrency and actual Braid product integration remain unqualified. No product pin/core API change. Next: review/checkpoint, then finite consumer adoption proposal. [Schedule evidence and limits](BRAID-DETERMINISTIC-LIFECYCLE-PROOF.md).


Lifecycle contract/proof reviewed (29 October 2026): 65 controls and 13 ordered state/lease traces verified, with balanced leases and one idle release request per case; source/assembly/snapshot hashes match. Earlier evidence preserved. Finite callback/cancellation schedules do not certify arbitrary concurrency or actual product integration. No pin/core API change. Earlier uncommitted notices are history; delivery resolves through review Git history. Next: finite consumer-owned read-only adoption proposal. [Review checkpoint](BRAID-LIFECYCLE-PROOF-REVIEW.md).


Finite read-only adoption proposal (29 October 2026): additive inspection-only session component at Braid current 1.26.2 pin, with port-free data, explicit lifecycle/refusals and bounded drain. Draft API shape compiles with XML docs and zero warnings/errors; no implementation or consumer test run. Existing designer editing/preview/export defaults remain outside this component. No actual pin/protocol/core API change. Next: review/checkpoint, then consumer-owned implementation/ADR scope. [API proposal and adoption gates](BRAID-READONLY-ADOPTION-PROPOSAL.md).


Read-only adoption proposal reviewed (29 October 2026): compiled draft source/project/assembly/XML hashes and port-free data shape verified; zero-warning/error build receipt preserved. This is an additive inspection-only proposal at Braid 1.26.2, not implementation or actual public API adoption. Existing default editing/preview/export stays untouched; no product pin/core API/ID change. Earlier uncommitted notices are history; delivery resolves through review Git history. Next: consumer-owned implementation task/ADR at the unchanged pin. [Proposal review checkpoint](BRAID-READONLY-ADOPTION-REVIEW.md).
