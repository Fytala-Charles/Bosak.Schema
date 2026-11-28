## Default groups on bounded all and choice owners - 28 November 2026

Development 0.52 qualifies original schema default-attribute groups on existing bounded all/choice complex owners without changing model recognition. All 31,008 source tests (2,201 internal / 28,807 public) and 28,807 overlapping exact-package checks pass. All 30,798 prior names and original fixture bytes remain. 98 authored contracts through both facades plus 14 native proofs add 210 checks. Historical reconciliations: 4 public / 0 internal assertions. Pilot: 21 matches; structural: 75 matches / 54 Unsupported; datatype: 584 matches / 14 Unsupported; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; full XSD 1.1, PSVI and C45 acceptance remain pending. [Scope and evidence](all-choice-default-groups/README.md). Earlier entries remain historical.

## Closed attributes after a bounded default-group opt-out - 28 November 2026

Development 0.51 qualifies ordinary undeclared-attribute validity on existing empty/finite-sequence complex owners that explicitly opt out of schema defaults, declare no attribute uses and have no wildcard. All 30,798 source tests (2,187 internal / 28,611 public) and 28,611 overlapping exact-package checks pass. All 30,597 prior names and original fixture bytes remain. 96 authored contracts through both facades plus 9 native proofs add 201 checks. Historical reconciliations: 24 public / 1 internal assertions. Pilot: 21 matches; structural: 75 matches / 54 Unsupported; datatype: 584 matches / 14 Unsupported; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; full XSD 1.1, PSVI and C45 acceptance remain pending. [Scope and evidence](default-optout-attributes/README.md). Earlier entries remain historical.

## Schema default-attribute groups - 28 November 2026

Development 0.50 qualifies schema default-attribute groups on existing empty and finite element-only sequence complex owners. Each source type independently applies original group uses unless explicitly opted out. No instance attributes are inserted. All 30,597 source tests (2,178 internal / 28,419 public) and 28,419 overlapping exact-package checks pass. All 30,298 prior names and original fixture bytes remain; 140 authored contracts through both facades plus 19 native proofs add 299 checks. Six historical public assertions are archived and reconciled. Pilot: 21 matches; structural: 74 matches / 55 Unsupported; datatype: 584 matches / 14 Unsupported; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; full XSD 1.1, PSVI and C45 acceptance remain pending. [Scope and evidence](default-attribute-groups/README.md). Earlier entries remain historical.

## QName length component compatibility - 28 November 2026

Development0.49 qualifies bounded QName length/minLength/maxLength component compatibility. QName values are not measured by prefix spelling, namespace/local-name strings or binary octets; original component inheritance/fixed consistency remains enforced. All30,298 source tests(2,159 internal/28,139 public) and28,139 overlapping exact-package checks pass. All29,913 prior names and original fixture bytes remain;185 authored contracts x2+15native=385new checks. Ten public expectations are archived/reconciled. Pilot21; structural70/59; datatype584/14; zeroerrors. API55/663 and nine signed Core1.86.0 payloads are unchanged. Unpublished; full XSD1.1/PSVI/C45 acceptance remains pending. [Scope and evidence](qname-length-facets/README.md). Earlier entries remain historical.

## Named integer restriction chains - 28 November 2026

Development 0.48 qualifies bounded named-integer restriction chains through original named type objects and source facets. All 29,913 source tests (2,144 internal / 27,769 public) and 27,769 overlapping exact-package checks pass. All 29,374 prior names and original fixture bytes remain. There are 252 authored contracts through both facades, one unchanged upstream trial through both facades and 33 native proofs: 539 new checks. Four public and one internal expectations are explicitly reconciled. Pilot21; structural 65/64; datatype 583/15; zero errors. API55/663 and nine signed Core1.86.0 payloads remain unchanged. Unpublished; full XSD1.1, PSVI and C45 acceptance remain pending. [Scope and evidence](named-integer-bases/README.md). Earlier entries remain historical.

## Post-anonymous-family coverage review — 28 November 2026

The post-anonymous-family review covers all 80 retained Unsupported cases across 65 distinct schemas. Fresh unchanged-binary checks reproduce 79 compile refusals and 1 assessment refusal, with zero harness errors. Product 0.47, API/Core pins and original fixtures remain unchanged. Full source 29,374 and overlapping package 27,263 counts are reused from PR112 qualification, not new suite runs. Next: bounded named-integer restriction chains, QName length-facet compatibility, then schema default-attribute groups before broader open content/composition. Samples are not a specification denominator; full XSD1.1/PSVI/public Core acceptance remains separate. [Review and next contract](post-anonymous-coverage/README.md). Earlier entries remain historical.

