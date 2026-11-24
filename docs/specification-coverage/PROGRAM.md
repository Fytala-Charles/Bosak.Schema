# Shared specification coverage programme

24 November 2026: [bounded date enumeration production candidate](../date-enumeration-implementation/README.md) adopts direct date membership with preserved timezone presence/original values on Core1.81.17. Source4,874cases, package3,509public/20native, six examples and unchanged55type/408rowAPI qualify. Expanded sample16matches/113Unsupported/zeroerrors. Retained migration passes124/22/32behavior cases; two full historical catalogs retain their explicit old-Core pin assertion failures. REQ-009 remains In Progress; no publication.

24 November 2026: [isolated date-enumeration feasibility](../date-enumeration-feasibility/README.md) passes428native controls and4,442regression results after three measured expectation updates. API55types/408rows unchanged; expanded sample16matches/113Unsupported/zero errors. Original container comments are already admitted; scalar comments remain refused. Production enumeration behavior and earlier sealed evidence are unchanged.

24 November 2026: [date-enumeration/value-equality contract](../date-enumeration/README.md) prepared for review on the Core 1.81.17 baseline candidate. Proposed direct date restrictions preserve timezone presence and original typed representations; no implementation/adoption or new semantic execution is claimed. REQ-009 remains In Progress.

19 November 2026. Owner priority: increase specification coverage and practical
application interoperability in **Bosak and Bosak.Schema**. This supersedes the
earlier diagnostic-first priority for subsequent feature selection. Published
beta.1 and the merged diagnostic API remain valid within their declared limits.

## Baseline and evidence discipline

Core baseline `f7b6c9959b46b67919c7f4677ff67e3fbf6bc03d`; Schema baseline
`3ff8cb6d6a0f14bf81f6e0f25a498c301580dcdc`. The identical machine-readable
[matrix](MATRIX.json) is mirrored in both repositories. It contains24 feature
areas, not every normative requirement. The retained365-case Schema catalog is
indexed in [CATALOG.json](CATALOG.json); this is historical passed evidence, not
a fresh run or a W3C conformance total.

No completion percentage is justified yet. Each area distinguishes bounded native
runtime, legacy helper evaluation, descriptor-only support, explicit refusal and
unassessed work. Catalog references identify relevant test classes for audit;
they do not assert every referenced test exercises every normative clause. W3C
case IDs are intentionally empty until selected and reviewed individually.
Passing Unsupported/refusal tests is not positive feature support. Metadata is
not structural validation. Existing .NET/XPath datatype machinery is not evidence
that Schema validates those types under XSD1.1 rules.

The original six dated specification-body hashes/adoption records are preserved.
They cover the existing decimal profile only. New features need their own exact
immutable bodies, sections and intentional deviations before implementation.
The dated Namespaces1.0 Third Edition is a review target, not a new adoption.
Use XSD1.1 Structures and Datatypes together, plus their referenced XPath/XDM/
namespace rules. Do not inherit stale status claims from the historical S0 matrix.

## Ownership and dependency direction

Schema owns schema parsing/assembly, components, structural assessment, datatypes
and facets, XSD assertion context and resulting validation facts. Core owns its
XPath/XQuery/XSLT semantics, neutral XDM/provider/typed-value transport and engine
lifetime guards. Core source projects must not gain a mandatory Schema dependency.
Use current public APIs first. A missing neutral engine capability must be
reproduced and requested under CoreREQ-142, linked to REQ-139/140; do not invent a
friend/reflection bypass or automatically accept general C45/provider/freeze work.

Core's optional integration currently pins older Schema9fd4e6a against Core1.81.17,
with a recorded260-case implementation/legacy lane and nine separate public cases.
That does not qualify current Schema365 or expanded grammar. Refresh its exact
source/test/legal pin only after a reviewed Schema implementation is delivered;
keep original receipts and distinct dependency identities. A success on an untyped
transform does not prove PSVI or schema-aware transform semantics.

## Milestones and exit gates

