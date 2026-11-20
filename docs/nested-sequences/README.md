# Bounded nested-sequence development candidate

20 November 2026. Local implementation on main c34e9fe, not a published release.
The [adopted contract](ADOPTION.md) implements finite acyclic nested element-only
sequences through NativeSchemaSession with existing scalar types and root
attributes. [Contract](CONTRACT.md), [qualification plan](QUALIFICATION.md),
[before-code public Core feasibility](FEASIBILITY.md) and
[packaged example](../NUGET_README.md#development-bounded-nested-sequences).

Complete element facts stay aligned with original nodes in preorder; a separate
internal projection drives terminal-only ScalarCount/ReadScalar. Containers retain
complex annotations and FOTY0012, and local declaration identity does not imply
global declaration metadata. Named types may be shared by distinct local
declarations; repeated instances retain distinct nodes/facts. Root/attribute reads
and old flat indexes are unchanged. There is no new public API or Core pin change.

Limits/exclusions are intentional profile refusals, not complete XSD semantics.
The authored nested-order fixture is representative, not an unchanged extraction
from orders.xsd, a W3C-suite case or customer acceptance. The full public orders
schema still refuses independent pattern/mixed/unbounded constructs. Published
0.5.0-beta.1 and general API/C45 acceptance remain separate.

## Assessment precedence

Root selection/attribute behavior retains existing ordering: an unknown root
with an application attribute can refuse as Unsupported before the wrong-root
Invalid check. Within nested containers, descendant attributes are checked before
non-whitespace text and unsupported markup, then each sequence occurrence is
fully assessed before its maximum count check, followed by minimum and trailing
unexpected-child checks. Deep failures publish no facts/document. Capacity is
computed eagerly across optional branches before any instance is admitted.


## Local qualification

[Exact results](QUALIFICATION-RESULTS.json) record 1,125 source cases
(395 implementation/730 public), all 1,036 prior named outcomes unchanged,
and 76 authored nested cases: 22 positive, 17 instance Invalid, ten instance
Unsupported and 27 compile Unsupported. Four additional public operation controls
and nine actual provider/lifecycle controls complete the 89 new cases.

The unsigned local0.6.0-alpha.0.13 compatibility package passes all730minimum-Core
public cases. On the explicitly overridden current-reference Core1.81.17 lane,
729unchanged public cases plus one version identity control pass; the original
minimum1.81.2-only version assertion stays in the minimum lane. Its initial
current-lane version failure is retained and is not a runtime defect. Both lanes
verify18signed dependencies/source/payloads in total. Retained124/23/33binary
callers pass with onlySchemaDLLchanged. These catalogs overlap, not additive
coverage totals. API52types/389rows is unchanged; ten actual packaged C#blocks
compile and run.54release controls, six workload controls, workload consumer
build and documentation hygiene pass. Source hashes identify the qualified dirty
files; local package metadata records only basec34e9fe. No publication or merge.

Pre-code probes/adoption and original sealed dossiers remain historical inputs.
The qualification plan's future-tense rows are selection criteria; this receipt
is the executed subset. Tests do not claim every normative clause or every
mixed-failure permutation. No official W3C suite, general API/C45 acceptance or
performance SLA is inferred. Source and evidence hashes are recorded separately.
