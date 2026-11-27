# ADR044: Documentation language union and opaque payloads

Status: qualified source candidate,27November2026.

Admit xml:lang only on supported documentation wrappers. Validate per-member union whitespace semantics: language collapses, exact empty string preserves. Pin the dated XML namespace schema separately from its mutable latest alias. Keep payloads opaque and instance/component semantics unchanged. Preserve other wrapper/PSVI exclusions and reconcile MK-084 without rewriting its fixture. [Adoption and qualification](annotation-language/README.md). API/Core pin unchanged.
