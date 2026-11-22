# Native empty typed-value feasibility

21 November 2026, base `2effee5bf9daa317599c9285ba2de5e333bc786f`.
[Experiment scope](EMPTY-VALUE-SCOPE.md),
[exact experiment-only adoption](empty-value-feasibility-evidence/EXPERIMENT-ADOPTION.json).
The original contract packet and all previous sealed receipts remain unchanged.

## Measured result

**29 native controls pass per signed Core pin**, 1.81.2 and 1.81.17. Before overlays,
the unchanged source passes four baseline controls per pin: existing decimal success
and prospective empty-type refusal through both facades. These are overlapping lanes,
not additive conformance coverage. [Execution receipts](empty-value-feasibility-evidence/EXECUTION.json).

Five isolated source overlays admit bare/empty-sequence complex types without
assertions, at root and nested positions, with named and anonymous type identities.
Each candidate is reconstructed from 56 exact production Git blobs. Original native
assessment issues a present non-list zero-atomic container: HasNoTypedValue=false,
IsList=false, TryGetSingleAtomic=false and ordered AtomicItems empty. The actual signed
Core original-node data operation returns the empty sequence, without a dummy atomic,
list flag workaround or per-atomic provenance. Original facts retain governing type,
origin and exact advisor/input ownership. No Core runtime edit or dependency change
is needed for this measured path.

Element-only types retain absent typed value/FOTY0012 even when optional children
are absent. Empty types reject character content, including whitespace, and elements;
invalid assessments publish no facts, evidence or snapshot. Comments/PI remain
Unsupported profile exclusions. Foreign/copy guards, cancellation/recovery, lifetime,
source export and reopen controls pass. Empty nodes do not appear among scalar facts;
HasRootScalar=false. Public scalar reads throw NotSupportedException: the data facade
reports its existing unsupported scalar-family condition, distinct from FOTY0012.
The typed-identity profile independently refuses the zero-atomic container as
Unsupported; this does not erase its authentic present-empty original facts.

All 18 Core package signatures, source commits, archive/cache/runtime DLL identities
verify. [Identities](empty-value-feasibility-evidence/IDENTITY.json),
[signature logs](empty-value-feasibility-evidence/signatures),
[isolated source delta](empty-value-feasibility-evidence/CANDIDATE.diff),
[probe source](empty-value-feasibility-evidence/EmptyTests.cs).
Core's public XML HasNoTypedValue example still conflates empty and element-only
content. The measured runtime supports this distinction on the tested data path;
the documentation discrepancy remains a separate public Core correction.

## Preserved failures and limits

The initial CLI command incorrectly passed --packages to dotnet test. The first
baseline probe compared the default exact-decimal ToString with numeric text; two
controls failed per pin. The first candidate probe reopened a second document while
its single snapshot slot was occupied; eight controls failed per pin. Corrected
harnesses pass without additional source overlays. Initial source/logs/TRX are
preserved beside the final receipts. No failure has been relabeled as a pass.

This is the first empty-value gate, not full structural-assertion feasibility.
Namespace-uri grammar, original assert_027_2, expression/node budget boundaries,
xsi:type combinations, all 27 planned payloads/18 gates, full 2,770-case comparison,
package/retained-binary qualification and production adoption remain pending.
Production src/tests/tools/Core files and the public API remain unchanged. No
package, frozen pilot or controller was changed or executed. The prior frozen result remains 20 matches / 1 Unsupported /
zero errors across 21 cases and 42 unchanged files. Published beta.1 is unchanged.

Next: extend the same isolated candidate with bounded structural namespace assertions,
then execute the complete contract, capacity and stable-regression controls before
considering production adoption. No commit/push/publication is authorized for this step.
