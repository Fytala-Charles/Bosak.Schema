# Shared specification coverage programme

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
