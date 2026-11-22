// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Compiles bounded structural namespace assertions against actual instance expanded names.
// SPECIAL NOTES        : Internal bounded integer assertion engine; no public authority issuance or full XSD claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 22-11-2026     | Creation of isolated structural assertion prototype |
//                      | Charles Korthout | 0.2   | 22-11-2026     | Adopt qualified bounded structural and empty-content semantics |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


using System.Collections.Immutable;
using System.Xml.Linq;
namespace Bosak.Schema;
internal sealed class NativeStructuralAssertion
{
    private const string Functions="http://www.w3.org/2005/xpath-functions";
    internal sealed record Context(XName Name);
    internal string Expression { get; }
    internal ImmutableDictionary<string,string> Namespaces { get; }
    internal string DefaultNamespace { get; }
    internal string BaseUri { get; }
    private readonly Func<Context,bool> predicate;
    private NativeStructuralAssertion(string expression, ImmutableDictionary<string,string> namespaces,string defaultNamespace,string baseUri,Func<Context,bool> predicate)
    {Expression=expression;Namespaces=namespaces;DefaultNamespace=defaultNamespace;BaseUri=baseUri;this.predicate=predicate;}
    internal bool Evaluate(Context context)=>predicate(context);
    private static void Need(bool condition,bool capacity=false)=>NativeDecimalSchema.Require(condition,
        capacity?"Structural assertion capacity exceeded.":"Structural assertion grammar is outside the profile.",
        capacity?NativeDecimalUnsupportedReason.RepresentationCapacity:NativeDecimalUnsupportedReason.AssertionGrammar);
    private sealed record Token(string Text,bool Literal=false);
    internal static NativeStructuralAssertion Compile(string expression,ImmutableDictionary<string,string> namespaces,string defaultNamespace,string baseUri)
    {
        Need(expression.Length<=512,true);var tokens=new List<Token>();
        for(var i=0;i<expression.Length;)
        {
            char c=expression[i];if(c is ' ' or '\t' or '\r' or '\n'){i++;continue;}int start=i++;
            if(c is '\'' or '"')
            {
                var text=new System.Text.StringBuilder();bool closed=false;
                while(i<expression.Length){char next=expression[i++];if(next!=c){text.Append(next);continue;}if(i<expression.Length&&expression[i]==c){text.Append(c);i++;continue;}closed=true;break;}
                Need(closed);tokens.Add(new(text.ToString(),true));
            }
            else if(char.IsAsciiLetter(c)||c=='_')
            {while(i<expression.Length&&(char.IsAsciiLetterOrDigit(expression[i])||expression[i] is '_' or '-' or ':' or '.'))i++;tokens.Add(new(expression[start..i]));}
            else
            {Need(c is '(' or ')' or '.' or '=' or '!');if(c=='!'){Need(i<expression.Length&&expression[i]=='=');i++;}tokens.Add(new(expression[start..i]));}
            Need(tokens.Count<=128,true);
        }
        var parser=new Parser(tokens,namespaces);var predicate=parser.Or();Need(parser.End);return new(expression,namespaces,defaultNamespace,baseUri,predicate);
    }
    private sealed class Parser(List<Token> tokens,ImmutableDictionary<string,string> namespaces)
    {
        private int index,depth;
        internal bool End=>index==tokens.Count;
        private bool Take(string s){if(index<tokens.Count&&!tokens[index].Literal&&tokens[index].Text==s){index++;return true;}return false;}
        internal Func<Context,bool> Or(){var left=And();while(Take("or")){var a=left;var b=And();left=c=>a(c)||b(c);}return left;}
        private Func<Context,bool> And(){var left=Term();while(Take("and")){var a=left;var b=Term();left=c=>a(c)&&b(c);}return left;}
        private Func<Context,bool> Term()
        {
            if(Take("(")){Need(++depth<=16,true);var inner=Or();Need(Take(")"));depth--;return inner;}
            Need(index<tokens.Count&&!tokens[index].Literal);var function=tokens[index++].Text;var parts=function.Split(':');
            Need(parts.Length is 1 or 2);Need(parts[^1]=="namespace-uri");
            if(parts.Length==2)Need(namespaces.TryGetValue(parts[0],out var ns)&&ns==Functions);
            Need(Take("("));if(!Take(")")){Need(Take("."));Need(Take(")"));}
            bool unequal=false;if(Take("=")||Take("eq")){}else if(Take("!=")||Take("ne"))unequal=true;else Need(false);
            Need(index<tokens.Count&&tokens[index].Literal);var expected=tokens[index++].Text;
            return c=>unequal?!StringComparer.Ordinal.Equals(c.Name.NamespaceName,expected):StringComparer.Ordinal.Equals(c.Name.NamespaceName,expected);
        }
    }
}
internal sealed partial class NativeDecimalSchema
{
    private ImmutableArray<NativeStructuralAssertion> structuralAssertions=[];
    internal ImmutableArray<NativeStructuralAssertion> StructuralAssertions=>structuralAssertions;
    private static bool IsStructuralAssertion(XElement element)
    {
        var text=(string?)element.Attribute("test");if(text is null)return false;
        if(text.Length>512)return text.Contains("namespace-uri",StringComparison.Ordinal);
        var unquoted=System.Text.RegularExpressions.Regex.Replace(text,"'(?:''|[^'])*'|\"(?:\"\"|[^\"])*\"", " ",System.Text.RegularExpressions.RegexOptions.CultureInvariant,TimeSpan.FromSeconds(1));
        return System.Text.RegularExpressions.Regex.IsMatch(unquoted,@"(?:^|[^\w.:-])(?:[A-Za-z_][\w.-]*:)?namespace-uri\s*\(",System.Text.RegularExpressions.RegexOptions.CultureInvariant,TimeSpan.FromSeconds(1));
    }
    private static ImmutableArray<NativeStructuralAssertion> CompileStructuralAssertions(IEnumerable<XElement> elements,string targetNamespace,string defaultNamespace,CancellationToken token)
    {
        var result=ImmutableArray.CreateBuilder<NativeStructuralAssertion>();
        foreach(var element in elements)
        {
            token.ThrowIfCancellationRequested();Require(element.Name==Xs+"assert","Expected structural assertion.",NativeDecimalUnsupportedReason.AssertionGrammar);
            Attributes(element,"test","xpathDefaultNamespace");Require(Elements(element).Count==0,"Assertion children excluded.",NativeDecimalUnsupportedReason.AssertionGrammar);
            var text=(string?)element.Attribute("test");Require(text is not null,"Assertion expression required.",NativeDecimalUnsupportedReason.AssertionGrammar);
            var ns=ImmutableDictionary.CreateBuilder<string,string>(StringComparer.Ordinal);ns["xml"]=XNamespace.Xml.NamespaceName;
            foreach(var scope in element.AncestorsAndSelf().Reverse())foreach(var a in scope.Attributes().Where(a=>a.IsNamespaceDeclaration))ns[a.Name.LocalName=="xmlns"?"":a.Name.LocalName]=a.Value;
            result.Add(NativeStructuralAssertion.Compile(text!,ns.ToImmutable(),AssertionNamespace(element,targetNamespace,defaultNamespace),element.BaseUri));
            Require(result.Count<=8,"Structural assertion capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
        }
        return result.ToImmutable();
    }
    private void AssessStructuralAssertions(XElement element,CancellationToken token)
    {
        var context=new NativeStructuralAssertion.Context(element.Name);
        foreach(var assertion in structuralAssertions){token.ThrowIfCancellationRequested();Check(assertion.Evaluate(context),"Structural assertion evaluated false.");}
    }
}
