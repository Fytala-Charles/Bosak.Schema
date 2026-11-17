<div align="center">
  <img src="../assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Feature Requests">
  <h1>Bosak.Schema Feature Requests</h1>
  <p>Current public schema scope, status and acceptance boundaries.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](PUBLICATION.json). No NuGet release or commercial infrastructure publication.

# Bosak.Schema Feature Requests

> Current public schema scope, status and acceptance boundaries.


## 1. Registry

A [trusted-publishing route](TRUSTED_PUBLISHING.md) is prepared locally following the owner's selection. The new release workflow is manual and preparation-only by default; its publisher verifies a successful same-repository/main/first-attempt preparation, the exact reviewed manifest/package and registry absence before OIDC login and again before one push. Product source/version/Core dependencies are unchanged. No workflow dispatch, policy/account update, login or publication has occurred. A hosted preparation will create its own artifact/digest for review; the earlier local package remains preserved.

The public owned decimal API is merged in [Schema PR #3](https://github.com/Fytala-Charles/Bosak.Schema/pull/3) at9fd4e6a; its272-case standalone main check passed. Core's optional source consumer is refreshed in [Core PR #503](https://github.com/Fytala-Charles/Bosak/pull/503), mergef7b6c995, with269 current-Core cases and all six main checks passing. Standalone/package dependencies remain Core1.81.2; current-Core1.81.17 source-consumer evidence is a separate lane. The [0.5 package-delivery review](package-0.5-review/README.md) prepares a local prerelease and a package README correction. No Schema package, tag or registry action has been published; global profile/ADR/API-freeze/performance acceptance remains open.

The 17 November public-entry candidate implements `NativeDecimalSession`, `NativeDecimalDocument`, immutable options/context and owned results with fixed bounded stage diagnostics. It keeps Core 1.81.2 and the finite backend grammar unchanged. The 0.5 development line has 272 passing cases: 249 implementation cases (243 retained plus six lifecycle/cleanup controls) and 23 separate public-consumer cases without friend access. The consumer also passes through the locally packed Schema binary and published Core dependencies; this repeats its case catalog and is not additive coverage. [Contract and qualification](NATIVE-DECIMAL-PUBLIC-API.md). Hosted review, overall profile acceptance, API freeze and package publication remain separate.

| Request | Status | Scope |
|---|---|---|
| Apache/public transition | Implemented and GitHub publication verified | Schema-only Apache-2.0; activation-free services; private infrastructure excluded |
| [REQ-009](ADR-005-xsd11-compiled-model-and-validation.md) staged XSD 1.1 support | In Progress | Compiled descriptors, assertion/facet evaluation, alternative applicability/filtering and selected metadata |
| [Core 1.81.2 dependency adoption](CORE-1.81.2-ADOPTION.md) | Merged | Four product references and one test provider reference updated; no C# adaptation |
| Core optional source integration | Refreshed in Core PR #503 | Merged Schema9fd4e6a pinned with58 source/test/fixture/legal inputs;260 implementation/legacy plus nine public current-Core cases. Original135-case receipt remains historical. |
| Core schema-session integration | Internal native scalar adapter delivered; overall acceptance pending | Bounded original facts/scalar flow and host lifetime controls; broader component/compatibility/public consumer qualification pending |
| NuGet trusted publishing | Local workflow/controller candidate; not configured or dispatched | Manual prepare/publish separation;41 refusal controls and real artifact rehearsal; profile/user policy and publication approval remain separate |
| Complete XSD 1.1 structural validation | Incomplete | No complete current implementation or blanket conformance claim |
| Native decimal structural backend | Implemented internally; 76 cases pass | Complete finite grammar only; [evidence](native-decimal-backend/2026-11-15/README.md) |
| Native decimal Core adapter | Implemented internally; 28 cases pass | Issuance, scalar flow, guarded leases, expiry and cleanup; [latest evidence](native-session-facade/2026-11-15/README.md) |
| Owned native decimal entry point | Merged in PR #3;23 external consumer cases and six added lifecycle controls pass; package-delivery review local | [ADR-010](ADR-010-owned-native-decimal-facade.md), [proposal](NATIVE-DECIMAL-PUBLIC-API-PROPOSAL.md); candidate development minimum 0.5; no tag or package release |

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


Hosted preparation first attempt passed product/package/legacy qualification but refused unexpected package content before sealing. [Diagnostic follow-up](preparation-diagnostics/README.md) preserves failure evidence and keeps the package allowlist unchanged; local candidate awaits delivery. No package is admitted or published.
