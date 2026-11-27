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

## Strict attribute wildcard checkpoint

Development 0.22 qualifies bounded strict attribute wildcards on empty complex owners. All 22,570 source tests (1,719 internal / 20,851 public) and 20,851 overlapping fresh package consumers pass. All 22,426 prior names remain; 144 controls are added. Pilot 21 matches; structural 43 matches / 86 Unsupported; datatype 582 matches / 16 Unsupported; zero errors. API 55 types / 663 rows and nine signed Core 1.86.0 payloads are unchanged. Unpublished; broader wildcard modes, negative constraints, inheritance and full XSD1.1/PSVI/C45 remain separate. Next: review remaining original wildcard schemas and strict-ancestor partial projection, then adopt one coherent contract. [Guide](docs/strict-attributes/GUIDE.md).

## Strict wildcard exclusion checkpoint

Development 0.23 qualifies bounded notNamespace/notQName exclusions on strict empty-owner attribute wildcards, including source-bound QNames and ##defined. All 22,704 source tests (1,729 internal /20,975 public) and20,975 overlapping fresh package consumers pass. All22,570 prior names remain;134 controls are added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads are unchanged. Unpublished; wider owners/modes/composition and full XSD1.1/PSVI/C45 remain separate. Next: original-example review and a separately adopted strict-owner extension or partial-projection design. [Guide](docs/wildcard-exclusions/GUIDE.md).

## Strict sequence-owner wildcard checkpoint

Development0.24 qualifies strict attribute wildcards on finite, direct, nonempty element-only sequence owners, preserving original governing types/declarations and independent child/attribute validation. All22,843source tests(1,740internal/21,103public) and21,103overlapping fresh package consumers pass. All22,704prior names remain;139controls added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads unchanged. Unpublished; wider owners/modes/composition and fullXSD1.1/PSVI/C45 remain separate. Next: original-example review and a separately adopted owner extension before broader modes and partial projection. [Guide](docs/sequence-wildcards/GUIDE.md).

## Strict all/choice root wildcard checkpoint

Development0.25 qualifies strict attribute wildcards on existing bounded nonempty all/choice root models and named-root aliases. Original particle validation, physical-order values and governing identities remain. All23,025source tests(1,752internal/21,273public) and21,273overlapping fresh package checks pass. All22,843prior names retained;182controls added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads unchanged. Unpublished; broader containers/modes/composition and fullXSD1.1/PSVI/C45 remain separate. Next: original-example review before a separately adopted owner/container or broader-mode extension. [Guide](docs/group-wildcards/GUIDE.md).

## Strict mixed-sequence wildcard checkpoint

Development0.26 qualifies strict attribute wildcards on existing finite mixed sequence owners, retaining original mixed types/declarations and raw untypedAtomic aggregates independently from normalized child/attribute values. All23,171source tests(1,764internal/21,407public) and21,407overlapping fresh package checks pass. All23,025prior names retained;146controls added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads unchanged. Unpublished; wider models/modes/composition and fullXSD1.1/PSVI/C45 remain separate. Next: original-example review then a separately adopted simple-content or bounded container/mode extension. [Guide](docs/mixed-wildcards/GUIDE.md).

## Strict simple-content wildcard checkpoint

Development 0.27 qualifies strict attribute wildcards on existing noninteger simple-content extension owners. Original complex governing components and actual scalar values remain distinct, with independent global attribute validation. All 23,313 source tests (1,776 internal / 21,537 public) and 21,537 overlapping fresh package checks pass. All 23,171 prior test names remain; 142 checks are added. The frozen pilot has 21 matches; structural samples have 43 matches / 86 Unsupported; datatype samples have 582 matches / 16 Unsupported, with zero errors. API 55 types / 663 rows and nine signed Core 1.86.0 payloads are unchanged. Unpublished; full XSD 1.1, PSVI and C45 acceptance remain separate. [Guide](docs/simple-content-wildcards/GUIDE.md). Next: original-example review and separately adopted bounded nested compositor or partial-projection work.

## Lax empty-owner wildcard checkpoint

Development 0.28 qualifies lax attribute wildcards on empty complex owners. Known globals retain their declaration/type constraints. Undeclared attributes at an empty root carry no declaration, anySimpleType node types and untypedAtomic values; partial roots project to canonical anyType while original declared schema components remain. All 23,485 source tests (1,790 internal / 21,695 public) and 21,695 overlapping package checks pass. All 23,313 prior names remain, with 172 checks added. Pilot21; structural44/85; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads are unchanged. Unpublished; broader projection, full XSD1.1, PSVI and C45 acceptance remain separate. [Guide](docs/lax-empty-attributes/GUIDE.md). Next: separately review skip attribute wildcards or broader ancestor projection.

