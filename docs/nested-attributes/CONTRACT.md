# Proposed nested container attribute contract

20 November 2026. Proposed only, not semantic adoption, API acceptance or runtime.

## Supported schema shape and budgets

Only NativeSchemaSession gains local required/optional attributes on descendant
named/anonymous element-only complex containers in the existing acyclic sequence
profile. Reuse current string/Boolean/integer/decimal types, admitted named
string/decimal restrictions, attribute forms and normalization rules. No attribute
uses on scalar leaves or nested simple-content types. No defaults/fixed,
inheritable/prohibited, global refs/groups/wildcards, anonymous simple types,
reserved declarations, new facets or assertions. New nested schemas stay
assertion-free. NativeDecimalSession and existing root/simple-content behavior
are unchanged.

Keep 0-16 attribute uses per container, combined global types/roots16each,
local anonymous types16, depth4, expanded terminals16 and defensive elements64.
Add a fixed aggregate ceiling of64potential attribute occurrences per root,
INCLUDING root attributes and absent optional uses/containers at their maximum
finite occurrence. Compute over the acyclic expanded plan before instance
assessment; reject excess as Unsupported/RepresentationCapacity even if an input
omits the optional branch. Shared types count once per potential element
occurrence, not once per component. Eager checking of unused definitions remains.
Use bounded arithmetic and cancellation; no unlimited product or public option.
A16line/four-attribute plan has64potential uses; adding one root use yields65and
must refuse. These are engineering limits, not normative XSD constraints.

Use existing explicit string UTF16and numeric/Boolean raw256lexical/query/cache
limits independently of total-use bounds. attributeFormDefault remains admitted
only when there are actual admitted attribute declarations, now including inline
nested containers. XML default namespaces never qualify unprefixed attributes.

## Validation and atomic publication

At every selected owner, attribute-enabled plans first refuse reserved xml/xsi
requests, then assess uses in schema declaration order: required presence,
scalar lexical/representation/facet checks; finally reject undeclared ordinary
attributes as Invalid. Attribute-free owners retain existing Unsupported/
Attributes for ordinary application attributes. Namespace declarations are not
attribute values. Missing required or in-bound invalid lexical/facet values are
Invalid. Keep reserved/capacity/unsupported-markup refusals distinct.

Preserve root-selection/root-attribute precedence. After each owner's attributes,
check element-only text/markup and recurse in sequence order with existing
occurrence rules. Complete every root, nested owner, terminal and assertion-free
sequence before publishing any facts/document. Deep failure/cancellation after
valid ancestors/siblings must issue no partial assessment or consume admission.
Capture each attribute against its exact original owner node and local use;
never resolve descendant attributes by a root-only name lookup or by owner path
alone without checking captured node identity.

## Owned access and indexing

Propose exactly these additions:

- NativeSchemaDocument.DescendantAttributeCount: number of present declared
  attributes on descendant complex containers, excluding all root attributes.
- NativeSchemaDocument.ReadDescendantAttribute(int attributeIndex,
  CancellationToken cancellationToken = default): detached NativeAttributeValue.
- NativeAttributeValue.OwnerElementPath: ImmutableArray<int>, never default.

Existing AttributeCount/ReadAttribute remain root-only, in declaration order,
with no changed indexes. Their new OwnerElementPath is empty. ScalarCount/
ReadScalar retain terminal-only document order; no attribute/container enters that
projection. HasRootScalar/ReadRootScalar are unchanged. No new public type,
constructor, option, enum, raw node, authority or arbitrary query interface.

Descendant attributes are ordered by owner element preorder, then schema
attribute declaration order, skipping absent optional uses. XML source attribute
order/prefix choices do not reorder results. Repeated containers share type/local
use component identity as appropriate, but each occurrence has its own node/facts
and path. Counts/read operations use the existing sequential owner gate.

OwnerElementPath is a sequence of zero-based ELEMENT-CHILD indexes from the
assessed root to the attribute's owner, ignoring text, comments/PIs and namespace
nodes. Empty identifies the root; [0] the first child element; [1,0] the first
child of the second child. Present descendant container owners have1-2segments in this profile: a container
requires a potential child, which must also fit within depth4.
Indexes describe present XML children, not schema declarations or flattened
scalar indexes: omitted optional elements shift later paths. The path is a
snapshot-relative detached locator, never schema identity or authority. Export/
reopen reassesses facts; equal paths across documents authenticate nothing.
Path storage is immutable and carries no session/input/node reference. Detached
attribute/name/scalar/path data remains usable after document/session disposal.

NativeAttributeValue's XML documentation must expand from root-only to owner-aware
attribute results while keeping its producer-only constructor and prior members.
Read bounds, cancellation, closed/overlap, cache/query/output limits, detached
values, revocation and cleanup follow existing exceptions/recovery contracts.
Changing XML prefixes, source attribute order or indentation does not change paths;
changing element presence/order can. No path-based read API is proposed.
