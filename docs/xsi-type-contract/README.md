# Bounded xsi:type contract

20 November 2026. **Contract and static review only; no production implementation.**
Base merged PR33/main b1759d22761587b2abe507d12aacf3f5b6b33535; worktree
Schema.XsiTypeContract, branch planning/bounded-xsi-type. No commit/push requested.

The proposed first slice accepts same-type requests and existing direct scalar
restrictions within the same family. It resolves QNames in the owning instance,
validates component derivation, applies the selected facets and retains the
original declaration separately from the effective governing type. Known valid
cross-family/simple-to-complex derivations are explicitly deferred as Unsupported.

- [Contract and intentional exclusions](CONTRACT.md).
- [Pinned W3C sources/sections](SPECIFICATION-REVIEW.json),
  [complete51-name inventory](BUILTIN-NAMES-REVIEW.json),
  [published Core contract mapping](CORE-REVIEW.json).
- [Native source findings](SOURCE-REVIEW.json) and
  [proposed qualification/case plan](QUALIFICATION.md).
- [Four frozen original case identities](PILOT-REVIEW.json): all are invalid QName
  or missing-type requests; they do not establish useful positive override support.

Static review identifies two required model changes: explicit base-type relations
and separate effective-type state. The existing provider currently uses the
original declaration type for all facts/annotations. A native feasibility probe
must prove the proposed D/T/G mapping on both pinned Core lanes before adoption
and production edits. No new public API or Core dependency change is proposed.

Current merged qualification remains1,886source cases, with frozen pilot10matches /
11Unsupported / zero mismatch/harness errors. No new product tests or upstream
measurements were executed for this plan. No blanket xsi:type refusal or ignored
attribute is proposed as a substitute for real type selection. Legacy decimal,
Core C45/general XPath/full-XSD/PSVI and publication boundaries remain unchanged.
