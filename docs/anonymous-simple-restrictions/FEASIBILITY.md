# Anonymous simple-type before-code feasibility

20 November 2026. Unchanged Schema base
`1438f719863d0c13f976f1b0eb683e146a3511e5` (merged PR29).
The [pre-implementation adoption](ADOPTION.md) remains unchanged.

**Both public Core 1.81.2 and 1.81.17 lanes pass 90 controls each**:
13 new anonymous simple-type controls and 77 inherited transport controls.
All **18 Core package signatures**, source revisions and archive/cache/runtime
payload identities verify in [IDENTITY.json](feasibility-evidence/IDENTITY.json).
Each lane restored independently with no project references. These controls use
a synthetic issuer that does not validate XML; the totals overlap and are not
180 native Schema cases or upstream conformance passes.

The new controls verify null QName annotations on simple (not complex/untyped)
nodes; empty and supplementary/combining/whitespace string payloads; zero and
large exact decimal payloads; original governing/declared/actual anonymous type
handles through node atomization and direct typed reads; shared original type
and declaration identity across repetitions; equal but distinct definitions;
foreign-node/value refusal; snapshot expiry and detached payload retention.
No primitive base handle, manufactured QName, private Core API, fallback validator
or engine/source-wrapper pin change was needed for this transport exercise.

Six **separate actual public Schema controls** pass on the unchanged engine:
anonymous decimal/string local types and NS-051 still compile Unsupported,
direct named decimal/string restrictions remain Valid and the unchanged ctB073
hint case retains its earlier validity match. The before lane's nine runtime
Core DLLs match the independently signature-verified minimum payloads.
These refusal controls do not establish positive anonymous XSD support.
The existing 1,302-case source regression remains the exact merged-main CI
receipt; no production, source-test or tool changes required a new full run.

Original project/source/TRX/signature receipts and separate scope/counts are in
[feasibility-evidence](feasibility-evidence/EXECUTION.json). The four inherited
probe sources are byte-identical to the prior 77-control dossier.
No native validation, anonymous facet parsing, named lookup, new API, package-only
Schema consumer or retained binary consumer qualification is claimed here.

## Reproduce

Copy the evidence directory to a scratch location before restoring, so immutable
receipts remain unchanged. From that copy, with .NET 10 and the NuGet feed:

```powershell
$probeRoot = (Get-Location).Path
dotnet test minimum/Probe.csproj -c Release "-p:RestorePackagesPath=$probeRoot/minimum-cache"
dotnet test current/Probe.csproj -c Release "-p:RestorePackagesPath=$probeRoot/current-cache"
python verify-identity.py
```

The identity verifier expects `minimum-cache` and `current-cache` directly under
its own directory; the commands use absolute RestorePackagesPath values. It verifies all
nine payloads per lane and exact package source commits. The separate before
project records its original local Schema ProjectReference; rebind only that path
to an unchanged checkout of the recorded Schema base and keep its results separate.

## Next implementation gate

Public transport feasibility is satisfied for the bounded adopted route. Next
implement shared anonymous/named restriction parsing and the combined local
definition budget, preserving original anonymous identities and eager refusal.
Actual Schema provenance/annotation checks, authored facet/capacity/legacy cases,
the explicit NS-051 correction, full source/package/retained qualification and
the unchanged 21-case pilot rerun remain required. Six upstream eligibility
targets are still unexecuted; the current pilot remains 1 match / 20 Unsupported.
No production changes, new publication or commit/push occurred in this step.
