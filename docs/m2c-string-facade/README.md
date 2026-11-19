# M2C1 opt-in string and decimal facade candidate

19 November 2026. The [before-code adoption](ADOPTION.md) implements the ten-type
[concrete API proposal](../m2c-builtin-datatypes/api-review/API-PROPOSAL.md) on
Schemafbb4c13. [Original ADR011](ORIGINAL-ADR-011.md) and all older sealed planning,
adoption and qualification receipts remain historical; current ADR011 records a
local implementation, not global API acceptance. No Core engine/API/dependency/
source-wrapper pin, release-controller, tag or registry change.

## Implemented bounded profile

NativeSchemaSession is a separate opt-in synchronous string/decimal entry point.
NativeSchemaDocument exposes present-leaf ScalarCount, checked ReadScalar and
bounded ExportXml. Detached NativeScalarValue contains only string or exact
decimal, with wrong-kind getter refusal; values survive owner disposal without
PSVI/authority. Explicit options/context and producer-only outcomes/diagnostics
complete the ten public types. No Boolean/integer or public raw node/query API
is added. Existing NativeDecimalSession retains its decimal-only members and
string-schema refusal; there is no global mode switch.

Primitive strings preserve XML1.0 parsed text, including empty/whitespace,
combining/supplementary characters, CDATA and referenced CR/tab values. Raw XML
line endings follow parser normalization. No decimal collapse or Unicode
normalization. String capacity is an explicit UTF16code-unit limit, not an XSD
length facet. Schema/instance byte/node, ordered occurrence and prior decimal
facet/number capacities remain; every repeated leaf is assessed. All definitions
including unused types are checked. String operands in finite assertions refuse;
existing numeric sibling assertions remain enforced. Other datatypes, named string
restrictions/facets, attributes/imports, nil/default/fixed and nested structures
remain outside this slice.

The native backend, provider and synchronous owner/gate/runner/cleanup are shared
under explicit profile selection. Actual string node annotations, atomization,
declared/actual/origin provenance and complex-root FOTY0012 are qualified through
Core; foreign/disposed facts refuse. Scalar reads copy detached values only after
Core typed read/evaluation. The new export serializes through a cancellation-aware
bounded writer before appending past output capacity, entitizes CR text for
semantic reopen, and never returns a failed partial buffer. Failed export/read
leaves the gate/document usable. No canonical/byte-preserving serialization,
arbitrary concurrency proof, hard time or memory SLA is claimed.

## Verification and identities

| Lane | Passed | Scope |
|---|---|---|
| Full source |614 |334implementation+280public; zero failures/skips |
| Minimum-Core package-only |280 |Published Core1.81.2; no project references |
| Current-Core package-only |71 |47authored+23contract+one runtime identity on1.81.17 |
| Retained unchanged callers |124/23/33 |OnlySchema.dll replaced; catalogs overlap |

The47authored cases contain20positive,9supported-invalid,
7instance-refusal and11compile-refusal controls.
Twenty-three further public controls cover arguments/options/context, detached
and producer-only results, quotas/closure/cancellation, wrong-kind/index checks,
legacy refusal, exact export capacity and numeric read recovery. Six mixed-owner
barrier/fault controls exercise the same implementation gate/cleanup helpers;
four actual Core/writer controls cover original nodes, foreign/disposed facts,
partial-assessment refusal and escaped/canceled output. Cleanup fault doubles
exercise the shared helper, not manufactured failures in production Core disposers.
These are authored regressions, not official W3C cases or full-spec percentages.
[Results](RESULTS.json) and [case map](CASE-MAP.json) keep scopes separate.

Release build has zero warnings/errors. The local unsigned0.6.0-alpha.0.7 package
is qualification only; baseline repository metadata does not identify uncommitted
source. SOURCE-MANIFEST.json pins actual files.18official Core signatures/source
pins and archive/cache/runtime payloads match; local candidate payloads and the
original signed beta.1 input are independently verified. [Identity](IDENTITY.json),
[retained inputs](RETAINED.json). No new Core gate/release-control run is claimed
because those engine/controller sources did not change.

[API surface comparison](API-SURFACE.json) preserves325prior public rows across41
types and adds exactly10types/42member rows matching the proposal. No old public
row is removed or changed. Public scalar families remain String=1/Decimal=2;
future values require consumers' default branches. Producer-only constructors
and nine explicit options are verified. Signature and runtime qualification do
not imply general C45/API freeze or acceptance beyond this candidate.

Four actual packaged README C# blocks compile/execute through the package,
including old decimal factories/diagnostics and the new scalar workflow. The
previously illustrative proposed consumer also compiles and runs with preserved
` INV-001 ` and exact12.5. Namespace/named-decimal XML examples remain verified.
[Example results](README-RESULTS.json). Strict docs/whitespace, programmatic
rendering/link checks and raw source/evidence hashes are recorded separately;
no new visual acceptance or publication claim. REQ009/CoreREQ142 remain In Progress.

Next separately adopted datatype slice is Boolean, then exact integer, followed
by string facets and attributes. Current candidate remains local/uncommitted
under AGENTS.md Git Rules; commit/push/release need their explicit scope.
