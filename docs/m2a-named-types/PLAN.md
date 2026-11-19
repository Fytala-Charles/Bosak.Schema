# M2A proposal: named complex types for decimal documents

19 November 2026. Planning only; no new grammar or conformance claim. Baseline
Schema `0d9e72ae93fe08920868fe38897beb38c1a237c1`, Core
`43935c34b7658a0365e3eecbef7815a950ce2d15`. M1 merged in Schema PR17 and
Core PR504; package guidance reconciled in Schema PR16. REQ-009/CoreREQ-142
remain in progress. Published beta.1 is unchanged.

## Useful next step

Derive a namespaced order/tax example from the existing public
[decimal fixture](../../tests/TestData/Schemas/native-decimal.xsd). Move its
anonymous root complex type into a global `OrderType`; use
`type="tns:OrderType"` on the root declaration. Keep decimal leaves, finite
sequence/occurrence limits and the existing assertion grammar. Include a
second root sharing the same type to demonstrate actual component reuse.
This is one part of M2. The broader public orders/sku examples still require
attributes, strings/facets and other excluded structures.

## Specification preparation before code

Review XSD1.1 Structures, immutable 5 April 2012 Recommendation, sections2.6,
3.3.2-3.3.4,3.4.1-3.4.4 and4.2, including component name resolution
(src-resolve). Existing body receipt: SHA256
`5ffb773f81fb81cfc9ac54dfb871c333cefb12fbdffe4caeb28170054b21fc6a` in
[M1 sources](../m1-namespaces/SPECIFICATION-SOURCES.json).
[Normative source](https://www.w3.org/TR/2012/REC-xmlschema11-1-20120405/).
M1 XPath/Namespaces adoptions remain applicable to unchanged assertions.
This is a review target, not semantic adoption. Before implementation record
clause-level mappings, reverify the downloaded body hash and approve the exact
bounded grammar, resource limits and outcome contract in a separate adoption
record. Keep original adoption receipts intact; record any errata review
separately. Do not claim the complete sections are implemented.

## Proposed bounded grammar and outcomes

Admit global named element-only complex types containing the current finite
sequence and zero-to-eight assertions, and root declarations referring to those
types by QName. Preserve anonymous roots. Resolve QNames at their actual schema
node using in-scope namespaces, including the default namespace; never use
xpathDefaultNamespace to resolve a type QName. Compare expanded names exactly.
Support declarations before or after their references and reuse by multiple
roots. Define an explicit finite global-type count before adoption; preserve
all byte/depth/token/numeric/instance limits.

Compile every admitted declaration, including unused types. Undefined prefixes,
malformed QNames, unresolved types, duplicate type expanded names and conflicting
inline/type declarations must refuse compilation with no owned assessment.
The type and element symbol spaces remain distinct: equal expanded names across
those spaces are allowed. Proposed result for these schema failures is the
existing Unsupported compile path, with stable detected-reason categorisation
specified before code; it is not instance Invalid. Supported schema plus wrong
instance content is Invalid; valid content is Valid. No exception or fallback
may turn refusal into acceptance. Do not silently ignore an excluded definition.

Named simple types, restriction/extension, recursion, nested complex children,
global element refs, imports/includes, attributes, xsi:type/nil, defaults/fixed,
additional datatypes/facets and arbitrary assertions remain refused. These are
intentional profile exclusions, not XSD requirements. Add separate slices for
them rather than using descriptor parsing as validation evidence.

## Acceptance map to implement

| Group | Required evidence |
|---|---|
| Positive component reuse | Before/after declaration, two roots sharing one type, zero and supported assertions; no-namespace and qualified schemas |
| QName identity | Alternate prefixes, default namespace resolution, local namespace shadowing; equal local name in wrong URI refuses |
| Schema failure | Unknown type/prefix, malformed QName, duplicate types, inline/type conflict, bad unused declaration; no partial owned result |
| Exclusion | Named simple type, derivation, reference/import/attribute and nested/recursive forms refuse explicitly |
| Instance assessment | Decimal value/order/occurrence/assertion successes and independent failures under the same compiled named type |
| Actual Core transport | ResolveType and root type annotation retain expanded name/component identity; element-only root atomization raises FOTY0012; decimal leaf atomization succeeds |
| Authority and lifetime | Shared type within one compiled schema; identically named type from another schema has different authority; foreign/disposed state refuses; no raw authority export |
| External application | Package-only consumer assess/read/export/reopen within existing contracts; immutable exact source/package/Core identities, no friend access |
| Regression | All409 existing source cases; minimum/current Core lanes and retained124/23/33 catalogs remain separate overlapping qualifications |

First reproduce the named-type refusal on the unchanged baseline. Build authored
fixture pairs and assign exact normative clauses and expected outcomes before
changing production code. Select and pin official W3C suite cases and their
redistribution terms separately; authored tests are not official conformance.
Use current public Core APIs first. A reproduced missing neutral capability
requires a CoreREQ-142 request; no Core engine/API change is presumed.

## Delivery gates

Full Schema solution build/test, package-only/current-Core checks, retained binary
qualification, strict documentation checks and source/evidence manifests are
required after implementation. Engine changes additionally require the existing
Core QT3, pinned XSLT and qt4 gates. This planning step changes no runtime and
adds no passed-test count. Commit/push of this new proposal awaits an explicit
request under AGENTS.md Git Rules; release/tag/publication remain separate.
