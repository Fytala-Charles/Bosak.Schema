## Text-only anyType choice alternatives — 27 November 2026

Development 0.31 qualifies direct text-only implicit/explicit anyType alternatives in bounded choice roots and named aliases. One selected expanded-name branch may repeat within existing occurrence bounds; mixed alternatives remain Invalid. Original declarations/canonical anyType and actual untypedAtomic text, including empty strings, retain physical order. All23,953 source tests(1,826internal/22,127public) and22,127 overlapping package checks pass. All23,793 prior names remain;160 checks added. Pilot21; structural53/76; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; broader composition/projection and fullXSD1.1/PSVI/C45 remain separate. [Scope and evidence](docs/choice-anytype-terminals/README.md). Earlier entries remain historical.

## Text-only anyType all-group members — 27 November 2026

Development 0.30 qualifies direct text-only implicit/explicit anyType members in bounded all-group roots and named aliases. Original local declarations and canonical anyType components remain; actual values are original untypedAtomic text, including empty strings. Unordered matching preserves physical-order facts and typed scalar siblings. All23,793 source tests(1,814internal/21,979public) and21,979 overlapping package checks pass. All23,649 prior names remain;144 checks added. Pilot21; structural50/79; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; broader composition/projection and fullXSD1.1/PSVI/C45 remain separate. [Scope and evidence](docs/all-anytype-terminals/README.md). Earlier entries remain historical.

## Empty-owner skip attribute wildcards — 27 November 2026

Development 0.29 qualifies skip attribute wildcards on empty complex owners. Skipped extras have no node declaration, anySimpleType node types and original untypedAtomic values, even when globals exist. Roots with skipped extras project to canonical anyType; no-extra roots retain original empty typing. Explicit uses retain validation. All 23,649 source tests (1,802 internal /21,847 public) and21,847 overlapping package checks pass. All23,485 prior names remain;164 checks are added. Pilot21; structural47/82; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads are unchanged. Unpublished; broader projection, full XSD1.1, PSVI and C45 acceptance remain separate. [Scope and evidence](docs/skip-empty-attributes/README.md). Earlier entries remain historical.

## Empty-owner lax attribute wildcards — 27 November 2026

Development 0.28 qualifies lax attribute wildcards on empty complex owners. Known globals retain their declaration/type constraints. Undeclared attributes at an empty root carry no declaration, anySimpleType node types and untypedAtomic values; partial roots project to canonical anyType while original declared schema components remain. All 23,485 source tests (1,790 internal / 21,695 public) and 21,695 overlapping package checks pass. All 23,313 prior names remain, with 172 checks added. Pilot21; structural44/85; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads are unchanged. Unpublished; broader projection, full XSD1.1, PSVI and C45 acceptance remain separate. [Scope and evidence](docs/lax-empty-attributes/README.md). Earlier entries remain historical.

## Simple-content strict attribute wildcards — 27 November 2026

Development 0.27 qualifies strict attribute wildcards on existing noninteger simple-content extension owners. Original complex governing components and actual scalar values remain distinct, with independent global attribute validation. All 23,313 source tests (1,776 internal / 21,537 public) and 21,537 overlapping fresh package checks pass. All 23,171 prior test names remain; 142 checks are added. The frozen pilot has 21 matches; structural samples have 43 matches / 86 Unsupported; datatype samples have 582 matches / 16 Unsupported, with zero errors. API 55 types / 663 rows and nine signed Core 1.86.0 payloads are unchanged. Unpublished; full XSD 1.1, PSVI and C45 acceptance remain separate. [Scope and evidence](docs/simple-content-wildcards/README.md). Earlier entries remain historical.

## Mixed-sequence strict attribute wildcards —26November2026

Development0.26 qualifies strict attribute wildcards on existing finite mixed sequence owners, retaining original mixed types/declarations and raw untypedAtomic aggregates independently from normalized child/attribute values. All23,171source tests(1,764internal/21,407public) and21,407overlapping fresh package checks pass. All23,025prior names retained;146controls added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads unchanged. Unpublished; wider models/modes/composition and fullXSD1.1/PSVI/C45 remain separate. [Scope and evidence](docs/mixed-wildcards/README.md). Earlier entries remain historical.

## All/choice-root strict attribute wildcards —26November2026

Development0.25 qualifies strict attribute wildcards on existing bounded nonempty all/choice root models and named-root aliases. Original particle validation, physical-order values and governing identities remain. All23,025source tests(1,752internal/21,273public) and21,273overlapping fresh package checks pass. All22,843prior names retained;182controls added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads unchanged. Unpublished; broader containers/modes/composition and fullXSD1.1/PSVI/C45 remain separate. [Scope and evidence](docs/group-wildcards/README.md). Earlier entries remain historical.

## Sequence-owner strict attribute wildcards —26November2026

Development0.24 qualifies strict attribute wildcards on finite, direct, nonempty element-only sequence owners, preserving original governing types/declarations and independent child/attribute validation. All22,843source tests(1,740internal/21,103public) and21,103overlapping fresh package consumers pass. All22,704prior names remain;139controls added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads unchanged. Unpublished; wider owners/modes/composition and fullXSD1.1/PSVI/C45 remain separate. [Scope and evidence](docs/sequence-wildcards/README.md). Earlier entries remain historical.

## Strict wildcard exclusions — 26 November 2026

Development 0.23 qualifies bounded notNamespace/notQName exclusions on strict empty-owner attribute wildcards, including source-bound QNames and ##defined. All 22,704 source tests (1,729 internal /20,975 public) and20,975 overlapping fresh package consumers pass. All22,570 prior names remain;134 controls are added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads are unchanged. Unpublished; wider owners/modes/composition and full XSD1.1/PSVI/C45 remain separate. [Scope and evidence](docs/wildcard-exclusions/README.md). Earlier entries remain historical.

## Strict attribute wildcards — 26 November 2026

Development 0.22 qualifies bounded strict attribute wildcards on empty complex owners. All 22,570 source tests (1,719 internal / 20,851 public) and 20,851 overlapping fresh package consumers pass. All 22,426 prior names remain; 144 controls are added. Pilot 21 matches; structural 43 matches / 86 Unsupported; datatype 582 matches / 16 Unsupported; zero errors. API 55 types / 663 rows and nine signed Core 1.86.0 payloads are unchanged. Unpublished; broader wildcard modes, negative constraints, inheritance and full XSD1.1/PSVI/C45 remain separate. [Scope and evidence](docs/strict-attributes/README.md). Earlier entries remain historical.

## Declaration-absent lax content — 26 November 2026

Development 0.21 qualifies declaration-absent lax elements and application attributes beneath uninterrupted anyType ancestor chains. All 22,426 source tests (1,711 internal / 20,715 public) and 20,715 overlapping fresh package consumers pass. All 22,346 prior names remain; 80 controls are added. Pilot 21 matches; structural 41 matches / 88 Unsupported; datatype 582 matches / 16 Unsupported; zero errors. API 55 types / 663 reflection rows and nine signed Core 1.86.0 payloads are unchanged. This candidate is unpublished; broader wildcards, partial-type projection and full XSD 1.1/PSVI/C45 remain separate. [Scope and evidence](docs/absent-lax/README.md). Earlier entries remain historical.

## Known-declaration anyType lax matches —26November2026

Development0.20 qualifies bounded anyType lax matches against original known global element/attribute declarations. Source22,346/internal1,703/public20,643 and fresh package20,643 overlapping checks pass; all22,198 prior names remain with148 added. Pilot21matches; structural41matches/88Unsupported; datatype582matches/16Unsupported; zero errors. API55names/663rows and nine signed Core1.86.0 payloads are unchanged. Unknown/declaration-absent lax nodes remain Unsupported; no publication or fullXSD1.1/PSVI/C45 acceptance. [Contract, guide and evidence](docs/known-lax/README.md). Earlier dated entries remain historical.

## Text-only anyType terminals —26November2026

Development0.19 qualifies text-only implicit/explicit anyType terminals with canonical anyType governing and actual untypedAtomic components. Source22,198/internal1,695/public20,503 and fresh package20,503 overlapping tests pass; all22,030 prior names remain with168 added. Pilot21matches; structural41matches/88Unsupported; datatype582matches/16Unsupported; zero errors. API55names/663rows and nine signed Core1.86.0 payloads are unchanged. Wildcard child/application-attribute assessment remains Unsupported; no publication or fullXSD1.1/PSVI/C45 acceptance. [Guide and evidence](docs/anytype-terminals/README.md). Earlier dated entries remain historical.

## Bounded numeric lexical patterns —26November2026

Development0.18 qualifies bounded integer/decimal lexical pattern facets independently from exact numeric normalization. Source22,030/internal1,687/public20,343 and fresh package20,343 overlapping tests pass; all21,870 prior names remain with160 added. Pilot21matches; structural39matches/90Unsupported; datatype582matches/16Unsupported; zero errors. API55names/663rows and nine signed Core1.86.0 payloads are unchanged. No publication or fullXSD1.1/PSVI/C45 acceptance. [Guide and evidence](docs/numeric-patterns/README.md). Earlier dated entries remain historical.

## Empty complex owners with attributes — 26 November 2026

Development0.17 qualifies declared attributes on bounded empty complex owners, with original empty typed values and independent attribute/member provenance. Source21,870/internal1,679/public20,191 and fresh package20,191 overlapping tests pass; all21,730 prior names remain with140 added. Pilot21matches; structural39matches/90Unsupported; datatype568matches/30Unsupported; zero errors. API55names/663rows and nine signed Core1.86.0 payloads are unchanged. No publication or fullXSD1.1/PSVI/C45 acceptance. [Guide and evidence](docs/empty-owners/README.md). Earlier dated entries remain historical.

## Bounded document identity coverage — 26 November 2026

Development 0.16 qualifies bounded ID/IDREF/IDREFS validation: owner-aware ID bindings, forward references, actual union/list members and assessment-local resolution. Source 21,730 and fresh package 20,059 overlapping tests pass; all 21,544 prior names remain. The frozen datatype sample improves to 558 matches / 40 Unsupported (+21 admissions), with zero errors. Pilot21 and structural39/90 remain unchanged. Public API55 types/663 rows and signed Core1.86.0 are unchanged. No publication or full XSD1.1/C45 acceptance. [Scope and qualification](docs/identity-coverage/README.md). Earlier dated entries below remain historical.

## Original markup and annotation coverage — 26 November 2026

Development0.15 adds original instance comment/PI support and bounded opaque schema annotations before declaration scans/closed assembly. [Guide](docs/markup-coverage/GUIDE.md); [readiness](docs/markup-coverage/BETA-READINESS.md). Source21,544/package19,881overlapping pass; API55names/663rows and signed Core1.86.0 unchanged. Frozen21matches; structural39/90; datatype537/61; zeroerrors. Original invoice workflow passes. Earlier dated entries are historical; publication/fullXSD/C45 remain separate.

## Bounded structural coverage — 26 November 2026

Development 0.14 adds empty all/optional empty choice and original anonymous attribute simple types on admitted owners. The [guide](docs/structural-coverage/GUIDE.md) records capacities and exclusions; [beta readiness](docs/structural-coverage/BETA-READINESS.md) separates bounded qualification from general XSD1.1/C45 acceptance. All21,358 source/19,701 overlapping package tests pass; API55names/663rows and signed Core1.86.0 are unchanged. Structural sample improves32→38 matches; datatype454matches remains co-blocked. Original invoice workflow passes. Earlier dated entries are historical; publication remains separate.

## Evidence-driven coverage checkpoint — 26 November 2026

The [current coverage guide](docs/SPECIFICATION_COVERAGE.md) reconciles development 0.13 / signed Core 1.86.0 with historical audits. A separately frozen 598-case datatype sample has 454 matches / 144 Unsupported / zero errors; the structural sample remains 32 / 97. Next are adopted structural fixes and consumer trials within an up-to-400-contract ceiling. Published beta.1 remains narrower; no publication or full-conformance claim. Earlier dated entries below are historical.

## Native Gregorian fragment candidate — 26 November 2026

The 0.13 development candidate adds bounded gYear/gYearMonth/gMonth/gMonthDay/gDay with exact nullable components, REC timezone-aware comparisons, finite facets and original typed/member facts on signed Core1.86.0. GR-901–GR-1300 track400 behavioral contracts, including the shared equal-exclusive-endpoint schema correction. Qualification passes21,147 source/19,497 overlapping package tests; API55types/663rows adds15 without removals. Frozen21 and expanded32matches/97Unsupported retain zero errors. Existing lifetime/XML and explicit Unsupported boundaries remain. General legacy operators, ID graphs, full XSD1.1/C45 and publication remain separate. [Scope, bounds and example](docs/gregorian-400/GUIDE.md).

## Exact native duration candidate — 26 November 2026

The 0.12 development candidate adds bounded duration, yearMonthDuration and dayTimeDuration with exact independent months/seconds, finite facets and original selected-member facts on signed Core 1.86.0. Immutable DurationMonths, DurationSeconds and collapsed DurationValue reads retain owner/lifetime and XML boundaries. DU-701–DU-900 track 200 behavioral contracts. Qualification passes17,866 source tests and16,249 overlapping package tests; API55 type names/648 rows adds9 without removals. Frozen21 and expanded32matches/97Unsupported retain zero errors. Partial-order and lexical ambiguity exclusions remain explicit; general legacy duration operators, full XSD1.1/C45 and publication remain separate. [Scope, capacities and example](docs/duration-200/GUIDE.md).

## Native URI and IEEE candidate — 26 November 2026

