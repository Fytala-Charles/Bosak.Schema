<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema">
  <h1>Bosak.Schema</h1>
  <p>Apache-2.0 schema integration and staged XSD 1.1 services for .NET.</p>
</div>

## 1. Status and Scope

Bosak.Schema provides Apache-2.0 schema services and bounded native XSD assessment for .NET 10 applications. Supported native workflows validate before exposing original-node typed facts, detached values and XML export, with explicit Invalid/Unsupported outcomes and resource limits.

| Availability | Scope |
|---|---|
| Published `0.5.0-beta.1` | Historical bounded decimal beta; use its release and integration guides |
| Development `0.20`, signed Core `1.86.0` | Broader bounded datatype families, finite structural models, anonymous attribute types, empty groups, original comments/PI, bounded annotations, ID/IDREF validation and declared attributes on empty complex owners, numeric lexical patterns, anyType terminals and bounded known global lax matches; unpublished |

The [current coverage guide](docs/SPECIFICATION_COVERAGE.md) distinguishes native implementation from legacy descriptor helpers and remaining specification gaps. General XSD 1.1 conformance, arbitrary schema-aware query integration and full PSVI/C45 acceptance remain incomplete.

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

The factory helpers above are separate from native-profile admission. For owned native assess/read/export workflows, choose the [development structural guide](docs/known-lax/GUIDE.md) and [consumer integration guide](docs/INTEGRATION.md); development APIs and scope are not a promise about the published beta.

## 3. Build and Test

```powershell
dotnet build Bosak.Schema.slnx -c Release
dotnet test Bosak.Schema.slnx -c Release
pwsh tools/check-docs.ps1 -ProjectPath .
```

The public solution needs the .NET 10 SDK; it has no Azure, payment-service, key-generation or Node/Azurite test dependency. For release verification and historical publication evidence, see the [release guide](docs/BETA1_RELEASE.md).

## 4. Documentation and Ecosystem

- [Architecture](docs/ARCHITECTURE.md) and [integration](docs/INTEGRATION.md)
- [XSD coverage](docs/SPECIFICATION_COVERAGE.md), [roadmap](ROADMAP.md) and [feature registry](docs/FEATURE_REQUESTS.md)
- [Datatype and structural guides](docs/markup-coverage/README.md) and [beta readiness](docs/markup-coverage/BETA-READINESS.md)
- [Development history](docs/DEVELOPMENT_HISTORY.md) for dated engineering checkpoints

Bosak.Schema is the public schema engine. Bosak.Licensing and Bosak.Folio are separate products; private credentials, issuance and billing implementations are outside this repository.

## 5. Compatibility and License

Existing schema factory signatures remain intact. The old `License` entry points remain compatibility shims: activation calls do nothing, and status exposes a perpetual Apache marker rather than a commercial entitlement. Signing, credential verification and issuance implementations are not shipped. See [migration details](docs/APACHE-TRANSITION.md).

Fytala retains copyright and trademark ownership; Apache-2.0 grants the rights stated in the license. Branding does not grant trademark rights. The new license applies to this public source release; old proprietary releases and private licensing-product agreements are separate historical artifacts.

*Last updated: 2026-11-26*
