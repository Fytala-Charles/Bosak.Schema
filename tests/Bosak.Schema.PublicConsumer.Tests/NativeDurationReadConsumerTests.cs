// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies detached exact duration components and strict getters.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 26-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Numerics;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Verifies exact detached duration reads and qualified lexical exclusions.</summary>
public sealed class NativeDurationReadConsumerTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,256,65536,1,256,16,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Preserves components and collapsed text beyond owner disposal; unrelated getters refuse.</summary>
    /// <param name="kind">Builtin family.</param><param name="data">Original-node facade selector.</param>
    [Theory][InlineData("duration",false)][InlineData("duration",true)][InlineData("yearMonthDuration",false)][InlineData("yearMonthDuration",true)][InlineData("dayTimeDuration",false)][InlineData("dayTimeDuration",true)]
    public void Detached(string kind,bool data)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:"+kind+"'/></xs:schema>";var lexical=kind=="yearMonthDuration"?"-P1Y13M":kind=="dayTimeDuration"?"-PT0.123456789012345678901234567890123S":"-P1Y13MT0.123456789012345678901234567890123S";NativeScalarValue value;string exported;
        if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;using var d=s.Assess("<r> "+lexical+" </r>",Context).Value!;value=d.ReadElement([]);exported=d.ExportXml();}
        else{using var s=NativeSchemaSession.Create(schema,Options).Value!;using var d=s.Assess("<r> "+lexical+" </r>",Context).Value!;value=d.ReadRootScalar();exported=d.ExportXml();}
        Assert.Equal(NativeDecimalOutcome.Unsupported,NativeDecimalSession.Create(schema,new(256,256,1,256,16,256,256)).Outcome);
        Assert.Equal(lexical,value.DurationValue);Assert.Equal(kind=="dayTimeDuration"?BigInteger.Zero:new BigInteger(-25),value.DurationMonths);Assert.Equal(kind=="yearMonthDuration"?"0":"-0.123456789012345678901234567890123",value.DurationSeconds.ToInvariantString(512));Assert.Contains(" "+lexical+" ",exported);
        Assert.Throws<InvalidOperationException>(()=>value.StringValue);Assert.Throws<InvalidOperationException>(()=>value.DecimalValue);Assert.Throws<InvalidOperationException>(()=>value.FloatValue);Assert.Throws<InvalidOperationException>(()=>value.DoubleValue);Assert.Throws<InvalidOperationException>(()=>value.UriValue);Assert.Throws<InvalidOperationException>(()=>value.BooleanValue);Assert.Throws<InvalidOperationException>(()=>value.IntegerValue);Assert.Throws<InvalidOperationException>(()=>value.CalendarValue);Assert.Throws<InvalidOperationException>(()=>value.BinaryValue);Assert.Throws<InvalidOperationException>(()=>value.QNameValue);
    }
    /// <summary>Leaves conflicting REC lexical forms outside this bounded profile without declaring invalidity.</summary>
    /// <param name="kind">Builtin family.</param><param name="text">Ambiguous seconds.</param><param name="data">Facade selector.</param>
    [Theory][InlineData("duration","PT.5S",false)][InlineData("duration","PT.5S",true)][InlineData("duration","PT1.S",false)][InlineData("duration","PT1.S",true)][InlineData("dayTimeDuration","PT.5S",false)][InlineData("dayTimeDuration","PT.5S",true)][InlineData("dayTimeDuration","PT1.S",false)][InlineData("dayTimeDuration","PT1.S",true)]
    public void AmbiguousLexical(string kind,string text,bool data)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:"+kind+"'/></xs:schema>";
        if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;var result=s.Assess("<r>"+text+"</r>",Context);Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);}
        else{using var s=NativeSchemaSession.Create(schema,Options).Value!;var result=s.Assess("<r>"+text+"</r>",Context);Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);}
    }
    /// <summary>Unrelated scalar families cannot expose fabricated duration components.</summary>
    /// <param name="data">Facade selector.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void WrongFamily(bool data)
    {
        const string schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:string'/></xs:schema>";NativeScalarValue value;
        if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;using var d=s.Assess("<r>P1D</r>",Context).Value!;value=d.ReadElement([]);}
        else{using var s=NativeSchemaSession.Create(schema,Options).Value!;using var d=s.Assess("<r>P1D</r>",Context).Value!;value=d.ReadRootScalar();}
        Assert.Throws<InvalidOperationException>(()=>value.DurationMonths);Assert.Throws<InvalidOperationException>(()=>value.DurationSeconds);Assert.Throws<InvalidOperationException>(()=>value.DurationValue);
    }
}
