# Bounded global simple-type final controls

Both owned native facades admit explicit final on existing supported global named simple types. XML whitespace separates restriction, extension, list and union. Empty sets and #all are supported; duplicate tokens have set semantics. Invalid admitted tokens and mixed #all lists are Invalid. The raw source bound is256UTF16 units; excess is Unsupported.

The compiler checks direct source-bound named derivation edges before publishing compiled types: simple restriction bases, list itemType, every union memberTypes entry and simple-content extension/restriction bases. Inline consumers, forward references and unused definitions are checked too. A prohibited method is Invalid. Ordinary element/attribute use and unrelated methods remain permitted. The final set is not inherited by a subsequently derived type.

For example, final="union" on a decimal restriction prevents that type from becoming a union member, while allowing a list to use it as its item type. final="#all" permits ordinary value validation but prohibits all four derivation methods.

finalDefault, local simple-type final and complex-type final remain Unsupported. Existing schema assembly/type/content/value constraints and finite limits remain. This adds compilation validity checks, not a public final accessor or general derivation validation. Original type/declaration authority, actual atoms, XML export, cancellation and expiry remain. Legacy decimal-only grammar remains unchanged.
