# XSD 1.1 implementation coverage

Updated 26 November 2026. [Current 28-area matrix](absent-lax/CURRENT-MATRIX.json). Historical dossiers are preserved.

Development 0.21 qualifies declaration-absent lax elements and application attributes beneath uninterrupted anyType ancestor chains. All 22,426 source tests (1,711 internal / 20,715 public) and 20,715 overlapping fresh package consumers pass. All 22,346 prior names remain; 80 controls are added. Pilot 21 matches; structural 41 matches / 88 Unsupported; datatype 582 matches / 16 Unsupported; zero errors. API 55 types / 663 reflection rows and nine signed Core 1.86.0 payloads are unchanged. This candidate is unpublished; broader wildcards, partial-type projection and full XSD 1.1/PSVI/C45 remain separate.

Unknown lax nodes now carry absent declarations without synthetic components. Non-anyType complex ancestors remain excluded pending partial-assessment projection. Explicit wildcard modes/namespaces/notQName/UPA, complex inheritance, type alternatives, broader assertions, augmentation, identity constraints and public Core integration remain gaps. Frozen samples are not a specification denominator; no overall coverage percentage is justified. Next: rank these gaps from existing public cases.
