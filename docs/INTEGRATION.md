<div align="center">
  <img src="../assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Integration">
  <h1>Bosak.Schema Integration</h1>
  <p>Use schema services without activation and with explicit supported coverage.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](PUBLICATION.json). No NuGet release or commercial infrastructure publication.

# Bosak.Schema Integration

> Use schema services without activation and with explicit supported coverage.


## 1. Dependency and Availability

Build this source with .NET 10. The project references Bosak XPath API/providers, XSLT and XQuery 1.26.3. The source transition targets the 0.4 development line through MinVer; no new NuGet release or tag is claimed.

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

## 4. Activation Migration

Remove `License.Activate(...)` from new applications. Existing binaries can retain that call: the public shim ignores the supplied value, stores no credential and never contacts a service. `IsActivated` is always true; `Current` is a perpetual Apache compatibility marker. It grants no entitlement to any other Fytala product. `GracePeriod`, `ActivatedLicense`, `LicenseTier` and `LicenseValidationException` remain legacy compatibility shapes, without schema access enforcement.

The new package contains no internal HMAC/product key, signing implementation, key issuer, billing pipeline or private deployment configuration. Those remain private infrastructure. Do not use the shim to authorize another product.

## 5. Version History and Limitations

| Date | Change |
|---|---|
| 2026-10-29 | Unpublished 0.5 native assessment candidate: 191 source tests on published/current core and 114 unchanged legacy binary cases; full typed-root projection remains unsupported |
| 2026-10-29 | Owner-approved Apache-2.0/public schema-only source, activation-free factories and credential-free compatibility shim; 114 tests pass |
| Before transition | Private proprietary development, preserved separately with its original Git history |

Public factory/type signatures are retained, with the intentional licensing behavior change documented above. Real previously compiled consumer replacement tests and full schema-session ABI/provider acceptance are not certified by these source tests.

## 6. Native Document Assessment (0.5 Candidate)

Use `SchemaServices.CreateDocumentAssessor(schemaDocument)` when a structurally checked document is needed within the explicit profile. Inspect `IsSupported` and diagnostics, then `Assess(instanceDocument, cancellationToken)`; only `Valid` exposes an owned `AssessedDocument`. This is separate from the assertion-only descriptor service above.

`Elements` retains original root/local declarations, anonymous/built-in types, normalized values and typed/no-typed-value state. `Copy()` preserves the original component authority without revalidation. `CreateDocumentCopy(false)` strips PSVI; its true branch explicitly refuses full anonymous-root projection. `CreateTypedElementCopy(name)` exports only a detached assessed decimal child for real legacy XPath/XSLT processing. For XSLT copy checks, use a document context and an explicit empty SchemaSet as the annotation-copy context; no 1.1 schema is routed through that set.

See [complete profile, example and ownership rules](DOCUMENT-ASSESSMENT.md) and [ADR-008](ADR-008-bounded-native-assessment.md). Library references remain published 1.26.3; no package, tag, core API or consumer pin is changed by this candidate.

*Last updated: 2026-10-29*

## Whole-document bridge candidate — 29 October 2026

The unpublished core 1.34.0 provider bridge now carries original anonymous-root and decimal-child state through whole-document exports and preserve/strip copies. Root atomization raises FOTY0012; stale content and legacy reassessment refuse explicitly. Use `-p:BosakSourceRoot=D:/Development/Bosak` for builds/tests/examples because candidate package dependencies are unpublished. See [the current guide](DOCUMENT-ASSESSMENT.md). The earlier 191-case receipts describe the historical projection-only checkpoint; fresh bridge qualification is recorded separately in the core `docs/schema-document-bridge` dossier. Broader coverage, consumer adoption and API freeze remain pending. No commit/push/tag/publication.

## Reviewed whole-document bridge — 29 October 2026

The optional provider attachment API now checks a replacement completely before removing the previous binding. Failed replacement retains original state and stale-content refusal; failed initial attachment leaves the node unregistered. The trusted host must supply immutable state and the correct assessed value. This API carries that state without performing schema validation. The finite native profile is unchanged; optional children and general API freeze remain pending. Current core candidate is unpublished 1.34.0; use the BosakSourceRoot override. New review qualification lives in the core `docs/schema-document-bridge/api-review` dossier, separately from historical assessment receipts.

Post-review verification: **24,209 core tests pass in one complete solution run**, **204 Schema tests pass**, and all six focused provider attachment cases pass. Release build: zero warnings/errors. QT3 **31,397/0/424**, qt4 **8,941/0/1,918** (230 gated sets, 11 experimental passes), with exact skip mappings unchanged. The full-document example retains original root state and passes typed-value and preserve/strip checks. Prior sealed assessment, bridge and integration receipts remain unchanged.

## Optional decimal child scope — 29 October 2026

The [new adoption record](OPTIONAL-DECIMAL-ADOPTION-2026-10-29.md) and [implementation plan](OPTIONAL-DECIMAL-PLAN.md) define the next finite extension. It is not implemented. Keep occurrence bounds on particles, actual element states only for present nodes, and use effective Boolean conversion for empty assertion results. Maximum occurrence stays one; no broader grammar/session admission. Proposed implementation version is Schema 0.6; this documentation step keeps 0.5 and core 1.34.0 unchanged. Fresh baseline: 204 Schema tests pass. Core solution/gates inherit the source-identical `a163ff11` checkpoint. No REQ completion or publication.

Scope refinement: a thirteenth isolated observation exposes ignorable-whitespace removal being refused for an empty element-only assessed root. Supporting all-optional-absent documents therefore needs a bounded provider binding correction. The future implementation proposes core patch 1.34.1 plus fresh complete core tests and exact conformance gates; current versions and production source stay unchanged. Both the initial twelve and extended thirteen-observation receipts are preserved in the optional-child dossier; they overlap.

## Optional decimal scope delivery — 29 October 2026

See the [synchronized checkpoint](OPTIONAL-DECIMAL-CHECKPOINT.md) for current delivery identity and remote target. Documentation is prepared for the user-authorized commit/push; optional-child semantics are not implemented. Current versions/pins and production source remain unchanged. Thirteen observations and 204 passing Schema baseline cases support the scope; the core suite/gates are inherited. Next: implement the bounded plan and its core binding correction with fresh required qualification.
