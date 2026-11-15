<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Roadmap">
  <h1>Bosak.Schema Roadmap</h1>
  <p>An open schema foundation with staged validation and integration.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](docs/PUBLICATION.json). No NuGet release or commercial infrastructure publication.

# Bosak.Schema Roadmap

> An open schema foundation with staged validation and integration.


## 1. Current Milestone

The 0.4 development line makes schema services Apache-2.0 and activation-free. Public source contains the schema implementation/tests only; the original commercial infrastructure and full Git history remain private. Core 1.81.2 dependency adoption is now merged in Schema PR #1. The Core optional source integration is refreshed to the merged Schema revision in Core PR #472. No new Schema NuGet release/tag is announced.

## 2. Next Milestones

1. Define the next bounded native provider/session adoption against the released Core surface and recorded source/design requirements; preserve explicit supported/refused profiles.
2. Qualify complete component/universe identity, compatible-schema handoff, PSVI/atomic flow and bounded resource ownership.
3. Implement or explicitly refuse remaining XSD 1.1 structural/assessment coverage.
4. Build/test real consumers and unchanged binaries, then run the full engine/conformance gates before API freeze or release.

The first bounded scope is [schema-native-decimal-session-v1](docs/NATIVE-SESSION-PROFILE.md). Its scalar transport probe, hashed specification basis, [executable grammar](docs/NATIVE-DECIMAL-GRAMMAR.md) and [internal native structural backend](docs/native-decimal-backend/2026-11-15/README.md) are delivered. Next: implement and qualify authority/advisor/input/lifetime adapters and a real scalar consumer. Overall native provider qualification remains pending.

## 3. Validation and Limits

The standalone public Schema suite passes 207 cases with Core package pins at 1.81.2: 124 retained regressions, seven test-fixture scalar transport probes and 76 internal native backend cases. The separate historical Core optional source integration has 135 cases; its count includes the original 124 upstream cases and is not requalified by this backend delivery. Neither current-draft XDM certification nor full XSD 1.1 support follows from this work. Billing/key-generation, other product licensing, Folio, XmlDb storage and Braid workflows remain outside this source change.

The [dependency adoption](docs/CORE-1.81.2-ADOPTION.md) and historical source/binary controls establish only their recorded compatibility scope. Broader component-universe interchange, native operation leases, concurrent disposal and complete structural assessment remain work. Select representative workloads and explicit budgets before making Schema performance claims; Core's scoped release deferral is not performance acceptance. Keep Schema package publication and consumer adoption decisions separate from the already published Core release.

*Last updated: 2026-11-15*
