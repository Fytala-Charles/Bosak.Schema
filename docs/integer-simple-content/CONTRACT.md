# Proposed integer complex simple-content contract

21 November 2026. Proposal only: not semantic adoption or runtime qualification.
Product base is the exact PR36 merge. Review sources are the immutable 5 April
2012 XSD 1.1 Recommendations, recorded with SHA-256 and section pins in
[SPECIFICATION-REVIEW.json](SPECIFICATION-REVIEW.json).

## Scope and identity

Admit extension of primitive xs:integer or an existing named direct integer simple
restriction, then one direct complex simple-content restriction of that named
extension base. Support named/anonymous root and nested terminal owners already
admitted by the mixed scalar and original-node facades. Keep both complex types
attribute-free and assertion-free. Existing decimal extension attribute behavior
and all prior scalar-family routes retain their semantics.

Extension G uses the actual original simple type C0 as content and base. Integer
C0 remains an integer component with decimal ancestry, never an xs:decimal alias.
Restriction G instead has complex base B and a distinct anonymous integer content
C1 whose actual base is B.content C0. Preserve both G->B and C1->C0, no-facet C1,
per-compiled-context identity and namespace/context without inventing a QName.
Declaration D and declared T remain original; facts and governing/declared atomic
provenance use G, actual atomic provenance uses original C0 or anonymous C1.
Register immutable owned components once and retain existing quota accounting.
No public API or Core behavior changes. Complex xsi:type substitution stays
Unsupported after recognizing true ancestry; weakening/unrelated types stay Invalid.

## Exact integer semantics and static facets

Use BigInteger throughout parsing, bounds, scalar reads, native atomization and
identity queries. Never convert through decimal, double or Int64. Collapse XML
whitespace before the existing integer grammar; signs, leading zeros and negative
zero are legal. A decimal point, exponent, embedded space or empty value is Invalid.
Detached scalar canonical spelling may differ from original exported XML. Count
raw digits before zero normalization using existing capacities; native, query and
export budgets remain separate. No unbounded-number claim.

Admit min/max inclusive/exclusive, totalDigits and fractionDigits=0. Integer fixes
fractionDigits=0 and whiteSpace=collapse (Datatypes3.4.13.3). Explicit whiteSpace,
fixed attributes, pattern/enumeration/list/union and inline simpleType remain out
of scope. totalDigits is positive, cannot weaken inherited limits, and counts
value digits without sign or insignificant leading zeros; zero needs one digit.

A new complex restriction needs static inherited facet checks, not just a runtime
intersection. Apply all sixteen new/base bound conditions and four same-type
conditions in FACET-RULES.json, using exact integer comparisons, plus duplicate/
conflicting-kind and digit rules. Do not replace exclusive bounds with +/-1 for
schema validity. Equal inclusive endpoints may define a singleton; equal exclusive
endpoints may define a valid empty type. Mixed equality is schema-invalid.
Even minExclusive=0/maxExclusive=1 defines a legal schema with no integer members;
instance invalidity and schema invalidity must remain separate.

New recognized restriction facets/base-reference errors use schema-Invalid;
excluded grammar/capacity remain Unsupported with existing reasons. Do not globally
change legacy simple-integer facet or existing extension reference outcomes.
The source review identifies SC-053: integer extension with a required string
attribute. It remains outside this attribute-free slice; a possible ValueType to
Attributes diagnostic change must be measured and explicitly reconciled before
production adoption. Do not silently admit its schema or change its instance.
Other affected authored expectations require individual evidence, never broad edits.

## Deferred work and evidence gates

No complex restriction chains, extension from complex bases, derived built-in
integer families (xs:int/long/nonNegativeInteger), inherited application attributes,
assertions, block/final/abstract, composition or general inheritance. Assert006
contains base membership/range and derived mod assertions: full validity needs
both assertions with real context-item typed semantics and conjunction. The
separate namespace-uri nested assertion fixture has independent requirements.
Do not claim that this integer slice yields any new frozen pilot matches.

First record experiment-only adoption against the exact pinned normative bodies,
then copy source into an isolated native probe. Prove integer atomic kind and exact
D/T/G/B/C0/C1 ownership, signed minimum/current Core behavior, all authored payloads
and bound relations, limits/cancellation/no partial publication, original versus
foreign nodes, root/nested/anonymous/no-namespace routes and typed-identity profile.
Keep data-profile ReadTypedValue refusal separate; no fake handles or authority.
Preserve failures and exact candidate/runtime hashes. Only after feasibility passes,
record separate production adoption before product source/test expectation edits.

Production qualification must preserve all 2,319 stable source cases (with explicit
admission receipts for measured changes), full source, package-only consumers on
both Core pins, retained124/23/33, API54types/401rows zero delta, signed public
archives/source/cache/runtime and beta.1 baseline identity checks. Run the unchanged
guarded 21-case pilot and record measured classifications rather than a forecast.
Proposed payloads and static XML lint are not executed product or conformance tests.
