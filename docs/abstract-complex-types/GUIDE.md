# Abstract global complex types

Both owned native facades admit abstract on existing supported global named complex types. XML Boolean true/false/1/0 values use whitespace collapse and a256UTF16 raw-source bound. Malformed admitted values are Invalid; excess is Unsupported. Flags on unused definitions are checked eagerly. Omitted/false/0 preserve ordinary behavior.

Abstractness is frozen by original compiled type identity, separately from declaration names, simple content atomic types and unrelated anonymous locals. A type shared by several declarations affects all of them. Abstractness is not inherited: a concrete restricted type derived from an abstract base remains concrete.

After admitted instance-type resolution returns the actual type, an abstract complex governing type makes assessment Invalid before nil/content/facts are admitted. This applies to roots, simple-content owners, references, nested containers and admitted known-lax matches. Failure publishes no facts; later concrete assessment recovers without changing metadata. Original declarations, governing/atomic types, normalized values and original XML remain intact for valid cases.

Existing self xsi:type selection is checked. Broader complex xsi:type overrides remain Unsupported, including a concrete derived override of an abstract declared type. The declared type is therefore not rejected before existing resolution. Complex nillability, local complex-type abstract and known-lax all/choice remain excluded. Element-declaration abstractness is a separate earlier check. No public abstract accessor, general derivation or full PSVI/C45 acceptance is added. Legacy grammar, finite bounds, no augmentation, cancellation, foreign authority and expiry remain.

For example, an empty global complex type declared abstract="false" can govern an empty root, whereas abstract="true" compiles but makes direct use Invalid. Separately, a concrete decimal simple-content restriction of an abstract simple-content base can govern its own declared element; selecting it through a broader complex xsi:type override remains Unsupported.
