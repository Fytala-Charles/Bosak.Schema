# Explicit-schema location hints

20 November 2026. The opt-in `NativeSchemaSession` now accepts the exact xsi
`schemaLocation` and `noNamespaceSchemaLocation` attributes on admitted roots,
containers, simple-content terminals and scalar leaves. It always uses the
supplied schema and never fetches, resolves or merges the hinted resources.
The [pre-implementation adoption](ADOPTION.md) pins the normative sources and
documents the bounded policy and intentional PSVI limitations.

Hints remain in original parsed XML and detached export. They do not add typed
application attribute slots or change root, descendant or scalar indexes.
They do not relax required attributes, scalar/facet checks or XML input limits.
`xsi:type`, `xsi:nil` and other special requests remain Unsupported/Attributes;
wrong-namespace lookalikes keep existing application-attribute behavior.
The legacy `NativeDecimalSession` retains both hint refusals. No public API,
Core dependency pin, published package, version/tag or controller changes.
The published beta.1 scope is unchanged.

## Measured result

The exact frozen upstream pilot now yields **1 validity match, 0 mismatches,
20 Unsupported, 0 harness errors**. Only
`MS-ComplexType2006-07-15/ctB073/ctB073.v` changes, from instance Unsupported to
Valid matching its upstream expected validity. All four invalid xsi:type cases
remain Unsupported. Sixteen cases still stop at schema compilation.
This is **one case of positive validity evidence**, not full clause, datatype,
PSVI or whole-suite conformance. No upstream file, expectation or original
manifest was changed. [Transitions](PILOT-TRANSITIONS.json) preserve the exact
before/after rows; [baseline](../conformance-pilot/README.md) remains historical.

**1,302 source tests pass**: 415 implementation and 887 public consumer.
All **1,255 previous named outcomes are unchanged**; 47 authored public tests
cover both hints, alternate prefixes, all owner shapes, empty/relative/hostile
values, original exports and typed indexes, invalid content, missing attributes,
special request refusals, lookalikes, legacy behavior, cancellation and recovery.
[Regression comparison](REGRESSION-COMPARISON.json) and original TRX receipts
are separate from upstream validity evidence. The driver Release build has zero
warnings/errors. Thirteen Python integrity controls pass, including source drift,
extra untracked production files, wrong base and runtime drift rejection.
This does not requalify package-only or retained binary consumer lanes.

## Candidate reproduction

The baseline harness still defaults to refusing changed production source.
`--candidate-evidence` adds an explicit, exact candidate path without altering
the frozen case manifest. [CANDIDATE.json](CANDIDATE.json) binds the two modified
production files and complete driver runtime directory by SHA256, including
Schema and Core DLLs. Extra/untracked production files, source drift, runtime
drift or a different base refuse before launch. This binds measured bytes, not
an assertion of external signature, API or publication acceptance.

```powershell
dotnet test Bosak.Schema.slnx -c Release
python -m unittest discover -s tools -p test_xsd_conformance_pilot.py
dotnet build tools/Bosak.Schema.ConformancePilot/Bosak.Schema.ConformancePilot.csproj -c Release
python tools/run-xsd-conformance-pilot.py --output .guard-tmp/hints-new-run --runner tools/Bosak.Schema.ConformancePilot/bin/Release/net10.0/Bosak.Schema.ConformancePilot.dll --candidate-evidence docs/schema-location-hints/CANDIDATE.json
```

Output must be new. Rebuilt or checked-out bytes can differ from the sealed local
runtime/source hashes; produce and review a new explicit candidate receipt for
such a run, preserving this original receipt. Exit 0 remains measurement completion,
not whole-suite success. Original third-party notices and corpus terms remain
in the [pilot notices](../conformance-pilot/THIRD-PARTY-NOTICES.md).

## Next

Review and deliver this bounded hint policy together with the pilot tooling.
Then adopt anonymous local decimal/string simple restrictions, preserving
separate integer-derivation and assertion boundaries. Do not select new upstream
cases based on desired outcomes or rewrite the frozen corpus to gain matches.
