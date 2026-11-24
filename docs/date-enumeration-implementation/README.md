# Bounded native date enumeration: production candidate

24 November 2026. [Production adoption](PRODUCTION-ADOPTION.json) preceded implementation. The approved Core **1.81.17 minimum** applies to this development candidate. The original two-pin dossiers and signed published beta.1 remain historical and unchanged. This is an uncommitted local candidate, not a release or a full XSD1.1 conformance claim.

## Result and behavior

Direct named/anonymous xs:date restrictions now support up to16source enumeration entries at existing simple roots, scalar terminals, local attributes and date simple-content extension bases. Membership uses exact Gregorian date value equality, distinguishes absent from explicit timezones and preserves original local components/offsets. It changes no public equality/hash/ordering APIs. Named simple derivation chains, date complex restriction, other temporal facets, dateTime/time and full PSVI/C45 remain separate.

**4,874 source tests pass**:1,365internal /3,509public. Added432controls comprise412public cases (408contract/timezone/facade executions plus four schema-text guards) and20internal identity/equality/capacity/lifetime/original controls. All4,442original display names/payloads remain; exactly three original results have measured expectation changes. [Admissions](ADMISSIONS.json). The [isolated feasibility](../date-enumeration-feasibility/README.md) keeps its original candidate and failures; [production review](PRODUCTION-REVIEW.json) adds the shared restriction-text guard and explains the four new controls.

The final local package passes3,509public-only consumer cases without explicit Core overrides,20packaged native controls, all six exact packaged README examples and supplemental same-type xsi:type/distinct-member/normalized/source-byte boundary gates. Counts overlap. Public API remains55types/408rows, zero delta. Exact source/package/cache/runtime identities, all nine signed Core archives and source pin6a7618e9dcf70eb8a67d5d1789ff7b787133b52d are in [IDENTITY.json](IDENTITY.json). Local unsigned Bosak.Schema.0.6.0-alpha.0.44.nupkg SHA256 2bb1bb62d4b5b2517dcead13401796f5a0e875beebbec80c2a4f8a81b8abd7c8; Schema DLL SHA256 df707ddb6db3350ef82ba5737fb44a653225de7950b4a4bfd7a64a0bce71c828.

[Scoped20gate evidence](GATE-EVIDENCE.json) identifies new executions, inherited guards and the explicit retained-migration boundary. The [summary](SUMMARY.json), [exact329production inputs](PRODUCTION-INPUTS.json), [64qualified source files](CANDIDATE-SOURCE.json), [API comparison](SURFACE.json), [retained results](RETAINED-RESULTS.json) and [corpus rows](CORPUS-COMPARISON.json) support this checkpoint.

The frozen21pilot is unchanged. Expanded129is16matches /113Unsupported /zero mismatches or harness errors, with exactly two original enumeration cases changing; all other outcome rows agree with the prior candidate. Upstream IDs, schema/XML bytes, expected validities and attribution remain unchanged. Existing container comments are supported, while scalar comments remain refused; the original contract's overly broad comment inference is corrected by later measured evidence.

## Core minimum and retained callers

Raising the dependency minimum changes retained-consumer qualification. The old callers are not recompiled, but their runtime is migrated to Core1.81.17: Schema.dll, nine Core DLLs and dependency-graph version fields change. This is **not** a Schema-only replacement claim.

The original124-case caller passes. The full retained23- and33-case callers yield22/23and32/33passes: each remaining failure is exactly PublicSurface_NoFriendGrantOrPublicFactoriesForAuthority asserting Core1.81.2.0 rather than the new1.81.17.0. Those failures are retained and classified, not rewritten into passes. Separate filtered migration runs pass22and32cases with that whole historical pin-specific fact explicitly excluded. The current3,509-case public suite runs the full surface/authority fact with the approved1.81.17assertion. Historical beta remains usable with its original dependencies; adopting a future package from this candidate requires upgrading Core.

## Evidence and limits

Initial alpha.0.44 package/cache and initial receipts are retained separately from the reviewed final artifact. A final source-shape review corrected restriction.Elements to the shared schema-child parser so stray non-whitespace restriction text cannot be silently ignored. All final source/package/native/example/API/corpus/retained lanes are requalified after that change. Normal audit and warning checks remain enabled; no machine error-reporting settings change.

Original68packets/4599sealed entries are preserved. The new source/input manifests distinguish immutable Git head357ae494f645d9d20d1790d0a15e20be290d13d7, merged maincb81d80e42528a9ebca98366818309995a70bb87, the separately approved Core baseline and current qualified checkout bytes. No sealed historical validator is rerun against changed production inputs. Documentation checks are programmatic, not visual acceptance.

Core public API closure, full XSD1.1, typed queries/PSVI, general calendar arithmetic and performance acceptance remain pending. Release controller, published beta, tags and registries are unchanged. No commit/push/publication occurred. Next: review the final diff and retained-migration boundary, then commit/push/open PR only on explicit owner request.
