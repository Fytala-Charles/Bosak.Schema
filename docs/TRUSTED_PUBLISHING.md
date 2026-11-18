# Schema NuGet trusted-publishing setup

18 November 2026. Beta.1 was published and independently verified using the configured trusted publisher. [Current release and exact receipt](BETA1_RELEASE.md). Future versions need their own reviewed controller, artifact and approval. [Scope and evidence](trusted-publishing/README.md).

## Owner configuration after workflow review

Following [Microsoft's NuGet trusted-publishing setup](https://learn.microsoft.com/en-us/nuget/nuget-org/trusted-publishing), use the NuGet account or organization intended to own Bosak.Schema. Confirm rights to create/publish that package ID; a404 registry response does not establish those rights.

| NuGet policy field | Value for this workflow |
|---|---|
| Repository owner | Fytala-Charles |
| Repository | Bosak.Schema |
| Workflow file | release.yml (filename only) |
| Environment | Leave blank; this workflow declares no GitHub environment |
| Package pattern | Bosak.Schema |
| Scopes | Push new packages and push new versions for this package |
| Policy owner | The intended individual/organization NuGet owner; confirm active rights/status |

Set the **NUGET_USER repository Actions variable** to that NuGet profile name, not an email/password/API key. This is an account identifier, not the publishing credential. Do not send a password, API key or OIDC token in chat. The policy belongs to the Schema repository; Core's existing policy does not authorize this different repository. The existing alpha policy and NUGET_USER=Charles_Korthout were exercised successfully; no new credentials or policy changes are needed for this beta preparation.

The workflow obtains a short-lived key using NuGet/login@v1 only in the publish job. Only that job has id-token:write. Do not rename release.yml without reviewing the NuGet policy. If an environment is introduced later, its name must also match the policy; none is configured by this candidate.

## Preparation, review, publication

1. After reviewing/merging the workflow and completing hosted build/test/docs/controls, dispatch **Schema release** on main with publish=false. Preparation is the default and performs no NuGet login/push.
2. The delivered beta controller admits accepted product source2a5abf6fb663313dc32bfeca5bbb8740bbe987ae, tagv0.5.0-beta.1 at that commit and ordinary MinVer version0.5.0-beta.1. Beta.1 controller/tag delivery is complete; this exact version is already published and must not be pushed again. It requires345source/33package-only cases, retained124legacy and23published-alpha cases per control/replacement lane, plus nine Core1.81.2 signatures/payloads, then seals one package/manifest. These catalogs overlap. [Exact plan and local artifact](BETA1_PREPARATION.md).
3. Download the successful first-attempt preparation run's schema-nuget artifact, verify its artifact digest, manifest source/controller/product identities, exact package bytes, case receipts and exclusions. Review the resulting manifest SHA-256 and package SHA-256. Hosted builds may differ from the earlier locally prepared archive; preserve both receipts and obtain a new exact-artifact decision. Never assert that a rebuild retained the local digest without checking it.
4. Obtain explicit publication approval for the selected run, version, manifest hash, package digest and NuGet.org destination. Confirm configured policy/owner rights and recheck registry absence.
5. Dispatch the workflow on main with publish=true, source_run_id=<reviewed successful preparation run>, expected_manifest_sha256=<reviewed manifest digest>. The publisher rejects PR/fork/non-main/wrong-workflow/failed/rerun/publish-source runs, incomplete metadata and expired/duplicate artifacts before download. It verifies all files/source/dependencies/manifest and registry absence before OIDC, and repeats file/registry verification immediately before pushing one exact package. It never rebuilds, uses a wildcard package glob or suppresses duplicate conflicts.
6. Retain the publisher run and verify NuGet listing, repository signature and package payload afterward. NuGet repository signing may add a signature entry, so compare reviewed contents separately from the outer signed archive hash.

No dispatch or publication is executed by this documentation. A failed preparation is retained; start a separately reviewed new preparation rather than treating a rerun as an admitted publication source. Artifact retention is30days; expired artifacts are refused. The workflow has finite job/process watchdogs, not workload performance acceptance.

## Local qualification and limits

Run the54 release controls using the commands in AGENTS.md, full solution build/test, and `pwsh tools/check-docs.ps1 -ProjectPath . -Strict`. Synthetic control ZIP/TRX files are not product executions; network/GitHub responses are mocked and no test logs in or pushes. The separate real package rehearsal verifies the preserved local package but uses an invalid-for-hosted all-zero controller placeholder; it is not a successful GitHub preparation and cannot be selected by the publisher.

SDK10.0.301, productCore1.81.2 minima and source pin remain explicit. No engine C# changes, package/tag/version publication, API/foundation acceptance, full XSD1.1 claim, broad concurrency guarantee or performance budget is introduced by this operational route. Original package review/preparation dossiers remain immutable historical receipts.

## Failed preparation diagnostics

The first hosted preparation failed at content sealing; no publishable artifact was admitted. [Follow-up evidence](preparation-diagnostics/README.md) records the limits of the diagnosis. The local diagnostic candidate reports rejected entry names and retains a distinct failure-only `schema-rejected-package-<run>-<attempt>` artifact for seven days. Such an artifact is unqualified, may lack a manifest and must never be supplied as a publication source. It has a different name from `schema-nuget-<controller SHA>`; failed preparations remain refused regardless of retained bytes. Inspect it before changing any allowlist. A new successful first-attempt preparation and exact-artifact review are still required.


The diagnostic run identified the exact standard metadata path `package/services/metadata/core-properties/nuget.psmdcp`. The [local verifier correction](package-metadata-correction/README.md) permits this filename alongside generated names; it does not admit other payload paths or make failed preparations eligible. A corrected local rehearsal of retained bytes is evidence only. Start a new preparation after the correction is reviewed and merged.
