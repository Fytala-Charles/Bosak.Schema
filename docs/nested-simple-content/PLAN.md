# Nested decimal simple-content proposal

20 November 2026. Planning only, based on merged Schema main
`32f8214c670a84286bd28e0a3d51e1d5ffb4f1f7` (PR27). Fresh main CI44726412611
passed1,200source cases(404implementation/796public),54release controls and six
workload controls. Published0.5.0-beta.1 and public Core minima are unchanged.

## Recommended scope

Reuse the delivered root decimal simple-content grammar for descendant local
elements in the bounded nested sequence profile. Admit named and anonymous
complex types with simpleContent/extension directly based on xs:decimal or an
already admitted named decimal restriction, plus existing local attributes.
The representative use is a price-with-currency inside a repeated order line.
No new public API is proposed: ReadScalar enumerates descendant scalar-bearing
terminals in document order, now including these complex simple-content elements;
root scalar reads stay separate. Existing valid document indexes remain unchanged.

[Contract](CONTRACT.md), [proposed ADR015](../ADR-015-nested-simple-content-scalar-projection.md),
[zero-delta surface](SURFACE.json), [qualification plan](QUALIFICATION.md) and
[fresh dated source review](SPECIFICATION-REVIEW.json) precede implementation.
Separate semantic adoption and before-code feasibility/baseline evidence remain
next. REQ-009 is In Progress; no full-XSD/W3C-suite or general API/C45 claim.

## Practical representative fixture

[nested-order.xsd](nested-order.xsd) derives from the previously authored nested
order fixture. Its amount declarations become price declarations referring to
PriceType; price instances contain decimal text and required currency attributes.
The PriceType XML block and standalone global price declaration are extracted
unchanged (LF normalization only) from the public orders.xsd. Their enclosing
schema namespace here is urn:bosak:nested-plan, so their expanded component
identity differs from the original example. No whole-schema equivalence is
claimed. [Origin/hashes](FIXTURE-ORIGIN.json) distinguish reused blocks from the
new composition. The full original orders.xsd still contains independent
pattern/mixed/unbounded exclusions and remains outside scope.

## Implementation seams and semantic risks

NativeNestedSchema already compiles decimal simple-content plans but refuses
ContentType on a descendant reference. Its budget helper already treats such a
plan as one node/scalar plus its attributes. Assessment currently routes every
container through element-only whitespace/sequence checks, so simply removing the
compile refusal would reject decimal text or publish the wrong facts.

The scalar-fact projection filters descendants by HasNoTypedValue, rather than
IsComplexType. Preserve that distinction: a complex simple-content terminal has
an original complex governing/declared type and an actual decimal atomic type.
Original local declaration identity must stay distinct from a global price root
using the same named complex type. Verify content-type handle registration and
actual minimum/current public Core read/atomization routes before engine edits.

Descendant attribute owner paths previously reached at mosttwo segments because
an element-only owner needs a child within depth4. A scalar-bearing complex
terminal can itself appear at depth4 and own attributes; this proposal deliberately
allows three path segments without changing depth or public member signatures.
All original owner/index/name/node checks, total-use budgets and lifetime rules
must continue to apply. This semantic change needs explicit before-code adoption.

## Known baseline refusals to freeze before code

Static catalog inspection identifies NS-062 and NA-046 as compile-Unsupported
nested simple-content shapes that may become admitted. Their existing instance
expectations were unreachable placeholders after compile refusal; do not infer
new instance Valid from those fields. NS-062 supplies nested markup inside the
simple-content element; NA-046 omits a required child. Both require explicit
new instance outcomes once compile is admitted. Record exact original row hashes
and intentional deltas before changing expectations. Other exclusions remain.

## Next sequence

1. Prove nested original complex/decimal provenance, root/local declaration
   identity and depth4attribute owners via signed minimum/current public Core
   APIs. Keep synthetic transport/path prototypes separate from native validity.
2. Freeze actual delivered refusal/index/API baselines, then record exact dated
   semantic adoption, deviations and intentional case transitions before code.
3. Implement and qualify actual native assessment, original facts, normalized
   node string-value versus lexical export, budgets and read/path semantics.
4. Deliver reviewed source/package/retained/API/README evidence under an explicit
   commit/push request; merge/main verification and publication remain separate.

No production/test catalog/API/Core pin/controller changes in this proposal.
Earlier sealed dossiers and publication receipts remain unchanged.
