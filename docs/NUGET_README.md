![Fytala Bosak.Schema](https://raw.githubusercontent.com/Fytala-Charles/Bosak.Schema/9fd4e6acd20b34446d736ab0ec3fa2d439f9e337/assets/logos/fytala-logo-color-dark.svg)

# Bosak.Schema

The 0.48 development candidate requires Bosak Core 1.86.0 or later.

## IDs and references (0.16 source)

Both owned native facades admit bounded ID/IDREF/IDREFS values and assessment-local reference checks before document/fact publication. ID attributes identify their owners; ID-valued child elements identify their parents. Same-owner repetition is allowed; conflicting owners and unresolved bindings are Invalid. Actual union/list members determine participation, and values retain original provenance/lifetime. Raw identity text256UTF16,128distinct strings and256identity atoms plus existing limits apply. Identity value constraints, ENTITY/NOTATION, xml:id special processing, public ID-table/PSVI and general fn:id/idref remain excluded. [Guide](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/identity-coverage/GUIDE.md).

## Original markup and annotations (0.15 source)

Both owned native facades preserve instance comments/PI while ignoring their text for schema validation and element string values. Original paths, typed facts, XML export, budgets and owner lifetime remain. Empty/nil content still rejects character and element children. Bounded schema annotations are checked before declaration scans; appinfo/documentation payloads stay opaque.16wrappers/source and original XML budgets apply; xml:lang/foreign wrapper attributes and public annotation PSVI remain excluded. [Guide](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/markup-coverage/GUIDE.md).

## Structural coverage (0.14 source)

Both owned facades admit bounded empty all/optional empty choice and anonymous attribute simple types on existing owners. Empty content yields zero typed items; attribute references/groups retain original anonymous identities. The shared16site budget and existing source/value/lifetime limits apply. Required empty choice, empty-owner attributes, annotation xml:lang/foreign attributes, augmentation and general XSD1.1/C45 remain excluded. See the [structural guide](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/structural-coverage/GUIDE.md).

## Gregorian fragment values (0.13 source)

Both owned facades admit bounded gYear, gYearMonth, gMonth, gMonthDay and gDay. Distinct scalar kinds expose nullable GregorianYear/Month/Day/TimezoneOffsetMinutes and collapsed original GregorianValue text. Missing fields stay absent; REC1972-12-31/month-last-day completion is private to exact comparison. Extended signed years, timezone presence, finite facets and original member/lifetime facts follow the [guide](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/gregorian-400/GUIDE.md). Raw input is bounded to256 UTF-16 units and caller capacity; years exclude long.MinValue. The earlier float/duration equal-exclusive-bound schema rejection is corrected. Source qualification does not imply a new registry publication, general legacy Gregorian operators or full XSD1.1/C45.

## Exact duration values (0.12 source)

Both owned native facades admit bounded duration, yearMonthDuration and dayTimeDuration. NativeScalarValue exposes exact BigInteger DurationMonths, Core exact-decimal DurationSeconds and collapsed original DurationValue text without TimeSpan/decimal precision loss. Applicable finite facets, partial ordering, selected-member and lifetime rules are qualified within the [duration guide](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/duration-200/GUIDE.md). Raw text is capped at256 UTF-16 units and caller capacity; supplied and aggregate digit/scale limits apply before normalization. Ambiguous decimal-point seconds syntax, incomparable local endpoints and conflicting coincident Gregorian projections remain Unsupported. This source candidate is separate from registry publication and complete XSD1.1/general duration-operator acceptance.

## URI and IEEE values (0.11 source)

Both owned native facades now admit bounded anyURI, float and double. Detached NativeScalarValue exposes AnyUri/UriValue, Float/FloatValue and Double/DoubleValue. URI XML whitespace collapse preserves ordinal text without URI resolution or decoding. IEEE values preserve signed zero, NaN, infinities and 32/64-bit rounding. Floating supplied text is bounded to256 UTF-16 units and caller string capacity; URI uses caller capacity. Applicable finite facets and original-node/member/lifetime rules remain. NaN bound endpoints and bounds/enumeration over patterned ancestors remain Unsupported. See the [guide](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/numeric-uri-200/GUIDE.md). This development candidate is not a new registry publication or full XSD1.1 implementation.

## Binary and QName values (0.8 source)

Both native facades admit bounded hexBinary, base64Binary and namespace-sensitive QName values. Detached scalars expose immutable `BinaryValue` octets or `QNameValue` components under distinct `NativeScalarKind` values. Existing admitted placements, original XML, owned authority and lifetime gates remain. Enumeration, finite patterns, collapse whitespace and binary decoded-length facets follow the bounded [datatype guide](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/datatypes-200/GUIDE.md). Values are limited to 256 supplied UTF-16 code units and caller string capacity; binary payloads are limited to 128 octets. QName namespace bindings come from each original instance owner or schema literal context. Lists/unions remain pending; this source candidate does not represent a registry publication or full XSD 1.1 acceptance.

Apache-2.0 schema integration and staged XSD 1.1 services for .NET 10. Schema services require no activation, license key or subscription.

The owned, synchronous native decimal-session API was introduced in 0.5.0-beta.1. The 0.6 development line adds detected Unsupported reasons and a bounded namespace-aware decimal profile with optional assertions and reusable global named complex types and direct named decimal restrictions with selected facets. It assesses a finite decimal-only schema profile, reads exact scalar values and exports detached XML. Existing schema compilation, validation and schema-aware XSLT helpers remain available. Complete XSD 1.1 validation is not implemented.

## Exact native calendars (0.7 source)

NativeSchemaSession and NativeSchemaNodeDataSession admit bounded dateTime, time and dateTimeStamp validation and exact scalar reads. NativeScalarValue.CalendarValue returns detached Core exact components; fractional seconds retain their precision and timezone presence. Finite enumeration, ordering, timezone, whitespace and pattern facets use the existing original-node/lifetime contract. The legacy NativeDecimalSession stays decimal-only. See the [consumer guide and runnable example](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/calendar-100/GUIDE.md). Complete XSD 1.1, general PSVI/C45, full regex and default-node augmentation remain outside this source candidate.

## Install

Select a reviewed version explicitly. To select an available prerelease:

```sh
dotnet add package Bosak.Schema --prerelease
```

The development package targets `net10.0` and declares minimum version 1.86.0 for Bosak.XPath.Api, Bosak.XPath.Providers, Bosak.Xslt and Bosak.XQuery. Source and fresh package-only public qualification use this single signed Core baseline. Published Schema 0.5.0-beta.1 retains its historical 1.81.2 dependency; a newer source candidate is not automatically available from NuGet.

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
other kinds refuses. Boolean assertions and Boolean complex content remain separate work. This local profile is not published beta.1.

## Exact integer leaves in the opt-in facade (0.6 development only)

NativeSchemaSession also admits built-in xs:integer leaves with exact BigInteger
values. XML whitespace collapses; signs and leading zeroes are accepted, while
fractional/exponent forms and non-ASCII digits are Invalid. Original parsed text
has a fixed256UTF-16character ceiling before collapse. MaxNativeCoefficientDigits
counts all supplied digits including leading zeroes, excluding sign; overflow is
Unsupported/RepresentationCapacity. The separate Core query digit limit can refuse
a read; export and subsequent in-bound reads remain available. Decimal scale and
string capacities do not constrain integer values. NativeDecimalSession remains
decimal-only. Contextual assertions on integer leaves remain excluded. Later sections
describe admitted direct restrictions and built-in integer descendants; this profile
is not published beta.1.

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
alternatives and intersect with the other supplied facets. Pattern facets use the
bounded profile below. Whitespace overrides and new restriction chains remain
excluded; direct anonymous local string restrictions use the existing finite leaf
placements.
NativeDecimalSession remains decimal-only; string assertion references remain
outside the decimal-only native assertion grammar. Published beta.1 is unchanged.

```csharp
using Bosak.Schema;

var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Label"><xs:restriction base="xs:string">
    <xs:length value="2"/>
    <xs:enumeration value="ðŸ˜€A"/>
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
var assessed = session.Assess("<r><label>ðŸ˜€A</label></r>",
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
arbitrary user restriction chains and broader query profiles remain excluded.
The built-in descendant family is described below. Legacy NativeDecimalSession and published 0.5.0-beta.1 are unchanged.


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
inline simpleType, general inheritance
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

Assertions on other scalar contexts, application attributes, broader type chains
and full XPath/PSVI remain outside this integer profile. Structural namespace
assertions use the separate bounded profile below. Decimal/string child-assertion behavior, bounded
complex xsi:type policy, NativeDecimalSession and published beta.1 are unchanged.


### Structural namespace assertions and empty content (0.6 development)

NativeSchemaSession and NativeSchemaNodeDataSession can assess attribute-free empty
complex types and bounded element-only sequences with namespace assertions. The
following schema accepts `<r xmlns="urn:document"/>`:

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
           targetNamespace="urn:document">
  <xs:element name="r"><xs:complexType>
    <xs:assert test="namespace-uri(.) = 'urn:document'"/>
  </xs:complexType></xs:element>
</xs:schema>
```

The accessor observes the actual instance element namespace, including unqualified
local elements; it does not substitute the type namespace or schema target namespace.
Prefix aliases preserve the same URI. URI comparison preserves case, relative spelling
and percent escapes. Supported expressions use namespace-uri(.) or namespace-uri(),
optionally qualified by an ASCII prefix bound to the XPath functions namespace,
= / != / eq / ne against string literals, parentheses and/or. xpathDefaultNamespace
does not change function bindings or instance expanded names. Other XML prefix forms,
functions, paths and broader XPath remain Unsupported in this finite grammar.

Empty complex content has a present empty typed sequence and forbids all character
or element content, including whitespace. Element-only content retains an absent
typed value even when its optional children are missing. An empty value is not a
scalar: HasRootScalar is false, scalar reads refuse with NotSupportedException, and
XML export remains available. The original-node data operation internally atomizes
authentic empty values to the empty sequence; element-only atomization retains FOTY0012.
Typed-identity operations keep their singleton-only profile.

Named/anonymous root and nested types retain original identity and ownership.
Failed assertions or invalid content publish no document. Existing limits include
eight assertions, 512 expression characters, 128 tokens, expression depth16,
64 assessed nodes and node depth4; zero-scalar empty nodes count toward those limits.
Application-attribute combinations with new structural assertions, structural
inheritance, mixed/list/union content, comments and processing instructions remain
outside this slice. NativeDecimalSession and published beta.1 retain their profiles.
This development extension does not establish full XSD1.1 or PSVI coverage.


### Bounded string pattern facets (0.6 development)

Direct xs:string restrictions can combine pattern with the existing length and
enumeration facets. Supported named/anonymous element and attribute placements
retain their original String values and type identity. For example:

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Code"><xs:restriction base="xs:string">
    <xs:pattern value="[A-Z]{3}-[0-9]{4}"/>
  </xs:restriction></xs:simpleType>
  <xs:element name="r"><xs:complexType><xs:sequence>
    <xs:element name="code" type="Code"/>
  </xs:sequence></xs:complexType></xs:element>
</xs:schema>
```

Assess `<r><code>ABC-1234</code></r>` with NativeSchemaSession or
NativeSchemaNodeDataSession, then read ReadScalar(0).StringValue or
ReadElement([0]).StringValue. `ABC-12345` is Invalid: XSD patterns match the whole
literal. Caret and dollar are literals outside classes, not XPath anchors.
Multiple pattern elements in one restriction are alternatives; length/enumeration
constraints still conjoin with that result. Parsed whitespace, case and Unicode
sequence are preserved. Empty matched text remains one String atomic, distinct from
an empty complex typed sequence.

The subset includes scalar literals, dot (excluding LF/CR), grouping/alternation,
explicit positive/negative scalar classes/ranges, XML whitespace escapes, XSD
single-character escapes, ?, * and +, and brace quantifiers under explicit bounds.
It matches Unicode scalars, including supplementary characters. Unicode digit,
property/name classes and class subtraction are Unsupported in this first subset;
\d must not be interpreted as ASCII-only. Simple-typed document roots and new
restriction chains remain excluded, so the original public SKU example is not
admitted by pattern support alone.

Limits are8source patterns per restriction,256UTF-16 pattern characters,64atoms,
group depth8,16class members and explicit repetition bounds0..16. Compiled states
are capped at128per pattern and1024per schema, including unused types. A fresh
4,194,304-unit budget covers the complete assessment across element/attribute
values, length scans and matching. Capacity/work exhaustion is Unsupported rather
than a false match; genuine pattern mismatch is Invalid. Cancellation, recovery,
source export and no-partial-document behavior retain their existing rules.
This matcher uses bounded state traversal with no regex backend or ambient setting
changes. Legacy NativeDecimalSession and published beta.1 retain their profiles.


### Simple scalar document roots

The opt-in scalar and original-node facades also admit global roots typed as
`xs:decimal`, `xs:integer`, `xs:boolean` or `xs:string`. Existing direct named and
anonymous decimal/integer/string/Boolean restrictions may appear at the root. Defaults/fixed values and nil remain excluded.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Code">
    <xs:restriction base="xs:string">
      <xs:pattern value="[A-Z]{3}-[0-9]{4}"/>
    </xs:restriction>
  </xs:simpleType>
  <xs:element name="code" type="Code"/>
</xs:schema>
```

For `<code>ABC-1234</code>`, `NativeSchemaDocument.HasRootScalar` is true and
`ReadRootScalar()` returns String `ABC-1234`. `ScalarCount` is zero: root values
are separate from descendant leaf indexes. The original-node facade reads the
same assessed root with `ReadElement([])`. Both facades reject a nonmatching value,
retain original declaration/type evidence, and export within their existing XML
serialization contract. String roots preserve parsed XML whitespace and empty
String remains one atomic value. Numeric/boolean roots retain their existing
whitespace normalization and bounded representation limits.

A schema may declare multiple admitted simple and complex roots; selection uses the
instance's expanded root name. Current type selection, combined anonymous/global
component ceilings, shared pattern work, cancellation and lifetime rules still
apply. The legacy `NativeDecimalSession` retains its earlier grammar. This example
uses explicit ASCII digits; the original public SKU's `\d` escape remains outside
the bounded pattern subset. No general XSD, component-closure or PSVI claim follows.


### Built-in integer descendants

The opt-in native facades admit `nonPositiveInteger`, `negativeInteger`, `long`,
`int`, `short`, `byte`, `nonNegativeInteger`, `unsignedLong`, `unsignedInt`,
`unsignedShort`, `unsignedByte` and `positiveInteger` in the XML Schema namespace.
These types work in existing simple roots, flat/nested leaves, local attributes,
direct named/anonymous restrictions and admitted integer simple-content placements.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="counter" type="xs:unsignedLong"/>
</xs:schema>
```

`<counter>18446744073709551615</counter>` assesses Valid. The scalar facade reads
`ReadRootScalar().IntegerValue`; the original-node facade reads
`ReadElement([]).IntegerValue`. Both return an exact `BigInteger`, including values
above Int64. Root values remain separate from descendant scalar indexes.
An unsignedLong value of `18446744073709551616` or `-1` is Invalid within admitted
capacity; `-0` is a valid spelling of zero. Export preserves parsed lexical content
within the existing serialization contract. Detached values carry no schema authority.

Each descendant keeps its original built-in type and derivation parent, inherited
inclusive bounds, fixed fractionDigits zero and fixed collapse whitespace behavior.
Direct restrictions may narrow existing bound/digit facets; weakening recognized
bounds or violating an admitted fixed facet is Invalid. Enumeration, integer pattern
facets and explicit whiteSpace/fixed-attribute facet syntax remain Unsupported.
No arbitrary new user-defined restriction chains are admitted.

Same-family `xsi:type` follows the actual derivation graph: int to byte is valid
when the selected byte value fits; int to integer and long to unsignedInt are invalid
selections even if the value would fit. Existing representation-changing selection
from decimal into an integer type remains Unsupported. Source/instance namespace
resolution and original declaration identity stay separate from selected type identity.

Explicit lexical digit capacity still applies before normalization and value-space
checks, including leading zeros. Standard built-in bound metadata does not require
a caller's small value digit limit to contain all standard bound constants, and
does not consume user source-definition quotas. Existing source/node/anonymous,
query, cancellation, ownership and lifetime limits remain. Legacy NativeDecimalSession,
public API, Core pins and published beta.1 are unchanged. This bounded profile does
not claim full datatype value spaces, general typed queries, closure or full XSD/PSVI.


### Bounded root choice

The opt-in `NativeSchemaSession` and `NativeSchemaNodeDataSession` also accept a root complex type containing one `xs:choice`: outer `minOccurs` 0 or 1, `maxOccurs` 1, with 1-16 direct scalar element alternatives. Each alternative requires `minOccurs` 1 and permits `maxOccurs` 1-16 within existing scalar and document budgets. One alternative is selected by expanded name; mixed alternatives are Invalid. Empty optional choice has no root typed value and publishes no omitted-branch scalar values. Existing scalar restrictions, root attributes and structural namespace assertions retain their existing exclusions.

The choice subset excludes repeated outer groups, nullable alternatives, nested choice containers, complex alternatives, implicit `xs:anyType`, wildcards and group references. These bounds are profile exclusions, not general XSD 1.1 limits. The legacy decimal-only grammar remains unchanged.


### Bounded root all group

The same two opt-in native facades also accept a root complex type containing one `xs:all`: outer minOccurs 0 or 1 and maxOccurs 1, with 1-16 direct explicitly scalar-typed members, each minOccurs 0 or 1 and maxOccurs 1. Present members may appear in any physical order; duplicate or unknown names, absent required members and bad scalar values are Invalid. Original declarations, governing types, scalar indexes and original-node paths follow the actual input order.

An absent optional whole group permits empty content. Once any member appears, all required members apply. A required group whose members are all optional can also match empty content. These valid empty roots remain element-only with no typed value (FOTY0012); they do not become empty-content types. An omitted member has no scalar slot; a present empty string remains one atomic. Root attribute reads retain their existing declaration-order policy.

Existing scalar restrictions, instance type selection, root attributes, structural namespace assertions and resource limits retain their current bounds. All-group budgets sum member maxima; choice keeps its maximum-alternative accounting. Nested all containers, complex members, group references, wildcards, implicit anyType and wider group/occurrence grammar remain excluded. These are profile limits, not full XSD 1.1 coverage. The legacy decimal-only grammar is unchanged.


Example all-group schema:

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="r"><xs:complexType><xs:all minOccurs="0">
    <xs:element name="a" type="xs:string"/>
    <xs:element name="b" type="xs:int"/>
  </xs:all></xs:complexType></xs:element>
</xs:schema>
```

A reordered instance with a present empty string:

```xml
<r><b>7</b><a/></r>
```

For this instance, `NativeSchemaSession` exposes two scalar slots: integer `7` at index 0 and an empty string at index 1. `NativeSchemaNodeDataSession` reads the same values at physical paths `[0]` and `[1]`. With this optional whole group, `<r/>` is valid and has no scalar slots or root typed value. `<r><b>7</b></r>` is Invalid because the present group still requires `a`. Check the returned outcome before reading its value.

### Bounded integer enumeration

The two opt-in native facades support enumeration on the existing integer primitive and twelve builtin descendants, at admitted named/anonymous roots, scalar members, attributes and integer simple-content placements. Entries are base-typed values: signs, leading zeros and permitted XML whitespace do not create distinct integers. Bad base-typed declarations are Invalid; values outside the set are Invalid instances. Other applicable facets still apply. Inherited enumeration is retained or validly narrowed.

At most sixteen source entries are admitted per restriction, including duplicates before deduplication. Existing 256-character lexical and explicit native digit limits apply before normalization. Capacity refusal is Unsupported. Integer patterns, arbitrary new chains and broad facet metadata syntax remain excluded. Attribute-only empty complex roots remain outside the profile; use an admitted element-only root for enumeration attributes.

Example integer enumeration:

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="N"><xs:restriction base="xs:int">
    <xs:enumeration value="1"/>
    <xs:enumeration value="+0002"/>
    <xs:maxInclusive value="1"/>
  </xs:restriction></xs:simpleType>
  <xs:element name="r" type="N"/>
</xs:schema>
```

```xml
<r>+001</r>
```

The instance reads exact integer one through ReadRootScalar() or ReadElement([]), with zero descendant scalar slots. Export preserves parsed lexical content. Two fails the sibling bound; zero fails membership. The declaration for two is base-int-valid even though the new sibling bound excludes it. Check outcomes before reading values. The legacy decimal facade is unchanged.

### Bounded native dates

The two opt-in native facades support xs:date at admitted simple roots, scalar members and attributes, including direct facet-free named/anonymous date restrictions. NativeScalarKind.Date (5) exposes an immutable NativeDateValue through DateValue: Year, Month, Day and nullable TimezoneOffsetMinutes. Null means no timezone; zero means explicit UTC. Detached values carry no schema authority and survive disposal.

Example date schema and instance:

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="date" type="xs:date"/>
</xs:schema>
```

```xml
<date>0000-02-29+14:00</date>
```

ReadRootScalar().DateValue or ReadElement([]).DateValue yields year zero, month 2, day 29 and offset 840 minutes. Export retains parsed lexical content. No conversion through DateTimeOffset or implicit timezone is used. Extended years and negative astronomical years use the Gregorian calendar directly.

The explicit year interval is -9223372036854775807 through 9223372036854775807, with at most 256 normalized lexical characters plus existing document/work capacities. Otherwise-valid values outside these bounds are Unsupported/RepresentationCapacity; invalid bounded lexical/calendar values are Invalid. The excluded long.MinValue year cannot be formatted by the pinned Core carrier and is never silently rounded. Temporal patterns and assertions, complex date restriction chains, arbitrary derived chains, date assertions and dateTime/time/dateTimeStamp remain outside this slice. The legacy decimal facade is unchanged; full calendar operations, PSVI and XSD1.1 conformance are not implied.

### Dates with simple-content metadata

The opt-in native facades also support direct simpleContent/extension of xs:date or an admitted direct date restriction, at existing named/anonymous roots and terminal placements. The complex governing type and the atomic date base remain distinct; reads use the existing NativeScalarKind.Date and NativeDateValue. No new scalar API is required.

Example date with attribute metadata:

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:complexType name="Dated"><xs:simpleContent><xs:extension base="xs:date">
    <xs:attribute name="label" type="xs:string" use="required"/>
  </xs:extension></xs:simpleContent></xs:complexType>
  <xs:element name="issued" type="Dated"/>
</xs:schema>
```

```xml
<issued label="invoice">0000-02-29</issued>
```

ReadRootScalar().DateValue or ReadElement([]).DateValue yields year zero, month 2, day 29 and no timezone. The root has zero descendant scalar slots. ReadAttribute(0).Value.StringValue or ReadAttribute([], "", "label").StringValue yields invoice. Values remain usable after disposal; XML export preserves parsed content and attributes.

Missing required, invalid typed or undeclared ordinary attributes are Invalid in this date-extension closure, including when zero attributes were declared. Existing scalar/decimal empty-use behavior is unchanged. Date simple content retains all date limits above and existing per-owner/expanded attribute, node, scalar and depth budgets. Temporal patterns/assertions, date complex restriction chains, arbitrary complex chains and general simple-to-complex xsi:type substitution remain excluded. Exact same complex xsi:type is supported. The legacy decimal facade remains unchanged.

### Bounded date enumeration

Direct named or anonymous restrictions of xs:date may contain up to 16 enumeration entries, including duplicate or equal entries. Existing root, scalar and attribute placements and date simple-content extension may use these types. A valid nonmember is Invalid. Members retain the original instance date and timezone offset; an absent timezone is distinct from explicit UTC. Matching never uses the assessment context's implicit timezone.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Allowed"><xs:restriction base="xs:date">
    <xs:enumeration value="2000-01-01+13:00"/>
  </xs:restriction></xs:simpleType>
  <xs:element name="issued" type="Allowed"/>
</xs:schema>
```

```xml
<issued>1999-12-31-11:00</issued>
```

These dates begin at the same instant, so membership succeeds. ReadRootScalar().DateValue or ReadElement([]).DateValue returns the instance's year 1999, month 12, day 31 and offset -660 minutes. Export retains its parsed lexical content. The matching enumeration entry does not replace the instance value.

All entries must be valid dates for the base type. Missing/invalid bounded facet values are Invalid schemas. The 17th source entry is Unsupported/RepresentationCapacity even when duplicates would collapse to fewer values. Facet and enumerated-instance text also obey MaxStringCharacters; the existing normalized 256-character and year capacities apply. Whitespace inside an otherwise empty enumeration element is allowed. General string/integer user-defined simple restriction chains, date assertions and date complex restriction chains remain outside the profile. This feature does not change public equality/hash methods, XPath date comparison semantics or the legacy decimal facade.

### Date bounds and timezone presence

Direct date restrictions may combine enumeration with minInclusive, minExclusive, maxInclusive, maxExclusive and explicitTimezone optional/required/prohibited. Bounds use XSD partial ordering. A value incomparable with a bound fails that facet; an implicit assessment timezone is never substituted. Existing date year, lexical, enumeration and document limits remain.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Window"><xs:restriction base="xs:date">
    <xs:minInclusive value="2000-01-01Z"/>
    <xs:maxExclusive value="2000-01-03Z"/>
    <xs:explicitTimezone value="required"/>
  </xs:restriction></xs:simpleType>
  <xs:element name="issued" type="Window"/>
</xs:schema>
```

```xml
<issued>2000-01-02Z</issued>
```

The instance is Valid and retains year 2000, month 1, day 2 and explicit zero offset. The same date without a timezone is Invalid. The upper-bound date is excluded. Base-valid enumeration and bound declarations are not validated against sibling facets; an empty intersection is not itself a schema error. Fixed facets and date complex restriction chains remain separate.

### Named date derivation chains

Named date restrictions may derive from other admitted named date types. Forward references are resolved within the closed schema; every component retains its own identity. Facet-free aliases inherit all constraints. Enumeration members and bound values must be valid for the base type, with the XSD exception for repeating an inherited same-side exclusive endpoint. Explicit timezone constraints may narrow optional to required/prohibited, but cannot widen or swap an inherited required/prohibited constraint.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Window"><xs:restriction base="Start">
    <xs:maxExclusive value="2000-01-03Z"/>
  </xs:restriction></xs:simpleType>
  <xs:simpleType name="Start"><xs:restriction base="xs:date">
    <xs:minInclusive value="2000-01-01Z"/>
    <xs:explicitTimezone value="required"/>
  </xs:restriction></xs:simpleType>
  <xs:element name="issued" type="Window"/>
</xs:schema>
```

```xml
<issued>2000-01-02Z</issued>
```

This forward reference compiles, and the instance retains its original date and zero offset. Both inherited lower/timezone constraints and the derived upper bound apply. Up to 16 global type definitions are admitted; cycles, unresolved bases and built-in redeclarations are Unsupported. Fixed facets, general string/integer restriction chains, date complex restriction chains remain outside this slice.

### Date complex simple-content restriction

An attribute-free, assertion-free date simple-content extension may be restricted by a named or anonymous complex type. Local facets are validated against the actual inherited date content type. The governing complex type and its anonymous atomic content restriction remain distinct, while public reads still return the original Date value.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:complexType name="Dated"><xs:simpleContent>
    <xs:extension base="xs:date"/>
  </xs:simpleContent></xs:complexType>
  <xs:complexType name="Window"><xs:simpleContent><xs:restriction base="Dated">
    <xs:minInclusive value="2000-01-01Z"/>
    <xs:maxExclusive value="2000-01-03Z"/>
    <xs:explicitTimezone value="required"/>
  </xs:restriction></xs:simpleContent></xs:complexType>
  <xs:element name="issued" type="Window"/>
</xs:schema>
```

```xml
<issued>2000-01-02Z</issued>
```

Both facades return year 2000, month 1, day 2 and explicit zero offset. Bounds and timezone requirements apply without normalizing the instance into another date. Caller character capacities apply to facets at roots and nested terminals. Local/inherited attributes, assertions, complex restriction chains, inline simple types and general complex xsi:type substitution remain outside this bounded restriction profile. Exact same named complex xsi:type selection is supported.

### Date lexical patterns

Date restrictions support the existing finite pattern grammar. Same-step patterns are alternatives; inherited groups all apply to the whitespace-collapsed lexical form. Calendar validity still applies first. Patterns distinguish Z, +00:00 and absent timezones without replacing the instance value.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="D"><xs:restriction base="xs:date">
    <xs:pattern value="2000-01-02Z"/>
  </xs:restriction></xs:simpleType>
  <xs:element name="issued" type="D"/>
</xs:schema>
```

```xml
<issued>2000-01-02Z</issued>
```

Both facades preserve the original date and XML. The same date without a timezone, the +00:00 spelling and 2000-01-03Z are Invalid for this pattern. Enumeration declarations must satisfy inherited lexical patterns; sibling constraints can have an empty intersection. New bounds over an already patterned base remain Unsupported.

Limits are eight pattern siblings, 256 UTF-16 source units, 128 states per pattern, and 1,024 states shared with string patterns across the schema. The existing grammar limits (16-repeat counts, 64 atoms, eight grouping levels, 16 class members) also apply. Date and string matching share the assessment work budget and cancellation. Full Unicode regex, fixed facets, date assertions and complex restriction chains remain outside this profile.

### Boolean patterns and whitespace

Direct Boolean restrictions support finite patterns and explicit whiteSpace collapse. Their four valid lexical forms are true, false, 1 and 0; patterns can distinguish forms with equal typed values. Same-step pattern siblings are alternatives. Whitespace collapse precedes lexical validation and pattern matching, while export preserves original XML text.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="WordTrue"><xs:restriction base="xs:boolean">
    <xs:whiteSpace value="collapse"/>
    <xs:pattern value="true"/>
  </xs:restriction></xs:simpleType>
  <xs:element name="enabled" type="WordTrue"/>
</xs:schema>
```

```xml
<enabled> true </enabled>
```

Both facades read Boolean true and preserve the original text. The lexical form 1 is Invalid for this pattern, although it maps to the same Boolean value. True and 2 are always Invalid Boolean forms. Enumeration and ordering/digit/length/timezone facets are inapplicable to Boolean and make such a restriction Invalid. Whitespace is fixed to collapse: preserve/replace and duplicate whiteSpace declarations are Invalid.

Patterns use the same finite grammar and shared schema-state/assessment-work budgets as strings and dates. The existing raw Boolean lexical limit is 256 characters. Boolean complex content, assertions and fixed facet attribute syntax remain separate work. Unsupported grammar/capacity is reported explicitly. No public API or published-package change.

### Named Boolean restriction chains

Named Boolean restrictions may derive from admitted named Boolean simple types. Forward references resolve in the closed schema. Facet-free aliases retain their actual base identity and every inherited pattern group. Same-step patterns are alternatives; inherited groups all apply. An empty lexical intersection is a valid schema with no matching instances.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="WordTrue"><xs:restriction base="Either">
    <xs:pattern value="true"/>
  </xs:restriction></xs:simpleType>
  <xs:simpleType name="Either"><xs:restriction base="xs:boolean">
    <xs:pattern value="(true|1)"/>
  </xs:restriction></xs:simpleType>
  <xs:element name="enabled" type="WordTrue"/>
</xs:schema>
```

```xml
<enabled> true </enabled>
```

Both facades return Boolean true and preserve original XML text. The lexical form 1 fails the derived pattern. Patterns are compiled once per definition and inherited matching shares the assessment budget and cancellation. Sixteen global types, including unused definitions, remain the shared cap. Cycles, unresolved bases and built-in redeclarations remain Unsupported. Closed xsi:type substitution follows actual ancestry; sibling types are not interchangeable. Boolean complex content, assertions and fixed attribute syntax remain outside this slice.

### Direct decimal enumeration

The opt-in scalar and original-node facades admit enumeration on direct decimal restrictions. Membership uses normalized exact coefficient/scale values, so signs, trailing zeroes and signed zero do not change membership. The original instance XML is preserved; an enumeration literal never replaces the instance value.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Price"><xs:restriction base="xs:decimal">
    <xs:enumeration value="1.2300"/>
  </xs:restriction></xs:simpleType>
  <xs:element name="price" type="Price"/>
</xs:schema>
```

```xml
<price>+001.23</price>
```

Both facades return the exact decimal value 1.23 and export the original text. The value 1.24 is Invalid. Enumeration combines with local bounds/digit facets; a base-valid enumeration declaration is not rejected against sibling constraints, and an empty intersection has Invalid instances. Missing or invalid decimal members are Invalid schemas.

Sixteen source entries are allowed, counting duplicates before deduplication. Existing 256-character numeric source and pre-normalization digit/scale capacities apply; excess is Unsupported/RepresentationCapacity. String text limits do not replace numeric capacities. The legacy decimal-only facade keeps its prior enumeration refusal. Fixed facet attributes, decimal patterns and assertion expansion remain separate work. Public equality/hash APIs and Core payloads are unchanged.

### Named decimal restriction chains

Named decimal restrictions may derive from admitted named decimal simple types, retaining exact enumeration, range and digit constraints and actual base-type identities. Forward references resolve in the closed schema. Enumeration and bound declarations must be valid for their actual base, with the REC exception for repeating a same-side exclusive endpoint. Bounds and digit facets cannot widen the base. Base-valid sibling declarations can have an empty intersection.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Price"><xs:restriction base="Prices">
    <xs:enumeration value="1.2300"/>
  </xs:restriction></xs:simpleType>
  <xs:simpleType name="Prices"><xs:restriction base="xs:decimal">
    <xs:enumeration value="1.23"/>
    <xs:enumeration value="2.34"/>
  </xs:restriction></xs:simpleType>
  <xs:element name="price" type="Price"/>
</xs:schema>
```

```xml
<price>+001.23</price>
```

Both facades return exact decimal 1.23 and preserve the input XML. The value 1.24 is Invalid. A derived enumeration value outside Prices makes the schema Invalid; inherited facets are not silently dropped by an alias. Sixteen global definitions and sixteen source enum entries per step remain bounded, with existing numeric capacities checked before normalization. Closed xsi:type follows actual ancestry. Legacy decimal-only grammar, fixed/annotation metadata and decimal patterns/assertions remain outside this slice.

### Decimal complex enumeration restriction

The existing attribute/assertion-free decimal simple-content extension route supports a named or anonymous complex restriction with local or inherited enumeration. Governing complex and anonymous atomic restriction components retain their distinct identities and actual bases. Declaration values must be valid for the atomic base; sibling constraints may still have an empty intersection.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Prices"><xs:restriction base="xs:decimal">
    <xs:enumeration value="1.23"/><xs:enumeration value="2.34"/>
  </xs:restriction></xs:simpleType>
  <xs:complexType name="DatedPrice"><xs:simpleContent>
    <xs:extension base="Prices"/>
  </xs:simpleContent></xs:complexType>
  <xs:complexType name="Price"><xs:simpleContent><xs:restriction base="DatedPrice">
    <xs:enumeration value="1.2300"/>
  </xs:restriction></xs:simpleContent></xs:complexType>
  <xs:element name="price" type="Price"/>
</xs:schema>
```

```xml
<price>+001.23</price>
```

Both facades return exact decimal 1.23 and preserve the original XML text. The value 1.24 is Invalid. A minInclusive declaration of 0 over Prices would make the schema Invalid because 0 is outside that base's enumeration. Bounds/digits must narrow and be base-valid, with the REC exception for repeating an inherited same-side exclusive endpoint.

Existing entry, raw numeric, digit/scale and document limits apply. Local/inherited attributes, assertions, inline simple types, general complex restriction chains and general complex xsi:type substitution stay Unsupported; exact same named complex selection is supported. Integer/date restriction routes and the legacy decimal-only grammar retain their existing profiles. No public API, Core payload or equality/hash change.

### Global attribute declarations and references

The opt-in facades support typed global attributes and local references in existing admitted attribute owners. Each reference has its own optional/required presence constraint and retains the canonical global declaration and scalar type. A global attribute uses targetNamespace regardless of attributeFormDefault; reference QNames follow the source namespace bindings.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:attribute name="price" type="Price"/>
  <xs:simpleType name="Price"><xs:restriction base="xs:decimal">
    <xs:enumeration value="1.23"/>
  </xs:restriction></xs:simpleType>
  <xs:element name="order"><xs:complexType>
    <xs:sequence><xs:element name="amount" type="xs:decimal"/></xs:sequence>
    <xs:attribute ref="price" use="required"/>
  </xs:complexType></xs:element>
</xs:schema>
```

```xml
<order price="+001.2300"><amount>1</amount></order>
```

ReadAttribute(0).Value or ReadAttribute([], "", "price") returns exact Decimal 1.23. The amount remains the first scalar/element child with value 1, and export preserves original XML. A missing required price or value 1.24 is Invalid. Optional uses do not inherit another owner's required flag.

Sixteen global declarations and sixteen attribute uses per owner are bounded; name/ref source limits are256UTF16units. Unused globals are validated. Malformed/empty/unbound ref QNames and illegal ref with name/type/form are Invalid; missing closed references are Unsupported. Global lookup keeps canonical declaration identity, with NotFound for a missing name in a present global catalog; empty-catalog profiles retain their prior Unsupported lookup. No external resolution occurs.

Existing primitive/named scalar attribute types and sequence/all/choice, decimal/date extension and nested owners retain their limits. Attribute-only empty complex owners, integer extension attributes, complex restriction attributes, prohibited uses, anonymous attribute types, defaults/fixed and imports remain separate work. Legacy decimal-only grammar and public/Core APIs are unchanged.

### Attribute groups

Named attribute groups collect existing local attributes and global references. Closed forward and nested acyclic references are resolved once; each owner receives the set union of the actual attribute-use components. Repeated references to the same group or shared diamond use do not clone components or duplicate facts. Different use components with the same expanded attribute name are Invalid.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Price"><xs:restriction base="xs:decimal">
    <xs:enumeration value="1.23"/>
  </xs:restriction></xs:simpleType>
  <xs:attributeGroup name="OrderAttributes">
    <xs:attribute name="price" type="Price" use="required"/>
  </xs:attributeGroup>
  <xs:element name="order"><xs:complexType>
    <xs:sequence><xs:element name="amount" type="xs:decimal"/></xs:sequence>
    <xs:attributeGroup ref="OrderAttributes"/>
  </xs:complexType></xs:element>
</xs:schema>
```

```xml
<order price="+001.2300"><amount>1</amount></order>
```

Both facades read exact price 1.23 and preserve original XML and amount indexing. The required flag belongs to the group use. Group-local declarations remain local to their actual definition; global references retain their canonical global declaration.

Limits are sixteen group definitions, sixteen raw attribute/group members and sixteen effective uses after union, with256UTF16name/ref source caps. Unused definitions are validated. Empty groups contribute no uses. Circular groups are permitted by XSD1.1, but fixed-point expansion is outside this bounded profile: cycles return Unsupported, never Invalid solely for circularity. Missing closed groups also return Unsupported. Wildcards, defaultAttributes, imports/redefine/override, annotations/default/fixed/prohibited metadata and previously excluded owner shapes remain separate. No external resolution, Core or public API change.

Structural assertions combined with application attributes or attribute-group members remain Unsupported in the bounded nested profile, matching root/all/choice owner boundaries.

### Global element references

Closed global declarations can be referenced by bounded particles. This sequence admits one or two decimal items; each original node retains the shared global declaration and its own value. Recursive graphs, substitution groups and nillable/default/fixed metadata remain outside this profile.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="order"><xs:complexType><xs:sequence>
    <xs:element ref="item" minOccurs="1" maxOccurs="2"/>
  </xs:sequence></xs:complexType></xs:element>
  <xs:element name="item" type="xs:decimal"/>
</xs:schema>
```

```xml
<order><item>1.2300</item><item>2.34</item></order>
```

Use `ReadScalar(0)` and `ReadScalar(1)`, or node-data `ReadElement([0])` and `ReadElement([1])`, to obtain exact Decimal values 1.23 and 2.34. Export preserves the supplied XML.

### Named sequence model groups

Closed named sequence groups reuse actual local declarations across owners. Group references currently require exactly one occurrence; element particles retain their admitted bounds. Other compositors, circular groups and recursive element graphs have explicit refusal outcomes.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="order"><xs:complexType><xs:group ref="Items"/></xs:complexType></xs:element>
  <xs:group name="Items"><xs:sequence>
    <xs:element name="item" type="xs:decimal" minOccurs="1" maxOccurs="2"/>
  </xs:sequence></xs:group>
</xs:schema>
```

```xml
<order><item>1.2300</item><item>2.34</item></order>
```

Both facades read exact Decimal values 1.23 and 2.34 using scalar indexes or physical element paths. Original XML remains unchanged.

### Closed schema includes

Both facades offer `Create(rootDocumentUri, schemaDocuments, options)`. Supply a closed `IReadOnlyDictionary<string,string>` of absolute URI identifiers and schema text. Relative includes use only that catalog. Matching target namespaces and effective form/assertion defaults are required; chameleon conversion and imports remain outside the profile. The existing single-string overload has no resolver.

Root schema (`https://schemas.example/root.xsd`):

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:include schemaLocation="types.xsd"/>
  <xs:element name="price" type="Price"/>
</xs:schema>
```

Included schema (`https://schemas.example/types.xsd`):

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Price"><xs:restriction base="xs:decimal">
    <xs:enumeration value="1.23"/>
  </xs:restriction></xs:simpleType>
</xs:schema>
```

Instance:

```xml
<price>+001.2300</price>
```

```csharp
var schemaDocuments = new Dictionary<string, string>
{
    ["https://schemas.example/root.xsd"] = rootSchema,
    ["https://schemas.example/types.xsd"] = includedSchema
};
using var session = NativeSchemaSession.Create(
    "https://schemas.example/root.xsd", schemaDocuments, options).Value!;
```

ReadRootScalar returns exact Decimal 1.23. The node-data facade uses the same catalog overload and `ReadElement([])`. Original XML is preserved; catalog changes after creation do not affect the compiled session. Source URI provenance is not exposed as a new public fact.

### Closed named simple-type imports

The catalog overloads can import foreign named simple restrictions, including cross-library base chains. Imported documents may contain only named simple types and closed composition directives. Foreign element/complex/attribute/group owners remain outside this profile. The import namespace must match the actual library targetNamespace; source prefix bindings and actual type identities remain intact.

Root schema (`https://schemas.example/root.xsd`):

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:p="urn:pricing">
  <xs:import namespace="urn:pricing" schemaLocation="types.xsd"/>
  <xs:element name="price" type="p:Price"/>
</xs:schema>
```

Library (`https://schemas.example/types.xsd`):

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" targetNamespace="urn:pricing">
  <xs:simpleType name="Price"><xs:restriction base="xs:decimal">
    <xs:enumeration value="1.23"/>
  </xs:restriction></xs:simpleType>
</xs:schema>
```

Instance:

```xml
<price>+001.2300</price>
```

Supply these texts using the preceding catalog example. Both facades read exact Decimal 1.23; the governing named type is `{urn:pricing}Price`. Existing single-string Create still refuses composition. No host network/file lookup or new public source-URI provenance is implied.

### Simple nillability and empty typed values

Simple-valued declarations can be nillable. A nilled element has a supported empty typed sequence: `NativeScalarKind.Empty`. This differs from the scalar empty string and from an element-only container with no typed value (`FOTY0012`). Any xsi:nil attribute, including false, requires a nillable declaration. True nil forbids character or element content, including whitespace. Complex nillability and assertion+nillability combinations remain outside this profile.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="price" type="xs:decimal" nillable="true"/>
</xs:schema>
```

```xml
<price xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true"/>
```

For the scalar facade, `IsRootNilled` is true, `HasRootScalar` is false and `ReadRootScalar().Kind` is Empty. Descendant scalar-bearing positions, including nilled simple positions, retain document-order indexes. The node-data facade returns true from `IsNilled([])` and Empty from `ReadElement([])`. Nonempty accessors on Empty throw InvalidOperationException. The existing non-nilled empty-complex typed sequence is also readable as Empty, with its nil flag false; element-only absent typed values still raise FOTY0012.

Both facades preserve the original XML and use authenticated original-node atomization for detached values. Empty results remain usable after disposal; nil flags and original facts retain their owner lifetime. The legacy decimal-only facade keeps its earlier grammar and typed-identity route.

### Provided element default and fixed constraints

Simple element default/fixed literals are checked against their complete declared types and facets. Provided fixed content uses actual value equality, including equivalent decimal/Boolean/integer/date lexical forms; existing string restrictions preserve strings. Default metadata does not replace supplied content. Both metadata attributes together are Invalid.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="price" type="xs:decimal" fixed="1.23"/>
</xs:schema>
```

```xml
<price>+001.2300</price>
```

Both facades read exact Decimal 1.23 and retain the original XML. A provided1.24 is Invalid. A nilled fixed-value element is Invalid; nilled defaults remain suppressed. An absent optional element is not created. A present empty element that requires default/fixed application returns Unsupported, because nonempty simple defaults also need correctly owned XDM text nodes. Complex value constraints and reference-use overrides remain outside this profile; this is not full default application.

### Provided attribute default and fixed constraints

Existing attribute owners admit default/fixed metadata, including globals and reference uses. Literals must satisfy complete type/facets. Provided fixed values compare by value; a local override of a global fixed declaration must be fixed with an identical value. Date equality can span different calendar/offset representations, while override identity cannot. Provided defaults never replace actual attributes, including empty strings.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:attribute name="price" type="xs:decimal" fixed="1.23"/>
  <xs:element name="order"><xs:complexType>
    <xs:sequence><xs:element name="amount" type="xs:decimal"/></xs:sequence>
    <xs:attribute ref="price" use="required"/>
  </xs:complexType></xs:element>
</xs:schema>
```

```xml
<order price="+001.2300"><amount>1</amount></order>
```

Both facades read exact Decimal1.23 from the attribute and1from the child, preserving original XML. A supplied1.24 or required absence is Invalid. Optional missing attributes needing default/fixed application return Unsupported until correctly owned augmented attribute nodes are supported. Explicit default requires optional use; explicit fixed cannot use prohibited. Inline attribute types, inherited attributes, wildcards and schema defaultAttributes remain outside this profile.

### Mixed sequence content

Finite nonempty sequence models can be mixed. Their original aggregate text is one xs:untypedAtomic value under the actual governing complex type, while child types/facets and attribute constraints remain independently validated. The detached facade projects textual payloads as Kind.String without transferable authority; actual Core facts retain xs:untypedAtomic rather than xs:string.

```xml
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="message"><xs:complexType mixed="true"><xs:sequence>
    <xs:element name="amount" type="xs:decimal"/>
  </xs:sequence></xs:complexType></xs:element>
</xs:schema>
```

```xml
<message>Total: <amount>1.2300</amount> EUR</message>
```

HasRootScalar is true; ReadRootScalar returns String `Total: 1.2300 EUR`, and ReadScalar(0) returns exact Decimal1.23. Node-data reads the same values at paths[]and[0]. ScalarCount includes supported descendant mixed value-bearing containers in preorder, plus existing simple/simple-content positions; element-only containers and attributes remain excluded. Original element paths ignore intervening text nodes.

An empty mixed aggregate is an empty string atom, not an empty typed sequence or nil. The16value-bearing-node ceiling includes each mixed aggregate and the root; each aggregate also obeys MaxStringCharacters. Mixed all/choice/empty/simple-content/assertion forms, default augmentation and complex nillability remain outside this profile. Comments/PI remain excluded; mixed text does not allow unknown children or invalid child values.

## Lists and unions (0.9 source)

Both native facades admit bounded lists and ordered unions over the existing admitted member families. NativeSchemaDocument adds ReadRootItems, ReadScalarItems, ReadAttributeItems and ReadDescendantAttributeItems; NativeSchemaNodeDataDocument adds ReadElementItems and ReadAttributeItems. Each returns an immutable array of detached NativeScalarValue items under the existing Core operation/lifetime gate. Atomic values become one item; nil and empty lists become zero items, distinguished by existing nil facts. Scalar accessors refuse list values, including empty/singleton lists and unions selecting lists. HasRootScalar is false for lists.

Lists have at most16items; unions have at most8directmembers. Raw compound values are limited to256UTF16units and caller string capacity. Inline member definitions are limited to16source sites/depth4; shared compound attempts are limited to4096 per document or schema-literal budget. Unsupported/capacity results stop union assessment and cannot fall through to a later member. Members are checked eagerly, even in unused definitions. Existing finite facets apply; general assertions, recursion, identity graphs, enumeration over patterned compound ancestors and direct union-member xsi:type override remain excluded. Named compound restriction selection is supported. See the [datatype guide](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/datatypes-200/GUIDE.md) for the precise grammar and current delivery status. No registry publication or complete XSD1.1 claim is implied.

## Native text datatypes (0.10 source)

Both owned native facades admit normalizedString, token, language, Name, NCName, NMTOKEN and nonempty NMTOKENS, plus named string restriction chains. Text whitespace is normalized before intrinsic lexical and finite facet checks; original parsed XML remains available for export. Detached text uses the existing String kind/getter. NMTOKENS uses explicit ordered item reads and preserves actual member provenance. String simple-content extensions and the earlier bounded restriction grammar are supported. The [text datatype guide](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/text-200/GUIDE.md) contains a runnable example, exact inheritance rules and capacities. Identity graphs, arbitrary regex/assertions, augmentation, general PSVI/C45 and a public schema query factory remain excluded. This is an unpublished source candidate.

## Empty complex owners with attributes (development0.17)

Declared attributes now work on bounded empty complex owners. Empty elements retain an empty typed sequence and reject all text, including whitespace; attribute facts retain their original types and owner paths. See the [consumer guide](empty-owners/GUIDE.md) for supported placements, capacities and exclusions.

## Numeric lexical patterns (development0.18)

Bounded integer and decimal pattern facets validate collapsed original spelling before exact numeric normalization. Same-step alternatives and inherited decimal conjunctions retain original values, components and lifetime. See the [consumer guide](numeric-patterns/GUIDE.md) for the finite grammar, capacities and explicit value-space/inheritance exclusions.

## Text-only anyType terminals (development0.19)

Implicit and explicit xs:anyType now work for bounded text-only roots and sequence/global-reference terminals, with real anyType/untypedAtomic components and original string values. Child and application-attribute wildcards remain Unsupported. See the [guide](anytype-terminals/GUIDE.md) for supported positions, capacities and exclusions.

## Known anyType lax declarations (development0.20)

AnyType can now assess supported known global child/attribute declarations, preserving original components and typed values. Unknown lax content stays Unsupported; known invalid data is never silently skipped. See the [guide](known-lax/GUIDE.md) for actual-document capacities, attribute ordering and provenance boundaries.

## Declaration-absent lax content (development 0.21)

Unknown elements and application attributes can be assessed beneath uninterrupted anyType ancestor chains. Their governing declaration stays absent; node annotations are anyType for elements and anySimpleType for attributes, with original untypedAtomic values. Known descendants still validate against actual global declarations. Unknown content below a strict complex ancestor, unknown xsi:type/nil and special XML attributes remain Unsupported. Existing document/value/string limits apply. See the [scope and qualification](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/absent-lax/README.md). This source candidate is unpublished.

## Strict attribute wildcards (development 0.22)

Empty complex owners admit one direct default/explicit strict anyAttribute with bounded namespace-any/other/target/local or absolute-URI list constraints. Explicit uses take precedence; other attributes must match and resolve to an original global declaration, then validate its actual type/value constraints. Missing globals do not augment XML. Lax/skip, negative constraints, inheritance, nonempty/mixed owners and assertion combinations remain excluded. See the [guide](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/strict-attributes/GUIDE.md). This development candidate is unpublished.

## Strict wildcard exclusions (development0.23)

The bounded empty-owner strict attribute profile supports notNamespace and notQName, including ##defined. QName exclusions use original schema namespace bindings; explicit uses retain precedence. Invalid schema combinations and excluded QName namespaces are checked before assessment. Namespace/QName lists have at most eight256UTF16 tokens. Relative URI constraints, lax/skip and broader owner/composition profiles remain excluded. [Scope and qualification](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/wildcard-exclusions/GUIDE.md). Unpublished development source.

## Strict sequence-owner attribute wildcards (development0.24)

Direct finite nonempty element-only sequence owners now admit the qualified strict attribute wildcard profile, including namespace/QName exclusions. Root/named/nested/global-reference owners retain original type/declaration identity and no-typed-value behavior; child validation remains independent. Mixed/simple-content/all/choice owners, top-level group shorthand, assertion combinations and lax/skip remain excluded. Existing document/constraint/value budgets apply. [Scope and qualification](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/sequence-wildcards/GUIDE.md). Unpublished development source.

## Strict all/choice-owner attribute wildcards (development0.25)

Supported nonempty all/choice root models and named-root aliases admit the qualified strict attribute wildcard profile, including namespace/QName exclusions. Original particle matching, physical-order scalar projection, type/declaration identity and element-only no-typed-value behavior remain. Nested all/choice and complex reference containers, mixed/simple-content/assertion combinations and lax/skip remain excluded. [Scope and qualification](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/group-wildcards/GUIDE.md). Unpublished development source.

## Strict mixed-sequence attribute wildcards (development0.26)

Existing finite mixed sequence owners admit qualified strict attribute wildcards and namespace/QName exclusions. Original mixed type/declaration identities remain; each owner yields one original untypedAtomic aggregate, independently from normalized child/attribute values. Optional absent children can yield an empty string atom, not no typed value. Mixed all/choice, empty mixed models, simple content, assertions and lax/skip remain excluded. Existing aggregate/scalar/document/lifetime bounds apply. [Scope and qualification](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/mixed-wildcards/GUIDE.md). Unpublished development source.

## Strict simple-content attribute wildcards (development0.27)

Existing noninteger simple-content extension owners admit one trailing strict/default-strict anyAttribute after declared uses. Original complex governing components, actual scalar values and independent original global attribute types remain. Roots/named/nested/global-reference owners and aliases retain prior XML, capacity and lifetime behavior. Integer/Boolean extension attributes, restriction wildcards, assertions, complex-base inheritance and lax/skip remain excluded. [Scope and qualification](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/simple-content-wildcards/GUIDE.md). Unpublished development source.

## Lax attribute wildcards on empty owners (development0.28)

Empty complex owners admit bounded lax attribute wildcards. Known attributes validate against original globals. Unknown attributes at an empty root carry no declaration, have anySimpleType node type and untypedAtomic values, and project the assessed root to canonical anyType with one empty string atom; known-only roots retain their empty typed sequence. Original declared schema components remain. Unknown attributes beneath strict ancestors, broader lax owner models and skip remain excluded. [Scope and qualification](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/lax-empty-attributes/GUIDE.md). Unpublished development source.

## Skip attribute wildcards on empty owners (development0.29)

Matched extras are skipped even when globally declared: no node declaration, anySimpleType node type and raw untypedAtomic value. Empty roots with skipped extras project to canonical anyType with one empty string atom; no-extra roots retain original empty typing. Explicit uses retain validation and precedence. Original empty-content validity, namespace/QName exclusions, bounds and lifetime remain. Skipped extras beneath strict ancestors and broader skip owner models remain excluded. [Scope and qualification](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/skip-empty-attributes/GUIDE.md). Unpublished development source.

## Text-only anyType members in all roots (development0.30)

Existing bounded all-group roots admit direct implicit/explicit anyType local members. Original declarations and canonical anyType governing components remain; text-only values yield one original untypedAtomic atom, including an empty string. Order-independent matching retains physical-order facts and typed scalar siblings. Terminal attributes/descendants, complex nillability/value constraints, global complex references, nested all and choice composition remain excluded. [Scope and qualification](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/all-anytype-terminals/GUIDE.md). Unpublished development source.

## Text-only anyType alternatives in choice roots (development0.31)

Bounded choice roots admit direct implicit/explicit anyType alternatives. One selected expanded-name branch may repeat within existing occurrence bounds; mixed branches remain Invalid. Original declarations/canonical anyType and raw untypedAtomic values, including empty strings, retain physical order. Scalar alternatives and element-only root typing remain. Terminal attributes/descendants, complex constraints, global complex references, nullable alternatives, outer repetition and nested groups remain excluded. [Scope and qualification](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/choice-anytype-terminals/GUIDE.md). Unpublished development source.

## Abstract global element declarations (development0.32)

Existing admitted global declarations accept bounded abstract Boolean flags. Direct use of an abstract declaration is Invalid at roots, references and known-lax matches before xsi:type/nil handling. Abstractness belongs to immutable canonical declaration identities, never shared types or same-named locals. False/omitted flags retain ordinary behavior; unused abstract declarations do not poison concrete aliases. Substitution groups, local abstract attributes and abstract complex-type definitions remain excluded. [Scope and qualification](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/abstract-elements/GUIDE.md). Unpublished development source.

## Simple-type final controls (development 0.33)

Explicit final on global named simple types blocks the selected restriction, list, union and simple-content extension methods. Empty sets and #all are supported within the 256-unit source bound. Unrelated methods and ordinary value use remain permitted. Source-bound QNames, forward and unused definitions are checked before schema publication. finalDefault, local simple-type final and complex-type final remain excluded. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/simple-type-final/GUIDE.md). Unpublished development source.

## Abstract complex types (development0.34)

Explicit abstract Boolean flags are admitted on existing supported global named complex types. Abstract actual governing types are Invalid after admitted instance-type resolution, while false/omitted flags retain existing behavior. Type identities remain separate from declarations and content atoms. Broader complex xsi:type overrides, complex nillability and local complex-type abstract remain excluded. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/abstract-complex-types/GUIDE.md). Unpublished development source.

