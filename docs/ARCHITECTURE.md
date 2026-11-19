<div align="center">
  <img src="../assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Architecture">
  <h1>Bosak.Schema Architecture</h1>
  <p>Modular Apache schema services with explicit resource and engine boundaries.</p>
</div>

## M1 namespace candidate - 19 November 2026

The local M1 candidate adds namespace-aware expanded declaration names, qualified/unqualified local forms, QName assertion binding and zero-to-eight assertions within the finite decimal shape. All409source cases pass (315implementation/94public), including41authored clause-mapped cases and three real Core-session controls. Package-only94cases pass with Core1.81.2; a separate42case public lane passes with published Core1.81.17. Retained124/23/33binary catalogs pass with onlySchema.dll replaced; these lanes overlap. No Core runtime/API/source-wrapper pin, published package, version/tag or release-controller change. [Evidence](m1-namespaces/README.md).


## Shared specification coverage priority - 19 November 2026

Owner priority is specification coverage and useful external application workflows across Bosak and Bosak.Schema. The shared24-area matrix separates bounded native behavior, legacy/descriptor support, refusal and unassessed work;365historical Schema case IDs are indexed without claiming conformance. M1 proposes namespace-aware decimal documents with optional assertions, followed by coherent named-type/attribute/datatype and structural slices. No runtime or release change. [Programme](specification-coverage/PROGRAM.md), [owner requirement](REQ-009-SPECIFICATION-COVERAGE.md).


## Bounded Unsupported-reason candidate - 19 November 2026

The local development candidate adds `NativeDecimalDiagnostic.UnsupportedReason` and the fixed `NativeDecimalUnsupportedReason` enum. Existing stage codes/messages, outcomes and grammar are preserved; categories describe one detected refusal and may be Unspecified. All365source cases pass (312 implementation/53public, including20new controls), as do53package-only cases and retained124/23 unchanged-binary replacement catalogs. These lanes overlap and are not additive. The54release and six workload-integrity controls pass. MinVer minimum advances to0.6 for this additive feature;0.6.0-alpha.0.2 is local/unsigned only. Published0.5.0-beta.1 and allCore1.81.2 pins remain unchanged. No commit/push, tag or publication. [Candidate](unsupported-reasons/README.md).


## Beta.2 gap assessment - 19 November 2026

A public-example beta.2 gap assessment finds no confirmed new beta.1 defect. Namespace support alone would not admit the existing sku/orders schemas. The proposed first bounded slice is clearer Unsupported reason categories, preserving stage codes, input-free diagnostics and current grammar; API/design acceptance and a concrete application priority remain pending. Beta.1 stays current; no implementation or release change. [Assessment](beta2-gap-assessment/README.md).


# Bosak.Schema Architecture

> Modular Apache schema services with explicit resource and engine boundaries.


## 1. Current Architecture

**Current release — 18 November 2026:** [Bosak.Schema 0.5.0-beta.1](https://www.nuget.org/packages/Bosak.Schema/0.5.0-beta.1) is published and independently verified. [GitHub prerelease](https://github.com/Fytala-Charles/Bosak.Schema/releases/tag/v0.5.0-beta.1), [scope and publication receipt](BETA1_RELEASE.md).

One public `Bosak.Schema` assembly references the published Bosak XPath API/providers, XSLT and XQuery packages at 1.81.2 after the merged dependency adoption. The core engine does not reference this package. Keeping schema services in a separate assembly avoids a dependency cycle; changing the license does not require merging them into Core.

## 2. Components

| Component | Responsibility and limit |
|---|---|
| `Compilation` | Explicit-version schema descriptor compilation and bounded resolved imports/includes |
| `Model` | Staged XSD 1.1 assertions, alternatives, declarations, simple facets, open-content/default-attribute metadata, wildcard negative constraints (notNamespace/notQName) |
| `Validation` | Assertion/facet evaluation and supported governing-type applicability through Bosak XPath |
| `Assessment` | Internal finite native backend; authority/advisor/input adapters; canonical guarded nodes, original facts and scalar provenance; host-owned leases and one shared sequential runner per session; public owned native decimal session/document facade |
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
| [ADR-009: first native decimal session profile](ADR-009-native-decimal-session-profile.md) | Accepted for the bounded Schema beta profile; broader Core/provider acceptance excluded |
| [ADR-010: owned synchronous native decimal facade](ADR-010-owned-native-decimal-facade.md) | Accepted for the bounded owned Schema facade; published and externally consumer-qualified |
| Require no activation for standards processing | Implemented |
| Full XSD 1.1 provider/session integration | Pending; no conformance promotion |

## 5. Validation and Extensibility

The published **0.5.0-beta.1** provides owned `NativeDecimalSession` and `NativeDecimalDocument` APIs with explicit capacities/context, exact decimal reads and detached XML export. ADR009/010 are accepted for this bounded Schema application scope. Qualification passed 345 source cases (312 implementation/33 public),33 package-only cases and retained 124/23 binary lanes using Core 1.81.2. These catalogs overlap. [Release, exclusions and receipt](BETA1_RELEASE.md). Full XSD 1.1, general Core C45/ADR006 and production SLA acceptance remain separate.

The [native decimal session profile](NATIVE-SESSION-PROFILE.md) maps N1–N8 onto inspected Core 1.81.2 contracts and defines twelve paired planned acceptance groups. It requires a native structural backend, authority-owned facts and host-retained operation lifetimes. Cross-universe handoff remains Unsupported in this first scope. No adapter or new public API is delivered by the definition.

Historical prototype qualification: all 243 standalone cases passed: 124 retained, seven scalar probes, 76 backend, 28 adapter and eight [owned-facade cases](native-session-facade/2026-11-15/README.md). The prototype owns the sole Core session/runner and each document admission, exposing only detached exact decimals and XML through its internal workflow. Borrowed container and scalar receipt expiry, quota/cancellation/fault cleanup and default-name argument rejection are qualified. The finite grammar is unchanged. The published beta supersedes the public-symbol/external-consumer work described here; ADR009/010 acceptance is bounded to the Schema application facade, with broader Core acceptance excluded. Historical receipts retain their scopes.

The dependency adoption changes no Schema C# implementation, upstream baseline/filter or storage format. The same 124 cases pass after rebuilding and in the separately retained unchanged-binary lane; these repeated case catalogs are not additive coverage. The Core optional source wrapper pins merged Schema c953ebe and runs 124 upstream plus 11 local cases. This does not prove arbitrary downstream ABI, native registry/lease safety, concurrent disposal or full PSVI/component completeness.

Earlier source/design inputs and unsigned drafts remain in their [historical inbox](core-disposition-inbox/2026-11-03/README.md); their original identities and decisions are unchanged. Current engineering state takes precedence over the archived local notices in [development history](DEVELOPMENT_HISTORY.md).

*Last updated: 2026-11-18*
