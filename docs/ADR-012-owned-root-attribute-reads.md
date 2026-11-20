# ADR-012: Separate owned root attribute reads

**Status:** Proposed - concrete contract and published-Core transport review only
**Date:** 2026-11-19
**Deciders:** Repository owner; no new contract/API acceptance recorded

## Context

The opt-in owned facade now covers four scalar families and selected named
restrictions. Its ScalarCount/ReadScalar promise present leaf indexing. Root
attributes are needed for application identifiers/currencies/flags, but adding
them to the leaf list would silently change caller behavior and retained binaries.
Actual attribute facts must remain bound to the original owner/source assessment.

## Decision

Propose root-only local required/optional attribute uses with existing datatypes
and forms, and add AttributeCount/ReadAttribute plus a producer-only detached
NativeAttributeValue namespace/local-name/scalar entry. Keep every existing leaf
index and old decimal refusal. [Full contract](m2e-attributes/CONTRACT.md) and
[signature ledger](m2e-attributes/SURFACE.json) define the exact additive surface,
finite limits, outcomes, ownership and exclusions before implementation.

## Consequences

One new type/five members require review and exact API qualification. The existing
Core can carry synthetic original attribute facts through published APIs; this is
not native Schema attribute validation. Separate adopted implementation and actual
native node/consumer/retained proof remain required. No raw node/authority escape,
Core upgrade, general API/C45 acceptance or release/publication is implied. Leaf
attributes, defaults/fixed, global refs and attribute assertions stay separate.

## Local M2E1 implementation checkpoint - 19 November 2026

The [before-code adoption and candidate](m2e-root-attributes/ADOPTION.md) implements exactly the proposed root-only attribute surface locally with950source tests, original native facts and unchanged prior leaf behavior. The sealed planning dossier retains its original before-code claims; current source/evidence is separate. ADR status remains Proposed; no general API/C45 acceptance or publication is inferred.
