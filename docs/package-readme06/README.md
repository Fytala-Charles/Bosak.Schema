# Packaged README update for the 0.6 diagnostics

19 November 2026. The packaged README now distinguishes the0.6diagnostic API from published0.5.0-beta.1, documents all seven detected reasons and Unspecified/future-value tolerance, and preserves stage-code/message, outcome and grammar boundaries. All three actual C#blocks compile against the local0.6package; public workflow and diagnostic branch probes pass, and ten immutable link destinations are verified. This is documentation only; no new product test count, package build, controller change, tag or publication. Final product source selection waits for review/merge of this package-text change.

## Changes and provenance

The owned session workflow remains usable in published0.5.0-beta.1. The new reason
property is explicitly labelled0.6API and unavailable in beta.1. A refusal-handler
example shows Namespace, Attributes and RepresentationCapacity cases plus a
default that accepts Unspecified and future enum values. The table includes all
seven categories. Messages, absence of owned values, unchanged Invalid handling
for namespaced instances, exception boundaries and non-exhaustive scope are stated.

Sample, banner, license and NOTICE links keep their original9fd4e6a pins.
Contract/integration/registry links point to merged3ff8cb6, which contains the
actual diagnostic implementation. No moving latest/main link replaces an
immutable contract destination. [Verified destinations](PINNED-LINKS.json).

## Executed checks

The probe extracts all three actual C#blocks from docs/NUGET_README.md. It compiles
the owned workflow, refusal handler and established factory sample. The factory
snippet is compiled, not executed. The workflow reads12.5/2/1 and exports the
original12.50lexical form. Refusal probes execute Namespace, Attributes,
RepresentationCapacity, AssertionGrammar/default and Unspecified/default, plus
the Valid guard; private input markers are not printed. Exceptions are caught.

[Results](RESULTS.json). The probe uses only PackageReference
Bosak.Schema0.6.0-alpha.0.2 from the earlier local qualification feed, with an
isolated cache, no source overrides/project references. Actual runtime Schema DLL
matches that package. This verifies new documentation against existing qualified
runtime code; it is not a new prerelease package or a rebuilt README-containing
artifact. No Version override, local/remote tag or registry operation occurred.

Ten pinned destinations were read as exact Git blobs and hashed. Authored
Markdown rendering/link checks and strict documentation checks are recorded
separately. Product code/tests/dependency/controller files are unchanged, so the
earlier main365/54/6result remains historical; no full-suite rerun is claimed.

## Delivery and product source gate

The previous version/controller review is preserved in REVIEW-SNAPSHOT.zip with
its original LF-normalized manifest. Its source selection remains intentionally
null: revised package text must be reviewed and merged before the release
controller can pin an exact new product commit. The proposed version is still
0.6.0-beta.1; no version/publication decision is made by this documentation edit.

Next deliver this documentation change when explicitly requested, review green
CI and merge, then record that exact product revision. Only after that should
controller implementation/rehearsal require365source/53package and retained
124legacy/23alpha/33published-beta catalogs, preserving provenance/identity guards.
The current beta.1 release controller is untouched and must not be dispatched
for the new candidate. No public/private licensing material was consumed.
