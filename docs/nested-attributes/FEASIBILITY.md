# Original nested attribute transport feasibility

20 November 2026. Before production changes; Schema base
`3e067cd73bd779a4539cd14e870950b0bc99430b`.

## Public-package controls

Both independent package-only lanes pass **69 controls each**: minimum Core1.81.2
(source7249d0e01628c47f92b89a3380bc69cf58d70045) and current-reference Core1.81.17
(source6a7618e9dcf70eb8a67d5d1789ff7b787133b52d). Each lane includes19new nested
attribute checks and50inherited scalar/attribute/complex-root/nested-node controls.
The lanes overlap and add no product/source test cases. All18Core dependency
signatures verify; exact archive/extracted-cache/runtime payload and source
identities are in [IDENTITY.json](evidence/IDENTITY.json).

The synthetic input captures repeated line owners with unqualified and namespaced
same-local-name attributes before opening the snapshot. New controls exercise:

- Eight scalar payload cases through actual public data($node) evaluation on
  both repeated owners, preserving governing/actual type and original assessment.
- Four owner/namespace navigation combinations with exact attribute parent and
  child-index paths; actual attribute wrapper identity survives navigation.
- Same-authority authentic facts swapped between different original nodes are
  refused, even when name, use/type handles and input are shared; recovery reads
  each owner's original payload.
- Foreign nodes/facts, absent facts, expired snapshot evidence and closed input
  refuse; the existing public typed-identity read route also consumes an original
  nested attribute value.
- A host-only immutable path prototype distinguishes[0]and[1]and keeps root[]
  nondefault; detached primitive strings/path arrays survive owner disposal.

The issuer records each fact's EXACT original node, not merely membership in a
same-authority facts set. Original typed-value/fact ownership underpins the
transport result. The path prototype has no Schema authority and does not
implement NativeAttributeValue.OwnerElementPath. This exercise proves neither
new facade members nor arbitrary path-query syntax nor XSD validation. The
synthetic assessor throws if asked to validate XML. Existing sealed probe sources
remain unchanged; current copies have their own header-history updates.

## Actual delivered native/API baseline

Four public-only controls pass on unchanged Schema3e067cd:

1. The proposed attribute-bearing nested fixture compiles Unsupported and admits
   no session.
2. All three proposed members are absent by public reflection.
3. The original nested fixture retains one root attribute and seven terminal
   scalars, unchanged indexes and no root scalar.
4. Supplying descendant attributes against an attribute-free delivered plan
   returns Unsupported/Attributes and no document.

The diagnostic initially named the old decimal-only unsupported-reason enum;
that was a compile-time mistake in the new probe. It was corrected to the scalar
facade's NativeSchemaUnsupportedReason before these four controls ran. No product
edit or runtime defect was involved. Refusal controls are not positive support.

## Reproduction and remaining gate

Use dotnet test evidence/minimum/Probe.csproj and evidence/current/Probe.csproj,
Release configuration, with separate RestorePackagesPath directories. These are
public Core package probes only, without Core project/source references. The
before/Before.csproj control must run at exact Schema3e067cd, not after attribute
implementation. Source tests remain1,125; they were verified in the planning step
and are unchanged here. Actual native owner-aware validation, total-use budgets,
publication/projection/paths, package/retained/API and README qualification remain
required after [semantic adoption](ADOPTION.md). No Core engine or pin change was
needed for this scoped feasibility exercise.
