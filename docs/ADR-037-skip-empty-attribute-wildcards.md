# ADR037: Skip attributes and empty-root projection

Status: qualified source candidate,27November2026.

Match namespace/QName constraints and explicit uses before skipping extras. Issue no node declarations or global validation evidence for skipped attributes, even when globals exist. Reuse canonical root projection without mutating declared components; keep ancestor projection and wider owners separate. [Adoption and qualification](skip-empty-attributes/README.md). API and Core pin unchanged.
