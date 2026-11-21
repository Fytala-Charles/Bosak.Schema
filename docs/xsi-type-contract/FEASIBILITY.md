# Actual native xsi:type feasibility

21 November 2026. **Isolated copied-source experiment; production implementation,
semantic adoption and ADR017 review remain pending.** Base
b1759d22761587b2abe507d12aacf3f5b6b33535. Production src/tests/tools/configuration
and the original sealed contract packet are unchanged.

## Result and qualification scope

| Lane | Result | Scope |
|---|---|---|
| Unchanged copied baseline | 3 passed | Original no-request facts and same-request/legacy refusal |
| Native candidate, Core 1.81.2 | 169 passed | 96 xsi:type controls plus 73 retained original-node controls |
| Native candidate, Core 1.81.17 | 169 passed | Same exact probe catalog, independent package cache |
| Original implementation catalog on candidate/minimum Core | 520 passed | Unmodified source tests |
| Original public catalog on candidate/minimum Core | 1,362 passed / 4 expected differences | Original Unsupported expectations for xsi:type=true now see Invalid |
| Scratch-reconciled public catalog | 1,366 passed | Exactly four recorded expectation changes; production tests unchanged |
| Public API comparison | 54 types / 401 rows, zero delta | Copied baseline/candidate assemblies with matching assembly metadata |

The scopes overlap and must not be summed into a product test count. The copied
Schema assembly is a project reference with synthetic assembly version1.0, not a
published Schema package or retained-binary qualification. All18Core archive
signatures/source revisions/cache/runtime payloads verify. Baseline1.81.2 archives
are byte-identical to those freshly verified in the minimum lane. No Core source
or project reference is used.

## What the native experiment demonstrates

The 33 concrete proposal payloads run through both existing owned facades (66
controls). Selected direct decimal/string/integer restrictions actually enforce
narrower facets: values accepted by the original primitive fail under G. No-request
and same-type behavior, named complex roots, complex simple-content roots and
nested containers/terminals are exercised with real NativeSchemaProvider assessment.

Native facts retain original declaration D and its declared T. Separate effective
state supplies G to facts.Type, annotations, typedValue.GoverningType and atomic
DeclaredType. Selected simple values retain actual G; unchanged complex simple
content retains distinct atomic C. Existing native component identities and exact
advisor/input/origin/snapshot ownership are authenticated internally. Nothing uses
a transport assessor, fake primitive alias or copied-node authority.

Additional controls exercise instance prefix shadowing/default-namespace
undeclaration, anonymous versus sibling identity, legitimate but deferred decimal
to named-integer and simple-to-complex derivation, illegal complex weakening,
literal xsi prefix in an application namespace, invalid/unsupported no-publication,
query-cache rollback, cancellation/recovery, export/reassessment, detached lifetime
and foreign/copied-node refusal. The 73 original-node controls also preserve
existing profile/static-context, quotas, original facts and lifetime boundaries.
This measures representative gates, not every scenario in the original60-row plan.

## Preserved failed iterations and profile finding

The first build was refused by xUnit2013 for a collection-size assertion; it was
corrected to Assert.Single. No runtime tests ran in that build iteration.

The first native run passed150/153 cases on each lane. Three additional probes
incorrectly called ReadTypedValue from a fixed original-node-data operation. The
released API intentionally returns BOSAK-ORIGINAL-DATA-UNSUPPORTED for that call;
the pinned1.81.2 source confirms the profile guard. A focused diagnostic retained
the exact refusal code. This is a probe correction, not a native-selection defect
or a reason to relax the guard.

The final tests explicitly preserve that refusal, then prove G evidence through
fixed data evaluation. Separate actual native typed-identity sessions compile
$value, perform their own assessment/admission and prove ReadTypedValue/evaluation
provenance. No existing assessment is rebound between environments. Failed probe
sources/TRX and the final sources are retained separately.

## Regression expectations and implementation shape

Original source comparison changes exactly four authored xsi:type=true outcomes:
AS-029, IR-088, and OtherXsiRequests_RemainUnsupported(type, owner0/owner1).
The QName true is legal but resolves to no type in those closed schemas. The
prototype therefore returns Invalid instead of Unsupported/Attributes. The nil
and unknown special-attribute cases keep their previous refusal. Original failed
comparison receipts and exact scratch-only expectation overlays are retained in
[REGRESSION-ADMISSIONS.json](feasibility-evidence/REGRESSION-ADMISSIONS.json). With these four explicit changes, all1,886 stable
method/case identities pass; the production fixture/test files remain untouched.

Only four copied existing source files plus one new partial helper differ from
52 exact baseline Git blobs. NativeSimpleRestriction retains the parsed base QName;
NativeDecimalSchema/NativeNestedSchema prepare explicit component base links,
resolve requests and preserve effective state; NativeSchemaProvider projects G;
NativeInstanceType contains closed QName/type lookup and same-family selection.
NativeSessionNode needed no change because it already reads guarded fact NativeType.

This is a minimal feasibility candidate, not reviewed shipping code. Production
adoption must still fix immutable graph construction details, complete path/
resource/error-priority controls and reconcile the four authored admissions in
review. Package-only consumers, retained124/23/33 callers, final product metadata
and the guarded frozen pilot require new production-candidate receipts.

## Reproduction and next step

The [evidence materializer](feasibility-evidence/materialize.py) reconstructs exact baseline/candidate source trees from
public Git blobs plus hash-verified overlays in a new scratch directory, rebinds
regression projects, and performs no network/build/launch/delete operation. It
requires the exact base with unchanged source/tests/tools/configuration. Its smoke
check reproduced both trees and project paths. Use independent minimum/current
caches and verify signed archives before interpreting results.

The current production frozen pilot remains10matches/11Unsupported/zero mismatch
or harness errors. No new upstream measurement, package, release/tag/controller,
publication or broader C45/general XPath/full-XSD/fullPSVI acceptance is implied.
Next: record actual production semantic adoption from the exact reviewed sources,
implement the qualified type/fact mapping in the public source worktree, then run
full consumer/binary/pilot qualification before review and delivery. No commit or
push has been requested for this candidate.
