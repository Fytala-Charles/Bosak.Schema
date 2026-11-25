// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies exact interchange contracts through both public facades.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 25-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Verifies original interchange stimuli with no friend access.</summary>
public sealed class NativeInterchange80ConsumerTests
{
    /// <summary>Gets stable positive/edge cases through both facades and extreme explicit contexts.</summary>
    /// <returns>Original schema/instance stimuli and execution contexts.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","Interchange80","cases.json")));
        foreach(var row in document.RootElement.GetProperty("cases").EnumerateArray())
            foreach(bool data in new[]{false,true}) foreach(int zone in new[]{-840,840}) yield return [row.GetProperty("id").GetString()!,row.GetRawText(),data,zone];
    }
    /// <summary>Compiles, assesses and reads every original contract stimulus.</summary>
    /// <param name="id">Stable contract case identity.</param><param name="json">Frozen case payload.</param><param name="data">Original-node facade selector.</param><param name="zone">Explicit assessment timezone.</param>
    [Theory][MemberData(nameof(Cases))]
    public void Contract(string id,string json,bool data,int zone)
    {
        using var parsed=JsonDocument.Parse(json);var row=parsed.RootElement;
        int Number(string name,int fallback)=>row.TryGetProperty(name,out var v)?v.GetInt32():fallback;
        string Text(string name,string fallback)=>row.TryGetProperty(name,out var v)?v.GetString()!:fallback;
        var options=new NativeSchemaSessionOptions(Number("digits",256),Number("scale",256),Number("maxCharacters",256),65536,1,256,1,256,Number("queryScale",256));
        var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,zone);
        var schema=Text("schema","");var xml=Text("instance","");var wantCompile=Enum.Parse<NativeSchemaOutcome>(Text("compile","Valid"));var want=Enum.Parse<NativeSchemaOutcome>(Text("outcome","Valid"));
        var path=row.TryGetProperty("path",out var p)?p.EnumerateArray().Select(v=>v.GetInt32()).ToImmutableArray():ImmutableArray<int>.Empty;
        NativeScalarValue? detached=null;string? exported=null;
        if(data)
        {
            var compiled=NativeSchemaNodeDataSession.Create(schema,options);Assert.True(compiled.Outcome==wantCompile,id+": compile "+compiled.Outcome);
            using var session=compiled.Value;if(session is null){Assert.NotEqual(NativeSchemaOutcome.Valid,wantCompile);return;}
            var assessed=session.Assess(xml,context);Assert.True(assessed.Outcome==want,id+": assess "+assessed.Outcome);using var document=assessed.Value;
            if(document is null){Assert.NotEqual(NativeSchemaOutcome.Valid,want);return;}
            NativeScalarValue Read()=>Text("target","")=="attribute"?document.ReadAttribute(path,"","a"):document.ReadElement(path);
            if(Text("goodRead","")=="Unsupported" || Text("badRead","")=="Unsupported"){Assert.Throws<NotSupportedException>(()=>Read());return;}
            detached=Read();exported=document.ExportXml();
        }
        else
        {
            var compiled=NativeSchemaSession.Create(schema,options);Assert.True(compiled.Outcome==wantCompile,id+": compile "+compiled.Outcome);
            using var session=compiled.Value;if(session is null){Assert.NotEqual(NativeSchemaOutcome.Valid,wantCompile);return;}
            var assessed=session.Assess(xml,context);Assert.True(assessed.Outcome==want,id+": assess "+assessed.Outcome);using var document=assessed.Value;
            if(document is null){Assert.NotEqual(NativeSchemaOutcome.Valid,want);return;}
            NativeScalarValue Read()=>Text("target","")=="attribute"?(path.IsEmpty?document.ReadAttribute(0):document.ReadDescendantAttribute(0)).Value:path.IsEmpty?document.ReadRootScalar():document.ReadScalar(0);
            if(Text("goodRead","")=="Unsupported" || Text("badRead","")=="Unsupported"){Assert.Throws<NotSupportedException>(()=>Read());return;}
            detached=Read();exported=document.ExportXml();
        }
        Assert.True(XNode.DeepEquals(XElement.Parse(xml,LoadOptions.PreserveWhitespace),XElement.Parse(exported!,LoadOptions.PreserveWhitespace)),id+": original XML");
        Assert.NotNull(detached);
        if(row.TryGetProperty("empty",out var empty)&&empty.GetBoolean()){Assert.Equal(NativeScalarKind.Empty,detached.Kind);Assert.Throws<InvalidOperationException>(()=>detached.CalendarValue);return;}
        if (Text("family", "") == "QName")
        {
            Assert.Equal(NativeScalarKind.QName, detached.Kind);
            Assert.Equal(Text("local", ""), detached.QNameValue.LocalName);
            Assert.Equal(Text("namespace", ""), detached.QNameValue.NamespaceUri);
        }
        else
        {
            Assert.Equal(Text("family", "") == "hexBinary" ? NativeScalarKind.HexBinary : NativeScalarKind.Base64Binary, detached.Kind);
            Assert.Equal(Text("bytes", ""), Convert.ToHexString(detached.BinaryValue.AsSpan()));
        }
        Assert.Throws<InvalidOperationException>(() => detached.StringValue);
    }
}
