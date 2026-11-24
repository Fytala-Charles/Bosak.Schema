// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Verifies isolated simple-root assessment and original scalar provenance.
// SPECIAL NOTES        : Test-only prototype; no production facade or synthetic XML validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 22-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 24-11-2026     | Read original Boolean root values after measured admission |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


using System.Numerics;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Verifies admitted simple roots through public scalar and original-node facades.</summary>
public sealed class NativeSimpleRootConsumerTests
{
    private static NativeSchemaSessionOptions Options => new(256,256,16384,65536,1,32,1,256,256);
    private static NativeSchemaAssessmentContext Context => new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Cases.</summary>
    /// <returns>The explicit payload controls.</returns>
    public static IEnumerable<object[]> Cases()
    {using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData/SimpleRoots/cases.json")));foreach(var row in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})yield return [row.GetProperty("id").GetString()!,row.GetRawText(),data];}
    /// <summary>Planned Payloads.</summary>
    /// <param name="id">The id control input.</param>
    /// <param name="json">The json control input.</param>
    /// <param name="data">The data control input.</param>
    [Theory][MemberData(nameof(Cases))]
    public void PlannedPayloads(string id,string json,bool data)
    {
        using var plan=JsonDocument.Parse(json);var r=plan.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;
        var compile=id is "SR-008" or "SR-009"?"Valid":r.GetProperty("expectedCompile").GetString();var assessment=id is "SR-008" or "SR-009"?"Valid":r.GetProperty("expectedAssessment").GetString();
        var root=XElement.Parse(xml);var family=root.Name.LocalName=="other"?"integer":r.GetProperty("scalarFamily").GetString()!;
        if(data)
        {var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(compile,c.Outcome.ToString());if(c.Value is not {} s){Assert.Null(c.Value);return;}using(s){var a=s.Assess(xml,Context);Assert.Equal(assessment,a.Outcome.ToString());if(a.Value is not {} d){Assert.Null(a.Value);return;}using(d){CheckValue(d.ReadElement([]),family,root.Value);Assert.True(XNode.DeepEquals(root,XElement.Parse(d.ExportXml())));Assert.Throws<ArgumentOutOfRangeException>(()=>d.ReadElement([0]));}}}
        else
        {var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(compile,c.Outcome.ToString());if(c.Value is not {} s){Assert.Null(c.Value);return;}using(s){var a=s.Assess(xml,Context);Assert.Equal(assessment,a.Outcome.ToString());if(a.Value is not {} d){Assert.Null(a.Value);return;}using(d){Assert.True(d.HasRootScalar);Assert.Equal(0,d.ScalarCount);Assert.Equal(0,d.AttributeCount);Assert.Equal(0,d.DescendantAttributeCount);CheckValue(d.ReadRootScalar(),family,root.Value);Assert.Throws<ArgumentOutOfRangeException>(()=>d.ReadScalar(0));Assert.True(XNode.DeepEquals(root,XElement.Parse(d.ExportXml())));}}}
    }
    private static void CheckValue(NativeScalarValue value,string family,string text)
    {
        switch(family)
        {case "string":Assert.Equal(NativeScalarKind.String,value.Kind);Assert.Equal(text,value.StringValue);break;
         case "boolean":Assert.Equal(NativeScalarKind.Boolean,value.Kind);Assert.Equal(text.Trim() is "true" or "1",value.BooleanValue);break;
         case "integer":Assert.Equal(NativeScalarKind.Integer,value.Kind);Assert.Equal(BigInteger.Parse(text.Trim(),System.Globalization.CultureInfo.InvariantCulture),value.IntegerValue);break;
         default:Assert.Equal(NativeScalarKind.Decimal,value.Kind);Assert.Equal(decimal.Parse(text.Trim(),System.Globalization.CultureInfo.InvariantCulture),decimal.Parse(value.DecimalValue.ToInvariantString(1024),System.Globalization.CultureInfo.InvariantCulture));break;}
    }
}