## Bounded anyType nillability (development0.35)

Existing implicit/explicit anyType roots, sequence particles/references and admitted known-lax matches accept declared nillability. True nil yields an original anyType-governed empty typed sequence while independently validating attributes; false/omitted nil retains ordinary untypedAtomic values. General complex nil, all/choice nil members, default/fixed constraints and broader type overrides remain excluded. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/anytype-nillability/GUIDE.md). Unpublished development source.

## Documentation annotation language (development0.36)

Existing xs:documentation wrappers admit bounded xml:lang values under the XML namespace schema union: collapsed valid xs:language or an exact empty undeclaration. Whitespace-only is Invalid. Payloads stay opaque and instance validity, component authority and values remain unchanged. Other wrapper attributes/locations, inherited metadata and annotation PSVI remain excluded. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/annotation-language/GUIDE.md). Unpublished development source.

## Anonymous compound restriction bases (development0.37)

Existing simple-type placements admit inline anonymous list/union restriction bases with direct original base objects, inherited facets and original item/member authority. No synthetic type names or declarations are created. The shared inline restriction/list/union source-site bound is16 and recursive inline depth remains4. Anonymous atomic and complex inline bases remain excluded. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/anonymous-restriction-bases/GUIDE.md). Unpublished development source.

## Anonymous decimal and string restriction bases (development0.38)

