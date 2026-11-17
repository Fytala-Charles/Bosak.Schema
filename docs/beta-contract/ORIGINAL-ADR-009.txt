# ADR-009: First native decimal session profile

**Status:** Proposed
**Date:** 2026-11-15
**Deciders:** Repository owner; acceptance pending review of implementation evidence

## Context

[REQ-009](FEATURE_REQUESTS.md) and the pending native session integration need an executable scope after Core 1.81.2 adoption. Current descriptor/assertion services and 124 standalone regression cases establish no complete native structural authority. Historical native decimal/source-feasibility receipts retain their original identities and cannot qualify this released dependency candidate. ADR-008 is referenced by archived native work and its number is reserved; this record does not supersede it or ADR-006.

## Decision

We propose implementing [schema-native-decimal-session-v1](NATIVE-SESSION-PROFILE.md) as an opt-in native finite decimal assessor with authority-owned facts and a single scalar session identity route on published Core 1.81.2. Explicit Unsupported results bound unimplemented schema, relation and compatibility operations. Provider resources remain host-owned and retained through operation completion. No core product dependency on Schema is introduced.

This selects a work scope. API freeze, full XSD 1.1 acceptance, performance budgets and publication require separate evidence and decisions.

Internal implementation checkpoint, 15 November: the backend is committed at `aba6967`; the [Core adapter](NATIVE-SESSION-ADAPTER.md) now supplies authentic issuance, guarded captured nodes and a real scalar integration test. One original assessment per capture is retained; reassessment refuses. One shared host runner serializes work through actual task completion because Core lease counts do not distinguish overlapping runs. This bounded implementation is not owner/API/global acceptance; this ADR remains Proposed.

## Consequences

### Positive

- One finite grammar and twelve paired acceptance groups make provider delivery reviewable.
- Released contracts can be qualified without treating old source overrides or equal schema text as authority.
- Original PSVI, exact scalar transport and resource ownership receive independent acceptance controls.

### Negative / Accepted Costs

- The native backend and Core adapter are implemented internally, with 76 backend and 20 adapter cases. Public entry point review and overall profile acceptance remain required; existing public assertion evaluation stays separate from native authority.
- Cross-universe positive compatibility, wider datatypes, general engine execution and full structural coverage remain pending.
- The [released-package probe](native-session-probe/2026-11-15/README.md) establishes `$value` with default compile options on a test-issued decimal. A real native consumer remains pending; the [specification basis](NATIVE-DECIMAL-SPECIFICATION-ADOPTION.md) is recorded before backend implementation.

## Alternatives Considered

| Option | Why Rejected for this first profile |
|---|---|
| Promote existing descriptor/assertion validation to native authority | Does not establish complete structural assessment or authentic component ownership |
| Import historical native work and claim its old tests qualify 1.81.2 | Source adoption, semantic evidence and fresh released-candidate qualification remain necessary |
| Implement all XSD 1.1 and positive universe interchange in one step | Prevents isolation of the first provider/session boundary and its failure modes |

## References

- [Profile and acceptance plan](NATIVE-SESSION-PROFILE.md)
- [Current architecture](ARCHITECTURE.md)
- [Historical scoped O2 return](core-disposition-responses/2026-11-03-v2/BCAP-O2-Schema.v2.response.json)


17 November2026 beta review: [API contract audit](beta-readiness/API-CONTRACT-AUDIT.md) and [decision draft](beta-readiness/CONTRACT-DECISIONS.md) propose current boundaries. Status remains Proposed; no acceptance is inferred from alpha publication.
