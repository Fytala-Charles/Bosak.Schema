# M2A semantic adoption ? 19 November 2026

Recorded before production edits under the owner's request to implement the recommended slice. Baselines and immutable source are in BASELINE.json and SPECIFICATION-SOURCE.json; the 2012 Structures body was freshly downloaded and its SHA256 matched. M1 namespace/XPath and original decimal adoptions remain unchanged. Later errata are not silently adopted.

| Structures clauses | Adopted bounded behavior |
|---|---|
| 2.6, 3.4.1/3.4.2 | Global named complex type components have expanded-name identity in the type symbol space; independent element symbol space |
| 3.3.2/3.3.3, 4.2, src-resolve | Root type QNames resolve using actual in-scope namespace/default bindings; forward references permitted; unknown/duplicate/conflicting declarations refuse |
| 3.4.2/3.4.3 | Compile every named complex type, including unused declarations, using the existing decimal sequence/assertion profile |
| 3.3.4, 3.4.4 | Select declared root by complete expanded name, assess its type's sequence/assertions, preserve original declaration/type facts |

Intentional profile restrictions: one to sixteen global roots; zero to sixteen global named complex types; existing 32KiB schema/256-node limits can impose stricter practical bounds. Root declarations use exactly one inline complex type or one named complex type reference. Named type attributes: name only. Existing complex-content sequence and zero-to-eight assertions, decimal leaves and finite bounds remain. Named simple types, derivation, recursive/nested structures, element refs, imports/includes, attributes, xsi requests and new datatypes remain Unsupported. No network/schema composition is added. Unused unsupported/invalid declarations refuse the whole schema; no partially accepted universe.

Outcome contract: malformed QName, undefined prefix, unresolved or non-complex root type => Unsupported/ValueType. Duplicate root/type expanded names, inline/type conflict, malformed named type name or excluded child structure => Unsupported/DeclarationShape. Unsupported schema attributes retain Attributes/Namespace classification; existing capacity failures remain RepresentationCapacity. Type/root count ceilings use RepresentationCapacity. Existing malformed root/local-name categorisation remains unchanged. Malformed XML retains existing Invalid behavior. Supported instance violations remain Invalid; no failed compile/assessment publishes an owned result. Stage codes/messages and enum symbols remain unchanged. The public compile path does not distinguish full XSD schema-invalid from unsupported-profile schemas; this is an intentional contract limitation.

Root and named type component handles retain per-compiled-schema identity; roots sharing a named type share its handle, while independently compiled schemas never do. Root named type annotation carries its expanded name. Element-only typed-value absence/FOTY0012 and decimal leaf provenance remain. ResolveType uses existing Core APIs, without friend/reflection bypass. RequestedType assessment stays unsupported; a supplied governing root must match the actual selected root. No Core runtime/public API/dependency/publishing change.

Acceptance cases are authored regressions tied to these clause groups, not official W3C case IDs. Existing historical sealed receipts remain unchanged. Record fresh package/source/retained lanes after implementation; publication is separate.
