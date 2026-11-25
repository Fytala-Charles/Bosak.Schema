<div align="center">
  <img src="../assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Feature Requests">
  <h1>Bosak.Schema Feature Requests</h1>
  <p>Current public schema scope, status and acceptance boundaries.</p>
</div>
## Closed named simple-type imports - 25 November 2026

Batch slice 15 extends the explicit catalog overloads with closed imports of named simple-type libraries. Private immutable component namespace metadata preserves actual foreign type/derivation identities and same-local-name distinctions; source QName bindings remain intact. Includes/imports validate expected namespaces even on reused documents. Foreign global elements, complex owners, attributes/groups and no-namespace imports remain Unsupported. Current catalog/component/value bounds apply. All8,788prior outcomes remain unchanged. REQ-009 remains In Progress; public API remains55types/410rows and signed Core1.81.17 is unchanged. [Contract and qualification](simple-type-imports/README.md).


## Closed schema inclusion - 25 November 2026

Batch slice 14 adds explicit closed schema catalogs to both owned facades through two additive Create overloads. Matching-namespace/context includes resolve relative identifiers and deduplicate cycles/diamonds without external I/O. Original per-source prefix bindings survive private assembly; chameleon conversion, differing effective form/assertion contexts and imports remain Unsupported. Catalog8/32KiB-per-source/64KiB-total/32KiB-aggregate limits apply. All 8,554 prior outcomes remain unchanged; original instance nodes and canonical declaration identities are retained. REQ-009 remains In Progress; nine signed Core 1.81.17 payloads are unchanged. [Contract and API review](closed-includes/README.md).


## Named sequence model groups - 25 November 2026

Batch slice 13 adds closed named sequence groups at owner content roots and inside sequences, with exactly-one group occurrences. Forward/nested groups cache their original source particles; actual local declarations and anonymous types remain shared across owners. Circular model groups are Invalid under the REC; recursive element graphs through groups are separately Unsupported. Current scalar facets, attribute/assertion exclusions, namespaces and finite budgets remain. All 8,260 earlier outcomes are unchanged; 48 authored cases across both facades/three contexts and six native identity controls qualify the candidate. REQ-009 remains In Progress; API and Core 1.81.17 are unchanged. [Contract and qualification](model-groups/README.md).



## Global element references - 25 November 2026

Batch slice 12 adds closed global element reference particles with canonical global declaration identity and independent occurrence bounds. Scalar references work in admitted sequence/all/choice shapes; admitted complex containers and simple-content terminals work in sequences. Forward references and eager globals preserve source-order root selection, original nodes and values. Recursive graphs and broader metadata remain Unsupported. All 7,966 previous tests retain their outcomes; 48 cases across both facades/three contexts plus six native identity controls bring source tests to 8,260. REQ-009 remains In Progress; public API and signed Core 1.81.17 payloads remain unchanged. [Contract and qualification](global-element-references/README.md).


## Attribute-group review correction - 25 November 2026

The corrected candidate preserves the existing nested structural-assertion/application-attribute exclusion for group members. A supported namespace-uri(.) probe reproduced six unintended compile-Valid outcomes; the shared member predicate now retains Unsupported. Original candidate receipts remain sealed, and the final candidate passes7,966source/6,550reviewed package cases. No API/Core/publication change. [Corrected evidence](attribute-groups-review/README.md).


## Attribute groups with component-preserving union - 25 November 2026

Batch slice 11 adds closed acyclic named groups and actual attribute-use set union on existing owners. Repeated/shared diamond components deduplicate by identity; distinct same-name uses are Invalid. Required flags, global/group-local declarations, original facts and private compilation contexts remain intact. Cycles are Unsupported rather than Invalid because XSD1.1 permits circular groups. Nine measured old results are reconciled without input/ID changes: six valid group cases, two Invalid duplicate-use schemas and one refined missing-group refusal reason. REQ-009 remains In Progress; API and signed Core 1.81.17 payloads are unchanged. [Contract and qualification](attribute-groups/README.md).


## Global attribute declarations and references - 25 November 2026

Batch slice 10 adds closed typed globals and optional/required reference uses on existing attribute owners. Canonical global declaration handles remain shared across uses; each owner keeps its presence constraint and original node origin. Private per-schema context and lookup avoid ambient state or per-use authoritative copies. One measured old malformed ref+name/type case now returns Invalid with unchanged input. Global namespaces, scalar facets, eager unused declarations and capacities are qualified. REQ-009 remains In Progress; public API and signed Core 1.81.17 payloads are unchanged. [Contract and qualification](global-attribute-references/README.md).


## Decimal complex enumeration restriction - 25 November 2026

Batch slice 9 enables local/inherited exact enumeration on the existing attribute/assertion-free decimal complex restriction route, using qualified native base-value validity and narrowing rules. Actual governing complex and anonymous atomic components keep their bases and authority. Fourteen measured old results are reconciled without changing payloads/IDs: six base-invalid schemas are Invalid and eight valid cases compile/assess successfully. Integer/date routes and legacy decimal-only grammar remain unchanged. REQ-009 remains In Progress; API and signed Core 1.81.17 payloads are unchanged. [Contract and qualification](decimal-complex-restriction/README.md).


## Named decimal restriction chains - 24 November 2026

Batch slice 8 adds closed named decimal bases and forward references with actual DerivationBase identity, inherited exact enumeration/range/digit constraints and base-valid declaration checks. Bounds and digit facets narrow the base; same-side exclusive endpoint repetition follows the REC exception. Six measured old results are reconciled with unchanged payloads/IDs. The explicit native derivation path leaves the prior complex/legacy route separate until slice 9. REQ-009 remains In Progress; public API and signed Core 1.81.17 payloads are unchanged. [Contract and qualification](decimal-chains/README.md).


## Direct decimal enumeration - 24 November 2026

Batch slice 7 adds bounded enumeration on direct decimal restrictions in the opt-in facades. Normalized exact coefficient/scale membership preserves original instance XML and typed values, including signed zero and equivalent lexical forms beyond System.Decimal precision. Sixteen source entries and existing raw numeric/digit/scale limits apply before normalization. Legacy enumeration refusal remains; complex restrictions over enumerated decimal bases are explicitly deferred. Two measured IE-093 results are reconciled with actual Decimal reads and unchanged inputs. REQ-009 remains In Progress; API and signed Core 1.81.17 payloads are unchanged. [Contract and qualification](decimal-enumeration/README.md).


## Named Boolean restriction chains - 24 November 2026

Batch slice 6 adds closed named Boolean bases, forward references and facet-free aliases with inherited pattern conjunctions and actual DerivationBase identity. Same-step alternatives, fixed-collapse whitespace, primitive lexical validity and original Boolean/XML facts remain. Automata are compiled once per definition; inherited matching shares the assessment budget/cancellation. Six measured old refusal outcomes are reconciled without changing payloads/IDs. REQ-009 remains In Progress; API and Core 1.81.17 are unchanged. [Contract and qualification](boolean-chains/README.md).


## Direct Boolean restrictions - 24 November 2026

Batch slice 5 adds direct Boolean patterns, facet-free aliases and explicit collapse whitespace on existing scalar placements. Primitive validity and patterns precede original Boolean fact issuance. Boolean enumeration is inapplicable under XSD 1.1 and is Invalid; assertions, named Boolean base chains, fixed attribute syntax and Boolean complex content stay separate. Nine measured original outcomes are reconciled with original payloads/IDs unchanged, including actual Boolean reads in two older catalogs. REQ-009 remains In Progress; Core 1.81.17 and API are unchanged. [Contract and qualification](boolean-patterns/README.md).


## Bounded date patterns - 24 November 2026

Batch slice 4 admits the existing finite pattern grammar on direct, named and bounded complex date restrictions. Same-step alternatives and inherited conjunctions check normalized literals; original date components, timezone presence and XML stay intact. New bounds over a patterned ancestor remain Unsupported pending a separate value-space contract. String/date states and matching work share their respective schema/assessment budgets. Numeric complex patterns retain their existing refusal. REQ-009 remains In Progress; Core 1.81.17 and the public API are unchanged. [Contract and qualification](date-patterns/README.md).



