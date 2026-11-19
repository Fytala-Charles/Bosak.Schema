![Fytala Bosak.Schema](https://raw.githubusercontent.com/Fytala-Charles/Bosak.Schema/9fd4e6acd20b34446d736ab0ec3fa2d439f9e337/assets/logos/fytala-logo-color-dark.svg)

# Bosak.Schema

Apache-2.0 schema integration and staged XSD 1.1 services for .NET 10. Schema services require no activation, license key or subscription.

The owned, synchronous native decimal-session API was introduced in 0.5.0-beta.1. The 0.6 development line adds a detected Unsupported reason without expanding the native grammar. It assesses a finite decimal-only schema profile, reads exact scalar values and exports detached XML. Existing schema compilation, validation and schema-aware XSLT helpers remain available. Complete XSD 1.1 validation is not implemented.

## Install

Select a reviewed version explicitly. To select an available prerelease:

```sh
dotnet add package Bosak.Schema --prerelease
```

The package targets `net10.0` and declares minimum version 1.81.2 for Bosak.XPath.Api, Bosak.XPath.Providers, Bosak.Xslt and Bosak.XQuery. Its default package-consumer qualification uses published Core 1.81.2 packages. Current-Core source-consumer qualification is a separate lane; it does not change these package dependencies or promise every future dependency combination.

## Native decimal workflow

Download the authored sample [native-decimal.xsd](https://raw.githubusercontent.com/Fytala-Charles/Bosak.Schema/9fd4e6acd20b34446d736ab0ec3fa2d439f9e337/tests/TestData/Schemas/native-decimal.xsd) and [native-decimal.xml](https://raw.githubusercontent.com/Fytala-Charles/Bosak.Schema/9fd4e6acd20b34446d736ab0ec3fa2d439f9e337/tests/TestData/Schemas/native-decimal.xml) into your application directory. These sample files are not bundled in the package.

```csharp
using Bosak.Schema;

var options = new NativeDecimalSessionOptions(256, 256, 1, 4, 1, 256, 256);
var episode = new NativeDecimalAssessmentContext(
    new DateTimeOffset(2026, 11, 17, 12, 0, 0, TimeSpan.Zero), 60);
var compiled = NativeDecimalSession.Create(File.ReadAllText("native-decimal.xsd"), options);
if (compiled.Outcome != NativeDecimalOutcome.Valid)
{
    Console.Error.WriteLine(compiled.Diagnostics[0].Code);
    return;
}

using var session = compiled.Value!;
var result = session.Assess(File.ReadAllText("native-decimal.xml"), episode);
if (result.Outcome != NativeDecimalOutcome.Valid)
{
    Console.Error.WriteLine(result.Diagnostics[0].Code);
    return;
}

using var document = result.Value!;
for (var index = 0; index < document.DecimalCount; index++)
    Console.WriteLine(document.ReadDecimal(index).ToInvariantString(256));
Console.WriteLine(document.ExportXml());
```

The sample reads `12.5`, `2` and `1`; export retains the decimal lexical spelling `12.50`. Limits and assessment date/timezone are explicit sample choices, not API defaults. Numerical capacities and XML/grammar ceilings are not elapsed-time or total-memory budgets.

Inspect Valid, Invalid and Unsupported before using Value. Invalid and Unsupported return no owned session/document. Arguments, cancellation, capacity exhaustion and lifecycle faults throw documented exceptions. Operations on one session must be sequential; overlap refuses. Disposal waits for admitted synchronous work and revokes owned access. Returned exact decimals and XML are detached, with no transferable schema authority.

The native route accepts strings and a finite grammar only: one anonymous element-only root, an ordered bounded sequence of decimal children and supported assertions. Target namespaces, named user types, attributes, lists/unions, general query syntax, stream/reader overloads and positive cross-universe handoff are outside that profile. See the [complete contract](https://github.com/Fytala-Charles/Bosak.Schema/blob/3ff8cb6d6a0f14bf81f6e0f25a498c301580dcdc/docs/NATIVE-DECIMAL-PUBLIC-API.md) and [finite grammar](https://github.com/Fytala-Charles/Bosak.Schema/blob/3ff8cb6d6a0f14bf81f6e0f25a498c301580dcdc/docs/NATIVE-DECIMAL-GRAMMAR.md).

## Detected Unsupported reasons (0.6 API)

This section requires the 0.6 API. Published 0.5.0-beta.1 does not expose
`NativeDecimalDiagnostic.UnsupportedReason`; the owned workflow above remains
usable without that property. Select the package version appropriate to your
application rather than assume every prerelease has the same API.

For an Unsupported result from Create or Assess, inspect the existing stage code
and the detected reason. Place this block inside the refusal branch above, before
returning or accessing Value. Here, `result` is the result of Assess; the same
pattern works with the result of Create:

```csharp
if (result.Outcome == NativeDecimalOutcome.Unsupported)
{
    var diagnostic = result.Diagnostics[0];
    Console.Error.WriteLine(diagnostic.Code);
    switch (diagnostic.UnsupportedReason)
    {
        case NativeDecimalUnsupportedReason.Namespace:
            Console.Error.WriteLine("A schema namespace setting is outside the profile.");
            break;
        case NativeDecimalUnsupportedReason.Attributes:
            Console.Error.WriteLine("An attribute is outside the profile.");
            break;
        case NativeDecimalUnsupportedReason.RepresentationCapacity:
            Console.Error.WriteLine("An explicit representation limit was exceeded.");
            break;
        default:
            Console.Error.WriteLine("The input is outside the profile; inspect its documented limits.");
            break;
    }
}
```

| Reason | Meaning |
|---|---|
| Unspecified | No category supplied; normal fallback for unmapped refusals and all Invalid diagnostics |
| Namespace | A rejected schema namespace setting |
| DeclarationShape | A rejected declaration or schema content shape |
| ValueType | A local type does not resolve to the admitted decimal type |
| Attributes | A rejected schema or instance attribute |
| AssertionGrammar | An assertion outside the finite expression grammar |
| RepresentationCapacity | An explicit byte, node, lexical, numeric or assertion limit |

A category identifies **one detected refusal**, not every unsupported feature or
an instruction for making the entire schema valid. Callers must tolerate
Unspecified and future enum values. Existing stage codes and fixed diagnostic
messages remain unchanged and do not include caller XML, names, paths or raw
exceptions. Valid results still have no diagnostics; refusals have no owned value.
Arguments, cancellation, quota and lifecycle errors retain their documented
exception behavior.

Namespace does not classify every namespaced document: instance names previously
Invalid remain Invalid and use Unspecified. The grammar still refuses target
namespaces, application attributes and broader datatypes. See the immutable
[detected-reason contract](https://github.com/Fytala-Charles/Bosak.Schema/blob/3ff8cb6d6a0f14bf81f6e0f25a498c301580dcdc/docs/unsupported-reasons/CONTRACT.md).

## Existing schema services

```csharp
using Bosak.Schema;

var compiler = SchemaServices.CreateSchemaCompiler();
var validator = SchemaServices.CreateValidationService();
var xsltCompiler = SchemaAwareXslt.CreateCompiler();
```

The established descriptor/assertion services and schema-aware XSLT route have their own supported profiles. Assertion success is not a certificate of complete structural XSD 1.1 validation. The native decimal profile does not broaden those services or make every XML tree schema-assessed.

## Documentation and feedback

- [Integration guide](https://github.com/Fytala-Charles/Bosak.Schema/blob/3ff8cb6d6a0f14bf81f6e0f25a498c301580dcdc/docs/INTEGRATION.md)
- [Supported scope and remaining requirements](https://github.com/Fytala-Charles/Bosak.Schema/blob/3ff8cb6d6a0f14bf81f6e0f25a498c301580dcdc/docs/FEATURE_REQUESTS.md)
- [Source and issues](https://github.com/Fytala-Charles/Bosak.Schema)
- [Apache-2.0 license](https://github.com/Fytala-Charles/Bosak.Schema/blob/9fd4e6acd20b34446d736ab0ec3fa2d439f9e337/LICENSE.md) and [NOTICE](https://github.com/Fytala-Charles/Bosak.Schema/blob/9fd4e6acd20b34446d736ab0ec3fa2d439f9e337/NOTICE)

Copyright 2026 Fytala. Trademark rights are retained. Full XSD 1.1, permanent API/foundation acceptance, general compatibility and performance qualification remain separate milestones.
