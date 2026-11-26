# Original markup and bounded annotations

26 November 2026. Development Schema 0.15 on signed Core 1.86.0.

Both owned native facades ignore instance comments and processing instructions when validating already admitted simple, complex, empty, mixed and nil content. Original nodes remain in the guarded tree and XML export. Comment/PI text never enters an element's string or typed value; split text still contributes in document order. Element paths count element siblings, and existing declaration/type/atomic authority remains unchanged. Comments/PI have their original node kinds and ordinary string values, without fabricated schema facts. Captured nodes and traversal cursors expire with the owner; detached values remain usable.

Empty and nil content still reject every text or element child, including whitespace. Comments/PI do not make an element a supplied default/fixed text value; augmentation remains excluded. Original XML node/byte budgets count markup before assessment. Legacy decimal-only leaf/simple-content refusals are preserved.

Schema annotations are validated before declaration scans or closed include/import projection. Supported XSD component positions accept one leading annotation; schema roots allow repeated annotations anywhere among their members. Annotation permits optional `id`; appinfo/documentation permit optional `source`. Annotation IDs are checked as NCNames and must be unique within the source. Wrapper text must be whitespace and payload wrappers must be appinfo/documentation. Their content is opaque: even XSD-named declarations, invalid QName attribute values and nested annotations inside a payload are application data and are never compiled as components. Source locations are metadata and cause no external access.

Each schema source allows at most 16 annotations, and original source XML budgets apply before annotations are omitted from the compiler-owned parsed copy. Caller strings and frozen fixture bytes remain unchanged. `xml:lang` and foreign attributes on annotation wrappers remain Unsupported, along with unsupported parent grammar and general schema features. Annotation PSVI is not publicly projected; this is validation-neutral metadata support, not full annotation/PSVI conformance.

For example, `<r>1<!--note--><?audit ok?>2</r>` validates as integer12 under an admitted integer root. `<r><!--note--><?audit ok?></r>` has a zero-item typed value under an admitted empty complex type. Adding a space to that empty element remains Invalid.

Signed Core dependencies and public APIs are unchanged. Registry publication and general XSD1.1/C45/query integration acceptance are separate.
