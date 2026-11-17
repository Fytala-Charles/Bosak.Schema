# Bounded Schema beta qualification

17 November 2026. Main/source22aa9ad84959f95d92dc82626117955edd1fdb0b; PR10 merged and main44147900719 green. The owner explicitly selected **bounded decimal-session beta with explicit exclusions**. [Scope decision](beta-qualification/SCOPE-DECISION.json), [measurement basis](beta-qualification/BASIS.json), [raw summary](beta-qualification/MEASUREMENTS.json), [evidence](beta-qualification/README.md).

## Chosen support boundary

The first beta targets the existing owned synchronous NativeDecimalSession/NativeDecimalDocument workflow: explicit string input and limits, finite unqualified decimal schema grammar, original assessment, detached exact decimals and XML, and explicit Invalid/Unsupported/exception boundaries. It does not target general XSD1.1 validation. Namespaced native schemas, application attributes, broader datatypes, nil/list/union, arbitrary XPath assertions, streams/async native APIs and positive cross-universe transfer remain excluded. Legacy descriptor/assertion and XSLT helpers remain separately documented compatibility services; their success is not a new native assessment or general schema certificate.

PR8 restricted resolution, PR9 immutable compiler snapshots and PR10 compile-diagnostic guard address the audited hardening slices within their documented limits. Existing directory/text-prefix helpers still have their old compatibility semantics. Restricted IO requires stable host-owned paths, not an atomic filesystem sandbox. Public descriptors/results remain constructible data. [Native grammar](NATIVE-DECIMAL-GRAMMAR.md), [owned API](NATIVE-DECIMAL-PUBLIC-API.md), [restricted IO](RESTRICTED_RESOLUTION.md), [snapshots](DESCRIPTOR_SNAPSHOTS.md), [legacy results](LEGACY_VALIDATION.md) define distinct scopes.

## Measured workflows

One local Windows x64 machine: Intel Core Ultra9 285HX,24 logical processors,102,496,944,128 physical-memory bytes; .NET10.0.11, SDK10.0.301, workstation GC. Each workload has three fresh-process first-use samples and one process with five warmups plus30 measured iterations. The parent imposes a30-second process watchdog and records separate wall time including startup/setup. First use does not flush filesystem caches; warmed runs do not guarantee fully converged JIT/PGO state. Fixtures are prepared before measured actions. Correctness checks, formatting/export and disposal are included in action measurements.

| Workflow | Fresh-process workload max ms | Warm p95 ms | Warm calling-thread allocation max MiB | Process peak working set max MiB |
|---|---:|---:|---:|---:|
| native-small | 262.78 | 0.544 | 0.089 | 69.1 |
| native-max-bytes | 265.16 | 9.826 | 1.311 | 70.0 |
| legacy-facets | 230.98 | 0.506 | 0.098 | 67.3 |
| restricted-xslt | 252.82 | 10.765 | 0.329 | 76.7 |
| refusal-recovery | 272.50 | 2.616 | 0.289 | 69.4 |

native-small compiles, assesses, reads/formats a decimal, exports and disposes. native-max-bytes uses exactly32KiB schema and64KiB instance,16 leaves, eight assertions, mixed256-digit integers and255-place decimals, then reads every value and exports. This is a selected combination of admitted limits, not an exhaustive maximum of every possible cost. legacy-facets compiles immutable descriptors, checks positive/negative facets and guarded bad compilation. restricted-xslt acquires a contained schema, compiles and runs a small transform. refusal-recovery checks malformed/unsupported/oversize input, quota recovery, pre-cancelled create/assess/read/export and post-disposal refusal/reuse.

Calling-thread allocations count managed bytes allocated during the interval, not live heap or native/worker-thread allocations ([GC API](https://learn.microsoft.com/en-us/dotnet/api/system.gc.getallocatedbytesforcurrentthread?view=net-10.0)). Peak working set is the lifetime process peak, including runtime/JIT/setup/warmups, not a per-operation memory delta ([Process API](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.peakworkingset64?view=net-10.0)). Warm p95 uses nearest rank over30 samples. No forced collection or isolated-machine treatment was used.

All five stayed within the **pre-recorded proposed local observation envelopes**: cold workload<=5s, warm p95<=250ms, cold calling-thread allocation<=64MiB, warm<=16MiB and lifetime process working set<=512MiB. These broad smoke thresholds are not owner-accepted production SLAs, hard engine budgets or cross-platform performance guarantees. Initial integer-only measurements remain raw historical data; final results here use mixed high-scale values and are not pooled with the earlier workload.

## Reproduction

```powershell
dotnet build tools/Bosak.Schema.Workloads/Bosak.Schema.Workloads.csproj -c Release
python -m unittest discover -s tools -p test_schema_workloads.py
python tools/run-schema-workloads.py --assembly tools/Bosak.Schema.Workloads/bin/Release/net10.0/Bosak.Schema.Workloads.dll --output .workload-results/run-001
```

Use a new output directory every time. The runner pins built DLL/configuration hashes before each child launch, validates raw sample identity and correctness, retains stdout/stderr and stops on failure/timeout. Six measurement-integrity/refusal controls use synthetic files and mocked launchers; they do not intentionally launch unhandled negative cases. The diagnostic catches failures internally and uses only self-created temporary fixtures, cleaned nonrecursively. Normal CI builds the tool and runs these six controls; it does not enforce local timing thresholds or run this measurement protocol automatically.

## Acceptance checklist after this pass

| Gate | Current state | Required next evidence/decision |
|---|---|---|
| B01 Beta scope | Owner selected bounded decimal profile with exclusions | Publish the exact reviewed support matrix with the beta |
| B02 API/ADR contract | Hardening delivered;40 types/317 rows since PR8; ADR009/010 still Proposed | Review concrete ADR/API decisions, C45/Core boundaries and freeze expectations explicitly |
| B03 Correctness/security |345 product cases; recorded native/IO/snapshot/legacy boundary controls pass | Review test-to-promise map and any remaining scoped defects; no general conformance/fuzz certification inferred |
| B04 Workloads |20 fresh diagnostic processes; named local measurements and recovery checks pass proposed envelopes | Owner reviews useful operating budgets; other hardware/platforms and mid-work cancellation latency remain unqualified |
| B05 Consumer compatibility | Prior alpha registry consumer verified; source public consumer now33 cases | Prepare exact beta candidate; compare API and retained binary consumers, intentional behavior changes and dependencies |
| B06 Release | Alpha remains published; no beta tag/artifact/publication | Update first-release-pinned controller only for reviewed beta source/version/catalogs; fresh exact-artifact review and explicit publication approval |

The current release workflow is intentionally pinned to the already published alpha's source/version and272-case catalog. Do not dispatch it expecting to produce this beta. Any beta MinVer/tag/controller change is a separate reviewed step, not a version override hidden in the workload tool. Proposed0.5.0-beta.1 is a future release identity, not created here.

## Limits and recommendation

Pre-cancelled calls and quota/disposal recovery were measured; existing deterministic product tests still provide their recorded mid-work/lifetime controls. No new mid-work cancellation latency, adversarial scheduler, long-duration leak, maximum-live-document, full hostile-input matrix, native-heap peak or cross-platform budget was established. No OS filesystem sandbox or full XSD1.1 support follows from these results. Product source, references and engine gates are unchanged by this tool/documentation slice.

Next: review updated ADR009/010 acceptance proposals against the chosen bounded scope, then qualify a beta candidate's compatibility and packaging. Scope selection and successful measurements do not themselves accept those ADRs or authorize a beta publication.
