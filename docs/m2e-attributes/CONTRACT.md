# M2E1 root attribute contract proposal

19 November 2026. **Proposed, not implemented or semantically adopted.** Schema
base098fdafdb38de451fc1342ce97ff01b3c71bf8f1 is merged PR23; tree equals qualified
9fa0536. Main CI44614488088 passed837source cases,54release controls and6workload
controls. Existing beta.1/package/Core pins and all sealed dossiers are unchanged.
This proposal is a concrete next slice for REQ009/CoreREQ142, not API/C45 acceptance.

## Scope and compatibility

First support local attributes on the assessed root's anonymous or named complex
type, with required/optional uses, explicit type QNames and qualified/unqualified
forms. Attributes on simple child leaves, nested elements, global attribute refs,
attribute groups/wildcards, prohibited/default/fixed/inheritable uses, anonymous
attribute types and xsi/xml special processing remain excluded. Root-only is an
intentional partial profile; it does not make the full public invoice fixture
supported. No fallback to unchecked XML attributes is allowed.

NativeDecimalSession remains decimal-only and refuses attribute-bearing schemas.
NativeSchemaSession's ScalarCount/ReadScalar stay present-leaf-only, in instance
order; never insert attributes into those indexes or reinterpret retained binaries.
Add a separate attribute path. A selected root with no attribute uses keeps its
existing Unsupported/Attributes behavior for application attributes. A root with
at least one admitted attribute use instead has a complete finite root-attribute
vocabulary: missing required or undeclared ordinary root attributes are Invalid.
Namespace declarations are not attribute values. Leaf application attributes stay
Unsupported/Attributes. Preserve existing diagnostic codes, sanitation and refusal
order outside this newly admitted grammar; all837baseline source cases remain
required with separately recorded intentional deltas if implementation discovers any.

## Exact proposed public surface

| Owner | Proposed member | Meaning |
|---|---|---|
| NativeSchemaDocument | int AttributeCount { get; } | Present declared root attributes only |
| NativeSchemaDocument | NativeAttributeValue ReadAttribute(int attributeIndex, CancellationToken cancellationToken = default) | Original Core typed read, detached result |
| Bosak.Schema | sealed class NativeAttributeValue | Producer-only immutable detached entry; no public constructor |
| NativeAttributeValue | string NamespaceUri { get; } | Expanded-name namespace, including empty string |
| NativeAttributeValue | string LocalName { get; } | Expanded-name local name; never a prefixed QName |
| NativeAttributeValue | NativeScalarValue Value { get; } | Existing checked detached scalar family |

One new public type and five members; no option, enum, existing signature or public
provider/raw-node/XdmValue/authority addition. [Signature ledger](SURFACE.json).
Attribute indexes follow schema attribute-use declaration order after omitting
absent optional uses, regardless of XML attribute order or prefix spelling.
No missing-value sentinel, defaulted value or invented false/zero/empty string.
A present empty string/false/zero is a real entry. Root-only needs no owner-element
index; later leaf attributes require a separate reviewed extension.

AttributeCount uses the same owner/sequential gate as ScalarCount. ReadAttribute
checks cancellation, owner/admission and index under the established gate, reads
the original issued attribute value through Core and returns a detached immutable
entry. Negative/out-of-range attributeIndex =>ArgumentOutOfRangeException;
closed document/session =>ObjectDisposedException; overlap or a typed read outside
query limits =>NotSupportedException; cancellation=>OperationCanceledException.
No partial entry/list or raw exception data is returned. Failed reads must leave
export and subsequent in-bound operations usable. Existing null checks, cleanup
failure semantics, no-callback/no-general-query surface and disposal waiting remain.
Returned namespace/name/scalar primitives survive both disposals without authority;
wrong-kind scalar getters retain their fixed InvalidOperationException messages.

## Proposed schema grammar and limits

Preserve sequence and assertion rules, adding zero-to-sixteen xs:attribute uses
between the sequence and assertions on a complex type. Require name and type;
use defaults to optional and accepts only required/optional. Permit form with
qualified/unqualified. attributeFormDefault defaults to unqualified; explicit
attributeFormDefault is admitted only when the schema has an admitted attribute
use, preserving its current Namespace refusal for attribute-free schemas. Type
QNames use declaration-site namespace bindings. Attribute namespace comes from
attribute form rules, independently of elementFormDefault and XML default element
namespaces. Duplicate expanded attribute declarations refuse complete compilation.
Reject xmlns names and reserved namespace/special request features explicitly.