The 0.11 development candidate adds bounded anyURI, float and double validation and immutable typed reads on signed Core1.86.0. URI collapse/ordinal equality and IEEE signed-zero/NaN/rounding semantics retain original nodes, member provenance and owner lifetime. NU-501–NU-700 track 200 behavioral contracts, not 200 new specification features. Qualification passes 16,196 source tests and 14,611 overlapping fresh package tests; API adds nine rows with no removals. Frozen 21 and expanded 32 matches / 97 Unsupported have zero errors. All nine signed Core dependencies verify. Reviewed green PR merge and exact-main CI remain delivery gates. Full XSD1.1, general PSVI/C45, augmentation and publication remain separate. [Scope, bounds and example](docs/numeric-uri-200/GUIDE.md).

## Native text datatype candidate — 26 November 2026

The 0.10 development candidate qualifies all 200 text contracts, TX-301–TX-500: normalizedString, token, language, XML Name/NCName/NMTOKEN, nonempty NMTOKENS and inherited finite string facets. All 14,548 source tests (1,567 internal / 12,981 public) and 12,981 overlapping fresh package-only public tests pass. Sixteen actual Core proof controls and 25 lifecycle/context controls supplement the 1,600 catalog checks. The public API remains 55 exported type names / 630 reflection rows, with zero additions or removals. All nine Core1.86.0 signed archives/DLLs match publication. Frozen21 and expanded25matches/104Unsupported retain zero mismatches or harness errors. Thirty historical contexts are reconciled with original fixture bytes unchanged. This is bounded behavioral coverage; no full-XSD/C45, public query factory or registry publication. [Scope and evidence](docs/text-200/README.md).

## Native list and union candidate — 25 November 2026

All 200 datatype contracts (DT-101–DT-300) are qualified in the 0.9 development candidate: binary/QName values plus bounded lists/unions, original member provenance and six immutable item-read methods. All 12,907 source tests (1,551 internal / 11,356 public) and 11,356 overlapping fresh package-only public tests pass; all nine signed Core 1.86.0 archives/DLLs match publication. API review preserves 55 exported type names and all earlier members, adding six item-read methods in this phase. The frozen pilot remains 21 matches; the expanded 129 cases now have 25 matches / 104 Unsupported / zero errors. Prior atomic/calendar packets and original upstream fixtures remain unchanged. These are behavioral contracts; they do not represent 200 new XSD features or complete specification coverage. No registry publication, public query factory or full-XSD/C45 acceptance. [Scope and sealed evidence](docs/datatypes-200/README.md).

## Datatype composition candidate — 25 November 2026

The 0.8 development candidate qualifies DT-101–DT-180: bounded hexBinary/base64Binary/QName values through both native facades, finite facets, original namespace contexts, actual owned Core provenance and detached immutable reads. All 11,848 source tests and 10,308 overlapping fresh package-only tests pass; all nine Core1.86.0 signed archives/DLLs match publication. API review preserves 55 exported type names and all existing members, adding seven reflection rows. The frozen pilot remains 21 matches; expanded samples now have20matches/109Unsupported/zero errors. Lists/unions (DT-181–DT-300) remain planned. No Schema publication, public query factory or full-XSD/C45 acceptance. [Scope and evidence](docs/datatypes-200/README.md).

## Exact native calendars - 25 November 2026

The 0.7 development candidate admits bounded dateTime/time/dateTimeStamp on signed Core 1.86.0 through both native facades, with exact fractional seconds, applicable finite facets, original typed facts and preserved lifetime/capacity gates. All 100 [calendar contracts](docs/calendar-100/README.md) are qualified: 11,024 source tests and 9,516 overlapping package-only public tests pass. API additions are three enum values and CalendarValue; existing exported type names and members are preserved. The frozen pilot is 21/21; the expanded sample remains 19 matches/110 Unsupported/zero errors. This supersedes earlier calendar-refusal/planned notices below. Published beta.1 and its controller remain historical; no Schema publication or general C45/full-XSD acceptance.

## Calendar batch: signed Core 1.86.0 baseline - 25 November 2026

The approved 100-contract [calendar batch](docs/calendar-100/README.md) first qualifies signed Core 1.86.0: all 10,248 source cases and 8,778 overlapping package-only public cases pass; all nine resolved Core DLLs match the verified signed publication. Development dependencies now select one Core baseline, 1.86.0. Existing calendar refusals and public APIs are unchanged at this checkpoint. Historical dossiers, the published beta.1 and its release controller remain unchanged. Ninety native calendar contracts remain planned; no Schema package publication.

# Bosak.Schema — Agent Guide


The [calendar transport prerequisite assessment](docs/calendar-transport-prerequisites/README.md) completes item 20 of the approved batch. Its 48 handled observations confirm current millisecond precision loss and native dateTime/time/dateTimeStamp refusals against the qualified package. Next is a separately adopted additive exact fractional-second Core contract. No production source/API/pin or publication changes occur; slice 19 qualification is reused without a new execution claim. The batch stops at 20.


## Mixed sequence content - 25 November 2026

Batch slice 19 admits finite sequence mixed content with actual complex governing types and xs:untypedAtomic aggregate facts. Original interleaved text, child order/types/attributes and physical paths remain; invalid children still prevent all fact publication. Empty mixed aggregate is one empty string atom, distinct from nil/Empty/FOTY0012. Existing original-node transport preserves payload annotation and actual atomic provenance. Mixed all/choice/empty/simple-content/assertion/default augmentation/complex nil remain excluded. The16value-bearing budget includes mixed aggregates, each under MaxStringCharacters. REQ-009 remains In Progress; API55types/413rows and signed Core1.81.17 are unchanged. [Contract and Core transport review](docs/mixed-sequences/README.md).


## Provided attribute default/fixed constraints - 25 November 2026

Batch slice 18 admits provided attribute default/fixed constraints across existing local/global/reference/group owners. Complete-type literals are checked eagerly, effective use/global fixed values compare exactly, and overrides of global fixed require identical values, including date calendar/offset identity. Explicit default+required and fixed+prohibited are Invalid. Provided defaults remain original; optional missing constrained attributes remain Unsupported pending augmented nodes, while required absence stays Invalid. REQ-009 remains In Progress; API55types/413rows and signed Core1.81.17 are unchanged. [Contract and qualification](docs/attribute-value-constraints/README.md).


## Provided element default/fixed constraints - 25 November 2026

Batch slice 17 admits default/fixed metadata on existing simple element declarations, validates literals against complete actual types/facets and compares provided fixed values with exact family equality. Original provided values and XML remain unchanged; default metadata does not replace provided content. Empty-present default/fixed application remains Unsupported pending augmented XDM text/provenance; optional absent elements remain absent. Nil suppresses default while fixed forbids nil. Complex constraints and reference overrides remain excluded. REQ-009 remains In Progress; API55types/413rows and signed Core1.81.17 are unchanged. [Contract and qualification](docs/element-value-constraints/README.md).


## Simple nillability and empty typed values - 25 November 2026

Batch slice 16 adds nillability on existing simple-valued element declarations with actual Core IsNilled facts and supported zero-atomic typed values. Any xsi:nil attribute on a nonnillable declaration is Invalid, including false. True nil requires no text/element children; complex nillability and assertion+nillability combinations remain Unsupported. Detached Empty=6, scalar IsRootNilled and node-data IsNilled(path) add three reviewed API rows. Opt-in scalar reads share captured original-node atomization; legacy decimal identity and FOTY0012 boundaries remain. REQ-009 remains In Progress; Core1.81.17 is unchanged. [Contract and transport review](docs/simple-nillability/README.md).


## Closed named simple-type imports - 25 November 2026

Batch slice 15 extends the explicit catalog overloads with closed imports of named simple-type libraries. Private immutable component namespace metadata preserves actual foreign type/derivation identities and same-local-name distinctions; source QName bindings remain intact. Includes/imports validate expected namespaces even on reused documents. Foreign global elements, complex owners, attributes/groups and no-namespace imports remain Unsupported. Current catalog/component/value bounds apply. All8,788prior outcomes remain unchanged. REQ-009 remains In Progress; public API remains55types/410rows and signed Core1.81.17 is unchanged. [Contract and qualification](docs/simple-type-imports/README.md).


## Closed schema inclusion - 25 November 2026

Batch slice 14 adds explicit closed schema catalogs to both owned facades through two additive Create overloads. Matching-namespace/context includes resolve relative identifiers and deduplicate cycles/diamonds without external I/O. Original per-source prefix bindings survive private assembly; chameleon conversion, differing effective form/assertion contexts and imports remain Unsupported. Catalog8/32KiB-per-source/64KiB-total/32KiB-aggregate limits apply. All 8,554 prior outcomes remain unchanged; original instance nodes and canonical declaration identities are retained. REQ-009 remains In Progress; nine signed Core 1.81.17 payloads are unchanged. [Contract and API review](docs/closed-includes/README.md).


## Named sequence model groups - 25 November 2026

Batch slice 13 adds closed named sequence groups at owner content roots and inside sequences, with exactly-one group occurrences. Forward/nested groups cache their original source particles; actual local declarations and anonymous types remain shared across owners. Circular model groups are Invalid under the REC; recursive element graphs through groups are separately Unsupported. Current scalar facets, attribute/assertion exclusions, namespaces and finite budgets remain. All 8,260 earlier outcomes are unchanged; 48 authored cases across both facades/three contexts and six native identity controls qualify the candidate. REQ-009 remains In Progress; API and Core 1.81.17 are unchanged. [Contract and qualification](docs/model-groups/README.md).


## Global element references - 25 November 2026

Batch slice 12 adds closed global element reference particles with canonical global declaration identity and independent occurrence bounds. Scalar references work in admitted sequence/all/choice shapes; admitted complex containers and simple-content terminals work in sequences. Forward references and eager globals preserve source-order root selection, original nodes and values. Recursive graphs and broader metadata remain Unsupported. All 7,966 previous tests retain their outcomes; 48 cases across both facades/three contexts plus six native identity controls bring source tests to 8,260. REQ-009 remains In Progress; public API and signed Core 1.81.17 payloads remain unchanged. [Contract and qualification](docs/global-element-references/README.md).


## Attribute-group review correction - 25 November 2026

The corrected candidate preserves the existing nested structural-assertion/application-attribute exclusion for group members. A supported namespace-uri(.) probe reproduced six unintended compile-Valid outcomes; the shared member predicate now retains Unsupported. Original candidate receipts remain sealed, and the final candidate passes7,966source/6,550reviewed package cases. No API/Core/publication change. [Corrected evidence](docs/attribute-groups-review/README.md).


## Attribute groups with component-preserving union - 25 November 2026

Batch slice 11 adds closed acyclic named groups and actual attribute-use set union on existing owners. Repeated/shared diamond components deduplicate by identity; distinct same-name uses are Invalid. Required flags, global/group-local declarations, original facts and private compilation contexts remain intact. Cycles are Unsupported rather than Invalid because XSD1.1 permits circular groups. Nine measured old results are reconciled without input/ID changes: six valid group cases, two Invalid duplicate-use schemas and one refined missing-group refusal reason. REQ-009 remains In Progress; API and signed Core 1.81.17 payloads are unchanged. [Contract and qualification](docs/attribute-groups/README.md).


## Global attribute declarations and references - 25 November 2026

Batch slice 10 adds closed typed globals and optional/required reference uses on existing attribute owners. Canonical global declaration handles remain shared across uses; each owner keeps its presence constraint and original node origin. Private per-schema context and lookup avoid ambient state or per-use authoritative copies. One measured old malformed ref+name/type case now returns Invalid with unchanged input. Global namespaces, scalar facets, eager unused declarations and capacities are qualified. REQ-009 remains In Progress; public API and signed Core 1.81.17 payloads are unchanged. [Contract and qualification](docs/global-attribute-references/README.md).


## Decimal complex enumeration restriction - 25 November 2026

Batch slice 9 enables local/inherited exact enumeration on the existing attribute/assertion-free decimal complex restriction route, using qualified native base-value validity and narrowing rules. Actual governing complex and anonymous atomic components keep their bases and authority. Fourteen measured old results are reconciled without changing payloads/IDs: six base-invalid schemas are Invalid and eight valid cases compile/assess successfully. Integer/date routes and legacy decimal-only grammar remain unchanged. REQ-009 remains In Progress; API and signed Core 1.81.17 payloads are unchanged. [Contract and qualification](docs/decimal-complex-restriction/README.md).


## Named decimal restriction chains - 24 November 2026

Batch slice 8 adds closed named decimal bases and forward references with actual DerivationBase identity, inherited exact enumeration/range/digit constraints and base-valid declaration checks. Bounds and digit facets narrow the base; same-side exclusive endpoint repetition follows the REC exception. Six measured old results are reconciled with unchanged payloads/IDs. The explicit native derivation path leaves the prior complex/legacy route separate until slice 9. REQ-009 remains In Progress; public API and signed Core 1.81.17 payloads are unchanged. [Contract and qualification](docs/decimal-chains/README.md).


## Direct decimal enumeration - 24 November 2026

Batch slice 7 adds bounded enumeration on direct decimal restrictions in the opt-in facades. Normalized exact coefficient/scale membership preserves original instance XML and typed values, including signed zero and equivalent lexical forms beyond System.Decimal precision. Sixteen source entries and existing raw numeric/digit/scale limits apply before normalization. Legacy enumeration refusal remains; complex restrictions over enumerated decimal bases are explicitly deferred. Two measured IE-093 results are reconciled with actual Decimal reads and unchanged inputs. REQ-009 remains In Progress; API and signed Core 1.81.17 payloads are unchanged. [Contract and qualification](docs/decimal-enumeration/README.md).


## Named Boolean restriction chains - 24 November 2026

