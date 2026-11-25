// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Owns original-node atomization and returns detached bounded scalar data.
// SPECIAL NOTES        : Internal Core 1.81.2 adapter; finite profile only, no public native factory.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 25-11-2026     | Carry private imported simple-type namespace projection through compilation |
//                      | Charles Korthout | 0.3   | 25-11-2026     | Preserve simple nillability and actual typed-empty values |
//                      | Charles Korthout | 0.4   | 25-11-2026     | Share captured original-node atomization for mixed scalar empty transport |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Xml;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Runtime.Vm;

namespace Bosak.Schema;

/// <summary>Owns original-node atomization and returns detached bounded scalar data.</summary>
internal sealed class NativeNodeDataCore : IDisposable
{
    private readonly object gate = new();
    private readonly SchemaQuerySession session;
    private readonly NativeSessionRunner runner;
    private readonly HashSet<Document> documents = new(ReferenceEqualityComparer.Instance);
    private int busy;
    private bool closed;
    private readonly NativeSchemaSessionOptions options;

    private NativeNodeDataCore(SchemaQuerySession session, NativeSchemaSessionOptions options)
    {
        this.session = session;
        this.options = options;
        runner = new NativeSessionRunner(session);
    }

    internal static NativeResult<NativeNodeDataCore> Create(string schema, NativeSchemaSessionOptions options,
        CancellationToken cancellationToken = default, IReadOnlyDictionary<int,string>? importedTypeNamespaces = null)
    {
        var compiled = NativeDecimalSchema.Compile(schema, options.NumericLimits, cancellationToken, options.MaxStringCharacters, importedTypeNamespaces);
        if (compiled.Outcome != NativeOutcome.Valid)
            return NativeResult<NativeNodeDataCore>.Failure(compiled.Outcome, compiled.Diagnostic!, compiled.UnsupportedReason);
        var query = CreateOriginalDataQuery(compiled.Value!, options, cancellationToken);
        return query.Outcome == NativeOutcome.Valid
            ? NativeResult<NativeNodeDataCore>.Valid(new NativeNodeDataCore(query.Value!, options))
            : NativeResult<NativeNodeDataCore>.Failure(query.Outcome, query.Diagnostic!, query.UnsupportedReason);
    }

