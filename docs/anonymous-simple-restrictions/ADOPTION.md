# Anonymous local decimal/string restriction adoption

20 November 2026. Recorded before production edits under the owner request to
continue after PR29 delivery. Verified base/main
`1438f719863d0c13f976f1b0eb683e146a3511e5` is the exact reviewed PR29 tree;
main CI44752564556 passes 1,302 source cases (415 implementation / 887 public),
release refusal controls, six workload controls and documentation hygiene.
This records the next bounded semantics and qualification obligations; it does
not claim implementation, feasibility execution or new upstream matches.

## Immutable normative basis

Reuse the exact dated Recommendation bodies and SHA256 in
[SPECIFICATION-REVIEW.json](../conformance-pilot/SPECIFICATION-REVIEW.json):

| Source | SHA256 | Affected clauses |
|---|---|---|
| XSD 1.1 Structures, 5 April 2012 | `5ffb773f81fb81cfc9ac54dfb871c333cefb12fbdffe4caeb28170054b21fc6a` | 3.3.2–3.3.4 local declarations; 3.16.1–3.16.4, especially 3.16.2.1–2 anonymous atomic type mappings |
| XSD 1.1 Datatypes, 5 April 2012 | `cc2ae20cdeab02600c73fd13f1cc31dbbdd61f62aa455026fa84388b540443b8` | 3.3.1/3.3.3 string/decimal; 4.1–4.2 restriction; 4.3.1–4.3.3 length facets; 4.3.5 enumeration; 4.3.6 whitespace; 4.3.7–4.3.12 numeric bounds/digits |

Reuse the exact existing XDM 3.1 20170321 adoption (2.7, 3.3, 6.2–6.3) for
original typed facts and value transport; its body SHA256 is
`a157e098f0de2fefd81ce95bb934762ff5a1f616a4745854f0340d177082acd3`,
recorded in [the earlier adoption](../nested-simple-content/ADOPTION.md).
New work must verify that anonymous simple-type identity transports correctly;
do not infer that from anonymous complex-root or named restriction support.
The pilot's upstream revision, review pins and earlier semantic receipts remain
distinct from this adoption. No moving latest text or later errata is adopted.

## Adopted bounded mapping

Only opt-in NativeSchemaSession: a local element particle in an already admitted
flat or nested sequence may omit `type` and contain exactly one anonymous
`xs:simpleType`, containing exactly one `xs:restriction` with base resolving at
the restriction's actual namespace context to primitive xs:decimal or xs:string.
Do not admit simultaneous named `type` and inline content. Do not accept a name
on the local simpleType. Existing form/occurrence/root/sequence rules apply.

Reuse the existing direct decimal/string facet compilers, not copies with subtly
different arithmetic or lexical rules. Decimal admits the existing four bounds,
totalDigits and fractionDigits; string admits existing Unicode-scalar length,
minLength, maxLength and exact enumeration. Empty restriction is allowed.
Preserve all existing facet duplication/intersection/component constraints,
fixed decimal whitespace collapse, string whitespace preservation, Unicode and
representation-before-facet rules. Excluded facets/attributes remain excluded.

Each source definition creates one original anonymous type identity. Repeated
occurrences and references to a shared enclosing named complex type reuse that
identity; different source definitions with equal facets remain different.
The anonymous type has no QName and no invented target namespace/name, does not
enter global named lookup, and is not replaced by its primitive base handle.
Its element declaration and original governing/declared/actual scalar type facts
must remain correctly bound to the same original nodes and assessment. Numeric
or string storage kind does not authorize primitive-type identity substitution.

Keep the existing 16 global simple+complex definitions, 16 roots, depth4,
16 potential scalar terminals, 64 defensive nodes and application-attribute
budgets. Extend the existing 16 local anonymous complex-definition budget to
count local anonymous simple and complex definitions together, once per source
definition even if a named enclosing type is reused. Existing anonymous root
complex-type treatment is unchanged. Enforce limits eagerly for unused named
definitions and optional particles; do not publish partial facts on refusal.

Decimal-only assertion admission remains decimal-only; anonymous decimal leaves
follow the existing allowed assertion names, string leaves remain excluded and
nested schemas remain assertion-free. Preserve special-attribute hint policy,
legacy NativeDecimalSession refusal, all lifetime/cache/ownership/cancellation
rules, unchanged root/scalar/attribute indexes and original lexical exports.

## Intentional exclusions and expected changes

No global/root scalar simple types, anonymous attribute types, simpleContent
restriction of complex bases, user-derived base chains, inline base definitions,
integer/Boolean restriction, lists/unions, pattern/regex, explicit whiteSpace,
fixed/default, annotations, composition, substitution, wildcard, nil or type
override support. No public API, Core pin, named lookup, general derivation,
package/version/tag/controller or C45/full-XSD acceptance change.

[ANTICIPATED-DELTAS.json](ANTICIPATED-DELTAS.json) pins NS-051's original row.
Its current compile Unsupported is intentionally targeted for admission; the old
unreachable seven-value placeholder is not a valid instance expectation. Its
unchanged instance should yield exactly one scalar `12.5` if implementation and
original-type transport qualify. Preserve the historical original row receipt;
make any later test expectation correction explicit and separate.

Six unchanged pilot cases (four decimal, two string) are eligibility targets,
not measured admissions. The two anonymous integer cases remain excluded, as do
all other independent namespace/assertion/complex restriction/xsi:type blockers.
Do not change fixture bytes, expected validity, case selection or sealed receipts.

## Qualification gate

Follow [QUALIFICATION.md](QUALIFICATION.md). First prove anonymous simple-type
transport through the signed minimum/current public Core lanes; then implement
and qualify actual Schema facts, not only synthetic assessors. A failure to
transport identity must be resolved explicitly, never by relabelling the type
as its primitive base or making a mandatory Core dependency change silently.
