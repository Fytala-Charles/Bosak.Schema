# Bounded root all-group production candidate

The opt-in native facades now assess a bounded root `xs:all`. Present scalar members may appear in any physical order while retaining their own original declarations, governing types, node paths and scalar slots. Optional-group/member rules and absent typed values remain distinct from omitted nodes and empty string values.

## Adoption and bounds

[Production adoption](PRODUCTION-ADOPTION.json) records immutable specification revisions, body hashes, affected sections and intentional exclusions before production source edits. The earlier [contract](README.md) and [native feasibility](FEASIBILITY.md) remain sealed historical checkpoints.

The outer all group has minOccurs 0 or 1 and maxOccurs 1. It has 1-16 direct explicitly scalar-typed members, each minOccurs 0 or 1 and maxOccurs 1. Expanded names determine attribution; duplicate competing declarations are Invalid within the recognized subset. Missing required members, repeated/unknown members and scalar lexical or facet failures are Invalid. Input and facts are never reordered to match declaration order.

An absent optional whole group allows empty content. Once any member is present, required members apply. A required group whose members are all optional can also be empty. Valid empty roots remain element-only, with HasNoTypedValue/FOTY0012, rather than empty-content complex types. A present empty string is one atomic; an omitted member has no node, facts or scalar slot.

Inline and named root types and both compiler routes work, including mixed catalogs with unrelated nested roots. Existing scalar restrictions, selected instance types, root attributes and structural namespace assertions retain their scope and combination limits. Root attribute reads keep declaration order and exact owner paths. All budgets sum member maxima; choice keeps maximum-alternative accounting. Definitions compile eagerly even when their members are omitted. Shared pattern work, no-partial-publication, cancellation before work, authority, export/reopen, detached scalar and disposal protections remain intact.

Nested all containers, complex members, group references, substitution groups, wildcards, implicit anyType, mixed/open content and broader occurrence/datatype/facet/type-chain grammar remain excluded. These are profile bounds, not general XSD 1.1 limits. Public API, Core pins and legacy decimal grammar are unchanged. No full PSVI/C45, general typed-query/component-closure or performance acceptance is claimed.

## Existing expectations and source coverage

All **3,739 previous test names and logical stimuli remain**, with 146 additional controls. [Eighteen measured compile expectations](ADMISSIONS.json) change explicitly: 17 previous all-refusal schemas now compile Valid and one duplicate-name schema is Invalid; five remain Unsupported. Original schemas, instances, IDs and member-data sources are unchanged. The historical AllRemainsExcluded method name is retained with a corrected summary to preserve exact test identity; the assertions use the previously authored all-phase outcomes and dispose newly successful sessions. No case is silently removed or renamed. [Stable identities](implementation-evidence/STABLE-CASES.json).

## Qualification

| Lane | Passed | Scope |
|---|---:|---|
| Source internal | 1,251 | Adds 60 original-node identity/boundary controls |
| Source public consumers | 2,634 | Adds 86 payload/facade controls |
| Package-only Core 1.81.2 | 2,634 | No source project references or friend access |
| Package-only Core 1.81.17 | 2,634 | Existing loaded-version expectation adjustment only |
| Current-package native | 60 | Original facts, governing types, optionality, budgets and lifetime |
| Retained callers | 124 / 23 / 33 | Only Schema.dll replaced |

The **3,885 source tests** pass. Release build/test has no compiler warnings or errors. The initially sandboxed restore could not reach NuGet advisory metadata; its receipt is preserved. Retrying with network access succeeds with normal advisory checks enabled. No repository warning or audit policy was weakened.

Public API remains **54 types / 401 rows**, zero delta. All **18 signed Core archive/source/cache/runtime identities**, signed published beta.1 baseline and unchanged retained non-Schema files verify. Source, local package, consumers, native diagnostics, API probe, packaged example and both corpus runners use the same Schema DLL. These overlapping lanes are not additive specification coverage. [Results](implementation-evidence/RESULTS.json), [identities](implementation-evidence/IDENTITY-FINAL.json), [API](implementation-evidence/SURFACE.json), [self-review](implementation-evidence/REVIEW.json).

The packaged README documents the bounds and includes an exact all schema/instance example. Both facades verify reordered integer/empty-string members, optional empty group, missing/duplicate-member refusals, export and detached lifetime. The existing unsignedLong example also passes. The example XML bytes match their packaged code blocks and the packaged README matches source. [Example identities](implementation-evidence/PACKAGED-EXAMPLES.json).

Local unsigned `Bosak.Schema.0.6.0-alpha.0.35.nupkg`:

- Package SHA-256: `eb2a50814e6d47d5ea5b2031903aed898a2c76a38eb19587e0a45028a893d9f4`
- Schema DLL SHA-256: `f05e0d9444cef03f842c02cdd8af4c8c0111d4f571979190e98dce6471373b96`

No registry publication occurred. Published beta.1, tags and release controllers remain unchanged.

## Measured original corpus effects

On both signed Core pins the frozen pilot stays **21 matches / 0 Unsupported / 0 errors**, with identical result rows and all 42 original files unchanged. [Frozen comparison](implementation-evidence/PILOT-DELTA.json).

The expanded 129 cases stay **8 matches / 121 Unsupported / 0 mismatches / 0 harness errors**. All 274 original files, metadata and expected outcomes remain unchanged. Six first-refusal reasons change per pin: mgB001, mgM002 and particlesHa017 now report RepresentationCapacity; mgB006, addB084 and mgA014 report ValueType, previously DeclarationShape. The latter three original schemas retain independent implicit-anyType co-blockers. No validity classification or other native row field changes. [Exact expanded comparison](implementation-evidence/EXPANDED-DELTA.json).

The unchanged measurement scripts retain their historical 22 November report date constant; these executions occurred on 23 November as recorded by the wrapper receipts. This selected sample is not the entire XSD suite or a conformance percentage.

## Delivery and next step

The all-group candidate and its documentation remain uncommitted. Next: explicit Git delivery and PR review, merge/main verification, then rank the remaining specification gaps using the existing public corpus and coverage inventory. Future grammar, datatype, schema-assembly and implicit-anyType work needs its own bounded contract and adoption evidence. Source pins and earlier sealed qualification remain unchanged. No commit, push, tag, controller preparation or publication occurred in this production step.
