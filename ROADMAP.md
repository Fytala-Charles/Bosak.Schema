<div align="center">
  <img src="assets/logos/fytala-logo-color-dark.svg" width="100" alt="Fytala Bosak.Schema Roadmap">
  <h1>Bosak.Schema Roadmap</h1>
  <p>An open schema foundation with staged validation and integration.</p>
</div>

## Specification coverage priority - 19 November 2026

Prioritize the [shared specification programme](docs/specification-coverage/PROGRAM.md): an accountable coverage ledger, then namespace-aware decimal assessment and optional assertions, followed by coherent named types, attributes, datatypes and structural/application profiles. Release packaging is secondary to demonstrated coverage.


# Bosak.Schema Roadmap

> An open schema foundation with staged validation and integration.


## 1. Current Milestone

**Bosak.Schema 0.5.0-beta.1 is published and verified**, with an accepted bounded decimal-session contract. [NuGet](https://www.nuget.org/packages/Bosak.Schema/0.5.0-beta.1), [GitHub prerelease](https://github.com/Fytala-Charles/Bosak.Schema/releases/tag/v0.5.0-beta.1), [release scope and receipt](docs/BETA1_RELEASE.md). Services remain Apache-2.0 and activation-free. Public Core dependency minima remain 1.81.2.

See the [beta.2 gap assessment](docs/beta2-gap-assessment/README.md): a bounded Unsupported-reason improvement is proposed; no beta.2 grammar expansion or release is selected.

## 2. Next Milestones

1. Keep public documentation aligned with the verified release and explicit exclusions.
2. Exercise an identified package-only consumer workflow; distinguish representative fixtures from real application acceptance.
3. Record unsupported constructs and usability/lifetime findings, then scope beta.2 from that evidence.
4. Review Braid/XmlDb adoption separately after the bounded Schema consumer workflow is understood.

Broader XSD 1.1/component-universe/PSVI and general Core C45/ADR006 work requires separate design, immutable specification adoption and acceptance. No automatic scope expansion or new release is authorized by this roadmap.

## 3. Validation and Limits

345 source cases (312 implementation/33 public),33 package-only cases, retained 124 legacy/23 published-alpha cases per control/replacement lane and nine signed Core dependency payloads passed. A fresh registry-only33-case consumer passed after publication. Catalogs overlap; these are not additive totals.54 release controls and six workload integrity controls are separate from product coverage.

The [native API](docs/NATIVE-DECIMAL-PUBLIC-API.md), [restricted resolution](docs/RESTRICTED_RESOLUTION.md), [descriptor ownership](docs/DESCRIPTOR_SNAPSHOTS.md) and [legacy validation](docs/LEGACY_VALIDATION.md) define the supported limits. [Workload measurements](docs/BETA_QUALIFICATION.md) are bounded local observations, not a production SLA. Full XSD 1.1, general provider interchange and a perpetual API freeze remain unaccepted. Historical source integration receipts retain their original revisions; publication does not requalify them.

*Last updated: 2026-11-20*

## Diagnostic development candidate

[Bounded Unsupported reasons](docs/unsupported-reasons/README.md) are implemented and qualified locally. Review this additive API before any release/controller decision; broader namespaces, attributes and datatypes remain separate scopes. The0.6development minimum is not a beta.2 publication decision.

## M1 local progress

The [bounded namespace/optional-assertion candidate](docs/specification-coverage/M1-CANDIDATE-DELTA.json) is implemented and locally qualified. Wider component/attribute/datatype and structural milestones remain open; official W3C suite selection/admission remains separate from authored controls.

## README reconciliation after M1

The [packaged guidance update](docs/package-readme06-reconciled/README.md) follows the merged namespace slice. Specification coverage remains the priority; release-controller/count decisions must use the later reviewed profile, not the historical365/53plan.


## M2A scope proposal - 19 November 2026

M1 is merged (Schema PR17/Core PR504), with packaged guidance reconciled in Schema PR16. The next [bounded named-complex-type proposal](docs/m2a-named-types/PLAN.md) defines QName references, component reuse, actual Core identity/atomization and explicit refusal controls. Planning only; no runtime/API/pin/publication change or additional passed-test claim. REQ-009 remains In Progress.


## M2A implementation candidate - 19 November 2026

The [named-complex-type slice](docs/m2a-named-types/README.md) is locally implemented and qualified:465source cases(320implementation/145public),145minimum-Core package cases,52current-Core cases and retained124/23/33binary catalogs pass; scopes overlap. Up to16global types and16root declarations, full expanded-name lookup, eager unused-type checks, shared type identity and original Core facts are supported within the decimal-only profile. No Core engine/API/pin or published package/controller change. REQ-009 remains In Progress; broader M2 datatypes/facets/attributes and official W3C qualification remain pending.


## M2B decimal restriction proposal - 19 November 2026

M2A merged in PR18 at6861c04. The next [named-decimal restriction proposal](docs/m2b-decimal-restrictions/PLAN.md) defines direct xs:decimal simple types, selected range/digit facets, actual Core provenance and distinct capacity/Invalid/refusal gates. Planning only: no new runtime/API, semantic adoption, passed-test count or publication. Broader datatypes and attributes follow in separate slices; REQ-009 remains In Progress.


## M2B implementation candidate - 19 November 2026

The [adopted decimal-restriction slice](docs/m2b-decimal-restrictions/README.md) implements named simple types directly based on xs:decimal and six selected range/digit facets, with named original Core provenance.534source cases(324implementation/210public)pass; source/package/retained qualification is recorded in the dossier with overlapping lanes kept separate. Combined16global type ceiling; explicit facet integer capacity256. Named nondecimal types, restriction chains, other facets and attributes/imports remain excluded. No Core engine/API/pin or release/publication change; REQ-009 remains In Progress.


## M2C datatype and owned-value proposal - 19 November 2026

M2B merged in PR19 atfbb4c13. The [next datatype proposal](docs/m2c-builtin-datatypes/PLAN.md) stages strings with an additive owned scalar facade, then Boolean/integer, followed by selected string facets and attributes. Existing DecimalCount/ReadDecimal semantics stay bounded to decimals. Planning only: no new API, datatype adoption, runtime, passed-test count or release change. Exact surface/contract and published-Core feasibility proof are required before implementation. REQ-009 remains In Progress.


## M2C1 concrete surface and Core feasibility - 19 November 2026

The [API review package](docs/m2c-builtin-datatypes/api-review/README.md) and [proposed ADR011](docs/ADR-011-owned-native-scalar-facade.md) define an additive ten-type string/decimal facade without changing old decimal semantics.14synthetic public transport cases pass separately on signed Core1.81.2 and1.81.17, with18package signatures/source/payload identities verified. No new Schema validation or API implementation/acceptance, engine/pin or publication change. Scalar provenance is snapshot-bound; complete-container views are operation-local. Actual mixed Schema node/facade qualification and exact string semantic adoption remain next; REQ-009 is In Progress.


## M2C1 string/decimal implementation candidate - 19 November 2026

The [adopted string/decimal facade](docs/m2c-string-facade/README.md) adds the ten reviewed public types under explicit opt-in and shares the existing owner/gate/cleanup path.614source cases(334implementation/280public)pass, including47authored string cases,23public contract controls,6mixed lifecycle and4actual Core/writer controls. Primitive string text is preserved with explicit UTF16capacity; typed reads retain original facts internally and bounded export preserves parsed CR references on reopen. NativeDecimalSession remains decimal-only. No Boolean/integer/facet/attribute/import expansion, Core engine/pin or release/publication; REQ-009 and overall API/C45 acceptance remain In Progress.


## M2C2 Boolean implementation candidate - 19 November 2026

M2C1 merged in PR20 at16355ea; fresh main CI passes614source cases and54release controls. The [bounded Boolean candidate](docs/m2c-boolean/README.md) adds Boolean=3 and a checked BooleanValue getter to the opt-in facade, actual Boolean provenance/atomization and a fixed256code-unit lexical ceiling before XML-whitespace collapse.671source cases(340implementation/331public)pass locally, with intentional prior-refusal deltas recorded separately. Legacy decimal facade unchanged; Boolean assertions/restrictions and integer support remain excluded. No Core engine/pin or release/publication change; REQ-009 and overall API/C45 acceptance remain In Progress.

## M2C3 exact integer implementation candidate - 19 November 2026

M2C2 merged in PR21 at20cc520; fresh main CI44602002427 passed671source cases and54release controls. The [bounded integer candidate](docs/m2c-integer/README.md) admits built-in xs:integer (a decimal restriction, not a primitive datatype), checked detached BigInteger values and original integer provenance/atomization.748source cases(347implementation/401public)pass locally. Raw lexical ceiling256UTF16code units; MaxNativeCoefficientDigits includes supplied leading zeroes, excludes sign and is enforced before normalization. Existing separate query digit limits can refuse a read without poisoning export/smaller reads. Legacy decimal facade remains decimal-only. Integer assertions, derived families and named integer restrictions/facets remain excluded. Local qualification is not a new release, official W3C-suite conformance or general API/C45 acceptance; no Core engine/pin/controller/publication change. REQ-009 remains In Progress.

## M2D1 named string restriction candidate - 19 November 2026

M2C3 merged in PR22 at014cb28; fresh main CI44609515923 passed748source cases,54release controls and6workload controls. The [named string candidate](docs/m2d-string-restrictions/README.md) adds opt-in direct xs:string restrictions with length/minLength/maxLength and enumeration.837source cases(355implementation/482public)pass locally. Length counts Unicode scalar values; UTF16leaf/enumeration representation limits stay distinct. Exact ordinal enumeration preserves parsed whitespace and Unicode sequence, including XML-normalized facet attributes; duplicate entries are admitted under a16entry ceiling. Named original typed facts/provenance remain; no public API member change. Legacy decimal facade, decimal sibling constraints and assertions remain unchanged. Pattern/regex, whitespace overrides, restriction chains and string assertions remain excluded. REQ-009 stays In Progress; no Core engine/pin or release/controller/publication change or general API/C45 acceptance.

## M2E1 root attribute contract proposal - 19 November 2026

M2D1 merged in PR23 at098fdaf; fresh main CI44614488088 passed837source cases,54release controls and6workload controls. The [root attribute contract](docs/m2e-attributes/CONTRACT.md) and [ADR012](docs/ADR-012-owned-root-attribute-reads.md) propose required/optional local root attributes with separate AttributeCount/ReadAttribute and detached namespace/local-name/scalar entries, preserving leaf indexes.27public-only synthetic Core probes pass on each signed1.81.2/1.81.17 lane, including original attribute facts/provenance and foreign/lifetime refusals. Those overlapping feasibility controls are not new Schema cases or attribute validation. Product source/API/pins and delivered837suite remain unchanged. New API/semantic adoption, actual native validation/atomization and consumer/retained qualification remain required; general API/C45 acceptance and registry publication are not inferred. REQ-009 stays In Progress.

## M2E1 root attribute implementation candidate - 19 November 2026

The [adopted root attribute slice](docs/m2e-root-attributes/README.md) implements the concrete ADR012 proposal locally on098fdaf.950source cases(372implementation/578public)pass, including all837prior outcomes unchanged. Only opt-in local required/optional root attributes are admitted; separate AttributeCount/ReadAttribute and detached NativeAttributeValue name/scalar entries preserve existing leaf indexes. Declaration-order indexing skips absent optionals, distinguishes false/zero/empty, respects namespace forms/XML normalization and uses original native typed facts/provenance. Scalar representation/query/cache and ownership/cancellation/cleanup/export rules remain. Legacy decimal facade, no-attribute root refusal and leaf-attribute exclusion stay unchanged. Defaults/fixed/global refs/groups/wildcards/attribute assertions remain excluded. Local qualification is not hosted CI/new publication or general API/C45 acceptance; no Core engine/pin/controller change. REQ-009 remains In Progress.

## Decimal simple-content root candidate - 20 November 2026

PR24 merged atd4eca99; main CI44689935935 passed950source/54release/6workload controls. The [adopted decimal simple-content slice](docs/decimal-simple-content/README.md) locally adds HasRootScalar/ReadRootScalar, preserving leaf/attribute indexes.1036source cases(386implementation/650public)pass with all950prior outcomes unchanged. Direct xs:decimal/admitted named decimal bases and current root attributes support the isolated unchanged PriceType; the full orders fixture still refuses its other unsupported definitions. Complex root annotation/governing provenance stays distinct from actual decimal atomic type; complex status is independent of no-typed-value, and root string-value uses schema whitespace collapse. Existing element-only FOTY0012, original facts, capacity/cache/gate/disposal and detached export semantics remain. Restriction/complex-base chains, other base families, nested/mixed content and simple-content assertions remain excluded. REQ-009 stays In Progress; no Core engine/pin, new release or general API/C45 acceptance.


## Bounded nested-sequence proposal - 20 November 2026

PR25 merged at c34e9fe; fresh main CI44695518279 passed 1,036 source cases,
54 release controls and six workload controls. The [nested-sequence proposal](docs/nested-sequences/PLAN.md)
defines acyclic named/anonymous element-only nesting, finite occurrences and an
internal terminal-scalar projection preserving existing flat indexes. Proposed
limits are depth 4, 64 expanded element nodes and 16 terminal scalar occurrences
per root. Root attributes remain separate; nested attributes/simple content,
choice/all, recursion, unbounded repetition and nested-profile assertions remain
excluded. Planning only: no new implementation, API, semantic adoption, passed
feature count, Core pin or publication. REQ-009 remains In Progress.


## Nested transport feasibility and semantic adoption - 20 November 2026

The [nested dossier](docs/nested-sequences/FEASIBILITY.md) records 50 public-only synthetic controls per signed
Core1.81.2/1.81.17 lane (13 new, 37 inherited), 18 package signature/source/payload
checks and an actual pre-implementation Schema refusal. These are transport and
baseline controls, not new product cases or nested validation. A separate exact
Structures/XDM adoption now precedes production changes. All production files,
the 1,036-case source catalog, public API and Core pins remain unchanged. Actual
nested assessment and terminal-only projection implementation are next; REQ-009
remains In Progress. No commit/push or publication.


## Nested-sequence implementation candidate - 20 November 2026

The [adopted nested-sequence candidate](docs/nested-sequences/README.md) locally implements acyclic named/
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
54release controls and six workload controls. The [nested-attribute proposal](docs/nested-attributes/PLAN.md)
and proposed ADR014 define separate descendant reads plus immutable owner-element
paths, preserving existing root/terminal indexes. Reuse current local attribute
semantics on nested element-only complex containers, with64potential attribute
uses including root/optional expansions. Planning only: no runtime/API, semantic
adoption or new passed product case. Nested simple content/leaf attributes remain
excluded. REQ-009 and general API/C45 acceptance remain In Progress; published
beta.1, Core pins and release controller are unchanged.


## Nested attribute feasibility and before-code adoption - 20 November 2026

The [nested attribute evidence](docs/nested-attributes/FEASIBILITY.md) records69public-only controls per signed
Core1.81.2/1.81.17lane (19new/50inherited),18signature/source/payload identities
and four actual pre-implementation Schema/API controls. Exact node/fact binding
rejects authentic same-authority facts swapped across owners. Path prototypes
are detached locators, not facade implementation or authority. Separate dated
Structures/Datatypes/XML/XDM semantic adoption now precedes production edits.
Product source/API and1,125source catalog remain unchanged; broader API/C45,
official suite and publication acceptance remain separate. REQ-009 is In Progress.


## Owner-aware nested attribute candidate - 20 November 2026

The [adopted nested-attribute candidate](docs/nested-attributes/README.md) implements separate descendant
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
54release/sixworkload controls and documentation hygiene. The [next proposal](docs/nested-simple-content/PLAN.md)
and proposedADR015 define complex decimal simple-content terminals in existing
ReadScalar order, original complex/atomic provenance and depth4three-segment
attribute-owner paths, with unchanged root reads/budgets and zero public API delta.
Planning only: no production change, semantic adoption or new passed feature case.
Potential NS-062/NA-046compile admissions need explicit before-code baselines and
instance outcomes. REQ-009/generalAPI/C45 remain In Progress; publishedbeta.1 and
Core pins/controller unchanged.


## Nested decimal simple-content implementation - 20 November2026

The [adopted nested decimal candidate](docs/nested-simple-content/README.md) implements direct decimal/named-decimal
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
[Pilot and gap evidence](docs/conformance-pilot/README.md). REQ-009 remains In Progress.

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
[Adoption and qualification](docs/schema-location-hints/README.md). REQ-009 remains In Progress.
