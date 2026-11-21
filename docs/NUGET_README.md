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
decimal-only. Contextual assertions on integer leaves, derived integer families and named integer
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

## Separate root attribute reads (0.6 development only)

NativeSchemaSession admits up to16local required/optional attributes on an
assessed root's anonymous or named complex type, after its sequence and before
assertions. Attribute types use existing scalar families and admitted named
string/decimal restrictions. form/attributeFormDefault distinguish qualified and
unqualified names; a default XML namespace does not qualify unprefixed attributes.
Namespace declarations are not scalar attributes.

ScalarCount/ReadScalar remain leaf-only. AttributeCount/ReadAttribute use separate
present-root-attribute indexes in schema declaration order, omitting absent optional
uses regardless of XML attribute order. A present empty string, false or zero is
a real value. NativeAttributeValue exposes detached NamespaceUri, LocalName and
Value; it carries no schema authority and survives document/session disposal.

For an attribute-enabled root, missing required or undeclared ordinary root
attributes are Invalid. Roots with no attribute uses keep their earlier
Unsupported/Attributes behavior. Leaf attributes, defaults/fixed, global refs,
groups/wildcards, anonymous types, xml/xsi requests and attribute assertions remain
excluded. NativeDecimalSession remains decimal-only. XML attribute normalization
precedes datatype processing; UTF16string and native numeric/Boolean limits match
leaf limits, and query/cache capacities remain separate read constraints.

```csharp
using System.Globalization;
using Bosak.Schema;

var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders"
           targetNamespace="urn:orders" elementFormDefault="qualified" attributeFormDefault="unqualified">
  <xs:simpleType name="Currency"><xs:restriction base="xs:string">
    <xs:length value="3"/><xs:enumeration value="EUR"/><xs:enumeration value="USD"/>
  </xs:restriction></xs:simpleType>
  <xs:complexType name="Order"><xs:sequence>
    <xs:element name="id" type="xs:string"/><xs:element name="amount" type="xs:decimal"/>
  </xs:sequence>
    <xs:attribute name="currency" type="t:Currency" use="required"/>
    <xs:attribute name="paid" type="xs:boolean"/>
    <xs:attribute name="quantity" type="xs:integer" form="qualified" use="required"/>
  </xs:complexType>
  <xs:element name="order" type="t:Order"/>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 32, 1, 256, 256);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(created.Diagnostics[0].Code);
    return;
}
using var session = created.Value!;
var assessed = session.Assess("""
<o:order xmlns:o="urn:orders" o:quantity="9223372036854775808" paid="0" currency="EUR"><o:id> INV-001 </o:id><o:amount>12.50</o:amount></o:order>
""", new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
Console.WriteLine(document.ReadScalar(0).StringValue);
Console.WriteLine(document.ReadScalar(1).DecimalValue.ToInvariantString(256));
for (var index = 0; index < document.AttributeCount; index++)
{
    var attribute = document.ReadAttribute(index);
    Console.WriteLine("{" + attribute.NamespaceUri + "}" + attribute.LocalName + ": " + attribute.Value.Kind);
}
Console.WriteLine(document.ReadAttribute(2).Value.IntegerValue.ToString(CultureInfo.InvariantCulture));
Console.WriteLine(document.ExportXml());
```

The two leaf indexes remain id and amount. The three attribute indexes are
currency/String, paid/Boolean(false) and quantity/Integer(9223372036854775808),
with quantity in urn:orders. Read capacity/overlap/cancellation/closed-owner and
wrong-kind rules match existing scalar reads; bounded export retains parsed XML
lexical spelling, not portable PSVI or canonical XSD text. This development slice
is not part of published beta.1 or a full XSD implementation.

## Decimal simple-content root values (0.6 development only)

NativeSchemaSession admits anonymous/named root complex types with direct
simpleContent/extension from xs:decimal or an admitted named decimal restriction.
Existing root attributes/forms and limits apply. HasRootScalar reports whether
the root has an admitted typed scalar; ReadRootScalar reads it separately.
ScalarCount/ReadScalar remain leaf-only, and attribute indexes do not move.
Existing element-only roots report HasRootScalar=false and ReadRootScalar refuses
with FOTY0012. Decimal simple-content roots have zero leaf scalars.

