# Bounded anyType nillability

Both owned native facades admit nillable on existing implicit/explicit anyType roots, sequence particles/references and admitted known-lax matches. Any xsi:nil attribute requires a nillable declaration. Schema/instance Boolean flags use true/false/1/0, XML whitespace collapse and the existing256UTF16 pre-collapse attribute-value bound. Malformed admitted values are Invalid; excess is Unsupported.

True nil forbids every text or element child, including whitespace and empty CDATA text nodes; original comments/PI remain. The element retains its original canonical anyType component and declaration, with IsNilled true and a supported empty typed sequence. No scalar exists for that element. An ordinary empty anyType element has one empty untypedAtomic atom instead. False/0/omitted nil preserves ordinary text and existing mixed-content behavior.

Nilled roots and descendants independently validate attributes. Known globals retain their declaration/type/value constraints. Existing unknown lax attributes retain declaration-absent anySimpleType/untypedAtomic facts. Source owner paths/indexes remain original. Unknown attributes below a strict ancestor retain the existing Unsupported projection boundary; nil cannot bypass it. Special attributes remain restricted, and admitted xsi:type resolution precedes nil. Non-self anyType overrides remain Unsupported. Abstract element declarations remain Invalid before nil.

Both public facades expose typed-empty item reads; root scalar presence is false for a nilled root. Node-data IsNilled(path) and scalar IsRootNilled report actual nil state. Original XML export retains the nil attribute, comments, PI and namespaces without augmentation. Failed content/attribute assessment publishes no document/facts, and later valid nil/ordinary assessment recovers without changing compiled authority. Cancellation, foreign ownership and expiry remain.

General complex nillability, anyType nil members under all/choice, anyType default/fixed constraints, broader type selection and unrelated schema/content/assembly gaps remain Unsupported. All finite source/dynamic/value budgets and the legacy grammar remain. No public API addition, full XSD/PSVI/C45 or production acceptance claim.

For example, a declared-nillable anyType root with xsi:nil="true" has no typed atoms, but an independently supplied known decimal attribute still must be a valid decimal. The same root without xsi:nil has its ordinary untypedAtomic value, including an empty string when empty.