Batch slice 6 adds closed named Boolean bases, forward references and facet-free aliases with inherited pattern conjunctions and actual DerivationBase identity. Same-step alternatives, fixed-collapse whitespace, primitive lexical validity and original Boolean/XML facts remain. Automata are compiled once per definition; inherited matching shares the assessment budget/cancellation. Six measured old refusal outcomes are reconciled without changing payloads/IDs. REQ-009 remains In Progress; API and Core 1.81.17 are unchanged. [Contract and qualification](docs/boolean-chains/README.md).


## Direct Boolean restrictions - 24 November 2026

Batch slice 5 adds direct Boolean patterns, facet-free aliases and explicit collapse whitespace on existing scalar placements. Primitive validity and patterns precede original Boolean fact issuance. Boolean enumeration is inapplicable under XSD 1.1 and is Invalid; assertions, named Boolean base chains, fixed attribute syntax and Boolean complex content stay separate. Nine measured original outcomes are reconciled with original payloads/IDs unchanged, including actual Boolean reads in two older catalogs. REQ-009 remains In Progress; Core 1.81.17 and API are unchanged. [Contract and qualification](docs/boolean-patterns/README.md).


## Bounded date patterns - 24 November 2026

Batch slice 4 admits the existing finite pattern grammar on direct, named and bounded complex date restrictions. Same-step alternatives and inherited conjunctions check normalized literals; original date components, timezone presence and XML stay intact. New bounds over a patterned ancestor remain Unsupported pending a separate value-space contract. String/date states and matching work share their respective schema/assessment budgets. Numeric complex patterns retain their existing refusal. REQ-009 remains In Progress; Core 1.81.17 and the public API are unchanged. [Contract and qualification](docs/date-patterns/README.md).


24 November 2026: authorized batch slice3 adds attribute/assertion-free date complex simple-content restriction with distinct governing/atomic components and exact caller capacities. [Contract](docs/date-complex-restriction/CONTRACT.md). Ten measured old results change without changing fixtures/IDs, including two unused-definition schemas whose string roots remain string-valued. No public API/Core/publication change.

24 November 2026: authorized batch slice2 adds named date restriction chains, closed forward references and inherited facet/timezone validation. [Contract](docs/date-chains/CONTRACT.md). Six measured original results change; original inputs/IDs remain. Core1.81.17, public API and publication boundaries remain unchanged.

24 November 2026: authorized specification batch slice1 adds bounded date bounds/explicitTimezone on Core1.81.17. [Contract](docs/date-bounds/CONTRACT.md); 25measured original results reconciled without changing stimuli/IDs. Full XSD1.1 and publication remain separate. Batch commit/push/PR/green-merge authorization persists for up to20slices.

24 November 2026: [bounded date enumeration production candidate](docs/date-enumeration-implementation/README.md) adopts direct date membership with preserved timezone presence/original values on Core1.81.17. Source4,874cases, package3,509public/20native, six examples and unchanged55type/408rowAPI qualify. Expanded sample16matches/113Unsupported/zeroerrors. Retained migration passes124/22/32behavior cases; two full historical catalogs retain their explicit old-Core pin assertion failures. REQ-009 remains In Progress; no publication.

24 November 2026: [isolated date-enumeration feasibility](docs/date-enumeration-feasibility/README.md) passes428native controls and4,442regression results after three measured expectation updates. API55types/408rows unchanged; expanded sample16matches/113Unsupported/zero errors. Original container comments are already admitted; scalar comments remain refused. Production enumeration behavior and earlier sealed evidence are unchanged.

24 November 2026: [date-enumeration/value-equality contract](docs/date-enumeration/README.md) prepared for review on the Core 1.81.17 baseline candidate. Proposed direct date restrictions preserve timezone presence and original typed representations; no implementation/adoption or new semantic execution is claimed. REQ-009 remains In Progress.

24 November 2026: the development candidate now requires **Core 1.81.17**. New qualification uses this single baseline; earlier two-pin evidence and published beta.1 remain historical and unchanged. [Dependency adoption](docs/core-18117-baseline/README.md).

> **Canonical agent instructions** for the Bosak.Schema project.
> Deeper `AGENTS.md` files in subdirectories override these instructions for their scopes.

---

## 1. Project Identity

**Bosak.Schema** is a Apache-2.0 schema integration and staged XSD 1.1 services for the Bosak XPath 3.1 / XSLT engine.

- **Solution:** `Bosak.Schema.slnx`
- **Framework:** .NET 10
- **Target:** net10.0

---

## 2. Build & Test

```bash
# Full build
dotnet build Bosak.Schema.slnx

# Run all tests
dotnet test Bosak.Schema.slnx

# Run with verbosity
dotnet test Bosak.Schema.slnx --logger "console;verbosity=detailed"

# Full clean rebuild (clean → restore → build → test)
pwsh tools/rebuild.ps1

# Documentation hygiene check
pwsh tools/check-docs.ps1 -ProjectPath .
```

**Rule:** All tests must pass before a task is considered complete.

---

## 3. Code Conventions

### 3.1 File Headers

**Every `.cs` file** (excluding generated files in `obj/`, `bin/`) must begin with the following header:

```csharp
// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : <dd MMMM yyyy>
// PURPOSE              : <One-line description of the file's responsibility>
// SPECIAL NOTES        : <Implementation notes, assumptions, or layer-specific warnings>
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | <dd-MM-yyyy>   | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
```

**Rules:**
1. **PURPOSE** — Derive from the XML `<summary>` of the primary type. Keep under 100 characters.
2. **Change History** — When modifying an existing file, append a new row with the current date, a bumped version number, and a brief note.
3. **Generated files** — Skip `obj/`, `bin/`, and any file containing "Generated" or "Auto-generated" in the first 200 characters.

### 3.2 Naming & Language

- Use **English** for all identifiers, comments, and documentation.
- Use `PascalCase` for classes, records, methods, enums, and public members.
- Use `camelCase` for local variables and parameters.
- Use `PascalCase` for acronyms longer than two characters (e.g., `XmlDocument`, not `XMLDocument`).
- Prefer `sealed` classes unless inheritance is explicitly designed.
- Use `record` or `record struct` for immutable DTOs.
- Use `file`-scoped namespaces when possible.

### 3.3 Nullability

All projects must have `<Nullable>enable</Nullable>`. Always annotate nullability in public APIs.

### 3.4 Implicit Usings

All projects should have `<ImplicitUsings>enable</ImplicitUsings>`. Do not redundantly import `System` namespaces.

### 3.5 Build Enforcement

Conventions are enforced by the build, not just by review:

- Root `Directory.Build.props` — `TreatWarningsAsErrors`, `AnalysisLevel=latest`, `EnforceCodeStyleInBuild`, MinVer versioning, shared package metadata.
- Root `.editorconfig` — formatting and analyzer severities.
- Root `global.json` — .NET SDK pin.

---

## 4. XML Documentation

All projects must generate documentation files:

```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

**Rules:**
- Every `public` and `protected` type and member must have a triple-slash XML comment (`///`).
- Use `<summary>` for the primary description.
- Use `<param>` for every parameter.
- Use `<returns>` for methods with non-void return types.
- Use `<exception>` when a method throws documented exceptions.
- Use `<remarks>` for extended context or examples.

**Example:**

```csharp
/// <summary>
/// Parses the supplied EDI payload into a canonical evidence set.
/// </summary>
/// <param name="payload">Raw EDI text. Must not be null.</param>
/// <param name="options">Optional parsing flags.</param>
/// <returns>A <see cref="CanonicalEvidenceSet"/> containing extracted evidence.</returns>
/// <exception cref="ArgumentNullException"><paramref name="payload"/> is null.</exception>
public CanonicalEvidenceSet Parse(string payload, ParseOptions? options = null)
```

---

## 5. Testing Requirements

### 5.1 Test Organization

| Test Project | Purpose |
|--------------|---------|
| `Bosak.Schema.Tests` | Activation-free factories, schema compilation/validation and legacy API compatibility |

The published beta source solution has 345 cases: 312 implementation and 33 public-consumer cases without friend access. Published-package and retained-binary lanes repeat scoped catalogs and are not additive coverage. Core qualification uses 1.81.2; the bounded ADR009/010 contract is accepted. See docs/BETA1_RELEASE.md for exact publication identities and exclusions.

### 5.2 Test Patterns

- Use the same test framework across all projects (MSTest, xUnit, or NUnit — pick one and stay consistent).
- Test classes are marked with the appropriate attribute (`[TestClass]`, `[TestCollection]`, etc.).
- Test methods describe the behavior under test: `MethodName_Scenario_ExpectedResult`.
- Use `NullLogger<T>.Instance` when testing services that require `ILogger<T>`.
- Clean up temporary files after tests.
- Add golden-file tests for serialization, XML, or document output.
- Place shared sample/golden data in `tests/TestData/`; suite-specific data in `tests/<Suite>/` (e.g., `Schemas/`).

### 5.3 Coverage Rule

New functionality must have **at least one unit test** covering the happy path and **one test** covering a failure mode or edge case.

---

## 6. Versioning

