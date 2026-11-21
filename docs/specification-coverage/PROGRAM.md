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
