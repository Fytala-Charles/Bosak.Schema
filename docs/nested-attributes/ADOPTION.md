# Nested container attribute semantic adoption

20 November 2026. Recorded BEFORE production edits under the owner's instruction
to continue the recommended feasibility/adoption step. Product base Schema
`3e067cd73bd779a4539cd14e870950b0bc99430b` remains unchanged. This selects bounded
implementation semantics; it is not runtime support, API/C45 acceptance, an
independent review, an official W3C-suite admission or a full-section claim.

## Exact immutable sources

Use the dated complete bodies pinned in [SPECIFICATION-REVIEW.json](SPECIFICATION-REVIEW.json):

| Source | SHA256 | Bounded clauses used |
|---|---|---|
| [XSD1.1 Structures, 5 April 2012](https://www.w3.org/TR/2012/REC-xmlschema11-1-20120405/) | 5ffb773f81fb81cfc9ac54dfb871c333cefb12fbdffe4caeb28170054b21fc6a | 3.2 attribute declarations;3.4.2-3.4.4 local complex-type assessment;3.5 attribute uses;4.2 component resolution |
| [XSD1.1 Datatypes, 5 April 2012](https://www.w3.org/TR/2012/REC-xmlschema11-2-20120405/) | cc2ae20cdeab02600c73fd13f1cc31dbbdd61f62aa455026fa84388b540443b8 | Existing adopted string/Boolean/integer/decimal and selected facet rules reused unchanged |
| [XML1.0 Fifth Edition, 26 November 2008](https://www.w3.org/TR/2008/REC-xml-20081126/) | f58a4b9e1e5b8bac13fb55c4e5c7c8c5e2c176aa45ee68e2f1ab2498511927ec | 2.11 line endings;3.3.3 attribute-value normalization;4.1 references |
| [XDM3.1, 21 March 2017](https://www.w3.org/TR/2017/REC-xpath-datamodel-31-20170321/) | a157e098f0de2fefd81ce95bb934762ff5a1f616a4745854f0340d177082acd3 | 2.3 identity;2.4 document order;2.7 schema information;6.2/6.3 element and attribute nodes |

These dated review pins and this semantic adoption remain separate records.
No moving latest specification or later errata is silently adopted. Prior XML,
Namespaces, datatype/facet, root attribute and nested-sequence adoptions stay
historical; no broader QName/type relation/datatype/assertion behavior is added.

## Exact bounded implementation profile

Adopt [CONTRACT.md](CONTRACT.md) and proposed[ADR014](../ADR-014-owned-descendant-attribute-paths.md):
required/optional local attributes on descendant named/anonymous element-only
complex containers, only through NativeSchemaSession. Reuse original scalar
families, named restrictions, form/default rules and representation/query limits.
Namespace declarations are skipped. Missing required/undeclared ordinary uses
on enabled owners or in-bound bad lexical/facet values are Invalid. Reserved
xml/xsi requests, capacity and unsupported shapes are Unsupported. Attribute-free
owners retain existing Unsupported/Attributes for application attributes.

No scalar-leaf attributes, nested simple content, defaults/fixed/inheritable/
prohibited, global refs/groups/wildcards, anonymous simple types, new datatype/
facet, mixed/choice/all/recursive/unbounded/derivation/composition/assertion work.
NativeDecimalSession and existing root/simple-content semantics remain unchanged.

Retain16uses per owner and all adopted nested component/depth/element/terminal
limits. Add64TOTAL potential attribute occurrences INCLUDING root and absent
optional uses/containers, multiplied by maximum finite occurrences. Count shared
plans for every potential instance, not once per type. Compile-time representation
capacity refuses excess even when the actual XML omits an optional branch. Eager
unused-definition checks, bounded arithmetic and cancellation remain required.
A16line/four-use plan reaches64; one extra root use reaches65and refuses. This
is an intentional engineering profile limit, not normative XSD validity.

## Original ownership, validation order and publication

Each captured attribute is bound to its original owner/node, local use, type and
assessment. Shared attribute-use/type components do not merge distinct instances.
Do not use root-only expanded-name lookup, same-authority membership alone or
an unauthenticated path as node identity. Before-code public-Core evidence in
[FEASIBILITY.md](FEASIBILITY.md) qualifies these distinctions, not native validity.

Keep existing root selection/attribute precedence. For each owner in preorder:
refuse reserved requests; assess present/required uses in schema order with scalar
capacity/lexical/facet rules; reject undeclared ordinary attributes; check existing
element-only text/markup; recursively assess sequence occurrences. Whole-document
success, including every descendant, precedes any fact/document publication.
Deep failure/cancellation must not leak facts or consume admission. Exact mixed
failure precedence must be covered by authored controls. Preserve old refusals
unless an exact intentional formerly unsupported shape is recorded separately.

## Three additive members, existing indexes unchanged

[SURFACE.json](SURFACE.json) proposes DescendantAttributeCount and
ReadDescendantAttribute on NativeSchemaDocument plus ImmutableArray<int>
OwnerElementPath on the existing producer-only NativeAttributeValue. No new type,
constructor, option, enum, selector syntax, live node or authority escape. Existing
52types/389rows remain; expected exact addition is three rows after implementation.
General owner API/C45 acceptance is not inferred from semantic adoption.

Keep root AttributeCount/ReadAttribute root-only in declaration order; root
OwnerElementPath is empty and nondefault. Keep ScalarCount/ReadScalar terminal-only
in document order; HasRootScalar/ReadRootScalar unchanged. Descendant attributes
are owner-preorder then schema declaration order, omitting absent optionals.
Each detached path contains zero-based ELEMENT-child indexes from the assessed
root, ignoring text/namespace nodes; under this element-only profile, descendant
attribute owners have at mosttwo segments so their potential children fit depth4.
Paths describe present XML, not schema/flattened scalar indexes; omission can shift
later paths. Equal paths across snapshots convey no authority or durable identity.

Store immutable paths without input/node/session references. Detached name/value/
path data remains usable after disposal; provenance and issued facts retain the
existing snapshot/owner guards. All gate/overlap/cache/query/export/cancellation/
revocation/quota/cleanup exceptions and recovery contracts remain. Export/reopen
reassesses rather than carrying old authority across.

## Before-code evidence and remaining work

Both signed public Core lanes pass69synthetic controls (19new/50inherited), with
18verified signatures/source/cache/runtime identities. Four actual native/API
baseline controls pass on3e067cd. These overlapping probes add no product cases.
No production implementation or API change has occurred at adoption. Next:
owner-aware native assessment/facts, bounded aggregate uses and detached path/read
implementation, then actual source/package/retained/API/README and documentation
qualification per[QUALIFICATION.md](QUALIFICATION.md). No publication or Core pin
change is authorized by this adoption.
