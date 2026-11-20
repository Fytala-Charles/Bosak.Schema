# Proposed nested decimal simple-content contract

20 November 2026. Proposed only; not adoption, implementation or acceptance.

## Grammar and unchanged limits

NativeSchemaSession may use a named or one inline anonymous complex type on a
local descendant element whose sole type child is simpleContent/extension based
directly on xs:decimal or an admitted named decimal restriction. Reuse current
local attribute declarations/forms/scalars and facet rules. This is complex
simple content, not a primitive scalar element with added attributes.

No simpleContent restriction, complex-base extension chain, other simple-content
base family, lists/unions, nested element content, mixed content, defaults/fixed/
nil, derivation/composition/wildcard/global refs or new assertions. Existing nested
schemas remain assertion-free. Primitive/named SIMPLE-type leaf attributes stay
Unsupported; admitted COMPLEX simple-content terminals may have local attributes.
NativeDecimalSession and standalone root simple-content behavior are unchanged.

Retain16global types/roots each,16local anonymous types, depth4 includingroot,
16potential descendant scalar terminals, defensive64element nodes,16attribute uses
per owner and64TOTAL potential uses includingroot/optional/repeated branches.
A simple-content terminal contributes one element and one scalar per maximum
occurrence, with all its attribute uses. Root simple-content still has zero
leaf/scalar projection entries and a separately read root value. No capacity
increase or new public option. Eager unused definitions/cycles/budgets remain.

## Assessment and original facts

For each descendant simple-content owner, assess its enabled attributes with the
current reserved/presence/scalar/facet/unknown-use precedence, then its content.
Child elements are Invalid; comments/PIs Unsupported; CDATA is text. Apply the
same raw256UTF16decimal lexical and supplied digit/scale/facet gates as the
existing root simple-content route. In-bound lexical/facet errors are Invalid;
profile/representation limits Unsupported. No partial facts/document on deep
failure or cancellation, even after valid ancestor/sibling attributes.

Keep the original complex type annotation and governing/declared handle. Actual
atomic type is the decimal content type, including admitted named restrictions;
do not replace the governing annotation with primitive decimal. IsComplexType
is true while HasNoTypedValue is false. ReadScalar and actual data(.) return the
exact decimal value. Original local declaration handles remain local, including
when a named type is shared with a global root. Local simple-content nodes must
not manufacture global SchemaElementDeclaration metadata. Element-only
ancestors retain HasNoTypedValue/FOTY0012.

Simple-content node StringValue uses schema-collapsed lexical decimal text, not
forced canonical numeric spelling. Export preserves parsed lexical XML using the
existing bounded writer. Read/query, StringValue and export have intentionally
different purposes: +0012.50 may be the normalized text and12.5the exact value,
while original surrounding parsed whitespace remains in exported XML. Reopen
must reassess original facts; no stale authority transfers between captures.

## Existing public reads, indexes and owner paths

No new public member/type/option/enum/constructor or query selector. ScalarCount/
ReadScalar enumerate DESCENDANT scalar-bearing terminal elements in document
order, including primitive/named simple leaves and admitted complex simple-content
terminals. Repeated terminal instances produce separate entries; absent optionals
produce none. No container without typed value or attribute enters that list.
All existing valid document indexes remain unchanged. HasRootScalar/ReadRootScalar
remain root-only; an element-only order root staysfalse/FOTY0012 even when it
contains price values.

AttributeCount/ReadAttribute remain root-only, with empty nondefault owner paths.
DescendantAttributeCount/ReadDescendantAttribute include present declared uses on
both element-only complex containers and complex simple-content terminals,
ordered by owner element preorder then declaration order. Values use the same
original authority/read/cache route. Primitive simple-type leaves stay excluded.

OwnerElementPath keeps zero-based PRESENT ELEMENT-child indexes relative to the
assessed root, omitting text/namespace nodes. At depth4a terminal owner may now
have three segments; this supersedes the prior element-only-owner bound of two
for newly admitted shapes only. No structural depth expansion. Paths are detached
immutable nondefault arrays, not schema indexes, scalar indexes or cross-document
identity/authority. Exact captured owner path/index/expanded-name/node consistency
remains mandatory. Root/owner/terminal read caches, independent query limits,
sequential gate, overlap/cancellation/disposal/revocation, quota and failure
recovery contracts remain unchanged. Detached values/paths survive disposal.