| Milestone | Deliverable | Exit evidence |
|---|---|---|
| M0: accountable baseline | Refine24areas into clause-level requirements, map exact positive/negative/refusal cases, pin a suitable upstream XSD suite revision and redistribution terms | Every scoped claim has immutable spec/fixture/source identities, route and outcome; unassessed remains explicit |
| M1: basic interoperable decimal documents | Namespace-aware names and zero-or-more assertions in the existing bounded document shape | Prefix/default-namespace, wrong namespace, form defaults/overrides, assertion-context and assertion-free controls; external package consumer plus actual Core provider lane |
| M2: ordinary application schema components | Named simple/complex types, references; coherent string/boolean/integer/decimal and selected facets; required/optional attributes | A namespaced order/invoice with identifiers and currency can be assessed, with invalid cases and exact typed-value expectations |
| M3: structural breadth | Nested sequence/choice/all and occurrence rules, schema composition, defaults/fixed/nil; wider assertion contexts | Component constraints and instance-validation rules independently mapped; schema-aware consumers never bypass missing structural checks |
| M4: XSD1.1 breadth | Conditional types, open content/wildcards, default attributes, lists/unions, derivation/substitution/identity constraints, remaining datatypes/facets | Positive enforcement and negative normative cases; descriptor recording alone cannot close rows |
| M5: interoperable release profile | Full selected application workflows, PSVI/atomization/query/copy/export/reopen ownership where supported, published-package and retained-binary lanes | Clear documented support profile, reproducible suite results, frozen API compatibility within that profile; no general conformance claim beyond demonstrated scope |

Milestones are direction and dependency order, not dates or a full implementation
commitment in one step. Several M2-M4 areas need multiple separately adopted
slices. A coherent useful profile comes before a broad list of partially parsed
features. Existing public orders/sku examples are representative engineering
targets, not production-user acceptance and not already supported by namespaces.

## First implementation slice: M1

Prepare exact adoption for expanded-name equality, targetNamespace, qualified/
unqualified local element forms, matching and assertion name resolution. Preserve
the unqualified profile and capacity limits. Permit schemas with no assertions;
the current mandatory1..8 assertion restriction is an implementation limitation,
not an XSD requirement. Retain the upper bound and explicit refusal for constructs
outside the chosen slice. This is proposed scope; it changes no current behavior.

Before code, decide and record which xpathDefaultNamespace forms the slice admits,
how prefixed assertion child names resolve, and how imported/named components stay
refused. Do not silently strip namespaces, compare prefixes/local names only, or
promise arbitrary XPath. Schema components/facts must retain expanded identity
through original assessment and Core adapters. No general C45 seam is assumed
necessary; establish a concrete missing capability before changing Core.

Minimum paired controls:

1. Same namespace with different prefixes; default namespace and prefix variants.
2. Same local name in wrong namespace; qualified/unqualified local forms and
   form overrides; preserve the previously supported no-namespace documents.
3. Zero assertions, true/false supported assertions, and rejected unsupported
   assertion syntax/namespace context. Full refusal exposes no owned result.
4. Identical expanded type/declaration identities survive actual public Core
   assessment/atomization/read/export paths; schema namespace does not grant a
   foreign schema or node authority. Unqualified compatibility remains intact.
5. Byte/numeric/document quotas, cancellation, disposal, detached XML and caller
   ownership remain explicit. Changed diagnostics/outcomes need a compatibility
   decision, not relabelling old tests as conformant.

## Joint qualification rules

- Source/unit results, W3C normative cases, representative workflows, performance
  observations and binary/package compatibility remain separate catalogs.
- Every slice needs positive, invalid and unsupported boundaries plus at least
  one external package-only consumer without friend access and an actual Core
  integration lane. Use fixed original binaries for replacement controls.
- Schema's1.81.2 package minimum and the optional current-Core source lane are
  separate compatibility targets. Do not suppress exact identity checks or claim
  every higher dependency version from one tested combination.
- Any engine-touching slice runs the complete Core solution and applicable
  conformance gates. Preserve QT3 **31,397/0/424**, pinned standalone XSLT3.0
  **10,221/0/4,380**, and qt4 gated-set rules, names/reasons, corpus/source hashes
  and watchdogs. Changes to skip admission require their own reviewed adoption.
- Independent reference implementations may help find disagreements; only the
  adopted normative source decides behavior. Pin reference version/settings,
  record discrepancies and never treat agreement alone as conformance.
