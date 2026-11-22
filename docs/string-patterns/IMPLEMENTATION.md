# Bounded string-pattern production candidate

22 November 2026, base `677b5628be9b22c993dddb6ff8dce1b268a282db`.
[Exact production adoption](PRODUCTION-ADOPTION.json) precedes implementation;
[native feasibility](FEASIBILITY.md), the contract and earlier assessment remain
separately sealed. This candidate is locally qualified and uncommitted.

## Resulting behavior

The opt-in native Schema facades now enforce the declared XSD string-pattern subset
on existing direct named/anonymous xs:string element and attribute restrictions.
Multiple source patterns at one restriction combine as OR, conjoined with existing
length/enumeration facets. Whole-literal Unicode-scalar matching preserves parsed
whitespace/case, treats caret/dollar as literals, excludes LF/CR for dot, and retains
empty String as one original scalar. New roots, chains and other scalar families
are outside this extension. Legacy NativeDecimalSession retains its profile.

An internal immutable state machine replaces any need for a regex backend. Fresh
compilation/assessment accounting covers unused types and all element/attribute
matches, including scalar-length and inactive-state scans. Cancellation is polled
during parsing, state construction, scanning and closure. Nullable loops terminate
through visited-state closure deduplication; no ambient setting, private Core call
or mandatory Core-to-Schema dependency was introduced.

Bounds remain 8 source patterns, 256 UTF-16 pattern characters, 64 atoms, 8 group depth,
16 class members, explicit repeat bounds 0..16, 128 states/pattern, 1,024 states/schema
and 4,194,304 work units/assessment. Capacity/work refusal is Unsupported, false match
Invalid. No partial original facts/document publish on failure. Source type identity,
authority/lifetime, scalar API and export behavior retain their qualified scope.

The subset supports literals/dot, grouping/alternation, explicit scalar classes/ranges,
XML whitespace and single-character escapes and bounded quantifier forms. Unicode
digit/property/name classes and subtraction remain Unsupported. No full regex/XSD/
PSVI/C45/performance claim; original SKU remains excluded for syntax/root dependencies.

## Qualification

| Lane | Passed | Scope |
|---|---:|---|
| Source implementation | 948 | Adds 48 native/oracle/boundary/provenance controls |
| Source public consumers | 2,102 | Adds 116 proposal/facade cases |
| Package-only Core 1.81.2 | 2,102 | No source project references or friend access |
| Package-only Core 1.81.17 | 2,102 | Existing copied loaded-version expectation only |
| Current package native | 48 | Existing test-friend boundary for actual original evidence |
| Retained callers | 124 /23 /33 | Only Schema.dll replaced |

These overlapping lanes are not additive coverage. All 2,886 prior source identities
are retained, with two explicit expectation reconciliations and 164 new cases, making
3,050 source cases. M2D1-66 now accepts its original pattern/abc instance; M2D1-80 eagerly
compiles its valid unused pattern and keeps unrelated Label/abc valid. Original
schema/instance/IDs are unchanged; current descriptions/section citations and expected
compile/assessment/values/reason fields are recorded in [ADMISSIONS.json](ADMISSIONS.json).
No other source expectation changed.

Public API remains 54 types / 401 rows with zero delta. All 18 signed Core archives/source/
cache/runtime identities and the signed published beta.1 baseline verify. Source,
package and guarded driver Schema.dll agree; packaged README matches source and its
exact XML example passes both facades, exact String/export and prefix/suffix-invalid
controls. Local unsigned alpha.0.30 is qualification only; no package was published.
[Results](implementation-evidence/RESULTS.json), [stable identities](implementation-evidence/STABLE-CASES.json),
[API](implementation-evidence/SURFACE.json), [identities](implementation-evidence/IDENTITY-FINAL.json).

## Measured corpus effects

The frozen pilot remains 21 matches / 0 Unsupported / 0 mismatches / 0 harness errors, with all 42
original payloads/expectations unchanged. Fifteen unchanged runner-integrity controls
pass. [Frozen delta](implementation-evidence/PILOT-DELTA.json).

Both original expanded cohorts are rerun on both Core pins, with identical results:
3 matches / 126 Unsupported / 0 mismatches / 0 harness errors across 129 distinct original cases.
Only elemU019.v, elemU008.v and elemU009.v newly match; cohort 1 remains 68 Unsupported,
cohort 2 now has 3 matches / 58 Unsupported. No reason-only changes or fixture/metadata/expectation
rewrite occurred. [Exact expanded delta](implementation-evidence/EXPANDED-DELTA.json).
The historical expanded assessment remains a record of the prior main, not overwritten.
These selected samples provide no whole-specification denominator or percentage.

## Delivery and remaining work

The reviewed prototype's class-escape/hyphen correction is retained, with its initial
failing receipts kept in the feasibility dossier. Production qualification lanes pass
without new failures. New code/test headers, consumer documentation and current
programme state are synchronized; source/package identities and original inputs are
recorded independently of future Git delivery identities.

No Core/API/pin/controller, published beta.1, tag or registry publication change.
Next: review/deliver on an explicit Git request, verify merge/main CI, then consider
the next ranked prerequisite (simple-typed global roots) with its own adoption and
actual-native provenance contract. Broader regex syntax and other large XSD areas
remain separate work; do not infer whole SKU/application acceptance from this slice.
