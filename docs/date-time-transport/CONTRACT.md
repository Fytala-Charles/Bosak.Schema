# Proposed bounded calendar contract

Status: design only, not accepted public API or adopted semantics. Normative basis is [the immutable source review](SPECIFICATION-REVIEW.json).

## First slice: date

Propose native xs:date at existing explicitly simple-typed root, supported scalar-member and supported attribute placements. Facet-free named/anonymous aliases may be admitted only through the existing derivation/compiler routes after native qualification. No implicit anyType, new particle grammar or arbitrary user-type chains is included. Compile unused declarations eagerly; unsupported facet/type syntax remains Unsupported rather than being ignored.

Validate the entire XML-whitespace-collapsed lexical form using ASCII digits, the XSD year grammar, valid Gregorian month/day and optional Z or signed hh:mm offset. Preserve year zero and astronomical negative years; compute leap validity directly from integer year divisibility. Do not use DateTimeOffset, DateOnly or current culture as the validator. Do not inject the assessment episode's timezone into a value whose timezone is absent. Date values retain their original offsets, including extreme offsets; lexical Z/+00:00/-00:00 may map to one explicit-zero offset while original XML remains unchanged.

Proposed representation bound: signed 64-bit year and at most 256 characters in the normalized date lexical form, in addition to existing document/source/work budgets. Otherwise-valid values outside those bounds are Unsupported, never rounded or reported invalid merely for exceeding representation. Invalid lexical/calendar values within bounds are Invalid. A capacity refusal may occur before full lexical classification; no universal syntax verdict is promised after a capacity limit is reached. Ordinary invalid instance dates publish no successful partial facts. No new public option or hidden machine setting is introduced.

The 37 [planned root payloads](CASE-PLAN.json) cover leap/century/year-zero rules, negative/extended and Int64-boundary years, timezone presence/limits, XML whitespace and malformed lexical forms. These are proposed expected outcomes, not execution evidence. Additional native controls must qualify existing placements and both compiler routes; this root-only data is not evidence for attributes or simple content.

## Public representation proposal

Append NativeScalarKind.Date = 5 without changing values1-4. Add NativeScalarValue.DateValue returning a sealed immutable NativeDateValue with long Year, int Month, int Day and nullable int TimezoneOffsetMinutes. Null means absent timezone; zero is an explicitly supplied UTC offset. Construction remains internal to checked assessment, with no public constructor and no invalid default struct state. Wrong-family access throws InvalidOperationException. A detached value has no origin, type handle, query authority or owner reference, and survives disposal. This is proposed surface only; API snapshot review and a consumer experiment must precede acceptance.

Represent date as a genuine Core Date atom with annotated actual type and the original declaration/governing/atomic provenance. Any internal placeholder clock fields required by Core's shared date/time carrier are implementation details, not new date components or a conversion to dateTime. Verify atom access, formatting and provider transport at long.MinValue/MaxValue; availability of a factory is insufficient. In particular, long.MinValue negation/absolute-value formatting is a known review boundary, not assumed safe. If any supported native path cannot preserve a proposed value exactly, stop feasibility and revise the bound or shared representation explicitly rather than fabricating values or broadening acceptance.

Original XML is retained through export; formatting a detached scalar is a separate future API question. No .NET conversion method, comparison interface, public equality semantics or canonical string API is added in this first proposal. Future value-space equality must be type-aware and distinct from representation identity and original lexical equality.

## Later dateTime/time contract

Use independent calendar components, timezone presence/offset and exact nonnegative decimal seconds. Preserve all admitted fractional digits under explicit digit/scale budgets; do not narrow them to milliseconds, ticks, double or fixed .NET decimal. A shared exact Core representation is a dependency whenever original-node atomization requires more precision than the current carrier supplies. A Schema-only DTO cannot repair a lossy Core fact. Any Core change needs its own adoption, compatibility and conformance gates before Schema qualification; no Core pin bump is implied here.

Settle end-of-day mapping (dateTime rolls to the following calendar day; time maps to midnight), year-boundary overflow, leap seconds, fractional-zero normalization, missing fields and xs:dateTimeStamp's required timezone explicitly. Do not invent a reference calendar date in detached time or treat date as a UTC midnight instant. Reject invalid forms with the exact XSD grammar; retain valid capacity refusals separately.

## Deferred comparison and facets

Schema date/time ordering is partial when timezone presence differs; never substitute the host or XPath implicit timezone to manufacture a total Schema order. Record D.2.1 comparison and equality rules before implementing enumeration, min/max, assertions or equality/hashing. Equality of values with different offsets does not imply representation identity. Builtin dateTimeStamp derives from dateTime with timezone required and needs genuine type provenance, not a string label. explicitTimezone, pattern, enumeration and range facets are separate scope. Duration and Gregorian fragments, lists/unions, full typed query, PSVI/public closure and general calendar arithmetic remain deferred.

## Admission gates

All 18 [native gates](NATIVE-GATES.json) are required before production adoption. Run package-only probes on signed Core1.81.2 and1.81.17, retain actual original-node facts, both public facades and both compiler routes. Preserve all original source stimuli and measure expected-outcome changes explicitly. Then qualify source, package consumers, native identities, retained callers, API differences, exact packaged examples and both frozen/expanded corpus pins. Never replace an original schema with a simplified case and claim its admission. Record immutable semantic adoption separately before changing production behavior.
