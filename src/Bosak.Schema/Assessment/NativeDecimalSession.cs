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
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Runtime.Vm;

namespace Bosak.Schema;

/// <summary>Owns the bounded native scalar workflow and exposes only detached results.</summary>
internal sealed class NativeDecimalSession : IDisposable
{
    private readonly object gate = new();
    private readonly SchemaQuerySession session;
    private readonly NativeSessionRunner runner;
    private readonly HashSet<Document> documents = new(ReferenceEqualityComparer.Instance);
    private int busy;
    private bool closed;

    private NativeDecimalSession(SchemaQuerySession session)
    {
        this.session = session;
        runner = new NativeSessionRunner(session);
    }

    internal static NativeResult<NativeDecimalSession> Create(string schema, NativeNumericLimits numericLimits,
        SchemaQueryLimits queryLimits, CancellationToken cancellationToken = default)
    {
        var compiled = NativeDecimalSchema.Compile(schema, numericLimits, cancellationToken);
        if (compiled.Outcome != NativeOutcome.Valid)
            return NativeResult<NativeDecimalSession>.Failure(compiled.Outcome, compiled.Diagnostic!);
        var result = SchemaQuerySession.CompileXPath("$value", queryLimits, assessor: new NativeSchemaProvider(compiled.Value!), cancellationToken: cancellationToken);
        return result.Kind == SchemaResultKind.Supported
            ? NativeResult<NativeDecimalSession>.Valid(new NativeDecimalSession(result.Value))
            : NativeResult<NativeDecimalSession>.Failure(NativeOutcome.Unsupported, result.DiagnosticCode ?? "Unsupported native scalar session.");
    }

    internal NativeResult<Document> Assess(string instance, DateTimeOffset currentDateTime, int implicitTimezoneOffsetMinutes,
        CancellationToken cancellationToken = default) => Work(() =>
    {
        var captured = NativeSchemaInput.Capture(instance, cancellationToken);
        if (captured.Outcome != NativeOutcome.Valid) return NativeResult<Document>.Failure(captured.Outcome, captured.Diagnostic!);
        var input = captured.Value!;
        SchemaQuerySnapshot? snapshot = null;
        var transferred = false;
        try
        {
            var assessment = (NativeSchemaProvider.Assessment)session.Advisor!.Assess(
                new(input, SchemaAssessmentMode.Strict, null, null, new Episode(currentDateTime, implicitTimezoneOffsetMinutes), null), cancellationToken);
            if (assessment.Status != SchemaAssessmentStatus.Valid)
                return NativeResult<Document>.Failure(assessment.Status == SchemaAssessmentStatus.Invalid ? NativeOutcome.Invalid : NativeOutcome.Unsupported,
                    assessment.Diagnostics.FirstOrDefault()?.Message ?? "Native assessment refused.");
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
            if (!transferred) { snapshot?.Dispose(); input.Dispose(); }
        }
    }, cancellationToken);

    private XdmExactDecimal ReadDecimal(Document document, int leafIndex, CancellationToken cancellationToken) => Work(() =>
    {
        Check(document);
        if (leafIndex < 0 || leafIndex >= document.Assessment.Facts.Length - 1)
            throw new ArgumentOutOfRangeException(nameof(leafIndex));
        return runner.Run([document.Snapshot], operation =>
        {
            var value = document.Assessment.Facts[leafIndex + 1].ReadTypedValue(cancellationToken);
            if (value.Kind != SchemaResultKind.Supported) throw new NotSupportedException(value.DiagnosticCode);
            var scalar = operation.ReadTypedValue(document.Snapshot, value.Value);
            if (scalar.Kind != SchemaResultKind.Supported) throw new NotSupportedException(scalar.DiagnosticCode);
            return operation.Evaluate(new EvaluationContext().WithVariable("value", scalar.Value)).ExactDecimalValue;
        }, cancellationToken);
    }, cancellationToken);

    private string Export(Document document, CancellationToken cancellationToken) => Work(() =>
    {
        Check(document);
        return document.Input.Root.ToXmlString();
    }, cancellationToken);

    private int LeafCount(Document document) => Work(() =>
    {
        Check(document);
        return document.Assessment.Facts.Length - 1;
    }, default);

    private T Work<T>(Func<T> work, CancellationToken cancellationToken)
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
            document.Snapshot.Dispose();
            document.Input.Dispose();
        }
    }

    public void Dispose()
    {
        lock (gate)
        {
            if (closed) return;
            closed = true;
            foreach (var document in documents)
            {
                document.Snapshot.Dispose();
                document.Input.Dispose();
            }
            documents.Clear();
            session.Dispose();
        }
    }

    // The facade exposes no callback/raw task, so disposal waits for its private synchronous
    // work via the gate. No external cancellation wait can release its resources early.
    internal sealed class Document : IDisposable
    {
        private readonly NativeDecimalSession owner;
        internal Document(NativeDecimalSession owner, NativeSchemaInput input, SchemaQuerySnapshot snapshot, NativeSchemaProvider.Assessment assessment)
        {
            this.owner = owner; Input = input; Snapshot = snapshot; Assessment = assessment;
        }
        internal NativeSchemaInput Input { get; }
        internal SchemaQuerySnapshot Snapshot { get; }
        internal NativeSchemaProvider.Assessment Assessment { get; }
        internal int DecimalCount => owner.LeafCount(this);
        internal XdmExactDecimal ReadDecimal(int leafIndex, CancellationToken cancellationToken = default) => owner.ReadDecimal(this, leafIndex, cancellationToken);
        internal string ExportXml(CancellationToken cancellationToken = default) => owner.Export(this, cancellationToken);
        public void Dispose() => owner.Release(this);
    }

    private sealed record Episode(DateTimeOffset CurrentDateTime, int ImplicitTimezoneOffsetMinutes) : ISchemaAssessmentEpisode;
}
