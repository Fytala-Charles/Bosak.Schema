# ADR-013: Separate root scalar reads for decimal simple content

**Status:** Proposed - local implementation qualified; general API/C45 acceptance pending
**Date:** 2026-11-20
**Deciders:** Repository owner; no general API/C45 acceptance inferred

## Context

PriceType in the public order fixture combines decimal text with a currency
attribute. Existing leaf and attribute indexes cannot silently include root text.
Complex type annotation and actual decimal atomics must remain distinct.

## Decision

Propose HasRootScalar and ReadRootScalar on NativeSchemaDocument under the existing
owner/gate, with direct decimal simpleContent extension only. Preserve old indexes,
old rootFOTY0012, actual type provenance and detached scalar semantics. The
[contract](decimal-simple-content/CONTRACT.md) and [adoption](decimal-simple-content/ADOPTION.md)
define exact grammar, limits, outcomes and excluded derivation/query features.

## Consequences

Two additive members require package/retained/API/example qualification. Complex
status is independent of typed-value absence; node annotation must not be replaced
by its decimal atomic type. Core feasibility is public-only and synthetic until
actual native assessment is qualified. No Core upgrade, broad derivation relation,
release/publication or overall API/C45 acceptance is implied.

## Local implementation checkpoint - 20 November 2026

The [candidate](decimal-simple-content/README.md) implements the exact two-member surface under the before-code adoption, with1036source cases and unchanged950baseline outcomes. Published-Core feasibility is distinct from actual native root/fact/provenance tests. ADR remains Proposed; no general API/C45 or publication acceptance is inferred.
