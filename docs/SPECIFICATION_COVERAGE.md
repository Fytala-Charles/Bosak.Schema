# XSD 1.1 implementation coverage

Updated 26 November 2026. The [current 24-area matrix](structural-coverage/CURRENT-MATRIX.json) supersedes the 25 November audit for current status; historical matrices and sealed feature dossiers remain unchanged.

Development Schema 0.14 uses signed Core 1.86.0. The last verified published Schema package remains 0.5.0-beta.1, whose scope is narrower. Development source supports bounded native datatype families, finite structural models and original-node typed facts. This is not full XSD 1.1 or general schema-aware XPath/XQuery/XSLT acceptance.

The latest qualified implementation passed 21,358 source tests and 19,701 overlapping package-consumer tests. These source and fresh package-only lanes pass for the structural checkpoint. The frozen 21-case pilot matches all cases; the separate 129-case structural sample has 38 matches and 91 explicit Unsupported results.

A new independently frozen 598-case datatype sample yields 454 matches, 144 Unsupported, zero mismatches and zero harness errors. Selection spans 43 family/variety buckets and valid/invalid facet strata; it is not an exhaustive datatype or specification inventory. See [selection and execution evidence](coverage-driven-400/README.md). No overall conformance percentage is justified.

The structural checkpoint admits anonymous attribute simple types and bounded empty groups. The next work should examine nested instance comment/PI handling and schema annotations before broader grammar expansion. Broader gaps include annotations, implicit anyType, schema assembly, wildcards/open content, identity constraints, type alternatives, general assertions, augmentation and public Core integration. Refusal buckets often contain several features; admitting one feature does not imply every case in its bucket will pass.

The owner-approved ceiling is 400 evidence-selected behavioral contracts, with smaller reviewed implementation checkpoints followed by representative application trials and beta-readiness assessment. It is a ceiling, not a feature quota. Publication requires a separate exact-package decision.
