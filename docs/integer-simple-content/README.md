# Bounded integer simple-content proposal

21 November 2026. PR36 is merged at `ab7e20afacd83edf93e71be2123fb40235c4f646`.
Its tree matches the reviewed candidate; main CI44896328993 passes 2,319 source
cases (651 implementation / 1,668 public), release/docs/workload checks.
[Merge receipt](MERGE.json), [main CI](MAIN-CI.json).

The next proposed slice admits integer complex simple-content extension and one
direct restriction using the existing exact integer family and numeric facets.
Both complex types are attribute-free and assertion-free. Integer support alone
does not solve inherited/contextual assertions in the original assert006 schema.
Current frozen qualification remains 17 matches / 4 Unsupported / zero errors.

[Contract](CONTRACT.md), [payloads and gates](CASE-PLAN.json),
[facet rules](FACET-RULES.json), [source review](SOURCE-REVIEW.json),
[immutable specification review pins](SPECIFICATION-REVIEW.json),
[original upstream inputs](UPSTREAM-REVIEW.json).

The plan contains 60 concrete payloads, 48 integer bound comparisons and 22
separate native/qualification gates. [Artifact lint](VALIDATION.json) checks XML,
identities and immutable source/corpus bytes; it does not execute Schema.

This is a contract and static artifact review only. No production code, fixture,
Core pin, API or release workflow changes, and no new engine/package/pilot run.
Next: experiment-only adoption and actual native feasibility on both signed Core
pins; production adoption follows only after that gate. Full XSD/PSVI/C45 and
performance acceptance remain separate. No commit/push/publication requested.
