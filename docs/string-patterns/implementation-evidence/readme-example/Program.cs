// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Measures native outcomes for hash-verified unchanged upstream XSD fixtures.
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
using System.Xml.Linq;
internal static class Program
{
    private static void Check(bool condition){if(!condition)throw new InvalidOperationException("Pattern README check failed.");}
    private static int Main()
    {
        try
        {
            var schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example.xsd"));var options=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);string xml="<r><code>ABC-1234</code></r>";
            using(var s=NativeSchemaSession.Create(schema,options).Value!){using(var d=s.Assess(xml,context).Value!){Check(d.ReadScalar(0).StringValue=="ABC-1234");Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(s.Assess("<r><code>ABC-12345</code></r>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<r><code>XABC-1234</code></r>",context).Outcome==NativeSchemaOutcome.Invalid);}
            using(var s=NativeSchemaNodeDataSession.Create(schema,options).Value!){using(var d=s.Assess(xml,context).Value!){Check(d.ReadElement([0]).StringValue=="ABC-1234");Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(s.Assess("<r><code>ABC-12345</code></r>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<r><code>XABC-1234</code></r>",context).Outcome==NativeSchemaOutcome.Invalid);}
            Console.WriteLine("Packaged pattern example passes both facades, exact scalar/export and whole-literal rejection controls.");return 0;
        }catch(Exception e){Console.Error.WriteLine("Caught pattern README diagnostic: "+e.Message);return 1;}
    }
}
