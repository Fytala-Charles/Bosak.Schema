# Representative beta.1 invoice trial

18 November 2026. The owner selected the existing public invoice fixture as a representative package-only trial. This is an authored public example, not production-user data or production acceptance. [Receipt and exact fixture identities](beta1-publication/INVOICE-TRIAL.json).

## Inputs and isolation

Schema/XML/license come from public Core commit `c6df2a6500ea0eeae26207f833b8d0ceed31e2d6`, [fixture directory](https://github.com/Fytala-Charles/Bosak/tree/c6df2a6500ea0eeae26207f833b8d0ceed31e2d6/docs/foundation-c45-handoff136/fixtures). Executed bytes are LF-equivalent to the immutable Git inputs; original and executed hashes are recorded separately. Apache-2.0 provenance is retained. No commercial/private source was used.

The standalone console uses only `PackageReference` Bosak.Schema 0.5.0-beta.1 and NuGet.org, with an isolated cache, no source overrides, friend access or project references. The actual signed Schema archive/cache/runtime DLL and all nine Core 1.81.2 package signatures and payloads match. Capacities and assessment date/timezone are explicit. Diagnostic exceptions are caught; no unhandled .NET negative case was launched.

## Observations

| Workflow | Observed result |
|---|---|
| Original invoice schema and XML | Valid; two present decimal leaves |
| Exact scalar reads | 10 and 5; original lexical XML retained in detached export |
| XML export | Parsed root tree equals the input root, including its comments/PI and namespace declaration; no exact source-byte or canonical serialization claim |
| One-document capacity | Second admission throws; disposing the first permits reuse |
| Supported assertion violation | Single amount refuses as Invalid, with no owned document |
| Application attribute | Unsupported, with no owned document |
| Schema target namespace | Unsupported, with no owned session |
| Pre-cancelled assessment | OperationCanceledException caught; next valid assessment succeeds |
| Disposal | Closed access refuses; detached exact values and XML remain usable |

## Implications

No new engine defect or evidence for changing beta.2 scope was observed. The fixture's historical Core dossier also exercises broader operations; this trial covers only the public Schema assess/read/export workflow above. It does not requalify PSVI, arbitrary XPath/sum queries, general Core C45/ADR006, production workloads, arbitrary concurrent schedules or timing/memory SLAs.

The next useful consumer gate is an identified application schema/XML workflow with explicit success and refusal expectations. Record a concrete gap before proposing beta.2 scope or Braid/XmlDb adoption. Keep representative observations separate from the 345 product tests and 33 package-consumer catalog.
