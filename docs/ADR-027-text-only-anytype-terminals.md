# ADR027: text-only anyType terminals

Accepted for development0.19,26November2026.

The [structural review](anytype-terminals/STRUCTURAL-REVIEW.json) distinguishes cofeatures from admissions. Adopt exact REC revisions/sections in [ADOPTION](anytype-terminals/ADOPTION.json) before implementing a text-only subset. Implicit and explicit anyType share one real builtin governing component per compiled schema, separate from the scalar registry, and use original mixed-value transport with actual untypedAtomic atoms. Empty text is one empty atom, never an empty complex sequence or absent typed value.

Refuse child/application-attribute wildcard work and non-self instance substitution before fact publication; do not validate as xs:string or silently skip known declarations. Keep complex nil/constraints/derivation/all-choice positions and complete component closure outside this slice. Reuse caller/XML/representation bounds, cancellation, original facts and expiry. Public API, signed Core pin and legacy grammar remain unchanged. See [guide](anytype-terminals/GUIDE.md) and [qualification](anytype-terminals/QUALIFICATION.json).
