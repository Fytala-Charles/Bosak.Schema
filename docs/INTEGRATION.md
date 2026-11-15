<div align="center">
  <img src="../assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Integration">
  <h1>Bosak.Schema Integration</h1>
  <p>Use schema services without activation and with explicit supported coverage.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](PUBLICATION.json). No NuGet release or commercial infrastructure publication.

# Bosak.Schema Integration

> Use schema services without activation and with explicit supported coverage.


## 1. Dependency and Availability

Build this source with .NET 10. The project references Bosak XPath API/providers, XSLT and XQuery 1.81.2 after the merged dependency adoption. The source transition targets the 0.4 development line through MinVer; no new NuGet release or tag is claimed.

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

## 4. Activation Migration

For planned native integration, use the [decimal session profile](NATIVE-SESSION-PROFILE.md). It defines a future explicit assessor-bound scalar route and twelve paired planned acceptance groups. No new factory or consumer API is available yet; continue using the existing services within their documented limits until implementation and qualification are recorded.

The [native decimal backend checkpoint](native-decimal-backend/2026-11-15/README.md) delivered the finite structural/assertion grammar internally, with exact decimals, present-only states and owned XML exports. Its 76 backend cases alone did not qualify a Core adapter. Existing public compilation/validation services retain their documented behavior.

The subsequent [internal Core adapter](NATIVE-SESSION-ADAPTER.md) now exercises actual native assessment and `$value` scalar transport, including original provenance, guarded nodes and host-owned resource leases. One shared NativeSessionRunner per session enforces the sequential host policy through raw task completion. This is an internal friend-assembly integration consumer; external consumer/public factory review remains pending. The existing public services above are unchanged.

The [owned-facade prototype/public proposal](NATIVE-DECIMAL-PUBLIC-API-PROPOSAL.md) now packages the internal assess/read/export/dispose workflow without exposing raw Core callbacks or mutable XML. Eight workflow cases and 28 adapter cases pass. These remain internal symbols, not a consumer quick-start. The default-name argument-handling fix is likewise internal; public factory signatures and package pins are unchanged.

Remove `License.Activate(...)` from new applications. Existing binaries can retain that call: the public shim ignores the supplied value, stores no credential and never contacts a service. `IsActivated` is always true; `Current` is a perpetual Apache compatibility marker. It grants no entitlement to any other Fytala product. `GracePeriod`, `ActivatedLicense`, `LicenseTier` and `LicenseValidationException` remain legacy compatibility shapes, without schema access enforcement.

The public source contains no internal HMAC/product key, signing implementation, key issuer, billing pipeline or private deployment configuration. Those remain private infrastructure. Do not use the shim to authorize another product.

## 5. Version History and Limitations

| Date | Change |
|---|---|
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

*Last updated: 2026-11-15*
