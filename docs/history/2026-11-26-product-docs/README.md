<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema">
  <h1>Bosak.Schema</h1>
  <p>Apache-2.0 schema integration and staged XSD 1.1 services for .NET.</p>
</div>

## Native Gregorian fragment candidate — 26 November 2026

The 0.13 development candidate adds bounded gYear/gYearMonth/gMonth/gMonthDay/gDay with exact nullable components, REC timezone-aware comparisons, finite facets and original typed/member facts on signed Core1.86.0. GR-901–GR-1300 track400 behavioral contracts, including the shared equal-exclusive-endpoint schema correction. Existing lifetime/XML and explicit Unsupported boundaries remain. General legacy operators, ID graphs, full XSD1.1/C45 and publication remain separate. [Scope, bounds and example](docs/gregorian-400/GUIDE.md).

## Exact native duration candidate — 26 November 2026

The 0.12 development candidate adds bounded duration, yearMonthDuration and dayTimeDuration with exact independent months/seconds, finite facets and original selected-member facts on signed Core 1.86.0. Immutable DurationMonths, DurationSeconds and collapsed DurationValue reads retain owner/lifetime and XML boundaries. DU-701–DU-900 track 200 behavioral contracts. Partial-order and lexical ambiguity exclusions remain explicit; general legacy duration operators, full XSD1.1/C45 and publication remain separate. [Scope, capacities and example](docs/duration-200/GUIDE.md).

## Native URI and IEEE candidate — 26 November 2026

The 0.11 development candidate adds bounded anyURI, float and double validation and immutable typed reads on signed Core1.86.0. URI collapse/ordinal equality and IEEE signed-zero/NaN/rounding semantics retain original nodes, member provenance and owner lifetime. NU-501–NU-700 track 200 behavioral contracts, not 200 new specification features. Full XSD1.1, general PSVI/C45, augmentation and publication remain separate. [Scope, bounds and example](docs/numeric-uri-200/GUIDE.md).

The development source supports bounded exact `dateTime`, `time` and `dateTimeStamp` validation and scalar reads on Core 1.86.0. See the [calendar guide](docs/calendar-100/GUIDE.md) for capacities, supported facets and examples. Published Schema beta.1 retains its historical scope.

See the [current XSD 1.1 coverage audit](docs/SPECIFICATION_COVERAGE.md) for implemented profiles, remaining gaps and release availability.

See the [calendar transport prerequisite assessment](docs/calendar-transport-prerequisites/README.md) for calendar precision limits and the proposed extension.



# Bosak.Schema

The 0.6 development candidate requires **Bosak Core 1.81.17 or later**. [Dependency compatibility](docs/core-18117-baseline/README.md). Native scalar and original-node assessment preserve original XML, exact typed values and component identity within an explicit bounded profile.

Date restrictions support [enumeration](docs/date-enumeration-implementation/README.md), [bounds and timezones](docs/date-bounds/CONTRACT.md), [named derivation](docs/date-chains/CONTRACT.md), [lexical patterns](docs/date-patterns/README.md) and [bounded complex restriction](docs/date-complex-restriction/CONTRACT.md). Boolean restrictions support [patterns](docs/boolean-patterns/README.md) and [named chains](docs/boolean-chains/README.md). Decimal restrictions support [exact enumeration](docs/decimal-enumeration/README.md), [named derivation](docs/decimal-chains/README.md) and [bounded complex restriction](docs/decimal-complex-restriction/README.md) without replacing the instance representation. [Global attribute references](docs/global-attribute-references/README.md) and [attribute groups](docs/attribute-groups-review/README.md) preserve actual declarations and use constraints.

> Apache-2.0 schema integration and staged XSD 1.1 services for .NET.


## 1. Status and Scope

