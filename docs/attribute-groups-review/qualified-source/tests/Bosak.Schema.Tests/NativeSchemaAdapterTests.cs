// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 15 November 2026
// PURPOSE              : Qualifies native assessment issuance and host lifetimes through real Core sessions.
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
//                      | Charles Korthout | 0.2   | 15-11-2026     | Qualify remaining bounded admission, expiry and failure-cleanup controls                    |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Xml.Linq;
using Bosak.XPath.Api;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Providers.Xml;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Qualifies native assessment issuance and host lifetimes through real Core sessions.</summary>
public sealed class NativeSchemaAdapterTests
{
    private static SchemaQueryLimits Limits => new(1, 4, 1, 256, 256);
    private static readonly Episode Context = new(new DateTimeOffset(2026, 11, 15, 12, 0, 0, TimeSpan.Zero), 60);

    /// <summary>Runs native structural assessment and an actual scalar identity session.</summary>
    [Fact]
    public void AssessAndRun_RealNativeDecimal_PreservesOriginalAuthorityAndValue()
    {
        var provider = Provider();
        using var session = Session(provider);
        using var input = Input();
        var result = Assess(session, input);
        Assert.Equal(SchemaAssessmentStatus.Valid, result.Status);
        Assert.True(result.IsComplete);
        Assert.Same(session.Environment, session.Advisor!.QueryEnvironment);
        var root = result.ValidatedRoot!;
        Assert.True(root.HasNoTypedValue);
        Assert.True(root.IsComplexType);
        Assert.False(root.IsNilled);
        Assert.Null(root.SchemaTypeAnnotation);
        Assert.True(session.Advisor.OwnsNode(root, result.Facts[0], input, result.Origin));
        var scalar = result.Facts[^1].ReadTypedValue(default).Value;
        Assert.True(session.Advisor.OwnsTypedValue(scalar, input, result.Origin));
        using var snapshot = Open(session, input, result);
        var detached = new NativeSessionRunner(session).Run([snapshot], operation =>
        {
            var read = operation.ReadTypedValue(snapshot, scalar);
            Assert.Equal(SchemaResultKind.Supported, read.Kind);
            var evaluated = operation.Evaluate(new EvaluationContext().WithVariable("value", read.Value));
            Assert.True(SchemaAtomics.TryGetProvenance(evaluated, out var provenance));
            Assert.Same(result.Origin, provenance!.OriginalAssessment);
            Assert.Same(provider.DecimalType, provenance.DeclaredType);
            Assert.Same(provider.DecimalType, provenance.ActualType);
            return evaluated.ExactDecimalValue.ToInvariantString(256);
        });
        Assert.Equal("1", detached);
        var export = XDocument.Parse(root.ToXmlString());
        export.Root!.RemoveAll();
        var children = 0;
        foreach (var child in root.Children(XdmNodeKind.Element)) { Assert.True(child.IsNode); children++; }
        Assert.Equal(3, children);
    }

    /// <summary>Checks engine atomization receives the original element-only root state.</summary>
    [Fact]
    public void Atomize_ElementOnlyRoot_ThrowsFoty0012()
    {
        using var session = Session(Provider());
        using var input = Input();
        var result = Assess(session, input);
        using var snapshot = Open(session, input, result);
        session.Run([snapshot], operation =>
        {
            var error = Assert.Throws<InvalidOperationException>(() =>
                XPath31Expression.Compile("data(.)").Evaluate(operation.GetRoot(snapshot)).GetEffectiveBooleanValue());
            Assert.StartsWith("FOTY0012:", error.Message, StringComparison.Ordinal);
            Assert.Equal("FOTY0012", Assert.Throws<XPathErrorException>(() => operation.GetRoot(snapshot).TypedValue).CodeLocalName);
            return 0;
        });
        Assert.Equal("FOTY0012", result.Facts[0].ReadTypedValue(default).DiagnosticCode);
    }

