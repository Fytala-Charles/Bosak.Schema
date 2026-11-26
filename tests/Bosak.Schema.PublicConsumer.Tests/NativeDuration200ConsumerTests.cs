// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies bounded exact duration contracts through both public facades.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 26-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Verifies compound definitions, values and explicit immutable item reads.</summary>
public sealed class NativeDuration200ConsumerTests
{
    /// <summary>Gets all clause-mapped stimuli through both facades and explicit contexts.</summary>
    /// <returns>Stable case identity, payload and context selectors.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var parsed=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","Duration200","cases.json")));
        foreach(var row in parsed.RootElement.GetProperty("cases").EnumerateArray())
            foreach(bool data in new[]{false,true})foreach(int zone in new[]{-840,840})yield return[row.GetProperty("id").GetString()!,row.GetRawText(),data,zone];
    }
    private static string Text(NativeScalarValue value) => value.Kind switch
    {
        NativeScalarKind.Duration or NativeScalarKind.YearMonthDuration or NativeScalarKind.DayTimeDuration => value.Kind+":"+value.DurationMonths.ToString(CultureInfo.InvariantCulture)+":"+value.DurationSeconds.ToInvariantString(512),
        NativeScalarKind.Float => "Float:"+(float.IsNaN(value.FloatValue)?"NaN":unchecked((uint)BitConverter.SingleToInt32Bits(value.FloatValue)).ToString("X8")),
        NativeScalarKind.Double => "Double:"+(double.IsNaN(value.DoubleValue)?"NaN":unchecked((ulong)BitConverter.DoubleToInt64Bits(value.DoubleValue)).ToString("X16")),
        NativeScalarKind.AnyUri => "AnyUri:"+value.UriValue,
        NativeScalarKind.String => "String:"+value.StringValue,
        NativeScalarKind.Integer => "Integer:"+value.IntegerValue.ToString(CultureInfo.InvariantCulture),
        NativeScalarKind.Decimal => "Decimal:"+value.DecimalValue.ToInvariantString(512),
        NativeScalarKind.Boolean => "Boolean:"+(value.BooleanValue?"true":"false"),
        NativeScalarKind.QName => "QName:"+value.QNameValue.NamespaceUri+"|"+value.QNameValue.LocalName,
        NativeScalarKind.HexBinary or NativeScalarKind.Base64Binary => value.Kind+":"+Convert.ToHexString(value.BinaryValue.AsSpan()),
        NativeScalarKind.DateTime or NativeScalarKind.Time or NativeScalarKind.DateTimeStamp => value.Kind+":"+value.CalendarValue.ToInvariantString(512),
        _ => throw new InvalidOperationException("Unexpected test family.")
    };
    /// <summary>Checks compile/assessment results, original XML and detached item payloads.</summary>
    /// <param name="id">Stable authored identity.</param><param name="json">Original stimulus.</param>
    /// <param name="data">Original-node selector.</param><param name="zone">Explicit timezone.</param>
    [Theory][MemberData(nameof(Cases))]
    public void Contract(string id,string json,bool data,int zone)
    {
        using var parsed=JsonDocument.Parse(json);var row=parsed.RootElement;
        int Number(string name,int fallback)=>row.TryGetProperty(name,out var v)?v.GetInt32():fallback;
        string Value(string name,string fallback)=>row.TryGetProperty(name,out var v)?v.GetString()!:fallback;
        var options=new NativeSchemaSessionOptions(Number("digits",256),Number("scale",256),Number("maxCharacters",256),65536,1,256,Number("atoms",16),256,256);
        var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,zone);
        var schema=Value("schema","");var xml=Value("instance","");var compile=Value("compile","Valid");var outcome=Value("outcome","Valid");
        var path=row.TryGetProperty("path",out var pathRow)?pathRow.EnumerateArray().Select(v=>v.GetInt32()).ToImmutableArray():ImmutableArray<int>.Empty;
        var attribute=Value("target","")=="attribute";var list=row.GetProperty("listValue").GetBoolean();
        ImmutableArray<NativeScalarValue> items;string exported;
        if(data)
        {
            var c=NativeSchemaNodeDataSession.Create(schema,options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome+":"+JsonSerializer.Serialize(c.Diagnostics));using var session=c.Value;if(session is null)return;
            var a=session.Assess(xml,context);Assert.True(a.Outcome.ToString()==outcome,id+":assess:"+a.Outcome+":"+JsonSerializer.Serialize(a.Diagnostics));using var document=a.Value;if(document is null)return;
            items=attribute?document.ReadAttributeItems(path,"","a"):document.ReadElementItems(path);
            if(list){if(attribute)Assert.Throws<NotSupportedException>(()=>document.ReadAttribute(path,"","a"));else Assert.Throws<NotSupportedException>(()=>document.ReadElement(path));}
            exported=document.ExportXml();
        }
        else
        {
            var c=NativeSchemaSession.Create(schema,options);Assert.True(c.Outcome.ToString()==compile,id+":compile:"+c.Outcome+":"+JsonSerializer.Serialize(c.Diagnostics));using var session=c.Value;if(session is null)return;
            var a=session.Assess(xml,context);Assert.True(a.Outcome.ToString()==outcome,id+":assess:"+a.Outcome+":"+JsonSerializer.Serialize(a.Diagnostics));using var document=a.Value;if(document is null)return;
            items=attribute?(path.IsEmpty?document.ReadAttributeItems(0):document.ReadDescendantAttributeItems(0)):path.IsEmpty?document.ReadRootItems():document.ReadScalarItems(0);
            if(list)
            {
                if(attribute){if(path.IsEmpty)Assert.Throws<NotSupportedException>(()=>document.ReadAttribute(0));else Assert.Throws<NotSupportedException>(()=>document.ReadDescendantAttribute(0));}
                else if(path.IsEmpty){Assert.False(document.HasRootScalar);Assert.Throws<NotSupportedException>(()=>document.ReadRootScalar());}
                else Assert.Throws<NotSupportedException>(()=>document.ReadScalar(0));
            }
            exported=document.ExportXml();
        }
        Assert.Equal(row.GetProperty("expected").EnumerateArray().Select(v=>v.GetString()),items.Select(Text));
        Assert.True(XNode.DeepEquals(XElement.Parse(xml,LoadOptions.PreserveWhitespace),XElement.Parse(exported,LoadOptions.PreserveWhitespace)),id+":original XML");
    }
}