- No release tag/registry push or API freeze is implied. Keep default builds and
  unrelated application workflows stable; publication follows demonstrated profile.

## Priority and pending delivery

Specification coverage is now the feature priority. More release-controller or
packaging-only work should not displace M0/M1. Existing SchemaPR16 is green and
open; leave it intact for separate review/merge authorization. It does not block
coverage planning. Braid production licensing remains an independent paused track.

This is a local planning/baseline-audit step, not an implementation, new semantic
adoption, normative-suite run or fresh engine qualification. No percentages,
runtime test totals or production readiness have been manufactured.


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
[Contract and qualification plan](<../xsi-type-contract/README.md>).


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
[Native feasibility and preserved iterations](<../xsi-type-contract/FEASIBILITY.md>).


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
[Implementation qualification](<../xsi-type-contract/implementation/README.md>).


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
[Merge receipt, ranked gaps and next contract](<../post-xsi-type/README.md>).


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
is Structures3.17.2. [Contract, adoption and receipts](<../namespace-form/README.md>).


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
[Merge evidence and contract](<../complex-simple-restriction/README.md>).


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
consumer/binary/pilot qualification. [Feasibility and receipts](<../complex-simple-restriction/FEASIBILITY.md>).


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
from context-item/inherited assertions. [Implementation and exact receipts](<../complex-simple-restriction/IMPLEMENTATION.md>).


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
[Contract and review evidence](<../integer-simple-content/README.md>).


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
[Native feasibility and preserved evidence](<../integer-simple-content/FEASIBILITY.md>).


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
[Implementation and exact qualification receipts](<../integer-simple-content/IMPLEMENTATION.md>).


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
[Proposed assertion contract and review evidence](<../contextual-assertions/README.md>).


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
[Native feasibility and preserved evidence](<../contextual-assertions/FEASIBILITY.md>).


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
[Implementation and exact qualification receipts](<../contextual-assertions/IMPLEMENTATION.md>).


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
[Contract and review evidence](<../structural-assertions/README.md>).


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
[Scoped native results and preserved receipts](<../structural-assertions/EMPTY-VALUE-FEASIBILITY.md>).


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
[Native feasibility and exact preserved receipts](<../structural-assertions/FEASIBILITY.md>).


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
[Implementation and qualification receipts](<../structural-assertions/IMPLEMENTATION.md>).


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
[Expanded sample, rankings and exact evidence](<../expanded-conformance/README.md>).


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
[Contract, exact review pins and measured Core capability](<../string-patterns/README.md>).


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
[Native feasibility and exact preserved receipts](<../string-patterns/FEASIBILITY.md>).


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
See the [implementation and qualification](../string-patterns/IMPLEMENTATION.md).


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
[Contract and next feasibility gates](../simple-typed-roots/README.md).


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
[Feasibility and preserved receipts](../simple-typed-roots/FEASIBILITY.md).


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
[Implementation and exact qualification](../simple-typed-roots/IMPLEMENTATION.md).


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
[Contract and next feasibility gates](../derived-integer-types/README.md).


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
[Native feasibility and preserved receipts](../derived-integer-types/FEASIBILITY.md).


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
[Implementation and exact qualification](../derived-integer-types/IMPLEMENTATION.md).


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
[Contract and next feasibility gates](../particles/README.md).


### 23 November 2026 — merged integer support; isolated choice feasibility

PR #42 merged as `aa07e801ad5f2d89ebccd7299a4f539800e20df1`; main CI `45139587221` passed 3,620 source tests. The [bounded choice experiment](../particles/CHOICE-FEASIBILITY.md) passes 119 controls per Core pin and all 3,620 unchanged source tests, with zero public API delta. Production inputs and published packages remain unchanged. Choice production adoption and full qualification are next; `xs:all` remains a separate deferred phase. Earlier pre-merge contract receipts remain historical evidence.


### 23 November 2026 - bounded choice production candidate

