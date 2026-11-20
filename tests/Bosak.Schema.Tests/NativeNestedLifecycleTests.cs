// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Checks disposal barriers for actual nested scalar documents.
// SPECIAL NOTES        : Bounded mixed scalar profile; deterministic shared-owner controls only.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Checks admitted attribute documents share the established synchronous owner close protocol.</summary>
public sealed class NativeNestedLifecycleTests
{
    /// <summary>Checks close cannot release a document while admitted synchronous work holds the gate.</summary>
    [Theory]
    [InlineData(false)] [InlineData(true)]
    public async Task Close_AdmittedWork_WaitsAndRevokesAfterConsumption(bool closeSession)
    {
        var schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:schema>";
        var xml = "<r><a><v>12.50</v></a></r>";
        using var session = NativeDecimalSessionCore.CreateMixed(schema, new NativeSchemaSessionOptions(256,256,256,65536,1,4,1,256,256)).Value!;
        var doc = session.Assess(xml, DateTimeOffset.UnixEpoch, 0).Value!;
        var detached = doc.ReadScalar(0);
        using var entered = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();
        using var closing = new ManualResetEventSlim();
        var work = Task.Run(() => session.Work(() =>
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
            Assert.Throws<NotSupportedException>(() => doc.HasRootScalar);
            close = Task.Run(() => { closing.Set(); if (closeSession) session.Dispose(); else doc.Dispose(); });
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
        Assert.Equal("12.5",detached.DecimalValue.ToInvariantString(256));
        if (!closeSession)
        {
            using var next = session.Assess(xml, DateTimeOffset.UnixEpoch, 0).Value!;
            Assert.Equal(1, next.DecimalCount);
            Assert.False(next.HasRootScalar);
            Assert.Equal(0,next.AttributeCount);
        }
    }

}
