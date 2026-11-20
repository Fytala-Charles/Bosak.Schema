## Beta.1 publication documentation — 18 November 2026

## Packaged guidance reconciled after M1 - 19 November 2026

PR16package guidance is reconciled with merged M1. The0.6candidate supports bounded namespaces/form rules, QName assertions and zero-to-eight assertions; published0.5.0-beta.1 is unchanged. Earlier365/53prerelease planning and alpha.0.2example receipts remain historical. Current M1 source qualification is409(315+94), with94minimum-Core and42current-Core package cases. Release packaging remains secondary to specification coverage; no tag/controller/publication change. [Reconciliation](package-readme06-reconciled/README.md).


## M1 namespace candidate - 19 November 2026

The local M1 candidate adds namespace-aware expanded declaration names, qualified/unqualified local forms, QName assertion binding and zero-to-eight assertions within the finite decimal shape. All409source cases pass (315implementation/94public), including41authored clause-mapped cases and three real Core-session controls. Package-only94cases pass with Core1.81.2; a separate42case public lane passes with published Core1.81.17. Retained124/23/33binary catalogs pass with onlySchema.dll replaced; these lanes overlap. No Core runtime/API/source-wrapper pin, published package, version/tag or release-controller change. [Evidence](m1-namespaces/README.md).


## Shared specification coverage priority - 19 November 2026

Owner priority is specification coverage and useful external application workflows across Bosak and Bosak.Schema. The shared24-area matrix separates bounded native behavior, legacy/descriptor support, refusal and unassessed work;365historical Schema case IDs are indexed without claiming conformance. M1 proposes namespace-aware decimal documents with optional assertions, followed by coherent named-type/attribute/datatype and structural slices. No runtime or release change. [Programme](specification-coverage/PROGRAM.md), [owner requirement](REQ-009-SPECIFICATION-COVERAGE.md).


## Bounded Unsupported-reason candidate - 19 November 2026

The local development candidate adds `NativeDecimalDiagnostic.UnsupportedReason` and the fixed `NativeDecimalUnsupportedReason` enum. Existing stage codes/messages, outcomes and grammar are preserved; categories describe one detected refusal and may be Unspecified. All365source cases pass (312 implementation/53public, including20new controls), as do53package-only cases and retained124/23 unchanged-binary replacement catalogs. These lanes overlap and are not additive. The54release and six workload-integrity controls pass. MinVer minimum advances to0.6 for this additive feature;0.6.0-alpha.0.2 is local/unsigned only. Published0.5.0-beta.1 and allCore1.81.2 pins remain unchanged. No commit/push, tag or publication. [Candidate](unsupported-reasons/README.md).


## Beta.2 gap assessment - 19 November 2026

A public-example beta.2 gap assessment finds no confirmed new beta.1 defect. Namespace support alone would not admit the existing sku/orders schemas. The proposed first bounded slice is clearer Unsupported reason categories, preserving stage codes, input-free diagnostics and current grammar; API/design acceptance and a concrete application priority remain pending. Beta.1 stays current; no implementation or release change. [Assessment](beta2-gap-assessment/README.md).


Base52a4997daba109bc5daec03522e9fe98725b78aa. Owner requested next steps after beta publication/GitHub prerelease. Canonical current-release docs now point to verified0.5.0-beta.1 and explicit accepted scope/exclusions; dated dossiers remain unchanged. Publication44245557198/preparation44244606595, exact hashes and GitHub asset identities are in docs/beta1-publication/RECEIPT.json. No engine/package/workflow/dependency/version changes or additional publication. [Release guide](BETA1_RELEASE.md). Worktree .worktrees/Schema.BetaReleaseDocs / docs/schema-beta1-publication. Commit/push/PR still require explicit instruction under AGENTS. Owner selected the public invoice fixture. The package-only trial passes exact reads/XML, supported violation, namespace/attribute refusal, quota/disposal and pre-cancel recovery; all nine Core signatures/payloads verified. No new engine defect or beta.2 expansion justified. See BETA1_INVOICE_TRIAL.md. Representative coverage is separate from product totals and is not production-user acceptance.



## Beta.1 preparation — 17 November 2026

Bounded ADR009/010 acceptance is merged in PR12; source `2a5abf6fb663313dc32bfeca5bbb8740bbe987ae`, main44174792275 green. [Beta.1 preparation](BETA1_PREPARATION.md) records the exact product/tag plan and fresh local beta package.345 source,33 package-only, retained124/23 control/replacement cases and nine signed Core1.81.2 payloads pass;54 release and six runner controls pass separately. The controller candidate requires the reviewed beta tag and both retained lanes. No remote beta tag or NuGet beta publication; controller delivery, hosted preparation and exact hosted-artifact approval remain pending. Accepted bounded application scope does not close general CoreC45/ADR006 or production SLA/full-XSD acceptance.

Worktree .worktrees/Schema.BetaRelease, branch release/schema-beta1-preparation; base2a5abf6. Local packet .guard-tmp/schema-beta1-20261117. No commit/push yet: AGENTS requires explicit request. Local tag is isolated from shared repository refs.

## Owner accepted bounded beta ADR/API decisions — 17 November 2026

Owner response: "Approve, please commit and push and open PR" to the explicit bounded ADR009/010 decision and31-file review delivery question. Recorded Accepted status for these application-facing Schema contracts, scoped publicCore1.81.2 consumption and listed exclusions. GeneralCoreC45/ADR006 acceptance, productionSLA, beta tag/artifact and publication remain separate/unapproved. Product/source compatibility package and all prior evidence unchanged. Deliver this31-file accepted-contract review as instructed, run hosted checks, then review/merge/main before exact beta source/tag/controller decisions.