```csharp
using Bosak.Schema;

var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:price" targetNamespace="urn:price">
  <xs:complexType name="PriceType"><xs:simpleContent><xs:extension base="xs:decimal">
    <xs:attribute name="currency" type="xs:string" use="required"/>
  </xs:extension></xs:simpleContent></xs:complexType>
  <xs:element name="price" type="t:PriceType"/>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 8, 1, 256, 256);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(created.Diagnostics[0].Code);
    return;
}
using var session = created.Value!;
var assessed = session.Assess("<p:price xmlns:p='urn:price' currency='EUR'>12.50</p:price>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
if (!document.HasRootScalar)
    throw new InvalidOperationException("Expected a root decimal value.");
Console.WriteLine(document.ReadRootScalar().DecimalValue.ToInvariantString(256));
Console.WriteLine(document.ScalarCount);
Console.WriteLine(document.ReadAttribute(0).Value.StringValue);
Console.WriteLine(document.ExportXml());
```

The outputs begin12.5,0,EUR. The root's schema annotation remains complex PriceType;
its actual atomic type is decimal. Root schema string-value collapses decimal
whitespace; XML export retains parsed lexical text. Detached root values survive
disposal without authority. Numeric representation/query/cache and all owner/
cancellation/overlap/export rules remain explicit. NativeDecimalSession stays
unchanged. Restriction/complex-base chains, other simple-content base families,
assertions, nested/mixed content and broader defaults/imports remain excluded.
This development slice does not make the entire orders fixture supported and is
not part of published beta.1 or a full XSD implementation.


## Development: bounded nested sequences

The 0.6 development profile admits acyclic named or anonymous element-only nested
sequences through NativeSchemaSession. ScalarCount/ReadScalar enumerate terminal
simple-typed leaves in document order, excluding containers and root attributes.
Existing flat-document indexes remain unchanged. For example:

```csharp
using Bosak.Schema;

var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:complexType name="Line"><xs:sequence>
    <xs:element name="sku" type="xs:string"/>
    <xs:element name="amount" type="xs:decimal"/>
  </xs:sequence></xs:complexType>
  <xs:element name="order"><xs:complexType><xs:sequence>
    <xs:element name="line" type="Line" maxOccurs="2"/>
  </xs:sequence><xs:attribute name="id" type="xs:string" use="required"/></xs:complexType></xs:element>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 8, 1, 256, 256);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
    throw new InvalidOperationException(created.Diagnostics[0].Code);
using var session = created.Value!;
var assessed = session.Assess("<order id='ORDER-1'><line><sku>ABC-0001</sku><amount>12.50</amount></line></order>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
    throw new InvalidOperationException(assessed.Diagnostics[0].Code);
using var document = assessed.Value!;
Console.WriteLine(document.ScalarCount);
Console.WriteLine(document.ReadScalar(0).StringValue);
Console.WriteLine(document.ReadScalar(1).DecimalValue.ToInvariantString(256));
Console.WriteLine(document.ReadAttribute(0).Value.StringValue);
Console.WriteLine(document.ExportXml());
```

Outputs begin 2, ABC-0001, 12.5, ORDER-1; XML export retains 12.50. Element-only
containers have no typed value (FOTY0012); HasRootScalar remains false. Worst-case
expansion is limited to depth 4 including the root, 16 terminal scalars and 64
element nodes (the node ceiling is defensive under the other limits). Sequences
have 1-16 unique expanded child names, minOccurs 0/1 and finite maxOccurs 1-16;
root/type counts and local anonymous types have separate 16-component ceilings.
Optional branches and repetition count toward compile-time budgets. New nested
element-only containers remain assertion-free; admitted integer simple-content terminals use the contextual profile below. Choice/all, recursion, unbounded repetition, nested
attributes/simple content, mixed content, composition and defaults/nil remain
excluded. NativeDecimalSession and published beta.1 retain their original scope.


## Development: owner-aware nested attributes

The 0.6 development profile supports required/optional local attributes on nested
element-only containers. Existing AttributeCount/ReadAttribute remain root-only;
DescendantAttributeCount/ReadDescendantAttribute enumerate present descendant
attributes by owner preorder then schema declaration order. Scalars stay separate.

