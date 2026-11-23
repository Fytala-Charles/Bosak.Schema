# Proposed bounded model-group semantics

## Specification and independent phases

The [immutable XSD/XDM/Namespaces review pins](SPECIFICATION-REVIEW.json) distinguish
review from semantic adoption. Structures 3.8.4 describes choice, sequence and all
validation; 3.8.6.2-4 constrains all groups, declaration consistency and unique particle
attribution. Particle occurrence is separate from group composition. Structures 3.3.2
also explains why an element with no explicit/included type may default to anyType.
XDM distinguishes missing typed value from an empty typed sequence or empty String.

Phase 1 proposes a direct, nonempty choice in an existing named/anonymous complex root:
1..16 explicit local scalar alternatives, distinct expanded names, each minOccurs 1
and maxOccurs 1..16. Outer group minOccurs 0 or 1, maxOccurs 1. It selects one alternative;
repetition of that alternative does not permit mixing another branch. Phase 2 adds
nonempty direct all groups: 1..16 explicit scalar members, each minOccurs 0 or 1 and
maxOccurs 1; outer group minOccurs 0 or 1, maxOccurs 1. All members may appear in any order.
These are intentional profile limits; XSD 1.1 allows broader all/occurrence grammar.

Existing scalar families, direct restrictions, namespaces/form rules, same-family
instance type selection, root attributes and structural namespace assertions retain
their established scope. Arbitrary model nesting, group references, wildcard,
substitution groups, implicit anyType, mixed/open content, complex alternatives,
general repeated groups, nullable choice alternatives and new numeric child
assertions remain Unsupported in the first phases. Existing sequence behavior and
legacy decimal-only grammar do not silently widen.

## Attribution and validity

Choose by expanded QName, not prefix spelling, declaration position or value tests.
A chosen element's bad lexical/facet value is Invalid; do not backtrack to another
branch to make it pass. Within the recognized direct grammar, competing same-QName
element particles are schema-Invalid even if their types match. Distinct namespaces
may disambiguate identical local names. Outside grammar remains Unsupported before
semantic attribution; no wildcard precedence or substitution-group claim is made.

All must enforce each present member's occurrence independently. A required member
cannot be replaced by a different optional member. When an optional whole group is
absent, empty content is allowed; once present, its required members apply. Excess
repeats, mixed choice alternatives, unknown names, missing required content and
non-whitespace element-only text are Invalid within admitted grammar.

Compile unused branch/type/facet definitions eagerly. A group must not hide an
excluded datatype, malformed recognized facet or over-capacity definition merely
because its instance does not select that branch. Source/group/capacity exclusions
remain explicit; this proposal does not adopt generic XSD-invalid-schema recognition.

## Physical order and original evidence

The current provider joins state index to captured input node index. Declaration-
order iteration is therefore insufficient for reordered all content. Append actual
selected states in physical document preorder, retaining each matched declaration
and original governing/atomic type. Second choice alternative must attach to the
first actual child when that is its position; reversed all members must keep their
own facts and types. No sorting of input XML or fabricated replacement tree is allowed.

Scalar leaf reads follow physical document order. Original-node paths use physical
child indexes, independent of schema declaration order. Existing attribute reads
retain their own declaration-order policy while owner paths/indexes remain exact.
Root/leaf separation, foreign authority, original source/snapshot/origin, read limits,
export/reopen, cancellation, sequential work and lifetime invariants remain intact.
Both flat and nested compiler routes must be considered: an unrelated nested root
in the same schema currently selects the nested route for the whole catalog.

An omitted branch has no node, facts or scalar entry. Empty String in a selected
string leaf remains one atomic. A nullable element-only model is still an element-
only type: even an empty valid instance has absent typed value and FOTY0012 for root
atomization, not the empty typed sequence of a genuinely empty complex content type.
Do not derive type variety from the instance's observed child count.

## Budgets and actual-native gates

Preserve source branch/declaration and named/anonymous ceilings. Worst-case choice
occurrences use the maximum alternative, all uses the sum of member maxima; do not
multiply mutually exclusive branches into an artificial instance size. Existing
scalar 16 / node 64 / depth 4 / attribute 64 limits remain. Shared pattern work is charged only
to actual assessed values; compile-time state/type budgets still cover unused inputs.
Representation overflow remains Unsupported. Invalid/Unsupported/cancellation must
publish no partial facts and leave fresh admissions usable.

Run the 50 planned XML payloads through both facades and 18 actual-native gates, first
choice then all, on both signed Core pins 1.81.2/1.81.17. Mandatory cases include
second-branch selection, reversed all order with distinct types, empty selected
String versus absent branch/root typed value, group/member occurrence, ambiguous
names, namespace forms, shared limits and lifetime. Source review alone proves none
of these behaviors. Existing public Core source pins are retained in the earlier
[Core review](../derived-integer-types/CORE-SOURCE-REVIEW.json); no private helper or
new dependency direction is proposed.

The [six original examples](ORIGINAL-EXAMPLES.json) also use implicit anyType, so
particle support alone is insufficient. Keep their original bytes/expectations,
probe independently, and measure actual corpus effects later. Do not rewrite them
into explicitly typed examples and label the variants as original passes.

After native feasibility, separately record production adoption before source edits,
preserve 3,620 original stimuli or explicitly reconcile authored expectations, then run
source, package-only, retained callers, API, exact packaged example and both corpus
pins. No full XSD/PSVI/C45/closure, performance or conformance percentage claim.
