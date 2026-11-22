# Proposed bounded simple-root semantics

## Normative scope and intentional exclusions

The exact [2012 XSD, 2009 Namespaces and 2017 XDM review pins](SPECIFICATION-REVIEW.json)
remain distinct from future implementation adoption. Structures 3.3.1-2 permits
simple declared types; 3.3.4.1-4 governs declaration matching, instance selection
and local type validity; 3.3.5.3-4 distinguishes original declaration, type and value.
Datatypes defines the existing four scalar families, lexical mapping and facets.
XDM typed values must come from the original assessed element, including an empty
string as one atomic value. This review adopts no new behavior by itself.

Proposed placements are global built-in scalar types, named direct restrictions
and anonymous direct restrictions on the already admitted scalar families. Reuse
existing restrictions, namespace/QName rules, xsi:type derivation checks and exact
numeric representation bounds; do not add chains, derived integer families,
list/union, defaults/fixed, nil, assertions on simple types, imports/includes,
choice/all or other datatypes. Unknown/unsupported schema syntax remains refused.
Comments and processing instructions retain the current text-only scalar refusal;
application attributes remain Unsupported in this finite profile. Location hints
remain unfetched. This is a bounded subset rather than complete simple-element XSD.

## Original identity and values

The actual element declaration remains global in its target namespace. A named
restriction retains its own name/namespace as governing type; an anonymous simple
type remains anonymous, without borrowing the declaration name or namespace. Built-in
types retain their XSD identities. The original atomic evidence must follow the
qualified scalar route; do not fabricate a complex simple-content wrapper or
relabel a detached atomic to make a root appear supported.

Existing same-family xsi:type selection must validate using the selected governing
type and its facets, retaining the original element declaration. Unknown/unbound/
unrelated selections retain their existing Invalid/Unsupported distinctions. The
integer-to-decimal representation-changing case remains outside the finite profile.

String root content preserves parsed XML whitespace, case and supplementary scalars.
Empty string is one String atomic. Numeric/boolean whitespace collapses as before.
Do not reuse the current numeric simple-content root's unconditional Collapse for
string normalized facts. Original-node data must agree with the scalar facade.

## Document shape and authority

A successful simple-root assessment has one original root element fact. Reuse
HasRootScalar/ReadRootScalar and the original-node empty element path. Root values
stay separate from ScalarCount/leaf indexes: zero descendant scalar leaves, no
root duplicated at leaf index zero. Root and descendant attribute counts stay zero.
Existing complex simple-content, empty-complex typed sequence and element-only
FOTY0012 behavior remain distinct and unchanged.

Reject child elements as Invalid; unknown expanded roots are Invalid within the
closed schema. Invalid or Unsupported assessment publishes no partial original
facts and releases its admission. Export keeps the original lexical XML within the
current serializer contract, carries no authority, and reopening requires new
assessment/origin. Detached scalar lifetime, foreign-origin/component rejection,
one-open-document policy, sequential operations and disposal guards remain intact.

## Compilation routes, limits and qualification gates

Both flat and nested root compilers need an explicit simple-root route. A simple
root must work in a catalog containing an unrelated nested complex root, regardless
of which declaration is first. Eager compilation of unused declarations, combined
anonymous/named budgets, per-schema pattern states and per-assessment work limits
remain enforced. No new public API, Core dependency direction or pin is planned.

Pinned Core source review confirms original-node data requires exact node ownership,
assessment origin and issued typed facts; element roots have no special exception.
Source review alone does not prove a Schema root can satisfy those checks. No private
helper invocation or test substitute may establish acceptance.

Run all 38 planned payloads through both facades, and the 17 actual-native gates on
signed Core 1.81.2 and 1.81.17. Add exact limit/one-over controls, cancellation and
recovery, eager unused types, selected-type identity, mixed catalogs, export/reopen,
foreign authority and disposal. Probe original SKU unchanged as Unsupported and the
separate ASCII example independently. Capture failures and iterations without
rewriting earlier evidence.

Only after feasibility: record separate production adoption before semantic edits;
preserve all 3,050 source identities or explicitly reconcile authored admissions;
run full source, package-only, retained binaries, public API, README example and both
frozen/expanded corpus gates. No full-suite percentage, publication, PSVI/C45 or
performance acceptance follows from these selected cases.
