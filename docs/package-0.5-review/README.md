# Schema0.5 package-delivery review

17 November 2026. User requested API/documentation/acceptance review and a local candidate with separate publication decision. Base: merged Schema `9fd4e6acd20b34446d736ab0ec3fa2d439f9e337`; prior adopted baseline `c953ebe0057bea1616d3e4f891f7b0a5fc4bd99b`. No engine semantics, specification adoption or Core dependency change is proposed.

## Recommendation

Proceed toward a bounded **prerelease** after committing/reviewing the package README correction and preparing from that immutable accepted source. Do not label the current local artifact as stable0.5.0 or publish it automatically. The exact clean-source candidate is `Bosak.Schema.0.5.0-alpha.0.6.nupkg`, derived normally by MinVer, without a Version override or tag. The two same-version local archives below are deliberately distinct and isolated; registry actions are not performed.

| Artifact | Bytes | SHA-256 | Disposition |
|---|---:|---|---|
| Original clean9fd4e6a package | 89,782 | f41d1909d25ae80969697a3ddba5d083ffd22cfaa973a8ffccf628c3957d0bdb | Preserved review input; repository README has a relative logo/documentation links |
| Corrected staging package | 89,515 | d10b32adc1a00da5a2ba9b35fc9941aab17bf798ba01ab58320a582f5050a1c8 | Package README/metadata correction; uncommitted packaging provenance, not registry-ready |

Both are unsigned local archives targeting net10.0. DLL, XML API documentation and LICENSE/NOTICE are byte-identical between them. Staging metadata records assembly source9fd4e6a; its distinct uncommitted packaging inputs are hashed in this dossier, so that metadata is not a claim that all staging package contents came from a committed revision. A later packaging commit/tag changes MinVer derivation; prepare/review a new exact artifact rather than relabeling these bytes.

## API and consumer review

The public API inventory grows from32 to39exportedtypes and278to313declaredcontractrows. No old row was removed or changed. The seven additions are NativeDecimalSession, NativeDecimalDocument, NativeDecimalSessionOptions, NativeDecimalAssessmentContext, NativeDecimalResult<T>, NativeDecimalOutcome and NativeDecimalDiagnostic. The [API delta](API-DIFF.json) covers declared public/protected signatures, base/interfaces, enum values, nullability, defaults and constraints; it is not an exhaustive proof of every behavioral/ABI combination.

The owned scalar/string/synchronous boundary is explicit. Limits/context are immutable and required. Valid owns the returned session/document; Invalid/Unsupported publishes no value and one fixed stage diagnostic. Exceptions retain argument/cancellation/quota/lifecycle distinctions. Overlap refuses; Dispose serializes behind admitted synchronous work. Cleanup attempts all owned resources after revocation and reports a sanitized failure without retry. Detached values/XML confer no transferable authority. Full async/concurrent-disposal schedules, positive compatible universes, relation closure, lists/unions, namespaces/attributes/user types and broader XSD are outside this slice. Read [the contract](../NATIVE-DECIMAL-PUBLIC-API.md), [grammar](../NATIVE-DECIMAL-GRAMMAR.md) and [acceptance gaps](DECISION.json).

| Executed lane | Result | Interpretation |
|---|---|---|
| Audited Release build | Zero warnings/errors | Source/package dependencies remain Core1.81.2 |
| Full solution | 272 pass, zero fail/skip | 249implementation+23publicconsumer identities |
| Fresh package-only public consumer | 23 pass for original and staging | Same catalog repeated; no project/source override; archive/cache/runtimeDLL identity checked |
| Retained legacy binaries | 124 pass baseline and replacement | Old tests/host/config/Core unchanged; onlySchemaDLL replaced by candidate payload |
| API comparison | Seven types/35rows added; zero removed/changed | Against exact adoptedc953ebe baseline |
| Package legal/provenance | Pass | Apache-2.0 expression, exact LICENSE/NOTICE, sourcecommit, DLL/XML and onlySchemaassembly bundled |
| Published Core dependency provenance | Nine packages verified | Core1.81.2 signatures pass; archive/cache/runtimeDLL bytes match and source commit agrees |
| README correction | Prepared and staging-tested | All10links absolute HTTPS; logo on Microsoft-allowed raw.githubusercontent.com |
| Registry read-only observation | Three404responses | IDindex/prerelease/stable endpoints, not proof of name rights or future availability |

Do not add overlapping source/package/baseline/replacement test catalogs. The previously merged Core1.81.17 source-consumer lane is separate from this default Core1.81.2 package lane; it does not qualify every dependency combination or upgrade Schema's package references.

## Documentation finding and fix

The original package embedded the repository README with a local logo path and relative documentation destinations. [Microsoft NuGet guidance](https://learn.microsoft.com/en-us/nuget/nuget-org/package-readme-on-nuget-org) requires supported image hosts and does not render local image paths. Directory.Build.props now packages [NUGET_README.md](../NUGET_README.md), with supported HTTPS links, clear finite-profile limits, a native quick start and existing service entry points. Repository README retains local navigation. Managed branding assets are unchanged. Local Markdown rendering/link checks do not claim NuGet web-preview acceptance; no upload or authenticated portal action was performed.

## Remaining publication gates

1. Review/commit the packaging-only README change and current status docs; existing repository Git rule requires explicit user instruction. No C# or dependency changes are needed.
2. Complete hosted checks, then prepare from the accepted immutable commit using normal tag-based MinVer. Keep the assembly/package/Core identities exact and distinguish reviewed unsigned payload from later repository signing.
3. Explicitly choose the prerelease contract/profile and publication version. ADR009/010 are still Proposed; merges and tests do not manufacture permanent API/foundation, full XSD, compatibility or performance acceptance. Stable0.5.0 needs its own owner decision and stable version preparation.
4. Verify package-ID/publisher rights and selected authenticated publishing route without exposing keys. The only existing Schema workflow is build/test/docs; this review did not create or invoke a publisher.
5. Recheck registry and obtain explicit approval for the exact final registry action. Publish reviewed final bytes without an unreviewed rebuild, then verify service-side identities/signatures/payloads.

[Decision and exact local inputs](DECISION.json), [verification](VERIFICATION.json), [source-only manifest](MANIFEST.json), [offline verifier](verify.py). Raw nupkg/cache/binaries remain in the ignored Core workspace packet, outside this source-only dossier. Original receipts and the original public Schema checkout are preserved.
