# Bounded global simple-type final controls

27 November 2026. Adopted before implementation at da7019c8ff03c14f8a11aaa423269710abfac2bf. See ADOPTION.json for immutable specification identities.

The opt-in native profile admits explicit final on global named simple types. XML whitespace separates restriction, extension, list and union; the empty set and #all are supported. Duplicate list tokens have set semantics. Other tokens and mixed #all lists are Invalid. Raw source capacity is 256 UTF16 units; excess is Unsupported.

Validate direct derivation edges against the final set of their source-bound named simple type before compilation. This includes restriction bases, list itemType, every union memberTypes entry and simple-content extension/restriction bases, also inside inline definitions, unused types and forward references. Prohibited derivations are Invalid and publish no compiled schema. An unrelated method remains permitted, and using a final type as an element/attribute value is permitted. Final does not propagate from a base to its derived type. Original governing types, actual atoms and declaration authority remain unchanged.

finalDefault, local simple-type final and complex-type final remain Unsupported. Existing unsupported schema shapes remain outside scope; this is not general derivation validation or full XSD/PSVI/C45 acceptance. Legacy grammar, assembly/namespace/QName handling, finite bounds, no augmentation, cancellation and session authority remain. Qualify happy and edge cases through both facades and native evidence, full source/exact fresh package, original samples, API/signed Core, invoice, docs and release controls before reviewed delivery and green merge/main CI. No publication included.