```csharp
using Bosak.Schema;

var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:complexType name="Line"><xs:sequence>
    <xs:element name="amount" type="xs:decimal"/>
  </xs:sequence><xs:attribute name="currency" type="xs:string" use="required"/></xs:complexType>
  <xs:element name="order"><xs:complexType><xs:sequence>
    <xs:element name="line" type="Line" maxOccurs="2"/>
  </xs:sequence><xs:attribute name="id" type="xs:string"/></xs:complexType></xs:element>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 16, 1, 256, 256);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
    throw new InvalidOperationException(created.Diagnostics[0].Code);
using var session = created.Value!;
var assessed = session.Assess("<order id='ROOT'><line currency='EUR'><amount>12.50</amount></line><line currency='USD'><amount>3.75</amount></line></order>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
    throw new InvalidOperationException(assessed.Diagnostics[0].Code);
using var document = assessed.Value!;
Console.WriteLine(document.ReadAttribute(0).Value.StringValue);
for (var i = 0; i < document.DescendantAttributeCount; i++)
{
    var attribute = document.ReadDescendantAttribute(i);
    Console.WriteLine($"{string.Join(".", attribute.OwnerElementPath)}: {attribute.LocalName}={attribute.Value.StringValue}");
}
Console.WriteLine(document.ScalarCount);
Console.WriteLine(document.ExportXml());
```

Outputs begin ROOT, `0: currency=EUR`, `1: currency=USD`, 2. OwnerElementPath is an
immutable array of zero-based ELEMENT-child indexes from the assessed root,
ignoring text and namespace nodes. Root attribute paths are empty/nondefault.
These detached paths describe present XML only; optional omission can shift later
paths, and equal paths across documents convey no identity or schema authority.
Detached name/value/path results remain usable after disposal. No live node or
path-based query interface is exposed.

Up to 16 uses per owner and 64 total potential attribute occurrences INCLUDING
root uses, absent optionals and maximum repetition are admitted. Existing depth,
terminal, string/numeric and independent query/cache limits still apply. Scalar
leaf attributes, nested simple content, defaults/fixed/inheritable/prohibited,
global refs/groups/wildcards and assertions on element-only containers remain excluded. Integer simple-content terminals can use the contextual profile below.
Published beta.1 and NativeDecimalSession keep their prior scope.


## Development: nested decimal simple content

The 0.6 development profile admits named/anonymous complex decimal simple-content
terminals inside nested sequences. They occupy existing descendant scalar slots
in document order; root scalar reads remain separate. Their attributes use the
existing owner-aware descendant reads.

```csharp
using Bosak.Schema;

var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:complexType name="Price"><xs:simpleContent><xs:extension base="xs:decimal">
    <xs:attribute name="currency" type="xs:string" use="required"/>
  </xs:extension></xs:simpleContent></xs:complexType>
  <xs:element name="order"><xs:complexType><xs:sequence>
    <xs:element name="price" type="Price" maxOccurs="2"/>
  </xs:sequence></xs:complexType></xs:element>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 16, 1, 256, 256);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
    throw new InvalidOperationException(created.Diagnostics[0].Code);
using var session = created.Value!;
var assessed = session.Assess("<order><price currency='EUR'>12.50</price><price currency='USD'>3.75</price></order>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
    throw new InvalidOperationException(assessed.Diagnostics[0].Code);
using var document = assessed.Value!;
for (var i = 0; i < document.ScalarCount; i++)
    Console.WriteLine(document.ReadScalar(i).DecimalValue.ToInvariantString(256));
for (var i = 0; i < document.DescendantAttributeCount; i++)
{
    var attribute = document.ReadDescendantAttribute(i);
    Console.WriteLine($"{string.Join(".", attribute.OwnerElementPath)}: {attribute.LocalName}={attribute.Value.StringValue}");
}
Console.WriteLine(document.HasRootScalar);
Console.WriteLine(document.ExportXml());
```

Outputs begin12.5,3.75,`0: currency=EUR`,`1: currency=USD`,False; export retains
12.50. Scalar and attribute lists have independent indexes; this example's shape
has one currency per price, which should not be generalized to arbitrary schemas.
Complex price annotations remain distinct from their actual decimal atomic type.
Their node string-values use collapsed lexical text, while export retains parsed
lexical XML. Original element-only ancestors have no typed value (FOTY0012).

Only direct xs:decimal or admitted named-decimal bases are supported. Existing
depth4/16terminal/64attribute-use budgets remain; scalar-bearing complex owners
at depth4may have three-segment OwnerElementPath values. No new public member or
path-based selector is added. Complex-base extension/restriction chains, other base families,
assertions, mixed content, defaults/nil/composition and primitive simple-type leaf
attributes remain excluded. NativeDecimalSession and published beta.1 are unchanged.

