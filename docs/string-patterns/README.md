# Bounded string-pattern proposal and Core capability review

22 November 2026. Product remains PR39/main
`677b5628be9b22c993dddb6ff8dce1b268a282db`. This proposal follows the
[expanded public assessment](../expanded-conformance/README.md), whose129additional
cases remain schema-compilation Unsupported on both Core pins. The frozen pilot
retains21matches/zeroerrors. No product or dependency changed.

**Recommendation: prototype a bounded Unicode-scalar automaton owned by Schema**
for a declared XSD regex subset. Use the existing String transport and public
Core interfaces; do not call an internal regex helper or alter ambient settings.
[Proposed contract](CONTRACT.md), [planned payloads/gates](CASE-PLAN.json),
[exact source pins](SPECIFICATION-REVIEW.json),
[Core source review](CORE-SOURCE-REVIEW.json),
[measured public capability](core-capability-evidence/RESULTS.json).

The normative review confirms whole-literal matching. Public fn:matches instead
searches a substring and treats caret/dollar as anchors. Multiple pattern elements
at one restriction are alternatives. The earlier ranking's substring item was a
review question; it is not adopted XSD facet behavior. No earlier sealed assessment
was rewritten to adopt different semantics.

Twenty safe tiny public-API probes per signed Core1.81.2/1.81.17 agree with XSD
expectations after experimental whole-string wrapping and literal-anchor escaping.
Raw fn:matches differs in seven intended XPath/XSD cases; no Core defect was
established. Both probes observe an infinite ambient .NET regex timeout, and exact
pinned Core sources use a default-timeout backend with no caller work budget.
Thus semantic adaptation alone does not meet the bounded native assessment contract.
No pathological patterns or unhandled negative .NET child were launched.

The proposal includes Unicode literals/dot, concatenation, grouping/alternation,
explicit scalar classes, XML whitespace escapes and bounded quantifiers. Unicode
property/name classes, class subtraction and new type chains remain separate work.
Do not silently redefine Unicode digit escapes as ASCII. The original SKU still
requires broader pattern syntax and a simple-typed root; no whole-example admission
is promised. Exact syntax/work limits and original provenance must pass isolated
and actual-native feasibility before production adoption.

Source/specification review and the safe Core experiment are separately recorded;
there is no production semantic adoption or Schema pattern implementation here.
The current product remains2,886source tests,API54types/401rows, same Core pins and
published beta.1. No package/controller/release, commit or push was performed.
Next: isolated automaton prototype and accounting, then real native feasibility.
