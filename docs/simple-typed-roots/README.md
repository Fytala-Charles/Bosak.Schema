# Simple-typed global roots: bounded contract

22 November 2026. Main `1f2cca34ce8dc93b25f11b402bbe60330b8ee1cf` merges
[PR40](https://github.com/Fytala-Charles/Bosak.Schema/pull/40); main CI45027251051
passes 3,050 source tests plus release, documentation and workload controls.
Its tree equals the reviewed head. Pattern source/package/corpus evidence remains
sealed; frozen pilot is 21 matches, expanded sample 3 matches / 126 Unsupported,
with zero mismatches or harness errors. Those counts describe the pattern candidate,
not execution of this proposal.

The next slice addresses global elements whose declared type is an already supported
simple scalar. Both flat and nested compilation currently require complex roots.
The proposed profile covers built-in decimal, integer, boolean and string, existing
named direct restrictions, and direct anonymous restrictions already supported on
local elements. It reuses the current opt-in scalar and original-node facades.
The legacy decimal-only session keeps its existing grammar.

[Contract](CONTRACT.md), [specification review pins](SPECIFICATION-REVIEW.json),
[exact Schema source review](SOURCE-REVIEW.json), [exact Core source review](CORE-SOURCE-REVIEW.json),
[38 proposed XML payloads](CASE-PLAN.json), [17 proposed native gates](NATIVE-GATES.json).
These are unexecuted plans. No implementation or semantic adoption is claimed.

The original public SKU retains its `\d` escape and remains outside the pattern
subset even if simple roots become available. The authored ASCII `[0-9]` example
is separate and must never replace the original fixture or its expectation.
No sample-admission forecast or whole-XSD coverage percentage is implied.

Next: isolated actual-native feasibility on both signed Core pins. Preserve any
Unsupported outcome if faithful original declaration, governing type and atomic
provenance cannot be issued through the public Core contracts. Production adoption
and full source/package/retained/corpus qualification follow separately.