Attribute types are existing primitive string/Boolean/decimal, built-in integer,
and admitted direct named decimal/string restrictions, with their same lexical,
facet and native representation limits. Primitive integer remains a decimal
restriction in XSD, not a primitive datatype. XML attribute normalization precedes
datatype whitespace/facets; literal whitespace and referenced tab/CR/LF must be
tested independently. Preserve string Unicode scalar facet length versus UTF16
capacity. No new datatype/facet/assertion syntax is inferred. Native query numeric
limits remain separate from assessment representation; attribute data does not
broaden arithmetic/comparison support.

Keep combined16global types/16roots, existing finite16leaf maxima,32KiBschema,
64KiBinstance,256descendant-node and bounded export limits. Add a fixed16declared
root attribute uses per complex type; namespace declarations never consume scalar
attribute indexes. Existing MaxStringCharacters bounds string attribute values;
raw numeric/Boolean256UTF16and supplied-digit/scale limits stay the same. No new
unlimited/default capacity or memory/time SLA. Host read/cache/atom limits remain
explicit existing options and can refuse reads; tests must exercise combined leaf
and attribute caching/recovery rather than assuming attributes are free.

## Internal identity and complete publication

Represent local attribute declaration/use identities separately from element
identities; do not masquerade an attribute type handle as its declaration. Shared
named complex types reuse their local declarations inside one schema, while same
spelled declarations from independent sessions stay foreign. Local declarations
must not be exposed as global ResolveAttributeDeclaration results. No general
schema derivation/closure/import seam is added.

Capture actual root attribute XObjects under the existing guarded source lifetime;
namespace nodes are distinct. Produce original attribute node facts only after
all root attributes, leaves, decimal assertions and constraints pass. Facts carry
original type, local declaration, origin and source/snapshot bindings; typed
annotation and actual atomic family must agree. Keep rootFOTY0012 and all existing
node ownership, revocation, cancellation and lease rules. Separate element/attribute
fact collections in the adapter so Facts.Length-1 cannot accidentally become the
public leaf count. Export remains XML with parsed lexical spelling, not portable
PSVI, canonical XSD lexical serialization or raw-byte preservation.

The executed synthetic probes establish existing Core attribute facts/transport,
not native Schema implementation. Its shipping advisor currently binds only the
adopted typed-identity environment. Public data($node) probe success does not
silently broaden that binding or expose arbitrary queries in NativeSchemaSession.
The future facade should use its established identity typed-read route; original
node atomization must be separately qualified with actual native attribute nodes.

## Specification review and mandatory adoption

[Fresh immutable body review](SPECIFICATION-REVIEW.json) retains dated XSD20120405
Structures/Datatypes and XML1.0 fifth-edition20081126 source hashes. Review targets:
Structures3.2.1-.6(local declaration/type/assessment),3.4.2-.4(complex type attribute
uses),3.5.1-.6(required/optional uses),3.16/4.2; XML3.3.3/2.11/4.1(attribute
normalization/line endings/references); already admitted datatype/facet clauses
and XDM3.1 original atomics. This is review evidence only. Before code, record a
new adoption with exact clauses, source revision, intentional exclusions and
before-code expectations, keeping historical adoptions and later errata separate.

## Exit gates

[Acceptance map](ACCEPTANCE.json) covers proposed authored tests; it is not executed
Schema support. Require required/optional/absent/empty/false/zero, forms/prefixes,
XML normalization, all admitted scalar families/named restrictions, capacity
boundaries, repeated named complex references, complete invalid/refusal outcomes,
unchanged leaf indexing, original node facts/provenance, foreign/disposed/cache/
cancellation/cleanup and export/reopen. Fixtures below are planning inputs only.

Before delivery: source suite, signed minimum/current Core package-only consumers,
retained124/23/33 with onlySchema.dll replaced, exact one-type/five-member API delta,
executable actual README and canonical docs/evidence. Core engine/pin changes need
a separate reproduction/adoption and existing conformance gates; none is presumed.
No publication, tag, controller dispatch or new registry version is authorized.
