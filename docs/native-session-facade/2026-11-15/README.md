# Owned facade and remaining adapter controls

Date: 2026-11-15. Baseline: committed adapter `939ba9a`. Follow-up changes remain uncommitted. [Public proposal](../../NATIVE-DECIMAL-PUBLIC-API-PROPOSAL.md), [ADR-010](../../ADR-010-owned-native-decimal-facade.md), [receipt](verification.json).

The adapter suite expands from 20 to 28 cases, verifying complete-container/raw-operation expiry, scalar receipt expiry on snapshot close, exact scale boundaries, native foreign/closed/missing membership, pre-canceled runner cleanup, async fault cleanup, balanced/double-disposed owner leases and default-name rejection. The latter found and fixed a NullReferenceException on default SchemaExpandedName: all three lookup routes now throw ArgumentException.

An internal NativeDecimalSession prototype owns the sole Core session/runner and document admissions. Eight full-workflow cases cover detached exact decimal/XML output and owned cleanup, including optional empty roots and failed captures/assessment/admission. No public native symbols are introduced and no separate external consumer is qualified.

Final Release build with NuGet auditing: zero warnings/errors. **243 standalone cases pass**, zero failures/skips: 124 retained, seven scalar probes, 76 backend, 28 adapter and eight facade cases. [Build](build.log), [focused controls](controls.log), [full suite](tests.log), [official documentation check](docs.log) and [receipt](verification.json) bind commands, sources, dependencies, runtime binaries and output. Earlier receipts retain original identities/counts.

```powershell
dotnet build Bosak.Schema.slnx -c Release --artifacts-path out/native-facade -p:NuGetAudit=true --force
dotnet test Bosak.Schema.slnx -c Release --artifacts-path out/native-facade --no-build --no-restore --filter "FullyQualifiedName~NativeSchemaAdapterTests|FullyQualifiedName~NativeDecimalSessionTests" --logger "console;verbosity=detailed"
dotnet test Bosak.Schema.slnx -c Release --artifacts-path out/native-facade --no-build --no-restore
pwsh -NoProfile -File tools/check-docs.ps1 -ProjectPath .
```

The final network-enabled restore/build performs NuGet auditing and the official PowerShell 7 checker passes with required host execution permissions. Earlier restricted-shell failures remain historical. Link/whitespace/hash checks also pass. No full XSD 1.1, overall profile/API sign-off, arbitrary disposal-race guarantee, numerical budget, tag or release is claimed. Next is public DTO/diagnostic/ownership review and implementation with an external assembly consumer, preserving the proposal's remaining gates.