The opt-in NativeSchemaSession on the 0.6 development line accepts instance
`xsi:schemaLocation` and `xsi:noNamespaceSchemaLocation` as unfetched hints.
The explicitly supplied schema remains authoritative; hints stay in exported XML
and do not add typed application attribute slots. `xsi:nil` remains unsupported;
the bounded `xsi:type` rules below apply only to the mixed facades. The legacy
NativeDecimalSession retains hint and instance-type refusal. This does
not change published 0.5.0-beta.1 or claim general schema discovery/full XSD PSVI.

The 0.6 development profile also admits anonymous local element types directly
restricted from `xs:decimal` or `xs:string`, using the same bounded facets as named
restrictions. Types retain their anonymous identity through owned scalar reads;
no type QName is invented. Integer derivation, anonymous attribute/root scalar
schemas and general schema-aware node queries remain outside this profile.
Legacy NativeDecimalSession behavior is unchanged. This is development support,
not a change to the published 0.5.0-beta.1 package or full XSD 1.1 conformance.

The 0.6 development profile supports direct named and local anonymous restrictions
of `xs:integer`: exact inclusive/exclusive bounds, totalDigits, and inherited fixed
fractionDigits zero. Values and bounds use BigInteger without decimal conversion.
Named integer restrictions also enforce facets on admitted local application
attributes. Existing representation limits still apply; assertion facets on integer simple types,
restriction chains, derived integer families and broader query profiles remain
excluded. Legacy NativeDecimalSession and published 0.5.0-beta.1 are unchanged.


## Original-node scalar data (0.6 development candidate)

`NativeSchemaNodeDataSession` is a separate owned session for fixed original-node
atomization. Create it with existing explicit `NativeSchemaSessionOptions`, then
Assess with `NativeSchemaAssessmentContext`. A valid assessment returns an owned
`NativeSchemaNodeDataDocument`. `ReadElement(ImmutableArray<int>)` selects original
element-child indexes (empty selects root, maximum three segments).
`ReadAttribute(ownerElementPath, namespaceUri, localName)` selects a present original
attribute by expanded name. Both return one detached `NativeScalarValue`.
`ExportXml` returns bounded parsed lexical XML. Dispose documents and session.

The existing bounded schema grammar and native scalar families apply. Element-only
containers refuse reads with NotSupportedException carrying FOTY0012. Default paths,
invalid names and missing targets throw documented argument exceptions; query/cache
capacity and unassessed hint attributes are Unsupported read operations, not Invalid
instance assessments. Operations are sequential; overlap is refused and disposal
waits for admitted synchronous work. No expression, raw node, provider authority,
callback, general XPath, list/nil support or transferable PSVI is exposed.

This additive API is implemented locally for review. ADR-016 owner API acceptance,
general Core C45 and full XSD 1.1 conformance remain pending. Published 0.5.0-beta.1
and the existing decimal/scalar facades are unchanged. See the package XML documentation for complete signatures and exception contracts.


The repository owner accepted the bounded two-type/seven-method node-data API on 20 November 2026. General Core C45 and full XSD 1.1 acceptance remain separate. Earlier pending-acceptance wording describes the qualification checkpoint.


## Bounded string assertions (0.6 development)

The opt-in scalar and owned node-data sessions admit string equality assertions
on declared immediate children of flat complex content. Primitive and existing
named/anonymous string restrictions use exact parsed string values. Quoted XPath
literals support doubled quote escaping. `eq`/`ne` require at most one value per
operand; multiple values make assessment Invalid. `=`/`!=` test whether any pair
matches the comparison; an empty operand gives false. Equality uses codepoint
semantics, without trimming, case folding or Unicode normalization.

Existing exists/empty/count checks now accept string children, including present
empty strings. They compose with existing decimal comparisons and and/or under
the original namespace and capacity rules. Attribute expressions, nested
assertions, string ordering, Boolean/integer operands, general XPath and
configurable collation remain outside this slice. The decimal-only facade keeps
its previous grammar. False assertions publish no document or native facts.

The public XML documentation describes the owned session types; string assertions extend the existing schema profile without new API members.
The frozen W3C fixtures/expectations are unchanged. No public API, Core source/pin,
release-controller, tag or published-package change is included.


### Bounded instance type selection (0.6 development)

