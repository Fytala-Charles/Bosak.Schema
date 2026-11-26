# Native URI and IEEE batch

The 0.11 development candidate qualifies 200 behavioral contracts, NU-501–NU-700, for bounded `anyURI`, `float`, and `double` validation. This is a contract count, not a count of new XSD features.

All **16,196 source tests** (1,585 internal / 14,611 public) and **14,611 overlapping fresh package-only public tests** pass. API review preserves all 55 exported type names and 630 prior members, adding nine reflection rows for the three kinds and getters. All nine signed Core 1.86.0 archives and runtime DLLs verify.

The frozen sample remains 21 matches. The expanded 129-case sample improves to **32 matches / 97 Unsupported**, with zero mismatches or harness errors. Seven admissions are recorded in [CORPUS-DELTA.json](CORPUS-DELTA.json). Original fixtures and earlier sealed packets remain unchanged; the historical SC-054 expectation is reconciled separately.

Passing PR checks, reviewed merge and exact-main CI are mandatory external delivery gates. This evidence packet is prepared before commit and does not claim registry publication, full XSD 1.1, general PSVI/C45 or a public query factory.

[Consumer guide](GUIDE.md), [contract ledger](CONTRACTS.json), [immutable adoption](ADOPTION.json), [semantic review](FACET-REVIEW.md), [qualification](QUALIFICATION.json), [review](REVIEW.json), [coverage update](COVERAGE-UPDATE.md). Run `python docs/numeric-uri-200/validate.py` for offline evidence checks.
