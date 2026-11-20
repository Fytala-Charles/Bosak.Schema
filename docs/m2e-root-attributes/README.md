# M2E1 bounded native root attribute candidate

19 November 2026. [Before-code adoption](ADOPTION.md) implements the sealed
[contract proposal](../m2e-attributes/CONTRACT.md) on merged Schema098fdaf.
[PR23/main delivery](M2D1-DELIVERY.json) passed837source tests,54release controls
and6workload controls. Its merge tree equals qualified9fa0536. This dossier records
subsequent local native implementation, not hosted CI or a published attribute API.
The original planning dossier and all older sealed receipts remain unchanged.

## Bounded behavior and public surface

Only NativeSchemaSession admits local attributes on the assessed root's anonymous
or named complex type. Required/optional(default) uses, explicit type QNames and
qualified/unqualified forms apply; attributeFormDefault is enabled only for schemas
with attribute uses. Attribute default namespaces remain distinct from element
default namespaces. Namespace declarations never create attribute indexes.

The exact additive surface is AttributeCount/ReadAttribute on NativeSchemaDocument
and one producer-only sealed NativeAttributeValue with NamespaceUri, LocalName and
Value getters. Exactly one type/five members, no public constructor or authority/
raw node/XdmValue escape. [API comparison](API-SURFACE.json) preserves all381prior
rows/51types and checks all six new rows against the concrete proposal (52types/
387rows total). Existing options, enum values and old members are unchanged.

ScalarCount/ReadScalar remain present-leaf-only in instance order. AttributeCount/
ReadAttribute have separate indexes in schema declaration order, omitting absent
optionals regardless of XML attribute order. Empty string/false/zero are real
present values. Detached immutable entries and scalars survive disposal without
schema authority. Read/gate/cancellation/closed/index/query-capacity exceptions
match the established contract; failed reads/export leave other in-bound work
usable. Attribute and leaf reads share the finite original snapshot cache.

String/Boolean/integer/decimal values and admitted named string/decimal restrictions
reuse exact existing scalar lexical, facet and representation rules. Integer is
an XSD built-in decimal restriction, not a primitive datatype. XML attribute
normalization precedes datatype processing; literal attribute tabs/newlines become
spaces while referenced tabs/CR/LF retain their parsed values. String facet length
counts Unicode scalar values, independently of UTF16capacity. Export/reopen keeps
parsed lexical signs/zeroes/attribute strings under the bounded writer policy,
not canonical XSD text, raw bytes or portable PSVI.

An attribute-enabled root validates required uses, all present typed values and
its finite ordinary attribute vocabulary: absent required/undeclared ordinary
attributes are Invalid. Its xml/xsi requests remain Unsupported/Attributes. Selected
roots without attribute uses keep prior Unsupported/Attributes instance behavior;
NativeDecimalSession remains decimal-only and refuses attribute-bearing schemas.
Leaf application attributes and attribute assertions stay excluded. Root attribute
uses must follow sequence and precede assertions; at most16uses per complex type.
Existing schema/type/root/node/leaf/input/numeric/string/admission/export limits
stay finite. Unknown/unsupported/unused declarations and invalid form/use/order/
capacity configurations refuse whole compilation under existing reason codes.
Defaults/fixed/inheritable/prohibited, global refs/groups/wildcards, anonymous
attribute types, leaf attributes and special xml/xsi processing are not added.

## Original facts and lifecycle

Actual root XAttribute wrappers belong to the original captured document. Only
complete successful root/leaf/assertion assessment publishes original element and
attribute facts. Local declaration handles differ from type handles and cannot be
looked up as global declarations. Named complex root references reuse local
components within one schema; foreign input/schema names do not authenticate them.
Separate ElementFacts/AttributeFacts preserve leaf counts even when no leaf is
present. Primitive/named type annotation, actual atomic family and original
declared/actual/origin provenance survive the adopted Core typed-read route.
Ordinary data(.) node atomization is checked on actual native attributes; the
shipping advisor's typed-identity binding is not silently broadened to data($node).
Complex-rootFOTY0012, source lifetime, revocation, cache/gate and cleanup remain.

## Qualification

| Lane | Passed | Scope |
|---|---|---|
| Full source |950 |372implementation+578public; zero failures/skips |
| Prior regressions |837 |All prior outcomes unchanged before new tests were added |
| Minimum-Core package-only |578 |Signed published1.81.2, no project references |
| Current-Core package-only |97 |89authored+7public controls+runtime identity on1.81.17 |
| Retained unchanged callers |124/23/33 |OnlySchema.dll replaced; overlapping catalogs |

[Results](RESULTS.json) and [case map](CASE-MAP.json) retain89authored cases:
38positive,17supported-invalid,11instance-refusal,
23compile-refusal. Seven public contract/capacity/cancellation/
index/cache/export controls,15actual native Core controls and2attribute disposal-
waiting cases bring the new source increment to113. Shared existing cleanup
fault tests remain green. [Regression delta](REGRESSION-DELTA.json) is empty;
no earlier fixture or sealed expectation was changed. These authored tests and
repeated package lanes are not official W3C cases, whole-spec percentages or SLAs.

Release build has zero warnings/errors. Local unsigned0.6.0-alpha.0.11 is a
qualification artifact whose repository metadata identifies base098fdaf only;
[SOURCE-MANIFEST](SOURCE-MANIFEST.json) pins actual uncommitted source. [Identity](IDENTITY.json)
verifies18Core signatures, exact nuspec source pins/archive-cache-runtime payloads,
candidate archive/cache/runtime payloads and original signed beta.1 input.
[Retained inputs](RETAINED.json). No Core engine/API/dependency/source-wrapper pin,
tag/controller/registry or published beta.1 change is included. No fresh Core
conformance or hosted950case CI is claimed for this local candidate.

[Example results](README-RESULTS.json) execute all eight actual packaged C#blocks,
the formerly illustrative root-attribute consumer against its unchanged sealed
planning fixtures, and the earlier scalar proposal. The order has two leaf indexes
and three separately ordered root attributes including false and an exact integer
beyond Int64. Earlier namespace/named-type/facet/Boolean/integer examples still run.
The historical planning baseline5tests/27synthetic probes remain before-code
receipts; reproduce baseline refusal tests only at recorded098fdaf before-code
source, not against this now-extended working tree.

Canonical documentation, strict hygiene, programmatic rendering/local links and
source/evidence hashes are recorded; no new visual acceptance, overall API/C45
freeze or full invoice workflow acceptance is implied. Source/proposal are local
and uncommitted under AGENTS.md. Next: commit/push/PR review for this qualified
slice. Broader attribute placements, defaults/fixed, derivation/regex and general
assertions require separate adoption; specification coverage remains the priority.
