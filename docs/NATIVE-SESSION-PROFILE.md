# Native decimal assessment/session profile

> Proposed implementation and qualification contract for REQ-009 on published Core 1.81.2.

## 1. Status and candidate

Profile identifier: `schema-native-decimal-session-v1`. Status: **Internal backend implemented; Core adapter and overall qualification pending**. The [backend receipt](native-decimal-backend/2026-11-15/README.md) records 76 cases on the [finite executable grammar](NATIVE-DECIMAL-GRAMMAR.md). This does not deliver a public provider, API acceptance, conformance promotion, performance budget or package release. Schema remains on the MinVer 0.4 development line. See [ADR-009](ADR-009-native-decimal-session-profile.md).

Baseline: Schema `302d6f6b168b692dc053b0160f4babde8ebfad15`, with the four product and one test references at 1.81.2. The published API/Core XML documentation and binaries were inspected locally on 2026-11-15. Observed SHA-256 identities:

| Package asset | SHA-256 |
|---|---|
| Bosak.XPath.Api 1.81.2, lib/net10.0/Bosak.XPath.Api.dll | `e978234fcdecdf3d94c9853ceec1fb45be5729169981657f35a6f09b04d3c0c1` |
| Bosak.XPath.Core 1.81.2, lib/net10.0/Bosak.XPath.Core.dll | `cc8a0f3ba4a0c2bd682d7e81c244208d9f46e6fa9e38f5b05eac9fcf04581fbe` |

These are inspection pins, not a complete runtime receipt. Implementation qualification must additionally record the final Schema revision/diff, all resolved runtime package versions, package and assembly hashes, SDK, OS, commands and complete results. Local source overrides are excluded from this profile's acceptance run.

The historical [native profile description](core-disposition-inbox/2026-11-02/inputs/native/docs/DOCUMENT-ASSESSMENT.md.txt), [N1–N8 requirements](core-disposition-inbox/2026-11-02/inputs/native/docs/S009-04-FOUNDATION-REQUIREMENTS.md.txt), and [O2 design return](core-disposition-responses/2026-11-03-v2/BCAP-O2-Schema.v2.response.json) supply requirements and prior observations. Their old source/package identities and test counts remain historical. The archived assessor is not implemented in this checkout and must not be treated as current functionality.

## 2. Selected coverage

The internal backend assesses one complete finite schema/instance grammar natively; exposing its issued facts through the released Core contracts remains next. Existing descriptor/assertion validation alone cannot supply structural authority.

| Area | Intended supported scope | Explicit refusal boundary |
|---|---|---|
| Schema | XML 1.0, no DTD, no target namespace; one global element with an anonymous element-only complex type | Imports/includes/override, named user types, external resources and multiple global roots |
| Content | One ordered sequence of 1–16 unique local built-in decimal declarations; minOccurs 0 or 1, finite maxOccurs 1–16, aggregate maximum 16 | Other particles, mixed content, wildcards/open content, attributes, identity constraints, nil/default/fixed and xsi requests |
| Assertions | 1–8 assertions; declared child operands, decimal constants, count(child), exists(child), empty(child), value comparisons and Boolean grouping, as completely parsed by the bounded historical grammar | CTA, facets, arbitrary XPath, arithmetic, prefixes, variables, axes or external functions |
| Values | Exact decimal leaves; scalar typed containers with original declaration/type and per-atomic provenance | List/union, other datatypes, precision loss or silent rounding |
| PSVI | Original element-only root with no typed value; present decimal children only; never-nil state | Fabricated states for absent optional children, fabricated anonymous QNames, general PSVI serialization |
| Identity | Authority-owned declaration/type handles and exact issuance checks within one compiled universe | QName/digest/source/snapshot labels as proof of authority; cross-universe compatibility |
| Session | Explicit opt-in assessor binding, owned immutable input, single scalar typed-value identity read inside one synchronous Run callback | General engine syntax, concurrent operations on the same session, transform capture and source-free construction |

