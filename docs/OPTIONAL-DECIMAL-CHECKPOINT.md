# Optional decimal scope delivery — 29 October 2026

This checkpoint synchronizes the [adoption record](OPTIONAL-DECIMAL-ADOPTION-2026-10-29.md), [implementation plan](OPTIONAL-DECIMAL-PLAN.md), current assessment guide, roadmap, architecture, integration, feature registry and handover. Optional-child support is **not implemented**. Current versions remain Schema 0.5 development and unpublished core 1.34.0; Schema 0.6/core 1.34.1 are proposed implementation targets only.

The plan covers optional local particles with maximum occurrence one, correct assertion effective Boolean conversion, present-only instance state and original declaration identity. Thirteen diagnostic observations include the empty element-only root whitespace binding gap. The implementation must correct that gap and run fresh full core tests and exact conformance gates.

Fresh scope baseline: 204 Schema tests pass. Full core 24,209-pass and QT3 31,397/0/424 / qt4 8,941/0/1,918 results are inherited from unchanged core source; no new full-engine run is claimed for this documentation delivery. The core `docs/schema-optional-children` dossier preserves original observations, raw baseline receipts and source identities. Earlier sealed bridge/assessment evidence remains unchanged.

## Delivery identity

Schema source base: `766eff7a77ee08e9c38cf479994e94eaf232d242`; core source base: `a163ff11895038ceb2b718ecfba77383384a3b35`. Schema delivery revision: `git log -1 --format=%H -- docs/OPTIONAL-DECIMAL-CHECKPOINT.md`. Remote target: `origin/feature/bounded-document-assessment` in the public Bosak.Schema repository.

The user explicitly requested documentation synchronization, commit and push. The paired core delivery is recorded in core `docs/schema-optional-children/delivery/CHECKPOINT.json`; core goes to the existing `checkpoint/restart-2026-10-29` remote branch for draft PR #467. Pushing is a backup/review delivery, not a merge, tag, package publication, general API freeze or product adoption. Original public main checkout and product consumer pins are unchanged.

Next: implement the bounded plan with before/after controls. Keep the pre-implementation adoption separate from later implementation and qualification evidence.
