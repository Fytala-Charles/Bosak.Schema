# ADR048: Original anonymous date restriction bases

Status: qualified source candidate, 27 November 2026.

Dispatch on the actual compiled xs:date NativeType. Preserve its original unnamed direct base link and compile exact original facet elements, skipping only the inline base object. Reuse bounded date equality/ordering/timezone/pattern ancestry and existing exclusions/limits. [Scope and qualification](anonymous-date-bases/README.md). API and Core pin unchanged.
