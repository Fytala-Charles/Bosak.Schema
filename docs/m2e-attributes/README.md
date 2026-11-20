# Root attribute contract and published-Core feasibility

19 November 2026. M2D1 merged in PR23 at098fdaf; [fresh main delivery](M2D1-DELIVERY.json)
passed837source tests,54release controls and6workload controls. The merge tree
equals qualified9fa0536. Product source/API/Core pins and published beta.1 remain
unchanged. This dossier is **planning and transport feasibility, not implemented
Schema attribute support or new semantic/API acceptance**.

## Concrete next slice

[Full proposed contract](CONTRACT.md), [signature ledger](SURFACE.json) and
[proposed ADR012](../ADR-012-owned-root-attribute-reads.md) define root-only local
required/optional attributes with existing datatypes and forms. AttributeCount/
ReadAttribute plus one producer-only detached namespace/local-name/scalar type
add exactly one public type and five members. Existing leaf-only ScalarCount/
ReadScalar indexes and the old decimal facade stay unchanged. Attribute-free
selected roots retain their existing application-attribute refusal; newly
attribute-enabled roots validate their complete finite ordinary attribute set.

[Illustrative future consumer](CONSUMER-EXAMPLE.md) uses the planning
[schema](future-order.xsd) and [XML](future-order.xml). These are XML-well-formed,
not currently XSD-assessed. The consumer is deliberately **not compiled/executed**
against nonexistent APIs. [Fixture checks](FIXTURES.json). Root-only scope,
16attribute-use ceiling, forms, type/lexical capacities, schema-use indexing,
XML normalization, diagnostic/refusal order, lifetime and exclusions are concrete;
[29planned acceptance groups](ACCEPTANCE.json) remain unexecuted Schema controls.

## Executed feasibility evidence

| Lane | Passed | Scope |
|---|---|---|
| Published minimum Core1.81.2 |27/0/0 |Public-only synthetic provider and original attribute nodes |
| Published current Core1.81.17 |27/0/0 |Same catalog, separate cache/runtime identity |
| Current Schema baseline |5/0/0 |Required/optional schema refusals, old instance refusal, leaf/API boundary and actual planning fixture refusal |

Fourteen scalar transport controls repeat payload/provenance/lifetime checks from
a separately pinned predecessor with a new explicit Unicode escape in this
probe source. Thirteen attribute controls comprise eight original-node data
payloads and five controls: established typed-identity reads, foreign nodes/facts,
missing facts, snapshot-bound provenance expiry and closed input refusal. Payloads
include preserved/empty/supplementary+combining strings, true/false,50digit signed
integers and exact decimal. Declaration handles remain distinct from type handles.
Original-node reads retain original declared/actual/origin evidence and detached
primitives; plain metadata cannot replace issued facts. [Results](RESULTS.json),
[probe source](TransportTests.cs), [signed identities](IDENTITY.json).

Both27case lanes use nine signed published Core packages and no Schema package,
project reference, friend access or private/candidate Core API.18package signatures,
exact nuspec source revisions and archive/cache/runtime DLL payloads match. The
provider is intentionally synthetic: Assess throws if called, and no XML/XSD
validation is performed. Synthetic original attribute data success does not
broaden NativeSchemaSession's shipping query profile or prove native attribute
lexical/form/presence/facet validation. These overlapping lanes are not54new Schema
tests or a whole-spec percentage. The delivered837suite is unchanged and its
main-CI evidence remains separate.

The five baseline controls prove present refusal and absent proposed API members;
the actual future fixture currently refuses Namespace because attributeFormDefault
is outside the shipped grammar. This does not validate its future expected outcomes.
No product build/test count, runtime/API/pin, tag/controller or registry change is
introduced by this proposal. Existing Core conformance gates are not rerun/claimed.

## Reproduce and proceed

Run the minimum/current projects under evidence with a fresh package cache:
`dotnet test evidence/minimum/Probe.csproj -c Release` and its current counterpart.
The independent baseline project under evidence/baseline references this public
Schema checkout. Sources keep author/legal headers. Exact source/evidence hashes,
strict docs and programmatic local-link/render receipts are retained.

[Immutable specification body review](SPECIFICATION-REVIEW.json) is distinct from
adoption. Next record exact semantic adoption before implementing the proposed
contract and qualifying actual native attributes, unchanged leaf indexes,
package/retained callers and the exact API delta. General API/C45 acceptance and
release/publication remain separate. New proposal files are local/uncommitted;
commit/push require explicit scope under AGENTS.md.
