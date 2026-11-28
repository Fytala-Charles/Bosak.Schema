# ADR055: Original anonymous floating bases

Status: qualified source candidate, 28 November 2026.

Admit actual float/double NativeType bases through the original unnamed direct base route. Reuse the existing floating facet compiler on original source elements, skipping only the exact inline child. Preserve IEEE family mapping/rounding, NaN/infinities/signed zero, fixed/value/lexical facet ancestry, exclusions and limits without aliases or source clones. [Scope and qualification](anonymous-floating-bases/README.md). API and Core pin unchanged.
