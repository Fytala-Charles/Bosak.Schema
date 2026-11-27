# ADR040: Abstract global declaration identity

Status: qualified source candidate,27November2026.

Freeze abstract global declarations in an immutable identity set after compilation. Enforce canonical declarations at root/scalar/container/known-lax dispatch before type/nil selection, without mutating shared types or matching local names. Keep substitutions, abstract complex types and public metadata breadth separate. [Adoption and qualification](abstract-elements/README.md). API/Core pin unchanged.
