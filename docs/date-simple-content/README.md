# Bounded date simple-content contract

24 November 2026; base383914562d89d3f7fad3a6a60cf4e29884cce912 (merged PR46). **Proposed, unexecuted and not semantically adopted.** The contract follows the [post-date assessment](../post-date-coverage/README.md); production source/tests, public APIs, Core pins and packages remain unchanged.

The proposed slice lets existing checked date values occupy simple-content complex roots and terminals with supported attribute metadata. It reuses NativeScalarKind.Date and NativeDateValue while preserving the complex governing type and the actual date content type as distinct identities. It adds no general complex inheritance, date facet, datetime or mixed-content capability.

[Detailed contract](CONTRACT.md), [77 unexecuted authored XML plans](CASE-PLAN.json), [18 native gates](NATIVE-GATES.json), [two unchanged original trials](ORIGINAL-EXAMPLES.json) and [potential existing-expectation impacts](POTENTIAL-EXPECTATION-IMPACTS.json) define the next isolated experiment. Proposed outcomes are review targets, not measured admissions. The original CType00201m schemas/instances and upstream expectations are unchanged; no forecast of two matches is made.

The contract explicitly closes ordinary attribute uses even when there are zero declarations, only for the new date-extension model. It also requires refusing date complex restriction before the decimal-specific fallback. Both differences need experimental adoption before source overlays and native controls on both compiler routes. Valid but unimplemented simple-to-complex xsi:type stays Unsupported; exact same complex type is admitted by the proposal.

[Immutable normative review](SPECIFICATION-REVIEW.json) and [exact source review](SOURCE-REVIEW.json) are separate from future semantic adoption. All328 production inputs and63 prior packets remain unchanged. The latest full selected sample remains12matches/117Unsupported/zeroerrors, reused from earlier date qualification; no new source/native/corpus execution occurs here. Documentation checks are programmatic, with no visual acceptance claim.

Next: record isolated experimental adoption, qualify the actual native candidate on signed Core1.81.2 and1.81.17, preserve initial regression failures and reconcile measured expectations explicitly. Separate production adoption and full package/retained/example/corpus qualification follow. No commit/push/publication.
