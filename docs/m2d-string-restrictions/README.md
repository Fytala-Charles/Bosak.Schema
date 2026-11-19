# M2D1 named string restriction candidate

19 November 2026. [Before-code adoption](ADOPTION.md) on merged integer
base014cb28 follows [PR22/main verification](M2C3-DELIVERY.json):748source tests,
54release controls and6workload controls passed. Its merge tree equals qualified
1b97efc. This dossier records a subsequent local string restriction slice;
that main run does not qualify this new code. All earlier sealed dossiers stay intact.

## Bounded increment

Only NativeSchemaSession admits global named simple types directly restricted
from xs:string, with length, minLength, maxLength and enumeration. Empty direct
restrictions are allowed. NativeDecimalSession remains decimal-only. Existing
named decimal restrictions, numeric/string/Boolean/integer values and decimal
sibling assertions/facets retain their behavior. The entire public surface stays
unchanged:51types and381rows; no new member, option or type is introduced.

Length counts Unicode scalar values: supplementary characters count one and a
base character plus a combining mark counts two. Parsed XML string whitespace and
Unicode sequence are preserved. MaxStringCharacters remains a separate UTF16
representation bound, enforced before instance facets; it also bounds each
parsed enumeration value at compile. Enumeration compares exact ordinal string
values without trimming, normalization, case/culture folding or grapheme guessing.
Literal XML attribute whitespace normalizes before facet interpretation;
referenced tabs/CRs retain their parsed characters. Export/reopen preserves those
parsed values under the existing bounded writer policy.

Length facet values accept nonNegativeInteger spellings, including signs/leading
zeroes/-0, under raw256UTF16lexical and value0..65536capacity limits. Duplicate
singleton facets, malformed/negative/conflicting bounds refuse whole compilation.
For direct primitive-string restrictions, length cannot coexist with minLength/
maxLength: the standard's allowed coexistence requires an inherited bound from an
earlier restriction, and chains are excluded. Min/max can coexist when consistent.
Enumeration entries are alternatives, including duplicate values, under16supplied
entries per named type (duplicates count). Enumeration intersects with other
facets: an entry outside the same restriction's length is permitted in the schema
but cannot make that instance pass length. [Pinned sources](SPECIFICATION-SOURCES.json).

All named string references remain excluded from native assertions, including
count/exists/empty routes. Pattern/XSDregex, explicit whiteSpace/fixed flags,
restriction chains, anonymous simple types, derived string built-ins, attributes/
imports/list/union/default/nil remain outside this adoption. No .NET regex fallback
or general derivation relation is introduced. Existing combined16global types,
16roots and schema/XML/node/sequence/ownership/admission/cancellation/output limits
remain. Every present/repeated leaf is assessed; absent optional leaves emit no
scalar; invalid/refused assessment issues no partial document or original facts.

Named leaf annotations and declared/actual type facts retain one original named
component per schema, distinct from primitive string and same-spelled foreign
schemas. Actual atomic values remain strings. Original Core reads/evaluation and
node atomization, shared component reuse, complex-rootFOTY0012, foreign/disposed
refusal and detached string/export lifetime are checked; public values carry no
transferable schema authority.

## Qualification

| Lane | Passed | Scope |
|---|---|---|
| Source |837 |355implementation+482public; no failures/skips |
| Minimum-Core package-only |482 |Signed published1.81.2; no project references |
| Current-Core package-only |82 |80authored+legacy control+runtime identity on1.81.17 |
| Retained unchanged callers |124/23/33 |OnlySchema.dll replaced; overlapping catalogs |

[Results](RESULTS.json) and [case map](CASE-MAP.json) retain80authored cases:
34positive,14supported-invalid,2instance-refusal and
30compile-refusal; plus one public legacy/representation control
and eight actual Core controls. [Intentional old-case delta](REGRESSION-DELTA.json)
changes M2C1-42 from refused named empty string restriction to valid compile/
assessment with its unchanged XML and expected values. Older sealed receipts
remain historical; all748baseline controls remain present with that explicit expectation change. These are
authored tests, not official W3C suite cases, specification percentages or
real-application/performance acceptance.

Release build has zero warnings/errors. Local unsigned0.6.0-alpha.0.10 in the raw
packet's final-feed is a qualification artifact. Repository metadata identifies
base014cb28 only; [source hashes](SOURCE-MANIFEST.json) pin actual uncommitted
files. [Identity](IDENTITY.json) verifies18Core signatures/source pins/archive-cache-
runtime payloads, candidate package/cache/runtime Schema payloads and original
signed beta.1 inputs. [Retained inputs](RETAINED.json). No Core engine/API/
dependency/source-wrapper pin, registry/tag/controller or published beta.1 change.
No new Core conformance-gate or hosted string-slice CI claim is made.

[API comparison](API-SURFACE.json) shows zero added/removed/changed rows.
[Seven packaged C#examples](README-RESULTS.json) execute, including a named
Unicode label of two characters/three UTF16units, older namespace/named decimal/
Boolean/exact integer examples and the previously proposed consumer. Documentation
hygiene, programmatic rendering/local links and source/evidence hashes are
recorded separately; no visual acceptance or general API/C45 freeze is implied.

Source remains local/uncommitted under AGENTS.md. Next: review/deliver this slice,
then bounded attribute support under separate adoption; XSDregex/pattern remains
its own work. REQ009/CoreREQ142 remain In Progress. Commit/push and publication
require their explicit scope.
