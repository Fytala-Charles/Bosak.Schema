# XSD1.1 implementation coverage

Updated27November2026. [Current28-area matrix](abstract-elements/CURRENT-MATRIX.json); earlier dossiers remain historical.

Development 0.32 qualifies abstract Boolean flags on existing admitted global element declarations. Immutable canonical declaration identities enforce direct-use rejection at roots, references and known-lax matches before type/nil selection, without marking shared types or same-name locals abstract. All24,147 source tests(1,840internal/22,307public) and22,307 overlapping package checks pass. All23,953 prior names and original test inputs remain unchanged;194 checks added with no historical expectation changes. Pilot21; structural55/74; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; substitution groups, abstract complex types and fullXSD1.1/PSVI/C45 remain separate.

Abstract global declarations are now enforced independently of shared type identity. Substitution groups, abstract complex types, broader derivation controls, nested composition, general ancestor projection, element/open-content wildcards, alternatives/assertions, augmentation, general identity constraints and public Core integration remain gaps. Samples are not exhaustive; no overall percentage justified.
