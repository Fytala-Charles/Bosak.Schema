# Post-anonymous-family coverage review

28 November 2026. Product base `7d9c332ad834e13cf037becfa6325ce510646cdc`, the verified PR112 merge.

The post-anonymous-family review covers all 80 retained Unsupported cases across 65 distinct schemas. Fresh unchanged-binary checks reproduce 79 compile refusals and 1 assessment refusal, with zero harness errors. Product 0.47, API/Core pins and original fixtures remain unchanged. Full source 29,374 and overlapping package 27,263 counts are reused from PR112 qualification, not new suite runs. Next: bounded named-integer restriction chains, QName length-facet compatibility, then schema default-attribute groups before broader open content/composition. Samples are not a specification denominator; full XSD1.1/PSVI/public Core acceptance remains separate.

The 28-area matrix now distinguishes current boundaries from historical checkpoints and uses explicit revision-pinned evidence links. Sixteen primary groups partition the 80 cases; reviewed co-blockers overlap and are not exhaustive. The original full samples remain pilot 21 matches, structural 65 matches / 64 Unsupported and datatype 582 matches / 16 Unsupported, with zero prior errors. Only the 80 selected refusals were executed for this review.

- [Ranked work](RANKING.md) and [next contract proposal](NEXT-CONTRACT.md)
- [Method and limits](METHODOLOGY.md), [case inventory](CASE-INVENTORY.json), [blockers](BLOCKERS.json) and [catalog prerequisites](COMPOSITION-PREREQUISITES.json)
- [Current 28-area matrix](CURRENT-MATRIX.json), [evidence basis](EVIDENCE-BASIS.json) and [immutable review pins](REVIEW-PINS.json)
- [Fresh public refusals](PUBLIC-REFUSALS.json), [detailed diagnostics](DETAILED-REFUSALS.json), [setup correction](SETUP-CORRECTIONS.json) and [solo review](REVIEW.md)
- [Diagnostic runtime identities](RUNTIME-IDENTITIES.json), [source](Diagnostic.cs.txt) and [project](Diagnostic.csproj.txt)

Run `python docs/post-anonymous-coverage/validate.py` offline. This review adopts no new validation semantics, changes no product/test/build input and publishes no package. Earlier sealed dossiers retain their original claims, adoption dates, outcomes and hashes.
