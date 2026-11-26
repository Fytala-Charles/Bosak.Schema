# Bounded validation-root IDs and references

26 November 2026. Development Schema0.16 on signed Core1.86.0.

Both owned native facades admit `xs:ID`, `xs:IDREF` and nonempty `xs:IDREFS` in already supported declarations, restrictions and list/union constructions. ID/IDREF reuse the XML1.0 NCName lexical rules and fixed whitespace collapse. Values remain ordinary detached String scalars/items with original governing and actual-member provenance; IDREFS requires explicit item reads and is never silently reduced to one scalar.

After local validation, each assessment builds its own ID/IDREF table before publishing any document or original facts. Forward and backward references work within that captured XML root. An ID attribute identifies its owner element; ID-valued element content identifies its parent within validation scope. Repeating an ID binding for the same owner is allowed; binding the same ID to different owners is Invalid. A standalone ID root has no parent binding and is Invalid unless another admitted ID value supplies a binding for that same string within the root. Nilled values do not contribute identity atoms.

Only actual ID/IDREF atomic members participate. A union which selects string first does not supply an ID simply because another member is ID. Ordinary string/NCName/NMTOKEN attributes named `id` are not ID declarations. IDREFS and admitted custom lists retain every selected item; repeated references are allowed. No state leaks across active documents, failures or parallel assessments of one compiled schema.

Identity raw lexical and enumeration-value text is capped at256UTF16 units and caller capacity. A table permits128distinct identity strings and256identity atomic occurrences. Existing XML/node/component/attribute/list/typed-value limits remain. Capacity refusal is Unsupported without truncation or union fallback that hides the refusal. Missing references, unbound root IDs and conflicting owners are Invalid and publish no owned document/facts.

Default/fixed constraints on identity-containing types remain Unsupported, including compound types containing identity members; lexical/value-only facet literals do not run document graph checks. ENTITY/ENTITIES, NOTATION, DTD and special xml:id processing, external references, key/keyref/unique, public ID-table/PSVI projection and general fn:id/idref integration remain outside this batch. Legacy decimal-only refusal boundaries and public API shape are unchanged. Internal diagnostics distinguish missing/conflicting bindings; public diagnostic codes/messages retain their fixed input-free/redacted contract.

For example, `<r id="a" alias="a" ref="a"><v>x</v></r>` is Valid when id/alias are ID attributes, ref is IDREF and the finite content model admits v. Reusing `a` as an ID on a different element is Invalid. `<r ref="a"><id>a</id></r>` can be Valid when the child id is typed ID: it identifies its parent r. `<id>a</id>` assessed as a standalone ID root is Invalid.

This is bounded validation-root semantics, not complete XSD1.1/identity-constraint/PSVI/C45 acceptance or a registry publication.
