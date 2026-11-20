# Proposed owned original-node data contract

20 November 2026. **Proposed, not adopted or implemented.** Scope: original
element/attribute atomization only, for the already admitted finite XSD profile.

## Session and assessment identity

Create a separate owned node-data session from schema text and explicit existing
NativeSchemaSessionOptions. Compile the released Core `data($node)` profile
before assessment, with empty/default captured static context and no caller
CompileOptions, namespace bindings, expression text, resolvers or extension hooks.
Use a dedicated native-provider configuration for that profile; the existing
provider constructor/scalar facade continues to accept only its current profile.

The dedicated binding must check the actual engine-issued environment: released
Core assembly identity, XPath3.1 host, dated XDM revision, exact original-node
data profile/descriptor and captured empty static context/fingerprint. Reject
wrong host/profile/descriptor/options and forged or foreign environments. Do not
make existing scalar Bind permissive or guess compatibility from a name alone.

Assess each owned XML capture using the same advisor/environment which issued
the node-data session. Bind input, original assessment, snapshot, nodes and facts
to that exact tuple. An existing NativeSchemaDocument from a `$value` session
cannot be relabelled/rebound or passed into this session. No raw capture, node,
provider authority, Core snapshot, provenance handle, callback or task is exposed.
Public document handles are private-constructor owned wrappers for this session.

## Original target selection

An element path is a copied, validated sequence of zero-based **element-child**
indexes from the assessed root. Empty means the root. Existing depth4 admits at
most3segments; count only element children, not text/comments/attributes. Resolve
the path to the original captured wrapper while holding the session operation
gate; never serialize/reparse, manufacture a node, search an exported copy or
reuse a detached scalar's provenance. The path is a locator, not transferable
authority. Equal paths in another document resolve only in that document's owner.

An attribute target consists of the owner element path plus exact namespace URI
and local NCName. No prefix resolution is needed. Resolve its original present
attribute node and require original issued facts for that exact node/owner/use.
Unassessed schema-location hints or namespace declarations do not acquire typed
facts. Schema admission and the current attribute grammar remain unchanged.

Default path, excessive depth, negative/out-of-range indexes, null names, invalid
NCNames or a missing attribute are argument failures (ArgumentNullException,
ArgumentException or ArgumentOutOfRangeException, with exact parameter mapping
to be fixed in adoption). A present but unassessed attribute is Unsupported;
do not return its lexical string as if it had an admitted typed value.

## Atomization and output

For each resolved original element/attribute, enter one admitted synchronous
Core operation and evaluate the fixed `data($node)` with the exact original node.
Require original issued facts and live tuple ownership. Preserve governing,
declared and actual type handles internally, including absent anonymous QName,
named restrictions and complex simple-content versus atomic type distinctions.
No primitive alias or fake QName may replace anonymous identity.

The current native grammar permits a single atomic value at admitted scalar
targets. Return exactly one detached NativeScalarValue of the existing string,
Boolean, integer or exact decimal kinds. Empty string, false and zero are real
values. Materialize and detach inside the admitted operation before returning;
no raw XdmValue/provenance or lazy sequence escapes. Zero/multiple/list/other
atomic outcomes are outside this first facade and refuse without partial output.

Element-only root/container targets have no typed value: preserve FOTY0012 via
the facade's documented NotSupportedException convention, not empty/false/zero.
Root and descendant decimal simple-content targets retain their actual atomic
value. Never turn a query/target refusal into Invalid instance assessment.

## Limits and lifetime

Reuse explicit native/string/export, open-document, cached scalar/result atom
and query digit/scale limits. Current path depth is inherited, not a new limit
on supported XML. Every distinct target/cache charge must follow the actual Core
data profile; measure element/attribute/repeated-read behavior before adoption.
Query capacity may be narrower than admitted native values. Refusal/cancellation
must leave existing cached values and other targets usable without partial cache
publication. Do not promise unverified cache rollback or timing guarantees.

One private synchronous gate covers assessment, reads, export and admission.
Overlapping/reentrant work refuses. Document closure revokes that admission;
session closure waits for admitted synchronous work and revokes every document,
attempting owned cleanup under existing sanitized failure rules. Detached scalar
values and XML remain usable after closure, carrying no PSVI or authority.
Assess/export date/timezone and XML capacity/DTD/namespace behavior remain as
in the existing native profile. No cross-session document compatibility is added.

## Explicit exclusions

No arbitrary XPath source, path/predicate query syntax, arithmetic/comparisons,
multi-document joins, caller-supplied variables, XQuery/XSLT, constructed/copy
nodes, validation-on-demand, lists/unions, new datatypes or nil/type override
semantics. The existing XSD validation grammar and frozen pilot are unaffected.
No claim of general node-query, C45/ADR006, fullPSVI or whole-XSD acceptance.
No Core pin, release-controller/tag or package publication change is selected.