Repeated decimal elements are separate scalar node values; they do not imply XSD list support. Query-side numeric arithmetic, casts and constructed values are deferred even when Core exposes those routes. A follow-up may extend the profile only with separate supported/refused evidence.

Input/parser ceilings retained as candidate constraints: serialized schema 32 KiB and instance 64 KiB, 256 descendant nodes, decimal lexical length 256 characters, assertion length 512 characters, 128 assertion tokens and sixteen parenthesis levels. The implementation must define the serialization encoding/counting and check both byte and decoded/tree limits before accepting input; these ceilings are not total-memory or runtime guarantees. They are profile limits to qualify, not measured workload budgets.

Every caller supplies finite `SchemaQueryLimits`: positive snapshot/cache/atom/digit counts and nonnegative scale. No implicit workload defaults. Provider decimal conversion must enforce supplied coefficient/scale limits before normalization and refuse an unrepresentable value. Keep backend lexical limits distinct from query limits.

## 3. Released contract mapping

Names below are present in the 1.81.2 package XML documentation; this is an implementation map, not a claim that every engine execution route supports every interface member.

| Need | Released contract | First-profile obligation |
|---|---|---|
| N1/N8 authority and identity separation | `ISchemaAuthority`, `ISchemaComponentHandle` | Owns/IsSameComponent authenticate actual issued handles. Resolve only the supported components; anonymous types remain anonymous. Independently compiled identical schemas have distinct authority. |
| N2 derivation/affiliation/closure | `IsDerivedFrom`, `IsAffiliatedWith`, `GetComponentClosure` | Supply complete answers only where implemented and tested; otherwise Unsupported, never guessed false or empty closure. Full relations remain pending. |
| N3 typed transport | `ISchemaTypedValue`, `SchemaAtomicItem`, `SchemaQueryOperation.ReadTypedValue` | Preserve scalar decimal kind, exact value, governing/actual type and original provenance. Reject forged containers before reading their contents. |
| N4 environment and assessment | `ISchemaAssessor.Bind`, `ISchemaAssessmentAdvisor.Assess` | Bind the exact engine-issued environment; explicitly refuse unsupported host/XDM/static-context profiles. Record immutable specification adoption evidence before assertion semantics are implemented. |
| N5 original facts | Advisor OwnsAssessment/OwnsNodeFacts/OwnsNode/OwnsTypedValue | Issue immutable original facts tied to the actual input, origin and node; copied labels or handles do not authenticate facts. Only complete Valid exposes a validated root. |
| N6 compatibility | `ISchemaCompatibilityAuthority.Establish`, `SchemaQuerySession.OpenCompatibleSnapshot` | Return Unsupported without a view for unimplemented cross-universe proof. No positive compatible handoff is accepted in v1. Incompatible requires a supported complete negative proof. |
| N7 lifetime | `IQueryOperationLifetime`, `IQueryOperationLease`, `SchemaQuerySession.Run`, `DrainAsync`, `Dispose` | Retain actual host-owned resources through complete callback consumption; revoke new admission on close, drain before owner disposal. Canceling a drain wait does not finish raw work. |

`SchemaQuerySession.CompileXPath` supports selected profiles only. The focused probe below establishes the scalar identity expression/options; it does not broaden the session parser or qualify a production provider. Unsupported binding/syntax blocks that route explicitly. `OpenSnapshot` documents raw compatibilityView admission as Unsupported; any future compatibility lane must use its engine-issued handoff route.

## 4. Planned acceptance cases

These are overall session test specifications; **none of the twelve groups is fully qualified**. The internal backend executes 76 cases toward bounded portions of P01/P02/P05/P08, as recorded in its receipt. Implement remaining controls in the existing xUnit project using authored fixtures under tests/TestData/Schemas. Retain all 124 original cases. Test names below identify intended behaviors rather than new public method signatures.

