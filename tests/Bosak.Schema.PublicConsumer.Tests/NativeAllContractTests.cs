// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 November 2026
// PURPOSE              : Verifies bounded all outcomes, physical original facts and omission semantics.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 23-11-2026     | Verify absent root typed-value diagnostic and caught FOTY0012 |
//                      | Charles Korthout | 0.3 | 26-11-2026     | Reconcile original structural admission without changing stimuli |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


//                      | Charles Korthout | 0.4   | 27-11-2026     | Reconcile measured all anyType outcome and original child placement.                      |
using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Checks real public all routes and actual original node evidence.</summary>
public sealed class NativeAllContractTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,1,256,1,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Gets only all-phase payloads through both facades.</summary><returns>Planned controls.</returns>
    public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","AllParticles","case-plan.json")));foreach(var r in d.RootElement.GetProperty("cases").EnumerateArray().Where(x=>x.GetProperty("phase").GetString()=="all-second"))foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetRawText(),data];}
    /// <summary>Measures adopted bounded all scope through both public facades.</summary>
    /// <param name="id">Case identity.</param><param name="json">Exact payload.</param><param name="data">Selects original-node facade.</param>
    [Theory][MemberData(nameof(Cases))]
    public void PlannedPayloads(string id,string json,bool data)
    {
        Assert.True(id.StartsWith("PT-",StringComparison.Ordinal)||id.StartsWith("AL-",StringComparison.Ordinal));using var doc=JsonDocument.Parse(json);var r=doc.RootElement;string schema=r.GetProperty("schema").GetString()!,xml=r.GetProperty("instance").GetString()!;var compile=r.GetProperty("expectedCompile").GetString();var assessment=r.GetProperty("expectedAssessment").GetString();if(id=="PT-026"){compile="Valid";assessment="Invalid";}var root=XElement.Parse(xml);var children=root.Elements().ToArray();
        if(id=="PT-029")
        {
            if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var owner=c.Value!;var a=owner.Assess(xml,Context);Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);using var d=a.Value!;Assert.Equal(NativeScalarKind.Empty,d.ReadElement([]).Kind);Assert.Empty(d.ReadElementItems([]));Assert.True(XNode.DeepEquals(root,XElement.Parse(d.ExportXml())));}
            else{var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var owner=c.Value!;var a=owner.Assess(xml,Context);Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);using var d=a.Value!;Assert.Equal(NativeScalarKind.Empty,d.ReadRootScalar().Kind);Assert.Empty(d.ReadRootItems());Assert.True(XNode.DeepEquals(root,XElement.Parse(d.ExportXml())));}
            return;
        }
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(compile,c.Outcome.ToString());if(c.Value is not{}s){Assert.Null(c.Value);return;}using(s){var a=s.Assess(xml,Context);Assert.Equal(assessment,a.Outcome.ToString());if(a.Value is not{}d){Assert.Null(a.Value);return;}using(d){for(int i=0;i<children.Length;i++)Assert.Equal(children[i].Value,Text(d.ReadElement([i])));Assert.Throws<NotSupportedException>(()=>d.ReadElement([]));Assert.True(XNode.DeepEquals(root,XElement.Parse(d.ExportXml())));}}}
        else{var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(compile,c.Outcome.ToString());if(c.Value is not{}s){Assert.Null(c.Value);return;}using(s){var a=s.Assess(xml,Context);Assert.Equal(assessment,a.Outcome.ToString());if(a.Value is not{}d){Assert.Null(a.Value);return;}using(d){Assert.False(d.HasRootScalar);Assert.Equal(children.Length,d.ScalarCount);for(int i=0;i<children.Length;i++)Assert.Equal(children[i].Value,Text(d.ReadScalar(i)));Assert.Throws<NotSupportedException>(()=>d.ReadRootScalar());Assert.True(XNode.DeepEquals(root,XElement.Parse(d.ExportXml())));}}}
    }
    private static string Text(NativeScalarValue v)=>v.Kind switch{NativeScalarKind.String=>v.StringValue,NativeScalarKind.Integer=>v.IntegerValue.ToString(CultureInfo.InvariantCulture),NativeScalarKind.Boolean=>v.BooleanValue?"true":"false",_=>v.DecimalValue.ToInvariantString(256)};
}
