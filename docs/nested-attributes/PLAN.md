# Nested container attributes: proposed next slice

20 November 2026. Planning only, based on merged Schema main
`3e067cd73bd779a4539cd14e870950b0bc99430b` (PR26). Fresh main CI44706432874
passed 1,125 source cases, 54 release controls and six workload controls. Those
are delivered baseline results, not support for this proposed slice. Published
0.5.0-beta.1 and public Core dependency minima remain unchanged.

## Recommendation

Admit existing required/optional local attribute uses on named/anonymous nested
element-only complex containers. Preserve root AttributeCount/ReadAttribute and
terminal ScalarCount/ReadScalar indexes. Add separate descendant-attribute reads
and a detached immutable owner-element path to NativeAttributeValue, so repeated
containers with identically named attributes can be distinguished.

[Concrete contract](CONTRACT.md), [surface ledger](SURFACE.json),
[proposed ADR014](../ADR-014-owned-descendant-attribute-paths.md),
[qualification plan](QUALIFICATION.md) and [authored fixture](nested-order.xsd)
make the proposal reviewable. [Exact dated bodies](SPECIFICATION-REVIEW.json)
are review pins only. Separate semantic adoption must precede production edits.
REQ-009 remains In Progress; this is a bounded structural sub-slice, not completion
of M2/M3, XSD1.1, general API/C45 or an official W3C-suite admission.

## Existing implementation seams

NativeNestedSchema already compiles attribute uses on complex plans, then refuses
them when such a plan is referenced by a descendant. Its assessor calls
InstanceAttributes on non-root containers. NativeSchemaProvider maps every issued
attribute through RootAttributeNode using only the expanded attribute name.
That mapping must change before descendant facts can publish: repeated owners
must never share or overwrite facts merely because their attribute names match.
The existing terminal ScalarFacts projection can remain unchanged.

Attribute-profile detection currently checks global/root complex definitions.
The new profile must also detect admitted attributes on inline nested complex
types when resolving attributeFormDefault, while retaining attribute-free schema
refusal. Existing root-only cases, unsupported branches and diagnostics remain
required unless an exact intentional former-refusal delta is recorded.

## Alternatives considered

| Option | Assessment |
|---|---|
| Extend existing AttributeCount to all attributes | Breaks root-only count/index semantics; reject |
| Return descendant names/values without owners | Ambiguous across repeated or equal-named containers; reject |
| Expose live native nodes or a general XPath locator | Expands lifetime/authority/query contracts unnecessarily; defer |
| Separate descendant list plus immutable element-child index path | Three additive members, no new public types; recommended |

A path identifies a location in this assessed XML only. It conveys no schema
identity, validation authority or cross-document identity. It is not a selector
accepted by the session. Future nested simple-content support remains separate.

## Execution sequence

1. Qualify original attributes under distinct nested owners using signed minimum
   and current-reference public Core packages. Separate synthetic transport from
   actual native validation and verify signatures/source/payload identities.
2. Record exact semantic adoption and pre-code native refusal/absent API evidence.
3. Implement owner-aware assessment/facts, bounded expansion, separate reads and
   detached paths; qualify native provider, package, retained and exact API delta.
4. Review/commit/push/PR only after the appropriate explicit delivery instruction;
   future merge/main verification, Core integration refresh and publication are
   separate. AGENTS.md requires explicit commit/push requests.

No production/test catalog/package/dependency/controller changes in this proposal.
Earlier sealed nested/root-attribute dossiers and adoptions remain unchanged.
