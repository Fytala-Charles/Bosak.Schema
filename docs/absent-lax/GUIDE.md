# Bounded lax-content consumer scope

Use either owned native facade with signed Core 1.86.0. A globally declared anyType root may contain undeclared elements and ordinary application attributes, including namespaced names. Unknown nodes retain null declarations, physical expanded names and original whitespace. Elements are annotated anyType, attributes anySimpleType, and typed values are untypedAtomic; empty elements produce one empty value.

Known global descendants and attributes still validate. Invalid known values or unresolved known IDREFs fail the whole assessment before fact publication. Unknown lexical id attributes are untyped and do not create ID bindings. Known attributes retain schema source order, followed by unknown attributes in instance order. No missing attributes are synthesized.

Admission requires every ancestor to be anyType. A strict complex ancestor would need separate partial-assessment type projection, so unknown content there remains Unsupported. Unknown xsi:type/nil and special XML attributes are also Unsupported. Schema location hints remain unfetched. Explicit wildcard syntax/modes/namespaces/notQName/UPA, broader complex derivation and augmentation are outside scope.

Actual document bounds remain 64 elements, depth four, 64 application attributes, 16 attributes per owner and 16 value-bearing element states. String and existing identity budgets also apply. Owner lifetime, cancellation, foreign-component refusal and original XML export remain enforced.
