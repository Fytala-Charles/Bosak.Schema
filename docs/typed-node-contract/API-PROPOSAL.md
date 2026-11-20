# Proposed additive native node-data API

20 November 2026. **Shape proposal only. No public type/member exists yet and
no API acceptance is inferred.** Prefer explicit atomization naming over a
general Evaluate method while the released Core operation is fixed and bounded.

```csharp
public sealed class NativeSchemaNodeDataSession : IDisposable
{
    public static NativeSchemaResult<NativeSchemaNodeDataSession> Create(
        string schema, NativeSchemaSessionOptions options,
        CancellationToken cancellationToken = default);

    public NativeSchemaResult<NativeSchemaNodeDataDocument> Assess(
        string instance, NativeSchemaAssessmentContext context,
        CancellationToken cancellationToken = default);

    public void Dispose();
}

public sealed class NativeSchemaNodeDataDocument : IDisposable
{
    public NativeScalarValue ReadElement(
        ImmutableArray<int> elementPath,
        CancellationToken cancellationToken = default);

    public NativeScalarValue ReadAttribute(
        ImmutableArray<int> ownerElementPath,
        string namespaceUri, string localName,
        CancellationToken cancellationToken = default);

    public string ExportXml(CancellationToken cancellationToken = default);
    public void Dispose();
}
```

The declarations above illustrate signatures and omit implementation/XML comments;
they are not compilable product examples. Implementation must supply complete
public XML documentation and exception contracts. Proposed two sealed owned
types have private constructors, reuse existing immutable options/context/scalar
and result/diagnostic shapes, and expose no callback/raw Core object.

Create compiles a separate fixed node-data environment and schema. Assess must
complete valid native assessment and snapshot admission before returning an owned
document. Invalid/Unsupported has no owned Value. Arguments, cancellation,
closure, overlap and quotas retain the existing exception pattern. A new query
refusal is not an Invalid schema/instance; stage diagnostics and FOTY0012 mapping
must be fixed in adoption without leaking caller schema/XML into messages.

ReadElement accepts empty path for the root, or copied element-child indexes.
ReadAttribute selects a present declared application attribute by its original
owner path and expanded name. Both return detached scalar data, with original
type/provenance authenticated internally. Export retains parsed lexical XML,
not byte identity, schema validity authority or PSVI. Existing NativeSchemaSession,
NativeSchemaDocument and NativeDecimalSession behavior and counts/indexes stay
unchanged. Nothing exposes the synthetic transport provider used in old probes.

Alternative considered: add Evaluate or raw node access to NativeSchemaDocument.
That document belongs to an already assessed `$value` environment; recompiling
an expression and borrowing its original assessment would conflate issuer/episode
bindings. The separate owned facade makes the new environment explicit and avoids
retroactive authority transfer. It costs additional types and reassessment if
the application also needs a scalar-session document. This tradeoff needs actual
native feasibility and owner API review; it is not a new accepted public freeze.
