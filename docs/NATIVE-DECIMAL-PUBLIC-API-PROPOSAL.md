# Owned native decimal public entry point proposal

Date: 2026-11-15. **Proposed, not public or accepted.** The internal NativeDecimalSession prototype and eight workflow cases make the design executable for review. [ADR-010](ADR-010-owned-native-decimal-facade.md) and [qualification](native-session-facade/2026-11-15/README.md). No API freeze, package/tag or external consumer acceptance is claimed.

## 1. Boundary demonstrated by the prototype

The session owns its Core session, the one shared sequential runner and every native document admission. Applications receive only document operations and detached results; no Core operation callback, raw session, provider handles, facts, provenance views or mutable backing XML is exposed. This closes the multiple-runner/bypassed-host-gate risk at the proposed entry point without claiming to change the raw Core APIs.

The prototype currently accepts explicit native numeric limits and SchemaQueryLimits. It compiles the finite schema and `$value` route, assesses already-owned string input with an explicit date/timezone episode, opens an original snapshot only after complete Valid and transfers ownership to a document. All unsuccessful captures, assessments and admissions release any acquired snapshot/input. A quota exception is operational failure, not Invalid schema/instance status.

Each document exposes its present scalar leaf count, zero-based present-leaf decimal reads and XML export. The root never becomes an artificial scalar; optional absent children occupy no index. ReadDecimal executes issued scalar admission and identity evaluation internally, then returns a plain XdmExactDecimal. ExportXml returns detached text with original lexical spelling, without PSVI/authority. The exact decimal is not a canonical XSD serializer.

Document close releases its snapshot and host input, freeing admission capacity. Session close closes all documents and the Core session. Calls after close throw ObjectDisposedException. The synchronous prototype holds its private operation/lifecycle gate through work; disposal waits for that private synchronous work. No async operations or caller callbacks are exposed. This is not arbitrary asynchronous/concurrent-disposal qualification. Overlapping work refuses at the operation gate; no hard time or memory bound follows from byte/count ceilings.

## 2. Proposed public shape

These signatures name the intended contract, not symbols shipped by the assembly. Implementation must add complete XML documentation and external-consumer tests before exposure.

```csharp
NativeDecimalResult<NativeDecimalSession> NativeDecimalSession.Create(
    string schema, NativeDecimalSessionOptions options,
    CancellationToken cancellationToken = default);

NativeDecimalResult<NativeDecimalDocument> NativeDecimalSession.Assess(
    string instance, NativeDecimalAssessmentContext context,
    CancellationToken cancellationToken = default);

int NativeDecimalDocument.DecimalCount { get; }
XdmExactDecimal NativeDecimalDocument.ReadDecimal(
    int leafIndex, CancellationToken cancellationToken = default);
string NativeDecimalDocument.ExportXml(CancellationToken cancellationToken = default);
void NativeDecimalDocument.Dispose();
void NativeDecimalSession.Dispose();
```

NativeDecimalSessionOptions is an immutable object with explicit positive maximum native coefficient digits, native scale (nonnegative), open documents, cached scalar values per document, typed-result atoms, query coefficient digits and query scale (nonnegative). No numerical/workload defaults or unlimited sentinel. The finite grammar's lexical/parser/byte ceilings remain separate hard restrictions. Invalid options must reject before input work; this validation order must be finalized in the public implementation. The prototype currently uses existing separate limit types.

NativeDecimalAssessmentContext carries explicit immutable current date/time and implicit timezone minutes, bounded to -840..840. No implicit host timezone/current-time policy. NativeDecimalResult<T> has Valid/Invalid/Unsupported outcome, a non-null owned value only for Valid, and bounded immutable diagnostics/truncation status. User construction of success-shaped session/document results is excluded. Existing SchemaDiagnostic may be reused after code/size policy review; no native facts or raw exception graph belongs in diagnostics.

The proposed XdmExactDecimal return type intentionally uses the published Core exact-value type already consumed by the implementation. External compilation/binary use, formatting capacity and dependency compatibility must be exercised before this becomes a permanent public dependency contract. A detached scalar carries no schema authority or transferable original provenance.

## 3. Executed controls and review gaps

The eight prototype cases cover golden scalar/XML output, detached values after close, invalid/unsupported/malformed input without admissions, document/session disposal and quota reuse, canceled/out-of-range reads, numeric admission refusal followed by successful reads, invalid compile limits/schema, invalid timezone cleanup and all-optional empty roots. The adapter suite now has 28 cases, adding container/raw expiry, scalar receipt expiry, exact scale limits, native membership/close checks, sync/async gate cleanup and balanced leases. Default lookup names now produce ArgumentException rather than NullReferenceException.

Remaining before public exposure:

- Implement the public DTO/result/option validation and bounded diagnostic contract with stable codes and complete XML documentation. Keep string capture and the finite grammar explicit; streams/readers are outside this first entry point.
- Build a consumer project without InternalsVisibleTo, through normal published Core packages, exercising happy/failure/ownership paths. The current consumer is still in the friend test assembly.
- Qualify facade close/work races and cleanup-failure policy separately; assess whether deterministic rejection versus serialized disposal is the desired permanent contract. Do not infer these guarantees from the runner or private synchronous workflow alone.
- Reconcile the exact bounded P01–P12 catalog for sign-off. Unsupported positive compatibility, relation closure, broader XSD/host profiles and performance budgets remain explicit separate work.
- Retain official documentation hygiene and NuGet auditing in public-entry qualification. Both now pass with required host permissions at this prototype checkpoint; earlier sandbox failures remain historical.

Target for a future new public feature is the next minor Schema line (candidate 0.5), subject to owner/version review. Current MinVer minimum stays 0.4; no version/tag/package change is made by this proposal.

*Last updated: 2026-11-15*
