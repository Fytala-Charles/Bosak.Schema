# Anonymous local decimal/string restriction implementation

20 November 2026. Uncommitted candidate on merged PR29 base
`1438f719863d0c13f976f1b0eb683e146a3511e5`.
The [before-code adoption](../ADOPTION.md) and
[signed public Core feasibility](../FEASIBILITY.md) remain unchanged.

The opt-in `NativeSchemaSession` now admits local element particles with one
anonymous simpleType directly restricted from decimal or string. Shared named
and anonymous parsing reuses the existing exact numeric and Unicode/string
facet implementations. Original unnamed type identities are distinct per source
definition and reused with shared enclosing types/repeated instances; no fake
QName, primitive handle substitution or public API is introduced.

All local anonymous simple/complex definitions share the existing 16-definition
budget, counted once per source site including unused named definitions. Global
budgets, depth/terminal/attribute capacities, legacy decimal refusal, QName/form
resolution, optional/repeated values, assertion boundaries and hint/type/nil
policy remain protected. Integer/Boolean derivation, attribute/root anonymous
simple types, restriction chains, pattern/whiteSpace/list/union and broader XSD
features remain excluded. Public types/rows remain exactly **52/392**.

## Qualification

| Lane | Result | Scope |
|---|---:|---|
| Source | 1,373 pass: 421 implementation / 952 public | 1,301 unaffected prior cases; one adopted NS-051 admission; 71 new cases |
| Minimum-Core package consumer | 952 pass | Unsigned local Schema package; nine signed Core1.81.2 packages; no project references |
| Current-Core package consumer | 952 pass | 951 shared public cases plus separate1.81.17version control; minimum-version-only assertion excluded |
| Retained binary callers | 124 / 23 / 33 pass | Only Schema.dll changed; other files byte-identical; catalogs overlap |
| Frozen upstream pilot | 7 matches / 14 Unsupported / 0 mismatches / 0 errors | Same21cases, expectations and42original files; six targeted decimal/string cases newly match |
| Pilot integrity controls | 15 pass | Hash/source/runtime/metadata/error classification controls, not conformance cases |

The 60 new authored public catalog cases plus five capacity/legacy/recovery tests
and six actual native Core-fact controls cover happy/edge behavior. Actual facts
retain null QName annotations, original governing/declared/actual handles,
shared/distinct identity, authority/expiry and typed scalar storage. Full source
outcomes, the explicit original NS-051 schema/instance preservation and replacement
of its unreachable seven-value placeholder with one `12.5` scalar are recorded
in [REGRESSION-DELTA.json](REGRESSION-DELTA.json).

[PILOT-TRANSITIONS.json](PILOT-TRANSITIONS.json) records six validity advances
and two reason-only changes: integer restrictions remain Unsupported, now with
ValueType instead of the earlier generic DeclarationShape refusal.
The two anonymous integer cases and other independent namespace/assertion/complex
restriction/xsi:type cases remain Unsupported. Seven selected validity matches
are not full XSD, whole-clause or PSVI conformance and do not justify a percentage.

## Query route limits

The native facade's qualified route remains owned scalar reads. Native node
queries such as `data($node)` remain refused by its existing scalar-only advisor
binding; no query profile expansion was adopted. An exploratory legacy XPath
`data(.)` call without the owned schema session treated an absent QName annotation
as untyped and returned a string for an anonymous decimal. This is recorded as
an unqualified integration route, not worked around by inventing a type name.
Synthetic public Core transport can carry anonymous node facts, but that does
not widen the native advisor contract. See [ROUTE-LIMITS.json](ROUTE-LIMITS.json).
General Core C45/full schema-aware expression acceptance remains separate.

## Reproduce and identities

```powershell
dotnet test Bosak.Schema.slnx -c Release
dotnet build tools/Bosak.Schema.ConformancePilot/Bosak.Schema.ConformancePilot.csproj -c Release
python tools/run-xsd-conformance-pilot.py --output .guard-tmp/anonymous-new-run --runner tools/Bosak.Schema.ConformancePilot/bin/Release/net10.0/Bosak.Schema.ConformancePilot.dll --candidate-evidence docs/anonymous-simple-restrictions/implementation/CANDIDATE.json
```

[CANDIDATE.json](CANDIDATE.json) pins changed production source (including the
new file), exact runtime bytes and the pilot's original product base; the case
manifest itself stays frozen. Rebuilt/check-out bytes may require a new reviewed
candidate receipt rather than modifying this historical one. The local package
is `0.6.0-alpha.0.17`, unsigned and for compatibility qualification only; package
hashes, all18Core signatures/source/archive/cache/runtime checks, signed published
beta.1 baseline and retained byte comparisons are in
[IDENTITY-FINAL.json](evidence/IDENTITY-FINAL.json).
Original projects, signature logs and TRX receipts are under evidence. Package
consumer project paths record the original worktree; rebind source/test-data paths
in a scratch copy for reproduction and keep receipts unchanged. The current-Core
lane excludes only `PublicSurface_NoFriendGrantOrPublicFactoriesForAuthority`,
whose minimum1.81.2version assertion is replaced by `CurrentCore_IsRecordedVersion`.
Minimum-source authority/public-factory checks still pass in the full suite.
No registry publication, tag/controller/Core pin or public API changes occurred.

Next: review and deliver this bounded slice, then choose the next specification
gap from the remaining unchanged cases and the separate query-route limitation.
