// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Verifies the packaged unsignedLong root example through both public facades.
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
//                      | Charles Korthout | 0.2   | 22-11-2026     | Verify packaged simple-root reads, export and whole-literal negative controls |
//                      | Charles Korthout | 0.3   | 22-11-2026     | Verify exact derived integer root, export and bounds |
//                      | Charles Korthout | 0.4   | 23-11-2026     | Verify exact packaged all example and optionality controls |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.Schema;
using System.Numerics;
using System.Globalization;
using System.Xml.Linq;
internal static class Program
{
    private static void Check(bool value){if(!value)throw new InvalidOperationException("Packaged counter example assertion failed.");}
    private static void AllExample()
    {
        string schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example-all.xsd")),xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example-all.xml"));
        var options=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);NativeScalarValue detached;
        using(var s=NativeSchemaSession.Create(schema,options).Value!)
        {
            using(var d=s.Assess(xml,context).Value!){Check(!d.HasRootScalar&&d.ScalarCount==2);Check(d.ReadScalar(0).IntegerValue==7);Check(d.ReadScalar(1).StringValue=="");detached=d.ReadScalar(0);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}
            using(var d=s.Assess("<r/>",context).Value!){Check(d.ScalarCount==0&&!d.HasRootScalar);}
            Check(s.Assess("<r><b>7</b></r>",context).Outcome==NativeSchemaOutcome.Invalid);
            Check(s.Assess("<r><b>7</b><b>8</b><a/></r>",context).Outcome==NativeSchemaOutcome.Invalid);
        }
        Check(detached.IntegerValue==7);
        using(var s=NativeSchemaNodeDataSession.Create(schema,options).Value!)
        {
            using(var d=s.Assess(xml,context).Value!){Check(d.ReadElement([0]).IntegerValue==7);Check(d.ReadElement([1]).StringValue=="");Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}
            using(var d=s.Assess("<r/>",context).Value!){Check(XNode.DeepEquals(XElement.Parse("<r/>"),XElement.Parse(d.ExportXml())));}
            Check(s.Assess("<r><b>7</b></r>",context).Outcome==NativeSchemaOutcome.Invalid);
        }
        Console.WriteLine("Exact packaged all example passes both facades, physical order, singleton empty string, optional empty group, invalid missing/duplicate members, export and detached lifetime.");
    }
    private static int Main()
    {
        try
        {
            string schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example.xsd")),xml="<counter>18446744073709551615</counter>";var expected=BigInteger.Parse("18446744073709551615",CultureInfo.InvariantCulture);var options=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);NativeScalarValue detached;
            using(var s=NativeSchemaSession.Create(schema,options).Value!){using(var d=s.Assess(xml,context).Value!){Check(d.HasRootScalar&&d.ScalarCount==0);detached=d.ReadRootScalar();Check(detached.IntegerValue==expected);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(s.Assess("<counter>18446744073709551616</counter>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<counter>-1</counter>",context).Outcome==NativeSchemaOutcome.Invalid);using var zero=s.Assess("<counter>-0</counter>",context).Value!;Check(zero.ReadRootScalar().IntegerValue.IsZero);}Check(detached.IntegerValue==expected);
            using(var s=NativeSchemaNodeDataSession.Create(schema,options).Value!){using(var d=s.Assess(xml,context).Value!){detached=d.ReadElement([]);Check(detached.IntegerValue==expected);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(s.Assess("<counter>18446744073709551616</counter>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<counter>-1</counter>",context).Outcome==NativeSchemaOutcome.Invalid);using var zero=s.Assess("<counter>-0</counter>",context).Value!;Check(zero.ReadElement([]).IntegerValue.IsZero);}Check(detached.IntegerValue==expected);
            AllExample();Console.WriteLine("Packaged unsignedLong example passes both facades, exact BigInteger/export/detached lifetime, invalid bounds and signed zero.");return 0;
        }catch(Exception e){Console.Error.WriteLine("Caught counter README diagnostic: "+e.Message);return 1;}
    }
}
