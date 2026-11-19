# M2B proposal: named decimal restrictions and value-space facets

19 November 2026. Definition only, not adoption or implementation. Schema baseline
`6861c04d6c62721f3622e0024960be2ef9578bca` (merged M2A PR18); Core baseline
`43935c34b7658a0365e3eecbef7815a950ce2d15`. The named complex type increment is
now on main. This proposal keeps REQ-009/CoreREQ-142 In Progress and changes no
runtime, Core/public dependency pin, release controller, package or tag.

## Application target and proposed grammar

Extend the existing authored decimal order/tax fixture with reusable `MoneyType`:

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
           xmlns:t="urn:orders" targetNamespace="urn:orders"
           elementFormDefault="qualified">
  <xs:simpleType name="MoneyType">
    <xs:restriction base="xs:decimal">
      <xs:minInclusive value="0"/>
      <xs:totalDigits value="12"/>
      <xs:fractionDigits value="2"/>
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name="OrderType">
    <xs:sequence>
      <xs:element name="amount" type="t:MoneyType" minOccurs="0" maxOccurs="2"/>
      <xs:element name="tax" type="xs:decimal"/>
    </xs:sequence>
    <xs:assert test="count(t:amount) eq 2 and t:tax gt 0"/>
  </xs:complexType>
  <xs:element name="order" type="t:OrderType"/>
