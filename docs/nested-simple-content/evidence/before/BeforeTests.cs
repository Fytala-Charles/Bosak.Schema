// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Probes nested complex decimal terminals through public Core APIs.
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

using System.Text.Json;
using Bosak.Schema;
using Xunit;

/// <summary>Freezes delivered refusal and existing scalar/root/attribute behavior before nested simple content.</summary>
public sealed class BeforeTests
{
    private static string Text(string name)=>File.ReadAllText(Path.Combine(AppContext.BaseDirectory,name));
    private static NativeSchemaSessionOptions Options()=>new(256,256,1024,65536,1,32,1,256,256);
    private static NativeSchemaAssessmentContext Context()=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>The composed nested price fixture currently refuses completely.</summary>
    [Fact]
    public void NestedPriceFixture_CurrentlyRefuses()
    {
        var r=NativeSchemaSession.Create(Text("proposed.xsd"),Options());Assert.Equal(NativeSchemaOutcome.Unsupported,r.Outcome);Assert.Null(r.Value);
    }
    /// <summary>Both anticipated catalog transitions are actual compile refusals at the delivered base.</summary>
    /// <param name="file">Existing catalog file.</param>
    /// <param name="id">Exact case identity.</param>
    [Theory][InlineData("sequences.json","NS-062")][InlineData("attributes.json","NA-046")]
    public void ExistingNestedSimpleContentCase_RefusesBeforeCode(string file,string id)
    {
        using var doc=JsonDocument.Parse(Text(file));var row=doc.RootElement.EnumerateArray().Single(r=>r.GetProperty("id").GetString()==id);var r=NativeSchemaSession.Create(row.GetProperty("schema").GetString()!,Options());Assert.Equal(NativeSchemaOutcome.Unsupported,r.Outcome);Assert.Null(r.Value);
    }
    /// <summary>The old standalone simple-content root keeps its separate scalar and root attribute reads.</summary>
    [Fact]
    public void StandaloneRoot_RemainsSeparateFromLeafProjection()
    {
        using var s=NativeSchemaSession.Create(Text("price.xsd"),Options()).Value!;using var d=s.Assess("<p:price xmlns:p='urn:fytala:bosak:schema:test' currency='EUR'>12.50</p:price>",Context()).Value!;Assert.True(d.HasRootScalar);Assert.Equal(0,d.ScalarCount);Assert.Equal("12.5",d.ReadRootScalar().DecimalValue.ToInvariantString(256));Assert.Equal(1,d.AttributeCount);Assert.Equal(0,d.DescendantAttributeCount);Assert.Empty(d.ReadAttribute(0).OwnerElementPath);
    }
    /// <summary>Delivered nested attribute and scalar indexes remain intact.</summary>
    [Fact]
    public void ExistingNestedAttributes_RetainIndexesAndOwnerPaths()
    {
        using var s=NativeSchemaSession.Create(Text("delivered.xsd"),Options()).Value!;using var d=s.Assess(Text("delivered.xml"),Context()).Value!;Assert.Equal(7,d.ScalarCount);Assert.Equal(3,d.DescendantAttributeCount);Assert.Equal(1,d.AttributeCount);Assert.Equal(new[]{0},d.ReadDescendantAttribute(0).OwnerElementPath);Assert.Equal("12.5",d.ReadScalar(2).DecimalValue.ToInvariantString(256));
    }
}
