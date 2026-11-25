# Native datatype composition candidate

The 0.9 development candidate supports bounded `xs:hexBinary`, `xs:base64Binary`, `xs:QName`, lists and ordered unions through both owned native facades on signed Core 1.86.0. This is an unpublished candidate; qualification status is tracked in the batch ledger.

Binary values expose `NativeScalarKind.HexBinary` or `Base64Binary` and immutable decoded `BinaryValue` octets. QName values expose `NativeScalarKind.QName` and detached `QNameValue` components: namespace URI, local name and original prefix. These detached values remain usable after disposal and carry no schema authority. Other-family getters throw `InvalidOperationException`.

The admitted existing root, sequence, choice, all, nested and attribute placements retain their structural restrictions. Named and anonymous simple restrictions support enumeration, finite patterns and fixed-collapse whitespace. Binary length/minLength/maxLength measure decoded octets, with bounded inherited narrowing and fixed length facets. QName enumeration compares expanded names; each literal uses the namespace bindings on its original schema facet, including closed imports. Instance QName values use their own element or attribute owner's namespace bindings. Prefix spelling does not change QName equality.

Simple-content extensions retain their existing owner restrictions. Complex simple-content restrictions admit the existing enumeration/pattern grammar with original namespace context; the earlier complex restriction gate still excludes length/whiteSpace/fixed facet syntax and restriction attributes. Do not infer the full direct simple restriction grammar for that route.

Every new-family supplied value is limited to 256 UTF-16 code units before whitespace normalization and to the caller's `MaxStringCharacters`. Binary values are also limited to 128 decoded octets; QName namespace URIs are limited to `MaxStringCharacters`. Enumeration and pattern counts remain 16 and eight per restriction. Pattern work shares existing compilation/assessment accounting and cancellation. Base64 accepts only XML whitespace, valid padding and zero unused pad bits. Hexadecimal accepts boundary whitespace, even ASCII hexadecimal digits and no internal separators.

Original XML is preserved; binary transport uses a canonical encoding solely for its immutable typed payload. No default content or missing attributes are augmented. Binary schema equality retains separate primitive value spaces for hex and base64; it does not reuse broader XPath comparison behavior. QName length facets and enumeration restrictions over patterned interchange ancestors remain `Unsupported` pending separate qualification. General assertions, broader regex grammar, arbitrary recursive graphs, external resolution and full PSVI/C45 acceptance remain outside this candidate.

Use the same explicit `NativeSchemaSessionOptions`, assessment context and owned-document lifetime as the [integration guide](../INTEGRATION.md). Read a value through `ReadRootScalar`, `ReadScalar`, the attribute methods, or original-node `ReadElement`/`ReadAttribute`; inspect `Kind` before using its getter. Existing enum values and getters retain their identities. Future family values require consumer handling.

The [contract ledger](CONTRACTS.json) separates planning from qualification. A passed generated catalog alone does not qualify the full batch. Source, fresh package-only, authority/lifetime and frozen corpus checks precede atomic PR delivery. No registry publication or release controller dispatch is authorized by completing this implementation packet.

## Lists, unions and explicit item reads

Use `ReadRootItems`, `ReadScalarItems`, `ReadAttributeItems` or `ReadDescendantAttributeItems` on `NativeSchemaDocument`; use `ReadElementItems` or `ReadAttributeItems` on `NativeSchemaNodeDataDocument`. They return immutable ordered arrays of detached `NativeScalarValue` items. Reads run and materialize inside the existing Core operation, cancellation, cache, typed-atom and owner-lifetime gates. No lazy Core sequence or authoritative handle escapes.

```csharp
using Bosak.Schema;

var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Numbers"><xs:list itemType="xs:integer"/></xs:simpleType>
  <xs:element name="values" type="Numbers"/>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 256, 16, 256, 256);
var compiled = NativeSchemaSession.Create(schema, options);
if (compiled.Outcome != NativeSchemaOutcome.Valid) throw new InvalidOperationException("Schema refused.");
using var session = compiled.Value!;
var assessed = session.Assess("<values>1 2 3</values>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid) throw new InvalidOperationException("Instance refused.");
using var document = assessed.Value!;
Console.WriteLine(string.Join(",", document.ReadRootItems().Select(item => item.IntegerValue)));
Console.WriteLine(document.HasRootScalar);
```

The output is `1,2,3` and `False`. List-variety values require item reads even when empty or singleton; scalar accessors refuse them. A union selecting a list keeps that boundary. An atomic value can be read as one item. Nil and empty lists both yield zero items; `IsRootNilled` or original-node `IsNilled` distinguishes them. A container without a typed value still refuses with FOTY0012. Existing descendant counts retain their value-bearing-node indexing; one list-bearing element occupies one index, independent of its item count.

Definitions may use named or inline list items, named/inline ordered union members, closed forward dependencies and existing import catalogs. Lists can contain admitted atomic-only unions, and unions can contain lists. Lists of lists, including transitive union members containing lists, are Invalid. An empty union definition is legal but accepts no lexical value. Selecting a named restriction of a compound type through `xsi:type` follows existing ancestry rules; direct union-member override remains Unsupported pending separate facet/selection qualification.

Union assessment tries members in declared order. Invalidity permits trying the next member; an Unsupported representation/work result stops assessment. Selected atomic types and list-item types remain actual provider-owned provenance while the governing list/union component is preserved. That provenance is separate from value equality. Decimal/integer values compare exactly across their shared primitive space, QName prefixes do not affect equality, different binary primitives remain disjoint, and a singleton list can equal an atomic value.

Lists support item-count length/minLength/maxLength, enumeration, finite patterns and fixed-collapse whitespace. Unions support finite patterns and enumeration. Inherited finite restrictions, provided fixed/default metadata and the existing simple-content extension route retain their placement exclusions. Complex simple-content restrictions admit the earlier enumeration/pattern grammar; broader complex length/whiteSpace/fixed syntax, attributes and restriction chains remain excluded. New compound fixed-attribute overrides compare schema value equality; earlier atomic date/calendar override behavior remains a separately documented legacy limitation.

Limits are 16 list items, eight direct union members, 16 inline member definitions and inline depth four, in addition to the existing global-component and node bounds. Every raw compound lexical value is limited to 256 UTF-16 code units and caller string capacity. A shared budget permits at most 4096 compound attempts per document or schema-literal compilation, also consuming existing pattern work and checking cancellation. Eager validation includes unused members. Namespace URI and atomic member capacities still apply. Set `MaxTypedResultAtoms` and cache capacity to accommodate the intended item count; the one-atom settings used by scalar examples intentionally refuse larger lists.

Enumeration over patterned compound ancestors, arbitrary assertions, general recursion, identity-graph builtins, full regex, default augmentation and general PSVI/C45 remain outside this candidate. The atomic packet stays sealed separately from composition qualification. See the [source review](COMPOSITION-SOURCE-REVIEW.md) for immutable source references, the empty-union planning correction and explicit adoption-label clarifications.
