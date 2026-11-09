# Bosak.Schema agent handover

Last updated: 2026-11-09

## Current source and dependencies

Schema main includes Core 1.81.2 adoption at mergec953ebe0057bea1616d3e4f891f7b0a5fc4bd99b, from reviewed commit 9d1881ef7ccc52e9c756e864e984a6211ae514cc. Four product PackageReference entries and the test provider reference select1.81.2; MinVer remains on the0.4 development line. No new Schema NuGet release/tag is announced. This documentation checkpoint is the commit containing this entry, based on mergedc953ebe; no product/test C# source changes are introduced.

Core 1.81.2 and extension0.2.6 are published. Core's optional Schema source wrapper is updated in Core merge 18946b3125bf3187090877e00f22ea5a1e746118 ([PR #472](https://github.com/Fytala-Charles/Bosak/pull/472)) and pins merged Schema c953ebe with exact LF/CRLF source/test/fixture/legal hashes.

## Qualification and acceptance boundaries

- Standalone Schema:124 source regression cases; zero build warnings/errors in the dependency-adoption qualification.
- Separately retained unchanged Schema/test/configuration lane:124 cases over nine verified published 1.81.2 replacement DLLs. Same case names, not additive coverage.
- Core optional integration:135 cases, including124 upstream, eight price/assertion and three caller stream/reader ownership cases. Hosted integration and typed-price output pass.
- These establish bounded existing compatibility and input ownership. They do not certify full XSD 1.1 structural validation, native assessor/advisor/session adoption, component-universe interchange, concurrent disposal or performance budgets.

[Adoption and exact receipts](CORE-1.81.2-ADOPTION.md), [architecture](ARCHITECTURE.md), [integration](INTEGRATION.md), [registry](FEATURE_REQUESTS.md) and [roadmap](../ROADMAP.md) agree on this scope. Core's scoped release approval/budget deferral does not complete Schema's broader native provider or performance requirements.

## Preserved local work and history

The original pre-sync checkout was still2009018 with uncommitted compatibility/design notes. All42 local input files are preserved: four original canonical files as [raw snapshots](history/2026-11-09-pre-sync/README.md), plus the original historical review inbox and responses at their existing paths. [Hash inventory](DOCUMENTATION-SYNC-2026-11-09.json). Exact input/response bytes, NotSubmitted draft fields and source/proposal identities are unchanged; no acceptance was manufactured. A local safety snapshot and Git stash also retain the pre-sync work.

The separate public native assessment worktree is not advanced or edited by this documentation task. Never copy private Bosak.Licensing source/history, credentials, billing or deployment material into this repository. Schema remains Apache-2.0 with no activation requirement.

## Next work

Select and qualify the next bounded native provider/session profile using the released Core surface and recorded supported/refused requirements. Keep native runtime/lifetime evidence, broader XSD coverage, numerical budgets and any Schema package publication as explicit separate work. Do not revive obsolete1.26.3 or pre-release statements from archived notes as current pins. Commit/push only on explicit user instruction; this documentation synchronization and delivery were explicitly requested.
