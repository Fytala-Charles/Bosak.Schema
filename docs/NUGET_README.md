![Fytala Bosak.Schema](https://raw.githubusercontent.com/Fytala-Charles/Bosak.Schema/9fd4e6acd20b34446d736ab0ec3fa2d439f9e337/assets/logos/fytala-logo-color-dark.svg)

# Bosak.Schema

Apache-2.0 schema integration and staged XSD 1.1 services for .NET 10. Schema services require no activation, license key or subscription.

The owned, synchronous native decimal-session API was introduced in 0.5.0-beta.1. The 0.6 development line adds detected Unsupported reasons and a bounded namespace-aware decimal profile with optional assertions and reusable global named complex types and direct named decimal restrictions with selected facets. It assesses a finite decimal-only schema profile, reads exact scalar values and exports detached XML. Existing schema compilation, validation and schema-aware XSLT helpers remain available. Complete XSD 1.1 validation is not implemented.

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

The native route accepts strings and a finite grammar only: one to sixteen element-only root declarations, each with an inline complex type or a QName reference to a global named complex type, an ordered bounded sequence of decimal children and zero-to-eight supported assertions. Up to sixteen combined global named simple/complex types are admitted and all are checked, including unused types. Other simple types, restriction chains, attributes, imports, element references, lists/unions, general query syntax, stream/reader overloads and positive cross-universe handoff remain outside the 0.6 profile. The linked immutable M1 contract predates these named-type/facet extensions; see its [baseline contract](https://github.com/Fytala-Charles/Bosak.Schema/blob/394635228573989f437b649a679cd70d7d997e40/docs/NATIVE-DECIMAL-PUBLIC-API.md) and [finite grammar](https://github.com/Fytala-Charles/Bosak.Schema/blob/394635228573989f437b649a679cd70d7d997e40/docs/NATIVE-DECIMAL-GRAMMAR.md).

## Namespace-aware decimal documents (0.6 API)

This profile requires 0.6; published 0.5.0-beta.1 retains its earlier unqualified
profile with at least one assertion. In 0.6, a bounded anonymous element-only
root can carry a finite decimal sequence and zero-to-eight assertions:

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
           targetNamespace="urn:orders" elementFormDefault="qualified">
  <xs:element name="order">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="amount" type="xs:decimal"/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
```

```xml
<o:order xmlns:o="urn:orders"><o:amount>12.50</o:amount></o:order>
```

Use these strings with Create/Assess as above. The decimal read is `12.5` and XML
export retains `12.50`. Names compare namespace URI plus local name, independently
of prefixes. `elementFormDefault` and local `form` choose qualified/unqualified
children. QName assertion operands resolve schema/assertion namespace bindings;
`xpathDefaultNamespace` admits local, target, declaration-site default namespace
and explicit absolute URI settings within the finite expression grammar.

Namespaces alone do not admit imports, attributes, strings/facets or
general XPath. The profile deliberately restricts namespace URIs and capacities.
See the immutable [M1 adoption](https://github.com/Fytala-Charles/Bosak.Schema/blob/394635228573989f437b649a679cd70d7d997e40/docs/m1-namespaces/ADOPTION.md)
for exact bounds and deviations. Unsupported still means missing coverage or
representation, not a supported structural violation.

## Reusable named complex type (0.6 development only)

A global complex type can precede or follow its root references. Type QNames use
the reference element's in-scope prefix/default bindings; xpathDefaultNamespace
is only for assertions. For example, replace the anonymous schema above with:

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
           xmlns:t="urn:orders" targetNamespace="urn:orders"
           elementFormDefault="qualified">
  <xs:element name="order" type="t:OrderType"/>
  <xs:element name="quote" type="t:OrderType"/>
  <xs:complexType name="OrderType">
    <xs:sequence>
      <xs:element name="amount" type="xs:decimal"/>
    </xs:sequence>
  </xs:complexType>
</xs:schema>
```

The preceding order instance still reads `12.5`; a `quote` root in `urn:orders`
uses the same type. Unknown types/prefixes, duplicate declarations and excluded
unused definitions refuse compilation, with no owned session. This is a local
development profile; published 0.5.0-beta.1 does not gain these features.

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
Invalid remain Invalid and use Unspecified. The grammar still refuses application attributes, named types and broader
datatypes; admitted namespace settings are described above. See the immutable
[detected-reason contract](https://github.com/Fytala-Charles/Bosak.Schema/blob/394635228573989f437b649a679cd70d7d997e40/docs/unsupported-reasons/CONTRACT.md).

## Existing schema services

```csharp
using Bosak.Schema;

var compiler = SchemaServices.CreateSchemaCompiler();
var validator = SchemaServices.CreateValidationService();
var xsltCompiler = SchemaAwareXslt.CreateCompiler();
```

The established descriptor/assertion services and schema-aware XSLT route have their own supported profiles. Assertion success is not a certificate of complete structural XSD 1.1 validation. The native decimal profile does not broaden those services or make every XML tree schema-assessed.

## Documentation and feedback

- [Integration guide](https://github.com/Fytala-Charles/Bosak.Schema/blob/394635228573989f437b649a679cd70d7d997e40/docs/INTEGRATION.md)
- [Supported scope and remaining requirements](https://github.com/Fytala-Charles/Bosak.Schema/blob/394635228573989f437b649a679cd70d7d997e40/docs/FEATURE_REQUESTS.md)
- [Source and issues](https://github.com/Fytala-Charles/Bosak.Schema)
- [Apache-2.0 license](https://github.com/Fytala-Charles/Bosak.Schema/blob/9fd4e6acd20b34446d736ab0ec3fa2d439f9e337/LICENSE.md) and [NOTICE](https://github.com/Fytala-Charles/Bosak.Schema/blob/9fd4e6acd20b34446d736ab0ec3fa2d439f9e337/NOTICE)

Copyright 2026 Fytala. Trademark rights are retained. Full XSD 1.1, permanent API/foundation acceptance, general compatibility and performance qualification remain separate milestones.

## Named decimal restrictions (0.6 development only)

Local decimal children can reference a global simple type restricted directly
from xs:decimal. Selected facets are minInclusive, minExclusive, maxInclusive,
maxExclusive, totalDigits and fractionDigits. Facets constrain decimal values;
lexical input capacities remain separate. All present repeated children are checked.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
           xmlns:t="urn:orders" targetNamespace="urn:orders"
           elementFormDefault="qualified">
  <xs:simpleType name="MoneyType">
    <xs:restriction base="xs:decimal">
      <xs:minInclusive value="0"/>
      <xs:totalDigits value="12"/>
      <xs:fractionDigits value="2"/>
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name="OrderType">
    <xs:sequence>
      <xs:element name="amount" type="t:MoneyType" minOccurs="0" maxOccurs="2"/>
      <xs:element name="tax" type="xs:decimal"/>
    </xs:sequence>
    <xs:assert test="count(t:amount) eq 2 and t:tax gt 0"/>
  </xs:complexType>
  <xs:element name="order" type="t:OrderType"/>
</xs:schema>
```

Use an order in urn:orders with amounts12.50 and2 and tax1; exact reads are12.5,2,1.
A negative amount or too many fractional value-space digits is Invalid.
An excessive supplied lexical digit/scale count is Unsupported even if removing
zeros would produce a value within the facets. Unknown bases/facets, inconsistent
components and unsupported unused definitions refuse compilation.

The combined global-type limit is16; totalDigits is1..256 and fractionDigits0..256.
Named nondecimal types, restriction chains, fixed/whiteSpace/pattern/enumeration
facets, attributes and imports remain excluded. Named leaf annotations and atomic
provenance preserve original type identity internally; exported XML and scalar
values carry no transferable PSVI. Published beta.1 retains its original scope.
