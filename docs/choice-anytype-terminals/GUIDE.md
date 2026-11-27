# Text-only anyType alternatives in bounded choice roots

Both owned native facades admit direct implicit/explicit xs:anyType local alternatives in existing bounded choice roots and named aliases. The first child selects one expanded-name branch, and only that branch may repeat within its existing minimum1/maximum16 occurrences. Mixed alternatives, unknown branches and exceeded occurrence bounds remain Invalid. Optional outer choice absence produces no child facts.

Repeated members retain their original declaration and canonical anyType governing component. Each actual value is one original untypedAtomic text atom, including an empty string for an empty element. Comments/PIs do not contribute; namespaces are allowed. Facts follow physical XML order. Scalar alternatives keep their actual types, and the enclosing element-only root retains no typed value. Shared aliases remain immutable.

Nullable alternatives, repeated outer choices, complex nillability/value constraints, terminal application/special attributes/XSI requests and descendants remain Unsupported before publication. Nested all/choice containers, global complex references and broader inheritance/projection remain separate. Existing root attributes, strict root wildcards, structural assertions and all-group anyType members retain their prior contracts.

Retain16source alternatives/max16selected occurrences, UTF16 string and document/node/depth/attribute/identity/fact bounds. Original XML export, cancellation, foreign authority, failure recovery and expiry remain. Legacy decimal-only grammar remains unchanged.

For example, three selected `<a>` members may yield original text ` 001 `, an empty string and `last` in that order. Mixing `<a>` with a scalar `<d>` remains Invalid. Selecting only `<d>01.20</d>` retains its decimal1.2 typed value.
