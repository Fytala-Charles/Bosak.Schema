<div align="center">
  <img src="../assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Feature Requests">
  <h1>Bosak.Schema Feature Requests</h1>
  <p>Current public schema scope, status and acceptance boundaries.</p>
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


# Bosak.Schema Feature Requests

> Current public schema scope, status and acceptance boundaries.


## 1. Registry

**Current release — 18 November 2026:** [Bosak.Schema 0.5.0-beta.1](https://www.nuget.org/packages/Bosak.Schema/0.5.0-beta.1) is published and independently verified. [GitHub prerelease](https://github.com/Fytala-Charles/Bosak.Schema/releases/tag/v0.5.0-beta.1), [scope and publication receipt](BETA1_RELEASE.md).

The published **0.5.0-beta.1** provides owned `NativeDecimalSession` and `NativeDecimalDocument` APIs with explicit capacities/context, exact decimal reads and detached XML export. ADR009/010 are accepted for this bounded Schema application scope. Qualification passed 345 source cases (312 implementation/33 public),33 package-only cases and retained 124/23 binary lanes using Core 1.81.2. These catalogs overlap. [Release, exclusions and receipt](BETA1_RELEASE.md). Full XSD 1.1, general Core C45/ADR006 and production SLA acceptance remain separate.

| Request | Status | Scope |
|---|---|---|
| Apache/public transition | Implemented and GitHub publication verified | Schema-only Apache-2.0; activation-free services; private infrastructure excluded |
| Packaged0.6consumer guidance | Reconciled after M1; no publication | [Examples and evidence](package-readme06-reconciled/README.md); earlier planning receipts retain original scope |
| Shared specification coverage | Owner-prioritized; M1/M2A/M2B/M2C1 merged, M2C2 merged; M2C3 merged; M2D1 merged; M2E1 locally qualified implementation | [REQ-009 programme](REQ-009-SPECIFICATION-COVERAGE.md); shared CoreREQ-142;24areas with explicit evidence limits |
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
