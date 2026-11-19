# M2C1 API proposal and published-Core feasibility

19 November 2026. Schema baselinefbb4c13; no product source, Core API/pin, controller
or package changes. [Concrete API contract](API-PROPOSAL.md), [signature ledger](SURFACE.json),
[proposed ADR011](../../ADR-011-owned-native-scalar-facade.md) and
[illustrative future consumer](CONSUMER-EXAMPLE.md) are ready for review.
These documents are proposals, not API acceptance or shipping implementation.

## Executed transport evidence

| Lane | Passed | Scope |
|---|---|---|
| Published minimum Core1.81.2 |14/0/0 |Synthetic provider, public APIs only |
| Published current Core1.81.17 |14/0/0 |Same cases, separate cache/runtime identity |

The14cases per lane are eight payload variants (preserved/empty/Unicode strings,
true/false,50digit positive/negative integers and exact decimal) plus six controls:
foreign values/snapshots, snapshot-bound provenance expiry, operation-local
complete-container expiry, revoked input, plain-value authority refusal and actual
runtime version. Original declared/actual/origin handles are checked inside public
typed read/evaluation. These are repeated transport cases, not28new Schema tests.
[Results](RESULTS.json), [exact signed identities](IDENTITY.json),
[probe source](TransportTests.cs) and evidence/logs retain the executed inputs.

Both projects use nine published packages with no project reference, Schema
package, InternalsVisibleTo or private/candidate Core APIs.18official package
signatures, nuspec source pins and archive/cache/runtime DLL bytes match. The
assessor is an intentionally synthetic issuer; its Assess refuses and it issues
no node facts. Passing does not prove new XSD lexical/structural validation,
actual future Schema atomization, string/numeric capacity behavior, performance
or full XSD conformance. No engine change is needed for the tested payload route.
Actual Schema backend/facts and all new facade behavior remain implementation gates.

The exploratory test initially assumed scalar provenance was operation-local.
That expectation failed on both versions: scalar observations are snapshot-bound.
The final probe independently verifies expiry on snapshot close and expiry of
complete-container views on operation exit. The proposed facade exposes neither.
[Experiment corrections](EXPERIMENTS.json) retain this distinction; no engine bug
or change is inferred from a mistaken probe expectation.

## Contract selected for review

M2C1 proposes10new public types, string/decimal only, explicit nine-capacity options,
producer-only outcomes, present-leaf indexing, detached checked scalar getters and
bounded XML export. Old NativeDecimalSession remains decimal-only. Boolean/integer
transport feasibility does not pre-admit their future Schema slices or members.
No default/unlimited options, ambient context, provider handles or arbitrary query
callbacks are exposed. Concrete null/index/wrong-kind/lifecycle/cancellation,
capacity and sanitized diagnostic rules are recorded in the proposal.

The future consumer example is intentionally **not executed** against nonexistent
APIs. Its XML files are planning fixtures checked only for well-formedness.
The synthetic probe projects can be run independently with their linked source;
`dotnet test evidence/minimum/Probe.csproj -c Release` and the corresponding
current project. Use a fresh package cache to repeat provenance verification.

Next: finalize semantic adoption for M2C1 string handling and the proposed surface,
then implement the additive facade and actual mixed Schema path with public,
Core-node, lifecycle, export-overflow, package and retained-binary controls.
534existing Schema cases remain a delivered baseline; this step does not rerun
or relabel them. Existing accepted ADR010 remains unchanged. No commit/push or
publication without the separately required authorization.
