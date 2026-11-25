# Native datatype composition batch

Next 200 owner-authorized contracts, DT-101 through DT-300. [Ledger](CONTRACTS.json) tracks implementation/qualification separately; these are behavior and integration contracts, not a count of new XSD features. [Immutable adoption](ADOPTION.json) precedes semantic implementation.

Priority: binary and QName primitives, then native lists/unions, their member provenance, value constraints, original-node reads and bounded transport. Core1.86.0 remains the signed baseline. Published Schema beta.1 and previous sealed packets remain historical. No new registry publication is part of steps1-5.

## Qualified atomic checkpoint

The 0.8 development candidate qualifies DT-101–DT-180: bounded hexBinary/base64Binary/QName values through both native facades, finite facets, original namespace contexts, actual owned Core provenance and detached immutable reads. All 11,848 source tests and 10,308 overlapping fresh package-only tests pass; all nine Core1.86.0 signed archives/DLLs match publication. API review preserves 55 exported type names and all existing members, adding seven reflection rows. The frozen pilot remains 21 matches; expanded samples now have20matches/109Unsupported/zero errors. Lists/unions (DT-181–DT-300) remain planned. No Schema publication, public query factory or full-XSD/C45 acceptance. [Consumer guide](GUIDE.md), [qualification](atomic/QUALIFICATION.json), [review](atomic/REVIEW.json), [API review](atomic/API-REVIEW.json).

The 177 lexical/placement/edge stimuli execute through both facades and two explicit timezone contexts (708 checks). Supplementary parser, authority, import, restriction and lifetime tests bring new source coverage to824cases. Package coverage overlaps source public tests; do not add the totals. The original attribute-owner fixture failures and their corrections remain in the source-only evidence ZIP. Original upstream stimuli and all previous calendar evidence are unchanged.

The subsequent120contracts follow the pre-implementation [composition decisions](COMPOSITION-DECISIONS.md). Publication and a complete specification-conformance claim remain separate decisions. Verify the atomic evidence offline with `python docs/datatypes-200/atomic/validate.py`.

## Completed composition qualification

All 200 datatype contracts (DT-101–DT-300) are qualified in the 0.9 development candidate: binary/QName values plus bounded lists/unions, original member provenance and six immutable item-read methods. All 12,907 source tests (1,551 internal / 11,356 public) and 11,356 overlapping fresh package-only public tests pass; all nine signed Core 1.86.0 archives/DLLs match publication. API review preserves 55 exported type names and all earlier members, adding six item-read methods in this phase. The frozen pilot remains 21 matches; the expanded 129 cases now have 25 matches / 104 Unsupported / zero errors. Prior atomic/calendar packets and original upstream fixtures remain unchanged. These are behavioral contracts; they do not represent 200 new XSD features or complete specification coverage. No registry publication, public query factory or full-XSD/C45 acceptance. [Guide](GUIDE.md), [composition qualification](composition/QUALIFICATION.json), [review](composition/REVIEW.json), [API review](composition/API-REVIEW.json).

The composition catalog has252stimuli exercised through both facades and two contexts (1,008checks). Eleven native proof checks,12integration smoke cases,18lifetime/capacity/import checks and10complex/fixed/selection checks bring this phase to1,059newsource cases. Across both phases the batch adds1,883source cases. Fresh package-only results overlap source public coverage and must not be added to it.

Four measured historical consumer outcomes changed because formerly excluded lists/unions are now admitted: AS-047, AS-048 and XT-051 through both facades. The original fixture bytes/IDs remain unchanged; separate C# assertions now verify actual list/selected scalar reads. Original failure receipts, strengthened catalog controls and corrections remain in the composition evidence ZIP. The empty-union planning correction and adoption section-label clarification are recorded in [source review](COMPOSITION-SOURCE-REVIEW.md), preserving the original adoption bytes.

Run `python docs/datatypes-200/validate.py` to verify both source-only packets offline. PR72 delivered the atomic80contracts; composition delivery follows the reviewed candidate and passing CI. No package, tag or registry release is part of this delivery.
