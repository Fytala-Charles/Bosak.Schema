# Expanded XSD coverage assessment

22 November 2026. Product: merged PR39/main
`677b5628be9b22c993dddb6ff8dce1b268a282db`. The completed frozen pilot remains
unchanged at21matches/zeroUnsupported/zeroerrors. This separate assessment measures
**129 original instances in108distinct groups**, across18feature buckets spanning
12areas of the historical24-area coverage matrix.

Both signed Core pins,1.81.2 and1.81.17, produce identical classifications:

| Expanded sample | Match | Unsupported | Mismatch | Harness error |
|---|---:|---:|---:|---:|
| Cohort1:68cases/54groups | 0 | 68 | 0 | 0 |
| Cohort2:61cases/54groups | 0 | 61 | 0 | 0 |
| Unique total, repeated per Core pin | 0 | 129 | 0 | 0 |

All129cases stop at schema compilation with Unsupported; no instance assessment
is reached. The instances include83original valid and46original invalid expectations.
Unsupported is neither a validation pass nor Invalid. These intentionally targeted
examples fall outside the closed native grammar; they do not indicate that all
Schema functionality fails. There is no whole-suite run, normative denominator or
justified XSD coverage percentage. Core XPath conformance was not rerun here.

## Recommended next work

Start a **bounded XSD string pattern-facet contract**, then actual-native feasibility.
The second cohort contains three declared-root bounded string-sequence schemas whose
visible new constraint is pattern: `.x`, `a*x`, `a?x`. Their original IDs and outcomes
are preserved. Existing string typed-value transport avoids introducing a new public
atomic family in the first slice. This is a qualitative engineering recommendation,
not measured customer demand or an implementation/admission promise.

Before implementation, define exact XSD regex semantics, supported syntax, Unicode,
facet combination, substring behavior and time/capacity limits; check the public Core
regex capabilities rather than assuming .NET or XPath regex is interchangeable.
Record exact semantic adoption and prove original named/anonymous provenance on both
pins. No new pattern semantics are adopted by this assessment.

Then consider simple-typed global roots for already supported families, derived
integer built-ins, and bounded choice/all particles. Date/time and other atomic
families, assembly, defaults/nil, lists/unions, alternatives, wildcards/open content
and identity constraints need separate contracts. [Ranked workstreams](RANKING.json),
[current scoped area review](COVERAGE-REVIEW.json).

## Why refusal counts alone do not choose the next feature

The first detected reasons are69DeclarationShape,34ValueType and26Attributes;
these are not an exhaustive inventory of blockers. Many minimal first-cohort cases
have no global element or test empty compositor normalization. The second cohort
was separately selected before its execution to require a global element and nonempty
choice/all particles; neither cohort was replaced or filtered using pass results.

All six second-cohort choice/all schema groups also declare implicit anyType children.
Particle support alone cannot admit them. Two selected current-cohort integer schemas
also need integer enumeration. The existing public SKU schema independently needs
both pattern and a simple-typed root; pattern alone does not make SKU pass. SKU/orders
are static comparisons here, not newly executed application workflows.
[Post-measurement co-feature audit](CO-BLOCKERS.json),
[hashed public examples](APPLICATION-EXAMPLES.json).

## Selection and provenance

Use immutable W3C xsdtests revision
`7bc3365c652a322f3d762021b3879eb92dae7e30`, the same upstream pin as the frozen
pilot. This is not a refreshed default-branch review or an upstream contribution.
Both selectors read registered suite metadata, apply the existing1.1/XML1.0 version
rules, require an accepted valid schema and accepted original instance expectations,
and select one original valid and invalid instance per group when available.
Schema-only invalid tests, multiple initial schema documents, missing/local-escape/
remote paths and inputs beyond the assessment ceilings are excluded explicitly.

Cohort1 statically chooses three groups per bucket by fewest unrelated syntactic
features, element count, LF-normalized byte length and metadata/group spelling.
Cohort2 excludes those54groups and the original15frozen groups, adding declared-root/
nonempty-particle requirements before its own execution. It retains the same ordering.
The first pass found9,642groups eligible for this selector, not a total W3C test count.
Full selection criteria and excluded-category counts are machine-readable:
[cohort1](CASES.json), [inventory](INVENTORY.json),
[cohort2](cohort2/CASES.json), [inventory](cohort2/INVENTORY.json).
The retained inputs are a subset, not a runnable full W3C checkout. Selection
scripts require the complete pinned upstream checkout; measurement/validation use
the retained subset. All retained fixture/metadata payloads are exact Git blob bytes, with original
[W3C notice](UPSTREAM-NOTICE.md); no fixture or expected-outcome rewrite occurred.

The initial selector stopped at a checkout-versus-Git line-ending difference before
execution. It now permits only checkout CRLF/LF conversion when comparing inputs,
while retaining exact original Git blobs. The initial script/failure note remain in
iterations. A second-cohort exclusion-count label was clarified to distinguish
15frozen groups from54prior-cohort groups; selection/outcomes did not change.

## Measurement route and limits

The minimum-pin run uses the previously qualified caught public native driver.
The current-pin run builds a package-only copy of its unchanged source, proving the
same Schema payload and hash-linked signed Core archives. All240product inputs match
merged main qualification. Both drivers use explicit native/query limits, fixed
assessment context, per-case caught errors and a parent watchdog. The sole API route
is NativeSchemaSession instance validity; no PSVI, other-facade, resolver or full Core
conformance claim follows. No imports/includes were fetched, resolved or flattened.
Those cases report the closed-schema contract boundary, not an incorrect XSD result.
[Minimum identity](PRODUCT-IDENTITY.json), [current identity](PRODUCT-IDENTITY-CURRENT.json),
[minimum cohort1](evidence/cohort1/RESULTS.json),
[minimum cohort2](evidence/cohort2/RESULTS.json),
[current cohort1](evidence/current-cohort1/RESULTS.json),
[current cohort2](evidence/current-cohort2/RESULTS.json), [summary](SUMMARY.json).

The historical mirrored matrix and all42frozen pilot files remain unchanged.
No product, test implementation, dependency, public API, package/controller or release
change; no full-XSD/PSVI/C45/performance acceptance. No commit/push requested or
performed. Next: pattern-facet contract and public-Core feasibility, then separately
recorded semantic adoption and measured implementation.
