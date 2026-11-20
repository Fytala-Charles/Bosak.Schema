# Integer restriction delivery review

20 November 2026. Base: merged PR30
`5bfbb1e798cdf18bac6ac7bb15705b1a0e50bc65`.

Reviewed the new exact integer facet compiler/parser, shared restriction dispatch,
element and attribute enforcement, original named/anonymous identity and ownership,
fixed-zero/digit/range/component boundaries, source/capacity diagnostics, adopted
catalog transitions, package/retained/API evidence and consumer documentation.

**No blocking findings.** BigInteger parsing/comparison stays bounded by the
existing lexical/supplied-digit limits; no decimal rounding or facet bypass for
attribute uses exists. Legacy refusals, assertion and scalar-only query guards
remain intact. Original pre-code adoption and its route clarification are distinct:
two opt-in prior admissions, not three legacy admissions. M2B-54's original row,
schema and instance are unchanged; its opt-in route is separately tested.

Nine sealed/historical packets, original upstream fixture/expectation bytes,
candidate production/runtime hashes and all three protected prior payloads were
rechecked. No code or runtime changes were made during review, so the 1,482-test
source qualification, package-only1,055/1,055 and retained124/23/33 results remain
applicable without a redundant run. Public API is still52types/392rows.

The pilot adds exactly two selected integer validity matches (9matches/12Unsupported,
zero mismatches/errors), with19other rows unchanged. This does not establish full
datatype/clause/suite/PSVI or generalCoreC45 acceptance. Package and retained lanes
overlap source/other catalogs; current-Core replaces only the minimum-version
assertion with an explicit current-version control. The local package is unsigned
compatibility evidence and published beta.1 remains unchanged.

[Proposed title](PR-TITLE.txt) and [description](PR-BODY.md) are ready.
No staging, commit, push or PR creation occurred in this review step. AGENTS.md
Git Rules require an explicit owner request before commit/push. After authorized
delivery, inspect the exact remote diff and hosted checks before recommending merge.
