# Bounded skip attribute wildcards on empty owners

Both owned native facades admit one processContents=skip anyAttribute on existing empty complex owners. Namespace/QName exclusions still match. Explicit attribute uses take precedence and validate normally; matched extras are skipped even if a same-name global declaration exists.

A skipped extra carries no node declaration, has xs:anySimpleType node type and one actual xs:untypedAtomic value from original XML-normalized text. Global datatype, fixed and ID rules are not applied to that extra. Its global declaration remains resolvable as a schema component. Schema declarations must still compile correctly, and missing values never augment XML. A skipped ID supplies no ID evidence for an explicitly validated reference.

Any skipped extra at an empty root makes assessment partial: the assessed root projects to canonical xs:anyType and one empty string atom. Original root declarations and declared empty complex components remain unchanged. Without extras, the original empty type and empty typed sequence remain. Shared plans and aliases are immutable across assessments.

Original empty-content validity runs before projection. Character/element children remain Invalid; comments/PIs remain allowed. Any skipped extra beneath a strict ancestor remains Unsupported before publication, while no-extra nested empty owners remain admissible. Skip on nonempty/mixed/simple-content owners, wildcard assertions, broader ancestor projection and inheritance remain excluded. Strict and lax behavior remain unchanged.

Prior document64nodes/depth4/64application attributes/16per owner, eight256UTF16 namespace/QName tokens, scalar/string/list/identity/fact bounds and XML/ownership/cancellation/expiry rules remain. Legacy decimal-only grammar remains unchanged.

For example, a globally declared integer `a="bad"` is accepted as raw untypedAtomic text when matched by skip. The same attribute explicitly declared on the owner remains Invalid. A root with no matched extras keeps an empty typed sequence; a root with skipped extras yields one empty string atom.
