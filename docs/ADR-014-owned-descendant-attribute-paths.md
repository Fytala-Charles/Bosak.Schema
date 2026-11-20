# ADR-014: Separate descendant attribute reads with detached owner paths

**Status:** Proposed - local implementation qualified; general API/C45 acceptance pending
**Date:** 2026-11-20
**Deciders:** Repository owner; no new API/C45 acceptance recorded

## Context

PR26 admits finite nested element-only sequences, while attribute validation and
reads remain root-only. Flattening every attribute into the existing root list
would change caller indexes. Names alone cannot identify attributes on repeated
containers. Live node handles or general path selection would expand authority,
query and lifetime surfaces before those contracts are needed.

## Decision

Propose DescendantAttributeCount/ReadDescendantAttribute on NativeSchemaDocument,
and an immutable OwnerElementPath on the producer-only NativeAttributeValue.
The [contract](nested-attributes/CONTRACT.md) defines element-child index paths,
root-empty metadata, descendant owner-preorder/declaration-order reads,64total
potential attribute uses including root and unchanged scalar/root indexes.
[Exact surface](nested-attributes/SURFACE.json) proposes three additive members,
no new public type, constructor, option, enum or path-based selector.

## Consequences

Application callers can distinguish repeated owners using detached data without
retaining native nodes or schema authority. Paths describe an assessed document
only; optional element presence can change later paths. The implementation must
retain exact original owner/use facts and independently validate the whole tree.
Signed public Core feasibility, separate adoption and real native/package/retained
qualification remain gates. Nested simple content, scalar-leaf attributes,
general API/C45 acceptance and publication are separate decisions.


## Local implementation checkpoint - 20 November 2026

The [adopted candidate](nested-attributes/README.md) implements exactly the
three-member proposal, with1,200source cases and original owner-aware facts,
immutable paths and preserved root/scalar indexing. Source/package/retained/API
receipts remain separate from the before-code proposal and feasibility. No
general API/C45 or publication acceptance is inferred; ADR status stays Proposed.
