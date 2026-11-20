<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema">
  <h1>Bosak.Schema</h1>
  <p>Apache-2.0 schema integration and staged XSD 1.1 services for .NET.</p>
</div>

# Bosak.Schema

> Apache-2.0 schema integration and staged XSD 1.1 services for .NET.


## 1. Status and Scope

**Current release — 18 November 2026:** [Bosak.Schema 0.5.0-beta.1](https://www.nuget.org/packages/Bosak.Schema/0.5.0-beta.1) is published and independently verified. [GitHub prerelease](https://github.com/Fytala-Charles/Bosak.Schema/releases/tag/v0.5.0-beta.1), [scope and publication receipt](docs/BETA1_RELEASE.md).

Bosak.Schema is free, open-source Fytala software under [Apache-2.0](LICENSE.md). Schema compilation and validation require no activation, license key, trial, subscription or billing service. This public repository contains only the schema engine, its tests and documentation.

The implementation targets .NET 10 and references published Bosak 1.81.2 packages after the merged dependency adoption. It provides schema-aware XSLT compiler helpers and a staged XSD 1.1 component/assertion pipeline. Complete XSD 1.1 structural validation and integration of its full component universe into running Bosak expressions are **not yet implemented**. See the [support matrix](docs/FEATURE_REQUESTS.md).

The initial public source is derived from Fytala's private schema source at `fa0da3e2d9972706a5a86616ef285af4daf5037b`, with the owner-approved Apache transition on 29 October 2026. Billing, key-generation, deployment material and private Git history remain outside this repository. See [provenance](docs/APACHE-TRANSITION.md).

The [owned decimal session API](docs/NATIVE-DECIMAL-PUBLIC-API.md) supports native assessment of a finite decimal-only schema profile, exact scalar reads and detached XML export. Callers supply explicit limits and assessment context. It is published in this bounded beta; complete XSD 1.1 and broader provider acceptance remain pending. [Restricted resolution](docs/RESTRICTED_RESOLUTION.md), [descriptor ownership](docs/DESCRIPTOR_SNAPSHOTS.md) and [legacy validation](docs/LEGACY_VALIDATION.md) retain their explicit limits.

## 2. Quick Start

```sh
dotnet add package Bosak.Schema --version 0.5.0-beta.1
```

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

The public solution needs the .NET 10 SDK; it has no Azure, payment-service, key-generation or Node/Azurite test dependency. For release verification and historical publication evidence, see the [release guide](docs/BETA1_RELEASE.md).

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

*Last updated: 2026-11-19*

See the [next-beta assessment](docs/beta2-gap-assessment/README.md) for proposed diagnostic improvements and the evidence required before expanding the native grammar.

A [development candidate for clearer Unsupported diagnostics](docs/unsupported-reasons/README.md) reports one detected category while retaining the bounded grammar. Published beta.1 remains unchanged.

The [shared specification coverage programme](docs/specification-coverage/PROGRAM.md) defines the next Schema/Core interoperability milestones and the evidence required for supported profiles.

The [shared coverage programme](docs/specification-coverage/PROGRAM.md) now records a local namespace/optional-assertion candidate with separate minimum/current-Core consumers; see its [bounded progress](docs/specification-coverage/M1-CANDIDATE-DELTA.json).

The [packaged0.6guidance](docs/package-readme06-reconciled/README.md) distinguishes the new diagnostic/namespace profile from published beta.1 and verifies consumer examples.

The development profile also supports [reusable named complex types](docs/m2a-named-types/README.md) within the bounded decimal workflow. Named simple types, derivation, attributes and imports remain excluded. Published beta.1 retains its original scope.

The development profile also admits [named decimal restrictions and selected facets](docs/m2b-decimal-restrictions/README.md) for reusable monetary values. Other datatypes, restriction chains, attributes and imports remain outside this bounded native profile.

The local development profile adds an [opt-in string/decimal owned facade](docs/m2c-string-facade/README.md) with checked detached scalar values and bounded XML export. The published beta and existing decimal-only API retain their original scope.

The opt-in scalar development profile also assesses [primitive Boolean leaves](docs/m2c-boolean/README.md), preserving actual true/false values through Core. Boolean assertions and custom Boolean restrictions remain excluded.

The opt-in development facade also supports [bounded exact integer leaves](docs/m2c-integer/README.md). Published beta.1 retains its documented scope.

The opt-in development facade also supports [named string length and enumeration restrictions](docs/m2d-string-restrictions/README.md).

The opt-in development facade supports [bounded root attributes with separate typed reads](docs/m2e-root-attributes/README.md), preserving leaf indexes and the published beta.1 boundary.

The opt-in development facade supports [decimal simple-content roots with separate typed reads](docs/decimal-simple-content/README.md), including price/currency documents.

See the [bounded nested-sequence development profile](docs/nested-sequences/README.md) for terminal scalar reads and explicit structural limits.

See [owner-aware nested attributes](docs/nested-attributes/README.md) for detached descendant values and document-relative owner paths.

See [nested decimal simple content](docs/nested-simple-content/README.md) for price/currency terminals through existing scalar and owner-aware attribute reads.

The [upstream validity pilot](docs/conformance-pilot/README.md) records current
interoperability gaps and the limits of conformance evidence.

The opt-in development profile accepts [unfetched schema-location hints](docs/schema-location-hints/README.md)
while retaining the explicitly supplied schema. Published beta.1 is unchanged.
