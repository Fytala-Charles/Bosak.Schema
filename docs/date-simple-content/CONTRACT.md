# Proposed date simple-content semantics and limits

Status: contract review only. [Source pins and exact affected specification sections](SPECIFICATION-REVIEW.json) are not an implementation adoption record. Every XML plan is unexecuted. Limits and exclusions describe this proposed native profile, not limits of XSD1.1.

## Grammar and placement

Admit one simpleContent containing one extension with base xs:date or an existing direct facet-free named date alias. Admit named/anonymous complex document roots and named/anonymous terminal complex elements within the existing bounded sequence grammar. Shared named types and forward simple-alias declarations retain their identities. No complex base extension chain, complex restriction, temporal facet/assertion, mixed/open content, wildcard, attributeGroup, default/fixed value, nil or new particle grammar is included. Attributes use current local name/type/use/form syntax and currently admitted scalar types; no inline attribute type or attribute-use inheritance is added.

An unused definition is compiled eagerly. A reachable date complex restriction, even with no facets and even if unused by the instance, must be refused before the existing noninteger decimal facet branch. Recognizing date extension cannot turn that fallback into date validation. Other temporal extension bases remain Unsupported. The legacy decimal facade still refuses date schemas.

## Declaration and typed-value roles

| Role | Required meaning |
|---|---|
| D | Original element declaration from the compiled schema |
| T | Type named or anonymously defined by D |
| G | Effective governing complex type; equals T for this slice's ordinary/exact-type selection |
| A | Simple date content type, either primitive xs:date or the admitted direct date alias |

Keep G and A distinct. The element has a singleton Date typed value, even though G is complex. Facts retain D/T/G; atomic evidence carries A and the original assessment origin. An anonymous G has no invented global name. Shared C across multiple declarations shares only its real compiled type component, never the declarations, original nodes or foreign-session authority.

Both public facades expose the existing detached DateValue. A simple-content root has HasRootScalar=true and zero descendant scalar slots; its representation budget still includes its one typed-value site. A containing element-only root has no root scalar and retains FOTY0012 for its absent typed value; date terminals are accessed by existing scalar indexes/element paths. Attribute indexes follow the existing declaration-use contract, owner paths refer to original elements, and export retains original parsed physical structure rather than reconstructing it from declaration order.

## Dates and attributes

Reuse the adopted Gregorian date validation, XML whitespace collapse, timezone absence/explicit-zero distinction and existing date interval. Do not convert through DateTimeOffset, infer timezone from the assessment context or change original parsed lexical text. Bounds and dateTime/time precision remain separate work.

For the new date-extension model, the admitted local attribute closure is complete even when empty: an undeclared ordinary application attribute is Invalid, as are missing required or invalid typed attributes. This is a scoped proposal; do not alter older scalar/decimal empty-use refusal behavior. Namespace declarations, admitted xsi:type and opaque location hints are not application attribute uses. Other xsi/xml special requests remain Unsupported. Interpret namespaces by expanded names, never literal prefix spelling. No location hint causes file/network acquisition.

Required/optional and qualified/unqualified uses retain current semantics. Attribute values can be checked strings, exact numbers, Boolean or existing date types, with existing facets where already supported. Additional date facets are not authorized. Valid date content cannot hide a bad attribute, and an attribute value cannot replace or alter the element's date typed value.

## Outcomes

- Valid requires supported compilation, valid date content and every admitted attribute use.
- Invalid covers bounded date lexical/calendar errors, required/typed/undeclared ordinary attribute failures and child elements in simple content.
- Unsupported covers representation/work capacity, unsupported grammar/types/derivations, deferred special requests, and comment/PI content outside the current native profile. It is not a validity verdict for all unimplemented schemas.
- Cancellation, invalid API arguments, closed objects and quota exceptions remain distinct from input outcomes. Failed inputs expose no successful partial facts or snapshot.

Exact same named complex xsi:type is in scope. Requesting its base simple date or an unrelated/absent type is Invalid. A valid derivation from a declared simple date to complex date extension remains Unsupported under the existing representation-route guard; it must not be misclassified as Invalid or accepted merely because graph reachability is true. No general complex substitution is added.

## Existing capacities retained

| Capacity | Proposed retained bound |
|---|---:|
| Schema UTF-8 input | 32,768 bytes, with existing XML parser safeguards |
| Global user type definitions / root declarations | 16 each |
| Local declarations / maximum occurrence per particle | 16 each |
| Local attribute uses per owner | 16 |
| Expanded representation nodes | 64 |
| Expanded typed scalar sites | 16 |
| Expanded element depth, counting root | 4 |
| Expanded attribute uses | 64 |
| Date year interval | -9223372036854775807 through 9223372036854775807 |
| Normalized date lexical form | 256 characters |

Source/expanded budgets use declared maxima, not only the supplied instance. Caller-supplied document/export/query/cache/open-document limits remain mandatory and unchanged; no new public option is introduced. UTF-8 source bytes and date lexical characters are distinct measures. The plan isolates16/17 attribute/scalar/type and64/65 expanded-node/attribute boundaries, plus depth4/5. Empty complex siblings exercise the node budget without inventing extra date scalar sites. Capacity may be refused before complete validation beyond that capacity.

## Qualification and original stimuli

Run all77 planned schema/XML rows through both public facades with a fixed assessment context whose implicit timezone is+120 minutes. Attribute/timezone/date facts must preserve their explicit/absent values regardless of that context. Plans include named/anonymous roots, alias bases, flat/nested terminals, shared types with distinct declarations, qualified names, selection boundaries and eager unused exclusions. These are authored proposed outcomes, not a promise of execution counts, compiler feasibility or original admissions.

All18 [native gates](NATIVE-GATES.json) require real native inputs, authority-owned facts and operations on signed Core1.81.2 and1.81.17 packages. No synthetic validator/provider or rewritten schema substitutes for the original-node route. Measure public API comparison; zero delta is the target, not an already measured result. Keep full typed-query/public closure/PSVI/C45 and performance acceptance separate.

Preserve every current source payload and test ID. The existing DeferredTypes fact's date extension refusal is a potential measured expectation change; retain its original factory payload and other temporal exclusions. Capture the initial unchanged-suite outcome before any reconciliation, retain failures and report any display rename/newly eligible theory. The two original CType00201m trials remain exact separate inputs with original valid/invalid expectations. Mixed CType00401m, enumeration and maxExclusive cases retain their deferred obligations.

Sequence: review this contract, record separate experimental adoption before isolated source edits, run native feasibility and measured regressions, then separate production adoption and full source/package/native/retained/API/exact-example/corpus qualification. No commit/push without an explicit owner request.
