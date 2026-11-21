# Bosak.Schema — Agent Guide

> **Canonical agent instructions** for the Bosak.Schema project.
> Deeper `AGENTS.md` files in subdirectories override these instructions for their scopes.

---

## 1. Project Identity

**Bosak.Schema** is a Apache-2.0 schema integration and staged XSD 1.1 services for the Bosak XPath 3.1 / XSLT engine.

- **Solution:** `Bosak.Schema.slnx`
- **Framework:** .NET 10
- **Target:** net10.0

---

## 2. Build & Test

```bash
# Full build
dotnet build Bosak.Schema.slnx

# Run all tests
dotnet test Bosak.Schema.slnx

# Run with verbosity
dotnet test Bosak.Schema.slnx --logger "console;verbosity=detailed"

# Full clean rebuild (clean → restore → build → test)
pwsh tools/rebuild.ps1

# Documentation hygiene check
pwsh tools/check-docs.ps1 -ProjectPath .
```

**Rule:** All tests must pass before a task is considered complete.

---

## 3. Code Conventions

### 3.1 File Headers

**Every `.cs` file** (excluding generated files in `obj/`, `bin/`) must begin with the following header:

```csharp
// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : <dd MMMM yyyy>
// PURPOSE              : <One-line description of the file's responsibility>
// SPECIAL NOTES        : <Implementation notes, assumptions, or layer-specific warnings>
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | <dd-MM-yyyy>   | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
```

**Rules:**
1. **PURPOSE** — Derive from the XML `<summary>` of the primary type. Keep under 100 characters.
2. **Change History** — When modifying an existing file, append a new row with the current date, a bumped version number, and a brief note.
3. **Generated files** — Skip `obj/`, `bin/`, and any file containing "Generated" or "Auto-generated" in the first 200 characters.

### 3.2 Naming & Language

- Use **English** for all identifiers, comments, and documentation.
- Use `PascalCase` for classes, records, methods, enums, and public members.
- Use `camelCase` for local variables and parameters.
- Use `PascalCase` for acronyms longer than two characters (e.g., `XmlDocument`, not `XMLDocument`).
- Prefer `sealed` classes unless inheritance is explicitly designed.
- Use `record` or `record struct` for immutable DTOs.
- Use `file`-scoped namespaces when possible.

### 3.3 Nullability

All projects must have `<Nullable>enable</Nullable>`. Always annotate nullability in public APIs.

### 3.4 Implicit Usings

All projects should have `<ImplicitUsings>enable</ImplicitUsings>`. Do not redundantly import `System` namespaces.

### 3.5 Build Enforcement

Conventions are enforced by the build, not just by review:

- Root `Directory.Build.props` — `TreatWarningsAsErrors`, `AnalysisLevel=latest`, `EnforceCodeStyleInBuild`, MinVer versioning, shared package metadata.
- Root `.editorconfig` — formatting and analyzer severities.
- Root `global.json` — .NET SDK pin.

---

## 4. XML Documentation

All projects must generate documentation files:

```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

**Rules:**
- Every `public` and `protected` type and member must have a triple-slash XML comment (`///`).
- Use `<summary>` for the primary description.
- Use `<param>` for every parameter.
- Use `<returns>` for methods with non-void return types.
- Use `<exception>` when a method throws documented exceptions.
- Use `<remarks>` for extended context or examples.

**Example:**

```csharp
/// <summary>
/// Parses the supplied EDI payload into a canonical evidence set.
/// </summary>
/// <param name="payload">Raw EDI text. Must not be null.</param>
/// <param name="options">Optional parsing flags.</param>
/// <returns>A <see cref="CanonicalEvidenceSet"/> containing extracted evidence.</returns>
/// <exception cref="ArgumentNullException"><paramref name="payload"/> is null.</exception>
public CanonicalEvidenceSet Parse(string payload, ParseOptions? options = null)
```

---

## 5. Testing Requirements

### 5.1 Test Organization

| Test Project | Purpose |
|--------------|---------|
| `Bosak.Schema.Tests` | Activation-free factories, schema compilation/validation and legacy API compatibility |

The published beta source solution has 345 cases: 312 implementation and 33 public-consumer cases without friend access. Published-package and retained-binary lanes repeat scoped catalogs and are not additive coverage. Core qualification uses 1.81.2; the bounded ADR009/010 contract is accepted. See docs/BETA1_RELEASE.md for exact publication identities and exclusions.

### 5.2 Test Patterns

- Use the same test framework across all projects (MSTest, xUnit, or NUnit — pick one and stay consistent).
- Test classes are marked with the appropriate attribute (`[TestClass]`, `[TestCollection]`, etc.).
- Test methods describe the behavior under test: `MethodName_Scenario_ExpectedResult`.
- Use `NullLogger<T>.Instance` when testing services that require `ILogger<T>`.
- Clean up temporary files after tests.
- Add golden-file tests for serialization, XML, or document output.
- Place shared sample/golden data in `tests/TestData/`; suite-specific data in `tests/<Suite>/` (e.g., `Schemas/`).

