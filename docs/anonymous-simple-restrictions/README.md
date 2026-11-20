# Next slice: anonymous local decimal/string restrictions

20 November 2026. PR29 is merged at
`1438f719863d0c13f976f1b0eb683e146a3511e5`, byte-identical to its reviewed head.
[Main CI44752564556](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/44752564556)
passes 1,302 source tests, release refusal controls, six workload controls and
documentation hygiene. The existing frozen pilot remains **1 validity match,
20 Unsupported, zero mismatches/errors**; there are no additional admissions yet.

The [pre-implementation adoption](ADOPTION.md) selects anonymous local element
types directly restricted from decimal/string, reusing existing facet semantics.
The work belongs to the opt-in NativeSchemaSession; legacy refusal, integer
derivation and other unsupported XSD constructs remain explicit boundaries.
No production code, public API, Core pin or package/release changed in this step.

The main feasibility issue is **original anonymous simple-type identity**:
primitive decimal/string value storage must not replace the anonymous governing
type or invent a type QName. Existing anonymous complex support does not prove
this route. [Qualification](QUALIFICATION.md) requires signed minimum/current
public Core probes before implementation and actual native provenance checks after.

[Anticipated deltas](ANTICIPATED-DELTAS.json) pin NS-051 and six upstream
eligibility targets. The original NS-051 instance contains one decimal; its old,
unreachable seven-value placeholder must be corrected explicitly when admitted.
Two inline integer pilot cases remain excluded. Targets are static review,
not executed transitions or a promise that fixing one blocker admits a case.

Next: run anonymous type transport feasibility, then implement and qualify the
bounded parser/facts extension. Changes in this planning worktree are uncommitted.
