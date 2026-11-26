# Evidence-driven Schema work

26 November 2026. Owner-approved ceiling: 400 behavioral contracts selected by application value and upstream evidence. [Plan](PLAN.json); [current coverage matrix](CURRENT-MATRIX.json); [human coverage guide](../SPECIFICATION_COVERAGE.md).

## Baseline evidence

The [unchanged 129-case structural ranking](BASELINE-GAP-RANKING.json) reuses sealed qualification: 32 matches / 97 Unsupported / zero errors. The independent [598-case datatype cohort](datatype-cohort/CASES.json) was frozen before execution from accepted W3C metadata at revision 7bc3365c652a322f3d762021b3879eb92dae7e30. Original metadata, schemas, instances, copyright and hashes are retained. [Selection review](SELECTION-REVIEW.json) records the corrected, unexecuted draft separately.

The new [baseline execution](datatype-cohort/BASELINE-RESULTS.json) reports 454 matches / 144 Unsupported / zero mismatches / zero harness errors against the exact qualified 0.13 package. A freshly built caught runner verifies its Schema DLL and all nine signed Core 1.86.0 DLL identities. No source-suite rerun or new engine implementation is claimed by that execution. Samples and authored tests overlap and do not define a normative denominator.

## Selected next work

First: empty all/optional empty choice and anonymous attribute simple types, with original type/declaration identities, empty typed values, existing capacities and refusal boundaries. Adoption and tests must precede semantic changes. Annotation processing is a subsequent candidate because source appinfo/documentation currently blocks otherwise supported declarations; general wildcard and anyType work requires broader provenance design.

No new implementation, package publication or general XSD 1.1 acceptance is claimed at this checkpoint. Historical dossiers are unchanged.
