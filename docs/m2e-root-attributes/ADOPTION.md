# M2E1 root attribute adoption - 19 November 2026

Recorded before product edits under the owner instruction to implement the concrete
[root contract](../m2e-attributes/CONTRACT.md) and proposed ADR012. Schema base
098fdafdb38de451fc1342ce97ff01b3c71bf8f1 is merged PR23/main837. The sealed planning
MANIFEST remains unchanged; its27/27synthetic probes are feasibility, not native
validation. Its5before-code baseline controls prove current refusal/absent API.
[Fresh source-body hashes](SPECIFICATION-SOURCES.json) match dated Structures and
Datatypes20120405 plus XML1.0fifth20081126. Review dates and historical adoptions
are separate; no moving latest text or later errata is silently adopted.

Adopt bounded local root attributes from XSD Structures3.2.1-.6,3.4.2-.4,3.5.1-.6,
3.16/4.2 and XML3.3.3/2.11/4.1 attribute normalization/line endings/references.
Existing adopted string/Boolean/integer/decimal and selected direct named string/
decimal facets apply to attribute values. Existing XDM3.1 2.7/3.3/6.2 underpins
original typed facts/provenance. Partial implementation only: no full-section,
full-XSD, API/C45 or general query/profile acceptance.

Only NativeSchemaSession admits0..16local root attribute uses after sequence and
before0..8assertions in anonymous/named complex types. Name/type required; use
optional(default)/required and form qualified/unqualified. attributeFormDefault
is admitted only in schemas with attribute uses; old attribute-free schemas keep
Namespace refusal for that setting. Root types without attribute uses retain old
Unsupported/Attributes for application attributes. Attribute-enabled roots require
all declared required uses and reject undeclared ordinary attributes as Invalid.
Namespaces are expanded-name identity; XML default namespaces do not qualify
unprefixed attributes. Types resolve at declaration-site QName bindings, sharing
only actual admitted primitive/built-in and named component identities.

Reserved xmlns declaration names and xsi/xml special processing are excluded;
namespace declarations themselves are skipped. Duplicate expanded declarations,
malformed use/form/order/structure, defaults/fixed/inheritable/prohibited/global
refs/groups/wildcards/anonymous types and unsupported types refuse compile with
existing bounded reason categories. Root xsi/xml application requests and leaf
application attributes remain Unsupported/Attributes. Unknown ordinary root
attributes on enabled roots and missing required uses are Invalid. No defaulted
values, nonexistent optional placeholders, attribute assertions or new datatypes.

Assessment order: bounded XML parse; selected attribute-enabled root first refuses
reserved requests, validates uses in schema order (presence then scalar lexical/
capacity/facets), rejects undeclared ordinary attributes, then validates root
text, leaves and existing decimal assertions. Attribute-free/unknown roots keep
legacy attribute-before-root-name refusal. Complete success is required before
publishing any root/leaf/attribute fact or admitting a document. Check cancellation
through use and leaf loops; old input/node/sequence/type/root/admission/export
limits and all837baseline controls stay required. Fixed16use ceiling is a profile
limit. String UTF16and numeric/Boolean raw256/supplied digit-scale bounds remain;
XML attribute normalization precedes the datatype's preserved/collapsed whitespace.
Native read/cache limits remain separately enforced; no total-memory/time SLA.

Add exactly NativeSchemaDocument.AttributeCount:int, ReadAttribute(int
attributeIndex,CancellationToken=default):NativeAttributeValue and producer-only
sealed NativeAttributeValue with NamespaceUri:string, LocalName:string and
Value:NativeScalarValue getters. One new type/five members, no public constructor.
Present root attributes index in schema declaration order, absent optionals omitted;
existing leaf ScalarCount/ReadScalar never include attributes. Reads use the same
owner/gate and original Core typed identity route; detached values/name primitives
survive disposal without authority. Existing argument/cancellation/overlap/closed/
query-capacity/cleanup exceptions and sanitized diagnostics remain.

Maintain separate element/attribute facts and local declaration identities;
ResolveAttributeDeclaration must not masquerade locals as globals. Named complex
references reuse their local declarations, foreign schemas never do. Real owned
attribute nodes retain named/primitive type annotation, original declared/actual/
origin evidence, exact atomic family and foreign/disposed guards. RootFOTY0012
unchanged. Do not broaden the shipping advisor to arbitrary data($node) profile:
actual ordinary node atomization and the existing typed-read route are qualified
separately. Export preserves parsed lexical spelling under existing bounded writer
policy, not canonical lexical XML or portable PSVI. Legacy NativeDecimalSession
stays decimal-only and refuses attribute-bearing schemas.

Require native provenance/normalization/form/capacity/lifetime tests, full source,
signed minimum/current public package consumers, unchanged retained124/23/33,
exact additive API delta, executable actual packaged examples and canonical docs.
Record intentional prior-outcome changes separately, never rewrite sealed receipts.
No Core engine/API/pin, tag/controller, registry or published beta.1 change.
