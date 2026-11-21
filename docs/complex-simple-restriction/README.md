# Decimal complex simple-content restriction: bounded contract proposal

21 November 2026. PR35 is merged as7ce8ada7ca78a77a13e07151b199639fa757317a.
Its tree equals the reviewed313fc1c tree; main CI44884962349 passes2,143source
cases(581implementation/1,562public), release controls, docs and workload integrity.
[Merge receipt](MERGE.json), [hosted verification](MAIN-CI.json).

The next proposed slice adds one direct decimal complex simple-content restriction
of an existing named complex extension base. It starts with attribute-free,
assertion-free base/derived types, the six existing decimal facets, original root/
nested scalar owners and exact decimal values. It does not claim general complex
inheritance or solve the integer/assertion cases. Published beta.1/Core pins and
public API remain unchanged; no new source, test or package execution in this step.

[Contract](CONTRACT.md), [outcomes](OUTCOMES.json), [facet conditions](FACET-RULES.json),
[proposed payloads/gates](CASE-PLAN.json), [immutable review pins](SPECIFICATION-REVIEW.json).

## Required representation and correctness

Complex G derives from complex B; anonymous atomic C1 derives from B's original
content type C0. Those are separate actual component edges. C1 stays distinct even
when no new facets are supplied. Original declaration D/T and current G/C1 must
remain authenticated through native facts, atomization, export/reassessment and
original session/snapshot ownership. No QName alias for the anonymous content.

Runtime enforcement of inherited and new facets is necessary but insufficient:
compile-time restriction validity must check all relevant inherited facet
conditions. The proposal records16 new/base bound relations plus four same-type
bound constraints. Equal exclusive bounds in one step may form a valid empty
type; mixed inclusive/exclusive equal endpoints are schema-invalid. Do not use
one generic nonempty-range rule or accept a widening because one value happens
to fit both types. Known admitted invalid facets/references return schema-Invalid;
excluded grammar/dependencies and capacities remain Unsupported. Existing legacy
simple-restriction/reference outcomes are preserved.

## Evidence limits and next gate

The53 authored schema/instance payloads and25 additional gate scenarios are plans,
not runtime tests or conformance passes. XML well-formedness, IDs, relation-table
shape, unchanged upstream hashes and local links are linted; those checks do not
execute Schema or prove the proposed expected outcomes. The prospective direct
upstream target is MS-ComplexType2006-07-15/ctD017/ctD017.v, currently Unsupported.
It remains part of the unchanged frozen16match/5Unsupported corpus. A future pass
must be measured against exact candidate hashes, not assumed from this proposal.

Next: record experiment-only adoption and run actual-native copied-source
feasibility on signed public Core1.81.2/1.81.17. Only after a successful gate,
record production adoption and implement/qualify the bounded slice. No new public
API, Core source/profile change, full-XSD/PSVI/C45 acceptance or performance SLA.
This proposal remains uncommitted; no commit/push/publication is requested here.
