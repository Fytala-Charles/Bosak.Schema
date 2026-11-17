# Bosak.Schema development history

## 9 November 2026: documentation synchronization

Current handover, architecture, integration, feature registry, README, roadmap and agent entry points now describe the merged Core 1.81.2 adoption and refreshed optional integration. Earlier statements that the upgrade was only a temporary override/unpushed candidate, or that source pins remained1.26.3, describe their original checkpoints rather than current main. No source/API/package change is introduced by this documentation synchronization.

Four original local canonical files are retained byte-for-byte as [pre-sync text snapshots](history/2026-11-09-pre-sync/README.md). The associated review inbox/response files remain unchanged at their original paths. [Preservation inventory](DOCUMENTATION-SYNC-2026-11-09.json) records all 42 hashes and their retained locations.

## 9 November 2026: merged source dependency adoption

[Schema PR #1](https://github.com/Fytala-Charles/Bosak.Schema/pull/1) adopts published Core 1.81.2 in four product and one test reference. The124-case source and retained binary catalogs and11 separate integration/ownership controls pass within their recorded scopes; [adoption](CORE-1.81.2-ADOPTION.md) links exact receipts. The standalone source remains the0.4 development line; this is not a Schema NuGet release.

[Core PR #472](https://github.com/Fytala-Charles/Bosak/pull/472) refreshes its optional source wrapper to merged Schema c953ebe. All135 cases and the typed-price example pass in hosted Linux CI. Full native integration, XSD 1.1 structural validation and budgets remain separate.

## 3 November 2026: scoped R2 design returns

[O1/O2 revision2](core-disposition-responses/2026-11-03-v2/README.md) records AcceptWithScope against proposal ff475d712f6ef6aa5033113cc39afb887ae4cbad24705098c5c9cb2097641688 and native evidence c3624998. These are authored scoped design returns, not independent human signatures or new runtime acceptance. Earlier [unsigned confirmation drafts](core-disposition-inbox/2026-11-03/README.md), old Revise forms and supplied input hashes are preserved unchanged. Their pending gates, versions and local-delivery statements retain their historical context.

## 29–30 October 2026: public Apache transition and staged schema coverage

The public schema-only repository and activation-free shims were published under Apache-2.0; licensing infrastructure/private history stayed separate. [Publication](PUBLICATION.json) records114 original tests. Staged wildcard negative-constraint recording grew the standalone suite to124 cases; full structural enforcement was not implied.


## 17 November 2026 — local beta readiness and restricted resolution

Assessed the published alpha, classified39 public types/313 contract rows and reproduced five caught published-package observations. Proposed beta/API decisions remain unaccepted. Implemented additive RestrictedSchemaFileResolver and CreateRestrictedCompiler using published Core1.81.2 controlled schema acquisition, including nested references. The old convenience paths retain their compatibility behavior. New31 implementation and three public consumer controls bring the suite to306 cases;45 release controls pass separately. Candidate API40types/317rows is strictly additive. Documented stable-filesystem/no-atomic-sandbox boundary, full XML/DTD/byte policy, URI alias checks and Core's unused-include warning behavior. Source-only receipts and immutable pre-change basis are in docs/restricted-resolution; no tag, release or commit/push is made by this local checkpoint.


## 17 November 2026 — compiler-owned descriptor snapshot candidate

After PR8 merge/main validation, froze compiler-produced outer/nested collections without changing positional record constructors. Preserved shared assertion identity and ordinal bindings;13 added cases bring suite to319, plus45 release controls. Public40types/317rows unchanged; runtime mutable implementation casts deliberately stop working. Caller records remain application-owned. No tag, beta/profile acceptance or publication; canonical contract and source-only receipts recorded.


## 17 November 2026 — legacy compile-diagnostic precondition candidate

After PR9 merge/main validation, added a common compile-finding guard to all three legacy validation entry points. Arguments retain precedence; original findings are copied unchanged before any other work. Clean result meaning remains assertion/facet/static-check scope, not structural validation.26 added cases bring suite to345, plus45 release controls; public40types/317rows unchanged. No native/Core/version change, beta/ADR acceptance or publication.


## 17 November 2026 — bounded beta workload qualification candidate

Owner selected bounded decimal-session beta with exclusions after PR10 merge/main verification. Added a separate non-packable public consumer workload tool, six mocked integrity controls and CI build/checks. Five named workflows ran in20 processes with explicit pre-recorded local envelopes; all final mixed-numeric observations pass. Product345 and release45 controls remain green; source/dependencies unchanged. Scope selection does not accept ADR/API contracts, production budgets or beta publication. Current alpha-only release pins require separate beta review.
