# Schema beta.2 gap assessment

19 November 2026. Assessment of public main `ec25a98da920348a89e0cff96c3af33e921f651c`
and published **0.5.0-beta.1**. [Exact public inputs](BASIS.json).

**Recommendation: retain beta.1's grammar and propose a small improvement to
Unsupported diagnostics before choosing a grammar extension.** This is an
engineering-priority proposal, not evidence of customer demand or an accepted
beta.2 scope. No new implementation, API, version, dependency or release change.

## Evidence and limits

The published invoice trial passed exact decimal reads, detached XML, a supported
assertion violation, namespace/attribute refusal, quota reuse, disposal and
pre-cancel recovery. It reported no engine defect. Its public provenance and
executed fixture hashes remain in the original immutable receipt.

The public consumer catalog already exercises the distinct `native-decimal`
order/tax example. That is test coverage, not an independently observed real
application deployment. Reusing the invoice in more than one consumer does not
provide a new schema shape. Private consumer material is not included here.

The following is **static comparison with the documented native grammar**;
orders/sku were not freshly executed against the package in this assessment.
Legacy descriptor tests using these files do not establish native assessment.

| Public input | What the native workflow can use | Native gaps visible in the schema |
|---|---|---|
| Published invoice | Recorded package-only assess/read/export pass | Deliberately added namespace/attribute variants refuse; no defect established |
| `native-decimal.xsd` + XML | Existing public tests: bounded repeated amount, tax and count assertion | No new requirement established; already-covered native profile |
| `sku.xsd` | Legacy example only for this assessment | Target namespace, named simple type/global type reference, string values and pattern facets |
| `orders.xsd` | Legacy example only for this assessment | Target namespace; multiple global/named declarations; nested/string/pattern content; unbounded repetition; required attributes; decimal simple content; mixed content |

No single namespace, attribute or datatype extension makes the complete orders
fixture supported. Accepting one construct while ignoring the others would
violate the complete-assessment boundary. These files are public authored
examples, not owner-selected production requirements.

## Candidate priorities

| Candidate | Evidence-based benefit | Cost/boundary | Disposition |
|---|---|---|---|
| Fix a beta.1 defect | No confirmed new defect in retained consumer evidence | Requires a minimal supported-input reproduction | No defect patch currently justified |
| Explain Unsupported more precisely | Concrete complex examples contain several independent exclusions; the public API currently supplies only stage summaries | Small diagnostic contract extension; must keep messages bounded and input-free | First proposed bounded slice |
| Namespace-aware decimal profile | Namespaces occur in both legacy examples | QName identity, schema/instance matching and assertion context need adoption and controls; alone unlocks neither full example | Candidate after a concrete workflow is selected |
| Attributes or string/pattern types | Explicitly needed by sku/orders | New typed values, structural rules and API/value projection decisions | Separate design; not combined into a nominal beta patch |
| Full orders schema or general XSD 1.1 | Would cover much broader shapes | Multiple independent semantics, provenance and ownership requirements | Outside the first beta extension |

The ranking uses scope and explainability, not measured user demand. No selected
application XML/schema workflow or explicit business priority was available.

## Proposed next slice: bounded Unsupported reasons

Existing stable stage codes and outcome/ownership behavior must remain intact.
The future proposal should add an optional bounded reason category for a
detected refusal (for example namespace use, declaration shape, value type,
attribute use, assertion grammar or representation capacity). Public consumers
must tolerate an unspecified reason. Do not expose internal exception text,
source fragments, names, URIs, full paths or parser diagnostics.

Report a reason actually detected, not an exhaustive compatibility certificate.
Multiple unsupported constructs need not all be enumerated. No line/column or
remediation hint should be promised without a separate bounded contract.

Before implementing, review the additive API shape, exact precedence/compatibility
rules, security/diagnostic basis and any required immutable specification
adoption. The existing semantic adoption records remain unchanged; this
assessment adopts no new XSD behavior or future diagnostic contract.

Proposed acceptance controls:

1. Existing valid invoice/order inputs retain Valid, owned values and no diagnostics.
2. Namespace and attribute refusals retain Unsupported, null owned values and
   existing stable stage codes, with the proposed detected categories.
3. Distinguish representation-capacity refusal from missing grammar when known;
   supported invalid values/assertions stay Invalid, not Unsupported.
4. Combined unsupported constructs expose no owned result and make no claim
   that all gaps were enumerated; messages remain fixed and bounded.
5. Arguments, cancellation, overlap, quota and closed access retain their documented
   exception semantics. Diagnostic reporting must not retain input/authority.
6. An external package-only consumer verifies the additive shape and old stage-code
   behavior. Run full source, compatibility and release controls appropriate to
   the resulting change before any beta.2 package proposal.

If an owner supplies a concrete workflow that changes priorities, first record
its schema/instance provenance, required outputs, expected Invalid/Unsupported
cases, capacities and ownership. Then choose one coherent supported shape rather
than infer demand from these examples. A public example may be selected, but it
must continue to be labelled representative rather than production acceptance.

## Verification and delivery

Documentation/static analysis only. Historical 345 source cases and the retained
33-case package consumer catalog are not new test executions and are not additive.
The original release and consumer dossiers remain unchanged. No fresh package
diagnostic, benchmark, conformance run or broader acceptance is claimed.

Worktree `Schema.Beta2GapAssessment`, branch `review/schema-beta2-gap-assessment`.
No commit/push or publication is performed. Beta.1 remains the current release.
