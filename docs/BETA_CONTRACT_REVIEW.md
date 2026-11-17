# Bounded beta contract review

17 November 2026. Review base6d4e1e2bae2ee7889791e9a9dd22a2034cf3ab05, PR11 merged/main44158885345 green. **Bounded ADR/API decisions accepted by the owner on17 November2026; no beta published.** Owner scope selection is already recorded in [qualification](BETA_QUALIFICATION.md). [Evidence](beta-contract/README.md), [revised ADR009](ADR-009-native-decimal-session-profile.md), [revised ADR010](ADR-010-owned-native-decimal-facade.md).

## Accepted bounded decisions

| Item | Accepted bounded decision | Explicit exclusion |
|---|---|---|
| Profile | Owned synchronous finite decimal-session beta, explicit capacities/context and complete supported-grammar assessment | General XSD1.1, native namespaces/attributes/broader datatypes, arbitrary XPath, nil/list/union |
| Ownership | Sequential gate, overlapping work refuses, disposal waits; owned values cleaned once and detached outputs survive | Raw callbacks/handles, async native APIs, arbitrary scheduler/fairness/latency or physical-release guarantee after cleanup failure |
| Results | Native Valid/Invalid/Unsupported distinct from documented exceptions; four fixed stage diagnostics | Fabricated full-schema conformance, native diagnostic redaction for legacy detail, caller-manufactured result authority |
| Compatibility services | Restricted IO opt-in with stable-host-path precondition; immutable compiler outputs; compile findings block legacy validation | Claiming old helpers are sandboxes, caller records are immutable, or assertion-only true means structural validity |
| Dependency | Schema consumes public Core1.81.2, including XdmExactDecimal, for this qualified facade | Blanket compatibility with every higher Core version; general Core C45/provider/session/ADR006 acceptance or whole-engine freeze |
| Evolution | Reviewed bounded beta contract baseline; future breaking changes explicitly reviewed and announced | Perpetual1.0 API freeze, accepted production SLA or automatic release authority |

Owner acceptance of ADR009/010 covers these application-facing Schema decisions only. General Core C45 and historical ADR006 stay pending in their own scope; the accepted Schema boundary explicitly excludes raw/general provider acceptance from this facade beta and does not close those Core decisions. If the owner requires broader Core acceptance before any beta consumer, that separate decision remains a blocker. No Core source/signature changes are made here.

## Compatibility evidence

The clean immutable source produces **0.5.0-alpha.0.14** through ordinary MinVer, without tag or Version override. It is an unsigned local compatibility package, **not0.5.0-beta.1**, not a renamed beta, and not publication-approved. Package bytes93,961; SHA-25661b2a4b59f780d3d71d2e2676a3443103ee23f37fde0aeb5747f501ef2ce323c. Source commit and legal/README/framework/dependency metadata are verified; only Schema DLL is bundled. Exact package/cache/runtime identities and nine Core1.81.2 repository signatures/payloads pass.

| Lane | Result | Meaning |
|---|---|---|
| Current source solution |345pass/0fail/0skip |312 implementation +33 external public cases |
| Fresh package-only public consumer |33pass |Same current public catalog, no project override/friend grant |
| Retained original legacy binaries |124 control +124 replacement pass |Only Schema DLL differs; all test/config/Core/fixture bytes and case identities unchanged |
| Retained published-alpha public binaries |23 control +23 replacement pass |Only Schema DLL differs; old native public workflow still runs |
| Reflected API |40types/317rows |Exact match to merged source; four additions/no removed rows versus alpha39/313 |

Catalogs overlap; these are not additive coverage totals. Binary compatibility is only for the exercised retained consumers. Intentional behavior changes remain visible: compiler output mutation now refuses and legacy compile findings no longer allow assertion success; unchanged signatures do not erase those changes. [API comparison](beta-contract/API-COMPATIBILITY.json), [named test map](beta-contract/TEST-MAP.json).

## Recorded acceptance disposition

Owner already selected bounded scope. The owner explicitly approved the concrete bounded ADR/API decisions above together with commit/push/PR delivery on17 November2026. Both ADR statuses are now Accepted for this bounded scope, supported by source, package, retained-binary and measured-workload evidence. All prior immutable specification adoption and execution dossiers remain historical; original ADR text/hashes are retained separately. This review does not claim every P01–P12 behavior, complete security/fuzz certification or general provider acceptance.

Local workload envelopes are recorded observations, not engine-enforced time/memory caps or production SLAs. Production operating budgets can remain explicitly unpromised in the beta contract; do not silently turn local smoke thresholds into customer guarantees. Cancellation timing remains pre-cancelled-call measurements plus separately recorded finite lifecycle controls.

## After acceptance

1. Deliver the recorded bounded ADR009/010 acceptance and its exclusions; review/merge and verify main.
2. Review the exact beta version/tag/source identity and adjust the alpha-only release controller for that accepted source/version and345-case catalog. Do not use a Version override or relabel this alpha archive.
3. Prepare a new immutable beta artifact through reviewed automation; rerun required source/package/binary/signature/docs qualification for those exact bytes.
4. Obtain separate exact-artifact publication approval, publish through the configured trusted route, then verify NuGet repository signature/payload and registry consumer.

No beta tag, beta artifact, controller change, login or publication has been performed. Commit/push approval and ADR acceptance are separate from exact-package release approval.
