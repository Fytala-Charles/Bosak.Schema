<div align="center">
  <img src="../assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Architecture">
  <h1>Bosak.Schema Architecture</h1>
  <p>Modular Apache schema services with explicit resource and engine boundaries.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](PUBLICATION.json). No NuGet release or commercial infrastructure publication.

# Bosak.Schema Architecture

> Modular Apache schema services with explicit resource and engine boundaries.


## 1. Current Architecture

One public `Bosak.Schema` assembly references the published Bosak XPath API/providers, XSLT and XQuery packages at 1.81.2 after the merged dependency adoption. The core engine does not reference this package. Keeping schema services in a separate assembly avoids a dependency cycle; changing the license does not require merging them into Core.

## 2. Components

| Component | Responsibility and limit |
|---|---|
| `Compilation` | Explicit-version schema descriptor compilation and bounded resolved imports/includes |
| `Model` | Staged XSD 1.1 assertions, alternatives, declarations, simple facets, open-content/default-attribute metadata, wildcard negative constraints (notNamespace/notQName) |
| `Validation` | Assertion/facet evaluation and supported governing-type applicability through Bosak XPath |
| `Assessment` | Internal finite native backend; authority/advisor/input adapters; canonical guarded nodes, original facts and scalar provenance; host-owned leases and one shared sequential runner per session; public native factory pending |
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
| [ADR-005: XSD 1.1 compiled model and validation pipeline](ADR-005-xsd11-compiled-model-and-validation.md) | Accepted by owner on 27 October 2026; transferred to the public repository 30 October 2026 as REQ-009 decision evidence |
| [ADR-006: XSD 1.1 S4/S5 engine seam compatibility](ADR-006-xsd11-engine-seam-compatibility.md) | Proposed; pending core-repo acceptance before the S4 design freeze (core tracking REQ-139) |
| [Core 1.81.2 dependency adoption](CORE-1.81.2-ADOPTION.md) | Merged in Schema PR #1; existing source and unchanged-binary coverage qualified |
| [Historical BCAP O1/O2 design review](core-disposition-responses/2026-11-03-v2/README.md) | Scoped design returns dated 3 November; not new native/runtime acceptance |
| Preserve modular package and core dependency direction | Retained |
| [ADR-009: first native decimal session profile](ADR-009-native-decimal-session-profile.md) | Proposed; internal backend/adapter implemented, overall acceptance pending |
| [ADR-010: owned synchronous native decimal facade](ADR-010-owned-native-decimal-facade.md) | Proposed; internal prototype and public contract proposal delivered; external/public acceptance pending |
| Require no activation for standards processing | Implemented |
| Full XSD 1.1 provider/session integration | Pending; no conformance promotion |

## 5. Validation and Extensibility

A [trusted-publishing route](TRUSTED_PUBLISHING.md) is prepared locally following the owner's selection. The new release workflow is manual and preparation-only by default; its publisher verifies a successful same-repository/main/first-attempt preparation, the exact reviewed manifest/package and registry absence before OIDC login and again before one push. Product source/version/Core dependencies are unchanged. No workflow dispatch, policy/account update, login or publication has occurred. A hosted preparation will create its own artifact/digest for review; the earlier local package remains preserved.

The public owned decimal API is merged in [Schema PR #3](https://github.com/Fytala-Charles/Bosak.Schema/pull/3) at9fd4e6a; its272-case standalone main check passed. Core's optional source consumer is refreshed in [Core PR #503](https://github.com/Fytala-Charles/Bosak/pull/503), mergef7b6c995, with269 current-Core cases and all six main checks passing. Standalone/package dependencies remain Core1.81.2; current-Core1.81.17 source-consumer evidence is a separate lane. The [0.5 package-delivery review](package-0.5-review/README.md) prepares a local prerelease and a package README correction. No Schema package, tag or registry action has been published; global profile/ADR/API-freeze/performance acceptance remains open.

The 17 November public-entry candidate implements `NativeDecimalSession`, `NativeDecimalDocument`, immutable options/context and owned results with fixed bounded stage diagnostics. It keeps Core 1.81.2 and the finite backend grammar unchanged. The 0.5 development line has 272 passing cases: 249 implementation cases (243 retained plus six lifecycle/cleanup controls) and 23 separate public-consumer cases without friend access. The consumer also passes through the locally packed Schema binary and published Core dependencies; this repeats its case catalog and is not additive coverage. [Contract and qualification](NATIVE-DECIMAL-PUBLIC-API.md). Hosted review, overall profile acceptance, API freeze and package publication remain separate.

The [native decimal session profile](NATIVE-SESSION-PROFILE.md) maps N1–N8 onto inspected Core 1.81.2 contracts and defines twelve paired planned acceptance groups. It requires a native structural backend, authority-owned facts and host-retained operation lifetimes. Cross-universe handoff remains Unsupported in this first scope. No adapter or new public API is delivered by the definition.

Historical prototype qualification: all 243 standalone cases passed: 124 retained, seven scalar probes, 76 backend, 28 adapter and eight [owned-facade cases](native-session-facade/2026-11-15/README.md). The prototype owns the sole Core session/runner and each document admission, exposing only detached exact decimals and XML through its internal workflow. Borrowed container and scalar receipt expiry, quota/cancellation/fault cleanup and default-name argument rejection are qualified. The finite grammar is unchanged. The 17 November candidate supersedes the public-symbol/external-consumer work described here; global acceptance remains pending under ADR-010. Historical receipts retain their scopes.

The dependency adoption changes no Schema C# implementation, upstream baseline/filter or storage format. The same 124 cases pass after rebuilding and in the separately retained unchanged-binary lane; these repeated case catalogs are not additive coverage. The Core optional source wrapper pins merged Schema c953ebe and runs 124 upstream plus 11 local cases. This does not prove arbitrary downstream ABI, native registry/lease safety, concurrent disposal or full PSVI/component completeness.

Earlier source/design inputs and unsigned drafts remain in their [historical inbox](core-disposition-inbox/2026-11-03/README.md); their original identities and decisions are unchanged. Current engineering state takes precedence over the archived local notices in [development history](DEVELOPMENT_HISTORY.md).

*Last updated: 2026-11-17*


Hosted preparation first attempt passed product/package/legacy qualification but refused unexpected package content before sealing. [Diagnostic follow-up](preparation-diagnostics/README.md) preserves failure evidence and keeps the package allowlist unchanged; local candidate awaits delivery. No package is admitted or published.


The retained hosted package identified a verifier mismatch for the exact NuGet-generated `nuget.psmdcp` metadata path. The [local correction](package-metadata-correction/README.md) accepts that path while preserving all other admission checks; the failed run remains ineligible and publication is pending. Product sources and dependencies are unchanged.
