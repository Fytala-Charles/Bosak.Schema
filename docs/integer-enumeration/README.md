# Bounded integer-enumeration contract

24 November 2026. Based on merged main b08187d5f59f47a068737154ecdd6872506d3a88, with 3,885 source tests passing. This follows the [post-particle prioritization](../post-particle-coverage/README.md). It is a proposed contract, not semantic adoption, implementation or executed native acceptance.

## Scope and normative basis

Add enumeration to the admitted primitive integer and twelve descendants, using existing exact BigInteger values and original component/typed-value transport. Cover existing named/anonymous simple roots, scalar members, attributes and supported integer simple-content extensions/restrictions. Retain both compiler routes and current declaration, named/anonymous, node and occurrence bounds.

[Exact specification pins](SPECIFICATION-REVIEW.json) cover immutable XSD 1.1 Structures/Datatypes, Namespaces and XDM bodies. Review enumeration/base validity under Datatypes 4.3.5.1-5 and 4.1.4, integer lexical/value rules under 3.4.13, whitespace under 4.3.6 and facet overlay under Structures 3.16.6.4. Publication revision, current review and future semantic adoption are separate records.

Arbitrary user simple-type chains, greater complex restriction depth, decimal enumeration, integer patterns, new scalar kinds, wider assertion grammar and generic facet metadata/annotations remain excluded. Legacy decimal and existing string-enumeration behavior do not silently widen. No full XSD/PSVI/C45 claim.

## Proposed semantics

Enumeration entries are base-typed **values**, not strings. Permitted signs, leading zeros and XML-whitespace aliases can denote the same integer. Signed zero does not create a distinct value. NBSP and Unicode digits are not added to integer lexical syntax by permissive runtime parsing.

Every enumeration declaration must be valid for its **base type**, including builtin ranges and inherited restrictions. A byte entry of 128 is a recognized invalid declaration. Malformed or missing values in admitted enumeration grammar are Invalid; syntax outside this profile remains Unsupported.

Instance values must satisfy membership and every applicable sibling/inherited facet. A base-valid entry is not rejected as a declaration merely because it fails a newly supplied sibling facet. Integer enumeration {1} with sibling maxInclusive 0 has no valid instance among zero or one; this proposal does not invent an extra cross-facet schema-invalid rule. [Recorded interpretation](SEMANTICS.json) follows base datatype validity and facet overlay; no substitute validator was used as proof.

An omitted derived enumeration retains its inherited constraint. Replacement entries must belong to the base value space, preventing widening. Existing one-level complex simple-content restriction supplies inherited-enumeration controls without introducing arbitrary user simple-type chains. Preserve original declaration/type, complex governing and atomic-content distinctions.

## Representation limits

Propose at most **16 source entries per restriction**, counted before canonical value-set deduplication. Sixteen identical literals consume sixteen source slots; seventeen refuse. Preserve the existing **256 UTF-16 character lexical bound** and explicit native digit capacity before normalization can hide large input. These are profile capacities, not XSD limits.

Capacity/work refusal is Unsupported, distinct from an invalid declaration or instance. Check unused definitions eagerly and retain aggregate schema/type/source-site limits, shared work/cancellation and existing lifetime guards.

## Planned validation

[105 XML cases](CASE-PLAN.json) propose both facades on signed Core 1.81.2 and 1.81.17: all thirteen families, lexical aliases, bad entries/instances, base bounds, exact beyond-Int64 values, duplicate/distinct 16/17 entries, lexical256/257, leading-zero capacity, existing placements, namespaced selected xsi:type, mixed-catalog routing, sibling empty intersections, inherited narrowing/retention and excluded grammar. XML well-formedness is checked statically; native outcomes remain unexecuted.

[18 native gates](NATIVE-GATES.json) require actual physical node/attribute paths, original declaration/governing/atomic identity, source/snapshot/origin and exact values. Include root/leaf counts, no partial facts, refusal recovery, authority, one-open-document constraints, export/reopen, detached reads, disposal and pre-cancellation. Attribute-only empty content retains its existing typed-value semantics. No general typed-query/closure or timing acceptance follows.

[Eleven exact source/test snapshots](SOURCE-REVIEW.json) identify allowlists, routing and expectation dependencies. All 311 recorded product/test/tool/build inputs are unchanged.

## Existing expectations and originals

[Static impacts](POTENTIAL-EXPECTATION-IMPACTS.json) identify DI-141, IR-070, excluded-facet and unused-bad-enumeration controls. They are possible changes, not measured admissions. Preserve original schemas, instances, IDs, data sources and earlier receipts; measure the candidate before reconciling outcomes, values or reasons.

The retained original-fixture harness currently asserts Invalid for every admitted original. One of the [two original examples](ORIGINAL-EXAMPLES.json) has upstream Valid expectation, the other Invalid; both currently compile Unsupported. If admission is measured, use the preserved upstream validity instead of retaining that latent Invalid-only assumption or rewriting metadata. Original schema/instance/metadata/upstream hashes remain pinned. No admission forecast.

## Next sequence

Record experimental adoption before isolated source edits. Then qualify baseline refusals, public/native controls, signed Core identities, API and original-source regression. Retain all 3,885 prior stimuli and record measured expectation/harness corrections separately, including any required displayed-name mapping.

Only after feasibility should separate production adoption precede source edits, explicit expectation reconciliation and full source/package/native/retained/API/exact example/frozen and expanded corpus qualification. No product source, tests, API, Core pin, package, controller, tag or registry change occurs here. Planning changes remain uncommitted on planning/post-particle-coverage.
