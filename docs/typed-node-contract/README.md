# Native original-node atomization proposal

20 November 2026. Base: merged PR31
`43f14dbf56c36e5185e705af901678c42c8d1aff`, identical to reviewed
`21939a1c6696eb8d5da035c365410f0cb374270f`.
This dossier is **proposed contract work**, not implementation, semantic adoption,
API acceptance or new qualification of native node queries.

Main CI44774979540 passes 1,482 source tests (427 implementation / 1,055 public),
release controls, six workload controls and documentation hygiene. The frozen
pilot remains 9 matches / 12 Unsupported / zero mismatches/errors. None of its
remaining12cases is a test of this proposed query integration.

## Finding and recommendation

The existing native facade creates an owned `$value` scalar Core session.
Its advisor accepts the typed-identity environment and refuses original-node
data binding. Released Core1.81.2/1.81.17 already expose a **bounded original-node
data profile**, accepting `data($node)` or `fn:data($node)` with authenticated
original element/attribute facts. The immutable source/compiled-doc review is
recorded in [CORE-REVIEW.json](CORE-REVIEW.json); existing package signatures and
synthetic transport evidence remain separate from this source review.

That public API does not establish arbitrary schema-aware XPath navigation,
predicates or arithmetic over the native tree. A guard relaxation alone would
also leave callers without a public, owned native node/episode selection route.
Recommend a separate original-node **atomization** facade first, with exact
node selection, a single original assessment and detached scalar results.
Keep general schema-aware XPath as later joint Core/Schema acceptance work.

[CONTRACT.md](CONTRACT.md) defines proposed ownership, target paths, bounds,
results and refusal behavior. [API-PROPOSAL.md](API-PROPOSAL.md) makes the additive
surface concrete; names and API acceptance are still proposed.
[QUALIFICATION.md](QUALIFICATION.md) distinguishes required actual-native proof
from the earlier synthetic90-control Core lanes.
[ADR-016](../ADR-016-owned-original-node-atomization.md) remains Proposed.

No existing scalar/decimal session guard, runtime, public API, Core pin, XSD
grammar, published package, tag/controller or upstream fixture was changed.
The legacy XPath `data(.)` null-QName limitation remains as previously recorded;
no fake primitive type QName or implicit assessment transfer is proposed.

Next: run an isolated native before-code feasibility candidate on both signed
public Core lanes, then record exact semantic adoption and review the concrete
API before production implementation. Passing that candidate would not by
itself approve C45, production SLAs or a publication.
