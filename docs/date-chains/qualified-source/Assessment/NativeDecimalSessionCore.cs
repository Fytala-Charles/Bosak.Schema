// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 15 November 2026
// PURPOSE              : Owns the bounded native scalar workflow and exposes only detached results.
// SPECIAL NOTES        : Internal Core 1.81.2 adapter; finite profile only, no public native factory.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 15-11-2026     | Prototype an owned synchronous native scalar facade                                                 |
//                      | Charles Korthout | 0.2   | 17-11-2026     | Keep the owned implementation behind the public facade; attempt all cleanup actions
//                      | Charles Korthout | 0.3   | 19-11-2026     | Carry bounded Unsupported reasons without changing refusal outcomes                       |
//                      | Charles Korthout | 0.4   | 19-11-2026     | Share explicit mixed scalar profile with preserved decimal compatibility                  |
//                      | Charles Korthout | 0.5   | 19-11-2026     | Add detached root attribute reads while preserving leaf-only counts and typed read indexes |
//                      | Charles Korthout | 0.6   | 20-11-2026     | Read the original root scalar separately from unchanged leaf and attribute indexes       |
//                      | Charles Korthout | 0.7   | 20-11-2026     | Read terminal scalar facts without exposing nested element-only containers as values     |
//                      | Charles Korthout | 0.8   | 20-11-2026     | Read detached descendant attributes through original owner-bound facts and shared gate   |
//                      | Charles Korthout | 0.9   | 21-11-2026     | Probe empty complex typing in isolated copies only |
//                      | Charles Korthout | 0.10   | 22-11-2026     | Adopt qualified bounded structural and empty-content semantics |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Runtime.Vm;

namespace Bosak.Schema;

/// <summary>Owns the bounded native scalar workflow and exposes only detached results.</summary>
internal sealed class NativeDecimalSessionCore : IDisposable
{
    private readonly object gate = new();
    private readonly SchemaQuerySession session;
    private readonly NativeSessionRunner runner;
    private readonly HashSet<Document> documents = new(ReferenceEqualityComparer.Instance);
    private int busy;
    private bool closed;
    private NativeSchemaSessionOptions? scalarOptions;

    private NativeDecimalSessionCore(SchemaQuerySession session)
    {
        this.session = session;
        runner = new NativeSessionRunner(session);
    }

    internal static NativeResult<NativeDecimalSessionCore> Create(string schema, NativeNumericLimits numericLimits,
        SchemaQueryLimits queryLimits, CancellationToken cancellationToken = default)
    {
        var compiled = NativeDecimalSchema.Compile(schema, numericLimits, cancellationToken);
        if (compiled.Outcome != NativeOutcome.Valid)
            return NativeResult<NativeDecimalSessionCore>.Failure(compiled.Outcome, compiled.Diagnostic!, compiled.UnsupportedReason);
        var result = SchemaQuerySession.CompileXPath("$value", queryLimits, assessor: new NativeSchemaProvider(compiled.Value!), cancellationToken: cancellationToken);
        return result.Kind == SchemaResultKind.Supported
            ? NativeResult<NativeDecimalSessionCore>.Valid(new NativeDecimalSessionCore(result.Value))
            : NativeResult<NativeDecimalSessionCore>.Failure(NativeOutcome.Unsupported, result.DiagnosticCode ?? "Unsupported native scalar session.");
    }

    internal static NativeResult<NativeDecimalSessionCore> CreateMixed(string schema, NativeSchemaSessionOptions options,
        CancellationToken cancellationToken = default)
    {
        var compiled = NativeDecimalSchema.Compile(schema, options.NumericLimits, cancellationToken, options.MaxStringCharacters);
        if (compiled.Outcome != NativeOutcome.Valid)
            return NativeResult<NativeDecimalSessionCore>.Failure(compiled.Outcome, compiled.Diagnostic!, compiled.UnsupportedReason);
        var result = SchemaQuerySession.CompileXPath("$value", options.QueryLimits, assessor: new NativeSchemaProvider(compiled.Value!), cancellationToken: cancellationToken);
        if (result.Kind != SchemaResultKind.Supported)
            return NativeResult<NativeDecimalSessionCore>.Failure(NativeOutcome.Unsupported, "Unsupported native scalar session.");
        return NativeResult<NativeDecimalSessionCore>.Valid(new NativeDecimalSessionCore(result.Value) { scalarOptions = options });
    }

