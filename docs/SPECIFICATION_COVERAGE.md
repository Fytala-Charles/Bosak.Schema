# XSD1.1 implementation coverage

Updated27November2026. [Current28-area matrix](simple-type-final/CURRENT-MATRIX.json); earlier dossiers remain historical.

Development 0.33 qualifies explicit final controls on global named simple types. Restriction, list, union and simple-content extension edges resolve source-bound names and reject prohibited methods before a compiled schema is published. All 24,313 source tests (1,846 internal /22,467 public) and22,467 overlapping exact-package checks pass. All24,147 prior names and original inputs remain unchanged;166 checks added. Pilot21; structural59/70; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; broader derivation, finalDefault, complex-type final and full XSD1.1/PSVI/C45 remain separate.

Explicit global simple-type final controls now constrain direct restriction/list/union/extension edges. finalDefault, complex final, general complex derivation, nested composition, ancestor projection, element/open-content wildcards, alternatives/assertions, augmentation, general identity constraints and public Core integration remain gaps. Samples are not exhaustive; no overall percentage justified.
