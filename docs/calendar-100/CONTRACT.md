# Calendar batch contract

25 November 2026. The owner authorized 100 contracts across qualification, implementation, testing, documentation and Git delivery. Contracts are cohesive behavior and delivery obligations; they are not 100 new XSD features or a normative coverage denominator.

| Contract IDs | Scope |
|---|---|
| CAL-001–010 | Signed Core migration, compatibility, unchanged evidence and release isolation. |
| CAL-011–020 | dateTime grammar, Gregorian components, exact seconds and rollover. |
| CAL-021–030 | time grammar, precision, timezone presence and UTC day displacement. |
| CAL-031–040 | dateTimeStamp timezone, actual subtype and instance selection. |
| CAL-041–050 | Enumeration, bounds, partial ordering, explicit timezone and patterns. |
| CAL-051–060 | Named/anonymous restrictions, inheritance, fixed collapse and source budgets. |
| CAL-061–070 | Root, finite sequence/choice/all, nested, attribute/reference/group and simple-content placements. |
| CAL-071–080 | Original nodes, component identities, provided constraints, nil and no partial facts. |
| CAL-081–090 | Explicit lexical/number/year/pattern/read capacities and compatibility refusals. |
| CAL-091–100 | Detached values, accessors, cancellation, lifetime, gates and bounded guarded integration. |

Every contract's status and evidence are in [CONTRACTS.json](CONTRACTS.json). The first ten qualify operational prerequisites. The remaining ninety cover native semantics and compatibility boundaries. Positive and negative/edge stimuli execute through both public facades; selected XSD cases run under both extreme assessment timezones to check context independence. Deeper identity/authority controls use the real Core protocol and are identified separately from no-friend public consumers.

The [adoption record](ADOPTION.json) precedes semantic edits and pins the immutable 2012 XSD 1.1 and 2017 XDM bodies by hash, affected sections, exact Core source and intentional bounds. The [guide](GUIDE.md) documents the additive public API and finite grammar. There is no fallback to .NET millisecond date/time parsing.

Earlier fixture payloads, IDs and sealed packets remain unchanged. [Legacy reconciliation](LEGACY-RECONCILIATION.json) lists the measured expectation changes: newly admitted calendar declarations, invalid date-only calendar facet literals and invalid date-only instances. [Test authoring corrections](TEST-AUTHORING-CORRECTIONS.json) separately retain the initial new short-pattern examples and their corrected complete-string expressions.

The source candidate advances MinVer's minimum minor to 0.7. Its local validation package is unsigned and is not a release reservation. Published Schema beta.1, its release controller and original Core qualification remain historical. Package publication requires preparing and verifying a future exact artifact set; it is separate from these implementation/PR steps.

Final lexical review recorded [signed-zero adoption](ADOPTION-SIGNED-ZERO.json) before correcting the native mapping. [Review evidence](SIGNED-ZERO-REVIEW.json) preserves the initial new-test assumption and ten reproduced failures; the final source and package runs pass the corrected specification expectations. Core 1.86.0 binaries and its narrower standalone lexical helper remain unchanged.
