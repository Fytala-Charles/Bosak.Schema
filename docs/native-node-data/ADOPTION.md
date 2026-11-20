# Bounded original-node data implementation adoption

20 November 2026. Product base: `43f14dbf56c36e5185e705af901678c42c8d1aff`.
Recorded before production edits. This is a bounded implementation decision;
ADR-016 remains proposed for owner API acceptance and does not grant general
Core C45, XPath or XSD 1.1 acceptance.

## Immutable normative sources

| Source | Adopted sections | SHA-256 of retrieved dated revision |
|---|---|---|
| https://www.w3.org/TR/2017/REC-xpath-31-20170321/ | 2.4.2 Atomization | ca1f8b00f9626e3e73589a371d702acf6bf0f4708c96a2cb0e87ca7a5ad3b88c |
| https://www.w3.org/TR/2017/REC-xpath-functions-31-20170321/ | 2.4 fn:data | b5bdcfb2e8b6edbe5a997f8971b0829da124bcd6d071c7f651ee71be99b2a988 |
| https://www.w3.org/TR/2017/REC-xpath-datamodel-31-20170321/ | 2.7, 3.3, 6.2, 6.3 | a157e098f0de2fefd81ce95bb934762ff5a1f616a4745854f0340d177082acd3 |

Historical XSD grammar/facet adoption and frozen upstream expectations are
unchanged. Review pins and earlier feasibility packets remain separate evidence.
Core packages stay at minimum 1.81.2 and qualification 1.81.17.

## Decisions and intentional limits

Implement the two sealed owned types and signatures in
[the proposed API](../typed-node-contract/API-PROPOSAL.md). A separate session
compiles only `data($node)` before native assessment; existing scalar sessions
are never rebound. Use the measured two-file provider/node adapter extension.
Capture the default operation context fingerprint in a first compilation whose
recorder always returns Unsupported and never assesses input or issues facts.
The real provider then requires that exact fingerprint, profile, descriptor,
engine assembly, language/revision and empty namespace map. Fingerprints constrain
context; they confer no authority. No CompileOptions, expressions, raw nodes,
callbacks, facts or provenance handles enter the public API.

Copy and validate element-child paths within the private synchronous session
gate. Empty path means root; default is ArgumentException. More than three
segments, negative or missing indexes are ArgumentOutOfRangeException with the
public path parameter name. Attribute null arguments are ArgumentNullException;
invalid NCName or missing attribute is ArgumentException(localName). A present
unassessed attribute is NotSupportedException. Select original captured wrappers
and require original published facts, then materialize one existing scalar kind
inside the admitted Core operation. Empty string, false and zero remain values.

Element-only facts have no typed value: report NotSupportedException containing
FOTY0012. Check the original guarded fact before evaluation, rather than broadly
catching Core InvalidOperationException or interpreting arbitrary error messages.
All other Core failures retain their existing exception behavior. Read refusal
never changes instance validity or publishes a new diagnostic outcome.

Reuse explicit native/query/cache/document/export capacities and immutable
assessment context. No new grammar, lists, nil, general query or PSVI transfer.
One private synchronous gate rejects overlapping work; disposal waits for admitted
work, revokes membership first, and attempts every owned cleanup once through
NativeCleanup. No fairness, latency or total-memory guarantee. Returned scalars
and XML are detached; XML is parsed lexical output, not original bytes or PSVI.

Qualify public argument/lifetime/capacity and original-node scalar paths, preserve
the existing source catalogs, then package/binary compatibility and frozen pilot.
No commit, push, package publication or tag is authorized by this record.
