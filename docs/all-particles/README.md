# Bounded all-group contract

PR [#43](https://github.com/Fytala-Charles/Bosak.Schema/pull/43) merged as `724d834b7e2cf1353bdacf100802ff29866c8c71`. [Main CI45168029488](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45168029488) passes all 3,739 source tests plus release-refusal, documentation and workload checks. The reviewed choice head and merge have identical trees. The [choice implementation](../particles/IMPLEMENTATION.md), historical contract and all earlier seals retain their original identities.

This separate proposal covers a direct **root `xs:all`** over existing bounded scalar types. It is a contract and source/specification review only. No all semantics have been adopted or implemented; product source, tests, build inputs, packages, Core pins and published beta.1 remain unchanged.

## Proposed subset

An inline or named root complex type may contain one nonempty all group: 1-16 direct explicitly scalar-typed local elements. The outer group has minOccurs 0 or 1, maxOccurs 1; each member has minOccurs 0 or 1, maxOccurs 1. Members can appear in any physical order, and each present member must match its own expanded QName and declaration. Direct competing same-QName particles are Invalid within the recognized subset, including equal declared types.

These are intentional profile exclusions, not general XSD 1.1 occurrence limits. XSD 1.1 permits broader all-group content/member occurrence syntax. Nested all containers, group references, wildcards, complex members, substitution groups, implicit anyType, mixed/open content, repeated groups, new scalar/type-chain families and broader child assertions remain excluded. This first all slice preserves the already merged choice and sequence grammar.

## Presence and validity

Required members must occur once; optional members may be absent. Duplicate members, unknown expanded names, missing required members and non-whitespace element-only text are Invalid within the admitted grammar. A member cannot substitute for another required member.

An absent optional whole group allows an empty instance even when its members are required. Once any member appears, all required members apply. A group whose members are all optional may itself match empty content even when its outer minOccurs is 1. Neither case changes the root into an empty-content complex type: its typed value remains absent, with HasNoTypedValue/FOTY0012 rather than an empty typed sequence. A present empty string is one atomic value; an omitted member has no node, facts or scalar slot.

## Original identity, routing and budgets

Capture and assess members in **physical document order**. Declaration-order iteration or sorting original XML would attach wrong facts and scalar indexes to reversed members. Each element retains its original declaration, governing/actual type, expanded name, native path, source/snapshot and assessment origin. Selected same-family xsi:type must preserve the declaration/governing-type distinction and enforce inherited bounds. Root attribute reads retain their existing declaration-order policy and exact owner paths.

Both flat and nested compiler routes must work, including catalogs where an unrelated nested root changes routing. Named root cloning must retain the all discriminator. Nested all containers remain excluded. Existing attributes, anonymous restrictions and structural namespace assertions retain their current budgets and combination restrictions.

All-group worst-case budgets sum member maxima; choice budgets continue to take the largest alternative. Sixteen all members produce at most sixteen scalar leaves and seventeen element states before attributes. Existing scalar16/node64/depth4/attribute64 and named/anonymous-site limits remain unchanged. Compile all definitions eagerly, including omitted members and unused types. Shared pattern work covers present scalar inputs; failure/refusal/cancellation must publish no partial facts. Snapshot opening, foreign authority, sequential work, export/reopen, disposal and detached-read rules remain intact.

## Evidence prepared, not executed

- [43 XML payloads](CASE-PLAN.json): 23 inherited original all-phase plans and 20 additional order, optionality, namespace, facet, attribute, assertion, capacity and selected-type controls. Schema/instance XML parses statically; proposed native outcomes are unexecuted.
- [20 actual-native gates](NATIVE-GATES.json), planned on signed Core pins 1.81.2 and 1.81.17, through both opt-in facades and the original native provider. No substitute XML validator or synthetic facts.
- [Exact specification revision/body hashes and sections](SPECIFICATION-REVIEW.json), reviewed before any future semantic change; this review is not semantic adoption. Existing public Core review pins remain in [the earlier Core review](../derived-integer-types/CORE-SOURCE-REVIEW.json).
- [Ten exact Git source inputs](SOURCE-REVIEW.json) and [296 unchanged production/test/tool/build inputs](ENGINE-INPUTS.json) from merged main.
- [Three unchanged original all examples](ORIGINAL-EXAMPLES.json) retain independent implicit-anyType co-blockers. They must be probed unchanged; no original admission forecast is made.

The current source contains 23 AllRemainsExcluded controls. The proposal would change 18 of their compile expectations; five remain Unsupported. [The explicit future reconciliation plan](EXISTING-REFUSALS.json) preserves each original ID, schema and instance hash. Do not silently remove these controls or reinterpret existing passing refusal tests as all validation evidence. Measure the isolated candidate first, then record justified production admissions before changing authored expectations.

## Next sequence

First build an isolated all-only source overlay and qualify actual native behavior, physical-order identity, nullable root type evidence, optional-group/member rules, selected types, budgets and lifetime on both signed Core pins. Preserve the 3,739 previous source stimuli; record any proposed outcome changes explicitly. Only after feasibility should a separate production adoption precede source edits and complete source/package/retained/API/exact packaged example/frozen and expanded corpus qualification.

The contract worktree is `D:/Development/Bosak/.worktrees/Schema.All`, branch `planning/bounded-all`. It has no new commits or pushes. No full XSD, full PSVI/C45, typed-query closure, performance or percentage claim follows from this proposal. No tag, controller preparation or registry publication is authorized by this contract.
