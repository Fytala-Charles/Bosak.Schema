# XSD1.1 implementation coverage

Updated27November2026. [Current28-area matrix](choice-anytype-terminals/CURRENT-MATRIX.json); earlier dossiers remain historical.

Development 0.31 qualifies direct text-only implicit/explicit anyType alternatives in bounded choice roots and named aliases. One selected expanded-name branch may repeat within existing occurrence bounds; mixed alternatives remain Invalid. Original declarations/canonical anyType and actual untypedAtomic text, including empty strings, retain physical order. All23,953 source tests(1,826internal/22,127public) and22,127 overlapping package checks pass. All23,793 prior names remain;160 checks added. Pilot21; structural53/76; datatype582/16; zero errors. API55/663 and nine signed Core1.86.0 payloads unchanged. Unpublished; broader composition/projection and fullXSD1.1/PSVI/C45 remain separate.

All/choice roots now admit direct text-only anyType members with original governing/actual evidence. Nested compositor containers, complex global references, terminal attributes/descendants, general ancestor projection, element/open-content wildcards, inheritance, alternatives/assertions, augmentation, general identity constraints and public Core integration remain gaps. Samples are not exhaustive; no overall percentage justified.
