# Bounded root choice production candidate

The two opt-in native facades now accept a bounded root `xs:choice` over existing scalar types. Choice alternatives are selected by expanded name, retain their original declarations and physical node order, and publish only present scalar facts. The optional empty choice remains element-only with no root typed value.

## Adopted scope

[Production adoption](PRODUCTION-ADOPTION.json) records the exact immutable specification revisions, body hashes, affected sections and exclusions before source changes. The [earlier contract](README.md) and [isolated feasibility](CHOICE-FEASIBILITY.md) remain sealed historical evidence.

The outer choice has minOccurs 0 or 1 and maxOccurs 1. It contains 1-16 direct scalar alternatives, each minOccurs 1 and maxOccurs 1-16. Explicit built-in, named and admitted anonymous simple types retain their existing facet and capacity rules. All alternatives compile eagerly, even when unused. Duplicate direct expanded names are Invalid in the recognized subset. Selected occurrences cannot mix alternative names or backtrack after a scalar failure.

Inline and named root types work through both existing compiler routes. Existing root attributes and structural namespace assertions retain their restrictions. Choice budget accounting takes the largest alternative rather than summing mutually exclusive branches; the 16-alternative by 16-occurrence boundary is covered. Original physical paths, declarations, scalar provenance, no-typed-value roots, export, detached reads and lifetime protections remain intact.

`xs:all`, repeated outer choices, nullable alternatives, nested choice containers, complex alternatives, groups, wildcards and implicit anyType remain excluded. The 23 all-phase payloads are refusal controls, not all validation coverage. These profile bounds do not state general XSD 1.1 limits. There is no new public API, Core pin or legacy decimal grammar. Broader xsi:type interactions and mid-execution cancellation timing are not newly accepted by this slice.

## Qualification

| Lane | Passed | Scope |
|---|---:|---|
| Source internal | 1,191 | Includes 65 added native identity/boundary controls |
| Source public consumers | 2,548 | Includes 54 added payload/facade controls |
| Package-only Core 1.81.2 | 2,548 | No source references or friend access |
| Package-only Core 1.81.17 | 2,548 | Existing loaded-version expectation adjustment only |
| Current-package native | 65 | Actual native identity, boundaries and original co-blockers |
| Retained binaries | 124 / 23 / 33 | Only Schema.dll replaced |

Source total is **3,739**, with all **3,620 previous test names and outcomes unchanged** and 119 additions. No original schema, XML payload, authored identity or expected outcome was reconciled. These overlapping lanes are not additive specification coverage. [Stable cases](implementation-evidence/STABLE-CASES.json) and [qualification results](implementation-evidence/RESULTS.json) preserve the exact comparison.

Public API remains **54 types / 401 rows**, zero delta. Eighteen signed Core archives retain their source pins, cache and runtime payload identities; signed published beta.1 and retained non-Schema files also verify. Final source, package consumers, native diagnostics and both corpus runtimes use the same Schema DLL. The exact packaged unsignedLong example still passes both facades, invalid bounds, signed zero, export and detached reads. Packaged README bytes match the current source and document choice exclusions. [Identities](implementation-evidence/IDENTITY-FINAL.json), [API](implementation-evidence/SURFACE.json), [review](implementation-evidence/REVIEW.json).

The local-only unsigned package is `Bosak.Schema.0.6.0-alpha.0.34.nupkg`.

- Package SHA-256: `22bfba95b0231a8c73a03e5a18a9a9c784a55407438636baf4770a9f6b532ecd`
- Schema DLL SHA-256: `9aae3db8edf6568ced815bf285b61c49af88f231f39939b1880af89455eadb18`

Initial external consumer projects omitted the new root-linked fixture; failed receipts and the corrected projects are preserved. No product correction was needed. An attempted diagnostic restore also encountered unreachable NuGet service/advisory endpoints and a cache missing MinVer. Final diagnostic builds used available cached dependencies with NuGet audit disabled only for those invocations; source restore likewise used cached dependencies. There is **no fresh vulnerability-data assessment**. No repository warning policy was weakened. The unusually long initial minimum-consumer receipt is not performance evidence. Early alpha.0.33 receipts remain separate; all final qualification uses alpha.0.34 after the source-header and packaged-documentation completion.

## Original corpus results

On both Core pins the frozen pilot stays **21 matches / 0 Unsupported / 0 errors**, with identical rows. All 42 frozen original files remain unchanged.

The expanded 129 cases stay **8 matches / 121 Unsupported / 0 mismatches / 0 harness errors**. All 274 original files, metadata and expected outcomes remain unchanged. Six first-refusal reasons change on each pin: three choice-capacity exclusions now report RepresentationCapacity, and three implicit-anyType examples now report ValueType, previously DeclarationShape. No validity classification or other row detail changes. [Frozen delta](implementation-evidence/PILOT-DELTA.json), [expanded delta](implementation-evidence/EXPANDED-DELTA.json).

Unchanged expanded measurement scripts retain their historical 22 November report date constant; these runs were executed on 23 November, as recorded in the wrapper receipts. This selected sample is not a complete XSD suite, PSVI acceptance or a coverage percentage.

## Delivery and next phase

The production candidate and documentation are uncommitted. Review and Git delivery require an explicit request under AGENTS.md section 8. After delivery and merge/main verification, assess `xs:all` as its own production slice. Published beta.1, registry state, tags, release controllers, Core sources and pins remain unchanged. No full XSD 1.1, full PSVI/C45 or performance acceptance is claimed.
