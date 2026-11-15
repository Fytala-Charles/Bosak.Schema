# Internal native adapter qualification

Date: 2026-11-15. Baseline: backend commit `aba6967`; adapter changes remain uncommitted. [Design and scope](../../NATIVE-SESSION-ADAPTER.md), [receipt](verification.json), [build](build.log), [adapter tests](adapter.log), [complete suite](tests.log).

Twenty adapter cases exercise real native assessment, authority/fact/node/value issuance, exact bound environments, original provenance through `$value`, engine-root atomization, no-root failure outcomes, relation/compatibility refusal, numeric/cache/snapshot limits, deferred access revocation, mid-assessment cancellation, canceled drain waits and sequential host gating. These extend the 207-case backend checkpoint to **227 standalone cases**, zero failures/skips. Release build has zero warnings/errors. Current inputs/packages/runtime binaries and logs are hash-bound in the receipt; earlier backend/probe receipts are retained unchanged.

The real consumer is an internal friend-assembly integration test, not an external packaged consumer. One shared NativeSessionRunner per session is required for the sequential host profile; direct/multiple runner use is outside that policy. Core's data(.) failure shape is InvalidOperationException with FOTY0012 in its message, while direct guarded root atomization uses XPathErrorException. No Core change was made.

```powershell
dotnet build Bosak.Schema.slnx -c Release --artifacts-path out/native-adapter -p:NuGetAudit=false
dotnet test Bosak.Schema.slnx -c Release --artifacts-path out/native-adapter --no-build --no-restore --filter FullyQualifiedName~NativeSchemaAdapterTests --logger "console;verbosity=detailed"
dotnet test Bosak.Schema.slnx -c Release --artifacts-path out/native-adapter --no-build --no-restore
```

NuGet auditing remains unverified because nuget.org metadata was unavailable; the explicit local workaround is not audit acceptance. The official documentation checker still requires a working PowerShell 7 executable. Repository links, receipt hashes and whitespace are checked separately. No public API, release, full XSD 1.1, exhaustive P01–P12 sign-off, numerical budgets or historical Core 135-case requalification is claimed.
