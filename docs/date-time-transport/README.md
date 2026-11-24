# Calendar values: representation and transport proposal

24 November 2026. Based on merged PR45, 2eb999052188a872a54548c71c378d6a05a8cc3c. Main CI45407113398 passes 4,146 source tests and supporting checks. This is a design contract only; no new semantic adoption or product implementation.

## Recommended sequence

First qualify **xs:date** through the native assessor and both owned public facades. Its year/month/day and optional timezone can be carried exactly by the currently pinned Core date representation within a signed 64-bit year bound. Then resolve exact fractional-second transport across Core, Schema facts and detached public values before adding xs:dateTime and xs:time. Treat xs:dateTimeStamp, temporal facets and calendar query operations as explicit later obligations.

The static [source review](SOURCE-REVIEW.json) verifies both immutable Core revisions: XPathDateTime stores milliseconds and has component-assignment constructors. Construction is not Schema lexical validation. Accepting higher-precision seconds by rounding, truncation, conversion through DateTimeOffset or string substitution would break exact typed-value transport. No Core source, package pin or API is changed here.

[Proposed ADR018](../ADR-018-native-calendar-value-transport.md) describes the additive public date shape and alternatives. [Contract](CONTRACT.md), [37 unexecuted XML plans](CASE-PLAN.json), [18 native qualification gates](NATIVE-GATES.json) and [nine unchanged original cases](ORIGINAL-EXAMPLES.json) make the next feasibility step reviewable. The native gates also cover placements, exact provenance, wrong-accessor behavior, cancellation, capacity, ownership and lifetime.

## Evidence boundaries

The [normative review](SPECIFICATION-REVIEW.json) pins exact XSD 1.1 Recommendation bodies and affected sections separately from future semantic adoption. The [source manifest](SOURCE-REVIEW.json) pins exact Git bytes for minimum Core1.81.2, current Core1.81.17 and merged Schema. The [engine input manifest](ENGINE-INPUTS.json) preserves all 323 production inputs. No authored plan has been executed and no new corpus run is claimed. Earlier seals remain intact.

The latest qualified sample remains 21 frozen matches and 10 expanded matches /119 Unsupported /zero errors, executed for integer enumeration. Six selected calendar content-type cases still need placement/content review; the other three need deferred date facets. Date-only support has no original-admission forecast. Unsupported is not a pass; neither these samples nor constructor availability establish a specification percentage, full XSD 1.1 conformance, PSVI/C45 acceptance or typed-query correctness.

Next: review the proposed API and run isolated date transport feasibility on both signed Core pins, then an exact native contract and separate semantic adoption before production changes. No commit, push, package build, publication or source/test change occurs in this step.
