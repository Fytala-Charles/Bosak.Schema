# Isolated Core 1.81.2 adoption candidate - 9 November 2026

Parent source20090180755d0427d824b091cf838e9a03ca9adc, branchfeature/core-1.81.2-adoption. The local dependency checkpoint is the commit containing this entry; the user requested continuing with review and commit. [Candidate scope](CORE-1.81.2-ADOPTION.md) records five actual dependency edits (four product/one test),124 baseline/source/unchanged-binary controls and11 separate integration/ownership controls. Product/test C# remains unchanged; dependency output hashes match the published1.81.2 cohort. Original public checkout, its dirty review documents and the separate native assessment worktree remain unchanged. No push/tag/release or production native session adoption. Rebuild and qualify the local committed checkpoint, then obtain explicit direction before remote delivery; broader native/XSD coverage remains separate.

---

# Bosak.Schema handover — 29 October 2026

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](PUBLICATION.json). No NuGet release or commercial infrastructure publication.

This is the clean Apache-2.0 public schema repository. The original private repository/history and billing/key-generation code are preserved separately as Bosak.Licensing. Do not copy private history, credential helpers, deployment settings or commercial test vectors into this repository.

What was built: owner-approved schema-only license/publication split, Apache/SPDX/package metadata, activation-free factories, credential-free legacy API shims, public CI and a 114-case schema regression suite. Source base `fa0da3e2d9972706a5a86616ef285af4daf5037b`; public delivery commit is recorded in the publication receipt and Git history.

30 October 2026: REQ-009 S5c — open-content wildcards now record the XSD 1.1 negative constraints `notNamespace`/`notQName` (W3C XML Schema 1.1 Part 1 §3.10) with explicit `BSK-XSD-016` representation diagnostics; content-model and attribute wildcards remain explicit `BSK-XSD-003` (no component recorded for them yet). The suite grows to 124 tests, all passing.

Also 30 October 2026: the REQ-009 decision-evidence set (`ADR-005`, `ADR-006`, `ADR-001`, `ADR-002`, `XSD11_SOURCE_AUDIT`, `XSD11_GAP_MATRIX`, `XSD11_S4_DESIGN_PREP`, `SEAM_DESIGN`, `Bosak_Schema_XSD11_Agent_Handover_v0.1`) was transferred from the private Bosak.Licensing repository into `docs/` with dated historical-context notes (licensing-era framing is pre-Apache history). Every source/test reference to `docs/ADR-005` and `docs/XSD11_S4_DESIGN_PREP.md` now resolves; the `COMMERCIAL.md` pointer in `LicenseTier.cs` was reworded. Commercial ADRs (003/004), REQ-005/REQ-008, go-live and enterprise-fabric documents remain private.

Current development line: 0.4 through MinVer; no new tag/NuGet publication is implied. Core package pins remain 1.26.3. Full XSD 1.1 structural validation and the core session/PSVI integration are incomplete/pending. The licensing change does not accept the C44-R2 public API proposal.

Next: reassess the smallest necessary core/session contract around the open implementation, then qualify real component completeness, compatible-schema handoff and consumers before engine integration. See [architecture](ARCHITECTURE.md), [registry](FEATURE_REQUESTS.md), [integration](INTEGRATION.md) and [transition](APACHE-TRANSITION.md).

Last updated: 2026-11-09
