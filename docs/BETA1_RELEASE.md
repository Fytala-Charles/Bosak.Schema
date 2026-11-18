# Bosak.Schema 0.5.0-beta.1

Published and verified on **18 November 2026**. Available on [NuGet.org](https://www.nuget.org/packages/Bosak.Schema/0.5.0-beta.1) and as a [GitHub prerelease](https://github.com/Fytala-Charles/Bosak.Schema/releases/tag/v0.5.0-beta.1). The GitHub prerelease includes the exact NuGet repository-signed package and its SHA-256 checksum. GitHub Packages is a separate registry and was not used.

## Install and use

```sh
dotnet add package Bosak.Schema --version 0.5.0-beta.1
```

Requires .NET 10. Four declared Core dependencies have minimum version 1.81.2; qualification used nine resolved Core 1.81.2 packages. Compatibility with every later dependency version is not implied. Start with the [owned native decimal API](NATIVE-DECIMAL-PUBLIC-API.md), [package workflow example](NUGET_README.md), [restricted schema resolution](RESTRICTED_RESOLUTION.md) and [legacy validation scope](LEGACY_VALIDATION.md).

## Accepted scope

This beta provides the owned, synchronous finite decimal-session API, explicit capacities and assessment context, distinct Valid/Invalid/Unsupported results, exact scalar reads and detached XML export. It also includes opt-in restricted schema resolution, immutable compiler-owned descriptors and the legacy compile-diagnostic guard.

Native namespaces, attributes, broader datatypes, nil/list/union and arbitrary XPath remain excluded. Full XSD 1.1 validation, general Core C45/ADR006 acceptance, a perpetual API freeze and production SLAs are not established. Restricted file resolution requires stable host-owned paths; legacy assertion/facet success does not certify structural validity. See [accepted bounded contract](BETA_CONTRACT_REVIEW.md) and [local workload qualification](BETA_QUALIFICATION.md).

## Verified delivery

Product tag `v0.5.0-beta.1` resolves to `2a5abf6fb663313dc32bfeca5bbb8740bbe987ae`; the release controller is `52a4997daba109bc5daec03522e9fe98725b78aa`. [Preparation44244606595](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/44244606595) and [publication 44245557198](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/44245557198) succeeded. Publication pushed the approved artifact without rebuilding.

Unsigned hosted package:93,836 bytes; SHA-256 `d13f07e17bdd5f6306260b48381f72dab613fdbefaa26e45de761f653d8be142`. Reviewed manifest SHA-256 `7be14eaa0db19ed623507ea90063b0c78ed11678f16bfc34e96c796e47ebceb4`.

Repository-signed published archive:106,930 bytes; SHA-256 `045fe07dda92dc19aeb954c4c2a99fe936ffc0d4202c29ccd59b0c42582cca70`. Official signature verification passed; all approved payload entries are byte-identical and only `.signature.p7s` was added. This signed archive is also the GitHub package asset.

345 source cases (312 implementation/33 public),33 package-only cases, retained 124 legacy/23 published-alpha cases per control/replacement lane, and nine Core signature/payload checks passed. A fresh post-publication registry-only 33-case consumer passed with an isolated cache. Catalogs overlap. Builds,54 release controls, six workload integrity controls and documentation checks passed. [Receipt](beta1-publication/README.md).

## Next qualification

Keep the beta contract bounded. Exercise an identified consumer workflow using the published package, record supported and unsupported inputs, and use concrete findings to scope beta.2. A public authored fixture is representative evidence, not production-user acceptance. Braid/XmlDb adoption and broader schema capabilities remain separate work.

Historical review/preparation guides retain their original checkpoint language; this page and the publication receipt define current release status.
