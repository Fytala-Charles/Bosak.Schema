# Strict attribute wildcards on sequence owners

26 November 2026. Pre-implementation contract against PR88 merge a487ddbd4662ed4cb33697175e27303e7a069064 and signed Core1.86.0.

Extend the existing bounded strict attribute wildcard validator to direct finite nonempty xs:sequence complex owners: anonymous/named roots, local sequence containers and global references. Preserve existing child order, occurrence, value and identity validation. Attributes must independently satisfy explicit uses or strict wildcard namespace/QName matching and actual global declaration validation. Explicit uses take precedence; no missing global attribute augmentation or wildcard-use declarations.

Keep original complex governing type/declaration and element-only no-typed-value behavior, even if the actual optional child sequence is empty. Child scalar projection and independent original attribute facts remain unchanged. Compilation must retain wildcard metadata through shared named-root aliases and nested/global references without replacing original components. Any invalid child, attribute, ID graph or capacity refusal precedes all fact publication.

No mixed/simple-content/all/choice owners, top-level group shorthand, wildcard+assertion combinations, inheritance/composition, lax/skip or wider partial type projection. Existing empty-owner behavior is preserved. Reuse prior namespace/QName list bounds and document64elements/depth4/64application attributes/16attributes per owner, plus sequence/string/identity/fact budgets. Preserve cancellation, ownership, expiry, source binding and XML.

Qualify both facades, actual native declaration/type/member/owner identities, original no-typed-value state and component sharing, required/optional/repeated children, strict known/unknown/fixed/ID behavior and failure/recovery. Retain every prior test name and fixture byte, reconcile only measured changes. Run full source/fresh package/API/signed Core/frozen samples/invoice/docs/release checks before Git delivery. No new execution or admission is claimed by this original contract.

See [exact immutable adoption](ADOPTION.json).
