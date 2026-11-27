# XSD1.1 implementation coverage

Updated27November2026. [Current28-area matrix](skip-empty-attributes/CURRENT-MATRIX.json); earlier dossiers remain historical.

Development 0.29 qualifies skip attribute wildcards on empty complex owners. Skipped extras have no node declaration, anySimpleType node types and original untypedAtomic values, even when globals exist. Roots with skipped extras project to canonical anyType; no-extra roots retain original empty typing. Explicit uses retain validation. All 23,649 source tests (1,802 internal /21,847 public) and21,847 overlapping package checks pass. All23,485 prior names remain;164 checks are added. Pilot21; structural47/82; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads are unchanged. Unpublished; broader projection, full XSD1.1, PSVI and C45 acceptance remain separate.

Prior strict owners and lax empty owners remain qualified. Skip now preserves raw extras and canonical root projection on empty owners. Unvalidated extras beneath strict ancestors, broader lax/skip owners, element/open-content wildcards, general inheritance, alternatives/assertions, augmentation, general identity constraints and public Core integration remain gaps. Frozen samples are not exhaustive; no overall percentage is justified.
