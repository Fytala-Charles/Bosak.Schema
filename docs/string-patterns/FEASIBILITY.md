# Bounded string-pattern native feasibility

22 November 2026. Isolated experiment on merged PR39/main
`677b5628be9b22c993dddb6ff8dce1b268a282db` after
[exact experiment-only adoption](feasibility-evidence/EXPERIMENT-ADOPTION.json).
The original proposal and public-Core capability packet remain sealed separately.
Production source, fixtures, dependencies and published packages are unchanged.

## Measured result

**164 controls pass per signed Core pin**,1.81.2 and1.81.17. The unchanged minimum-pin
baseline separately passes four controls: existing String assess/read and prospective
pattern-schema refusal through both facades. No backtracking matcher or private Core
helper is called by the candidate. The independent test oracle uses explicit-timeout
nonbacktracking .NET matching over a finite authored ASCII domain only.

The candidate has58source files from57exact production Git blobs, with six overlays:
five copied existing files plus one new internal automaton/budget implementation.
It compiles the proposed Unicode-scalar subset to immutable instructions and traverses
counted states, with visited-state closure deduplication for nullable cycles. Each
compile and each assessment owns fresh accounting objects; no ambient settings or
shared mutable assessment budget. The compilation-wide state ceiling includes unused
types; unused malformed/excluded patterns also refuse eagerly. Runtime work is shared across all scalar elements/attributes in an assessment.

All58planned payloads run through both facades (116controls). They measure whole
literals, caret/dollar literals, OR patterns with other facets AND, empty strings,
Unicode supplementary scalars, classes/quantifiers/whitespace, syntax/capacity refusal,
anonymous types and attributes. Five pure controls,21boundary/resource controls and
22further/original/provenance/admission controls make164per pin. The oracle performs
**37,147 comparisons** across307patterns/121inputs within one control; these are not
upstream cases or additive coverage. [Execution](feasibility-evidence/EXECUTION.json),
[automaton and adapter delta](feasibility-evidence/CANDIDATE.diff).

Exact/one-over limits pass for256characters,64atoms,8group depth,16class members,
16explicit repeats,128states/pattern,1024states/schema and the4,194,304default work
counter. The same counter proves exact per-match allowances and one-fewer refusal.
Two6000-character values exceed the shared native allowance, including an attribute/
leaf pair, and publish no partial facts/evidence/snapshot; a later assessment recovers.
Matching also charges inactive-state scans and pre-match scalar-length scans, giving
a conservative accounting bound. Length/enumeration checks precede pattern evaluation.
Cancellation is checked in compilation, length scanning, state closure and matching;
a controlled dedicated-task probe observes mid-match cancellation after counted work.

Original named/anonymous nested String facts, type/provenance identity, empty String
as one atomic (not a complex empty sequence), foreign/copy refusal, disposal and export
controls pass. The three unchanged original expanded-cohort pattern inputs also pass
direct probes through both facades: elemU019,elemU008,elemU009. This is **not** a full
expanded or frozen corpus run or a new product admission. Original bytes and validity
expectations remain unchanged. [Exact inputs](feasibility-evidence/ORIGINAL-INPUTS.json).

## Regression measurements and explicit authored deltas

The true unchanged implementation catalog passes900/900. The unchanged public catalog
measures1,984pass/2fail across exactly two authored inputs:

| ID | Original expectation | Candidate measurement |
|---|---|---|
| M2D1-66 | Pattern schema Unsupported | Schema Valid; originalabc instance Valid; Stringabc |
| M2D1-80 | Unused pattern definition makes schema Unsupported | Valid pattern compiled eagerly; unrelated Label/abc remains Valid |

The two original schema/instance/ID fields remain unchanged. Four extra direct native
controls per pin confirm both outcomes through both facades. Only the copied runtime
fixture is reconciled (compile/assessment/values/reason); its rerun passes1,986public
cases. Together, all2,886prior source case identities are retained after these explicit
two admissions, not with unchanged expectations. Production fixtures were never edited.
[Exact before/after expectations](feasibility-evidence/REGRESSION-ADMISSIONS.json),
[stable identities](feasibility-evidence/STABLE-CASES.json).

API54types/401rows has zero delta;18Core package signatures, source commits and exact
archive/cache/runtime payload identities verify. [API](feasibility-evidence/SURFACE.json),
[identities](feasibility-evidence/IDENTITY.json). These remain overlapping scoped lanes.

## Preserved initial findings

A source review found the prototype treated class escape followed by hyphen as a
scalar range. XSD1.1's disambiguation rule applies to single-character parts; `\s`
is a class escape, so `[\s-a]` is a union of whitespace,hyphen,a. Four caught native
compiler controls per pin reproduced the error; the isolated parser now starts a
range only after a scalar member, and those controls pass. Initial candidate source,
hashes, failingTRX and correction are retained separately. No production defect is
claimed from this experimental source.

The boundary test initially compared the public reason enum to the internal enum;
the baseline copied project also referenced an unused missing fixture. Initial source/
projects/logs are preserved; these were harness setup errors, not conformance results.
The unreconciled public failures and copied expectation rerun are retained distinctly.

## Next step and limits

Native feasibility passes within the proposed subset. Production semantic adoption
and implementation are pending, including explicit reconciliation of the two authored
expectations. Then qualify full source, package-only consumers, retained binaries,
README/API identities and exact frozen/expanded corpus effects. No product package,
retained caller or full corpus was executed in this experiment. The existing product
still has2,886passed source cases and frozen21matches; the expanded assessment remains
129schema-compilation Unsupported per pin on unchanged main.

Unicode property/name/digit classes, class subtraction, simple-typed roots, new
restriction chains and broader datatypes remain excluded. The original SKU is not
an admission target for this subset. No full-XSD/PSVI/C45/SLA or publication acceptance.
No commit/push/tag/publication requested or performed. Next: separate production
adoption and measured implementation of this reviewed bounded slice.
