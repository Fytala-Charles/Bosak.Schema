# M2C3 built-in exact integer candidate

19 November 2026. The [before-code adoption](ADOPTION.md) extends the merged
Boolean baseline20cc520; [PR21/main verification](M2C2-DELIVERY.json) passed671source
cases,54release controls and6workload controls. Its merge tree matches qualified
2d87423. This dossier records a subsequent local integer implementation; it is
not hosted CI for the new integer source. Older sealed dossiers remain historical.

## Bounded increment

NativeSchemaSession admits built-in xs:integer leaves alongside existing string,
Boolean and decimal leaves. Integer is a built-in restriction of decimal, not a
primitive datatype. Public changes are exactly NativeScalarKind.Integer=4 and
checked NativeScalarValue.IntegerValue:System.Numerics.BigInteger. Existing51public
types/379rows remain; wrong-kind getters refuse. NativeDecimalSession remains
decimal-only. Detached exact values survive owner disposal without authority.

Original parsed text has a fixed256UTF16code-unit ceiling before XML whitespace
collapse. Valid tokens have optional +/-, then one or more ASCII digits. Fractions,
exponents, non-ASCII digits, embedded whitespace and empty/sign-only forms are
Invalid when within the raw lexical ceiling. All supplied digits, including
leading zeroes but excluding sign, count against MaxNativeCoefficientDigits before
normalization; excess capacity is Unsupported/RepresentationCapacity. This is an
explicit representation limit, not the XSD totalDigits facet. Signed integers
therefore have at most255digits under the raw ceiling. Decimal scale and string
capacity do not constrain integer leaves. Separate Core query digit capacity
can refuse a read; bounded export and subsequent small reads remain usable.

Zero/-0/+0, repeated present values and absent optional leaves are distinguished
without fabricated scalars. XML export retains lexical signs/zeroes subject to
existing serialization, rather than imposing canonical XSD lexical output.
Actual node annotations/atomization and original typed facts carry integer values
without Int64/decimal/double narrowing. Declared/actual type and original assessment
identity, complex-rootFOTY0012, foreign/disposed guards and complete failure
non-publication are checked through published Core. Existing finite schema/XML/
node/sequence/owner/gate/cancellation/admission/output limits remain.

All integer assertion references, even count/exists/empty, refuse compilation.
Decimal sibling assertions/facets remain enforced. Derived integer families,
named integer restrictions/facets, attributes/imports, defaults/fixed/nil,
list/union and nested simple content remain excluded. No general derivation,
integer arithmetic/comparison/assertion adoption or broader Core engine support
is inferred from exact transport. [Pinned sources](SPECIFICATION-SOURCES.json).

## Qualification

| Lane | Passed | Scope |
|---|---|---|
| Source |748 |347implementation+401public; no failures/skips |
| Minimum-Core package-only |401 |Signed published1.81.2; no project references |
| Current-Core package-only |71 |68authored+2public controls+runtime identity on1.81.17 |
| Retained unchanged callers |124/23/33 |OnlySchema.dll replaced; overlapping catalogs |

[Results](RESULTS.json) and [case map](CASE-MAP.json) record68authored cases: 25
positive,27 supported-invalid,6 instance-refusal
and10 compile-refusal. Two public contract/capacity-recovery
controls and seven actual Core controls bring the new source increment to77.
[Intentional regression deltas](REGRESSION-DELTA.json) move two former integer
refusals to valid compile/invalid unchanged text and move the generic public
refusal probe to xs:date. Original671baseline controls remain required; sealed
prior expectation receipts are not rewritten. These authored regressions are
not official W3C cases, whole-spec percentages, application acceptance or SLAs.

Release build has zero warnings/errors. Local unsigned package0.6.0-alpha.0.9
is a qualification artifact; repository metadata identifies baseline20cc520 only.
[SOURCE-MANIFEST](SOURCE-MANIFEST.json) identifies actual uncommitted source.
[Identity checks](IDENTITY.json) verify18Core signatures/source pins/archive-cache-
runtime payloads, candidate package/cache/runtime Schema payloads and original
signed beta.1 inputs. [Retained inputs](RETAINED.json). No Core engine/API/
dependency/source-wrapper pin, tag/controller or publication change is included;
existing Core conformance gates are not newly claimed for this Schema-only slice.

[Public API comparison](API-SURFACE.json) preserves all379rows/51types and adds
exactly two rows. [Example results](README-RESULTS.json) cover all six actual
packaged C#blocks, older namespace/named-decimal controls, exact9223372036854775808
with lexical+09223372036854775808export and the earlier proposed consumer.
Documentation hygiene, programmatic rendering/local links and raw evidence hashes
are recorded separately; no visual or general API/C45 acceptance is implied.

Source remains local/uncommitted under AGENTS.md. Commit/push and publication need
their explicit scope. Next: review/deliver this slice, then adopt selected string
restrictions/facets before attribute support. REQ009/CoreREQ142 remain In Progress.
