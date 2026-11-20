# Bounded nested-sequence proposal

20 November 2026. Planning only, based on merged Schema main
`c34e9fe83f108bbb786c5978cd27816cabe895b4` (PR25). Fresh main CI
44695518279 passed 1,036 source cases, 54 release controls and six workload
controls. These are prior delivered results, not new runs or nested support.
Published 0.5.0-beta.1 and public Core dependency minima remain unchanged.
REQ-009 remains In Progress.

## Recommended slice

Extend only NativeSchemaSession with finite acyclic nested element-only sequences.
Admit local elements with an existing scalar type, a named element-only complex
type, or one inline anonymous element-only complex type. Reuse existing expanded
names, form defaults/overrides, admitted scalar datatypes and named restrictions.
Resolve named types independently of declaration order. Preserve distinct local
declaration identity when two declarations share a type. Continue eager checking
of every definition, including unused and unselected roots.

This is the first structural sub-slice of M3, not completion of M2/M3 or XSD 1.1.
No public API addition is proposed. [Contract](CONTRACT.md),
[qualification plan](QUALIFICATION.md), [review sources](SPECIFICATION-REVIEW.json)
and [representative fixture](nested-order.xsd) make the next step reviewable.

## Why this shape

Existing public orders.xsd supplies namespaced OrderType and PriceType examples,
but its order contains a scalar string item with an anonymous pattern restriction;
it is not itself a nested-complex-type example. Pattern, mixed content and
unbounded repetition independently keep the full fixture outside the profile.
The new nested order here is authored specifically for this proposal, not copied
unchanged, an official W3C case or a real application acceptance report.
It demonstrates repeated line containers with string SKU, exact integer quantity
and decimal amount. Root id is an existing required string attribute. It does
not claim to support PriceType nested inside a line: nested simple content and
nested attributes remain separate scopes. The unchanged root PriceType remains
supported by the delivered simple-content slice.

## Implementation risks discovered by inspection

NativeDecimalSchema currently compiles only scalar local declarations and assesses
one root plus direct leaves. NativeSchemaInput already wraps DescendantsAndSelf in
document order; NativeSchemaProvider maps assessment states positionally onto that
array. NativeDecimalSessionCore treats every element fact after the root as a
scalar. Merely allowing complex type references would misalign counts/reads or
atomize containers. Implementation must preserve a complete element-fact array
and introduce a separate internal ordered scalar-fact projection.

Every container needs its original local declaration, original complex type,
HasNoTypedValue and guarded FOTY0012 behavior. Terminal leaves need original
simple typed facts. Complete recursive assessment, including each sibling's
occurrence count, must succeed before facts are published. No fallback to legacy
validation, fabricated facts, reflection/friend access or Core dependency change.

## Next execution order

1. Review the proposed contract and record a separate exact semantic adoption
   before production changes. Keep this dated review pin distinct from adoption.
2. Prove nested node/type/declaration identity and container/leaf atomization
   through signed minimum/current public Core APIs. A synthetic probe qualifies
   transport only, not Schema validation.
3. Implement bounded component resolution and recursive assessment with the
   separate scalar projection; qualify the actual native provider and public
   consumers against the plan. Preserve all prior 1,036 outcomes unless an exact
   intentional former-refusal delta is documented.
4. Review the implementation and evidence before delivery. Package publication,
   general API/C45 acceptance and refreshed Core integration pins are separate.

Commit/push require an explicit request under AGENTS.md. This proposal changes
no production source, test catalog, package, dependency, release controller or
historical sealed receipt. No feature implementation is claimed.
