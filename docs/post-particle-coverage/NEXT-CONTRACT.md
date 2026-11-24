# Proposed next contract: bounded integer enumeration

This is a scope and review checklist, not adopted semantics or executed cases. Use the pinned XSD 1.1 Datatypes section 4.3.5 together with integer lexical/value rules, base-type datatype validity, fixed whitespace and inherited bounds/digits. Record exact semantic adoption separately before an isolated source change.

## Proposed first slice

Support enumeration on the already admitted integer primitive and twelve builtin descendants, at existing global simple roots, direct scalar members, simple content and attributes where the current profile already supports integer restrictions. Include existing named and anonymous placements and instance type selection. Preserve the accepted derivation depth and compiler routes; no arbitrary new user-type chains, decimal enumeration, integer pattern facets or new public scalar kind is bundled into this slice.

Candidate capacity proposal for the detailed contract: at most sixteen source enumeration entries per restriction, counting duplicate lexical entries before canonical value-set deduplication, under existing explicit integer digit limits. Exceeding representation/work capacity must remain distinct from an invalid literal or illegal restriction. Confirm this choice against existing facet/source budgets before adoption.

## Normative questions to settle before implementation

1. Interpret entries as values of the base type, not strings: signed zero, leading zeros and permitted signs/whitespace can denote the same integer. Reject non-integer lexical forms under exact XSD rules rather than permissive runtime parsing.
2. Require declaration values to be valid for their base datatype, including inherited builtin bounds and previously admitted restrictions. Distinguish this from the interaction of same-level facets; settle empty/intersecting value-space and invalid-schema rules from the normative clauses rather than assuming a convenient intersection algorithm.
3. Multiple enumeration entries define one value set. Preserve inherited restrictions and narrowing rules; do not let a new set widen its base or discard inherited bounds/digits.
4. Distinguish recognized invalid-schema cases, valid-schema invalid-instance membership and explicitly unsupported syntax/capacity. Decide source attributes/annotations and limits without implicitly claiming all XSD facet syntax.
5. Keep original declaration/type/governing/atomic identities, exact BigInteger values, source/snapshot/origin and selected xsi:type restrictions. Canonicalized equality must not rewrite source XML or fabricate public type aliases.

## Required planned controls

Author positive, invalid-schema, invalid-instance and refusal payloads for lexical aliases, plus/minus zero, whitespace, duplicate entries, empty/missing values, int/unsigned/positive/negative bounds, values beyond Int64, digit capacities, sibling min/max/totalDigits facets and existing inherited restrictions. Include 16/17 source-entry boundaries with duplicate literals so deduplication cannot hide capacity use. Compile unused definitions eagerly.

Run both public facades and actual original-node Core/provider checks on signed Core 1.81.2 and 1.81.17, through both compiler routes and existing placements. Verify original declaration versus governing/atomic type, selected-type narrowing, attribute owner paths, root/leaf/scalar counts, physical order, shared limits, pre-cancel behavior, no partial facts, foreign authority, export/reopen, disposal and detached reads. Legacy grammar stays unchanged; no general typed-query/closure or timing claim.

Probe the two original int-enumeration schemas/instances unchanged and preserve their upstream expected validities. Separately retain every existing source stimulus and measure any authored Unsupported expectations before reconciling them. Do not decide those admissions from static inspection.

After contract review and isolated feasibility, production adoption must precede source edits, followed by full source, package-only, native, retained, API, exact packaged examples and both corpus-pin qualification. No full enumeration, full XSD/PSVI/C45 or publication claim follows from this proposed slice.
