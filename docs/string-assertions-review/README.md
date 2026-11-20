# Bounded string assertion delivery review

20 November 2026. **Local review complete; commit/push/PR delivery pending.**
This is a Codex self-review, not an independent review or GitHub approval.
Public main remains `2b4a3497232da3cee20be0d4f561a853baa23aa8`.

No blocking implementation finding was identified. The change matches the
[pre-code adoption](../string-assertions/ADOPTION.md): only the two internal
assertion/schema files change production behavior. No new public member, Core
source/pin, release controller or publication operation is introduced.

## Reviewed contract

| Area | Finding |
|---|---|
| Grammar | String eq/ne and =/!= are enabled only in the mixed profile; quoted literals are scanned once with doubled-delimiter escaping; unknown/mixed/ordering grammar refuses |
| Evaluation | Value cardinality errors invalidate the instance; general equality/inequality is existential over all pairs; empty comparisons have false assertion EBV |
| String identity | Parsed values retain whitespace/case/Unicode; ordinal equality implements the selected codepoint equality on valid XML strings |
| Namespace resolution | Existing expanded-name/default-namespace rules resolve immediate declared children; no new path, attribute or nested assertion route |
| Bounds | Existing expression/token/depth, native/string and occurrence ceilings remain; no general expression engine added |
| Publication/lifetime | Native validation finishes before original facts or snapshots are published; failed assertions leave sessions recoverable; original type/provenance checks remain |
| Legacy behavior | Decimal-only grammar stays separate; all 401 public surface rows remain unchanged |

The direct code review follows literal scanning, compile-time operand families,
scalar/general comparison branches, presence/count dispatch and schema assessment
publication order. The final executed source/test hashes and product binary match
the sealed qualification receipt, so no tests were repeated for this docs-only
review. No implementation correction was required.

## Admissions and evidence audit

The [machine-readable review](REVIEW.json) verifies 218 entries across six sealed
packets, exact current source/test identities, the local package archive and its
product DLL. The seven [authored admissions](../string-assertions/ADMISSIONS.json)
match their before/after records, with all schema/instance inputs preserved. All
other 180 rows in those three authored fixture files are unchanged, and case order
and identities are preserved. These are explicit semantic admissions, not a claim
that every prior Unsupported expectation stayed unchanged.

The 42 frozen W3C file hashes and unchanged case manifest verify. Only original
Assert/assert_025/assert_025_2 changes from Unsupported to a validity match. The
125 authored payloads and actual native/Core comparison controls supplement this
small corpus; they do not constitute whole-XSD conformance or a coverage percentage.

## Qualification reviewed

- 1,886 source passes: 520 implementation / 1,366 public; 271 added cases.
- Both package-only minimum/current Core lanes: 1,366 passes each. The current
  lane replaces exactly the minimum-version-specific assertion with a current
  version check; test scopes overlap.
- Retained callers: 124 / 23 / 33 pass, only Schema.dll replaced.
- All 18 Core signatures, source pins and cache/runtime payloads verified.
- Nine published minimum-Core string-subset differential controls pass.
- API remains 54 types / 401 rows; no additions/removals.
- Frozen pilot: 10 matches / 11 Unsupported / zero mismatch or harness errors;
  15 pilot integrity controls pass.
- Documentation checker and programmatic rendering/link checks pass.

[Qualification](../string-assertions/QUALIFICATION.md) retains exact scope and
reproduction limits. The local unsigned alpha.0.20 package is qualification
material; published beta.1 is unchanged. Source and all earlier sealed receipts
remain unchanged by this review.

## Prepared delivery

[PR title](PR-TITLE.txt) and [description](PR-BODY.md) are ready. Request commit,
push and open PR for this reviewed bounded slice. Schema AGENTS.md section 8
requires an explicit request: "Never run `git commit`, `git push`, `git reset`,
`git rebase` unless the user explicitly asks."

General XPath, attribute/nested assertions, string ordering, Boolean/integer
assertion operands, full-XSD/C45/PSVI and production SLA acceptance remain outside
this slice. Merge and release/publication are separate steps. After review delivery
and merge verification, the next ranked semantic gap is xsi:type.
