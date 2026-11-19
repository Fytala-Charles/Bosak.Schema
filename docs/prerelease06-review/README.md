# Next prerelease version and controller review

19 November 2026. Product development baseline:
`3ff8cb6d6a0f14bf81f6e0f25a498c301580dcdc`, merged diagnostic PR #15.
[Immutable inspected inputs](BASIS.json).

**Recommend `0.6.0-beta.1`.** The additive public diagnostic property/enum moved
the development minimum to0.6 under the repository's feature-versioning rule.
Do not rename the earlier0.6alpha package to0.5.0-beta.2 or reuse beta.1 bytes.
This review proposes a version; it does not reserve it or authorize publication.

The [NuGet index check](REGISTRY.json) found only0.5.0-alpha.0.7 and0.5.0-beta.1.
The [exact remote-tag query](TAG-CHECK.txt) returned no v0.6.0-beta.1 ref.
These are point-in-time observations and must be repeated before a tag/push.

## Review findings

| Finding | Current state | Required disposition before release preparation |
|---|---|---|
| Source/version admission | Workflow/controller admit2a5abf6 and0.5.0-beta.1 only | Select a final immutable product commit and align exact pins; do not replace checks with arbitrary dispatch inputs |
| Qualification catalogs | Controller seals345source/33package cases | Require365source as312+53 and53package cases; exact source/public catalog equality remains mandatory |
| Latest published compatibility | Existing retained lanes cover legacy124 and published alpha23 | Retain both; add registry-only published beta.1 consumer33 control/replacement with fixed source and signed package identity |
| Packaged README |0.5intro and immutable9fd4e6a contract links omit UnsupportedReason | Update packaged consumer guidance for the0.6API before selecting the final product pin |
| Manifest profile | Exact version1/beta.1 qualification profile | Propose version2 with the added beta.1 lane, strict equality and rejection of old/incomplete profiles |

The existing controller must not be dispatched as a new-version release tool.
Its fixed admission is a safety boundary, not a defect to bypass. No controller
or workflow is modified in this review.

## Concrete version/source proposal

- Version: **0.6.0-beta.1**; proposed tag: **v0.6.0-beta.1**.
- Product implementation baseline: **3ff8cb6d6a0f14bf81f6e0f25a498c301580dcdc**.
- Final release source: **not yet selected**. First merge/review the packaged
  README update, verify that product source/tests/dependencies are unchanged,
  then record that exact40-character commit. Do not use a moving main ref.
- Controller: later independent reviewed commit, distinct from the tagged
  product source. Record both in the hosted preparation manifest.
- Core dependencies: retain all four declared minimum1.81.2 references and
  verify all nine signed runtime package archives/cache/DLLs.
- No new grammar, API freeze, general provider/C45 acceptance or SLA implied.

The README update should explain `.UnsupportedReason`, the seven fixed enum
values, Unspecified/future-value tolerance, first-detected/non-exhaustive scope,
unchanged stage codes/messages and the namespaced-instance Invalid boundary.
Keep the existing sample valid and explicitly say that published beta.1 does not
have the new property. Point documentation to an immutable revision containing
the merged contract; existing sample/license provenance stays unchanged.

## Exact controller change map

| File | Proposed bounded change |
|---|---|
| `.github/scripts/schema_release.py` | Replace SOURCE/VERSION/TAG with reviewed new values; source catalogs[312,53], consumer53, source total365. Add beta.1 constants and identity/retained lane. Seal and verify a strict version2 qualification object. Preserve all existing archive/legal/dependency/provenance/absence checks. |
| `.github/workflows/release.yml` | Align SOURCE_COMMIT/PACKAGE_VERSION; add separately pinned beta.1 consumer checkout and registry-only restore/signature/control/replacement steps. Retain legacy and alpha steps. Preparation stays default; publish continues to download and verify exact reviewed bytes. |
| `.github/scripts/test_schema_release.py` | Update synthetic catalogs; add missing/partial/wrong-beta-baseline and old-manifest-profile refusals. Maintain old refusal cases, never reduce counts to accommodate failures. |
| `.github/scripts/schema-source-run.cjs` and its tests | Keep first-attempt/same-main-workflow/controller/artifact guards. Add coverage that old beta.1 prepared content cannot satisfy the new Python source/version/profile checks; avoid unrelated refactoring. |

### New published-beta.1 lane

Pin all three independently, from the original publication receipt:

| Identity | Fixed value |
|---|---|
| Beta.1 version |0.5.0-beta.1 |
| Consumer source |2a5abf6fb663313dc32bfeca5bbb8740bbe987ae |
| Repository-signed NuGet archive SHA-256 |045fe07dda92dc19aeb954c4c2a99fe936ffc0d4202c29ccd59b0c42582cca70 |
| Original external public consumer catalog |33cases |

Restore/build that consumer against the published package with NuGet.org only,
no project references or candidate feed. Verify signature plus archive/cache/
runtime equality. Copy the exact resulting binary layout twice, run control
and candidate after replacing only Schema.dll, compare complete test catalogs
and all other file hashes. An unsigned preparation-package hash is not the
published baseline hash. Current53case source tests are not a retained33case
binary baseline. Keep unique lane/evidence/cache names to avoid alpha collisions.

Require the beta baseline's33case identity in both manifest sealing and later
publication verification, alongside the existing124/23lanes. Missing, mismatched,
partial, unsigned or substituted baseline evidence must refuse.

## Guards to retain and prove

1. Clean exact product commit, main ancestry, same repository and matching
   immutable annotated/peeled tag; no source or Version override.
2. One bounded package, safe ZIP entries, exact Apache/license/NOTICE/README/XML
   documentation, net10.0, only the Schema DLL and fixed dependency minima.
3. Exact source/public catalogs365/53; unchanged124/23lanes plus new33lane;
   nine Core signatures and payload identities. Catalogs overlap, never add them.
4. First-attempt successful main workflow preparation; exact controller SHA,
   artifact SHA and manifest/package bytes; reject old/incomplete provenance.
5. OIDC only in publish, publication defaults to false, no rebuild during publish,
   no skip-duplicate and registry absence checked immediately before a push.
6. Local mocks cannot approve publication. Remote tag, hosted preparation,
   exact hosted artifact approval and registry publication remain separate actions.

The existing fifteen-minute preparation timeout needs observation with the
third retained lane; this review does not increase it speculatively.

## Recommended sequence

1. Review this version/controller proposal and update the packaged README.
2. Deliver that documentation change; select/review its immutable product pin.
3. Implement the mapped controller/test changes with no remote tag or publish.
4. Rehearse in an independent local clone with an isolated local tag and normal
   MinVer. Run365source/53package,124/23/33retained catalogs, exact package/API/
   legal/Core identities and release/refusal controls. Preserve old receipts.
5. Review/deliver the controller and verify main. Only then consider the exact
   remote tag and `publish=false` hosted preparation, followed by a separate
   decision about exact artifact bytes and NuGet.org publication.

## Verification boundary

Static review/documentation only. No runtime, package README, controller,
workflow, dependency or version property was changed here. No local/remote tag,
package build, release dispatch, login or push. Product qualification remains
the fresh passing365/54/6main CI44504735125 from PR15; it is not a new test run.
Historical dossiers/receipts remain unchanged. Braid licensing is independent.
