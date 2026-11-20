Applications can read typed data from original assessed elements and attributes
through a separate owned node-data session. The fixed `data($node)` operation is
bound before native assessment; element-child paths and expanded attribute names
select original nodes, and reads return detached scalar values under existing
capacity and lifetime limits. Element-only targets refuse with FOTY0012.

The change adds two sealed owned types and seven methods while preserving the
existing decimal/scalar facades and all 392 prior public API rows. It includes
normative adoption, original-node feasibility, an ADR-016 review addendum,
consumer guidance and sealed qualification evidence. XSD grammar and the published
beta remain unchanged; general XPath and transferable PSVI remain outside scope.

Validation: all 1,615 source cases pass (503 implementation / 1,112 public).
Package-only consumers pass 1,112 cases on each of Core 1.81.2 and 1.81.17; the
latter replaces only the deliberately minimum-version-specific assertion with
an explicit current-version check. Retained 124/23/33 callers pass with only
Schema.dll replaced. All 18 Core signatures/payload identities verify. Frozen
upstream pilot remains 9 matches / 12 Unsupported / zero mismatches or harness
errors; 15 integrity controls and documentation checks pass. Counts overlap.

See docs/native-node-data-review/README.md for the scoped contract review and
docs/native-node-data/QUALIFICATION.md for evidence and remaining acceptance limits.
