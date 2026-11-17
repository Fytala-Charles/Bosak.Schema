# Legacy compile-diagnostic precondition qualification

17 November 2026. Base/main88c3a45fa2f7f37980e2105ba0947399dd77af44 (PR9 merge); main44140782563 green. [Basis](BASIS.json), [contract](../LEGACY_VALIDATION.md), [receipt](VERIFICATION.json), [offline verifier](verify.py).

All three legacy operations now snapshot compile findings after argument validation and return false before any other model/typed/assertion work. Findings retain code/message/path/order/duplicates; a mutable caller diagnostic list cannot rewrite the returned collection. Clean model results retain scoped assertion/facet/static-check meaning, including unmatched-root success when no applicable tests exist. That success is not a structural-validity claim.

Full345pass/0fail/0skip:312 implementation and33 external no-friend consumer cases.22 new implementation cases cover each route with invalid roots, deliberately unusable other collections, mutable diagnostic lists, clean/no-test models, XSD version findings and real denied imports, plus null precedence, lookup/lexical bypass, recovery and existing missing-type codes. Four new public cases cover the three guarded entry points and explicit assertion-only clean success. These26 cases extend the319-case PR9 baseline. All45 separate release controls pass; build0warnings/errors. API40types/317rows identical to PR9.

No network request occurs for denied-import controls; existing DenyAll resolver refuses before acquisition. Expected exceptions stay caught by the test framework. Final TRX, source/API snapshots and merge/main receipts are retained as source-only evidence. Earlier beta, restricted-resolution and descriptor-snapshot dossiers remain unchanged and are verified at their recorded revisions/checkouts.

The source change implements a host-level legacy precondition, not new XSD structural semantics. Copied legacy detail is neither redacted nor bounded as native diagnostics. No new severity model or public result invariant is introduced. Rawpacket `.guard-tmp/schema-compile-diagnostics-20261117`. No commit/push, beta/ADR acceptance, tag or package publication performed.
