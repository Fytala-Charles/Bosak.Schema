# ADR-007: Apache-2.0 Schema Engine and Clean Public Repository

**Status:** Accepted by owner.
**Date:** 2026-10-29.
**Decider:** Charles Korthout.

## Context

Schema services are shared standards infrastructure. The owner approved Apache-2.0 and public GitHub availability, with key-generation/billing infrastructure explicitly remaining private. The existing private repository contains both, including their shared history.

## Decision

Retain the original repository/history privately under Bosak.Licensing and publish only an explicitly selected current schema snapshot as a new Bosak.Schema repository. Use Apache-2.0, copyright/SPDX notices, expression-based NuGet metadata and activation-free schema factories. Retain public legacy license type/member signatures as credential-free compatibility shims. Publish no credential implementation or private history.

Keep the schema assembly modular and dependent on published Bosak packages; do not introduce a core dependency cycle. Revisit the pending provider/session API design separately. No licensing choice substitutes for type/provenance/resource correctness or complete provider/consumer tests.

## Consequences

Schema consumers need no subscription or activation. Public builds no longer require payment/cloud/Azurite tooling. Private licensing services keep their original implementation and repository history. Apache rights apply to the new public source; historical releases and other product licenses remain separate. This decision does not change Bosak.Folio's proprietary license or make a NuGet release.

## Validation

All 114 public schema tests pass. Check package license metadata, public file selection/history and repository IDs/visibility before publication. Preserve a migration receipt and an exact source manifest. See [transition evidence](APACHE-TRANSITION.md).
