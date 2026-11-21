# Proposed contextual and inherited integer assertion contract

21 November 2026. Proposal only, not semantic adoption or execution. Base is the
PR37 merge. [Review identities](SPECIFICATION-REVIEW.json) retain exact source-body
hashes, affected sections and edition/revision notes. XSD uses XPath2.0 here; Core's
XPath3.1 surface is an implementation dependency, not a replacement normative pin.

## First useful slice

Admit xs:assert children on existing attribute-free integer simple-content extension
and its one direct complex restriction. Keep primitive/named direct integer simple
bases, six admitted facets, named/anonymous root and existing nested terminals.
Effective assertions contain base components then local components, preserving
component identity and captured static context. All must succeed on each assessed
owner; local assertions cannot replace inherited tests. Assessment may stop after
a failure. Identical expression strings are distinct components and count separately.
No new derivation level, application attributes or complex xsi:type substitution.
Existing decimal/string child-assertion grammar and legacy profiles stay unchanged.

## Assertion context versus final original facts

Structures3.13.4.1 constructs a parentless context rooted at the assessed element,
using partial validation with that owner's assertions skipped. Descendant validation
is still required. Its partial validation status maps the context type to anyType
under XDM2.0; atomization therefore yields untypedAtomic from the context string
value. Do not reuse a public NativeSessionNode that atomizes as final G/C1, and do
not replace that string with canonical BigInteger spelling. Leading zeros provide
an observable lexical control. Ancestors, siblings and other documents are absent.

For valid admitted integer simple content, unprefixed `$value` supplies the typed
integer actual value, represented exactly with BigInteger. Invalid structural/
lexical/facet prerequisites cannot be repaired by assertions. The specification's
empty `$value` cases for invalid/nil/nonsimple content do not grant new nil or
structural support in this slice. No other variables are admitted.

The temporary assertion projection is internal and prepublication. It must not
mint public component authority or pass unvalidated values to the public issued-
atomic path. After all checks succeed, ordinary native facts still carry original
D/T/G and actual C0/C1 provenance, using their existing assessment episode. Keep
complex G->B separate from anonymous C1->C0. An internal equivalent representation
is permissible only if it preserves the specified observable context semantics.

## Bounded expression grammar and required semantics

Propose context item `.`, `$value`, signed integer literals, quoted string literals
for scalar comparisons, a single mod step on a numeric operand and integer divisor,
scalar value comparisons eq/ne/lt/le/gt/ge, general =/!= with bounded integer
sequences, comma sequences/inclusive integer ranges/empty sequence, parentheses,
and/or. String operands are admitted for scalar eq/ne; incompatible typed operands
produce a dynamic type error under the chosen policy. General sequences contain
integer literals/ranges only. Parenthesized logical expressions retain precedence.
Bare operands/EBV forms, other arithmetic/functions/paths/variables are excluded.

Do not implement this as one all-BigInteger predicate:

- `$value` integer operations and integer literals retain exact integer arithmetic.
- A general numeric comparison with untypedAtomic requires its numeric conversion
  to double; integer partners are promoted as required. It can lose precision. The proposed profile uses IEEE binary64 nearest/ties-even conversion; verify actual conversion routes against the sentinels.
- Arithmetic on untypedAtomic likewise converts it to double before mod.
- A value comparison converts untypedAtomic to string; `. eq 5` is a type error,
  while `. = 5` can be true. `. eq '0005'` and `$value eq 5` can both succeed.
- General != is existential inequality, not the complement of membership.
  Empty general comparison sequences yield false for both = and !=.
- Integer mod follows truncating remainder, with dividend sign, and raises an
  error for divisor zero. Double mod by zero yields NaN; comparison to zero is
  false. These outcomes must never escape as unhandled host exceptions.

The2^53sentinels deliberately distinguish `. = 9007199254740992` and
`$value = 9007199254740992` for actual integer9007199254740993; parity can differ
for the same reason. This does not weaken the exact stored/returned integer value.
Preserve existing collapse/lexical rules and all type/facet prerequisites.

## Static context, errors and capacities

Retain each assertion's original namespace/default/base-URI context when inheriting
it; do not compile base tests using derived declaration bindings. Use fixed XPath codepoint collation for admitted string equality. Existing symbolic
xpathDefaultNamespace capture and explicit-URI limits remain unchanged. The first
grammar does not admit QName paths or new callable functions. Prefix/rebinding
controls must inspect retained context even when the bounded expression does not
consume it. CurrentDateTime/timezone stay episode-stable; acquisition remains absent.

Adopt as a future policy choice: type errors in admitted expressions are detected
dynamically. A false result or evaluated dynamic/type error makes the assertion
unsatisfied and the instance Invalid. Legal syntax such as `. eq 5` can therefore
compile but fail its assertion. Excluded syntax stays Unsupported/AssertionGrammar,
not an accepted false test. Eagerly compile all assertion components and logical
branches, including unused type definitions; do not hide unsupported grammar behind
runtime short-circuiting. At runtime, use documented left-to-right logical short
circuiting; an unevaluated error does not itself fail the instance. Preserve legacy
static diagnostic policies outside this new path.

Retain512expression characters/128tokens/16nesting levels and native integer-digit
limits; cap effective inherited-plus-local assertions at8per owner. Introduce a
fixed internal expanded-sequence limit32, checked using bounded cardinality before
allocation even for huge ranges. Descending ranges are legal empty sequences.
These assertion intermediates are separate from the public single-result-atom
query limit and cache accounting. Capacity remains Unsupported/RepresentationCapacity;
cancellation remains cancellation, never assertion-false. Keep no-partial-facts,
rollback, recovery, detached export/value and existing lifetime/ownership guards.

## Explicit exclusions and qualification order

Defer contextual assertions on other scalar families, application attributes,
assertion facets on simple types, general element-only/empty-content assertions,
child/ancestor/attribute paths, namespace-uri and other functions, broader XPath,
complex derivation chains and new publication/PSVI/C45/performance claims. The
original assert_027 fixture requires nested empty content and namespace-uri; do
not mix that separate work into this integer assertion slice.

The original assert006 family combines base membership and local parity. Verify
base-false/local-true and base-true/local-false separately, plus nested repeated
owners and original frozen metadata. Do not rewrite its schema to use `$value`;
its dot coercion is a required semantic boundary. No forecast match count.

After sealing this proposal, record experiment-only normative adoption before
editing an isolated copied-source candidate. Run actual native public-facade and
original-node probes on signed Core1.81.2/1.81.17, preserving all intermediate
sources/failures and exact signature/cache/archive/runtime identities. Verify the
partial assertion projection independently from successful final native evidence;
passing a standalone evaluator or fabricated authority is insufficient.

Compare all2,522current source identities. POTENTIAL-ADMISSIONS is an inventory,
not permission to change expectations: measure each affected fixture and record
separate receipts. Only after feasibility passes, record production adoption and
then implement/reconcile tests. Production qualification includes full source,
package-only consumers on both Core pins, retained124/23/33, API54types/401rows
zero delta, signed dependency/beta.1 baselines and the unchanged guarded21case
pilot. Original corpus/notices/metadata stay byte-identical. No Git delivery or
package publication is authorized by this contract step.
