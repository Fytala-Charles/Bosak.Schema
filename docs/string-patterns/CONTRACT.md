# Proposed bounded string-pattern contract

22 November 2026. Review/proposal only; production semantic adoption and Schema
implementation remain pending. [Exact source review](SPECIFICATION-REVIEW.json).

## Owners and existing profile

Extend only direct restrictions of xs:string already admitted by the opt-in native
facades, named or anonymous. Existing scalar element leaves, finite attribute uses
and nested placements share the same facet semantics. Combine patterns with the
existing string length/enumeration facets. Keep actual named/anonymous type identity,
original facts, string payloads, authority, disposal and export guarantees unchanged.
Legacy NativeDecimalSession, general legacy helpers, simple-typed document roots,
new restriction chains, other datatypes and public API/Core pins remain unchanged.
All unused admitted type definitions are compiled eagerly under existing type limits.

## Normative meaning

XSD Datatypes4.3.4/G matches the **whole literal**, not a substring. Pattern `.x`
accepts `bx` and rejects `abx`; `a?x` rejects `aax`. Empty regex matches only the
empty string; `a*` also accepts empty. Matching consumes Unicode scalar characters,
not UTF-16 code units or graphemes. Dot excludes LF/CR and accepts one supplementary
scalar. Combining sequences are not normalized; case is preserved.

Caret and dollar are literal characters outside classes in XSD; they are not XPath
anchors. Use the actual parsed pattern and xs:string lexical value without trimming,
case folding or Unicode normalization. Existing XML parsing/attribute normalization
happens before facet evaluation. Apply xs:string's fixed preserve whitespace rule;
do not introduce normalizedString/token behavior. Attribute and element probes must
use character references where needed to distinguish XML normalization from matching.

Multiple pattern elements in one restriction map to one alternative (OR) pattern
component. Conjoin that result with length/enumeration facets. General inherited
pattern sets would combine through derivation, but new type chains are excluded from
this first slice; do not introduce inherited OR or silently flatten future levels.
Count source pattern elements before any deduplication for capacity enforcement.
An empty matched string remains an authenticated String scalar, distinct from an
empty complex-content typed sequence and from absent element-only typed value.

## Proposed grammar

Admit concatenation, grouping/alternation, Unicode scalar literals, dot, the XSD
single-character escapes, XML whitespace classes \s/\S, explicit positive/negative
character classes with scalar/range members, ?, * and +, and bounded/open-ended
brace quantifiers under the limits below. ASCII ranges are not locale-sensitive;
explicit supplementary literals/ranges must remain scalar-correct. Outside classes,
$ is an ordinary literal; \$ is not an XSD single-character escape. Escaped caret
is allowed. The single-character escapes are n/r/t, backslash, | . - ^ ? * + { } ( ) [ ].

Initially refuse class subtraction, Unicode property/block classes, \d/\D,
\w/\W and XML-name classes \i/\I/\c/\C. These need separate table/version and
syntax qualification; never implement \d as ASCII-only. The public SKU's original
pattern uses \d and its global element has a simple type, so that whole fixture
remains outside this first proposal. Do not rewrite SKU to make it pass.

XSD does not admit lookaround, backreferences, lazy/possessive quantifiers or XPath
flags. No XPath/.NET dialect is accepted accidentally. Well-formed but excluded
syntax is Unsupported. Return schema Invalid only for a proven violation of the
supported XSD syntax or existing facet rules; when syntax outside the subset has
not been fully validated, retain Unsupported rather than guessing Invalid.
No regex fallback is allowed after a subset refusal, including for unused types.

## Resource and execution contract

Use a Schema-owned Thompson-style Unicode-scalar automaton with finite immutable
state and bounded per-assessment work. No backtracking engine, private Core helper,
reflection bypass, ambient AppDomain timeout change or background runaway thread.
A future shared public bounded Core service is a separate contract; this proposal
neither changes Core APIs nor creates a mandatory Core-to-Schema dependency.

Proposed ceilings to prove in feasibility before production adoption:

| Resource | Proposed bound |
|---|---:|
| Pattern elements per direct restriction | 8 |
| Parsed pattern UTF-16 characters | 256 |
| Parsed atoms | 64 |
| Parenthesis depth | 8 |
| Class intervals/members | 16 per class |
| Explicit quantifier bounds | 0..16 |
| Compiled states per source pattern | 128 |
| Total compiled pattern states per schema, including unused types | 1,024 |
| State/interval work per complete assessment | 4,194,304 |

Count epsilon-state visits, consuming-state tests and class interval checks in the
work budget; deduplicate closure visits so nullable loops terminate. Check cancellation
before compilation and assessment and at bounded intervals during scanning/closure.
Reuse no mutable state or exhausted budget across assessments. Exact bound/one-over
probes and deterministic budget exhaustion are mandatory; the proposed numbers may
change only with a separately recorded contract revision before production adoption.

Capacity or work exhaustion yields Unsupported/RepresentationCapacity, never a
false match or schema/instance Invalid. A valid compiled pattern's false match is
instance Invalid. Malformed schema XML and proven facet syntax errors retain Invalid.
No invalid/refused/cancelled result publishes partial facts, evidence or snapshot.
Keep existing input/string/numeric/type/node/attribute/query/export limits alongside
these matcher bounds. No public option, scalar kind or result-shape change is proposed.

## Gates and preserved evidence

First prove compilation/matching and resource accounting in isolation. Then run actual
native assessment with original named/anonymous element and attribute facts on both
signed Core pins. Cover empty String versus empty typed sequence, invalid rollback,
foreign/copy refusal, cancellation/recovery, disposal and source export. Preserve
all2,886current source identities and existing API54types/401rows. Package/retained/
frozen and expanded measurements belong to later production qualification; no match
is forecast from a changed first refusal. Keep all original corpus bytes/expectations.

The public-Core probe is capability evidence only:20tiny vectors per pin agree after
experimental semantic wrapping, but no caller-bounded execution gate is satisfied.
No Schema pattern assessment, Unicode property table acceptance, automaton implementation
or production semantic adoption is established in this packet.
