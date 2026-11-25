# Batch completion report

Nineteen bounded implementation slices are merged and main CI verified. Item 20 is the queued shared-calendar prerequisite assessment, with a handled diagnostic and proposal rather than a production semantic change. No package was published. The final delivery receipt records the assessment PR and its main CI after merge.

| Item | Scope | PR | Verified main revision | Main CI |
|---|---|---|---|---|
| 1 | Date bounds and explicitTimezone | [#49](https://github.com/Fytala-Charles/Bosak.Schema/pull/49) | `4213eef605d83308f1a23c841a5aa64daa8757f5` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45484956845) |
| 2 | Named date restriction chains | [#50](https://github.com/Fytala-Charles/Bosak.Schema/pull/50) | `3aedb17868829398945e6d55de05fd7cffb9d85f` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45488937953) |
| 3 | Date complex simple-content restriction | [#51](https://github.com/Fytala-Charles/Bosak.Schema/pull/51) | `10be217780bd28e94a57a3dfb6e635a052efedb0` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45493219579) |
| 4 | Date lexical patterns | [#52](https://github.com/Fytala-Charles/Bosak.Schema/pull/52) | `b7526ab3a900048057e8137e595de9000f781292` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45496820417) |
| 5 | Direct Boolean patterns and fixed-collapse whitespace (enumeration is inapplicable) | [#53](https://github.com/Fytala-Charles/Bosak.Schema/pull/53) | `83f3569306debb157f5815bd687f44234ea06b8b` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45499527530) |
| 6 | Named Boolean restriction chains | [#54](https://github.com/Fytala-Charles/Bosak.Schema/pull/54) | `6113f77b464e6f9fe62e235c7211c0e5176b02a2` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45501361314) |
| 7 | Decimal enumeration | [#55](https://github.com/Fytala-Charles/Bosak.Schema/pull/55) | `402bc57b9b17d9dd06347ad0fff9a11d08455e71` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45503779928) |
| 8 | Named decimal restriction chains | [#56](https://github.com/Fytala-Charles/Bosak.Schema/pull/56) | `8e492823f1ecbc2ef09f9ec304127f56bdebbad3` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45505693358) |
| 9 | Decimal enumeration in complex restriction | [#57](https://github.com/Fytala-Charles/Bosak.Schema/pull/57) | `2825b8b81ed3ec8f466b488e452cf8c0667f5187` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45508186472) |
| 10 | Global attribute references | [#58](https://github.com/Fytala-Charles/Bosak.Schema/pull/58) | `9f48b79521c2da64eecd1f85f3cd1b6d1b21d48b` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45511937675) |
| 11 | Attribute groups | [#59](https://github.com/Fytala-Charles/Bosak.Schema/pull/59) | `d4651e5e1aa0ec9428655da6de0a2d119008dc7a` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45516353791) |
| 12 | Global element references | [#60](https://github.com/Fytala-Charles/Bosak.Schema/pull/60) | `893bb92355f5c6e75d0a6181b363d2e394f1ebb0` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45519099251) |
| 13 | Named model groups | [#61](https://github.com/Fytala-Charles/Bosak.Schema/pull/61) | `fb87dd63e4059baaa925e349d65aac63a2264cde` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45520923896) |
| 14 | Closed schema include | [#62](https://github.com/Fytala-Charles/Bosak.Schema/pull/62) | `d16b5541c67ac4f197c85be788f98d41669f044d` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45523145744) |
| 15 | Closed namespace import | [#63](https://github.com/Fytala-Charles/Bosak.Schema/pull/63) | `f861575ac1ca9a130fac42e383aabe157f9dfb9e` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45524908190) |
| 16 | Nillable element assessment | [#64](https://github.com/Fytala-Charles/Bosak.Schema/pull/64) | `495d60906946ec9333c652077a566161a03790fd` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45528223099) |
| 17 | Element default and fixed values | [#65](https://github.com/Fytala-Charles/Bosak.Schema/pull/65) | `28158f1d870968f20a140320fd096aa5a3c6e181` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45531232904) |
| 18 | Attribute default and fixed values | [#66](https://github.com/Fytala-Charles/Bosak.Schema/pull/66) | `5dffaaba6115656d3f64c42a5e6146c558e3cbdf` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45534086951) |
| 19 | Bounded mixed content | [#67](https://github.com/Fytala-Charles/Bosak.Schema/pull/67) | `522fae16c574b643dc65a875dfb42a0a7a04d31e` | [run](https://github.com/Fytala-Charles/Bosak.Schema/actions/runs/45537011236) |

Final implementation qualification: 10,248 source tests, 8,778 fresh-cache package tests, 125 native controls and 25 exact packaged examples pass. These lanes are not specification coverage percentages. Frozen pilot: 21 matches; expanded sample: 19 matches / 110 Unsupported / zero mismatches or harness errors. Nine signed Core 1.81.17 payloads remain identical. API preserves all 408 baseline rows and adds five reviewed rows (two closed-catalog factories and three nil/empty members).

Retained caller migration: legacy 124/124; filtered alpha 22/22 and beta 32/32. Full alpha 23 and beta 33 runs each retain one documented historical old-Core version assertion failure. These are Schema plus nine-Core/deps upgrades with unchanged caller binaries, not Schema-only swaps.

Support remains bounded. Closed includes/simple-type imports, sequence/attribute groups, global refs, scalar nil, provided default/fixed values and mixed sequences do not imply unrestricted XSD 1.1, default-node augmentation, complete imports, complex nil, full PSVI or C45 acceptance.

The 48 calendar observations are separate from product tests. Next recommended work requires a new Core contract and compatibility/conformance review. This capped batch starts no item 21.
