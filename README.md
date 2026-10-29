<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema">
  <h1>Bosak.Schema</h1>
  <p>Apache-2.0 schema integration and staged XSD 1.1 services for .NET.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](docs/PUBLICATION.json). No NuGet release or commercial infrastructure publication.

# Bosak.Schema

> Apache-2.0 schema integration and staged XSD 1.1 services for .NET.


## 1. Status and Scope

Bosak.Schema is free, open-source Fytala software under [Apache-2.0](LICENSE.md). Schema compilation and validation require no activation, license key, trial, subscription or billing service. This public repository contains only the schema engine, its tests and documentation.

The implementation targets .NET 10. This feature candidate requires unpublished Bosak 1.34.1 through the local source override below; the original public checkout retains its published 1.26.3 dependency pins. It provides schema-aware XSLT compiler helpers and a staged XSD 1.1 component/assertion pipeline. Complete XSD 1.1 structural validation and integration of its full component universe into running Bosak expressions are **not yet implemented**. See the [support matrix](docs/FEATURE_REQUESTS.md).

The unpublished 0.8 development candidate adds [native document assessment](docs/DOCUMENT-ASSESSMENT.md): a complete structural/decimal/assertion check for one explicit XSD 1.1 profile, an owned result with original declaration/type state, and whole-document exports for Bosak queries/transforms. Anonymous-root and decimal-child assessment state survives preserve copies; strip removes it. Unsupported coverage, including general declaration matching and compatible-universe interchange, is refused.

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
dotnet build Bosak.Schema.slnx -c Release -p:BosakSourceRoot=D:/Development/Bosak
dotnet test Bosak.Schema.slnx -c Release -p:BosakSourceRoot=D:/Development/Bosak
dotnet run --project examples/Bosak.Schema.Assessment.Example -c Release -p:BosakSourceRoot=D:/Development/Bosak
pwsh tools/check-docs.ps1 -ProjectPath .
```

The public solution needs the .NET 10 SDK; it has no Azure, payment-service, key-generation or Node/Azurite test dependency. The transition passed all 114 public schema tests with zero failures/skips. No package was published to NuGet as part of the repository migration.

## 4. Repository Layout

| Path | Purpose |
|---|---|
| `src/Bosak.Schema` | Schema compilation, models, validation and activation-free integration |
| `tests/Bosak.Schema.Tests` | Schema and Apache compatibility regressions |
| `tests/TestData/Schemas` | Locally authored schema fixtures |
| `examples/Bosak.Schema.Assessment.Example` | Runnable native assessment and typed query/transformation |
| `docs` | Architecture, integration, roadmap and transition decisions |
| `tools` | Build and documentation checks |

## 5. Compatibility and License

Existing schema factory signatures remain intact. The old `License` entry points remain compatibility shims: activation calls do nothing, and status exposes a perpetual Apache marker rather than a commercial entitlement. Signing, credential verification and issuance implementations are not shipped. See [migration details](docs/APACHE-TRANSITION.md).

Fytala retains copyright and trademark ownership; Apache-2.0 grants the rights stated in the license. Branding does not grant trademark rights. The new license applies to this public source release; old proprietary releases and private licensing-product agreements are separate historical artifacts.

*Last updated: 2026-10-29*

## Whole-document bridge candidate — 29 October 2026

The unpublished core 1.34.0 provider bridge now carries original anonymous-root and decimal-child state through whole-document exports and preserve/strip copies. Root atomization raises FOTY0012; stale content and legacy reassessment refuse explicitly. Use `-p:BosakSourceRoot=D:/Development/Bosak` for builds/tests/examples because candidate package dependencies are unpublished. See [the current guide](docs/DOCUMENT-ASSESSMENT.md). The earlier 191-case receipts describe the historical projection-only checkpoint; fresh bridge qualification is recorded separately in the core `docs/schema-document-bridge` dossier. Broader coverage, consumer adoption and API freeze remain pending. No commit/push/tag/publication.

## Reviewed whole-document bridge — 29 October 2026

The optional provider attachment API now checks a replacement completely before removing the previous binding. Failed replacement retains original state and stale-content refusal; failed initial attachment leaves the node unregistered. The trusted host must supply immutable state and the correct assessed value. This API carries that state without performing schema validation. The finite native profile is unchanged; optional children and general API freeze remain pending. Current core candidate is unpublished 1.34.0; use the BosakSourceRoot override. New review qualification lives in the core `docs/schema-document-bridge/api-review` dossier, separately from historical assessment receipts.

The [optional-decimal-child candidate](docs/OPTIONAL-DECIMAL-IMPLEMENTATION.md) now implements zero-or-one local decimals with ordered matching and original present-node state; required-only v1 and optional v2 profiles are distinct.

The [scope checkpoint](docs/OPTIONAL-DECIMAL-CHECKPOINT.md) links the adoption, plan and current review-delivery identity; optional support remains future work.

## Optional decimal implementation candidate — 29 October 2026

Current behavior: [Schema 0.6 optional assessment](docs/OPTIONAL-DECIMAL-IMPLEMENTATION.md), with core 1.34.1 and required local source override. Local zero-or-one particles, ordered matching, assertion EBV and present-only original state are implemented; required-only v1 remains and optional schemas use v2. Empty element-only root whitespace normalization is corrected while real/decimal mutation refusal remains. The complete Schema suite passes 260 cases, including 56 new optional cases; the full-document example retains later declaration identity. Earlier planned-scope/version notices are historical. Broader REQ-009/general session/API freeze and consumer acceptance remain pending. Original public checkout and product pins are unchanged; implementation is uncommitted and unpublished.

The [optional assessment review checkpoint](docs/OPTIONAL-DECIMAL-REVIEW.md) records the bounded implementation review and paired delivery identity.

The [bounded repeated-decimal candidate](docs/REPEATED-DECIMAL-IMPLEMENTATION.md) implements finite occurrence, assertion-cardinality and copy/projection boundaries without widening assertion grammar.

The [bounded repetition review checkpoint](docs/REPEATED-DECIMAL-REVIEW.md) records current implementation review and paired delivery identity.

The [count assertion candidate](docs/COUNT-ASSERTION-IMPLEMENTATION.md) implements a finite cardinality-function grammar extension while preserving native validation and existing child-name semantics.

[Bounded Schema assessment review and next scope](docs/COUNT-ASSERTION-REVIEW.md).

The [bounded presence assertion candidate](docs/PRESENCE-ASSERTION-IMPLEMENTATION.md) adds exists/empty predicates while preserving count and typed-copy behavior.

[Presence assertion implementation review](docs/PRESENCE-ASSERTION-REVIEW.md).

[Consumer compatibility and native Schema priorities](docs/CONSUMER-COVERAGE-PRIORITIES.md).

[Isolated persistent-provider compatibility qualification](docs/XMLDB-COMPATIBILITY-QUALIFICATION.md).

[Isolated authoring/preview source compatibility qualification](docs/BRAID-COMPATIBILITY-QUALIFICATION.md).

[Consumer qualification review](docs/CONSUMER-QUALIFICATION-REVIEW.md).

[Unchanged-binary compatibility qualification plan](docs/UNCHANGED-BINARY-COMPATIBILITY-PLAN.md).

[Unchanged-binary qualification and runtime identity limits](docs/UNCHANGED-BINARY-COMPATIBILITY-RESULTS.md).

[Unchanged-binary qualification review](docs/UNCHANGED-BINARY-COMPATIBILITY-REVIEW.md).