    /// <summary>Checks invalid and unsupported outcomes never expose validated roots.</summary>
    /// <param name="xml">The captured instance.</param>
    /// <param name="expected">The expected Core assessment status.</param>
    [Theory]
    [InlineData("<order><tax>1</tax></order>", SchemaAssessmentStatus.Invalid)]
    [InlineData("<order><amount>1</amount><amount>2</amount><tax>-1</tax></order>", SchemaAssessmentStatus.Invalid)]
    [InlineData("<order flag='x'><amount>1</amount><amount>2</amount><tax>1</tax></order>", SchemaAssessmentStatus.Unsupported)]
    public void Assess_NonvalidNativeInput_PublishesNoRoot(string xml, SchemaAssessmentStatus expected)
    {
        using var session = Session(Provider());
        using var input = Input(xml);
        var result = Assess(session, input);
        Assert.Equal(expected, result.Status);
        Assert.Null(result.ValidatedRoot);
        Assert.Null(result.Evidence);
        Assert.Single(result.Diagnostics);
        Assert.False(input.IsAssessed);
    }

    /// <summary>Checks component handles authenticate actual membership rather than a copied authority.</summary>
    [Fact]
    public void Authority_OwnedAndForeignComponents_AnswersOrRefusesExplicitly()
    {
        var provider = Provider();
        var foreign = Provider();
        Assert.Same(provider.DecimalType, provider.ResolveType(new("http://www.w3.org/2001/XMLSchema", "decimal"), default).Value);
        Assert.Same(provider.RootDeclaration, provider.ResolveElementDeclaration(new("", "order"), default).Value);
        Assert.Equal(SchemaResultKind.NotFound, provider.ResolveElementDeclaration(new("", "tax"), default).Kind);
        Assert.True(provider.IsSameComponent(provider.DecimalType, provider.DecimalType, default).Value);
        Assert.False(provider.IsSameComponent(provider.DecimalType, provider.RootDeclaration, default).Value);
        Assert.False(provider.Owns(foreign.DecimalType));
        Assert.False(provider.Owns(new CopiedComponent(provider)));
        Assert.Throws<ArgumentException>(() => provider.IsSameComponent(provider.DecimalType, foreign.DecimalType, default));
        Assert.Equal(SchemaResultKind.Unsupported, provider.IsDerivedFrom(provider.DecimalType, provider.DecimalType, default).Kind);
        Assert.Equal(SchemaResultKind.Unsupported, provider.IsAffiliatedWith(provider.RootDeclaration, provider.RootDeclaration, default).Kind);
        Assert.Equal(SchemaResultKind.Unsupported, provider.GetComponentClosure(provider.RootDeclaration, default).Kind);
        Assert.Equal(SchemaResultKind.Unsupported, provider.ResolveType(new("", "anonymous"), default).Kind);
    }

    /// <summary>Checks advisor issuance is bound to exact facts, node, input and assessment origin.</summary>
    [Fact]
    public void Advisor_CopiedOrCrossBoundEvidence_RefusesWithoutForeignGetters()
    {
        var provider = Provider();
        using var session = Session(provider);
        using var otherSession = Session(provider);
        using var input = Input();
        using var other = Input();
        var result = Assess(session, input);
        var facts = result.Facts[^1];
        var value = facts.ReadTypedValue(default).Value;
        Assert.False(otherSession.Advisor!.OwnsAssessment(result.Origin));
        Assert.False(otherSession.Advisor.OwnsNodeFacts(facts));
        Assert.False(session.Advisor!.OwnsNode(input.Root, facts, input, result.Origin));
        Assert.False(input.OwnsNode(new NativeSessionNode(input, new XDocumentNode(new XElement("forged")))));
        Assert.False(session.Advisor.OwnsTypedValue(value, other, result.Origin));
        Assert.False(session.Advisor.OwnsTypedValue(value, input, null));
        Assert.False(session.Advisor.OwnsNodeFacts(new CopiedFacts()));
        var forged = new NativeSchemaProvider.Facts(input, input.ElementNodes[^1],
            (NativeSchemaProvider.Origin)result.Origin, provider.DecimalType, provider.RootDeclaration,
            new NativeElementState(ProviderSchema().Children[0], null));
        Assert.False(session.Advisor.OwnsNodeFacts(forged));
        Assert.False(session.Advisor.OwnsNode(input.ElementNodes[^1], forged, input, result.Origin));
        using var snapshot = Open(session, input, result);
        session.Run([snapshot], operation =>
        {
            Assert.Throws<ArgumentException>(() => operation.ReadTypedValue(snapshot, new CopiedValue()));
            return 0;
        });
    }

