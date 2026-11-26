# Declared attributes on empty complex owners

26 November 2026. Development Schema0.17 on signed Core1.86.0.

Both owned facades admit existing supported attribute uses on named or anonymous empty complex types, at the root or admitted nested sequence/global-reference positions. Bare empty types and existing empty sequence/all/optional-choice shapes retain their content semantics. An empty owner has an empty typed sequence, not an absent element-only typed value or one empty-string atom. The original complex component governs the element; original attribute facts retain their own atomic/list/selected-member components.

For example, `<item id="a" refs="a a"/>` can validate with declared ID and IDREFS attributes. Forward references and same-owner repeated bindings use the separately adopted document identity table. Use explicit attribute-item reads for IDREFS; scalar list reads remain refused. Original XML, attribute lexical spelling, comments/PIs and owner paths are preserved in export.

Empty content rejects every text or element child, including whitespace and nonempty CDATA. Original comments and processing instructions are permitted. Node-data `ReadElement(path)` returns Empty and `ReadElementItems(path)` returns no items; this is not nil. On the scalar facade `HasRootScalar` remains false, root reads return Empty for empty roots, and empty descendants do not consume scalar indices. An outer element-only sequence still has absent typed value/FOTY0012.

Use existing local/anonymous typed attributes, closed global references and acyclic attribute groups. Required presence, unknown ordinary attributes, lexical/facet failures, provided fixed/default values, actual union/list members and ID resolution use the same validation rules as other admitted owners. No document/facts are published after Invalid or Unsupported assessment. Missing default/fixed augmentation remains Unsupported; no attribute is fabricated.

Limits remain16 effective attribute uses per owner,64 expanded attribute uses,64 elements and depth4 for nested plans, existing scalar/list/value/XML/component limits and caller query/output capacities. Excess returns Unsupported without truncation. Cancellation and owner disposal revoke the same original facts and cursors; tables and sessions remain isolated.

Application attributes combined with structural assertions, mixed empty content, complex-type inheritance, implicit anyType, wildcards/open content, prohibited use and complex nillability remain outside this batch. Owners with no effective uses preserve historical unknown-attribute refusal. Identity default/fixed values, DTD/xml:id, key/keyref/unique, general PSVI/C45 and public schema-aware queries remain excluded. Public API and Core pin are unchanged; no registry publication is implied.
