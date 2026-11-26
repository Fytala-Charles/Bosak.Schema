## Original markup and annotation coverage — 26 November 2026

Development0.15 adds original instance comment/PI support and bounded opaque schema annotations before declaration scans/closed assembly. [Guide](markup-coverage/GUIDE.md); [readiness](markup-coverage/BETA-READINESS.md). Source21,544/package19,881overlapping pass; API55names/663rows and signed Core1.86.0 unchanged. Frozen21matches; structural39/90; datatype537/61; zeroerrors. Original invoice workflow passes. Earlier dated entries are historical; publication/fullXSD/C45 remain separate.

## Product documentation reconciliation — 26 November 2026

README and ROADMAP now describe product use and current direction. Exact prior files, including historical checkpoint entries, are retained with source identity in [the historical snapshot](history/2026-11-26-product-docs/SOURCE.json). Their relative links retain the original repository-root context.

## Bounded structural coverage â€” 26 November 2026

Development 0.14 adds empty all/optional empty choice and original anonymous attribute simple types on admitted owners. The [guide](structural-coverage/GUIDE.md) records capacities and exclusions; [beta readiness](structural-coverage/BETA-READINESS.md) separates bounded qualification from general XSD1.1/C45 acceptance. All21,358 source/19,701 overlapping package tests pass; API55names/663rows and signed Core1.86.0 are unchanged. Structural sample improves32â†’38 matches; datatype454matches remains co-blocked. Original invoice workflow passes. Earlier dated entries are historical; publication remains separate.

## Evidence-driven coverage checkpoint â€” 26 November 2026

The [current coverage guide](SPECIFICATION_COVERAGE.md) reconciles development 0.13 / signed Core 1.86.0 with historical audits. A separately frozen 598-case datatype sample has 454 matches / 144 Unsupported / zero errors; the structural sample remains 32 / 97. Next are adopted structural fixes and consumer trials within an up-to-400-contract ceiling. Published beta.1 remains narrower; no publication or full-conformance claim. Earlier dated entries below are historical.

# Bosak.Schema development history

## 9 November 2026: documentation synchronization

Current handover, architecture, integration, feature registry, README, roadmap and agent entry points now describe the merged Core 1.81.2 adoption and refreshed optional integration. Earlier statements that the upgrade was only a temporary override/unpushed candidate, or that source pins remained1.26.3, describe their original checkpoints rather than current main. No source/API/package change is introduced by this documentation synchronization.

Four original local canonical files are retained byte-for-byte as [pre-sync text snapshots](history/2026-11-09-pre-sync/README.md). The associated review inbox/response files remain unchanged at their original paths. [Preservation inventory](DOCUMENTATION-SYNC-2026-11-09.json) records all 42 hashes and their retained locations.

## 9 November 2026: merged source dependency adoption

