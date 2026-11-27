# Bounded lax attribute wildcards on empty owners

Both owned native facades admit one processContents=lax anyAttribute on the existing empty-complex owner profile. Known extra attributes validate against original globals, preserving actual type, fixed and identity rules. Namespace/QName exclusions and explicit-use precedence remain. Lax never skips validation of an invalid known global.

An undeclared attribute at an empty root has no declaration. Its node type is xs:anySimpleType and its actual typed value is one xs:untypedAtomic containing original XML-normalized text, including whitespace. Partial validation projects the assessed root to canonical xs:anyType, yielding one empty untypedAtomic string atom. Original root declarations and declared empty complex types remain schema components. With no unknown extras, the owner keeps its original empty type and empty typed sequence. Named-root aliases share immutable plans; projection belongs only to each assessment and cannot contaminate later known-only reads.

Original empty-content validation runs before projection. Character or element children remain Invalid, while comments/PIs remain allowed. Missing attributes never augment XML, and failure publishes no partial result. Known-only lax attributes work on existing nested empty owners; unknown extras beneath strict ancestors remain Unsupported pending a separate ancestor-projection contract.

Skip, lax wildcards on nonempty/mixed/simple-content owners, wildcard assertions and wider inheritance/projection remain excluded. Existing document limits remain:64nodes, depth4,64application attributes,16per owner; eight namespace/QName tokens of at most256UTF16 characters; prior scalar/string/list/identity/fact bounds. Source binding, cancellation, foreign authority, export and expiry remain. Legacy decimal-only grammar remains unchanged.

For example, a known integer attribute `a="001"` reads as integer1 and preserves an empty typed root. Adding undeclared `u=" 01.20 "` reads that original text as untypedAtomic and changes the assessed root to anyType with one empty string atom. An invalid known `a="bad"` still makes the document Invalid.
