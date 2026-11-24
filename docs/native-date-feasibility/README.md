# Revised native date feasibility

24 November 2026. Isolated candidate based on merged main2eb999052188a872a54548c71c378d6a05a8cc3c. Production source/tests remain unchanged. This experiment follows the [original proposal](../date-time-transport/README.md) and [measured formatter stop](../date-transport-preflight/README.md); their sealed receipts are preserved.

## Scope and adoption

The [experimental adoption record](EXPERIMENT-ADOPTION.json) was written before isolated source edits. It pins the immutable normative bodies and revises the supported year interval to [-9223372036854775807,9223372036854775807], with256 normalized lexical characters. Original DT-013, containing long.MinValue, remains Valid/unexecuted in the original sealed plan; the separate [experimental plan](case-plan.json) expects Unsupported. Capacity refusal occurs separately from an invalid bounded lexical/calendar value. No production semantic adoption follows from this experiment.

The candidate admits direct xs:date roots, supported scalar members/attributes and facet-free direct named/anonymous date restrictions. It preserves actual date atoms, declaration/governing/atomic identities and selected date aliases. No new particle grammar or arbitrary user-type chains. Date simple-content extension/restriction, date assertions and all temporal facets remain explicitly excluded in this experimental scope. dateTime/time/dateTimeStamp and exact fractional-second transport are separate dependencies.

## Results on each signed Core pin

**99 uniquely identified native controls pass** on Core1.81.2 and1.81.17. The37 revised XML cases run through both public facades. Additional controls cover actual original-node date atoms and provenance, direct/named/anonymous roots, flat and nested compiler routes, attributes, xsi:type aliases, extreme admitted years, optional/explicit timezone, capacity diagnostics, wrong accessors, cancellation, recovery, export/reopen, owner isolation, document/session disposal and detached lifetime. NativeDateValue is immutable with no public constructor; enum values1-4 and all existing API signatures are retained.

After explicitly measured scratch-only reconciliation, **4,147 source cases pass per pin:1,301 internal /2,846 public**. The original4,146 logical cases remain. One expected-outcome display changes from Unsupported to Invalid; an existing AL-008 identity theory becomes newly eligible, adding one source execution. These source lanes overlap the native controls and are not additive product coverage. [Exact names and counts](SUMMARY.json).

The first source runs recorded nine semantic expectation failures per pin. Formerly excluded date roots/members/attributes and an optional all-member become compilable; invalid date instance text remains Invalid. A date requested as an unrelated decimal xsi:type is Invalid rather than Unsupported. Seven logical fixture/harness changes reconcile those nine failures. The current-pin lane also initially failed its hard-coded minimum-Core version assertion; only that isolated qualification copy now expects1.81.17. [Original failures and explicit reconciliation](REGRESSION-RECONCILIATION.json). Original schema/XML/IDs and upstream expectations are retained. The surface diagnostic test preserves its newly admitted date stimulus and adds a separate deferred-time refusal control.

Early native runs had colliding truncated placement display names. Stable DP identifiers distinguish all placements in the final receipts; no payload was removed or changed to resolve those labels. All initial logs/TRXs remain historical. A redundant unreconciled regression repeat is retained separately; it is not claimed as passing.

## API and identity

[API comparison](SURFACE.json) preserves all401 original rows and adds seven: the NativeDateValue type, its four readonly properties, NativeScalarValue.DateValue and NativeScalarKind.Date=5. The resulting proposed surface is55 types /408 rows. This is an explicit additive proposal, not zero API delta or public C45 acceptance.

Eighteen Core archive/runtime payload identities match their earlier signed qualification, and both Core archives are freshly signature-verified. [Identities](IDENTITY.json). The isolated Schema source project references only published Core packages; it is not a packaged Schema consumer qualification. Five source overlays plus two new files form the [candidate manifest](CANDIDATE-SOURCE.json). Original Git bytes, checkout transforms and candidate hashes are distinguished so the source is reconstructable without copying unrelated work.

## Limits and next step

Full Schema package consumers, retained binary callers, packaged examples and both frozen/expanded corpus pins were not rerun. No original upstream date admission or conformance percentage is claimed. All323 production inputs and60 earlier packets remain intact. No production API/source/test/Core pin, package or publication changes. The published beta and release controllers remain unchanged. No commit or push.

Next: separate production adoption of the reviewed bounded API and semantics, then full source/package/native/retained/API/example/corpus qualification. The original [18 native gates](../date-time-transport/NATIVE-GATES.json) remain historical; their [current scope review](NATIVE-GATE-REVIEW.json) distinguishes measured controls from remaining delivery gates. Full XSD1.1, PSVI/C45, calendar operations and higher-precision dateTime/time transport remain separate.
