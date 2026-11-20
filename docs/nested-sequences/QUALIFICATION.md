# Nested-sequence qualification plan

20 November 2026. All rows below are planned, not executed passes. The delivered
baseline is 1,036 cases; planning adds no product or conformance test count.

| Area | Required positive and negative evidence | Source clauses |
|---|---|---|
| Local components | Named and anonymous containers; forward references; shared type, distinct declarations; undefined QName and direct/indirect/unused cycle refusal | XSD Structures 3.3, 3.4, 4.2 |
| Sequence assessment | Required/optional/repeated siblings at two levels; missing/extra/out-of-order children; present optional with invalid required descendant; same local name in different containers | Structures 3.8, 3.9 |
| Occurrence and capacity | 0/1 minimum, finite maximum; per-parent counts; depth 4/5, leaves 16/17, defensive node-budget arithmetic (64 is unreachable under depth/leaf bounds), anonymous-type 16/17; multiplicative expansion/overflow; unreachable optional excess | Structures 3.9 plus intentional profile limits |
| Namespace/forms | Default/prefixed names, equal local names with wrong URI; local qualified/unqualified overrides; namespaced root attributes | Structures 3.3, 3.4; existing namespace adoption |
| Scalars and attributes | All admitted scalar families/facets nested; exact integer/decimal values and lexical capacities; required/optional root attributes; descendant attribute refusal | Existing datatype/facet/attribute adoptions unchanged |
| Actual Core facts | Every node maps to its original fact; type/declaration identity; container data() FOTY0012; exact leaf data(); foreign/missing/expired facts refuse; interleaved container/leaf positions | XDM 2.3, 2.4, 2.7, 6.2 |
| Public projection | Preorder terminal-only reads; no container/attribute insertion; repeated and omitted nodes; old root scalar/no-typed-value behavior and bounds | Proposed contract; existing facade contract |
| Atomic publication | Deep lexical/facet/order/count failure after valid siblings; no partial facts/document; quota recovered; exact deterministic diagnostics | Structures 3.3.4, 3.4.4; existing ownership contract |
| Lifetime/operations | Cancellation/overlap, owner/document disposal, shared caches/query limits, failed export then reuse, detached values survive | Existing session contracts |
| Export/reopen | Bounded lexical-preserving export, whitespace and character references, exact reassessed values; container string-value distinct from serialization | XDM 6.2; existing XML/writer adoption |
| Scope refusal | Choice/all, recursion, groups, imports, nil/default/fixed, nested simple content/attributes, nested-profile assertions and whole orders.xsd | Intentional exclusions, not support coverage |

Before code, run synthetic nested transport controls on independently verified
signed Core 1.81.2 and current published Core, preserving signature/source/payload
identities. Then exercise the real native provider; synthetic success is not
validation evidence. Run full source and prior-regression comparisons, package-only
minimum/current consumers, unchanged retained 124/23/33 binary catalogs, API
reflection showing zero public delta, all actual packaged README examples,
documentation/release/workload controls and hashes/render/link checks.

Record schema/instance/source revisions, exact positive/Invalid/Unsupported counts
and intentional deltas separately. Authored examples are not official W3C-suite
admissions or customer acceptance. Keep old immutable receipts unchanged.
