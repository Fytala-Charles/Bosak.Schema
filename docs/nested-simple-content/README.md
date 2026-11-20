# Nested decimal simple-content development candidate

20 November2026. Implemented locally on merged main32f8214; no new publication.
[Before-code adoption](ADOPTION.md), [contract](CONTRACT.md),
[proposal](PLAN.md), [feasibility](FEASIBILITY.md),
[proposed ADR015](../ADR-015-nested-simple-content-scalar-projection.md) and
[actual packaged example](../NUGET_README.md#development-nested-decimal-simple-content).

Local named/anonymous complex simple-content terminals directly based on decimal
or an admitted named-decimal restriction now join descendant scalar reads in
document order. Root reads remain separate; existing valid document indexes are
unchanged. Original complex governing/declared type differs from actual decimal
atomic type, and local declaration metadata stays local. Terminal node string-value
uses schema whitespace collapse; exact scalar reads and lexical XML export remain
distinct. Original element-only ancestors still raise FOTY0012 on atomization.

Attributes retain exact captured original owners. Depth4scalar-bearing owners may
now have three-segment element-child paths; this does not increase structural
depth, terminal/attribute limits or public surface. No new public types/members.
Other simple-content bases, complex derivation/restriction chains, broader particle/
composition/assertion/default/nil/mixed features and primitive simple-type leaf
attributes remain outside scope. Original complete orders.xsd still refuses its
independent pattern/mixed/unbounded constructs.

## Source qualification

1,255source cases pass(415implementation/840public):1,198prior outcomes unchanged,
NS-062/NA-046exact compile admissions with Invalid supplied instances,55new cases.
The39authored cases plus five public operation/root-reuse and11actual native/
lifecycle controls cover the new profile. Before-code original row hashes and
adoption preserve the two intentional deltas; old unreachable instance Valid
fields are not evidence. Source passes do not by themselves qualify package,
retained, API or official W3C-suite behavior; those lanes are recorded separately.
General API/C45, publication and performance SLA acceptance remain open.


## Completed local compatibility qualification

[Exact results](QUALIFICATION-RESULTS.json) record840minimum-Core public package
cases and839applicable current-reference1.81.17cases plus one identity control;
the original minimum-only version assertion remains in its minimum lane. All
18Core signatures/source/archive-cache-runtime identities verify. Retained124/
23/33binary callers pass with onlySchemaDLLchanged. These lanes overlap and are
not additive conformance totals. API52types/392rows is unchanged. Twelve actual
packaged README C#blocks execute, including nested price/currency;54release and
six workload controls plus workload consumer build pass.

The unsigned local0.6.0-alpha.0.15 package metadata records base32f8214only;
actual dirty source hashes identify the qualified implementation. Original
before-code and earlier sealed dossiers remain separate. No registry/tag/
controller dispatch, Core integration-pin refresh or new acceptance is inferred.
