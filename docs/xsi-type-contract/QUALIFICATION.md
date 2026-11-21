# Proposed xsi:type qualification gates

20 November 2026. This is a plan, not test execution or a conformance result.
[CASE-PLAN.json](CASE-PLAN.json) lists proposed authored controls with expected
bounded outcomes. Original W3C cases are reviewed separately in
[PILOT-REVIEW.json](PILOT-REVIEW.json).

1. Prove actual native D/T/G and original episode ownership on both pinned Core
   lanes in a scratch candidate. Use existing registered type components and real
   NativeSchemaProvider assessment, not a transport provider or mocked authority.
   Capture all source/config/runtime hashes and keep failed iterations separately.
2. Record ADOPTION.md before production semantic edits, referencing the exact
   immutable review bodies/sections and explicit first-slice exclusions. Do not
   conflate review date, adoption, feature PR, fixture revision or publication.
3. Implement QName selection, explicit base metadata and separate effective-type
   state. Qualify all root/leaf/nested/attribute-use paths; preserve no-request and
   decimal-only behavior. Do not simply add xsi:type to an ignored attribute set.
4. Author happy and invalid/unsupported cases in both public owned facades, and
   native component/annotation/provenance tests. Include positive overrides which
   actually narrow facets; a suite consisting only of rejected requests is not
   sufficient. Include same complex identity and distinct sibling identity.
5. Verify unknown/unbound/malformed names, instance default/prefix shadowing,
   256-unit raw-token ceiling, unsupported known built-ins, illegal weakening,
   sibling restrictions, cross-family valid derivation refusal and simple-to-complex
   valid derivation refusal. Verify no hint acquisition or authority from QName text.
6. Reconcile existing authored refusal probes explicitly. Preserve original bytes
   and case IDs, and record each changed expectation; do not silently reinterpret
   old Unsupported expectations. Failed requests publish no facts/snapshot.
7. Run full source, package-only minimum/current Core and retained124/23/33 lanes;
   compare existing stable case identities and exact public API54types/401rows.
   Verify all18Core archive signatures/source pins/runtime payloads and published
   beta baseline. Case counts and quotas must describe their actual scope.
8. Run the unchanged guarded frozen21-case/42-file corpus with new candidate
   source/runtime hashes. Keep all upstream metadata/notices/expectations. The four
   complex008 invalid cases are potential additional matches, not current passes.
   Do not promote the slice without independent positive override evidence.
9. Sync canonical docs and seal new receipts, then review the bounded contract
   and implementation. Explicit commit/push requests remain required by AGENTS.

Current source qualification remains the merged1,886cases; current frozen pilot
remains10matches/11Unsupported/0mismatch/0harness errors. Those are historical
executions of the merged implementation, not this planned behavior. General
complex inheritance, new datatypes, lists/unions, nil, general XPath/fullPSVI,
production SLA and publication remain separate.