## Bounded beta ADR/API review and package compatibility local — 17 November 2026

PR11 merged6d4e1e2bae2ee7889791e9a9dd22a2034cf3ab05/treecad07729d9a568a2524be3dd477b45ba751e799a, equal accepted head; main44158885345 green. Isolated .worktrees/Schema.BetaContract / review/schema-beta-contract; clean detached .worktrees/Schema.BetaCompatibilitySource atsame immutable source. Normal MinVer produces0.5.0-alpha.0.14,93,961bytes,SHA61b2a4b59f780d3d71d2e2676a3443103ee23f37fde0aeb5747f501ef2ce323c, no tag/override; unsigned local compatibility only, not beta artifact.345source and33fresh package-only consumer cases pass; nineCore1.81.2 signatures/payloads match. Retained124legacy and23originalpublic controls/candidates pass with onlySchemaDLLchanged, same catalogs. API40types317rows exactcurrentmatch;4additions/no removalsvsalpha.

[Contract review](BETA_CONTRACT_REVIEW.md) and revised ADR009/010 are concrete Proposed owner decisions, with original Git text/hashes retained in newdossier. No acceptance inferred from scope choice or evidence. Proposed acceptance covers Schema's bounded application facade/consumption of publicCore1.81.2 only, explicitly not generalCoreC45/ADR006/provider acceptance. OperatingSLAs, fullXSD, publicraw authority, perpetual1.0freeze and publication excluded. Source/test/dependency/engine unchanged; earlier sealed dossiers preserved. No commit/push, beta tag/controller change/login/publication. Rawpacket .guard-tmp/schema-beta-contract-20261117. Next explicit bounded ADR/API decision, then reviewed exact beta source/tag/controller and freshartifact qualification/publication approval.

## Bounded beta scope chosen; workload qualification local — 17 November 2026

PR10 merged22aa9ad84959f95d92dc82626117955edd1fdb0b/tree031d1fa78a694181c9d99716ad4b35ec1461eb55, identical accepted PR tree; main44147900719 green. Isolated .worktrees/Schema.BetaQualification / qualification/schema-beta-workloads. Owner explicitly chose bounded decimal-session beta with exclusions. Added non-packable public-API workload tool, watchdog/hash-validating parent and six mocked integrity controls; normal CI builds/checks tool but does not gate timing. Product source/tests/dependencies unchanged. [Report](BETA_QUALIFICATION.md), [evidence](beta-qualification/README.md).

Five workflows/20processes:15 first-use samples,150 measured warmed iterations,25 warmups. Final max-input workload uses32KiB/64KiB,16 leaves, eight assertions, mixed256digit integers/255place decimals. All pass proposed local envelopes; exact table/raw JSON retained. .NET10.0.11/SDK10.0.301 on Windowsx64 CoreUltra9 285HX/24logical, workstationGC. Caller-thread allocation is not live/native heap; process peak includes runtime/JIT/setup/warmups. Pre-cancelled calls only in timing; no new mid-work latency or arbitrary scheduler/leak/platform guarantee. Initial integer-only runs remain raw, not pooled.

Full345 product,45 release and6runner controls pass; tool/product builds0warnings/errors. Owner scope accepted, ADR009/010/C45/API freeze and production budgets remain pending. Current release controller still pinned to old alpha/version272cases and cannot prepare beta without separate review. No commit/push, tag, beta artifact or publication. Next bounded ADR/contract decision review and exact beta compatibility/package qualification. Rawpacket .guard-tmp/schema-beta-qualification-20261117; prior dossiers preserved.

## Legacy compile-diagnostic guard local — 17 November 2026

PR9 merged88c3a45fa2f7f37980e2105ba0947399dd77af44/tree0eded90c944d922c2cfa28770fddfc109cddf5c3, equal accepted PR tree; main44140782563 green. Isolated .worktrees/Schema.CompileDiagnostics / fix/schema-compile-diagnostics. Validate/ValidateValue/ValidateAlternativeTests now return false with copied original compile findings after null checks and before other model/evaluation work. Clean success remains assertion/facet/static-check scope; unknown-root/no-tests true explicitly tested, not structural validity. [Contract](LEGACY_VALIDATION.md), [evidence](compile-diagnostics/README.md).

Full345pass (312implementation/33external),22+4 new cases,45release controls; zero-warning/error build. API40types317rows identical toPR9. Copied compile diagnostic collections are immutable; no blanket legacy result redaction/immutability promise. Historical dossiers preserved, Core1.81.2/native/profile unchanged. No commit/push, beta/ADR acceptance, tag or publication. Next beta acceptance/checklist and resource-budget work after review; do not silently expand XSD scope. Rawpacket .guard-tmp/schema-compile-diagnostics-20261117.

## Compiler-owned descriptor snapshots local — 17 November 2026

PR8 merged as7e16b1609741caa5ac445cf9c81ae1725cf36da3, tree8da044cdb13d29780fd9d8a7cfa0409b86505a69 matches accepted PR head; fresh main44121838200 green. New isolated .worktrees/Schema.DescriptorSnapshots / fix/schema-descriptor-snapshots. Compiler publication now freezes outer and nested bindings/facets/wildcards, preserving shared assertion identity. Public positional constructors/with records unchanged and caller-owned. [Contract](DESCRIPTOR_SNAPSHOTS.md), [evidence](descriptor-snapshots/README.md).

