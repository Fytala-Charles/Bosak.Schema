# M1 namespace and optional-assertion candidate

19 November 2026. The local M1 candidate adds namespace-aware expanded declaration names, qualified/unqualified local forms, QName assertion binding and zero-to-eight assertions within the finite decimal shape. All409source cases pass (315implementation/94public), including41authored clause-mapped cases and three real Core-session controls. Package-only94cases pass with Core1.81.2; a separate42case public lane passes with published Core1.81.17. Retained124/23/33binary catalogs pass with onlySchema.dll replaced; these lanes overlap. No Core runtime/API/source-wrapper pin, published package, version/tag or release-controller change.

## Supported increment

Schema target namespaces and local element form defaults/overrides are admitted
under the explicitly bounded [adoption](ADOPTION.md). Declaration and instance
names compare namespace URI plus local name; changing prefixes does not change
identity. URI case/escape spelling is preserved. Distinct expanded names sharing
a local part can coexist. Guarded root declaration annotations and provider
lookup now retain the same namespace identity.

Assertions may be absent. Existing finite assertions can use prefixed child
QNames; unprefixed names use the captured schema/assertion xpathDefaultNamespace
setting: local, target, declaration-site default binding or explicit absolute URI.
Unknown prefix/child and unsupported syntax refuse compilation. This admits no
arbitrary XPath, imports/references/named types, attributes, lists/unions, nil,
mixed/nested content, choice/all or general schema validity.

Namespace URIs are deliberately restricted to absolute, non-whitespace values;
reserved target namespaces and empty explicit targetNamespace refuse. These
profile restrictions are not presented as universal XSD rules. Existing decimal,
byte/node/lexical/token/depth/admission limits and cancellation/ownership remain.

A minimal admitted form is:

```text
xs:schema(targetNamespace?, elementFormDefault?, xpathDefaultNamespace?)
  xs:element(name=NCName)
    xs:complexType
      xs:sequence
        xs:element(name=NCName, type=QName(xs:decimal), form?, minOccurs?, maxOccurs?){1..16}
      xs:assert(test=FiniteExpression, xpathDefaultNamespace?){0..8}
```

Forms are qualified/unqualified. The explicit capacities and all other exclusions
are fixed by the adoption below; this is not a general XSD grammar.

## Clause and test evidence

[Specification bodies and hashes](SPECIFICATION-SOURCES.json) were captured before
source edits. Structures/XPath body hashes equal their original adoption pins;
Namespaces1.0 thirdedition20091208 is newly pinned. Full HTML bodies stay in ignored
local evidence, not the public repository. Original adoption/history is unchanged.

[CASE-MAP.json](CASE-MAP.json) maps41stable authored IDs to actual passed case names
and affected sections. Positive, supported-invalid and profile-refusal cases are
counted separately. These are authored regressions, **not** imported official W3C
suite IDs or a whole-spec percentage. The initial24-area/365case coverage inventory
remains a pinned baseline; CANDIDATE-DELTA.json records only the bounded increment.

Three additional implementation controls use real Core sessions: full declaration
lookup/foreign-schema identity, original facts/typed-value provenance plus FOTY0012
root atomization, and foreign input/fact/origin refusal. The external1.81.17lane
uses the same41public namespace cases plus one explicit runtime identity check;
it does not rewrite/suppress the minimum1.81.2consumer identity test. No friend
access/source override is used in either external package lane.

| Verification | Result | Scope |
|---|---|---|
| Full Schema source |409/0/0 |315implementation +94public;44new controls |
| Minimum-Core package consumer |94/0/0 |Candidate package +nineCore1.81.2 packages |
| Current-Core package consumer |42/0/0 |41namespace cases +runtime identity; nineCore1.81.17packages |
| Retained unchanged binaries |124/23/33pass |Legacy, alpha and signed-beta.1 consumer; onlySchema.dll changed |
| Release/provenance controls |54pass |32Python/22Node; no login/push |
| Workload integrity |6pass/tool build |No new timing or SLA claim |

Eighteen official Core signatures/source pins/archive-cache-runtime payloads match
in the two lanes. Core1.81.2source is7249d0e; Core1.81.17source is6a7618e9. The
published beta.1 archive signature/hash and original consumer DLL match. Candidate
archive/cache/runtime Schema payloads match in both lanes. [Identity](IDENTITY.json).
Core's optional source wrapper remains at9fd4e6a; its old260+9receipt is not newly
requalified or silently relabelled. No Core source/API changed, so no fresh Core
solution/QT3/XSLT/qt4 run is claimed. Existing gates remain required for engine changes.

## Compatibility and diagnostics

Previously refused valid namespace/zero-assertion schemas can now succeed. Wrong
namespaces remain Invalid; missing broader coverage remains Unsupported. Existing
stage codes/messages and enum symbols stay intact. Refusal probes that formerly
used namespace settings or missing assertions were deliberately moved to still-
excluded constructs (attributeFormDefault, choice). Complete sku/orders examples
now refuse broader declaration shape. Existing test counts are preserved, not
claimed byte-identical where expectations/input changed. Their original sealed
receipts remain historical. No automatic expansion of general C45/API acceptance.

Preliminary logs retain a local variable-name compile error, an empty-QName parsing
regression corrected before final qualification, and an editing encoding error
restored from original UTF-8 Git bytes. The deliberate zero-assertion admission
also required its old refusal expectation to change. Final409case run passes.

## Delivery

Local unsigned candidate: Bosak.Schema.0.6.0-alpha.0.3.nupkg; SHA-256
`987e17dd87c84353a8d856a66d78444f106579ceb4d9a25fdf140cf4d8ad0660`. It is uncommitted-source qualification only,
not a beta tag/artifact/publication decision. Explicit SOURCE.json binds source
bytes separately from the Git-base metadata. Existing0.6development line and
published0.5.0-beta.1 remain; no new version override or registry operation.

New work and shared planning are local/uncommitted in two isolated worktrees.
Review source/paired controls, then explicit Git delivery/hosted checks. Wider
named types/attributes/datatypes and structural rules remain future milestones.
The existing packaging/controller plan is historical and must use new reviewed
source/counts if resumed; do not dispatch the old beta.1 controller.