Existing inline restriction placements admit decimal and string/normalizedString/token bases using original NativeType objects and facet sources. Decimal value/numeric-pattern and text whitespace/length/enumeration/pattern ancestry remain bounded and independent. Other atomic/complex families stay excluded. The preceding empty-union guard is corrected: empty unions compile but accept no value. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/anonymous-scalar-bases/GUIDE.md). Unpublished development source.

## Anonymous integer and Boolean restriction bases (development 0.39)

Existing inline restriction placements admit already compiled integer-family and Boolean bases using their actual unnamed type objects and original facet sources. Exact integer bounds/enumeration/digits, numeric patterns and Boolean fixed collapse whitespace/pattern ancestry retain existing limits. Other atomic/complex families, assertions and broader named-integer chains remain excluded. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/anonymous-integral-bases/GUIDE.md). Unpublished development source.

## Anonymous date restriction bases (development0.40)

Existing inline restriction placements admit xs:date bases using original unnamed type objects and original facet sources. Existing bounded date equality, enumeration, partial-order bounds, explicitTimezone and lexical pattern ancestry retain their semantics and limits. Other calendar/interchange/complex families and broader derivation remain separate. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/anonymous-date-bases/GUIDE.md). Unpublished development source.

## Anonymous dateTime and time restriction bases (development0.41)