Full319pass (290implementation/29external), ten+three new cases,45 release controls; build0warnings/errors. API40types/317rows identical toPR8; runtime implementation collection types intentionally change. Initial two test failures came from invalid fixture order and were corrected without grammar relaxation. No new engine language semantics, API/pin/version/tag/publication or ADR/beta acceptance. Existing sealed dossiers unchanged; verify at original commits/checkouts. Rawpacket .guard-tmp/schema-descriptor-snapshots-20261117. No commit/push; explicit instruction required. Next legacy compile-diagnostic/result precondition, not full structural validation.

## Restricted schema resolution implemented locally — 17 November2026

Same isolated docs/schema-beta-assessment atd82e93d. Added RestrictedSchemaFileResolver and SchemaAwareXslt.CreateRestrictedCompiler, controlled through publishedCore1.81.2 resource authority. Basis recorded before implementation with Core7249d0e/package and immutable platform source review.31 new path/XML/controlled-XSLT controls and three new no-friend consumer cases; full306pass (280/26),45 unchanged release controls. API40types/317rows, four additions/no removals. Build0warnings/errors. [Guide](RESTRICTED_RESOLUTION.md) and [evidence](restricted-resolution/README.md).

Restricts canonical contained local files, root spelling, reparse points, byte cap and full-document DTD/XML; no legacy callback fallback, nested references use Core policy, non-Schema acquisition denies. Existing helpers remain compatible. Stable host-owned filesystem required; no atomic rename/link-race or malicious-filesystem sandbox guarantee. Core may warn for unused denied include: denied receipts tested, required-type failure separately tested. Fixed initial Uri.LocalPath trailing-dot/space normalization gap with OriginalString check. Earlier failed TRX retained raw. No Core source/pin/frozen-gate change, beta acceptance, tag/release or commit/push. Next snapshot immutability and legacy-result decisions remain separate.

# Schema beta assessment — 17 November 2026

Base/current main d82e93d023233eb85b15ed203cb04f964054908d; isolated docs/schema-beta-assessment. Owner prioritizes Schema beta before Braid/XmlDb. [Assessment](beta-readiness/ASSESSMENT.md) inventories supported/refused scope, source findings, blockers B01–B06 and proposed acceptance checklist. Recommends explicit bounded-beta versus broader-validator choice and API/ADR review before implementation. No engine/specification/dependency/API/version/tag change, beta acceptance or commit/push. Current alpha publication is verified; earlier dossiers remain historical.

Publication: preparation44102733436/publish44106769923; approvedpackageSHA f3f7107e83106746a2f6a83e5f0a8e39fa8f4187e6378c25fa1140d366807b8c; signedSHA6a11b8980b6f7cca68beec160845bff83edf317b51e27fcfd06775da22f48722. Registry indexed, all entries unchanged except signature; fresh NuGet-only23-case consumer and nineCore1.81.2 signatures/payloads verified. Raw publication receipts remain ignored in Core packet. Native full solution272 and45 release controls are distinct catalogs. ADR009/010 still Proposed; C45/globalprofile/performance remain pending.

Next: owner review beta scope/API contract matrix and proposed ADR acceptance. Commit/push only on explicit request per AGENTS. Original owner checkout and sealed evidence preserved.

---

## Hosted metadata filename mismatch identified; local correction — 17 November 2026

PR6 merged as `2e7c0aa5ba30cafabb53d46c001d0a4034223d3e`, main44097747001 green. New first-attempt preparation44098084319 passed product272, repeated package23, nine signed Core1.81.2 payload checks and legacy124 cases per lane, then sealing refused `package/services/metadata/core-properties/nuget.psmdcp`. Rejected archive retained:89,416bytes, SHA-256 `b88a25bdaa8f59294aaac775b3c11c79bb442c858943eb73c629a5d081d03a45`. Its OPC relationship points to that normal metadata file; creator NuGet.Packaging7.9.0.0. NuGet upstream source pinned in [correction evidence](package-metadata-correction/README.md) confirms the exact path. The verifier accepted only the older generated-name form; this is a release-controller defect, not an engine/test failure.

Isolated local `fix/schema-package-metadata` adds only that exact metadata path to the existing allowlist, plus positive/near-name refusal coverage.23Python+22Node controls pass. The actual retained archive and actual hosted TRX/dependency/legacy receipts pass corrected local seal/verify without altering bytes. This rehearsal does not make the failed hosted run admissible. Publisher still refuses failed runs; a new successful preparation and independently reviewed manifest/package are required. Original archives and historical dossiers remain sealed.

No commit/push, new preparation, NuGet login, profile-variable/policy change, tag or publication has occurred for this correction. Next: explicit commit/push/PR approval, review/merge/main validation, new preparation-only run. Product/API/semantic/Core/version scope unchanged; full-schema/API/performance acceptance remains separate.

## First hosted preparation refused; diagnostics prepared — 17 November 2026

PR #5 merged as `84c0fc9df3df6f4a97a8a459c2c21fca12d25002`; main run44087274600 passed. Preparation-only [run44087538178](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/44087538178), first attempt, passed controls, product272 cases, package23 cases, nine Core signatures/payload checks and legacy124 cases per lane. Sealing refused `Unexpected package content`. Publish was skipped and no eligible package artifact was uploaded. The unexpected entry is unknown: the original error omitted its name, and rejected package bytes were not retained. Local same-source packaging passed the existing allowlist, so it does not prove the hosted cause.

