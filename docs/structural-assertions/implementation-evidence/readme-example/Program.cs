// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Compares the delivered scalar surface against its proposal and prior public APIs.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
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
using System.Xml.Linq;
internal static class Program
{
    private static void Require(bool condition){if(!condition)throw new InvalidOperationException("README example check failed.");}
    private static int Main()
    {
        try
        {
            var schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example.xsd"));var xml="<r xmlns='urn:document'/>";var options=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);
            using(var session=NativeSchemaSession.Create(schema,options).Value!){using(var d=session.Assess(xml,context).Value!){Require(!d.HasRootScalar);Require(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));bool refused=false;try{_=d.ReadRootScalar();}catch(NotSupportedException){refused=true;}Require(refused);}Require(session.Assess("<r xmlns='urn:document'> </r>",context).Outcome==NativeSchemaOutcome.Invalid);}
            using(var session=NativeSchemaNodeDataSession.Create(schema,options).Value!){using(var d=session.Assess(xml,context).Value!){Require(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));bool refused=false;try{_=d.ReadElement([]);}catch(NotSupportedException e){refused=!e.Message.Contains("FOTY0012",StringComparison.Ordinal);}Require(refused);}Require(session.Assess("<r xmlns='urn:document'> </r>",context).Outcome==NativeSchemaOutcome.Invalid);}
            Console.WriteLine("Packaged README XML example passes both facades, scalar refusal and whitespace invalidity.");return 0;
        }catch(Exception error){Console.Error.WriteLine("Caught README diagnostic: "+error.Message);return 1;}
    }
}