    internal static NativeResult<SchemaQuerySession> CreateOriginalDataQuery(NativeDecimalSchema compiled,
        NativeSchemaSessionOptions options, CancellationToken cancellationToken)
    {
        var recorder = new ContextRecorder(new NativeSchemaProvider(compiled));
        var capture = SchemaQuerySession.CompileXPath("data($node)", options.QueryLimits, assessor: recorder, cancellationToken: cancellationToken);
        if (capture.Kind == SchemaResultKind.Supported)
        {
            capture.Value.Dispose();
            return NativeResult<SchemaQuerySession>.Failure(NativeOutcome.Unsupported, "Native context capture refused.");
        }
        if (string.IsNullOrEmpty(recorder.Fingerprint))
            return NativeResult<SchemaQuerySession>.Failure(NativeOutcome.Unsupported, "Native context capture refused.");
        var provider = new NativeSchemaProvider(compiled, true, recorder.Fingerprint);
        var result = SchemaQuerySession.CompileXPath("data($node)", options.QueryLimits, assessor: provider, cancellationToken: cancellationToken);
        return result.Kind == SchemaResultKind.Supported
            ? NativeResult<SchemaQuerySession>.Valid(result.Value)
            : NativeResult<SchemaQuerySession>.Failure(NativeOutcome.Unsupported, "Native node-data session refused.");
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

    private IXdmNode Element(Document document, ImmutableArray<int> path, string parameter)
    {
        Check(document);
        if (path.IsDefault) throw new ArgumentException("An explicit element path is required.", parameter);
        if (path.Length > 3) throw new ArgumentOutOfRangeException(parameter);
        var segments = path.ToArray();
        IXdmNode node = document.Input.Root;
        foreach (var segment in segments)
        {
            if (segment < 0) throw new ArgumentOutOfRangeException(parameter);
            var children = new List<IXdmNode>();
            foreach (var child in node.Children(XdmNodeKind.Element)) children.Add(child.NodeValue);
            if (segment >= children.Count) throw new ArgumentOutOfRangeException(parameter);
            node = children[segment];
        }
        return node;
    }

    private NativeScalarValue Read(Document document, IXdmNode node, CancellationToken cancellationToken)
    {
        if (node is not NativeSessionNode original || document.Input.FactsFor(original) is not { } facts)
            throw new NotSupportedException("The original node has no admitted native facts.");
        if (facts.HasNoTypedValue) throw new NotSupportedException("FOTY0012: The original node has no typed value.");
        return runner.Run([document.Snapshot], operation => NativeScalarValue.FromValue(
            operation.Evaluate(new EvaluationContext().WithVariable("node", XdmValue.FromNode(original)))), cancellationToken);
    }

    private bool IsNilled(Document document, ImmutableArray<int> elementPath, CancellationToken cancellationToken) => Work(() =>
    {
        var node = Element(document, elementPath, nameof(elementPath));
        if (node is not NativeSessionNode original || document.Input.FactsFor(original) is not { } facts)
            throw new NotSupportedException("The original node has no admitted native facts.");
        return facts.IsNilled;
    }, cancellationToken);

    private NativeScalarValue ReadElement(Document document, ImmutableArray<int> elementPath, CancellationToken cancellationToken) => Work(() =>
        Read(document, Element(document, elementPath, nameof(elementPath)), cancellationToken), cancellationToken);

    private NativeScalarValue ReadAttribute(Document document, ImmutableArray<int> ownerElementPath, string namespaceUri,
        string localName, CancellationToken cancellationToken) => Work(() =>
    {
        Check(document);
        ArgumentNullException.ThrowIfNull(namespaceUri);
        ArgumentNullException.ThrowIfNull(localName);
        try { XmlConvert.VerifyNCName(localName); }
        catch (Exception error) when (error is XmlException or ArgumentException) { throw new ArgumentException("An XML NCName is required.", nameof(localName)); }
        var owner = Element(document, ownerElementPath, nameof(ownerElementPath));
        foreach (var attribute in owner.Attributes(localName, namespaceUri))
            return Read(document, attribute.NodeValue, cancellationToken);
        throw new ArgumentException("The requested attribute is absent.", nameof(localName));
    }, cancellationToken);

    private string Export(Document document, CancellationToken cancellationToken) => Work(() =>
    {
        Check(document);
        return NativeBoundedTextWriter.Export(document.Input, options.MaxExportCharacters, cancellationToken);
    }, cancellationToken);

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
        private readonly NativeNodeDataCore owner;
        internal Document(NativeNodeDataCore owner, NativeSchemaInput input, SchemaQuerySnapshot snapshot, NativeSchemaProvider.Assessment assessment)
        {
            this.owner = owner; Input = input; Snapshot = snapshot; Assessment = assessment;
        }
        internal NativeSchemaInput Input { get; }
        internal SchemaQuerySnapshot Snapshot { get; }
        internal NativeSchemaProvider.Assessment Assessment { get; }
        internal bool IsNilled(ImmutableArray<int> elementPath, CancellationToken cancellationToken) => owner.IsNilled(this, elementPath, cancellationToken);
        internal NativeScalarValue ReadElement(ImmutableArray<int> elementPath, CancellationToken cancellationToken) => owner.ReadElement(this, elementPath, cancellationToken);
        internal NativeScalarValue ReadAttribute(ImmutableArray<int> ownerElementPath, string namespaceUri, string localName, CancellationToken cancellationToken) => owner.ReadAttribute(this, ownerElementPath, namespaceUri, localName, cancellationToken);
        internal string ExportXml(CancellationToken cancellationToken) => owner.Export(this, cancellationToken);
        /// <summary>Revokes this document and releases its owned admission.</summary>
        public void Dispose() => owner.Release(this);
    }

    private sealed class ContextRecorder(ISchemaAuthority authority) : ISchemaAssessor
    {
        internal string? Fingerprint { get; private set; }
        /// <inheritdoc/>
        public ISchemaAuthority Authority => authority;
        /// <inheritdoc/>
        public SchemaLanguageVersion SchemaLanguage => SchemaLanguageVersion.Xsd11;
        /// <inheritdoc/>
        public string AssessmentProfile => "context-recorder-never-assesses";
        /// <inheritdoc/>
        public SchemaResult<ISchemaAssessmentAdvisor> Bind(IQuerySemanticEnvironment environment, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Fingerprint = environment.StaticContextFingerprint;
            return NativeCoreResult<ISchemaAssessmentAdvisor>.Unsupported("RECORDER-ONLY");
        }
    }

    private sealed record Episode(DateTimeOffset CurrentDateTime, int ImplicitTimezoneOffsetMinutes) : ISchemaAssessmentEpisode;
}
