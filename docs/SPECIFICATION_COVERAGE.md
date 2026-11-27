# XSD 1.1 implementation coverage

Updated 27 November 2026. [Current 28-area matrix](simple-content-wildcards/CURRENT-MATRIX.json); earlier dossiers remain historical.

Development 0.27 qualifies strict attribute wildcards on existing noninteger simple-content extension owners. Original complex governing components and actual scalar values remain distinct, with independent global attribute validation. All 23,313 source tests (1,776 internal / 21,537 public) and 21,537 overlapping fresh package checks pass. All 23,171 prior test names remain; 142 checks are added. The frozen pilot has 21 matches; structural samples have 43 matches / 86 Unsupported; datatype samples have 582 matches / 16 Unsupported, with zero errors. API 55 types / 663 rows and nine signed Core 1.86.0 payloads are unchanged. Unpublished; full XSD 1.1, PSVI and C45 acceptance remain separate.

Strict attribute wildcards now cover empty owners, finite element-only/mixed sequences, bounded all/choice roots and admitted noninteger simple-content extensions. Remaining gaps include integer/Boolean extension attributes, restriction wildcards, nested all/choice, mixed all/choice, lax/skip and partial projection, element wildcards, general inheritance, alternatives/assertions, augmentation, general identity constraints and public Core integration. Frozen samples are not exhaustive; no overall percentage is justified.
