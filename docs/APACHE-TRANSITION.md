# Apache/public schema transition — 29 October 2026

## Owner Decision and Provenance

The owner explicitly requested Apache-2.0 licensing and public GitHub availability for Bosak.Schema, then selected **schema engine only; keep licensing/billing infrastructure private**. Fytala is the declared copyright owner in the selected source. The previous private Git authors are Charles Korthout/Fytala-Charles. This transition retains Fytala copyright, adds SPDX notices and preserves source change-history rows.

Source base: `fa0da3e2d9972706a5a86616ef285af4daf5037b`. The public initial repository is a clean, schema-only source snapshot, not a rewritten copy of the private Git history. The original repository is retained privately as `Fytala-Charles/Bosak.Licensing`; its history, issues, settings and commercial code remain associated with that repository. A new `Fytala-Charles/Bosak.Schema` contains this Apache source.

## Published and Excluded Material

Published: the schema Compilation/Model/Validation implementation, factory helpers, public legacy API compatibility shapes, schema regression tests/local fixtures, Fytala-owned selected documentation branding and public documentation/build tooling.

Excluded: the licensing pipeline, key-generation executable, credential verification/issuance implementations and embedded product key, all payment/deployment/local settings, private commercial/Enterprise Fabric documents, old Git objects/history and original commercial test vectors. Only selected current files enter the new public Git history. Historical pricing/agreement terms and their copyright notices are not rewritten.

No third-party implementation code or upstream conformance-suite checkout is included. Bosak dependency packages retain their own Apache notices. Locally authored XSD/XML test inputs are retained with the schema source. Fytala trademarks are not licensed by the Apache copyright grant.

## Behavioral Change and Compatibility

The schema factories no longer check activation. The old public `License` type remains a credential-free compatibility shim, with repeated calls ignored, a perpetual Apache status marker and no entitlement for other products. Existing type/member signatures are retained; credential authorization semantics are intentionally retired for this public package. Internal issuers/verifiers and their friend-assembly access are removed.

W3C schema semantics are unchanged by this licensing policy change. Earlier implementation/adoption evidence is preserved in the private history. This does not adopt new schema validation semantics, approve the pending core session API or promote conformance cases.

## Validation and Publication Evidence

The complete public solution passes 114 tests. Schema integration/assertion happy and invalid paths, invalid configuration, legacy API calls and absence of credential implementations are covered. The isolated pre-split transition also passed 151 tests while retaining the private credential implementation; those results are distinct from the final public suite.

The publication receipt records local/remote commits, original/private and new/public repository IDs, visibility, package license metadata and selected-file/credential scan results. No Git history is force-pushed, no old repository is deleted, and no NuGet package or live licensing deployment is changed.

## Verified Publication

GitHub recognizes [Bosak.Schema](https://github.com/Fytala-Charles/Bosak.Schema) as public and Apache-2.0. Original repository ID 1381449647 remains private as Bosak.Licensing; new public ID 1438210599 has a clean parentless initial commit `c4058c33f83960c7937989571d6a01f4ae1e4ffc`. The 114-test public CI passed on [attempt 2](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/40851118519). The first private-visibility attempt was blocked before execution by account billing limits and is retained as historical evidence.

See [machine-readable receipt](PUBLICATION.json). Local working copies are `D:/Development/Bosak.Schema.Public` for the public engine and `D:/Development/Bosak.Licensing` for private infrastructure. The original locked `D:/Development/Bosak.Schema` checkout remains intact and points to the private licensing remote; do not use it as the public engine checkout. No process was stopped or private directory deleted.

*Last updated: 2026-10-29*
