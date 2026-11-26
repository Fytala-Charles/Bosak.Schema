# ADR025: declared attributes on empty complex owners

Accepted for development0.17,26November2026.

The [pre-implementation adoption](empty-owners/ADOPTION.json) records exact immutable REC revisions and affected sections. Reuse the existing bounded attribute compiler for empty complex owners at both root and nested compilation sites. Reuse original empty-content assessment and provider facts: zero atomic items, actual complex governing component, independent original attribute/member components. Validate all supplied values and document identities before any successful fact publication.

Do not loosen empty-content character rules or attribute budgets. Keep mixed-empty, attribute+structural assertions, inheritance, implicit anyType, wildcards, complex nil and augmentation excluded. No public API/Core dependency change. See [guide](empty-owners/GUIDE.md) and [qualification](empty-owners/QUALIFICATION.json).
