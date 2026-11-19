# M2C2 primitive Boolean candidate

19 November 2026. [Before-code adoption](ADOPTION.md) on Schema16355ea,
after [PR20 delivery/main verification](M2C1-DELIVERY.json). The merged string
facade's tree equals qualified331ae2f; fresh main CI44592483458 passed614source
cases and54release controls. This dossier records a subsequent **local** Boolean
implementation, not main CI for that new code. Prior sealed receipts remain intact.
No Core engine/API/dependency/source-wrapper pin, controller, tag or publication change.

## Bounded increment

NativeSchemaSession admits primitive xs:boolean leaves alongside strings/decimals.
NativeScalarKind.Boolean=3 and checked NativeScalarValue.BooleanValue:bool are the
only added public members; old enum values/members and NativeDecimalSession's
Boolean refusal stay unchanged. NativeScalarValue is still producer-only and
detached, with fixed wrong-kind getter refusal and no transferable authority.

After XML parsing, the Boolean leaf text is capped at256UTF16code units before
collapsing XML whitespace only. Exact true/1 map totrue; false/0 map tofalse.
Other representable tokens are Invalid; capacity overflow is Unsupported before
collapse. This lexical limit is a profile restriction, not an XSD facet and not
MaxStringCharacters. Every present repeated leaf is assessed; absent optional
leaves create no false placeholder. XML export retains lexical forms subject to
existing serialization rules; values are not serialized canonically by this slice.

Primitive Boolean facts/annotations and declared/actual provenance use a distinct
owned xs:boolean handle. All four lexical forms are qualified through actual
Core typed reads/evaluation/node atomization, including false and rootFOTY0012.
Foreign/disposed state and failed-assessment no-publication guards remain. Existing
owner/gate/cleanup, numeric/string/output limits and decimal assertions remain.
Every Boolean assertion reference (even count/exists/empty) and Boolean function
syntax refuses. Named Boolean restrictions/facets, integer, attributes/imports,
nil/default/fixed and nested content stay outside this adoption.

## Qualification

| Lane | Passed | Scope |
|---|---|---|
| Full source |671 |340implementation+331public; zero failures/skips |
| Minimum-Core package-only |331 |Published Core1.81.2; no project references |
| Current-Core package-only |52 |50authored+legacy/enum+runtime identity on1.81.17 |
| Retained unchanged callers |124/23/33 |OnlySchema.dll replaced; overlapping catalogs |

Fifty new authored cases comprise16positive,22supported-invalid,
3instance-refusal and9compile-refusal controls, plus one public
legacy/enum control and six actual Core controls. [Results](RESULTS.json),
[case map](CASE-MAP.json) and [intentional old-case delta](REGRESSION-DELTA.json)
keep scopes explicit. The old Boolean-declaration refusal case now compiles and
its old identifier text is Invalid; a generic refusal probe moved to still-excluded
integer. Original sealed M2C1 case/qualification records remain historical.
These are authored regressions, not official W3C suite cases or whole-spec percentages.

Release build has zero warnings/errors. Local unsigned0.6.0-alpha.0.8 is a
qualification artifact; its repository metadata remains baseline16355ea and
SOURCE-MANIFEST pins uncommitted changes.18official Core signatures/source/payload
identities, local package/cache/runtime payloads and original signedbeta.1 input
match. [Identity](IDENTITY.json), [retained inputs](RETAINED.json). Existing release
controls/Core gates are not newly claimed for this local slice because their
sources did not change; fresh PR20 main614/54 is recorded separately above.

[API comparison](API-SURFACE.json) preserves377prior public rows/51types, with
exactly two additions and no removed/changed row. Five actual packaged README
C# blocks compile and execute, including false-from0 with unchanged XML spelling;
older namespace/named-decimal/scalar examples and the prior proposed consumer
still execute. [Example results](README-RESULTS.json). Strict documentation,
programmatic rendering/link checks and source/evidence hashes are retained;
no new visual acceptance, performance SLA or general C45/API freeze is implied.

Next datatype is exact integer under separate adoption; Boolean assertion
expressions remain separate. REQ009/CoreREQ142 stay In Progress. New Boolean
work is local/uncommitted under AGENTS.md; commit/push/release need their explicit scope.
