// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Measures safe public Core regex semantics for a bounded XSD pattern contract.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 22-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Bosak.XPath.Api;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Runtime.Vm;
internal static class Program
{
    private static string WholePattern(string pattern)
    {
        var text=new System.Text.StringBuilder();int classes=0;bool escaped=false;
        foreach(var c in pattern)
        {
            if(escaped){text.Append(c);escaped=false;continue;}
            if(c=='\\'){text.Append(c);escaped=true;continue;}
            if(c=='[')classes++;if(c==']')classes--;
            if(classes==0&&c is '^' or '$')text.Append('\\');
            text.Append(c);
        }
        return "^("+text+")$";
    }
    private static int Main(string[] args)
    {
        try
        {
            var expression=XPath31Expression.Compile("matches($s,$p)");using var document=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"vectors.json")));var rows=new List<object>();
            foreach(var vector in document.RootElement.EnumerateArray())
            {
                var input=vector.GetProperty("input").GetString()!;var pattern=vector.GetProperty("pattern").GetString()!;bool expected=vector.GetProperty("xsdExpected").GetBoolean();
                bool? raw=null,whole=null;string? error=null;
                try{raw=expression.Evaluate(new EvaluationContext().WithVariable("s",XdmValue.FromString(input)).WithVariable("p",XdmValue.FromString(pattern))).BooleanValue;whole=expression.Evaluate(new EvaluationContext().WithVariable("s",XdmValue.FromString(input)).WithVariable("p",XdmValue.FromString(WholePattern(pattern)))).BooleanValue;}
                catch(Exception caught){error=caught.GetType().Name+": "+caught.Message;}
                rows.Add(new{id=vector.GetProperty("id").GetString(),input,pattern,xsdExpected=expected,rawMatches=raw,wholeCandidatePattern=WholePattern(pattern),wholeCandidateMatches=whole,error});
            }
            var report=new{coreAssembly=typeof(XdmValue).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion,defaultRegexTimeoutInfinite=new Regex("x").MatchTimeout==Regex.InfiniteMatchTimeout,rows,scope="Tiny public API capability sentinels; wrapping is experimental, not an admitted Schema implementation",noAdversarialPatterns=true};
            File.WriteAllText(args[0],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("Recorded20safe regex vectors; no Schema source changed.");return 0;
        }
        catch(Exception error){Console.Error.WriteLine("Caught regex probe failure: "+error.GetType().Name+": "+error.Message);return 1;}
    }
}
