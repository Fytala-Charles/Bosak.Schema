# Schema beta gap assessment

17 November 2026. Reviewed public main `d82e93d023233eb85b15ed203cb04f964054908d`; published baseline `0.5.0-alpha.0.7`. This is a proposed acceptance plan, not beta acceptance or a new semantic implementation. Braid and XmlDb adoption are deferred while this work is prioritized.

## Recommendation and decision required

Pursue Schema stabilization first. Propose a **bounded native decimal-session beta**, with the older descriptor/assertion services documented separately. Do not describe that release as a general XSD 1.1 validator. An ordinary application schema typically needs namespaces, attributes, reusable types and wider datatypes that the native grammar deliberately refuses. If those are the owner's minimum expectations, expand the implementation before beta instead of relabeling the existing alpha. No date or effort estimate is justified until this scope decision and the next test audit are complete.

Apache-2.0 permits open development/distribution; it does not establish semantic completeness, API stability or runtime safety. Core remains an external dependency, with package qualification on1.81.2 and separate current-Core source-integration evidence. No dependency update is proposed by this assessment.

## Evidence and capability inventory

| Area | Observed capability | Beta gap or exclusion |
|---|---|---|
| Distribution | Published Apache-2.0 alpha; trusted publisher; exact signed payload verified; fresh registry-only23-case consumer passes | Record publication in current docs; repeat qualification for exact beta artifact |
| Native structure | One unqualified root, ordered decimal leaves, bounded occurrences, finite assertions | No target namespaces, application attributes, reusable/global type system, arbitrary content model, non-decimal/list/union/nil profile |
| Exact values | Native lexical limits before normalization; detached XdmExactDecimal; no numeric fallback | Explicit precision/scale/formatting contract and endpoint tests must be reviewed as a beta promise |
| Legacy services | Schema descriptors, assertions/facets and selected type-alternative behavior | Not complete structural assessment; a successful assertion result cannot certify arbitrary instance validity |
| Public ownership | Synchronous string-input session/documents; detached XML/scalars; overlapping work refused; disposal serialized | ADR-009/010 remain Proposed; review precise exception, cancellation, cleanup and dependency contracts |
| Identity/compatibility | Owned issuance, foreign/copied-evidence refusal; actual scalar integration | Positive cross-universe handoff and complete relations unimplemented; exclude unless explicitly required |
| Security/limits | Native parser DTD prohibition/null resolver; byte/tree/grammar/numeric limits; negative tests | Audit all public paths separately; legacy diagnostics may expose expression/error details; no blanket sanitized-diagnostic claim |
| Workload budgets | Finite count/byte limits and selected deterministic lifecycle schedules | No measured time/memory budgets or broad concurrency guarantee |
| Qualification |272 product cases:249 implementation +23 external public consumer;45 workflow controls | Counts are not a coverage argument; no complete W3C XSD suite qualification recorded |

Native grammar ceilings include32KiB schema/64KiB instance UTF-8,256 descendant nodes, at most16 local declarations/total occurrences,1–8 assertions,512 assertion characters/128 tokens/16 nesting pairs and256 leaf UTF-16 characters. Caller-owned strings are allocated before these checks. Consumer options can impose tighter numerical/session capacities. These are documented implementation limits, not measured operating budgets.

## Concrete source findings

1. `src/Bosak.Schema/Validation/XsdValidationService.cs`, `Validate`: aggregates applicable assertion diagnostics and returns success when none occur. It does not run a full structural validator. Its `DetermineGoverningType` explicitly leaves derivation checks for later work and treats an unresolvable xsi:type prefix as absent. Audit and document this service's promise before beta; these observations are not a claim of native-assessor false acceptance.
2. `Assessment/NativeDecimalSchema.cs` and [native grammar](../NATIVE-DECIMAL-GRAMMAR.md): explicit supported/refused finite syntax. Expanding a datatype or grammar is a semantic change requiring immutable adoption evidence and new paired controls before implementation.
3. [Public contract](../NATIVE-DECIMAL-PUBLIC-API.md): four stage diagnostic codes; argument/cancellation/quota/closed/overlap errors throw. Cleanup attempts all resources but failure does not certify physical release. Export retains lexical XML rather than canonical XSD bytes. Beta documentation must preserve these distinctions.
4. [P01–P12 evidence map](../native-decimal-public/2026-11-17/README.md): finite controls exist for each recorded boundary, but P08 budget/result-shape scope, wider lifetime schedules, P11 positive compatibility and P12 relations remain excluded or unaccepted. Do not mark every group accepted from total test counts.
5. [ADR-009](../ADR-009-native-decimal-session-profile.md) and [ADR-010](../ADR-010-owned-native-decimal-facade.md) are Proposed and contain superseded implementation-pending descriptions. Keep historical receipts immutable; reconcile current decisions and explicitly approve contracts before calling them stable.