### 5.3 Coverage Rule

New functionality must have **at least one unit test** covering the happy path and **one test** covering a failure mode or edge case.

---

## 6. Versioning

- Use [Semantic Versioning](https://semver.org/) (`Major.Minor.Patch`).
- Versions are produced **from git tags** (`v1.2.3`) by MinVer, configured in root `Directory.Build.props`. Never hard-code `<Version>` in project files.
- Bump `Minor` for new features; bump `Patch` for bug fixes (tag accordingly).
- Record version bumps in:
  - File header change-history tables
  - `docs/FEATURE_REQUESTS.md` target versions
  - `docs/ARCHITECTURE.md` status line
  - `docs/INTEGRATION.md` version history
  - `ROADMAP.md` milestones

### 6.1 Architecture Decision Records

- **Minor decisions** go in the `docs/ARCHITECTURE.md` *Key Design Decisions* table.
- **Cross-cutting or expensive-to-reverse decisions** get an ADR: copy `docs/ADR-000-TEMPLATE.md` to `docs/ADR-NNN-<slug>.md` (NNN = next zero-padded number, never reused).
- Link ADRs from the REQ detail sections that motivated them.

---

## 7. Documentation Sync Checklist (Mandatory)

After **every** successful implementation step, update the following canonical documents before concluding the step or session:

| File | Purpose | When to Update |
|------|---------|----------------|
| `README.md` | Human-facing project overview | Structural, CLI, or dependency changes |
| `ROADMAP.md` | Release stages, milestones, known limitations | Stage or milestone change |
| `docs/ARCHITECTURE.md` | Package tiers, dependency rules, use cases, canonical models | New projects, tier changes, new use cases, API additions |
| `docs/FEATURE_REQUESTS.md` | Living registry of all REQ items | Any REQ status change, new REQ, or completed item |
| `docs/INTEGRATION.md` | Consumer integration guide, APIs, extension points | New public APIs, CLI commands, behavioral changes |
| `docs/DOCUMENTATION_STYLE_GUIDE.md` | Documentation & diagram rules | Any documentation-style change |
| `docs/ADR-*.md` | Architecture decision records | Any significant decision (one new file per decision) |
| `docs/AGENT_HANDOVER.md` | Session state and canonical agent context | Major architectural shifts, current session scratchpad |

**Rule:** If a file was modified during the step, its documentation counterpart must be updated in the same step. No exceptions.

### Pre-Handover Checklist

```markdown
- [ ] `README.md` — Repository tree, build/test commands, and quick-start are current.
- [ ] `ROADMAP.md` — Stage and milestone status reflect reality.
- [ ] `docs/ARCHITECTURE.md` — Status line reflects reality; use-case catalog and project matrix are current.
- [ ] `docs/FEATURE_REQUESTS.md` — Registry table and detail sections are accurate; "Last updated" date is today.
- [ ] `docs/INTEGRATION.md` — Feature status matrix is accurate; version history appended for behavioral changes.
- [ ] `docs/AGENT_HANDOVER.md` — Date and commit hash match latest; "What Was Built" includes every feature/bugfix.
- [ ] `AGENTS.md` — Coding conventions still match reality.
```

---

## 8. Git Rules

- Never run `git commit`, `git push`, `git reset`, `git rebase` unless the user explicitly asks.
- Never modify files outside the working directory unless explicitly instructed.
- Keep commits focused; one logical change per commit.

---

## 9. Related Documents

| Document | Purpose |
|----------|---------|
| `README.md` | Human-readable quick start |
| `ROADMAP.md` | Release stages, milestones, known limitations |
| `docs/ARCHITECTURE.md` | Package tiers, dependency rules, use cases |
| `docs/FEATURE_REQUESTS.md` | Living registry of cross-cutting feature requests |
| `docs/INTEGRATION.md` | Consumer integration guide |
| `docs/DOCUMENTATION_STYLE_GUIDE.md` | Documentation & diagram rules |
| `docs/AGENT_HANDOVER.md` | Session state for AI agents |

---

*Last updated: 2026-11-21*



## Apache and Public Repository Boundary

Schema services require no activation. Legacy License/ActivatedLicense types are compatibility shapes only. No private Git history, credential issuer/verifier, signing material, licensing/payment infrastructure, private deployment documents or commercial test vectors may be copied here. Preserve Fytala copyright/SPDX notices and record semantic adoption evidence before new standards behavior. Full XSD 1.1 support and core/session API acceptance remain pending.

## Historical dependency and documentation checkpoint

Product and test Core references select published 1.81.2 after Schema PR #1. The public-entry candidate advances Schema's MinVer development minimum to 0.5; no new Schema package release is implied. Standalone124-case regression coverage and the separate Core 135-case optional integration have distinct scopes. Use `docs/AGENT_HANDOVER.md` for current state and `docs/DEVELOPMENT_HISTORY.md` for archived local review notices; older inputs keep their original source hashes and do not replace current package pins. Full XSD 1.1 structural/native session/lease coverage and performance acceptance remain separate tasks.


## Release automation controls

Workflow-only release controls are separate from the current 345 product/source cases (published alpha: 272). Run `python -m unittest discover -s .github/scripts -p test_schema_release.py` and `node --test .github/scripts/test-schema-source-run.cjs`. They use synthetic artifacts and mocked GitHub/network responses; they do not log in or push. `.github/workflows/release.yml` admits only manually selected main preparation/publication for the recorded first prerelease, with OIDC permission confined to the publish job. Neither passing controls nor a prepared artifact authorizes registry publication. Keep the product source/Core pins and earlier immutable receipts unchanged.


## Restricted resolution candidate

The local restricted resolver/XSLT factory requires stable host-owned filesystem paths, exact captured root spelling and explicit byte limits. It is not an atomic filesystem sandbox. Use docs/RESTRICTED_RESOLUTION.md; qualify nested acquisition through published Core policy, not a fallback-capable convenience callback. All 31 restricted implementation cases and three added public consumer cases are included in the current suite (306 cases at the restricted-resolution checkpoint). Preserve the published alpha/audit evidence as historical inputs; beta acceptance and publication remain separate.


## Compiler snapshot ownership

Compiler-produced descriptors own immutable outer and nested collections; preserve shared assertion identity across root/facet lists. Public record constructors/with copies still accept application-owned data. Use docs/DESCRIPTOR_SNAPSHOTS.md and do not extend compiler guarantees to arbitrary ICompiledSchema implementations. Snapshot checkpoint319; historical verifiers must run against their recorded source revisions.


## Legacy validation compile findings

All three legacy validation entry points validate null arguments, snapshot compile findings and return false before other model/typed/assertion work. Preserve original findings and scoped clean success; do not reinterpret IsValid as structural certification. See docs/LEGACY_VALIDATION.md. Current suite345; historical API/dossier records retain their original source scope.


## Workload qualification

Read docs/BETA_QUALIFICATION.md before measuring workloads. Build tools/Bosak.Schema.Workloads separately; run six Python controls with `python -m unittest discover -s tools -p test_schema_workloads.py`. Use tools/run-schema-workloads.py with a new ignored .workload-results output directory. No unhandled negative diagnostic launches or machine-wide WER changes. Proposed local timing/memory envelopes are not acceptance or SLAs. Product345/release45/runner6 scopes remain distinct.


## Beta contract review boundary

Use docs/BETA_CONTRACT_REVIEW.md for the current concrete ADR009/010 proposal. The owner accepted ADR009/010 for the bounded Schema beta scope on17 November2026; broader acceptance is not inferred. Local alpha compatibility packages are not beta artifacts. General Core C45/ADR006, production SLAs, exact beta tag/controller/artifact and publication remain separate; do not infer their acceptance from package/binary passes.


## Beta.1 release candidate — historical 17 November checkpoint

Read docs/BETA1_PREPARATION.md before release operations. Exact source2a5abf6fb663313dc32bfeca5bbb8740bbe987ae/tagv0.5.0-beta.1; controller requires345/33 and retained124/23 lanes, nine signed Core1.81.2 payloads.32 Python +22 GitHub provenance controls, six runner controls. Local manifest zero controller is never hosted evidence. No remote tag or beta publication yet; do not infer publication authority from preparation. Historical dossiers remain unchanged.


## Current published beta

Bosak.Schema0.5.0-beta.1 is published on NuGet.org and GitHub Releases. Read docs/BETA1_RELEASE.md and docs/beta1-publication/RECEIPT.json for current status. Earlier guides/dossiers retain their original checkpoint claims and hashes; run historical verifiers at their recorded revisions. The release controller remains pinned to this already-published version and must not be dispatched for a duplicate push. Future versions require fresh review and explicit approval. GitHub Packages is separate and unused.

## Unsupported-reason development candidate

19November2026: the local0.6candidate has365source cases(312implementation/53public), including20new detected-reason controls. Published beta.1 remains345. Preserve stage codes/messages, refusal order and grammar; use docs/unsupported-reasons/CONTRACT.md. Package-only53 and retained124/23 lanes overlap source coverage. No commit/push/publication without explicit request.

## M1 namespace development candidate

19November2026: local409cases(315implementation/94public),41authored namespace cases+threeactualCore controls. Read docs/m1-namespaces/ADOPTION.md before semantic edits. Preserve expanded-name identity, assertion namespace capture, zero-to-eight assertions, limits and explicit unsupported boundaries.94package-only/42current-Core and124/23/33retained lanes overlap. Core runtime/pins and published beta.1 remain unchanged.

## M2A named complex type candidate

19November2026:465source cases(320implementation/145public). Read docs/m2a-named-types/ADOPTION.md before semantic edits. Preserve16global type/root ceilings, eager unused-definition refusal, QName/default namespace resolution, shared type identity and foreign/disposed authority guards.145minimum-Core/52current-Core/124-23-33retained lanes overlap. Core pins, published beta.1 and release controller remain unchanged.

## M2B decimal restriction candidate

19November2026:534source cases(324implementation/210public). Read docs/m2b-decimal-restrictions/ADOPTION.md before semantic edits. Preserve exact value-space facets, representation-before-facet gates, combined16type limit, eager unused-definition refusal and named declared/actual provenance. No general derivation relation or new datatype/attribute/import support; no commit/push/publication without explicit request.

## M2C1 string/decimal candidate

19November2026:614source cases(334implementation/280public). Read docs/m2c-string-facade/ADOPTION.md before semantic edits. New NativeSchemaSession opt-in shares the native owner/gate; legacy NativeDecimalSession stays decimal-only. Preserve XML string text, explicit UTF16/output capacities, original typed facts and bounded export/cancellation. No Boolean/integer/named string facets or Core engine/pin/publication expansion. API/C45 freeze remains separate; no commit/push without explicit request.

## M2C2 Boolean candidate

19November2026:671source cases(340implementation/331public). Read docs/m2c-boolean/ADOPTION.md before semantic edits. Opt-in Boolean leaves use true/false/1/0 after XML-whitespace collapse with an independent256UTF16lexical ceiling. Preserve false-vs-absence, actual Boolean facts/provenance and legacy decimal refusal. Boolean assertions/restrictions and integer support remain excluded. No commit/push/publication without explicit request.

## M2C3 integer candidate

19November2026:748source cases(347implementation/401public). Read docs/m2c-integer/ADOPTION.md before semantic edits. Exact built-in integer leaves retain original typed facts, BigInteger values, supplied-digit capacity and fixed256UTF16lexical ceiling. Preserve legacy decimal refusal and separate read/query capacities. Integer assertions/derived restrictions remain excluded. No commit/push/publication without explicit request.

## M2D1 string restriction candidate

19November2026:837source cases(355implementation/482public). Read docs/m2d-string-restrictions/ADOPTION.md before semantic edits. Only opt-in direct named xs:string restrictions admit Unicode scalar length/minLength/maxLength and exact ordinal enumeration. Preserve parsed UTF16representation limits,16supplied enumeration ceiling, original named facts, legacy decimal refusal and decimal-only assertions. Pattern/whiteSpace/anonymous types/restriction chains remain excluded. No commit/push/publication without explicit request.

## M2E1 attribute contract planning

19November2026: current837source suite is unchanged after PR23 merge098fdaf. Read docs/m2e-attributes/CONTRACT.md and proposed ADR012 before attribute adoption/implementation.27synthetic public Core probes per minimum/current lane qualify attribute facts/transport only, not native XSD validation. Proposed root-only separate attribute reads preserve leaf indexing and old decimal behavior; no new API/adoption or release acceptance yet. No commit/push/publication without explicit request.

## M2E1 root attribute candidate

19November2026:950source cases(372implementation/578public), all837prior outcomes unchanged. Read docs/m2e-root-attributes/ADOPTION.md before semantic edits. Root-only opt-in attributes have separate counts/indexes/original facts and detached typed reads. Preserve declaration-order optional omission, form/type/normalization/capacity rules, no partial facts, legacy decimal refusal, same gate/lifetime and old no-attribute/leaf-attribute behavior. Sealed planning dossier remains historical. No commit/push/publication without explicit request.

## Decimal simple-content candidate

20November2026:1036source cases(386implementation/650public), all950prior outcomes unchanged. Read docs/decimal-simple-content/ADOPTION.md before edits. Only direct decimal simpleContent extension with current root attributes is admitted. HasRootScalar/ReadRootScalar never change leaf/attribute indexes; root complex annotation/declared type differs from actual decimal atomic type. Preserve normalized root string-value versus lexical export, old rootFOTY0012, owner/cache/gate and legacy decimal refusal. No commit/push/publication without explicit request.


## Nested-sequence planning

20 November 2026: PR25/main c34e9fe is verified at 1,036 source cases. Read docs/nested-sequences/PLAN.md and CONTRACT.md before nested work. These are proposals, not semantic adoption or implemented support. Preserve full element facts and separate terminal scalar indexing; no commit/push without explicit request.


## Nested-sequence pre-implementation adoption

20 November 2026: read docs/nested-sequences/ADOPTION.md before production edits. Separate public Core feasibility is 50 synthetic controls per minimum/current lane, not native validation. 1,036 source cases remain unchanged; the64node budget is defensive under depth4/16terminal bounds. Preserve old receipts and explicit commit/push rule.


## Nested-sequence implementation candidate

20 November 2026: local1,125source cases(395implementation/730public), prior1,036unchanged. Read docs/nested-sequences/ADOPTION.md before semantic edits. Separate complete element facts from terminal scalar projection; preserve local/global declaration identity, eager cycle/expansion/refusal gates, legacy facade and lifetime rules. Package/retained qualification is separate. No commit/push without explicit request.


## Nested container attribute planning

20 November 2026: read docs/nested-attributes/PLAN.md, CONTRACT.md and proposedADR014 before descendant-attribute work. Base3e067cd/main1,125cases remains unchanged. Three-member/path proposal is not implemented or semantically adopted. Keep root/terminal indexes and original owner/use facts; explicit commit/push rule still applies.


## Nested container attribute pre-implementation adoption

20 November 2026: read docs/nested-attributes/ADOPTION.md before implementation.69synthetic public Core controls per signed lane and four native/API baseline controls pass, not new native support. Bind facts to exact original attribute owner/node, preserve root/scalar indexing and64total potential-use budget. Production/API remains unchanged; no commit/push without explicit request.


## Nested attribute implementation candidate

20 November 2026: local1,200source cases(404implementation/796public);1,123prior unchanged and NS-049/075intentional admission recorded before edits. Read docs/nested-attributes/ADOPTION.md before semantic work. Preserve exact captured owner/fact identity, separate root/descendant/scalar indexes, detached immutable paths and64total potentialuses includingroot/optional/repetition. Three new API members, no new type. No commit/push without explicit request.


## Nested decimal simple-content planning

20 November2026: read docs/nested-simple-content/PLAN.md and CONTRACT.md before this next slice. Base32f8214/main1,200cases verified. Proposed zeroAPI delta, scalar-bearing complex terminals and depth4three-segment owner paths are not adopted or implemented yet. Preserve prior receipts and explicit commit/push rule.


## Nested decimal simple-content implementation

20 November2026: read docs/nested-simple-content/ADOPTION.md before semantic edits. Local1,255cases(415implementation/840public),1,198prior unchanged/two adopted admissions/55new. Preserve complex declared versus actual decimal provenance, root/local declaration identity, original captured owner paths up to3segments at depth4, scalar/root index separation and old budgets. No new public API or Corepin. No commit/push without explicit request.

## Frozen upstream conformance pilot

20 November 2026: read docs/conformance-pilot/README.md and MEASUREMENT-CONTRACT.md
before pilot work. Preserve the 21 cases, original 42 Git-blob files, notices,
expected validity and frozen manifest bytes. Unsupported is never a match; all
21 baseline cases are Unsupported. Do not rewrite fixtures or relax xsi:type
handling to obtain passes. New semantics require specification adoption first.
Driver is manual and nonpackable; no new gate or release. Explicit commit/push rule applies.

## Explicit-schema hint policy

20 November 2026: read docs/schema-location-hints/ADOPTION.md before semantic edits.
Only opt-in NativeSchemaSession admits exact xsi location hints without fetching;
preserve original XML, typed application indexes, legacy refusal and xsi:type/nil
refusal. 1,302 source tests pass, including all 1,255 prior outcomes and 47 new cases.
Frozen upstream baseline stays sealed; candidate separately yields 1 match / 20
Unsupported / zero mismatches/errors. Candidate harness requires exact source and
runtime evidence. No full-suite claim, API/pin/publication change or implicit delivery.

## Anonymous local restriction adoption

20 November 2026: read docs/anonymous-simple-restrictions/ADOPTION.md before
production edits. Base merged PR29/main1438f71 has1,302passing source tests.
Adopt only opt-in local decimal/string direct restrictions with original anonymous
identity; require public Core feasibility before implementation. NS-051 original
row and six upstream targets are pinned, not executed admissions. Combined16local
anonymous definition budget, legacy refusals and immutable corpus remain protected.
No implementation/API/pin/publication change; commit/push needs explicit request.

## Anonymous simple-type transport feasibility

20 November 2026: FEASIBILITY.md records90synthetic transport controls per signed
Core lane(13new/77inherited),18signature/payload checks and six actual unchanged
Schema baseline controls. Transport succeeds, but native anonymous restrictions
remain Unsupported. Read original ADOPTION.md before implementation, preserve
NS-051 receipt and sealed corpus. No production/API/Corepin/publication change;
native/package/retained qualification remains pending. No commit/push requested.

## Anonymous local restriction implementation

20 November2026: read original ADOPTION.md and implementation/README.md.
1,373source cases pass(421/952); NS-051 is the one adopted prior admission.
Preserve original anonymous type identities, eager combined16local definition
budget and shared facets. Package-only952minimum/952current and retained124/23/33
pass. Frozenpilot7matches/14Unsupported/0errors; no fullconformance claim.
Native advisor stays scalar-only; legacy data(.) on absent QName is unqualified.
No public API/Corepin/publication change; commit/push requires explicit request.

## Post-PR30 specification planning

20 November2026: merged base5bfbb1e has the exact reviewed PR30 tree. Read
 docs/specification-next/README.md and INTEGER-CONTRACT-PROPOSAL.md before next
work. Integer restriction scope is proposed, not adopted/implemented. Preserve
frozen21case/42file corpus, sevenmatches/fourteenUnsupported checkpoint, scalar
query boundary and historical receipts. New semantics need exact adoption first.
No source/API/pin/publication change; commit/push requires explicit request.

## Integer restriction implementation

20 November2026: read docs/integer-restrictions/ADOPTION.md and route clarification.
Exact named/anonymous integer facets and named attribute uses are qualified;
preserve fixedfractionDigits0, BigInteger, original identities and old limits.
1,482source(427/1,055), package1,055/1,055,retained124/23/33andAPI52/392pass.
Two prior opt-in admissions only; legacyM2B-54 stays refused. Frozenpilot9matches/
12Unsupported/0errors is selected validity evidence, not fullXSD. Nativeadvisor
stays scalar-only; no Corepin/API/publication change. No commit/push requested.

## Original-node atomization contract proposal

20 November2026: merged PR31/main43f14db is verified at1,482source cases. Read
 docs/typed-node-contract and proposed ADR016 before node integration. Released
Core has an exact bounded original-node-data profile; do not promise arbitrary
XPath or relax existing scalar Bind. Proposed separate owned data session must
assess in its own environment and resolve exact original targets; no rebind of
existing scalar documents. Feasibility/adoption/API review remain pending; no
source/API/Corepin/publication change. Commit/push requires explicit request.

## Actual native original-node data feasibility

20 November2026: FEASIBILITY.md records73native controls per signed Core lane,
eight baseline controls and1,482unchanged candidate regression outcomes. Two
isolated source copies change: provider data-mode/context binding and guarded
ISchemaAnnotatedNode facts. Real src/ stays unchanged. Preserve iteration failures;
Core FOTY0012 is InvalidOperationException, proposed facade mapping still pending.
Before production edits record semantic adoption and resolve API/lifetime gates.
No public API/Corepin/package/publication or new pilot change; no commit/push request.


## Original-node scalar data (0.6 development candidate)

`NativeSchemaNodeDataSession` is a separate owned session for fixed original-node
atomization. Create it with existing explicit `NativeSchemaSessionOptions`, then
Assess with `NativeSchemaAssessmentContext`. A valid assessment returns an owned
`NativeSchemaNodeDataDocument`. `ReadElement(ImmutableArray<int>)` selects original
element-child indexes (empty selects root, maximum three segments).
`ReadAttribute(ownerElementPath, namespaceUri, localName)` selects a present original
attribute by expanded name. Both return one detached `NativeScalarValue`.
`ExportXml` returns bounded parsed lexical XML. Dispose documents and session.

The existing bounded schema grammar and native scalar families apply. Element-only
containers refuse reads with NotSupportedException carrying FOTY0012. Default paths,
invalid names and missing targets throw documented argument exceptions; query/cache
capacity and unassessed hint attributes are Unsupported read operations, not Invalid
instance assessments. Operations are sequential; overlap is refused and disposal
waits for admitted synchronous work. No expression, raw node, provider authority,
callback, general XPath, list/nil support or transferable PSVI is exposed.

This additive API is implemented locally for review. ADR-016 owner API acceptance,
general Core C45 and full XSD 1.1 conformance remain pending. Published 0.5.0-beta.1
and the existing decimal/scalar facades are unchanged. See [implementation adoption](docs/native-node-data/ADOPTION.md).


## Owned node-data qualification ? 20 November 2026

Local candidate on base `43f14dbf56c36e5185e705af901678c42c8d1aff` passes
1,615 source cases (503 implementation / 1,112 public), preserving all 1,482 prior
named outcomes. Package-only minimum Core 1.81.2 passes 1,112; Core 1.81.17 passes
1,111 shared cases plus its explicit current-version check. The deliberately
minimum-version-only surface test is excluded only from that current lane.
Retained 124/23/33 unchanged callers pass with only Schema.dll replaced. Counts
overlap. All 392 prior public surface rows are preserved; two sealed owned types
add nine rows (54 types / 401 rows total). Frozen pilot remains 9 matches / 12
Unsupported / zero mismatch or harness error. No new XSD expectations or grammar.

The local unsigned package is `0.6.0-alpha.0.19`; published beta.1, Core source/pins,
release controllers and tags are unchanged. The earlier sealed proposal and
feasibility packets are unchanged. Owner API acceptance/ADR-016 and delivery
remain pending. No commit or push has been performed for this candidate.


## Owned node-data contract review ? 20 November 2026

The local code/API self-review found no blocking implementation issue. The current
review addendum supersedes the sealed original ADR's historical pending-implementation
wording; ADR-016 owner acceptance and explicit commit/push/PR authorization are
still pending. Qualification and sealed packets are unchanged. [Review and prepared delivery](docs/native-node-data-review/README.md).


## Bounded ADR-016/API accepted ? 20 November 2026

The owner accepted the reviewed two-type/seven-method owned original-node data
contract and authorized commit, push and PR delivery. This dated acceptance
supersedes historical pending-owner-acceptance wording in the sealed ADR/proposal,
feasibility, implementation and review packets; those originals remain unchanged.
All qualified source/binary identities remain unchanged. General C45/ADR006,
full-XSD/general XPath and release/publication decisions remain separate.
[Owner acceptance](<docs/native-node-data-acceptance/README.md>).


## Bounded string assertions (0.6 development)

The opt-in scalar and owned node-data sessions admit string equality assertions
on declared immediate children of flat complex content. Primitive and existing
named/anonymous string restrictions use exact parsed string values. Quoted XPath
literals support doubled quote escaping. `eq`/`ne` require at most one value per
operand; multiple values make assessment Invalid. `=`/`!=` test whether any pair
matches the comparison; an empty operand gives false. Equality uses codepoint
semantics, without trimming, case folding or Unicode normalization.

Existing exists/empty/count checks now accept string children, including present
empty strings. They compose with existing decimal comparisons and and/or under
the original namespace and capacity rules. Attribute expressions, nested
assertions, string ordering, Boolean/integer operands, general XPath and
configurable collation remain outside this slice. The decimal-only facade keeps
its previous grammar. False assertions publish no document or native facts.

See [bounded string assertion evidence](docs/string-assertions/README.md) for adoption, seven explicit authored admissions and qualification.
The frozen W3C fixtures/expectations are unchanged. No public API, Core source/pin,
release-controller, tag or published-package change is included.


### String assertion qualification ? 20 November 2026

The local string-assertion candidate passes 1,886 source cases (520 implementation /
1,366 public), including 271 added controls and seven explicitly recorded authored
admissions. Both package-only Core lanes pass 1,366; retained 124/23/33 callers
pass, with no public API delta (54 types / 401 rows). Nine pure-string controls
agree with published minimum Core. The unchanged frozen pilot is now 10 matches /
11 Unsupported / zero mismatch or harness errors. Counts overlap. Core pins and
published beta.1 stay unchanged; alpha.0.20 is local unsigned qualification only.
No commit/push has been performed. See [string assertion evidence](docs/string-assertions/README.md).


### String assertion delivery review ? 20 November 2026

Local code/contract review found no blocking implementation issue. The six sealed
packets, qualified source/binary identities, exact seven authored admissions and
unchanged frozen corpus verify. Other 180 rows in the three amended fixture files
are unchanged. PR title/body are prepared; explicit commit/push/PR authorization
is pending. No implementation or qualification result changed in this review.
[Review and prepared delivery](docs/string-assertions-review/README.md).


### String assertion delivery authorized ? 20 November 2026

The owner approved commit, push and open PR for the reviewed bounded slice.
The qualified implementation and sealed evidence are unchanged. Earlier pending
statements are historical; merge and publication remain separate.
[Delivery authorization](docs/string-assertions-delivery/README.md).


### PR33 lifecycle-test correction ? 20 November 2026

The initial hosted run failed two pre-existing lifecycle entry waits under
concurrent blocking-worker load. Dedicated test-only workers now run the same
barrier protocol across eight lifecycle classes; assertions/timeouts are unchanged.
All25focused lifecycle and1,886full source cases pass with unchanged named outcomes.
Product source/API and prior package/pilot/signature evidence are unchanged.
[Original failure and correction](<docs/string-assertions-ci/README.md>).


## Next bounded xsi:type contract ? 20 November 2026

PR33 is merged at b1759d22761587b2abe507d12aacf3f5b6b33535; main CI44819529110
passed 1,886 source cases and supporting controls. The next slice is a proposed
instance-type contract, not an implementation or new admission. It covers exact
type identity and current same-family direct restrictions, with explicit QName
resolution, component derivation and separate original declaration/declared type /
effective governing type. Cross-family and new complex projections are deferred.

Static review records all 51 built-in names to distinguish absent names from known
unsupported types, both pinned Core contracts, four original negative pilot cases,
33 well-formed proposed payloads and 27 scenario gates. These are planned controls,
not executed product tests or new pilot matches. Current frozen results stay
10 matches / 11 Unsupported/zero mismatches or harness errors. Next gate is actual
native feasibility on both Core pins, then separate adoption before source edits.
[Contract and qualification plan](<docs/xsi-type-contract/README.md>).


## Actual native xsi:type feasibility ? 21 November 2026

An isolated copied-source candidate passes 169 controls on each pinned Core version
(96 type-selection controls plus 73 retained original-node controls). It preserves
original declaration D/declared T and supplies effective governing G to guarded
facts, annotations and atomic evidence; selected facets are enforced and failed
requests publish no facts. Fixed node-data operations still refuse ReadTypedValue;
separate native typed-identity sessions prove that route without rebinding origins.

The unmodified source comparison has 1,882 passes and four expected xsi:type=true
Unsupported-to-Invalid differences. With exactly four scratch-only expectation
changes, all 1,886 stable source cases pass. Production tests/fixtures are unchanged.
All 18 Core signatures/source/cache/runtime identities verify; public API remains
54 types / 401 rows. These lanes overlap and are not a new product test count.

Production src/tests/tools/configuration and the sealed original proposal remain
unchanged. No new pilot/package/publication/commit/push. Original pilot remains
10 matches / 11 Unsupported / zero mismatches or harness errors. Next: record
production semantic adoption, implement the measured mapping, then full consumer,
retained-binary and frozen-pilot qualification. ADR017 review remains separate.
[Native feasibility and preserved iterations](<docs/xsi-type-contract/FEASIBILITY.md>).


## Bounded xsi:type production candidate - 21 November 2026

The two mixed owned facades now admit same-component requests and existing direct
same-family restrictions using instance-scoped QNames and an immutable compiled
base graph. Original D/T remain distinct from effective G; selected facets,
annotations and original atomic provenance agree. Failed requests publish no facts.
Legacy decimal-only refusal and the54-type/401-row public API remain unchanged.

Qualification:2,024source passes(550implementation/1,474public),1,474package-only
passes per Core1.81.2/1.81.17,30overlapping current-package native controls and
retained124/23/33passes. Four authored absent-type expectations are explicitly
reconciled. The unchanged frozen pilot now measures14matches/7Unsupported/zero
mismatches or harness errors; positive selected restrictions are tested separately.
Signed dependencies and published beta.1 baseline verify. These lanes overlap.

Production adoption is recorded; original proposal/feasibility dossiers remain
historical. ADR017 owner review and Git delivery remain pending. Published beta.1,
Core pins and release controller are unchanged. No commit/push/publication.
[Implementation qualification](<docs/xsi-type-contract/implementation/README.md>).


## PR34 merged and next specification gaps - 21 November 2026

PR34 is merged as ee32ac81c7a704c95647b121d1cde23cfa32dcbd; its tree equals the
reviewed9d48071 tree. Main CI44871576728 passes2,024source tests(550/1,474),
release controls, docs and workload integrity. Existing bounded xsi:type scope,
API and publication boundaries remain unchanged. No new package/pilot measurement.

The seven frozen Unsupported cases span four schemas. The next proposed slice
is schema namespace/form declarations: review relative namespace-name support
and admit legal attributeFormDefault without requiring application attributes.
One such gate currently masks the assert006 family's independent integer content,
complex restriction and inherited assertion gaps. Decimal complex simple-content
restriction follows; context-item/nested assertions remain separate. Analyse
original inputs and record exact adoption before implementation; no forecast pass
count or full-conformance percentage. This step changes documentation only.
[Merge receipt, ranked gaps and next contract](<docs/post-xsi-type/README.md>).


## Namespace/form development candidate - 21 November 2026

The mixed owned facades now accept bounded relative target namespace references
as exact strings, with no URI resolution/normalization/acquisition. Legal schema
attributeFormDefault no longer requires application attribute uses. Explicit
assertion-default URI literals and legacy decimal-only limits/diagnostics remain
unchanged. Actual adoption follows31native controls per signed Core pin.

Final qualification:2,143source passes(581implementation/1,562public),1,562public
package-only passes per Core1.81.2/1.81.17,31overlapping current-package native
controls and retained124/23/33. Two authored expectation admissions are recorded;
the prior2,024stable source IDs remain. API54types/401rows has zero delta, and
18Core signatures/source/runtime identities plus signed beta.1 verify. Frozen
pilot16matches/5Unsupported/zero mismatch/harness errors; assert006 remains
Unsupported at its next assertion gate. All lanes overlap; no full-XSD percentage.

One production source file changes. Local unsigned alpha.0.25 is qualification
only; no commit/push, Core pin, tag/controller or publication change. Next after
review/delivery: bounded decimal complex simple-content restriction. Original
post-xsi-type proposal remains historical; corrected schema representation section
is Structures3.17.2. [Contract, adoption and receipts](<docs/namespace-form/README.md>).
