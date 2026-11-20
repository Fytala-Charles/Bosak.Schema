# Owner-aware nested attribute development candidate

20 November 2026. Local implementation on main3e067cd, not a new publication.
The [before-code adoption](ADOPTION.md) extends existing local attributes to nested
named/anonymous element-only containers. Root attribute/scalar indexes remain
unchanged. [Contract](CONTRACT.md), [surface](SURFACE.json),
[proposed ADR014](../ADR-014-owned-descendant-attribute-paths.md),
[pre-code feasibility](FEASIBILITY.md) and
[packaged example](../NUGET_README.md#development-owner-aware-nested-attributes).

Attribute facts bind to the exact captured owner index, immutable element-child
path, owner expanded name and original attribute node. All facts publish together
only after complete assessment. Root and descendant projections remain separate,
with owner-preorder/schema-use-order descendant reads and omitted optional uses.
Three additive members expose detached data without raw nodes/authority.
Root paths are empty/nondefault; descendant paths identify present XML, not schema
or scalar indexes and not cross-document identity.

A fixed64potential attribute-use budget includes root, repeated and optional
branches before assessment. Existing sixteen-use and nested limits remain.
Scalar-leaf attributes, nested simple content, assertions and broader XSD
composition/derivation/default/mixed/particle features remain outside scope.

## Intentional former-refusal transitions

[Recorded deltas](INTENTIONAL-DELTAS.json) identify exactly NS-049 and NS-075 from
the delivered nested catalog. Their optional nested attribute definitions now
compile/assess Valid, including the unused definition's eager check. The original
sealed receipts remain unchanged; initial new-source results confirm these two
expected changes and all1,123other prior outcomes unchanged. Current authored
catalog expectations are updated explicitly, not counted as new feature cases.

## Current source qualification

1,200source cases pass (404implementation/796public). New62authored attribute
cases cover repeated/shared/anonymous owners, present-element paths, namespaces,
existing scalar families/facets and capacity/Invalid/refusal behavior. Four public
operation controls and nine actual provider/lifecycle controls add75source cases.
Compatibility/API/README and independent controls are recorded separately after
qualification; source passes alone do not establish those lanes. No official
W3C-suite, general API/C45 acceptance, production SLA or published release claim.


## Completed local compatibility qualification

[Exact results](QUALIFICATION-RESULTS.json) record package-only796minimum-Core
cases and795unchanged current-reference Core1.81.17cases plus one current-version
identity control. The original minimum-only1.81.2pin assertion stays in its
minimum lane. Retained124/23/33binary catalogs pass with onlySchemaDLLchanged;
18signed Core dependency/source/payload identities verify. Catalogs overlap and
are not additive conformance totals. All52public types/389prior rows remain,
with exactly three adopted additions/392rows. Eleven actual packaged C#examples,
54release controls, six workload controls and workload consumer build pass.

The unsigned local0.6.0-alpha.0.14 package's repository metadata identifies only
base3e067cd; actual dirty source hashes identify the qualified implementation.
[Regression comparison](implementation/REGRESSION-DELTA.json) keeps two intentional
former-refusal changes separate from1,123unchanged prior outcomes. NS-075's TRX
display truncation hides the changed payload; the explicit ID/original-row hash
ledger and initial two-failure receipt preserve the semantic transition.
No registry/tag/controller dispatch, Core engine/pin change or new acceptance.
