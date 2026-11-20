# Public Core nested transport feasibility

20 November 2026. Product base c34e9fe, before nested production changes.

Both independently restored public-package lanes pass **50 controls each**:
Core1.81.2 (source7249d0e01628c47f92b89a3380bc69cf58d70045) and Core1.81.17
(source6a7618e9dcf70eb8a67d5d1789ff7b787133b52d). Each has13new nested checks
and37inherited scalar/attribute/simple-content-root controls; lanes overlap and
add no Schema product cases. All18Core package signatures verify; archive,
extracted cache and loaded-output payloads match. Exact hashes are in
[evidence/IDENTITY.json](evidence/IDENTITY.json); no project/source Core references.

The synthetic input is an order with two line containers, exact decimal amounts,
integer quantities and a false Boolean. Checks cover all five nested terminals,
three element-only containers (FOTY0012 then successful leaf reuse), wrapped
parent/children identity and shared type/distinct local declaration handles,
foreign nodes/facts, missing facts, revoked snapshot evidence and closed input.
Actual public data($node) evaluation exercises issued original leaf/container
facts. Host navigation checks wrapper identity; no arbitrary query-path syntax
or schema-aware transformation support is inferred.

The first minimum run had41passes/9failures because my synthetic leaf facts
named a governing type different from the issued value's governing type. Core
correctly refused that inconsistency. The fixture was corrected to use matching
handles; final minimum/current50/50passes are separate receipts. The first TRX
is preserved as minimum-first-run.trx and is not a product defect report.

The assessor deliberately throws if asked to validate XML. These controls prove
transport/guards only, not nested XSD validation. A separate public Schema
baseline control passes by proving the authored nested schema returns Unsupported
and no session on the unchanged delivered engine. This one refusal case is not
positive feature support and does not increase the1,036product baseline.

Reproduce the probes with dotnet test evidence/minimum/Probe.csproj or
current/Probe.csproj in Release and independent RestorePackagesPath directories.
The baseline project must run at the exact product base c34e9fe, not after nested
implementation. No Core engine change/pin or public API addition was needed for
this scoped feasibility exercise. Actual native provider qualification remains.
