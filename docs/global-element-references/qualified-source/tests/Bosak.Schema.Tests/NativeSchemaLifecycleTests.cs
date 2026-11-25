// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Qualifies deterministic facade lifecycle exclusion and all-resource cleanup.
// SPECIAL NOTES        : Bounded mixed scalar profile; deterministic shared-owner controls only.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 20-11-2026     | Use dedicated workers for blocking lifecycle barriers to avoid pool scheduling contention |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.XPath.Api.Schema;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Qualifies deterministic facade lifecycle exclusion and all-resource cleanup.</summary>
public sealed class NativeSchemaLifecycleTests
{
    /// <summary>Checks close cannot release a document while admitted synchronous work holds the gate.</summary>
    [Theory]
    [InlineData(false)] [InlineData(true)]
    public async Task Close_AdmittedWork_WaitsAndRevokesAfterConsumption(bool closeSession)
    {
        var schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='s' type='xs:string'/><xs:element name='n' type='xs:decimal'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
        var xml = "<r><s>value</s><n>12.50</n></r>";
        using var session = NativeDecimalSessionCore.CreateMixed(schema, new NativeSchemaSessionOptions(256,256,256,65536,1,4,1,256,256)).Value!;
        var doc = session.Assess(xml, DateTimeOffset.UnixEpoch, 0).Value!;
        using var entered = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();
        using var closing = new ManualResetEventSlim();
        var work = NativeLifecycleWorker.Start(() => session.Work(() =>
        {
            entered.Set();
            if (!release.Wait(TimeSpan.FromSeconds(10))) throw new TimeoutException("Lifecycle barrier expired.");
            Assert.False(doc.Input.ResourcesReleased);
            return 42;
        }, default));
        Task? close = null;
        try
        {
            Assert.True(entered.Wait(TimeSpan.FromSeconds(10)));
            Assert.Throws<NotSupportedException>(() => doc.ReadScalar(0));
            close = NativeLifecycleWorker.Start(() => { closing.Set(); if (closeSession) session.Dispose(); else doc.Dispose(); });
            Assert.True(closing.Wait(TimeSpan.FromSeconds(10)));
            Assert.False(close.IsCompleted);
            Assert.False(doc.Input.ResourcesReleased);
        }
        finally { release.Set(); }
        Assert.Equal(42, await work.WaitAsync(TimeSpan.FromSeconds(10)));
        await close!.WaitAsync(TimeSpan.FromSeconds(10));
        Assert.True(doc.Input.ResourcesReleased);
        Assert.Equal(0, doc.Input.ActiveLeases);
        Assert.Throws<ObjectDisposedException>(() => doc.ReadScalar(0));
        if (!closeSession)
        {
            using var next = session.Assess(xml, DateTimeOffset.UnixEpoch, 0).Value!;
            Assert.Equal(2, next.DecimalCount);
        }
    }

    /// <summary>Checks reentrant operations refuse and an admitted cancellation/fault releases the gate.</summary>
    [Fact]
    public void Work_ReentryAndCancellationOrFault_ReleaseAdmission()
    {
        var schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='s' type='xs:string'/><xs:element name='n' type='xs:decimal'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
        using var session = NativeDecimalSessionCore.CreateMixed(schema, new NativeSchemaSessionOptions(256,256,256,65536,1,4,1,256,256)).Value!;
        Assert.Equal(7, session.Work(() =>
        {
            Assert.Throws<NotSupportedException>(() => session.Work(() => 0, default));
            return 7;
        }, default));
        using var canceled = new CancellationTokenSource();
        Assert.Throws<OperationCanceledException>(() => session.Work<int>(() =>
        {
            canceled.Cancel(); canceled.Token.ThrowIfCancellationRequested(); return 0;
        }, canceled.Token));
        Assert.Throws<FormatException>(() => session.Work<int>(() => throw new FormatException(), default));
        Assert.Equal(8, session.Work(() => 8, default));
    }

    /// <summary>Checks every cleanup is attempted in ownership order and exception payloads are not exported.</summary>
    [Theory]
    [InlineData(0)] [InlineData(1)] [InlineData(2)]
    public void Cleanup_FaultAtAnyResource_AttemptsAllAndReportsSanitizedFailure(int failing)
    {
        var calls = new List<int>();
        var resources = Enumerable.Range(0, 3).Select(index => new Resource(() =>
        {
            calls.Add(index);
            if (index == failing) throw new IOException("PRIVATE-RAW-EXCEPTION");
        })).ToArray();
        var error = Assert.Throws<InvalidOperationException>(() => NativeCleanup.Release(resources));
        Assert.Equal(new[] { 0, 1, 2 }, calls);
        Assert.Null(error.InnerException);
        Assert.DoesNotContain("PRIVATE-RAW-EXCEPTION", error.ToString());
    }

    private sealed class Resource(Action close) : IDisposable
    {
        /// <summary>Executes the deterministic cleanup action.</summary>
        public void Dispose() => close();
    }
}
