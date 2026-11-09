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
