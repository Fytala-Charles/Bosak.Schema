<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Roadmap">
  <h1>Bosak.Schema Roadmap</h1>
  <p>An open schema foundation with staged validation and integration.</p>
</div>

**Publication verified — 29 October 2026:** Public GitHub repository and Apache-2.0 detection verified; all 114 tests and the content-commit CI pass. [Publication receipt](docs/PUBLICATION.json). No NuGet release or commercial infrastructure publication.

# Bosak.Schema Roadmap

> An open schema foundation with staged validation and integration.


## 1. Current Milestone

The public 0.4 baseline makes schema services Apache-2.0 and activation-free. The unpublished 0.5 candidate implements [native bounded document assessment](docs/DOCUMENT-ASSESSMENT.md) with original declaration/type state and explicit typed-value projection; complete XSD 1.1 and anonymous-root interchange remain incomplete. Public source contains the schema implementation/tests only; the original commercial infrastructure and full Git history remain private. No NuGet release/tag is included in this migration.

## 2. Next Milestones

1. Qualify the smallest anonymous-root/whole-document bridge from the real native assessment result, preserving original type/declaration state and no-typed-value/copy behavior.
2. Qualify complete component/universe identity, compatible-schema handoff, PSVI/atomic flow and bounded resource ownership.
3. Implement or explicitly refuse remaining XSD 1.1 structural/assessment coverage.
4. Build/test real consumers and unchanged binaries, then run the full engine/conformance gates before API freeze or release.

## 3. Validation and Limits

The public schema suite passes 114 cases. Existing core package pins stay at 1.26.3. Neither current-draft XDM certification nor full XSD 1.1 support follows from the Apache transition. Billing/key-generation, other product licensing, Folio, XmlDb storage and Braid workflows remain outside this source change.

*Last updated: 2026-10-29*

## Whole-document bridge candidate — 29 October 2026

The unpublished core 1.34.0 provider bridge now carries original anonymous-root and decimal-child state through whole-document exports and preserve/strip copies. Root atomization raises FOTY0012; stale content and legacy reassessment refuse explicitly. Use `-p:BosakSourceRoot=D:/Development/Bosak` for builds/tests/examples because candidate package dependencies are unpublished. See [the current guide](docs/DOCUMENT-ASSESSMENT.md). The earlier 191-case receipts describe the historical projection-only checkpoint; fresh bridge qualification is recorded separately in the core `docs/schema-document-bridge` dossier. Broader coverage, consumer adoption and API freeze remain pending. No commit/push/tag/publication.

## Reviewed whole-document bridge — 29 October 2026

The optional provider attachment API now checks a replacement completely before removing the previous binding. Failed replacement retains original state and stale-content refusal; failed initial attachment leaves the node unregistered. The trusted host must supply immutable state and the correct assessed value. This API carries that state without performing schema validation. The finite native profile is unchanged; optional children and general API freeze remain pending. Current core candidate is unpublished 1.34.0; use the BosakSourceRoot override. New review qualification lives in the core `docs/schema-document-bridge/api-review` dossier, separately from historical assessment receipts.
