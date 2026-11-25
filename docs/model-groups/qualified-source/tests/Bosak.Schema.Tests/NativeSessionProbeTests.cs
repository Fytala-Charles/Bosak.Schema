// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 15 November 2026
// PURPOSE              : Probes published Core scalar session contracts with an issued test decimal.
// SPECIAL NOTES        : Test-only issuance fixture; no native structural assessment or XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 15-11-2026     | Probe published 1.81.2 typed identity and refusal boundaries                              |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Xml.Linq;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Providers.Xml;
using Bosak.XPath.Runtime.Vm;
using Xunit;
using Xunit.Abstractions;

namespace Bosak.Schema.Tests;

/// <summary>
/// Probes published Core scalar session contracts with an issued test decimal.
/// </summary>
/// <param name="output">The captured test evidence sink.</param>
public sealed class NativeSessionProbeTests(ITestOutputHelper output)
{
    private static SchemaQueryLimits Limits => new(1, 4, 1, 28, 8);

    /// <summary>Checks actual binding, scalar admission, identity evaluation and original provenance.</summary>
    [Fact]
    public void CompileXPath_IssuedDecimalIdentity_PreservesValueAndProvenance()
    {
        var provider = new ProbeProvider();
        using var session = Compile(provider);
        Assert.Same(session.Environment, session.Advisor!.QueryEnvironment);
        output.WriteLine($"Host: {session.Environment.HostLanguage} {session.Environment.HostVersion}");
        output.WriteLine($"XDM: {session.Environment.XdmSpecificationRevision}");
        output.WriteLine($"Behavior: {session.Environment.BehaviorProfile}");
        output.WriteLine($"Static context: {session.Environment.DescriptorFormatVersion} {session.Environment.StaticContextFingerprint}");
        using var snapshot = Open(session, provider);
        var plain = session.Run([snapshot], operation =>
        {
            var read = operation.ReadTypedValue(snapshot, provider.TypedValue);
            Assert.Equal(SchemaResultKind.Supported, read.Kind);
            var result = operation.Evaluate(new EvaluationContext().WithVariable("value", read.Value));
            Assert.Equal(12.50m, result.DecimalValue);
            Assert.True(SchemaAtomics.TryGetProvenance(result, out var provenance));
            Assert.Same(provider.Type, provenance!.DeclaredType);
            Assert.Same(provider.Type, provenance.ActualType);
            Assert.Same(provider.Origin, provenance.OriginalAssessment);
            return result.DecimalValue;
        });
        Assert.Equal(12.50m, plain);
        Assert.Equal(1, provider.ActiveLeases);
        snapshot.Dispose();
        Assert.Equal(0, provider.ActiveLeases);
    }

    /// <summary>Checks that a copied scalar container cannot authenticate itself.</summary>
    [Fact]
    public void ReadTypedValue_UnissuedContainer_RejectsBeforePayloadAccess()
    {
        var provider = new ProbeProvider();
        using var session = Compile(provider);
        using var snapshot = Open(session, provider);
        var forged = new UnissuedValue();
        session.Run([snapshot], operation =>
        {
            Assert.Throws<ArgumentException>(() => operation.ReadTypedValue(snapshot, forged));
            return 0;
        });
        Assert.Equal(0, forged.PayloadReads);
    }

    /// <summary>Checks exact immutable operation membership before typed-value use.</summary>
    [Fact]
    public void ReadTypedValue_SnapshotOutsideRun_Rejects()
    {
        var provider = new ProbeProvider();
        using var session = Compile(provider);
        using var snapshot = Open(session, provider);
        session.Run([], operation =>
        {
            Assert.Throws<ArgumentException>(() => operation.ReadTypedValue(snapshot, provider.TypedValue));
            return 0;
        });
    }

    /// <summary>Checks that revoked admissions cannot be used by new work.</summary>
    [Fact]
    public void Run_ClosedSnapshot_Rejects()
    {
        var provider = new ProbeProvider();
        using var session = Compile(provider);
        var snapshot = Open(session, provider);
        snapshot.Dispose();
        Assert.Throws<ObjectDisposedException>(() => session.Run([snapshot], _ => 0));
        Assert.Equal(0, provider.ActiveLeases);
    }

    /// <summary>Checks that unsupported syntax does not select the scalar profile.</summary>
    [Fact]
    public void CompileXPath_UnsupportedFunction_ReturnsUnsupported()
    {
        var result = SchemaQuerySession.CompileXPath("unknown()", Limits, assessor: new ProbeProvider());
        Assert.Equal(SchemaResultKind.Unsupported, result.Kind);
        Assert.NotEmpty(result.DiagnosticCode!);
    }

