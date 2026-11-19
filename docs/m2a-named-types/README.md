# M2A named complex type candidate

19 November 2026. Local implementation of the [adopted bounded slice](ADOPTION.md), following the preserved [proposal](PLAN.md). Schema baseline0d9e72a; Core main43935c34. Published beta.1, Core source/dependency pins and release controller remain unchanged. No tag/publication or full XSD1.1 conformance claim.

## Supported increment

Up to16global named complex types and16global roots per schema. Each root selects an inline complex type or a QName reference to a named complex type. Names use exact namespace URI/local-name identity and actual declaration-site prefix/default bindings. Forward references and reuse across distinct roots work; element/type symbol spaces are independent. Existing finite decimal sequence, local forms and zero-to-eight assertion grammar stay unchanged. Every admitted named declaration is checked, including unused types. Existing schema byte/node ceilings can impose stricter practical bounds.

Unknown prefixes/types, malformed QNames, duplicates, inline/type conflicts and excluded definitions refuse without an owned result. Supported instance violations return Invalid. Named simple types, derivation, recursive/nested content, element refs, imports/includes, attributes, new datatypes/facets and arbitrary XPath remain excluded. The broader public orders/sku examples are not unlocked by this slice alone.

The provider resolves named types through existing Core APIs. Roots sharing a type share its component handle while retaining distinct declarations. Root type annotations carry the expanded type name; element-only roots still have no typed value/FOTY0012. Decimal leaves retain exact scalar provenance. Foreign schemas/inputs/facts and disposed state remain guarded. A supplied governing declaration must match the selected root. The public facade still exports detached decimal/XML values without raw authority or transferable PSVI.

## Verification

| Lane | Passed | Scope |
|---|---|---|
| Full source |465 |320implementation +145public-consumer; no failures/skips |
| Minimum-Core package-only |145 |Published Core1.81.2; no project refs |
| Current-Core package-only |52 |51new authored cases +Core1.81.17 identity |
| Retained unchanged callers |124/23/33 |OnlySchema.dll replaced; catalogs overlap |

[Results](RESULTS.json), [authored case map](CASE-MAP.json), [dependency/package identity](IDENTITY-FINAL.json) and [retained caller identities](RETAINED.json) preserve separate lane meanings.51new public cases comprise17positive,8supported-invalid and26profile-refusal controls; five additional controls exercise actual Core identity/facts/lifetime. They are authored regressions, not official W3C cases. A baseline run before production edits reproduced33expected failures out of43initial new cases; the final source run passes all465. Build has zero warnings/errors.

The local unsigned0.6.0-alpha.0.5 package is qualification-only. Its repository metadata points to the baseline HEAD, not these uncommitted edits; the source manifest identifies the actual candidate. Both package lanes verify archive/cache/runtime Schema payload equality and18official Core signatures/source pins/payload identities. The signed published-beta.1 original payload/hash is verified separately. No positive cross-universe acceptance, general C45/API sign-off, official suite qualification, new SLA or fresh Core gate run is claimed.

All three packaged README C# snippets compile and execute; its namespace and named-type XML examples preserve exact12.5 and expanded root names for order/quote. Export/reopen public tests re-assess detached XML under the existing contract. Strict documentation and programmatic rendering receipts are recorded separately; no new visual acceptance claim.

The [original source fixture](../../tests/TestData/Schemas/native-decimal.xsd) motivates the decimal order/tax workflow. New exact schemas/instances are in [the authored fixture catalog](../../tests/TestData/NamedTypes/m2a-cases.json). Historical M1/shared coverage matrices remain sealed baselines; this dossier is the incremental delta under REQ-009/CoreREQ-142. Wider M2 named simple types, facets/datatypes and attributes remain pending.