## Anonymous floating restriction bases — 28 November 2026

Development 0.47 qualifies bounded inline anonymous float and double restriction bases. Original unnamed objects, direct base links and facet sources preserve IEEE family mapping/rounding, special values, value/lexical facet ancestry and bounded lifetime rules. All 29,374 source tests (2,111 internal / 27,263 public) and 27,263 overlapping exact-package checks pass. All 28,931 prior names and original test inputs remain; four public expectations are explicitly reconciled and 443 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; broader simple-type/complex-content derivation and full XSD 1.1/PSVI/C45 acceptance remain separate. [Scope and evidence](anonymous-floating-bases/README.md). Earlier entries remain historical.

## Anonymous URI and text-derived restriction bases — 28 November 2026

Development 0.46 qualifies bounded inline anonymous anyURI, language, Name, NCName, NMTOKEN, ID and IDREF restriction bases. Original unnamed objects, direct base links and facet sources preserve URI/text normalization, lexical families, Unicode scalar lengths, facet ancestry and validation-root identity checks. All 28,931 source tests (2,080 internal / 26,851 public) and 26,851 overlapping exact-package checks pass. All 28,107 prior names and original test inputs remain; 32 public expectations are explicitly reconciled and 824 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; floating/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate. [Scope and evidence](anonymous-text-bases/README.md). Earlier entries remain historical.

## Anonymous binary and QName restriction bases — 28 November 2026

Development 0.45 qualifies bounded inline anonymous hexBinary, base64Binary and QName restriction bases. Original unnamed objects, direct base links and facet namespace scopes preserve exact octets/length, expanded-name QName equality and fixed/enumeration/whitespace/pattern ancestry. All 28,107 source tests (2,044 internal / 26,063 public) and 26,063 overlapping exact-package checks pass. All 27,708 prior names and original test inputs remain; 26 public expectations are explicitly reconciled and 399 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; other atomic/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate. [Scope and evidence](anonymous-interchange-bases/README.md). Earlier entries remain historical.

## Anonymous duration restriction bases — 27 November 2026

Development 0.44 qualifies bounded inline anonymous duration, yearMonthDuration and dayTimeDuration restriction bases. Original unnamed objects, direct base links and original facet sources preserve exact month/second values, family grammar, equality/partial ordering and fixed/enumeration/bound/whitespace/pattern ancestry. All 27,708 source tests (2,025 internal / 25,683 public) and 25,683 overlapping exact-package checks pass. All 27,209 prior names and original test inputs remain; six public expectations are explicitly reconciled and 499 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; other atomic/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate. [Scope and evidence](anonymous-duration-bases/README.md). Earlier entries remain historical.

## Anonymous Gregorian restriction bases — 27 November 2026

Development 0.43 qualifies bounded inline anonymous gYear, gYearMonth, gMonth, gMonthDay and gDay restriction bases. Original unnamed objects, direct base links and original facet sources preserve existing Gregorian component/value mapping, timezone equality/partial order and fixed/enumeration/bound/whitespace/pattern ancestry. All 27,209 source tests (2,002 internal / 25,207 public) and 25,207 overlapping exact-package checks pass. All 26,412 prior names and original test inputs remain; six public expectations are explicitly reconciled and 797 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; other atomic/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate. [Scope and evidence](anonymous-gregorian-bases/README.md). Earlier entries remain historical.

## Anonymous dateTimeStamp restriction bases — 27 November 2026

Development 0.42 qualifies bounded inline anonymous xs:dateTimeStamp restriction bases. Original unnamed base objects, direct derivation links and original facet sources preserve inherited required timezone, exact calendar values and existing fixed/bound/whitespace/pattern ancestry. All 26,412 source tests (1,975 internal / 24,437 public) and 24,437 overlapping exact-package checks pass. All 26,198 prior names and original test inputs remain; two public expectation/kind cases are explicitly reconciled and 214 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; other atomic/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate. [Scope and evidence](anonymous-stamp-bases/README.md). Earlier entries remain historical.

## Anonymous dateTime and time restriction bases — 27 November 2026

