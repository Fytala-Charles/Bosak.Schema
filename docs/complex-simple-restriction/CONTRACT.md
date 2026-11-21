# Proposed bounded decimal complex simple-content restriction

21 November 2026. **Proposal only: not adopted, implemented or runtime-qualified.**
Product base7ce8ada7ca78a77a13e07151b199639fa757317a, merged PR35. Main CI
passes2,143source cases on the exact reviewed tree. Core1.81.2/1.81.17 and public
APIs stay unchanged. [Review pins](SPECIFICATION-REVIEW.json) identify exact
immutable source bodies and sections; these are not implementation adoption.

## Useful first slice and explicit exclusions

Admit one direct decimal complex simple-content restriction of a named complex
base B whose current definition extends xs:decimal or an admitted named decimal
simple restriction. Require B and the derived type to have no application
attribute uses, wildcards or assertions in this first slice. Do not silently
inherit, drop or approximate prohibited/required/default/fixed attributes.

Support existing named derived types and anonymous root/local complex owners
where their existing structural routes admit scalar complex content. Resolve
forward named base references using actual compiled components, not declaration
order; detect cycles eagerly even for unused types. Restriction-of-restriction
complex chains remain Unsupported until separately scoped. Existing mixed-profile
structure/depth/node/scalar/type/input/operation capacities remain unchanged.
Legacy NativeDecimalSession continues to refuse complex simple content.

Admit only the six existing exact-decimal facets: min/max inclusive/exclusive,
totalDigits and fractionDigits, including an empty facet list. No inline simpleType,
list/union, enumeration/pattern, explicit whiteSpace/fixed facets, other scalar
families, complexContent, assertions, block/final/abstract or new acquisition.
Unrelated definitions retain their current behavior; eagerly refuse excluded
constructs in the new restriction whether or not an element later uses it.

## Two distinct derivation edges

| Component | Required identity and relationship |
|---|---|
| Original element declaration D | Retains its original declared type T |
| Governing complex type G | Original derived complex component for declared use; its actual complex base is B |
| Base content atomic type C0 | Original B.content simple type, primitive or admitted named decimal restriction |
| Derived content atomic type C1 | Distinct anonymous simple restriction of C0, owned by G and retained even with zero new facets |
| Core facts.Type / typedValue.GoverningType | G |
| Core atomic declared/governing provenance | G |
| Core atomic actual provenance | C1, never G, B or a fabricated xs:decimal alias |

Structures3.4.2.2 maps a restriction's simple content to an anonymous simple
component whose actual base is the base complex type's content type (when no
inline simpleType is supplied). Store both G->B and C1->C0. The existing fallback
which equates a complex type's base with ContentType applies to extension;
it is insufficient for restriction. Preserve actual component objects in an
immutable compiled graph; do not infer derivation from equal facets or values.
C1 has no QName/name alias. Construct it once per compiled G, never per instance.
Different complex contexts own distinct C1 components even when facet sets and
value spaces are equal; immutable facet data may be shared without merging type
identity. Account for this metadata under the existing bounded schema/definition/
expanded-structure budgets. Register every component required by guarded native
facts and original atomization; its target namespace/context is not a name.

No request or same-G xsi:type retains existing behavior. Existing xsi:type
policy deliberately defers changing one complex governing type to another:
requesting derived G under declared B must be Unsupported for this slice once
its true derivation is known, not incorrectly Invalid or silently admitted.
Weakening G to B or selecting an unrelated complex sibling is Invalid. Do not
expand that separate policy merely to implement declared complex restrictions.

## Static restriction validity and runtime enforcement

Preserve and enforce all inherited C0 facets, then valid new C1 facets. A runtime
intersection alone cannot prove a legal restriction declaration. Reject an
attempted bound weakening even when the tested instance falls inside both ranges.
Handle inclusive/exclusive equality correctly, inherit omitted facets, require
nonincreasing digit limits and validate the resulting effective constraints.
Capacity refusal remains separate from an invalid declaration.

The outcome mapping is fixed for this proposed slice in OUTCOMES.json. Within
recognized new reference/facet syntax, malformed/unbound/absent closed-schema
base names and invalid facet values/derivations are schema-Invalid using existing
BSK-NATIVE-SCHEMA-INVALID. Capacity and excluded grammar/dependencies remain
Unsupported with existing reasons. Complex restriction chains/cyclic dependencies
are outside the one-level profile and eagerly Unsupported/DeclarationShape;
this does not claim full cycle diagnosis. No global rewrite of legacy direct
restriction, nested-structure or type-reference outcomes is permitted.

FACET-RULES.json records all16 new-versus-base bound conditions and four
same-datatype bound-pair constraints directly from Datatypes4.3.7.4-4.3.10.4.
Do not substitute a generic nonempty-interval or value-space-containment check.
Equal inclusive bounds may define a singleton; equal exclusive bounds in one new
step may define a valid empty type. Mixed inclusive/exclusive equal bounds are
invalid. Cross-check new bounds against every relevant inherited bound, including
the opposite end. Preserve the distinction between no accepted instances and an
invalid schema. Inherited digit limits and consistency checks apply even when
one digit facet is omitted locally. A runtime conjunction remains useful for
value enforcement after static derivation is proven; it is not that proof.
Existing helpers expose only runtime Accepts, so prepare an internal immutable
facet profile/derivation check without changing public APIs or old route outcomes.

Root and nested-terminal assessment must use C1, retain D/G, and publish only
complete valid original facts. ReadRootScalar / ReadScalar and fixed original-node
data must agree on the exact decimal. HasNoTypedValue stays false on scalar
complex owners and true on element-only ancestors. Preserve collapsed node
string-values versus original parsed lexical XML export, cancellation, quotas,
no partial facts/snapshots and detached-value lifetime. Reassessment must create
fresh authority; copied/foreign nodes never inherit it.

## Feasibility and qualification gates

After reviewing and sealing this proposal, record an experiment-only scope using
the exact review pins before editing copied source. Use a copied-source actual
native probe on signed Core pins: prove a value allowed by B but rejected by C1,
exact D/T/G/B/C0/C1 identities, anonymous content provenance/annotations, root and
nested reads, no-facet distinct identity, valid-empty versus invalid-schema rules,
no-publication and foreign ownership refusal. Preserve prototype source and
runtime hashes, signed package/source/cache identities and failed iterations.
A Schema project reference in an experiment is not package-only qualification.
Only after that gate passes, record a separate production ADOPTION.md before
production semantic edits. Review pins, this proposal, upstream fixture revision
and feature PR/publication identities remain distinct from adoption.

Then implement and run authored static/instance/refusal controls, full source,
min/current package-only consumers, retained124/23/33, zero54-type/401-row API delta,
all signed dependency/source/runtime checks and the unchanged guarded frozen
corpus. The direct external case is MS-ComplexType2006-07-15/ctD017/ctD017.v.
It is one valid example, not a forecast pass or proof of negative enforcement.
The three assert006 cases still require integer content and inherited assertion
semantics; the nested namespace-uri case remains a separate slice.
