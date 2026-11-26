// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Qualifies scalar facade limits, detached contracts and compatibility boundaries.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 19-11-2026     | Keep refusal probe on excluded integer after Boolean admission                              |
//                      | Charles Korthout | 0.3   | 19-11-2026     | Keep generic refusal probe on excluded date type after integer adoption                   |
//                      | Charles Korthout | 0.4   | 24-11-2026     | Reconcile measured date outcomes in isolated test copy |
//                      | Charles Korthout | 0.5   | 24-11-2026     | Adopt qualified bounded native date transport |
//                      | Charles Korthout | 0.6   | 25-11-2026     | Reconcile measured native calendar outcomes with original stimuli |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.7   | 26-11-2026     | Reconcile measured duration admission; preserve original inputs |
//                      | Charles Korthout | 0.8   | 26-11-2026     | Reconcile measured Gregorian admission with original inputs retained |
// ===========================================================================================================================================================

using Bosak.Schema;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Checks public typed scalar arguments, capacity recovery, ownership and legacy compatibility.</summary>
public sealed class NativeSchemaContractTests
{
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='s' type='xs:string'/><xs:element name='n' type='xs:decimal'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
    private const string Xml="<r><s> kept </s><n>12.50</n></r>";
    private static NativeSchemaSessionOptions Options(int export=65536,int queryDigits=256)=>new(256,256,256,export,1,4,1,queryDigits,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);

    /// <summary>Options reject each invalid capacity before any work or allocation of a session.</summary>
    /// <param name="index">Constructor argument index.</param>
    /// <param name="value">Rejected argument.</param>
    [Theory]
    [InlineData(0,0)][InlineData(0,257)][InlineData(1,-1)][InlineData(1,257)]
    [InlineData(2,0)][InlineData(2,65537)][InlineData(3,0)][InlineData(3,524289)]
    [InlineData(4,0)][InlineData(5,0)][InlineData(6,0)][InlineData(7,0)][InlineData(8,-1)]
    public void InvalidOptions_RefuseBeforeSession(int index,int value)
    {
        int[] a=[256,256,256,65536,1,4,1,256,256];a[index]=value;
        Assert.Throws<ArgumentOutOfRangeException>(()=>new NativeSchemaSessionOptions(a[0],a[1],a[2],a[3],a[4],a[5],a[6],a[7],a[8]));
    }

