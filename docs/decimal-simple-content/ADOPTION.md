# Decimal simple-content root adoption - 20 November 2026

Recorded before product edits under the user request to continue the simpleContent
recommendation. Base Schema d4eca99db337187425f56d9d622d6226489b99cd is merged PR24,
qualified f01795a tree; main950/54/6 passed. [Concrete contract](CONTRACT.md) defines
exact API/profile constraints. Fresh dated specification bodies and SHA256 pins
are in SPECIFICATION-SOURCES.json; prior adoption/fixture origins/review dates
remain separate and no later errata/moving latest text is silently adopted.

Adopt bounded rules from XSD Structures20120405 3.4.1,3.4.2.1-.2/3.4.2.4,
3.4.3-.4,3.3.4 and3.2/3.5 for direct decimal simple-content extension with root
attributes; existing Datatypes3.3.3/4.3.6 and selected named-decimal facets govern
lexical/value processing. XML1.0fifth 2.11/3.3.3/4.1 precedes schema normalization.
XDM3.1dated20170321 3.3.1.2/6.2 governs complex simple-content typed-value and
schema-normalized string-value, retaining complex annotation with actual decimal
atomics. Partial profile only, not whole-section/full-XSD or C45/API acceptance.

Only opt-in root complex types with one simpleContent/extension child and a direct
xs:decimal/admitted named decimal base are admitted. Reuse0..16root attributes,
combined16global types/16roots, finite XML/schema/node/admission/query/cache/export
limits, raw scalar256UTF16and supplied numeric digit/scale limits. No simpleContent
restriction, complex base/derivation chain, other base families, inline simple
content type, mixed/nested content, assertions, special requests/default/fixed/
imports/groups/wildcards are added. NativeDecimalSession stays decimal-only with
its old simple-content refusal. Exact existing declaration-name grammar is retained.

Two members only: HasRootScalar and ReadRootScalar(CancellationToken=default).
Leaf and attribute counts/indexes never include or shift for the root scalar.
Complex IsComplexType is decoupled from HasNoTypedValue: price root has both true
complex status and an actual typed decimal; old element-only root still has no
typed value and FOTY0012. Root facts/declaration/annotation stay original complex
identities, issued governing/declared type complex and actual atomic type decimal
or named decimal base. No replacement by a primitive node annotation. Public-only
37case probes per signed Core lane qualify transport separately before code;
synthetic issuers do not establish native XSD validity.

Numeric root text is schema-collapsed only after raw256UTF16capacity and XML
capture limits. Actual value/facet checks reuse existing exact decimal logic.
No nested element =>Invalid; comments/PI unsupported; empty text and malformed
in-bound lexical forms =>Invalid. Root attributes then scalar then publication;
all failures/cancellation issue no partial fact/document. XML export stays parsed
lexical spelling; root string-value is collapsed lexical text, not forced numeric
canonical text. Owner/lease/gate/revocation/provenance/foreign/disposed/read-capacity/
cleanup rules remain. Record current baseline refusals and absent API before code,
then qualify actual native nodes and preserve all prior outcomes where unaffected.