**Current release — 18 November 2026:** [Bosak.Schema 0.5.0-beta.1](https://www.nuget.org/packages/Bosak.Schema/0.5.0-beta.1) is published and independently verified. [GitHub prerelease](https://github.com/Fytala-Charles/Bosak.Schema/releases/tag/v0.5.0-beta.1), [scope and publication receipt](docs/BETA1_RELEASE.md).

Bosak.Schema is free, open-source Fytala software under [Apache-2.0](LICENSE.md). Schema compilation and validation require no activation, license key, trial, subscription or billing service. This public repository contains only the schema engine, its tests and documentation.

The implementation targets .NET 10 and references published Bosak 1.81.2 packages after the merged dependency adoption. It provides schema-aware XSLT compiler helpers and a staged XSD 1.1 component/assertion pipeline. Complete XSD 1.1 structural validation and integration of its full component universe into running Bosak expressions are **not yet implemented**. See the [support matrix](docs/FEATURE_REQUESTS.md).

The initial public source is derived from Fytala's private schema source at `fa0da3e2d9972706a5a86616ef285af4daf5037b`, with the owner-approved Apache transition on 29 October 2026. Billing, key-generation, deployment material and private Git history remain outside this repository. See [provenance](docs/APACHE-TRANSITION.md).

The [owned decimal session API](docs/NATIVE-DECIMAL-PUBLIC-API.md) supports native assessment of a finite decimal-only schema profile, exact scalar reads and detached XML export. Callers supply explicit limits and assessment context. It is published in this bounded beta; complete XSD 1.1 and broader provider acceptance remain pending. [Restricted resolution](docs/RESTRICTED_RESOLUTION.md), [descriptor ownership](docs/DESCRIPTOR_SNAPSHOTS.md) and [legacy validation](docs/LEGACY_VALIDATION.md) retain their explicit limits.

The 0.6 development API also offers opt-in bounded namespace assertions and empty
complex-content assessment, bounded string pattern facets, simple scalar document
roots, twelve derived integer built-in types, integer enumeration and checked dates with simple-content metadata,
with original typing
and explicit limits. See the
[consumer guide](docs/NUGET_README.md) for its supported grammar and exclusions;
the published beta retains its documented profile.

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

*Last updated: 2026-11-25*

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

The opt-in development profile supports [anonymous local decimal/string restrictions](docs/anonymous-simple-restrictions/implementation/README.md)
with the same bounded facets as named restrictions and original anonymous type identity.

The opt-in development profile includes [bounded integer restrictions](docs/integer-restrictions/README.md)
with exact range and digit facets for local elements and admitted attribute uses.

See the [original-node atomization proposal](docs/typed-node-contract/README.md)
for the next planned integration step and its current boundaries.

[Native atomization feasibility](docs/typed-node-contract/FEASIBILITY.md) records
the isolated candidate results and the remaining adoption/API decisions.


## Original-node scalar data (0.6 development candidate)

`NativeSchemaNodeDataSession` is a separate owned session for fixed original-node
atomization. Create it with existing explicit `NativeSchemaSessionOptions`, then
Assess with `NativeSchemaAssessmentContext`. A valid assessment returns an owned
`NativeSchemaNodeDataDocument`. `ReadElement(ImmutableArray<int>)` selects original
element-child indexes (empty selects root, maximum three segments).
`ReadAttribute(ownerElementPath, namespaceUri, localName)` selects a present original
attribute by expanded name. Both return one detached `NativeScalarValue`.
`ExportXml` returns bounded parsed lexical XML. Dispose documents and session.

The existing bounded schema grammar and native scalar families apply. Element-only
containers refuse reads with NotSupportedException carrying FOTY0012. Default paths,
invalid names and missing targets throw documented argument exceptions; query/cache
capacity and unassessed hint attributes are Unsupported read operations, not Invalid
instance assessments. Operations are sequential; overlap is refused and disposal
waits for admitted synchronous work. No expression, raw node, provider authority,
callback, general XPath, list/nil support or transferable PSVI is exposed.

This additive API is implemented locally for review. ADR-016 owner API acceptance,
general Core C45 and full XSD 1.1 conformance remain pending. Published 0.5.0-beta.1
and the existing decimal/scalar facades are unchanged. See [implementation adoption](docs/native-node-data/ADOPTION.md).


[Owned node-data contract review](docs/native-node-data-review/README.md) describes the current development API and its acceptance boundary.


The development owned node-data API has [bounded owner acceptance](docs/native-node-data-acceptance/README.md).


The 0.6 development mixed native profile also supports [bounded string equality assertions](docs/string-assertions/README.md) over immediate string children, with explicit cardinality, namespace and capacity limits.


[Bounded string assertion review](docs/string-assertions-review/README.md) records the development slice and its delivery scope.

See the [bounded global element reference guide](docs/global-element-references/README.md) for reusable declarations and supported particle shapes.

See the [named sequence group guide](docs/model-groups/README.md) for reusable content models and identity-preserving group references.

Use an [explicit closed schema catalog](docs/closed-includes/README.md) to resolve supported same-namespace includes through either owned facade.

The catalog also supports [bounded imports of named simple-type libraries](docs/simple-type-imports/README.md), with actual namespace and base-type identities.

The [simple nillability guide](docs/simple-nillability/README.md) explains nil flags, empty typed values and their distinction from empty strings and absent typed values.

See [element value constraints](docs/element-value-constraints/README.md) for supplied-content fixed validation and the explicit boundary around default application.

The [attribute value-constraint guide](docs/attribute-value-constraints/README.md) covers provided values, global/use inheritance and the defaulted-node boundary.

Use [bounded mixed sequences](docs/mixed-sequences/README.md) for interleaved original text with validated children and correctly typed aggregate values.

## Datatype composition candidate — 25 November 2026

The next native datatype candidate adds bounded binary and QName values; list/union integration is in progress. See the [datatype composition work](docs/datatypes-200/README.md) for its scope and qualification status.

## Native list and union candidate — 25 November 2026

The native datatype candidate now includes bounded lists/unions with explicit immutable item reads. See the [datatype consumer guide](docs/datatypes-200/GUIDE.md) for scope, capacities and qualification status.

## Native text datatype candidate — 26 November 2026

Native text datatype coverage is being extended with normalized strings, tokens and XML name types. See the [text datatype candidate](docs/text-200/README.md) for its scope and qualification status.
