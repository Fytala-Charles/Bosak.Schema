<div align="center">
  <img src="../assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Integration">
  <h1>Bosak.Schema Integration</h1>
  <p>Use schema services without activation and with explicit supported coverage.</p>
</div>

## Bounded Unsupported-reason candidate - 19 November 2026

The local development candidate adds `NativeDecimalDiagnostic.UnsupportedReason` and the fixed `NativeDecimalUnsupportedReason` enum. Existing stage codes/messages, outcomes and grammar are preserved; categories describe one detected refusal and may be Unspecified. All365source cases pass (312 implementation/53public, including20new controls), as do53package-only cases and retained124/23 unchanged-binary replacement catalogs. These lanes overlap and are not additive. The54release and six workload-integrity controls pass. MinVer minimum advances to0.6 for this additive feature;0.6.0-alpha.0.2 is local/unsigned only. Published0.5.0-beta.1 and allCore1.81.2 pins remain unchanged. No commit/push, tag or publication. [Candidate](unsupported-reasons/README.md).


## Beta.2 gap assessment - 19 November 2026

A public-example beta.2 gap assessment finds no confirmed new beta.1 defect. Namespace support alone would not admit the existing sku/orders schemas. The proposed first bounded slice is clearer Unsupported reason categories, preserving stage codes, input-free diagnostics and current grammar; API/design acceptance and a concrete application priority remain pending. Beta.1 stays current; no implementation or release change. [Assessment](beta2-gap-assessment/README.md).


# Bosak.Schema Integration

> Use schema services without activation and with explicit supported coverage.


## 1. Dependency and Availability

