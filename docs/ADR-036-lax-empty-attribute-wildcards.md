# ADR036: Lax attributes and bounded partial root projection

Status: qualified source candidate,27November2026.

Separate known global validation from declaration-absent lax attributes. Preserve declared schema components, but project partially validated empty roots to canonical anyType per XDM; unknown attributes have anySimpleType node types and actual untypedAtomic values. Keep general ancestor projection, skip and wider owner models separate. [Adoption and qualification](lax-empty-attributes/README.md). API and Core pin unchanged.
