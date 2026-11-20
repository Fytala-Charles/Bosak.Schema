# REQ-009: specification coverage programme

Owner priority recorded19November2026: specification coverage and useful external
application workflows in both Bosak and Bosak.Schema. Existing REQ-009 remains
In Progress; CoreREQ-142 is registered locally for the shared consumer/engine work.
The shared [programme](specification-coverage/PROGRAM.md) and [coverage matrix](specification-coverage/MATRIX.json)
separate native assessment, legacy helpers/descriptors and Core integration.

M0 establishes exact clause/case evidence. M1 proposes namespace-aware decimal
assessment and optional assertions before wider named types/attributes/datatypes.
New behavior requires immutable adoption and paired acceptance controls first.
Core C45/REQ139/140, full XSD1.1 and API/publication acceptance remain distinct.
No compiler/provider/session behavior or existing release/pin changes here.

## M1 local implementation - 19 November 2026

The namespace/optional-assertion slice is adopted and implemented locally in Schema with409source,94minimum-Core package and42current-Core package cases passing. The [candidate delta](specification-coverage/M1-CANDIDATE-DELTA.json) and [authored case map](specification-coverage/M1-CASE-MAP.json) distinguish positive/invalid/profile-refusal evidence. No Core runtime/API or optional-wrapper pin changes. Delivery, broader scope and official W3C suite qualification remain pending.


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
