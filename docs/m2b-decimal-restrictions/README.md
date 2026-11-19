# M2B named decimal restriction candidate

19 November 2026. The [adoption](ADOPTION.md) implements the preserved [proposal](PLAN.md) on Schema6861c04. Fresh dated Structures/Datatypes bodies match the earlier review hashes; new semantics are recorded separately before production edits. M2A and other historical dossiers remain unchanged. No Core engine/API/pin, release-controller, tag or registry change.

## Supported increment

Global named simple types can directly restrict xs:decimal, with minInclusive/minExclusive/maxInclusive/maxExclusive/totalDigits/fractionDigits. Local decimal leaves resolve these names in their actual QName context; forward references and shared type identity work. All definitions, including unused types, are compiled before admitting a session. The16global type ceiling is shared with complex types;16roots and prior XML/sequence/assertion capacities remain.

Facet validation uses exact value-space coefficients/scales. Lexical leading/trailing zeros do not alter facet results after representation admission. totalDigits follows the normative magnitude/scale equation:0.001 requires at least3, despite coefficient1. Equal inclusive bounds allow a singleton; equal exclusive bounds admit an empty value space and no present valid value. Mixed equal inclusive/exclusive bounds are inconsistent schema components. Optional absence creates no scalar; every repeated leaf is checked.

Existing supplied lexical digit/scale capacity runs before facets. Supported facet violations are Invalid, capacity or excluded features are Unsupported, and no refusal publishes an owned result. Facet integer values are explicitly bounded to totalDigits1..256 and fractionDigits0..256; exact range comparisons allocate at most the documented bounded scale-alignment temporaries. Limits are profile restrictions, not XSD requirements or SLAs.

Named decimal leaves retain their actual/declared named handles, node annotations and original provenance through real Core snapshots/read/evaluate operations. Primitive xs:decimal remains separately resolvable even with no primitive leaf. Root complex types retain FOTY0012; foreign/disposed state is guarded. Detached XML/scalars expose no raw authority or transferable PSVI.

Named nondecimal types, restriction chains, anonymous simple types, fixed/whiteSpace/pattern/enumeration/assertion facets, lists/unions, attributes/imports, scalar roots and nested structures remain excluded. General derivation relations, schema-aware query syntax, cross-universe handoff and overall C45/API acceptance are separate. Full orders/sku support and full XSD1.1 conformance are not claimed.

## Qualification

| Lane | Passed | Scope |
|---|---|---|
| Full source |534 |324implementation+210public; no failures/skips |
| Minimum-Core package-only |210 |Published Core1.81.2, no project refs |
| Current-Core package-only |66 |65authored facet cases+Core1.81.17 identity |
| Retained unchanged callers |124/23/33 |OnlySchema.dll replaced; overlapping catalogs |

The65new authored cases comprise22positive,12supported-invalid,2instance-capacity refusals and29compile-refusal controls, with four additional actual Core controls. [Case map](CASE-MAP.json), [results](RESULTS.json), [deliberate prior-case changes](REGRESSION-DELTA.json), [dependency/package identities](IDENTITY.json) and [retained caller identities](RETAINED.json) keep scopes distinct. The initial63-case baseline run reproduced60expected feature failures before source edits. These are authored regressions, not official W3C suite results or a whole-spec percentage.

Release build has zero warnings/errors. The local unsigned0.6.0-alpha.0.6 package is a qualification artifact; repository metadata points to baselineHEAD and the source manifest identifies uncommitted changes.18official Core signatures/source pins/payloads and archive/cache/runtime Schema identities match; original signed beta.1 input payload is independently verified. Existing release-control acceptance stays historical because workflow/controller code is unchanged.

All three actual packaged README C# snippets compile and execute. Four XML blocks, including the monetary restriction schema, run successfully; exact reads are12.5,2,1 and negative/extra-fraction inputs are Invalid. Public success cases export/reopen and re-assess detached XML; lifetime/refusal checks remain. Strict docs, rendered/link checks and source/evidence hashes are retained separately; rendering is programmatic, not new visual acceptance.

The [MoneyType workflow](money-order.xsd) with [instance](order.xml) is included verbatim in the authored catalog. Its origin is the existing public decimal order/tax fixture, not an identified production deployment. Next specification slices remain coherent string/boolean/integer support with selected facets, then attributes. REQ-009/CoreREQ-142 remain In Progress. Commit/push of this implementation awaits an explicit request under AGENTS.md; no publication is authorized.
