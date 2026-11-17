<div align="center">
  <img src="../assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Feature Requests">
  <h1>Bosak.Schema Feature Requests</h1>
  <p>Current public schema scope, status and acceptance boundaries.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](PUBLICATION.json). Historical GitHub transition receipt; the current NuGet release is recorded below.

# Bosak.Schema Feature Requests

> Current public schema scope, status and acceptance boundaries.


## 1. Registry

The 17 November public-entry candidate implements `NativeDecimalSession`, `NativeDecimalDocument`, immutable options/context and owned results with fixed bounded stage diagnostics. It keeps Core 1.81.2 and the finite backend grammar unchanged. The 0.5 development line has 272 passing cases: 249 implementation cases (243 retained plus six lifecycle/cleanup controls) and 23 separate public-consumer cases without friend access. The consumer also passes through the locally packed Schema binary and published Core dependencies; this repeats its case catalog and is not additive coverage. [Contract and qualification](NATIVE-DECIMAL-PUBLIC-API.md). The alpha is published; overall profile acceptance and API freeze remain pending.

| Request | Status | Scope |
|---|---|---|
| Apache/public transition | Implemented and GitHub publication verified | Schema-only Apache-2.0; activation-free services; private infrastructure excluded |
| [REQ-009](ADR-005-xsd11-compiled-model-and-validation.md) staged XSD 1.1 support | In Progress | Compiled descriptors, assertion/facet evaluation, alternative applicability/filtering and selected metadata |
| [Core 1.81.2 dependency adoption](CORE-1.81.2-ADOPTION.md) | Merged | Four product references and one test provider reference updated; no C# adaptation |
| Core optional source integration | Refreshed in Core PR #503 | Merged Schema9fd4e6a pinned with58 source/test/fixture/legal inputs;260 implementation/legacy plus nine public current-Core cases. Original135-case receipt remains historical. |
| Core schema-session integration | Internal native scalar adapter delivered; overall acceptance pending | Bounded original facts/scalar flow and host lifetime controls; broader component/compatibility/public consumer qualification pending |
| NuGet trusted publishing | Published alpha verified | Manual prepare/publish separation;45 release controls; approved exact artifact, repository signature and registry-only consumer verified |
| Complete XSD 1.1 structural validation | Incomplete | No complete current implementation or blanket conformance claim |
| Native decimal structural backend | Implemented internally; 76 cases pass | Complete finite grammar only; [evidence](native-decimal-backend/2026-11-15/README.md) |
| Native decimal Core adapter | Implemented internally; 28 cases pass | Issuance, scalar flow, guarded leases, expiry and cleanup; [latest evidence](native-session-facade/2026-11-15/README.md) |
| Owned native decimal entry point | Published in0.5.0-alpha.0.7;23 external consumer cases and six added lifecycle controls pass | [ADR-010](ADR-010-owned-native-decimal-facade.md), [proposal](NATIVE-DECIMAL-PUBLIC-API-PROPOSAL.md); development minimum0.5; alpha published, beta acceptance pending |

## 2. Implemented Coverage

The public tests cover XSD version selection, schema components, explicit resolution/security, assertion evaluation, simple-type assertion facets, alternative compilation/applicability, open-content/default-attribute descriptor handling, open-content wildcard negative constraints (notNamespace/notQName) and the existing typed-XSLT integration. Support is bounded by those implementations and diagnostics; recorded metadata is not equivalent to structural enforcement.

## 3. Acceptance and Exclusions

Historical 15 November follow-up after adapter commit `939ba9a`: eight additional adapter controls plus eight facade cases bring the suite to 243. Default lookup names now reject with ArgumentException instead of NullReferenceException. The owned prototype cleans unsuccessful admissions and returns detached values/XML; the [public proposal](NATIVE-DECIMAL-PUBLIC-API-PROPOSAL.md) is reviewable but unimplemented publicly. Release build with network-enabled NuGet audit and the official PowerShell 7 documentation check pass. REQ-009/global acceptance remains In Progress; no release/tag or historical Core-lane requalification.

Latest 15 November checkpoint: backend/profile work committed as `aba6967`; the next internal Core adapter step delivers 20 cases and 227 total standalone passes. [Adapter qualification map](NATIVE-SESSION-ADAPTER.md) distinguishes executed controls from remaining group acceptance, external consumer/API review and broader coverage. No P01–P12/global sign-off, version/tag/release or historical Core-lane requalification is implied. REQ-009 remains In Progress on the unchanged 0.4 development line.

