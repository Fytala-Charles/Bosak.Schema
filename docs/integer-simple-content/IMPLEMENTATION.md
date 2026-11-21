# Bounded integer simple-content implementation

21 November 2026. Candidate based on the verified PR36 merge
`ab7e20afacd83edf93e71be2123fb40235c4f646`. [Production semantic adoption](ADOPTION.md)
was recorded after203native controls per signed Core pin and before production
edits. Original proposal and feasibility packets remain sealed historical evidence.

Three production source files now admit attribute/assertion-free complex extension
of primitive xs:integer or an existing named direct integer simple restriction,
followed by one direct complex restriction. Integer content stays BigInteger;
G->complex B and anonymous integer C1->actual C0 remain separate original compiled
components. Static inherited facet validity precedes instance enforcement, with
fixed fractionDigits=0, valid-empty integer ranges and existing capacities.
Named/anonymous root and existing nested terminal routes use the existing APIs.

## Qualification

| Lane | Passed |
|---|---|
| Full source | 2,522 (734 implementation / 1,788 public) |
| Package-only Core1.81.2 | 1,788 |
| Package-only Core1.81.17 | 1,788; existing scratch-only loaded-version expectation overlay |
| Current-package native metadata/static controls | 83; overlaps source |
| Retained callers, only Schema.dll replaced | 124 / 23 / 33 |
| Public API | 54 types / 401 rows, zero delta |
| Public dependency and baseline identity | 18 Core signatures/source/cache/runtime payloads plus signed beta.1 verified |
| Unchanged frozen corpus | 17 matches / 4 Unsupported / zero mismatches or harness errors |
| Frozen runner integrity controls | 15 |
| Exact packaged README XML example | Large BigInteger value and fractional lexical refusal pass |

All2,319prior stable source identities remain;203new cases are83internal plus120
public. Test lanes overlap and are not additive conformance coverage. The current-
Core public lane uses only the existing copied test's expected loaded-version
change, not a product/test authority workaround. Package-only lanes contain no
project references. Current native controls use the existing friend assembly for
actual original component evidence; public consumers have no internal access.

Two authored reconciliations are explicit in [ADMISSIONS.json](ADMISSIONS.json):
SC-053 retains Unsupported with Attributes rather than ValueType; CR-042 now
compiles but rejects its unchanged4.5instance as invalid integer lexical text
through both facades. Original schema/instance/ID stay unchanged; no upstream
expectation or corpus file changed. No conformance classification changes occurred.
The remaining assertion-family cases still have independent unsupported features.

[Results](implementation-evidence/RESULTS.json),
[stable identities](implementation-evidence/STABLE-CASES.json),
[API](implementation-evidence/SURFACE.json),
[archive/runtime signatures](implementation-evidence/IDENTITY-FINAL.json),
[pilot comparison](implementation-evidence/PILOT-DELTA.json).
TRX, input projects, exact candidate source/fixture snapshots, source/runtime
identity receipts and signed dependency logs are preserved separately from prior
sealed evidence. Binary caches/packages remain outside public documentation.

Local unsigned0.6.0-alpha.0.27 is qualification only. Its README bytes match the
reconciled consumer guide; published0.5.0-beta.1, Core pins and release controllers
remain unchanged. No tag, commit, Git push or registry publication is performed.

## Review and remaining scope

Self-review has no blocking findings within the adopted scope. It covers static
bound/digit legality versus runtime conjunction, primitive and named integer
content, exact atomic kind/value, original D/T/G/B/C0/C1 provenance, empty/no-facet
identity, all resolver call sites, eager cycles/chains, actual data/typed-identity
profiles, resource and ownership guards, legacy routes and explicit fixture deltas.
The source build and all qualification lanes passed without candidate failures.

Integer complex types remain attribute/assertion-free. Longer complex chains,
derived built-in integer families, inline simpleType, general inheritance and
broader complex xsi:type substitution remain excluded. Data-profile ReadTypedValue
refusal stays separate from the independently proven typed-identity profile.
No full-XSD/PSVI/C45 or performance acceptance is claimed.

Next: review and deliver this qualified candidate on an explicit Git request.
Then assess context-item/inherited assertion semantics using exact normative
adoption and real original-node evidence. Integer support alone does not complete
the original assert006 schemas. Do not patch or forecast individual conformance
cases in place of a coherent assertion contract.