The [bounded root choice implementation](../particles/IMPLEMENTATION.md) is qualified: 3,739 source tests (1,191 internal / 2,548 public), 2,548 package consumers per Core pin, 65 current-package native controls and retained callers 124/23/33 pass. All 3,620 prior source names/outcomes remain unchanged; 119 controls are added. API 54/401 has zero delta. Frozen pilot stays 21 matches; expanded stays 8 matches / 121 Unsupported / zero errors, with six explicit first-refusal reason changes per pin. Eighteen signed Core identities and signed beta.1 verify. Local unsigned alpha.0.34 only; published beta, pins and controllers unchanged. NuGet advisory endpoints were unavailable; no fresh vulnerability-data assessment is claimed. Choice is bounded and opt-in; xs:all remains excluded and is the next separate phase after review/delivery and merge verification. No commit/push/publication.


### 23 November 2026 - choice merged; separate all-group contract

PR43 merged as `724d834b7e2cf1353bdacf100802ff29866c8c71`; main CI45168029488 passes all 3,739 source tests and supporting checks. Next is the [bounded root all-group proposal](../all-particles/README.md): 43 unexecuted XML plans, 20 actual-native gates and three unchanged original co-blocked examples. It explicitly records 18 proposed changes among 23 existing authored refusal controls, preserving payloads and IDs. This is contract review only: all semantics, production source/tests, API, Core pins, packages and release controllers are unchanged. Native feasibility precedes separate production adoption and qualification. No new commit, push or publication.


### 23 November 2026 - isolated all-group feasibility

The [bounded all-group scratch candidate](../all-particles/FEASIBILITY.md) passes 146 controls per signed Core pin, with 86 unchanged-production baseline refusals per pin. All 3,739 original source names/stimuli remain: the initial run has exactly 18 predeclared all-refusal failures; scratch-only expected compile reconciliation passes 1,191 internal and 2,548 public tests. Production source/tests remain unchanged. Original physical order, declaration/governing type, optional-group/member rules, FOTY0012, sum budgets and lifetime pass; API54/401zero delta and18signed Core identities verify. Three original all schemas remain Unsupported; no new full corpus/package/retained run or publication. Next is separate production adoption and full qualification, explicitly recording the18authored admissions. No commit/push.


### 23 November 2026 - bounded all-group production candidate

The [bounded all-group implementation](../all-particles/IMPLEMENTATION.md) is qualified: 3,885 source tests (1,251 internal / 2,634 public), 2,634 package consumers per Core pin, 60 current-package native controls and retained 124/23/33 pass. All 3,739 previous names/stimuli remain, with 18 explicit measured compile expectation changes and 146 new controls. API 54/401 zero delta; 18 signed Core identities and signed beta.1 verify. Frozen stays 21 matches; expanded stays 8 matches / 121 Unsupported / zero errors, with six explicit first-refusal changes per pin. Exact packaged all/unsignedLong examples pass; advisory checks enabled after network retry. Local unsigned alpha.0.35 only; beta/pins/controllers unchanged. Next: explicit Git delivery/review and merge/main verification, then re-rank remaining specification coverage. No commit/push/publication.


### 24 November 2026 - all groups merged; specification priorities reassessed

PR44 merged as `b08187d5f59f47a068737154ecdd6872506d3a88`; main CI45382084851 passes 3,885 source tests and supporting checks. The [post-particle coverage reassessment](../post-particle-coverage/README.md) reuses the qualified 23 November sample: 8 matches / 121 Unsupported / zero errors, with no new corpus run or coverage percentage. Next recommended contract is bounded integer enumeration, followed by date/time transport design and wider Unicode regex support; schema assembly and implicit anyType remain separate dependencies. Source/test/build inputs, API, Core pins and packages are unchanged. Earlier matrices, rankings and seals remain historical; no new semantic adoption, commit/push or publication.


### 24 November 2026 - integer-enumeration contract prepared

The [bounded integer-enumeration contract](../integer-enumeration/README.md) proposes value-space membership for existing integer families and placements, with 105 unexecuted XML cases and 18 native gates. Exact specification/source pins distinguish base-type entry validity from sibling intersections, preserve inheritance and define capacities. Static expectation impacts and a retained original-fixture Invalid-only harness assumption are recorded for measured reconciliation; upstream validities and payloads stay unchanged. No semantic adoption or product/API/Core/package change. Next: isolated native feasibility, then separate production adoption and qualification. No commit/push/publication.


### 24 November 2026 - isolated integer-enumeration feasibility

