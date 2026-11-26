# XSD1.1 implementation coverage

Updated26November2026. [Current28-area matrix](group-wildcards/CURRENT-MATRIX.json); previous dossiers remain historical.

Development0.25 qualifies strict attribute wildcards on existing bounded nonempty all/choice root models and named-root aliases. Original particle validation, physical-order values and governing identities remain. All23,025source tests(1,752internal/21,273public) and21,273overlapping fresh package checks pass. All22,843prior names retained;182controls added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads unchanged. Unpublished; broader containers/modes/composition and fullXSD1.1/PSVI/C45 remain separate.

Strict attribute matching with qualified exclusions now covers empty/sequence owners and supported all/choice roots. Remaining gaps include nested all/choice/complex reference containers, mixed/simple-content owners, broader modes and partial projection, element wildcards, inheritance/composition, type alternatives, assertions, augmentation, general identity constraints and public Core integration. Frozen cohorts are not an exhaustive denominator; no overall percentage is justified.
