# Native text datatype candidate

The unpublished 0.10 development candidate adds `normalizedString`, `token`, `language`, `Name`, `NCName`, `NMTOKEN` and nonempty `NMTOKENS` to the owned native facades on signed Core 1.86.0. Named and anonymous string restrictions support inherited finite length, enumeration, pattern and whitespace facets. Derived text values use the existing `NativeScalarKind.String` and `StringValue`; their actual governing/member type remains separate native Core provenance. No public API member is added or removed.

Whitespace processing follows XML Schema: preserve retains parsed text; replace changes only tab, carriage return and linefeed to spaces; collapse also removes boundary spaces and reduces internal runs. This runs before lexical-family and facet checks. XML parser normalization happens first. Non-XML whitespace and Unicode spelling are not normalized. Typed values and assessed node string facts use normalized text; exported parsed XML retains the original content and does not promise byte preservation.

```csharp
using Bosak.Schema;

var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Code">
    <xs:restriction base="xs:token"><xs:enumeration value="invoice ready"/></xs:restriction>
  </xs:simpleType>
  <xs:element name="status" type="Code"/>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 256, 16, 256, 256);
var compiled = NativeSchemaSession.Create(schema, options);
if (compiled.Outcome != NativeSchemaOutcome.Valid) throw new InvalidOperationException("Schema refused.");
using var session = compiled.Value!;
var assessed = session.Assess("<status>  invoice   ready  </status>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid) throw new InvalidOperationException("Instance refused.");
using var document = assessed.Value!;
Console.WriteLine(document.ReadRootScalar().StringValue);
Console.WriteLine(document.ExportXml().Contains("  invoice   ready  "));
```

The output is `invoice ready` and `True`. The original-node facade uses the same normalization and immutable detached values. Existing scalar/item accessors, explicit assessment contexts, cancellation, sequential-operation gates, owner disposal and cache/query limits remain applicable.

`language` uses the specification's ASCII tag grammar, without language-registry lookup or case folding. `Name`, `NCName` and `NMTOKEN` use the adopted XML 1.0 Fifth Edition character ranges, including supplementary characters. Name and NMTOKEN permit colons without QName namespace resolution; NCName excludes them. NMTOKEN permits all NameChar characters at the first position. These types do not imply ID/IDREF/ENTITY identity-graph semantics.

`NMTOKENS` is a list with at least one NMTOKEN. It preserves ordered actual item provenance and requires explicit item reads, even when singleton. Existing user-defined lists/unions can use text-family members; union member order and Unsupported propagation remain unchanged. Type names and foreign namespaces retain their canonical component identity through existing closed catalogs.

Existing scalar, sequence, choice, all, nested, attribute, reference and group placements apply. String simple-content extensions now work within the existing finite owner grammar. Complex restrictions support the earlier finite enumeration/pattern route with actual atomic-content identity. Their broader length/whitespace/fixed syntax, attributes and restriction chains remain excluded. Complex value constraints and arbitrary assertions are still Unsupported.

Length counts Unicode scalar values after normalization. Restriction chains preserve original bounds and fixed flags; whitespace may only strengthen. The normative inherited length/minLength/maxLength combination is admitted only with its qualifying ancestor, as explained in [facet review](FACET-REVIEW.md). Enumeration compares normalized string values using ordinal identity, including Unicode spelling and case. Pattern source whitespace is retained, same-step patterns are alternatives and inherited patterns are conjunctions.

Whitespace strengthening over inherited enumeration/pattern constraints, and enumeration over patterned ancestors, remain Unsupported pending separate value-space qualification. New builtin text strings retain the caller's parsed raw `MaxStringCharacters` limit; they do not acquire the 256-character compound limit. Every text facet literal, including pattern and whitespace keywords, is also bounded by that caller capacity. Length-facet lexical forms retain a separate 256-character bound. There are at most 16 enumeration declarations and eight patterns per restriction; existing pattern state/work, compound, component, input and inline limits remain. Failed or cancelled reads release their operation gate.

Provided default/fixed metadata uses complete normalized values. Missing-content/attribute augmentation remains Unsupported. Simple nil is distinct from an empty string and from an empty list; an empty NMTOKENS value is Invalid. No external schema resolution, arbitrary regex/assertions, general PSVI/C45, public query factory or registry publication is included. Earlier sealed packets retain their original scope and receipts.
