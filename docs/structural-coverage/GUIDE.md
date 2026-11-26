# Bounded structural coverage

26 November 2026. Development Schema 0.14, signed Core 1.86.0. Both owned facades accept empty direct `all` and `sequence` with minimum 0 or 1 and maximum 1, and empty direct `choice` with minimum 0 and maximum 1. Root/local and named/anonymous owners retain the actual governing type. Empty content has a supported zero-item typed value: scalar reads return `NativeScalarKind.Empty`; item reads return an empty array. Text (including whitespace) and element children are Invalid. Nil and element-only FOTY0012 remain distinct.

Local/global attributes on already admitted owners can declare one anonymous simple type instead of a `type` QName. Existing restriction, list/union, normalization, facet and provided-value constraint semantics apply. References and groups share original source identities; unrelated anonymous definitions remain distinct and never acquire invented QNames. Attribute source sites now share the existing 16-site anonymous budget, including unused definitions. Other source, component, value, lifetime and cancellation limits remain.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="r">
    <xs:complexType>
      <xs:sequence><xs:element name="v" type="xs:string"/></xs:sequence>
      <xs:attribute name="a">
        <xs:simpleType>
          <xs:restriction base="xs:NMTOKEN">
            <xs:enumeration value="A-B"/>
          </xs:restriction>
        </xs:simpleType>
      </xs:attribute>
    </xs:complexType>
  </xs:element>
</xs:schema>
```

`<r a="A-B"><v>x</v></r>` is Valid; `a="A B"` is Invalid. The original lexical attribute remains in exported XML. Detached values survive document disposal; authority-bearing facts and document reads expire.

Required empty choices, zero/general maximum occurrences, empty-owner attributes, mixed empty models, annotations, general derivation, ID/IDREF/ENTITY/NOTATION semantics and missing-value augmentation remain excluded. Instance comments/processing instructions still refuse nested assessments. No public API or Core pin change is made; this candidate is separate from publication and full XSD 1.1/C45 acceptance.
