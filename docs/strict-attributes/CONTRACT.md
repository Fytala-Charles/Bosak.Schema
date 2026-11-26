# Strict attribute wildcards on empty owners

26 November 2026. Recorded before implementation against PR86 merge f41a6e1c1288403daf3f85c52199c6aba199f083, signed Core 1.86.0.

Admit one direct anyAttribute after existing attribute uses on an empty complex owner. Omitted processContents means strict. Support namespace-any, namespace-other (excluding target and absent namespace), target/local tokens and finite absolute namespace-URI lists. Explicit attribute uses win even when their namespace is excluded by the wildcard. Extra attributes must match the namespace constraint and resolve to an original global declaration; otherwise Invalid. Known declarations retain their actual types, fixed constraints and document identity semantics. Absence of a global default/fixed attribute does not create a use or augment XML.

Preserve empty-owner typed values, original owner/declaration identities, existing attribute order followed by wildcard globals in schema source order, original XML, ownership, cancellation and lifetime. Refuse special XML/XSI requests. No lax/skip, relative namespace constraints, notNamespace/notQName, attribute-group wildcard composition, inheritance, mixed/nonempty owners or wildcard+assertion combinations. Limits: one wildcard per admitted owner, eight constraint tokens of256UTF16; schemas with this wildcard use actual document64elements/depth4/64application attributes/16attributes per owner plus existing capacities.

Qualify both facades and original Core facts, including known Invalid values, unknown strict names, namespace exclusion, explicit-use precedence, no augmentation, ID graphs, capacity refusals before publication and subsequent recovery. Preserve every prior test name and fixture byte; reconcile measured expectation changes separately. Run full source, fresh exact-package consumers, API comparison, signed Core payload checks, original frozen samples/invoice and documentation/release controls before delivery. This is a bounded profile, not full wildcard or XSD1.1/C45 acceptance.

See [exact dated adoption](ADOPTION.json). No execution results are claimed by this original contract.
