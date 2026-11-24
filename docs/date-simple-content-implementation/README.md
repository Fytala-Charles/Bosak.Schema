# Bounded date simple-content production qualification

24 November 2026; base383914562d89d3f7fad3a6a60cf4e29884cce912. [Production adoption](PRODUCTION-ADOPTION.json) precedes three source overlays. Earlier [assessment](../post-date-coverage/README.md), [contract](../date-simple-content/README.md) and [isolated feasibility](../date-simple-content-feasibility/README.md) keep their original states and seals.

## Behavior

The opt-in native facades now assess simpleContent/extension of xs:date or an admitted direct facet-free date alias at existing named/anonymous complex roots and terminal placements. Existing local attribute-use grammar, date limits, namespace/form and representation budgets apply. Actual original-node facts preserve declaration D, declared/effective complex T/G and distinct atomic date/alias content type A. Public reads use the existing NativeScalarKind.Date and immutable detached NativeDateValue; there is no new API.

The date-extension attribute model is closed even at zero declarations: undeclared ordinary attributes are Invalid, as are missing required or invalid typed values. Namespace declarations, previously admitted hints/type requests and deferred xsi/xml special requests retain their existing distinctions. The older scalar/decimal empty-use path is unchanged. Date complex restriction is explicitly refused before the noninteger decimal facet fallback. Temporal facets/assertions, complex chains, mixed content, default/fixed/wildcards, general simple-to-complex xsi:type and dateTime/time remain separate. Exact same complex xsi:type works.

The existing date interval[-long.MaxValue,+long.MaxValue] and256 normalized lexical characters remain. No DateTimeOffset conversion or implicit timezone substitution is introduced. A simple-content root has one root scalar and zero descendant slots; containing element-only roots keep their absent typed value/FOTY0012 behavior. Attribute indexing and original owner paths retain existing contracts; export preserves parsed physical content. Failed assessments publish no partial facts or snapshot. Foreign authority and disposal rules remain enforced; detached date/attribute values survive disposal.

## Qualification

**4,442 source tests pass:1,345 internal /3,097 public. Package consumers pass3,097 tests per signed Core pin.** Current-package actual-node controls37 and retained callers124/23/33 pass. All4,246 prior source names/stimuli remain with no removal, rename or newly eligible prior theory;196 date-extension controls are added (159public/37internal). The one measured semantic factory expectation changes Unsupported to Valid and disposes its session, retaining its exact original payload and every other temporal exclusion. [Original measured reconciliation](ADMISSIONS.json), [exact source/test inputs](PRODUCTION-INPUTS.json) and [counts](SUMMARY.json).

The current package consumer lane retains its existing explicit1.81.17 assertion; production source retains the minimum1.81.2 assertion. No arbitrary pin assertion is weakened. API remains **55types /408rows, zero delta**. [API comparison](SURFACE.json). Eighteen signed Core archives/source pins/runtime payloads, unchanged signed published beta.1, local package/cache/source DLL consistency and retained copies with only Schema.dll replaced verify. [Identities](IDENTITY-FINAL.json). Normal advisory checks remain enabled; no warning/audit suppression or machine error-reporting change.

All five exact packaged examples pass: date extension with typed metadata, primitive date, integer enumeration, all-group and unsignedLong. The new extension example checks both facades, year zero and absent timezone under a nonzero implicit context, required/extra attributes, bad calendars, parsed export and detached values. [Executable example](readme-example/Program.cs). Final review removed a stale earlier README phrase excluding date extension; the local package was repacked without rebuilding its identical DLL, then qualified using fresh caches/runners and final corpus receipts. Initial package identity is retained separately. [Document-only package revision](PACKAGE-README-REVISION.json).

## Corpus effects

Both pins retain **21 frozen pilot matches**, zero mismatches/Unsupported/harness errors. The expanded129-case sample is now **14matches /115Unsupported /zero errors**, up from12matches. Exactly two original CType/contenttype00201m instances change: _p compiles/assesses Valid, _n compiles Valid and assesses Invalid, matching unchanged upstream expectations. Every other expanded detail row is unchanged, including mixed content, date facets and earlier wildcard refusal. Cohort2 has no row changes. [Exact before/after rows](SUMMARY.json).

Original source manifests and upstream expectations remain unchanged. Expanded scripts retain their historical22November date field; these executions occurred24November2026. This selected sample is not a specification denominator or full conformance percentage. Full typed query, public closure/PSVI/C45, general calendar operations and performance acceptance remain separate.

## Delivery and evidence

Local unsigned Bosak.Schema.0.6.0-alpha.0.43 only. Package SHA256 60e571cfdbb78191bd8a4943a3678a969fe6be27ae597c1a4e83e98e6db2fca5; Schema DLL SHA256 04e15c8e1325a6c3b51e48a52ac048660854f8248724a3895de5066bb9da9db3. Published beta, Core pins and release controllers remain unchanged. No commit/push/tag/publication.

The [source manifest](CANDIDATE-SOURCE.json) pins63 source files and three overlays; qualified bodies match the isolated candidate with production header histories. [Production inputs](PRODUCTION-INPUTS.json) cover338 files (six changed existing, ten new). Original XML fixtures retain their permission/attribution record. Earlier65 packets/4251 entries remain intact. Documentation checks are programmatic, not visual acceptance.

Next: explicit Git delivery request, PR review and merge/main verification, then a separate date-facet contract beginning with value-space enumeration/equality. Exact-seconds Core transport remains a distinct broader foundation. No additional scope is bundled into this candidate.
