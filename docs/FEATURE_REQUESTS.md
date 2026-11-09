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
| Core schema-session integration | Pending | Complete component authority, actual PSVI/atomic flow, compatible-schema interchange and real consumer qualification |
| Complete XSD 1.1 structural validation | Incomplete | No complete current implementation or blanket conformance claim |

## 2. Implemented Coverage

The public tests cover XSD version selection, schema components, explicit resolution/security, assertion evaluation, simple-type assertion facets, alternative compilation/applicability, open-content/default-attribute descriptor handling, open-content wildcard negative constraints (notNamespace/notQName) and the existing typed-XSLT integration. Support is bounded by those implementations and diagnostics; recorded metadata is not equivalent to structural enforcement.

## 3. Acceptance and Exclusions

The Apache transition passed 114 public schema tests with zero failures/skips. Commercial credential, payments, delivery and key-generation requests remain in the private licensing repository; they are not public schema dependencies or open-source engine features.

The transition does not implement the proposed core session API, change frozen W3C gates, admit skipped tests, certify XmlDb/Braid adoption or release a package. Future semantic additions require immutable specification adoption evidence and happy-path/edge regression tests.

The 3 November [O1/O2 versioned responses](core-disposition-responses/2026-11-03-v2/README.md) are scoped historical design acceptance, not proof of current native adapter/runtime delivery. Their original source/proposal hashes and the unsigned [confirmation drafts](core-disposition-inbox/2026-11-03/README.md) remain unchanged. The published Core 1.81.2 release and its budget deferral do not complete Schema's native provider/session or performance requirements. See [history](DEVELOPMENT_HISTORY.md).

*Last updated: 2026-11-09*