Isolated local `ci/schema-preparation-diagnostics` at the merge adds offending entry names to the refusal and a failure-only `schema-rejected-package-<run>-<attempt>` artifact with seven-day retention. No allowlist change. Eligible artifact names, success requirement, manual default, source/version pins and publisher remain unchanged. Two new diagnostic controls bring local controls to21 Python plus22 Node. Historical trusted-publishing evidence stays sealed. See [diagnostic evidence](preparation-diagnostics/README.md). This candidate is uncommitted/unpushed pending explicit instruction; no rerun or new preparation has been dispatched.

Next: commit/push/PR approval, review/merge/main validation, then a new preparation run rather than rerunning the failed first attempt. Inspect any retained rejected package before proposing a specific acceptance fix. No NuGet policy/profile variable change, login, publication or tag occurred. Earlier89,520-byte local package SHA-256 remains `d9f3ed5897ce30c6069e72a8e651e3648d132251a3c6ef232d8aa946dd3428d6`.

# Schema trusted-publishing route prepared locally - 17 November 2026

Last updated: 2026-11-19

Owner selected NuGet trusted publishing after metadata inspection found Schema has no secrets/variables/environment or publisher workflow configured. Isolated .worktrees/Schema.TrustedPublishing / ci/schema-trusted-publishing based on merged1783d1a. A [trusted-publishing route](TRUSTED_PUBLISHING.md) is prepared locally following the owner's selection. The new release workflow is manual and preparation-only by default; its publisher verifies a successful same-repository/main/first-attempt preparation, the exact reviewed manifest/package and registry absence before OIDC login and again before one push. Product source/version/Core dependencies are unchanged. No workflow dispatch, policy/account update, login or publication has occurred. A hosted preparation will create its own artifact/digest for review; the earlier local package remains preserved.

Release workflow/controller/tests are uncommitted; explicit commit/push instruction is still required by AGENTS. Product src/tests/global.json and package dependencies unchanged. Build0warnings/errors and272product cases pass;19Python+22Node=41separatecontrol cases pass. Release/build YAML parses;11PowerShell bodies parse without execution. A local rehearsal against the actual preserved89,520-byte0.5alpha0.7package verifies source, package/9Core signatures/payloads,272+23case receipts,124legacy cases per lane,seal/verify andregistryGET. It uses all-zero controller placeholder, has no hosted run, and is explicitly ineligible for publication. PyYAML6.0.3 installed only under ignored Core packet for local syntax validation; no product dependency.

Current source/version allowlist is1783d1a99b37e206b10592c24b3a5fa3ac126969/0.5.0-alpha.0.7, legacybaselinec953ebe, SDK10.0.301, Core1.81.2. Manual publish consumes a first-attempt successful release preparation run and exactmanifesthash; no rebuild, duplicate suppression, broad package glob or long-lived APIkey. VariableNUGET_USER (NuGet profile name) and NuGet policy must be configured by owner/explicit authorized action. Policy fields:ownerFytala-Charles/repoBosak.Schema/workflowrelease.yml/environmentblank, packageBosak.Schema, pushnew+versions scope. See docs/TRUSTED_PUBLISHING.md. No trust-policy/account or GitHub setting changes made; no credential requested/read.

Next: review concrete workflow/source-only evidence payload and request explicit commit/push/PR approval. After merge+checks, dispatch preparation only, download and independently review new hosted artifact/manifest, then ask explicit publication approval for that exact run/hash before login/push. Earlier localpackageSHA d9f3ed5897ce30c6069e72a8e651e3648d132251a3c6ef232d8aa946dd3428d6 remains sealed; hosted rebuild has a distinct provenance/digest and cannot be silently substituted. No tag/release/publication/profilefreeze acceptance. Rawlogs/rehearsal/parser/controllers .guard-tmp/schema-trusted-publishing-20261117; source-only dossier docs/trusted-publishing. Originalownercheckouts andsealedpackets preserved.

---

# Schema0.5 package-delivery review - 17 November 2026

Last updated: 2026-11-17