Development 0.41 qualifies bounded inline anonymous xs:dateTime and xs:time restriction bases. Original unnamed base objects, direct derivation links and exact original facet sources preserve fractional seconds, enumeration equality, partial-order bounds, fixed/timezone/whitespace facets and lexical pattern ancestry. All 26,198 source tests (1,961 internal / 24,237 public) and 24,237 overlapping exact-package checks pass. All 25,834 prior names and original test inputs remain; four public expectations are explicitly reconciled and 364 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; dateTimeStamp, other atomic/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate. [Scope and evidence](anonymous-calendar-bases/README.md). Earlier entries remain historical.

## Anonymous date restriction bases — 27 November 2026

Development 0.40 qualifies bounded inline anonymous xs:date restriction bases. Original unnamed base objects, direct derivation links and original facet sources preserve existing date equality, enumeration, partial-order bounds, explicitTimezone and lexical pattern ancestry. All 25,834 source tests (1,939 internal / 23,895 public) and 23,895 overlapping exact-package checks pass. All 25,624 prior names and original test inputs remain; four public expectations are explicitly reconciled and 210 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; other calendar/interchange/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate. [Scope and evidence](anonymous-date-bases/README.md). Earlier entries remain historical.

## Anonymous integer and Boolean restriction bases — 27 November 2026

Development 0.39 qualifies bounded inline anonymous integer-family and Boolean restriction bases. Actual unnamed base objects, direct derivation links and original facet sources preserve exact integer bounds/enumeration/digits, numeric patterns and Boolean fixed-collapse/pattern ancestry. All 25,624 source tests (1,925 internal / 23,699 public) and 23,699 overlapping exact-package checks pass. All 25,369 prior names and original test inputs remain; four public expectations are explicitly reconciled and 255 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; other atomic/complex inline bases, assertions, broader named-integer derivation and full XSD 1.1/PSVI/C45 acceptance remain separate. [Scope and evidence](anonymous-integral-bases/README.md). Earlier entries remain historical.

## Anonymous decimal and string restriction bases — 27 November 2026

Development 0.38 qualifies bounded inline anonymous decimal and xs:string/normalizedString/token restriction bases. Original unnamed base objects, direct derivation links, facet ancestry and original source contexts remain authoritative. All 25,369 source tests (1,910 internal / 23,459 public) and 23,459 overlapping exact-package checks pass. All 25,171 prior names and original test inputs remain; five public expectations and one native case are explicitly reconciled, and 198 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. The preceding empty-union rejection is corrected: an empty union is a valid type with no values. Unpublished; other atomic/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate. [Scope and evidence](anonymous-scalar-bases/README.md). Earlier entries remain historical.

## Anonymous compound restriction bases — 27 November2026

Development0.37 qualifies inline anonymous list/union restriction bases at admitted simple-type placements. Original unnamed base objects, direct derivation links, item/member identities and facet/source scopes are preserved without synthetic names or declarations. All25,171 source tests(1,898 internal/23,273 public) and23,273 overlapping exact-package checks pass. All24,979 prior names, expectations and original inputs remain unchanged;192 checks added. Pilot21; structural65/64; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; anonymous atomic/complex bases, broader overrides/derivation and full XSD1.1/PSVI/C45 remain separate. [Scope and evidence](anonymous-restriction-bases/README.md). Earlier entries remain historical.

## Documentation annotation language — 27 November2026

Development0.36 qualifies xml:lang on admitted xs:documentation wrappers using the dated XML namespace schema union: collapsed valid xs:language or exact empty undeclaration; whitespace-only is Invalid. Payloads stay opaque and original assessment/component/value/nil authority is unchanged. All24,979 source tests(1,884 internal/23,095 public) and23,095 overlapping exact-package checks pass. All24,733 prior names and original inputs remain;246 checks added. Original MK-084 has two explicitly reconciled facade compile expectations. Pilot21; structural64/65; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; other wrapper attributes/locations, inherited metadata and full XSD1.1/PSVI/C45 remain separate. [Scope and evidence](annotation-language/README.md). Earlier entries remain historical.

## Bounded anyType nillability — 27 November2026

