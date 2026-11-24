# Proposed date-enumeration and value-equality contract

Status: review only. Normative revision and exact affected sections are in SPECIFICATION-REVIEW.json. No implementation adoption is recorded here.

## Grammar and placement

Admit one or more enumeration children on an otherwise supported direct restriction of xs:date, named or locally anonymous. Preserve facet-free direct date aliases. Reuse named types at supported simple roots, terminals, local attributes and as the simple base of the delivered date simple-content extension. This does not add anonymous attribute-type syntax, new particles or general user-defined simple restriction chains. Restrictions based on a named alias or named enumeration type remain Unsupported in this slice, including a facet-free derived alias; inherited enumeration narrowing requires a separate contract. Complex simple-content restriction stays Unsupported and must retain its explicit date guard.

Only value is admitted on an enumeration element, with no child markup. Missing value, empty/invalid bounded date lexical values and impossible calendar days make the schema Invalid. Unsupported markup, annotation/fixed/id requests, other temporal facets and unsupported base/derivation shapes remain Unsupported. Compile unused definitions eagerly. A facet value must be datatype-valid for the declared base; do not silently discard bad members. A schema Invalid verdict applies only after unsupported shape/capacity screening for the supported profile; it is not a complete schema-for-schemas validator.

Multiple enumeration siblings form one alternative set. Duplicate lexical forms, explicit-zero spellings and equal-but-nonidentical values are legal. Count source entries before any deduplication. A restriction with no facets remains the existing facet-free alias; do not reinterpret it as an empty enumeration. An empty enumeration value is a supplied invalid date, not an absent facet.

## Equality and representation identity

XSD enumeration membership uses value equality or identity. For the supported date domain, the proposed internal predicate is:

1. Two values with absent timezones match exactly when their year/month/day properties match.
2. A value with an absent timezone never matches one with a supplied timezone. Do not supply an implicit timezone from the assessment context.
3. Two timezoned values match when the instants at the starts of their local days, adjusted by their signed timezone offsets, match. Different local calendar properties/offsets can therefore match.

The normative example 2000-01-01+13:00 versus 1999-12-31-11:00 must match while remaining distinct representations. Z, +00:00 and -00:00 map to the same explicit zero offset; an absent offset stays absent. Different dates with the same offset do not match. Same local day with different nonzero offsets does not match merely because their represented intervals overlap. Preserve symmetry and transitivity within timezone-presence classes; there is no equality by interval overlap.

Use exact Gregorian day arithmetic with mathematical floor division for negative years and arbitrary-precision intermediate day/minute keys. An internal equality key may use 1440 times a Gregorian ordinal minus the offset, on an arbitrary common origin. It must not convert through DateTime, DateTimeOffset, milliseconds, a host timezone or a bounded long day count. Treat year zero and negative Gregorian leap years according to XSD1.1. Intermediate UTC positions outside the representable local-year interval must still be comparable; do not construct an out-of-range carrier to compare them.

Equality is an internal validation operation. This proposal does not change NativeDateValue.Equals, XPathDateTime.Equals/GetHashCode, XPath comparison/ordering operators, query implicit-timezone rules, public APIs or public canonicalization. If equality keys are hashed internally, equal date values must share a key/hash without replacing the original parsed value.

## Typed values and ownership

Successful assessment retains the instance's local year/month/day and optional offset. Never replace an instance with the matching enumeration member or a UTC-normalized value. Element facts retain original D/T/G, with the actual restricted atomic type A. For simple-content extension G is complex and distinct from A. Anonymous types receive no invented QName. Attribute facts retain their original owner/name/type and value. Both public facades, snapshots and detached export preserve current behavior and authority boundaries.

Validate membership at every supported date-valued site: simple root, flat/nested terminal, root/descendant attribute and date simple-content extension content. Valid metadata cannot mask a failing enumeration and valid content cannot mask a bad attribute. Same-type xsi:type selection must enforce membership; no new substitution graph is added. Legacy NativeDecimalSession continues to refuse date types.

## Outcomes and limits

A supported well-formed member is Valid; a supported well-formed nonmember or invalid instance date is Invalid. Unsupported schemas, deferred requests and exhausted representation capacity remain Unsupported, with no partial successful facts. Cancellation, object lifetime, options errors and API exceptions stay distinct.

Propose a maximum of 16 source enumeration entries per restriction; the 17th is Unsupported/RepresentationCapacity even if every entry is a duplicate. This is a profile bound, not a new public option. Each facet's parsed value obeys the existing MaxStringCharacters UTF-16 budget and the 256-character collapsed date lexical budget. Schema input remains bounded at 32,768 UTF-8 bytes. Local years remain -9223372036854775807 through 9223372036854775807 inclusive; long.MinValue and larger magnitudes are Unsupported, not wrapped or rounded. Test source bytes, raw value characters and normalized characters separately. Existing node/depth/type/scalar/attribute/document/work quotas remain unchanged.

Temporal pattern, bounds, explicitTimezone, assertions, dateTime/time/dateTimeStamp, complex restrictions, user-defined simple derivation chains, lists/unions, mixed/open content, defaults/fixed/nil, assembly and general typed-query/PSVI/C45 closure remain separate. No date ordering is manufactured by adding enumeration.

## Acceptance and original stimuli

CASE-PLAN.json contains authored proposed schema/instance outcomes, all unexecuted. Run through both facades using Core 1.81.17 and at least implicit timezones -840, 0 and +840 minutes for timezone-sensitive controls. Include actual original-node typed facts, attributes, equal-but-nonidentical values, snapshots/export, cross-session refusal, disposal and cancellation. NATIVE-GATES.json records obligations rather than passed tests.

Preserve all 4,442 baseline source names/payloads. Before reconciling any expected refusal, capture the unchanged-suite results; retain every failure and separately explain any changed expected outcome, renamed display or newly eligible theory. Compare the public API with the qualified 55-type/408-row surface; zero delta is a target, not a measurement here.

The two original public enumeration examples keep exact XML, schema bytes, IDs and upstream validities. Their instances contain comments, which remain outside the native profile. The empty-value case also contains an invalid date. Do not promise two new corpus matches or change comment handling to obtain admissions; measure actual first refusal without treating Unsupported as an upstream invalidity verdict. Retain literal location hints without file/network acquisition. Any comment-free authored control must be identified separately from the originals.

Qualification sequence: separate experimental adoption; isolated overlays and unchanged-suite comparison; native ownership/API/package/retained caller checks on signed 1.81.17; exact examples and unchanged frozen/expanded corpora; measured impact reconciliation; separate production adoption if justified. Existing Core baseline changes remain a distinct uncommitted dependency change.
