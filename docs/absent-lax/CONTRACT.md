# Declaration-absent lax assessment contract

26 November 2026. Proposed next implementation batch; not implemented or qualified.

The reviewed baseline is PR85, main `a6adb688c55861d466955c44208e24b9f1617c18`, development 0.20 with signed Core 1.86.0. Its recorded 22,346 source and 20,643 overlapping package tests remain historical results. This contract adds no passing test count or corpus admission.

## Required semantics

For unknown names encountered through builtin anyType lax wildcards, preserve the physical expanded name and original node while publishing a null governing declaration. Continue recursively assessing known descendants and global attributes against their actual declarations, including value constraints and document ID bindings. An unknown lexical `id` attribute does not acquire ID semantics.

The dated XDM mapping is precise: lax notKnown elements have type annotation xs:anyType, attributes xs:anySimpleType, and atomic values xs:untypedAtomic. Do not substitute xs:untyped element annotations or xs:untypedAtomic attribute annotations: those apply when the relevant PSVI properties are absent. Preserve XML-parsed whitespace and aggregate descendant text without replacing it with normalized child values. Empty elements yield one empty untypedAtomic value.

The owned Core interface already permits null ISchemaNodeFacts.Declaration. Schema currently indexes every state declaration into its component ring and derives attribute names from declarations. Those implementation assumptions must be removed without fabricating declarations or weakening lifetime, ownership and cancellation checks. An absent declaration does not make the type or assessment origin absent.

## Bounded first candidate

Only admit absence when every element ancestor is governed by anyType. Unknown content below a strict non-anyType complex ancestor stays Unsupported: partial validation would require separately qualified XDM type projection for that ancestor. Known-only paths retain their existing behavior.

Keep strict global root selection, current known-plan exclusions, document limits of 64 elements/depth 4/64 application attributes/16 attributes per owner, 16 value-bearing element states, string limits and pre-publication failure behavior. Unknown xsi:type/nil and special xml attributes remain Unsupported. Location hints remain unfetched. Explicit wildcard syntax, skip/strict modes, namespace constraints, augmentation, general PSVI and C45 acceptance are outside this candidate.

## Native qualification gates

1. Actual null declaration for unknown elements and attributes; real owned type and untypedAtomic components, with no successful global declaration lookup invented for an unknown name.
2. Node annotations and scalar payload types agree; empty and whitespace-rich values survive both detached facades and export.
3. Known invalid descendants below unknown ancestors return Invalid before facts publish; known ID/IDREF graphs remain document-wide.
4. Cancellation, foreign ownership, disposal and repeated assessment preserve existing failure behavior, including access to an absent declaration after expiry.
5. Ancestor-projection, special-attribute and all capacity boundaries return Unsupported without partial facts; a subsequent valid assessment recovers.
6. Preserve every prior test name and fixture byte; reconcile measured expectation changes separately. Run full source and fresh exact-package consumers, API comparison, signed Core payload checks, frozen corpora and original invoice workflow before a semantic PR.

See [exact experimental adoption](EXPERIMENT-ADOPTION.json). No production adoption, API change, Core pin change, publication or full XSD 1.1 coverage is asserted by this document.
