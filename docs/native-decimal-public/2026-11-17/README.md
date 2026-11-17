# Public native decimal entry qualification

17 November 2026. Local candidate based on df40a17, published Core 1.81.2 unchanged. The [pre-implementation basis](BASIS.json) and [public contract](../../NATIVE-DECIMAL-PUBLIC-API.md) define exact scope. The source-only manifest/receipts bind the final files; raw local packages/binaries stay outside Git.

## Executed lanes

| Lane | Result | Interpretation |
|---|---|---|
| Audited Release build | Zero warnings/errors | Network-enabled restore, no NuGet audit suppression |
| Full solution | 272 passed, zero failed/skipped | 249 implementation plus 23 external public-consumer identities |
| Local-package external consumer | 23 passed, zero failed/skipped | Same consumer cases through packed Schema binary and published Core packages; not additive coverage |
| Deterministic lifecycle/cleanup | Six cases included in 249 | Two close/work schedules, one reentry/cancel/fault case, three fault-position cleanup controls |
| Official documentation hygiene | Recorded in DOCUMENTATION-CHECKS.json | No kit-managed assets changed |
| Hosted candidate check | Not run | Candidate and three local predecessor commits not yet pushed |
| Historical Core optional integration | Not rerun | Original c953ebe/135-case lane remains historical |

Initial sandbox restore could not reach vulnerability metadata (NU1900); network-enabled audited restore/build passes. The first package-consumer command incorrectly sent --source to dotnet test (MSB1001); the separate restore then normalized the second source into a local path (NU1301). An explicit temporary NuGet.Config corrected source selection before the successful unchanged consumer run. Product failures were not suppressed and tests were not weakened. Source-lane 271-case success preceded one added multi-document isolation case; final count is272. All original diagnostic attempts remain local alongside final receipts.

## Bounded acceptance reconciliation

This map records executed evidence and explicit restrictions; it grants no global P01-P12 sign-off.

| Group | Executed evidence in this candidate | Retained boundary |
|---|---|---|
| P01 structure | NativeDecimalBackendTests structural golden/invalid/unsupported; external golden and refusal cases | Finite decimal grammar only |
| P02 assertions | Existing supported, false/type-error and unsupported grammar theories, rerun unchanged | Arbitrary XPath/functions refused |
| P03 authority | NativeSchemaAdapterTests.Authority_OwnedAndForeignComponents_AnswersOrRefusesExplicitly | No public raw component port |
| P04 facts/values | Existing copied/cross-bound evidence controls plus external detached output/mutation | Exported bytes grant no authority |
| P05 root | Existing Atomize_ElementOnlyRoot_ThrowsFoty0012 and external optional-empty-root case | Nil/list/union/non-decimal refused |
| P06 environment | Existing BindAndAssess_ForeignObjects_RefuseBeforeGetters and explicit external context bounds | Exact released scalar environment only |
| P07 scalar session | Existing forged/outside/closed snapshot controls plus external real ReadDecimal | Only internally compiled $value route |
| P08 limits | Existing backend byte/grammar/digit/scale boundaries and native cache/snapshot controls; external options/quota/query precision | No measured time/memory budget; not arbitrary result shapes |
| P09 lifetime | Existing expiry/drain/raw-task controls; two facade close/work barriers and post-close multi-document tests | Finite synchronous schedules; no arbitrary async disposal certification |
| P10 cancellation/ownership | Existing mid-episode cancellation and independent snapshots; public null/pre-cancel/quota recovery | String ownership only; no native stream/reader API |
| P11 compatibility | Existing independent-equal-universe Unsupported/no-view control | Positive cross-universe proof unimplemented |
| P12 relations | Existing explicit derivation/affiliation/closure Unsupported controls | Complete relations unimplemented |

## Reproduction

Run `dotnet build Bosak.Schema.slnx -c Release`, `dotnet test Bosak.Schema.slnx -c Release --no-build --logger trx`, and `pwsh tools/check-docs.ps1 -ProjectPath . -Strict`. `Bosak.Schema.PublicConsumer.Tests` normally references the source project; setting `SchemaConsumerPackageVersion` selects a normal PackageReference instead. Pack Schema locally, restore the consumer with an explicit NuGet.Config containing the local candidate source and nuget.org, then test with that property and an isolated artifacts path. Do not publish the local package or reuse a stale same-version package cache. Compare package lib DLL to the final source-build DLL and consumer runtime DLL; inventory all resolved Core package versions/hashes.

No commit/push/tag/package publication, permanent API acceptance, full XSD 1.1 or Braid native human acceptance is claimed. The next review payload includes prior local backend/adapter/facade commits, not only this wrapper diff.