- Use [Semantic Versioning](https://semver.org/) (`Major.Minor.Patch`).
- Versions are produced **from git tags** (`v1.2.3`) by MinVer, configured in root `Directory.Build.props`. Never hard-code `<Version>` in project files.
- Bump `Minor` for new features; bump `Patch` for bug fixes (tag accordingly).
- Record version bumps in:
  - File header change-history tables
  - `docs/FEATURE_REQUESTS.md` target versions
  - `docs/ARCHITECTURE.md` status line
  - `docs/INTEGRATION.md` version history
  - `ROADMAP.md` milestones

### 6.1 Architecture Decision Records

- **Minor decisions** go in the `docs/ARCHITECTURE.md` *Key Design Decisions* table.
- **Cross-cutting or expensive-to-reverse decisions** get an ADR: copy `docs/ADR-000-TEMPLATE.md` to `docs/ADR-NNN-<slug>.md` (NNN = next zero-padded number, never reused).
- Link ADRs from the REQ detail sections that motivated them.

---

## 7. Documentation Sync Checklist (Mandatory)

After **every** successful implementation step, update the following canonical documents before concluding the step or session:

| File | Purpose | When to Update |
|------|---------|----------------|
| `README.md` | Human-facing project overview | Structural, CLI, or dependency changes |
| `ROADMAP.md` | Release stages, milestones, known limitations | Stage or milestone change |
| `docs/ARCHITECTURE.md` | Package tiers, dependency rules, use cases, canonical models | New projects, tier changes, new use cases, API additions |
| `docs/FEATURE_REQUESTS.md` | Living registry of all REQ items | Any REQ status change, new REQ, or completed item |
| `docs/INTEGRATION.md` | Consumer integration guide, APIs, extension points | New public APIs, CLI commands, behavioral changes |
| `docs/DOCUMENTATION_STYLE_GUIDE.md` | Documentation & diagram rules | Any documentation-style change |
| `docs/ADR-*.md` | Architecture decision records | Any significant decision (one new file per decision) |
| `docs/AGENT_HANDOVER.md` | Session state and canonical agent context | Major architectural shifts, current session scratchpad |

**Rule:** If a file was modified during the step, its documentation counterpart must be updated in the same step. No exceptions.

### Pre-Handover Checklist

```markdown
- [ ] `README.md` — Repository tree, build/test commands, and quick-start are current.
- [ ] `ROADMAP.md` — Stage and milestone status reflect reality.
- [ ] `docs/ARCHITECTURE.md` — Status line reflects reality; use-case catalog and project matrix are current.
- [ ] `docs/FEATURE_REQUESTS.md` — Registry table and detail sections are accurate; "Last updated" date is today.
- [ ] `docs/INTEGRATION.md` — Feature status matrix is accurate; version history appended for behavioral changes.
- [ ] `docs/AGENT_HANDOVER.md` — Date and commit hash match latest; "What Was Built" includes every feature/bugfix.
- [ ] `AGENTS.md` — Coding conventions still match reality.
```

---

## 8. Git Rules

- Never run `git commit`, `git push`, `git reset`, `git rebase` unless the user explicitly asks.
- Never modify files outside the working directory unless explicitly instructed.
- Keep commits focused; one logical change per commit.

---

## 9. Related Documents

| Document | Purpose |
|----------|---------|
| `README.md` | Human-readable quick start |
| `ROADMAP.md` | Release stages, milestones, known limitations |
| `docs/ARCHITECTURE.md` | Package tiers, dependency rules, use cases |
| `docs/FEATURE_REQUESTS.md` | Living registry of cross-cutting feature requests |
| `docs/INTEGRATION.md` | Consumer integration guide |
| `docs/DOCUMENTATION_STYLE_GUIDE.md` | Documentation & diagram rules |
| `docs/AGENT_HANDOVER.md` | Session state for AI agents |

---

*Last updated: 2026-11-27*



## Apache and Public Repository Boundary

Schema services require no activation. Legacy License/ActivatedLicense types are compatibility shapes only. No private Git history, credential issuer/verifier, signing material, licensing/payment infrastructure, private deployment documents or commercial test vectors may be copied here. Preserve Fytala copyright/SPDX notices and record semantic adoption evidence before new standards behavior. Full XSD 1.1 support and core/session API acceptance remain pending.

## Historical dependency and documentation checkpoint

Product and test Core references select published 1.81.2 after Schema PR #1. The public-entry candidate advances Schema's MinVer development minimum to 0.5; no new Schema package release is implied. Standalone124-case regression coverage and the separate Core 135-case optional integration have distinct scopes. Use `docs/AGENT_HANDOVER.md` for current state and `docs/DEVELOPMENT_HISTORY.md` for archived local review notices; older inputs keep their original source hashes and do not replace current package pins. Full XSD 1.1 structural/native session/lease coverage and performance acceptance remain separate tasks.


## Release automation controls

Workflow-only release controls are separate from the current 345 product/source cases (published alpha: 272). Run `python -m unittest discover -s .github/scripts -p test_schema_release.py` and `node --test .github/scripts/test-schema-source-run.cjs`. They use synthetic artifacts and mocked GitHub/network responses; they do not log in or push. `.github/workflows/release.yml` admits only manually selected main preparation/publication for the recorded first prerelease, with OIDC permission confined to the publish job. Neither passing controls nor a prepared artifact authorizes registry publication. Keep the product source/Core pins and earlier immutable receipts unchanged.


## Restricted resolution candidate

The local restricted resolver/XSLT factory requires stable host-owned filesystem paths, exact captured root spelling and explicit byte limits. It is not an atomic filesystem sandbox. Use docs/RESTRICTED_RESOLUTION.md; qualify nested acquisition through published Core policy, not a fallback-capable convenience callback. All 31 restricted implementation cases and three added public consumer cases are included in the current suite (306 cases at the restricted-resolution checkpoint). Preserve the published alpha/audit evidence as historical inputs; beta acceptance and publication remain separate.


## Compiler snapshot ownership

Compiler-produced descriptors own immutable outer and nested collections; preserve shared assertion identity across root/facet lists. Public record constructors/with copies still accept application-owned data. Use docs/DESCRIPTOR_SNAPSHOTS.md and do not extend compiler guarantees to arbitrary ICompiledSchema implementations. Snapshot checkpoint319; historical verifiers must run against their recorded source revisions.


## Legacy validation compile findings

All three legacy validation entry points validate null arguments, snapshot compile findings and return false before other model/typed/assertion work. Preserve original findings and scoped clean success; do not reinterpret IsValid as structural certification. See docs/LEGACY_VALIDATION.md. Current suite345; historical API/dossier records retain their original source scope.


## Workload qualification

Read docs/BETA_QUALIFICATION.md before measuring workloads. Build tools/Bosak.Schema.Workloads separately; run six Python controls with `python -m unittest discover -s tools -p test_schema_workloads.py`. Use tools/run-schema-workloads.py with a new ignored .workload-results output directory. No unhandled negative diagnostic launches or machine-wide WER changes. Proposed local timing/memory envelopes are not acceptance or SLAs. Product345/release45/runner6 scopes remain distinct.


## Beta contract review boundary

Use docs/BETA_CONTRACT_REVIEW.md for the current concrete ADR009/010 proposal. The owner accepted ADR009/010 for the bounded Schema beta scope on17 November2026; broader acceptance is not inferred. Local alpha compatibility packages are not beta artifacts. General Core C45/ADR006, production SLAs, exact beta tag/controller/artifact and publication remain separate; do not infer their acceptance from package/binary passes.


## Beta.1 release candidate — historical 17 November checkpoint

Read docs/BETA1_PREPARATION.md before release operations. Exact source2a5abf6fb663313dc32bfeca5bbb8740bbe987ae/tagv0.5.0-beta.1; controller requires345/33 and retained124/23 lanes, nine signed Core1.81.2 payloads.32 Python +22 GitHub provenance controls, six runner controls. Local manifest zero controller is never hosted evidence. No remote tag or beta publication yet; do not infer publication authority from preparation. Historical dossiers remain unchanged.


## Current published beta

Bosak.Schema0.5.0-beta.1 is published on NuGet.org and GitHub Releases. Read docs/BETA1_RELEASE.md and docs/beta1-publication/RECEIPT.json for current status. Earlier guides/dossiers retain their original checkpoint claims and hashes; run historical verifiers at their recorded revisions. The release controller remains pinned to this already-published version and must not be dispatched for a duplicate push. Future versions require fresh review and explicit approval. GitHub Packages is separate and unused.

## Unsupported-reason development candidate

19November2026: the local0.6candidate has365source cases(312implementation/53public), including20new detected-reason controls. Published beta.1 remains345. Preserve stage codes/messages, refusal order and grammar; use docs/unsupported-reasons/CONTRACT.md. Package-only53 and retained124/23 lanes overlap source coverage. No commit/push/publication without explicit request.

## M1 namespace development candidate

19November2026: local409cases(315implementation/94public),41authored namespace cases+threeactualCore controls. Read docs/m1-namespaces/ADOPTION.md before semantic edits. Preserve expanded-name identity, assertion namespace capture, zero-to-eight assertions, limits and explicit unsupported boundaries.94package-only/42current-Core and124/23/33retained lanes overlap. Core runtime/pins and published beta.1 remain unchanged.

## M2A named complex type candidate

19November2026:465source cases(320implementation/145public). Read docs/m2a-named-types/ADOPTION.md before semantic edits. Preserve16global type/root ceilings, eager unused-definition refusal, QName/default namespace resolution, shared type identity and foreign/disposed authority guards.145minimum-Core/52current-Core/124-23-33retained lanes overlap. Core pins, published beta.1 and release controller remain unchanged.

## M2B decimal restriction candidate

19November2026:534source cases(324implementation/210public). Read docs/m2b-decimal-restrictions/ADOPTION.md before semantic edits. Preserve exact value-space facets, representation-before-facet gates, combined16type limit, eager unused-definition refusal and named declared/actual provenance. No general derivation relation or new datatype/attribute/import support; no commit/push/publication without explicit request.

## M2C1 string/decimal candidate

19November2026:614source cases(334implementation/280public). Read docs/m2c-string-facade/ADOPTION.md before semantic edits. New NativeSchemaSession opt-in shares the native owner/gate; legacy NativeDecimalSession stays decimal-only. Preserve XML string text, explicit UTF16/output capacities, original typed facts and bounded export/cancellation. No Boolean/integer/named string facets or Core engine/pin/publication expansion. API/C45 freeze remains separate; no commit/push without explicit request.

## M2C2 Boolean candidate

19November2026:671source cases(340implementation/331public). Read docs/m2c-boolean/ADOPTION.md before semantic edits. Opt-in Boolean leaves use true/false/1/0 after XML-whitespace collapse with an independent256UTF16lexical ceiling. Preserve false-vs-absence, actual Boolean facts/provenance and legacy decimal refusal. Boolean assertions/restrictions and integer support remain excluded. No commit/push/publication without explicit request.

## M2C3 integer candidate

19November2026:748source cases(347implementation/401public). Read docs/m2c-integer/ADOPTION.md before semantic edits. Exact built-in integer leaves retain original typed facts, BigInteger values, supplied-digit capacity and fixed256UTF16lexical ceiling. Preserve legacy decimal refusal and separate read/query capacities. Integer assertions/derived restrictions remain excluded. No commit/push/publication without explicit request.

## M2D1 string restriction candidate

19November2026:837source cases(355implementation/482public). Read docs/m2d-string-restrictions/ADOPTION.md before semantic edits. Only opt-in direct named xs:string restrictions admit Unicode scalar length/minLength/maxLength and exact ordinal enumeration. Preserve parsed UTF16representation limits,16supplied enumeration ceiling, original named facts, legacy decimal refusal and decimal-only assertions. Pattern/whiteSpace/anonymous types/restriction chains remain excluded. No commit/push/publication without explicit request.

## M2E1 attribute contract planning

19November2026: current837source suite is unchanged after PR23 merge098fdaf. Read docs/m2e-attributes/CONTRACT.md and proposed ADR012 before attribute adoption/implementation.27synthetic public Core probes per minimum/current lane qualify attribute facts/transport only, not native XSD validation. Proposed root-only separate attribute reads preserve leaf indexing and old decimal behavior; no new API/adoption or release acceptance yet. No commit/push/publication without explicit request.

## M2E1 root attribute candidate

19November2026:950source cases(372implementation/578public), all837prior outcomes unchanged. Read docs/m2e-root-attributes/ADOPTION.md before semantic edits. Root-only opt-in attributes have separate counts/indexes/original facts and detached typed reads. Preserve declaration-order optional omission, form/type/normalization/capacity rules, no partial facts, legacy decimal refusal, same gate/lifetime and old no-attribute/leaf-attribute behavior. Sealed planning dossier remains historical. No commit/push/publication without explicit request.

## Decimal simple-content candidate

20November2026:1036source cases(386implementation/650public), all950prior outcomes unchanged. Read docs/decimal-simple-content/ADOPTION.md before edits. Only direct decimal simpleContent extension with current root attributes is admitted. HasRootScalar/ReadRootScalar never change leaf/attribute indexes; root complex annotation/declared type differs from actual decimal atomic type. Preserve normalized root string-value versus lexical export, old rootFOTY0012, owner/cache/gate and legacy decimal refusal. No commit/push/publication without explicit request.


## Nested-sequence planning

20 November 2026: PR25/main c34e9fe is verified at 1,036 source cases. Read docs/nested-sequences/PLAN.md and CONTRACT.md before nested work. These are proposals, not semantic adoption or implemented support. Preserve full element facts and separate terminal scalar indexing; no commit/push without explicit request.


## Nested-sequence pre-implementation adoption

20 November 2026: read docs/nested-sequences/ADOPTION.md before production edits. Separate public Core feasibility is 50 synthetic controls per minimum/current lane, not native validation. 1,036 source cases remain unchanged; the64node budget is defensive under depth4/16terminal bounds. Preserve old receipts and explicit commit/push rule.


## Nested-sequence implementation candidate

20 November 2026: local1,125source cases(395implementation/730public), prior1,036unchanged. Read docs/nested-sequences/ADOPTION.md before semantic edits. Separate complete element facts from terminal scalar projection; preserve local/global declaration identity, eager cycle/expansion/refusal gates, legacy facade and lifetime rules. Package/retained qualification is separate. No commit/push without explicit request.


## Nested container attribute planning

20 November 2026: read docs/nested-attributes/PLAN.md, CONTRACT.md and proposedADR014 before descendant-attribute work. Base3e067cd/main1,125cases remains unchanged. Three-member/path proposal is not implemented or semantically adopted. Keep root/terminal indexes and original owner/use facts; explicit commit/push rule still applies.


## Nested container attribute pre-implementation adoption

20 November 2026: read docs/nested-attributes/ADOPTION.md before implementation.69synthetic public Core controls per signed lane and four native/API baseline controls pass, not new native support. Bind facts to exact original attribute owner/node, preserve root/scalar indexing and64total potential-use budget. Production/API remains unchanged; no commit/push without explicit request.


## Nested attribute implementation candidate

20 November 2026: local1,200source cases(404implementation/796public);1,123prior unchanged and NS-049/075intentional admission recorded before edits. Read docs/nested-attributes/ADOPTION.md before semantic work. Preserve exact captured owner/fact identity, separate root/descendant/scalar indexes, detached immutable paths and64total potentialuses includingroot/optional/repetition. Three new API members, no new type. No commit/push without explicit request.


## Nested decimal simple-content planning

20 November2026: read docs/nested-simple-content/PLAN.md and CONTRACT.md before this next slice. Base32f8214/main1,200cases verified. Proposed zeroAPI delta, scalar-bearing complex terminals and depth4three-segment owner paths are not adopted or implemented yet. Preserve prior receipts and explicit commit/push rule.


## Nested decimal simple-content implementation

20 November2026: read docs/nested-simple-content/ADOPTION.md before semantic edits. Local1,255cases(415implementation/840public),1,198prior unchanged/two adopted admissions/55new. Preserve complex declared versus actual decimal provenance, root/local declaration identity, original captured owner paths up to3segments at depth4, scalar/root index separation and old budgets. No new public API or Corepin. No commit/push without explicit request.

## Frozen upstream conformance pilot

20 November 2026: read docs/conformance-pilot/README.md and MEASUREMENT-CONTRACT.md
before pilot work. Preserve the 21 cases, original 42 Git-blob files, notices,
expected validity and frozen manifest bytes. Unsupported is never a match; all
21 baseline cases are Unsupported. Do not rewrite fixtures or relax xsi:type
handling to obtain passes. New semantics require specification adoption first.
Driver is manual and nonpackable; no new gate or release. Explicit commit/push rule applies.

## Explicit-schema hint policy

20 November 2026: read docs/schema-location-hints/ADOPTION.md before semantic edits.
Only opt-in NativeSchemaSession admits exact xsi location hints without fetching;
preserve original XML, typed application indexes, legacy refusal and xsi:type/nil
refusal. 1,302 source tests pass, including all 1,255 prior outcomes and 47 new cases.
Frozen upstream baseline stays sealed; candidate separately yields 1 match / 20
Unsupported / zero mismatches/errors. Candidate harness requires exact source and
runtime evidence. No full-suite claim, API/pin/publication change or implicit delivery.

## Anonymous local restriction adoption

20 November 2026: read docs/anonymous-simple-restrictions/ADOPTION.md before
production edits. Base merged PR29/main1438f71 has1,302passing source tests.
Adopt only opt-in local decimal/string direct restrictions with original anonymous
identity; require public Core feasibility before implementation. NS-051 original
row and six upstream targets are pinned, not executed admissions. Combined16local
anonymous definition budget, legacy refusals and immutable corpus remain protected.
No implementation/API/pin/publication change; commit/push needs explicit request.

## Anonymous simple-type transport feasibility

20 November 2026: FEASIBILITY.md records90synthetic transport controls per signed
Core lane(13new/77inherited),18signature/payload checks and six actual unchanged
Schema baseline controls. Transport succeeds, but native anonymous restrictions
remain Unsupported. Read original ADOPTION.md before implementation, preserve
NS-051 receipt and sealed corpus. No production/API/Corepin/publication change;
native/package/retained qualification remains pending. No commit/push requested.

## Anonymous local restriction implementation

20 November2026: read original ADOPTION.md and implementation/README.md.
1,373source cases pass(421/952); NS-051 is the one adopted prior admission.
Preserve original anonymous type identities, eager combined16local definition
budget and shared facets. Package-only952minimum/952current and retained124/23/33
pass. Frozenpilot7matches/14Unsupported/0errors; no fullconformance claim.
Native advisor stays scalar-only; legacy data(.) on absent QName is unqualified.
No public API/Corepin/publication change; commit/push requires explicit request.

## Post-PR30 specification planning

20 November2026: merged base5bfbb1e has the exact reviewed PR30 tree. Read
 docs/specification-next/README.md and INTEGER-CONTRACT-PROPOSAL.md before next
work. Integer restriction scope is proposed, not adopted/implemented. Preserve
frozen21case/42file corpus, sevenmatches/fourteenUnsupported checkpoint, scalar
query boundary and historical receipts. New semantics need exact adoption first.
No source/API/pin/publication change; commit/push requires explicit request.

## Integer restriction implementation

20 November2026: read docs/integer-restrictions/ADOPTION.md and route clarification.
Exact named/anonymous integer facets and named attribute uses are qualified;
preserve fixedfractionDigits0, BigInteger, original identities and old limits.
1,482source(427/1,055), package1,055/1,055,retained124/23/33andAPI52/392pass.
Two prior opt-in admissions only; legacyM2B-54 stays refused. Frozenpilot9matches/
12Unsupported/0errors is selected validity evidence, not fullXSD. Nativeadvisor
stays scalar-only; no Corepin/API/publication change. No commit/push requested.

## Original-node atomization contract proposal

20 November2026: merged PR31/main43f14db is verified at1,482source cases. Read
 docs/typed-node-contract and proposed ADR016 before node integration. Released
Core has an exact bounded original-node-data profile; do not promise arbitrary
XPath or relax existing scalar Bind. Proposed separate owned data session must
assess in its own environment and resolve exact original targets; no rebind of
existing scalar documents. Feasibility/adoption/API review remain pending; no
source/API/Corepin/publication change. Commit/push requires explicit request.

## Actual native original-node data feasibility

20 November2026: FEASIBILITY.md records73native controls per signed Core lane,
eight baseline controls and1,482unchanged candidate regression outcomes. Two
isolated source copies change: provider data-mode/context binding and guarded
ISchemaAnnotatedNode facts. Real src/ stays unchanged. Preserve iteration failures;
Core FOTY0012 is InvalidOperationException, proposed facade mapping still pending.
Before production edits record semantic adoption and resolve API/lifetime gates.
No public API/Corepin/package/publication or new pilot change; no commit/push request.


## Original-node scalar data (0.6 development candidate)

`NativeSchemaNodeDataSession` is a separate owned session for fixed original-node
atomization. Create it with existing explicit `NativeSchemaSessionOptions`, then
Assess with `NativeSchemaAssessmentContext`. A valid assessment returns an owned
`NativeSchemaNodeDataDocument`. `ReadElement(ImmutableArray<int>)` selects original
element-child indexes (empty selects root, maximum three segments).
`ReadAttribute(ownerElementPath, namespaceUri, localName)` selects a present original
attribute by expanded name. Both return one detached `NativeScalarValue`.
`ExportXml` returns bounded parsed lexical XML. Dispose documents and session.

The existing bounded schema grammar and native scalar families apply. Element-only
containers refuse reads with NotSupportedException carrying FOTY0012. Default paths,
invalid names and missing targets throw documented argument exceptions; query/cache
capacity and unassessed hint attributes are Unsupported read operations, not Invalid
instance assessments. Operations are sequential; overlap is refused and disposal
waits for admitted synchronous work. No expression, raw node, provider authority,
callback, general XPath, list/nil support or transferable PSVI is exposed.

This additive API is implemented locally for review. ADR-016 owner API acceptance,
general Core C45 and full XSD 1.1 conformance remain pending. Published 0.5.0-beta.1
and the existing decimal/scalar facades are unchanged. See [implementation adoption](docs/native-node-data/ADOPTION.md).


## Owned node-data qualification ? 20 November 2026

Local candidate on base `43f14dbf56c36e5185e705af901678c42c8d1aff` passes
1,615 source cases (503 implementation / 1,112 public), preserving all 1,482 prior
named outcomes. Package-only minimum Core 1.81.2 passes 1,112; Core 1.81.17 passes
1,111 shared cases plus its explicit current-version check. The deliberately
minimum-version-only surface test is excluded only from that current lane.
Retained 124/23/33 unchanged callers pass with only Schema.dll replaced. Counts
overlap. All 392 prior public surface rows are preserved; two sealed owned types
add nine rows (54 types / 401 rows total). Frozen pilot remains 9 matches / 12
Unsupported / zero mismatch or harness error. No new XSD expectations or grammar.

The local unsigned package is `0.6.0-alpha.0.19`; published beta.1, Core source/pins,
release controllers and tags are unchanged. The earlier sealed proposal and
feasibility packets are unchanged. Owner API acceptance/ADR-016 and delivery
remain pending. No commit or push has been performed for this candidate.


## Owned node-data contract review ? 20 November 2026

The local code/API self-review found no blocking implementation issue. The current
review addendum supersedes the sealed original ADR's historical pending-implementation
wording; ADR-016 owner acceptance and explicit commit/push/PR authorization are
still pending. Qualification and sealed packets are unchanged. [Review and prepared delivery](docs/native-node-data-review/README.md).


## Bounded ADR-016/API accepted ? 20 November 2026

The owner accepted the reviewed two-type/seven-method owned original-node data
contract and authorized commit, push and PR delivery. This dated acceptance
supersedes historical pending-owner-acceptance wording in the sealed ADR/proposal,
feasibility, implementation and review packets; those originals remain unchanged.
All qualified source/binary identities remain unchanged. General C45/ADR006,
full-XSD/general XPath and release/publication decisions remain separate.
[Owner acceptance](<docs/native-node-data-acceptance/README.md>).


## Bounded string assertions (0.6 development)

The opt-in scalar and owned node-data sessions admit string equality assertions
on declared immediate children of flat complex content. Primitive and existing
named/anonymous string restrictions use exact parsed string values. Quoted XPath
literals support doubled quote escaping. `eq`/`ne` require at most one value per
operand; multiple values make assessment Invalid. `=`/`!=` test whether any pair
matches the comparison; an empty operand gives false. Equality uses codepoint
semantics, without trimming, case folding or Unicode normalization.

Existing exists/empty/count checks now accept string children, including present
empty strings. They compose with existing decimal comparisons and and/or under
the original namespace and capacity rules. Attribute expressions, nested
assertions, string ordering, Boolean/integer operands, general XPath and
configurable collation remain outside this slice. The decimal-only facade keeps
its previous grammar. False assertions publish no document or native facts.

See [bounded string assertion evidence](docs/string-assertions/README.md) for adoption, seven explicit authored admissions and qualification.
The frozen W3C fixtures/expectations are unchanged. No public API, Core source/pin,
release-controller, tag or published-package change is included.


### String assertion qualification ? 20 November 2026

The local string-assertion candidate passes 1,886 source cases (520 implementation /
1,366 public), including 271 added controls and seven explicitly recorded authored
admissions. Both package-only Core lanes pass 1,366; retained 124/23/33 callers
pass, with no public API delta (54 types / 401 rows). Nine pure-string controls
agree with published minimum Core. The unchanged frozen pilot is now 10 matches /
11 Unsupported / zero mismatch or harness errors. Counts overlap. Core pins and
published beta.1 stay unchanged; alpha.0.20 is local unsigned qualification only.
No commit/push has been performed. See [string assertion evidence](docs/string-assertions/README.md).


### String assertion delivery review ? 20 November 2026

Local code/contract review found no blocking implementation issue. The six sealed
packets, qualified source/binary identities, exact seven authored admissions and
unchanged frozen corpus verify. Other 180 rows in the three amended fixture files
are unchanged. PR title/body are prepared; explicit commit/push/PR authorization
is pending. No implementation or qualification result changed in this review.
[Review and prepared delivery](docs/string-assertions-review/README.md).


### String assertion delivery authorized ? 20 November 2026

The owner approved commit, push and open PR for the reviewed bounded slice.
The qualified implementation and sealed evidence are unchanged. Earlier pending
statements are historical; merge and publication remain separate.
[Delivery authorization](docs/string-assertions-delivery/README.md).


### PR33 lifecycle-test correction ? 20 November 2026

The initial hosted run failed two pre-existing lifecycle entry waits under
concurrent blocking-worker load. Dedicated test-only workers now run the same
barrier protocol across eight lifecycle classes; assertions/timeouts are unchanged.
All25focused lifecycle and1,886full source cases pass with unchanged named outcomes.
Product source/API and prior package/pilot/signature evidence are unchanged.
[Original failure and correction](<docs/string-assertions-ci/README.md>).


## Next bounded xsi:type contract ? 20 November 2026

PR33 is merged at b1759d22761587b2abe507d12aacf3f5b6b33535; main CI44819529110
passed 1,886 source cases and supporting controls. The next slice is a proposed
instance-type contract, not an implementation or new admission. It covers exact
type identity and current same-family direct restrictions, with explicit QName
resolution, component derivation and separate original declaration/declared type /
effective governing type. Cross-family and new complex projections are deferred.

Static review records all 51 built-in names to distinguish absent names from known
unsupported types, both pinned Core contracts, four original negative pilot cases,
33 well-formed proposed payloads and 27 scenario gates. These are planned controls,
not executed product tests or new pilot matches. Current frozen results stay
10 matches / 11 Unsupported/zero mismatches or harness errors. Next gate is actual
native feasibility on both Core pins, then separate adoption before source edits.
[Contract and qualification plan](<docs/xsi-type-contract/README.md>).


## Actual native xsi:type feasibility ? 21 November 2026

An isolated copied-source candidate passes 169 controls on each pinned Core version
(96 type-selection controls plus 73 retained original-node controls). It preserves
original declaration D/declared T and supplies effective governing G to guarded
facts, annotations and atomic evidence; selected facets are enforced and failed
requests publish no facts. Fixed node-data operations still refuse ReadTypedValue;
separate native typed-identity sessions prove that route without rebinding origins.

The unmodified source comparison has 1,882 passes and four expected xsi:type=true
Unsupported-to-Invalid differences. With exactly four scratch-only expectation
changes, all 1,886 stable source cases pass. Production tests/fixtures are unchanged.
All 18 Core signatures/source/cache/runtime identities verify; public API remains
54 types / 401 rows. These lanes overlap and are not a new product test count.

Production src/tests/tools/configuration and the sealed original proposal remain
unchanged. No new pilot/package/publication/commit/push. Original pilot remains
10 matches / 11 Unsupported / zero mismatches or harness errors. Next: record
production semantic adoption, implement the measured mapping, then full consumer,
retained-binary and frozen-pilot qualification. ADR017 review remains separate.
[Native feasibility and preserved iterations](<docs/xsi-type-contract/FEASIBILITY.md>).


## Bounded xsi:type production candidate - 21 November 2026

The two mixed owned facades now admit same-component requests and existing direct
same-family restrictions using instance-scoped QNames and an immutable compiled
base graph. Original D/T remain distinct from effective G; selected facets,
annotations and original atomic provenance agree. Failed requests publish no facts.
Legacy decimal-only refusal and the54-type/401-row public API remain unchanged.

Qualification:2,024source passes(550implementation/1,474public),1,474package-only
passes per Core1.81.2/1.81.17,30overlapping current-package native controls and
retained124/23/33passes. Four authored absent-type expectations are explicitly
reconciled. The unchanged frozen pilot now measures14matches/7Unsupported/zero
mismatches or harness errors; positive selected restrictions are tested separately.
Signed dependencies and published beta.1 baseline verify. These lanes overlap.

Production adoption is recorded; original proposal/feasibility dossiers remain
historical. ADR017 owner review and Git delivery remain pending. Published beta.1,
Core pins and release controller are unchanged. No commit/push/publication.
[Implementation qualification](<docs/xsi-type-contract/implementation/README.md>).


## PR34 merged and next specification gaps - 21 November 2026

PR34 is merged as ee32ac81c7a704c95647b121d1cde23cfa32dcbd; its tree equals the
reviewed9d48071 tree. Main CI44871576728 passes2,024source tests(550/1,474),
release controls, docs and workload integrity. Existing bounded xsi:type scope,
API and publication boundaries remain unchanged. No new package/pilot measurement.

The seven frozen Unsupported cases span four schemas. The next proposed slice
is schema namespace/form declarations: review relative namespace-name support
and admit legal attributeFormDefault without requiring application attributes.
One such gate currently masks the assert006 family's independent integer content,
complex restriction and inherited assertion gaps. Decimal complex simple-content
restriction follows; context-item/nested assertions remain separate. Analyse
original inputs and record exact adoption before implementation; no forecast pass
count or full-conformance percentage. This step changes documentation only.
[Merge receipt, ranked gaps and next contract](<docs/post-xsi-type/README.md>).


## Namespace/form development candidate - 21 November 2026

The mixed owned facades now accept bounded relative target namespace references
as exact strings, with no URI resolution/normalization/acquisition. Legal schema
attributeFormDefault no longer requires application attribute uses. Explicit
assertion-default URI literals and legacy decimal-only limits/diagnostics remain
unchanged. Actual adoption follows31native controls per signed Core pin.

Final qualification:2,143source passes(581implementation/1,562public),1,562public
package-only passes per Core1.81.2/1.81.17,31overlapping current-package native
controls and retained124/23/33. Two authored expectation admissions are recorded;
the prior2,024stable source IDs remain. API54types/401rows has zero delta, and
18Core signatures/source/runtime identities plus signed beta.1 verify. Frozen
pilot16matches/5Unsupported/zero mismatch/harness errors; assert006 remains
Unsupported at its next assertion gate. All lanes overlap; no full-XSD percentage.

One production source file changes. Local unsigned alpha.0.25 is qualification
only; no commit/push, Core pin, tag/controller or publication change. Next after
review/delivery: bounded decimal complex simple-content restriction. Original
post-xsi-type proposal remains historical; corrected schema representation section
is Structures3.17.2. [Contract, adoption and receipts](<docs/namespace-form/README.md>).


## PR35 merged; decimal complex restriction contract - 21 November 2026

PR35 merged as7ce8ada7ca78a77a13e07151b199639fa757317a, exactly preserving the
reviewed313fc1c tree. Main CI44884962349 passes2,143source tests(581/1,562),
release/docs/workload checks. Existing frozen qualification remains16matches/
5Unsupported/zero errors; no new package, controller/pin or publication change.

The next bounded contract proposes one direct attribute-free/assertion-free decimal
complex simple-content restriction, using the six existing facets. It preserves
separate G->complex B and anonymous C1->content C0 identities, including a distinct
C1 when no facets are supplied. Static inherited facet validity precedes runtime
checks; equal exclusive endpoints may define a valid empty type, whereas mixed
endpoint equality is invalid. Proposed new admitted facet/reference errors are
schema-Invalid; excluded grammar/dependencies/capacity stay Unsupported. Legacy
routes and deferred complex xsi:type substitution policy remain unchanged.

53concrete payloads,25separate gates and16facet relations are proposed artifacts,
not executed tests or conformance passes. Exact normative review pins and source/
upstream hashes are recorded. No semantic adoption or production/source/test/tool
change. Next: experimental scope + actual native copied-source feasibility on both
signed Core pins, then production adoption only after the gate passes. General
inheritance, integer assertions, full-XSD/PSVI/C45 remain separate.
[Merge evidence and contract](<docs/complex-simple-restriction/README.md>).


## Complex restriction native feasibility - 21 November 2026

An isolated copied-source candidate passes 176 controls per signed Core pin,
including all 53 proposed payloads through both facades, 48 normative facet-bound
conditions and exact native D/T/G/B/C0/C1 ownership/provenance gates. Static facet
validity, valid empty types, anonymous/sibling contexts, no-target namespaces,
limits, query rollback and fresh assessment origins are demonstrated. Data-profile
ReadTypedValue refusal and independent typed-identity evidence stay separate.

Original source comparison: 581 implementation + 1,561 public pass, with exactly
one existing refusal-reason difference. SC-058 still refuses its restriction
attribute, now as Attributes rather than DeclarationShape. With that one explicit
scratch-only overlay, all 2,143 stable cases pass. Production fixtures/source are
unchanged. API stays 54 types / 401 rows; all 18 signed Core source/runtime/archive
identities verify. These scopes overlap and are not new product coverage totals.

Production adoption and implementation remain pending. Original proposal and
sealed prior dossiers remain historical. No new package/retained-binary/pilot
execution, Core change, commit/push or publication. Current production pilot stays
16 matches / 5 Unsupported / zero errors. Next: separate production adoption,
implement the measured mapping with explicit admission review, then full product
consumer/binary/pilot qualification. [Feasibility and receipts](<docs/complex-simple-restriction/FEASIBILITY.md>).


## Decimal complex restriction production candidate - 21 November 2026

Production adoption followed176actual-native controls per signed Core pin. The
mixed owned facades now admit one direct decimal complex simple-content restriction
with attribute/assertion-free base/derived types and six existing numeric facets.
Static inherited-facet validity, G->B versus anonymous C1->C0 identities and original
D/T/G/actual-C1 provenance are enforced. No public API/Core profile expansion; legacy
and deferred complex xsi:type policies remain intact. SC-058 has one explicit
refusal-reason reconciliation, preserving schema/instance/ID and Unsupported.

Qualification:2,319source passes(651implementation/1,668public),1,668public
package-only passes per Core1.81.2/1.81.17,70overlapping current-package native
controls, retained124/23/33, API54types/401rows zero delta,18verified signed Core
archives/source/runtime payloads and signed beta.1 baseline. All2,143oldcase IDs
remain;176new source cases are70internal+106public. The unchanged frozen corpus
measures17matches/4Unsupported/zero mismatch/harness errors; onlyctD017.v becomes
a new match. Remaining assertion-family cases retain independent unsupported requirements.

Six production source files change. Local unsigned alpha.0.26 is qualification
only; published beta.1, Core pins and release controllers are unchanged. No commit/
push/tag/publication requested. Original proposal and feasibility seals remain
historical. Next: review/deliver, then assess integer complex content separately
from context-item/inherited assertions. [Implementation and exact receipts](<docs/complex-simple-restriction/IMPLEMENTATION.md>).


## PR36 merged; proposed integer simple content - 21 November 2026

PR36 merged as ab7e20afacd83edf93e71be2123fb40235c4f646, preserving reviewed
7cfe414 tree. Main CI44896328993 passes 2,319 source cases (651/1,668), release,
docs and workload controls. Frozen qualification remains 17 matches/4 Unsupported/
zero errors. The next proposal covers exact integer complex extension plus one
direct restriction, with attribute/assertion-free types and fixed fractionDigits=0.
Actual G->B and anonymous C1->C0 identities, inherited static facet validity,
valid-empty integer ranges and raw digit capacities remain distinct requirements.
SC-053 remains excluded; any refusal reason change needs measured reconciliation.
Assert006 still needs inherited/contextual assertions, so no new matches forecast.
No implementation/adoption/new engine execution, Core/API/controller or publication
change. Next: experiment-only adoption and actual signed-Core native feasibility.
[Contract and review evidence](<docs/integer-simple-content/README.md>).


## Actual native integer simple-content feasibility - 21 November 2026

An isolated three-file copied-source candidate passes 203 controls per signed Core
pin:120public-facade payloads,48normative bound comparisons and35native identity/
resource controls. Exact50digit integer atomics, G->B/C1->C0, anonymous/sibling/
namespace contexts, no-facet C1, fixed fractionDigits0 and valid-empty ranges pass.
Data-profile ReadTypedValue refusal and separate typed-identity evidence remain.
All18Core signatures/source/cache/runtime identities verify; API54types/401rows
has zero delta. Unmodified source comparison651implementation+1665public pass,
with3expected failures across SC-053(reason) and CR-042(schema admitted, original
fractional instance Invalid through both facades). With two scratch-only fixture
expectation reconciliations, all2319stable source identities pass. Original source/
fixtures unchanged; lanes overlap. No new package/retained-binary/pilot execution.
Production adoption remains pending; frozen pilot stays17matches/4Unsupported/zero
errors. Next: separate production adoption, measured implementation/reconciliations
and full consumer/binary/pilot qualification. No commit/push/publication.
[Native feasibility and preserved evidence](<docs/integer-simple-content/FEASIBILITY.md>).


## Integer simple-content production candidate - 21 November 2026

Production adoption followed203actual-native controls per signed Core pin. Three
source files now admit attribute/assertion-free integer complex extension and one
direct restriction. BigInteger values, static inherited facets/fixedfractionDigits0,
G->B versus anonymousC1->C0, original authority and existing quotas are retained.
SC-053 staysUnsupported/Attributes;CR-042 schemaValid/original4.5instanceInvalid;
explicit authored receipts preserve schema/instance/IDs and all2319stablecases.
Qualification:2522source(734implementation/1788public),1788package-only perCore,
83overlapping current-package native controls, retained124/23/33,API54types/401rows
zero delta,18signedCore identities and signedbeta.1baseline. README example passes.
Frozenpilot17matches/4Unsupported/zeroerrors with no classificationchanges;
42originalcorpusfiles unchanged. Localunsignedalpha.0.27 only;noCore/API/controller
or publishedbeta.1change. No commit/push/tag/publication requested. Originalproposal/
feasibility seals remain historical. Next: review/deliver, then contextual/inherited
assertion contract with separate exact adoption;no fullXSD/PSVI/C45/SLAacceptance.
[Implementation and exact qualification receipts](<docs/integer-simple-content/IMPLEMENTATION.md>).


## PR37 merged; contextual/inherited assertion proposal - 21 November 2026

PR37 merged as2e8d6102e10e9ccbddee87249eaefa2fdfaa4468; reviewed16694d1tree
preserved and mainCI44910612674 passes2522source(734/1788),release/docs/workload
checks. Next proposed slice:attribute-free integer simple-content assertions with
inherited base/local conjunction, dot versus typed$value, bounded sequence/range/
mod and existing original type/authority semantics. XSD's partial context normally
atomizes dot as untypedAtomic, not integer; numeric comparison/arithmetic conversions
and value-comparison type errors need separate controls.71planned payloads/25gates
are not executed tests/admissions. No new pilot/package/source/API/Core/controller
change;current frozen17matches/4Unsupported/zeroerrors. Separate namespace-uri/
nested-empty-content case remains deferred. Next:experiment-only adoption then
actual signed-Core native feasibility. No commit/push/publication requested.
[Proposed assertion contract and review evidence](<docs/contextual-assertions/README.md>).


## Actual native contextual assertion feasibility - 21 November 2026

An isolated five-overlay candidate passes248native controls per signedCore pin:
142proposal/facade+48facet+58context/ownership/grammar controls. Partial dot
untypedAtomic versus exact typed$value, inherited/static contexts, original final
G/C1provenance, invalid/no-publication, capacities and retained ownership guards
are verified. API54types/401rows zero delta;18Core signatures/source/cache/runtime
identities verify. True source comparison734implementation+1784public pass/4fail
across exactlyIC-046/IC-050. Their unchanged `. gt 0` instances fail value-comparison
type errors after schema admission. With2scratch-only expectation reconciliations,
all2522stable source identities pass. Initial nested-gate failures, probe build
errors and stale historical fixture wiring are preserved separately;SC-053 has
no product delta. Production source/fixtures remain unchanged;no new package,
retained-binary or frozen-pilot execution. Frozen17matches/4Unsupported/zeroerrors.
Next:separate production adoption, measured implementation/reconciliations and full
consumer/binary/pilot qualification. No commit/push/tag/publication requested.
[Native feasibility and preserved evidence](<docs/contextual-assertions/FEASIBILITY.md>).


## Contextual/inherited integer assertion production candidate - 21 November 2026

Production adoption followed248native controls per signedCore pin. Five source
files now admit bounded integer simple-content assertions with inherited base/local
conjunction, distinct dot untypedAtomic/typed$value, original static bindings and
finalG/C1provenance. No public API/Core/profile change. IC-046/IC-050 schemasValid,
original `. gt 0` instancesInvalid;explicit receipts preserve schema/instance/IDs.
All2522priorstablecases retained. Qualification:2770source(840/1930),1930package-only
perCore,106overlapping current-package native controls,retained124/23/33,API54types/
401rows zero delta,18signedCore identities and signedbeta.1baseline. README example
passes bothfacades and separate inherited/local negatives. Frozen21casepilot now
20matches/1Unsupported/zeroerrors:three originalassert006cases newlymatch;42corpus
files/expectations unchanged. namespace-uri structural/empty-content case remains
Unsupported. Localunsignedalpha.0.28only;publishedbeta.1/pins/controllers unchanged.
Originalproposal/feasibilityseals preserved. Next:review/deliver,then separate
structural-context/namespace-uri contract. No commit/push/tag/publication or
fullXSD/PSVI/C45/SLAacceptance requested.
[Implementation and exact qualification receipts](<docs/contextual-assertions/IMPLEMENTATION.md>).


## PR38 merged; structural assertion contract - 21 November 2026

PR38 merged as `2effee5bf9daa317599c9285ba2de5e333bc786f`; reviewed tree preserved. Main CI44944736601
passes2,770source(840/1,930) plus release/docs/workload checks. Frozen pilot remains
20matches/1Unsupported/zeroerrors; all42original files remain unchanged.
Next proposed slice: bounded namespace-uri structural assertions and genuine empty
complex content. Empty typed sequence differs from absent element-only typed value;
actual original-node fidelity on both signed Core pins is a mandatory feasibility
gate. Current provider/session assumes single atomics; public Core XML documentation
also needs reconciliation. 27proposed XML payloads/18native gates are unexecuted plans.
No semantic adoption, source/API/Core/package/controller or published-beta change.
Next: isolated native feasibility, retaining Unsupported if empty typing cannot be
represented faithfully. No commit/push/publication requested for this proposal.
[Contract and review evidence](<docs/structural-assertions/README.md>).


## Empty typed-value native gate - 21 November 2026

The first structural-assertion feasibility gate passes 29 actual-native controls
per signed Core pin (1.81.2/1.81.17); unchanged baseline passes four per pin.
Five scratch-only overlays faithfully issue present non-list empty typed sequences.
Original-node data atomizes them to empty; typed identity and scalar APIs refuse
without fabricating atomics. Element-only FOTY0012, original ownership, invalid/no
publication, export/reopen, cancellation and lifetime controls pass. All 18 Core
signatures/archive/cache/runtime identities verify; initial harness failures remain
preserved. No Core change is needed for this measured path; its public XML example
still needs separate correction. Namespace assertions, quotas, full stable-case
comparison and production adoption remain pending. Product source/API/pins/packages,
frozen pilot (20 matches/1 Unsupported/zero errors) and published beta.1 unchanged.
Next: full isolated structural namespace-assertion candidate and qualification.
No commit/push/publication requested for this step.
[Scoped native results and preserved receipts](<docs/structural-assertions/EMPTY-VALUE-FEASIBILITY.md>).


## Structural namespace assertion feasibility - 22 November 2026

The isolated six-overlay candidate passes 114 native controls per signed Core pin
(1.81.2/1.81.17), including all 27 proposal payloads through both facades, retained
empty-value controls, static/context/authority guards and exact expression/node limits.
All 2,770 unchanged source identities pass (840 implementation/1,930 public), without
expectation changes. API remains 54 types/401 rows; all 18 signed Core identities verify.
The original remaining assert_027_2 passes a direct native probe, distinct from a
frozen-pilot run. Initial named-root empty-whitespace failure and its scratch-only
plan-copy correction are retained separately; earlier evidence remains sealed.
Production source/API/pins/packages/controllers unchanged; frozen pilot stays
20 matches/1 Unsupported/zero errors. Next: separate production adoption and measured
implementation/package/retained/pilot qualification. No commit/push/publication.
[Native feasibility and exact preserved receipts](<docs/structural-assertions/FEASIBILITY.md>).


## Structural namespace assertion production candidate - 22 November 2026

Exact production adoption follows the retained native feasibility. Six source files
now implement opt-in bounded namespace-uri assertions and faithful empty complex typed
values, preserving element-only FOTY0012, original authority, scalar refusal and limits.
The established decimal-only facade retains its profile, with two new refusal controls.
All2,770priorstablecases preserved without expectation changes; source2,886(900/1,986),
package-only1,986per signedCore pin,60current-package native controls,retained124/23/33
pass. API54types/401rows zero delta;18Core identities and signedbeta.1baseline verify.
Frozen21casepilot now21matches/0Unsupported/zeroerrors; only originalassert_027_2
newlymatches. All42corpus files/expectations unchanged. This is not fullXSDcoverage.
Packaged README/example and source/package/runner payloads agree. Initial legacy
profile refusal failure and qualification-project error remain separately retained.
Localunsignedalpha.0.29only; publishedbeta.1,Corepins,controllers unchanged. Next:
review/deliver on explicitGit request,then mainCIverification and broader gap ranking.
No commit/push/tag/publication or fullXSD/PSVI/C45/SLAacceptance requested.
[Implementation and qualification receipts](<docs/structural-assertions/IMPLEMENTATION.md>).


## Expanded public coverage assessment - 22 November 2026

PR39 merged/mainCI45002890073verified at677b5628be9b22c993dddb6ff8dce1b268a282db.
A separate statically selected assessment measures129original instances/108groups,
18feature buckets across12matrix areas, on both signedCore1.81.2/1.81.17. Each pin
reports0matches/129Unsupported/zeroerrors, with identical classifications; refusal
is not positive validation coverage. Frozenpilot remains21matches/zeroUnsupported/
zeroerrors;all42files and the historical matrix remain unchanged. This is not a
fullsuite run or coverage percentage. Original metadata, selection, product/runtime
hashes, W3C notices, raw receipts and co-feature dependencies are retained separately.
Recommendation: boundedXSDstring-pattern facet contract/publicCorefeasibility first,
then simple-typed roots, derivedintegerfamilies and choice/all. SKU independently needs
both pattern and simple root; particle examples also contain implicitanyType. No
single-feature admission forecast or new semantic adoption. Product/API/Corepins/
packages/controllers unchanged. No commit/push/publication requested for assessment.
[Expanded sample, rankings and exact evidence](<docs/expanded-conformance/README.md>).


## String-pattern contract and public Core review - 22 November 2026

A new bounded pattern-facet proposal follows the preserved expanded assessment.
Exact XSD/F&O sources confirm whole-literal XSD matching, literal caret/dollar and
same-restriction pattern OR.20safe public-Core sentinels per signed1.81.2/1.81.17
agree after experimental wrapping;7rawdifferences are specified XPath semantics,
not Core defects. The caller-bounded execution gate is not met: pinned sources use
ambient regex timeout and probes observe infinity. No adversarial patterns, private
calls, machine settings or product source changes. Proposed route: Schema-owned
Unicode-scalar bounded automaton, existing String transport, deterministic work and
cancellation budgets.58XML payloads/12nativegates are plans, not Schema test passes.
Unicode property/name/digit classes, class subtraction, new chains and simple roots
remain outside the first proposed subset. SKU remains excluded; no admission forecast.
Next: isolated automaton/accounting prototype then actual-native feasibility, before
separate production adoption. No commit/push/package/publication or fullXSDacceptance.
[Contract, exact review pins and measured Core capability](<docs/string-patterns/README.md>).


## Native string-pattern feasibility - 22 November 2026

An isolated six-overlay Unicode-scalar automaton candidate passes164controls per
signedCore1.81.2/1.81.17, with37147finite-oracle comparisons within one control.
Exact syntax/state/work bounds, shared element/attribute work exhaustion, cancellation/
recovery, original emptyString/named/anonymous provenance and disposal/export pass.
The three unchanged original pattern examples pass direct native probes, not full
corpus runs. Source comparison:900implementation pass;1984public pass/2authored
admissions (M2D1-66/M2D1-80 nowValid/abc). Copied runtime expectation reconciliation
passes1986public;all2886stable identities retained,productionfixtures unchanged.
API54types/401rows zero delta;18Core signatures/source/runtime identities verify.
Initial class-escape/hyphen prototype defect and harness setup errors preserved;
finalnativecontrols pass. No product/Core/API/pin/package/controller/corpus change.
Next: separate production adoption and implementation,then full package/retained/
frozen/expanded qualification. No commit/push/publication or fullXSD/PSVIacceptance.
[Native feasibility and exact preserved receipts](<docs/string-patterns/FEASIBILITY.md>).


## Bounded string-pattern production candidate - 22 November 2026

The qualified candidate implements bounded scalar pattern automata on existing
`xs:string` placements. It preserves whole-literal matching, OR between patterns
in one restriction, String provenance, shared work limits and cancellation.
Source qualification passes 3,050 tests (948 internal and 2,102 public). All 2,886
prior test identities remain; the two authored expectation admissions are recorded
separately, with schema, instance and case IDs unchanged.

Package consumers pass 2,102 tests on each Core pin; 48 package-native controls and
retained callers (124/23/33) pass. The public API remains 54 types / 401 rows.
The frozen pilot remains 21 matches with zero errors. The expanded 129-case sample
now has 3 matches and 126 Unsupported on both pins, with zero mismatches or harness
errors. Original corpus inputs and expectations remain unchanged.

This is an unsigned local alpha.0.30 candidate. Published beta.1, Core pins and
release controllers remain unchanged. Broader regex syntax, new roots and type
chains remain outside this slice. Git delivery awaits an explicit request.
See the [implementation and qualification](docs/string-patterns/IMPLEMENTATION.md).


## PR40 merged; simple-root contract - 22 November 2026

PR40 merges as `1f2cca34ce8dc93b25f11b402bbe60330b8ee1cf`, preserving the reviewed
tree. Main CI45027251051 passes 3,050 source tests plus release, documentation and
workload controls. Pattern qualification remains sealed: frozen pilot 21 matches,
expanded sample 3 matches / 126 Unsupported, zero mismatches or harness errors.

Next proposed slice: global simple-typed roots for existing bounded decimal,
integer, boolean and string families. Exact specification and source reviews,
38 XML payloads and 17 actual-native gates are prepared, unexecuted. Preserve root
scalar/leaf separation, original declaration/governing/atomic identity, selected
xsi:type facets, string whitespace, shared limits and both compiler routes.
Original SKU remains excluded by its unsupported digit escape. No product source,
API, Core pin, package/controller or semantic-adoption change in this proposal.
[Contract and next feasibility gates](docs/simple-typed-roots/README.md).


## Native simple-root feasibility - 22 November 2026

The isolated three-overlay candidate passes 135 actual-native controls per signed
Core pin (1.81.2/1.81.17), with four unchanged-production baseline controls per pin.
Original simple root declaration/type/atomic evidence, root/leaf separation, string
whitespace, selected governing facets, mixed catalogs, budgets and lifetime pass.
Two proposal boolean restrictions remain Unsupported; built-in boolean roots pass.
All 3,050 prior source identities remain: 948 implementation pass; public comparison
2,101 pass / one authored admission (AS-050), then scratch reconciliation 2,102 pass.
Original schema/instance/ID and product fixture remain unchanged. API54/401 has zero
delta; all 18 signed Core identities verify. Product source/pins/packages/controllers
and frozen/expanded results remain unchanged. Next: separate production adoption
and complete package/retained/corpus qualification. No commit/push/publication.
[Feasibility and preserved receipts](docs/simple-typed-roots/FEASIBILITY.md).


## Bounded simple-root production candidate - 22 November 2026

Production adoption precedes the qualified simple-root implementation. Existing
bounded scalar families now work at declared global roots through both opt-in
facades, preserving original type evidence, root/leaf separation, string whitespace,
selected governing facets, compiler routes, limits and lifetime. Boolean restrictions
and new datatype/type-chain breadth remain excluded; legacy grammar is unchanged.

All 3,050 prior source identities remain, with explicit AS-050 compile/label
reconciliation and 135 new cases. Source 3,185 (1,007 internal / 2,178 public), package
consumers 2,178 per Core, 59 current-package native controls and retained 124/23/33 pass.
API 54 types / 401 rows has zero delta; 18 signed Core identities and signed beta.1 verify.
Frozen pilot stays 21 matches; expanded 129 cases stay 3 matches / 126 Unsupported, zero errors
on both pins. Eight first-refusal reason changes are recorded explicitly; all 42 frozen
and 274 expanded original files/expectations remain unchanged. Packaged example passes.
Local unsigned alpha.0.31 only; published beta.1, pins and controllers unchanged.
Next: review/deliver on explicit Git request, then merge/main verification and the
next ranked derived-integer family contract. No commit/push/tag/publication.
[Implementation and exact qualification](docs/simple-typed-roots/IMPLEMENTATION.md).


## PR41 merged; derived-integer contract - 22 November 2026

PR41 merges as `f9e9ad7c0599ba7b36b6d8d71704875990b54b98`, preserving the reviewed
tree. Main CI45039993274 passes 3,185 source tests plus release, documentation and
workload controls. Earlier simple-root qualification and all original evidence stay
sealed; frozen pilot 21 matches, expanded 3 matches / 126 Unsupported / zero errors unchanged.

Next proposed slice: all twelve built-in integer descendants, existing placements,
original type graph/provenance and inherited bounds/fixed semantics. Exact normative
ranges/parents and four spec bodies, ten Schema/eight Core source files are reviewed.
158 XML payloads and 18 native gates are unexecuted plans. Enumeration and other facet/
datatype breadth remain separate; no corpus-admission forecast. Capacity, legacy
profile, ownership and type-selection distinctions remain mandatory. No production
source/API/Core pin/package/controller or semantic-adoption change in this proposal.
[Contract and next feasibility gates](docs/derived-integer-types/README.md).


## Native derived-integer feasibility - 22 November 2026

The isolated three-overlay/59-file candidate passes 435 actual-native controls per
signed Core pin (1.81.2/1.81.17), with 24 unchanged-production baseline controls per
pin. All 158 proposed payloads run through both facades; 84 original identity gates
per pin retain builtin/named/anonymous and complex/atomic distinctions, node QNames,
BigInteger values beyond Int64, inherited bounds and selection. Descriptive type
names are recorded separately; no general typed-query or closure acceptance.

All 3,185 original source stimuli remain. Scratch reconciliation passes 1,007 internal
and 2,178 public tests after 11 logical expectations / 14 executed admissions, with one
explicit outcome-argument display rename. Production fixtures unchanged. Initial
identity-selector harness failures are preserved; candidate source needed no fix.
18 signed Core identities verify; API 54/401 zero delta. Direct original totalDigits
probe matches invalid expectation; two enumeration schemas remain Unsupported.
No full corpus/package/retained run here; product source/pins/packages/controllers
and earlier frozen/expanded measurements unchanged. Next: separate production
adoption and full qualification. No commit/push/publication.
[Native feasibility and preserved receipts](docs/derived-integer-types/FEASIBILITY.md).


## Bounded derived-integer production candidate - 22 November 2026

Production adoption precedes the qualified twelve-type builtin integer registry and
two compiler/restriction overlays. Existing placements preserve original type graph,
inherited bounds, exact BigInteger values, selected governing identity, limits and
lifetime. No public API/Core pin or legacy grammar change. Excluded facet/type-chain
breadth remains separate.

All 3,185 original stimuli remain; 11 logical expectations / 14 executed cases are
explicitly reconciled, with one expected-outcome display rename and no schema/XML/ID
change. Source 3,620 (1,126 internal / 2,494 public), package consumers 2,494 per Core,
119 current-package native controls and retained 124/23/33 pass. API 54 types / 401 rows zero
delta; 18 signed Core identities and signed beta.1 verify. Exact packaged unsignedLong
example passes. Frozen stays 21 matches / zero errors; expanded 129 cases now 8 matches /
121 Unsupported / zero errors on both pins, five new matches and no other detail changes.
All 42 frozen and 274 expanded original files/expectations unchanged. Local unsigned
alpha.0.32 only; published beta.1, pins and controllers unchanged. Next: explicit Git
delivery/review, merge/main checks, then bounded choice/all contract with UPA and
implicit-anyType co-blockers. No commit/push/tag/publication.
[Implementation and exact qualification](docs/derived-integer-types/IMPLEMENTATION.md).


## Reviewed PR42; bounded particle contract - 23 November 2026

PR42 remains open at reviewed `442f742cebf98b9c4818be51edffc760c6b85ee9`, with
CI45130016318 passing 3,620 source tests plus release/docs/workload controls. Merge
is pending explicit owner approval. This reversible contract uses that reviewed
head rather than claiming a merged-main base. Existing qualification remains sealed.

Next proposal: bounded nonempty choice first, direct all second, with 50 unexecuted
XML payloads and 18 actual-native gates. Expanded-name UPA/occurrences, physical-order
facts, root/leaf and nullable element-only typed-value distinctions are mandatory.
All six ranked original examples also require implicit anyType, still excluded.
No original admission forecast, source/API/Corepin/package/controller change or
semantic adoption. Product source remains unchanged in this contract step.
[Contract and next feasibility gates](docs/particles/README.md).


### 23 November 2026 — merged integer support; isolated choice feasibility

PR #42 merged as `aa07e801ad5f2d89ebccd7299a4f539800e20df1`; main CI `45139587221` passed 3,620 source tests. The [bounded choice experiment](docs/particles/CHOICE-FEASIBILITY.md) passes 119 controls per Core pin and all 3,620 unchanged source tests, with zero public API delta. Production inputs and published packages remain unchanged. Choice production adoption and full qualification are next; `xs:all` remains a separate deferred phase. Earlier pre-merge contract receipts remain historical evidence.


### 23 November 2026 - bounded choice production candidate

The [bounded root choice implementation](docs/particles/IMPLEMENTATION.md) is qualified: 3,739 source tests (1,191 internal / 2,548 public), 2,548 package consumers per Core pin, 65 current-package native controls and retained callers 124/23/33 pass. All 3,620 prior source names/outcomes remain unchanged; 119 controls are added. API 54/401 has zero delta. Frozen pilot stays 21 matches; expanded stays 8 matches / 121 Unsupported / zero errors, with six explicit first-refusal reason changes per pin. Eighteen signed Core identities and signed beta.1 verify. Local unsigned alpha.0.34 only; published beta, pins and controllers unchanged. NuGet advisory endpoints were unavailable; no fresh vulnerability-data assessment is claimed. Choice is bounded and opt-in; xs:all remains excluded and is the next separate phase after review/delivery and merge verification. No commit/push/publication.


### 23 November 2026 - choice merged; separate all-group contract

PR43 merged as `724d834b7e2cf1353bdacf100802ff29866c8c71`; main CI45168029488 passes all 3,739 source tests and supporting checks. Next is the [bounded root all-group proposal](docs/all-particles/README.md): 43 unexecuted XML plans, 20 actual-native gates and three unchanged original co-blocked examples. It explicitly records 18 proposed changes among 23 existing authored refusal controls, preserving payloads and IDs. This is contract review only: all semantics, production source/tests, API, Core pins, packages and release controllers are unchanged. Native feasibility precedes separate production adoption and qualification. No new commit, push or publication.


### 23 November 2026 - isolated all-group feasibility

The [bounded all-group scratch candidate](docs/all-particles/FEASIBILITY.md) passes 146 controls per signed Core pin, with 86 unchanged-production baseline refusals per pin. All 3,739 original source names/stimuli remain: the initial run has exactly 18 predeclared all-refusal failures; scratch-only expected compile reconciliation passes 1,191 internal and 2,548 public tests. Production source/tests remain unchanged. Original physical order, declaration/governing type, optional-group/member rules, FOTY0012, sum budgets and lifetime pass; API54/401zero delta and18signed Core identities verify. Three original all schemas remain Unsupported; no new full corpus/package/retained run or publication. Next is separate production adoption and full qualification, explicitly recording the18authored admissions. No commit/push.


### 23 November 2026 - bounded all-group production candidate

The [bounded all-group implementation](docs/all-particles/IMPLEMENTATION.md) is qualified: 3,885 source tests (1,251 internal / 2,634 public), 2,634 package consumers per Core pin, 60 current-package native controls and retained 124/23/33 pass. All 3,739 previous names/stimuli remain, with 18 explicit measured compile expectation changes and 146 new controls. API 54/401 zero delta; 18 signed Core identities and signed beta.1 verify. Frozen stays 21 matches; expanded stays 8 matches / 121 Unsupported / zero errors, with six explicit first-refusal changes per pin. Exact packaged all/unsignedLong examples pass; advisory checks enabled after network retry. Local unsigned alpha.0.35 only; beta/pins/controllers unchanged. Next: explicit Git delivery/review and merge/main verification, then re-rank remaining specification coverage. No commit/push/publication.


### 24 November 2026 - all groups merged; specification priorities reassessed

PR44 merged as `b08187d5f59f47a068737154ecdd6872506d3a88`; main CI45382084851 passes 3,885 source tests and supporting checks. The [post-particle coverage reassessment](docs/post-particle-coverage/README.md) reuses the qualified 23 November sample: 8 matches / 121 Unsupported / zero errors, with no new corpus run or coverage percentage. Next recommended contract is bounded integer enumeration, followed by date/time transport design and wider Unicode regex support; schema assembly and implicit anyType remain separate dependencies. Source/test/build inputs, API, Core pins and packages are unchanged. Earlier matrices, rankings and seals remain historical; no new semantic adoption, commit/push or publication.


### 24 November 2026 - integer-enumeration contract prepared

The [bounded integer-enumeration contract](docs/integer-enumeration/README.md) proposes value-space membership for existing integer families and placements, with 105 unexecuted XML cases and 18 native gates. Exact specification/source pins distinguish base-type entry validity from sibling intersections, preserve inheritance and define capacities. Static expectation impacts and a retained original-fixture Invalid-only harness assumption are recorded for measured reconciliation; upstream validities and payloads stay unchanged. No semantic adoption or product/API/Core/package change. Next: isolated native feasibility, then separate production adoption and qualification. No commit/push/publication.


### 24 November 2026 - isolated integer-enumeration feasibility

The [isolated candidate](docs/integer-enumeration/FEASIBILITY.md) passes 261 controls per signed Core pin and 210 unchanged-baseline refusals per pin. One authored attribute-only root is retained as Unsupported; IE-073Q explicitly qualifies the supported attribute placement without widening grammar. Initial source regression has 10 measured expectation failures; scratch-only expectation/upstream-validity reconciliation passes all 3,885 prior test names with no removal or rename. Two original schemas compile Valid and their instances match preserved upstream valid/invalid outcomes. API 54/401 has zero delta and 18 signed Core identities verify. Production source/tests and earlier seals remain unchanged. Next: separate production adoption and full qualification; no commit/push/publication.


### 24 November 2026 - integer-enumeration production qualification

The [bounded integer-enumeration implementation](docs/integer-enumeration/IMPLEMENTATION.md) passes 4,146 source tests (1,300 internal / 2,846 public), 2,846 package consumers per Core pin, 49 packaged native controls and retained callers 124/23/33. All 3,885 prior source names remain; 261 controls are added and ten measured expectations plus the original-fixture harness assumption are explicitly reconciled. API54/401 zero delta; 18 signed Core identities and published beta.1 verify. Frozen stays 21 matches; expanded is now 10 matches /119 Unsupported /zero errors, with exactly two original integer-enumeration admissions per pin. Integer membership is bounded to existing types/placements; original IE-073 remains Unsupported and its qualified variant is separate. Local unsigned alpha.0.37 only; no publication, pin/controller change or full XSD/PSVI claim. Next: explicit Git delivery/review and merge verification, then separate date/time transport design.


### 24 November 2026 - PR45 merged; date/time transport proposal

PR45 merged as 2eb999052188a872a54548c71c378d6a05a8cc3c; main CI45407113398 passes 4,146 source tests and supporting checks. The [calendar representation contract](docs/date-time-transport/README.md) and proposed ADR018 recommend xs:date feasibility first, with exact year/month/day and optional timezone; dateTime/time need separate exact fractional-second transport review because both pinned Core carriers store milliseconds. There are 37 unexecuted root XML plans and 18 unexecuted native gates. Nine original calendar cases retain additional obligations; no admission forecast or new corpus execution. All 323 production inputs and earlier seals remain unchanged. No semantic adoption, public API, Core pin, source/test/package change or publication. Next: isolated package-only date transport feasibility and API review before native semantic adoption.


### 24 November 2026 - date transport preflight reaches boundary stop

The [package-only date preflight](docs/date-transport-preflight/README.md) measures32 component round-trips and28 expected formatter results per Core pin; all four long.MinValue/zone combinations throw caught OverflowException. Both actual Schema facades still refuse xs:date. No native facts, detached facade, lifetime or validation success is claimed. The original full-Int64 proposal is preserved; a separate bound revision proposes excluding long.MinValue before the next isolated native experiment. All18 native gates remain pending. Eighteen unchanged Core archives/runtime payloads and two fresh Core signatures verify; all323 production inputs and59 earlier packets stay unchanged. No production/API/pin/package change, semantic adoption, commit/push or publication.


### 24 November 2026 - revised native date feasibility

The [isolated date candidate](docs/native-date-feasibility/README.md) passes 99 uniquely labelled native controls and 4,147 scratch source cases per signed Core pin (1,301 internal / 2,846 public). All 4,146 original logical cases remain, with one explicit expected-outcome display rename and one newly eligible AL-008 identity case. Nine measured semantic failures are reconciled in isolated copies; the current lane additionally adjusts its hard-coded minimum-version assertion. The revised interval excludes long.MinValue; original sealed proposal and formatter failures remain intact. API preserves 401 rows and proposes seven additions (55 types / 408 rows). Eighteen signed Core payloads and two fresh Core signatures verify. All 323 production inputs and 60 prior seals stay unchanged. Native original-node facts, both public facades, ownership, cancellation, export and lifetime are measured; full packaged/retained/corpus qualification remains. Date complex simple content and temporal facets remain excluded. No production adoption, source/API/pin/package change, commit/push or publication. Next: separate production adoption and full qualification.


### 24 November 2026 - bounded native date production candidate

The [native date implementation](docs/native-date-implementation/README.md) passes 4,246 source tests (1,308 internal / 2,938 public), 2,938 package consumers per Core pin, seven packaged native identity controls and retained124/23/33. Original4,146 logical cases remain with one explicit display rename,99 new controls and one newly eligible original AL-008 identity case; measured expectations are reconciled explicitly. API preserves401rows and adds seven (55types/408rows);18signed Core identities and published beta.1 verify. Frozen stays21matches; expanded now12matches/117Unsupported/zeroerrors: two original date admissions and one unchanged wildcard refusal with changed first reason. Exact packaged date/enumeration/all/unsignedLong examples pass; normal advisory checks stay enabled. Year interval excludes only long.MinValue within the signed representation; temporal facets and dateTime/time remain separate. Local unsigned alpha.0.40 only. Earlier61packets stay intact; no publication or Corepin/controller change. Next: explicit Git delivery/review and merge verification, then updated coverage reassessment.


### 24 November 2026 - PR46 merged; calendar gaps reassessed

PR46 merged as 383914562d89d3f7fad3a6a60cf4e29884cce912; main CI45426915018 passes 4,246 source tests. The [post-date reassessment](docs/post-date-coverage/README.md) reuses the qualified sample of 12 matches / 117 Unsupported / zero errors, with no new execution. Nine calendar cases split into two delivered date matches, two date simple-content extensions, two mixed-content cases, two enumerations and one bound. Recommended next contract is bounded date simple-content extension with accurate complex-governing/atomic-date identity and an explicit guard against decimal restriction fallback. Date equality/facets and exact-seconds Core transport remain distinct follow-ups. All 328 production inputs and 62 earlier packets remain unchanged. No semantic adoption, API/source/test/pin/package change, commit/push or publication.


### 24 November 2026 - date simple-content contract prepared

The [bounded date simple-content contract](docs/date-simple-content/README.md) defines named/anonymous roots and terminals, existing attribute uses, distinct complex G versus atomic date A, retained capacities and explicit derivation exclusions. It contains 77 unexecuted XML proposals and 18 native gates plus two unchanged original trials, without an admission forecast. Ordinary extra attributes are Invalid within this proposed date-extension closure even with zero uses; older profile behavior stays unchanged. Date complex restriction must be refused before decimal fallback. Static expectation impacts are recorded; no source/native/corpus execution or semantic adoption occurs. All 328 production inputs and 63 prior packets remain unchanged. Next: separate experimental adoption and isolated native feasibility on both signed Core pins. No API/pin/package change, commit/push or publication.


### 24 November 2026 - date simple-content isolated feasibility

The [isolated date extension candidate](docs/date-simple-content-feasibility/README.md) passes 196 native controls per signed Core pin, including all 77 authored payloads on both facades, actual G/atomic-A identities and both unchanged original upstream trials matching their validities. All 4,246 original source names pass after one measured date-extension expectation correction in scratch copies; the current lane also adjusts its known minimum-version assertion. API remains55types/408rows with zero delta;18signed Core payloads and two fresh Core signatures verify. Three isolated source overlays implement date extension, scoped ordinary attribute closure and the date-restriction fallback guard. An initial nested-operation diagnostic error was corrected without source bypass; all receipts remain. All 328 production inputs and64 earlier packets remain unchanged. No production semantic adoption, source/API/pin/package change or full corpus/package/retained run. Next: separate production adoption and full qualification. No commit/push/publication.


### 24 November 2026 - date simple-content production candidate

The [date extension implementation](docs/date-simple-content-implementation/README.md) passes 4,442 source tests (1,345 internal / 3,097 public), 3,097 package consumers per Core pin, 37 packaged identity controls and retained124/23/33. All4,246 prior source names/stimuli remain;196 controls are added, and the single measured factory refusal is explicitly reconciled. Actual complex G versus atomic date A, scoped closed attributes including zero uses and date-restriction fallback refusal are qualified. API55types/408rows has zero delta;18signed Core identities and published beta.1 verify. Frozen stays21matches; expanded now14matches/115Unsupported/zeroerrors, with exactlytwo original date-extension admissions per pin and no other row changes. Five exact packaged examples pass after a stale README exclusion was corrected without changing the DLL. Local unsigned alpha.0.43 only;65earlier packets intact. No publication/Corepin/controller change. Next: explicit Git delivery/review and merge verification, then separate date-enumeration/equality work.

## Current coverage review (25 November 2026)

Use docs/specification-coverage-current for current feature boundaries and availability. Preserve the original matrix/programme and sealed dossiers as historical evidence. Core 1.86.0 is unpublished; native calendar admission still awaits a verified signed pin and separate adoption. This documentation audit changes no native grammar or dependency pin.
