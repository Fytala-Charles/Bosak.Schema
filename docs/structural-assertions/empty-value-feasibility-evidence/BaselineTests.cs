// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Verifies isolated empty typed-value representation through original native assessments.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 21-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 21-11-2026     | Adapt native controls for bounded integer simple content |
//                      | Charles Korthout | 0.3   | 21-11-2026     | Verify isolated contextual assertions and retained native baseline |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.Schema;
using Xunit;
namespace EmptyProbe;
/// <summary>Verifies the unchanged native baseline before empty-value overlays.</summary>
public sealed class BaselineTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,1024,65536,1,32,1,256,256);
    /// <summary>Existing scalar success and prospective empty refusal remain observable.</summary>
    /// <param name="data">Original-node data facade.</param>
    /// <param name="empty">Prospective empty complex type.</param>
    [Theory][InlineData(false,false)][InlineData(true,false)][InlineData(false,true)][InlineData(true,true)]
    public void UnchangedBaseline(bool data,bool empty)
    {
        var body=empty?"":"<xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence>";
        var schema=$"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType>{body}</xs:complexType></xs:element></xs:schema>";
        var expected=empty?NativeSchemaOutcome.Unsupported:NativeSchemaOutcome.Valid;
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(expected,c.Outcome);if(c.Value is {} s){using(s){using var d=s.Assess("<r><v>4</v></r>",new(DateTimeOffset.UnixEpoch,0)).Value!;Assert.Equal("4",d.ReadElement([0]).DecimalValue.Coefficient.ToString());}}}
        else{var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(expected,c.Outcome);if(c.Value is {} s){using(s){using var d=s.Assess("<r><v>4</v></r>",new(DateTimeOffset.UnixEpoch,0)).Value!;Assert.Equal("4",d.ReadScalar(0).DecimalValue.Coefficient.ToString());}}}
    }
}
