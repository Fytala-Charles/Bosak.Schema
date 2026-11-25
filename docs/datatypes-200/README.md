# Native datatype composition batch

Next 200 owner-authorized contracts, DT-101 through DT-300. [Ledger](CONTRACTS.json) tracks implementation/qualification separately; these are behavior and integration contracts, not a count of new XSD features. [Immutable adoption](ADOPTION.json) precedes semantic implementation.

Priority: binary and QName primitives, then native lists/unions, their member provenance, value constraints, original-node reads and bounded transport. Core1.86.0 remains the signed baseline. Published Schema beta.1 and previous sealed packets remain historical. No new registry publication is part of steps1-5.

## Qualified atomic checkpoint

The 0.8 development candidate qualifies DT-101–DT-180: bounded hexBinary/base64Binary/QName values through both native facades, finite facets, original namespace contexts, actual owned Core provenance and detached immutable reads. All 11,848 source tests and 10,308 overlapping fresh package-only tests pass; all nine Core1.86.0 signed archives/DLLs match publication. API review preserves 55 exported type names and all existing members, adding seven reflection rows. The frozen pilot remains 21 matches; expanded samples now have20matches/109Unsupported/zero errors. Lists/unions (DT-181–DT-300) remain planned. No Schema publication, public query factory or full-XSD/C45 acceptance. [Consumer guide](GUIDE.md), [qualification](atomic/QUALIFICATION.json), [review](atomic/REVIEW.json), [API review](atomic/API-REVIEW.json).

The 177 lexical/placement/edge stimuli execute through both facades and two explicit timezone contexts (708 checks). Supplementary parser, authority, import, restriction and lifetime tests bring new source coverage to824cases. Package coverage overlaps source public tests; do not add the totals. The original attribute-owner fixture failures and their corrections remain in the source-only evidence ZIP. Original upstream stimuli and all previous calendar evidence are unchanged.

The remaining120contracts follow the pre-implementation [composition decisions](COMPOSITION-DECISIONS.md). Publication and a complete specification-conformance claim remain separate decisions. Verify the atomic evidence offline with `python docs/datatypes-200/atomic/validate.py`.
