# Descriptor snapshot qualification

17 November 2026. Base/main7e16b1609741caa5ac445cf9c81ae1725cf36da3 (PR8 merge); main44121838200 passed. [Pre-implementation basis](BASIS.json), [contract](../DESCRIPTOR_SNAPSHOTS.md), [receipt](VERIFICATION.json), [offline verifier](verify.py).

The compiler publication boundary now snapshots all outer collections and nested namespace/facet/wildcard collections into immutable storage. It preserves shared assertion instances across root and facet lists. Public descriptor constructors/with expressions still accept caller-owned collections and do not become authenticated immutable factories. Core and all other product behavior remain unchanged.

Full suite319pass/0fail/0skip:290 implementation and29 external public-consumer cases. Ten new implementation cases cover populated outer collections, empty/invalid models, nested ordinal namespace bindings, facet identity/validation, wildcard lists, source XML changes, caller record copies, repeated compilation and eight finite parallel readers. Three new no-friend consumer cases cover outer mutation, nested/facet mutation and compatible caller record copies. These13 cases extend the306-case PR8 baseline; no targeted/repeated lane is counted twice. All45 separate release controls pass. Release build0warnings/errors.

Reflection shows40 exported types/317 declared contract rows identical to PR8; no additions/removals. Runtime collection types deliberately change from writable implementation collections to immutable storage; casts/mutation were not a promised public contract. Dictionary iteration order is not promised. Performance impact from copying is unmeasured.

Initial targeted run failed because the new fixture placed defaultOpenContent after a component declaration; the fixture was corrected to match the already enforced grammar. No schema behavior was relaxed. Original failed TRX remains in the ignored raw packet. Full final TRX and source/API snapshots are retained in the source-only archive here; no product binaries/packages/credentials. Original alpha and PR8 dossiers are unchanged and should be verified at their own commits/checkouts. Raw packet `.guard-tmp/schema-descriptor-snapshots-20261117`.

No beta/profile/ADR acceptance, public DTO-constructor redesign, legacy result behavior change, dependency update, tag, commit/push or publication is claimed. Next: define and test the legacy compile-diagnostic/result precondition (A01/A02) before making any broader validation promise.