    internal NativeResult<Document> Assess(string instance, DateTimeOffset currentDateTime, int implicitTimezoneOffsetMinutes,
        CancellationToken cancellationToken = default) => Work(() =>
    {
        var captured = NativeSchemaInput.Capture(instance, cancellationToken);
        if (captured.Outcome != NativeOutcome.Valid) return NativeResult<Document>.Failure(captured.Outcome, captured.Diagnostic!, captured.UnsupportedReason);
        var input = captured.Value!;
        SchemaQuerySnapshot? snapshot = null;
        var transferred = false;
        try
        {
            var assessment = (NativeSchemaProvider.Assessment)session.Advisor!.Assess(
                new(input, SchemaAssessmentMode.Strict, null, null, new Episode(currentDateTime, implicitTimezoneOffsetMinutes), null), cancellationToken);
            if (assessment.Status != SchemaAssessmentStatus.Valid)
                return NativeResult<Document>.Failure(assessment.Status == SchemaAssessmentStatus.Invalid ? NativeOutcome.Invalid : NativeOutcome.Unsupported,
                    assessment.Diagnostics.FirstOrDefault()?.Message ?? "Native assessment refused.", assessment.UnsupportedReason);
            var admitted = session.OpenSnapshot(input, input, assessment.Origin, cancellationToken: cancellationToken);
            if (admitted.Kind != SchemaResultKind.Supported)
                return NativeResult<Document>.Failure(NativeOutcome.Unsupported, admitted.DiagnosticCode ?? "Native snapshot admission refused.");
            snapshot = admitted.Value;
            var document = new Document(this, input, snapshot, assessment);
            documents.Add(document);
            transferred = true;
            return NativeResult<Document>.Valid(document);
        }
        finally
        {
            if (!transferred) { NativeCleanup.Release(snapshot, input); }
        }
    }, cancellationToken);

    private XdmExactDecimal ReadDecimal(Document document, int leafIndex, CancellationToken cancellationToken) =>
        Work(() => ReadValue(document, leafIndex, value => value.ExactDecimalValue, cancellationToken), cancellationToken);

    private NativeScalarValue ReadScalar(Document document, int leafIndex, CancellationToken cancellationToken) =>
        Work(() => ReadValue(document, leafIndex, NativeScalarValue.FromValue, cancellationToken), cancellationToken);

    private T ReadValue<T>(Document document, int leafIndex, Func<XdmValue, T> project, CancellationToken cancellationToken)
    {
        Check(document);
        if (leafIndex < 0 || leafIndex >= document.Assessment.ScalarFacts.Length)
            throw new ArgumentOutOfRangeException(nameof(leafIndex));
        return ReadFact(document, document.Assessment.ScalarFacts[leafIndex], project, cancellationToken);
    }

    private T ReadFact<T>(Document document, NativeSchemaProvider.Facts fact, Func<XdmValue, T> project, CancellationToken cancellationToken)
    {
        return runner.Run([document.Snapshot], operation =>
        {
            var value = fact.ReadTypedValue(cancellationToken);
            if (value.Kind != SchemaResultKind.Supported) throw new NotSupportedException(value.DiagnosticCode);
            var scalar = operation.ReadTypedValue(document.Snapshot, value.Value);
            if (scalar.Kind != SchemaResultKind.Supported) throw new NotSupportedException(scalar.DiagnosticCode);
            return project(operation.Evaluate(new EvaluationContext().WithVariable("value", scalar.Value)));
        }, cancellationToken);
    }

    private bool HasRootScalar(Document document) => Work(() =>
    {
        Check(document);
        return !document.Assessment.ElementFacts[0].HasNoTypedValue && !document.Assessment.ElementFacts[0].NativeType.EmptyContent;
    }, default);

    private NativeScalarValue ReadRootScalar(Document document, CancellationToken cancellationToken) => Work(() =>
    {
        Check(document);
        return ReadFact(document, document.Assessment.ElementFacts[0], NativeScalarValue.FromValue, cancellationToken);
    }, cancellationToken);

    private int AttributeCount(Document document) => Work(() =>
    {
        Check(document);
        return document.Assessment.AttributeFacts.Length;
    }, default);

    private NativeAttributeValue ReadAttribute(Document document, int attributeIndex, CancellationToken cancellationToken) => Work(() =>
    {
        Check(document);
        if (attributeIndex < 0 || attributeIndex >= document.Assessment.AttributeFacts.Length)
            throw new ArgumentOutOfRangeException(nameof(attributeIndex));
        var fact = document.Assessment.AttributeFacts[attributeIndex];
        var value = ReadFact(document, fact, NativeScalarValue.FromValue, cancellationToken);
        return new NativeAttributeValue(fact.NativeDeclaration.NamespaceUri, fact.NativeDeclaration.Name, value, fact.OwnerElementPath);
    }, cancellationToken);

    private int DescendantAttributeCount(Document document) => Work(() =>
    {
        Check(document);
        return document.Assessment.DescendantAttributeFacts.Length;
    }, default);

