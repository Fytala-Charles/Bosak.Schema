# ADR041: Explicit global simple-type final controls

Status: qualified source candidate,27November2026.

Check method-specific direct source-bound derivation edges before compilation publishes any type. Keep explicit final sets local to the original global simple definition; ordinary value use and unrelated methods remain valid. Retain existing grammar boundaries for defaults, local and complex controls. [Adoption and qualification](simple-type-final/README.md). API/Core pin unchanged.
