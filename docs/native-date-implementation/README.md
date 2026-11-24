# Bounded native date production qualification

24 November 2026. Candidate based on merged main2eb999052188a872a54548c71c378d6a05a8cc3c. [Production adoption](PRODUCTION-ADOPTION.json) was recorded before production source edits. The [original transport proposal](../date-time-transport/README.md), [formatter-boundary preflight](../date-transport-preflight/README.md) and [isolated native feasibility](../native-date-feasibility/README.md) retain their original receipts. [ADR019](../ADR-019-bounded-native-date-adoption.md) records this bounded adoption without changing historical ADR018.

## Behavior and limits

The opt-in native facades assess xs:date at existing simple roots, scalar members and attributes, including direct facet-free named/anonymous date restrictions and supported xsi:type selection. NativeScalarKind.Date=5 exposes an immutable NativeDateValue with Year, Month, Day and nullable TimezoneOffsetMinutes; null means absent, zero means explicit UTC. Detached values carry no schema authority. Actual original-node date atoms retain declaration/governing/atomic provenance, ownership and existing lifetime rules.

Gregorian validation supports year zero, negative and extended years directly, without DateTimeOffset or implicit timezone substitution. The explicit interval is [-9223372036854775807,9223372036854775807] and the normalized lexical limit is256 characters, alongside existing source/document/work limits. Otherwise-valid capacity excess is Unsupported/RepresentationCapacity; bounded lexical/calendar errors are Invalid. The one excluded long.MinValue year has a measured Core formatting overflow, retained in its original preflight. Original DT-013 remains unchanged in the sealed original proposal; the qualified revised plan explicitly changes its expected outcome. No rounding or fabricated dateTime/string atom is used.

Temporal facets, date complex simple content, arbitrary type chains, temporal assertions, dateTime/time/dateTimeStamp, Gregorian fragments and durations remain separate. Structural operations already admitted by the existing profile retain their original scope. General calendar comparisons/arithmetic, full typed queries, public PSVI/C45 and full XSD1.1 conformance are not claimed.

## Qualification

**4,246 source tests pass:1,308 internal /2,938 public. Package-only consumers pass2,938 tests per signed Core pin.** Seven current-package actual-node identity controls and retained callers124/23/33 pass. All4,146 previous logical cases and their original stimuli remain;99 date controls are added, plus one newly eligible original AL-008 identity case. One expected-outcome display is explicitly renamed Unsupported to Invalid. The [summary](SUMMARY.json) records all added/removed display names. These lanes overlap and must not be added into a conformance total.

Nine measured earlier semantic failures are reconciled through seven fixture/harness changes, with original XML, schemas, IDs and upstream expected validities preserved. [Original measured reconciliation](ADMISSIONS.json) remains historical; [exact production before/after identities](PRODUCTION-INPUTS.json) distinguish proper production header histories and line-ending normalization from the isolated copies. The production minimum-Core assertion remains unchanged; the current package lane retains its explicit selected-Core version check. No test is silently deleted or a negative stimulus replaced. The existing surface diagnostic test retains its admitted date stimulus and separately exercises a deferred-time refusal.

[API comparison](SURFACE.json) preserves all401 prior rows and adds exactly seven, yielding55 types /408 rows: one sealed date type, four readonly properties, the scalar DateValue accessor and Date enum member. [Package identity verification](IDENTITY-FINAL.json) verifies18 signed Core archives, exact source pins and runtime payloads, the local package/cache/source DLL identity, retained copies with only Schema.dll changed, and unchanged signed published beta.1. Normal NuGet advisory checks remain enabled; no audit/warning suppression or machine error-reporting change is introduced.

Exact packaged date, integer-enumeration, all-group and unsignedLong examples pass both facades, export and detached lifetime. The date example also covers year zero, an extended year, timezone presence/absence, invalid calendar input and excluded-year capacity. [Executable example](readme-example/Program.cs) uses the exact packaged XML blocks. Three initial package-lane invocations used an unsupported CLI cache switch; the retained logs identify that harness-only failure, corrected by the MSBuild cache property without weakening policy. Final source/consumer receipts follow normalization of inherited test-copy line endings; earlier passing executions are retained separately, not counted additively.

## Measured corpus effects

On both pins, the frozen21-case pilot is unchanged:21matches, zero mismatches/Unsupported/harness errors. The expanded129-case sample is now **12matches /117Unsupported /zero errors**, up from10matches. Exactly two unchanged originals are admitted: CType/contenttype00301m/contentType00301m1_p matches its valid expectation, and contentType00301m1_n matches its invalid expectation. Wild/wild080/wild080.v1.xml stays Unsupported, with first refusal moving ValueType to DeclarationShape. Every other expanded detail row is unchanged; cohort2 has no row changes. [Exact before/after rows](SUMMARY.json).

Original manifest hashes and upstream expectations remain unchanged. The unchanged expanded measurement scripts retain their historical22November date field; the executions occurred24November2026. These samples are not a specification denominator or full-suite conformance percentage. No stronger dateTime/time/PSVI or performance conclusion follows from the two admissions.

## Delivery identity and next step

Local unsigned Bosak.Schema.0.6.0-alpha.0.40 only. Package SHA256: c56ec62d172373e9db553833eb2723b5b1d6b70ddbfb84e1f846dd9fda0d53bc. Schema DLL SHA256: 31f964170c0674b495601f146763b28efba3fed06e1f0e5871f512d5a5c09d7a. Published beta, Core pins, controllers and public releases remain unchanged. No commit/push/tag/publication.

The [source manifest](CANDIDATE-SOURCE.json) contains63 files with seven overlays/new files, keeping immutable base Git bytes distinct from qualified Windows checkout bytes. [Production input manifest](PRODUCTION-INPUTS.json) has328 inputs:14 changed existing and five new. Earlier61 packets/3974 entries remain intact. New documentation render checks are programmatic; no visual acceptance is claimed.

Next: explicit Git delivery request, PR review and merge/main verification, then reassess temporal facets against exact-seconds Core transport design using the updated sample. No further semantic scope is bundled into this qualification.
