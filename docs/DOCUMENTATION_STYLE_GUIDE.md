# Bosak.Schema Documentation Style Guide

> **How documents in this repository are written, styled, and reviewed.**
> Applies to all Markdown in `README.md`, `ROADMAP.md`, and `docs/`.
> Enforced by `tools/check-docs.ps1` and the CI documentation gate.

---

## 1. Document Scope

| Tier | Documents | Contract |
|------|-----------|----------|
| **Canonical** | `README.md`, `ROADMAP.md`, `docs/ARCHITECTURE.md`, `docs/INTEGRATION.md`, `docs/FEATURE_REQUESTS.md` | Full structure: standard header, numbered sequential sections, last-updated footer |
| **Working notes** | `docs/AGENT_HANDOVER.md`, `.kimi/HANDOVER.md`, `docs/ADR-*.md`, `docs/REQ-*.md` | Compact headings allowed; dated entries required |

---

## 2. Standard Header

Every canonical document starts with:

```markdown
# <Title>

> **One-line purpose of this document.**
> Optional second line: audience or cross-references.
```

- If the project has brand assets, a centered logo banner (from `assets/logos/`) may precede the title; alt text is mandatory.
- Asset paths are relative to the document: `assets/...` from the repository root, `../assets/...` from `docs/`.

---

## 3. Brand Tokens

- Palette and typography tokens live in `assets/css/` — the single source of truth.
- Documents reference tokens by **role**, never raw hex values: Primary, Secondary, Accent, Success, Warning, Danger, Neutral Light/Dark.
- One swatch image per token lives in `assets/images/brand-swatches/`.

---

## 4. Mermaid Diagrams

- Use the semantic `classDef` set consistently:
  - `current` — exists today
  - `platform` — shared / reused component
  - `external` — outside this repository
  - `planned` — proposed, not implemented
- Solid arrows = active behavior; dashed arrows = planned behavior.
- Every diagram gets a stable anchor (`<a id="..."></a>`) immediately before it and a caption immediately after it. Do not use "Figure N" numbering.
- Direction: `LR` for process/sequence flows, `BT` for tier/stack diagrams.
- Proposed design must be visibly marked (`planned` class + dashed arrows) — see `ARCHITECTURE.md` §Proposed — Not Implemented.

---

## 5. Renderer Integration

| Renderer | Wiring |
|----------|--------|
| VS Code preview | `.vscode/settings.json` → `markdown.styles` |
| Markdown Preview Enhanced | `.crossnote/` |
| Markdown PDF | `markdown-pdf.styles` |

Documents must degrade gracefully on GitHub: no renderer-critical inline CSS or HTML.

---

## 6. Accessibility & Portability

- Alt text on every image.
- Never convey status by color alone — pair with text or icons.
- Repository-relative paths only; no absolute machine paths.
- No layout-critical inline CSS.

---

## 7. Hygiene Enforcement

- `tools/check-docs.ps1` validates required files, required sections, stale placeholders, sequential section numbering, referenced-file existence, and date freshness.
- CI runs the same checks on every push and pull request (see `.github/workflows/build.yml`).
- New rules roll out **advisory first** (warnings), then become blocking after one release.

---

*Last updated: 2026-09-22*
