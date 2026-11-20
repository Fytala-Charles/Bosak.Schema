# Bounded string assertion adoption

20 November 2026, recorded before production source edits. Product base:
`2b4a3497232da3cee20be0d4f561a853baa23aa8` (merged PR32). Core minimum 1.81.2
and separate qualification 1.81.17 stay pinned. This is an internal bounded
semantic extension of the existing mixed native profile; no new public API.

## Exact immutable normative revisions

[SPECIFICATION-SOURCES.json](SPECIFICATION-SOURCES.json) records retrieved body
SHA-256, byte counts, dated URLs and adopted sections. Structures REC20120405:
`5ffb773f81fb81cfc9ac54dfb871c333cefb12fbdffe4caeb28170054b21fc6a`;
XPath 2.0 Second Edition REC20101214:
`8f570fb5f09ee66b5a645a7d7e3f185d6314379eb2c4d80ac944dc66d2fe8e0a`;
F&O 2.0 Second Edition REC20101214 (retrieved body notes revision 13 April 2015):
`6d85cf91b013989979335bb6c2d21f48ab193f8debe5ad67647bcbc6651d721f`.
These are actual semantic adoption pins, distinct from publication/review/test
fixture dates. The XSD assertion host uses XPath 2.0 semantics for this subset;
reviewed 3.1 behavior is not substituted for the host's normative reference.

## Admitted grammar and evaluation

Extend only flat element-only complex content in opt-in NativeSchemaSession and
NativeSchemaNodeDataSession. Reference declared immediate string children, both
primitive and existing named/anonymous direct string restrictions. Preserve typed
string values after XML parsing and admitted string facets, with no trimming,
case folding or Unicode normalization. QName resolution/default namespace rules
remain those already adopted for numeric assertions, including assertion-local
prefix shadowing and xpathDefaultNamespace inheritance/overrides.

Admit quoted XPath string literals with doubled same-delimiter escaping and
single/double quote delimiters. XML entities are processed once by the XML parser;
backslash has no XPath escape meaning. Literal contents are never retokenized.
Admit string-to-string `eq`, `ne`, `=` and `!=`, including literal-to-literal,
child-to-literal and child-to-child. Use Unicode codepoint equality, implemented
with ordinal equality for valid XML Unicode strings; do not use culture-sensitive
comparison or configurable collation. Numeric operands do not coerce to strings.

Value eq/ne admits zero or one atom per operand. Empty gives an empty comparison
whose assertion/logical effective Boolean value is false. Multiple items raise
XPTY0004 internally and produce Invalid assessment, publishing no document.
General =/!= is existential over all pairs, not first-item comparison or logical
negation: empty pairs give false; the same nontrivial sequences can satisfy both
operators. Preserve all existing exact-decimal comparison behavior and syntax.
String lt/le/gt/ge and numeric general comparisons remain Unsupported grammar.
Mixed string/numeric comparisons refuse compilation as AssertionGrammar.

Extend existing exists/empty/count child operands to admitted string children;
a present empty string counts as one item. Count remains numeric and may compose
with existing decimal comparisons. Existing and/or precedence and parentheses
apply; deterministic short circuiting remains. An unexecuted branch need not
raise a dynamic cardinality error. Boolean/integer assertion operands, attributes,
paths/predicates/functions beyond the above, nested/simple-content assertions,
new datatypes, nil/overrides, lists/unions, ordering, regex and arbitrary XPath
stay outside this slice. Unknown/ill-typed grammar is Unsupported, not acceptance
of a schema merely because its assertion happens to be true for one instance.

## Boundaries and qualification gates

Keep zero-to-eight assertions, 512 UTF-16 units per expression, 128 tokens,
16 expression nesting, existing input/schema/string/node/occurrence/admission
budgets, cancellation, diagnostics and refusal categories. Quoted literals are
one token under the same expression ceiling. False assertions and evaluation
errors are Invalid instances; unsupported grammar and representation limits
remain distinct Unsupported reasons. No partial original facts or snapshots.

The decimal-only facade retains its old grammar and semantics. Shared parser
implementation may be extended internally, but string names/literals must be
explicitly enabled only for the mixed profile. Existing node-data/static-context
and authority/lifetime guards are unchanged. No new public surface or Core change.

Qualify namespace/escaping/empty/cardinality/existential/culture cases, mixed
numeric logical composition, resource ceilings and original native facts. Preserve
all 1,615 previous source outcomes except explicitly reviewed authored refusal
probes if now admitted; do not silently rewrite expected outcomes. Run full source,
package-only minimum/current Core, retained 124/23/33 callers and exact API diff.
Run the unmodified hash-verified 21-case frozen corpus without modifying its
metadata/expectations/notices; the direct target is Assert/assert_025/assert_025_2.
A larger match count is evidence for those identities only, not full conformance.
Historical dossiers remain sealed. No commit/push/publication authorized here.
