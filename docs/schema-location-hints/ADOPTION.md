# Explicit-schema location-hint adoption

20 November 2026. Recorded before production edits, following the owner request
to continue the completed conformance pilot recommendation. Product base is
972948c68da872dfd1ca31250608e224c4d20252. The frozen pilot remains historical:
all 21 cases Unsupported, with one instance blocked only by a location hint in
static review. No expected validity or upstream fixture will change.

## Immutable normative basis

Adopt the dated XSD 1.1 Recommendation bodies reviewed in
[the pilot review receipt](../conformance-pilot/SPECIFICATION-REVIEW.json):
Structures 20120405 SHA256
`5ffb773f81fb81cfc9ac54dfb871c333cefb12fbdffe4caeb28170054b21fc6a`,
Datatypes 20120405 SHA256
`cc2ae20cdeab02600c73fd13f1cc31dbbdd61f62aa455026fa84388b540443b8`.
The exact bodies were freshly retrieved and checked before this adoption.
Structures 2.7.3, 3.2.7.3–4, 3.3.4, 3.4.4.2, 4.2.1 and 4.3.2 govern built-in
location attributes, supplied schema selection and optional dereferencing.
Datatypes 3.3.17 defines anyURI; this does not adopt general anyURI typed leaves.
Review dates, upstream fixture pin and historical semantic adoptions stay separate.

## Contract and intentional limits

Only opt-in NativeSchemaSession admits the exact expanded names
`{http://www.w3.org/2001/XMLSchema-instance}schemaLocation` and
`{http://www.w3.org/2001/XMLSchema-instance}noNamespaceSchemaLocation` on any
otherwise admitted root, container, simple-content terminal or scalar leaf.
The supplied schema is authoritative. Never dereference, resolve, merge, interpret
or use hints to change schema selection. Retain original parsed values and nodes
for export, without adding entries to application attribute counts or indexes.
Hint strings are not subjected to .NET Uri absolute-URI validation: XSD 1.1 anyURI
does not impose that lexical restriction. No namespace/location pairing is
consumed or inferred. Existing XML well-formedness and input size limits apply.

This is an explicitly supplied-schema policy, not general schema discovery or
full built-in attribute PSVI support. Built-in hint typed facts are deliberately
not added; only existing application declarations supply public typed attribute
reads. No hints are stripped, normalized in-place, promoted into declaration
facts or allowed to replace original component identity. Retained XML preserves
the parser's ordinary XML attribute normalization, not byte-identical serialization.

Legacy NativeDecimalSession/internal decimal-only compilation keeps all previous
hint refusals. All other xsi attributes, including type and nil even if lexically
valid, remain Unsupported/Attributes. Same local names in other namespaces and
undeclared application attributes retain current refusal/invalid behavior.
Required attributes, scalar/facet/structure validation, cancellation, disposal,
capacity limits, entity/DTD refusal and export ownership remain unchanged.
No API, Core pin, package/version/tag/controller or public conformance gate change.

## Qualification before completion

Exercise both hints with alternate prefixes, root/leaf/nested/simple-content
owners, coexisting required application attributes, hostile/unreachable locations,
empty/relative/whitespace values, retained export and unchanged typed indexes.
Check invalid scalar/required-attribute cases and xsi:type/nil/unknown requests,
wrong-namespace lookalikes, legacy refusal, cancellation and recovery.
Rerun the exact frozen 21 cases against an explicitly hash-qualified candidate;
preserve baseline receipts and report any transition separately. No counterfactual
fixture edits or expectation changes. All source tests and documentation checks
must pass; no claim of package/binary consumer requalification without execution.
