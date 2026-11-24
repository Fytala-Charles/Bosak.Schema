# Specification priorities after root particles

24 November 2026. PR [#44](https://github.com/Fytala-Charles/Bosak.Schema/pull/44) merged as `b08187d5f59f47a068737154ecdd6872506d3a88`, preserving reviewed `76c4baaf174f590a1749287b91893d2994c7fa62`. [Main CI45382084851](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45382084851) passes **3,885 source tests** (1,251 internal / 2,634 public), build, release-refusal, documentation and workload checks. All earlier packets retain their original states and hashes.

## Recommendation

Prepare a separate contract for **bounded integer enumeration facets**, then qualify an isolated native candidate before production adoption. Two unchanged public int-enumeration schemas isolate this missing constraining facet; exact integer values and existing component/lifetime transport are already qualified. This continues specification breadth without requiring an immediate new public scalar family. [Next contract requirements](NEXT-CONTRACT.md).

The current [engineering ranking](RANKING.json) puts date/time primitive/transport design next, followed by wider bounded regex escapes, closed schema assembly and implicit-anyType/mixed-content design. Lists/unions, augmentation, conditional typing and identity/substitution remain separate large areas. Rank reflects dependency cost and the reviewed public sample, not customer demand, a normative denominator or a pass-count forecast. No new semantic adoption or implementation occurs in this assessment.

## Evidence actually available

The latest full selected sample was executed on **23 November**, during the qualified all-group candidate. This assessment reuses those exact receipts; it does not label them as a new run. Both signed Core pins have identical rows: **129 cases, 8 matches / 121 Unsupported / zero mismatches or harness errors**. The frozen pilot remains 21 matches. Unsupported is never counted as a pass, and no full-suite percentage is justified. [Current sample and bucket ledger](CURRENT-SAMPLE.json).

The 18 buckets still include nine unsupported date/time cases, six floating-point, six QName/binary, eight assembly, nine list and six union cases. These are case labels, not feature-isolated opportunities. [The static co-feature audit](STATIC-COFEATURES.json) preserves each original schema hash and records lexical type references, restriction bases, facets, include/import sites and other syntactic indicators. It is not component resolution or Schema validation; a case can require several missing features.

The older [ranking](../expanded-conformance/RANKING.json) remains historical. Bounded string patterns, simple-typed roots, twelve integer descendants and direct root choice/all have since been delivered within explicit limits. [Delivered-slice overlay](CURRENT-SLICES.json) records their bounds. The historical shared 24-area matrix remains unchanged; no whole area is declared fully conformant by these slices.

## Why this order

| Priority | Next work | Dependency and practical limit |
|---|---|---|
| 1 | Integer enumeration | Existing BigInteger transport; exact value equality, base validity, inherited constraints, capacities and original identity still need their own contract |
| 2 | Date/time primitives and transport | Useful broader scalar foundation; the public detached scalar kind currently admits only string/decimal/Boolean/integer. Constructor availability does not prove Schema validation |
| 3 | Wider regex escapes, starting with digit semantics | Original SKU still uses an explicitly refused digit escape. Review Unicode Nd, supplementary values, complements, data version and bounded NFA costs; do not replace the original with an ASCII rewrite |
| 4 | Closed include/import assembly | Requires host-owned resolver closure, graph identity/cycles and bounded acquisition; assembly alone does not remove unrelated datatype/structure blockers |
| 5 | Implicit anyType and wider mixed/particle content | Six original ranked particle examples remain co-blocked despite bounded choice/all support. Model content/typed values accurately, with no fabricated simple-string type |
| 6 | Lists/unions, augmentation, alternatives and identity/substitution | Separate multi-atomic, original-source versus augmentation, conditional and graph contracts |

The original [two integer examples](INTEGER-ENUMERATION-EXAMPLES.json) retain exact schema/instance/metadata hashes and their measured Unsupported outcomes. They are candidates for unchanged-original probes, not promised admissions. Two fixture outcomes are not a reason to claim all enumeration semantics, all integer values or full facet support.

## Bosak / Schema boundary

Schema owns lexical/value/facet enforcement and original validation facts. Core owns neutral XDM/typed-value transport and query execution. For integer enumeration, first try the existing public BigInteger and original-type path; do not assume a new Core capability is needed or that transport success validates the facet. For new calendar or untyped/mixed families, review public representation, timezone absence, extended years and ownership before proposing an API extension. Reproduce any missing neutral capability through public APIs and route it under REQ-142; no private/friend/reflection bypass or mandatory Core-to-Schema dependency is introduced.

[Six exact merged source snapshots](SOURCE-REVIEW.json) support the ranking. [Immutable specification review pins](SPECIFICATION-REVIEW.json) separate review date from publication and semantic adoption. All **311 production/test/tool/build inputs remain unchanged**. This is a reversible planning update in `D:/Development/Bosak/.worktrees/Schema.Coverage`, branch `planning/post-particle-coverage`; no new commit/push, package, Core pin, tag, controller or registry change. Next is the integer-enumeration contract, not an implementation or release claim.
