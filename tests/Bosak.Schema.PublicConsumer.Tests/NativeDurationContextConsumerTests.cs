// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies exact imported duration values without ambient resolution.
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
/// <summary>Checks actual imported primitive carriers without ambient network resolution.</summary>
public sealed class NativeDurationContextConsumerTests
{
    /// <summary>Checks closed named type imports preserve primitive URI/IEEE values.</summary>
    /// <param name="kind">Primitive name.</param><param name="data">Original-node facade selector.</param>
    [Theory][InlineData("duration",false)][InlineData("duration",true)][InlineData("yearMonthDuration",false)][InlineData("yearMonthDuration",true)][InlineData("dayTimeDuration",false)][InlineData("dayTimeDuration",true)]
    public void Imported(string kind,bool data)
    {
        const string root="https://example.test/root.xsd";
        var catalog=new Dictionary<string,string>{[root]="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:f='urn:foreign'><xs:import namespace='urn:foreign' schemaLocation='types.xsd'/><xs:element name='r' type='f:T'/></xs:schema>",["https://example.test/types.xsd"]="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:foreign'><xs:simpleType name='T'><xs:restriction base='xs:"+kind+"'/></xs:simpleType></xs:schema>"};
        var xml=kind=="yearMonthDuration"?"<r>P0M</r>":"<r>PT0S</r>";var options=new NativeSchemaSessionOptions(256,256,256,65536,1,256,16,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);NativeScalarValue value;
        if(data){var result=NativeSchemaNodeDataSession.Create(root,catalog,options);Assert.Equal(NativeSchemaOutcome.Valid,result.Outcome);using var s=result.Value!;using var d=s.Assess(xml,context).Value!;value=d.ReadElement([]);}
        else{var result=NativeSchemaSession.Create(root,catalog,options);Assert.Equal(NativeSchemaOutcome.Valid,result.Outcome);using var s=result.Value!;using var d=s.Assess(xml,context).Value!;value=d.ReadRootScalar();}
        Assert.Equal(kind=="duration"?NativeScalarKind.Duration:kind=="yearMonthDuration"?NativeScalarKind.YearMonthDuration:NativeScalarKind.DayTimeDuration,value.Kind);
        Assert.Equal(System.Numerics.BigInteger.Zero,value.DurationMonths);Assert.Equal("0",value.DurationSeconds.ToInvariantString(512));
    }
    /// <summary>Distinguishes equal local names from two original imported namespaces.</summary>
    /// <param name="data">Facade selector.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void SameLocalImportedNames(bool data)
    {
        const string root="https://example.test/root.xsd";
        var catalog=new Dictionary<string,string>{[root]="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:a='urn:a' xmlns:b='urn:b'><xs:import namespace='urn:a' schemaLocation='a.xsd'/><xs:import namespace='urn:b' schemaLocation='b.xsd'/><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='a:T'/><xs:element name='b' type='b:T'/></xs:sequence></xs:complexType></xs:element></xs:schema>",["https://example.test/a.xsd"]="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:a'><xs:simpleType name='T'><xs:restriction base='xs:yearMonthDuration'/></xs:simpleType></xs:schema>",["https://example.test/b.xsd"]="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:b'><xs:simpleType name='T'><xs:restriction base='xs:dayTimeDuration'/></xs:simpleType></xs:schema>"};
        var options=new NativeSchemaSessionOptions(256,256,256,65536,1,256,16,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,840);NativeScalarValue a,b;
        if(data){using var s=NativeSchemaNodeDataSession.Create(root,catalog,options).Value!;using var d=s.Assess("<r><a>P1M</a><b>PT1M</b></r>",context).Value!;a=d.ReadElement([0]);b=d.ReadElement([1]);}
        else{using var s=NativeSchemaSession.Create(root,catalog,options).Value!;using var d=s.Assess("<r><a>P1M</a><b>PT1M</b></r>",context).Value!;a=d.ReadScalar(0);b=d.ReadScalar(1);}
        Assert.Equal(NativeScalarKind.YearMonthDuration,a.Kind);Assert.Equal(NativeScalarKind.DayTimeDuration,b.Kind);Assert.Equal(System.Numerics.BigInteger.One,a.DurationMonths);Assert.Equal(System.Numerics.BigInteger.Zero,b.DurationMonths);Assert.Equal("60",b.DurationSeconds.ToInvariantString(512));
    }
}