The [isolated candidate](../integer-enumeration/FEASIBILITY.md) passes 261 controls per signed Core pin and 210 unchanged-baseline refusals per pin. One authored attribute-only root is retained as Unsupported; IE-073Q explicitly qualifies the supported attribute placement without widening grammar. Initial source regression has 10 measured expectation failures; scratch-only expectation/upstream-validity reconciliation passes all 3,885 prior test names with no removal or rename. Two original schemas compile Valid and their instances match preserved upstream valid/invalid outcomes. API 54/401 has zero delta and 18 signed Core identities verify. Production source/tests and earlier seals remain unchanged. Next: separate production adoption and full qualification; no commit/push/publication.


### 24 November 2026 - integer-enumeration production qualification

The [bounded integer-enumeration implementation](../integer-enumeration/IMPLEMENTATION.md) passes 4,146 source tests (1,300 internal / 2,846 public), 2,846 package consumers per Core pin, 49 packaged native controls and retained callers 124/23/33. All 3,885 prior source names remain; 261 controls are added and ten measured expectations plus the original-fixture harness assumption are explicitly reconciled. API54/401 zero delta; 18 signed Core identities and published beta.1 verify. Frozen stays 21 matches; expanded is now 10 matches /119 Unsupported /zero errors, with exactly two original integer-enumeration admissions per pin. Integer membership is bounded to existing types/placements; original IE-073 remains Unsupported and its qualified variant is separate. Local unsigned alpha.0.37 only; no publication, pin/controller change or full XSD/PSVI claim. Next: explicit Git delivery/review and merge verification, then separate date/time transport design.


### 24 November 2026 - PR45 merged; date/time transport proposal

PR45 merged as 2eb999052188a872a54548c71c378d6a05a8cc3c; main CI45407113398 passes 4,146 source tests and supporting checks. The [calendar representation contract](../date-time-transport/README.md) and proposed ADR018 recommend xs:date feasibility first, with exact year/month/day and optional timezone; dateTime/time need separate exact fractional-second transport review because both pinned Core carriers store milliseconds. There are 37 unexecuted root XML plans and 18 unexecuted native gates. Nine original calendar cases retain additional obligations; no admission forecast or new corpus execution. All 323 production inputs and earlier seals remain unchanged. No semantic adoption, public API, Core pin, source/test/package change or publication. Next: isolated package-only date transport feasibility and API review before native semantic adoption.


### 24 November 2026 - date transport preflight reaches boundary stop

The [package-only date preflight](../date-transport-preflight/README.md) measures32 component round-trips and28 expected formatter results per Core pin; all four long.MinValue/zone combinations throw caught OverflowException. Both actual Schema facades still refuse xs:date. No native facts, detached facade, lifetime or validation success is claimed. The original full-Int64 proposal is preserved; a separate bound revision proposes excluding long.MinValue before the next isolated native experiment. All18 native gates remain pending. Eighteen unchanged Core archives/runtime payloads and two fresh Core signatures verify; all323 production inputs and59 earlier packets stay unchanged. No production/API/pin/package change, semantic adoption, commit/push or publication.


### 24 November 2026 - revised native date feasibility

The [isolated date candidate](../native-date-feasibility/README.md) passes 99 uniquely labelled native controls and 4,147 scratch source cases per signed Core pin (1,301 internal / 2,846 public). All 4,146 original logical cases remain, with one explicit expected-outcome display rename and one newly eligible AL-008 identity case. Nine measured semantic failures are reconciled in isolated copies; the current lane additionally adjusts its hard-coded minimum-version assertion. The revised interval excludes long.MinValue; original sealed proposal and formatter failures remain intact. API preserves 401 rows and proposes seven additions (55 types / 408 rows). Eighteen signed Core payloads and two fresh Core signatures verify. All 323 production inputs and 60 prior seals stay unchanged. Native original-node facts, both public facades, ownership, cancellation, export and lifetime are measured; full packaged/retained/corpus qualification remains. Date complex simple content and temporal facets remain excluded. No production adoption, source/API/pin/package change, commit/push or publication. Next: separate production adoption and full qualification.


### 24 November 2026 - bounded native date production candidate

