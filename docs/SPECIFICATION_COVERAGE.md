# XSD1.1 implementation coverage

Updated27November2026. [Current28-area matrix](lax-empty-attributes/CURRENT-MATRIX.json); earlier dossiers remain historical.

Development 0.28 qualifies lax attribute wildcards on empty complex owners. Known globals retain their declaration/type constraints. Undeclared attributes at an empty root carry no declaration, anySimpleType node types and untypedAtomic values; partial roots project to canonical anyType while original declared schema components remain. All 23,485 source tests (1,790 internal / 21,695 public) and 21,695 overlapping package checks pass. All 23,313 prior names remain, with 172 checks added. Pilot21; structural44/85; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads are unchanged. Unpublished; broader projection, full XSD1.1, PSVI and C45 acceptance remain separate.

Strict attribute wildcard owners remain qualified. Lax attributes now support known-only empty owners and bounded partial root projection for undeclared attributes. Unknown attributes below strict ancestors, skip, broader lax owner models, element/open-content wildcards, general inheritance, alternatives/assertions, augmentation, general identity constraints and public Core integration remain gaps. Frozen samples are not exhaustive; no overall percentage is justified.
