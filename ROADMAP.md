<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema roadmap">
  <h1>Bosak.Schema Roadmap</h1>
  <p>Specification coverage guided by unchanged upstream examples and application needs.</p>
</div>

## 1. Current Direction

Develop broad, explicit XSD 1.1 coverage shared with Bosak while retaining actual component identity, original-node provenance, owner lifetime and bounded resource behavior. The [current matrix](docs/SPECIFICATION_COVERAGE.md) describes implemented profiles and remaining gaps. Published beta.1 and the broader development source have different scopes.

## 2. Next Work

1. Re-rank remaining unchanged upstream refusals after original comment/PI and bounded annotation admission.
2. Qualify additional annotation metadata attributes where original examples justify them; retain opaque payloads and original source budgets.
3. Select the next application-relevant structural contract across implicit anyType, schema assembly, derivation, wildcards/open content, assertions, identities and type alternatives.
4. Re-run representative consumers and assess a precisely bounded beta release. Exact-package review and publication are separate decisions.

Use smaller reviewed implementation checkpoints. A behavioral-contract ceiling is a limit, not a target to fill with repetitive tests. The [structural checkpoint](docs/markup-coverage/README.md) and [readiness assessment](docs/markup-coverage/BETA-READINESS.md) record the current evidence.

## 3. Acceptance Boundaries

No overall conformance percentage is claimed without an exhaustive normative denominator. Selected W3C samples, authored tests, package consumers and repeated lanes must remain distinguishable. Keep historical adoption evidence and sealed receipts intact; record changed expectations separately. See [development history](docs/DEVELOPMENT_HISTORY.md) for earlier checkpoints.

*Last updated: 2026-11-26*

## Document identity and remaining specification work

Bounded ID/IDREF/IDREFS validation is qualified in development 0.16; see [scope and limits](docs/identity-coverage/GUIDE.md). Next, rank remaining original facet/regex and structural application refusals before a separately adopted implementation. General key/keyref/unique, augmentation, wildcards/open content, type alternatives, assertion breadth and public Core integration remain separate work.

## Empty owners and next specification priorities

Development0.17 qualifies declared attributes on bounded empty complex owners. See [scope and limits](docs/empty-owners/GUIDE.md). Next, rank remaining original regex/facet and structural application gaps before adopting the next bounded contract. Wildcards/open content, complex inheritance, general identity constraints, augmentation and public Core integration remain separate.

## Numeric lexical patterns and next priorities

Development0.18 qualifies bounded integer/decimal pattern facets on original collapsed spelling. See [scope and limits](docs/numeric-patterns/GUIDE.md). Next, rank remaining original datatype/structural refusals against application needs before adopting another bounded contract. Broader regex/facets, complex inheritance, wildcard/open content, augmentation and public Core integration remain separate.

## anyType terminals and wildcard boundaries

Development0.19 qualifies text-only implicit/explicit anyType at roots and sequence/global-reference terminals with original anyType/untypedAtomic facts. See [scope and limits](docs/anytype-terminals/GUIDE.md). Next is a separately adopted lax wildcard/provenance contract; known declarations, namespace constraints, child/attribute facts and PSVI must not be silently ignored. Broader anyType substitution/nillability/constraints, explicit wildcard grammar and public Core integration remain separate.

## Known lax matches and declaration-absent provenance

Development0.20 qualifies known global element/attribute matches through builtin anyType lax wildcards, with actual validation and original components. See [scope and limits](docs/known-lax/GUIDE.md). Unknown nodes remain Unsupported. Next is a separate declaration-absent/untyped provenance contract and feasibility review before unknown lax content or general wildcard grammar is admitted. Namespace/notQName/UPA, skip/strict modes, complete PSVI and public Core integration remain separate.

## Declaration-absent lax checkpoint

Development 0.21 qualifies declaration-absent lax elements and application attributes beneath uninterrupted anyType ancestor chains. All 22,426 source tests (1,711 internal / 20,715 public) and 20,715 overlapping fresh package consumers pass. All 22,346 prior names remain; 80 controls are added. Pilot 21 matches; structural 41 matches / 88 Unsupported; datatype 582 matches / 16 Unsupported; zero errors. API 55 types / 663 reflection rows and nine signed Core 1.86.0 payloads are unchanged. This candidate is unpublished; broader wildcards, partial-type projection and full XSD 1.1/PSVI/C45 remain separate. Next: evidence-based review of explicit wildcard modes and strict-ancestor partial-assessment type projection; select one coherent contract before implementation. [Current scope](docs/absent-lax/GUIDE.md).
