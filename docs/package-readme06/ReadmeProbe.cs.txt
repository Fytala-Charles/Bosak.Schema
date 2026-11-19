// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Compiles and exercises packaged README examples against a local public package.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : license.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.Schema;
using System.Text.Json;
internal static class Program
{
    private static NativeDecimalSessionOptions Options => new(256, 256, 1, 4, 1, 256, 256);
    private static NativeDecimalAssessmentContext Context => new(new DateTimeOffset(2026,11,19,0,0,0,TimeSpan.Zero),0);
    private static int Main(string[] args)
    {
        try
        {
            Directory.SetCurrentDirectory(args[0]);
            var original = Console.Out; var output = new StringWriter(); Console.SetOut(output);
            try { Workflow(); } finally { Console.SetOut(original); }
            Require(output.ToString().StartsWith("12.5"+Environment.NewLine+"2"+Environment.NewLine+"1"+Environment.NewLine), "workflow output");
            Require(output.ToString().Contains("12.50"), "lexical export");
            string schema=File.ReadAllText("native-decimal.xsd");
            string xml=File.ReadAllText("native-decimal.xml");
            Check(NativeDecimalSession.Create(schema.Replace("<xs:schema ", "<xs:schema targetNamespace='urn:PRIVATE-MARKER' "),Options), NativeDecimalUnsupportedReason.Namespace, "A schema namespace setting");
            Check(NativeDecimalSession.Create(schema.Replace("count(amount) eq 2 and tax gt 0", "sum(amount) eq 2"),Options), NativeDecimalUnsupportedReason.AssertionGrammar, "inspect its documented limits");
            using var session=NativeDecimalSession.Create(schema,Options).Value!;
            Check(session.Assess(xml.Replace("<order>","<order flag='PRIVATE-MARKER'>"),Context), NativeDecimalUnsupportedReason.Attributes, "An attribute");
            Check(session.Assess(new string(' ',65537),Context),NativeDecimalUnsupportedReason.RepresentationCapacity,"An explicit representation limit");
            Check(session.Assess("<!DOCTYPE order><order/>",Context),NativeDecimalUnsupportedReason.Unspecified,"inspect its documented limits");
            var valid=session.Assess(xml,Context); using var document=valid.Value!;
            Inspect(valid); Require(valid.Diagnostics.IsEmpty,"valid diagnostics");
            File.WriteAllText(args[1],JsonSerializer.Serialize(new {codeBlocksCompiled=3,workflowOutput=true,namespaceBranch=true,attributeBranch=true,capacityBranch=true,grammarDefault=true,unspecifiedDefault=true,validGuard=true,privateInputNotPrinted=true,packageVersion="0.6.0-alpha.0.2",published=false},new JsonSerializerOptions {WriteIndented=true}));
            return 0;
        }
        catch(Exception e) { Console.Error.WriteLine("Caught README diagnostic failure: "+e.GetType().Name); return 1; }
    }
    private static void Require(bool condition,string label) { if(!condition) throw new InvalidOperationException(label); }
    private static void Check<T>(NativeDecimalResult<T> result,NativeDecimalUnsupportedReason reason,string expected) where T:class
    {
        Require(result.Outcome==NativeDecimalOutcome.Unsupported && result.Value is null,"refusal");
        Require(result.Diagnostics[0].UnsupportedReason==reason,"reason");
        var original=Console.Error;var output=new StringWriter();Console.SetError(output);
        try { Inspect(result); } finally {Console.SetError(original);}
        Require(output.ToString().Contains(result.Diagnostics[0].Code) && output.ToString().Contains(expected) && !output.ToString().Contains("PRIVATE-MARKER"),"diagnostic text");
    }
    private static void Workflow()
    {


var options = new NativeDecimalSessionOptions(256, 256, 1, 4, 1, 256, 256);
var episode = new NativeDecimalAssessmentContext(
    new DateTimeOffset(2026, 11, 17, 12, 0, 0, TimeSpan.Zero), 60);
var compiled = NativeDecimalSession.Create(File.ReadAllText("native-decimal.xsd"), options);
if (compiled.Outcome != NativeDecimalOutcome.Valid)
{
    Console.Error.WriteLine(compiled.Diagnostics[0].Code);
    return;
}

using var session = compiled.Value!;
var result = session.Assess(File.ReadAllText("native-decimal.xml"), episode);
if (result.Outcome != NativeDecimalOutcome.Valid)
{
    Console.Error.WriteLine(result.Diagnostics[0].Code);
    return;
}

using var document = result.Value!;
for (var index = 0; index < document.DecimalCount; index++)
    Console.WriteLine(document.ReadDecimal(index).ToInvariantString(256));
Console.WriteLine(document.ExportXml());
    }
    private static void Inspect<T>(NativeDecimalResult<T> result) where T:class
    {
if (result.Outcome == NativeDecimalOutcome.Unsupported)
{
    var diagnostic = result.Diagnostics[0];
    Console.Error.WriteLine(diagnostic.Code);
    switch (diagnostic.UnsupportedReason)
    {
        case NativeDecimalUnsupportedReason.Namespace:
            Console.Error.WriteLine("A schema namespace setting is outside the profile.");
            break;
        case NativeDecimalUnsupportedReason.Attributes:
            Console.Error.WriteLine("An attribute is outside the profile.");
            break;
        case NativeDecimalUnsupportedReason.RepresentationCapacity:
            Console.Error.WriteLine("An explicit representation limit was exceeded.");
            break;
        default:
            Console.Error.WriteLine("The input is outside the profile; inspect its documented limits.");
            break;
    }
}
    }
    private static void ExistingFactoriesCompile()
    {


var compiler = SchemaServices.CreateSchemaCompiler();
var validator = SchemaServices.CreateValidationService();
var xsltCompiler = SchemaAwareXslt.CreateCompiler();
    }
}
