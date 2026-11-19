# M2C1 concrete API proposal

19 November 2026. **Proposed, unimplemented, not API acceptance.** Baseline Schema
fbb4c1331b81c8daf3131626399119d4c2b312b9. This is the concrete review package
for [ADR011](../../ADR-011-owned-native-scalar-facade.md), not a replacement of
accepted bounded ADR010. [Exact signatures](SURFACE.json) define10new public
types in Bosak.Schema. No existing type/member is removed, widened or renamed.

## Selected surface

NativeSchemaSession.Create/Assess/Dispose and NativeSchemaDocument.ScalarCount/
ReadScalar/ExportXml/Dispose form an additive synchronous opt-in profile.
NativeScalarValue is sealed, immutable and producer-only, with Kind and checked
StringValue/DecimalValue getters. It holds only a detached CLR string or published
XdmExactDecimal; no node, XdmValue, provider/component/assessment handle, callback,
live PSVI or authority is returned. Its getters work after all owners close.
Wrong-kind access throws a fixed InvalidOperationException. No public factories,
null string scalar or default scalar object can manufacture successful reads.

M2C1 Kind values are String=1 and Decimal=2;0 is reserved/unissued. Consumers use
a default branch for future enum values. BooleanValue/IntegerValue and Boolean/
Integer enum members are **future proposals**, absent until their own slices.
The Core probes cover their feasibility without shipping them in M2C1.

Present leaf indices are zero-based in instance order across both kinds; absent
optional leaves have no placeholder. Root is never a fabricated scalar. Reads
must pass through original Core facts/snapshot/typed-value transport before
copying detached values. Named decimal restrictions preserve identity internally.
No public type metadata or relations are added merely by exposing a value.

## Explicit options and fixed profile bounds

The exact nine-parameter constructor and immutable properties are in SURFACE.json.
All arguments are explicit; there is no default options instance, mutable builder,
unlimited sentinel or host clock. Proposed validation:

| Option | Admitted values and unit |
|---|---|
| MaxNativeCoefficientDigits |1..256 supplied coefficient digits before normalization |
| MaxNativeScale |0..256 supplied fractional digits before normalization |
| MaxStringCharacters |1..65536 UTF-16 code units of parsed XML string value, before issuing facts |
| MaxExportCharacters |1..524288 UTF-16 code units of detached serialized XML |
| MaxOpenDocuments |Positive admission count |
| MaxCachedScalarValuesPerDocument |Positive Core typed scalar cache capacity |
| MaxTypedResultAtoms |Positive Core typed atomic result capacity |
| MaxQueryCoefficientDigits |Positive Core numerical read capacity |
| MaxQueryScale |Nonnegative Core numerical read scale capacity |

Bad options throw ArgumentOutOfRangeException at construction. Upper bounds on
native numbers/string/export are deliberate new-profile restrictions; they do
not alter the old decimal options. Fixed schema/instance ceilings stay32/64KiB
UTF-8 and256XMLnodes;16global types/16roots/finite sequences/assertions remain.
String code-unit caps are representation limits, **not XSD length facets**.
Supplementary characters count two units; no Unicode normalization is performed.
Empty strings are valid despite positive capacity. DTD policy and XML1.0 parser
normalization stay explicit. Input byte caps and string character caps are separate.

Export measures serialized UTF-16 length with a bounded writer before appending
beyond MaxExportCharacters. Escape expansion is accounted for. Overflow throws
a fixed NotSupportedException; never truncate or return partial XML. This is a
proposed implementation requirement, not a measured memory/time guarantee.

NativeSchemaAssessmentContext(DateTimeOffset,int) copies the caller's constant
date/time and validates timezone -840..840minutes with ArgumentOutOfRangeException.
No ambient date/timezone, resolver or network resource is acquired.

## Results, exceptions and ownership