[Schema PR #1](https://github.com/Fytala-Charles/Bosak.Schema/pull/1) adopts published Core 1.81.2 in four product and one test reference. The124-case source and retained binary catalogs and11 separate integration/ownership controls pass within their recorded scopes; [adoption](CORE-1.81.2-ADOPTION.md) links exact receipts. The standalone source remains the0.4 development line; this is not a Schema NuGet release.

[Core PR #472](https://github.com/Fytala-Charles/Bosak/pull/472) refreshes its optional source wrapper to merged Schema c953ebe. All135 cases and the typed-price example pass in hosted Linux CI. Full native integration, XSD 1.1 structural validation and budgets remain separate.

## 3 November 2026: scoped R2 design returns

[O1/O2 revision2](core-disposition-responses/2026-11-03-v2/README.md) records AcceptWithScope against proposal ff475d712f6ef6aa5033113cc39afb887ae4cbad24705098c5c9cb2097641688 and native evidence c3624998. These are authored scoped design returns, not independent human signatures or new runtime acceptance. Earlier [unsigned confirmation drafts](core-disposition-inbox/2026-11-03/README.md), old Revise forms and supplied input hashes are preserved unchanged. Their pending gates, versions and local-delivery statements retain their historical context.

## 29â€“30 October 2026: public Apache transition and staged schema coverage

The public schema-only repository and activation-free shims were published under Apache-2.0; licensing infrastructure/private history stayed separate. [Publication](PUBLICATION.json) records114 original tests. Staged wildcard negative-constraint recording grew the standalone suite to124 cases; full structural enforcement was not implied.


## 17 November 2026 â€” local beta readiness and restricted resolution

Assessed the published alpha, classified39 public types/313 contract rows and reproduced five caught published-package observations. Proposed beta/API decisions remain unaccepted. Implemented additive RestrictedSchemaFileResolver and CreateRestrictedCompiler using published Core1.81.2 controlled schema acquisition, including nested references. The old convenience paths retain their compatibility behavior. New31 implementation and three public consumer controls bring the suite to306 cases;45 release controls pass separately. Candidate API40types/317rows is strictly additive. Documented stable-filesystem/no-atomic-sandbox boundary, full XML/DTD/byte policy, URI alias checks and Core's unused-include warning behavior. Source-only receipts and immutable pre-change basis are in docs/restricted-resolution; no tag, release or commit/push is made by this local checkpoint.


## 17 November 2026 â€” compiler-owned descriptor snapshot candidate

After PR8 merge/main validation, froze compiler-produced outer/nested collections without changing positional record constructors. Preserved shared assertion identity and ordinal bindings;13 added cases bring suite to319, plus45 release controls. Public40types/317rows unchanged; runtime mutable implementation casts deliberately stop working. Caller records remain application-owned. No tag, beta/profile acceptance or publication; canonical contract and source-only receipts recorded.


## 17 November 2026 â€” legacy compile-diagnostic precondition candidate

After PR9 merge/main validation, added a common compile-finding guard to all three legacy validation entry points. Arguments retain precedence; original findings are copied unchanged before any other work. Clean result meaning remains assertion/facet/static-check scope, not structural validation.26 added cases bring suite to345, plus45 release controls; public40types/317rows unchanged. No native/Core/version change, beta/ADR acceptance or publication.


## 17 November 2026 â€” bounded beta workload qualification candidate

Owner selected bounded decimal-session beta with exclusions after PR10 merge/main verification. Added a separate non-packable public consumer workload tool, six mocked integrity controls and CI build/checks. Five named workflows ran in20 processes with explicit pre-recorded local envelopes; all final mixed-numeric observations pass. Product345 and release45 controls remain green; source/dependencies unchanged. Scope selection does not accept ADR/API contracts, production budgets or beta publication. Current alpha-only release pins require separate beta review.


## 17 November 2026 â€” bounded beta contract review and compatibility

After PR11 merge/main verification, prepared current Proposed ADR009/010 decisions with archived original Git text. Clean source produces normalMinVer alpha0.14 without tag/override;345source/33package consumer/124legacy and23public retained binary controls pass, with onlySchemaDLLreplaced in candidate lanes. NineCore1.81.2 signatures/payloads and exact40type317row API verify. Contract review distinguishes scoped Schema consumption from generalCoreC45/ADR006 acceptance; no beta tag/artifact/publication or inferred owner acceptance.


17 November2026 owner decision: explicitly accepted bounded ADR009/010/API contracts and instructed commit/push/PR. Scoped Schema facade/publicCore1.81.2 use accepted; generalCoreC45/ADR006, productionSLAs and beta tag/artifact/publication remain outside approval.


## Beta.1 preparation â€” 17 November 2026

Bounded ADR009/010 acceptance is merged in PR12; source `2a5abf6fb663313dc32bfeca5bbb8740bbe987ae`, main44174792275 green. [Beta.1 preparation](BETA1_PREPARATION.md) records the exact product/tag plan and fresh local beta package.345 source,33 package-only, retained124/23 control/replacement cases and nine signed Core1.81.2 payloads pass;54 release and six runner controls pass separately. The controller candidate requires the reviewed beta tag and both retained lanes. No remote beta tag or NuGet beta publication; controller delivery, hosted preparation and exact hosted-artifact approval remain pending. Accepted bounded application scope does not close general CoreC45/ADR006 or production SLA/full-XSD acceptance.

## Beta.1 publication documentation â€” 18 November 2026

Base52a4997daba109bc5daec03522e9fe98725b78aa. Owner requested next steps after beta publication/GitHub prerelease. Canonical current-release docs now point to verified0.5.0-beta.1 and explicit accepted scope/exclusions; dated dossiers remain unchanged. Publication44245557198/preparation44244606595, exact hashes and GitHub asset identities are in docs/beta1-publication/RECEIPT.json. No engine/package/workflow/dependency/version changes or additional publication. [Release guide](BETA1_RELEASE.md). Worktree .worktrees/Schema.BetaReleaseDocs / docs/schema-beta1-publication. Commit/push/PR still require explicit instruction under AGENTS. Package-only representative consumer trial is separate from product test totals and is not production-user acceptance.