## Prioritized beta work

| Order | Work | Required exit evidence |
|---|---|---|
| B01 | Owner selects bounded beta or broader validator target | Signed-off support/exclusion matrix; consumers can tell which service answers which question |
| B02 | Review API and legacy/native result meaning; resolve ADR decisions | Public symbol diff against published alpha, explicit exception/diagnostic/lifetime contract; no undocumented stability promise |
| B03 | Audit supported/refused boundaries and security per public path | Named test-to-requirement inventory; happy/invalid/unsupported/cancel/quota cases; parser/resolver/diagnostic review; fix confirmed defects |
| B04 | Qualify representative standalone consumer workloads | Deterministic examples, cold/warm timing and peak-memory measurements with documented machine/input/budgets; cancellation/disposal evidence |
| B05 | Review integration without changing product scope | Fresh package-only consumer and retained binary compatibility; reviewed Core contract acceptance boundaries, not Braid/XmlDb implementation |
| B06 | Beta candidate packaging and release review | Full tests/controls/docs, API diff, dependency/license scan, exact-artifact signed NuGet verification, explicit owner publication approval |

B01 and B02 are release blockers now. B03/B04 require an audit/measurement rather than assumed missing functionality. Positive compatibility, broader types, streams and asynchronous APIs are conditional scope additions, not automatically required for a bounded beta. A general-XSD beta would require a separate component/datatype/structural implementation and conformance plan; this assessment does not estimate or authorize that expansion.

## Proposed release checklist

- [ ] Owner approves beta support matrix, name and explicit exclusions (B01).
- [ ] ADR-009/010 decisions and public contract reviewed; C45/Core acceptance dependencies resolved or expressly excluded (B02).
- [ ] Every promised supported/refused behavior mapped to named tests; no open correctness/security defect within promised scope (B03).
- [ ] Limits, cancellation, ownership and diagnostics qualified on each exposed route (B03).
- [ ] Representative workload and time/memory budgets selected and measured (B04).
- [ ] Package-only and retained-binary compatibility reviewed against alpha; no hidden source overrides (B05).
- [ ] Canonical docs/examples agree with shipped behavior; historical receipts retain original identities (B06).
- [ ] Exact beta version/tag strategy reviewed using MinVer; beta1 is a proposal, no version/tag changed here (B06).
- [ ] Full solution/controls/docs and hosted pipeline green; exact package separately approved and verified after publication (B06).

## Next recommended slice

Complete B01/B02 before expanding grammar: write an API-by-API support/result/exception matrix, audit legacy assertion success semantics, and draft updated ADR decisions for owner review. Existing package publication does not accept those decisions. Remain on alpha until beta criteria are met. No beta release or engine implementation is authorized or claimed by this report.


## Contract audit follow-up

The [API-by-API audit](API-CONTRACT-AUDIT.md) and [decision draft](CONTRACT-DECISIONS.md) complete the first audit deliverable for B01/B02; acceptance stays open. Five caught published-package observations sharpen B03 work: directory/prefix policy, shallow mutability and assertion-only success semantics. Prioritize explicit restricted resolution qualification before descriptor snapshot and legacy-result changes.


17 November2026 implementation follow-up: [restricted resolution](../RESTRICTED_RESOLUTION.md) provides an opt-in alternative for A04/A05 with31 implementation controls and three public consumer controls. Original alpha observations remain true; no old helper behavior changed. Native/profile/ADR acceptance remains pending. The candidate surface now has40 types/317 rows; published baseline39/313 remains historical.
