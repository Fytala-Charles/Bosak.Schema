# Bounded structural namespace assertions: production candidate

22 November 2026, base `2effee5bf9daa317599c9285ba2de5e333bc786f`.
[Exact production adoption](PRODUCTION-ADOPTION.json) precedes implementation;
[114-controls-per-pin native feasibility](FEASIBILITY.md) and earlier packets remain
separately sealed. The candidate is locally qualified and uncommitted.

## Resulting behavior

The opt-in NativeSchemaSession and NativeSchemaNodeDataSession admit attribute-free
empty complex types and bounded structural namespace assertions on finite sequences.
Bare/empty-sequence, named/anonymous root and nested owners retain original governing
identity, source authority and lifetimes. A separate internal context observes the
actual instance expanded name; it never fabricates a scalar or reuses an integer zero.

The closed grammar supports namespace-uri()/namespace-uri(.), unprefixed or with an
ASCII prefix bound to the functions namespace, equality/inequality with string literals,
and/or and parentheses. Static bindings are captured from each original component.
URI strings preserve case, percent escapes and relative spelling. Function bindings
are independent of xpathDefaultNamespace. Unsupported syntax is checked even in dead
branches. Legacy string/decimal child assertions and integer assertions remain separate.

Empty complex content has a present non-list zero-atomic typed sequence, rejecting
all character/element children, including whitespace. Element-only content retains
absent typed value/FOTY0012 even when optional children are absent. data() atomization
of original empty nodes yields empty; scalar reads and singleton typed identity refuse.
Empty owners are excluded from scalar counts, with HasRootScalar=false. Source export
remains available. False/invalid/cancelled/refused assessment publishes no partial facts.

Existing64node/4depth,16scalar/64attribute and16occurrence budgets remain. Empty nodes
consume node/depth capacity. New assertions retain8components/512characters/128tokens/
16expression-depth limits. New structural/application-attribute combinations, structural
inheritance, other function-prefix forms, general XPath/traversal, mixed/list/union
content, comments and PI remain outside this bounded extension. Legacy decimal-only
NativeDecimalSession retains its prior profile; two explicit public refusal controls
protect the boundary. No public API or Core pin changes were made.

## Qualification

| Lane | Passed | Scope |
|---|---:|---|
| Source implementation | 900 | Includes60new original/context/limit controls |
| Source public consumers | 1,986 | Includes54proposal/facade and2legacy-boundary controls |
| Package-only minimum Core1.81.2 | 1,986 | No project references or friend access |
| Package-only current Core1.81.17 | 1,986 | Existing copied loaded-version assertion only |
| Current package native controls | 60 | Existing friend boundary for original evidence |
| Retained callers | 124 /23 /33 | Only Schema.dll changed |

These overlapping lanes are not additive specification coverage. All2,770prior stable
source identities pass without fixture reconciliation;116new cases make2,886total.
The API remains54types/401rows with zero delta. All18Core package signatures/source/
archive/cache/runtime identities and the signed published beta.1 baseline verify.
The source DLL, local package payload and guarded pilot driver agree; packaged README
matches source and its exact XML example passes both facades, scalar refusal and
whitespace-invalid controls. [Results](implementation-evidence/RESULTS.json),
[stable identities](implementation-evidence/STABLE-CASES.json),
[API](implementation-evidence/SURFACE.json),
[package/runtime identities](implementation-evidence/IDENTITY-FINAL.json).

The frozen21case pilot now measures **21matches/0Unsupported/0mismatch/0harnessError**.
Only Assert/assert_027/assert_027_2 leaves Unsupported and matches its original valid
expectation. All42metadata/schema/instance files and expectations remain unchanged.
[Measured delta](implementation-evidence/PILOT-DELTA.json),
[guarded runner report](implementation-evidence/pilot/RESULTS.json).
A complete match on this selected pilot does not establish whole-specification coverage.

## Preserved findings and delivery limits

The first expanded source run found one new legacy-profile refusal failure: the
structural function path still admitted decimal-only input. The production compiler
now explicitly gates both empty types and new functions to the opt-in profile. An
initial edit missed its multiline source anchor; corrected source line endings and
the final source/package lanes pass. Initial source/logs and the unconsumed initial
package hash remain retained separately. A copied current-consumer project had a
duplicate Exclude attribute; its initial project/log and corrected run are retained.
No prior receipt was rewritten or failed run relabeled as a pass.

Local unsigned0.6.0-alpha.0.29 is qualification only. Published0.5.0-beta.1, signed
Core pins and release controller remain unchanged. There is no commit/push/tag or
registry publication. Full XSD1.1/PSVI/C45/performance acceptance remains separate.
The Core public XML empty-content example is still a separate documentation correction.

Next: review and deliver this candidate on an explicit Git request. After merge,
verify main CI before selecting the next specification gap beyond the frozen pilot.
