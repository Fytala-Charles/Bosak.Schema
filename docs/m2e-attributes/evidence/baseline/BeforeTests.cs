// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Records current attribute refusal and leaf-only facade behavior before adoption.
// SPECIAL NOTES        : Internal finite native profile; no Core adapter or full XSD 1.1 conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.Schema;
using Xunit;

/// <summary>Records the delivered Schema boundary before implementing the proposed attribute slice.</summary>
public sealed class BeforeAttributeTests
{
    private const string Leaf="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='amount' type='xs:decimal'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
    private static NativeSchemaSessionOptions Options=>new(256,256,256,65536,1,8,1,256,256);
    /// <summary>Both required and optional local root attribute declarations are currently refused.</summary>
    /// <param name="use">Future attribute use.</param>
    [Theory][InlineData("required")][InlineData("optional")]
    public void RootAttributeDeclaration_IsNotCurrentlyAdmitted(string use)
    {
        var schema=Leaf.Replace("</xs:sequence>","</xs:sequence><xs:attribute name='code' type='xs:string' use='"+use+"'/>");var result=NativeSchemaSession.Create(schema,Options);
        Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);Assert.Equal(NativeSchemaUnsupportedReason.DeclarationShape,Assert.Single(result.Diagnostics).UnsupportedReason);
    }
    /// <summary>Existing attribute-free schema behavior remains an explicit unsupported instance result.</summary>
    [Fact]
    public void AttributeFreeSchema_RefusesApplicationAttributes()
    {
        using var s=NativeSchemaSession.Create(Leaf,Options).Value!;var result=s.Assess("<r code='x'><amount>12.5</amount></r>",new(DateTimeOffset.UnixEpoch,0));
        Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);Assert.Equal(NativeSchemaUnsupportedReason.Attributes,Assert.Single(result.Diagnostics).UnsupportedReason);
    }
    /// <summary>Delivered scalar indexing is leaf-only and proposed public attribute members are absent.</summary>
    [Fact]
    public void DeliveredFacade_RemainsLeafOnly()
    {
        using var s=NativeSchemaSession.Create(Leaf,Options).Value!;using var d=s.Assess("<r><amount>12.50</amount></r>",new(DateTimeOffset.UnixEpoch,0)).Value!;
        Assert.Equal(1,d.ScalarCount);Assert.Equal("12.5",d.ReadScalar(0).DecimalValue.ToInvariantString(256));
        Assert.Null(typeof(NativeSchemaDocument).GetProperty("AttributeCount"));Assert.Null(typeof(NativeSchemaDocument).GetMethod("ReadAttribute"));Assert.Null(typeof(NativeSchemaDocument).Assembly.GetType("Bosak.Schema.NativeAttributeValue"));
    }
    /// <summary>The actual future fixture is refused before any attribute validation is claimed.</summary>
    [Fact]
    public void FutureOrderFixture_IsNotCurrentlySupported()
    {
        var result=NativeSchemaSession.Create(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"future-order.xsd")),Options);
        Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);Assert.Equal(NativeSchemaUnsupportedReason.Namespace,Assert.Single(result.Diagnostics).UnsupportedReason);
    }

}