NativeSchemaSession and NativeSchemaNodeDataSession accept `xsi:type` for the same
existing component or an existing direct restriction of a declared primitive in
the same string/decimal/integer family. For example, a leaf declared `xs:decimal`
can select a named decimal restriction; its narrower facets are then enforced.
The QName uses the instance owner's in-scope namespaces, including its default
namespace. Its parsed raw value is limited to256UTF-16units before whitespace
collapse. Export retains the request; reassessment resolves it afresh.

Malformed, unbound, absent or unrelated types are Invalid. Known excluded built-ins
and valid derivations that change scalar family or simple/complex representation
remain Unsupported. Siblings and weakening are not substitutions. Selected values
retain original declaration and assessment ownership, with the selected governing
type supplying annotations and atomic provenance. The request itself adds no
application-attribute scalar. `xsi:nil`, imports, general complex inheritance, block/final,
abstract types, lists/unions and additional scalar families remain excluded.
NativeDecimalSession and published0.5.0-beta.1 are unchanged. This is bounded
development support; no full XSD1.1 or general PSVI claim is made.


### Namespace names and schema form defaults (0.6 development)

NativeSchemaSession and NativeSchemaNodeDataSession accept bounded nonempty
relative target namespace references, such as `orders/v1`, as exact namespace
names. They are never resolved against a base URI or fetched. Different case,
percent escapes and dot segments remain different names. Relative namespace names
are deprecated by Namespaces1.0; using an absolute namespace is still portable.
This bounded profile retains its no-whitespace/.NET URI lexical predicate and
reserved/empty target exclusions; it does not implement every anyURI/IRI string.

Legal `attributeFormDefault="qualified"` or `"unqualified"` is accepted even when
there are no application attribute declarations. Existing form overrides and
namespace-aware attributes keep their behavior. Explicit xpathDefaultNamespace
URI literals remain absolute-only; symbolic `##targetNamespace` and
`##defaultNamespace` capture the exact namespace name. NativeDecimalSession keeps
its previous absolute-only namespace/form limits. Published beta.1 is unchanged.


### Decimal complex simple-content restriction (0.6 development)

The mixed owned facades admit one direct restriction of a named complex type
whose simple content extends `xs:decimal` or an existing named decimal simple
restriction. Both complex types must be free of application attributes and
assertions. This example admits `4.5` and rejects `5`:

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
           xmlns:t="urn:amount" targetNamespace="urn:amount">
  <xs:complexType name="Amount">
    <xs:simpleContent><xs:extension base="xs:decimal"/></xs:simpleContent>
  </xs:complexType>
  <xs:complexType name="SmallAmount">
    <xs:simpleContent><xs:restriction base="t:Amount">
      <xs:maxExclusive value="5"/>
    </xs:restriction></xs:simpleContent>
  </xs:complexType>
  <xs:element name="amount" type="t:SmallAmount"/>
</xs:schema>
```

Supply `<t:amount xmlns:t="urn:amount">4.5</t:amount>` to the existing session
assessment API. Read the root through `ReadRootScalar()` or fixed original-node
`ReadElement([])`. Existing nested scalar-terminal reads work in their bounded
paths. The decimal stays exact; the original declaration and complex governing
type remain distinct from the owned anonymous simple content type.

Only the existing six decimal facets are admitted. Inherited bounds/digits still
apply, and static weakening or invalid facet/reference syntax in this bounded
route makes the schema Invalid. Numeric/type/raw-token capacities and excluded
grammar remain Unsupported. Equal exclusive endpoints can define a valid empty
type whose instances all fail; that is different from an invalid schema.

Forward named base references are supported. Complex restriction chains, base or
derived attributes/assertions, scalar families beyond decimal/integer, inline simpleType and
broader inheritance remain excluded. An xsi:type request changing one complex
governing type to another remains Unsupported even when its true derivation is
known; same-component requests keep their existing behavior. Legacy
NativeDecimalSession and published beta.1 are unchanged. No general XSD/PSVI or
Core provider API expansion is implied.


### Integer complex simple content (0.6 development)

NativeSchemaSession and NativeSchemaNodeDataSession admit simple-content extension
of xs:integer or an existing named direct integer simple restriction, followed by
one direct complex restriction. Both integer complex types must be attribute-free.
Bounded assertions are described below. This example accepts integers below the stated exact bound:

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
           xmlns:t="urn:quantity" targetNamespace="urn:quantity">
  <xs:complexType name="Quantity">
    <xs:simpleContent><xs:extension base="xs:integer"/></xs:simpleContent>
  </xs:complexType>
  <xs:complexType name="SmallQuantity">
    <xs:simpleContent><xs:restriction base="t:Quantity">
      <xs:maxExclusive value="100000000000000000000"/>
    </xs:restriction></xs:simpleContent>
  </xs:complexType>
  <xs:element name="quantity" type="t:SmallQuantity"/>
</xs:schema>
```