    /// <summary>Checks that zero-initialized admission limits are rejected.</summary>
    [Fact]
    public void CompileXPath_DefaultLimits_Rejects()
    {
        Assert.Throws<ArgumentException>(() => SchemaQuerySession.CompileXPath("$value", default));
    }

    /// <summary>Checks that pre-canceled binding throws instead of succeeding partially.</summary>
    [Fact]
    public void CompileXPath_PreCanceled_Throws()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        Assert.Throws<OperationCanceledException>(() => SchemaQuerySession.CompileXPath(
            "$value", Limits, assessor: new ProbeProvider(), cancellationToken: cancellation.Token));
    }

    private static SchemaQuerySession Compile(ProbeProvider provider)
    {
        var result = SchemaQuerySession.CompileXPath("$value", Limits, assessor: provider);
        Assert.Equal(SchemaResultKind.Supported, result.Kind);
        return result.Value;
    }

    private static SchemaQuerySnapshot Open(SchemaQuerySession session, ProbeProvider provider)
    {
        var result = session.OpenSnapshot(provider, provider, provider.Origin);
        Assert.Equal(SchemaResultKind.Supported, result.Kind);
        return result.Value;
    }

    // This fixture issues only a known scalar. Assess deliberately refuses all requests.
    // It proves the released transport route, never structural backend correctness.
    private sealed class ProbeProvider : ISchemaAssessor, ISchemaAuthority, ISchemaAssessmentInput,
        IQueryOperationLifetime
    {
        public ProbeProvider()
        {
            Type = new Component(this);
            Origin = new AssessmentOrigin(Source);
            TypedValue = new DecimalValue(Type, Origin);
        }

        public ISchemaAuthority Authority => this;
        public SchemaLanguageVersion SchemaLanguage => SchemaLanguageVersion.Xsd11;
        public string AssessmentProfile => "test-only-issued-decimal-v1";
        public Component Type { get; }
        public AssessmentOrigin Origin { get; }
        public DecimalValue TypedValue { get; }
        public int ActiveLeases { get; private set; }
        public IXdmNode Root { get; } = new XDocumentNode(new XDocument(new XElement("probe", "12.50")));
        public ISchemaSourceBinding Source { get; } = new SourceBinding();
        public ISchemaSnapshotBinding Snapshot { get; } = new SnapshotBinding();

        public SchemaResult<ISchemaAssessmentAdvisor> Bind(IQuerySemanticEnvironment environment, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return new Result<ISchemaAssessmentAdvisor>(SchemaResultKind.Supported, new Advisor(this, environment));
        }

        public void ThrowIfUnavailable(CancellationToken cancellationToken) => cancellationToken.ThrowIfCancellationRequested();
        public bool OwnsNode(IXdmNode node) => ReferenceEquals(node, Root);
        public bool Owns(ISchemaComponentHandle handle) => ReferenceEquals(handle, Type);
        public SchemaResult<ISchemaComponentHandle> ResolveType(SchemaExpandedName name, CancellationToken cancellationToken) => Unsupported<ISchemaComponentHandle>();
        public SchemaResult<ISchemaComponentHandle> ResolveElementDeclaration(SchemaExpandedName name, CancellationToken cancellationToken) => Unsupported<ISchemaComponentHandle>();
        public SchemaResult<ISchemaComponentHandle> ResolveAttributeDeclaration(SchemaExpandedName name, CancellationToken cancellationToken) => Unsupported<ISchemaComponentHandle>();
        public SchemaResult<bool> IsSameComponent(ISchemaComponentHandle left, ISchemaComponentHandle right, CancellationToken cancellationToken) =>
            new Result<bool>(SchemaResultKind.Supported, Owns(left) && ReferenceEquals(left, right));
        public SchemaResult<bool> IsDerivedFrom(ISchemaComponentHandle derived, ISchemaComponentHandle baseType, CancellationToken cancellationToken) => Unsupported<bool>();
        public SchemaResult<bool> IsAffiliatedWith(ISchemaComponentHandle member, ISchemaComponentHandle head, CancellationToken cancellationToken) => Unsupported<bool>();
        public SchemaResult<ImmutableArray<ISchemaComponentHandle>> GetComponentClosure(ISchemaComponentHandle component, CancellationToken cancellationToken) => Unsupported<ImmutableArray<ISchemaComponentHandle>>();
        public IQueryOperationLease Enter(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ActiveLeases++;
            return new Lease(this);
        }

        private sealed class Lease(ProbeProvider provider) : IQueryOperationLease
        {
            private bool disposed;
            public void ThrowIfUnavailable(CancellationToken cancellationToken)
            {
                ObjectDisposedException.ThrowIf(disposed, this);
                cancellationToken.ThrowIfCancellationRequested();
            }

            public void Dispose()
            {
                if (disposed) return;
                disposed = true;
                provider.ActiveLeases--;
            }
        }
    }

    private sealed class Advisor(ProbeProvider provider, IQuerySemanticEnvironment environment) : ISchemaAssessmentAdvisor
    {
        public ISchemaAssessor Assessor => provider;
        public IQuerySemanticEnvironment QueryEnvironment => environment;
        public bool OwnsAssessment(ISchemaAssessmentOrigin origin) => ReferenceEquals(origin, provider.Origin);
        public bool OwnsNodeFacts(ISchemaNodeFacts facts) => false;
        public bool OwnsNode(IXdmNode node, ISchemaNodeFacts facts, ISchemaAssessmentInput input, ISchemaAssessmentOrigin origin) => false;
        public bool OwnsTypedValue(ISchemaTypedValue value, ISchemaAssessmentInput input, ISchemaAssessmentOrigin? originalAssessment) =>
            ReferenceEquals(value, provider.TypedValue) && ReferenceEquals(input, provider) && ReferenceEquals(originalAssessment, provider.Origin);
        public void ThrowIfUnavailable(CancellationToken cancellationToken) => cancellationToken.ThrowIfCancellationRequested();
        public ISchemaAssessmentResult Assess(SchemaAssessmentRequest request, CancellationToken cancellationToken) => throw new NotSupportedException("No structural assessor in this probe.");
        public SchemaResult<ISchemaTypedValue> ConstructSimpleValue(XdmValue input, ISchemaComponentHandle targetType, ISchemaAssessmentEpisode episode, CancellationToken cancellationToken) => Unsupported<ISchemaTypedValue>();
    }

    private sealed class Component(ISchemaAuthority authority) : ISchemaComponentHandle
    {
        public ISchemaAuthority Authority => authority;
    }

    private sealed class SourceBinding : ISchemaSourceBinding;
    private sealed class SnapshotBinding : ISchemaSnapshotBinding;
    private sealed class AssessmentOrigin(ISchemaSourceBinding source) : ISchemaAssessmentOrigin
    {
        public ISchemaSourceBinding Source => source;
    }

    private sealed class Evidence(Component type, AssessmentOrigin origin) : ISchemaAtomicProvenance
    {
        public ISchemaComponentHandle DeclaredType => type;
        public ISchemaComponentHandle ActualType => type;
        public ISchemaAssessmentOrigin OriginalAssessment => origin;
    }

    private sealed class DecimalValue(Component type, AssessmentOrigin origin) : ISchemaTypedValue
    {
        private readonly SchemaAtomicItem item = new(XdmValue.FromDecimal(12.50m), new Evidence(type, origin));
        public ISchemaComponentHandle GoverningType => type;
        public bool IsList => false;
        public IEnumerable<SchemaAtomicItem> AtomicItems => [item];
        public bool TryGetSingleAtomic(out SchemaAtomicItem value)
        {
            value = item;
            return true;
        }
    }

    private sealed class UnissuedValue : ISchemaTypedValue
    {
        public int PayloadReads { get; private set; }
        public ISchemaComponentHandle GoverningType => throw new InvalidOperationException("Unissued metadata read.");
        public bool IsList => throw new InvalidOperationException("Unissued shape read.");
        public IEnumerable<SchemaAtomicItem> AtomicItems => throw new InvalidOperationException("Unissued items read.");
        public bool TryGetSingleAtomic(out SchemaAtomicItem value)
        {
            PayloadReads++;
            throw new InvalidOperationException("Unissued payload read.");
        }
    }

    private sealed class Result<T>(SchemaResultKind kind, T value, string? diagnosticCode = null) : SchemaResult<T>
    {
        public override SchemaResultKind Kind => kind;
        public override T Value => kind == SchemaResultKind.Supported ? value : throw new InvalidOperationException("Unsupported has no value.");
        public override string? DiagnosticCode => diagnosticCode;
    }
    private static Result<T> Unsupported<T>() => new(SchemaResultKind.Unsupported, default!, "PROBE-UNSUPPORTED");
}
