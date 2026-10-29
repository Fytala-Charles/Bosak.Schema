# Bosak.Schema handover — 29 October 2026

This is the clean Apache-2.0 public schema repository. The original private repository/history and billing/key-generation code are preserved separately as Bosak.Licensing. Do not copy private history, credential helpers, deployment settings or commercial test vectors into this repository.

What was built: owner-approved schema-only license/publication split, Apache/SPDX/package metadata, activation-free factories, credential-free legacy API shims, public CI and a 114-case schema regression suite. Source base `fa0da3e2d9972706a5a86616ef285af4daf5037b`; public delivery commit is recorded in the publication receipt and Git history.

Current development line: 0.4 through MinVer; no new tag/NuGet publication is implied. Core package pins remain 1.26.3. Full XSD 1.1 structural validation and the core session/PSVI integration are incomplete/pending. The licensing change does not accept the C44-R2 public API proposal.

Next: reassess the smallest necessary core/session contract around the open implementation, then qualify real component completeness, compatible-schema handoff and consumers before engine integration. See [architecture](ARCHITECTURE.md), [registry](FEATURE_REQUESTS.md), [integration](INTEGRATION.md) and [transition](APACHE-TRANSITION.md).

Last updated: 2026-10-29
