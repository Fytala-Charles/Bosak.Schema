# Native URI and IEEE values

The 0.11 development candidate adds `xs:anyURI`, `xs:float` and `xs:double` to both owned facades on signed Core 1.86.0. Select the exact reviewed local package; these APIs are not in published beta.1.

```csharp
using Bosak.Schema;
var options = new NativeSchemaSessionOptions(256,256,256,65536,1,256,16,256,256);
const string schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:float'/></xs:schema>";
using var session = NativeSchemaSession.Create(schema,options).Value!;
using var document = session.Assess("<r>-0</r>",new(DateTimeOffset.UnixEpoch,0)).Value!;
var value = document.ReadRootScalar();
Console.WriteLine(value.Kind);
Console.WriteLine(unchecked((uint)BitConverter.SingleToInt32Bits(value.FloatValue)).ToString("X8"));
```

Expected output: `Float` and `80000000`. `DoubleValue` reads only Double; `FloatValue` reads only Float; `UriValue` reads only AnyUri. Other getters throw InvalidOperationException. Immutable values remain usable after the document/session closes. Lists/unions use the existing item-read APIs and retain their selected member kinds.

URI normalization collapses XML whitespace. All finite XML character strings are admitted within caller capacity, including relative references, spaces and malformed percent escapes. No URI parsing, resolution, case folding, percent decoding or external I/O occurs. URI equality is ordinal after collapse and is distinct from string schema equality.

Float/Double accept the XSD numeric grammar and INF, +INF, -INF, NaN. Numeric conversion uses .NET10 Single/Double directly after lexical validation, with IEEE rounding, signed zero, subnormals, overflow to infinity and underflow to signed zero. Supplied floating text is capped at 256 UTF-16 units and MaxStringCharacters before collapse; URI text uses MaxStringCharacters and existing XML/input limits. Exact-decimal digit/scale limits do not truncate fixed-size IEEE values.

Finite enumeration, patterns, collapse whitespace, bounds, named/anonymous restrictions, existing placements, provided fixed/default values and closed imports are supported within existing budgets. Same-family NaN is schema-same for enumeration/fixed; signed zeros compare equal. Float, Double and Decimal remain disjoint for schema membership even though ordinary XPath numeric operators have separate promotion rules. NaN satisfies no admitted ordered bound. NaN endpoints and enumeration/bounds over patterned ancestors return Unsupported. The existing complex restriction grammar, assertion, graph, pattern and augmentation exclusions remain. No full XSD1.1, general PSVI/C45, public query factory or registry publication is claimed.
