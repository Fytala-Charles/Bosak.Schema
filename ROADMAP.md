<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Roadmap">
  <h1>Bosak.Schema Roadmap</h1>
  <p>An open schema foundation with staged validation and integration.</p>
</div>

# Bosak.Schema Roadmap

> An open schema foundation with staged validation and integration.


## 1. Current Milestone

**Bosak.Schema 0.5.0-beta.1 is published and verified**, with an accepted bounded decimal-session contract. [NuGet](https://www.nuget.org/packages/Bosak.Schema/0.5.0-beta.1), [GitHub prerelease](https://github.com/Fytala-Charles/Bosak.Schema/releases/tag/v0.5.0-beta.1), [release scope and receipt](docs/BETA1_RELEASE.md). Services remain Apache-2.0 and activation-free. Public Core dependency minima remain 1.81.2.

## 2. Next Milestones

1. Keep public documentation aligned with the verified release and explicit exclusions.
2. Exercise an identified package-only consumer workflow; distinguish representative fixtures from real application acceptance.
3. Record unsupported constructs and usability/lifetime findings, then scope beta.2 from that evidence.
4. Review Braid/XmlDb adoption separately after the bounded Schema consumer workflow is understood.

Broader XSD 1.1/component-universe/PSVI and general Core C45/ADR006 work requires separate design, immutable specification adoption and acceptance. No automatic scope expansion or new release is authorized by this roadmap.

## 3. Validation and Limits

345 source cases (312 implementation/33 public),33 package-only cases, retained 124 legacy/23 published-alpha cases per control/replacement lane and nine signed Core dependency payloads passed. A fresh registry-only33-case consumer passed after publication. Catalogs overlap; these are not additive totals.54 release controls and six workload integrity controls are separate from product coverage.

The [native API](docs/NATIVE-DECIMAL-PUBLIC-API.md), [restricted resolution](docs/RESTRICTED_RESOLUTION.md), [descriptor ownership](docs/DESCRIPTOR_SNAPSHOTS.md) and [legacy validation](docs/LEGACY_VALIDATION.md) define the supported limits. [Workload measurements](docs/BETA_QUALIFICATION.md) are bounded local observations, not a production SLA. Full XSD 1.1, general provider interchange and a perpetual API freeze remain unaccepted. Historical source integration receipts retain their original revisions; publication does not requalify them.

*Last updated: 2026-11-18*
