<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema">
  <h1>Bosak.Schema</h1>
  <p>Apache-2.0 schema integration and staged XSD 1.1 services for .NET.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](docs/PUBLICATION.json). Historical GitHub transition receipt; the current NuGet release is recorded below.

# Bosak.Schema

> Apache-2.0 schema integration and staged XSD 1.1 services for .NET.


## 1. Status and Scope

Bosak.Schema is free, open-source Fytala software under [Apache-2.0](LICENSE.md). Schema compilation and validation require no activation, license key, trial, subscription or billing service. This public repository contains only the schema engine, its tests and documentation.

The implementation targets .NET 10 and references published Bosak 1.81.2 packages after the merged dependency adoption. It provides schema-aware XSLT compiler helpers and a staged XSD 1.1 component/assertion pipeline. Complete XSD 1.1 structural validation and integration of its full component universe into running Bosak expressions are **not yet implemented**. See the [support matrix](docs/FEATURE_REQUESTS.md).

The initial public source is derived from Fytala's private schema source at `fa0da3e2d9972706a5a86616ef285af4daf5037b`, with the owner-approved Apache transition on 29 October 2026. Billing, key-generation, deployment material and private Git history remain outside this repository. See [provenance](docs/APACHE-TRANSITION.md).

The dependency upgrade is merged in [Schema PR #1](https://github.com/Fytala-Charles/Bosak.Schema/pull/1). The adoption baseline retained **124 standalone cases**; the separate historical [Core optional integration](https://github.com/Fytala-Charles/Bosak/blob/main/docs/schema-integration-1812/README.md) exercised **135 cases**, including those public Schema tests and bounded price/ownership controls. The public entry point uses the **0.5 development line**; no new Schema NuGet release is announced. See [dependency adoption](docs/CORE-1.81.2-ADOPTION.md) and the [roadmap](ROADMAP.md).

The [owned decimal session API](docs/NATIVE-DECIMAL-PUBLIC-API.md) supports native assessment of a finite decimal-only schema profile, exact scalar reads and detached XML export. Callers supply explicit limits and assessment context. It is merged and consumer-qualified; complete XSD 1.1 and broader provider acceptance remain pending. See [package-delivery status](docs/package-0.5-review/README.md).

## 2. Quick Start

```csharp
using Bosak.Schema;

var schemaCompiler = SchemaServices.CreateSchemaCompiler();
var validator = SchemaServices.CreateValidationService();
var xsltCompiler = SchemaAwareXslt.CreateCompiler();
```

Select XSD 1.0 or XSD 1.1 explicitly with `SchemaCompileOptions.Default(XsdLanguageVersion.Xsd10)` or `Xsd11`. Resource imports/includes use the caller's explicit resolver policy. The default resolver does not fetch external resources.

The `SchemaAwareXslt` helper explicitly enables the existing schema-aware compiler path. Ordinary Bosak compiler defaults remain unchanged. Full examples and limitations are in [integration](docs/INTEGRATION.md).

## 3. Build and Test

```powershell
dotnet build Bosak.Schema.slnx -c Release
dotnet test Bosak.Schema.slnx -c Release
pwsh tools/check-docs.ps1 -ProjectPath .
```

The public solution needs the .NET 10 SDK; it has no Azure, payment-service, key-generation or Node/Azurite test dependency. The transition passed all 114 public schema tests with zero failures/skips. No package was published to NuGet as part of the repository migration.

## 4. Repository Layout

| Path | Purpose |
|---|---|
| `src/Bosak.Schema` | Schema compilation, models, validation and activation-free integration |
| `src/Bosak.Schema/Assessment` | Public owned decimal workflow over the internal finite backend, Core adapter and sequential runner |
| `tests/Bosak.Schema.Tests` | Schema and Apache compatibility regressions |
| `tests/Bosak.Schema.PublicConsumer.Tests` | Public contract consumer without internal access; source and local-package lanes |
| `tests/TestData/Schemas` | Locally authored schema fixtures |
| `docs` | Architecture, integration, roadmap and transition decisions |
| `tools` | Build and documentation checks |

NuGet packaging uses the separate [package README](docs/NUGET_README.md) with absolute HTTPS links; this repository overview retains its normal local documentation links. See [trusted-publishing preparation](docs/TRUSTED_PUBLISHING.md) for the manual prerelease delivery route.

## 5. Compatibility and License

Existing schema factory signatures remain intact. The old `License` entry points remain compatibility shims: activation calls do nothing, and status exposes a perpetual Apache marker rather than a commercial entitlement. Signing, credential verification and issuance implementations are not shipped. See [migration details](docs/APACHE-TRANSITION.md).

Fytala retains copyright and trademark ownership; Apache-2.0 grants the rights stated in the license. Branding does not grant trademark rights. The new license applies to this public source release; old proprietary releases and private licensing-product agreements are separate historical artifacts.

*Last updated: 2026-11-17*


**Current release — 17 November 2026:** [Bosak.Schema 0.5.0-alpha.0.7](https://www.nuget.org/packages/Bosak.Schema/0.5.0-alpha.0.7) is published and independently verified. Beta readiness remains proposed; full XSD 1.1/API freeze/performance acceptance is not implied.

Schema beta readiness is now the next priority before Braid/XmlDb adoption. The [beta gap assessment](docs/beta-readiness/ASSESSMENT.md) proposes a bounded decimal-session beta and distinguishes a broader validator target. Scope and public-contract decisions remain pending; no beta version or semantic change is made.


The local [restricted-resolution APIs](docs/RESTRICTED_RESOLUTION.md) add an explicit bounded file resolver and a controlled schema-only XSLT factory. Canonical containment, reparse-point refusal, full XML/DTD checks and published Core policy cover direct/nested schema reads. Stable host-owned filesystem paths are required; this is not an atomic sandbox.


Compiler-produced [descriptor snapshots](docs/DESCRIPTOR_SNAPSHOTS.md) now own immutable outer/nested collections. Caller-created positional records retain their existing ownership behavior. This is local beta-readiness work; the published alpha and legacy validation meanings are unchanged.


[Legacy validation](docs/LEGACY_VALIDATION.md) now refuses models with compile diagnostics before assertion/facet/static-check work, preserving original findings. For clean models, success remains operation-scoped and does not certify structural validity. The published alpha remains unchanged; this source fix is not a beta release.


The next beta targets the bounded decimal-session profile with explicit exclusions. See [beta qualification and workload reproduction](docs/BETA_QUALIFICATION.md) for the supported boundary and diagnostic tool. Beta contract acceptance and publication remain pending.
