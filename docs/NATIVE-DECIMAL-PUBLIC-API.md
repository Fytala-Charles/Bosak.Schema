# Owned native decimal public API

Date: 17 November 2026. Scope: the owned API shipped in0.5.0-alpha.0.7; current bounded beta contract acceptance under REQ-009 and [ADR-010](ADR-010-owned-native-decimal-facade.md) was accepted for the bounded Schema beta scope on17 November2026. [Pre-implementation basis](native-decimal-public/2026-11-17/BASIS.json) retains the historical specification hashes, sections and intentional restrictions unchanged. The finite backend and assertion evaluator are unchanged from df40a17; public ownership and host-policy behavior is implemented here.

## Public contract

`NativeDecimalSession.Create(schema, options, token)` validates required options/schema references, checks cancellation and compiles the existing finite schema. `NativeDecimalSessionOptions` must be constructed with all seven capacities; counts/digits must be positive and scales nonnegative. Invalid options fail in the constructor, before any input. `NativeDecimalAssessmentContext` validates timezone -840..840 at construction and retains the caller's explicit fixed date/time. No host clock, timezone or implicit workload defaults are read.

`session.Assess(instance, context, token)` validates required references before entering the shared implementation gate. Complete Valid returns an owned `NativeDecimalDocument`; otherwise Value is null. The document exposes only present-leaf DecimalCount, zero-based ReadDecimal and detached ExportXml. Optional absent children occupy no scalar index. A root is not a fabricated scalar. ReadDecimal returns the published `XdmExactDecimal` type; its formatting capacity is separately supplied by the consumer. Export is detached XML retaining decimal lexical spelling, not canonical XSD serialization or exact source bytes. Neither detached output carries PSVI or transferable authority.

The schema/instance UTF-8 ceilings remain 32/64 KiB, with the independent parser/grammar/lexical limits in the [grammar](NATIVE-DECIMAL-GRAMMAR.md). Native numerical limits apply before normalization. Query limits may refuse a previously assessed native value; no rounding or fallback numeric type is permitted. Counts are not total-memory, elapsed-time or concurrency performance budgets.

## Outcomes and bounded diagnostics

`NativeDecimalResult<T>` is a sealed class with no public constructor. Only Valid owns a non-null session/document, with no diagnostics. Invalid indicates malformed XML or a supported violation; Unsupported indicates missing finite coverage or representation. Each refusal has exactly one immutable `NativeDecimalDiagnostic`, a fixed message below 160 characters and one stable code:

| Stage | Invalid | Unsupported |
|---|---|---|
| Create | BSK-NATIVE-SCHEMA-INVALID | BSK-NATIVE-SCHEMA-UNSUPPORTED |
| Assess | BSK-NATIVE-INSTANCE-INVALID | BSK-NATIVE-INSTANCE-UNSUPPORTED |

No parser text, caller input, raw exception graph or provider evidence is copied into these diagnostics. This is deliberately a stage summary, not an exhaustive backend report; DiagnosticsTruncated is false because the public one-stage collection itself is not truncated. Detailed stable subcodes/locations are not promised. Callers cannot construct success-shaped results or diagnostics. Result inspection does not transfer cleanup ownership away from the returned value.

Arguments, pre/during-work cancellation, quota exhaustion, closed objects and overlapping operations throw their documented exceptions. They are not converted to Invalid. Cancellation is cooperative; XML export checks before synchronous serialization, not during it. A caller abandoning its own Task wait cannot close resources that admitted synchronous work is still using.

## Lifecycle and cleanup

One owned Core session/runner gates every document operation. Overlapping and reentrant operations refuse with NotSupportedException, not a queued parallel run. Dispose serializes behind admitted synchronous work on the private gate. This does not promise fairness or an upper latency bound. A racing operation either completes before closure or refuses as overlapping/closed; no callback or raw live authority is exposed publicly. The underlying Core APIs are unchanged.

Document disposal removes its admission before attempting snapshot and input cleanup. Session disposal first marks the session closed, captures and clears all document admissions, then attempts every snapshot/input cleanup followed by Core session disposal. Every action is attempted once even if another fails; a sanitized InvalidOperationException reports the number of failures with no raw inner exception. Repeated disposal does not retry failed cleanup. Access remains revoked, but a cleanup exception does not certify physical cleanup success. Catastrophic process/runtime failures and arbitrary asynchronous/reentrant-disposal schedules are outside this bounded policy.

Six deterministic implementation controls cover held-work versus document/session close, overlap/reentry, admitted cancellation/fault gate release and faults at each position in the common all-resource cleanup helper. The close tests hold the actual facade work gate with barriers; they do not use timing sleeps. Fault controls use disposable doubles through the same cleanup helper, not manufactured failures in Core's production disposer. External controls cover disposal of multiple documents and independence of a second session. These are finite schedules, not arbitrary concurrency proof.

## Qualification and release boundary

The [receipt and P01-P12 map](native-decimal-public/2026-11-17/README.md) distinguish executed source, package and lifecycle lanes. Both consumer lanes use published Core 1.81.2 references without source overrides or an InternalsVisibleTo grant. The merged source and clean-source compatibility package now pass345source/33package-only cases and retained124/23 binary catalogs; [current contract review](BETA_CONTRACT_REVIEW.md) records the owner-accepted scoped Schema dependency/ADR contract. The0.5 development minimum and Core1.81.2 remain unchanged. A compatibility alpha package is not a beta release.

Full XSD 1.1, arbitrary queries, streams/readers, positive cross-universe handoff, complete relations, overall provider sign-off, performance budgets and Braid product integration remain outside this candidate. Existing services retain their documented distinct scopes; the later [compile-diagnostic guard](LEGACY_VALIDATION.md) intentionally prevents legacy success on bad compiled models. Historical adoption/proposal receipts are not rewritten.
