# Internal native decimal backend qualification

Date: 2026-11-15. Base HEAD: `302d6f6b168b692dc053b0160f4babde8ebfad15`; uncommitted backend/profile/probe changes. The [receipt](verification.json) binds current implementation, tests/project inputs, authored fixtures, package/runtime identities and logs. Earlier scalar-probe receipts retain their original source/project hashes and test counts.

## Delivered behavior

Internal NativeDecimalSchema compiles and completely assesses the [finite grammar](../../NATIVE-DECIMAL-GRAMMAR.md): ordered optional/repeated decimal particles, decimal lexical forms, exact coefficient/scale capacities and bounded assertions. It refuses unrecognized schema constructs without delegating to a BCL structural validator. NativeDecimalAssertion fully parses the expression grammar, bounds nesting/tokens/length and evaluates exact comparisons with deterministic short-circuit rules.

Supported structural/lexical/assertion failures are Invalid. Missing coverage or capacity is Unsupported. Both expose no assessed document. Cancellation throws. Successful assessment retains immutable original declaration/type references and present-only states, including the anonymous element-only root without typed value. Repeated leaves are scalar decimals; optional absence creates no synthetic node. XML exports are owned copies, without Core authority.

The implementation was written from the recorded specification basis and executable grammar; no archived native implementation or private source was imported. It stays internal through the existing test-friend boundary. Public APIs, factories, Core pins and MinVer configuration are unchanged. No package version/tag/release is declared.

## Qualification and limits

All **76 backend cases** pass. Together with 124 retained regressions and seven scalar transport probes, the **207-case standalone suite** passes with zero failures/skips. Release build has zero warnings/errors. [Build](build.log), [focused cases](backend.log) and [complete tests](tests.log) retain output with repository paths replaced by `<repo>`.

Coverage includes authored golden XML/state/export checks; valid and invalid sequence/occurrence behavior; exact large decimals; lexical and pre-normalization capacity boundaries; namespace/QName handling; schema and instance refusals; empty/repeated assertion operands; Boolean precedence/short-circuit/type errors; expression length/token/nesting, declaration/assertion/node and UTF-8 byte ceilings; invalid Unicode/pre-cancellation; and concurrent independent assessments. Source and fixtures enumerate the exact catalog.

This is backend evidence toward P01/P02/P05/P08, not completion of those session groups. Engine-root atomization, authentic Core fact associations, adapter environment binding, leases, mid-work cancellation/drain/disposal, real native consumers and cross-universe proof remain pending. Independent-assessor concurrency is not shared-session safety. Full XSD 1.1, N1–N8 completion and the historical Core 135-case lane are not qualified by this delivery.

## Reproduction

```powershell
dotnet build Bosak.Schema.slnx -c Release --artifacts-path out/native-backend -p:NuGetAudit=false
dotnet test Bosak.Schema.slnx -c Release --artifacts-path out/native-backend --no-build --no-restore --filter FullyQualifiedName~NativeDecimalBackendTests --logger "console;verbosity=detailed"
dotnet test Bosak.Schema.slnx -c Release --artifacts-path out/native-backend --no-build --no-restore
```

NuGetAudit=false works around unavailable vulnerability metadata; no vulnerability audit is claimed. Isolated output avoids default obj access failures. The official docs check requires PowerShell 7; the local app alias cannot launch and Windows PowerShell 5.1 cannot satisfy its requirement. Link, whitespace and receipt checks are separate controls, not that official gate.

Next: implement authority/advisor/input/lifetime adapters and a real `$value` consumer, then reconcile the supported/refused P01–P12 controls on one exact candidate.
