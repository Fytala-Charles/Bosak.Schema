# ADR-010: Owned synchronous native decimal facade

**Status:** Proposed
**Date:** 2026-11-15
**Deciders:** Repository owner; public contract acceptance pending

## Context

[REQ-009 and native integration](FEATURE_REQUESTS.md) have an internal backend and adapter at commits aba6967/939ba9a. Raw Core session leases do not enforce sequential operations; the internal host profile needs one shared runner. Borrowed container adapters expire with raw work while detached decimal payloads survive. Exposing raw callbacks/handles would force every public consumer to reproduce those boundaries.

## Decision

We propose an owned synchronous scalar facade whose session retains the sole Core session/runner and whose documents own assessed inputs/snapshot admissions. The [concrete proposed contract](NATIVE-DECIMAL-PUBLIC-API-PROPOSAL.md) exposes only decimal count, detached exact decimal reads and detached XML. Explicit numerical/query limits and an explicit immutable assessment episode remain caller responsibilities. Current implementation is an internal prototype, not an accepted public API.

## Consequences

### Positive

- Applications cannot bypass the runner through the proposed public surface or retain live provider facts as detached data.
- Failed assessment/admission cleans captured ownership before a document is returned.
- A narrow real workflow can be reviewed and tested before a long-lived API is introduced.

### Negative / Accepted Costs

- This first surface is decimal-only, synchronous and string-input-only; general engine operations and stream/reader ownership are deferred.
- Public exact-value dependency, structured diagnostics, close/work races and cleanup failure policy still need explicit qualification.
- An external consumer cannot use the internal prototype; public symbols and separate assembly tests remain work.

## Alternatives Considered

| Option | Why not selected for this prototype |
|---|---|
| Expose raw sessions/providers and require each caller to coordinate lifetime | Leaves sequential ownership and borrowed-value expiry easy to bypass |
| Export live typed nodes or provenance as ordinary detached results | Misrepresents owner/operation-bound facts as transferable authority |
| Introduce asynchronous callbacks and full query syntax immediately | Adds lifetime and execution profiles beyond the qualified scalar workflow |

## References

- [ADR-009 profile](ADR-009-native-decimal-session-profile.md)
- [Internal adapter](NATIVE-SESSION-ADAPTER.md)
- [Prototype evidence](native-session-facade/2026-11-15/README.md)
