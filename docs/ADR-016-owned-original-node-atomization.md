# ADR-016: Separate owned original-node atomization

**Status:** Proposed — feasibility, semantic adoption and owner API review pending
**Date:** 2026-11-20
**Deciders:** Repository owner; general CoreC45/ADR006 acceptance remains separate

## Context

REQ-009 prioritizes specification coverage and useful applications across Schema
and Bosak. The native facade owns `$value` scalar sessions, while the released
public Core has a distinct bounded original-node data profile. Original facts
cannot be transferred to a different advisor/environment merely by exposing nodes
or changing a profile guard. Anonymous types also must not gain invented QNames.

## Decision proposed

Introduce a separate owned original-node data facade with fixed Core data profile,
its own complete native assessment/admission episode, validated original target
paths/expanded attribute names and detached NativeScalarValue results. Keep old
scalar guards and APIs unchanged. Do not expose arbitrary expressions, nodes,
callbacks or live authority. [Contract](typed-node-contract/CONTRACT.md) and
[API proposal](typed-node-contract/API-PROPOSAL.md) remain unimplemented proposals.

## Consequences

The environment and issuer boundary becomes explicit; applications can address
admitted original scalar elements/attributes without primitive aliases or escaped
PSVI handles. There are additional owned types and potentially separate captures
when an application uses both scalar and data facades. This still does not provide
general XPath navigation/comparison over schema-aware native trees.

## Alternatives considered

| Alternative | Assessment |
|---|---|
| Relax the existing Bind guard | Insufficient public selection/ownership contract; risks mixing assessment environments |
| Add arbitrary Evaluate to existing documents | Released Core bounded profiles and existing scalar episode do not establish that promise |
| Return raw node/snapshot/authority callbacks | Widens lifetime, reentrancy and caller ownership beyond the accepted synchronous facade |
| Add named primitive aliases to anonymous types | Violates original anonymous type identity and masks the integration boundary |
| General Core/Schema typed-query integration first | Remains a separate broader contract; retain it as a future requirement |

## References

- [Owner requirement](REQ-009-SPECIFICATION-COVERAGE.md).
- [Existing bounded facade ADR010](ADR-010-owned-native-decimal-facade.md).
- [Native route limits](anonymous-simple-restrictions/implementation/ROUTE-LIMITS.json).
- [Required qualification](typed-node-contract/QUALIFICATION.md).