</xs:schema>
```

The [proposed instance](order.xml) has decimal values12.5,2,1. Both amount
occurrences must meet the declared type; tax remains primitive decimal.
The fixture is derived from [the existing authored sample](../../tests/TestData/Schemas/native-decimal.xsd),
not an identified production application or an official W3C case. The displayed
schema is retained exactly as [money-order.xsd](money-order.xsd). The broader
orders/sku examples remain unsupported pending datatypes, facets and attributes.

Propose global named atomic simple types restricted **directly from xs:decimal**,
referenced by local leaf type QNames. Permit zero-to-six selected facets:
minInclusive, minExclusive, maxInclusive, maxExclusive, totalDigits and
fractionDigits; each at most once. Lower inclusive/exclusive facets are mutually
exclusive; likewise upper facets. Preserve anonymous/named complex roots, finite
sequence/assertions and all prior admitted schemas. Resolve every QName in its
actual namespace/default context, support forward references and check unused
types eagerly. Simple and complex types share the type symbol space; global
elements retain their separate space.

Proposed capacities: existing16global types becomes a combined simple+complex
ceiling, with16roots unchanged; selected facets are bounded by the six names,
schema byte/node ceilings and explicit decimal representation limits. Do not
reuse XSD facets as parser or allocation budgets. Record exact facet-integer
parsing bounds and numerical comparison allocations before adoption.

## Normative sources and semantic decisions before code

The [review receipts](SPECIFICATION-REVIEW.json) freshly match the original dated
5April2012 Structures and Datatypes body hashes. They are review evidence only;
M2B must receive a separate exact adoption record before production edits.
Historical M1/M2A and original decimal adoptions stay sealed. Do not incorporate
moving latest-version text or later errata silently.

| Source and sections | Requirement to adopt and test |
|---|---|
| [Structures](https://www.w3.org/TR/2012/REC-xmlschema11-1-20120405/),2.6,3.3.2-3.3.4,3.16,4.2/src-resolve | Named simple components, shared type symbol space, QName resolution and complete assembly |
| [Datatypes](https://www.w3.org/TR/2012/REC-xmlschema11-2-20120405/),3.3.3,4.1,4.2,4.3.6 | Decimal value/lexical mapping, atomic restriction and fixed collapse whitespace |
| Datatypes4.3.7-4.3.10 | Inclusive/exclusive value bounds and schema-component consistency |
| Datatypes4.3.11-4.3.12 | totalDigits/fractionDigits validation equations and facet consistency |
| Existing XDM3.1 adoption,2.7,3.3,6.2 | Original named type annotations/provenance and exact atomic values |

Evaluate facets in the decimal value space, with exact coefficient/scale math;
never use System.Decimal or floating-point rounding. `12.5000` and `12.5` have
the same facet result when representation capacities admit both. Separate
positive/negative zero, leading/trailing zeros, pure fractional leading zeros,
large integer magnitudes and equality at inclusive/exclusive bounds. Derive
expected totalDigits results from4.3.11.3's equation, not a guessed count of
characters or normalized coefficient digits alone. Cover0,0.001,1000 and12.5000
explicitly before implementation.

Representation capacity remains a separate earlier gate: excessive original
lexical digits/scale can be Unsupported even for a value that would satisfy a
facet. For admitted representations, supported facet violations are Invalid.
Validate facet schema components before any instance assessment: bound lexical
validity, positive totalDigits, nonnegative fractionDigits, fractionDigits not
greater than totalDigits, duplicate/conflicting facet declarations and normative
bound consistency. Do not invent a blanket rule for empty value spaces: use the
exact constraints in the adopted clauses and record any stricter profile refusal.
All present repeated leaves must be checked; absent optional leaves do not
manufacture values or facet violations.

## Outcome and compatibility contract to finalize

Proposed compile refusal: unknown/malformed type or base QNames, unsupported
base/facets and inconsistent facet components use the current Unsupported path
without an owned session. Define exact detected reasons before code; candidate
mapping is ValueType for invalid base/facet semantics, DeclarationShape for
component shape/duplicates, Attributes for excluded attributes, and
RepresentationCapacity for explicit ceilings. Preserve existing stage codes,
messages, enum symbols and prior refusal ordering outside newly admitted paths.
Malformed XML keeps existing Invalid behavior. No fallback validator or partial
universe is allowed.

Named restriction facts/leaf annotations and declared/actual atomic provenance
must retain the named component identity; underlying numeric values remain exact
decimals. Do not label every named leaf as primitive xs:decimal merely because
its storage is decimal. ResolveType must return the original named handle;
shared use has equal identity, separately compiled equal schemas never do.
Root complex types still have no typed value/FOTY0012. Prove transport through
actual Core snapshot/read/evaluate APIs; derived-type relations, schema-aware
query syntax and cross-universe handoff remain separate unless independently
implemented and qualified. Use current Core APIs first; a reproduced neutral
capability gap must be filed under CoreREQ-142 before an engine change.

## Explicit exclusions

No restriction chains/user-defined bases, anonymous simple types, extension,
fixed/redefined facets, enumeration/pattern/length/assertion facets, list/union,
new primitive datatypes, simple-content/root scalar documents, attributes,
imports/includes, xsi:type/nil, defaults/fixed instance values, nested content or
arbitrary XPath. Decimal whitespace remains inherited fixed collapse; an explicit
whiteSpace facet can remain refused in this first slice. These are intentional
profile restrictions, not claims that XSD forbids these constructs.

## Acceptance and delivery

The [planned acceptance catalog](ACCEPTANCE.json) distinguishes future positive,
instance-invalid, compile-refusal, capacity and actual-Core controls. No case is
executed or counted as support yet. First reproduce named-simple-type refusal on
this merged baseline, then pin exact fixture bytes and per-case clauses before
code. Select official W3C cases against a freshly fetched suite revision and
review redistribution terms separately; authored fixtures cannot replace that
qualification or establish a whole-spec completion percentage.

After implementation: build/test the complete Schema solution; preserve all465
existing source cases; qualify minimum/current published-Core package consumers,
retained124/23/33 callers, original declared/actual type identity, exact decimal
values, export/reopen and foreign/disposed state. Keep overlapping catalog totals
separate. Record package/archive/cache/runtime/source hashes and distinguish
local unsigned qualification artifacts from releases. Engine changes additionally
need the existing Core gates. Documentation and evidence checks complete the
slice; no publication follows automatically.

Follow-on order after M2B: coherent string/boolean/integer datatypes and selected
facets, then required/optional attributes against a fuller order/invoice workflow.
Each needs its own adoption and paired validation/transport evidence. This
proposal alone adds no API, product behavior or passed-test count.
