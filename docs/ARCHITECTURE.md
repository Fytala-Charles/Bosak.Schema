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
| `Assessment` | Internal complete finite decimal grammar and native structural/assertion assessment; immutable present-only states and exact decimal values; no Core adapter/public native factory yet |
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
| [ADR-009: first native decimal session profile](ADR-009-native-decimal-session-profile.md) | Proposed; scope defined, implementation/qualification pending |
| Require no activation for standards processing | Implemented |
| Full XSD 1.1 provider/session integration | Pending; no conformance promotion |

## 5. Validation and Extensibility

The [native decimal session profile](NATIVE-SESSION-PROFILE.md) maps N1–N8 onto inspected Core 1.81.2 contracts and defines twelve paired planned acceptance groups. It requires a native structural backend, authority-owned facts and host-retained operation lifetimes. Cross-universe handoff remains Unsupported in this first scope. No adapter or new public API is delivered by the definition.

All 207 standalone tests pass: 124 retained regressions, seven published-package scalar transport probes and 76 [internal native decimal backend cases](native-decimal-backend/2026-11-15/README.md). The [finite grammar](NATIVE-DECIMAL-GRAMMAR.md) binds strict UTF-8 capture limits, schema/instance coverage, exact arithmetic comparison and immutable original state to the recorded specification basis. The backend remains internal; Core authority/advisor/input/lifetime integration and native provider acceptance remain pending. Historical source/binary and separate Core integration receipts retain their original scopes.

The dependency adoption changes no Schema C# implementation, upstream baseline/filter or storage format. The same 124 cases pass after rebuilding and in the separately retained unchanged-binary lane; these repeated case catalogs are not additive coverage. The Core optional source wrapper pins merged Schema c953ebe and runs 124 upstream plus 11 local cases. This does not prove arbitrary downstream ABI, native registry/lease safety, concurrent disposal or full PSVI/component completeness.

Earlier source/design inputs and unsigned drafts remain in their [historical inbox](core-disposition-inbox/2026-11-03/README.md); their original identities and decisions are unchanged. Current engineering state takes precedence over the archived local notices in [development history](DEVELOPMENT_HISTORY.md).

*Last updated: 2026-11-15*
