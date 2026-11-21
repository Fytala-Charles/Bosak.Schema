# Actual native complex restriction feasibility

21 November 2026. Isolated copied-source experiment based on
`7ce8ada7ca78a77a13e07151b199639fa757317a`. Production source/tests/tools/configuration
and the original sealed 17-file contract proposal remain unchanged. Production
adoption and implementation are still pending.

## Measured results

| Lane | Result | Scope |
|---|---|---|
| Unchanged copied baseline | 3 passed | Native extension success, relative namespace and restriction refusal |
| Copied candidate, Core 1.81.2 | 176 passed | Actual native assessment through the signed public packages |
| Copied candidate, Core 1.81.17 | 176 passed | Same exact final protocol with an independent package cache |
| Unmodified implementation catalog on candidate | 581 passed | Existing source tests; same minimum candidate DLL |
| Unmodified public catalog on candidate | 1,561 passed / 1 reason difference | SC-058 stays Unsupported; reason changes to Attributes |
| Public catalog with one scratch-only overlay | 1,562 passed | Only SC-058 unsupportedReason changes; payload/ID unchanged |
| Public API | 54 types / 401 rows, zero delta | Copied baseline/candidate with matching synthetic assembly metadata |

The lanes overlap. They are not a new product test count, external package-only
trial or retained-binary qualification. The copied Schema assembly has synthetic
version 1.0 and is a project reference. Core is consumed only through nine public
packages per lane; all 18 signatures, source commits and archive/cache/runtime
payloads verify. Baseline Core archives are byte-identical to the verified minimum
archives. Both regression runtimes use the exact final minimum candidate DLL.

## What is demonstrated

All 53 concrete contract payloads run through both owned public facades (106
controls). The 16 normative new/base bound conditions run at less/equal/greater
boundaries (48 controls). A further 22 native/ownership/resource controls prove:

- Actual complex G derives from actual complex B, and distinct anonymous C1
  derives from B's actual C0; no QName alias or facet/value-based type identity.
- Root, nested, anonymous and no-target-namespace owners preserve original D/T/G,
  annotations, scalar values and guarded native facts.
- Both data evaluation and an independent typed-identity episode authenticate
  actual C1 provenance, not merely any owned type different from G. A typed lookup
  of already registered native components is used by the provider and probe; it
  cannot mint handles or bypass Core. Equal-facet sibling contexts have different
  C1 handles, while reassessment reuses compiled types with a fresh origin.
- No-facet restrictions retain a distinct anonymous C1. Equal exclusive bounds
  can compile as a valid empty type; mixed endpoint equality and inherited
  weakening fail statically. Omitted inherited facets still constrain values.
- Exact scalar values, including the large decimal payload, survive native read,
  detached export and independent reassessment. Fixed-data ReadTypedValue remains
  BOSAK-ORIGINAL-DATA-UNSUPPORTED; typed identity uses its own original assessment.
- Invalid facet values publish no facts/evidence/snapshot. Foreign/copied nodes,
  unsupported dependency cycles, raw/native numeric limits, query-cache rollback,
  cancellation/recovery, detached values and disposal preserve existing guards.

This exercises representative gates, not every possible combination of the
25 separate planned scenarios. The contract's source/specification pins remain
review evidence; FEASIBILITY-SCOPE.md records experiment-only adoption.

## Candidate shape and preserved comparisons

Only five copied existing files plus one new helper differ from 53 exact Git
source blobs. NativeDecimalSchema/NativeNestedSchema add bounded named dependency
resolution. NativeComplexRestriction validates the new grammar and creates G/C1.
NativeDecimalFacets keeps immutable inherited/local constraints and checks actual
restriction validity before runtime acceptance. NativeInstanceType records the
two separate edges; NativeSchemaProvider registers required components and uses
a typed existing-component lookup. No Core source, public API or profile changes.

The first native protocol passed 163 per lane. Stronger exact-handle/edge controls
passed 171, then the final protocol passed 176. Sources and receipts of earlier
successful protocols are retained; no failed native semantic cases are hidden.
Two original-catalog builds initially targeted the same copied project at once,
causing CS2012 before the public comparison ran. The comparison was then executed
sequentially; the orchestration failure is recorded in BUILD-COLLISION.json.
No machine policy or candidate source change was used to address that collision.

The original public comparison exposes exactly one authored diagnostic difference:
SC-058 contains a restriction attribute declaration, still outside this first
slice. The first detected refusal becomes Attributes rather than the earlier
unknown restriction DeclarationShape. Its outcome remains Unsupported. The only
scratch overlay changes that reason; the original schema, instance, description
and case ID remain intact. All 2,143 prior method/case identities then pass.
Production fixtures are unchanged. Original failed comparison TRX and the precise
[overlay record](feasibility-evidence/REGRESSION-ADMISSIONS.json) are preserved.

## Reproduction and next step

[Execution scope/results](feasibility-evidence/EXECUTION.json),
[package/runtime identity](feasibility-evidence/IDENTITY.json),
[source identities](feasibility-evidence/CANDIDATE-SOURCE.json),
[API comparison](feasibility-evidence/SURFACE.json) and
[source-only materializer](feasibility-evidence/materialize.py) retain the exact
candidate. The materializer reconstructs 53 baseline / 54 candidate files from
pinned public Git blobs and hash-checked overlays in a new directory, rebinds the
probe/comparison projects, and performs no build, network, launch or deletion.
Its smoke check reproduces both source trees and project paths.

The experiment is not reviewed shipping code. Before production edits, record
separate semantic adoption, review resolver/refusal ordering and graph/component
budgets, preserve legacy routes and explicitly reconcile SC-058. Then implement
and run full source, minimum/current package-only, retained callers, signatures,
API and the unchanged guarded frozen corpus. No new pilot execution occurred:
current production qualification remains 16 matches / 5 Unsupported / zero errors.
No new package, release/tag/controller, Core pin, commit/push, broader C45/full-XSD/
full-PSVI or performance acceptance is implied.
