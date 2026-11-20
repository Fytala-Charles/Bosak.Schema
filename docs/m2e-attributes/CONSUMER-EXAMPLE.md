# Illustrative future root attribute consumer

This example is **not compiled/executed**: NativeAttributeValue/AttributeCount/
ReadAttribute are proposed APIs. The planning XML/schema are well-formed only,
not currently supported or validated by these transport probes. No published
package includes this attribute surface.

```csharp
using Bosak.Schema;

var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 32, 1, 256, 256);
var created = NativeSchemaSession.Create(File.ReadAllText("future-order.xsd"), options);
if (created.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(created.Diagnostics[0].Code);
    return;
}
using var session = created.Value!;
var assessed = session.Assess(File.ReadAllText("future-order.xml"),
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
// Existing leaf indexes remain id=0, amount=1.
Console.WriteLine(document.ReadScalar(0).StringValue);
Console.WriteLine(document.ReadScalar(1).DecimalValue.ToInvariantString(256));
for (var index = 0; index < document.AttributeCount; index++)
{
    var attribute = document.ReadAttribute(index);
    Console.WriteLine($"{{{attribute.NamespaceUri}}}{attribute.LocalName}: {attribute.Value.Kind}");
}
Console.WriteLine(document.ExportXml());
```

Future expected attributes, in declaration order: currency/EUR string in empty
namespace; paid/false Boolean in empty namespace; quantity/exact9223372036854775808
integer in urn:orders. XML attribute order is deliberately different. An absent
optional paid attribute would produce two entries, never a fabricated false.
Future string/numeric values and exported XML are detached, without PSVI/authority.
