# Bounded native Gregorian fragments

The 0.13 development candidate adds `gYear`, `gYearMonth`, `gMonth`, `gMonthDay` and `gDay` to both owned facades on signed Core1.86.0. These source APIs are separate from published Schema beta.1; select an exact reviewed development package.

```csharp
using Bosak.Schema;
var options = new NativeSchemaSessionOptions(256,256,256,65536,1,256,16,256,256);
const string schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:gMonthDay'/></xs:schema>";
using var session = NativeSchemaSession.Create(schema,options).Value!;
using var document = session.Assess("<r>--02-29Z</r>",new(DateTimeOffset.UnixEpoch,840)).Value!;
var value = document.ReadRootScalar();
Console.WriteLine(value.Kind);
Console.WriteLine(value.GregorianYear is null);
Console.WriteLine($"{value.GregorianMonth}/{value.GregorianDay}/{value.GregorianTimezoneOffsetMinutes}");
```

Expected output: `GMonthDay`, `True`, `2/29/0`. Five distinct NativeScalarKind values preserve the actual primitive family. GregorianYear is a nullable Int64; GregorianMonth and GregorianDay are nullable integers. A field absent in the primitive value stays null. GregorianTimezoneOffsetMinutes is null when absent and0 for Z/+00:00/-00:00. GregorianValue returns collapsed original lexical text. These reads do not invent calendar fields or perform canonical XSD serialization. Unrelated scalar getters throw InvalidOperationException. Detached values remain usable after document/session disposal; original exported XML is retained. Existing immutable item reads expose list/union members without transferring schema authority.

Years range from -long.MaxValue through long.MaxValue. Year zero and -0000 map to the same year0 while original text is retained; long.MinValue and larger years are Unsupported representation bounds. Raw instance and numeric facet text must fit256 UTF-16 units and MaxStringCharacters before collapse. Fixed Gregorian fields do not consume the separate numeric coefficient/scale budget. Existing schema/input/graph limits,16 enum entries,8 patterns, shared matching work/cancellation and query atom/cache limits still apply.

Lexical rules use the dated XSD1.1 forms: gMonth is --MM without obsolete trailing dashes; gMonthDay admits --02-29 using the REC's leap-year reference. Offsets are within±14:00. Month/day widths and ranges, year spelling and XML-only whitespace collapse are checked before any facts are issued.

For comparison only, REC D.2.1 fills missing fields from1972-12-31T00:00:00, except that an absent day with a present month becomes the last permitted day of that month. For example gYear2000 compares at2000-12-31, gYearMonth2000-02 at2000-02-29, and gMonth--02 at1972-02-29. BigInteger timeline arithmetic preserves extended signed years; DateTime/TimeSpan are not used for production conversion. Unknown timezone comparisons use the full±14-hour interval rather than the caller's implicit timezone. Runtime values incomparable to admitted bounds are Invalid; incomparable local endpoint pairs remain Unsupported.

Enumeration and provided fixed values compare within their actual primitive space, retaining absent/present timezone distinctions. Zoned values can be equal without identical fields: ---01-12:00 equals ---02+12:00 under gDay's completion. Attribute-use overrides of a global fixed literal still require identity, so equal-but-nonidentical values do not bypass that rule. Different Gregorian primitives, strings and numbers remain distinct schema value spaces. Finite pattern/whitespace/explicitTimezone/order/enum/fixed facets and inherited restrictions work on the existing admitted placements. Enumeration/bounds over patterned ancestors, arbitrary regex/assertions, excluded complex grammar and augmentation remain Unsupported.

Equal local minExclusive/maxExclusive endpoints define a valid empty value space. This batch corrects the earlier float/double and duration-family schema rejection and qualifies empty-range invalid assessments without publishing facts. Historical source receipts remain unchanged; the correction has its own pre-implementation adoption and tests.

No general Gregorian XPath/XQuery operators, map keys, ID/IDREF/ENTITY graph, full XSD1.1/PSVI/C45, public query factory or registry publication is implied. [Adoption](ADOPTION.json), [semantic review](FACET-REVIEW.md), [contracts](CONTRACTS.json).
