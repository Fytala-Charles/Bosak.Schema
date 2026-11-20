// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Records native refusal and absent descendant-attribute API before implementation.
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

using Bosak.Schema;
using Xunit;

/// <summary>Freezes the delivered native/API boundary before nested attributes are implemented.</summary>
public sealed class BeforeTests
{
    private static string Fixture(string name)=>File.ReadAllText(Path.Combine(AppContext.BaseDirectory,name));
    private static NativeSchemaSessionOptions Options()=>new(256,256,1024,65536,1,32,1,256,256);
    private static NativeSchemaAssessmentContext Context()=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>The new attribute-bearing nested schema refuses without admitting a session.</summary>
    [Fact]
    public void ProposedFixture_IsUnsupportedOnDeliveredEngine()
    {
        var result=NativeSchemaSession.Create(Fixture("nested-attributes.xsd"),Options());Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);Assert.Equal("BSK-NATIVE-SCHEMA-UNSUPPORTED",Assert.Single(result.Diagnostics).Code);
    }
    /// <summary>All three proposed additions are absent from the delivered public surface.</summary>
    [Fact]
    public void ProposedMembers_AreAbsentBeforeImplementation()
    {
        Assert.Null(typeof(NativeSchemaDocument).GetProperty("DescendantAttributeCount"));Assert.DoesNotContain(typeof(NativeSchemaDocument).GetMethods(),m=>m.Name=="ReadDescendantAttribute");Assert.Null(typeof(NativeAttributeValue).GetProperty("OwnerElementPath"));
    }
    /// <summary>Root-only reads and terminal indexes remain supported on the original nested fixture.</summary>
    [Fact]
    public void DeliveredNestedFixture_KeepsRootAndTerminalIndexes()
    {
        using var s=NativeSchemaSession.Create(Fixture("delivered.xsd"),Options()).Value!;using var d=s.Assess(Fixture("delivered.xml"),Context()).Value!;
        Assert.Equal(1,d.AttributeCount);Assert.Equal("ORDER-1",d.ReadAttribute(0).Value.StringValue);Assert.Equal(7,d.ScalarCount);Assert.Equal("ABC-0001",d.ReadScalar(0).StringValue);Assert.Equal("12.5",d.ReadScalar(2).DecimalValue.ToInvariantString(256));Assert.False(d.HasRootScalar);
    }
    /// <summary>Descendant application attributes on an attribute-free container still refuse completely.</summary>
    [Fact]
    public void DescendantInstanceAttributes_RefuseWithoutDocument()
    {
        using var s=NativeSchemaSession.Create(Fixture("delivered.xsd"),Options()).Value!;var r=s.Assess(Fixture("nested-attributes.xml"),Context());Assert.Equal(NativeSchemaOutcome.Unsupported,r.Outcome);Assert.Null(r.Value);Assert.Equal(NativeSchemaUnsupportedReason.Attributes,Assert.Single(r.Diagnostics).UnsupportedReason);
    }
}
