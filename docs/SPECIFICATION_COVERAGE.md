# XSD1.1 implementation coverage

Updated26November2026. [Current28-area matrix](sequence-wildcards/CURRENT-MATRIX.json); original dossiers remain historical.

Development0.24 qualifies strict attribute wildcards on finite, direct, nonempty element-only sequence owners, preserving original governing types/declarations and independent child/attribute validation. All22,843source tests(1,740internal/21,103public) and21,103overlapping fresh package consumers pass. All22,704prior names remain;139controls added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads unchanged. Unpublished; wider owners/modes/composition and fullXSD1.1/PSVI/C45 remain separate.

Bounded strict attribute wildcard matching now covers empty and direct finite element-only sequence owners, including previous namespace/QName exclusions. All/choice/mixed/simple-content owner extensions, broader wildcard modes and partial projection, element wildcards, inheritance/composition, type alternatives, assertions, augmentation, general identity constraints and public Core integration remain gaps. Frozen samples are not an exhaustive denominator; no overall percentage is justified.
