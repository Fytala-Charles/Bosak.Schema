# Proposed bounded xsi:type contract

20 November 2026. **Proposal only: not adopted, implemented or runtime-qualified.**
Base b1759d22761587b2abe507d12aacf3f5b6b33535. Normative review pins and affected
sections are in [SPECIFICATION-REVIEW.json](SPECIFICATION-REVIEW.json). Record a
separate implementation ADOPTION.md after the feasibility gates below; do not
reinterpret these review pins as historical adoption or owner API acceptance.

## First slice

Opt in through the existing NativeSchemaSession and NativeSchemaNodeDataSession.
Keep NativeDecimalSession unchanged. Add no public API, enum, Core profile or pin.
Admit instance type requests at existing root, scalar descendant and nested
container/simple-content positions only under the rules below. Schema grammar
stays closed: no import/include, complex inheritance, abstract/block/final,
alternatives, lists/unions, nil or additional datatype families.

Let D be the original governing element declaration, T = D.Type its declared type,
G the effective governing type, and C the atomic content type of unchanged complex
simple content. With no type request, G = T and every old identity is preserved.

## QName and symbol lookup

Recognize only the expanded attribute name
{http://www.w3.org/2001/XMLSchema-instance}type. Prefix spelling is irrelevant.
An unqualified type attribute or a type attribute in another namespace retains
ordinary attribute behavior; it never selects a type. Other special attributes,
including xsi:nil even when false, keep their current refusal boundaries.

Use the owning instance element's in-scope namespaces, including shadowing and
default-namespace undeclaration. Collapse XML whitespace in the parsed QName
value, verify one optional prefix plus NCName components, then resolve its expanded
name. An unprefixed QName uses the instance default namespace, or no namespace
when absent. Do not substitute the schema target namespace, assertion default
namespace, query namespace map or a parent whose binding has been shadowed.
Namespace URIs compare exactly, without normalization/case folding.

A proposed fixed ceiling of 256 UTF-16 units in the parsed attribute value applies
before QName whitespace collapse. Exceeding it is Unsupported/RepresentationCapacity, not Invalid.
Malformed QName, missing prefix binding and genuinely absent type components are
Invalid within the closed admitted schema. Anonymous types cannot be addressed
by inventing names or primitive aliases.

Distinguish absent names from known but unsupported built-ins. The reviewed
[51-name inventory](BUILTIN-NAMES-REVIEW.json) includes 49 simple types plus
Structures anyType/error. It is a name inventory, not authority or implementation
of those datatypes. Known excluded built-ins return Unsupported/ValueType for
this first slice, not a false NotFound/Invalid result. Unknown names in the XSD
namespace, and missing user types in the complete closed compiled schema, are
Invalid. No schemaLocation hint, namespace URI or QName triggers acquisition.
Keep the public authority lookup limited to actually owned supported components;
do not issue fake handles for inventory-only names.

## Derivation and outcome rules

Store actual base-component relationships during compilation. Never infer
substitutability from equal names, equal facet sets, matching value families or
one value fitting both types. All current user restrictions are direct; sibling
restrictions do not derive from one another. An anonymous direct restriction
retains its distinct component identity.

| Requested supported component | First-slice result |
|---|---|
| G is exactly T | Assess normally with that original component, including named complex roots/containers and existing facets/assertions |
| T is primitive string/decimal/integer and G is an existing direct restriction of that same primitive | Assess with G's facets and the unchanged scalar representation |
| G does not derive from T according to the compiled component graph | Invalid, even if the particular value fits both |
| A valid derivation changes scalar family, such as decimal to integer or a direct integer restriction | Unsupported/ValueType in this slice; never falsely Invalid |
| A supported complex simple-content type derives from a simple declared type | Unsupported/ValueType; changing simple content into a complex declaration/attribute projection is deferred |
| A known built-in lies outside the owned native type set | Unsupported/ValueType; no positive validity claim |

The graph must include integer -> decimal and complex simple-content extension
-> its actual simple base. This matters even for deferred valid derivations:
complex/simple mismatch alone is not a sound Invalid rule. Element-only complex
types have their implicit anyType ancestry; no arbitrary complex-to-complex
inheritance is added. A complex simple-content element cannot be weakened to its
simple content base via xsi:type. Existing named complex sibling definitions
remain unrelated, however similar their shape.

Attributes/block/final/abstract on schema definitions remain eagerly Unsupported
at compilation under the existing grammar. Their effects are not silently ignored
or approximated by the instance type selector. Same-family selections use the
same structural model and existing numeric/string assertion projection. Deferred
decimal-to-integer changes would need exact integer-to-numeric assertion handling;
they must not produce an empty numeric operand under a decimal declaration.

## Original facts and atomic evidence

The [pinned Core contract review](CORE-REVIEW.json) agrees on both published lanes.
ISchemaNodeFacts.Type means the original governing type of this assessment, which
is G. It does not require pretending G equals T. ISchemaAtomicProvenance.DeclaredType
means the current declared/governing type in that API, not the XSD PSVI's separate
original declared-type property. Preserve T through original declaration D.

| Evidence | Required identity |
|---|---|
| NativeElementState.Declaration and facts.Declaration | Original D, including original T; do not replace the declaration |
| Separate effective-type state, facts.Type, typedValue.GoverningType | G, an existing owned compiled component |
| Native node SchemaTypeAnnotation | G's actual expanded type name; no anonymous alias |
| Atomic provenance DeclaredType | G under the published Core contract |
| Atomic provenance ActualType | G for selected simple types; C for unchanged complex simple content |
| Atomic provenance OriginalAssessment | The same original advisor/environment/input/origin/snapshot episode |

Update all consumers of declaration-derived type metadata together: scalar
assessment/facets, native node annotations, HasNoTypedValue, facts, issued values
and provenance. Mere attribute whitelisting is insufficient. Root/descendant
application attribute declarations, paths and projections remain owned as before.
No reassessment, cross-session authority transfer or caller-supplied RequestedType
mode is enabled. Existing Core Bind fingerprints/profiles stay strict.

## Publication, limits and output

Finish request resolution, substitutability, selected-type lexical/facet checks,
structure and assertions before any original facts or snapshot are published.
Invalid/Unsupported creates no document and preserves later admission. Keep
bounded input/schema/occurrence/depth/type/document/cache/export/query capacities,
sequential operation gate, cancellation and revoking/waiting cleanup.

xsi:type is a processed validation request; its own QName typed-value/PSVI exposure
is outside this scalar slice. Application attribute counts do not gain it and
ReadAttribute on that special attribute remains Unsupported. Export preserves
parsed lexical XML and namespace bindings, not transferable authority. Export and
reassessment must select the same type afresh; no raw QName string confers trust.
Diagnostics remain input-free and reuse current stage/outcome/reason categories.
Existing other-attribute refusals must not be weakened to make this request work.

## Mandatory next gate

Before production edits: isolated actual-native feasibility on Core 1.81.2 and
1.81.17, with a genuinely narrower selected named type, original D/T/G identities,
invalid selected facets, same-type complex simple content, node-data atomization,
foreign/copy refusal and complete-failure no-publication. Capture prototype source
and runtime hashes. Only then record actual semantic adoption and implement.
No new pilot pass or general C45/XSD/PSVI acceptance is implied by this contract.
