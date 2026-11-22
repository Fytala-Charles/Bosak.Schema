# Proposed structural assertion contract

Status: review only, 21 November 2026. Production adoption and implementation remain pending.

## Proposed scope

Admit attribute-free empty complex types (bare complexType, an empty sequence, and
those forms followed by bounded assertions), named or anonymous, at the root or
inside the existing finite nested sequence profile. Preserve existing scalar leaves,
ordering, form/namespace rules and governing/actual type provenance. Assertions on
structural owners use a separate temporary partial-PSVI context carrying the actual
instance expanded name; they must never reuse an integer context with a dummy zero.

Initially admit namespace-uri(.) and namespace-uri(), including an explicitly bound
function prefix with the XPath functions namespace. Unprefixed calls use the fixed
XPath default function namespace. Compare their xs:anyURI result with a string
literal using =, !=, eq or ne, plus and/or and parentheses. Use XPath2.0 URI-to-string
promotion and codepoint equality. Preserve lexical URI spelling; no resolution,
case folding, percent normalization or network access. A function prefix rebound to
another namespace is Unsupported. xpathDefaultNamespace affects name tests, not
function namespace binding or the node's expanded name. General traversal, arbitrary
functions, string ordering and broader XPath remain excluded. Unsupported grammar
is rejected statically, including dead branches.

For non-simple structural content $value is the empty sequence under the host
assertion rules, but variable/sequence expressions are outside this initial grammar.
Do not expose the final typed value as the temporary assertion context's atomization.
No new structural inheritance, attributes, mixed content, lists, unions, wildcards,
imports/includes, identity constraints, defaults or full PSVI are admitted.

## Content varieties and native feasibility gate

| Content variety | Instance text rule | Final typed value | HasNoTypedValue |
|---|---|---|---|
| Empty | No character or element children, including whitespace | Present empty sequence | false |
| Element-only, including absent optional children | XML whitespace permitted; particle still enforced | Absent; atomization FOTY0012 | true |
| Existing supported simple content | Existing lexical/facet rules | Existing atomic value | false |

Comments and processing instructions remain an explicit profile exclusion; this is
not a claim that XSD empty-content validity forbids those information items.

The current provider issues single atomics and the session adapter assumes one.
Core public XML documentation also lists empty content among HasNoTypedValue
examples, contrary to XDM's distinction. This is a documentation discrepancy and a
runtime question, not evidence that either signed runtime supports or rejects it.
Before adoption, test both pinned signed Core versions using actual original nodes:
HasNoTypedValue=false, IsList=false, zero ordered AtomicItems, TryGetSingleAtomic=false,
correct governing/actual type identity, stable lifetime/authority and no fake atomic.
Query atomization must yield a genuine empty sequence wherever that query profile is
admitted. Public scalar-only reads must refuse a zero-atomic result rather than
inventing a scalar or reporting absent-value FOTY0012. Capture the actual refusal
category before freezing that API behavior; do not promise a new public result type.
If Core cannot represent/admit this faithfully, stop empty-type admission and retain
the original fixture as Unsupported until a separate public Core contract is agreed.
No Core source edit, pin change, list flag workaround or private interface is authorized
by this review. A structural-only subset must be separately measured and documented.

## Publication, limits and preservation

Every failed assertion, invalid empty-content instance, cancellation or Unsupported
input publishes no partial assessed facts. Recovery, disposal, foreign/copied nodes,
original source export and retained ownership guards remain mandatory.
Empty nodes count toward existing64node/4depth quotas even though they add no scalar;
keep16scalar/64attribute budgets, bounded16occurrences, eight effective assertions,
512expression characters,128tokens and16expression depth. Test exact boundaries and
one-over refusal; preserve existing cancellation checks and deterministic diagnostics.

Keep all2,770 current source identities, both1,930 package-consumer lanes, retained
124/23/33 lanes, public API54types/401rows and both signed Core pins unchanged.
These overlapping lanes do not constitute additive coverage. Re-run the original
21case pilot only after implementation; preserve all42files and original expectations.
The remaining assert_027_2 requires both inner empty typing and outer structural
assertions, so namespace-uri alone does not establish an admission.

## Adoption and delivery sequence

This packet pins source bodies and sections for review only. Record separate exact
experiment adoption before isolated native probes; retain initial failures. Then
record separate production adoption before implementation, full consumer/binary
qualification and measured pilot effects. No package release or broad XSD1.1/PSVI/
C45/performance acceptance follows from this bounded slice.
