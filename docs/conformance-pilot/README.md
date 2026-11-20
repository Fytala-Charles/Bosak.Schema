# Pinned upstream XSD validity pilot

20 November 2026. Product source: `972948c68da872dfd1ca31250608e224c4d20252` (PR28).
Upstream master: `7bc3365c652a322f3d762021b3879eb92dae7e30`.

The frozen **21 instance cases in 15 groups** produced **0 validity matches,
0 mismatches, 21 Unsupported and 0 harness errors**. No instance reached a
Valid/Invalid comparison. This establishes a gap baseline and provides **no
positive conformance evidence or specification coverage percentage**.

| First observed stage/reason | Cases | Static fixture review |
|---|---:|---|
| Compile / DeclarationShape | 8 | Anonymous local simple restrictions; decimal/string routes differ from integer restrictions |
| Compile / DeclarationShape | 1 | Restriction of complex simple content |
| Compile / Namespace | 2 | Relative target namespace |
| Compile / Namespace | 3 | Explicit attributeFormDefault without attribute uses; further assertion/inheritance exclusions |
| Compile / AssertionGrammar | 2 | String or nested namespace assertions outside the decimal subset |
| Instance / Attributes | 1 | Schema-location hint with an explicitly supplied schema |
| Instance / Attributes | 4 | Invalid xsi:type overrides; separate from hints |

Five case schemas compile; their five instances remain Unsupported. Sixteen cases
stop during schema compilation. [GAPS.json](GAPS.json) distinguishes measured
diagnostics from static review and additional blockers. No counterfactual fixture
edits or engine changes were executed. No defect can be concluded from these
Unsupported outcomes alone.

The cases were selected by static metadata/shape/feature review before native
execution, not by outcomes. [CASES.json](CASES.json) freezes their identities,
accepted upstream expectations, version applicability, original documentation
references, authored XSD 1.1 clause mappings and 42 exact Git-blob file hashes.
[The pre-execution contract](MEASUREMENT-CONTRACT.md) and
[original manifest snapshot](evidence/CASES-BEFORE-EXECUTION.json) remain unchanged.
Original older specification references are not replaced by our 2012 review mappings.
This is selected top-level validity only, not PSVI, all XSD 1.1 clauses or the full
upstream suite. Existing authored tests and package/lifetime evidence stay separate.

## Reproduce

From the repository root, with .NET 10 and Python 3:

```powershell
python -m unittest discover -s tools -p test_xsd_conformance_pilot.py
dotnet build tools/Bosak.Schema.ConformancePilot/Bosak.Schema.ConformancePilot.csproj -c Release
python tools/run-xsd-conformance-pilot.py --output .guard-tmp/pilot-new-run --runner tools/Bosak.Schema.ConformancePilot/bin/Release/net10.0/Bosak.Schema.ConformancePilot.dll
```

The output directory must be new. The harness verifies hashes, metadata, accepted
statuses, version OR/expected AND semantics and the unchanged product source pin
before public native API execution. There is no fallback validator, fixture rewrite
or network resolver. Explicit capacities and assembly identities are recorded in
[results](evidence/RESULTS.json). Exit 0 means a completed measurement without an
observed mismatch/error, **not conformance success**; 2 means mismatches or per-case
harness errors; 3 means preflight/process failure. The driver catches exceptions
and the parent applies a watchdog. This manual pilot is not an admission gate.

Validation: **1,255 source tests** (415 implementation, 840 public consumer),
**10 Python integrity controls**, and the Release driver build pass. Source tests
are not 1,255 upstream conformance passes. Binary/source identities and original
TRX results accompany the evidence; no API, Core pin, package, tag or release changes.

## Recommended next slice

Define and adopt safe treatment of `xsi:schemaLocation` and
`xsi:noNamespaceSchemaLocation` when the caller explicitly supplies the schema,
without fetching locations. Preserve separate refusal/validation policy for
`xsi:type` and `xsi:nil`. Then consider anonymous decimal/string simple restrictions;
integer derivation, relative namespaces and broader assertions need separate decisions.
Each semantic slice requires immutable specification adoption, happy/edge controls
and rerunning this unchanged corpus. A first-blocker change does not guarantee admission.

See [third-party notices](THIRD-PARTY-NOTICES.md). Original fixtures and metadata
are redistributed unchanged under their upstream terms, separately from authored code.
