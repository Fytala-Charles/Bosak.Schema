# Strict attribute wildcard exclusions

26 November 2026. Pre-implementation contract against PR87 merge8aa76553fb338be40d8e8b109c98f561d15738d6 and signed Core1.86.0.

Extend the qualified empty-owner strict attribute profile with notNamespace (complement of a bounded namespace set) and notQName (bounded expanded QName exclusions and ##defined). Resolve QNames at the original wildcard element, using its prefix/default bindings; compare expanded names, not prefixes. ##targetNamespace/##local retain absent-namespace semantics. ##defined excludes actual global declarations available in the closed catalog; explicit attribute uses still take precedence, including references to those same globals. Strict unknown attributes remain Invalid, and known permitted attributes validate their original types/fixed/ID rules.

Schema namespace/notNamespace coexistence, empty notNamespace, malformed/unbound QName, QName member outside the wildcard's allowed namespaces and attribute ##definedSibling are Invalid. Empty notQName means no exclusions. Preserve the earlier empty namespace-enumeration refusal; do not widen relative-URI constraints. Each namespace and QName list has at most8 lexical tokens of256UTF16; duplicate tokens still count. Prior actual-document/string/identity/fact budgets and special-attribute boundaries remain.

Root/named/nested/global-reference empty owners retain original declarations and zero typed items. No synthetic wildcard-use handles, missing-attribute augmentation, lax/skip, element wildcards, nonempty/mixed owners, assertions or inheritance/composition. Qualify actual owned type/member/declaration identities, explicit-use precedence, failure-before-publication, recovery, cancellation/foreign ownership/expiry and both detached facades. Preserve original test names/fixture bytes; reconcile measured prior expectations separately. Run full source/fresh package/API/signed Core/frozen samples/invoice/docs/release controls before Git delivery. No new result or admission is claimed by this contract.

See [exact immutable adoption](ADOPTION.json).
