# Bounded schema namespace/form contract

21 November 2026. Base ee32ac81c7a704c95647b121d1cde23cfa32dcbd.
Existing Core minimum1.81.2/qualification1.81.17 and public APIs stay unchanged.
[Immutable source bodies and corrected sections](SPECIFICATION-SOURCES.json)
were rechecked before experimentation. The previous3.15.2 review target denotes
Annotations; schema representation is3.17.2. Original proposal receipts stay sealed.

Mixed NativeSchemaSession/NativeSchemaNodeDataSession admit nonempty relative
target namespace references accepted by the existing .NET URI lexical predicate
using RelativeOrAbsolute, after XML whitespace collapse. Keep whitespace rejection,
reserved XS/XSI/XML/XMLNS target names and explicit empty-target refusal. This
remains a bounded lexical profile, not full anyURI/IRI validation. Namespaces1.0
relative names are deprecated; identity is still the exact supplied string.
Never resolve against a base URI, canonicalize dots/case/percent escapes, fetch
namespaces or infer authority from URI spelling. Existing schema/input byte quotas
bound token storage; no new public option or capacity is added.

Admit and validate attributeFormDefault on every mixed schema, independently of
whether any application attributes appear. Its qualified/unqualified values,
XML whitespace collapse, defaultunqualified and local form overrides stay as
already implemented. Do not admit invalid values or new schema attribute grammar.
The decimal-only profile retains its existing absolute namespace policy and
attributeFormDefault refusal.

TargetNamespace passes an explicit mixed-profile policy to NamespaceUri. Keep
explicit xpathDefaultNamespace literal values absolute-only under the previous
adoption. Existing ##targetNamespace and ##defaultNamespace symbolic settings use
the exact captured namespace name, including a relative target in the new mixed
profile. Prefixed operands retain the actual XML namespace context. No arbitrary
assertion grammar or BaseUri resolver is added.

Require real public-Core native facts, QName annotation/component identity,
original ownership, atomization, export/reassessment and failure no-publication
on both pins before production adoption. Then qualify source, public package-only,
retained callers, signatures/runtime payloads, zero API delta and frozen corpus.
Record authored expectation changes individually; do not rewrite frozen W3C files.
The assert006 first gate may move from Namespace, but integer complex content,
restriction and assertions remain independent exclusions. No future pass count
is claimed. No commit/push/publication is authorized for this slice.