Development0.35 qualifies nillability on admitted implicit/explicit anyType roots, sequence particles/references and known-lax matches. True nil preserves original anyType/declaration authority with a typed-empty sequence and independent wildcard-attribute assessment; false/omitted nil retains ordinary untypedAtomic values. All24,733 source tests(1,872 internal/22,861 public) and22,861 overlapping exact-package checks pass. All24,501 prior names and original inputs remain;232 checks added. Original AT-055/056 have four explicitly reconciled facade compile expectations. Pilot21; structural63/66; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; general complex/all-choice nil, broader overrides and full XSD1.1/PSVI/C45 remain separate. [Scope and evidence](anytype-nillability/README.md). Earlier entries remain historical.

## Abstract global complex types — 27 November2026

Development0.34 qualifies explicit abstract Boolean flags on admitted global named complex types. Frozen original type identities reject abstract actual governing types after admitted instance-type resolution at roots/references/nested/known-lax paths. All24,501 source tests(1,858 internal/22,643 public) and22,643 overlapping exact-package checks pass. All24,313 prior names and original inputs remain;188 checks added. Original AE-089 has two explicitly reconciled facade expectations. Pilot21; structural61/68; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; broader complex overrides/nillability, local complex abstract and full XSD1.1/PSVI/C45 remain separate. [Scope and evidence](abstract-complex-types/README.md). Earlier entries remain historical.

## Global simple-type final controls — 27 November 2026

Development 0.33 qualifies explicit final controls on global named simple types. Restriction, list, union and simple-content extension edges resolve source-bound names and reject prohibited methods before a compiled schema is published. All 24,313 source tests (1,846 internal /22,467 public) and22,467 overlapping exact-package checks pass. All24,147 prior names and original inputs remain unchanged;166 checks added. Pilot21; structural59/70; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; broader derivation, finalDefault, complex-type final and full XSD1.1/PSVI/C45 remain separate. [Scope and evidence](simple-type-final/README.md). Earlier entries remain historical.

## Abstract global element declarations — 27 November 2026

Development 0.32 qualifies abstract Boolean flags on existing admitted global element declarations. Immutable canonical declaration identities enforce direct-use rejection at roots, references and known-lax matches before type/nil selection, without marking shared types or same-name locals abstract. All24,147 source tests(1,840internal/22,307public) and22,307 overlapping package checks pass. All23,953 prior names and original test inputs remain unchanged;194 checks added with no historical expectation changes. Pilot21; structural55/74; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; substitution groups, abstract complex types and fullXSD1.1/PSVI/C45 remain separate. [Scope and evidence](abstract-elements/README.md). Earlier entries remain historical.

## Text-only anyType choice alternatives — 27 November 2026

Development 0.31 qualifies direct text-only implicit/explicit anyType alternatives in bounded choice roots and named aliases. One selected expanded-name branch may repeat within existing occurrence bounds; mixed alternatives remain Invalid. Original declarations/canonical anyType and actual untypedAtomic text, including empty strings, retain physical order. All23,953 source tests(1,826internal/22,127public) and22,127 overlapping package checks pass. All23,793 prior names remain;160 checks added. Pilot21; structural53/76; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; broader composition/projection and fullXSD1.1/PSVI/C45 remain separate. [Scope and evidence](choice-anytype-terminals/README.md). Earlier entries remain historical.

## Text-only anyType all-group members — 27 November 2026

Development 0.30 qualifies direct text-only implicit/explicit anyType members in bounded all-group roots and named aliases. Original local declarations and canonical anyType components remain; actual values are original untypedAtomic text, including empty strings. Unordered matching preserves physical-order facts and typed scalar siblings. All23,793 source tests(1,814internal/21,979public) and21,979 overlapping package checks pass. All23,649 prior names remain;144 checks added. Pilot21; structural50/79; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; broader composition/projection and fullXSD1.1/PSVI/C45 remain separate. [Scope and evidence](all-anytype-terminals/README.md). Earlier entries remain historical.

## Empty-owner skip attribute wildcards — 27 November 2026

Development 0.29 qualifies skip attribute wildcards on empty complex owners. Skipped extras have no node declaration, anySimpleType node types and original untypedAtomic values, even when globals exist. Roots with skipped extras project to canonical anyType; no-extra roots retain original empty typing. Explicit uses retain validation. All 23,649 source tests (1,802 internal /21,847 public) and21,847 overlapping package checks pass. All23,485 prior names remain;164 checks are added. Pilot21; structural47/82; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads are unchanged. Unpublished; broader projection, full XSD1.1, PSVI and C45 acceptance remain separate. [Scope and evidence](skip-empty-attributes/README.md). Earlier entries remain historical.