## Packaged guidance reconciled after M1 - 19 November 2026

PR16package guidance is reconciled with merged M1. The0.6candidate supports bounded namespaces/form rules, QName assertions and zero-to-eight assertions; published0.5.0-beta.1 is unchanged. Earlier365/53prerelease planning and alpha.0.2example receipts remain historical. Current M1 source qualification is409(315+94), with94minimum-Core and42current-Core package cases. Release packaging remains secondary to specification coverage; no tag/controller/publication change. [Reconciliation](package-readme06-reconciled/README.md).


## M1 namespace candidate - 19 November 2026

The local M1 candidate adds namespace-aware expanded declaration names, qualified/unqualified local forms, QName assertion binding and zero-to-eight assertions within the finite decimal shape. All409source cases pass (315implementation/94public), including41authored clause-mapped cases and three real Core-session controls. Package-only94cases pass with Core1.81.2; a separate42case public lane passes with published Core1.81.17. Retained124/23/33binary catalogs pass with onlySchema.dll replaced; these lanes overlap. No Core runtime/API/source-wrapper pin, published package, version/tag or release-controller change. [Evidence](m1-namespaces/README.md).


## Shared specification coverage priority - 19 November 2026

Owner priority is specification coverage and useful external application workflows across Bosak and Bosak.Schema. The shared24-area matrix separates bounded native behavior, legacy/descriptor support, refusal and unassessed work;365historical Schema case IDs are indexed without claiming conformance. M1 proposes namespace-aware decimal documents with optional assertions, followed by coherent named-type/attribute/datatype and structural slices. No runtime or release change. [Programme](specification-coverage/PROGRAM.md), [owner requirement](REQ-009-SPECIFICATION-COVERAGE.md).


## Bounded Unsupported-reason candidate - 19 November 2026

The local development candidate adds `NativeDecimalDiagnostic.UnsupportedReason` and the fixed `NativeDecimalUnsupportedReason` enum. Existing stage codes/messages, outcomes and grammar are preserved; categories describe one detected refusal and may be Unspecified. All365source cases pass (312 implementation/53public, including20new controls), as do53package-only cases and retained124/23 unchanged-binary replacement catalogs. These lanes overlap and are not additive. The54release and six workload-integrity controls pass. MinVer minimum advances to0.6 for this additive feature;0.6.0-alpha.0.2 is local/unsigned only. Published0.5.0-beta.1 and allCore1.81.2 pins remain unchanged. No commit/push, tag or publication. [Candidate](unsupported-reasons/README.md).


## Beta.2 gap assessment - 19 November 2026

A public-example beta.2 gap assessment finds no confirmed new beta.1 defect. Namespace support alone would not admit the existing sku/orders schemas. The proposed first bounded slice is clearer Unsupported reason categories, preserving stage codes, input-free diagnostics and current grammar; API/design acceptance and a concrete application priority remain pending. Beta.1 stays current; no implementation or release change. [Assessment](beta2-gap-assessment/README.md).


# Bosak.Schema Feature Requests

24 November 2026: authorized batch slice3 adds attribute/assertion-free date complex simple-content restriction with distinct governing/atomic components and exact caller capacities. [Contract](date-complex-restriction/CONTRACT.md). Ten measured old results change without changing fixtures/IDs, including two unused-definition schemas whose string roots remain string-valued. No public API/Core/publication change.

24 November 2026: authorized batch slice2 adds named date restriction chains, closed forward references and inherited facet/timezone validation. [Contract](date-chains/CONTRACT.md). Six measured original results change; original inputs/IDs remain. Core1.81.17, public API and publication boundaries remain unchanged.

24 November 2026: authorized specification batch slice1 adds bounded date bounds/explicitTimezone on Core1.81.17. [Contract](date-bounds/CONTRACT.md); 25measured original results reconciled without changing stimuli/IDs. Full XSD1.1 and publication remain separate. Batch commit/push/PR/green-merge authorization persists for up to20slices.

24 November 2026: [bounded date enumeration production candidate](date-enumeration-implementation/README.md) adopts direct date membership with preserved timezone presence/original values on Core1.81.17. Source4,874cases, package3,509public/20native, six examples and unchanged55type/408rowAPI qualify. Expanded sample16matches/113Unsupported/zeroerrors. Retained migration passes124/22/32behavior cases; two full historical catalogs retain their explicit old-Core pin assertion failures. REQ-009 remains In Progress; no publication.

24 November 2026: [isolated date-enumeration feasibility](date-enumeration-feasibility/README.md) passes428native controls and4,442regression results after three measured expectation updates. API55types/408rows unchanged; expanded sample16matches/113Unsupported/zero errors. Original container comments are already admitted; scalar comments remain refused. Production enumeration behavior and earlier sealed evidence are unchanged.

24 November 2026: [date-enumeration/value-equality contract](date-enumeration/README.md) prepared for review on the Core 1.81.17 baseline candidate. Proposed direct date restrictions preserve timezone presence and original typed representations; no implementation/adoption or new semantic execution is claimed. REQ-009 remains In Progress.

24 November 2026: the development candidate now requires **Core 1.81.17**. New qualification uses this single baseline; earlier two-pin evidence and published beta.1 remain historical and unchanged. [Dependency adoption](core-18117-baseline/README.md).

> Current public schema scope, status and acceptance boundaries.


## 1. Registry

