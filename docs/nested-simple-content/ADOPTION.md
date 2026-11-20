# Nested decimal simple-content adoption

20 November2026. Recorded BEFORE production edits under the user request to
continue the merge/verification and next nested simple-content recommendation.
Product base32f8214c670a84286bd28e0a3d51e1d5ffb4f1f7 is merged PR27; fresh main
CI44726412611 passed1,200source/54release/6workload and documentation checks.
This adoption selects bounded semantics, not shipped support, official W3C-suite
conformance or general API/C45 acceptance.

## Immutable normative basis

[Exact dated bodies and SHA256](SPECIFICATION-REVIEW.json) select XSD1.1 Structures
20120405 (5ffb773f81fb81cfc9ac54dfb871c333cefb12fbdffe4caeb28170054b21fc6a),
Datatypes20120405 (cc2ae20cdeab02600c73fd13f1cc31dbbdd61f62aa455026fa84388b540443b8),
XML1.0Fifth20081126 (f58a4b9e1e5b8bac13fb55c4e5c7c8c5e2c176aa45ee68e2f1ab2498511927ec)
and XDM3.1dated20170321 (a157e098f0de2fefd81ce95bb934762ff5a1f616a4745854f0340d177082acd3).
Adopt bounded Structures3.3.2-.4/3.4.1-.4/3.5 for local declarations, direct
simple-content extension and original attributes; existing3.8-.9/4.2 govern
sequence/occurrence/component context. Datatypes decimal3.3.3/whiteSpace4.3.6
and existing named-decimal facets, XML2.11/3.3.3/4.1 and XDM2.3-.4/2.7/
3.3.1.2/6.2-.3 govern original identity and normalized/typed node values.
Review pins remain distinct from adoption; no moving latest text/later errata
is implicitly adopted. Existing datatype/facet/namespace adoptions are reused,
not broadened or replaced. Original sealed receipts remain unchanged.

## Adopted bounded behavior and intentional deviations

Adopt[CONTRACT.md](CONTRACT.md): descendant named/anonymous complex types with
one simpleContent/extension directly based on xs:decimal or an admitted named
decimal restriction and current local attributes. Keep assertions absent in
nested schemas. No complex-base chain/restriction/other family/list/union/mixed/
composition/default/fixed/nil/global-reference/wildcard expansion. Primitive
simple-type leaf attributes and NativeDecimalSession remain unchanged.

Preserve component limits, depth4,16potential terminals,64defensive elements,
16attribute uses/owner and64total potential uses includingroot/optionals/repetition.
Each complex simple terminal counts1node/1scalar plus all attribute uses.
Admit three-segment descendant owner paths for depth4scalar-bearing complex
owners, deliberately extending the previous element-only-owner bound of two.
This adds no structural depth, public API, option or authority/query interface.

Retain original complex governing/declared type and local declaration; actual
atomic type is the decimal content type. IsComplexType=true does not imply
HasNoTypedValue. Element-only ancestors retain FOTY0012. ScalarCount/ReadScalar
include descendant scalar-bearing terminals in document order, root separate.
Existing valid document indexes and all root/attribute read semantics remain.
Normalize complex terminal node string-value by schema whitespace collapse;
exact scalar values and parsed lexical XML export remain distinct.

Assess original owner attributes before decimal content. Nested child elements
are Invalid, comments/PIs Unsupported, CDATA text; raw256UTF16and supplied
numeric/facet gates remain. All descendant checks precede single complete fact/
document publication. Preserve original owner/index/path/name/node checks and
existing foreign/lifetime/cache/query/cancellation/disposal/export/quota recovery.
No stale facts/authority transfer on export/reopen.

## Intentional baseline changes and remaining proof

Before-code[feasibility](FEASIBILITY.md) passes77synthetic controls per signed
Core lane,18signature/source/payload checks and five actual Schema controls.
[Original case hashes](ANTICIPATED-DELTAS.json) identify NS-062andNA-046.
Both now intentionally compile Valid; their old supplied instances are Invalid:
NS-062has child markup in simple content, NA-046omits a required child. Record
these two exact transitions separately from all unaffected prior cases; old
unreachable Valid instance fields are not validation evidence.

Actual native complex/atomic provenance, root/local declaration identity,
three-segment owner paths, source/package/retained/API/README and documentation
qualification remain required per[QUALIFICATION.md](QUALIFICATION.md). No Core
engine/pin, tag/controller/publication or acceptance expansion is implied.
