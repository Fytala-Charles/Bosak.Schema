# Bounded date enumeration: isolated feasibility

24 November 2026. This source experiment uses signed Core **1.81.17 only**, after the separately approved, still-uncommitted dependency baseline. Reviewed Git head357ae494f645d9d20d1790d0a15e20be290d13d7 and merged maincb81d80e42528a9ebca98366818309995a70bb87 have identical trees. [Experimental adoption](EXPERIMENT-ADOPTION.json) preceded source overlays. The [original contract](../date-enumeration/CONTRACT.md) remains sealed historical evidence. No production enumeration source/test changes or new production adoption occurred.

## Measured results

**428 native controls pass**:408 executions of68 authored proposals through both public facades and three implicit timezones, plus20 ownership/equality/lifetime/capacity/original/comment controls. The two attribute-owner plans require the documented [plan correction](PLAN-CORRECTIONS.json); original schema/instance payloads remain separately retained. These are authored plan corrections, not upstream fixture rewrites.

All **4,442 regression results pass** (1,345 internal /3,097 public) after exactly **three measured expected-result changes** in isolated copies. The unchanged public suite initially yields3,094passes/3failures: the original unused-enumeration exclusion fact and both DSC-075 facade cases. The same original schemas/XML and IDs remain; exclusion now compiles Valid and the existing nonmember date-extension case now compiles Valid /assesses Invalid. No original test was removed/renamed and no theory became newly eligible. All other temporal refusals remain. [Reconciliation](REGRESSION-RECONCILIATION.json) and retained initial/final TRXs distinguish actual semantic changes from scratch-harness mistakes.

The equality key uses arbitrary-precision Gregorian ordinal/minute arithmetic with floor division for negative years, and separates absent from explicit timezones. Membership preserves the instance's year/month/day/offset and actual original-node restricted-type identity. A final review control also verifies ordinary formatting whitespace inside an otherwise empty enumeration element, using the shared schema-child parser. No public equality/hash/ordering implementation changes. [Public API comparison](SURFACE.json):55types/408rows, zero delta.

All nine signed Core archives and actual runtime payloads are identical to earlier qualification, and all nine signatures freshly verify. Source pin6a7618e9dcf70eb8a67d5d1789ff7b787133b52d is retained. [Identity](IDENTITY.json) describes isolated source compilation, not a packaged Schema consumer run. [Candidate source manifest](CANDIDATE-SOURCE.json) contains64files:63 original sources plus one new file, with exactly three overlays.

## Corpus and correction of the contract review

The frozen21-case pilot is unchanged. The expanded129 sample has **16matches /113Unsupported /zero mismatches or harness errors**, up from14matches. Exactly the two original MS date-enumeration rows change: their schemas compile Valid; the empty date is Invalid and the matching date is Valid, agreeing with unchanged upstream expectations. All other rows are byte-for-byte equivalent outcome records. [Before/after results](CORPUS-COMPARISON.json).

The earlier contract incorrectly inferred that comments anywhere in these instances would remain a blocker. Actual unchanged code already accepts comments in their element-only container while refusing comments inside scalar content. Four exact-original/facade executions and two separate scalar-comment controls confirm that distinction. No comment handling or upstream XML was changed. This later measured correction supersedes that review inference without altering its sealed original record. Location hints remain opaque and cause no external acquisition.

## Evidence boundaries and next step

[Harness iteration record](HARNESS-ITERATIONS.json) retains fixture wiring, diagnostic API and budget-setting corrections rather than presenting the first runs as passes. All failures were caught test/runner failures; no unhandled negative .NET launch or machine error-reporting change occurred. Current319engine/test input hashes and all67previous packets/4444sealed entries remain unchanged by this experiment. Documentation rendering is programmatic, not visual acceptance.

Full packaged Schema consumers, retained binaries, exact packaged examples and exhaustive completion of all20 proposed native gates remain production-qualification obligations. The source experiment demonstrates feasibility, not complete public API/PSVI/C45 or performance acceptance. Temporal bounds/explicitTimezone, named simple derivation chains, complex date restriction, dateTime/time and general XPath date operations stay separate. Published beta.1 and its frozen release controller are unchanged.

Next: record separate production adoption, port the qualified overlays and measured expectation changes, then run full source/package/native/retained/example/API/corpus qualification on Core1.81.17. No commit, push, tag or publication was performed.