    private NativeAttributeValue ReadDescendantAttribute(Document document, int attributeIndex, CancellationToken cancellationToken) => Work(() =>
    {
        Check(document);
        if (attributeIndex < 0 || attributeIndex >= document.Assessment.DescendantAttributeFacts.Length)
            throw new ArgumentOutOfRangeException(nameof(attributeIndex));
        var fact = document.Assessment.DescendantAttributeFacts[attributeIndex];
        var value = ReadFact(document, fact, NativeScalarValue.FromValue, cancellationToken);
        return new NativeAttributeValue(fact.NativeDeclaration.NamespaceUri, fact.NativeDeclaration.Name, value, fact.OwnerElementPath);
    }, cancellationToken);

    private string Export(Document document, CancellationToken cancellationToken) => Work(() =>
    {
        Check(document);
        return scalarOptions is null ? document.Input.Root.ToXmlString()
            : NativeBoundedTextWriter.Export(document.Input, scalarOptions.MaxExportCharacters, cancellationToken);
    }, cancellationToken);

    private int LeafCount(Document document) => Work(() =>
    {
        Check(document);
        return document.Assessment.ScalarFacts.Length;
    }, default);

    internal T Work<T>(Func<T> work, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (Interlocked.CompareExchange(ref busy, 1, 0) != 0)
            throw new NotSupportedException("Overlapping facade operations are outside the sequential profile.");
        try
        {
            lock (gate)
            {
                ObjectDisposedException.ThrowIf(closed, this);
                cancellationToken.ThrowIfCancellationRequested();
                return work();
            }
        }
        finally { Volatile.Write(ref busy, 0); }
    }

    private void Check(Document document)
    {
        if (!documents.Contains(document)) throw new ObjectDisposedException(nameof(Document));
    }

    private void Release(Document document)
    {
        lock (gate)
        {
            if (!documents.Remove(document)) return;
            NativeCleanup.Release(document.Snapshot, document.Input);
        }
    }

    /// <summary>Revokes admissions and attempts all owned cleanup after admitted synchronous work.</summary>
    public void Dispose()
    {
        lock (gate)
        {
            if (closed) return;
            closed = true;
            var resources = documents.SelectMany(document => new IDisposable[] { document.Snapshot, document.Input }).Append(session).ToArray();
            documents.Clear();
            NativeCleanup.Release(resources);
        }
    }

    // The facade exposes no callback/raw task, so disposal waits for its private synchronous
    // work via the gate. No external cancellation wait can release its resources early.
    internal sealed class Document : IDisposable
    {
        private readonly NativeDecimalSessionCore owner;
        internal Document(NativeDecimalSessionCore owner, NativeSchemaInput input, SchemaQuerySnapshot snapshot, NativeSchemaProvider.Assessment assessment)
        {
            this.owner = owner; Input = input; Snapshot = snapshot; Assessment = assessment;
        }
        internal NativeSchemaInput Input { get; }
        internal SchemaQuerySnapshot Snapshot { get; }
        internal NativeSchemaProvider.Assessment Assessment { get; }
        internal int DecimalCount => owner.LeafCount(this);
        internal bool HasRootScalar => owner.HasRootScalar(this);
        internal NativeScalarValue ReadRootScalar(CancellationToken cancellationToken = default) => owner.ReadRootScalar(this, cancellationToken);
        internal int AttributeCount => owner.AttributeCount(this);
        internal int DescendantAttributeCount => owner.DescendantAttributeCount(this);
        internal NativeAttributeValue ReadDescendantAttribute(int attributeIndex, CancellationToken cancellationToken = default) => owner.ReadDescendantAttribute(this, attributeIndex, cancellationToken);
        internal NativeAttributeValue ReadAttribute(int attributeIndex, CancellationToken cancellationToken = default) => owner.ReadAttribute(this, attributeIndex, cancellationToken);
        internal NativeScalarValue ReadScalar(int leafIndex, CancellationToken cancellationToken = default) => owner.ReadScalar(this, leafIndex, cancellationToken);
        internal XdmExactDecimal ReadDecimal(int leafIndex, CancellationToken cancellationToken = default) => owner.ReadDecimal(this, leafIndex, cancellationToken);
        internal string ExportXml(CancellationToken cancellationToken = default) => owner.Export(this, cancellationToken);
        /// <summary>Revokes this document and releases its owned admission.</summary>
        public void Dispose() => owner.Release(this);
    }

    private sealed record Episode(DateTimeOffset CurrentDateTime, int ImplicitTimezoneOffsetMinutes) : ISchemaAssessmentEpisode;
}
