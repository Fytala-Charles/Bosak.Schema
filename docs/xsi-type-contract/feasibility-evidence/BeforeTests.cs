// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Checks the unchanged native xsi:type refusal boundary before candidate edits.
// SPECIAL NOTES        : Test-only prototype; no production facade or synthetic XML validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 21-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;

namespace XsiTypeProbe;

/// <summary>Checks the unchanged production source boundary in an isolated build.</summary>
public sealed class BeforeTests
{
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
    /// <summary>No request still produces original native facts and scalar data.</summary>
    [Fact]
    public void NoRequest_IsValid(){using var f=new Fixture(Schema,"<r><v>9</v></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Equal("9",f.Read(f.Element([0])).DecimalValue.ToInvariantString(256));}
    /// <summary>The baseline refuses even same-type requests without publishing facts.</summary>
    [Fact]
    public void SameTypeRequest_IsUnsupported(){using var f=new Fixture(Schema,"<r xmlns:i='http://www.w3.org/2001/XMLSchema-instance' xmlns:xs='http://www.w3.org/2001/XMLSchema'><v i:type='xs:decimal'>9</v></r>");Assert.Equal(SchemaAssessmentStatus.Unsupported,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Null(f.Snapshot);}
    /// <summary>The unchanged decimal-only public facade retains request refusal.</summary>
    [Fact]
    public void LegacyRequest_IsUnsupported(){using var session=NativeDecimalSession.Create(Schema,new(256,256,1,8,1,256,256)).Value!;Assert.Equal(NativeDecimalOutcome.Unsupported,session.Assess("<r xmlns:i='http://www.w3.org/2001/XMLSchema-instance' xmlns:xs='http://www.w3.org/2001/XMLSchema'><v i:type='xs:decimal'>9</v></r>",new(DateTimeOffset.UnixEpoch,0)).Outcome);}
}
