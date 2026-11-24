# Specification priorities after bounded dates

24 November 2026. PR46 merged as 383914562d89d3f7fad3a6a60cf4e29884cce912; main CI45426915018 passes 4,246 source tests and supporting checks. This assessment changes no implementation or adopted semantics. [Merge receipt](PR46-MERGE.json), [main CI receipt](MAIN-CI.json).

## Recommendation

Prepare a separate contract for **bounded date simple-content extension**, then qualify an isolated candidate before production adoption. It would support date-valued elements carrying metadata attributes while reusing the checked date representation, existing public facades and owned original-node facts. The two unchanged CType/contenttype00201m instances provide representative extension inputs. This is engineering priority from reviewed dependencies and public examples, not identified customer demand or an admission forecast.

[Next contract requirements](NEXT-CONTRACT.md) make governing complex type versus atomic date base explicit. The implementation currently refuses IsDate simple-content bases. Enabling extension also requires an explicit refusal guard on date complex restriction: its existing noninteger fallback is decimal-specific. Removing one exclusion without checking that downstream path would widen the profile incorrectly. Broader restrictions, type chains and simple-to-complex xsi:type substitution stay separate.

## Calendar gaps in the existing sample

| Reviewed calendar feature | Cases | Latest measured outcome | Next obligation |
|---|---:|---|---|
| Explicit date member in element-only content | 2 | Match | Preserve delivered behavior |
| Date simple-content extension | 2 | Unsupported/ValueType | Recommended separate extension contract |
| Mixed complex content containing a date member | 2 | Unsupported/Attributes | Mixed content and typed-value design |
| Date enumeration | 2 | Unsupported/ValueType | Value equality, base facet validity and timezone handling |
| Date maxExclusive | 1 | Unsupported/ValueType | Partial-order comparison and bound semantics |

The nine cases all concern xs:date; the bucket name date-time does not establish dateTime/time coverage. The mixed=true schemas account for the two Attributes refusals; date attributes already work within the delivered profile. Diagnostic first-refusal categories are not a feature taxonomy. [Exact measured rows and unchanged file hashes](CALENDAR-LEDGER.json).

## Ranking and evidence limits

After date simple content, prepare a date-enumeration contract, then separate date-bound/explicitTimezone work. Enumeration requires value-space equality/identity rules, not a fabricated total order. DateTime/time exact-seconds Core transport design remains an important separate foundation; the selected calendar examples do not measure those types. Wider Unicode regex, mixed/default-anyType content and closed assembly remain on the broader programme. [Current dependency-aware ranking](RANKING.json).

The latest qualified selected sample, executed 24 November during date production qualification, remains **129 cases: 12 matches / 117 Unsupported / zero mismatches or harness errors** on both signed Core pins. Frozen pilot remains 21 matches. This assessment reuses those receipts; no new source test, native or corpus execution is claimed. Original expanded scripts retain their 22 November date field; execution date and source candidate base are recorded separately from merged main. [Receipt identities and bucket counts](CURRENT-SAMPLE.json).

No test-count or sample percentage is an XSD1.1 completion figure. Full PSVI/C45, typed queries, general calendar operations and publication remain separate. The original [post-particle ranking](../post-particle-coverage/RANKING.json), original date proposal and all earlier packets remain historical evidence.

[Exact source review](SOURCE-REVIEW.json), [immutable normative review](SPECIFICATION-REVIEW.json) and [328 unchanged production inputs](ENGINE-INPUTS.json) support the recommendation. This is not semantic adoption, new API/pin/package/controller work, a performance result, commit/push or publication.
