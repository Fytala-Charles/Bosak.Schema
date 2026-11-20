# Bounded string assertions

20 November 2026. Implemented locally on merged PR32/main
`2b4a3497232da3cee20be0d4f561a853baa23aa8`; branch
`feature/native-string-assertions`. No commit or push has been requested.

Both owned native facades can assess flat schemas with immediate string-child
equality assertions. `eq`/`ne` use scalar cardinality; `=`/`!=` use existential
pairs. Quoted literals, string presence/count and existing logical/decimal
composition use the adopted namespace and capacity rules. The legacy decimal
facade and public API are preserved.

[Adoption](ADOPTION.md) records the exact normative revisions and intentional
limits before implementation. [Qualification](QUALIFICATION.md) distinguishes
source, package, retained, differential and frozen-pilot evidence. The seven
previous authored refusal probes now admitted are recorded explicitly in
[ADMISSIONS.json](ADMISSIONS.json), with original fixture bytes retained.

All 1,886 source cases pass (520 implementation / 1,366 public), including 271
new cases. Package-only minimum/current Core lanes each pass 1,366; retained
124/23/33 unchanged callers pass. These scopes overlap. All 18 Core signatures,
source pins and runtime payloads verify. The public API stays 54 types / 401 rows.

The unchanged frozen pilot improves from 9 to 10 validity matches, with 11
Unsupported and zero mismatch/harness errors. Only the original
Assert/assert_025/assert_025_2 string equality case changes classification.
This small selected corpus is not a coverage percentage or full-XSD certificate.

Attribute expressions, nested/simple-content assertions, ordering, Boolean/integer
assertion operands, new datatypes and general XPath remain outside this slice.
Published beta.1, Core sources/pins, release controllers and tags are unchanged.
The local unsigned alpha.0.20 is qualification material only. Next: review this
bounded semantic change, then explicitly request commit/push/PR delivery.
