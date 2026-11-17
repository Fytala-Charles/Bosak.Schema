# ADR-009: Bounded native decimal session beta profile

**Status:** Accepted — bounded Schema beta contract, 17 November 2026
**Date:** 2026-11-15
**Review date:** 2026-11-17
**Deciders:** Repository owner; bounded decision explicitly accepted 17 November 2026

## Context

The owner selected a bounded decimal-session beta with explicit exclusions. The public owned workflow shipped in0.5.0-alpha.0.7; PR8/9/10 added restricted IO, immutable compiler snapshots and legacy compile-diagnostic preconditions; PR11 records local workload evidence. Those deliveries did not themselves accept this ADR; the owner explicitly accepted the bounded decision on17 November2026. The [original proposal](beta-contract/ORIGINAL-ADR-009.txt) and [source identity](beta-contract/ORIGINAL-ADRS.json) preserve the historical context; original specification/adoption dossiers are unchanged.

## Accepted bounded decision

Accept the [finite grammar](NATIVE-DECIMAL-GRAMMAR.md) as the first Schema beta's native support boundary: an owned synchronous string-input workflow, one unqualified root, ordered bounded decimal leaves and the recorded finite assertion grammar. Complete Valid admits an owned document only after the supported grammar succeeds. Invalid/Unsupported publish no document. Argument, cancellation, quota, closure and overlap exceptions remain separate. No source bytes or exported scalar/XML confer transferable schema authority.

Require explicit numerical/session capacities and explicit assessment context; consume released Core1.81.2 for the qualified scalar path. No Core dependency on Schema is added. Public raw provider/session contracts and their general C45 acceptance are not accepted or frozen by this Schema decision. [The bounded contract review](BETA_CONTRACT_REVIEW.md) records this accepted scoped consumption disposition and leaves wider Core decisions pending for their own owner review.

Exclude native target namespaces, application attributes, broader datatypes, nil/list/union profiles, arbitrary XPath, stream/reader/async native APIs, full component relations and positive cross-universe transfer. Legacy descriptor/assertion and Core XSD1.0/XSLT helpers remain separate services; metadata recording and assertion-only success are not full XSD1.1 structural assessment. Neither all P01–P12 scenarios nor skipped engine tests are accepted globally.

## Evidence and costs

[Compatibility qualification](beta-contract/README.md) records345 source cases,33 repeated package-only cases, retained124 legacy/23 public binary catalogs and nine signed Core packages. API is40types/317rows, four additions/no removals versus the published alpha. Intentional runtime behavior changes are documented. Counts overlap and do not prove arbitrary consumer/semantic coverage.

[Local measurements](BETA_QUALIFICATION.md) qualify named Windows workflows only. Representation/admission limits are not total memory/time budgets. No production SLA, long-duration leak, adversarial scheduler or cross-platform performance acceptance follows. Wider schema profiles require new immutable semantic adoption evidence and paired tests before implementation.

## Approval boundary

The owner explicitly accepted this bounded decision on17 November2026. Acceptance approves Schema's selected beta support contract and exclusions only. It does not waive general Core C45/ADR006 decisions, certify complete XSD1.1, create a beta tag, accept a future artifact or authorize publication. [Review decisions and remaining release steps](BETA_CONTRACT_REVIEW.md).
