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
| Native document assessment (REQ-009, 0.5 candidate) | Implemented candidate | Complete admitted-profile structure, exact decimals and assertions; owned original component state and bounded typed exports |
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