Isolated .worktrees/Schema.Release05Review / release/schema-0.5-review at clean merged9fd4e6a; original Schema checkout remains untouched. The public owned decimal API is merged in [Schema PR #3](https://github.com/Fytala-Charles/Bosak.Schema/pull/3) at9fd4e6a; its272-case standalone main check passed. Core's optional source consumer is refreshed in [Core PR #503](https://github.com/Fytala-Charles/Bosak/pull/503), mergef7b6c995, with269 current-Core cases and all six main checks passing. Standalone/package dependencies remain Core1.81.2; current-Core1.81.17 source-consumer evidence is a separate lane. The [0.5 package-delivery review](package-0.5-review/README.md) prepares a local prerelease and a package README correction. No Schema package, tag or registry action has been published; global profile/ADR/API-freeze/performance acceptance remains open.

Clean-source MinVer produced0.5.0-alpha.0.6 without tag/Version override. Original unsigned89782-byte nupkgSHAf41d1909d25ae80969697a3ddba5d083ffd22cfaa973a8ffccf628c3957d0bdb preserved. Staging89515-byteSHA d10b32adc1a00da5a2ba9b35fc9941aab17bf798ba01ab58320a582f5050a1c8 changes only package README/metadata; runtimeDLL/XML/legal identical. Directory.Build.props now selects docs/NUGET_README.md, using only absolute HTTPS and an allowed-domain remote logo. This packaging patch is uncommitted; metadata source9fd4e6a identifies assembly source, not an immutable packaging commit. Do not publish either same-version local archive without resolving the review/commit/preparation gates.

API review32->39exportedtypes/278->313declaredrows, seven new types/35rows, no removed/changed legacy row. Full272source tests and23 package-only cases pass. Old124-case test/host/config binaries pass baseline and with onlySchemaDLL replaced; identical identities/non-Schemabytes/config, not248distinctcases. Core package references stay1.81.2, fresh archive/cache/runtimeDLL verified. Build0warnings/errors. Read-onlyNuGetindex/candidate/stable endpoints404 on17November; no package-ID rights/availability guarantee. ADR009/010 remainProposed; stable0.5.0 not prepared, no globalXSD/compatibility/API-freeze/performance acceptance. Existing source+package counts overlap and must not be added.

Rawcandidate archives/caches/binaries/logs/scanner .guard-tmp/schema-05-package-review-20261117; source-only review dossier docs/package-0.5-review. Offline verification and documentation checks must pass before closing. Next: explicit commit/push approval for package-only README correction and synchronized review docs, hosted source qualification, then a new normal MinVer preparation from immutable accepted source. Registry publication is a separate future decision; no API key, tag, publisher workflow, NuGet push, Git commit or push was executed. AGENTS Git rule still requires explicit commit/push instruction. Historical source/proposal receipts preserved.

---

# Public native decimal entry candidate - 17 November 2026

Last updated: 2026-11-17

Isolated worktree `.worktrees/Schema.PublicDecimal`, branch `feature/public-native-decimal-session`, based on local clean Schema df40a17. Original public checkout preserved. The 17 November public-entry candidate implements `NativeDecimalSession`, `NativeDecimalDocument`, immutable options/context and owned results with fixed bounded stage diagnostics. It keeps Core 1.81.2 and the finite backend grammar unchanged. The 0.5 development line has 272 passing cases: 249 implementation cases (243 retained plus six lifecycle/cleanup controls) and 23 separate public-consumer cases without friend access. The consumer also passes through the locally packed Schema binary and published Core dependencies; this repeats its case catalog and is not additive coverage. [Contract and qualification](NATIVE-DECIMAL-PUBLIC-API.md). Hosted review, overall profile acceptance, API freeze and package publication remain separate.

Implementation not committed/pushed. AGENTS.md Git rule requires explicit commit/push instruction; continuation of implementation does not waive it. Exact source/evidence inventory and review payload live in docs/native-decimal-public/2026-11-17. Raw logs/local nupkg and diagnostic scripts stay in the Core workspace .guard-tmp/schema-public-decimal-20261117, with source-only copies in the dossier. First sandbox restore failed NU1900; network-enabled audited build passes. Package-consumer initial command incorrectly passed --source to dotnet test; separate restore then hit command-line source-path normalization; explicit NuGet.Config corrected the harness, and unchanged product/consumer passes. Original errors remain retained. No engine edits, historical Core135-case rerun, C45/global acceptance, package publication, tag or Braid integration.

Next: explicit commit/push approval for this concrete candidate and predecessor local commits aba6967/939ba9a/df40a17, then hosted review. Do not advance Core optional source pin automatically. Braid PR5 is already merged as9fac4441 and main44034731777 green; a focused native smoke review is the next Braid step after Schema hosted review. New native human acceptance has not been performed.

---

Historical handover entries below retain original dates and scope.

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

Current checkpoint: user-requested adapter commit completed as `939ba9a` (feat: integrate native decimal assessment with Core sessions). Follow-up remains uncommitted: eight additional adapter controls, default lookup-name argument fix, internal NativeDecimalSession owned-facade prototype and eight workflow cases. Total 243 = 124 retained + seven probes + 76 backend + 28 adapter + eight facade. Release build: zero warnings/errors. Official PowerShell 7 docs checker and fresh network-enabled NuGet auditing now pass under required host permissions; previous restricted-shell limitations are historical. [Latest evidence](native-session-facade/2026-11-15/README.md), [public proposal](NATIVE-DECIMAL-PUBLIC-API-PROPOSAL.md) and [proposed ADR-010](ADR-010-owned-native-decimal-facade.md) define the actual scope and next work. No public symbols, package pin/version/tag change or overall P01–P12/global acceptance. Next: public result/option/diagnostic and ownership contract implementation with an external consumer; separately qualify close/work races and cleanup-failure policy before permanent API acceptance. Earlier receipts remain unchanged.

Latest checkpoint: user requested commit and continuation; backend/profile/probe work committed as `aba6967` (feat: implement bounded native decimal assessment backend), then internal Core adapter work continued without a second commit. NativeSchemaProvider, NativeSchemaInput, NativeSessionNode and NativeSessionRunner now connect actual native assessment to Core 1.81.2 scalar sessions. Twenty adapter cases and 227 total standalone cases pass; Release build has zero warnings/errors. [Adapter design/acceptance map](NATIVE-SESSION-ADAPTER.md) and [receipt](native-session-adapter/2026-11-15/README.md) record exact scope. Reassessment refuses; issued facts/node/container membership is authenticated; input close revokes guarded access; raw work retains actual host leases through canceled drain waits. One shared runner gates overlapping operations, since raw Core lease counts do not enforce sequential work. Engine data(.) raises InvalidOperationException with FOTY0012; direct root.TypedValue raises XPathErrorException. No public API or package pin/version/tag change. Next: remaining bounded-profile controls and public entry point/external consumer review. Broader compatibility/XSD/concurrent-disposal/performance acceptance stays pending. Official PowerShell 7 docs and NuGet audit limitations remain; prior receipts are unchanged.

Native backend checkpoint, 15 November: implemented internal Assessment/NativeDecimalSchema.cs and NativeDecimalAssertion.cs from the adopted basis, with [finite grammar/byte-counting rules](NATIVE-DECIMAL-GRAMMAR.md). NativeDecimalBackendTests adds 76 cases and three authored golden fixtures. Fresh build: zero warnings/errors. Full standalone: 207 passed, zero failed/skipped (124 retained + seven transport probes + 76 backend). [Backend evidence](native-decimal-backend/2026-11-15/README.md) pins current sources, fixtures, dependencies and logs; earlier probe receipts remain historical and unchanged. No public native entry point, Core adapter, full XSD 1.1, API freeze, performance acceptance, tag or release. Next: implement authority/advisor/input/lifetime adapters and a real scalar consumer, then execute missing P01–P12 controls. Base HEAD remains 302d6f6b168b692dc053b0160f4babde8ebfad15; all work is uncommitted. Documentation style and public API conventions are unchanged. NuGet audit and official PowerShell 7 documentation execution remain unverified locally.

15 November follow-up: [published Core scalar transport probe](native-session-probe/2026-11-15/README.md) delivered in NativeSessionProbeTests.cs. All seven focused cases and 131 full standalone cases pass, zero skips, with zero build warnings/errors. Test-only issued decimal via `$value`, default compile options, XPath 3.1/XDM 3.1 environment; no structural assessment. Snapshot retains a host lease after Run until snapshot disposal. [Six hashed W3C bodies](NATIVE-DECIMAL-SPECIFICATION-SOURCES.json) and the [semantic adoption basis](NATIVE-DECIMAL-SPECIFICATION-ADOPTION.md) are recorded before backend implementation. No package pin, product API, runtime standards behavior or release/version change. Retained logs and package/runtime/source hashes are in the probe receipt. The complete 131-case standalone count does not alter the separate historical 135-case Core lane. Next: finalize executable finite grammar and byte-counting rules, implement native structural assessment and P01/P02/P05/P08 controls, then authority/advisor/lifetime adapters. All P01–P12 remain unqualified as a whole. Local NuGet audit/documentation execution limitations below remain; no commit/push requested.

The first scope is defined in [schema-native-decimal-session-v1](NATIVE-SESSION-PROFILE.md) and [proposed ADR-009](ADR-009-native-decimal-session-profile.md). The focused scalar probe and specification basis are delivered in the follow-up above. Native structural backend and authority/advisor/lifetime adapters remain next. The twelve paired acceptance groups are planned tests, not completed provider evidence. Positive cross-universe handoff and general XSD 1.1 remain separate tasks.

What was built on 15 November: an uncommitted documentation-only profile definition based on HEAD `302d6f6b168b692dc053b0160f4babde8ebfad15`, inspected published Core 1.81.2 contracts and retained historical requirements. No source/test implementation, API acceptance or release. Historical files remain unchanged. Local review baseline: isolated-output build passed with zero warnings/errors; all 124 tests passed with zero skips. NuGet audit was disabled because nuget.org was unreachable, so vulnerability checking is not qualified. Default obj output failed with access denied. The pwsh alias could not launch; Windows PowerShell 5.1 cannot run the PowerShell 7 documentation check. No commit/push was requested.

Select and qualify the next bounded native provider/session profile using the released Core surface and recorded supported/refused requirements. Keep native runtime/lifetime evidence, broader XSD coverage, numerical budgets and any Schema package publication as explicit separate work. Do not revive obsolete1.26.3 or pre-release statements from archived notes as current pins. Commit/push only on explicit user instruction; this documentation synchronization and delivery were explicitly requested.


17 November2026 follow-up: API-CONTRACT-AUDIT/CONTRACT-DECISIONS/API-INVENTORY classify39 published types/313 unchanged rows. Five caught package-only observations: assertion-only unknown-root success, compile diagnostics not propagated, writable descriptor list, directory parent hint and text-prefix sibling resolution. Only self-created temp files accessed/cleaned. No source changes. Proposed next: explicit restricted resolution contract/tests before immutable descriptors and legacy result preconditions. ADR009/010 remain Proposed. Raw diagnostics `.guard-tmp/schema-beta-contract-20261117`; all exceptions caught.


## M2A scope proposal - 19 November 2026

M1 is merged (Schema PR17/Core PR504), with packaged guidance reconciled in Schema PR16. The next [bounded named-complex-type proposal](m2a-named-types/PLAN.md) defines QName references, component reuse, actual Core identity/atomization and explicit refusal controls. Planning only; no runtime/API/pin/publication change or additional passed-test claim. REQ-009 remains In Progress.


## M2A implementation candidate - 19 November 2026

The [named-complex-type slice](m2a-named-types/README.md) is locally implemented and qualified:465source cases(320implementation/145public),145minimum-Core package cases,52current-Core cases and retained124/23/33binary catalogs pass; scopes overlap. Up to16global types and16root declarations, full expanded-name lookup, eager unused-type checks, shared type identity and original Core facts are supported within the decimal-only profile. No Core engine/API/pin or published package/controller change. REQ-009 remains In Progress; broader M2 datatypes/facets/attributes and official W3C qualification remain pending.


## M2B decimal restriction proposal - 19 November 2026

M2A merged in PR18 at6861c04. The next [named-decimal restriction proposal](m2b-decimal-restrictions/PLAN.md) defines direct xs:decimal simple types, selected range/digit facets, actual Core provenance and distinct capacity/Invalid/refusal gates. Planning only: no new runtime/API, semantic adoption, passed-test count or publication. Broader datatypes and attributes follow in separate slices; REQ-009 remains In Progress.


## M2B implementation candidate - 19 November 2026

The [adopted decimal-restriction slice](m2b-decimal-restrictions/README.md) implements named simple types directly based on xs:decimal and six selected range/digit facets, with named original Core provenance.534source cases(324implementation/210public)pass; source/package/retained qualification is recorded in the dossier with overlapping lanes kept separate. Combined16global type ceiling; explicit facet integer capacity256. Named nondecimal types, restriction chains, other facets and attributes/imports remain excluded. No Core engine/API/pin or release/publication change; REQ-009 remains In Progress.


## M2C datatype and owned-value proposal - 19 November 2026

M2B merged in PR19 atfbb4c13. The [next datatype proposal](m2c-builtin-datatypes/PLAN.md) stages strings with an additive owned scalar facade, then Boolean/integer, followed by selected string facets and attributes. Existing DecimalCount/ReadDecimal semantics stay bounded to decimals. Planning only: no new API, datatype adoption, runtime, passed-test count or release change. Exact surface/contract and published-Core feasibility proof are required before implementation. REQ-009 remains In Progress.


## M2C1 concrete surface and Core feasibility - 19 November 2026

The [API review package](m2c-builtin-datatypes/api-review/README.md) and [proposed ADR011](ADR-011-owned-native-scalar-facade.md) define an additive ten-type string/decimal facade without changing old decimal semantics.14synthetic public transport cases pass separately on signed Core1.81.2 and1.81.17, with18package signatures/source/payload identities verified. No new Schema validation or API implementation/acceptance, engine/pin or publication change. Scalar provenance is snapshot-bound; complete-container views are operation-local. Actual mixed Schema node/facade qualification and exact string semantic adoption remain next; REQ-009 is In Progress.


## M2C1 string/decimal implementation candidate - 19 November 2026

The [adopted string/decimal facade](m2c-string-facade/README.md) adds the ten reviewed public types under explicit opt-in and shares the existing owner/gate/cleanup path.614source cases(334implementation/280public)pass, including47authored string cases,23public contract controls,6mixed lifecycle and4actual Core/writer controls. Primitive string text is preserved with explicit UTF16capacity; typed reads retain original facts internally and bounded export preserves parsed CR references on reopen. NativeDecimalSession remains decimal-only. No Boolean/integer/facet/attribute/import expansion, Core engine/pin or release/publication; REQ-009 and overall API/C45 acceptance remain In Progress.


## M2C2 Boolean implementation candidate - 19 November 2026

M2C1 merged in PR20 at16355ea; fresh main CI passes614source cases and54release controls. The [bounded Boolean candidate](m2c-boolean/README.md) adds Boolean=3 and a checked BooleanValue getter to the opt-in facade, actual Boolean provenance/atomization and a fixed256code-unit lexical ceiling before XML-whitespace collapse.671source cases(340implementation/331public)pass locally, with intentional prior-refusal deltas recorded separately. Legacy decimal facade unchanged; Boolean assertions/restrictions and integer support remain excluded. No Core engine/pin or release/publication change; REQ-009 and overall API/C45 acceptance remain In Progress.

## M2C3 exact integer implementation candidate - 19 November 2026

M2C2 merged in PR21 at20cc520; fresh main CI44602002427 passed671source cases and54release controls. The [bounded integer candidate](m2c-integer/README.md) admits built-in xs:integer (a decimal restriction, not a primitive datatype), checked detached BigInteger values and original integer provenance/atomization.748source cases(347implementation/401public)pass locally. Raw lexical ceiling256UTF16code units; MaxNativeCoefficientDigits includes supplied leading zeroes, excludes sign and is enforced before normalization. Existing separate query digit limits can refuse a read without poisoning export/smaller reads. Legacy decimal facade remains decimal-only. Integer assertions, derived families and named integer restrictions/facets remain excluded. Local qualification is not a new release, official W3C-suite conformance or general API/C45 acceptance; no Core engine/pin/controller/publication change. REQ-009 remains In Progress.

## M2D1 named string restriction candidate - 19 November 2026

M2C3 merged in PR22 at014cb28; fresh main CI44609515923 passed748source cases,54release controls and6workload controls. The [named string candidate](m2d-string-restrictions/README.md) adds opt-in direct xs:string restrictions with length/minLength/maxLength and enumeration.837source cases(355implementation/482public)pass locally. Length counts Unicode scalar values; UTF16leaf/enumeration representation limits stay distinct. Exact ordinal enumeration preserves parsed whitespace and Unicode sequence, including XML-normalized facet attributes; duplicate entries are admitted under a16entry ceiling. Named original typed facts/provenance remain; no public API member change. Legacy decimal facade, decimal sibling constraints and assertions remain unchanged. Pattern/regex, whitespace overrides, restriction chains and string assertions remain excluded. REQ-009 stays In Progress; no Core engine/pin or release/controller/publication change or general API/C45 acceptance.

## M2E1 root attribute contract proposal - 19 November 2026

M2D1 merged in PR23 at098fdaf; fresh main CI44614488088 passed837source cases,54release controls and6workload controls. The [root attribute contract](m2e-attributes/CONTRACT.md) and [ADR012](ADR-012-owned-root-attribute-reads.md) propose required/optional local root attributes with separate AttributeCount/ReadAttribute and detached namespace/local-name/scalar entries, preserving leaf indexes.27public-only synthetic Core probes pass on each signed1.81.2/1.81.17 lane, including original attribute facts/provenance and foreign/lifetime refusals. Those overlapping feasibility controls are not new Schema cases or attribute validation. Product source/API/pins and delivered837suite remain unchanged. New API/semantic adoption, actual native validation/atomization and consumer/retained qualification remain required; general API/C45 acceptance and registry publication are not inferred. REQ-009 stays In Progress.

## M2E1 root attribute implementation candidate - 19 November 2026

The [adopted root attribute slice](m2e-root-attributes/README.md) implements the concrete ADR012 proposal locally on098fdaf.950source cases(372implementation/578public)pass, including all837prior outcomes unchanged. Only opt-in local required/optional root attributes are admitted; separate AttributeCount/ReadAttribute and detached NativeAttributeValue name/scalar entries preserve existing leaf indexes. Declaration-order indexing skips absent optionals, distinguishes false/zero/empty, respects namespace forms/XML normalization and uses original native typed facts/provenance. Scalar representation/query/cache and ownership/cancellation/cleanup/export rules remain. Legacy decimal facade, no-attribute root refusal and leaf-attribute exclusion stay unchanged. Defaults/fixed/global refs/groups/wildcards/attribute assertions remain excluded. Local qualification is not hosted CI/new publication or general API/C45 acceptance; no Core engine/pin/controller change. REQ-009 remains In Progress.

## Decimal simple-content root candidate - 20 November 2026

PR24 merged atd4eca99; main CI44689935935 passed950source/54release/6workload controls. The [adopted decimal simple-content slice](decimal-simple-content/README.md) locally adds HasRootScalar/ReadRootScalar, preserving leaf/attribute indexes.1036source cases(386implementation/650public)pass with all950prior outcomes unchanged. Direct xs:decimal/admitted named decimal bases and current root attributes support the isolated unchanged PriceType; the full orders fixture still refuses its other unsupported definitions. Complex root annotation/governing provenance stays distinct from actual decimal atomic type; complex status is independent of no-typed-value, and root string-value uses schema whitespace collapse. Existing element-only FOTY0012, original facts, capacity/cache/gate/disposal and detached export semantics remain. Restriction/complex-base chains, other base families, nested/mixed content and simple-content assertions remain excluded. REQ-009 stays In Progress; no Core engine/pin, new release or general API/C45 acceptance.


## Bounded nested-sequence proposal - 20 November 2026

PR25 merged at c34e9fe; fresh main CI44695518279 passed 1,036 source cases,
54 release controls and six workload controls. The [nested-sequence proposal](nested-sequences/PLAN.md)
defines acyclic named/anonymous element-only nesting, finite occurrences and an
internal terminal-scalar projection preserving existing flat indexes. Proposed
limits are depth 4, 64 expanded element nodes and 16 terminal scalar occurrences
per root. Root attributes remain separate; nested attributes/simple content,
choice/all, recursion, unbounded repetition and nested-profile assertions remain
excluded. Planning only: no new implementation, API, semantic adoption, passed
feature count, Core pin or publication. REQ-009 remains In Progress.


## Nested transport feasibility and semantic adoption - 20 November 2026

The [nested dossier](nested-sequences/FEASIBILITY.md) records 50 public-only synthetic controls per signed
Core1.81.2/1.81.17 lane (13 new, 37 inherited), 18 package signature/source/payload
checks and an actual pre-implementation Schema refusal. These are transport and
baseline controls, not new product cases or nested validation. A separate exact
Structures/XDM adoption now precedes production changes. All production files,
the 1,036-case source catalog, public API and Core pins remain unchanged. Actual
nested assessment and terminal-only projection implementation are next; REQ-009
remains In Progress. No commit/push or publication.


## Nested-sequence implementation candidate - 20 November 2026

The [adopted nested-sequence candidate](nested-sequences/README.md) locally implements acyclic named/
anonymous element-only nesting, finite occurrence rules and separate terminal
scalar reads. Existing root attributes/scalar datatypes are reused; complete
original element facts include containers, while ScalarCount/ReadScalar exclude
them. All 1,036 prior source outcomes remain unchanged. The full local suite is
1,125 cases (395 implementation/730 public), including 76 authored nested cases,
four public operation controls and nine actual provider/lifecycle controls.
Depth 4, 16 terminal scalars, defensive 64 element nodes and 16 local anonymous
types are explicit profile ceilings. Nested attributes/simple content, choice/all,
recursion, unbounded repetition and assertions in nested schemas remain excluded.
No new public API, Core pin or published package/controller change; REQ-009 and
general API/C45 acceptance remain In Progress. Package/retained/API evidence is
recorded separately in the candidate dossier, not inferred from source tests.


Nested qualification is complete locally: package-only730minimum-Core and
729unchanged current-reference public cases plus one current-version identity
control pass; retained124/23/33callers pass with onlySchemaDLLchanged. API52types/
389rows is unchanged. All ten actual packaged README C#blocks,18Core signatures/
payload identities,54release and six workload controls pass. Exact source hashes
and overlapping lane scopes are in docs/nested-sequences/QUALIFICATION-RESULTS.json.
Local unsigned0.6.0-alpha.0.13 retains basec34e9femetadata; it is not a published
release. No commit/push, tag/controller or Core integration-pin change.