**Current release — 18 November 2026:** [Bosak.Schema 0.5.0-beta.1](https://www.nuget.org/packages/Bosak.Schema/0.5.0-beta.1) is published and independently verified. [GitHub prerelease](https://github.com/Fytala-Charles/Bosak.Schema/releases/tag/v0.5.0-beta.1), [scope and publication receipt](BETA1_RELEASE.md).

Build this source with .NET 10. The project references Bosak XPath API/providers, XSLT and XQuery 1.81.2 after the merged dependency adoption. Install the published package with `dotnet add package Bosak.Schema --version 0.5.0-beta.1`. Source builds use MinVer; the product release tag and later documentation/controller commits have distinct identities.

## 2. Compile and Validate

```csharp
using System.Xml.Linq;
using Bosak.Schema;

var options = SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11);
var compiler = SchemaServices.CreateSchemaCompiler();
using var stream = File.OpenRead("schema.xsd");
var schema = compiler.Compile(stream, new Uri(Path.GetFullPath("schema.xsd")), options);
var result = SchemaServices.CreateValidationService()
    .Validate(schema, XElement.Load("candidate.xml"));
```

Always inspect compile/validation diagnostics and the [coverage status](FEATURE_REQUESTS.md). A successful supported assertion evaluation is not a claim of complete XSD 1.1 structural validation. The stream compiler prohibits DTD resolution; imports/includes require an explicit `ISchemaResourceResolver`. The caller owns its input stream/tree.

## 3. Schema-Aware XSLT

`SchemaAwareXslt.CreateCompiler()` enables the existing schema-aware compiler path. Its directory overload preserves the existing resolver behavior; pass an appropriate directory and resource policy for your host. This path still uses the core's established `XmlSchemaSet` integration and does not turn it into a complete XSD 1.1 processor.

The [current Core integration guide](https://github.com/Fytala-Charles/Bosak/tree/main/integrations/Schema) builds an optional nonpackable source wrapper at an exact Schema revision. It does not install a new Schema NuGet package or make Core depend on this repository. The 135-case suite includes the standalone 124 cases, eight price/assertion cases and three caller stream/reader ownership cases; they are not 259 independent tests. Full structural XSD 1.1 validation remains incomplete.

## 4. Owned Native Decimal Workflow

The published **0.5.0-beta.1** provides owned `NativeDecimalSession` and `NativeDecimalDocument` APIs with explicit capacities/context, exact decimal reads and detached XML export. ADR009/010 are accepted for this bounded Schema application scope. Qualification passed 345 source cases (312 implementation/33 public),33 package-only cases and retained 124/23 binary lanes using Core 1.81.2. These catalogs overlap. [Release, exclusions and receipt](BETA1_RELEASE.md). Full XSD 1.1, general Core C45/ADR006 and production SLA acceptance remain separate.

```csharp
using Bosak.Schema;

var options = new NativeDecimalSessionOptions(256, 256, 1, 4, 1, 256, 256);
var episode = new NativeDecimalAssessmentContext(
    new DateTimeOffset(2026, 11, 17, 12, 0, 0, TimeSpan.Zero), 60);
var compiled = NativeDecimalSession.Create(File.ReadAllText("native-decimal.xsd"), options);
if (compiled.Outcome != NativeDecimalOutcome.Valid)
    throw new InvalidOperationException(compiled.Diagnostics[0].Code);
using var session = compiled.Value!;
var result = session.Assess(File.ReadAllText("native-decimal.xml"), episode);
if (result.Outcome == NativeDecimalOutcome.Valid)
{
    using var document = result.Value!;
    for (var index = 0; index < document.DecimalCount; index++)
        Console.WriteLine(document.ReadDecimal(index).ToInvariantString(256));
}
```

Use only the finite [grammar](NATIVE-DECIMAL-GRAMMAR.md). Instances and schemas are strings; the API takes no ownership of caller streams/readers. Operations on a session must be sequential. Dispose waits for admitted synchronous work and revokes owned access; detached values and XML remain usable. Inspect Invalid/Unsupported before using Value. Cancellation, invalid arguments, capacity exhaustion and lifecycle faults throw; they are not schema-invalid results. Read the [complete contract](NATIVE-DECIMAL-PUBLIC-API.md) before depending on diagnostics, disposal or numerical limits.

[Representative package-only invoice trial](BETA1_INVOICE_TRIAL.md) exercises the public fixture with exact scalar/export and refusal/recovery checks. It does not establish production acceptance or full schema/XPath coverage.

## 5. Activation Migration

Historical 15 November internal checkpoint (superseded for public entry by the [current contract](NATIVE-DECIMAL-PUBLIC-API.md)): use the [decimal session profile](NATIVE-SESSION-PROFILE.md). It defines a future explicit assessor-bound scalar route and twelve paired planned acceptance groups. The public bounded decimal factory is available in0.5.0-beta.1; broader component/provider adoption remains outside that contract.

Remove `License.Activate(...)` from new applications. Existing binaries can retain that call: the public shim ignores the supplied value, stores no credential and never contacts a service. `IsActivated` is always true; `Current` is a perpetual Apache compatibility marker. It grants no entitlement to any other Fytala product. `GracePeriod`, `ActivatedLicense`, `LicenseTier` and `LicenseValidationException` remain legacy compatibility shapes, without schema access enforcement.

The public source contains no internal HMAC/product key, signing implementation, key issuer, billing pipeline or private deployment configuration. Those remain private infrastructure. Do not use the shim to authorize another product.

## 6. Version History and Limitations

| Date | Change |
|---|---|
| 2026-11-18 | Beta.1 published on NuGet.org and GitHub Releases; bounded ADR009/010 accepted; signed payload and registry-only consumer verified |
| 2026-11-17 | Local0.5 prerelease package review:272 source cases,23 repeated package cases and124 retained legacy binary cases pass; seven additive public types; package-only README correction; no publication |
| 2026-11-17 | Public owned decimal entry candidate, explicit options/context, fixed diagnostics, deterministic lifecycle controls and separate consumer; 272 solution cases; Core remains 1.81.2, development minimum 0.5, no publication |
| 2026-11-15 | Adapter committed as 939ba9a; internal owned-facade prototype and public proposal delivered; default component lookup names now throw ArgumentException; 243 tests, audited Release build and official docs check pass |
| 2026-11-15 | Backend committed as aba6967; internal native authority/advisor/input/lifetime adapter and sequential runner delivered with 20 cases; 227 standalone tests pass; public API/package pins unchanged |
| 2026-11-15 | Internal native decimal backend and finite grammar delivered; 76 backend cases and 207 total standalone tests pass; public APIs/package pins unchanged; authority/advisor/session qualification pending |
| 2026-11-15 | Published Core `$value` transport probe passes seven cases; 131 standalone tests pass. Test-only issuance, not native assessment; [receipt](native-session-probe/2026-11-15/README.md) and [specification basis](NATIVE-DECIMAL-SPECIFICATION-ADOPTION.md) recorded |
| 2026-11-15 | First native decimal session profile defined with proposed ADR-009; implementation and qualification pending; no API, dependency or runtime change |
| 2026-11-09 | Core 1.81.2 dependencies merged in Schema PR #1; 124 existing regression cases and separate source/binary compatibility evidence retained |
| 2026-11-09 | Core optional source wrapper refreshed to merged Schema c953ebe in Core PR #472; 135 cases and typed-price example pass, with exact LF/CRLF input hashes |
| 2026-10-30 | REQ-009 decision evidence (ADR-005/006, ADR-001/002, XSD 11_SOURCE_AUDIT, XSD 11_GAP_MATRIX, XSD 11_S4_DESIGN_PREP, SEAM_DESIGN, XSD 1.1 design handover) transferred from the private repository with dated historical-context notes; all source references to docs/ADR-005 and docs/XSD 11_S4_DESIGN_PREP.md now resolve |
| 2026-10-30 | REQ-009 S5c: open-content wildcards record the XSD 1.1 negative constraints notNamespace/notQName with explicit representation diagnostics (BSK-XSD-016); content-model wildcards remain explicit BSK-XSD-003; 124 tests pass |
| 2026-10-29 | Owner-approved Apache-2.0/public schema-only source, activation-free factories and credential-free compatibility shim; 114 tests pass |
| Before transition | Private proprietary development, preserved separately with its original Git history |

Public factory/type signatures are retained, with the intentional licensing behavior change documented above. The separate dependency-adoption dossier records the exercised unchanged-Schema/test-binary replacement lane. It does not certify arbitrary previously compiled consumers or full schema-session ABI/provider acceptance.

*Last updated: 2026-11-18*
