# Exact native duration batch

The 0.12 development candidate qualifies **200 behavioral contracts**, DU-701–DU-900, for bounded exact `duration`, `yearMonthDuration` and `dayTimeDuration`. This is a contract count, not200 new XSD features.

All **17,866 source tests** (1,617 internal / 16,249 public) and **16,249 overlapping fresh package-only public tests** pass.32 internal controls cover actual Core facts, component equality, REC order vectors and an independent Gregorian oracle;38 new public controls cover imports, strict detached getters, lexical ambiguity and lifetime/cache/cancellation.400 authored good/edge rows run through both facades and two explicit timezone extremes (1,600 catalog checks).26 further fresh package boundary controls pass. API preserves55 exported type names and all639 prior reflection rows, adding9. All9 signed Core1.86.0 archives/DLLs verify.

The frozen sample is **21 matches / 0 Unsupported**. The expanded129-case sample is **32 matches / 97 Unsupported**, with zero mismatches or harness errors. [Corpus delta](CORPUS-DELTA.json) records0 admissions and any changed Unsupported diagnostics. Earlier sealed dossiers and original upstream/legacy JSON remain unchanged. Six measured old duration-refusal contexts are reconciled separately; original source copies and fixture hashes are retained.

Passing PR review/checks, merge and exact-main CI are mandatory external delivery gates recorded after this pre-commit qualification packet. No registry publication, general legacy duration-operator acceptance, full XSD1.1, PSVI/C45 or public query factory is claimed.

[Guide](GUIDE.md), [adoption](ADOPTION.json), [ordering review adoption](ORDER-REVIEW-ADOPTION.json), [contract ledger](CONTRACTS.json), [semantic review](FACET-REVIEW.md), [qualification](QUALIFICATION.json), [review](REVIEW.json), [ADR020](../ADR-020-bounded-native-duration-transport.md). Run `python docs/duration-200/validate.py` for offline packet verification.
