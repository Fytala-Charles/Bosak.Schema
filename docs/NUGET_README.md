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

NativeDecimalSession accepts string inputs and a finite grammar only: one to sixteen element-only root declarations, each with an inline complex type or a QName reference to a global named complex type, an ordered bounded sequence of decimal children and zero-to-eight supported assertions. Up to sixteen combined global named simple/complex types are admitted and all are checked, including unused types. Other simple types, restriction chains, attributes, imports, element references, lists/unions, general query syntax, stream/reader overloads and positive cross-universe handoff remain outside its 0.6 decimal profile. The linked immutable M1 contract predates these named-type/facet extensions; see its [baseline contract](https://github.com/Fytala-Charles/Bosak.Schema/blob/394635228573989f437b649a679cd70d7d997e40/docs/NATIVE-DECIMAL-PUBLIC-API.md) and [finite grammar](https://github.com/Fytala-Charles/Bosak.Schema/blob/394635228573989f437b649a679cd70d7d997e40/docs/NATIVE-DECIMAL-GRAMMAR.md).

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
On NativeDecimalSession, named nondecimal types, restriction chains,
fixed/whiteSpace/pattern/enumeration facets, attributes and imports remain excluded. Named leaf annotations and atomic
provenance preserve original type identity internally; exported XML and scalar
values carry no transferable PSVI. Published beta.1 retains its original scope.

## Opt-in string and decimal facade (0.6 development only)

NativeSchemaSession is a separate synchronous entry point for primitive strings
and the existing decimal/named-decimal profile. NativeDecimalSession retains its
decimal-only meaning. String whitespace/Unicode are preserved after XML parsing;
Primitive Boolean and built-in integer leaves are also supported. Named string
restrictions support the selected facets described below.

```csharp
using Bosak.Schema;

var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="r"><xs:complexType><xs:sequence>
    <xs:element name="id" type="xs:string"/>
    <xs:element name="amount" type="xs:decimal"/>
  </xs:sequence></xs:complexType></xs:element>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 4, 1, 256, 256);
var context = new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(created.Diagnostics[0].Code);
    return;
}
using var session = created.Value!;
var assessed = session.Assess("<r><id> INV-001 </id><amount>12.50</amount></r>", context);
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
for (var index = 0; index < document.ScalarCount; index++)
{
    var scalar = document.ReadScalar(index);
    switch (scalar.Kind)
    {
        case NativeScalarKind.String:
            Console.WriteLine(scalar.StringValue);
            break;
        case NativeScalarKind.Decimal:
            Console.WriteLine(scalar.DecimalValue.ToInvariantString(256));
            break;
        default:
            throw new NotSupportedException("This application does not handle this scalar kind.");
    }
}
Console.WriteLine(document.ExportXml());
```

The exact reads are the preserved string ` INV-001 ` and decimal `12.5`.
Wrong-kind access throws InvalidOperationException. Returned values and XML are
detached and survive closure without PSVI/authority. String capacity counts UTF-16
code units; output capacity measures serialized XML before appending beyond the
bound. These limits are not XSD length facets, deadlines or memory SLAs. Export
checks cancellation during bounded writes and refuses overflow without a partial
return; it is not canonical/byte-preserving XML. Published beta.1 does not include
this new facade. All operation/ownership and whole-schema refusal rules still apply.

## Boolean leaves in the opt-in facade (0.6 development only)

NativeSchemaSession also admits primitive xs:boolean. After XML-whitespace
collapse, true/1 yield true and false/0 yield false. Other representable tokens
are Invalid. The original Boolean text is bounded to256UTF-16code units before
collapse, independently of MaxStringCharacters; capacity overflow is Unsupported.
NativeDecimalSession remains decimal-only.

```csharp
using Bosak.Schema;

var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="r"><xs:complexType><xs:sequence>
    <xs:element name="paid" type="xs:boolean"/>
  </xs:sequence></xs:complexType></xs:element>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 1, 65536, 1, 4, 1, 1, 0);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(created.Diagnostics[0].Code);
    return;
}
using var session = created.Value!;
var assessed = session.Assess("<r><paid>0</paid></r>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
var scalar = document.ReadScalar(0);
if (scalar.Kind != NativeScalarKind.Boolean)
    throw new InvalidOperationException("Expected a Boolean scalar.");
Console.WriteLine(scalar.BooleanValue ? "true" : "false");
Console.WriteLine(document.ExportXml());
```

The value is false, not an absent scalar; XML export retains lexical0.
StringValue/DecimalValue getters on Boolean values refuse, and BooleanValue on
other kinds refuses. Boolean assertions and named Boolean restrictions/facets remain separate work. This local profile is not published beta.1.

## Exact integer leaves in the opt-in facade (0.6 development only)

NativeSchemaSession also admits built-in xs:integer leaves with exact BigInteger
values. XML whitespace collapses; signs and leading zeroes are accepted, while
fractional/exponent forms and non-ASCII digits are Invalid. Original parsed text
has a fixed256UTF-16character ceiling before collapse. MaxNativeCoefficientDigits
counts all supplied digits including leading zeroes, excluding sign; overflow is
Unsupported/RepresentationCapacity. The separate Core query digit limit can refuse
a read; export and subsequent in-bound reads remain available. Decimal scale and
string capacities do not constrain integer values. NativeDecimalSession remains
decimal-only. Integer assertions, derived integer families and named integer
restrictions/facets remain excluded; this profile is not published beta.1.

```csharp
using System.Globalization;
using Bosak.Schema;

var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="r"><xs:complexType><xs:sequence>
    <xs:element name="quantity" type="xs:integer"/>
  </xs:sequence></xs:complexType></xs:element>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 0, 1, 65536, 1, 4, 1, 256, 0);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(created.Diagnostics[0].Code);
    return;
}
using var session = created.Value!;
var assessed = session.Assess("<r><quantity>+09223372036854775808</quantity></r>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
var scalar = document.ReadScalar(0);
if (scalar.Kind != NativeScalarKind.Integer)
    throw new InvalidOperationException("Expected an integer scalar.");
Console.WriteLine(scalar.IntegerValue.ToString(CultureInfo.InvariantCulture));
Console.WriteLine(document.ExportXml());
```

The exact value is9223372036854775808; export preserves the supplied plus sign
and leading zero. Wrong-kind getters throw InvalidOperationException. Detached
BigInteger values survive disposal without carrying schema authority.

## Named string restrictions (0.6 development only)

NativeSchemaSession admits global simple types directly restricted from xs:string
with length, minLength, maxLength and enumeration. Length counts Unicode characters,
so one supplementary character counts one, while a base character plus a combining
mark counts two. MaxStringCharacters still counts UTF-16 units before facet checks
and also bounds each parsed enumeration value. Each type permits at most16supplied
enumeration entries, including duplicates. Matching preserves parsed whitespace,
case and Unicode sequence; there is no Unicode normalization. XML attribute
normalization applies to facet values before comparison.

Length/minLength/maxLength values are bounded to0..65536 with a256character lexical
ceiling. A direct restriction cannot combine length with minLength/maxLength;
minLength and maxLength may coexist when consistent. Enumeration entries form
alternatives and intersect with the other supplied facets. Pattern, whitespace
overrides, restriction chains and anonymous string types remain excluded.
NativeDecimalSession remains decimal-only; string assertion references remain
outside the decimal-only native assertion grammar. Published beta.1 is unchanged.

```csharp
using Bosak.Schema;

var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Label"><xs:restriction base="xs:string">
    <xs:length value="2"/>
    <xs:enumeration value="😀A"/>
  </xs:restriction></xs:simpleType>
  <xs:element name="r"><xs:complexType><xs:sequence>
    <xs:element name="label" type="Label"/>
  </xs:sequence></xs:complexType></xs:element>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 3, 65536, 1, 4, 1, 256, 256);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(created.Diagnostics[0].Code);
    return;
}
using var session = created.Value!;
var assessed = session.Assess("<r><label>😀A</label></r>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
Console.WriteLine(document.ReadScalar(0).StringValue);
Console.WriteLine(document.ExportXml());
```

The label has two Unicode characters and three UTF-16 units. An in-bound facet
mismatch is Invalid; explicit representation overflow is Unsupported. Original
named type facts remain internal; returned strings and XML carry no authority.
