# XSD1.1 implementation coverage

Updated26November2026. [Current28-area matrix](wildcard-exclusions/CURRENT-MATRIX.json); earlier dossiers are preserved.

Development 0.23 qualifies bounded notNamespace/notQName exclusions on strict empty-owner attribute wildcards, including source-bound QNames and ##defined. All 22,704 source tests (1,729 internal /20,975 public) and20,975 overlapping fresh package consumers pass. All22,570 prior names remain;134 controls are added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads are unchanged. Unpublished; wider owners/modes/composition and full XSD1.1/PSVI/C45 remain separate.

Negative namespace and QName matching is now native within the bounded strict empty-owner attribute profile. Wider nonempty/mixed owners, lax/skip and partial assessment projection, element wildcards, inheritance/composition, type alternatives, broader assertions, augmentation, identity constraints and public Core integration remain gaps. Frozen samples are not an exhaustive denominator; no overall specification percentage is justified.