**Current release — 18 November 2026:** [Bosak.Schema 0.5.0-beta.1](https://www.nuget.org/packages/Bosak.Schema/0.5.0-beta.1) is published and independently verified. [GitHub prerelease](https://github.com/Fytala-Charles/Bosak.Schema/releases/tag/v0.5.0-beta.1), [scope and publication receipt](BETA1_RELEASE.md).

The published **0.5.0-beta.1** provides owned `NativeDecimalSession` and `NativeDecimalDocument` APIs with explicit capacities/context, exact decimal reads and detached XML export. ADR009/010 are accepted for this bounded Schema application scope. Qualification passed 345 source cases (312 implementation/33 public),33 package-only cases and retained 124/23 binary lanes using Core 1.81.2. These catalogs overlap. [Release, exclusions and receipt](BETA1_RELEASE.md). Full XSD 1.1, general Core C45/ADR006 and production SLA acceptance remain separate.

| Request | Status | Scope |
|---|---|---|
| Apache/public transition | Implemented and GitHub publication verified | Schema-only Apache-2.0; activation-free services; private infrastructure excluded |
| Packaged0.6consumer guidance | Reconciled after M1; no publication | [Examples and evidence](package-readme06-reconciled/README.md); earlier planning receipts retain original scope |
| Shared specification coverage | Owner-prioritized; bounded components/scalars/attributes/nesting, original-node data and string assertions merged through PR33; bounded xsi:type merged PR34; namespace/form merged PR35; decimal restriction merged PR36; integer simple content merged PR37; contextual assertions merged PR38 | [REQ-009 programme](REQ-009-SPECIFICATION-COVERAGE.md); shared CoreREQ-142;24areas with explicit evidence limits |
| Direct integer restrictions | Merged PR31; main verified | [Exact bounds/digits, named/anonymous and attribute uses](integer-restrictions/README.md); owned scalar route; no publication |
| Bounded string assertions | Merged PR33; main verified | [String equality, cardinality, namespaces and limits](string-assertions/README.md) |
| Decimal complex simple-content restriction | Merged PR36; main CI verified | [Bounded implementation and evidence](complex-simple-restriction/IMPLEMENTATION.md); no publication |
| Integer complex simple content | Merged PR37; main CI verified | [Bounded implementation and qualification](integer-simple-content/IMPLEMENTATION.md); no publication |
| Contextual/inherited integer assertions | Merged PR38; main CI verified | [Bounded implementation and qualification](contextual-assertions/IMPLEMENTATION.md); no publication |
| Bounded string pattern facets | Merged PR40; main CI qualified | [Bounded implementation and qualification](string-patterns/IMPLEMENTATION.md); no publication |
| Simple-typed global roots | Merged PR41; main CI qualified | [Bounded implementation and qualification](simple-typed-roots/IMPLEMENTATION.md); no publication |
| Derived integer built-ins | PR42 merged; main CI qualified | [Bounded implementation and qualification](derived-integer-types/IMPLEMENTATION.md); no publication |
| Native calendar values | Bounded dates merged PR46; main CI verified | [Implementation, API adoption and exact qualification](native-date-implementation/README.md); dateTime/time remain separate |
| Date simple-content extension | Production candidate qualified; uncommitted | [Implementation and exact qualification](date-simple-content-implementation/README.md); no publication |
| Bounded integer enumeration | Merged PR45; main CI qualified | [Exact integer value membership and qualification](integer-enumeration/IMPLEMENTATION.md); no publication |
| Bounded choice/all particles | Choice/all merged; main CI qualified | [Original order, UPA and occurrence gates](particles/README.md); post-particle specification reassessment |
| Expanded public XSD assessment | Measured129original cases on both Core pins; date simple-content contract recommended | [Sample and dependency-aware ranking](expanded-conformance/README.md); Unsupported is not a pass |
| Structural namespace assertions and empty content | Merged PR39; main CI verified | [Bounded implementation and qualification](structural-assertions/IMPLEMENTATION.md); no publication |
| Namespace/form declarations | Merged PR35; main verified | [Scope and evidence](namespace-form/README.md); mixed profile only |
| Bounded xsi:type | Merged PR34; main verified | [Implementation/adoption/evidence](xsi-type-contract/implementation/README.md); unchanged bounded scope |
| Native original-node atomization | Merged PR32; bounded ADR-016/API accepted | [Owned implementation](native-node-data/README.md); fixed operation, existing bounded grammar |
| [REQ-009](ADR-005-xsd11-compiled-model-and-validation.md) staged XSD 1.1 support | In Progress | Compiled descriptors, assertion/facet evaluation, alternative applicability/filtering and selected metadata |
| [Core 1.81.2 dependency adoption](CORE-1.81.2-ADOPTION.md) | Merged | Four product references and one test provider reference updated; no C# adaptation |
| Core optional source integration | Refreshed in Core PR #503 | Merged Schema9fd4e6a pinned with58 source/test/fixture/legal inputs;260 implementation/legacy plus nine public current-Core cases. Original135-case receipt remains historical. |
| Core schema-session integration | Internal native scalar adapter delivered; overall acceptance pending | Bounded original facts/scalar flow and host lifetime controls; broader component/compatibility/public consumer qualification pending |
| NuGet trusted publishing | Beta.1 publication verified | Manual prepare/publish separation;54 release controls; beta 345/33 and retained 124/23 qualification; hosted beta delivered; post-publication registry consumer verified |
| Bounded Unsupported reasons | Local implementation/qualification; API/release review pending | [Candidate](unsupported-reasons/README.md);365source/53package-only, unchanged stage codes and grammar |
| Beta.2 gap assessment | Public examples selected; diagnostic-first candidate implemented locally | [Public-example assessment](beta2-gap-assessment/README.md); bounded Unsupported reasons proposed before broader grammar |
| Representative invoice consumer | Passed using published beta; production acceptance excluded | [Trial](BETA1_INVOICE_TRIAL.md); no observed engine defect or beta.2 expansion justified |
| Complete XSD 1.1 structural validation | Incomplete | No complete current implementation or blanket conformance claim |
| Native decimal structural backend | Implemented internally; 76 cases pass | Complete finite grammar only; [evidence](native-decimal-backend/2026-11-15/README.md) |
| Native decimal Core adapter | Implemented internally; 28 cases pass | Issuance, scalar flow, guarded leases, expiry and cleanup; [latest evidence](native-session-facade/2026-11-15/README.md) |
| Owned native decimal entry point | Published in0.5.0-beta.1;33 current external consumer cases pass | [ADR-010](ADR-010-owned-native-decimal-facade.md), [proposal](NATIVE-DECIMAL-PUBLIC-API-PROPOSAL.md); development minimum0.5; bounded ADR009/010 accepted; beta published with explicit exclusions |

## 2. Implemented Coverage

The public tests cover XSD version selection, schema components, explicit resolution/security, assertion evaluation, simple-type assertion facets, alternative compilation/applicability, open-content/default-attribute descriptor handling, open-content wildcard negative constraints (notNamespace/notQName) and the existing typed-XSLT integration. Support is bounded by those implementations and diagnostics; recorded metadata is not equivalent to structural enforcement.

## 3. Acceptance and Exclusions

Historical 15 November follow-up after adapter commit `939ba9a`: eight additional adapter controls plus eight facade cases bring the suite to 243. Default lookup names now reject with ArgumentException instead of NullReferenceException. The owned prototype cleans unsuccessful admissions and returns detached values/XML; the [public proposal](NATIVE-DECIMAL-PUBLIC-API-PROPOSAL.md) is reviewable but unimplemented publicly. Release build with network-enabled NuGet audit and the official PowerShell 7 documentation check pass. REQ-009/global acceptance remains In Progress; no release/tag or historical Core-lane requalification.

Latest 15 November checkpoint: backend/profile work committed as `aba6967`; the next internal Core adapter step delivers 20 cases and 227 total standalone passes. [Adapter qualification map](NATIVE-SESSION-ADAPTER.md) distinguishes executed controls from remaining group acceptance, external consumer/API review and broader coverage. No P01–P12/global sign-off, version/tag/release or historical Core-lane requalification is implied. REQ-009 remains In Progress on the unchanged 0.4 development line.

Native backend delivery on 15 November: [grammar](NATIVE-DECIMAL-GRAMMAR.md) finalized, internal structural/assertion assessor implemented, 76 backend cases and 207 total standalone cases pass. Backend controls cover bounded portions of P01/P02/P05/P08 plus independent snapshot/concurrency observations; engine-root atomization, full adapter identity/environment/lifetime and overall P01–P12 acceptance remain pending. REQ-009 stays In Progress. Schema's 0.4 development line and all published Core pins remain unchanged; no tag or release is implied.

Earlier scalar-only checkpoint on 15 November: the [published-package scalar probe](native-session-probe/2026-11-15/README.md) passed seven cases, with 131 standalone cases then (124 retained plus seven). The [specification adoption record](NATIVE-DECIMAL-SPECIFICATION-ADOPTION.md) pinned six downloaded W3C publication bodies. Subsequent backend delivery above supersedes its implementation status and standalone count; its receipts remain historical. REQ-009 stays In Progress; no release/version change or general provider acceptance.

Historical profile-definition checkpoint: the [first native decimal session profile](NATIVE-SESSION-PROFILE.md) defines supported/refused coverage, a released-contract map and twelve paired planned acceptance groups. [ADR-009](ADR-009-native-decimal-session-profile.md) is Proposed. This documentation checkpoint retains the Schema 0.4 development line; the implementation release version is undecided. REQ-009 remains In Progress. No runtime functionality or provider acceptance is claimed.

The Apache transition passed 114 public schema tests with zero failures/skips. Commercial credential, payments, delivery and key-generation requests remain in the private licensing repository; they are not public schema dependencies or open-source engine features.

The transition does not implement the proposed core session API, change frozen W3C gates, admit skipped tests, certify XmlDb/Braid adoption or release a package. Future semantic additions require immutable specification adoption evidence and happy-path/edge regression tests.

The 3 November [O1/O2 versioned responses](core-disposition-responses/2026-11-03-v2/README.md) are scoped historical design acceptance, not proof of current native adapter/runtime delivery. Their original source/proposal hashes and the unsigned [confirmation drafts](core-disposition-inbox/2026-11-03/README.md) remain unchanged. The published Core 1.81.2 release and its budget deferral do not complete Schema's native provider/session or performance requirements. See [history](DEVELOPMENT_HISTORY.md).

*Last updated: 2026-11-25*


## M2A scope proposal - 19 November 2026

M1 is merged (Schema PR17/Core PR504), with packaged guidance reconciled in Schema PR16. The next [bounded named-complex-type proposal](m2a-named-types/PLAN.md) defines QName references, component reuse, actual Core identity/atomization and explicit refusal controls. Planning only; no runtime/API/pin/publication change or additional passed-test claim. REQ-009 remains In Progress.


## M2A implementation candidate - 19 November 2026

The [named-complex-type slice](m2a-named-types/README.md) is locally implemented and qualified:465source cases(320implementation/145public),145minimum-Core package cases,52current-Core cases and retained124/23/33binary catalogs pass; scopes overlap. Up to16global types and16root declarations, full expanded-name lookup, eager unused-type checks, shared type identity and original Core facts are supported within the decimal-only profile. No Core engine/API/pin or published package/controller change. REQ-009 remains In Progress; broader M2 datatypes/facets/attributes and official W3C qualification remain pending.


## M2B decimal restriction proposal - 19 November 2026

M2A merged in PR18 at6861c04. The next [named-decimal restriction proposal](m2b-decimal-restrictions/PLAN.md) defines direct xs:decimal simple types, selected range/digit facets, actual Core provenance and distinct capacity/Invalid/refusal gates. Planning only: no new runtime/API, semantic adoption, passed-test count or publication. Broader datatypes and attributes follow in separate slices; REQ-009 remains In Progress.


## M2B implementation candidate - 19 November 2026

The [adopted decimal-restriction slice](m2b-decimal-restrictions/README.md) implements named simple types directly based on xs:decimal and six selected range/digit facets, with named original Core provenance.534source cases(324implementation/210public)pass; source/package/retained qualification is recorded in the dossier with overlapping lanes kept separate. Combined16global type ceiling; explicit facet integer capacity256. Named nondecimal types, restriction chains, other facets and attributes/imports remain excluded. No Core engine/API/pin or release/publication change; REQ-009 remains In Progress.


## M2C datatype and owned-value proposal - 19 November 2026

M2B merged in PR19 atfbb4c13. The [next datatype proposal](m2c-builtin-datatypes/PLAN.md) stages strings with an additive owned scalar facade, then Boolean/integer, followed by selected string facets and attributes. Existing DecimalCount/ReadDecimal semantics stay bounded to decimals. Planning only: no new API, datatype adoption, runtime, passed-test count or release change. Exact surface/contract and published-Core feasibility proof are required before implementation. REQ-009 remains In Progress.


## M2C1 concrete surface and Core feasibility - 19 November 2026

The [API review package](m2c-builtin-datatypes/api-review/README.md) and [proposed ADR011](ADR-011-owned-native-scalar-facade.md) define an additive ten-type string/decimal facade without changing old decimal semantics.14synthetic public transport cases pass separately on signed Core1.81.2 and1.81.17, with18package signatures/source/payload identities verified. No new Schema validation or API implementation/acceptance, engine/pin or publication change. Scalar provenance is snapshot-bound; complete-container views are operation-local. Actual mixed Schema node/facade qualification and exact string semantic adoption remain next; REQ-009 is In Progress.


## M2C1 string/decimal implementation candidate - 19 November 2026

The [adopted string/decimal facade](m2c-string-facade/README.md) adds the ten reviewed public types under explicit opt-in and shares the existing owner/gate/cleanup path.614source cases(334implementation/280public)pass, including47authored string cases,23public contract controls,6mixed lifecycle and4actual Core/writer controls. Primitive string text is preserved with explicit UTF16capacity; typed reads retain original facts internally and bounded export preserves parsed CR references on reopen. NativeDecimalSession remains decimal-only. No Boolean/integer/facet/attribute/import expansion, Core engine/pin or release/publication; REQ-009 and overall API/C45 acceptance remain In Progress.


## M2C2 Boolean implementation candidate - 19 November 2026

M2C1 merged in PR20 at16355ea; fresh main CI passes614source cases and54release controls. The [bounded Boolean candidate](m2c-boolean/README.md) adds Boolean=3 and a checked BooleanValue getter to the opt-in facade, actual Boolean provenance/atomization and a fixed256code-unit lexical ceiling before XML-whitespace collapse.671source cases(340implementation/331public)pass locally, with intentional prior-refusal deltas recorded separately. Legacy decimal facade unchanged; Boolean assertions/restrictions and integer support remain excluded. No Core engine/pin or release/publication change; REQ-009 and overall API/C45 acceptance remain In Progress.

## M2C3 exact integer implementation candidate - 19 November 2026

M2C2 merged in PR21 at20cc520; fresh main CI44602002427 passed671source cases and54release controls. The [bounded integer candidate](m2c-integer/README.md) admits built-in xs:integer (a decimal restriction, not a primitive datatype), checked detached BigInteger values and original integer provenance/atomization.748source cases(347implementation/401public)pass locally. Raw lexical ceiling256UTF16code units; MaxNativeCoefficientDigits includes supplied leading zeroes, excludes sign and is enforced before normalization. Existing separate query digit limits can refuse a read without poisoning export/smaller reads. Legacy decimal facade remains decimal-only. Integer assertions, derived families and named integer restrictions/facets remain excluded. Local qualification is not a new release, official W3C-suite conformance or general API/C45 acceptance; no Core engine/pin/controller/publication change. REQ-009 remains In Progress.

## M2D1 named string restriction candidate - 19 November 2026

M2C3 merged in PR22 at014cb28; fresh main CI44609515923 passed748source cases,54release controls and6workload controls. The [named string candidate](m2d-string-restrictions/README.md) adds opt-in direct xs:string restrictions with length/minLength/maxLength and enumeration.837source cases(355implementation/482public)pass locally. Length counts Unicode scalar values; UTF16leaf/enumeration representation limits stay distinct. Exact ordinal enumeration preserves parsed whitespace and Unicode sequence, including XML-normalized facet attributes; duplicate entries are admitted under a16entry ceiling. Named original typed facts/provenance remain; no public API member change. Legacy decimal facade, decimal sibling constraints and assertions remain unchanged. Pattern/regex, whitespace overrides, restriction chains and string assertions remain excluded. REQ-009 stays In Progress; no Core engine/pin or release/controller/publication change or general API/C45 acceptance.

## M2E1 root attribute contract proposal - 19 November 2026

M2D1 merged in PR23 at098fdaf; fresh main CI44614488088 passed837source cases,54release controls and6workload controls. The [root attribute contract](m2e-attributes/CONTRACT.md) and [ADR012](ADR-012-owned-root-attribute-reads.md) propose required/optional local root attributes with separate AttributeCount/ReadAttribute and detached namespace/local-name/scalar entries, preserving leaf indexes.27public-only synthetic Core probes pass on each signed1.81.2/1.81.17 lane, including original attribute facts/provenance and foreign/lifetime refusals. Those overlapping feasibility controls are not new Schema cases or attribute validation. Product source/API/pins and delivered837suite remain unchanged. New API/semantic adoption, actual native validation/atomization and consumer/retained qualification remain required; general API/C45 acceptance and registry publication are not inferred. REQ-009 stays In Progress.

## M2E1 root attribute implementation candidate - 19 November 2026

The [adopted root attribute slice](m2e-root-attributes/README.md) implements the concrete ADR012 proposal locally on098fdaf.950source cases(372implementation/578public)pass, including all837prior outcomes unchanged. Only opt-in local required/optional root attributes are admitted; separate AttributeCount/ReadAttribute and detached NativeAttributeValue name/scalar entries preserve existing leaf indexes. Declaration-order indexing skips absent optionals, distinguishes false/zero/empty, respects namespace forms/XML normalization and uses original native typed facts/provenance. Scalar representation/query/cache and ownership/cancellation/cleanup/export rules remain. Legacy decimal facade, no-attribute root refusal and leaf-attribute exclusion stay unchanged. Defaults/fixed/global refs/groups/wildcards/attribute assertions remain excluded. Local qualification is not hosted CI/new publication or general API/C45 acceptance; no Core engine/pin/controller change. REQ-009 remains In Progress.

## Decimal simple-content root candidate - 20 November 2026

PR24 merged atd4eca99; main CI44689935935 passed950source/54release/6workload controls. The [adopted decimal simple-content slice](decimal-simple-content/README.md) locally adds HasRootScalar/ReadRootScalar, preserving leaf/attribute indexes.1036source cases(386implementation/650public)pass with all950prior outcomes unchanged. Direct xs:decimal/admitted named decimal bases and current root attributes support the isolated unchanged PriceType; the full orders fixture still refuses its other unsupported definitions. Complex root annotation/governing provenance stays distinct from actual decimal atomic type; complex status is independent of no-typed-value, and root string-value uses schema whitespace collapse. Existing element-only FOTY0012, original facts, capacity/cache/gate/disposal and detached export semantics remain. Restriction/complex-base chains, other base families, nested/mixed content and simple-content assertions remain excluded. REQ-009 stays In Progress; no Core engine/pin, new release or general API/C45 acceptance.


## Bounded nested-sequence proposal - 20 November 2026

PR25 merged at c34e9fe; fresh main CI44695518279 passed 1,036 source cases,
54 release controls and six workload controls. The [nested-sequence proposal](nested-sequences/PLAN.md)
defines acyclic named/anonymous element-only nesting, finite occurrences and an
internal terminal-scalar projection preserving existing flat indexes. Proposed
limits are depth 4, 64 expanded element nodes and 16 terminal scalar occurrences
per root. Root attributes remain separate; nested attributes/simple content,
choice/all, recursion, unbounded repetition and nested-profile assertions remain
excluded. Planning only: no new implementation, API, semantic adoption, passed
feature count, Core pin or publication. REQ-009 remains In Progress.


## Nested transport feasibility and semantic adoption - 20 November 2026

The [nested dossier](nested-sequences/FEASIBILITY.md) records 50 public-only synthetic controls per signed
Core1.81.2/1.81.17 lane (13 new, 37 inherited), 18 package signature/source/payload
checks and an actual pre-implementation Schema refusal. These are transport and
baseline controls, not new product cases or nested validation. A separate exact
Structures/XDM adoption now precedes production changes. All production files,
the 1,036-case source catalog, public API and Core pins remain unchanged. Actual
nested assessment and terminal-only projection implementation are next; REQ-009
remains In Progress. No commit/push or publication.


## Nested-sequence implementation candidate - 20 November 2026

The [adopted nested-sequence candidate](nested-sequences/README.md) locally implements acyclic named/
anonymous element-only nesting, finite occurrence rules and separate terminal
scalar reads. Existing root attributes/scalar datatypes are reused; complete
original element facts include containers, while ScalarCount/ReadScalar exclude
them. All 1,036 prior source outcomes remain unchanged. The full local suite is
1,125 cases (395 implementation/730 public), including 76 authored nested cases,
four public operation controls and nine actual provider/lifecycle controls.
Depth 4, 16 terminal scalars, defensive 64 element nodes and 16 local anonymous
types are explicit profile ceilings. Nested attributes/simple content, choice/all,
recursion, unbounded repetition and assertions in nested schemas remain excluded.
No new public API, Core pin or published package/controller change; REQ-009 and
general API/C45 acceptance remain In Progress. Package/retained/API evidence is
recorded separately in the candidate dossier, not inferred from source tests.


## Nested container attribute proposal - 20 November 2026

PR26 merged at3e067cd; fresh main CI44706432874 passed1,125source cases,
54release controls and six workload controls. The [nested-attribute proposal](nested-attributes/PLAN.md)
and proposed ADR014 define separate descendant reads plus immutable owner-element
paths, preserving existing root/terminal indexes. Reuse current local attribute
semantics on nested element-only complex containers, with64potential attribute
uses including root/optional expansions. Planning only: no runtime/API, semantic
adoption or new passed product case. Nested simple content/leaf attributes remain
excluded. REQ-009 and general API/C45 acceptance remain In Progress; published
beta.1, Core pins and release controller are unchanged.


## Nested attribute feasibility and before-code adoption - 20 November 2026

The [nested attribute evidence](nested-attributes/FEASIBILITY.md) records69public-only controls per signed
Core1.81.2/1.81.17lane (19new/50inherited),18signature/source/payload identities
and four actual pre-implementation Schema/API controls. Exact node/fact binding
rejects authentic same-authority facts swapped across owners. Path prototypes
are detached locators, not facade implementation or authority. Separate dated
Structures/Datatypes/XML/XDM semantic adoption now precedes production edits.
Product source/API and1,125source catalog remain unchanged; broader API/C45,
official suite and publication acceptance remain separate. REQ-009 is In Progress.


## Owner-aware nested attribute candidate - 20 November 2026

The [adopted nested-attribute candidate](nested-attributes/README.md) implements separate descendant
reads and detached immutable owner-element paths with unchanged root/terminal
indexes. Source1,200cases(404implementation/796public)pass:1,123prior outcomes
unchanged, exactly NS-049/NS-075former refusals deliberately admitted,75new cases.
Original owner/path/index/name checks precede fact publication; aggregate64uses
include root/optional/repeated branches, with16perowner and existing nested limits.
Three additive members/no new public type; nested simple content and scalar-leaf
attributes remain excluded. Package/API/retained proof is separate from source
counts. REQ-009 remains In Progress; no Core pin or publication/controller change.


## Nested decimal simple-content proposal - 20 November 2026

PR27 merged at32f8214; fresh main CI44726412611 passed1,200source cases,
54release/sixworkload controls and documentation hygiene. The [next proposal](nested-simple-content/PLAN.md)
and proposedADR015 define complex decimal simple-content terminals in existing
ReadScalar order, original complex/atomic provenance and depth4three-segment
attribute-owner paths, with unchanged root reads/budgets and zero public API delta.
Planning only: no production change, semantic adoption or new passed feature case.
Potential NS-062/NA-046compile admissions need explicit before-code baselines and
instance outcomes. REQ-009/generalAPI/C45 remain In Progress; publishedbeta.1 and
Core pins/controller unchanged.


## Nested decimal simple-content implementation - 20 November2026

The [adopted nested decimal candidate](nested-simple-content/README.md) implements direct decimal/named-decimal
complex scalar-bearing terminals and existing owner-aware attributes.1,255source
cases(415implementation/840public)pass;1,198prior unchanged, exact NS-062/NA-046
compile admissions now have Invalid supplied instances,55new controls. Root reads
and existing valid indexes remain separate. Complex/atomic provenance, normalized
node text versus lexical export and depth4three-segment owners are explicit;
original budgets and zero public API delta remain. Signed Core feasibility and
adoption precede implementation; compatibility/retained/API evidence is separate.
No Core pin/controller/publication change; REQ-009/generalAPI/C45 stay In Progress.

## Upstream conformance pilot — 20 November 2026

20 November 2026: the unchanged PR28 engine was measured against a frozen,
clause-mapped upstream XSD 1.1 pilot (21 cases / 15 groups / 42 original files).
All 21 are Unsupported: 16 during compilation and five during assessment;
zero Valid/Invalid comparisons means no positive conformance claim. Source
regression remains 1,255 passing cases (415 implementation / 840 public), with
10 separate harness integrity controls. No engine/API/Core pin or release change.
Next: adopt a scoped schema-location hint policy under an explicitly supplied
schema, keeping xsi:type separate, then assess anonymous decimal/string restrictions.
[Pilot and gap evidence](conformance-pilot/README.md). REQ-009 remains In Progress.

## Explicit-schema location hints — 20 November 2026

The opt-in NativeSchemaSession preserves exact xsi location-hint attributes
without fetching them or changing the explicitly supplied schema. Legacy decimal
refusals and xsi:type/nil refusal remain unchanged. Original parsed hints are
exported but do not enter typed application-attribute indexes; built-in hint PSVI
is outside this bounded policy. All 1,255 previous source outcomes are unchanged;
47 new public cases give 1,302 passing tests (415 implementation / 887 public).
The unchanged upstream pilot advances to one validity match / zero mismatches /
20 Unsupported / zero errors; four invalid xsi:type cases remain Unsupported.
One case is not whole-suite conformance. No API/Core pin or publication change.
[Adoption and qualification](schema-location-hints/README.md). REQ-009 remains In Progress.

## Anonymous restriction adoption ? 20 November 2026

PR29 merged at1438f719863d0c13f976f1b0eb683e146a3511e5 with the exact reviewed tree.
Main CI44752564556 passes1,302source cases(415implementation/887public), release
refusal controls, six workload controls and documentation hygiene. The next
anonymous local decimal/string restriction adoption is recorded before production
edits. Anonymous identity/provenance transport requires feasibility qualification;
no implementation or new upstream matches yet. NS-051 and six original upstream
eligibility targets are pinned, preserving integer and other profile exclusions.
[Adoption and qualification plan](anonymous-simple-restrictions/README.md). REQ-009 remains In Progress.

## Anonymous restriction feasibility — 20 November 2026

Before-code public Core feasibility passes90controls per separately restored
signed1.81.2/1.81.17lane(13newanonymous/77inherited), with18package signatures
and source/archive/cache/runtime identities verified. Original anonymous simple
type handles survive node atomization/direct reads, repeated/distinct identity,
foreign refusal and expiry. Six separate actual Schema baseline controls pass;
anonymous types still refuse. No production/API/pin/package or upstream outcome
change. Native implementation and source/package/retained qualification remain.
[Before-code evidence](anonymous-simple-restrictions/FEASIBILITY.md). REQ-009 remains In Progress.

## Anonymous restriction implementation — 20 November 2026

Anonymous local decimal/string restrictions are locally implemented under the
original adoption: shared facets, original unnamed identities and combined16local
definition budget. Source1,373passes(421implementation/952public),1,301prior
unaffected/NS-051adopted admission/71new. Package-only952minimum/952current and
retained124/23/33pass with exact signature/payload checks. Frozenpilot now7matches,
14Unsupported,0mismatches/errors; six targeted cases advance unchanged. Public
API52types/392rows unchanged. Owned scalar reads are qualified; native general
node queries remain refused and legacy null-QName data(.) is unqualified.
No Corepin, published beta.1, tag/controller or generalC45/full-XSD acceptance change.
[Implementation and qualification](anonymous-simple-restrictions/implementation/README.md). REQ-009 remains In Progress.

## Specification priorities after PR30 ? 20 November 2026

PR30 is merged at5bfbb1e798cdf18bac6ac7bb15705b1a0e50bc65 with the exact reviewed
a637a5d tree. The remaining frozen14Unsupported cases comprise10schema compile
refusals and4instance refusals; the pilot stays7validitymatches/0mismatches/errors.
Next recommendation is bounded direct integer restrictions, followed by separately
contracted typed-node integration and other ranked gaps. The integer contract is
proposed only: no adoption, engine change or new outcomes. GeneralCoreC45/query
acceptance remains separate; published beta.1 and all Core pins remain unchanged.
[Ranked gaps and next contract](specification-next/README.md). REQ-009 remains In Progress.

## Integer restriction implementation — 20 November 2026

Bounded direct named/anonymous xs:integer restrictions are locally qualified:
exact BigInteger bounds/totalDigits/fixedfractionDigits0, including named root
and descendant attribute uses. Source1,482passes(427implementation/1,055public),
1,371prior unaffected/twoadopted admissions/109new. M2B-54legacyrow stays unchanged;
its separate opt-in payload becomes Invalid. Package-only1,055minimum/1,055current
and retained124/23/33pass; all18Core signatures/payloads and API52types/392rows
verify. Frozenpilot9matches/12Unsupported/0mismatches/errors, exactlytwointeger
validity advances with unchanged fixtures. No Corepin, publicAPI, query-profile,
publishedbeta.1/tag/controller or generalC45/fullXSDacceptance change.
[Adoption and qualification](integer-restrictions/README.md). REQ-009 remains In Progress.

## Original-node data contract proposal ? 20 November 2026

PR31 is merged at43f14dbf56c36e5185e705af901678c42c8d1aff with the exact reviewed
21939a1tree. Main CI44774979540 passes1,482source(427implementation/1,055public),
release controls, sixworkload and documentation hygiene. The next proposed API
is a separate owned original-node data session, not arbitrary XPath. It binds
released Core's exact original-node-data profile before native assessment,
resolves original element/attribute targets, and returns detached scalars. Old
scalar guards/episodes remain unchanged; feasibility/adoption/API review pending.
Frozenpilot9matches/12Unsupported unchanged. No production/API/Corepin or
published beta.1/tag/controller change, and no new generalC45/PSVI acceptance.
[Contract, proposed API and qualification](typed-node-contract/README.md). REQ-009 remains In Progress.

## Actual native node-data feasibility — 20 November 2026

Actual native original-node data feasibility passes73controls per signed
Core1.81.2/1.81.17lane and eight before-code controls. The isolated two-file
candidate preserves all1,482existing named test outcomes on minimum Core and
API52types/392rows;18Core signatures/payload identities verify. Native nodes need
the guarded original-fact interface in addition to explicit provider mode/context.
Core FOTY0012 is InvalidOperationException; proposed facade mapping/adoption/API
review remain pending. Production source/guards/API/Corepins and frozen9/12pilot
are unchanged; this is native transport feasibility, not new XSD conformance.
[Native evidence and remaining decisions](typed-node-contract/FEASIBILITY.md). REQ-009 remains In Progress.


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
and the existing decimal/scalar facades are unchanged. See [implementation adoption](native-node-data/ADOPTION.md).


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
still pending. Qualification and sealed packets are unchanged. [Review and prepared delivery](native-node-data-review/README.md).


## Bounded ADR-016/API accepted ? 20 November 2026

The owner accepted the reviewed two-type/seven-method owned original-node data
contract and authorized commit, push and PR delivery. This dated acceptance
supersedes historical pending-owner-acceptance wording in the sealed ADR/proposal,
feasibility, implementation and review packets; those originals remain unchanged.
All qualified source/binary identities remain unchanged. General C45/ADR006,
full-XSD/general XPath and release/publication decisions remain separate.
[Owner acceptance](<native-node-data-acceptance/README.md>).


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

See [bounded string assertion evidence](string-assertions/README.md) for adoption, seven explicit authored admissions and qualification.
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
No commit/push has been performed. See [string assertion evidence](string-assertions/README.md).


### String assertion delivery review ? 20 November 2026

Local code/contract review found no blocking implementation issue. The six sealed
packets, qualified source/binary identities, exact seven authored admissions and
unchanged frozen corpus verify. Other 180 rows in the three amended fixture files
are unchanged. PR title/body are prepared; explicit commit/push/PR authorization
is pending. No implementation or qualification result changed in this review.
[Review and prepared delivery](string-assertions-review/README.md).


### String assertion delivery authorized ? 20 November 2026

The owner approved commit, push and open PR for the reviewed bounded slice.
The qualified implementation and sealed evidence are unchanged. Earlier pending
statements are historical; merge and publication remain separate.
[Delivery authorization](string-assertions-delivery/README.md).


### PR33 lifecycle-test correction ? 20 November 2026

The initial hosted run failed two pre-existing lifecycle entry waits under
concurrent blocking-worker load. Dedicated test-only workers now run the same
barrier protocol across eight lifecycle classes; assertions/timeouts are unchanged.
All25focused lifecycle and1,886full source cases pass with unchanged named outcomes.
Product source/API and prior package/pilot/signature evidence are unchanged.
[Original failure and correction](<string-assertions-ci/README.md>).


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
[Contract and qualification plan](<xsi-type-contract/README.md>).


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
[Native feasibility and preserved iterations](<xsi-type-contract/FEASIBILITY.md>).


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
[Implementation qualification](<xsi-type-contract/implementation/README.md>).


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
[Merge receipt, ranked gaps and next contract](<post-xsi-type/README.md>).


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
is Structures3.17.2. [Contract, adoption and receipts](<namespace-form/README.md>).


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
[Merge evidence and contract](<complex-simple-restriction/README.md>).


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
consumer/binary/pilot qualification. [Feasibility and receipts](<complex-simple-restriction/FEASIBILITY.md>).


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
from context-item/inherited assertions. [Implementation and exact receipts](<complex-simple-restriction/IMPLEMENTATION.md>).


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
[Contract and review evidence](<integer-simple-content/README.md>).


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
[Native feasibility and preserved evidence](<integer-simple-content/FEASIBILITY.md>).


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
[Implementation and exact qualification receipts](<integer-simple-content/IMPLEMENTATION.md>).


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
[Proposed assertion contract and review evidence](<contextual-assertions/README.md>).


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
[Native feasibility and preserved evidence](<contextual-assertions/FEASIBILITY.md>).


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
[Implementation and exact qualification receipts](<contextual-assertions/IMPLEMENTATION.md>).


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
[Contract and review evidence](<structural-assertions/README.md>).


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
[Scoped native results and preserved receipts](<structural-assertions/EMPTY-VALUE-FEASIBILITY.md>).


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
[Native feasibility and exact preserved receipts](<structural-assertions/FEASIBILITY.md>).


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
[Implementation and qualification receipts](<structural-assertions/IMPLEMENTATION.md>).


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
[Expanded sample, rankings and exact evidence](<expanded-conformance/README.md>).


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
[Contract, exact review pins and measured Core capability](<string-patterns/README.md>).


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
[Native feasibility and exact preserved receipts](<string-patterns/FEASIBILITY.md>).


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
See the [implementation and qualification](string-patterns/IMPLEMENTATION.md).


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
[Contract and next feasibility gates](simple-typed-roots/README.md).


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
[Feasibility and preserved receipts](simple-typed-roots/FEASIBILITY.md).


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
[Implementation and exact qualification](simple-typed-roots/IMPLEMENTATION.md).


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
[Contract and next feasibility gates](derived-integer-types/README.md).


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
[Native feasibility and preserved receipts](derived-integer-types/FEASIBILITY.md).


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
[Implementation and exact qualification](derived-integer-types/IMPLEMENTATION.md).


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
[Contract and next feasibility gates](particles/README.md).


### 23 November 2026 — merged integer support; isolated choice feasibility

PR #42 merged as `aa07e801ad5f2d89ebccd7299a4f539800e20df1`; main CI `45139587221` passed 3,620 source tests. The [bounded choice experiment](particles/CHOICE-FEASIBILITY.md) passes 119 controls per Core pin and all 3,620 unchanged source tests, with zero public API delta. Production inputs and published packages remain unchanged. Choice production adoption and full qualification are next; `xs:all` remains a separate deferred phase. Earlier pre-merge contract receipts remain historical evidence.


### 23 November 2026 - bounded choice production candidate

The [bounded root choice implementation](particles/IMPLEMENTATION.md) is qualified: 3,739 source tests (1,191 internal / 2,548 public), 2,548 package consumers per Core pin, 65 current-package native controls and retained callers 124/23/33 pass. All 3,620 prior source names/outcomes remain unchanged; 119 controls are added. API 54/401 has zero delta. Frozen pilot stays 21 matches; expanded stays 8 matches / 121 Unsupported / zero errors, with six explicit first-refusal reason changes per pin. Eighteen signed Core identities and signed beta.1 verify. Local unsigned alpha.0.34 only; published beta, pins and controllers unchanged. NuGet advisory endpoints were unavailable; no fresh vulnerability-data assessment is claimed. Choice is bounded and opt-in; xs:all remains excluded and is the next separate phase after review/delivery and merge verification. No commit/push/publication.


### 23 November 2026 - choice merged; separate all-group contract

PR43 merged as `724d834b7e2cf1353bdacf100802ff29866c8c71`; main CI45168029488 passes all 3,739 source tests and supporting checks. Next is the [bounded root all-group proposal](all-particles/README.md): 43 unexecuted XML plans, 20 actual-native gates and three unchanged original co-blocked examples. It explicitly records 18 proposed changes among 23 existing authored refusal controls, preserving payloads and IDs. This is contract review only: all semantics, production source/tests, API, Core pins, packages and release controllers are unchanged. Native feasibility precedes separate production adoption and qualification. No new commit, push or publication.


### 23 November 2026 - isolated all-group feasibility

The [bounded all-group scratch candidate](all-particles/FEASIBILITY.md) passes 146 controls per signed Core pin, with 86 unchanged-production baseline refusals per pin. All 3,739 original source names/stimuli remain: the initial run has exactly 18 predeclared all-refusal failures; scratch-only expected compile reconciliation passes 1,191 internal and 2,548 public tests. Production source/tests remain unchanged. Original physical order, declaration/governing type, optional-group/member rules, FOTY0012, sum budgets and lifetime pass; API54/401zero delta and18signed Core identities verify. Three original all schemas remain Unsupported; no new full corpus/package/retained run or publication. Next is separate production adoption and full qualification, explicitly recording the18authored admissions. No commit/push.


### 23 November 2026 - bounded all-group production candidate

The [bounded all-group implementation](all-particles/IMPLEMENTATION.md) is qualified: 3,885 source tests (1,251 internal / 2,634 public), 2,634 package consumers per Core pin, 60 current-package native controls and retained 124/23/33 pass. All 3,739 previous names/stimuli remain, with 18 explicit measured compile expectation changes and 146 new controls. API 54/401 zero delta; 18 signed Core identities and signed beta.1 verify. Frozen stays 21 matches; expanded stays 8 matches / 121 Unsupported / zero errors, with six explicit first-refusal changes per pin. Exact packaged all/unsignedLong examples pass; advisory checks enabled after network retry. Local unsigned alpha.0.35 only; beta/pins/controllers unchanged. Next: explicit Git delivery/review and merge/main verification, then re-rank remaining specification coverage. No commit/push/publication.


### 24 November 2026 - all groups merged; specification priorities reassessed

PR44 merged as `b08187d5f59f47a068737154ecdd6872506d3a88`; main CI45382084851 passes 3,885 source tests and supporting checks. The [post-particle coverage reassessment](post-particle-coverage/README.md) reuses the qualified 23 November sample: 8 matches / 121 Unsupported / zero errors, with no new corpus run or coverage percentage. Next recommended contract is bounded integer enumeration, followed by date/time transport design and wider Unicode regex support; schema assembly and implicit anyType remain separate dependencies. Source/test/build inputs, API, Core pins and packages are unchanged. Earlier matrices, rankings and seals remain historical; no new semantic adoption, commit/push or publication.


### 24 November 2026 - integer-enumeration contract prepared

The [bounded integer-enumeration contract](integer-enumeration/README.md) proposes value-space membership for existing integer families and placements, with 105 unexecuted XML cases and 18 native gates. Exact specification/source pins distinguish base-type entry validity from sibling intersections, preserve inheritance and define capacities. Static expectation impacts and a retained original-fixture Invalid-only harness assumption are recorded for measured reconciliation; upstream validities and payloads stay unchanged. No semantic adoption or product/API/Core/package change. Next: isolated native feasibility, then separate production adoption and qualification. No commit/push/publication.


### 24 November 2026 - isolated integer-enumeration feasibility

The [isolated candidate](integer-enumeration/FEASIBILITY.md) passes 261 controls per signed Core pin and 210 unchanged-baseline refusals per pin. One authored attribute-only root is retained as Unsupported; IE-073Q explicitly qualifies the supported attribute placement without widening grammar. Initial source regression has 10 measured expectation failures; scratch-only expectation/upstream-validity reconciliation passes all 3,885 prior test names with no removal or rename. Two original schemas compile Valid and their instances match preserved upstream valid/invalid outcomes. API 54/401 has zero delta and 18 signed Core identities verify. Production source/tests and earlier seals remain unchanged. Next: separate production adoption and full qualification; no commit/push/publication.


### 24 November 2026 - integer-enumeration production qualification

The [bounded integer-enumeration implementation](integer-enumeration/IMPLEMENTATION.md) passes 4,146 source tests (1,300 internal / 2,846 public), 2,846 package consumers per Core pin, 49 packaged native controls and retained callers 124/23/33. All 3,885 prior source names remain; 261 controls are added and ten measured expectations plus the original-fixture harness assumption are explicitly reconciled. API54/401 zero delta; 18 signed Core identities and published beta.1 verify. Frozen stays 21 matches; expanded is now 10 matches /119 Unsupported /zero errors, with exactly two original integer-enumeration admissions per pin. Integer membership is bounded to existing types/placements; original IE-073 remains Unsupported and its qualified variant is separate. Local unsigned alpha.0.37 only; no publication, pin/controller change or full XSD/PSVI claim. Next: explicit Git delivery/review and merge verification, then separate date/time transport design.


### 24 November 2026 - PR45 merged; date/time transport proposal

PR45 merged as 2eb999052188a872a54548c71c378d6a05a8cc3c; main CI45407113398 passes 4,146 source tests and supporting checks. The [calendar representation contract](date-time-transport/README.md) and proposed ADR018 recommend xs:date feasibility first, with exact year/month/day and optional timezone; dateTime/time need separate exact fractional-second transport review because both pinned Core carriers store milliseconds. There are 37 unexecuted root XML plans and 18 unexecuted native gates. Nine original calendar cases retain additional obligations; no admission forecast or new corpus execution. All 323 production inputs and earlier seals remain unchanged. No semantic adoption, public API, Core pin, source/test/package change or publication. Next: isolated package-only date transport feasibility and API review before native semantic adoption.


### 24 November 2026 - date transport preflight reaches boundary stop

The [package-only date preflight](date-transport-preflight/README.md) measures32 component round-trips and28 expected formatter results per Core pin; all four long.MinValue/zone combinations throw caught OverflowException. Both actual Schema facades still refuse xs:date. No native facts, detached facade, lifetime or validation success is claimed. The original full-Int64 proposal is preserved; a separate bound revision proposes excluding long.MinValue before the next isolated native experiment. All18 native gates remain pending. Eighteen unchanged Core archives/runtime payloads and two fresh Core signatures verify; all323 production inputs and59 earlier packets stay unchanged. No production/API/pin/package change, semantic adoption, commit/push or publication.


### 24 November 2026 - revised native date feasibility

The [isolated date candidate](native-date-feasibility/README.md) passes 99 uniquely labelled native controls and 4,147 scratch source cases per signed Core pin (1,301 internal / 2,846 public). All 4,146 original logical cases remain, with one explicit expected-outcome display rename and one newly eligible AL-008 identity case. Nine measured semantic failures are reconciled in isolated copies; the current lane additionally adjusts its hard-coded minimum-version assertion. The revised interval excludes long.MinValue; original sealed proposal and formatter failures remain intact. API preserves 401 rows and proposes seven additions (55 types / 408 rows). Eighteen signed Core payloads and two fresh Core signatures verify. All 323 production inputs and 60 prior seals stay unchanged. Native original-node facts, both public facades, ownership, cancellation, export and lifetime are measured; full packaged/retained/corpus qualification remains. Date complex simple content and temporal facets remain excluded. No production adoption, source/API/pin/package change, commit/push or publication. Next: separate production adoption and full qualification.


### 24 November 2026 - bounded native date production candidate

The [native date implementation](native-date-implementation/README.md) passes 4,246 source tests (1,308 internal / 2,938 public), 2,938 package consumers per Core pin, seven packaged native identity controls and retained124/23/33. Original4,146 logical cases remain with one explicit display rename,99 new controls and one newly eligible original AL-008 identity case; measured expectations are reconciled explicitly. API preserves401rows and adds seven (55types/408rows);18signed Core identities and published beta.1 verify. Frozen stays21matches; expanded now12matches/117Unsupported/zeroerrors: two original date admissions and one unchanged wildcard refusal with changed first reason. Exact packaged date/enumeration/all/unsignedLong examples pass; normal advisory checks stay enabled. Year interval excludes only long.MinValue within the signed representation; temporal facets and dateTime/time remain separate. Local unsigned alpha.0.40 only. Earlier61packets stay intact; no publication or Corepin/controller change. Next: explicit Git delivery/review and merge verification, then updated coverage reassessment.


### 24 November 2026 - PR46 merged; calendar gaps reassessed

PR46 merged as 383914562d89d3f7fad3a6a60cf4e29884cce912; main CI45426915018 passes 4,246 source tests. The [post-date reassessment](post-date-coverage/README.md) reuses the qualified sample of 12 matches / 117 Unsupported / zero errors, with no new execution. Nine calendar cases split into two delivered date matches, two date simple-content extensions, two mixed-content cases, two enumerations and one bound. Recommended next contract is bounded date simple-content extension with accurate complex-governing/atomic-date identity and an explicit guard against decimal restriction fallback. Date equality/facets and exact-seconds Core transport remain distinct follow-ups. All 328 production inputs and 62 earlier packets remain unchanged. No semantic adoption, API/source/test/pin/package change, commit/push or publication.


### 24 November 2026 - date simple-content contract prepared

The [bounded date simple-content contract](date-simple-content/README.md) defines named/anonymous roots and terminals, existing attribute uses, distinct complex G versus atomic date A, retained capacities and explicit derivation exclusions. It contains 77 unexecuted XML proposals and 18 native gates plus two unchanged original trials, without an admission forecast. Ordinary extra attributes are Invalid within this proposed date-extension closure even with zero uses; older profile behavior stays unchanged. Date complex restriction must be refused before decimal fallback. Static expectation impacts are recorded; no source/native/corpus execution or semantic adoption occurs. All 328 production inputs and 63 prior packets remain unchanged. Next: separate experimental adoption and isolated native feasibility on both signed Core pins. No API/pin/package change, commit/push or publication.


### 24 November 2026 - date simple-content isolated feasibility

The [isolated date extension candidate](date-simple-content-feasibility/README.md) passes 196 native controls per signed Core pin, including all 77 authored payloads on both facades, actual G/atomic-A identities and both unchanged original upstream trials matching their validities. All 4,246 original source names pass after one measured date-extension expectation correction in scratch copies; the current lane also adjusts its known minimum-version assertion. API remains55types/408rows with zero delta;18signed Core payloads and two fresh Core signatures verify. Three isolated source overlays implement date extension, scoped ordinary attribute closure and the date-restriction fallback guard. An initial nested-operation diagnostic error was corrected without source bypass; all receipts remain. All 328 production inputs and64 earlier packets remain unchanged. No production semantic adoption, source/API/pin/package change or full corpus/package/retained run. Next: separate production adoption and full qualification. No commit/push/publication.


### 24 November 2026 - date simple-content production candidate

The [date extension implementation](date-simple-content-implementation/README.md) passes 4,442 source tests (1,345 internal / 3,097 public), 3,097 package consumers per Core pin, 37 packaged identity controls and retained124/23/33. All4,246 prior source names/stimuli remain;196 controls are added, and the single measured factory refusal is explicitly reconciled. Actual complex G versus atomic date A, scoped closed attributes including zero uses and date-restriction fallback refusal are qualified. API55types/408rows has zero delta;18signed Core identities and published beta.1 verify. Frozen stays21matches; expanded now14matches/115Unsupported/zeroerrors, with exactlytwo original date-extension admissions per pin and no other row changes. Five exact packaged examples pass after a stale README exclusion was corrected without changing the DLL. Local unsigned alpha.0.43 only;65earlier packets intact. No publication/Corepin/controller change. Next: explicit Git delivery/review and merge verification, then separate date-enumeration/equality work.
