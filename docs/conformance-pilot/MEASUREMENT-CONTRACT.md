# Frozen upstream validity measurement contract

20 November2026. Recorded before first native pilot execution. Product base
972948c68da872dfd1ca31250608e224c4d20252; upstreammaster7bc3365c652a322f3d762021b3879eb92dae7e30.
The frozen21instance cases/15groups in CASES.json were selected by metadata,
static shape and clause/feature review, not native outcomes. Every original
schema/instance/metadata byte comes from the pinned Git blob; no annotation,
schema-location hint, namespace, restriction or expected validity is rewritten.
42original files and their hashes form the complete selected corpus.

Use XSD1.1/XML1.0fifth configuration tokens. Follow original metadata OR semantics
for test applicability and AND semantics for expected conditions; require one
unambiguous valid schema and accepted schema/instance statuses. Retain original
2004normative documentation links separately from our2012XSD1.1clause mappings.
The latter are review targets, not a new engine semantic adoption or proof of
complete clause conformance. Schema-only invalid tests and composition/multiple
schema documents are outside this pilot. Suite-level schemaVersion prose is not
silently substituted for machine-readable applicability tokens.

The parent verifies all hashes, IDs, local document paths, statuses, applicability
and expected results before native launch. It refuses altered production source
relative to the product pin. Use public NativeSchemaSession with explicit options
recorded in native.json and no legacy/System.Xml.Schema fallback, XML/schema
rewriting or network resolution. Fixed assessment time is UnixEpoch/timezone0.
UTF8selected fixture decoding is strict; original XML bytes remain preserved.

CompileUnsupported or assessmentUnsupported =>Unsupported, NEVER a validity match.
CompileInvalid for an expected-valid schema =>mismatch. Only admitted Valid/Invalid
instance assessments can match the exact upstream expected validity. Exceptions,
timeouts, missing/duplicate results or preflight failures are harness failures,
not Unsupported or matches. Report schemas admitted and instances unsupported
separately; a completed run with allUnsupported proves no instance conformance.
Exit0means no observed mismatch/harness failure, not whole-suite or positive
coverage; exit2means mismatch/harness rows, exit3preflight/process failure.

The upstream suite checks top-level validity only, not PSVI/provenance/copy/export
or lifetime correctness. Existing authored source/package/retained evidence stays
separate. This is a small reviewed pilot, not all15,486scanned groups or an XSD
coverage percentage. No new gate/admission or engine change is implied.
