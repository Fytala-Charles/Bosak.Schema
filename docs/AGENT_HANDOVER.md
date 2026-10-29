Current delivery: [optional decimal scope checkpoint](OPTIONAL-DECIMAL-CHECKPOINT.md). This is the entry point for current source bases, qualification and remote targets; older checkpoint sections below are historical.

Current next slice: [optional decimal child plan](OPTIONAL-DECIMAL-PLAN.md), with its pre-implementation adoption record. Product baseline remains the reviewed whole-document bridge; optional support is not yet implemented.

# Reviewed whole-document bridge checkpoint — 29 October 2026

Current candidate: Schema 0.5 development line with unpublished core 1.34.0 source override. The original public checkout/pins are untouched. The API review and local checkpoint receipts are in the owning core `docs/schema-document-bridge/api-review` dossier. Earlier projection-only notices below are historical; full-document export is now qualified for the finite profile. General API/session acceptance remains pending.

# Native document assessment candidate — 29 October 2026

Current feature worktree: `feature/bounded-document-assessment`, based on public `044d2cb2e67decd46e03e0f5ee7cb03029f34bd9`; implementation changes remain uncommitted. This is the 0.5 MinVer development line, with original published Bosak 1.26.3 package pins unchanged. No tag, hosted CI, package publication or core/session API freeze.

What was built: strict finite native XSD 1.1 schema/profile compiler, native structural/exact-decimal checks before assertions, immutable successful document snapshots and original declaration/type/value state, native PSVI-preserving copies, stripped XML copies and explicit decimal-child projections. A full typed anonymous-root export is refused. The [guide](DOCUMENT-ASSESSMENT.md), [adoption record](ASSESSMENT-ADOPTION-2026-10-29.md) and [ADR-008](ADR-008-bounded-native-assessment.md) own the exact boundary.

Fresh qualification: 191 complete Schema cases (114 original plus 77 new), against both published 1.26.3 and current core source, and 114 unchanged original test binaries with only Schema replaced. Example reports valid assessment, decimal typing, total 15 and successful preserve/strip checks. These scopes overlap; no full XSD 1.1 certification or new XmlDb/Braid adoption. The original first fixture failure used a detached initial context; later copy fixtures required the documented empty BCL annotation context. Original receipts remain separate from final passes.

Next: qualify a minimal whole-document/anonymous-root bridge retaining actual components, atomization errors, preserving/stripping copies, cancellation and lifetime. Preserve earlier general-session proposals and pending acceptance records; do not adopt all proposed types by implication. Follow the existing Git rule: no commit/push/tag unless explicitly requested.

## Historical Apache publication checkpoint

# Bosak.Schema handover — 29 October 2026

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](PUBLICATION.json). No NuGet release or commercial infrastructure publication.

This is the clean Apache-2.0 public schema repository. The original private repository/history and billing/key-generation code are preserved separately as Bosak.Licensing. Do not copy private history, credential helpers, deployment settings or commercial test vectors into this repository.

What was built: owner-approved schema-only license/publication split, Apache/SPDX/package metadata, activation-free factories, credential-free legacy API shims, public CI and a 114-case schema regression suite. Source base `fa0da3e2d9972706a5a86616ef285af4daf5037b`; public delivery commit is recorded in the publication receipt and Git history.

Current development line: 0.4 through MinVer; no new tag/NuGet publication is implied. Core package pins remain 1.26.3. Full XSD 1.1 structural validation and the core session/PSVI integration are incomplete/pending. The licensing change does not accept the C44-R2 public API proposal.

Next: reassess the smallest necessary core/session contract around the open implementation, then qualify real component completeness, compatible-schema handoff and consumers before engine integration. See [architecture](ARCHITECTURE.md), [registry](FEATURE_REQUESTS.md), [integration](INTEGRATION.md) and [transition](APACHE-TRANSITION.md).

Last updated: 2026-10-29

## Whole-document bridge candidate — 29 October 2026

The unpublished core 1.34.0 provider bridge now carries original anonymous-root and decimal-child state through whole-document exports and preserve/strip copies. Root atomization raises FOTY0012; stale content and legacy reassessment refuse explicitly. Use `-p:BosakSourceRoot=D:/Development/Bosak` for builds/tests/examples because candidate package dependencies are unpublished. See [the current guide](DOCUMENT-ASSESSMENT.md). The earlier 191-case receipts describe the historical projection-only checkpoint; fresh bridge qualification is recorded separately in the core `docs/schema-document-bridge` dossier. Broader coverage, consumer adoption and API freeze remain pending. No commit/push/tag/publication.

