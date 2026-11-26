# XSD1.1 implementation coverage

Updated26November2026. [Current28-area matrix](mixed-wildcards/CURRENT-MATRIX.json); earlier dossiers remain historical.

Development0.26 qualifies strict attribute wildcards on existing finite mixed sequence owners, retaining original mixed types/declarations and raw untypedAtomic aggregates independently from normalized child/attribute values. All23,171source tests(1,764internal/21,407public) and21,407overlapping fresh package checks pass. All23,025prior names retained;146controls added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads unchanged. Unpublished; wider models/modes/composition and fullXSD1.1/PSVI/C45 remain separate.

Strict attribute matching/exclusions now cover empty, finite element-only/mixed sequence owners and bounded all/choice roots. Remaining gaps include simple-content/mixed all/choice and empty mixed models, nested all/choice, broader modes/partial projection, element wildcards, inheritance/composition, type alternatives, assertions, augmentation, general identity constraints and public Core integration. Frozen samples are not an exhaustive denominator; no overall percentage is justified.
