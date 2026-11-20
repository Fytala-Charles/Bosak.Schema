# Proposed bounded direct integer restrictions

20 November 2026. **Proposed, not semantically adopted or implemented.**
Base: merged PR30 `5bfbb1e798cdf18bac6ac7bb15705b1a0e50bc65`.
Owner priority remains specification coverage and useful application integration
across Bosak and Bosak.Schema, ahead of a new package publication.

## Normative review targets

The exact dated XSD 1.1 Structures/Datatypes Recommendation bodies and hashes are
preserved in [the original review receipt](../conformance-pilot/SPECIFICATION-REVIEW.json):

| Source | SHA256 | Review sections |
|---|---|---|
| Structures 20120405 | `5ffb773f81fb81cfc9ac54dfb871c333cefb12fbdffe4caeb28170054b21fc6a` | 3.3.2–3.3.4 declarations, 3.16.1–3.16.4 atomic restriction/identity |
| Datatypes 20120405 | `cc2ae20cdeab02600c73fd13f1cc31dbbdd61f62aa455026fa84388b540443b8` | 3.4.13 integer and fixed facets; 4.3.6 whitespace; 4.3.7–4.3.12 range/digit facets and component constraints |

The [dated integer definition](https://www.w3.org/TR/2012/REC-xmlschema11-2-20120405/#integer)
uses signed ASCII digit lexical forms and fixes fractionDigits to zero and
whiteSpace to collapse. Review these normative rules before deciding the exact
compile grammar and diagnostics. An integer's mathematical derivation from decimal
does not justify converting bounded exact integer values through .NET decimal.
Preserve earlier XDM and primitive-integer adoptions; record a separate immutable
semantic adoption before implementation. Review pins are not new adoption evidence.

## Proposed scope

Only NativeSchemaSession: direct global named simpleType restrictions and the
already admitted anonymous local element simpleType route, based on xs:integer.
Use exact BigInteger storage/comparison, original named/anonymous type handles,
existing lexical/supplied-digit capacities, original scalar indexes and prior
required/optional/repeated/nested declaration rules. Retain global16/local16,
depth4, terminal16, defensive node64 and existing attribute-use limits.

Candidate facet subset: minInclusive, minExclusive, maxInclusive, maxExclusive,
totalDigits and explicit fractionDigits only when consistent with inherited fixed
zero. Omitted fractionDigits still inherits zero; reject attempts to widen it.
Keep inherited whitespace collapse without adding arbitrary whiteSpace overrides.
Define exact facet lexical/value constraints, duplicate/mutual-exclusion rules,
empty integer value spaces, digit counting for zero/negative/leading-zero input,
signed-zero facet forms and resource limits in the adoption. Do not copy decimal
facet parsing unchanged: range values must be appropriate to the integer base,
and decimal scale handling must not introduce fractional or truncated integers.

Preserve representation-before-facet gating: out-of-capacity input is Unsupported;
an admitted integer violating a supported facet is Invalid with no partial facts.
Keep schema component errors/refusal categories separate from instance validity.
Integer assertions, restriction chains, built-in derived integer families,
enumeration/pattern/list/union, anonymous attribute/root scalar schemas and general
derivation/type overrides remain outside this first proposal. Existing attribute
type references must not silently discard facets if named integer restrictions
become available there; either enforce them through the shared scalar route or
explicitly refuse that use in the adoption.

Legacy NativeDecimalSession and existing decimal/string/numeric behavior remain
unchanged. No public API, Core pin, general typed-node query profile, package/tag,
publication/controller or C45/full-specification acceptance change is proposed.

## Before-code and qualification gates

1. Resolve the facet/component decisions above and record exact adoption plus
   intentional limits before production edits. Identify every prior refusal test
   affected by named/anonymous integer restrictions and pin original rows/sources.
2. Prove before-code native named/anonymous restriction refusals while built-in
   integer and decimal/string restrictions remain admitted. Existing signed Core
   integer/anonymous transport evidence is relevant but is not native facet proof.
3. Add authored happy/edge cases for every selected facet, values beyond decimal
   precision, negative/zero/leading-zero spellings, fixed fractionDigits, invalid
   fractional range values, empty intersections, capacities and eager unused
   definitions. Check named/anonymous identity, shared/distinct definitions,
   attribute type uses, optional/repeated/nested owners, namespace binding,
   authority/expiry, legacy refusal, hint/type/nil boundaries and cancellation.
4. Run source/package-only minimum/current-Core, retained binary and unchanged
   API qualification with exact source/runtime/signature evidence. Totals overlap.
5. Rerun all 21 unchanged upstream cases. The two integer maxInclusive cases are
   eligibility targets only; their metadata expects one Valid and one Invalid.
   Never edit fixtures, expectations, selection or earlier sealed receipts.
   Expected future matches must not be reported before execution.
6. Sync consumer/canonical docs and rendered links; obtain the applicable explicit
   commit/push request before delivery. No registry activity belongs to this slice.
