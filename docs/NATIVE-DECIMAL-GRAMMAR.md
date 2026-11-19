# Native decimal backend grammar

Development M2B,19November2026: [named decimal restriction adoption](m2b-decimal-restrictions/ADOPTION.md) adds direct xs:decimal simple types and selected inclusive/exclusive range plus total/fractionDigits facets. Combined16types/16roots; named leaf identity preserved. Earlier restrictions below remain historical; no other datatype, chain, attribute/import or release expansion.

Development M2A,19November2026: the [named-complex-type adoption](m2a-named-types/ADOPTION.md) supersedes the anonymous/single-root restriction below for the local0.6candidate. Up to16named complex types/16roots; named simple types, derivation, attributes/imports remain excluded. Historical published profiles and receipts are unchanged.

Development M1,19November2026: the [namespace/optional-assertion candidate](m1-namespaces/README.md) supersedes the unqualified/mandatory-assertion restriction below for the local0.6candidate only. Published beta.1 and the original adoption remain historical at their pins. New exact grammar/boundaries: [M1 adoption](m1-namespaces/ADOPTION.md).


Date: 2026-11-15. Implementation basis: [adoption record](NATIVE-DECIMAL-SPECIFICATION-ADOPTION.md). This is the executable backend subset of the proposed [session profile](NATIVE-SESSION-PROFILE.md); the Core adapter remains separate. The first backend stays internal until provider qualification establishes a public entry point.

## Input and schema grammar

Inputs are already-owned .NET strings, encoded conceptually as strict UTF-8 without a BOM. Count bytes over the complete supplied string before XML parsing, including declaration, comments and whitespace. Limits are 32,768 schema bytes and 65,536 instance bytes, with a preliminary UTF-16 length check and a 256-descendant-node tree limit. These checks cannot retroactively bound the caller's string allocation. Parse with DTD prohibited, a null resolver and an XML character ceiling equal to the byte ceiling. No stream/tree ownership is acquired. Malformed XML is Invalid; DTDs and non-1.0 XML versions are Unsupported. Invalid Unicode is Invalid. Check cancellation before capture, after bounded parsing and during compilation/evaluation loops; cancellation throws.

Schema content, with exactly the indicated order:

```text
xs:schema(version?="1.1", xpathDefaultNamespace?="##local")
  xs:element(name=NCName)
    xs:complexType
      xs:sequence
        xs:element(name=NCName, type=QName(xs:decimal), minOccurs?=0|1, maxOccurs?=1..16){1..16}
      xs:assert(test=Expression, xpathDefaultNamespace?="##local"){1..8}
```

Occurrence defaults are one; min must not exceed max and the sum of maxima must not exceed sixteen. Local names are unique, although a local name may equal the root name. Resolve the type QName using the declaration's actual namespace context, not a literal prefix. No target namespace or other schema attributes/elements are admitted. Namespace declarations, XML whitespace, comments and processing instructions are allowed between schema elements; other text is refused. Schema element/attribute order follows XML rules except for the child order above. All omitted attributes and constructs are Unsupported; this backend does not certify arbitrary schema validity.

## Instance and value grammar

The root and its children are unqualified and match compiled declarations. Namespace declarations are allowed; xsi requests and other application attributes are Unsupported. Root content permits ordered child elements, XML whitespace, comments and processing instructions. Leaf content consists only of text/CDATA, with XML-whitespace collapse for decimal lexical validation; nested elements are Invalid and leaf comments/PIs are Unsupported. Unknown root/child names, wrong order, count, non-whitespace element-only root text and malformed decimal lexical values are Invalid. Mixed invalid/unsupported documents expose no assessed document regardless of the first diagnostic.

Decimal lexical form after collapse is `[+-]?([0-9]+(\.[0-9]*)?|\.[0-9]+)`. There is no exponent. Bound the original leaf text to 256 UTF-16 characters before collapse. Explicit positive coefficient-digit and nonnegative scale capacities are supplied to compile and retained by the immutable backend; assertion constants use the same limits. Every supplied lexical digit, including leading/trailing zeros, counts before normalization; scale counts supplied fractional digits. This is a conservative lexical capacity restriction. Construct exact coefficient/scale values without System.Decimal or binary floating-point conversion; refuse capacity overflow as Unsupported. Backend limits cap actual lexical lengths even when caller numeric limits are larger.

## Assertion grammar and evaluation

```text
Expression := Or
Or         := And ('or' And)*
And        := Primary ('and' Primary)*
Primary    := '(' Expression ')' | Presence | Operand Compare Operand
Presence   := ('exists' | 'empty') '(' DeclaredChildName ')'
Operand    := DecimalLiteral | DeclaredChildName | 'count' '(' DeclaredChildName ')'
Compare    := 'eq' | 'ne' | 'lt' | 'le' | 'gt' | 'ge'
```

Names are XML NCNames matching the complete compiled child name; function and comparison words are recognized by grammar position, so a child may be named count, exists, and or eq. Tokens are XML-whitespace separated or delimited by parentheses; decimal tokens admit an optional adjacent sign. Tokenize the entire expression with no skipped characters and require end of input. Limits: 512 UTF-16 characters, 128 tokens, sixteen nested parentheses/function argument pairs. Unsupported syntax/unknown child names/numeric capacities refuse schema compilation. Compile all assertions before assessment.

Use left-to-right short-circuit evaluation, with and binding tighter than or. A false left operand skips the right operand of and; true skips the right operand of or. An error actually evaluated fails the assertion and is never suppressed. Value comparisons atomize child decimal values: an absent operand yields an empty result (false at Boolean conversion); a repeated operand with more than one item is an assertion type error, even when the other operand is absent. count uses actual present children; exists/empty test presence. Numeric comparisons align bounded exact coefficient/scale values; no query arithmetic is added. A supported false assertion/type error makes the instance Invalid.

## State and status boundary

Successful assessment creates immutable present-only states referencing the compiled declaration/type objects. Each compiled schema owns distinct objects. The root has an anonymous complex type, no typed value and never-nil state; decimal leaves each have one exact value. Retain an immutable internal document representation and return caller-owned XML copies only. No Core handles, compatible views, engine leases or public native factory are supplied in this backend step.

P01/P02/P05/P08 are backend test groups, not overall session acceptance. Full provider identity, environment, ownership/concurrency, positive compatibility and release qualification remain pending. The internal backend adds no package-version commitment; MinVer and public package pins remain unchanged.

*Last updated: 2026-11-15*