## Empty-owner lax attribute wildcards — 27 November 2026

Development 0.28 qualifies lax attribute wildcards on empty complex owners. Known globals retain their declaration/type constraints. Undeclared attributes at an empty root carry no declaration, anySimpleType node types and untypedAtomic values; partial roots project to canonical anyType while original declared schema components remain. All 23,485 source tests (1,790 internal / 21,695 public) and 21,695 overlapping package checks pass. All 23,313 prior names remain, with 172 checks added. Pilot21; structural44/85; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads are unchanged. Unpublished; broader projection, full XSD1.1, PSVI and C45 acceptance remain separate. [Scope and evidence](lax-empty-attributes/README.md). Earlier entries remain historical.

## Simple-content strict attribute wildcards — 27 November 2026

Development 0.27 qualifies strict attribute wildcards on existing noninteger simple-content extension owners. Original complex governing components and actual scalar values remain distinct, with independent global attribute validation. All 23,313 source tests (1,776 internal / 21,537 public) and 21,537 overlapping fresh package checks pass. All 23,171 prior test names remain; 142 checks are added. The frozen pilot has 21 matches; structural samples have 43 matches / 86 Unsupported; datatype samples have 582 matches / 16 Unsupported, with zero errors. API 55 types / 663 rows and nine signed Core 1.86.0 payloads are unchanged. Unpublished; full XSD 1.1, PSVI and C45 acceptance remain separate. [Scope and evidence](simple-content-wildcards/README.md). Earlier entries remain historical.

## Mixed-sequence strict attribute wildcards —26November2026

Development0.26 qualifies strict attribute wildcards on existing finite mixed sequence owners, retaining original mixed types/declarations and raw untypedAtomic aggregates independently from normalized child/attribute values. All23,171source tests(1,764internal/21,407public) and21,407overlapping fresh package checks pass. All23,025prior names retained;146controls added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads unchanged. Unpublished; wider models/modes/composition and fullXSD1.1/PSVI/C45 remain separate. [Scope and evidence](mixed-wildcards/README.md). Earlier entries remain historical.

## All/choice-root strict attribute wildcards —26November2026

Development0.25 qualifies strict attribute wildcards on existing bounded nonempty all/choice root models and named-root aliases. Original particle validation, physical-order values and governing identities remain. All23,025source tests(1,752internal/21,273public) and21,273overlapping fresh package checks pass. All22,843prior names retained;182controls added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads unchanged. Unpublished; broader containers/modes/composition and fullXSD1.1/PSVI/C45 remain separate. [Scope and evidence](group-wildcards/README.md). Earlier entries remain historical.

## Sequence-owner strict attribute wildcards —26November2026

Development0.24 qualifies strict attribute wildcards on finite, direct, nonempty element-only sequence owners, preserving original governing types/declarations and independent child/attribute validation. All22,843source tests(1,740internal/21,103public) and21,103overlapping fresh package consumers pass. All22,704prior names remain;139controls added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads unchanged. Unpublished; wider owners/modes/composition and fullXSD1.1/PSVI/C45 remain separate. [Scope and evidence](sequence-wildcards/README.md). Earlier entries remain historical.

## Strict wildcard exclusions — 26 November 2026

Development 0.23 qualifies bounded notNamespace/notQName exclusions on strict empty-owner attribute wildcards, including source-bound QNames and ##defined. All 22,704 source tests (1,729 internal /20,975 public) and20,975 overlapping fresh package consumers pass. All22,570 prior names remain;134 controls are added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads are unchanged. Unpublished; wider owners/modes/composition and full XSD1.1/PSVI/C45 remain separate. [Scope and evidence](wildcard-exclusions/README.md). Earlier entries remain historical.

## Strict attribute wildcards — 26 November 2026

Development 0.22 qualifies bounded strict attribute wildcards on empty complex owners. All 22,570 source tests (1,719 internal / 20,851 public) and 20,851 overlapping fresh package consumers pass. All 22,426 prior names remain; 144 controls are added. Pilot 21 matches; structural 43 matches / 86 Unsupported; datatype 582 matches / 16 Unsupported; zero errors. API 55 types / 663 rows and nine signed Core 1.86.0 payloads are unchanged. Unpublished; broader wildcard modes, negative constraints, inheritance and full XSD1.1/PSVI/C45 remain separate. [Scope and evidence](strict-attributes/README.md). Earlier entries remain historical.

