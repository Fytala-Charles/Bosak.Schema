# M2C proposal: built-in datatypes and an additive owned scalar facade

19 November 2026. Planning only, not semantic adoption or implementation. Schema
baseline `fbb4c1331b81c8daf3131626399119d4c2b312b9`, merged PR19; Core baseline
`43935c34b7658a0365e3eecbef7815a950ce2d15`. M2B supports named decimal
restrictions and selected facets;534source cases passed on its delivered head.
REQ-009/CoreREQ-142 remain In Progress. No runtime, API, dependency/source-wrapper
pin, controller, package or release change is made by this proposal.

## Practical target and sequence

Define separate adopted slices, each with complete validation and original typed
transport before delivery:

| Slice | Useful increment | Exit example |
|---|---|---|
| M2C1 | Additive owned typed-value contract plus primitive xs:string leaves | Namespaced order with a preserved textual identifier and decimal amount |
| M2C2 | Primitive xs:boolean leaves | Explicit paid flag, including all four legal lexical forms |
| M2C3 | Built-in xs:integer leaves | Exact quantity beyond64-bit range under explicit capacity |
| M2D | Selected string restrictions/facets | Named identifier/currency constraints with independently adopted Unicode/XSD regex semantics |
| M2E | Required/optional attributes | Identifier/currency attributes assessed on the fuller order/invoice profile |

The [proposed final built-in example](order.xsd) and [instance](order.xml) are
authored planning fixtures, not currently accepted inputs. M2C1 uses its string
and decimal declarations; boolean/integer declarations join only after their
own adoption. The existing [orders fixture](../../tests/TestData/Schemas/orders.xsd)
also needs attributes, pattern/anonymous simple types, unbounded occurrences,
simple/mixed content and other structures. These datatype slices alone do not
make that whole fixture supported. No production application acceptance is implied.

## Public API boundary to review first

Current NativeDecimalDocument exposes DecimalCount and ReadDecimal, and its
internal read path projects every present leaf as ExactDecimalValue. It cannot
silently accept heterogeneous children without changing the meaning of those
members. Preserve the existing NativeDecimalSession profile and its retained
binary behavior; new datatypes should remain refused there. Share implementation
internally with explicit profile selection rather than duplicating ownership.

Propose a separate additive owned session/document entry point with an immutable
discriminated detached scalar (string/boolean/integer/exact decimal), present-leaf
count/index in instance order, and detached XML export. Names/signatures are
**not approved** by this plan. Create a concrete surface/ADR proposal before code:
include every member, options, result/diagnostic semantics, null/index/wrong-kind
behavior and literal consumer examples. Avoid a raw object or unrestricted node/
XdmValue escape. Returned values must not carry transferable authority or expose
provider handles. Metadata alone must never claim structural validation.

Explicit options must bound input/string/output text and integer/numeric digits;
retain schema/instance byte/node/depth, admission and sequential-operation limits.
Decide XML-whitespace handling per datatype, UTF-8 bytes versus UTF-16 length versus
Unicode scalar counting, and detached output limits. No hidden time/memory SLA.
Retain cancellation, overlap refusal, disposal/revocation and all-resource cleanup
semantics, with public read paths going through original Core facts/snapshots.
Do not infer new API/ADR acceptance from the old bounded ADR010 acceptance.

## Specification targets before semantic adoption

[Fresh source-body review](SPECIFICATION-REVIEW.json) matches the original dated
Structures/Datatypes SHA256 pins. It is distinct from a new adoption. Before each
slice, record exact clauses, intended deviations, before-code fixtures and source
identities; preserve old adoptions and record later errata review separately.

