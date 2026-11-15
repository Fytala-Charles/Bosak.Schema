# Native decimal specification adoption

> Semantic implementation basis for schema-native-decimal-session-v1, recorded before backend implementation.

## 1. Identity and status

Date: 2026-11-15. Scope: the proposed [native decimal profile](NATIVE-SESSION-PROFILE.md), under REQ-009 and proposed [ADR-009](ADR-009-native-decimal-session-profile.md). This record fixes the intended bounded semantics; implementation, all P01–P12 acceptance groups and conformance remain pending. It changes no current schema validation behavior.

[Source identities](NATIVE-DECIMAL-SPECIFICATION-SOURCES.json) record dated W3C publication URLs, retrieval date, SHA-256 and byte length of the downloaded HTML response bodies. Hashes bind the inspected bytes, including editorial updates in those dated publications; a dated URL alone is not a byte-immutability guarantee. Full HTML files remain in ignored local output, not redistributed as repository source. On re-fetch, compare hashes and review any difference before changing this basis. No automatic incorporation of later errata or moving latest-version URLs is authorized by this record.

## 2. Adopted sections

| Source | Sections | Bounded implementation use |
|---|---|---|
| [XSD 1.1 Structures, Recommendation 5 April 2012](https://www.w3.org/TR/2012/REC-xmlschema11-1-20120405/) | 3.3 element declarations, 3.4 complex types, 3.8 model groups, 3.9 particles, 3.13 assertions | Anonymous element-only root, decimal local declarations, ordered finite sequence occurrences, complete assessment before publishing Valid, assertion-specific context |
| [XSD 1.1 Datatypes, Recommendation 5 April 2012](https://www.w3.org/TR/2012/REC-xmlschema11-2-20120405/) | 3.3.3 decimal, 4.3.6 whiteSpace | Decimal lexical/value mapping and XML-whitespace collapse; exact equality/order without rounding |
| [XPath 2.0, Second Edition, 14 December 2010](https://www.w3.org/TR/2010/REC-xpath20-20101214/) | 2.4.2 atomization, 2.4.3 effective Boolean value, 3.2 path expressions, 3.5.1 value comparisons, 3.6 logical expressions | Child operands, empty/singleton/multiple-item distinctions, six value comparisons, and/or with XPath precedence and grouping |
| [Functions and Operators 2.0, Second Edition](https://www.w3.org/TR/2010/REC-xpath-functions-20101214/) | fn:count, fn:exists, fn:empty, fn:boolean, numeric equality/order | Count and presence predicates plus effective Boolean conversion; no general function library admission |
| [XDM 2.0, Second Edition](https://www.w3.org/TR/2010/REC-xpath-datamodel-20101214/) | 3.3 PSVI construction; 6.2 element nodes | Assertion-local typed child view and element-only root with no typed value |
| [XDM 3.1, Recommendation 21 March 2017](https://www.w3.org/TR/2017/REC-xpath-datamodel-31-20170321/) | 2.7 schema information, 3.3 PSVI construction, 6.2 element nodes | Session transport of original declared/actual type and scalar value in the actual Core XPath 3.1 host environment |

The assertion language remains XPath 2.0 within XSD's assertion context. The external session reports XPath 3.1 and the dated XDM 3.1 revision above; that does not change assertion grammar, XSD language selection or component authority. The probe uses the engine-issued environment as-is and does not certify an assertion evaluator against it.

## 3. Explicit semantic rules for the backend

1. Completely parse the admitted schema and assertion grammar. Refuse constructs outside the profile; never drop them or fall back to an XSD 1.0 validator. Supported instance structural/lexical violations are Invalid. Missing coverage or insufficient numeric representation is Unsupported. Cancellation throws. Only complete Valid may expose original validated state.
2. Keep particle bounds on declarations and states only for present instance nodes. Distinct repeated children each retain their declaration and scalar decimal type; missing optional children create no placeholder state. Enforce sequence order as well as counts.
3. Collapse only XML whitespace for decimal lexical validation. Reject exponent, INF, NaN and non-decimal syntax as Invalid for a supported decimal leaf. Bound lexical length and supplied coefficient digits/scale before normalization. A valid lexical decimal that exceeds the qualified representation is Unsupported; never round, overflow silently or convert through binary floating point. Preserve exact value rather than lexical spelling; canonical XSD serialization is outside this profile.
4. Assertion evaluation uses the assessed root as the parentless context element with the selected local namespace context and typed decimal child values. For value comparison, an absent child operand produces an empty result; final assertion effective Boolean conversion makes that false. More than one selected child for a scalar comparison raises a type error and fails the assertion; do not select the first item or substitute a general comparison. count/exists/empty consume the bounded child sequence, so repeated and absent children remain meaningful.
5. A supported false assertion or dynamic/type error makes assessment Invalid. Unknown syntax/operands/functions indicate unsupported coverage. This distinction needs separate tests. Choose and document deterministic logical evaluation order; never turn an observed assertion error into success via an implementation shortcut.
6. The element-only root has no typed value, distinct from nil, an absent value and an empty typed list. Supported leaves are never-nil scalar decimals. Retain original root/leaf facts and original declaration/type objects independently of exported XML, source labels and snapshot labels.
7. Binding is opt-in and tied to the exact engine semantic environment. Issuance checks must authenticate actual input/origin/node/container associations. Session `$value` transport creates no structural proof; the backend must establish that proof first.

## 4. Coverage restrictions and deferred decisions

The grammar and numeric/parser ceilings in the profile are implementation restrictions, not W3C requirements. Full XSD 1.1, imports/includes, attributes, named user types, facets, CTA, nil/default/fixed, lists/unions, arbitrary XPath and cross-universe proof remain outside this adoption slice. Refusals must be observable and independently tested. Do not claim language conformance from successful supported examples.

The [executable grammar and counting rules](NATIVE-DECIMAL-GRAMMAR.md) were finalized before the internal backend was coded. The [backend receipt](native-decimal-backend/2026-11-15/README.md) records its 76 tests. No historical parser code was imported; this implementation uses the recorded adopted semantics. A future source reuse needs provenance review and preserved legal notices. The historical specification source manifest is unchanged.

Before provider qualification, test P01/P02/P05/P08 against the semantic rules here, then complete the identity/environment/lifetime groups on the exact candidate. This record and a passing scalar test fixture do not satisfy those groups or grant API freeze, performance acceptance or release approval.

*Last updated: 2026-11-15*
