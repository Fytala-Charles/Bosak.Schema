# Beta.1 release preparation

17 November 2026. The owner requested preparation steps1–4 after accepting the bounded decimal-session contract. PR12 is merged at `2a5abf6fb663313dc32bfeca5bbb8740bbe987ae`; main44174792275 passed. The beta controller and documentation are a local review candidate. [Evidence](beta1-preparation/README.md), [bounded contract](BETA_CONTRACT_REVIEW.md), [workload scope](BETA_QUALIFICATION.md).

## Exact source and tag plan

Proposed product tag **v0.5.0-beta.1** must resolve to `2a5abf6fb663313dc32bfeca5bbb8740bbe987ae`. No remote beta tag exists at verification time. A tag exists only inside the isolated local qualification clone. Ordinary MinVer builds **0.5.0-beta.1**, with no Version override or renamed alpha. Public-tag delivery requires explicit owner instruction; preserve its exact target. The controller is delivered in a later commit and is not the tagged product source.

After controller review/delivery and main checks, publish the reviewed Git tag at the exact product commit, then dispatch release.yml with publish=false. Tag creation alone does not dispatch this manual workflow. Download and review the new hosted package/manifest before any NuGet publication decision. The existing trusted-publishing policy uses the same repository/workflow; no new account configuration is requested.

## Local artifact review

Fresh unsigned package: **Bosak.Schema.0.5.0-beta.1.nupkg**, **93,938 bytes**, SHA-256 `7241d41d7e5eba3b56833c89a2917dd656b123420febb1789ed7cadc0914b5ef`. Local manifest SHA-256 `26d833ce01439b806a176def98d87753b864b6bb00aee465687ec174b4a624af`. The local manifest has an all-zero controller placeholder and is not eligible hosted preparation evidence. A hosted archive can have different bytes and requires its own hash review.

Source345 cases (312 implementation/33 external), fresh package-only33 cases, retained legacy124 control/replacement and retained published-alpha23 control/replacement all pass. Only Schema DLL changes in retained layouts; exact catalogs/test/configuration/fixture/dependency inventories are preserved. The new hosted alpha-baseline restore/build/signature/retained23 steps also pass in a separate fresh registry-only local rehearsal. These catalogs overlap. All nine Core1.81.2 signatures and archive/cache/runtime payload identities pass. Reflection matches accepted40 types/317 rows. Controller/product/workload builds have zero warnings/errors;32 Python +22 GitHub provenance controls and six workload-runner controls pass.

## Admission and remaining delivery

The controller requires the exact clean product source, correct beta tag, Apache2 legal/README/XML documentation, net10.0, unchanged four minimum Core1.81.2 references, package-only consumer and nine signed Core payloads. It now admits345/33 cases and both124/23 retained lanes. Published-alpha baseline comes from fixed source1783d1a and the exact repository-signed alpha archive hash; it cannot silently use the new source or beta package. Missing/wrong tag, partial catalogs, wrong alpha bytes/signature, package mutation and source/controller/artifact drift refuse. OIDC remains confined to the separate publish job; no rebuild/skip-duplicate is introduced there.

Pending: explicit commit/push/PR instruction for this controller, reviewed remote tag, successful hosted preparation and exact hosted-artifact review. NuGet publication requires separate approval of the selected hosted run, package/manifest hashes and destination. No beta is published by this step. Accepted scope remains bounded decimal sessions; full XSD1.1, general CoreC45/ADR006, production SLAs and a perpetual API freeze remain excluded. Historical alpha/contract/qualification dossiers are unchanged.