## Skip empty-owner wildcard checkpoint

Development 0.29 qualifies skip attribute wildcards on empty complex owners. Skipped extras have no node declaration, anySimpleType node types and original untypedAtomic values, even when globals exist. Roots with skipped extras project to canonical anyType; no-extra roots retain original empty typing. Explicit uses retain validation. All 23,649 source tests (1,802 internal /21,847 public) and21,847 overlapping package checks pass. All23,485 prior names remain;164 checks are added. Pilot21; structural47/82; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads are unchanged. Unpublished; broader projection, full XSD1.1, PSVI and C45 acceptance remain separate. [Guide](docs/skip-empty-attributes/GUIDE.md). Next: separately review ancestor projection or a bounded compositor extension against original examples.

## AnyType all terminal checkpoint

Development 0.30 qualifies direct text-only implicit/explicit anyType members in bounded all-group roots and named aliases. Original local declarations and canonical anyType components remain; actual values are original untypedAtomic text, including empty strings. Unordered matching preserves physical-order facts and typed scalar siblings. All23,793 source tests(1,814internal/21,979public) and21,979 overlapping package checks pass. All23,649 prior names remain;144 checks added. Pilot21; structural50/79; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; broader composition/projection and fullXSD1.1/PSVI/C45 remain separate. [Guide](docs/all-anytype-terminals/GUIDE.md). Next: separately contract anyType choice terminals.

## AnyType choice terminal checkpoint

Development 0.31 qualifies direct text-only implicit/explicit anyType alternatives in bounded choice roots and named aliases. One selected expanded-name branch may repeat within existing occurrence bounds; mixed alternatives remain Invalid. Original declarations/canonical anyType and actual untypedAtomic text, including empty strings, retain physical order. All23,953 source tests(1,826internal/22,127public) and22,127 overlapping package checks pass. All23,793 prior names remain;160 checks added. Pilot21; structural53/76; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; broader composition/projection and fullXSD1.1/PSVI/C45 remain separate. [Guide](docs/choice-anytype-terminals/GUIDE.md). Next: original-example review, then a separately adopted nested compositor/reference extension.

## Abstract global element checkpoint

Development 0.32 qualifies abstract Boolean flags on existing admitted global element declarations. Immutable canonical declaration identities enforce direct-use rejection at roots, references and known-lax matches before type/nil selection, without marking shared types or same-name locals abstract. All24,147 source tests(1,840internal/22,307public) and22,307 overlapping package checks pass. All23,953 prior names and original test inputs remain unchanged;194 checks added with no historical expectation changes. Pilot21; structural55/74; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; substitution groups, abstract complex types and fullXSD1.1/PSVI/C45 remain separate. [Guide](docs/abstract-elements/GUIDE.md). Next: review remaining original declaration/type constraints before separate adoption.

## Global simple-type final checkpoint

Development 0.33 qualifies explicit final controls on global named simple types. Restriction, list, union and simple-content extension edges resolve source-bound names and reject prohibited methods before a compiled schema is published. All 24,313 source tests (1,846 internal /22,467 public) and22,467 overlapping exact-package checks pass. All24,147 prior names and original inputs remain unchanged;166 checks added. Pilot21; structural59/70; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; broader derivation, finalDefault, complex-type final and full XSD1.1/PSVI/C45 remain separate. [Guide](docs/simple-type-final/GUIDE.md). Next: rank remaining original type/declaration constraints before separate adoption.

## Abstract global complex-type checkpoint

Development0.34 qualifies explicit abstract Boolean flags on admitted global named complex types. Frozen original type identities reject abstract actual governing types after admitted instance-type resolution at roots/references/nested/known-lax paths. All24,501 source tests(1,858 internal/22,643 public) and22,643 overlapping exact-package checks pass. All24,313 prior names and original inputs remain;188 checks added. Original AE-089 has two explicitly reconciled facade expectations. Pilot21; structural61/68; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; broader complex overrides/nillability, local complex abstract and full XSD1.1/PSVI/C45 remain separate. [Guide](docs/abstract-complex-types/GUIDE.md). Next: rank remaining original specification gaps before separate adoption.

## Bounded anyType nillability checkpoint

Development0.35 qualifies nillability on admitted implicit/explicit anyType roots, sequence particles/references and known-lax matches. True nil preserves original anyType/declaration authority with a typed-empty sequence and independent wildcard-attribute assessment; false/omitted nil retains ordinary untypedAtomic values. All24,733 source tests(1,872 internal/22,861 public) and22,861 overlapping exact-package checks pass. All24,501 prior names and original inputs remain;232 checks added. Original AT-055/056 have four explicitly reconciled facade compile expectations. Pilot21; structural63/66; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; general complex/all-choice nil, broader overrides and full XSD1.1/PSVI/C45 remain separate. [Guide](docs/anytype-nillability/GUIDE.md). Next: rank remaining original specification gaps before separate adoption.

