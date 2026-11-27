# ADR052: Original anonymous duration bases

Status: qualified source candidate, 27 November 2026.

Admit actual compiled duration NativeType bases through the original unnamed direct base route. Reuse the existing duration facet compiler on original source elements, skipping only the exact inline child. Preserve exact month/second values, lexical family grammar, equality/partial ordering, fixed/facet/pattern ancestry, exclusions and limits without aliases or source clones. [Scope and qualification](anonymous-duration-bases/README.md). API and Core pin unchanged.
