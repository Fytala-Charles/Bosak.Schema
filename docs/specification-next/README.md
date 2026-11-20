# Specification priorities after PR30

20 November 2026. Merged product base:
`5bfbb1e798cdf18bac6ac7bb15705b1a0e50bc65` (PR30).
The merge tree equals reviewed head `a637a5d51a324fa5430a4697c6b557c1b2989209`.
This is an assessment and proposed sequence, not a semantic adoption, engine
implementation, new consumer acceptance or additional upstream measurement.

The latest frozen pilot remains **7 validity matches, 14 Unsupported, zero
mismatches and zero harness errors**. Of the remaining cases, ten stop at schema
compilation and four at instance assessment. [REMAINING-CASES.json](REMAINING-CASES.json)
joins the original identities/hashes to the sealed current outcomes and static
blocker review. The 21 cases are a small reviewed sample, not a coverage percentage
or a measure of how much of XSD 1.1 remains.

## Recommended sequence

| Priority | Work | Frozen cases blocked | Reason and required gate |
|---|---|---:|---|
| 1 | Direct integer restrictions | 2 | Complete the existing exact-integer scalar family with bounded facets; preserve BigInteger values and original named/anonymous identity |
| 2 | Native typed-node query integration | Outside pilot | Important for application use across Bosak and Schema; needs an explicit query-environment/API/lifetime contract and native qualification, not just a Bind guard edit |
| 3 | Bounded string assertions | 1 direct target | Existing string values are supported, but string comparison assertions need explicit grammar, namespace, cardinality and equality semantics |
| 4 | xsi:type semantics | 4 | Needs both valid and invalid override coverage, expanded QName resolution and type/derivation rules; do not ignore it as a hint or reject all overrides to gain four matches |
| 5 | Complex simple-content restriction | 1 direct target | Requires inherited content/attribute/assertion restrictions and original complex versus atomic provenance |
| 6 | Namespace/attribute-form compatibility | 2 relative namespace + 3 attributeFormDefault cases | Separate policies; changing attributeFormDefault alone would leave integer/inheritance/assertion blockers in all three cases |
| Later | Broader/nested assertion semantics | 1 additional direct target plus overlaps | Includes nested empty content and namespace functions; cannot be inferred from a string-comparison slice |

Priorities weigh coherent application capability and semantic dependencies, not
only the number of pilot matches. The string/advanced assertion and namespace
categories overlap; the priority-table counts must not be added as independent
coverage areas. Other XSD features absent from this frozen sample remain unassessed.

## First proposed slice

[INTEGER-CONTRACT-PROPOSAL.md](INTEGER-CONTRACT-PROPOSAL.md) proposes opt-in direct
named and anonymous integer restrictions using the existing exact integer lexical
route and bounded range/digit facets. It is a proposal with explicit pre-code
adoption and qualification gates. No parser, facet semantics or profile guard
has been changed here. Named/anonymous decimal/string and all earlier receipts
remain unchanged.

## Separate integration boundary

Actual native anonymous facts and owned scalar reads are qualified. Public Core
transport feasibility is separately positive, but the native advisor deliberately
does not bind a general `data($node)` environment. A legacy XPath `data(.)` call
without owned schema facts treated null-QName anonymous decimal annotations as
untyped. These observations are recorded in
[ROUTE-LIMITS.json](../anonymous-simple-restrictions/implementation/ROUTE-LIMITS.json).
Future integration needs explicit public node/snapshot access, original ownership,
type/provenance lifetime, detached-copy behavior, expression semantics and consumer
acceptance. Preserve the scalar-only boundary and Core pins until that work qualifies.

No publication, version/tag/controller or broad C45/full-XSD/PSVI acceptance is
implied. Changes in this planning worktree are uncommitted.

[Main CI44762728337](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/44762728337)
passed 1,373 source tests (421 implementation / 952 public), release controls,
six workload controls and documentation hygiene. [Merge receipt](MERGE-VERIFICATION.json).
