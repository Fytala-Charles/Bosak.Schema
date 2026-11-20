# Proposed nested-sequence contract

20 November 2026. Proposed only; not semantic adoption or implementation.

## Grammar and bounds

- Opt-in NativeSchemaSession only; NativeDecimalSession retains its old grammar,
  counters, diagnostics and refusal behavior.
- Every new container is an element-only complex type containing exactly one
  sequence. A sequence has 1-16 local element declarations, each using exactly
  one admitted type reference or inline anonymous complex type. No inline simple
  types. Names are unique by expanded name within each sequence; equal names in
  different containers are allowed and retain different declaration identities.
- Existing combined global-type and global-root ceilings remain 16 each. Local
  anonymous types also consume an explicit new internal ceiling of 16 per schema.
  Acyclic named reuse is allowed. Direct and indirect type cycles are Unsupported,
  even if an optional edge could terminate an instance. Eager unused-definition
  checks prevent hiding a cycle or unsupported construct in an unused type.
- Element minOccurs is 0 or 1; maxOccurs is a finite integer 1-16 with min <= max.
  The sequence itself has implicit 1/1 occurrence. Explicit sequence occurrence,
  maxOccurs=0, unbounded, choice/all, model-group references and element references
  remain Unsupported. Existing flat restrictions remain unchanged.
- For each global root, calculate worst-case expansion over all optional branches
  and multiplicative repetitions before instance assessment: at most 64 element
  nodes including the root, at most 16 terminal scalar occurrences, maximum depth
  4 counting the root as depth 1. Reject excess at compile time as Unsupported
  representation capacity; do not rely on a small supplied instance. Use checked
  or saturating arithmetic; recursion and overflow must never escape as crashes.
  These are proposed engineering limits, not normative XSD restrictions.
- Existing root attributes are supported with unchanged 16-use/form/scalar rules.
  Descendant attributes, nested decimal simple-content types, mixed content,
  derivation, wildcard/open content, nil/default/fixed, imports/includes and new
  datatype/facet families remain Unsupported.
- New schemas containing nested containers must be assertion-free, including
  root and unused named complex definitions. Existing flat assertion schemas
  continue unchanged. Descendant assertion contexts require separate adoption.

## Assessment and observable values

Each container matches its own sequence in order by expanded name. Count repeated
complex children as sibling occurrences, not as their descendant leaf count.
An absent optional container creates no facts or phantom scalar entries; if
present, all its required descendants must validate. Element-only non-whitespace
text and in-profile ordering/count/name/value/facet violations are Invalid.
Depth/capacity/profile grammar, special attributes and unsupported markup are
Unsupported. Preserve existing XML parser/byte/text limits and deterministic
validation ordering; implementation must document exact first-refusal precedence.
In the new nested profile comments/PIs inside containers or scalar leaves are
Unsupported; CDATA is text, and whitespace-only container text is allowed.

Publish all element and root-attribute facts once, after complete assessment.
All element facts match original nodes in preorder, including containers. Maintain
a distinct internal scalar-fact projection: ScalarCount/ReadScalar enumerate only
terminal simple-typed leaves in document order. Existing flat-document counts,
indexes and exceptions are unchanged; attributes remain root-only separate reads.
Repeated leaves produce separate entries; absent optionals produce none.
No new public type/member, raw node/authority escape or path-selector API.

HasRootScalar stays false on element-only roots; ReadRootScalar retains FOTY0012.
Each nested element-only container has a complex annotation and no typed value;
actual Core data(container) must raise FOTY0012. data(leaf) uses original scalar
facts and existing exact typed provenance. Reused named types share type identity
without conflating local declarations, node identity or assessment episodes.
Container string-value follows descendant text in document order; export retains
parsed lexical XML using the existing bounded writer. Export/reopen must reassess
with the same schema, not reuse stale authority.

Existing owner/gate, cache, independent query/representation limits, cancellation,
quota, export-failure recovery, disposal/revocation and detached-value lifetime
contracts apply. A rejected descendant publishes no partial document or facts.
No Core engine or package-pin change is proposed; capability gaps must first be
reproduced against public APIs and tracked separately.
