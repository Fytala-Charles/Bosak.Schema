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
/// <summary>Checks actual imported primitive carriers without ambient network resolution.</summary>
public sealed class NativeNumericUriContextConsumerTests
{
    /// <summary>Checks closed named type imports preserve primitive URI/IEEE values.</summary>
    /// <param name="kind">Primitive name.</param><param name="data">Original-node facade selector.</param>
    [Theory][InlineData("float",false)][InlineData("float",true)][InlineData("double",false)][InlineData("double",true)][InlineData("anyURI",false)][InlineData("anyURI",true)]
    public void Imported(string kind,bool data)
    {
        const string root="https://example.test/root.xsd";
        var catalog=new Dictionary<string,string>{[root]="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:f='urn:foreign'><xs:import namespace='urn:foreign' schemaLocation='types.xsd'/><xs:element name='r' type='f:T'/></xs:schema>",["https://example.test/types.xsd"]="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:foreign'><xs:simpleType name='T'><xs:restriction base='xs:"+kind+"'/></xs:simpleType></xs:schema>"};
        var options=new NativeSchemaSessionOptions(256,256,256,65536,1,256,16,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);NativeScalarValue value;
        if(data){var result=NativeSchemaNodeDataSession.Create(root,catalog,options);Assert.Equal(NativeSchemaOutcome.Valid,result.Outcome);using var s=result.Value!;using var d=s.Assess("<r>1</r>",context).Value!;value=d.ReadElement([]);}
        else{var result=NativeSchemaSession.Create(root,catalog,options);Assert.Equal(NativeSchemaOutcome.Valid,result.Outcome);using var s=result.Value!;using var d=s.Assess("<r>1</r>",context).Value!;value=d.ReadRootScalar();}
        Assert.Equal(kind=="float"?NativeScalarKind.Float:kind=="double"?NativeScalarKind.Double:NativeScalarKind.AnyUri,value.Kind);
        if(kind=="float")Assert.Equal(1f,value.FloatValue);else if(kind=="double")Assert.Equal(1d,value.DoubleValue);else Assert.Equal("1",value.UriValue);
    }
}