Existing inline restriction placements admit xs:dateTime and xs:time bases using original unnamed type objects and facet sources. Existing exact fractional seconds, enumeration, partial-order bounds, fixed/timezone/whitespace facets and lexical pattern ancestry retain their semantics and limits. dateTimeStamp, other atomic/complex families and wider derivation remain separate. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/anonymous-calendar-bases/GUIDE.md). Unpublished development source.

## Anonymous dateTimeStamp restriction bases (development0.42)

Existing inline restriction placements admit xs:dateTimeStamp bases using original unnamed type objects and original facet sources. Required timezone, exact calendar values and inherited fixed/bound/whitespace/pattern rules retain their existing semantics and limits. Other atomic/complex families and wider derivation remain separate. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/anonymous-stamp-bases/GUIDE.md). Unpublished development source.

## Anonymous Gregorian restriction bases (development0.43)

Existing inline restriction placements admit gYear, gYearMonth, gMonth, gMonthDay and gDay bases using original unnamed type objects and facet sources. Existing bounded Gregorian component/value mapping, timezone equality/partial ordering and fixed/enumeration/bound/whitespace/pattern ancestry retain their semantics and limits. Other atomic/complex families and wider derivation remain separate. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/anonymous-gregorian-bases/GUIDE.md). Unpublished development source.

