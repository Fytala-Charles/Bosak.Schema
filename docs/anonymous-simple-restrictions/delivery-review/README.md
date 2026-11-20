# Anonymous restriction delivery review

20 November 2026. Base: merged PR29
`1438f719863d0c13f976f1b0eb683e146a3511e5`.

Reviewed the two modified native compilation files, new shared restriction parser,
actual anonymous identity/provenance tests, 60 authored catalog cases and five
public lifetime/capacity/legacy controls, explicit NS-051 correction, package and
retained lanes, upstream transitions and documentation limits.

**No blocking findings.** The implementation uses original source-node identities,
does not merge equal definitions, counts combined local simple/complex definitions
once per source site including unused definitions, and retains old profile guards.
The six original sealed/historical packets, unchanged upstream corpus and exact
candidate source/runtime hashes were verified again. No source/runtime changes
were made during review, so the existing 1,373-test qualification remains applicable.

The preserved exploratory legacy route failure is explicit: QName-free native
anonymous values must use the qualified scalar-read route. Synthetic Core node
transport feasibility does not widen the native advisor's deliberately scalar-only
binding. General expression/C45/PSVI acceptance remains outside this PR.

Upstream changes are six validity matches plus two diagnostic-only changes for
still-unsupported integer restrictions. No Unsupported result is counted as a match;
the 7/14 selected pilot result is not a full-specification percentage. Package and
retained test totals overlap source/other lanes. Current-Core qualification replaces
only the minimum-version assertion with an explicit current-version control.

[Proposed title](PR-TITLE.txt) and [PR description](PR-BODY.md) are ready.
No staging, commit, push or PR creation occurred in this review step.
AGENTS.md Git Rules require an explicit request before commit/push. After delivery,
review hosted checks and the exact remote diff before any merge recommendation.