The [native date implementation](../native-date-implementation/README.md) passes 4,246 source tests (1,308 internal / 2,938 public), 2,938 package consumers per Core pin, seven packaged native identity controls and retained124/23/33. Original4,146 logical cases remain with one explicit display rename,99 new controls and one newly eligible original AL-008 identity case; measured expectations are reconciled explicitly. API preserves401rows and adds seven (55types/408rows);18signed Core identities and published beta.1 verify. Frozen stays21matches; expanded now12matches/117Unsupported/zeroerrors: two original date admissions and one unchanged wildcard refusal with changed first reason. Exact packaged date/enumeration/all/unsignedLong examples pass; normal advisory checks stay enabled. Year interval excludes only long.MinValue within the signed representation; temporal facets and dateTime/time remain separate. Local unsigned alpha.0.40 only. Earlier61packets stay intact; no publication or Corepin/controller change. Next: explicit Git delivery/review and merge verification, then updated coverage reassessment.


### 24 November 2026 - PR46 merged; calendar gaps reassessed

PR46 merged as 383914562d89d3f7fad3a6a60cf4e29884cce912; main CI45426915018 passes 4,246 source tests. The [post-date reassessment](../post-date-coverage/README.md) reuses the qualified sample of 12 matches / 117 Unsupported / zero errors, with no new execution. Nine calendar cases split into two delivered date matches, two date simple-content extensions, two mixed-content cases, two enumerations and one bound. Recommended next contract is bounded date simple-content extension with accurate complex-governing/atomic-date identity and an explicit guard against decimal restriction fallback. Date equality/facets and exact-seconds Core transport remain distinct follow-ups. All 328 production inputs and 62 earlier packets remain unchanged. No semantic adoption, API/source/test/pin/package change, commit/push or publication.


### 24 November 2026 - date simple-content contract prepared

The [bounded date simple-content contract](../date-simple-content/README.md) defines named/anonymous roots and terminals, existing attribute uses, distinct complex G versus atomic date A, retained capacities and explicit derivation exclusions. It contains 77 unexecuted XML proposals and 18 native gates plus two unchanged original trials, without an admission forecast. Ordinary extra attributes are Invalid within this proposed date-extension closure even with zero uses; older profile behavior stays unchanged. Date complex restriction must be refused before decimal fallback. Static expectation impacts are recorded; no source/native/corpus execution or semantic adoption occurs. All 328 production inputs and 63 prior packets remain unchanged. Next: separate experimental adoption and isolated native feasibility on both signed Core pins. No API/pin/package change, commit/push or publication.


### 24 November 2026 - date simple-content isolated feasibility

The [isolated date extension candidate](../date-simple-content-feasibility/README.md) passes 196 native controls per signed Core pin, including all 77 authored payloads on both facades, actual G/atomic-A identities and both unchanged original upstream trials matching their validities. All 4,246 original source names pass after one measured date-extension expectation correction in scratch copies; the current lane also adjusts its known minimum-version assertion. API remains55types/408rows with zero delta;18signed Core payloads and two fresh Core signatures verify. Three isolated source overlays implement date extension, scoped ordinary attribute closure and the date-restriction fallback guard. An initial nested-operation diagnostic error was corrected without source bypass; all receipts remain. All 328 production inputs and64 earlier packets remain unchanged. No production semantic adoption, source/API/pin/package change or full corpus/package/retained run. Next: separate production adoption and full qualification. No commit/push/publication.


### 24 November 2026 - date simple-content production candidate

The [date extension implementation](../date-simple-content-implementation/README.md) passes 4,442 source tests (1,345 internal / 3,097 public), 3,097 package consumers per Core pin, 37 packaged identity controls and retained124/23/33. All4,246 prior source names/stimuli remain;196 controls are added, and the single measured factory refusal is explicitly reconciled. Actual complex G versus atomic date A, scoped closed attributes including zero uses and date-restriction fallback refusal are qualified. API55types/408rows has zero delta;18signed Core identities and published beta.1 verify. Frozen stays21matches; expanded now14matches/115Unsupported/zeroerrors, with exactlytwo original date-extension admissions per pin and no other row changes. Five exact packaged examples pass after a stale README exclusion was corrected without changing the DLL. Local unsigned alpha.0.43 only;65earlier packets intact. No publication/Corepin/controller change. Next: explicit Git delivery/review and merge verification, then separate date-enumeration/equality work.
