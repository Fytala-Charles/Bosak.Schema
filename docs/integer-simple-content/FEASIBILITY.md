# Actual native integer simple-content feasibility

21 November 2026. Isolated experiment on base
`ab7e20afacd83edf93e71be2123fb40235c4f646`, after
[experiment-only adoption](FEASIBILITY-SCOPE.md). Production source/tests/tools,
original fixtures and the sealed contract proposal are unchanged. This is not
production adoption, package qualification or full XSD/PSVI/C45 acceptance.

## Result and scope

The candidate passes **203 actual-native controls per signed Core pin**, minimum
1.81.2 and current 1.81.17. Each lane includes the 60 proposed payloads through
both public facades (120), 48 normative bound comparisons and 35 additional
identity/static/resource/ownership controls. The unchanged native baseline passes
three controls, including original decimal success and integer-extension refusal.

Three copied source files change: NativeDecimalSchema admits attribute/assertion-
free integer extension of primitive or named direct integer simple restrictions;
NativeComplexRestriction dispatches integer content to an actual anonymous integer
C1; NativeIntegerFacets retains inherited profiles and validates restrictions
statically before runtime conjunction. No Core workaround or public API addition.
54 original source files are reconstructed from exact Git blobs, with three
explicit experimental overlays; all 54 candidate source hashes are retained.

Real native facts and atomization prove integer kind, 50-digit exact values beyond
Int64/decimal, G->B versus C1->C0, no-facet distinct C1, anonymous root/local and
sibling contexts, primitive/named extension C0 and original D/T/G provenance.
Absolute/relative/no-target namespaces retain original components. True complex
xsi:type substitution remains Unsupported; weakening stays Invalid. Cycles and
used/unused restriction chains are eagerly refused. All six admitted numeric
facets, fixed fractionDigits=0, 16 inherited-bound relations, valid empty integer
ranges, raw/native digit caps, failed-query rollback, cancellation/recovery,
foreign/copy refusal and reassessment origin separation are covered.

The data-profile ReadTypedValue operation remains Unsupported. A separate native
`$value` typed-identity session proves actual integer content provenance in its
own assessment/snapshot episode. The fixture recorder captures the existing static
context fingerprint only; all assessments use the real NativeSchemaProvider,
original inputs and signed Core sessions. No stand-in authority or validation.

## Original source comparison and explicit differences

The unmodified implementation catalog passes **651/651**. The original public
catalog has **1,665 pass / 3 fail**, from exactly two authored fixtures:

| Input | Original expectation | Measured candidate result |
|---|---|---|
| SC-053 | Unsupported / ValueType | Unsupported / Attributes; its required application attribute remains excluded |
| CR-042, both facades | Schema Unsupported / ValueType | Schema Valid; original `4.5` instance Invalid under integer lexical grammar |

Schema, instance and authored IDs remain unchanged. The second difference is an
admission of integer schema grammar, not acceptance of a fractional integer value.
Only copied runtime fixture expectations were reconciled. That separate run passes
all **1,668 public cases**. Together, all **2,319 prior stable source cases** match,
preserving class/method, authored ID and every theory selector. Production fixture
expectations have not been edited. These explicit deltas must accompany any later
production adoption; no general legacy outcome rewrite is implied.

[Original failures and scratch-only reconciliation](feasibility-evidence/REGRESSION-ADMISSIONS.json),
[stable case identities](feasibility-evidence/STABLE-CASES.json),
[execution counters](feasibility-evidence/EXECUTION.json).
These test catalogs overlap and are not an additive conformance total.

## Identity, preservation and reproduction

All **18 public Core package signatures** verify. Archive nuspec source commits,
cache DLL bytes and loaded runtime bytes agree with the two recorded published
pins. Both lanes use isolated caches and Schema source project references; they
are not package-only Schema consumer lanes. Prototype and comparison runtime
Schema DLLs match exactly. The public API remains **54 types / 401 rows**, zero
delta. [Identity receipts](feasibility-evidence/IDENTITY.json),
[API comparison](feasibility-evidence/SURFACE.json),
[production input hashes](feasibility-evidence/PRODUCTION-INPUTS.json).

The unsupported dotnet test argument in the first command attempt and incorrect
probe-only Core member names in an intermediate strengthened build are preserved,
with corrections and source snapshots. Neither launched semantic negative cases.
The first stable-case normalizer dropped dataFacade selectors; the corrected
inventory retains every selector and verifies all 2,319 unique names.
[Iteration record](feasibility-evidence/ITERATIONS.json). No machine policy changes,
unhandled negative .NET launches or test-assertion weakening.

[Reconstruction tool](feasibility-evidence/materialize.py) accepts `--repository`
pointing at the exact base with unchanged product inputs and `--output` pointing
to a new directory. It reconstructs original/candidate source and rebound probe
projects from pinned Git blobs plus hash-checked overlays without network/build/
launch/delete. Example from the repository root:

```powershell
python docs/integer-simple-content/feasibility-evidence/materialize.py `
  --repository . --output D:/Development/Bosak/.guard-tmp/integer-reproduction
```

For each of baseline/minimum/current, run its Probe.csproj in Release with an
explicit absolute `-p:RestorePackagesPath=...` and capture TRX. The two regression
projects share the minimum Schema project: build/run those sequentially. Original
regression fixtures stay linked from the exact base; copy the documented overrides
into the public runtime TestData only before a separate `--no-build --no-restore`
comparison. Retain the original failed run. Do not edit source fixtures to conceal
the original differences. No product/package/pilot command is run by the tool.

## Remaining gates and next step

[Gate review](feasibility-evidence/GATE-REVIEW.json) separates demonstrated native
behavior, unchanged source regressions and deferred production qualification.
No new product package, retained-binary run or frozen pilot was executed here.
Original corpus bytes/notices/expectations and previous seals remain unchanged;
production qualification stays 17 matches / 4 Unsupported / zero errors. Integer
support alone does not satisfy inherited/contextual assertions in assert006.

Next: record production semantic adoption, implement the measured bounded slice
with both explicit authored reconciliations, then qualify full source, min/current
package-only consumers, retained124/23/33, API/signatures and unchanged guarded
pilot. Excluded attributes/assertions, derived integer built-ins, longer complex
chains and broader xsi:type policies remain separate. No Git commit/push, tag,
controller or registry action is authorized by this experiment.
