# ADR-011: Additive owned native scalar facade

**Status:** Proposed ? concrete API review and published-Core feasibility only
**Date:** 2026-11-19
**Deciders:** Repository owner; no new API/contract acceptance recorded

## Context

REQ009/CoreREQ142 prioritizes specification coverage and usable application
workflows. The delivered decimal facade promises DecimalCount/ReadDecimal and
cannot silently adopt heterogeneous values. M2C needs an explicit consumer path
for string, then Boolean/integer, without weakening owner lifetime or confusing
primitive storage with original schema authority. ADR010's bounded acceptance
remains valid for its old profile.

## Decision

Propose NativeSchemaSession/Document, a sealed detached discriminated scalar,
explicit immutable options/context and producer-only result/diagnostic shapes.
M2C1 exposes only string/decimal. Later Boolean/integer members require separately
adopted slices. Exact names/signatures/exceptions and candidate limits are in
[the API proposal](m2c-builtin-datatypes/api-review/API-PROPOSAL.md) and
[machine-readable surface](m2c-builtin-datatypes/api-review/SURFACE.json).
Share guarded internal ownership under explicit profile selection. Preserve every
old decimal member and its mixed-schema refusal. No provider/node/raw XdmValue
or transferable PSVI escapes. This is a proposed decision, not implementation.

## Consequences

### Positive

- Mixed application values get a precise opt-in path without changing old indexing.
- Existing Core typed identity can be reused on both tested published versions.
- Detached scalars retain a simple application contract while authority remains internal.

### Negative / Accepted Costs

- Two public profiles coexist and need explicit guides and retained compatibility lanes.
- New result/options/context types add surface and need independent contract evidence.
- New accessor, export-capacity and lifecycle behavior requires real implementation controls.

## Alternatives Considered

| Option | Why not selected |
|---|---|
| Widen DecimalCount/ReadDecimal silently |Changes existing public meaning and fails typed access on nondecimals |
| Return raw object/XdmValue/provider handles |Weakens type/lifetime clarity and exposes unintended payload/authority surfaces |
| Replace or rename existing facade |Unnecessary binary/source break for the bounded published contract |
| Ship all datatypes/facets in one slice |Conflates separate semantic/adoption and validation gates |

## References

- [REQ009](REQ-009-SPECIFICATION-COVERAGE.md), shared CoreREQ142
- [Existing accepted ADR010](ADR-010-owned-native-decimal-facade.md)
- [Datatype plan](m2c-builtin-datatypes/PLAN.md)
- [Published-Core feasibility](m2c-builtin-datatypes/api-review/README.md)

Broader CoreC45/provider freeze, full XSD1.1, performance SLAs and publication
remain separate. No existing acceptance is extended by inference.


## M2C1 local implementation - 19 November 2026

The ten-type string/decimal surface is implemented as a local candidate. See [adoption and qualification](m2c-string-facade/README.md). Original proposal source is preserved in that dossier; this records implementation, not global API/ADR acceptance, C45 freeze or publication. Legacy decimal APIs remain unchanged.
