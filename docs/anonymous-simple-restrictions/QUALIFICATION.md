# Anonymous restriction qualification plan

20 November 2026. Requirements below are pending execution; no new tests or
support are claimed by this document. Base is merged PR29/main1438f71.

1. Prove anonymous decimal/string type handles, null QName annotations and
   original declared/actual provenance through signed public Core1.81.2 and
   separately1.81.17. Cover original nodes, same/foreign issuer, repeated/shared
   type identity, equal but distinct definitions and expiry. Synthetic controls
   qualify transport only, not XSD validation. Record exact signed/runtime hashes.
2. Implement a shared restriction-definition parser for named/anonymous routes,
   preserving previous facet rules and legacy refusals. Do not reset the shared
   local anonymous-definition budget per owner or count reused source sites twice.
   No public API or primitive identity substitution is allowed.
3. Add authored clause-mapped happy/edge cases: all six numeric facets, string
   length/min/max/enumeration, empty restrictions, local QName shadowing,
   namespace forms, optional/repeated/shared/nested definitions and original
   provenance. Cover inconsistent/duplicate facets, unsupported types/facets,
   type-plus-inline conflicts, attribute/root exclusions, 16/17 local-definition
   boundary including unused definitions, invalid values, input/output capacities,
   cancellation/disposal/cache recovery and hint/type/nil policy coexistence.
4. Preserve all unaffected 1,302 baseline source outcomes. Qualify NS-051's
   explicitly anticipated compile/instance transition using the original schema
   and XML; replace its unreachable placeholder values transparently. Scan C#
   theories and fixture catalogs for any further affected refusal expectations,
   recording each original row/source hash before edits.
5. Run the full source suite, then package-only minimum/current-Core consumers,
   unchanged public API surface and retained binary consumer lanes appropriate
   to the native feature. Keep overlap and synthetic/native scopes explicit.
6. Rerun all 21 frozen upstream cases through exact candidate source/runtime
   hashes. Compare each result against the sealed PR29 checkpoint (1 match,
   20 Unsupported); the six eligibility targets are not guaranteed passes.
   Preserve original notices and every fixture/metadata/expected-validity byte.
   Never count Unsupported as validity matches or report a whole-XSD percentage.
7. Sync canonical and packaged consumer documentation, render/check links and
   preserve original sealed dossiers. No commit/push or publication without the
   applicable explicit owner request. Historical baseline harness defaults remain
   pinned to the original product; use a separately reviewed candidate receipt.
