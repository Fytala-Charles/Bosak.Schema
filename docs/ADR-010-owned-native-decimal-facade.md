# ADR-010: Owned synchronous native decimal beta facade

**Status:** Accepted — bounded Schema beta contract, 17 November 2026
**Date:** 2026-11-15
**Review date:** 2026-11-17
**Deciders:** Repository owner; bounded public contract explicitly accepted 17 November 2026

## Context

The facade is public and package-qualified; it is no longer an inaccessible prototype. [Original proposal text](beta-contract/ORIGINAL-ADR-010.txt) and [its identity](beta-contract/ORIGINAL-ADRS.json) remain historical evidence. The owner selected the bounded profile in ADR009; this public API/ownership decision was explicitly accepted on17 November2026 within that bounded scope.

## Accepted bounded decision

Accept NativeDecimalSession.Create/Assess/Dispose, NativeDecimalDocument.DecimalCount/ReadDecimal/ExportXml/Dispose, immutable options/context and factory-free result/diagnostic shapes as the bounded0.5 beta facade. Callers supply all capacities and assessment date/timezone. Successful results transfer cleanup responsibility for their owned Value; refusal has no owned Value. ReadDecimal exposes published XdmExactDecimal; callers separately supply formatting capacity. No rounding or fallback numeric type is introduced.

One private synchronous gate covers every document operation in a session. Overlapping/reentrant operations refuse; disposal waits for admitted synchronous work. Document disposal revokes its admission; session disposal revokes all documents and attempts every owned cleanup once. Cleanup failure is sanitized and does not certify physical release; repeated disposal does not retry it. No arbitrary fairness, maximum close latency, raw callback/handle, asynchronous API or arbitrary-schedule guarantee is selected.

Detached exact values and XML survive closure; neither carries live PSVI/authority. XML preserves decimal lexical spelling, not original source bytes or canonical XSD serialization. Export cancellation is checked before synchronous serialization, not guaranteed during it. Public native refusals carry one immutable fixed stage diagnostic using the four recorded codes; arguments/cancellation/quota/closure/overlap remain documented exceptions, not Invalid outcomes.

Keep compatibility services explicit: restricted resolver/factory is opt-in and requires a stable host filesystem; old directory/text-prefix helpers retain old semantics. Compiler-produced descriptor collections are immutable; caller-created positional records/with copies remain application data. Allthree legacy validation methods refuse compile findings before operation work; clean true results remain assertion/facet/static-check scope. Native diagnostic redaction does not extend to detailed legacy messages.

## Evidence and compatibility

Source/package consumer and retained-binary results are in [the qualification dossier](beta-contract/README.md). Public surface matches the merged source exactly at40types/317rows. Relative to published alpha, four rows were added for restricted IO; no declared row was removed. Mutating compiler collection implementations and ignoring compile findings no longer work: unchanged signatures are not a promise to preserve those defects.

The default qualified dependency combination is published Core1.81.2. Its use by this facade, including the public exact-value type, is the accepted scoped Schema dependency decision; it does not accept the whole Core provider/session surface or promise all higher dependency combinations. Wider C45/ADR006 and other product integration remain separate. Existing deterministic lifecycle tests and local workloads support only their recorded schedules/options.

## Approval boundary

Owner explicitly accepted this as the reviewed bounded beta contract baseline on17 November2026, with subsequent breaking changes explicitly reviewed/documented; it is not a perpetual1.0 freeze. API/ADR acceptance does not approve production SLAs, all caller policies, a beta tag/artifact, or NuGet publication. [Concrete review package](BETA_CONTRACT_REVIEW.md).
