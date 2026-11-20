// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Records delivered simple-content refusal and absent root-read members before code.
// SPECIAL NOTES        : Bounded opt-in root attribute result; no transferable schema authority.
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
/// <summary>Records current Schema behavior before decimal simple-content adoption.</summary>
public sealed class BeforeTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,256,65536,1,8,1,256,256);
    /// <summary>Original standalone PriceType and an anonymous equivalent currently refuse compilation.</summary>
    /// <param name="named">Whether to use the exact extracted PriceType.</param>
    [Theory][InlineData(true)][InlineData(false)]
    public void SimpleContent_IsCurrentlyUnsupported(bool named)
    {
        var schema=named?File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"price.xsd")):"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='price'><xs:complexType><xs:simpleContent><xs:extension base='xs:decimal'/></xs:simpleContent></xs:complexType></xs:element></xs:schema>";
        var r=NativeSchemaSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Unsupported,r.Outcome);Assert.Null(r.Value);
    }
    /// <summary>The full existing order fixture remains outside the native grammar.</summary>
    [Fact]
    public void WholeOrderFixture_IsNotCurrentlySupported()=>Assert.Equal(NativeSchemaOutcome.Unsupported,NativeSchemaSession.Create(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"orders.xsd")),Options).Outcome);
    /// <summary>Legacy decimal facade also refuses the proposed simple-content input.</summary>
    [Fact]
    public void OldDecimalFacade_RemainsUnsupported()=>Assert.Equal(NativeDecimalOutcome.Unsupported,NativeDecimalSession.Create(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"price.xsd")),new(256,256,1,8,1,256,256)).Outcome);
    /// <summary>Delivered root-read members are absent while existing leaf and attribute indexes remain usable.</summary>
    [Fact]
    public void ExistingIndexes_AndAbsentRootApi_AreExplicit()
    {
        const string schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='n' type='xs:decimal'/></xs:sequence><xs:attribute name='code' type='xs:string'/></xs:complexType></xs:element></xs:schema>";
        using var s=NativeSchemaSession.Create(schema,Options).Value!;using var d=s.Assess("<r code='x'><n>12.50</n></r>",new(DateTimeOffset.UnixEpoch,0)).Value!;Assert.Equal(1,d.ScalarCount);Assert.Equal(1,d.AttributeCount);Assert.Equal("12.5",d.ReadScalar(0).DecimalValue.ToInvariantString(256));Assert.Equal("x",d.ReadAttribute(0).Value.StringValue);
        Assert.Null(typeof(NativeSchemaDocument).GetProperty("HasRootScalar"));Assert.Null(typeof(NativeSchemaDocument).GetMethod("ReadRootScalar"));
    }
}
