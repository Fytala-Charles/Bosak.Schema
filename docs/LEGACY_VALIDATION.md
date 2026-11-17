# Legacy validation and compile diagnostics

17 November 2026. Local bug-fix candidate after PR9; not yet published. [Pre-change basis](compile-diagnostics/BASIS.json), [qualification](compile-diagnostics/README.md).

## Service precondition

Validate, ValidateValue and ValidateAlternativeTests now perform their existing null-argument checks, then capture schema.Diagnostics once. Any compile finding returns SchemaValidationResult with IsValid=false before typed context construction, type lookup, assertion/alternative compilation or evaluation. The returned collection owns an immutable snapshot of those findings. Codes, messages, paths, order and duplicates are preserved; no synthetic validation diagnostic is added. Every compile finding blocks because the current diagnostic model has no severity distinction.

An invalid schema root can therefore no longer produce an apparently successful assertion check solely because it recorded no applicable assertions. Denied/unavailable import and unsupported-language-mode findings also block all three operations. Unknown-type and bad lexical input handling is not reached when compile findings already block; otherwise the previous lookup/evaluation behavior applies.

```csharp
var model = compiler.Compile(schemaStream, schemaBaseUri, options);
var result = validator.Validate(model, candidateElement);
if (!result.IsValid)
{
    // These may be original compile findings or operation-specific assertion findings.
    foreach (var diagnostic in result.Diagnostics)
        Report(diagnostic.Code, diagnostic.Message, diagnostic.NodePath);
}
```

The existing Compile reader/parser exceptions still require caller handling; this guard does not turn a thrown compiler exception into a model. Null method arguments continue to throw even when the supplied model has findings. No new exception, result type, code or public member is introduced.

## What success means

For a model without compile findings, Validate still evaluates applicable complex-type assertions/governing-type tests; ValidateValue evaluates recorded simple-type assertion facets and its existing base-value route; ValidateAlternativeTests performs static alternative-test checks. Successful results certify only those operations. They do not establish complete schema/component correctness, arbitrary element structure, all datatype facets, every xsi:type rule or full XSD1.1 conformance.

In particular, a clean model with no applicable assertions can return IsValid=true for an unmatched element. That remains deliberate assertion-only behavior and is explicitly tested; do not use it as a structural validator. NativeDecimalSession has a different result/assessment contract for its explicit finite grammar. Neither service should be presented as complete arbitrary-schema validation.

## Diagnostics and caller-owned models

Compile findings are copied verbatim. Detailed legacy messages may contain expressions, names, paths, URIs or engine information; they do not acquire the native stage-diagnostic redaction/length guarantee. Apply application logging policy separately. Only this guarded compile-failure collection receives a new immutable ownership promise; this change does not redesign all SchemaValidationResult collections.

SchemaValidationResult remains publicly constructible application data, so callers can still manufacture inconsistent records. Arbitrary ICompiledSchema implementations must obey their declared non-null collection contracts; no guarantee is made for concurrent mutation or hostile getters during snapshot capture. The guard reads only Diagnostics before validation work, and its returned collection is detached from later ordinary mutation of caller-supplied lists.

## Compatibility and beta status

Public surface remains40 types/317 declared contract rows, identical to PR9. The intentional behavioral change is that models with compile findings no longer proceed to validation or yield a successful result; consumers relying on ignored compile findings must now handle failure. Clean model behavior, Core1.81.2, native decimal profile and caller-created result shape remain unchanged. No broad lexical-exception normalization, full structural engine, beta/ADR acceptance, tag or publication is implied.
