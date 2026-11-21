# Bounded instance type selection adoption

21 November 2026, recorded before production semantic edits. Product base:
`b1759d22761587b2abe507d12aacf3f5b6b33535`. Core minimum 1.81.2 and qualification
1.81.17 remain unchanged. This adopts an internal bounded semantic extension for
NativeSchemaSession and NativeSchemaNodeDataSession; no public API addition.

## Immutable normative sources and affected sections

The exact dated URLs, body SHA-256, sizes and affected sections are recorded in
[SPECIFICATION-REVIEW.json](SPECIFICATION-REVIEW.json). All four retained bodies
were rechecked byte-for-byte before this adoption. Adopt the recorded Structures
REC20120405 sections 2.7.1, 3.2.7.1, 3.3.4.1-3, 3.3.5.3-4, 3.4.6.5, 3.4.7,
3.16.6.3, 3.16.7.3 and 3.17.6.3; Datatypes REC20120405 sections 3.2-3.4,
3.3.18, 4.1.6 and 4.3.6; Namespaces REC20091208 sections 4 and 6.1-2;
and XDM31 REC20170321 sections 2.7, 3.3, 6.2-3 for the bounded mapping below.
This is actual adoption on this date, distinct from prior contract review,
experimental adoption, upstream fixture revision, feature PR or publication.

## Adopted behavior and intentional exclusions

Adopt the bounded rules in [CONTRACT.md](CONTRACT.md): owner-instance QName
resolution with raw 256 UTF-16 ceiling; closed supported-component lookup and
separate known-but-excluded built-in inventory; identity and actual base-component
substitutability; same-family direct string/decimal/integer restrictions with
selected facets; original D/T retained separately from G and atomic C. All facts,
annotations and original episode provenance use the published Core mapping.
Use immutable schema-owned base and name tables constructed before publication;
do not mutate shared type components during assessment.

Unknown, malformed, unbound and unrelated requests are Invalid; known excluded
built-ins and legitimate family/complex representation changes are Unsupported.
Invalid/Unsupported publishes no original facts or snapshot. Resolve type selection
at roots, nested containers, complex scalar terminals and scalar leaves before
publishing or admitting the special request attribute. Preserve existing structural,
attribute, assertion, occurrence, resource, cancellation and ownership gates.
The processed request attribute has no fabricated application-attribute typed fact.

NativeDecimalSession stays decimal-only and refuses these requests. No imports,
include/acquisition, complex inheritance, abstract/block/final, alternatives,
lists/unions, nil or new scalar families. Schema grammar remains eagerly closed.
Known built-in name inventory supplies no component authority. Element-only types'
implicit anyType ancestry grants no new owned anyType handle or valid selection.
Cross-family decimal-to-integer and simple-to-complex substitutions are deferred,
not falsely declared invalid. General XPath, full PSVI, full XSD 1.1, C45 and
production performance acceptance remain separate.

## Qualification and authored admissions

The actual-native feasibility prerequisite passed169 controls per Core lane; see
[FEASIBILITY.md](FEASIBILITY.md), whose sealed historical bytes remain unchanged.
Reconcile exactly AS-029, IR-088 and OtherXsiRequests_RemainUnsupported(type,
owner0/1) from Unsupported to Invalid because QName true resolves to no component.
Preserve their original receipts and identities; record production changes separately.
Add genuine public consumer and original-native provenance controls, full source,
minimum/current package-only, retained124/23/33, zero API-delta and signed-payload
checks. Execute the unchanged guarded frozen corpus against new exact candidate
hashes; do not predict its result. No commit/push/tag/publication is authorized.
