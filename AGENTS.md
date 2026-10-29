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

The public schema solution has 114 tests and no licensing-pipeline/Azurite dependency.

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

*Last updated: 2026-10-29*



## Apache and Public Repository Boundary

Schema services require no activation. Legacy License/ActivatedLicense types are compatibility shapes only. No private Git history, credential issuer/verifier, signing material, licensing/payment infrastructure, private deployment documents or commercial test vectors may be copied here. Preserve Fytala copyright/SPDX notices and record semantic adoption evidence before new standards behavior. Full XSD 1.1 support and core/session API acceptance remain pending.
