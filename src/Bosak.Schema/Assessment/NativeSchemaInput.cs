// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 15 November 2026
// PURPOSE              : Owns immutable XML capture and revocable host leases for native schema sessions.
// SPECIAL NOTES        : Internal Core 1.81.2 adapter; finite profile only, no public native factory.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 15-11-2026     | Implement internal native decimal Core adapter                                                 |
//                      | Charles Korthout | 0.2   | 19-11-2026     | Carry bounded Unsupported reasons without changing refusal outcomes                       |
//                      | Charles Korthout | 0.3   | 19-11-2026     | Share explicit mixed scalar profile with preserved decimal compatibility                  |
//                      | Charles Korthout | 0.4   | 19-11-2026     | Keep actual root attribute wrappers in the original guarded capture                      |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Xml.Linq;
using System.Xml;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Providers.Xml;

namespace Bosak.Schema;

/// <summary>Owns immutable XML capture and revocable host leases for native schema sessions.</summary>
internal sealed class NativeSchemaInput : ISchemaAssessmentInput, IQueryOperationLifetime, IDisposable
{
    private readonly object gate = new();
    private readonly Dictionary<XObject, NativeSessionNode> nodes = new(ReferenceEqualityComparer.Instance);
    private readonly ISchemaSourceBinding source = new SourceBinding();
    private readonly ISchemaSnapshotBinding snapshot = new SnapshotBinding();
    private readonly string xml;
    private readonly XDocument document;
    private readonly NativeSessionNode root;
    private Publication? publication;
    private bool closed;
    private int leases;
    private TaskCompletionSource drained = Completed();

    private NativeSchemaInput(string xml, XDocument document)
    {
        this.xml = xml;
        this.document = document;
        root = Wrap(new XDocumentNode(document.Root!));
        ElementNodes = document.Root!.DescendantsAndSelf().Select(element => Wrap(new XDocumentNode(element))).ToImmutableArray();
    }

    internal static NativeResult<NativeSchemaInput> Capture(string xml, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(xml);
        cancellationToken.ThrowIfCancellationRequested();
        try { return NativeResult<NativeSchemaInput>.Valid(new NativeSchemaInput(xml, NativeDecimalSchema.Parse(xml, 65536, cancellationToken))); }
        catch (NativeProfileException ex) { return NativeResult<NativeSchemaInput>.Failure(ex.Outcome, ex.Message, ex.UnsupportedReason); }
    }

    internal void WriteRootTo(XmlWriter writer, CancellationToken cancellationToken)
    {
        Check(cancellationToken);
        document.Root!.WriteTo(writer);
        cancellationToken.ThrowIfCancellationRequested();
    }

    internal string Xml { get { Check(); return xml; } }
    internal ImmutableArray<NativeSessionNode> ElementNodes { get; }
    internal bool IsAssessed => Volatile.Read(ref publication) is not null;
    internal int ActiveLeases { get { lock (gate) return leases; } }
    internal bool ResourcesReleased { get { lock (gate) return closed && leases == 0; } }
    public IXdmNode Root { get { Check(); return root; } }
    public ISchemaSourceBinding Source { get { Check(); return source; } }
    public ISchemaSnapshotBinding Snapshot { get { Check(); return snapshot; } }
    public bool OwnsNode(IXdmNode node)
    {
        lock (gate) return !closed && node is NativeSessionNode issued && ReferenceEquals(issued.Input, this) && nodes.ContainsValue(issued);
    }
    public void ThrowIfUnavailable(CancellationToken cancellationToken) => Check(cancellationToken);
    internal void Check(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ObjectDisposedException.ThrowIf(Volatile.Read(ref closed), this);
    }

    internal NativeSessionNode Wrap(XDocumentNode raw)
    {
        Check();
        lock (gate)
        {
            var member = raw.UnderlyingObject switch
            {
                XDocument candidate => ReferenceEquals(candidate, document),
                XNode candidate => ReferenceEquals(candidate.Document, document),
                XAttribute candidate => ReferenceEquals(candidate.Parent?.Document, document),
                _ => false
            };
            if (!member) throw new NotSupportedException("Node is outside the captured tree.");
            if (nodes.TryGetValue(raw.UnderlyingObject, out var existing)) return existing;
            var node = new NativeSessionNode(this, raw);
            nodes.Add(raw.UnderlyingObject, node);
            return node;
        }
    }

    internal NativeSessionNode RootAttributeNode(XName name)
    {
        Check();
        var attribute = document.Root!.Attribute(name) ?? throw new InvalidOperationException("Assessed root attribute is absent from the capture.");
        return Wrap(new XDocumentNode(attribute));
    }

    internal bool Publish(NativeSchemaProvider.Origin origin, ImmutableArray<NativeSchemaProvider.Facts> facts)
    {
        lock (gate)
        {
            Check();
            if (publication is not null) return false;
            publication = new Publication(origin, facts);
            return true;
        }
    }
    internal bool IsIssued(NativeSchemaProvider.Origin origin) =>
        !Volatile.Read(ref closed) && ReferenceEquals(Volatile.Read(ref publication)?.Origin, origin);
    internal bool OwnsFacts(NativeSchemaProvider.Facts facts) =>
        !Volatile.Read(ref closed) && Volatile.Read(ref publication)?.Facts.Any(issued => ReferenceEquals(issued, facts)) == true;
    internal NativeSchemaProvider.Facts? FactsFor(NativeSessionNode node)
    {
        Check();
        var current = Volatile.Read(ref publication);
        return current?.Facts.FirstOrDefault(facts => ReferenceEquals(facts.Node, node));
    }

    public IQueryOperationLease Enter(CancellationToken cancellationToken)
    {
        lock (gate)
        {
            Check(cancellationToken);
            if (leases++ == 0) drained = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            return new Lease(this);
        }
    }
    private void Leave()
    {
        lock (gate)
        {
            if (--leases == 0) drained.TrySetResult();
        }
    }
    internal Task DrainAsync(CancellationToken cancellationToken = default)
    {
        lock (gate) return drained.Task.WaitAsync(cancellationToken);
    }
    public void Dispose()
    {
        lock (gate) closed = true;
    }
    private static TaskCompletionSource Completed()
    {
        var result = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        result.SetResult();
        return result;
    }

    private sealed class Lease(NativeSchemaInput input) : IQueryOperationLease
    {
        private int disposed;
        public void ThrowIfUnavailable(CancellationToken cancellationToken)
        {
            ObjectDisposedException.ThrowIf(Volatile.Read(ref disposed) != 0, this);
            input.Check(cancellationToken);
        }
        public void Dispose()
        {
            if (Interlocked.Exchange(ref disposed, 1) == 0) input.Leave();
        }
    }
    private sealed class SourceBinding : ISchemaSourceBinding;
    private sealed class SnapshotBinding : ISchemaSnapshotBinding;
    private sealed record Publication(NativeSchemaProvider.Origin Origin, ImmutableArray<NativeSchemaProvider.Facts> Facts);
}
