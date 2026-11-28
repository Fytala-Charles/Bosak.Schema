# ADR059: Closed attributes after a bounded default-group opt-out

Status: qualified source candidate, 28 November 2026.

Carry a private source-bound closed-attribute policy for explicit schema-default opt-outs with zero uses/no wildcard in existing empty/finite-sequence plans. Preserve it through root aliases/references and reuse established declared-use/wildcard validation. Preserve original objects and all special-request, other-profile, limit and lifetime boundaries. [Scope and qualification](default-optout-attributes/README.md). No public API/Core pin or publication change.
