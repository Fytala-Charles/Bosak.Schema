# Bounded simple-root production candidate

22 November 2026, base `1f2cca34ce8dc93b25f11b402bbe60330b8ee1cf`.
[Production adoption](PRODUCTION-ADOPTION.json) was recorded before implementation.
The separately sealed [native feasibility](FEASIBILITY.md) remains historical evidence.
This candidate is locally qualified and uncommitted.

## Resulting behavior

The opt-in scalar and original-node facades admit global decimal, integer, boolean
and string roots, plus existing direct named/anonymous decimal/integer/string
restrictions. Both flat and nested compiler routes support mixed simple/complex
root catalogs, selected by expanded root name. The legacy decimal-only facade
retains its existing grammar; boolean restrictions and broader datatype/type-chain
features remain excluded.

Actual original root declarations, governing types and atomic provenance are retained.
String whitespace is preserved and empty String is one atomic. Numeric/boolean
normalization keeps existing collapse semantics. Root values use HasRootScalar and
ReadRootScalar, or the original-node empty element path. They do not enter descendant
scalar indexes: a simple-root document has zero scalar leaves and no application
attributes. Namespace/type selection, cancellation, shared pattern work, combined
anonymous/global quotas, original ownership and lifetime guards retain their scope.

Four source files changed: the three qualified compiler/assessment overlays plus
public root-read XML documentation. No public API, Core pin or dependency change.
New tests cover the original 38 proposal payloads through both facades and 59 native
identity/boundary controls. Two proposed boolean restriction placements are explicitly
Unsupported, preserving the existing profile rather than adding a restriction family.

## Qualification

| Lane | Passed | Scope |
|---|---:|---|
| Source implementation | 1,007 | Adds 59 native controls |
| Source public consumers | 2,178 | Adds 76 proposal/facade cases |
| Package-only Core 1.81.2 | 2,178 | No source project or friend access |
| Package-only Core 1.81.17 | 2,178 | Copied loaded-version assertion selects current pin |
| Current-package native | 59 | Original facts, root identity and capacity controls |
| Retained callers | 124 / 23 / 33 | Only Schema.dll replaced |

These overlapping lanes are not additive coverage. All **3,050 prior source case
identities** remain, with 135 new cases, making 3,185. One authored expectation is
reconciled explicitly: AS-050 now compiles its original anonymous decimal root,
assesses `<r>1</r>` as Valid, and returns root decimal 1 with zero descendant leaves.
Schema, instance, ID and other expectations remain unchanged; only compile status
and descriptive label change. [Exact admission](ADMISSIONS.json),
[stable identities](implementation-evidence/STABLE-CASES.json).

Release build has zero warnings/errors. API remains 54 types / 401 rows with zero delta.
All 18 signed Core archive/source/cache/runtime identities and the signed published
beta.1 baseline verify. Source/package/guarded runner DLLs agree. The packaged README
matches source, and its exact simple-root XML example passes both facade root reads,
export equality and two whole-literal rejection controls. The initial current-Core
lane retained the minimum loaded-version expectation; its failing receipt is kept,
followed by the corrected 1.81.17 assertion and full passing run. No product defect
was found during this qualification.

The unsigned local `Bosak.Schema.0.6.0-alpha.0.31.nupkg` SHA-256 is
`a8e7b70c7fc8f4e56aeb11fd6efe30998d46dfa7a182327a4ff081c5029f9fcd`.
[Results](implementation-evidence/RESULTS.json), [identity](implementation-evidence/IDENTITY-FINAL.json),
[API](implementation-evidence/SURFACE.json). No registry publication occurred.

## Original corpus effects

The frozen pilot remains 21 matches / 0 Unsupported / zero errors; all 42 original files and
expectations remain unchanged, with no row changes. The unchanged runner's 15 integrity
controls pass. [Frozen delta](implementation-evidence/PILOT-DELTA.json).

Both expanded cohorts measure **3 matches / 126 Unsupported / zero mismatches or
harness errors** on both Core pins, across 129 original cases and 274 unchanged files.
No classification changes occur. Eight rows change only their first detected
Unsupported reason as compilation reaches different excluded features: six change
DeclarationShape to ValueType and two change ValueType to Attributes. Exact before/
after rows are preserved in [expanded delta](implementation-evidence/EXPANDED-DELTA.json).
A first refusal is not a complete list of co-blockers. Historical reports are not
rewritten, and this sample provides no whole-specification coverage percentage.

## Remaining work and delivery

Boolean restrictions, new type chains, defaults/fixed/nil, list/union, broader regex,
new datatype families and schema assembly remain separate work. The original SKU
still uses excluded `\d`; the packaged ASCII example is a separate authored schema.
Component closure remains Unsupported; no full XSD, PSVI/C45 or performance acceptance.

Next: review/deliver on an explicit Git request, verify merge/main CI, then prepare
the next ranked derived-integer built-in family contract with fixed facets and
original type provenance. Published beta.1, Core pins, release controllers and earlier
sealed evidence remain unchanged. No commit, push, tag or publication in this step.
