# Text-only anyType members in bounded all roots

Both owned native facades admit direct local implicit or explicit xs:anyType members in existing all-group roots, including named-root aliases. Each child retains its original element declaration and canonical anyType governing component. Its actual value is one xs:untypedAtomic containing original XML text, including one empty string atom for an empty element. Comments/PIs do not contribute. Namespace declarations are allowed. No normalized scalar sibling value is substituted for the child's original text.

Expanded-name matching remains order independent, with duplicate and missing-required rejection. Facts and scalar indexes follow physical XML order. Optional absence produces no child facts, while a present empty anyType element produces one atom. Typed scalar siblings retain their actual types. The enclosing element-only root still has no typed value. Shared root aliases preserve schema component identity.

Terminal application/special attributes, xsi requests and descendant elements remain Unsupported before facts publish. Complex nillability/value constraints, nested all/choice containers, complex global references, anyType choice composition and broader inheritance/projection remain excluded. Existing root attributes, strict root wildcards and admitted structural assertions retain their earlier rules.

Retain16source members/16scalar states, UTF16 string and document/node/depth/attribute/identity/fact bounds. Original XML export, cancellation, foreign authority, failure recovery and expiry remain. Legacy decimal-only grammar remains unchanged.

For example, `<a> 001 </a>` under all reads as untypedAtomic text ` 001 `, while a decimal sibling `<d>01.20</d>` reads as decimal1.2. Reordering the elements changes their scalar indexes but preserves declaration matching. An empty `<a/>` reads as one empty string atom.