## Anonymous duration restriction bases (development0.44)

Existing inline restriction placements admit duration, yearMonthDuration and dayTimeDuration bases using original unnamed type objects and facet sources. Existing exact month/second values, family grammar, partial ordering and fixed/enumeration/bound/whitespace/pattern ancestry retain their semantics and limits. Other atomic/complex families and wider derivation remain separate. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/anonymous-duration-bases/GUIDE.md). Unpublished development source.

## Anonymous binary and QName restriction bases (development0.45)

Existing inline restriction placements admit hexBinary, base64Binary and QName bases using original unnamed types and facet elements. Binary octet values/lengths, namespace-aware QName equality, fixed/enumeration/whitespace/pattern ancestry and existing limits remain. QName facet literals retain original namespace context. Other atomic/complex families and wider derivation remain separate. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/anonymous-interchange-bases/GUIDE.md). Unpublished development source.

## Anonymous URI and text-derived bases (development0.46)

Existing inline restriction placements admit anyURI, language, Name, NCName, NMTOKEN, ID and IDREF using original unnamed types and facet sources. Lexical-family normalization, scalar lengths, inherited/fixed facets and validation-root identity checks retain their rules and limits. URI resolution and general key/keyref are separate. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/anonymous-text-bases/GUIDE.md). Unpublished development source.

## Anonymous floating restriction bases (development0.47)

Existing inline restriction placements admit float and double using original unnamed types and facet sources. Existing IEEE family mapping/rounding, NaN/infinities/signed zero, inherited/fixed facets, patterned-ancestor refusals and limits remain. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/anonymous-floating-bases/GUIDE.md). Unpublished development source.

## Named-integer restriction bases (development0.48)

Existing integer placements admit ordinary named integer restriction chains and their bounded numeric-pattern route. Original named base objects/QNames, exact inherited facets, pattern exclusions, budgets and lifetime remain; integer pattern facets use original source elements. [Scope](https://github.com/Fytala-Charles/Bosak.Schema/blob/main/docs/named-integer-bases/GUIDE.md). Unpublished development source; no broader fixed-facet syntax, API or Core pin change.