## Documentation annotation language checkpoint

Development0.36 qualifies xml:lang on admitted xs:documentation wrappers using the dated XML namespace schema union: collapsed valid xs:language or exact empty undeclaration; whitespace-only is Invalid. Payloads stay opaque and original assessment/component/value/nil authority is unchanged. All24,979 source tests(1,884 internal/23,095 public) and23,095 overlapping exact-package checks pass. All24,733 prior names and original inputs remain;246 checks added. Original MK-084 has two explicitly reconciled facade compile expectations. Pilot21; structural64/65; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; other wrapper attributes/locations, inherited metadata and full XSD1.1/PSVI/C45 remain separate. [Guide](docs/annotation-language/GUIDE.md). Next: separately contract anonymous restriction bases with original unnamed authority.

## Anonymous compound restriction base checkpoint

Development0.37 qualifies inline anonymous list/union restriction bases at admitted simple-type placements. Original unnamed base objects, direct derivation links, item/member identities and facet/source scopes are preserved without synthetic names or declarations. All25,171 source tests(1,898 internal/23,273 public) and23,273 overlapping exact-package checks pass. All24,979 prior names, expectations and original inputs remain unchanged;192 checks added. Pilot21; structural65/64; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; anonymous atomic/complex bases, broader overrides/derivation and full XSD1.1/PSVI/C45 remain separate. [Guide](docs/anonymous-restriction-bases/GUIDE.md). Next: rank remaining original gaps before a separately adopted extension.

## Anonymous scalar restriction base checkpoint

Development 0.38 qualifies bounded inline anonymous decimal and xs:string/normalizedString/token restriction bases. Original unnamed base objects, direct derivation links, facet ancestry and original source contexts remain authoritative. All 25,369 source tests (1,910 internal / 23,459 public) and 23,459 overlapping exact-package checks pass. All 25,171 prior names and original test inputs remain; five public expectations and one native case are explicitly reconciled, and 198 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. The preceding empty-union rejection is corrected: an empty union is a valid type with no values. Unpublished; other atomic/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate. [Guide](docs/anonymous-scalar-bases/GUIDE.md). Next: re-rank original remaining gaps before separate adoption.

## Anonymous integer and Boolean restriction base checkpoint

Development 0.39 qualifies bounded inline anonymous integer-family and Boolean restriction bases. Actual unnamed base objects, direct derivation links and original facet sources preserve exact integer bounds/enumeration/digits, numeric patterns and Boolean fixed-collapse/pattern ancestry. All 25,624 source tests (1,925 internal / 23,699 public) and 23,699 overlapping exact-package checks pass. All 25,369 prior names and original test inputs remain; four public expectations are explicitly reconciled and 255 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; other atomic/complex inline bases, assertions, broader named-integer derivation and full XSD 1.1/PSVI/C45 acceptance remain separate. [Guide](docs/anonymous-integral-bases/GUIDE.md). Next: re-rank original remaining gaps before separate adoption.

## Anonymous date restriction base checkpoint

Development 0.40 qualifies bounded inline anonymous xs:date restriction bases. Original unnamed base objects, direct derivation links and original facet sources preserve existing date equality, enumeration, partial-order bounds, explicitTimezone and lexical pattern ancestry. All 25,834 source tests (1,939 internal / 23,895 public) and 23,895 overlapping exact-package checks pass. All 25,624 prior names and original test inputs remain; four public expectations are explicitly reconciled and 210 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; other calendar/interchange/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate. [Guide](docs/anonymous-date-bases/GUIDE.md). Next: rank remaining original gaps before separate adoption.

## Anonymous dateTime and time restriction base checkpoint

Development 0.41 qualifies bounded inline anonymous xs:dateTime and xs:time restriction bases. Original unnamed base objects, direct derivation links and exact original facet sources preserve fractional seconds, enumeration equality, partial-order bounds, fixed/timezone/whitespace facets and lexical pattern ancestry. All 26,198 source tests (1,961 internal / 24,237 public) and 24,237 overlapping exact-package checks pass. All 25,834 prior names and original test inputs remain; four public expectations are explicitly reconciled and 364 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; dateTimeStamp, other atomic/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate. [Guide](docs/anonymous-calendar-bases/GUIDE.md). Next: rank original remaining gaps before separate adoption.
