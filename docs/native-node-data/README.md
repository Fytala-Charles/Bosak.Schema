# Owned original-node data implementation

20 November 2026. Local implementation on product base
`43f14dbf56c36e5185e705af901678c42c8d1aff`.

[Adoption](ADOPTION.md) fixes the normative revisions, bounded signatures,
error mapping, context binding and lifetime decisions before source edits.
The earlier [proposal and feasibility](../typed-node-contract/README.md) remain
historical evidence; their sealed packets are unchanged. ADR-016 remains proposed
for owner API acceptance. This candidate adds two owned public types and no XSD
grammar or general query support. Package qualification and result receipts belong
to this directory, separately from the feasibility prototype.


[Qualification](QUALIFICATION.md) and the [execution receipt](evidence/RESULTS.json)
record all 1,615 source passes, both 1,112-case package lanes, retained 124/23/33
callers, exact API additions, frozen pilot and preserved historical packet hashes.
[Package identities](evidence/IDENTITY-FINAL.json) record the unsigned local
candidate and all 18 verified Core signatures and payloads. These scopes overlap.

The source, public-consumer and internal native controls are in the normal solution;
run `dotnet build Bosak.Schema.slnx -c Release` and
`dotnet test Bosak.Schema.slnx -c Release`. Package consumers use the recorded
isolated projects/caches and local feed in the evidence. Set their absolute source
and feed paths to a new scratch directory if reproducing elsewhere. Their assets
must resolve only packages, never Core or Schema project references. The current
lane excludes exactly the deliberately minimum-version-only surface assertion;
its added CurrentVersionTests checks Core 1.81.17. The identity helper verifies
archives, signatures, source pins and retained replacement bytes before reporting.

The sealed evidence records raw source hashes and the full frozen-driver runtime
set. Rebuild and make a new receipt if checkout line endings change. Original
proposal and feasibility packets remain intact; this packet does not revise their
historical statements. Programmatic HTML/link checks are not visual acceptance.

Next: owner review of ADR-016 and the exact additive API, then explicit delivery
authorization for commit/push/PR. The next specification slice should be selected
from the ranked remaining grammar gaps after this integration contract is reviewed.