## Declaration-absent lax content — 26 November 2026

Development 0.21 qualifies declaration-absent lax elements and application attributes beneath uninterrupted anyType ancestor chains. All 22,426 source tests (1,711 internal / 20,715 public) and 20,715 overlapping fresh package consumers pass. All 22,346 prior names remain; 80 controls are added. Pilot 21 matches; structural 41 matches / 88 Unsupported; datatype 582 matches / 16 Unsupported; zero errors. API 55 types / 663 reflection rows and nine signed Core 1.86.0 payloads are unchanged. This candidate is unpublished; broader wildcards, partial-type projection and full XSD 1.1/PSVI/C45 remain separate. [Scope and evidence](absent-lax/README.md). Earlier entries remain historical.

## Known-declaration anyType lax matches —26November2026

Development0.20 qualifies bounded anyType lax matches against original known global element/attribute declarations. Source22,346/internal1,703/public20,643 and fresh package20,643 overlapping checks pass; all22,198 prior names remain with148 added. Pilot21matches; structural41matches/88Unsupported; datatype582matches/16Unsupported; zero errors. API55names/663rows and nine signed Core1.86.0 payloads are unchanged. Unknown/declaration-absent lax nodes remain Unsupported; no publication or fullXSD1.1/PSVI/C45 acceptance. [Contract, guide and evidence](known-lax/README.md). Earlier dated entries remain historical.

## Text-only anyType terminals —26November2026

Development0.19 qualifies text-only implicit/explicit anyType terminals with canonical anyType governing and actual untypedAtomic components. Source22,198/internal1,695/public20,503 and fresh package20,503 overlapping tests pass; all22,030 prior names remain with168 added. Pilot21matches; structural41matches/88Unsupported; datatype582matches/16Unsupported; zero errors. API55names/663rows and nine signed Core1.86.0 payloads are unchanged. Wildcard child/application-attribute assessment remains Unsupported; no publication or fullXSD1.1/PSVI/C45 acceptance. [Guide and evidence](anytype-terminals/README.md). Earlier dated entries remain historical.

## Bounded numeric lexical patterns —26November2026

Development0.18 qualifies bounded integer/decimal lexical pattern facets independently from exact numeric normalization. Source22,030/internal1,687/public20,343 and fresh package20,343 overlapping tests pass; all21,870 prior names remain with160 added. Pilot21matches; structural39matches/90Unsupported; datatype582matches/16Unsupported; zero errors. API55names/663rows and nine signed Core1.86.0 payloads are unchanged. No publication or fullXSD1.1/PSVI/C45 acceptance. [Guide and evidence](numeric-patterns/README.md). Earlier dated entries remain historical.

## Empty complex owners with attributes — 26 November 2026

Development0.17 qualifies declared attributes on bounded empty complex owners, with original empty typed values and independent attribute/member provenance. Source21,870/internal1,679/public20,191 and fresh package20,191 overlapping tests pass; all21,730 prior names remain with140 added. Pilot21matches; structural39matches/90Unsupported; datatype568matches/30Unsupported; zero errors. API55names/663rows and nine signed Core1.86.0 payloads are unchanged. No publication or fullXSD1.1/PSVI/C45 acceptance. [Guide and evidence](empty-owners/README.md). Earlier dated entries remain historical.

## Bounded document identity coverage — 26 November 2026

Development 0.16 qualifies bounded ID/IDREF/IDREFS validation: owner-aware ID bindings, forward references, actual union/list members and assessment-local resolution. Source 21,730 and fresh package 20,059 overlapping tests pass; all 21,544 prior names remain. The frozen datatype sample improves to 558 matches / 40 Unsupported (+21 admissions), with zero errors. Pilot21 and structural39/90 remain unchanged. Public API55 types/663 rows and signed Core1.86.0 are unchanged. No publication or full XSD1.1/C45 acceptance. [Scope and qualification](identity-coverage/README.md). Earlier dated entries below remain historical.

## Original markup and annotation coverage — 26 November 2026

