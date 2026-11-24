# Date transport preflight: boundary refusal required

24 November 2026; base merged main2eb999052188a872a54548c71c378d6a05a8cc3c. This is a caught package-only constructor/accessor/formatter diagnostic. It does not implement date validation, native provider facts, the proposed detached date facade or lifetime behavior.

## Measured result

Both signed Core1.81.2 and1.81.17 produce the same result: **32/32 date component round-trips**, **28/32 successful expected strings**, and **four caught OverflowException formatting failures**. Every failure has year long.MinValue (-9223372036854775808), across absent timezone, UTC, +14:00 and -14:00. The other seven year values include zero, negative years,10000 and both signed long.MaxValue extremes. These are selected component/formatter observations, not Schema-validity or universal boundary proofs.

The actual current Schema package returns Unsupported for an xs:date global root through both NativeSchemaSession and NativeSchemaNodeDataSession. No document is obtained and no native authority, original-node atomization or detached lifetime result is claimed. That known baseline refusal was measured without a substitute validator. Neither a DTO nor synthetic provider was added to make those gates appear green.

## Stop condition and next recommendation

The [original design contract](../date-time-transport/CONTRACT.md) explicitly requires stopping if a supported path cannot preserve the proposed boundary. That stop condition is reached. The recorded [bound revision proposal](BOUND-REVISION-PROPOSAL.json) excludes just long.MinValue, making the proposed native year interval [-9223372036854775807,9223372036854775807]. An otherwise-valid excluded year would be Unsupported/RepresentationCapacity. This is a proposed future native rule, not an existing implementation or measured assessment. Original planned DT-013 remains Valid in the historical sealed plan; its proposed change is recorded separately.

The alternative is a separate Core formatting fix and full Core qualification, publication and new-pin Schema qualification. This preflight changes no Core source or pins. The narrower explicit interval lets the next date-only experiment proceed on existing pins without bundling unrelated Core release work. No dateTime/time precision obligation is resolved by this date boundary restriction.

Next: record the revised exact native experimental contract and semantic adoption before isolated source overlays, then qualify both facades and actual original-node facts on both signed pins. All18 original native gates remain unaccepted: component preflight only partially informs the year/timezone and package-identity gates. Named/anonymous placements, both compiler routes, eager unused declarations, XML lexical/calendar rules, capacities, ownership, export/reopen, cancellation and lifetime still need native execution.

## Identities and verification

[Results minimum](minimum/RESULTS.json), [results current](current/RESULTS.json), [caught diagnostic](Program.cs), [package identities](IDENTITY.json) and fresh Core signature logs record the experiment. Eighteen Core archive hashes and runtime DLLs match the earlier signed qualification; both Core archives are freshly signature-verified. Schema.dll matches qualified unsigned alpha.0.37. The projects reference packages only and use the original signed minimum/current caches, with no production warning/audit policy changes. No package was built or published.

An initial diagnostic-only build accidentally retained test SDK auto-entry-point generation from its template and failed CS7022. The original logs are retained; removing test-only references made the console project build and run without suppressions. This was a harness build correction, not a product or semantic failure.

All323 production inputs and59 previous sealed packets remain unchanged. There is no new source-test, native or corpus execution. The integer-enumeration qualification remains4,146 source tests and expanded10matches/119Unsupported/zeroerrors; these earlier results are not presented as this experiment's tests. No new public API or accepted semantics, commit/push/tag/publication.
