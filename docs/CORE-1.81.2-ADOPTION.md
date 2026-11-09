# Core 1.81.2 dependency adoption candidate

9 November 2026. Isolated branch `feature/core-1.81.2-adoption`, based on public Schema commit `20090180755d0427d824b091cf838e9a03ca9adc`. The original checkout and its uncommitted review documents are preserved; they are excluded from this candidate.

## Change

Four product PackageReference entries and the test project's direct provider reference change from1.26.3 to1.81.2. No C# product or existing test source changes. MinVer remains unchanged on the0.4 development line; no Schema version tag or publication is performed. Dependencies resolve to the nine published1.81.2 engine libraries with no Core source project references or DLL HintPaths.

## Qualification

- Baseline1.26.3: full solution124/0/0, zero build warnings/errors.
- Candidate1.81.2: full solution124/0/0, zero build warnings/errors; identical case names.
- Unchanged-binary lane: original1.26.3-built Schema/test/configuration bytes retained, only nine engine DLLs overlaid with approved published1.81.2 payloads;124/0/0.
- Additional package-backed integration: eight existing price/staged-assertion cases and three stream/reader ownership cases;11/0/0. A compiled model remains usable after the caller disposes its input, failed compilation leaves the stream open, and supplied readers remain caller-owned. These are135 distinct exercised candidate cases; the repeated124-case binary/baseline lanes are not additive coverage.

The Core-side evidence dossier is `docs/schema-core-1.81.2-adoption` in the adjacent Bosak repository. It retains source/hash pins, the dependency-only patch, test logs/TRX, output DLL/package comparisons, harness inputs and reproducibility notes. The existing historical Core integration wrapper and its044d2cb2 source pin are unchanged.

## Remaining boundaries

Existing XSD1.0/schema-aware transformation and staged XSD1.1 assertion behavior are compatible within the exercised cases. The retained extra-child test still demonstrates incomplete XSD1.1 structural validation. This does not implement native foundation assessor/advisor/session attachment, component-universe interchange, concurrent disposal/operation leases or production provider adoption. Workload budgets remain unaccepted; Core's scoped release deferral creates no Schema performance guarantee. The user requested continuing with review and local commit. This guide belongs to that local dependency checkpoint. Remote push/review/merge and any Schema publication remain separate explicit decisions.
