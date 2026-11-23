# Native derived-integer feasibility

22 November 2026, production base `f9e9ad7c0599ba7b36b6d8d71704875990b54b98`.
The isolated candidate passes **435 controls per signed Core pin**, 1.81.2 and
1.81.17. Unchanged-production baselines pass 24 refusal controls per pin. The lanes
overlap and are not additive specification coverage.

## Qualified candidate

Two existing-source overlays and one new private registry construct all twelve
built-in integer descendants with their exact parents and inherited bounds. The
59-file source candidate is reconstructible from the pinned 58-file base and three
explicit overlays. [Experimental adoption](feasibility-evidence/EXPERIMENT-ADOPTION.json)
precedes scratch source changes; production adoption remains separate.

All 158 contract payloads run through both public native facades (316 controls).
Another 84 controls verify actual original declaration/type/atomic identity across
12 types and 7 placements. The remaining 35 controls exercise inherited complex integer
restrictions/assertions, selected governing identity, source quotas, excluded syntax,
recovery, cancellation, disposal, original authority, legacy refusal and original
upstream inputs. Built-in metadata does not consume user declaration capacity or
force caller value digit limits to contain every standard bound. Unsupported syntax
is checked before admitted facet legality, preserving enumeration refusal.

Values beyond Int64, including unsignedLong's exact maximum, pass BigInteger reads.
Original node QName annotations and authority-owned declared/actual components are
correct on both pins, including named/anonymous restrictions and distinct complex
versus atomic content types. The descriptive XdmValue.SchemaTypeName field is null;
that observation is retained separately from original provenance and does not claim
general typed-query acceptance. No detached relabelled value substitutes for original
facts. Public component closure remains Unsupported as before.

[Results](feasibility-evidence/RESULTS.json), [gate review](feasibility-evidence/GATE-REVIEW.json),
[84 original identity observations per pin](feasibility-evidence/ORIGINAL-IDENTITY-OBSERVATIONS.json),
[Core/runtime identities](feasibility-evidence/IDENTITY.json), [API](feasibility-evidence/SURFACE.json).

## Preserved initial failure and regression admissions

The first run passed all 316 public payload controls but its identity selector used
numeric ID positions, which include interleaved negative boundary cases. Thirteen
identity assertions per pin failed as a result. Selection now uses the explicit
positive identity purpose across all twelve types; the original tests, logs and TRX
remain preserved. No candidate source fix was required.

The unchanged source comparison passes 1,005 implementation cases with 2 authored
admissions and 2,166 public cases with 12 admissions. Scratch-only reconciliations pass
all 1,007 implementation and 2,178 public cases, retaining **all 3,185 original stimuli**.
Nine original JSON rows now accept existing derived-family values: M2C3-62..65,
IR-096/097 and IC-051..053 (the IC rows run through both facades). Two native controls
change expectations: xs:int global root becomes Valid; xs:int selected for a string
declaration becomes Invalid. These are 11 logical expectations / 14 executed cases.
Schema, instance and authored IDs remain unchanged. The selected-type test's explicit
expected-outcome argument is renamed in its display identity, recorded as one exact
before/after mapping rather than a removed case. Production fixtures remain unchanged.
[Exact admissions](feasibility-evidence/REGRESSION-ADMISSIONS.json),
[stable stimuli and display mapping](feasibility-evidence/STABLE-CASES.json).

## Original examples and limits

Six direct facade probes per pin use the three unchanged original int fixture pairs.
The two enumeration schemas remain Unsupported. The totalDigits schema compiles Valid
and its original instance is Invalid, matching its upstream invalid expectation.
These are direct probes, not full corpus runs or a predicted corpus admission count.
All 18 signed Core archive/source/cache/runtime identities verify, and the public API
stays 54 types / 401 rows with zero delta.

Production source/tests/API/Corepins/packages/controllers and published beta.1 remain
unchanged. Frozen 21 matches and expanded 3 matches / 126 Unsupported / zero errors remain prior
production measurements; no new full corpus/package/retained-binary qualification
was performed here. No full integer value-space, XSD, PSVI/C45, closure or SLA claim.

Next: separate production adoption, implement the qualified registry and two overlays,
record the 11 logical authored reconciliations explicitly, transfer public/native
controls, update consumer guidance, then qualify source, both package lanes, retained
callers, API, exact packaged example and both corpus pins. No commit/push/publication.
