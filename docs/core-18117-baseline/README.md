# Core 1.81.17 development baseline

24 November 2026. Owner approved raising Schema development dependencies from 1.81.2 to the latest published Core 1.81.17 after PR #47 (357ae494f645d9d20d1790d0a15e20be290d13d7; merged cb81d80e42528a9ebca98366818309995a70bb87).

This dependency adoption introduces no new XSD semantics or normative specification adoption. Core 1.81.17 source pin: 6a7618e9dcf70eb8a67d5d1789ff7b787133b52d. Product API, Providers, XSLT and XQuery references and the direct test Providers reference select 1.81.17. The exact runtime assembly-version assertion also selects 1.81.17.

Future qualification uses a single current-Core lane. Historical two-pin dossiers, their manifests and published beta.1 remain unchanged. Old header references describe original implementation provenance. The beta.1 release controller intentionally continues to require its original 1.81.2 inputs; it is not a controller for this candidate. Consumers adopting a future package from this candidate must resolve Core 1.81.17 or later. No publication or tag change.

Validation: all 4,442 source cases pass (1,345 internal /3,097 public), and all 3,097 package-only public cases pass using a fresh cache with no explicit Core overrides. All nine transitive Core packages resolve to 1.81.17; see RESOLVED-CORE.json. Documentation hygiene and 32 Python /22 Node release controls pass. TRX files are retained here. Counts overlap and are not additive. The historical beta controller is unchanged.

The unsigned local package is a validation artifact only; its MinVer-derived version is not a release reservation. No package publication, tag, commit or push was performed. Earlier sealed dossiers are unchanged. Source test stimuli remain identical apart from the exact runtime-version assertion.
