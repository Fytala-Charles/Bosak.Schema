# Proposed M2C1 consumer

Illustrative use of the proposed surface only. **Not compiled or executed:**
NativeSchemaSession and these new types are not implemented/shipped. The Core
transport probes are separate and do not make this example runnable today.

```csharp
using Bosak.Schema;

var options = new NativeSchemaSessionOptions(
    maxNativeCoefficientDigits: 256, maxNativeScale: 256,
    maxStringCharacters: 256, maxExportCharacters: 65536,
    maxOpenDocuments: 1, maxCachedScalarValuesPerDocument: 4,
    maxTypedResultAtoms: 1, maxQueryCoefficientDigits: 256, maxQueryScale: 256);
var context = new NativeSchemaAssessmentContext(
    new DateTimeOffset(2026, 11, 19, 12, 0, 0, TimeSpan.Zero), 0);
var compiled = NativeSchemaSession.Create(File.ReadAllText("string-order.xsd"), options);
if (compiled.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(compiled.Diagnostics[0].Code);
    return;
}
using var session = compiled.Value!;
var assessed = session.Assess(File.ReadAllText("string-order.xml"), context);
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
for (var index = 0; index < document.ScalarCount; index++)
{
    var value = document.ReadScalar(index);
    switch (value.Kind)
    {
        case NativeScalarKind.String:
            Console.WriteLine(value.StringValue);
            break;
        case NativeScalarKind.Decimal:
            Console.WriteLine(value.DecimalValue.ToInvariantString(256));
            break;
        default:
            throw new NotSupportedException("This application does not handle this scalar kind.");
    }
}
```

With the supplied proposed fixture, expected future output is the preserved string
` INV-001 ` followed by exact decimal `12.5`. Present leaf count is2. Export retains
XML lexical spelling and carries no PSVI. Wrong-kind scalar access throws the
proposed fixed InvalidOperationException. See [the exact contract](API-PROPOSAL.md).
