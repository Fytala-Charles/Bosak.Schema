# Proposed next contract: bounded date simple-content extension

Status: scope/review checklist only. No authored XML execution plan, implementation adoption or source change is made by this document. Detailed clauses, limits and a separate experimental adoption record precede an isolated source overlay. [Normative source pins and reviewed areas](SPECIFICATION-REVIEW.json).

## Proposed scope

Admit simpleContent/extension of xs:date and currently admitted direct facet-free date aliases at supported named/anonymous complex roots and terminal placements. Reuse current attribute-use grammar, namespace/form rules, declaration and document budgets. Keep checked date lexical/calendar behavior and the [-long.MaxValue,+long.MaxValue] interval unchanged. Settle the precise accepted placement and alias matrix in the detailed contract rather than treating compiler reuse as proof.

No new scalar kind or public date DTO is required. Preserve original element declaration D, its declared type T, effective governing complex type G and actual atomic date content type A. A single date typed value is not absent merely because the governing type is complex. Do not publish the complex type itself as the atomic type, flatten G into a fabricated simple type, lose anonymous identity, or fabricate a QName.

Keep date complex simple-content restriction, arbitrary extension/restriction chains, temporal facets and assertions outside this first slice. Explicitly guard the existing decimal-specific complex-restriction fallback when a date extension base becomes reachable. Valid but unimplemented derivations must remain Unsupported rather than being mislabeled Invalid. Review exact-type complex xsi:type separately from new simple-to-complex substitution; generic derivation graph reachability does not itself authorize a new representation route.

## Required planned controls

Before implementation, author actual XML/schema cases for root and terminal simple content, named/anonymous types, direct/alias bases, supported attributes and form/namespace combinations. Preserve the original scalar-date payloads and legacy decimal behavior. Include year zero/negative/extended years, admitted boundary values and excluded-year capacity, timezone absence/explicit zero/extreme offsets, XML whitespace, invalid calendars, invalid/extra/missing attributes and disallowed children. Verify invalid/unsupported results publish no partial facts.

Exercise flat and nested compiler paths, eager unused declarations, shared named-type identity, forward references within the existing grammar, derivation cycles and excluded complex date restriction. Define budgets for the source model and document/attribute uses using existing limits; do not accidentally ignore attribute uses on a reused plan or add unbounded derived graphs. Scan existing authored refusals before predicting admissions. NativeDateConsumerTests.DeferredTypes contains an explicit date-extension refusal; retain its original stimulus and measure its changed outcome before reconciliation.

Run both NativeSchemaSession and NativeSchemaNodeDataSession against signed Core 1.81.2 and 1.81.17 packages. Actual Core data($node) must return one genuine Date atom with the actual content type while facts retain G and the original declaration/type. Verify root/leaf/scalar counts, correct owner paths and attribute values, original physical order, authority isolation, snapshot/episode provenance and supplied assessment context. Timezone absence must survive the transport without an implicit offset.

Cancellation, cache/work/document budgets, failed admission recovery, export/reopen, session/document/facts disposal and detached date/attribute lifetime remain required. XML export preserves parsed lexical content and attributes. General comparison/ordering, arbitrary typed queries, public closure and full PSVI acceptance are not granted by a date extension.

## Original examples and delivery sequence

Probe CType/contenttype00201m/contentType00201m1_p and _n unchanged, retaining schemas, XML, metadata and upstream validities. They are representative trials, not a forecast of two admissions. Keep the mixed=true CType00401m pair separate. Date enumeration and maxExclusive originals retain their deferred facet obligations; no simplified rewritten schema can substitute for the original trial.

After exact contract review: isolated native feasibility, measured regression/expectation reconciliation, separate production adoption, then source, package-only, actual-native, retained-binary, API, exact packaged examples and both corpus-pin qualification. Record no-public-API-delta only after measurement. All earlier seals and original stimuli remain intact. Commit/push require an explicit owner request under AGENTS section8.
