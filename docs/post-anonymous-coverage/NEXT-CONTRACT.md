# Proposed next contract: bounded named-integer restriction bases

Proposal only. No semantic adoption, product implementation or new contract execution occurs in this review. Record fresh immutable adoption before a future implementation.

The unchanged original `MS-DataTypes2006-07-15/test111092_1657/test111092_1657.i` declares B as a restriction of xs:int with maxInclusive=5, then R as a restriction of B with maxExclusive=5. Both instance values are 5, and the original expected result is Invalid. Current compilation refuses the named base on the ordinary integer facet path; anonymous integer bases already have a separately qualified route. Source evidence is NativeSimpleRestriction.cs: the named-integer branch restricts baseName.Namespace to the XML Schema namespace.

The next implementation should admit ordinary named integer bases using their actual NativeType object and existing exact inherited facet compiler. Preserve BaseName/source QName and direct DerivationBase identity, original facet source, dependency ordering/final rules and all existing capacities. Keep patterned-ancestor value-facet refusals and the legacy decimal-only facade boundary. No synthetic alias, source cloning, Core pin/API expansion, complex derivation or instance override widening.

Required gates for that future contract:

1. Fresh immutable Structures 3.16 / Datatypes 4.1.5 and integer facet adoption, distinct from these review pins.
2. Built-in integer families and multi-step named bases with exact bounds/digits/enumeration ancestry; valid values and boundary failures.
3. Ordinary and numeric-pattern routes agree on actual type/base identity while retaining their qualified exclusions.
4. Forward references, cycles, final restrictions, namespace scopes and unused-definition validation retain original authority.
5. Fixed/nonwidening facets and equal inclusive/exclusive endpoints follow the adopted component constraints.
6. Existing root/local/attribute/compound/extension placements only; actual selected member and governing types remain distinct where required.
7. Source-site, type, supplied/aggregate digit, atom/value/string/regex/schema and lifetime budgets remain.
8. Complete Invalid/Unsupported assessment precedes facts; nil, ownership, cancellation, recovery, original XML/export and detached expiry remain.
9. Preserve all old test names and fixture bytes; archive every measured expectation reconciliation separately.
10. Re-run the original test111092 schema/XML unchanged. Add separately labeled authored positive and boundary variants; never relabel those variants as upstream originals.
11. Qualify both owned facades and native source/type/value authority, then a fresh isolated exact-package consumer, API and signed Core payloads.
12. Run full source, original samples/invoice, docs/release/workload controls and authorized PR review/green merge/exact-main CI.

Subsequent scopes are separate: QName length/minLength/maxLength must validate schema components without measuring lexical prefix strings; defaultAttributes/defaultAttributesApply applies schema groups to type attribute uses rather than inserting instance attributes. No registry/tag/controller action is part of this proposal.
