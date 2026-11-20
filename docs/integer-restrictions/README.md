# Bounded direct integer restrictions

20 November 2026. Locally qualified candidate on merged PR30 base
`5bfbb1e798cdf18bac6ac7bb15705b1a0e50bc65`.
The [before-code adoption](ADOPTION.md), original anticipated rows and
[route clarification](ADOPTION-CLARIFICATION.md) remain separate records.

`NativeSchemaSession` now admits direct named and anonymous local-element
restrictions of xs:integer. Exact BigInteger range/digit checks enforce
minInclusive, minExclusive, maxInclusive, maxExclusive, totalDigits and inherited
fixed fractionDigits zero. No decimal conversion or rounding occurs. Named
integer restrictions enforce the same facets on admitted root/descendant
application attributes. Original type/owner identity, scalar ordering and
anonymous QName absence remain preserved.

Signed zero and leading-zero lexical forms retain integer value semantics;
supplied-digit and 256-character representation limits apply before facets.
Nonzero fractionDigits, fractional/exponent range values, duplicate/conflicting
components, unsupported facets and eager unused-definition failures refuse as
documented. Supported instance facet failures are Invalid with no partial facts.
Empty integer value spaces may compile and reject every present value.
Global/local definition, depth/node/scalar/attribute and read/output budgets are
unchanged. Legacy decimal refusal, assertions, simpleContent, hint/type/nil and
scalar-only query boundaries remain protected; chains, derived integer families,
enumeration/pattern and anonymous attribute/root scalar types remain excluded.

## Qualification

| Lane | Result | Evidence scope |
|---|---:|---|
| Before-code native controls | 6 pass | Three original opt-in refusals plus built-in integer and decimal/string support |
| Source | 1,482 pass: 427 implementation / 1,055 public | 1,371 unaffected prior cases, two adopted admissions, 109 new cases |
| Minimum-Core package consumer | 1,055 pass | Public package-only tests; nine signed1.81.2Core dependencies |
| Current-Core package consumer | 1,055 pass | 1,054shared public cases plus explicit1.81.17version control; minimum-version-only assertion excluded |
| Retained binary callers | 124 / 23 / 33 pass | Only Schema.dll changed, other files byte-identical; overlapping catalogs |
| API comparison | 52 public types / 392 rows | Exact zero surface delta |
| Frozen upstream pilot | 9 matches / 12 Unsupported / 0 mismatches / 0 errors | Two integer cases newly match unchanged original Valid/Invalid expectations |
| Pilot integrity controls | 15 pass | Harness controls, not native conformance cases |

The 99 new authored catalog cases plus four public legacy/capacity/lifetime
controls and six actual Core-fact controls exercise exact large values, each
facet boundary, fixed zero, empty intersections, lexical/component/refusal
categories, attributes, eager/global limits, namespace binding, original named
and anonymous provenance, authority/expiry and cancellation/recovery.
All18Core package signatures, source/archive/cache/runtime identities and the
signed published-beta baseline verify in
[IDENTITY-FINAL.json](evidence/IDENTITY-FINAL.json). These lanes overlap and are
not additive specification coverage totals.

[REGRESSION-DELTA.json](REGRESSION-DELTA.json) pins the original and current
AS-034/M2C3-67 rows and their unchanged XML/schema. Unreachable expectations and
stale labels/reasons were corrected explicitly. Legacy M2B-54 remains byte-for-byte
unchanged; the same payload is separately Invalid through the opt-in route because
`12.5000` is not integer lexical syntax. Initial regression evidence and the
route clarification are preserved rather than relabelled as an extra legacy admission.

[PILOT-TRANSITIONS.json](PILOT-TRANSITIONS.json) records exactly two new validity
matches; all19other rows remain identical to the previous7/14checkpoint.
All21case identities/expectations and42upstream files retain original bytes and
licensing notices. Nine selected matches are not complete clause, datatype,
whole-suite, PSVI or generalC45acceptance and justify no XSD coverage percentage.

## Reproduce

```powershell
dotnet test Bosak.Schema.slnx -c Release
dotnet build tools/Bosak.Schema.ConformancePilot/Bosak.Schema.ConformancePilot.csproj -c Release
python tools/run-xsd-conformance-pilot.py --output .guard-tmp/integer-new-run --runner tools/Bosak.Schema.ConformancePilot/bin/Release/net10.0/Bosak.Schema.ConformancePilot.dll --candidate-evidence docs/integer-restrictions/CANDIDATE.json
```

[CANDIDATE.json](CANDIDATE.json) binds exact source/runtime hashes relative to
the pilot's original product base without altering its frozen case manifest.
Rebuilt/check-out bytes may need a new reviewed candidate receipt; preserve this
original one. The local `0.6.0-alpha.0.18` package is unsigned, for compatibility
qualification only. Original projects, TRX and signature logs are under evidence;
rebind recorded local source/test-data paths only in a scratch reproduction copy.
The current-Core lane excludes only
`PublicSurface_NoFriendGrantOrPublicFactoriesForAuthority` (minimum-version
assertion), replacing it with `CurrentCore_IsRecordedVersion`. Full source and
minimum package lanes still check the original authority/public-factory surface.

The qualified integration route remains owned scalar reads. General native node
queries/C45 and legacy XPath null-QName behavior retain the separate
[route limits](../anonymous-simple-restrictions/implementation/ROUTE-LIMITS.json).
No public API, Core pin, published beta.1, tag/controller or registry changes.
Changes remain uncommitted. Next: review and deliver this bounded slice, then
define the separately ranked native typed-node query contract before changing guards.