Development0.15 adds original instance comment/PI support and bounded opaque schema annotations before declaration scans/closed assembly. [Guide](markup-coverage/GUIDE.md); [readiness](markup-coverage/BETA-READINESS.md). Source21,544/package19,881overlapping pass; API55names/663rows and signed Core1.86.0 unchanged. Frozen21matches; structural39/90; datatype537/61; zeroerrors. Original invoice workflow passes. Earlier dated entries are historical; publication/fullXSD/C45 remain separate.

## Product documentation reconciliation — 26 November 2026

README and ROADMAP now describe product use and current direction. Exact prior files, including historical checkpoint entries, are retained with source identity in [the historical snapshot](history/2026-11-26-product-docs/SOURCE.json). Their relative links retain the original repository-root context.

## Bounded structural coverage â€” 26 November 2026

Development 0.14 adds empty all/optional empty choice and original anonymous attribute simple types on admitted owners. The [guide](structural-coverage/GUIDE.md) records capacities and exclusions; [beta readiness](structural-coverage/BETA-READINESS.md) separates bounded qualification from general XSD1.1/C45 acceptance. All21,358 source/19,701 overlapping package tests pass; API55names/663rows and signed Core1.86.0 are unchanged. Structural sample improves32â†’38 matches; datatype454matches remains co-blocked. Original invoice workflow passes. Earlier dated entries are historical; publication remains separate.

## Evidence-driven coverage checkpoint â€” 26 November 2026

The [current coverage guide](SPECIFICATION_COVERAGE.md) reconciles development 0.13 / signed Core 1.86.0 with historical audits. A separately frozen 598-case datatype sample has 454 matches / 144 Unsupported / zero errors; the structural sample remains 32 / 97. Next are adopted structural fixes and consumer trials within an up-to-400-contract ceiling. Published beta.1 remains narrower; no publication or full-conformance claim. Earlier dated entries below are historical.

# Bosak.Schema development history

## 9 November 2026: documentation synchronization

Current handover, architecture, integration, feature registry, README, roadmap and agent entry points now describe the merged Core 1.81.2 adoption and refreshed optional integration. Earlier statements that the upgrade was only a temporary override/unpushed candidate, or that source pins remained1.26.3, describe their original checkpoints rather than current main. No source/API/package change is introduced by this documentation synchronization.

Four original local canonical files are retained byte-for-byte as [pre-sync text snapshots](history/2026-11-09-pre-sync/README.md). The associated review inbox/response files remain unchanged at their original paths. [Preservation inventory](DOCUMENTATION-SYNC-2026-11-09.json) records all 42 hashes and their retained locations.

## 9 November 2026: merged source dependency adoption

