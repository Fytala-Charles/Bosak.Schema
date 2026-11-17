# Beta contract decision draft

Status: **Proposed for owner review**.17 November2026. Supplements ADR-009/010; does not accept or rewrite historical decisions.

| Decision | Proposed choice | Consequence / remaining review |
|---|---|---|
| D01 Beta scope | Bounded native decimal-session profile; descriptor/assertion/XSLT compatibility routes explicitly identified separately | Owner must confirm useful scope; broader XSD validator requires more implementation |
| D02 Success semantics | Native Valid certifies complete admitted finite grammar; legacy IsValid certifies only its documented assertion/facet operation | Do not conflate result types or transfer authority from exported XML/descriptor objects; decide A02 compile-diagnostic precondition |
| D03 Ownership and threading | One synchronous sequential session; overlapping calls refuse; close waits for admitted work; detached scalar/XML survives | No async/raw-handle public expansion or arbitrary concurrency promise; preserve documented cleanup failure/cancellation distinctions |
| D04 Numerics/dependencies | Explicit native/query/formatting limits; public XdmExactDecimal dependency; released Core pins separately qualified | No silent numeric fallback; resolve C45/Core contract questions or expressly exclude unsupported interchange before freeze |
| D05 Diagnostics | Four fixed bounded native stage codes; legacy diagnostics have separate detail/security contract | No fabricated W3C location/code or blanket redaction claim; define logs/truncation expectations |
| D06 Resolution/security | Beta must identify trusted convenience helpers versus enforceable restricted input paths | A04/A05 and reader/stream/root budget findings require policy/test work; existing helpers are not sandboxes |
| D07 Descriptor immutability | Publish immutable owned snapshots before promising safe sharing; public record inputs remain data, not authority | Nested list/dictionary aliasing and constructor compatibility need audit/implementation |
| D08 Compatibility | Freeze the reviewed beta surface only after alpha API/binary comparisons and per-route acceptance | Existing39 types/313 rows are baseline evidence, not automatic acceptance; keep inert License shapes |
| D09 Deferred scope | No complete XSD1.1, broader types, positive universe transfer, streams/async native API, Braid or XmlDb in first bounded beta | Explicit refusal/support matrix must remain visible; no automatic admission of skipped engine tests |

Proposed ADR-009 resolution: accept the finite profile as the supported beta boundary only after D01/D02/D04/D09 owner agreement and remaining scoped qualification. Proposed ADR-010 resolution: accept the owned synchronous facade contract only after D03/D05/D08 and security/ownership audit completion. Keep both original ADRs Proposed now; publishing alpha did not accept them. Neither this draft nor approval to continue documentation changes their status.

Recommended order: security/resolution contract and tests (A04/A05), descriptor snapshot ownership (A03), legacy compile/result precondition (A01/A02), scoped resource measurements and final API review. Every implementation requires pre-change immutable specification/behavior basis, paired controls and canonical docs. No beta tag/version/publish decision is made here.


17 November2026 implementation follow-up: [restricted resolution](../RESTRICTED_RESOLUTION.md) provides an opt-in alternative for A04/A05 with31 implementation controls and three public consumer controls. Original alpha observations remain true; no old helper behavior changed. Native/profile/ADR acceptance remains pending. The candidate surface now has40 types/317 rows; published baseline39/313 remains historical.
