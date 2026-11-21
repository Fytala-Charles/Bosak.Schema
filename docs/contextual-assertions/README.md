# Contextual and inherited integer assertions: bounded proposal

21 November 2026. PR37 is merged as `2e8d6102e10e9ccbddee87249eaefa2fdfaa4468`;
its tree matches the reviewed16694d1 candidate. Main CI44910612674 passes2,522
source tests plus release/docs/workload checks. [Merge receipt](MERGE.json),
[main CI](MAIN-CI.json).

The proposed first slice admits contextual assertions on existing integer complex
simple content and inherits the base assertion components into its one-level
restriction. It separates the temporary partial-PSVI assertion context from final
original typed provenance: `.` normally atomizes to untypedAtomic, while `$value`
is typed integer. Those inputs can produce different numeric and comparison results.

[Contract](CONTRACT.md), [71 proposed payloads and25gates](CASE-PLAN.json),
[semantic sentinels](SEMANTIC-SENTINELS.json),
[exact specification review pins](SPECIFICATION-REVIEW.json),
[source review](SOURCE-REVIEW.json), [original upstream inputs](UPSTREAM-REVIEW.json),
[possible authored effects](POTENTIAL-ADMISSIONS.json).

This step changes documentation only. Payloads are XML-linted plans, not executed
Schema tests or conformance passes. The unchanged frozen result remains17matches/
4Unsupported/zeroerrors; prospective admissions must be measured. The namespace-uri
nested/empty-content fixture stays a separate structural/function slice. Next:
experiment-only adoption and actual native signed-Core feasibility before any
production semantic adoption. No commit/push/package/controller/publication change.
