// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies URI and floating values through actual owned native Core operations.
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
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Checks actual native URI/IEEE transport and detached bit patterns.</summary>
public sealed class NativeNumericUriSmokeConsumerTests
{
    /// <summary>Checks primitive original facts, signed bits and retained detached values.</summary>
    /// <param name="kind">Primitive name.</param><param name="lexical">Original text.</param><param name="bits">Expected floating bits or URI text.</param><param name="data">Original-node selector.</param>
    [Theory]
    [InlineData("float","-0","80000000",false)][InlineData("float","-0","80000000",true)]
    [InlineData("float","16777217","4B800000",false)][InlineData("float","16777217","4B800000",true)]
    [InlineData("double","-0","8000000000000000",false)][InlineData("double","-0","8000000000000000",true)]
    [InlineData("double","9007199254740993","4340000000000000",false)][InlineData("double","9007199254740993","4340000000000000",true)]
    [InlineData("anyURI","  ../a  b%xx  ","../a b%xx",false)][InlineData("anyURI","  ../a  b%xx  ","../a b%xx",true)]
    public void Transport(string kind,string lexical,string bits,bool data)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:"+kind+"'/></xs:schema>";
        var options=new NativeSchemaSessionOptions(256,256,256,65536,1,256,16,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);NativeScalarValue value;
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;var a=s.Assess("<r>"+lexical+"</r>",context);Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);using var d=a.Value!;value=d.ReadElement([]);Assert.Single(d.ReadElementItems([]));}
        else{var c=NativeSchemaSession.Create(schema,options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;var a=s.Assess("<r>"+lexical+"</r>",context);Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);using var d=a.Value!;value=d.ReadRootScalar();Assert.Single(d.ReadRootItems());}
        if(kind=="float"){Assert.Equal(NativeScalarKind.Float,value.Kind);Assert.Equal(bits,unchecked((uint)BitConverter.SingleToInt32Bits(value.FloatValue)).ToString("X8"));Assert.Throws<InvalidOperationException>(()=>value.DoubleValue);}
        else if(kind=="double"){Assert.Equal(NativeScalarKind.Double,value.Kind);Assert.Equal(bits,unchecked((ulong)BitConverter.DoubleToInt64Bits(value.DoubleValue)).ToString("X16"));Assert.Throws<InvalidOperationException>(()=>value.FloatValue);}
        else{Assert.Equal(NativeScalarKind.AnyUri,value.Kind);Assert.Equal(bits,value.UriValue);Assert.Throws<InvalidOperationException>(()=>value.DoubleValue);}
        Assert.Throws<InvalidOperationException>(()=>value.StringValue);
    }
}