Fresh bridge qualification: Release build has zero warnings/errors; complete core aggregate **24,207 passed**, native Schema **204 passed** (13 whole-document cases), QT3 **31,397/0/424**, qt4 gated **8,941/0/1,918**, 230 sets and 11 experimental passes. Exact skip mappings match the sealed integration checkpoint. The initial sandbox database failures are retained separately; the local-HTTP-authorized rerun passes all 47 cases. Whole-document example and strict Schema documentation checks pass.

Core base `a2ade8fd`; core candidate 1.34.0, Schema feature base `044d2cb2e67decd46e03e0f5ee7cb03029f34bd9`. What was built: original-state whole-document export, atomization/copy/mutation protections and full-document runnable example. Core dossier `docs/schema-document-bridge` owns new receipts, separately from sealed assessment evidence. Next: bounded-candidate review before broader coverage/session acceptance. Both trees remain uncommitted under the existing Git rule.

## Reviewed whole-document bridge — 29 October 2026

The optional provider attachment API now checks a replacement completely before removing the previous binding. Failed replacement retains original state and stale-content refusal; failed initial attachment leaves the node unregistered. The trusted host must supply immutable state and the correct assessed value. This API carries that state without performing schema validation. The finite native profile is unchanged; optional children and general API freeze remain pending. Current core candidate is unpublished 1.34.0; use the BosakSourceRoot override. New review qualification lives in the core `docs/schema-document-bridge/api-review` dossier, separately from historical assessment receipts.

Post-review verification: **24,209 core tests pass in one complete solution run**, **204 Schema tests pass**, and all six focused provider attachment cases pass. Release build: zero warnings/errors. QT3 **31,397/0/424**, qt4 **8,941/0/1,918** (230 gated sets, 11 experimental passes), with exact skip mappings unchanged. The full-document example retains original root state and passes typed-value and preserve/strip checks. Prior sealed assessment, bridge and integration receipts remain unchanged.

Local checkpoint authorized by the user instruction to continue the review/commit recommendations. Delivery revision: `git log -1 --format=%H -- docs/AGENT_HANDOVER.md`. What was built includes the bounded native assessment, original-state whole-document export, executable example and review correction qualified above. Core delivery is independently recorded in its `docs/schema-document-bridge/api-review/CHECKPOINT.json`. No push/tag/package/merge. Next: separately pin and plan optional-decimal-child semantics; no general API freeze.

## Optional decimal child scope — 29 October 2026

The [new adoption record](OPTIONAL-DECIMAL-ADOPTION-2026-10-29.md) and [implementation plan](OPTIONAL-DECIMAL-PLAN.md) define the next finite extension. It is not implemented. Keep occurrence bounds on particles, actual element states only for present nodes, and use effective Boolean conversion for empty assertion results. Maximum occurrence stays one; no broader grammar/session admission. Proposed implementation version is Schema 0.6; this documentation step keeps 0.5 and core 1.34.0 unchanged. Fresh baseline: 204 Schema tests pass. Core solution/gates inherit the source-identical `a163ff11` checkpoint. No REQ completion or publication.

Current Schema HEAD `766eff7a77ee08e9c38cf479994e94eaf232d242`; the original scope capture preceded the authorized delivery below. What was built: pre-implementation immutable normative selection, matching/state/assertion/export design and acceptance matrix; no production source change. Next: implement that plan with before/after controls. Preserve sealed core dossiers; evidence/snapshots for this step are in core `docs/schema-optional-children`.

Scope refinement: a thirteenth isolated observation exposes ignorable-whitespace removal being refused for an empty element-only assessed root. Supporting all-optional-absent documents therefore needs a bounded provider binding correction. The future implementation proposes core patch 1.34.1 plus fresh complete core tests and exact conformance gates; current versions and production source stay unchanged. Both the initial twelve and extended thirteen-observation receipts are preserved in the optional-child dossier; they overlap.

## Optional decimal scope delivery — 29 October 2026

See the [synchronized checkpoint](OPTIONAL-DECIMAL-CHECKPOINT.md) for current delivery identity and remote target. Documentation is prepared for the user-authorized commit/push; optional-child semantics are not implemented. Current versions/pins and production source remain unchanged. Thirteen observations and 204 passing Schema baseline cases support the scope; the core suite/gates are inherited. Next: implement the bounded plan and its core binding correction with fresh required qualification.
