# String assertion qualification

20 November 2026. [Adoption before source edits](ADOPTION.md) and
[exact source pins](SPECIFICATION-SOURCES.json).

| Lane | Passed | Scope |
|---|---:|---|
| Source implementation | 520 | Original native facts, invalid no-publication, decimal-only boundary and published-Core differential controls |
| Source public | 1,366 | Existing workflows plus both owned facades for the new assertion grammar |
| Package-only minimum Core 1.81.2 | 1,366 | No project references; independent package cache |
| Package-only Core 1.81.17 | 1,366 | 1,365 shared cases plus explicit current-version assertion; excludes exactly the minimum-version-specific surface assertion |
| Retained callers | 124 / 23 / 33 | Only Schema.dll replaced; all other runtime/caller bytes preserved |
| Frozen pilot integrity controls | 15 | Metadata/hash/expectation and candidate-bound runtime guards |

The scopes overlap and must not be summed as independent conformance coverage.

## Authored controls and expected admissions

125 clause-mapped payloads run through both public facades (250 cases), plus four
culture/recovery cases. Seventeen new internal cases include three successful
primitive/named/anonymous original-fact probes, three invalid no-publication
probes, two decimal-only compatibility probes and nine published Bosak comparisons.
Together these add 271 source cases, taking 1,615 to 1,886.

The first unchanged-catalog run passed all 503 implementation cases but found
seven authored Unsupported expectations now admitted by this slice. Their exact
IDs, original/current fields, source hashes and original fixture bytes are retained
in ADMISSIONS.json and evidence/original. Schema/instance bytes for these seven
probes remain unchanged. Presence/count assertions may now validate; the empty()
probes with present strings correctly make the instance Invalid. Other original
1,608 source cases keep their expectations and passing outcome. All 1,615 stable
original method/case identities remain in the final catalog. No W3C expectations
or fixtures were rewritten. For refused schema cases, subsequent instance
expectations in authored payloads are not executed.

Controls cover empty versus absent values, multi-item cardinality, general !=
existential behavior, literals/doubled quotes/backslashes/XML parsing, Unicode,
case/culture, namespace/defaulting/shadowing, string restrictions, numeric logical
composition, expression/token/depth/string ceilings, cancellation and recovery.
False assertions and cardinality errors admit no document or original facts.

## Published Bosak comparison scope

Nine controls compare effective Boolean results from ordinary published Core
1.81.2 evaluation and native assessment for pure string/literal/general/presence
expressions. Ordinary XML adapter nodes are untyped; these controls establish
agreement for this particular string subset, not general typed-native query
support. Separate actual-native probes verify original assessment/type/provenance
identity for primitive, named and anonymous strings. Current Core is qualified
through the entire package-only public consumer catalog, not an inferred rerun
of the internal differential suite.

## Frozen upstream and compatibility

The unchanged 21-case corpus, 15 groups and 42 file hashes at upstream
7bc3365c652a322f3d762021b3879eb92dae7e30 retain all original W3C notices and metadata.
Exactly Assert/assert_025/assert_025_2 moves Unsupported to match. Results are
10 matches / 11 Unsupported / zero mismatch or harness errors. Other blockers,
including nested namespace-uri assertions and xsi:type, remain visible.

All 18 Core archives verify signatures, source commits and archive/cache/runtime
payload equality in independent caches. The signed published beta.1 baseline is
checked; no Core/Schema project references exist in package consumers. API reflection
preserves all 54 types / 401 rows with zero delta. Earlier five sealed node-data
packets remain byte-identical. The local alpha.0.20 is unsigned and unpublished.

Reproduction projects, raw TRX/signature logs, source identities and candidate
pilot binding belong in evidence. Run the normal solution for source coverage.
Package projects link the public test catalog and fixture files into an isolated
local-feed setup; adjust recorded absolute paths in a new scratch directory when
reproducing. Rebuild and produce new receipts if checkout line endings differ.
No release, broad C45/full-XSD/fullPSVI or production SLA acceptance is implied.
