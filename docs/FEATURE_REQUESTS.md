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
| [REQ-009](ADR-005-xsd11-compiled-model-and-validation.md) staged XSD 1.1 support | In Progress | Compiled descriptors, assertion/facet evaluation, alternative applicability/filtering and selected metadata |
| [Core 1.81.2 dependency adoption](CORE-1.81.2-ADOPTION.md) | Merged | Four product references and one test provider reference updated; no C# adaptation |
| Core optional source integration | Refreshed in Core PR #472 | Merged Schema c953ebe pinned with source/test/fixture/legal hashes; 135 cases |
| Core schema-session integration | Internal native scalar adapter delivered; overall acceptance pending | Bounded original facts/scalar flow and host lifetime controls; broader component/compatibility/public consumer qualification pending |
| Complete XSD 1.1 structural validation | Incomplete | No complete current implementation or blanket conformance claim |
| Native decimal structural backend | Implemented internally; 76 cases pass | Complete finite grammar only; [evidence](native-decimal-backend/2026-11-15/README.md) |
| Native decimal Core adapter | Implemented internally; 20 cases pass | Authentic issuance, real scalar flow, guarded owner leases and shared sequential runner; public API/overall acceptance pending; [evidence](native-session-adapter/2026-11-15/README.md) |

## 2. Implemented Coverage

The public tests cover XSD version selection, schema components, explicit resolution/security, assertion evaluation, simple-type assertion facets, alternative compilation/applicability, open-content/default-attribute descriptor handling, open-content wildcard negative constraints (notNamespace/notQName) and the existing typed-XSLT integration. Support is bounded by those implementations and diagnostics; recorded metadata is not equivalent to structural enforcement.

## 3. Acceptance and Exclusions

Latest 15 November checkpoint: backend/profile work committed as `aba6967`; the next internal Core adapter step delivers 20 cases and 227 total standalone passes. [Adapter qualification map](NATIVE-SESSION-ADAPTER.md) distinguishes executed controls from remaining group acceptance, external consumer/API review and broader coverage. No P01–P12/global sign-off, version/tag/release or historical Core-lane requalification is implied. REQ-009 remains In Progress on the unchanged 0.4 development line.

Native backend delivery on 15 November: [grammar](NATIVE-DECIMAL-GRAMMAR.md) finalized, internal structural/assertion assessor implemented, 76 backend cases and 207 total standalone cases pass. Backend controls cover bounded portions of P01/P02/P05/P08 plus independent snapshot/concurrency observations; engine-root atomization, full adapter identity/environment/lifetime and overall P01–P12 acceptance remain pending. REQ-009 stays In Progress. Schema's 0.4 development line and all published Core pins remain unchanged; no tag or release is implied.

Earlier scalar-only checkpoint on 15 November: the [published-package scalar probe](native-session-probe/2026-11-15/README.md) passed seven cases, with 131 standalone cases then (124 retained plus seven). The [specification adoption record](NATIVE-DECIMAL-SPECIFICATION-ADOPTION.md) pinned six downloaded W3C publication bodies. Subsequent backend delivery above supersedes its implementation status and standalone count; its receipts remain historical. REQ-009 stays In Progress; no release/version change or general provider acceptance.

The [first native decimal session profile](NATIVE-SESSION-PROFILE.md) defines supported/refused coverage, a released-contract map and twelve paired planned acceptance groups. [ADR-009](ADR-009-native-decimal-session-profile.md) is Proposed. This documentation checkpoint retains the Schema 0.4 development line; the implementation release version is undecided. REQ-009 remains In Progress. No runtime functionality or provider acceptance is claimed.

The Apache transition passed 114 public schema tests with zero failures/skips. Commercial credential, payments, delivery and key-generation requests remain in the private licensing repository; they are not public schema dependencies or open-source engine features.

The transition does not implement the proposed core session API, change frozen W3C gates, admit skipped tests, certify XmlDb/Braid adoption or release a package. Future semantic additions require immutable specification adoption evidence and happy-path/edge regression tests.

The 3 November [O1/O2 versioned responses](core-disposition-responses/2026-11-03-v2/README.md) are scoped historical design acceptance, not proof of current native adapter/runtime delivery. Their original source/proposal hashes and the unsigned [confirmation drafts](core-disposition-inbox/2026-11-03/README.md) remain unchanged. The published Core 1.81.2 release and its budget deferral do not complete Schema's native provider/session or performance requirements. See [history](DEVELOPMENT_HISTORY.md).

*Last updated: 2026-11-15*
