# Compiler-owned descriptor snapshots

17 November 2026. Local bug-fix candidate after PR8, not a published package. [Basis](descriptor-snapshots/BASIS.json), [qualification](descriptor-snapshots/README.md).

## Ownership contract

SchemaCompiler returns a completed snapshot whose outer collections and nested collection-bearing components own immutable storage. This applies to both Compile overloads, including empty and diagnostic-bearing results. Assertions, alternatives, element declarations, simple types, open/default-open content, default-attribute references/opt-outs and diagnostics cannot be cleared or replaced through mutable collection interfaces. Namespace bindings preserve ordinal prefix keys, including distinct p and P; dictionary enumeration order is not a promised serialization format.

Simple-type facet lists use the same frozen assertion instances as the model's Assertions collection. Wildcard namespace, notNamespace and notQName lists are copied into immutable storage. Source XML mutation after compilation and a new compilation do not rewrite an earlier snapshot. Digests, descriptor order and existing validation meanings remain unchanged.

Consumers should depend on IReadOnlyList/IReadOnlyDictionary behavior rather than runtime List/Dictionary types. Code that cast compiler output to mutable lists or dictionaries and changed it will now fail. This intentionally closes the ownership defect identified by the beta audit; unchanged signatures do not mean unchanged mutable implementation types.

## Caller-created data remains separate

Public positional record constructors, init properties and with expressions retain their compatibility behavior. A caller can create a CompiledSchema or component record containing application-owned mutable collections, or create a modified copy using with. Such data is not automatically defensively copied, is not a compiler-issued snapshot, and carries no authenticated native schema authority. Replacing a collection on a new record does not change the original compiler snapshot; mutating the replacement can still change that caller-created copy.

The guarantee is an ordinary managed ownership contract. Reflection, unsafe/marshal access to storage and malicious host code are outside it. Immutable model storage does not certify arbitrary thread safety of custom ICompiledSchema implementations, resolvers or service consumers. Eight finite concurrent facet evaluations are observed in the tests, not an arbitrary-schedule proof or performance budget.

## Beta status and compatibility

This implements the compiler-output part of audit finding A03. It does not freeze all public DTO constructors, validate all caller-created models, propagate legacy compile diagnostics, implement full structural validation or accept ADR009/010. A01/A02 legacy result preconditions remain next. Public surface stays40 types/317 declared contract rows, exactly matching PR8; no Core1.81.2 pin or new public API change. No version override, tag, beta acceptance or publication is made. Historical alpha and beta audit evidence remains in its original checkout/commit; run those original verifiers there, not against later changed source files.
