# XSD 1.1 implementation coverage

Updated 27 November 2026. [Current 28-area matrix](anonymous-gregorian-bases/CURRENT-MATRIX.json); earlier dossiers remain historical.

Development 0.43 qualifies bounded inline anonymous gYear, gYearMonth, gMonth, gMonthDay and gDay restriction bases. Original unnamed objects, direct base links and original facet sources preserve existing Gregorian component/value mapping, timezone equality/partial order and fixed/enumeration/bound/whitespace/pattern ancestry. All 27,209 source tests (2,002 internal / 25,207 public) and 25,207 overlapping exact-package checks pass. All 26,412 prior names and original test inputs remain; six public expectations are explicitly reconciled and 797 checks are added. Pilot 21; structural 65/64; datatype 582/16; zero errors. API 55/663 and nine signed Core 1.86.0 payloads are unchanged. Unpublished; other atomic/complex inline bases, broader derivation and full XSD 1.1/PSVI/C45 acceptance remain separate.

Other anonymous atomic/complex bases, broader annotation/PSVI, general complex/all-choice nil, broader overrides, finalDefault/complex final, general derivation, nested composition, ancestor projection, element/open-content wildcards, alternatives/assertions, augmentation, general identity constraints and public Core integration remain gaps. Samples are not exhaustive; no overall percentage is justified.