    /// <summary>Checks unsupported captured environments and foreign inputs are refused before getters.</summary>
    [Fact]
    public void BindAndAssess_ForeignObjects_RefuseBeforeGetters()
    {
        var provider = Provider();
        Assert.Equal(SchemaResultKind.Unsupported, provider.Bind(new ForeignEnvironment(), default).Kind);
        using var session = Session(provider);
        var result = session.Advisor!.Assess(new(new ForeignInput(), SchemaAssessmentMode.Strict, null, null, Context, null), default);
        Assert.Equal(SchemaAssessmentStatus.Unsupported, result.Status);
        Assert.Null(result.ValidatedRoot);
        Assert.Equal(SchemaResultKind.Unsupported, SchemaQuerySession.CompileXPath("$value", Limits,
            new CompileOptions { Namespaces = new Dictionary<string, string> { ["custom"] = "urn:custom" } }, provider).Kind);
    }

    /// <summary>Checks original evidence cannot be rewritten by reassessment or a new advisor.</summary>
    [Fact]
    public void Assess_AlreadyAssessedInput_RefusesAndPreservesOriginalFacts()
    {
        var provider = Provider();
        using var session = Session(provider);
        using var other = Session(provider);
        using var input = Input();
        var original = Assess(session, input);
        Assert.Equal(SchemaAssessmentStatus.Unsupported, Assess(session, input).Status);
        Assert.Equal(SchemaAssessmentStatus.Unsupported, Assess(other, input).Status);
        Assert.True(session.Advisor!.OwnsAssessment(original.Origin));
        Assert.False(other.Advisor!.OwnsAssessment(original.Origin));
        Assert.Same(original.Facts[0].Declaration, provider.RootDeclaration);
    }

    /// <summary>Checks unimplemented compatibility returns no handoff even for equal schema text.</summary>
    [Fact]
    public void OpenCompatibleSnapshot_IndependentEqualSchemas_ReturnsUnsupportedWithoutView()
    {
        using var source = Session(Provider());
        using var target = Session(Provider());
        using var input = Input();
        var result = Assess(source, input);
        using var snapshot = Open(source, input, result);
        var compatible = target.OpenCompatibleSnapshot(snapshot);
        Assert.Equal(SchemaCompatibilityDecision.Unsupported, compatible.Decision);
        Assert.Null(compatible.Snapshot);
        Assert.True(source.Advisor!.OwnsAssessment(result.Origin));
    }

    /// <summary>Checks deferred values and nodes refuse access when the host owner closes.</summary>
    [Fact]
    public void Dispose_InputOwner_RevokesBorrowedNodesFactsAndDeferredValues()
    {
        using var session = Session(Provider());
        var input = Input();
        var result = Assess(session, input);
        var facts = result.Facts[^1];
        var typed = facts.ReadTypedValue(default).Value;
        var cursor = typed.AtomicItems.GetEnumerator();
        var root = result.ValidatedRoot!;
        var nodes = root.Children(XdmNodeKind.Element).GetEnumerator();
        input.Dispose();
        Assert.False(session.Advisor!.OwnsAssessment(result.Origin));
        Assert.False(session.Advisor.OwnsNodeFacts(facts));
        Assert.Throws<ObjectDisposedException>(() => root.StringValue);
        Assert.Throws<ObjectDisposedException>(() => facts.Type);
        Assert.Throws<ObjectDisposedException>(() => cursor.MoveNext());
        Assert.Throws<ObjectDisposedException>(() => nodes.MoveNext());
        Assert.True(input.ResourcesReleased);
    }

