// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Checks disposal barriers for original descendant attributes and detached owner paths.
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
//                      | Charles Korthout | 0.2   | 20-11-2026     | Use dedicated workers for blocking lifecycle barriers to avoid pool scheduling contention |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Checks admitted attribute documents share the established synchronous owner close protocol.</summary>
public sealed class NativeNestedSimpleContentLifecycleTests
{
    /// <summary>Checks close cannot release a document while admitted synchronous work holds the gate.</summary>
    [Theory]
    [InlineData(false)] [InlineData(true)]
    public async Task Close_AdmittedWork_WaitsAndRevokesAfterConsumption(bool closeSession)
    {
        var schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a'><xs:complexType><xs:simpleContent><xs:extension base='xs:decimal'><xs:attribute name='code' type='xs:string'/></xs:extension></xs:simpleContent></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:schema>";
        var xml = "<r><a code='X'>12.50</a></r>";
        using var session = NativeDecimalSessionCore.CreateMixed(schema, new NativeSchemaSessionOptions(256,256,256,65536,1,4,1,256,256)).Value!;
        var doc = session.Assess(xml, DateTimeOffset.UnixEpoch, 0).Value!;
        var detached = doc.ReadDescendantAttribute(0);
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
            Assert.Throws<NotSupportedException>(() => doc.ReadDescendantAttribute(0));
            Assert.Throws<NotSupportedException>(() => doc.DescendantAttributeCount);
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
        Assert.Throws<ObjectDisposedException>(() => doc.ReadDescendantAttribute(0));
        Assert.Equal("X",detached.Value.StringValue);
        Assert.Equal(new[]{0},detached.OwnerElementPath);
        if (!closeSession)
        {
            using var next = session.Assess(xml, DateTimeOffset.UnixEpoch, 0).Value!;
            Assert.Equal(1, next.DecimalCount);
            Assert.False(next.HasRootScalar);
            Assert.Equal(0,next.AttributeCount);
            Assert.Equal(1,next.DescendantAttributeCount);
        }
    }

}
