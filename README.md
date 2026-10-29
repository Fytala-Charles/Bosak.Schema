<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema">
  <h1>Bosak.Schema</h1>
  <p>Apache-2.0 schema integration and staged XSD 1.1 services for .NET.</p>
</div>

# Bosak.Schema

> Apache-2.0 schema integration and staged XSD 1.1 services for .NET.


## 1. Status and Scope

Bosak.Schema is free, open-source Fytala software under [Apache-2.0](LICENSE.md). Schema compilation and validation require no activation, license key, trial, subscription or billing service. This public repository contains only the schema engine, its tests and documentation.

The implementation targets .NET 10 and currently references published Bosak 1.26.3 packages. It provides schema-aware XSLT compiler helpers and a staged XSD 1.1 component/assertion pipeline. Complete XSD 1.1 structural validation and integration of its full component universe into running Bosak expressions are **not yet implemented**. See the [support matrix](docs/FEATURE_REQUESTS.md).

The initial public source is derived from Fytala's private schema source at `fa0da3e2d9972706a5a86616ef285af4daf5037b`, with the owner-approved Apache transition on 29 October 2026. Billing, key-generation, deployment material and private Git history remain outside this repository. See [provenance](docs/APACHE-TRANSITION.md).

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
| `tests/Bosak.Schema.Tests` | Schema and Apache compatibility regressions |
| `tests/TestData/Schemas` | Locally authored schema fixtures |
| `docs` | Architecture, integration, roadmap and transition decisions |
| `tools` | Build and documentation checks |

## 5. Compatibility and License

Existing schema factory signatures remain intact. The old `License` entry points remain compatibility shims: activation calls do nothing, and status exposes a perpetual Apache marker rather than a commercial entitlement. Signing, credential verification and issuance implementations are not shipped. See [migration details](docs/APACHE-TRANSITION.md).

Fytala retains copyright and trademark ownership; Apache-2.0 grants the rights stated in the license. Branding does not grant trademark rights. The new license applies to this public source release; old proprietary releases and private licensing-product agreements are separate historical artifacts.

*Last updated: 2026-10-29*
