<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Roadmap">
  <h1>Bosak.Schema Roadmap</h1>
  <p>An open schema foundation with staged validation and integration.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](docs/PUBLICATION.json). Historical GitHub transition receipt; the current NuGet release is recorded below.

# Bosak.Schema Roadmap

> An open schema foundation with staged validation and integration.


## 1. Current Milestone

Schema services remain Apache-2.0 and activation-free; the public decimal-entry candidate advances the development minimum to 0.5. Public source contains the schema implementation/tests only; the original commercial infrastructure and full Git history remain private. Core 1.81.2 dependency adoption is now merged in Schema PR #1. The Core optional source integration is refreshed to the merged Schema revision in Core PR #472. No new Schema NuGet release/tag is announced.

## 2. Next Milestones

1. Define the next bounded native provider/session adoption against the released Core surface and recorded source/design requirements; preserve explicit supported/refused profiles.
2. Qualify complete component/universe identity, compatible-schema handoff, PSVI/atomic flow and bounded resource ownership.
3. Implement or explicitly refuse remaining XSD 1.1 structural/assessment coverage.
4. Build/test real consumers and unchanged binaries, then run the full engine/conformance gates before API freeze or release.

The 17 November public-entry candidate implements `NativeDecimalSession`, `NativeDecimalDocument`, immutable options/context and owned results with fixed bounded stage diagnostics. It keeps Core 1.81.2 and the finite backend grammar unchanged. The 0.5 development line has 272 passing cases: 249 implementation cases (243 retained plus six lifecycle/cleanup controls) and 23 separate public-consumer cases without friend access. The consumer also passes through the locally packed Schema binary and published Core dependencies; this repeats its case catalog and is not additive coverage. [Contract and qualification](docs/NATIVE-DECIMAL-PUBLIC-API.md). The alpha is published; overall profile acceptance and API freeze remain pending.

## 3. Validation and Limits

The standalone solution passes 272 cases with Core pins at 1.81.2: 249 implementation cases and 23 external public-consumer cases. The Release build with NuGet auditing and official documentation checks pass. The historical Core optional source integration has 135 cases including the original 124 upstream cases; it is not requalified by this delivery. Full XSD 1.1, global profile acceptance and performance budgets remain separate. Billing/key-generation, Folio, XmlDb and Braid workflows remain outside this change.

The [dependency adoption](docs/CORE-1.81.2-ADOPTION.md) and historical source/binary controls establish only their recorded compatibility scope. Broader component-universe interchange, native operation leases, concurrent disposal and complete structural assessment remain work. Select representative workloads and explicit budgets before making Schema performance claims; Core's scoped release deferral is not performance acceptance. Keep Schema package publication and consumer adoption decisions separate from the already published Core release.

*Last updated: 2026-11-17*


**Current release — 17 November 2026:** [Bosak.Schema 0.5.0-alpha.0.7](https://www.nuget.org/packages/Bosak.Schema/0.5.0-alpha.0.7) is published and independently verified. Product coverage is 272 cases; 45 separate release controls pass. Beta readiness remains proposed; full XSD 1.1/API freeze/performance acceptance is not implied.

Schema beta readiness is now the next priority before Braid/XmlDb adoption. The [beta gap assessment](docs/beta-readiness/ASSESSMENT.md) proposes a bounded decimal-session beta and distinguishes a broader validator target. Scope and public-contract decisions remain pending; no beta version or semantic change is made.


The [public API audit](docs/beta-readiness/API-CONTRACT-AUDIT.md) classifies all39 published types/313 contract rows and records five caught observations. Legacy result semantics, mutable descriptor collections and file-resolution boundaries need beta decisions/hardening; [contract decisions](docs/beta-readiness/CONTRACT-DECISIONS.md) remain Proposed. No runtime behavior or beta acceptance changes.


The local [restricted-resolution APIs](docs/RESTRICTED_RESOLUTION.md) add an explicit bounded file resolver and a controlled schema-only XSLT factory. Canonical containment, reparse-point refusal, full XML/DTD checks and published Core policy cover direct/nested schema reads. Stable host-owned filesystem paths are required; this is not an atomic sandbox. Current local suite:306 cases (280 implementation/26 public consumer), plus45 separate release controls. Published alpha remains272 cases; the source candidate is not a new release.