| ID | Happy path | Failure or edge case |
|---|---|---|
| P01 structural assessment | Ordered required/optional/repeated decimal children pass a completely supported schema | Missing required, wrong order/count or invalid decimal is Invalid; unsupported schema syntax is Unsupported; neither publishes ValidatedRoot |
| P02 assertions | Presence/count and exact decimal comparisons pass under the adopted finite grammar | False supported assertion is Invalid; trailing syntax, unknown operands and disallowed functions are Unsupported |
| P03 authority | Repeated lookup returns the same issued component; anonymous root type retains identity | Equal text compiled twice and forged handles never authenticate as the same owned component |
| P04 facts and values | Decimal child scalar read retains exact value and original declared/actual type | Forged facts/container, wrong node/input/origin and edited exported XML cannot authorize typed execution |
| P05 root state | Root has original complex type, no typed value, and never-nil state; omitted children have no node facts | Root atomization preserves no-typed-value failure; nil/list/union requests are refused rather than coerced |
| P06 environment | Advisor is bound to the exact session environment and assessment episode | Foreign advisor/environment or unsupported host/static-context binding refuses before assessment |
| P07 scalar session | Compile the recorded supported identity expression; admit input and consume its issued scalar inside Run | Foreign/closed snapshot or snapshot outside exact Run membership rejects before provider getters |
| P08 limits | Values at each lexical/digit/scale/atom/cache/snapshot boundary succeed where representable | One beyond each bound refuses; invalid default limits reject; no normalization bypass or rounding |
| P09 lifetime | Host resources remain live through callback completion and are disposed by their owner after drain | Revocation blocks new work; canceled drain wait does not dispose resources; resumed stale access refuses |
| P10 ownership/cancellation | Independent assessments use independent immutable snapshots; caller streams/readers remain caller-owned | Pre-canceled and mid-work cancellation throw with no successful partial root and no owner-resource disposal |
| P11 compatibility | Original same-universe provenance remains unchanged through the supported scalar route | Independent equal schemas return Unsupported/no compatibility view; names/digests never grant handoff |
| P12 unsupported relations | Owned-component identity is answered completely | Missing derivation/affiliation/closure coverage returns Unsupported, not false/empty success |

Use deterministic barriers for lifetime tests, no timing sleeps. Test independent-assessor concurrency separately from same-session concurrency, which v1 refuses. Golden fixtures for serialized/exported XML must check preserve/strip boundaries without claiming that bytes carry live authority. Do not add skipped tests as acceptance evidence.

## 5. Delivery gates and follow-up

1. Record immutable specification revisions/sections, hashes and deviations for the finite grammar and decimal/assertion semantics. Preserve archived inputs unchanged; review any reused Apache source with provenance and notices intact. No private source imports.
2. Implement and test the native assessment backend. A descriptor compile or partial assertion pass cannot publish a structural Valid result.
3. Implement authority/advisor/input/lifetime adapters and execute P01–P12 against published 1.81.2 without source overrides. Record any actual contract gap explicitly; do not silently revise Core APIs.
4. Run fresh build, full standalone tests, documentation checks and a real scalar consumer. Retain exact commands, hashes, supported/refused case names and result logs. A focused Core probe is not provider acceptance. The separate Core 135-case historical lane is not rerun or extended by these checks.
5. Review the bounded evidence before marking this profile qualified. Full XSD 1.1, complete N1–N8 support, positive compatible-universe interchange, general XSLT/XQuery adoption, concurrent session disposal, numeric performance budgets, joint freeze and Schema publication remain separate milestones.

The published-package scalar probe establishes `$value` with omitted/default compile options on a test-issued decimal. The specification basis, executable grammar and internal backend are delivered. The current full suite passes 207 cases (124 retained, seven transport probes, 76 backend cases). Next: authority/advisor/input/lifetime adapters and a real scalar consumer, including missing root atomization, authentic issuance, cancellation/drain and refusal controls. Backend evidence alone does not complete P01/P02/P05/P08 or overall P01–P12 qualification.

*Last updated: 2026-11-15*
