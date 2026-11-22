# Structural namespace assertion native feasibility

22 November 2026. Isolated candidate on PR38 merge
`2effee5bf9daa317599c9285ba2de5e333bc786f` after
[experiment-only adoption](assertion-feasibility-evidence/EXPERIMENT-ADOPTION.json).
[Scope](FEASIBILITY-SCOPE.md). The earlier proposal and empty-value evidence remain
sealed separately; production source, fixtures, dependencies and public API are unchanged.

## Measured qualification

**114 native controls pass per signed Core pin**, 1.81.2 and 1.81.17:
54 controls execute the unchanged27proposal payloads through both facades;29repeat
the empty-value controls;21cover namespace contexts and exact quota boundaries;
10cover named-root invalidity, original upstream input, authority, legacy routing
and bounded xsi:type self-selection. These overlapping lanes are not additive coverage.

All **2,770 unchanged source tests pass** (840 implementation /1,930 public).
Every original case identity is preserved, including authored IDs and theory selectors;
no runtime fixture reconciliation or expectation change was needed.
[Stable identities](assertion-feasibility-evidence/STABLE-CASES.json),
[execution receipts](assertion-feasibility-evidence/EXECUTION.json).

The candidate has57source files from56original Git blobs with six explicit overlays:
five modified copies and one new internal helper. The structural parser admits only
namespace-uri()/namespace-uri(.) with correctly bound function prefixes, equality or
inequality against string literals, and/or and parentheses. Static parsing rejects
excluded syntax even in dead branches. It captures original namespace/default/base
context and evaluates against the actual instance expanded name, without synthetic
integer/scalar typing. Namespace URI strings preserve relative spelling, case and
percent escapes. Function default namespace is independent of xpathDefaultNamespace.
Legacy child-value/string and integer assertion routes remain separate. This first
finite function-name lexer admits ASCII prefix names; other valid XML prefix forms
remain Unsupported and need an explicit follow-up before wider QName coverage.

Character512/513, token128/129, expression-depth16/17, assertion8/9, node64/65 and
node-depth4/5 controls pass. Zero-scalar empty nodes consume node/depth quotas.
New structural/application-attribute combinations remain explicitly Unsupported;
existing scalar/attribute quotas and legacy semantics retain their regression coverage.
Named/anonymous original empty values remain non-list zero-atomic containers; data()
returns empty, scalar APIs/typed identity refuse, and element-only FOTY0012 stays distinct.
False nested/root assertions publish no facts, evidence or snapshot. Original authority,
foreign/copy refusal, cancellation/recovery, disposal and export controls pass.

The original frozen `Assert/assert_027/assert_027_2` schema/instance passes a direct
native probe on both pins: outer element-only facts and inner present-empty typing
are verified. This is **not a new frozen-pilot result**. The retained pilot remains
20matches/1Unsupported/zeroerrors until a separately measured production run.
All42original corpus files/expectations remain unchanged.

All18Core package signatures, source commits and archive/cache/runtime identities
verify. The public surface remains54types/401rows with zero delta against the
previously qualified local alpha.0.28 assembly; no package was produced or published.
[Identity evidence](assertion-feasibility-evidence/IDENTITY.json),
[API comparison](assertion-feasibility-evidence/SURFACE.json),
[source delta](assertion-feasibility-evidence/CANDIDATE.diff).

## Initial findings and preserved corrections

The expanded probes exposed a real defect in the earlier scratch empty-value overlay:
a named root's copied plan lost NestedProfile, allowing whitespace under empty content.
Before correction, four native controls per pin measured2pass/2fail. Preserve the
original receipt; the new candidate retains the route and all four now pass. This was
not an admitted production behavior; neither earlier sealed evidence nor production
source was rewritten. The earlier29controls did not cover named-root invalid content.

An initial overlay script stopped at an incorrect source anchor before any compile;
a boundary probe had XML string-quoting compile errors. The API helper initially
compared assembly-qualified generic by-reference parameter names, producing a
version-only difference; recursive structural normalization fixes the helper, with
no product signature change. Initial source/logs/TRX and construction-failure notes remain separately retained.
The assertion dispatcher also explicitly excludes quoted namespace-uri spellings
from new routing; three positive legacy string controls verify this preservation.

## Remaining gates and next step

This establishes bounded native feasibility and unchanged source regression behavior.
Production adoption and implementation are still pending. Package-only consumer lanes,
retained124/23/33 callers, measured frozen-pilot effects, production API/binary evidence
and release documentation must be qualified after a separate production adoption.
No full XSD1.1/PSVI/C45/performance or publication acceptance follows from this result.
The Core public XML empty-content example remains a separate documentation correction.

Next: record exact production adoption, implement the reviewed bounded slice, then
run full source/package/retained/pilot qualification. No commit/push/tag/publication
is requested or performed for the proposal and these experiments.