    /// <summary>Checks drain cancellation does not release a resource retained by raw asynchronous work.</summary>
    /// <returns>The completion of the deterministic lifetime control.</returns>
    [Fact]
    public async Task RunAsync_CanceledDrainWait_RetainsHostResourcesUntilRawCompletion()
    {
        using var session = Session(Provider());
        using var input = Input();
        var result = Assess(session, input);
        var snapshot = Open(session, input, result);
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var raw = session.RunAsync([snapshot], async _ =>
        {
            started.SetResult();
            await release.Task;
            return 7;
        });
        await started.Task;
        session.Dispose();
        snapshot.Dispose();
        input.Dispose();
        Assert.False(input.ResourcesReleased);
        using var cancelled = new CancellationTokenSource();
        cancelled.Cancel();
        try
        {
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => session.DrainAsync(cancelled.Token));
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => input.DrainAsync(cancelled.Token));
            Assert.False(input.ResourcesReleased);
        }
        finally { release.SetResult(); }
        Assert.Equal(7, await raw);
        await session.DrainAsync();
        await input.DrainAsync();
        Assert.True(input.ResourcesReleased);
        Assert.Equal(0, input.ActiveLeases);
    }

    /// <summary>Checks unsupported modes, diagnostic budgets and pre-canceled native assessment.</summary>
    [Fact]
    public void Assess_ModesBudgetsAndCancellation_RefusesOrThrowsWithoutPartialSuccess()
    {
        var provider = Provider();
        using var session = Session(provider);
        using var input = Input();
        var request = new SchemaAssessmentRequest(input, SchemaAssessmentMode.Lax, null, null, Context, new(1, 1));
        var refused = session.Advisor!.Assess(request, default);
        Assert.Equal(SchemaAssessmentStatus.Unsupported, refused.Status);
        Assert.True(refused.DiagnosticsTruncated);
        Assert.Empty(refused.Diagnostics);
        Assert.Null(refused.ValidatedRoot);
        Assert.Throws<ArgumentException>(() => session.Advisor.Assess(request with { Limits = new(0, 1) }, default));
        using var cancelled = new CancellationTokenSource();
        cancelled.Cancel();
        Assert.Throws<OperationCanceledException>(() => session.Advisor.Assess(request, cancelled.Token));
        Assert.False(input.IsAssessed);
        Assert.Equal(0, input.ActiveLeases);
        Assert.Equal(SchemaResultKind.Unsupported, session.Advisor.ConstructSimpleValue(XdmValue.FromInteger(1), provider.DecimalType, Context, default).Kind);
    }

    private static NativeDecimalSchema ProviderSchema() => NativeDecimalSchema.Compile(
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "TestData", "Schemas", "native-decimal.xsd")), new(256, 256)).Value!;

    /// <summary>Checks complete container adapters expire with raw work while copied payloads survive.</summary>
    [Fact]
    public void ReadTypedContainer_RawOperationEnds_ExpiresBorrowedAdapterOnly()
    {
        using var session = Session(Provider());
        using var input = Input();
        var assessment = Assess(session, input);
        using var snapshot = Open(session, input, assessment);
        var result = session.Run([snapshot], operation =>
        {
            var read = operation.ReadTypedContainer(snapshot, assessment.Facts[^1].ReadTypedValue(default).Value);
            Assert.Equal(SchemaResultKind.Supported, read.Kind);
            Assert.False(read.Value.IsList);
            var item = Assert.Single(read.Value.AtomicItems);
            Assert.Same(assessment.Origin, item.TypeEvidence.OriginalAssessment);
            return (Container: read.Value, Plain: item.Value.ExactDecimalValue);
        });
        Assert.Equal("1", result.Plain.ToInvariantString(256));
        Assert.Throws<ObjectDisposedException>(() => result.Container.GoverningType);
        Assert.NotNull(assessment.Facts[^1].Type);
    }

    /// <summary>Checks registered scalar provenance expires on snapshot close without closing its host owner.</summary>
    [Fact]
    public void Dispose_Snapshot_ExpiresScalarReceiptButKeepsProviderOwnerAlive()
    {
        using var session = Session(Provider());
        using var input = Input();
        var assessment = Assess(session, input);
        var snapshot = Open(session, input, assessment);
        var result = session.Run([snapshot], operation =>
        {
            var scalar = operation.ReadTypedValue(snapshot, assessment.Facts[^1].ReadTypedValue(default).Value).Value;
            Assert.True(SchemaAtomics.TryGetProvenance(scalar, out var provenance));
            return (Scalar: scalar, Provenance: provenance!);
        });
        snapshot.Dispose();
        Assert.False(SchemaAtomics.TryGetProvenance(result.Scalar, out _));
        Assert.Throws<ObjectDisposedException>(() => result.Provenance.DeclaredType);
        Assert.True(session.Advisor!.OwnsAssessment(assessment.Origin));
        Assert.Equal(0, input.ActiveLeases);
        Assert.NotEmpty(input.Root.ToXmlString());
    }

    /// <summary>Checks exact supplied scale is admitted at the query ceiling and refused one beyond.</summary>
    [Fact]
    public void ReadTypedValue_ScaleBoundary_AdmitsExactValueAndRefusesExcess()
    {
        using var session = SchemaQuerySession.CompileXPath("$value", new SchemaQueryLimits(1, 4, 1, 10, 2), assessor: Provider()).Value;
        using var exact = Input("<order><amount>1</amount><amount>2</amount><tax>1.23</tax></order>");
        var assessment = Assess(session, exact);
        using (var snapshot = Open(session, exact, assessment))
            Assert.Equal("1.23", session.Run([snapshot], operation => operation.ReadTypedValue(snapshot,
                assessment.Facts[^1].ReadTypedValue(default).Value).Value.ExactDecimalValue.ToInvariantString(256)));
        using var excess = Input("<order><amount>1</amount><amount>2</amount><tax>1.234</tax></order>");
        var other = Assess(session, excess);
        using var beyond = Open(session, excess, other);
        session.Run([beyond], operation =>
        {
            Assert.Throws<NotSupportedException>(() => operation.ReadTypedValue(beyond, other.Facts[^1].ReadTypedValue(default).Value));
            return 0;
        });
    }

    /// <summary>Checks missing membership and foreign/closed snapshots with actual native inputs.</summary>
    [Fact]
    public void Run_ForeignOrClosedNativeAdmission_RejectsBeforeCallback()
    {
        using var session = Session(Provider());
        using var foreign = Session(Provider());
        using var input = Input();
        var assessment = Assess(session, input);
        var snapshot = Open(session, input, assessment);
        var called = false;
        Assert.Throws<ArgumentException>(() => foreign.Run([snapshot], _ => called = true));
        session.Run([], operation =>
        {
            Assert.Throws<ArgumentException>(() => operation.ReadTypedValue(snapshot, assessment.Facts[^1].ReadTypedValue(default).Value));
            return 0;
        });
        snapshot.Dispose();
        Assert.Throws<ObjectDisposedException>(() => session.Run([snapshot], _ => called = true));
        Assert.False(called);
        Assert.Equal(0, input.ActiveLeases);
    }

    /// <summary>Checks a pre-canceled host runner never invokes work and remains usable.</summary>
    [Fact]
    public void Run_PreCanceled_LeavesGateAndAdmissionUsable()
    {
        using var session = Session(Provider());
        using var input = Input();
        var assessment = Assess(session, input);
        using var snapshot = Open(session, input, assessment);
        var runner = new NativeSessionRunner(session);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var called = false;
        Assert.Throws<OperationCanceledException>(() => runner.Run([snapshot], _ => called = true, cancellation.Token));
        Assert.False(called);
        Assert.Equal(6, runner.Run([snapshot], _ => 6));
    }

    /// <summary>Checks asynchronous callback failure releases the shared sequential runner gate.</summary>
    /// <returns>The completion of the asynchronous failure control.</returns>
    [Fact]
    public async Task RunAsync_CallbackFault_ReleasesGateAfterActualCompletion()
    {
        using var session = Session(Provider());
        using var input = Input();
        var assessment = Assess(session, input);
        using var snapshot = Open(session, input, assessment);
        var runner = new NativeSessionRunner(session);
        await Assert.ThrowsAsync<InvalidOperationException>(() => runner.RunAsync<int>([snapshot],
            _ => Task.FromException<int>(new InvalidOperationException("async control"))));
        Assert.Equal(9, await runner.RunAsync([snapshot], _ => Task.FromResult(9)));
        Assert.Equal(1, input.ActiveLeases);
    }

    /// <summary>Checks host leases reject cancellation and dispose exactly once.</summary>
    [Fact]
    public void Enter_LeaseCancellationAndDoubleDispose_KeepsBalancedOwnership()
    {
        using var input = Input();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        Assert.Throws<OperationCanceledException>(() => input.Enter(cancellation.Token));
        Assert.Equal(0, input.ActiveLeases);
        var lease = input.Enter(default);
        Assert.Equal(1, input.ActiveLeases);
        lease.Dispose();
        lease.Dispose();
        Assert.Equal(0, input.ActiveLeases);
        Assert.Throws<ObjectDisposedException>(() => lease.ThrowIfUnavailable(default));
    }

    /// <summary>Checks invalid/default lookup names reject as arguments instead of null dereferences.</summary>
    [Fact]
    public void Resolve_DefaultExpandedName_RejectsInvalidArgument()
    {
        var provider = Provider();
        Assert.Throws<ArgumentException>(() => provider.ResolveElementDeclaration(default, default));
        Assert.Throws<ArgumentException>(() => provider.ResolveType(default, default));
        Assert.Throws<ArgumentException>(() => provider.ResolveAttributeDeclaration(default, default));
    }

    /// <summary>Checks overlapping work on the same admitted input refuses without leaking a lease.</summary>
    /// <returns>The completion of the deterministic concurrency control.</returns>
    [Fact]
    public async Task Run_OverlappingSameInput_RefusesAndKeepsOriginalAdmissionUsable()
    {
        using var session = Session(Provider());
        using var input = Input();
        var assessment = Assess(session, input);
        using var snapshot = Open(session, input, assessment);
        var runner = new NativeSessionRunner(session);
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var raw = runner.RunAsync([snapshot], async _ => { started.SetResult(); await release.Task; return 1; });
        await started.Task;
        using var cancelledWait = new CancellationTokenSource();
        cancelledWait.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => raw.WaitAsync(cancelledWait.Token));
        try { Assert.Throws<NotSupportedException>(() => runner.Run([snapshot], _ => 2)); }
        finally { release.SetResult(); }
        Assert.Equal(1, await raw);
        Assert.Equal(1, input.ActiveLeases);
        Assert.Equal(3, runner.Run([snapshot], _ => 3));
    }

    /// <summary>Checks the engine applies its numeric admission capacity to real native values.</summary>
    [Fact]
    public void ReadTypedValue_RealNativeValueBeyondQueryCapacity_Refuses()
    {
        var provider = Provider();
        using var session = SchemaQuerySession.CompileXPath("$value", new SchemaQueryLimits(1, 4, 1, 3, 2), assessor: provider).Value;
        using var input = Input("<order><amount>1</amount><amount>2</amount><tax>1234</tax></order>");
        var assessment = Assess(session, input);
        Assert.Equal(SchemaAssessmentStatus.Valid, assessment.Status);
        using var snapshot = Open(session, input, assessment);
        session.Run([snapshot], operation =>
        {
            Assert.Throws<NotSupportedException>(() => operation.ReadTypedValue(snapshot, assessment.Facts[^1].ReadTypedValue(default).Value));
            return 0;
        });
        Assert.Equal(1, input.ActiveLeases);
    }

    /// <summary>Checks cancellation during episode capture cannot publish partial assessment.</summary>
    [Fact]
    public void Assess_CancellationDuringEpisodeCapture_ThrowsAndReleasesLease()
    {
        using var session = Session(Provider());
        using var input = Input();
        using var cancellation = new CancellationTokenSource();
        var request = new SchemaAssessmentRequest(input, SchemaAssessmentMode.Strict, null, null, new CancelingEpisode(cancellation), null);
        Assert.Throws<OperationCanceledException>(() => session.Advisor!.Assess(request, cancellation.Token));
        Assert.False(input.IsAssessed);
        Assert.Equal(0, input.ActiveLeases);
    }

    /// <summary>Checks an operation fault releases the sequential host gate.</summary>
    [Fact]
    public void Run_CallbackFault_ReleasesRunnerGateAndKeepsSnapshotUsable()
    {
        using var session = Session(Provider());
        using var input = Input();
        var assessment = Assess(session, input);
        using var snapshot = Open(session, input, assessment);
        var runner = new NativeSessionRunner(session);
        Assert.Throws<InvalidOperationException>(() => runner.Run<int>([snapshot], _ => throw new InvalidOperationException("control")));
        Assert.Equal(4, runner.Run([snapshot], _ => 4));
    }

    /// <summary>Checks actual native scalar-cache and snapshot admission quotas.</summary>
    [Fact]
    public void OpenAndRead_RealNativeQuotaBoundaries_RefuseWithoutLeakingAdmissions()
    {
        var provider = Provider();
        using var session = SchemaQuerySession.CompileXPath("$value", new SchemaQueryLimits(1, 1, 1, 256, 256), assessor: provider).Value;
        using var input = Input();
        using var other = Input();
        var assessment = Assess(session, input);
        var second = Assess(session, other);
        using var snapshot = Open(session, input, assessment);
        Assert.Throws<InvalidOperationException>(() => session.OpenSnapshot(other, other, second.Origin));
        Assert.Equal(0, other.ActiveLeases);
        session.Run([snapshot], operation =>
        {
            Assert.Equal(SchemaResultKind.Supported, operation.ReadTypedValue(snapshot, assessment.Facts[1].ReadTypedValue(default).Value).Kind);
            Assert.Throws<NotSupportedException>(() => operation.ReadTypedValue(snapshot, assessment.Facts[2].ReadTypedValue(default).Value));
            return 0;
        });
    }

    /// <summary>Checks malformed and DTD capture failures precede native assessment.</summary>
    /// <param name="xml">The capture input.</param>
    /// <param name="outcome">The expected refusal kind.</param>
    [Theory]
    [InlineData("<order", (int)NativeOutcome.Invalid)]
    [InlineData("<!DOCTYPE order><order/>", (int)NativeOutcome.Unsupported)]
    public void Capture_InvalidOrUnsupportedXml_PublishesNoInput(string xml, int outcome)
    {
        var result = NativeSchemaInput.Capture(xml);
        Assert.Equal((NativeOutcome)outcome, result.Outcome);
        Assert.Null(result.Value);
    }
    private static NativeSchemaProvider Provider() => new(ProviderSchema());
    private static NativeSchemaInput Input(string? xml = null) => NativeSchemaInput.Capture(xml ??
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "TestData", "Schemas", "native-decimal.xml"))).Value!;
    private static SchemaQuerySession Session(NativeSchemaProvider provider)
    {
        var result = SchemaQuerySession.CompileXPath("$value", Limits, assessor: provider);
        Assert.True(result.Kind == SchemaResultKind.Supported, result.DiagnosticCode);
        return result.Value;
    }
    private static NativeSchemaProvider.Assessment Assess(SchemaQuerySession session, NativeSchemaInput input) =>
        (NativeSchemaProvider.Assessment)session.Advisor!.Assess(new(input, SchemaAssessmentMode.Strict, null, null, Context, null), default);
    private static SchemaQuerySnapshot Open(SchemaQuerySession session, NativeSchemaInput input, NativeSchemaProvider.Assessment assessment)
    {
        var result = session.OpenSnapshot(input, input, assessment.Origin);
        Assert.True(result.Kind == SchemaResultKind.Supported, result.DiagnosticCode);
        return result.Value;
    }
    private sealed record Episode(DateTimeOffset CurrentDateTime, int ImplicitTimezoneOffsetMinutes) : ISchemaAssessmentEpisode;
    private sealed class CancelingEpisode(CancellationTokenSource cancellation) : ISchemaAssessmentEpisode
    {
        public DateTimeOffset CurrentDateTime { get { cancellation.Cancel(); return Context.CurrentDateTime; } }
        public int ImplicitTimezoneOffsetMinutes => Context.ImplicitTimezoneOffsetMinutes;
    }
    private sealed class CopiedComponent(ISchemaAuthority authority) : ISchemaComponentHandle
    {
        public ISchemaAuthority Authority => authority;
    }
    private sealed class CopiedFacts : ISchemaNodeFacts
    {
        public ISchemaComponentHandle Type => throw new InvalidOperationException("Foreign getter.");
        public ISchemaComponentHandle? Declaration => throw new InvalidOperationException("Foreign getter.");
        public ISchemaAssessmentOrigin Origin => throw new InvalidOperationException("Foreign getter.");
        public bool HasNoTypedValue => throw new InvalidOperationException("Foreign getter.");
        public bool IsNilled => throw new InvalidOperationException("Foreign getter.");
        public SchemaResult<ISchemaTypedValue> ReadTypedValue(CancellationToken cancellationToken) => throw new InvalidOperationException("Foreign getter.");
    }
    private sealed class CopiedValue : ISchemaTypedValue
    {
        public ISchemaComponentHandle GoverningType => throw new InvalidOperationException("Foreign getter.");
        public bool IsList => throw new InvalidOperationException("Foreign getter.");
        public IEnumerable<SchemaAtomicItem> AtomicItems => throw new InvalidOperationException("Foreign getter.");
        public bool TryGetSingleAtomic(out SchemaAtomicItem item) => throw new InvalidOperationException("Foreign getter.");
    }
    private sealed class ForeignInput : ISchemaAssessmentInput
    {
        public IXdmNode Root => throw new InvalidOperationException("Foreign getter.");
        public ISchemaSourceBinding Source => throw new InvalidOperationException("Foreign getter.");
        public ISchemaSnapshotBinding Snapshot => throw new InvalidOperationException("Foreign getter.");
        public void ThrowIfUnavailable(CancellationToken cancellationToken) => throw new InvalidOperationException("Foreign getter.");
        public bool OwnsNode(IXdmNode node) => throw new InvalidOperationException("Foreign getter.");
    }
    private sealed class ForeignEnvironment : IQuerySemanticEnvironment
    {
        public QueryHostLanguage HostLanguage => throw new InvalidOperationException("Foreign getter.");
        public string HostVersion => throw new InvalidOperationException("Foreign getter.");
        public Uri XdmSpecificationRevision => throw new InvalidOperationException("Foreign getter.");
        public string BehaviorProfile => throw new InvalidOperationException("Foreign getter.");
        public IQueryStaticContext StaticContext => throw new InvalidOperationException("Foreign getter.");
        public string DescriptorFormatVersion => throw new InvalidOperationException("Foreign getter.");
        public string StaticContextFingerprint => throw new InvalidOperationException("Foreign getter.");
    }
}
