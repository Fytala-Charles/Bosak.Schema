# Bounded decimal simple-content root candidate

20 November 2026. [Before-code adoption](ADOPTION.md) and [exact contract](CONTRACT.md)
extend merged Schema d4eca99. [PR24/main verification](ROOT-ATTRIBUTE-DELIVERY.json)
passed950source cases/54release controls/6workload controls; the merge tree matches
qualified f01795a. This dossier records a subsequent local candidate, not hosted
CI for the new code, publication, general API/C45 acceptance or full XSD support.
Older sealed attribute/datatype dossiers remain unchanged.

## Useful increment and API

The isolated [PriceType schema](price.xsd) copies the existing public fixture's
PriceType and price declaration unchanged in text (line endings normalized),
with its original namespace header. [Origin](FIXTURE-ORIGIN.json). Its
[price/currency XML](price.xml) now assesses natively. The whole original orders.xsd
still refuses its other unsupported definitions; no general invoice/application
acceptance is implied by selecting this supported subset.

Only opt-in anonymous/named root complex types with one simpleContent/extension,
directly based on xs:decimal or an admitted named decimal restriction, are added.
Root attributes/forms/limits reuse the prior adoption. Restriction or complex-base
chains, other content base families, assertions in the extension, nested/mixed
content, defaults/fixed/groups/wildcards/imports remain excluded. NativeDecimalSession
retains its old refusal and all950prior source outcomes remain unchanged.

HasRootScalar and ReadRootScalar(CancellationToken=default) are the only public
additions. No new type/options/enums. Root decimal text is not inserted into leaf
or attribute indexes: a simple-content root has zero leaf scalars, existing root
attributes retain declaration-order indexes, and existing element-only roots still
have no typed value (FOTY0012). HasRootScalar=false and ReadRootScalar refuses with
fixed NotSupportedException("FOTY0012") on those old roots. Availability does not
promise a read will fit separate query/cache limits. Detached decimal values
survive owner disposal without authority.

## Original identity and normalized values

Complex root annotation/governing type stays PriceType (or the original anonymous
complex handle), while ActualType on its atomic provenance is decimal or the
admitted named decimal content type. DeclaredType and ActualType intentionally
differ. Complex status is independent of typed-value absence. Original root
declaration metadata remains; local attributes keep their own simple type path.
The provider does not manufacture a general derivation relation or broaden its
adopted typed-identity query environment. Actual native data(.) atomization and
original typed reads/evaluation are qualified separately from synthetic feasibility.

Root string-value is schema-collapsed lexical decimal text, not canonical numeric
spelling; exported XML retains parsed lexical text under the existing bounded
writer policy. XML normalization comes first. Raw parsed text256UTF16and supplied
numeric digits/scale/facets stay bounded and are enforced before normalization.
Invalid lexical/facet/nested elements issue Invalid, representation excess or
excluded comments/PIs/grammar issue Unsupported. CDATA is text. No root/attribute
facts or document publish until complete success. Existing input/schema/node/type/
root/attribute/admission/query/cache/export/gate/cancellation/revocation/cleanup
bounds remain. Root/attribute reads share finite cache; failure leaves in-bound
reads/export usable. The old element-only root behavior is unchanged.

## Qualification

| Lane | Passed | Scope |
|---|---|---|
| Full source |1036 |386implementation+650public; zero failures/skips |
| Prior regressions |950 |All old outcomes unchanged before adding new tests |
| Minimum-Core package-only |650 |Signed published1.81.2; no project references |
| Current-Core package-only |73 |67authored+5public controls+runtime identity on1.81.17 |
| Retained callers |124/23/33 |OnlySchema.dll replaced; overlapping catalogs |

[Results](RESULTS.json), [case map](CASE-MAP.json) and [empty regression delta](REGRESSION-DELTA.json)
keep scopes explicit.67authored cases comprise22positive,18supported-invalid,
10instance-refusal and17compile-refusal; five public controls,
12actual native Core controls and two disposal-waiting cases bring the new increment
to86. These authored/repeated catalogs are not official W3C cases, whole-spec
percentages or performance acceptance. Existing cleanup-fault tests stay green.

Before code,37public-only synthetic probes pass on each signed minimum/current
Core:27retained typed/attribute controls and10new complex-root/decimal controls.
They have no Schema package/project/friend/private API and Assess throws if called;
they qualify metadata/transport, not native XSD validity. Five before-code baseline
checks show current simple-content refusal/absent new members/old index behavior.
They are historical inputs, not extra Schema cases; reproduce baseline tests at
d4eca99 before-code source, not this now-extended working tree. Synthetic root
metadata is distinct from the actual native root qualification above.

Release build has zero warnings/errors. Local unsigned0.6.0-alpha.0.12 is a
qualification artifact whose metadata identifies base d4eca99; [source hashes](SOURCE-MANIFEST.json)
pin actual uncommitted files. [Identity](IDENTITY.json) verifies18official Core
signatures/source revisions/archive-cache-runtime payloads, candidate payloads,
retained inputs and original signed beta.1 baseline. [Probe identity](PROBE-IDENTITY.json)
verifies the same signed archives and actual probe runtime DLLs independently.
[Retained inputs](RETAINED.json). No Core engine/API/pin, registry/tag/controller or
published beta.1 change, fresh Core conformance gate or hosted1036case claim.

[API comparison](API-SURFACE.json) preserves52types/all387prior rows, adds exactly
the two proposed members and removes/changes none (389rows total).
[Nine actual packaged C#examples](README-RESULTS.json) execute including the root
12.5/zero-leaf/EUR workflow; earlier namespace/named-type/facet/Boolean/integer/root
attribute examples and prior illustrative consumers still execute. Canonical docs,
strict hygiene, programmatic rendering/local links and evidence hashes are recorded;
no visual acceptance or overall API/C45 freeze is implied.

Source is local/uncommitted under AGENTS.md. Next: commit/push/PR review; after
delivery, nested complex content needs a separate contract/adoption and original
node/consumer qualification. Specification coverage remains the priority.
