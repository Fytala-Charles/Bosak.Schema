# ADR 030: Strict attribute wildcards on empty owners

Status: qualified source candidate,26November2026.

Admit strict/default-strict anyAttribute independently from lax/skip and partial-assessment projection. Explicit uses take precedence; matching extras must resolve to original global declarations and validate supplied values. Preserve empty-owner typed values and canonical attribute authority. Namespace constraints and actual document bounds are explicit. [Adoption and qualification](strict-attributes/README.md). No Core pin or exported API change.
