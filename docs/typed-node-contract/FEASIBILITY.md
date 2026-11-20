# Actual native original-node data feasibility

20 November 2026. Unchanged production base
`43f14dbf56c36e5185e705af901678c42c8d1aff`.
**Both signed public Core1.81.2 and1.81.17 lanes pass73native controls each.**
Eight original-source before-code controls pass. All1,482existing source tests
(427implementation/1,055public) also pass against the isolated candidate on the
minimum Core lane with identical named outcomes. Public API remains52types/392rows.
All18Core signatures/source/archive/cache/runtime identities verify in
[IDENTITY.json](feasibility-evidence/IDENTITY.json).

These tests use actual native compiled declarations, captured XML, original nodes,
native assessment facts and Core admission. They are distinct from the earlier
synthetic90-control transport probes. They use Schema's existing friend assembly
for internal facts and a local Schema source-project reference; Core dependencies
are published packages only. No product package/new public API was built, and
these overlapping counts are not additional XSD conformance cases.

## Findings

The first provider-only candidate bound/assessed native XML but could not perform
successful data reads: the existing native wrapper did not implement the public
ISchemaAnnotatedNode original-fact contract. Both original failed runs and source
snapshots are retained. [Iteration2scope](FEASIBILITY-ITERATION-2.md) records the
required second candidate file rather than silently revising the one-file forecast.

The final isolated candidate changes only **NativeSchemaProvider** (explicit data
mode, exact profile/descriptor/default-context fingerprint) and **NativeSessionNode**
(guarded original-fact interface). The production files remain untouched. The
recorder captures descriptive context only and returns Unsupported without ever
assessing XML; successful facts are issued solely by the actual native advisor.
The default scalar mode keeps its existing binding/refusal behavior.

The48payload/owner cases cover all four scalar families, empty/false/zero,
supplementary/combining/control text, integers/decimals beyond decimal precision,
primitive/named/anonymous identity, root/descendant attributes and root/depth4
decimal complex simple content. The remaining25controls cover exact profile and
static-context rejection, unavailable navigation/predicate/arithmetic syntax,
forged-environment getter refusal, copied/foreign nodes, old scalar episodes,
selected snapshots in one session, aliased facts, invalid instances/targets,
unassessed hint attributes, FOTY0012, cache accounting/rollback, cancellation,
overlap, snapshot expiry, admission quotas and bounded export recovery.

Measured cache behavior: repeat reads reuse a target entry; a distinct element
or attribute consumes another entry; numeric digit/scale refusal leaves room for
a later small target. Original type/owner handles are authenticated inside the
operation; public-prototype outputs are detached scalar copies. Exact original
snapshots are required even for equal paths/content within one session.

## Decisions still required

Core reports element-only atomization as **InvalidOperationException containing
FOTY0012**. The proposed public facade suggested NotSupportedException; any mapping
must be explicitly adopted/documented and tested. The diagnostic now checks the
actual Core exception class/code; it does not pretend a production mapping exists.
The first run's interface failure and the second run's exception-class mismatch
are preserved alongside final passing receipts.

The strict context prototype captures a default-operation fingerprint through a
recorder which refuses binding, then requires that exact fingerprint in native
mode. This constrains context; fingerprints do not authenticate authority. The
engine-issued type/host/profile/descriptor and original tuple checks remain.
Hidden default-element-namespace changes, as well as namespace bindings, refuse.
Production adoption must select this mechanism or another equally qualified
closed default-context mechanism; do not broaden the old scalar provider guard.

No public facade was implemented. Product path/name argument mapping, exception
redaction, full facade cleanup-failure and deterministic disposal/overlap schedules,
consumer API review and production package/retained/API-delta qualification remain
pending. Successful native feasibility does not close generalC45/ADR006, arbitrary
XPath navigation, fullPSVI or whole-XSD acceptance. Frozen pilot9matches/12Unsupported
is unchanged and was not rerun for a contract/transport-only production step.

## Reproduce

[Materializer](feasibility-evidence/materialize.py) reconstructs exact public
Schema Git blobs and overlays only the recorded two candidate files into a new
scratch directory, preserving the sealed evidence. Pass a local Schema checkout
at the recorded base; the tool checks its test/source state before rebinding the
optional regression project paths. From the scratch directory:

```powershell
dotnet test baseline/Probe.csproj -c Release
dotnet test minimum/Probe.csproj -c Release "-p:RestorePackagesPath=$((Get-Location).Path)/minimum-cache"
dotnet test current/Probe.csproj -c Release "-p:RestorePackagesPath=$((Get-Location).Path)/current-cache"
python verify-identity.py
```

Use new result directories for new runs. Original diffs, source/hash manifest,
projects, test helpers, failed/final TRX, package signature logs and the API
comparison are under [feasibility-evidence](feasibility-evidence/EXECUTION.json).
Test helpers are not proposed product classes. Materialization and testing do not
modify the real src/ tree or publish packages.

Next: record a scoped semantic adoption resolving the measured adapter, context
and FOTY0012 points, then review the proposed API and implement the owned facade.
All current repository changes are documentation/evidence and remain uncommitted.
