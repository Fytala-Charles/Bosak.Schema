# S009-01 — Native structural demand and capability inventory

> **Executed 2026-10-30** (Schema owner work under REQ-009, registered in [REQ-009-OWNER-WORK-PACKAGES](REQ-009-OWNER-WORK-PACKAGES.md)).
> **Outcome:** the inventory and demand-ranking portion is complete. **No next implementation slice is selected** — no public or owner-authorized schema/instance fixture with expected typed-consumer operations is on record, and the registration forbids adding assertion grammar or structural coverage without a demonstrated use case. Slice selection returns to the owner with the exact ask in §5. The native 0.9/v5 profile is unchanged; nothing here adopts new semantics.

## 1. Demand sources reviewed

| Source | Observation | Demand established? |
|---|---|---|
| Native 0.9/v5 suite (451 cases, commit `31f6661`, verified green this date) | v1 required particles → v2 optional → v3 bounded repeated → v4 bounded count assertions → v5 bounded presence predicates; exact decimals; preserve/strip bridge | The implemented profile itself — finite, already closed |
| Public `tests/TestData/Schemas` (`orders.xsd`, `sku.xsd`) | Legacy XSD 1.0 `xsl:import-schema` fixtures ([identities](S009-02-FIXTURE-IDENTITIES.json)) | No — they predate and sit outside the native profile |
| Main-branch REQ-009 staged descriptors (S1–S5c: assertions, facets, CTA, open content, default attributes, wildcard constraints) | Recorded metadata + add-side evaluation | Context only, per registration; does not establish native structural demand |
| Inspected consumers ([CONSUMER-COVERAGE-PRIORITIES](CONSUMER-COVERAGE-PRIORITIES.md)) | XmlDb: untyped persistent XML, no schema dependency; Braid: authoring inspection/preview contracts | **No** — no inspected consumer dependency establishes structural, facet, nondecimal or closure demand; grammar convenience (sum/average/not, broader Boolean comparisons) explicitly ranked lowest there |

## 2. Capability matrix

Legend: **Native** = real 0.9/v5 enforcement · **Descriptor** = recorded by main-branch REQ-009 staged pipeline (metadata/evaluation, no structural authority) · **Refused** = explicit Unsupported outside the profile.

| Coverage area | Main descriptor metadata | Native 0.9/v5 enforcement | Outside profile |
|---|---|---|---|
| Global element declaration | Element declarations with resolved types (S1/S2 typed view) | One named global element, one anonymous element-only complex type | Multiple globals, element refs |
| Local particles | — | Required, optional and bounded repeated `xs:decimal` declarations; order and count enforced; Σ max ≤ 16 | Non-decimal types, unbounded/nested groups, choice/all |
| Assertions | `xs:assert` recording + W3C-constrained evaluation (S2); CTA applicability (S4a/S4b) | Bounded `count(name)` eq/ne/lt/le/gt/ge and `exists/empty` predicates, and/or grouping, XPath 2.0 | General XPath, variables, axes, arithmetic, external access |
| Typed values | Built-in simple-type typed view for assertion evaluation (S2/S3) | Exact decimal lexical/value assessment; rounding/overflow/underflow refused | Non-decimal datatypes, list/union values, QName/NOTATION |
| Attributes | Default-attributes descriptors (S5b) | Namespace declarations only | Attribute declarations, uses, wildcards |
| Wildcards / open content | Recorded with representation validation (S5a/S5c) | Refused | All instance matching |
| Alternatives | Recorded + applicability evaluation (S4a/S4b) | Refused | CTA at assessment time |
| Imports/includes | Bounded resolution with explicit diagnostics | Refused | All schemaLocation references |
| Facets beyond profile | `xs:assertion` facets via ValidateValue (S3) | Refused | pattern/enumeration/range facets |
| Identity constraints, nil, default/fixed | Recorded boundaries only | Refused (never admits nilled elements) | All |

## 3. Demand ranking of candidate extensions

| Candidate extension | Demonstrated demand | Verdict |
|---|---|---|
| More assertion grammar (sum/average/not, wider comparisons) | None — explicitly flagged as convenience without a use case | **Defer** (registration forbids) |
| Named/anonymous non-root types | None | Defer until a fixture names one |
| Non-decimal values / facets | None | Defer |
| Namespaces / imports-included profiles | None | Defer |
| Component closure beyond the single root type | None | Defer |

## 4. Slice decision

**No new slice is selected.** The inventory shows the implemented v1–v5 profile is complete within its bounds and every extension lacks demonstrated demand. Per the registration — "choose one finite next implementation slice rather than adding assertion grammar without a use case" — the honest execution of that instruction, given zero demonstrated demand, is to decline invention and return the choice to the owner.

## 5. Exact owner ask (unblocks slice selection)

Supply **one** public or owner-authorized schema + instance fixture together with the expected typed-consumer operation(s) on the assessed result (e.g. "assert totals via XQuery over typed decimals", "preserve annotations through this transform"). Against that fixture the next slice will be ranked by namespaces, named/anonymous types, nondecimal values, facets and component closure — with immutable specification adoption recorded before any code, and happy-path/edge regressions following the suite rule.

## 6. Status

- Inventory: **complete** (this document).
- Slice selection: **blocked on owner fixture** — not on Braid production-validation, C45 or core releases.
- Native profile, receipts and prior sealed evidence: unchanged.

---

*Executed 2026-10-30. Documentation only — no runtime behavior change, no new grammar, no conformance claim.*
