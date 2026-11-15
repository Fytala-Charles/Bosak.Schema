# Published Core scalar-session probe

Date: 2026-11-15. Baseline HEAD: `302d6f6b168b692dc053b0160f4babde8ebfad15` with the uncommitted profile and probe changes identified in the [receipt](verification.json). Test source: [NativeSessionProbeTests](../../../tests/Bosak.Schema.Tests/NativeSessionProbeTests.cs).

## Result and scope

Seven focused cases exercise the published Core 1.81.2 session contracts with an issued **test-only** decimal fixture. The fixture performs no structural assessment; its Assess method refuses execution. The real native provider/backend remains pending. This is released transport evidence, not P01–P12 completion or XSD 1.1 acceptance.

The supported path is `SchemaQuerySession.CompileXPath("$value", new SchemaQueryLimits(1, 4, 1, 28, 8), assessor: fixture)` with omitted/default compile options, then OpenSnapshot with the exact input/lifetime/origin, and Run over that snapshot. Inside the callback, ReadTypedValue returns a registered scalar; Evaluate receives it through `new EvaluationContext().WithVariable("value", scalar)`.

The observed environment is XPath 3.1, XDM `https://www.w3.org/TR/2017/REC-xpath-datamodel-31-20170321/`, behavior `bosak-typed-identity-v1`. The focused log retains the descriptor version and fingerprint. These facts describe this exact default-options probe, not all accepted options or provider environments. The small numeric/count limits are test choices, not recommended workload defaults.

The identity read preserves decimal 12.50 and declared/actual component plus original assessment provenance inside the live callback. A detached decimal payload is returned; borrowed nodes/metadata are not exported. The host lease is still retained after Run and is released when the snapshot is disposed.

## Executed controls

- Issued decimal identity: exact value and original provenance preserved.
- Forged container: ArgumentException before payload getter access.
- Snapshot outside Run membership: ArgumentException.
- Closed snapshot: ObjectDisposedException on new Run.
- `unknown()` syntax: explicit Unsupported and nonempty diagnostic.
- Default limits: ArgumentException.
- Pre-canceled compilation: OperationCanceledException.

All seven use the same published-package runtime as the full standalone suite. They do not prove a production authority/advisor, relation completeness, compatibility, concurrent disposal, async drain cancellation, performance or a real native consumer. No historical Core integration lane is requalified.

## Reproduction and evidence

From the repository root:

```powershell
dotnet build Bosak.Schema.slnx --artifacts-path out/review -p:NuGetAudit=false
dotnet test Bosak.Schema.slnx --artifacts-path out/review --no-build --no-restore --filter FullyQualifiedName~NativeSessionProbeTests --logger "console;verbosity=detailed"
dotnet test Bosak.Schema.slnx --artifacts-path out/review --no-build --no-restore
```

NuGetAudit=false is a local workaround for unreachable nuget.org, not a vulnerability-audit result. The isolated output avoids the default obj access-denied condition. Documentation checks still require a working PowerShell 7 installation; the current app alias cannot launch here.

[Build log](build.log), [focused log](probe.log), [full test log](tests.log) and [verification receipt](verification.json) record the final run. Paths in logs are replaced by `<repo>`. The receipt hashes the test source, dependency/package assets, runtime Bosak assemblies and logs. Any later source or package change requires fresh qualification.
