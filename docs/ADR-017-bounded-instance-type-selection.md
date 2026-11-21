# ADR-017: Bounded instance-specified type selection

**Status:** Proposed ? native feasibility, semantic adoption and review pending.
**Date:** 2026-11-20
**Deciders:** Repository owner; general Core C45/ADR006 acceptance stays separate.

## Context

REQ-009 prioritizes specification coverage and application usefulness. The next
ranked gap is xsi:type. Four frozen invalid cases currently refuse as Unsupported,
but rejecting every override would not provide the requested capability. Native
states retain original declarations but have no separate selected governing type
or explicit derivation relationships. Published Core facts and atomic evidence
require the governing type, with the original declaration retained separately.

## Proposed decision

Implement only the [bounded contract](xsi-type-contract/CONTRACT.md) after actual
native feasibility on both pinned Core versions and a separate semantic adoption
record. Preserve D and T, represent effective G, resolve instance QNames through
a closed schema registry and validate true component derivation. Admit exact-type
and current same-family direct restrictions; refuse deferred valid derivations
as Unsupported rather than Invalid. Keep known unsupported built-ins distinct
from absent names, with no invented authority handles. No new public API.

## Consequences and alternatives

All annotations, facts, scalar values and provenance must agree on G. This costs
explicit type-graph/effective-state metadata and qualification across all native
paths. It preserves original declaration identity, facets and episode ownership.
Simple attribute whitelisting, treating QName text as authority, reusing T after
selecting G, deriving type compatibility from value/facet similarity, and rejecting
all overrides merely to gain four pilot matches are insufficient alternatives.

Cross-family decimal-to-integer selection, complex inheritance, new datatypes,
nil, lists/unions, general XPath/fullPSVI and release/publication remain separate.
The original adopted API ADR016 remains unchanged; this proposal grants no broader
API/C45 acceptance. [Qualification gates](xsi-type-contract/QUALIFICATION.md).
