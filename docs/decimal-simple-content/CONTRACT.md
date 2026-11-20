# Bounded decimal simple-content root contract - 20 November 2026

Base d4eca99 is merged PR24/main950. This concrete contract precedes implementation;
ADR013 remains Proposed, not general API/C45 acceptance. Extend only opt-in
NativeSchemaSession with anonymous/named root complex types whose sole child is
simpleContent/extension directly based on xs:decimal or an already admitted named
decimal restriction. Reuse existing root attributes/forms and their limits.
No simpleContent restriction, complex-base extension chain, nested elements,
assertions in the simple-content extension, mixed content, defaults/fixed or new
datatype is admitted. Eager whole-schema/unused-definition checks remain.

Add exactly two NativeSchemaDocument members: bool HasRootScalar { get; } and
NativeScalarValue ReadRootScalar(CancellationToken cancellationToken = default).
No new type, option, enum, constructor or other member. HasRootScalar is true for
an admitted decimal simple-content root, false for existing element-only roots;
both getters/reads use the owner/sequential gate. Existing leaf ScalarCount/
ReadScalar and AttributeCount/ReadAttribute indexes are unchanged. A simple-content
root has zero leaf scalars; its separately read value is exact decimal. Existing
element-only roots retain FOTY0012/HasNoTypedValue and ReadRootScalar throws fixed
NotSupportedException("FOTY0012"). Null/cancellation/closed/overlap/query/cache/
admission/export/cleanup exceptions and recovery follow existing contracts.

The root node's governing/declared type and annotation stay the complex PriceType
(or original anonymous complex handle); its actual atomic type is the decimal
content type, including an admitted named decimal restriction. DeclaredType and
ActualType are therefore intentionally distinct on root atomic provenance. Local
attributes keep their existing simple type/fact path. Complex-type status is
independent from absence of a typed value. The root's original global declaration
metadata is retained. No global derivation/type relation is manufactured.

Native numeric parsing retains raw parsed text256UTF16, supplied digit/scale
limits and inherited decimal facets before normalization. In-bound lexical/facet/
nested-element failures are Invalid; profile capacity/unsupported markup/grammar
is Unsupported. Simple-content root comments/PIs remain excluded, CDATA is text.
Root attributes are validated before its scalar under existing deterministic
rules. No root/attribute fact or document publishes on failure. Root string-value
uses XML-schema whitespace collapse (not canonical numeric spelling), whereas
export retains parsed XML lexical text under the bounded writer policy. Detached
NativeScalarValue holds no authority and survives disposal.

Existing950outcomes remain required, with deliberate former-refusal deltas kept
separate if any arise. Qualify actual native node/facts/provenance, signed minimum/
current Core package consumers, retained124/23/33, exact two-member API delta and
executable actual README, plus immutable source/case/evidence hashes. No Core
engine/pin, tag/controller or registry change. The isolated price fixture copies
the existing PriceType unchanged; the complete orders.xsd still refuses because
its other unsupported definitions are eagerly checked.
