# Empty-union correction

27 November2026. The preceding anonymous compound-base batch rejected empty unions in its newly admitted base graph. That guard was incorrect: XSD1.1 Datatypes4.1.1 explicitly permits an empty member sequence. This correction adopts the immutable source pinned in ADOPTION.json, removes that extra guard, compiles empty union bases, and requires all attempted values (including empty text) to assess Invalid. A list with an empty-union item type may still accept an empty list.

Preserve the previous adoption/contract/receipts as historical evidence. Reconcile original AB-089 separately without rewriting its fixture. Do not reinterpret that earlier guard as a valid XSD restriction. No new value is accepted by an empty union, and no phantom member/type is introduced.
