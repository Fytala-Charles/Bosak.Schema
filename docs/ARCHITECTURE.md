<div align="center">
  <img src="../assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Architecture">
  <h1>Bosak.Schema Architecture</h1>
  <p>Modular Apache schema services with explicit resource and engine boundaries.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](PUBLICATION.json). Historical GitHub transition receipt; the current NuGet release is recorded below.

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

The 17 November public-entry candidate implements `NativeDecimalSession`, `NativeDecimalDocument`, immutable options/context and owned results with fixed bounded stage diagnostics. It keeps Core 1.81.2 and the finite backend grammar unchanged. The 0.5 development line has 272 passing cases: 249 implementation cases (243 retained plus six lifecycle/cleanup controls) and 23 separate public-consumer cases without friend access. The consumer also passes through the locally packed Schema binary and published Core dependencies; this repeats its case catalog and is not additive coverage. [Contract and qualification](NATIVE-DECIMAL-PUBLIC-API.md). The alpha is published; overall profile acceptance and API freeze remain pending.

The [native decimal session profile](NATIVE-SESSION-PROFILE.md) maps N1–N8 onto inspected Core 1.81.2 contracts and defines twelve paired planned acceptance groups. It requires a native structural backend, authority-owned facts and host-retained operation lifetimes. Cross-universe handoff remains Unsupported in this first scope. No adapter or new public API is delivered by the definition.

Historical prototype qualification: all 243 standalone cases passed: 124 retained, seven scalar probes, 76 backend, 28 adapter and eight [owned-facade cases](native-session-facade/2026-11-15/README.md). The prototype owns the sole Core session/runner and each document admission, exposing only detached exact decimals and XML through its internal workflow. Borrowed container and scalar receipt expiry, quota/cancellation/fault cleanup and default-name argument rejection are qualified. The finite grammar is unchanged. The 17 November candidate supersedes the public-symbol/external-consumer work described here; global acceptance remains pending under ADR-010. Historical receipts retain their scopes.

The dependency adoption changes no Schema C# implementation, upstream baseline/filter or storage format. The same 124 cases pass after rebuilding and in the separately retained unchanged-binary lane; these repeated case catalogs are not additive coverage. The Core optional source wrapper pins merged Schema c953ebe and runs 124 upstream plus 11 local cases. This does not prove arbitrary downstream ABI, native registry/lease safety, concurrent disposal or full PSVI/component completeness.

Earlier source/design inputs and unsigned drafts remain in their [historical inbox](core-disposition-inbox/2026-11-03/README.md); their original identities and decisions are unchanged. Current engineering state takes precedence over the archived local notices in [development history](DEVELOPMENT_HISTORY.md).

*Last updated: 2026-11-17*


**Current release — 17 November 2026:** [Bosak.Schema 0.5.0-alpha.0.7](https://www.nuget.org/packages/Bosak.Schema/0.5.0-alpha.0.7) is published and independently verified. Product coverage is 272 cases; 45 separate release controls pass. Beta readiness remains proposed; full XSD 1.1/API freeze/performance acceptance is not implied.

Schema beta readiness is now the next priority before Braid/XmlDb adoption. The [beta gap assessment](beta-readiness/ASSESSMENT.md) proposes a bounded decimal-session beta and distinguishes a broader validator target. Scope and public-contract decisions remain pending; no beta version or semantic change is made.


The [public API audit](beta-readiness/API-CONTRACT-AUDIT.md) classifies all39 published types/313 contract rows and records five caught observations. Legacy result semantics, mutable descriptor collections and file-resolution boundaries need beta decisions/hardening; [contract decisions](beta-readiness/CONTRACT-DECISIONS.md) remain Proposed. No runtime behavior or beta acceptance changes.


The local [restricted-resolution APIs](RESTRICTED_RESOLUTION.md) add an explicit bounded file resolver and a controlled schema-only XSLT factory. Canonical containment, reparse-point refusal, full XML/DTD checks and published Core policy cover direct/nested schema reads. Stable host-owned filesystem paths are required; this is not an atomic sandbox. Restricted-resolution checkpoint:306 cases (280 implementation/26 public consumer), plus45 separate release controls. Published alpha remains272 cases; the source candidate is not a new release.


Compiler-produced [descriptor snapshots](DESCRIPTOR_SNAPSHOTS.md) now own immutable outer/nested collections. Caller-created positional records retain their existing ownership behavior. The descriptor-snapshot checkpoint passed319 product cases (290 implementation/29 external) and45 separate release controls; public signatures remain40 types/317 rows. This is local beta-readiness work; the published alpha and legacy validation meanings are unchanged.


[Legacy validation](LEGACY_VALIDATION.md) now refuses models with compile diagnostics before assertion/facet/static-check work, preserving original findings. For clean models, success remains operation-scoped and does not certify structural validity. Local suite345 cases (312 implementation/33 external), plus45 release controls; public40types/317rows unchanged. The published alpha remains unchanged; this source fix is not a beta release.


The owner selected a bounded decimal-session beta with explicit exclusions. The [beta qualification](BETA_QUALIFICATION.md) records five measured standalone workflows and a reproducible watchdog-controlled diagnostic. Product345 and release45 checks remain green; six separate runner controls qualify measurement integrity. Timing/memory results are local observations, not a production SLA. API/ADR acceptance and exact beta packaging/publication remain pending.


`tools/Bosak.Schema.Workloads` is a non-packable console consumer of the public Schema API, built separately from the product solution. The Python parent pins its binaries/configurations, enforces a per-process watchdog and preserves raw observations. CI compiles it and runs mocked refusal/measurement-integrity controls; timing budgets are not CI gates.


The [bounded beta contract review](BETA_CONTRACT_REVIEW.md) records the owner-accepted bounded public API/ADR decisions and compatibility evidence. ADR009/010 were accepted for this bounded scope on17 November2026. A clean-source alpha compatibility package passes package-only and retained-binary checks; no beta tag/artifact or publication is implied.
