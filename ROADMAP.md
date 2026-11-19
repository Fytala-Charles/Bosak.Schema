<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Roadmap">
  <h1>Bosak.Schema Roadmap</h1>
  <p>An open schema foundation with staged validation and integration.</p>
</div>

## Specification coverage priority - 19 November 2026

Prioritize the [shared specification programme](docs/specification-coverage/PROGRAM.md): an accountable coverage ledger, then namespace-aware decimal assessment and optional assertions, followed by coherent named types, attributes, datatypes and structural/application profiles. Release packaging is secondary to demonstrated coverage.


# Bosak.Schema Roadmap

> An open schema foundation with staged validation and integration.


## 1. Current Milestone

**Bosak.Schema 0.5.0-beta.1 is published and verified**, with an accepted bounded decimal-session contract. [NuGet](https://www.nuget.org/packages/Bosak.Schema/0.5.0-beta.1), [GitHub prerelease](https://github.com/Fytala-Charles/Bosak.Schema/releases/tag/v0.5.0-beta.1), [release scope and receipt](docs/BETA1_RELEASE.md). Services remain Apache-2.0 and activation-free. Public Core dependency minima remain 1.81.2.

See the [beta.2 gap assessment](docs/beta2-gap-assessment/README.md): a bounded Unsupported-reason improvement is proposed; no beta.2 grammar expansion or release is selected.

## 2. Next Milestones

1. Keep public documentation aligned with the verified release and explicit exclusions.
2. Exercise an identified package-only consumer workflow; distinguish representative fixtures from real application acceptance.
3. Record unsupported constructs and usability/lifetime findings, then scope beta.2 from that evidence.
4. Review Braid/XmlDb adoption separately after the bounded Schema consumer workflow is understood.

Broader XSD 1.1/component-universe/PSVI and general Core C45/ADR006 work requires separate design, immutable specification adoption and acceptance. No automatic scope expansion or new release is authorized by this roadmap.

## 3. Validation and Limits

345 source cases (312 implementation/33 public),33 package-only cases, retained 124 legacy/23 published-alpha cases per control/replacement lane and nine signed Core dependency payloads passed. A fresh registry-only33-case consumer passed after publication. Catalogs overlap; these are not additive totals.54 release controls and six workload integrity controls are separate from product coverage.

The [native API](docs/NATIVE-DECIMAL-PUBLIC-API.md), [restricted resolution](docs/RESTRICTED_RESOLUTION.md), [descriptor ownership](docs/DESCRIPTOR_SNAPSHOTS.md) and [legacy validation](docs/LEGACY_VALIDATION.md) define the supported limits. [Workload measurements](docs/BETA_QUALIFICATION.md) are bounded local observations, not a production SLA. Full XSD 1.1, general provider interchange and a perpetual API freeze remain unaccepted. Historical source integration receipts retain their original revisions; publication does not requalify them.

*Last updated: 2026-11-19*

## Diagnostic development candidate

[Bounded Unsupported reasons](docs/unsupported-reasons/README.md) are implemented and qualified locally. Review this additive API before any release/controller decision; broader namespaces, attributes and datatypes remain separate scopes. The0.6development minimum is not a beta.2 publication decision.

## M1 local progress

The [bounded namespace/optional-assertion candidate](docs/specification-coverage/M1-CANDIDATE-DELTA.json) is implemented and locally qualified. Wider component/attribute/datatype and structural milestones remain open; official W3C suite selection/admission remains separate from authored controls.

## README reconciliation after M1

The [packaged guidance update](docs/package-readme06-reconciled/README.md) follows the merged namespace slice. Specification coverage remains the priority; release-controller/count decisions must use the later reviewed profile, not the historical365/53plan.


## M2A scope proposal - 19 November 2026

M1 is merged (Schema PR17/Core PR504), with packaged guidance reconciled in Schema PR16. The next [bounded named-complex-type proposal](docs/m2a-named-types/PLAN.md) defines QName references, component reuse, actual Core identity/atomization and explicit refusal controls. Planning only; no runtime/API/pin/publication change or additional passed-test claim. REQ-009 remains In Progress.


## M2A implementation candidate - 19 November 2026

The [named-complex-type slice](docs/m2a-named-types/README.md) is locally implemented and qualified:465source cases(320implementation/145public),145minimum-Core package cases,52current-Core cases and retained124/23/33binary catalogs pass; scopes overlap. Up to16global types and16root declarations, full expanded-name lookup, eager unused-type checks, shared type identity and original Core facts are supported within the decimal-only profile. No Core engine/API/pin or published package/controller change. REQ-009 remains In Progress; broader M2 datatypes/facets/attributes and official W3C qualification remain pending.
