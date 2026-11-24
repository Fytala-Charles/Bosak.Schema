// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Measures date transport boundaries through published package APIs.
// SPECIAL NOTES        : Caught diagnostic only; no Schema validation substitute or production change.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 24-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Globalization;
using System.Text.Json;
using Bosak.Schema;
using Bosak.XPath.Core.Xdm;

CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;
var rows=new List<object>();
var years=new long[]{0,-4,-1,1,10000,-long.MaxValue,long.MaxValue,long.MinValue};
var zones=new int?[]{null,0,840,-840};
foreach(var year in years)foreach(var zone in zones)
{
    try
    {
        var raw=new XPathDateTime(year,1,1,0,0,0,0,zone??0,zone.HasValue);
        var atom=XdmValue.FromDate(raw,zone.HasValue,"date");
        var read=atom.DateXPathValue;
        if(atom.Kind!=XdmValueKind.Date || read.Year!=year || read.Month!=1 || read.Day!=1 || read.HasTimezone!=zone.HasValue || read.TimezoneOffsetMinutes!=(zone??0))throw new InvalidOperationException("Component transport mismatch.");
        string? format=null,error=null;
        try{format=atom.ToString();}catch(Exception e){error=e.GetType().Name+": "+e.Message;}
        var expectedYear=year<0?"-"+System.Numerics.BigInteger.Abs(new System.Numerics.BigInteger(year)).ToString("D4"):year.ToString("D4");
        var expectedZone=zone is null?"":zone==0?"Z":zone==840?"+14:00":"-14:00";
        var expected=expectedYear+"-01-01"+expectedZone;
        rows.Add(new{year,zone,componentTransport="Pass",formatting=format==expected?"Pass":"Fail",format,expected,error});
    }
    catch(Exception e){rows.Add(new{year,zone,componentTransport="Fail",error=e.GetType().Name+": "+e.Message});}
}
// Native date support is not implemented: report actual refusals, without a synthetic provider.
var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:date'/></xs:schema>";
var options=new NativeSchemaSessionOptions(256,256,16384,65536,1,256,1,256,256);
using var scalar=NativeSchemaSession.Create(schema,options).Value;
var a=NativeSchemaSession.Create(schema,options);a.Value?.Dispose();
var b=NativeSchemaNodeDataSession.Create(schema,options);b.Value?.Dispose();
var report=new{core=typeof(XdmValue).Assembly.GetName().Version!.ToString(),schema=typeof(NativeSchemaSession).Assembly.GetName().Version!.ToString(),scope="Constructor/accessor/formatting preflight only; no Schema validation, original-node facts, detached facade or lifetime qualification",rows,baselineScalar=a.Outcome.ToString(),baselineNodeData=b.Outcome.ToString(),nativeFeasibility="Stopped at proposed full-Int64 formatter boundary; remaining native gates unexecuted"};
File.WriteAllText(args[0],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true})+Environment.NewLine);
Console.WriteLine("Captured 32 caught date transport/formatting rows and two actual native compilation refusals.");
