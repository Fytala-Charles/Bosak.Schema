# Structural-context assertions: bounded contract review

21 November 2026. PR38 is merged at `2effee5bf9daa317599c9285ba2de5e333bc786f`.
Its reviewed tree is preserved; main CI44944736601 passes2,770 source tests
(840 implementation/1,930 public), release controls, documentation and workload checks.

The next proposed slice combines namespace-uri assertions on existing finite
structural sequences with genuine empty complex content. Empty content has a
present empty typed sequence; element-only content has no typed value. Supporting
one by pretending it is the other would corrupt original-node provenance.

[Contract](CONTRACT.md), [proposed payloads and native gates](CASE-PLAN.json),
[exact specification review](SPECIFICATION-REVIEW.json),
[source review](SOURCE-REVIEW.json), [Core documentation review](CORE-CONTRACT-REVIEW.json),
[remaining original upstream case](UPSTREAM-REVIEW.json), [artifact lint](VALIDATION.json).

This packet is a documentation/static assessment, with no semantic adoption or
native execution. The frozen pilot remains20matches/1Unsupported/zeroerrors across
21cases; no additional match is forecast. All42frozen corpus files remain unchanged.
The next step is isolated native feasibility against both signed Core pins, beginning
with faithful non-list empty typed-value enumeration. If that gate fails, retain
Unsupported and prepare a separate Core contract; do not fabricate an atomic value.
No product, API, dependency, package, controller or published-beta change is made.