| Dated5April2012 source | Clauses to review |
|---|---|
| [Datatypes](https://www.w3.org/TR/2012/REC-xmlschema11-2-20120405/) |3.3.1 string,3.3.2 boolean,3.4.13 integer; their lexical/value mappings and inherited whiteSpace/fractionDigits constraints |
| Datatypes4.3.6 and existing decimal clauses | Preserve string whitespace; collapse boolean/integer XML whitespace; keep decimal facet behavior unchanged |
| [Structures](https://www.w3.org/TR/2012/REC-xmlschema11-1-20120405/) |3.3.2-3.3.4,3.4,3.16,4.2: component/QName identity, leaf validation and original type facts |
| Existing XDM3.1 adoption2.7/3.3/6.2 | Actual string/boolean/integer/decimal atomics with declared/actual provenance; complex root still FOTY0012 |

M2C1: preserve XML-normalized text, including empty content and leading/trailing
whitespace. Do not apply decimal collapse or normalize Unicode. XML parser line
ending/entity processing is distinct from datatype whitespace preservation.
Reject nested element content under a simple leaf; keep DTD/entity policy and
other excluded markup boundaries explicit. Additional string facets wait forM2D.

M2C2: after XML-whitespace collapse, true,false,1,0 map to two Boolean values;
case variants/other words/numbers are Invalid when representable. Preserve actual
Boolean type, not a string or numeric surrogate. No Boolean assertion syntax or
casts are implied by parsing these leaves.

M2C3: signed decimal digit lexical forms, no decimal point/exponent, exact integer
value space under an explicit bounded representation. Cover leading zeroes and
negative zero. Use arbitrary-precision integer transport where current published
Core supports it; do not silently coerce to Int64, System.Decimal or double, and
do not claim unbounded resource support. Derived integer families and custom
integer restrictions/facets require subsequent adoption.

## Schema/instance outcome and assertion boundaries

For new entry points only, type QNames resolve at the actual declaration site;
unknown prefixes/types, unsupported simple/complex structures and unused invalid
or excluded definitions refuse complete compilation. Preserve combined16global
types/16roots and finite ordered leaves unless separately adopted. New successful
schemas must not bypass numeric facets on decimal siblings or skip any repeated
leaf. Supported lexical/structural violations => Invalid; explicit representation
limits/excluded features => Unsupported; neither owns a result. Pin reason/order
and public diagnostic redaction before code; argument, cancellation, admission
and lifecycle exceptions remain distinct.

Existing finite assertions remain decimal-only. In the first datatype slices,
refuse assertions referencing nondecimal operands (including unsupported presence/
count routes unless separately adopted), rather than dropping predicates or
coercing types. Existing decimal-only assertions may retain their old behavior.
General XPath/string/Boolean/integer assertion semantics are separate work.
Defaults/fixed/nil, attributes/imports, list/union and nested content remain out.

## Core feasibility and acceptance gates

Current Core source documents checked string/Boolean results and typed identity,
but that is not a qualification of these new Schema routes or minimum published
Core1.81.2. First build caught public-API probes against exact signed1.81.2 and
separate current1.81.17 packages. Verify actual runtime identities, original
facts/typed values and exact output. Any missing neutral capability needs a
minimal reproduction and CoreREQ-142 request before engine work; no new seam,
reflection/friend bypass or pin upgrade is presumed.

The [planned acceptance map](ACCEPTANCE.json) covers positive/invalid/refusal/
capacity/ownership controls. Use authored stable IDs with exact per-case clauses;
select official W3C cases against a freshly fetched suite revision and reviewed
redistribution terms separately. No new support count or whole-spec percentage
comes from this planning artifact.

Before delivery of each implemented slice: full Schema build/tests; all534
existing cases preserved with separately recorded intentional changes; public
package-only consumer on minimum/current Core; retained124/23/33 binaries with
only Schema.dll changed; actual original-node atomization, declared/actual type
identity, foreign/disposed refusal, detached export/reopen and cleanup controls.
Distinguish overlapping catalogs, exact source/package/signature identities and
unsigned local artifacts from releases. Engine changes additionally require
existing Core QT3/pinnedXSLT/qt4 gates. New surface and contract evidence require
review before claiming API acceptance. Packaging remains secondary to this work.
