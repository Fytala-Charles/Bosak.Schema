# Proposed derived integer semantics

## Exact normative contract

The [2012 XSD review pins](SPECIFICATION-REVIEW.json) distinguish datatype definition
from implementation adoption. Datatypes 3.4.13-25 specifies the integer family and
its built-in derivation graph; 4.1 and 4.3 govern inherited facets and restriction.
Structures 3.3/3.16 governs actual declaration/type selection and validity. XDM schema
facts and typed values retain original type evidence. Immutable body hashes,
affected sections and source revision are recorded before any semantic change.

The following inclusive effective ranges and immediate parents were checked against
each pinned datatype section. Bounds may be narrowed; they are not fixed facets.
All twelve inherit fixed fractionDigits=0 and fixed whiteSpace=collapse, plus the
integer ASCII-digit lexical grammar and optional sign. Canonical spelling is not
required: permitted plus signs, leading zeros and signed zero must retain their
value-space meaning.

| Built-in | Immediate base | Minimum inclusive | Maximum inclusive |
|---|---|---:|---:|
| nonPositiveInteger | integer | Unbounded | 0 |
| negativeInteger | nonPositiveInteger | Unbounded | -1 |
| long | integer | -9223372036854775808 | 9223372036854775807 |
| int | long | -2147483648 | 2147483647 |
| short | int | -32768 | 32767 |
| byte | short | -128 | 127 |
| nonNegativeInteger | integer | 0 | Unbounded |
| unsignedLong | nonNegativeInteger | 0 | 18446744073709551615 |
| unsignedInt | unsignedLong | 0 | 4294967295 |
| unsignedShort | unsignedInt | 0 | 65535 |
| unsignedByte | unsignedShort | 0 | 255 |
| positiveInteger | nonNegativeInteger | 1 | Unbounded |

The sign-unbounded types remain subject to explicit caller digit/lexical limits;
this profile does not claim their entire infinite value spaces. Unsigned types
permit negative spelling only when its value is zero. Positive/negative families
exclude all lexical spellings of zero. Decimal point, exponent, empty/sign-only and
non-ASCII decimal digits are invalid integer lexical forms within admitted capacity.

## Scope and identity

Propose built-in family types in existing global roots, flat/nested leaves, local
attributes, direct named/anonymous simple restrictions and existing integer simple-
content extension/restriction placements. Reuse existing integer assertion grammar
and inheritance. No arbitrary new user-defined chains, enumeration, integer regex
pattern facets, new whitespace syntax, list/union, new scalar family, schema assembly,
particle grammar or general typed query. The finite built-in graph is distinct from
an admission of arbitrary user-type chains. Legacy decimal-only grammar stays fixed.

An xs:int declaration must retain xs:int as its governing original type and original
atomic component; an xs:integer stand-in with matching numeric value is insufficient.
Direct named/anonymous restrictions retain their own governing identities and base
relation. Scalar-bearing complex types retain the distinct complex governing and
simple atomic content components. Declaration target namespaces, anonymity,
original node/source/origin and raw component ownership remain accurate.

Use exact BigInteger payloads and BigIntegerValue, including unsignedLong's maximum
and sign-unbounded values beyond Int64. Do not pass these through Int64/CLR decimal
or infer accepted XSD type identity from a detached number. Inspect descriptive
SchemaTypeName separately from authority-owned original provenance on both signed
Core pins; neither a descriptive label nor TryGetProvenance alone authorizes general
typed execution. Follow the existing scalar and data(original) profiles only. Retain
Unsupported if actual original evidence cannot be issued through public Core APIs.

## Restriction, selection and outcome rules

Apply inherited built-in bounds in addition to existing admitted min/max/totalDigits
facets. Reject weakening admitted bounds and illegal admitted fractionDigits values
as Invalid in the new derived-family route. New enumeration/pattern/whiteSpace syntax
remains Unsupported before facet-value interpretation. Explicit facet `fixed`
attribute syntax remains excluded; inherited built-in fixed semantics do not
authorize new XML facet syntax. In particular, do not reuse
CompileDerived's catch in a way that converts an excluded feature into Invalid.
Record any intentional changes to existing authored expectations separately.

xsi:type follows the actual derivation graph, not value-range inclusion. Integer to
int and int to byte are valid selections with the selected type's bounds enforced.
Int to integer and long to unsignedInt are invalid selections. Nonnegative to
positive is valid only for positive values. The existing decimal-to-integer-family
representation boundary remains Unsupported even for a valid schema derivation.
Registering formerly excluded known types can legitimately change prior unrelated
xsi:type refusals from Unsupported to Invalid; measure and record those admissions.

Caller capacity precedes value/facet checks as before. Parsing counts lexical digits
before normalization; leading zeros consume capacity. Standard built-in bound
metadata must not itself require a caller's small value digit limit to hold all
standard bounds, or consume user source declaration quotas. Internal immutable
built-in graph size is finite; source/schema/node/attribute/anonymous budgets remain
enforced without silently relaxing them.

Invalid or Unsupported assessment publishes no partial facts, releases admission
and leaves fresh attempts usable. Root values remain separate from descendant leaf
indexes. Existing empty-complex vs absent typed values, attribute owner paths,
export/reopen, exact-original ownership, foreign rejection, sequential operations,
cancellation, detached values and disposal behavior stay intact. Public component
closure remains Unsupported; no new closure or compatible-universe acceptance.

## Native gates and original examples

Run all 158 proposed XML payloads through both public native facades and the 18 actual-
native gates on signed Core 1.81.2 and 1.81.17. Measure exact builtin min/max and
one-beyond values, original type graph, root/leaf/attribute/complex provenance,
selected types, lexical and capacity controls, weak/illegal facets, eager unused
components, mixed catalogs and legacy refusal. Source review alone is not a pass.

The [preserved original references](ORIGINAL-EXAMPLES.json) include
int_totalDigits001_1654.i. That example uses an anonymous xs:int
restriction with totalDigits 2. The two original int_enumeration001/002 examples
independently require integer enumeration, which is still excluded. Do not rewrite
the originals or predict sample passes; first probe original input pairs, then
measure the full frozen/expanded reports with exact candidate identity separately.

After feasibility, record separate production adoption before source edits, preserve
all 3,185 source identities or explicitly reconcile authored changes, then qualify
source, both package lanes, retained callers, public API, exact packaged example and
both corpus pins. No tag, publication, full XSD percentage, PSVI/C45 or SLA claim.
