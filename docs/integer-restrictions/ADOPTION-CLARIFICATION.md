# Before-code target routes clarified

20 November 2026, after the first implementation regression run. Preserve the
original ADOPTION.md, ANTICIPATED-DELTAS.json and before-code controls unchanged.

The first full source run preserves 1,371 prior outcomes and finds only AS-034
and M2C3-67 transitioning from compile Unsupported to Valid. M2B-54 is exercised
by the legacy decimal-only test harness and correctly remains Unsupported there.
Its before-code NativeSchemaSession probe was separately Unsupported; the adopted
target of Valid compile / Invalid `12.5000` instance applies to the opt-in facade.
Add that unchanged schema/instance as a separate opt-in control; do not change
the legacy M2B-54 expected outcome. This clarification corrects the forecast's
route ambiguity, not the integer semantics or protected legacy behavior.

AS-034's existing public fixture reader must project the newly admitted Integer
scalar kind as well as its previous decimal/string kinds. Its schema/XML remain
unchanged; one exact integer scalar replaces the old unreachable empty expectation.
M2C3-67's existing built-in integer instance becomes Valid even though its newly
admitted named integer restriction is unused. Preserve eager checks on that definition.
