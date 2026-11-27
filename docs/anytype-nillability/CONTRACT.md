# Bounded anyType nillability

27 November2026. Recorded before implementation at fbd1d405f6ef9f894357b48e716b5b3d214e42eb. Immutable source identities/sections: ADOPTION.json.

Admit nillable on existing implicit/explicit anyType roots, sequence particles/references and admitted known-lax matches. Preserve the canonical declaration nillability flag; any xsi:nil attribute requires a nillable declaration. Use existing XML Boolean true/false/1/0 collapse and256UTF16 pre-collapse attribute-value limits for schema and instance flags. Malformed admitted values are Invalid; excess is Unsupported.

True nil forbids all text (including whitespace/empty CDATA represented as text) and element children; comments/PI remain original. Publish a nilled state with original anyType component, original declaration and an empty typed sequence, not one empty untypedAtomic string. No scalar exists for that element. Omitted/false/0 nil retains normal anyType content, including one empty untypedAtomic atom for an empty non-nilled element. Export original XML without augmentation.

Attribute validation is independent of nil: assess known global attribute values/declarations and existing unknown lax attributes with anySimpleType/untypedAtomic evidence. Preserve source-owner paths/indexes, special-attribute refusal and existing strict-ancestor unknown-attribute projection exclusion. Resolve admitted xsi:type before nil; non-self anyType overrides remain Unsupported. Abstract global element use remains Invalid. Failure publishes no facts; later ordinary/nilled assessment recovers without mutating compiled authority.

General complex nillability, anyType nil members under all/choice, default/fixed constraints on anyType, broader overrides and unrelated schema/content/assembly gaps remain Unsupported. Legacy grammar, source/dynamic/value budgets, cancellation, foreign ownership, expiry and no augmentation remain. No public API, full XSD/PSVI/C45 or production acceptance claim.

Qualify both facades and native facts, original samples, full source/exact fresh package/API/signed Core/invoice/docs/release controls; preserve old input bytes/names and explicitly reconcile measured expectations. Reviewed commit/push/PR/green merge/main CI precede completion. No publication included.
