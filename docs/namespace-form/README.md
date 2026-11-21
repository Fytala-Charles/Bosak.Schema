# Bounded namespace/form development candidate

21 November2026. Base ee32ac81c7a704c95647b121d1cde23cfa32dcbd.
[Contract](CONTRACT.md), [immutable source pins](SPECIFICATION-SOURCES.json),
[production adoption](ADOPTION.md), [authored admissions](ADMISSIONS.json).

The mixed owned facades accept bounded nonempty relative target namespaces with
exact parsed string identity, no URI resolution/normalization/acquisition, and
legal attributeFormDefault declarations regardless of attribute-use presence.
Explicit assertion-default URI literals retain the absolute-only policy; symbolic
namespace defaults capture exact names. Reserved/empty/whitespace target refusals,
legacy decimal-only grammar, quotas and original authority remain unchanged.

Actual-native isolated feasibility passes31controls on each signed public Core
pin. Production qualification passes2,143source cases; no full anyURI/IRI,
full-XSD, new assertion/inheritance grammar or package publication is implied.
Original post-xsi-type and earlier proposal/feasibility/implementation packets are
historical and remain sealed. The corrected Structures schema representation
section is3.17.2;3.15.2 in the earlier review targets Annotations.

## Final qualification

- Source: **2,143 passed** (581 implementation / 1,562 public consumer).
- Public package-only: **1,562 passed per Core pin** (1.81.2 / 1.81.17).
- Current-package actual-native facts/provenance:31passed, overlapping source.
- Retained callers:124/23/33passed, replacing only Schema.dll.
- Signed Core archives/source/cache/runtime:18verified; published beta.1 verified.
- Public API:54types/401rows, zero delta.
- Unchanged frozen21-case/42-file corpus: **16matches / 5Unsupported / zero
  mismatch / zero harness errors**. Only the two ElemDecl/typedef00202m instances
  move into matches; the three assert006 cases remain Unsupported, now first
  refusing nested assertions (AssertionGrammar), with further independent gaps.

The source catalog adds119cases (88 public from44payloads,31internal native)
to the prior2,024stable identities. The two authored form-refusal admissions are
NA-050 and SC-067; their original schema/instance bytes and IDs remain. NA-050
now reaches Invalid for a missing required child; SC-067 reaches Unsupported/
Attributes for its undeclared currency. Legacy M1-26 still refuses relative names.

An initial new empty-target fixture included the illegal unused xmlns:t empty
binding, so XML capture returned Invalid before namespace-profile validation.
NF-033 retains its expected Unsupported/Namespace result after removing only that
unused binding; the failed2-case run and original fixture are preserved. Production
code was not changed to accommodate the fixture. Review separately preserved the
exact old legacy/explicit-assertion refusal message and added compatibility checks.

Final local unsigned artifact is0.6.0-alpha.0.25. Intermediate alpha.0.23/24
qualification inputs are superseded; no registry push or version tag is implied.
Evidence scopes overlap, and this small frozen sample is not a full-conformance
percentage. The .NET namespace lexical predicate and explicit absolute-only
assertion namespace literals remain intentional profile boundaries.

## Review and next step

[Final results](evidence/RESULTS.json), [authored admissions](ADMISSIONS.json),
[API comparison](evidence/SURFACE.json), [pilot changes](evidence/PILOT-DELTA.json)
and [exact signed package/runtime identities](evidence/IDENTITY-FINAL.json).
Original proposal and prior implementation seals remain unchanged. New source,
fixtures, project inputs, runtime hashes and failed iteration are retained.

Self-review covers mixed-profile opt-in, URI string identity and call sites,
form validation independent of declarations, original authority, failure paths,
legacy diagnostics and explicit assertion-policy boundaries. No blocking finding
remains. No commit/push/publication is requested for this step. Next: review and
deliver this slice, then specify bounded decimal complex simple-content restriction
with actual inherited facets and complex/atomic/derivation identities. Integer
inherited assertions and empty/nested context assertions remain later slices.
