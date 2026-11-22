// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Verifies isolated bounded pattern matching and native original string assessment.
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

using System.Text.Json;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace PatternProbe;
/// <summary>Checks an independent bounded oracle, original inputs and nested provenance.</summary>
public sealed class FurtherTests
{
    private static string F(string p)=>new XElement(XName.Get("pattern","http://www.w3.org/2001/XMLSchema"),new XAttribute("value",p)).ToString(SaveOptions.DisableFormatting);
    /// <summary>Finite ASCII languages agree with an explicit-timeout nonbacktracking oracle.</summary>
    [Fact]
    public void FiniteLanguageOracle()
    {
        string[] basic=["","a","b",".","a*","a?","a+","(a|b)","(ab|a)","[ab]","[^b]","[a-b]","(a|aa)*","a{0,2}","(a?)*","(a|)*","()","()*"];
        var patterns=basic.Concat(basic.SelectMany(a=>basic.Select(b=>a+b))).Distinct().ToArray();var inputs=new List<string>{""};var last=new List<string>{""};for(int length=1;length<=4;length++){last=last.SelectMany(s=>new[]{s+"a",s+"b",s+"x"}).ToList();inputs.AddRange(last);}int comparisons=0;
        foreach(var pattern in patterns)
        {
            var expected=new Regex("\\A(?:"+pattern+")\\z",RegexOptions.CultureInvariant|RegexOptions.NonBacktracking,TimeSpan.FromMilliseconds(100));var actual=NativePattern.Compile(pattern,new(default));
            foreach(var input in inputs){Assert.Equal(expected.IsMatch(input),actual.Match(input,new(default)));comparisons++;}
        }
        File.WriteAllText(Path.Combine(AppContext.BaseDirectory,"oracle-counts.json"),JsonSerializer.Serialize(new{patterns=patterns.Length,inputs=inputs.Count,comparisons,scope="Finite authored ASCII oracle comparisons; not upstream cases or additive conformance tests"}));
    }
    /// <summary>Measures unchanged original public pattern cases through both owned facades.</summary>
    /// <param name="id">Original identity.</param><param name="schemaFile">Exact original schema payload.</param><param name="xmlFile">Exact original instance payload.</param><param name="data">Selects data facade.</param>
    [Theory][MemberData(nameof(Originals))]
    public void OriginalPublicPatterns(string id,string schemaFile,string xmlFile,bool data)
    {
        Assert.StartsWith("MS-Element",id);string schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"originals",schemaFile)),xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"originals",xmlFile));var options=new NativeSchemaSessionOptions(256,256,65536,524288,1,256,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);
        if(data){using var s=NativeSchemaNodeDataSession.Create(schema,options).Value!;var a=s.Assess(xml,context);Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);using var d=a.Value!;Assert.Equal(NativeScalarKind.String,d.ReadElement([0]).Kind);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}
        else{using var s=NativeSchemaSession.Create(schema,options).Value!;var a=s.Assess(xml,context);Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);using var d=a.Value!;Assert.Equal(NativeScalarKind.String,d.ReadScalar(0).Kind);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}
    }
    /// <summary>Gets original immutable fixture identifiers and paths.</summary>
    /// <returns>Original input references and facade selector.</returns>
    public static IEnumerable<object[]> Originals()
    {using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"original-patterns.json")));foreach(var row in d.RootElement.EnumerateArray())foreach(bool data in new[]{false,true})yield return [row.GetProperty("id").GetString()!,row.GetProperty("schema").GetString()!,row.GetProperty("instance").GetString()!,data];}
    /// <summary>Anonymous nested original types preserve identity and false matches publish nothing.</summary>
    [Theory][InlineData(true)][InlineData(false)]
    public void NestedAnonymousProvenance(bool valid)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='box'><xs:complexType><xs:sequence><xs:element name='v'><xs:simpleType><xs:restriction base='xs:string'>"+F("a+")+"</xs:restriction></xs:simpleType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:schema>";using var f=new Fixture(schema,valid?"<r><box><v>aaa</v></box></r>":"<r><box><v>bbb</v></box></r>");Assert.Equal(valid?SchemaAssessmentStatus.Valid:SchemaAssessmentStatus.Invalid,f.Assessment.Status);
        if(!valid){Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Assessment.Evidence);return;}
        var node=f.Element([0,0]);Assert.Null(node.SchemaTypeAnnotation);var fact=f.Input.FactsFor((NativeSessionNode)node)!;Assert.False(fact.HasNoTypedValue);Assert.Null(fact.NativeType.Name);f.Run(op=>{Assert.True(SchemaAtomics.TryGetProvenance(f.Evaluate(op,node),out var evidence));Assert.Same(fact.Type,evidence!.DeclaredType);Assert.Same(fact.Type,evidence.ActualType);Assert.Same(f.Assessment.Origin,evidence.OriginalAssessment);return 0;});Assert.Equal("aaa",f.Read(node).StringValue);
    }
    /// <summary>Compilation pre-cancellation never leaves shared state and a new attempt recovers.</summary>
    [Fact]
    public void CompilationCancellation()
    {using var c=new CancellationTokenSource();c.Cancel();Assert.ThrowsAny<OperationCanceledException>(()=>NativePattern.Compile("a*",new(c.Token)));Assert.True(NativePattern.Compile("a*",new(default)).Match("aaa",new(default)));}
    /// <summary>XSD1.1 class escapes do not become scalar range endpoints.</summary>
    /// <param name="value">A union member or a negative control.</param><param name="expected">Expected membership.</param>
    [Theory][InlineData(" ",true)][InlineData("-",true)][InlineData("a",true)][InlineData("b",false)]
    public void ClassEscapeHyphenUnion(string value,bool expected)
    {Assert.Equal(expected,NativePattern.Compile("[\\s-a]",new(default)).Match(value,new(default)));}
    /// <summary>Measures the unchanged original authored cases that the new grammar admits.</summary>
    /// <param name="id">Original authored ID.</param><param name="data">Selects data facade.</param>
    [Theory][InlineData("M2D1-66",false)][InlineData("M2D1-66",true)][InlineData("M2D1-80",false)][InlineData("M2D1-80",true)]
    public void ExplicitAuthoredAdmissions(string id,bool data)
    {
        using var doc=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"prior-string-cases.json")));var row=doc.RootElement.EnumerateArray().Single(r=>r.GetProperty("id").GetString()==id);string schema=row.GetProperty("schema").GetString()!,xml=row.GetProperty("instance").GetString()!;var options=new NativeSchemaSessionOptions(256,256,256,65536,1,8,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal("Unsupported",row.GetProperty("expectedCompile").GetString());
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;var a=s.Assess(xml,context);Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);using var d=a.Value!;Assert.Equal("abc",d.ReadElement([0]).StringValue);}
        else{var c=NativeSchemaSession.Create(schema,options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;var a=s.Assess(xml,context);Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);using var d=a.Value!;Assert.Equal("abc",d.ReadScalar(0).StringValue);}
    }
    /// <summary>Unused definitions are parsed eagerly under syntax and profile rules.</summary>
    /// <param name="pattern">Unused source pattern.</param><param name="outcome">Expected compile outcome.</param>
    [Theory][InlineData("(","Invalid")][InlineData("\\p{L}","Unsupported")][InlineData("a{17}","Unsupported")]
    public void UnusedDefinitionsCompileEagerly(string pattern,string outcome)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='Unused'><xs:restriction base='xs:string'>"+F(pattern)+"</xs:restriction></xs:simpleType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:string'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
        var options=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);var a=NativeSchemaSession.Create(schema,options);var b=NativeSchemaNodeDataSession.Create(schema,options);Assert.Equal(outcome,a.Outcome.ToString());Assert.Equal(outcome,b.Outcome.ToString());Assert.Null(a.Value);Assert.Null(b.Value);
    }
    /// <summary>The legacy decimal-only profile does not admit string pattern types.</summary>
    [Fact]
    public void LegacyDecimalProfileStillRefuses()
    {var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='S'><xs:restriction base='xs:string'>"+F("a*")+"</xs:restriction></xs:simpleType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='S'/></xs:sequence></xs:complexType></xs:element></xs:schema>";var r=NativeDecimalSession.Create(schema,new(256,256,1,32,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,r.Outcome);Assert.Null(r.Value);}
}