Assess `<t:quantity xmlns:t="urn:quantity">9223372036854775808</t:quantity>` with
the existing options/context and read `ReadRootScalar().IntegerValue` or original-
node `ReadElement([]).IntegerValue`. The detached value is a BigInteger, without
Int64 or decimal conversion. Named/anonymous root and existing nested terminal
routes retain original declaration/governing versus actual atomic provenance.
Leading signs/zeros and collapsed surrounding whitespace are admitted; `4.5`,
`4.0`, exponents and empty integer text are Invalid. Export preserves the parsed
XML lexical value even when the detached integer uses canonical spelling.

The existing six numeric facets apply, with fractionDigits fixed at zero. New
complex restriction bounds/digits must legally restrict inherited facets; runtime
intersection alone is insufficient. Invalid recognized new facets/references are
schema-Invalid; capacity and excluded grammar remain Unsupported. Legal empty
integer ranges (for example, minExclusive=0/maxExclusive=1) compile but reject all
instances. Raw digit capacities apply before leading-zero normalization.

Application attributes on integer complex types, complex chains,
derived built-ins such as xs:int/xs:long, inline simpleType, general inheritance
and broader complex xsi:type substitution remain excluded. Only the bounded
contextual assertion profile below is admitted. Legacy NativeDecimalSession and published beta.1
are unchanged; this development behavior does not claim full XSD/PSVI support.


### Contextual and inherited integer assertions (0.6 development)

Attribute-free integer simple-content extensions and their one direct complex
restriction can carry bounded xs:assert components. Every inherited base assertion
and local assertion must succeed. This example accepts20, rejects80 by the base
membership test, and rejects5 by the derived parity test:

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
           xmlns:t="urn:assertions" targetNamespace="urn:assertions">
  <xs:complexType name="Allowed">
    <xs:simpleContent><xs:extension base="xs:integer">
      <xs:assert test="$value = (1 to 10, 20, 30)"/>
    </xs:extension></xs:simpleContent>
  </xs:complexType>
  <xs:complexType name="EvenAllowed">
    <xs:simpleContent><xs:restriction base="t:Allowed">
      <xs:assert test="$value mod 2 eq 0"/>
    </xs:restriction></xs:simpleContent>
  </xs:complexType>
  <xs:element name="value" type="t:EvenAllowed"/>
</xs:schema>
```

Assess `<t:value xmlns:t="urn:assertions">20</t:value>` through the existing API,
then read the exact IntegerValue using ReadRootScalar() or original-node
ReadElement([]). Root, anonymous and existing nested integer terminals retain
original complex/atomic provenance and ownership after assertion success.

Use `$value` for exact integer arithmetic and comparison. During XSD assertion
checking, dot normally atomizes to untypedAtomic from a temporary partial context.
Numeric general comparison/arithmetic on dot converts to double and can round;
`. eq 5` instead uses a value comparison and fails a string/numeric type check.
These rules do not change the exact stored or returned BigInteger. Temporary
assertion values carry no transferable schema authority.

The closed grammar supports dot/$value, signed integer literals, scalar value
comparisons, general=/!= with integer sequences/ranges, mod, parentheses and/or.
Quoted strings are admitted for scalar eq/ne only. String ordering, functions,
paths, other variables/arithmetic and broader XPath remain Unsupported. General!=
is existential inequality, not negated membership. Evaluated dynamic/type errors
make the assertion unsatisfied and the instance Invalid; no partial facts publish.

Effective inherited-plus-local assertions are capped at8per owner; expressions
at512characters/128tokens/depth16. Internal expanded sequences are capped at32
items before allocation, separate from the public single-result query limit.
Existing integer/input/query/export budgets remain; capacity refusal and
cancellation stay distinct from a failed assertion. Static bindings are retained
from each original assertion, including when inherited by a derived type.

Assertions on other scalar contexts or element-only/empty-content owners,
application attributes, broader type chains, node functions and full XPath/PSVI
remain outside this profile. Decimal/string child-assertion behavior, bounded
complex xsi:type policy, NativeDecimalSession and published beta.1 are unchanged.
