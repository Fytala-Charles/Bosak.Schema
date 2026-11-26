# ADR 029: Declaration-absent lax provenance

Status: qualified source candidate, 26 November 2026.

Publish null declaration evidence for unknown lax matches using original node identity. XDM notKnown maps to element anyType / attribute anySimpleType with untypedAtomic values, not absence-of-PSVI annotations. Known descendants remain recursively validated. Restrict first admission to anyType ancestor chains until partial-assessment type projection is separately qualified. [Adoption and qualification](absent-lax/README.md). Core pin and exported API remain unchanged.
