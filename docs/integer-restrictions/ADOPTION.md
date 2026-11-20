# Bounded direct integer restriction adoption

20 November 2026. Recorded before production edits under the owner request to
continue the recommended slice. Base: merged PR30
`5bfbb1e798cdf18bac6ac7bb15705b1a0e50bc65`; main CI44762728337 passes
1,373 source cases (421 implementation / 952 public), release/workload controls
and documentation. Original proposal and earlier dossiers remain unchanged.

## Immutable normative basis

Adopt bounded Structures 20120405 3.3.2–3.3.4 and 3.16.1–3.16.4 for direct
named/anonymous atomic restrictions, original element/attribute facts and type
identity; SHA256 `5ffb773f81fb81cfc9ac54dfb871c333cefb12fbdffe4caeb28170054b21fc6a`.
Adopt Datatypes 20120405 3.4.13.1–3 integer lexical/fixed facets and
4.3.6–4.3.12 validation/component constraints; SHA256
`cc2ae20cdeab02600c73fd13f1cc31dbbdd61f62aa455026fa84388b540443b8`.
Exact source URLs/bodies are pinned in the
[original review receipt](../conformance-pilot/SPECIFICATION-REVIEW.json).
Reuse existing XDM 3.1 20170321 2.7/3.3/6.2–6.3 original identity/transport
adoption, SHA256 `a157e098f0de2fefd81ce95bb934762ff5a1f616a4745854f0340d177082acd3`.
Review pins, historical semantic adoptions, feature-origin commits and upstream
fixture revisions stay separate. No moving/latest text or later errata is adopted.

## Adopted grammar and exact semantics

Only NativeSchemaSession admits direct restriction(base QName resolving to
xs:integer) through current global named or anonymous local-element simpleType
routes. Reuse primitive integer lexical mapping: XML whitespace collapse,
optional sign and one or more ASCII digits, no decimal point/exponent. Store
BigInteger throughout; never use .NET decimal or a decimal cast for facets.

Admit zero/one each of minInclusive, minExclusive, maxInclusive, maxExclusive,
totalDigits and fractionDigits, with value only and no element content beyond
the current schema parser's whitespace/comment/PI rules. Empty restriction is
allowed. Range facet lexical forms must be valid integer lexical forms in the
primitive base and fit the configured native supplied-digit bound. Reject
fractional or exponent spellings, including `1.0`, as Unsupported/ValueType.

Range comparisons use exact BigInteger ordering. Reject duplicate facets as
DeclarationShape; mutually competing lower/upper bounds and inconsistent
component values as ValueType. Require lower<=upper for inclusive/inclusive
or exclusive/exclusive endpoints; require lower<upper for mixed endpoints.
Equal exclusive endpoints and adjacent exclusive integer endpoints may define
an empty value space: compile succeeds, every present instance is Invalid.
Do not reject other empty intersections merely for emptiness.

totalDigits accepts signed integer lexical forms after collapse, value1..256.
Zero/negative/malformed =>ValueType; positive value>256 =>RepresentationCapacity.
Count decimal digits of abs(normalized integer), with zero count1; input signs
and leading zeroes do not increase value-space totalDigits. They still count
against supplied representation capacities before normalization/facet checks.
fractionDigits inherits fixed zero whether omitted or explicit: +0/-0/leading
zero lexical spellings are admitted, every nonzero value =>ValueType. Do not
silently accept positive fractionDigits or add fractional integer values.
Inherited whiteSpace=collapse remains fixed; explicit whiteSpace facets and
fixed/id/annotation attributes remain outside this bounded grammar.

Facet raw text and instance integer text each retain the existing256UTF16lexical
ceiling before collapse; native digit ceilings precede instance facet validity.
Digit-facet parameter parsing is bounded at256supplied digits, independent of
the configured value digit limit. A supported integer violating facets is
Invalid with no document/partial facts; out-of-capacity input is Unsupported.
Keep compile-time component refusal distinct from instance validity/stage codes.

## Identity, ownership and limits

Named restrictions retain original expanded type names. Anonymous source sites
retain absent type QName and original distinct/shared source identity. Governing,
declared and actual atomic type facts stay bound to original nodes and owner;
primitive integer storage does not authorize primitive handle substitution.
Named integer attribute type references are admitted through existing root and
descendant application-attribute routes and MUST enforce the same facets.
Do not discard facets for attribute uses. Anonymous attribute/root scalar types
remain excluded, and simpleContent is still direct decimal only.

Preserve global16/local-anonymous16/root16/depth4/terminal16/node64 and all
attribute, input, read/cache/export/query capacities. Compile unused definitions
eagerly. Keep integer assertion references excluded, decimal/string behavior,
legacy NativeDecimalSession refusal, hint/type/nil policy and scalar-only native
query binding unchanged. No enumeration/pattern/list/union, base chains, built-in
derived integer families, general derivation/type overrides, API/Core pin,
package/tag/controller/publication or C45/full-XSD/PSVI acceptance expansion.

## Anticipated transitions and qualification

Pin before edits all original rows in ANTICIPATED-DELTAS.json: AS-034 (anonymous
empty restriction), M2B-54 (named integer base with unchanged `12.5000` instance,
which must become Invalid), M2C3-67 (unused named integer definition, existing
integer instance becomes Valid). Preserve original schema/XML and replace only
unreachable expectations explicitly. Original sealed receipts remain historical.
Two frozen integer maxInclusive cases are targets only, not executed admissions.

Run before-code native refusals plus built-in/decimal/string controls, then actual
native facet/identity/attribute/authority tests, full source regression, signed
minimum/current-Core package consumers, retained callers, exact API comparison
and unchanged21case/42file upstream pilot. Include >decimal-precision values,
all facet boundaries, empty spaces, fixed zero, invalid lexical/duplicate/conflict,
capacities and cancellation/disposal recovery. Keep overlapping counts separate.
No commit/push/publication without the applicable explicit owner instruction.
