# Contextual and inherited integer assertion implementation

21 November 2026. Candidate based on verified PR37 merge
`2e8d6102e10e9ccbddee87249eaefa2fdfaa4468`. [Production adoption](ADOPTION.md)
was recorded after248actual-native controls per signed Core pin and before source
edits. Original proposal and feasibility records remain separately sealed.

Five production source files (three existing, two new internal helpers) implement
bounded contextual assertions on attribute-free integer simple-content extension
and its one direct complex restriction. The internal partial context keeps dot
untypedAtomic semantics separate from exact typed$value. Effective assertions
retain inherited/static component identity and require base/local conjunction.
Original final D/T/G/B/C0/C1, native ownership and authority remain unchanged.
No public API or Core pin/profile change is introduced.

## Qualification

| Lane | Passed |
|---|---|
| Full final source | 2,770 (840 implementation / 1,930 public) |
| Package-only Core1.81.2 | 1,930 |
| Package-only Core1.81.17 | 1,930; existing copied loaded-version-only expectation overlay |
| Current-package native/static/ownership | 106; overlaps source |
| Retained binary callers, only Schema.dll replaced | 124 / 23 / 33 |
| Public API | 54 types / 401 rows, zero delta |
| Identity verification | 18 signed Core source/archive/cache/runtime identities and signed beta.1 baseline |
| Unchanged frozen corpus | 20 matches / 1 Unsupported / zero mismatch or harness errors |
| Frozen runner integrity controls | 15 |
| Exact packaged README example | Both facades:20valid,80inherited failure,5local failure |

All2,522prior stable source identities remain. New248source cases are106internal
and142public. These overlapping catalogs are not additive specification coverage.
Package-only consumer assets contain no project references; the internal current-
package lane uses the existing friend assembly for real native component evidence.
Its public counterparts have no internal or authority access. The current Core
public lane changes only the existing copied test's loaded-version expectation.

[ADMISSIONS.json](ADMISSIONS.json) records exactly two authored reconciliations:
IC-046/IC-050 now compile but reject their unchanged `. gt 0` instances because
value comparison converts dot to string and encounters numeric type mismatch.
Schema/instance/ID remain unchanged. No SC-053 reconciliation or historical fixture
override is introduced. Quota/cancellation and excluded grammar remain distinct
from assertion failure. The full source catalog passes without scratch overrides.

[Results](implementation-evidence/RESULTS.json),
[stable source identities](implementation-evidence/STABLE-CASES.json),
[API](implementation-evidence/SURFACE.json),
[signature/runtime identities](implementation-evidence/IDENTITY-FINAL.json),
[pilot delta](implementation-evidence/PILOT-DELTA.json).
TRX, exact input projects, source/test/fixture snapshots and signature logs retain
their individual qualification scope. Packages and binary caches remain outside
public documentation. This candidate has no blocking self-review findings and no
failed production qualification lanes.

## Measured conformance change

The frozen21case corpus and all42original metadata/schema/instance files stay
byte-identical. Three cases leave Unsupported and match their original validity:
Assert/assert006/assert006.v1.xml, assert006.n1.xml and assert006.n2.xml. Both
negative cases remain instance-Invalid, not unsupported. Original schema tests
still use dot membership and mod, with no rewrite to$value or expected outcomes.

The remaining Unsupported case is Assert/assert_027/assert_027_2, involving
namespace-uri and structural/empty-content contexts. Its original refusal is
preserved. One detected refusal reason does not inventory every missing feature;
no full XSD conformance percentage is inferred from the pilot.

## Review, consumer guidance and remaining scope

Self-review covers the closed grammar, dot/value numeric and type-error boundaries,
NaN/mod-zero behavior, inherited immutable static bindings, implicit xml prefix,
effective quotas before allocation, every clone/root/nested routing path and eager
unused definitions. Original facts publish only after success; cancellation,
recovery, rollback, foreign/copy refusal, detached lifetime and independent typed-
identity episodes retain their measured guarantees. The temporary partial context
is an internal finite projection, with no issued public schema authority.

The reconciled packaged guide recommends$value for exact integer tests and explains
why dot comparisons can round or fail type checking. Its XML example was compiled
and executed from the local package through both public facades. Source Schema.dll,
package payload and guarded pilot runner agree; packaged README bytes match source.

Local unsigned0.6.0-alpha.0.28 is qualification only. Published0.5.0-beta.1, Core
pins and release controller remain unchanged. No commit/push/tag/publication.
General XPath/functions/paths, other scalar contexts, application attributes,
structural/empty-content assertions, broader type chains and complex xsi:type
substitution remain outside this slice. No full-XSD/PSVI/C45/SLA acceptance.

Next: review and deliver the qualified candidate on an explicit Git request.
Then define a separate bounded structural-context/namespace-uri assertion contract,
with exact normative adoption and actual-native feasibility before implementation.
