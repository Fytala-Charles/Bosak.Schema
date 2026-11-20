# Qualification and limits

20 November 2026. See [adoption before source edits](ADOPTION.md).


## Owned node-data qualification ? 20 November 2026

Local candidate on base `43f14dbf56c36e5185e705af901678c42c8d1aff` passes
1,615 source cases (503 implementation / 1,112 public), preserving all 1,482 prior
named outcomes. Package-only minimum Core 1.81.2 passes 1,112; Core 1.81.17 passes
1,111 shared cases plus its explicit current-version check. The deliberately
minimum-version-only surface test is excluded only from that current lane.
Retained 124/23/33 unchanged callers pass with only Schema.dll replaced. Counts
overlap. All 392 prior public surface rows are preserved; two sealed owned types
add nine rows (54 types / 401 rows total). Frozen pilot remains 9 matches / 12
Unsupported / zero mismatch or harness error. No new XSD expectations or grammar.

The local unsigned package is `0.6.0-alpha.0.19`; published beta.1, Core source/pins,
release controllers and tags are unchanged. The earlier sealed proposal and
feasibility packets are unchanged. Owner API acceptance/ADR-016 and delivery
remain pending. No commit or push has been performed for this candidate.

## Implementation-specific evidence

The 133 added source cases comprise 73 retained native feasibility controls,
three actual-facade gate/disposal cases and 57 public consumer cases. Authored
48-payload scalar/owner controls run through both actual Core operations and
the public owned API; these repeats are not independent specification cases.
Original type/provenance identity, anonymous types, namespace/form, all four
scalar families and complex simple content remain separately tested internally.

The public tests exercise selectors/parameter names, FOTY0012, unassessed hints,
invalid assessment without admission, cache target identity, coefficient/scale
rollback, export failure, cancellation, quotas, detached values and disposal.
Actual-facade gate tests verify overlap/reentrancy refusal and document/session
disposal waiting on admitted synchronous work. They use internal test access;
no public callback or raw Core operation is added.

The first source run found XmlConvert's empty-name ArgumentException reported
`name`; the facade now maps both XML and argument name-validation failures to
`localName`, without including caller text. A lifetime test's blocking Task.Wait
was rejected by xUnit1031 and replaced with an awaited Task.WhenAny check. Both
failed-run receipts are retained separately from the final passing run.

An initial current-Core run deliberately included the minimum-version assertion
and failed only that 1.81.2 expectation. Its final lane excludes exactly that test
and adds a 1.81.17 identity assertion; all other 1,111 shared cases pass.

The first surface comparison used the feasibility copy's synthetic assembly
version 1.0 and hit version-bearing byref type strings. The qualified comparison
uses the prior real product package assembly, preserves every one of its 392 rows,
and allows only the specified two owned types and seven methods (nine new rows).

All 18 Core archive signatures/source commits/cache payloads/runtime bytes are
verified in independent caches. Both package lanes contain no project references.
The signed published beta.1 archive and retained beta caller baseline are checked.
This is local qualification, not publication, general XPath, full XSD 1.1,
performance/SLA acceptance or owner API approval. No Core engine changes were made.
