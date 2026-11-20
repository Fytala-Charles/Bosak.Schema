<div align="center">
  <img src="../assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Architecture">
  <h1>Bosak.Schema Architecture</h1>
  <p>Modular Apache schema services with explicit resource and engine boundaries.</p>
</div>

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


# Bosak.Schema Architecture

> Modular Apache schema services with explicit resource and engine boundaries.


## 1. Current Architecture

**Current release — 18 November 2026:** [Bosak.Schema 0.5.0-beta.1](https://www.nuget.org/packages/Bosak.Schema/0.5.0-beta.1) is published and independently verified. [GitHub prerelease](https://github.com/Fytala-Charles/Bosak.Schema/releases/tag/v0.5.0-beta.1), [scope and publication receipt](BETA1_RELEASE.md).

One public `Bosak.Schema` assembly references the published Bosak XPath API/providers, XSLT and XQuery packages at 1.81.2 after the merged dependency adoption. The core engine does not reference this package. Keeping schema services in a separate assembly avoids a dependency cycle; changing the license does not require merging them into Core.

## 2. Components

| Component | Responsibility and limit |
|---|---|
| `Compilation` | Explicit-version schema descriptor compilation and bounded resolved imports/includes |
| `Model` | Staged XSD 1.1 assertions, alternatives, declarations, simple facets, open-content/default-attribute metadata, wildcard negative constraints (notNamespace/notQName) |
| `Validation` | Assertion/facet evaluation and supported governing-type applicability through Bosak XPath |
| `Assessment` | Internal finite native backend; authority/advisor/input adapters; canonical guarded nodes, original facts and scalar provenance; host-owned leases and one shared sequential runner per session; public owned native decimal session/document facade |
| `SchemaServices` | Activation-free factories for compiler and validation services |
| `SchemaAwareXslt` | Opt-in helper for the existing schema-aware XSLT path |
| Legacy license DTO/shim types | API compatibility only; no credential implementation or authority |

The separate private licensing repository retains key issuers, payments, delivery/deployment services and historical commercial code. None is referenced by the public solution.

## 3. Identity and Integration Limits

The current compiled root-document digest is a legacy identifier, not proof of a complete schema-component universe. Full compatible-universe/PSVI session integration remains a separate design and provider qualification task. No new core session API is accepted by this licensing change.

Schema language version, host/XDM profile, component authority and source/snapshot identity must remain distinct in future integration. Preserve original PSVI across compatible schemas; QName or digest equality alone must not authenticate a component authority.

## 4. Key Design Decisions

| Decision | Status |
|---|---|
| [ADR-007: Apache/public schema split](ADR-007-apache-schema-publication.md) | Accepted by owner on 29 October 2026 |
| [ADR-005: XSD 1.1 compiled model and validation pipeline](ADR-005-xsd11-compiled-model-and-validation.md) | Accepted by owner on 27 October 2026; transferred to the public repository 30 October 2026 as REQ-009 decision evidence |
| [ADR-006: XSD 1.1 S4/S5 engine seam compatibility](ADR-006-xsd11-engine-seam-compatibility.md) | Proposed; pending core-repo acceptance before the S4 design freeze (core tracking REQ-139) |
| [Core 1.81.2 dependency adoption](CORE-1.81.2-ADOPTION.md) | Merged in Schema PR #1; existing source and unchanged-binary coverage qualified |
| [Historical BCAP O1/O2 design review](core-disposition-responses/2026-11-03-v2/README.md) | Scoped design returns dated 3 November; not new native/runtime acceptance |
| Preserve modular package and core dependency direction | Retained |
| [ADR-009: first native decimal session profile](ADR-009-native-decimal-session-profile.md) | Accepted for the bounded Schema beta profile; broader Core/provider acceptance excluded |
| [ADR-010: owned synchronous native decimal facade](ADR-010-owned-native-decimal-facade.md) | Accepted for the bounded owned Schema facade; published and externally consumer-qualified |
| Require no activation for standards processing | Implemented |
| Full XSD 1.1 provider/session integration | Pending; no conformance promotion |

## 5. Validation and Extensibility

The published **0.5.0-beta.1** provides owned `NativeDecimalSession` and `NativeDecimalDocument` APIs with explicit capacities/context, exact decimal reads and detached XML export. ADR009/010 are accepted for this bounded Schema application scope. Qualification passed 345 source cases (312 implementation/33 public),33 package-only cases and retained 124/23 binary lanes using Core 1.81.2. These catalogs overlap. [Release, exclusions and receipt](BETA1_RELEASE.md). Full XSD 1.1, general Core C45/ADR006 and production SLA acceptance remain separate.

The [native decimal session profile](NATIVE-SESSION-PROFILE.md) maps N1–N8 onto inspected Core 1.81.2 contracts and defines twelve paired planned acceptance groups. It requires a native structural backend, authority-owned facts and host-retained operation lifetimes. Cross-universe handoff remains Unsupported in this first scope. No adapter or new public API is delivered by the definition.

Historical prototype qualification: all 243 standalone cases passed: 124 retained, seven scalar probes, 76 backend, 28 adapter and eight [owned-facade cases](native-session-facade/2026-11-15/README.md). The prototype owns the sole Core session/runner and each document admission, exposing only detached exact decimals and XML through its internal workflow. Borrowed container and scalar receipt expiry, quota/cancellation/fault cleanup and default-name argument rejection are qualified. The finite grammar is unchanged. The published beta supersedes the public-symbol/external-consumer work described here; ADR009/010 acceptance is bounded to the Schema application facade, with broader Core acceptance excluded. Historical receipts retain their scopes.

The dependency adoption changes no Schema C# implementation, upstream baseline/filter or storage format. The same 124 cases pass after rebuilding and in the separately retained unchanged-binary lane; these repeated case catalogs are not additive coverage. The Core optional source wrapper pins merged Schema c953ebe and runs 124 upstream plus 11 local cases. This does not prove arbitrary downstream ABI, native registry/lease safety, concurrent disposal or full PSVI/component completeness.

Earlier source/design inputs and unsigned drafts remain in their [historical inbox](core-disposition-inbox/2026-11-03/README.md); their original identities and decisions are unchanged. Current engineering state takes precedence over the archived local notices in [development history](DEVELOPMENT_HISTORY.md).

*Last updated: 2026-11-19*


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
