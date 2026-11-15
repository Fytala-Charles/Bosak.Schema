# Bosak.Schema agent handover

Last updated: 2026-11-15

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

Native backend checkpoint, 15 November: implemented internal Assessment/NativeDecimalSchema.cs and NativeDecimalAssertion.cs from the adopted basis, with [finite grammar/byte-counting rules](NATIVE-DECIMAL-GRAMMAR.md). NativeDecimalBackendTests adds 76 cases and three authored golden fixtures. Fresh build: zero warnings/errors. Full standalone: 207 passed, zero failed/skipped (124 retained + seven transport probes + 76 backend). [Backend evidence](native-decimal-backend/2026-11-15/README.md) pins current sources, fixtures, dependencies and logs; earlier probe receipts remain historical and unchanged. No public native entry point, Core adapter, full XSD 1.1, API freeze, performance acceptance, tag or release. Next: implement authority/advisor/input/lifetime adapters and a real scalar consumer, then execute missing P01–P12 controls. Base HEAD remains 302d6f6b168b692dc053b0160f4babde8ebfad15; all work is uncommitted. Documentation style and public API conventions are unchanged. NuGet audit and official PowerShell 7 documentation execution remain unverified locally.

15 November follow-up: [published Core scalar transport probe](native-session-probe/2026-11-15/README.md) delivered in NativeSessionProbeTests.cs. All seven focused cases and 131 full standalone cases pass, zero skips, with zero build warnings/errors. Test-only issued decimal via `$value`, default compile options, XPath 3.1/XDM 3.1 environment; no structural assessment. Snapshot retains a host lease after Run until snapshot disposal. [Six hashed W3C bodies](NATIVE-DECIMAL-SPECIFICATION-SOURCES.json) and the [semantic adoption basis](NATIVE-DECIMAL-SPECIFICATION-ADOPTION.md) are recorded before backend implementation. No package pin, product API, runtime standards behavior or release/version change. Retained logs and package/runtime/source hashes are in the probe receipt. The complete 131-case standalone count does not alter the separate historical 135-case Core lane. Next: finalize executable finite grammar and byte-counting rules, implement native structural assessment and P01/P02/P05/P08 controls, then authority/advisor/lifetime adapters. All P01–P12 remain unqualified as a whole. Local NuGet audit/documentation execution limitations below remain; no commit/push requested.

The first scope is defined in [schema-native-decimal-session-v1](NATIVE-SESSION-PROFILE.md) and [proposed ADR-009](ADR-009-native-decimal-session-profile.md). The focused scalar probe and specification basis are delivered in the follow-up above. Native structural backend and authority/advisor/lifetime adapters remain next. The twelve paired acceptance groups are planned tests, not completed provider evidence. Positive cross-universe handoff and general XSD 1.1 remain separate tasks.

What was built on 15 November: an uncommitted documentation-only profile definition based on HEAD `302d6f6b168b692dc053b0160f4babde8ebfad15`, inspected published Core 1.81.2 contracts and retained historical requirements. No source/test implementation, API acceptance or release. Historical files remain unchanged. Local review baseline: isolated-output build passed with zero warnings/errors; all 124 tests passed with zero skips. NuGet audit was disabled because nuget.org was unreachable, so vulnerability checking is not qualified. Default obj output failed with access denied. The pwsh alias could not launch; Windows PowerShell 5.1 cannot run the PowerShell 7 documentation check. No commit/push was requested.

Select and qualify the next bounded native provider/session profile using the released Core surface and recorded supported/refused requirements. Keep native runtime/lifetime evidence, broader XSD coverage, numerical budgets and any Schema package publication as explicit separate work. Do not revive obsolete1.26.3 or pre-release statements from archived notes as current pins. Commit/push only on explicit user instruction; this documentation synchronization and delivery were explicitly requested.
