// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 November 2026
// PURPOSE              : Verifies isolated choice outcomes, physical original facts and omission semantics.
// SPECIAL NOTES        : Test-only prototype; no production facade or synthetic XML validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.Schema;
using Xunit;
namespace Baseline;
/// <summary>Verifies unchanged sequence-only production refusal.</summary>
public sealed class Tests
{
    /// <summary>Checks named/inline choices are still excluded by unchanged production.</summary>
    /// <param name="named">Selects named complex type.</param><param name="data">Selects facade.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void ExistingRefusal(bool named,bool data)
    {var model="<xs:choice><xs:element name='a' type='xs:string'/><xs:element name='b' type='xs:int'/></xs:choice>";var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+(named?"<xs:complexType name='C'>"+model+"</xs:complexType><xs:element name='r' type='C'/>":"<xs:element name='r'><xs:complexType>"+model+"</xs:complexType></xs:element>")+"</xs:schema>";var options=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);if(data){var c=NativeSchemaNodeDataSession.Create(schema,options);Assert.Equal(NativeSchemaOutcome.Unsupported,c.Outcome);Assert.Null(c.Value);}else{var c=NativeSchemaSession.Create(schema,options);Assert.Equal(NativeSchemaOutcome.Unsupported,c.Outcome);Assert.Null(c.Value);}}
}
