# Derived integer built-ins: bounded contract

22 November 2026. [PR41](https://github.com/Fytala-Charles/Bosak.Schema/pull/41)
merged at `f9e9ad7c0599ba7b36b6d8d71704875990b54b98`, preserving the reviewed tree.
Main CI45039993274 passes 3,185 source tests plus release, documentation and workload
controls. The previous simple-root qualification and original evidence remain sealed.
Frozen pilot: 21 matches. Expanded sample: 3 matches / 126 Unsupported, zero errors;
these are prior measured results, not execution of this proposal.

The next slice proposes all twelve built-in integer descendants under existing
bounded exact integer transport. It covers their original type identities, finite
built-in derivation graph, inherited bounds and fixed integer semantics. Existing
placements and supported facets are reused; enumeration and other datatype/facet
breadth remain independent work.

[Contract](CONTRACT.md), [normative type table](BUILTIN-TYPES.json),
[specification review pins](SPECIFICATION-REVIEW.json),
[exact Schema source review](SOURCE-REVIEW.json), [exact Core source review](CORE-SOURCE-REVIEW.json),
[158 proposed XML payloads](CASE-PLAN.json), [18 actual-native gates](NATIVE-GATES.json).
The payloads and gates are unexecuted. Source review and plain BigInteger construction
do not prove native acceptance or original derived-type provenance.

Next: isolated native feasibility on signed Core 1.81.2/1.81.17, retaining Unsupported
if original identity cannot be represented faithfully. Probe the unchanged original
int fixtures separately; do not predict corpus admission from datatype names alone.
Production adoption, full source/package/retained/corpus qualification and Git
delivery follow separately. No product source, public API, Core pin, package,
controller or registry change here.
