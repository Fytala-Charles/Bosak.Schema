Flat mixed native schemas can now assert string equality on immediate children.
The scalar and original-node facades admit string eq/ne, existential =/!=,
quoted literals and string presence/count with existing logical/numeric composition.
Namespace and capacity rules remain bounded; false assertions and cardinality
errors publish no document or original facts.

The change preserves the decimal-only facade and all 401 public API rows. It
records normative adoption before source edits and seven explicit authored
admissions with original fixture bytes preserved. The unchanged frozen W3C pilot
gains exactly Assert/assert_025/assert_025_2: 10 matches / 11 Unsupported / zero
mismatches or harness errors. General XPath and broader assertion grammar remain
outside scope.

Validation: 1,886 source passes (520 implementation / 1,366 public), including
271 added cases and nine published-Core differential controls. Both package-only
Core 1.81.2/1.81.17 lanes pass 1,366; retained 124/23/33 callers pass with only
Schema.dll replaced. All 18 Core signatures/source pins/payloads verify, as do
15 pilot integrity controls and documentation checks. These scopes overlap.

See docs/string-assertions-review/README.md for the reviewed contract and audit,
and docs/string-assertions/QUALIFICATION.md for execution evidence and limitations.
No Core source/pin, release-controller, tag or package publication change.