[Schema PR #1](https://github.com/Fytala-Charles/Bosak.Schema/pull/1) adopts published Core 1.81.2 in four product and one test reference. The124-case source and retained binary catalogs and11 separate integration/ownership controls pass within their recorded scopes; [adoption](CORE-1.81.2-ADOPTION.md) links exact receipts. The standalone source remains the0.4 development line; this is not a Schema NuGet release.

[Core PR #472](https://github.com/Fytala-Charles/Bosak/pull/472) refreshes its optional source wrapper to merged Schema c953ebe. All135 cases and the typed-price example pass in hosted Linux CI. Full native integration, XSD 1.1 structural validation and budgets remain separate.

## 3 November 2026: scoped R2 design returns

[O1/O2 revision2](core-disposition-responses/2026-11-03-v2/README.md) records AcceptWithScope against proposal ff475d712f6ef6aa5033113cc39afb887ae4cbad24705098c5c9cb2097641688 and native evidence c3624998. These are authored scoped design returns, not independent human signatures or new runtime acceptance. Earlier [unsigned confirmation drafts](core-disposition-inbox/2026-11-03/README.md), old Revise forms and supplied input hashes are preserved unchanged. Their pending gates, versions and local-delivery statements retain their historical context.

## 29â€“30 October 2026: public Apache transition and staged schema coverage

The public schema-only repository and activation-free shims were published under Apache-2.0; licensing infrastructure/private history stayed separate. [Publication](PUBLICATION.json) records114 original tests. Staged wildcard negative-constraint recording grew the standalone suite to124 cases; full structural enforcement was not implied.


## 17 November 2026 â€” local beta readiness and restricted resolution

Assessed the published alpha, classified39 public types/313 contract rows and reproduced five caught published-package observations. Proposed beta/API decisions remain unaccepted. Implemented additive RestrictedSchemaFileResolver and CreateRestrictedCompiler using published Core1.81.2 controlled schema acquisition, including nested references. The old convenience paths retain their compatibility behavior. New31 implementation and three public consumer controls bring the suite to306 cases;45 release controls pass separately. Candidate API40types/317rows is strictly additive. Documented stable-filesystem/no-atomic-sandbox boundary, full XML/DTD/byte policy, URI alias checks and Core's unused-include warning behavior. Source-only receipts and immutable pre-change basis are in docs/restricted-resolution; no tag, release or commit/push is made by this local checkpoint.


## 17 November 2026 â€” compiler-owned descriptor snapshot candidate

After PR8 merge/main validation, froze compiler-produced outer/nested collections without changing positional record constructors. Preserved shared assertion identity and ordinal bindings;13 added cases bring suite to319, plus45 release controls. Public40types/317rows unchanged; runtime mutable implementation casts deliberately stop working. Caller records remain application-owned. No tag, beta/profile acceptance or publication; canonical contract and source-only receipts recorded.


## 17 November 2026 â€” legacy compile-diagnostic precondition candidate

After PR9 merge/main validation, added a common compile-finding guard to all three legacy validation entry points. Arguments retain precedence; original findings are copied unchanged before any other work. Clean result meaning remains assertion/facet/static-check scope, not structural validation.26 added cases bring suite to345, plus45 release controls; public40types/317rows unchanged. No native/Core/version change, beta/ADR acceptance or publication.


## 17 November 2026 â€” bounded beta workload qualification candidate

Owner selected bounded decimal-session beta with exclusions after PR10 merge/main verification. Added a separate non-packable public consumer workload tool, six mocked integrity controls and CI build/checks. Five named workflows ran in20 processes with explicit pre-recorded local envelopes; all final mixed-numeric observations pass. Product345 and release45 controls remain green; source/dependencies unchanged. Scope selection does not accept ADR/API contracts, production budgets or beta publication. Current alpha-only release pins require separate beta review.


## 17 November 2026 â€” bounded beta contract review and compatibility

After PR11 merge/main verification, prepared current Proposed ADR009/010 decisions with archived original Git text. Clean source produces normalMinVer alpha0.14 without tag/override;345source/33package consumer/124legacy and23public retained binary controls pass, with onlySchemaDLLreplaced in candidate lanes. NineCore1.81.2 signatures/payloads and exact40type317row API verify. Contract review distinguishes scoped Schema consumption from generalCoreC45/ADR006 acceptance; no beta tag/artifact/publication or inferred owner acceptance.


17 November2026 owner decision: explicitly accepted bounded ADR009/010/API contracts and instructed commit/push/PR. Scoped Schema facade/publicCore1.81.2 use accepted; generalCoreC45/ADR006, productionSLAs and beta tag/artifact/publication remain outside approval.


## Beta.1 preparation â€” 17 November 2026

Bounded ADR009/010 acceptance is merged in PR12; source `2a5abf6fb663313dc32bfeca5bbb8740bbe987ae`, main44174792275 green. [Beta.1 preparation](BETA1_PREPARATION.md) records the exact product/tag plan and fresh local beta package.345 source,33 package-only, retained124/23 control/replacement cases and nine signed Core1.81.2 payloads pass;54 release and six runner controls pass separately. The controller candidate requires the reviewed beta tag and both retained lanes. No remote beta tag or NuGet beta publication; controller delivery, hosted preparation and exact hosted-artifact approval remain pending. Accepted bounded application scope does not close general CoreC45/ADR006 or production SLA/full-XSD acceptance.

## Beta.1 publication documentation â€” 18 November 2026

Base52a4997daba109bc5daec03522e9fe98725b78aa. Owner requested next steps after beta publication/GitHub prerelease. Canonical current-release docs now point to verified0.5.0-beta.1 and explicit accepted scope/exclusions; dated dossiers remain unchanged. Publication44245557198/preparation44244606595, exact hashes and GitHub asset identities are in docs/beta1-publication/RECEIPT.json. No engine/package/workflow/dependency/version changes or additional publication. [Release guide](BETA1_RELEASE.md). Worktree .worktrees/Schema.BetaReleaseDocs / docs/schema-beta1-publication. Commit/push/PR still require explicit instruction under AGENTS. Package-only representative consumer trial is separate from product test totals and is not production-user acceptance.
