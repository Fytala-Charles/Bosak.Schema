# After bounded xsi:type: merge verification and next specification slice

21 November 2026. PR34 is merged as ee32ac81c7a704c95647b121d1cde23cfa32dcbd.
Its tree exactly equals the reviewed9d48071 tree. Main hosted run44871576728
passes2,024source tests(550implementation/1,474public), release controls,
documentation hygiene and workload integrity. [Merge receipt](MERGE.json),
[main CI](MAIN-CI.json). No release/tag/controller/Core pin/publication change.

## What the remaining cases actually require

The prior exact frozen21-case corpus remains14matches/7Unsupported/zero mismatch
or harness errors. This step analyses the seven original inputs; it does not
rerun the pilot or change its classifications. All referenced schema, instance
and metadata hashes verify against the unchanged manifest. [Case map](GAPS.json).

| Rank | Coherent work | Current frozen cases | Required behavior and limits |
|---|---|---|---|
| 1 | Schema namespace/form declarations | Two ElemDecl/typedef00202m cases; one separate first gate in assert006 | Review nonempty relative namespace names and accept legal attributeFormDefault even when the schema has no application attributes; preserve exact expanded names, no acquisition and legacy profile |
| 2 | Bounded complex simple-content restriction | ctD017.v | Restrict an actual named decimal complex simple-content base with inherited and narrower facets, correct complex/component/atomic identity, attributes and derivation constraints |
| 3 | Integer simple content plus inherited assertions | Three assert006 cases | Integer complex content, complex restriction and inherited assertions using context item, sequence/range equality and mod; not solved by removing one Namespace refusal |
| 4 | Empty complex content and nested context assertions | assert_027_2 | Empty element-only type and namespace-uri(.) assertion context on nested and root owners; broader than the existing immediate-child assertion subset |

Rank considers specification alignment and bounded implementation risk, not only
potential pilot matches. The seven cases represent four schemas, not seven
independent feature requirements. No promised future match count is measured.
The original24-area matrix remains a historical baseline, not a completeness
percentage. Broader datatypes, composition, choices, identity constraints and
application workflows remain important beyond this small frozen sample.

## Recommended next contract: schema namespace/form declarations

The ElemDecl schema uses the literal relative namespace name ElemDecl/typeDef.
Its Boolean scalar is already in the mixed profile. The current NamespaceUri
helper requires an absolute URI, so compilation refuses before evaluating either
instance. Namespaces1.0Third Edition section2.2 deprecates relative references;
it does not make an absolute-URI-only native profile full namespace coverage.
Section2.3 compares namespace names as exact strings. Review Structures schema
representation/targetNamespace, Datatypes anyURI and the actual Core contracts
before changing this deliberately bounded policy. No automatic URI resolution,
case folding, percent decoding, base-URI expansion or network acquisition.

The assert006 schema specifies attributeFormDefault=unqualified but declares no
application attributes. Current mixed compilation admits this schema attribute
only if it first detects application attribute declarations. This is its measured
first Namespace gate. Later independent gates remain: integer complex content,
complex restriction, inherited assertions and additional assertion syntax. Do not
claim this three-case family is admitted after the form-attribute gate changes.

Record a new production adoption before semantic edits. Keep the old immutable
namespace adoption/review pins and receipts; do not rewrite history. Review exact
immutable Structures/Datatypes/Namespaces sections and lexical boundaries, audit
all NamespaceUri callers (including xpathDefaultNamespace), and decide any
remaining assertion-namespace exclusions explicitly. Use signed public Core
1.81.2/1.81.17 for a small actual-native feasibility check with relative declaration
identity, atomization, export/reassessment and original ownership. No Core bypass.

Planned controls include qualified/unqualified local forms, different prefixes for
one exact name, relative versus base-expanded names staying distinct, absolute
namespace compatibility, reserved/empty/invalid namespace refusals, form defaults
without any attribute use, unused named types, hints with no acquisition and the
legacy decimal-only profile. Map exact authored expectation changes before edits.
Require positive Boolean/decimal/string reads, invalid instance discrimination,
original fact/provenance checks, no partial publication and existing quotas.

Then run full source, package-only minimum/current and retained callers, zero API
delta and unchanged guarded pilot. Keep Unsupported distinct from validity and
avoid adding schema composition or assertion grammar just to inflate one count.
The next contract is proposed only; no semantic adoption or implementation here.

## Reviewed immutable source bodies

The retained Structures REC20120405, Datatypes REC20120405 and Namespaces
REC20091208 bodies were rechecked against the original exact SHA-256 pins in
[SPECIFICATION-REVIEW.json](../xsi-type-contract/SPECIFICATION-REVIEW.json).
New sections named above are review targets, not implementation adoption. The
current source gates are recorded in [SOURCE-GATES.json](SOURCE-GATES.json).
Earlier main/pilot/package/feasibility evidence stays sealed and unchanged.
