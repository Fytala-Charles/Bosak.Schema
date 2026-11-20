// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Checks the owned node-data gate, revocation and disposal sequencing.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
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

using System.Reflection;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Checks synchronous admission and disposal of the actual owned facade.</summary>
public sealed class NativeNodeDataLifecycleTests
{
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
    private static NativeSchemaNodeDataSession Open()=>NativeSchemaNodeDataSession.Create(Schema,new(256,256,1024,65536,1,32,1,256,256)).Value!;
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private static NativeNodeDataCore Core(NativeSchemaNodeDataSession session)=>(NativeNodeDataCore)typeof(NativeSchemaNodeDataSession).GetField("core",BindingFlags.Instance|BindingFlags.NonPublic)!.GetValue(session)!;
    /// <summary>Reentrant and overlapping operations are refused, then the facade recovers.</summary>
    [Fact]
    public async Task Gate_RejectsOverlapAndRecovers()
    {
        using var session=Open();using var doc=session.Assess("<r><v>1</v></r>",Context).Value!;var core=Core(session);
        core.Work(()=>{Assert.Throws<NotSupportedException>(()=>doc.ReadElement([0]));return 0;},default);
        using var entered=new ManualResetEventSlim();using var release=new ManualResetEventSlim();
        var admitted=Task.Run(()=>core.Work(()=>{entered.Set();Assert.True(release.Wait(TimeSpan.FromSeconds(20)));return 0;},default));
        try{Assert.True(entered.Wait(TimeSpan.FromSeconds(20)));Assert.Throws<NotSupportedException>(()=>doc.ReadElement([0]));Assert.Throws<NotSupportedException>(()=>session.Assess("<r><v>1</v></r>",Context));}finally{release.Set();}
        await admitted;Assert.Equal("1",doc.ReadElement([0]).DecimalValue.ToInvariantString(256));
    }
    /// <summary>Disposal waits for admitted work and revokes document or session membership.</summary>
    /// <param name="wholeSession">Selects session-wide revocation instead of document revocation.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public async Task Dispose_WaitsAndRevokes(bool wholeSession)
    {
        using var session=Open();var doc=session.Assess("<r><v>1</v></r>",Context).Value!;var core=Core(session);
        using var entered=new ManualResetEventSlim();using var release=new ManualResetEventSlim();using var disposing=new ManualResetEventSlim();
        var admitted=Task.Run(()=>core.Work(()=>{entered.Set();Assert.True(release.Wait(TimeSpan.FromSeconds(20)));return 0;},default));
        Task? disposal=null;
        try{Assert.True(entered.Wait(TimeSpan.FromSeconds(20)));disposal=Task.Run(()=>{disposing.Set();if(wholeSession)session.Dispose();else doc.Dispose();});Assert.True(disposing.Wait(TimeSpan.FromSeconds(20)));Assert.NotSame(disposal, await Task.WhenAny(disposal,Task.Delay(100)));}
        finally{release.Set();}
        await admitted;await disposal!;Assert.Throws<ObjectDisposedException>(()=>doc.ReadElement([0]));
        if(!wholeSession){using var next=session.Assess("<r><v>2</v></r>",Context).Value!;Assert.Equal("2",next.ReadElement([0]).DecimalValue.ToInvariantString(256));}
    }
}
