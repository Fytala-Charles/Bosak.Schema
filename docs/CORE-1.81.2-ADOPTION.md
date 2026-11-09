# Core 1.81.2 dependency adoption

Updated 9 November 2026. **Merged in [Schema PR #1](https://github.com/Fytala-Charles/Bosak.Schema/pull/1)** at c953ebe0057bea1616d3e4f891f7b0a5fc4bd99b; reviewed source commit 9d1881ef7ccc52e9c756e864e984a6211ae514cc. This is no longer an unpushed local candidate. Four product references and the test project's direct provider reference select published 1.81.2 packages. Schema C# source and MinVer configuration are unchanged; its development line remains0.4. No new Schema NuGet package or version tag is announced.

## Recorded qualification

- Baseline1.26.3 and rebuilt1.81.2 Schema solutions each pass the same124 cases, with zero build warnings/errors.
- The unchanged1.26.3-built Schema/test/configuration lane passes124 cases after only the nine engine DLLs are replaced by verified published 1.81.2 payloads. This is exercised compatibility, not an arbitrary downstream ABI guarantee.
- The exact committed candidate is freshly rebuilt/tested:124 cases plus11 additional bounded price/assertion/ownership cases pass. Repeated source/binary catalogs are not additive coverage.
- [Schema CI](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/42601780777) validates build,124 tests and documentation hygiene over the candidate source.

[Core-side adoption receipts](https://github.com/Fytala-Charles/Bosak/tree/main/docs/schema-core-1.81.2-adoption/committed) preserve exact source, binaries, case catalogs and the initial local delivery state. They are historical receipts, not a live remote-delivery status query.

## Core optional integration

[Core PR #472](https://github.com/Fytala-Charles/Bosak/pull/472) refreshes the optional wrapper to merged Schema c953ebe. Its source/asset guard pins20 library files,13 upstream test files, two fixtures and LICENSE/NOTICE, accepting only exact LF/CRLF forms. The suite passes135 cases (124 upstream, eight price/assertion and three ownership), and the typed-price example matches. [Current pin/qualification](https://github.com/Fytala-Charles/Bosak/tree/main/docs/schema-integration-1812), [hosted integration](https://github.com/Fytala-Charles/Bosak/actions/runs/42611185565).

The wrapper compiles actual Schema source against Core projects, remains nonpackable and outside Core's normal solution. It neither republishes Schema nor introduces a Core-to-Schema package dependency. Its original044d2cb2/114-test dossier remains historical and unchanged.

## Remaining scope

Current stream/reader ownership checks do not establish native operation leases or concurrent-disposal safety. Full XSD 1.1 structural validation, native assessor/advisor/session integration and component-universe compatibility remain separate work. The retained extra-child case explicitly shows the structural-validation gap. No accepted Schema workload budget, independent reviewer signature or performance guarantee is inferred.
