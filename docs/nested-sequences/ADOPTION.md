# Bounded nested-sequence semantic adoption

20 November 2026. Recorded before production edits under the owner's instruction
to continue the feasibility/adoption recommendation. Product base is Schema
`c34e9fe83f108bbb786c5978cd27816cabe895b4`; production files remain unchanged.
This adoption selects implementation semantics; it is not shipped support, owner
API/C45 acceptance, a W3C-suite admission or whole-section conformance.

## Exact normative sources

Use only the dated W3C Recommendations whose full bodies are freshly verified in
[SPECIFICATION-REVIEW.json](SPECIFICATION-REVIEW.json):

- [XSD 1.1 Structures, 5 April 2012](https://www.w3.org/TR/2012/REC-xmlschema11-1-20120405/),
  SHA256 `5ffb773f81fb81cfc9ac54dfb871c333cefb12fbdffe4caeb28170054b21fc6a`.
  Adopt bounded local declaration/type resolution from 3.3.2-3.3.4 and 4.2,
  element-only complex content from 3.4.2-3.4.4, sequence composition from 3.8
  and finite particle occurrence/matching from 3.9. Unique expanded names within
  a sequence conservatively avoid ambiguous competing local particles.
- [XDM 3.1, 21 March 2017](https://www.w3.org/TR/2017/REC-xpath-datamodel-31-20170321/),
  SHA256 `a157e098f0de2fefd81ce95bb934762ff5a1f616a4745854f0340d177082acd3`.
  Adopt original node identity/document order (2.3-2.4), schema information (2.7)
  and element-node string/typed-value rules (6.2). Element-only containers retain
  complex annotation with no typed value; descendants retain their original
  simple typed facts. Container atomization raises FOTY0012.

The review date and this adoption date are separate fields/records. No moving
latest text or later errata is implicitly adopted. Existing Datatypes/XML/
Namespaces/facet/root-attribute adoptions are reused without changing lexical,
normalization, numeric, facet or namespace semantics. Their original receipts
remain historical; this is not a new datatype or assertion adoption.

## Intentional bounded profile

[CONTRACT.md](CONTRACT.md) defines the selected opt-in shape: finite acyclic named
or anonymous element-only complex sequences, existing terminal scalar types and
root attributes. Local occurrence min0/1 and max1..16, depth4 including root,
16 maximum expanded scalar terminals, 64 maximum expanded element nodes,
16 global types/roots each and 16 local anonymous complex types per schema.
Worst-case expansion includes optional branches and multiplicative occurrences;
reject cycles/excess eagerly, with checked or saturating budget arithmetic.
These limits refuse otherwise legal XSD and are not normative validity rules.

With nonempty sequence declarations, depth4 and at most16 expanded terminals,
the maximal element count is at most49 (root plus three levels of16); the64node
ceiling is therefore defensive and redundant in this slice. Do not claim a
reachable accepted64node fixture or an independent65node-only refusal. Qualify
budget arithmetic and applicable first-refusal precedence honestly.

No choice/all, model groups, global element references, recursion/unbounded,
sequence occurrence attributes, zero maximum, nested attributes/simple content,
mixed content, derivation/composition/nil/default/fixed or new datatypes/facets.
Schemas with new nested containers must be assertion-free, including unused
complex definitions; existing flat assertion semantics remain unchanged.
Continue eager checking all definitions. NativeDecimalSession remains unchanged.

## Fact publication and deterministic traversal

Retain the existing root-selection/root-attribute handling and parser limits.
For each admitted nested container in preorder: refuse unsupported descendant
attributes; check element-only text; refuse unsupported comment/PI nodes;
match local declarations in sequence order; assess each occurrence completely
before the next sibling; check each maximum/minimum and trailing unexpected
children. A present optional container must satisfy all its required descendants.
CDATA is text. In-profile structural/lexical/facet failures are Invalid; profile
and representation exclusions are Unsupported. Record precise mixed-failure
precedence with authored controls; no partial assessment/facts may publish.

Keep complete element facts aligned to original wrapped nodes in preorder.
Separately project only terminal simple-typed facts for ScalarCount/ReadScalar;
old flat indexes and separate root/attribute reads must not change. No new public
API, selector syntax, authority escape, fallback validator or Core pin change.
Existing owner/gate/cache/query/cancellation/disposal/export contracts apply.
Export/reopen must reassess rather than transfer stale authority.

## Before-code evidence and remaining gate

[FEASIBILITY.md](FEASIBILITY.md) records50public-only synthetic controls per signed
Core1.81.2/1.81.17 lane (13new,37prior),18signature/source/runtime checks and the
actual delivered engine's refusal of the authored fixture. Synthetic success is
transport evidence only. Actual native validation, complete-node fact alignment,
leaf-only scalar projection and package/retained regression qualification remain
unimplemented and must follow [QUALIFICATION.md](QUALIFICATION.md).