Native backend delivery on 15 November: [grammar](NATIVE-DECIMAL-GRAMMAR.md) finalized, internal structural/assertion assessor implemented, 76 backend cases and 207 total standalone cases pass. Backend controls cover bounded portions of P01/P02/P05/P08 plus independent snapshot/concurrency observations; engine-root atomization, full adapter identity/environment/lifetime and overall P01–P12 acceptance remain pending. REQ-009 stays In Progress. Schema's 0.4 development line and all published Core pins remain unchanged; no tag or release is implied.

Earlier scalar-only checkpoint on 15 November: the [published-package scalar probe](native-session-probe/2026-11-15/README.md) passed seven cases, with 131 standalone cases then (124 retained plus seven). The [specification adoption record](NATIVE-DECIMAL-SPECIFICATION-ADOPTION.md) pinned six downloaded W3C publication bodies. Subsequent backend delivery above supersedes its implementation status and standalone count; its receipts remain historical. REQ-009 stays In Progress; no release/version change or general provider acceptance.

Historical profile-definition checkpoint: the [first native decimal session profile](NATIVE-SESSION-PROFILE.md) defines supported/refused coverage, a released-contract map and twelve paired planned acceptance groups. [ADR-009](ADR-009-native-decimal-session-profile.md) is Proposed. This documentation checkpoint retains the Schema 0.4 development line; the implementation release version is undecided. REQ-009 remains In Progress. No runtime functionality or provider acceptance is claimed.

The Apache transition passed 114 public schema tests with zero failures/skips. Commercial credential, payments, delivery and key-generation requests remain in the private licensing repository; they are not public schema dependencies or open-source engine features.

The transition does not implement the proposed core session API, change frozen W3C gates, admit skipped tests, certify XmlDb/Braid adoption or release a package. Future semantic additions require immutable specification adoption evidence and happy-path/edge regression tests.

The 3 November [O1/O2 versioned responses](core-disposition-responses/2026-11-03-v2/README.md) are scoped historical design acceptance, not proof of current native adapter/runtime delivery. Their original source/proposal hashes and the unsigned [confirmation drafts](core-disposition-inbox/2026-11-03/README.md) remain unchanged. The published Core 1.81.2 release and its budget deferral do not complete Schema's native provider/session or performance requirements. See [history](DEVELOPMENT_HISTORY.md).

*Last updated: 2026-11-17*


**Current release — 17 November 2026:** [Bosak.Schema 0.5.0-alpha.0.7](https://www.nuget.org/packages/Bosak.Schema/0.5.0-alpha.0.7) is published and independently verified. Product coverage is 272 cases; 45 separate release controls pass. Beta readiness remains proposed; full XSD 1.1/API freeze/performance acceptance is not implied.

Schema beta readiness is now the next priority before Braid/XmlDb adoption. The [beta gap assessment](beta-readiness/ASSESSMENT.md) proposes a bounded decimal-session beta and distinguishes a broader validator target. Scope and public-contract decisions remain pending; no beta version or semantic change is made.


The [public API audit](beta-readiness/API-CONTRACT-AUDIT.md) classifies all39 published types/313 contract rows and records five caught observations. Legacy result semantics, mutable descriptor collections and file-resolution boundaries need beta decisions/hardening; [contract decisions](beta-readiness/CONTRACT-DECISIONS.md) remain Proposed. No runtime behavior or beta acceptance changes.


The local [restricted-resolution APIs](RESTRICTED_RESOLUTION.md) add an explicit bounded file resolver and a controlled schema-only XSLT factory. Canonical containment, reparse-point refusal, full XML/DTD checks and published Core policy cover direct/nested schema reads. Stable host-owned filesystem paths are required; this is not an atomic sandbox. Restricted-resolution checkpoint:306 cases (280 implementation/26 public consumer), plus45 separate release controls. Published alpha remains272 cases; the source candidate is not a new release.


Compiler-produced [descriptor snapshots](DESCRIPTOR_SNAPSHOTS.md) now own immutable outer/nested collections. Caller-created positional records retain their existing ownership behavior. The descriptor-snapshot checkpoint passed319 product cases (290 implementation/29 external) and45 separate release controls; public signatures remain40 types/317 rows. This is local beta-readiness work; the published alpha and legacy validation meanings are unchanged.


[Legacy validation](LEGACY_VALIDATION.md) now refuses models with compile diagnostics before assertion/facet/static-check work, preserving original findings. For clean models, success remains operation-scoped and does not certify structural validity. Local suite345 cases (312 implementation/33 external), plus45 release controls; public40types/317rows unchanged. The published alpha remains unchanged; this source fix is not a beta release.
