<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Roadmap">
  <h1>Bosak.Schema Roadmap</h1>
  <p>An open schema foundation with staged validation and integration.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](docs/PUBLICATION.json). No NuGet release or commercial infrastructure publication.

# Bosak.Schema Roadmap

> An open schema foundation with staged validation and integration.


## 1. Current Milestone

Schema services remain Apache-2.0 and activation-free; the public decimal-entry candidate advances the development minimum to 0.5. Public source contains the schema implementation/tests only; the original commercial infrastructure and full Git history remain private. Core 1.81.2 dependency adoption is now merged in Schema PR #1. The Core optional source integration is refreshed to the merged Schema revision in Core PR #472. No new Schema NuGet release/tag is announced.

## 2. Next Milestones

1. Define the next bounded native provider/session adoption against the released Core surface and recorded source/design requirements; preserve explicit supported/refused profiles.
2. Qualify complete component/universe identity, compatible-schema handoff, PSVI/atomic flow and bounded resource ownership.
3. Implement or explicitly refuse remaining XSD 1.1 structural/assessment coverage.
4. Build/test real consumers and unchanged binaries, then run the full engine/conformance gates before API freeze or release.

The 17 November public-entry candidate implements `NativeDecimalSession`, `NativeDecimalDocument`, immutable options/context and owned results with fixed bounded stage diagnostics. It keeps Core 1.81.2 and the finite backend grammar unchanged. The 0.5 development line has 272 passing cases: 249 implementation cases (243 retained plus six lifecycle/cleanup controls) and 23 separate public-consumer cases without friend access. The consumer also passes through the locally packed Schema binary and published Core dependencies; this repeats its case catalog and is not additive coverage. [Contract and qualification](docs/NATIVE-DECIMAL-PUBLIC-API.md). Hosted review, overall profile acceptance, API freeze and package publication remain separate.

## 3. Validation and Limits

The standalone solution passes 272 cases with Core pins at 1.81.2: 249 implementation cases and 23 external public-consumer cases. The Release build with NuGet auditing and official documentation checks pass. The historical Core optional source integration has 135 cases including the original 124 upstream cases; it is not requalified by this delivery. Full XSD 1.1, global profile acceptance and performance budgets remain separate. Billing/key-generation, Folio, XmlDb and Braid workflows remain outside this change.

The [dependency adoption](docs/CORE-1.81.2-ADOPTION.md) and historical source/binary controls establish only their recorded compatibility scope. Broader component-universe interchange, native operation leases, concurrent disposal and complete structural assessment remain work. Select representative workloads and explicit budgets before making Schema performance claims; Core's scoped release deferral is not performance acceptance. Keep Schema package publication and consumer adoption decisions separate from the already published Core release.

*Last updated: 2026-11-17*
