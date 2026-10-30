# REQ-009 — Bosak.Schema owner execution packages

Registered: 30 October 2026. Accountable owner role: **Public Bosak.Schema maintainers**; individual scheduling/assignment belongs to that owner. Parent status remains **In Progress**. This registers scoped follow-up work and does not claim full XSD 1.1 conformance, C45/public API acceptance or publication.

Strategic outcome: full XSD 1.1 implementation on the shared Bosak foundation, with Schema and XmlDb jointly qualifying the core freeze surface. The bounded candidate remains a milestone toward that outcome, not completion. [Core REQ-140 plan](https://github.com/Fytala-Charles/Bosak/blob/checkpoint/restart-2026-10-29/docs/FOUNDATION-API-FREEZE-PLAN.md).

## Current basis and ownership

Candidate baseline: `7e498a1922dbda4ae14005acc4b13a049896c75a`, persistent feature/bounded-document-assessment worktree. Original public checkout remains separate at `044d2cb2e67decd46e03e0f5ee7cb03029f34bd9`. Native 0.9/v5 and its 451-case qualification are finite existing evidence; no new execution is supplied by this registration. The candidate uses current core 1.34.1 through the documented source override because its package dependency is unpublished.

Read [current assessment scope](DOCUMENT-ASSESSMENT.md), [consumer coverage priorities](CONSUMER-COVERAGE-PRIORITIES.md) and [proposed engine-session integration](BRAID-ENGINE-SESSION-INTEGRATION-DESIGN.md). Existing isolated consumer proofs remain historical finite evidence; they do not freeze a general schema/session API. Apache-2.0 schema assessment stays activation-free. Billing, issuance, key generation and private deployment material remain in Bosak.Licensing and are not public Schema work items.

## Owner queue

| Work package | Start condition | Concrete owner deliverable | Completion evidence |
|---|---|---|---|
| S009-01 Native structural demand and capability inventory | Ready now; no Braid production-validation or C45 dependency | Select public or owner-authorized schema/instance fixtures with expected typed-consumer operations. Deliver a matrix separating descriptor metadata, real native enforcement and explicit Unsupported coverage. Rank namespaces, named/anonymous types, nondecimal values, facets and component closure by demonstrated demand. Choose one finite next implementation slice rather than adding assertion grammar without a use case. | Fixture/source/permission hashes, immutable normative adoption for the selected slice, expected Valid/Invalid/Unsupported outcomes, exact supported bounds and happy/error/security/copy tests. Existing v1-v5 contracts retain their recorded behavior. |
| S009-02 Provider facts, provenance and core-adapter mapping | Internal mapping/proofs can start now; final production binding waits for accepted core C139-01 shape | Map provider-owned component/universe identity, affiliation/derivation/closure, original assessment/PSVI and actual typed values to the neutral requirements. Prepare compatibility and lifetime fixtures. After the core shape is accepted, implement an opt-in adapter for capabilities actually supported; explicitly refuse the remainder. | Real-provider tests for supported and foreign components, compatible views without rewriting original PSVI, original-source identity, preserve/strip/copies, cancellation/disposal and no unsupported downgrade to BCL XSD 1.0. Proposed core types or QName equality alone are not complete component authority. |
| S009-03 Bounded-candidate delivery and consumer qualification | Source/build/package preparation ready now against existing bounded APIs; package publication requires available core package and separate owner authorization | Produce exact source/core/package pins, public API delta, support matrix and package-only example/regression recipe for the bounded candidate. Refresh isolated consumer qualification against those exact artifacts, keeping original checkouts/pins unchanged. Keep this delivery separate from a general C45 adapter/API freeze. | Full Schema suite including retained 114 legacy binary expectations and current native regressions, exact input/archive/output identities, docs/examples, refused-coverage cases and no private licensing material. Compare recorded counts to actual execution; publication and each consumer adoption remain explicit subsequent decisions. |

| S009-04 Full-XSD-1.1 foundation requirements and joint freeze acceptance | Requirements work ready now; final sign-off follows agreed candidate and actual qualification | Deliver the full validator coverage/requirements roadmap and map all known core needs, including general component identity, derivation/list/union/typed value transport, assertions/alternatives and original PSVI. Close required foundation gaps with core REQ-140 and provide Schema owner sign-off alongside XmlDb. | Known required API needs mapped and representative actual provider cases qualified on the same candidate; no freeze from decimal-only/prototype evidence. Full backend conformance/release remains separately evidenced. |

## Acceptance checklist and constraints

- [ ] S009-01 identifies real demand and exact bounded behavior; metadata/descriptor recognition is not labeled structural enforcement.
- [ ] Each new semantic slice records immutable specification revision/sections/deviations before code and adds happy-path and edge regressions.
- [ ] S009-02 exposes only actual provider authority, preserving original assessment state and explicit Unsupported outcomes; the accepted core interface is identified before production adapter binding.
- [ ] S009-03 records actual complete tests, source/package/API evidence and unsupported scope without claiming full XSD 1.1 coverage or automatic Braid/XmlDb adoption.
- [ ] Source changes, old expectation migrations and review receipts are separated; all prior sealed dossiers remain unchanged.

Shared API freeze remains pending until both Schema and XmlDb owners qualify the named required contract. Schema backend features can continue within that stable scope, while unresolved required API needs must be closed before freeze.

The native assessment profile remains exactly the [documented 0.9 candidate](DOCUMENT-ASSESSMENT.md) until a selected extension is adopted and implemented. Do not infer list/union/general derivation, namespaces or broader structure from decimal count/presence examples. Do not add licensing checks or core-to-Schema production references.

## Core dependency and parallel execution

The matching [Bosak REQ-139 packages](https://github.com/Fytala-Charles/Bosak/blob/checkpoint/restart-2026-10-29/docs/REQ-139-OWNER-WORK-PACKAGES.md) require a minimal C45 disposition before permanent general contract implementation. Schema fixture/coverage work and provider-fact preparation proceed in parallel with that review and with Braid D02. This registration does not approve C45 or adopt a new Braid engine pin.

For current-source qualification, use the existing documented command with a pinned core source checkout, for example `dotnet test Bosak.Schema.slnx -c Release -p:BosakSourceRoot=D:/Development/Bosak`, and record the actual checkout revision. A local override is not published-package evidence. Owner handoff must state completed package IDs, exact commits/packages, executed results, remaining limits and required consumer action. Update [FEATURE_REQUESTS](FEATURE_REQUESTS.md) when those facts change.

## Progress

- **2026-10-30 — S009-01 executed (inventory complete, no slice selected):** [native coverage inventory](S009-01-NATIVE-COVERAGE-INVENTORY.md) maps the current descriptor/native/Unsupported coverage against the owner-queue demand dimensions and records zero demonstrated fixture demand. Selecting a next implementation slice without a real use case would violate the registration, so the outcome is the owner ask in §5 of the inventory: one public or owner-authorized schema+instance fixture with the expected typed-consumer operation. No behavior change.
- **2026-10-30 — S009-02 started (internal scope):** [provider-fact mapping](S009-02-PROVIDER-FACT-MAPPING.md) records the native 0.9/v5 candidate's actual provider facts against the seven neutral requirement dimensions, plus the F-01–F-06 compatibility/lifetime fixture inventory. Fixture inputs are now hash-pinned in [S009-02-FIXTURE-IDENTITIES.json](S009-02-FIXTURE-IDENTITIES.json) against the 451-case suite source at the recorded commit. Production adapter binding still waits for the accepted core C139-01 contract shape.
- **2026-10-30 — S009-04 started (requirements scope):** [foundation requirements roadmap](S009-04-FOUNDATION-REQUIREMENTS.md) maps needs N1–N8 to gap-matrix features, current evidence, required neutral core contract elements and freeze-blocker status, including the known-core-needs consume/supply map. Joint sign-off still waits for the agreed candidate, actual qualification and REQ-140 closure.

Both steps are documentation-only: no runtime behavior, test execution, API adoption or conformance is claimed.
