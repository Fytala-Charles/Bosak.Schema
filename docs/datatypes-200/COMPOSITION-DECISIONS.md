# List and union decisions before implementation

This records the intended second phase under the immutable [adoption](ADOPTION.json). It does not claim implemented behavior or new adoption of a later specification revision.

Native simple types will retain separate list item and ordered union member references; complex `ContentType` must keep its existing meaning. Lists preserve order and their variety even with zero or one items. A union retains its governing union component while each atomic item identifies the selected original atomic member. Inline definitions remain anonymous; no invented type names or external authority factories are introduced.

List construction permits admitted atomic items or atomic-only unions, excludes lists of lists, and bounds the item count to 16. Union construction permits admitted atomic/list members, with eight direct members and bounded dependency/inline depth. Unknown or unsupported members are checked eagerly, including unused declarations. Recursive graphs remain outside the bounded profile. No identity graph builtins are added implicitly.

Union assessment tries members in source order. An invalid value can advance to the next member; an unsupported representation or work limit stops assessment. It must not select a later member by treating inability to assess as invalidity. Namespace-sensitive members use the original instance scope. Lists split only XML whitespace and retain complete item validation and inherited facets.

New explicit item-read APIs will return immutable ordered detached scalar arrays under the existing Core query operation and lifetime gates. Scalar accessors must refuse list-variety values even when empty or singleton. Nil and empty lists remain distinct through existing nil facts. Atomic values can be read as a one-item sequence; nil yields an empty sequence. A container with no typed value still refuses with FOTY0012. No path exposes raw Core handles or transferable authority.

Schema value equality follows the adopted datatype value spaces: numeric decimal/integer equality is exact, QName equality uses expanded names, binary primitives remain disjoint, calendar equality uses the existing exact calendar rules, and list equality is ordered item equality. The specification also equates a singleton list with an equal atomic value where applicable. Equality is independent of selected member name; provenance is retained separately. Pattern/value-space derivation cases that cannot be proved in the finite grammar remain explicit Unsupported boundaries.

Sequence operation budgets, cancellation, owner disposal, invalid-document fact suppression, foreign provenance refusal and package-only consumers must be exercised before composition qualification. No Core semantic change is presumed; actual native transport tests must establish compatibility with signed Core1.86.0.