    /// <summary>Timezone limits are explicit and include both edge offsets.</summary>
    /// <param name="minutes">Timezone minutes.</param>
    /// <param name="valid">Whether the offset is admitted.</param>
    [Theory][InlineData(-841,false)][InlineData(-840,true)][InlineData(840,true)][InlineData(841,false)]
    public void Context_ValidatesExplicitTimezone(int minutes,bool valid)
    {
        if(valid)Assert.Equal(minutes,new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,minutes).ImplicitTimezoneOffsetMinutes);
        else Assert.Throws<ArgumentOutOfRangeException>(()=>new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,minutes));
    }

    /// <summary>The old decimal facade continues refusing strings instead of changing decimal indexing.</summary>
    [Fact]
    public void LegacyFacade_RemainsDecimalOnly()
    {
        var old=NativeDecimalSession.Create(Schema,new(256,256,1,4,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,old.Outcome);Assert.Null(old.Value);
        using var current=NativeSchemaSession.Create(Schema,Options()).Value!;using var doc=current.Assess(Xml,Context).Value!;
        Assert.Equal(2,doc.ScalarCount);Assert.Equal(" kept ",doc.ReadScalar(0).StringValue);Assert.Equal("12.5",doc.ReadScalar(1).DecimalValue.ToInvariantString(256));
    }

    /// <summary>Neither arguments nor closed/cancelled operations become successful input results.</summary>
    [Fact]
    public void Arguments_CancellationAndClosure_AreDistinct()
    {
        Assert.Throws<ArgumentNullException>(()=>NativeSchemaSession.Create(null!,Options()));
        Assert.Throws<ArgumentNullException>(()=>NativeSchemaSession.Create(Schema,null!));
        using var canceled=new CancellationTokenSource();canceled.Cancel();
        Assert.Throws<OperationCanceledException>(()=>NativeSchemaSession.Create(Schema,Options(),canceled.Token));
        using var s=NativeSchemaSession.Create(Schema,Options()).Value!;
        Assert.Throws<ArgumentNullException>(()=>s.Assess(null!,Context));Assert.Throws<ArgumentNullException>(()=>s.Assess(Xml,null!));
        Assert.Throws<OperationCanceledException>(()=>s.Assess(Xml,Context,canceled.Token));
        using var d=s.Assess(Xml,Context).Value!;
        Assert.Throws<ArgumentOutOfRangeException>(()=>d.ReadScalar(-1));Assert.Throws<ArgumentOutOfRangeException>(()=>d.ReadScalar(2));
        Assert.Throws<OperationCanceledException>(()=>d.ReadScalar(0,canceled.Token));Assert.Throws<OperationCanceledException>(()=>d.ExportXml(canceled.Token));
        var detached=d.ReadScalar(0);s.Dispose();Assert.Equal(" kept ",detached.StringValue);
        Assert.Throws<ObjectDisposedException>(()=>d.ReadScalar(0));Assert.Throws<ObjectDisposedException>(()=>d.ExportXml());Assert.Throws<ObjectDisposedException>(()=>s.Assess(Xml,Context));
    }

    /// <summary>Failed admission does not leak quota and disposal frees a subsequent document admission.</summary>
    [Fact]
    public void QuotaAndInvalidAssessment_PreserveAdmissions()
    {
        using var s=NativeSchemaSession.Create(Schema,Options()).Value!;
        Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess(Xml.Replace("12.50","bad"),Context).Outcome);
        using var d=s.Assess(Xml,Context).Value!;Assert.Throws<InvalidOperationException>(()=>s.Assess(Xml,Context));
        d.Dispose();using var next=s.Assess(Xml,Context).Value!;Assert.Equal(2,next.ScalarCount);
    }

    /// <summary>Serialized length is measured exactly and overflow exposes no partial result or closed gate.</summary>
    [Fact]
    public void Export_ExactCapacityAndOverflow_KeepDocumentUsable()
    {
        using var source=NativeSchemaSession.Create(Schema,Options()).Value!;using var original=source.Assess(Xml,Context).Value!;
        var expected=original.ExportXml();
        using var exact=NativeSchemaSession.Create(Schema,Options(expected.Length)).Value!;using var a=exact.Assess(Xml,Context).Value!;
        Assert.Equal(expected,a.ExportXml());
        using var shortSession=NativeSchemaSession.Create(Schema,Options(expected.Length-1)).Value!;using var b=shortSession.Assess(Xml,Context).Value!;
        Assert.Throws<NotSupportedException>(()=>b.ExportXml());Assert.Equal(" kept ",b.ReadScalar(0).StringValue);Assert.Equal(2,b.ScalarCount);
    }

    /// <summary>Query numeric refusal never rounds and leaves strings and the owner gate usable.</summary>
    [Fact]
    public void QueryCapacity_RefusesNumericReadWithoutAffectingStrings()
    {
        using var s=NativeSchemaSession.Create(Schema,Options(queryDigits:2)).Value!;using var d=s.Assess(Xml,Context).Value!;
        Assert.Throws<NotSupportedException>(()=>d.ReadScalar(1));Assert.Equal(" kept ",d.ReadScalar(0).StringValue);
    }

    /// <summary>Producer-only results/scalars cannot be manufactured and diagnostics stay fixed and input-free.</summary>
    [Fact]
    public void SurfaceAndDiagnostics_AreProducerOnlyAndRedacted()
    {
        foreach(var t in new[]{typeof(NativeSchemaSession),typeof(NativeSchemaDocument),typeof(NativeScalarValue),typeof(NativeSchemaDiagnostic),typeof(NativeSchemaResult<NativeSchemaDocument>)})Assert.Empty(t.GetConstructors());
        Assert.Equal(1,(int)NativeScalarKind.String);Assert.Equal(2,(int)NativeScalarKind.Decimal);
        var admitted=NativeSchemaSession.Create(Schema.Replace("type='xs:string'","type='xs:date'"),Options());Assert.Equal(NativeSchemaOutcome.Valid,admitted.Outcome);admitted.Value?.Dispose();
        var calendar=NativeSchemaSession.Create(Schema.Replace("type='xs:string'","type='xs:time'"),Options());Assert.Equal(NativeSchemaOutcome.Valid,calendar.Outcome);calendar.Value!.Dispose();
        var duration=NativeSchemaSession.Create(Schema.Replace("type='xs:string'","type='xs:duration'"),Options());Assert.Equal(NativeSchemaOutcome.Valid,duration.Outcome);
        using(var owner=duration.Value!){using var document=owner.Assess(Xml.Replace(" kept ","P1D",StringComparison.Ordinal),Context).Value!;Assert.Equal("86400",document.ReadScalar(0).DurationSeconds.ToInvariantString(512));}
        var gregorian=NativeSchemaSession.Create(Schema.Replace("type='xs:string'","type='xs:gYear'"),Options());Assert.Equal(NativeSchemaOutcome.Valid,gregorian.Outcome);
        using(var owner=gregorian.Value!){using var document=owner.Assess(Xml.Replace(" kept ","2000",StringComparison.Ordinal),Context).Value!;Assert.Equal(2000L,document.ReadScalar(0).GregorianYear);}
        var result=NativeSchemaSession.Create(Schema.Replace("type='xs:string'","type='xs:ID'"),Options());
        Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);Assert.False(result.DiagnosticsTruncated);
        var diagnostic=Assert.Single(result.Diagnostics);Assert.Equal(NativeSchemaUnsupportedReason.ValueType,diagnostic.UnsupportedReason);
        Assert.Equal("The input or representation is outside the supported native scalar profile.",diagnostic.Message);Assert.True(diagnostic.Message.Length<=160);
        using var s=NativeSchemaSession.Create(Schema,Options()).Value!;
        var refused=s.Assess(Xml.Replace("<r>","<r secret='PRIVATE-MARKER'>"),Context);
        Assert.Null(refused.Value);Assert.DoesNotContain("PRIVATE-MARKER",Assert.Single(refused.Diagnostics).Message);
    }
}
