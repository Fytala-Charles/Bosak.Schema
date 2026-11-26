# XSD 1.1 implementation coverage

Updated 26 November 2026. [Current 28-area matrix](strict-attributes/CURRENT-MATRIX.json). Historical dossiers remain preserved.

Development 0.22 qualifies bounded strict attribute wildcards on empty complex owners. All 22,570 source tests (1,719 internal / 20,851 public) and 20,851 overlapping fresh package consumers pass. All 22,426 prior names remain; 144 controls are added. Pilot 21 matches; structural 43 matches / 86 Unsupported; datatype 582 matches / 16 Unsupported; zero errors. API 55 types / 663 rows and nine signed Core 1.86.0 payloads are unchanged. Unpublished; broader wildcard modes, negative constraints, inheritance and full XSD1.1/PSVI/C45 remain separate.

Strict attribute wildcards on empty owners now enforce bounded namespace constraints and require actual global declarations. AnyType lax unknown content remains bounded to anyType ancestor chains. Remaining gaps include nonempty owner wildcards, lax/skip and partial type projection, negative constraints, inheritance, type alternatives, broader assertions, augmentation, general identity constraints and public Core integration. Frozen samples are not an exhaustive specification denominator; no overall percentage is justified. Next: evidence-based gap review before further semantic adoption.
