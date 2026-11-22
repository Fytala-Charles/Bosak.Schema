// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Verifies isolated bounded pattern matching and native original string assessment.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 22-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.Schema;
using Xunit;
namespace PatternProbe;
/// <summary>Verifies unchanged product refusal and existing String success.</summary>
public sealed class BaselineTests
{
    /// <summary>Both public facades retain prior bounded behavior before the copied overlays.</summary>
    /// <param name="pattern">Selects prospective pattern schema.</param><param name="data">Selects data facade.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void Unchanged(bool pattern,bool data)
    {var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='S'><xs:restriction base='xs:string'>"+(pattern?"<xs:pattern value='.x'/>":"")+"</xs:restriction></xs:simpleType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='S'/></xs:sequence></xs:complexType></xs:element></xs:schema>";var options=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);var expected=pattern?NativeSchemaOutcome.Unsupported:NativeSchemaOutcome.Valid;if(data){var c=NativeSchemaNodeDataSession.Create(schema,options);Assert.Equal(expected,c.Outcome);using var session=c.Value;if(session is not null){using var d=session.Assess("<r><v>bx</v></r>",new(DateTimeOffset.UnixEpoch,0)).Value!;Assert.Equal("bx",d.ReadElement([0]).StringValue);}}else{var c=NativeSchemaSession.Create(schema,options);Assert.Equal(expected,c.Outcome);using var session=c.Value;if(session is not null){using var d=session.Assess("<r><v>bx</v></r>",new(DateTimeOffset.UnixEpoch,0)).Value!;Assert.Equal("bx",d.ReadScalar(0).StringValue);}}}
}
