# Strict attribute wildcards on mixed sequence owners

26 November 2026. Pre-implementation contract against PR90 merge c9669dd109991110506bd65bda40a2cbe4417185, signed Core1.86.0.

Admit the qualified strict/default-strict anyAttribute validator on existing finite nonempty explicit mixed sequence owners: anonymous/named roots, nested containers, global references and named-root aliases. Preserve original mixed governing type/declaration handles; actual atomic evidence is xs:untypedAtomic, not an invented anyType governing component. Aggregate original descendant text in document order, excluding attributes/comments/PIs; do not substitute normalized child values. An optional sequence with no actual children still produces one string atom, possibly empty, rather than no typed value or an empty sequence.

Known children and attributes validate independently under prior order/occurrence/type/fixed/ID rules. Namespace/QName exclusions and explicit-use precedence remain; unknown strict attributes are Invalid. No missing global augmentation or synthesized wildcard declarations. All assessment completes before facts publish. Preserve XML, original source binding, cancellation, foreign authority and expiry.

Exclude mixed all/choice, empty mixed models, simple content, top-level group shorthand, assertions, inheritance/composition and lax/skip. Existing empty/element-only/all/choice owner behavior and legacy decimal-only grammar remain. Retain UTF16 aggregate limits and all mixed scalar/schema/node/depth/attribute/constraint/identity/fact budgets.

Qualify both facades and actual owned evidence, raw aggregate versus normalized child/attribute values, nested/global/alias propagation, optional absence, capacities and recovery. Preserve prior names/fixture bytes; reconcile measured expectations separately. Full source, fresh exact package, API/signed Core, original frozen samples/invoice and docs/release controls precede delivery. No new result or admission claimed by this original contract.

See [exact immutable adoption](ADOPTION.json).