NativeSchemaOutcome values are Valid=0, Invalid=1, Unsupported=2. A producer-only
NativeSchemaResult<T> has non-null owned Value and empty immutable Diagnostics
only for Valid. A refusal has null Value, exactly one immutable diagnostic and
DiagnosticsTruncated=false. No public constructors allow success-shaped forgery.
The separate NativeSchemaUnsupportedReason names/values are pinned in SURFACE.json;
consumers tolerate Unspecified/future values. Existing decimal result/enums remain
unchanged.

Use the existing BSK-NATIVE-{SCHEMA|INSTANCE}-{INVALID|UNSUPPORTED} stage codes on
the new result types. Messages are fixed, <=160characters, without schema/XML,
raw parser text, exception graphs or secrets. Proposed exact messages:

- Invalid: "The input is malformed or violates the supported native scalar profile."
- Unsupported: "The input or representation is outside the supported native scalar profile."

Unmapped refusal/Invalid reasons are Unspecified. Exact new grammar/refusal order
is fixed in semantic adoption before code, not inferred from these enums. String
capacity refusal is RepresentationCapacity. M2C1 admits primitive xs:string plus
the existing M2B decimal profile; new datatype/attribute/import/assertion syntax
outside that profile refuses complete compilation, including unused declarations.
Decimal-only assertions may retain behavior; nondecimal references refuse until
separately adopted. The old NativeDecimalSession still refuses mixed schemas.

| Operation | Proposed exception behavior |
|---|---|
| Create |ArgumentNullException for schema/options; cancellation throws OperationCanceledException; malformed/profile refusals use result |
| Assess |ArgumentNullException for instance/context; closure=>ObjectDisposedException; overlap=>NotSupportedException; quota/cleanup=>fixed InvalidOperationException; cancellation throws |
| ScalarCount |Closed owner=>ObjectDisposedException; overlap=>NotSupportedException |
| ReadScalar |Null is not an index; out of range=>ArgumentOutOfRangeException; closure/overlap/cancellation as above; Core/query representation refusal=>NotSupportedException |
| ExportXml |Closed/overlap/cancellation as above; output capacity=>NotSupportedException; cancellation checked before and during bounded writer output |
| Scalar getters |Wrong kind=>InvalidOperationException; detached values survive owner closure |
| Dispose |Revocation and all-resource cleanup; fixed InvalidOperationException if any cleanup fails |

Check required arguments before entering session work; after admission, closure
and overlap gates precede instance/index work as in the existing owner model.
Document disposal releases its admission and revokes that document; session
remains usable. Session disposal revokes all owned documents. A private gate
serializes admitted work/close; overlapping/reentrant operations refuse, not queue.
Every cleanup action is attempted once, even after failure, with sanitized counts
and no raw inner exception. Repeated disposal does not retry cleanup or certify
physical release. Shared internal ownership helpers may be reused under explicit
profile selection; no duplicate weaker gate. Deterministic concurrency/failure
controls must prove this implementation, not merely copy ADR010 wording.

## Public-only feasibility outcome and remaining gates

Two14-case synthetic-provider lanes passed on exact published Core1.81.2 and
1.81.17. They establish typed payload identity/evaluation, declared/actual/origin
observation, foreign/plain refusal and snapshot/operation lifetime distinctions.
They are **not** XSD validation, actual future Schema node facts, input lexical
validation, Unicode facets, limits/performance acceptance or a new API implementation.
[Probe details](README.md), [package identities](IDENTITY.json).

Core scalar provenance observation is snapshot-bound; complete-container views
are operation-local. The proposed facade exports neither. No engine change or
package upgrade is needed by the tested transport route; original-node atomization
and the actual mixed Schema assessor remain implementation gates. Missing
capabilities require a concrete CoreREQ142 reproduction before new engine work.

Before product edits: finalize exact string semantic adoption using pinned W3C
clauses and intentional limits, then implement M2C1 with new public-consumer,
lifecycle, actual-node/provenance, export-overflow and compatibility controls.
Keep534baseline cases and retained124/23/33catalogs distinct; add signed package
qualification, API surface diff and documentation evidence. No new API is shipped
or accepted and no tag/publication/commit/push follows from this document.
