// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 15 November 2026
// PURPOSE              : Serializes native host work through complete synchronous and asynchronous consumption.
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
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using Bosak.XPath.Api.Schema;

namespace Bosak.Schema;

/// <summary>Serializes native host work through complete synchronous and asynchronous consumption.</summary>
internal sealed class NativeSessionRunner(SchemaQuerySession session)
{
    private int busy;

    internal T Run<T>(ImmutableArray<SchemaQuerySnapshot> snapshots, Func<SchemaQueryOperation, T> work,
        CancellationToken cancellationToken = default)
    {
        Enter(cancellationToken);
        try { return session.Run(snapshots, work, cancellationToken); }
        finally { Volatile.Write(ref busy, 0); }
    }

    internal async Task<T> RunAsync<T>(ImmutableArray<SchemaQuerySnapshot> snapshots, Func<SchemaQueryOperation, Task<T>> work,
        CancellationToken cancellationToken = default)
    {
        Enter(cancellationToken);
        try { return await session.RunAsync(snapshots, work, cancellationToken).ConfigureAwait(false); }
        finally { Volatile.Write(ref busy, 0); }
    }

    private void Enter(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (Interlocked.CompareExchange(ref busy, 1, 0) != 0)
            throw new NotSupportedException("Overlapping native session work is outside the sequential host profile.");
    }
}
