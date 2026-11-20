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

*Last updated: 2026-11-19*



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
