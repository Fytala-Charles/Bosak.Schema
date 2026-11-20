# ADR-015: Nested decimal simple content in the existing scalar projection

**Status:** Proposed - local implementation; general API/C45 acceptance pending
**Date:** 2026-11-20
**Deciders:** Repository owner; broader API/C45 acceptance remains separate

## Context

The delivered profile supports decimal simple-content roots and nested
element-only containers with owner-aware attributes. It refuses price-with-currency
inside an order. These complex terminal nodes have a typed value and must not be
classified as element-only containers merely because IsComplexType is true.

## Decision

Propose direct decimal/named-decimal simpleContent extension on local descendant
complex elements, with existing attributes and original complex/atomic provenance.
Reuse ScalarCount/ReadScalar for descendant scalar-bearing terminals in document
order; keep root scalar/attribute reads separate. No public surface additions.
[Contract](nested-simple-content/CONTRACT.md) defines the exact scope and refusals.

Permit three-segment OwnerElementPath only when a scalar-bearing complex owner is
at existing maximumdepth4. Keep all element/terminal/attribute budgets, original
captured identity checks and lifetime semantics. The previous two-segment bound
was specific to element-only owners requiring a child, not a new public path type.

## Consequences

Applications can consume nested price/currency without a new read API. Scalar
indexes of already valid documents remain unchanged. Qualification must prove
complex governing/declared versus actual decimal types, local/global declaration
identity, normalized node string versus lexical XML export and complete atomic
assessment. Signed public Core feasibility, actual native baseline/adoption and
source/package/retained/API evidence precede delivery. No full-XSD, general
API/C45, registry publication or perpetual API-freeze acceptance is inferred.


## Local checkpoint - 20 November2026

The adopted candidate implements this zero-surface semantic extension with1,255
source cases. Before-code adoption/feasibility and actual native qualification
remain distinct; compatibility evidence follows in the candidate dossier. No
general API/C45 or publication acceptance is inferred.
