// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Verifies isolated empty typed-value representation through original native assessments.
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
//                      | Charles Korthout | 0.2   | 22-11-2026     | Adapt native controls for bounded integer simple content |
//                      | Charles Korthout | 0.3   | 22-11-2026     | Verify isolated contextual assertions and retained native baseline |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.Structural;
/// <summary>Verifies structural context, static refusal, limits and original authority.</summary>
public sealed class NativeStructuralBoundaryCoreTests
{
    private static string AssertXml(string expression)=>new XElement(XName.Get("assert","http://www.w3.org/2001/XMLSchema"),new XAttribute("test",expression)).ToString(SaveOptions.DisableFormatting);
    private static string Schema(string members,string extra="")=>$"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:f='http://www.w3.org/2005/xpath-functions' {extra}><xs:element name='r'><xs:complexType>{members}</xs:complexType></xs:element></xs:schema>";
    private static NativeOutcome Compile(string schema)=>NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:1024).Outcome;
    /// <summary>Expression byte-independent character, token, depth and component boundaries are exact.</summary>
    /// <param name="kind">Boundary selector.</param><param name="over">One beyond the supported limit.</param>
    [Theory][InlineData("characters",false)][InlineData("characters",true)][InlineData("tokens",false)][InlineData("tokens",true)][InlineData("depth",false)][InlineData("depth",true)][InlineData("assertions",false)][InlineData("assertions",true)]
    public void ExpressionBoundaries(string kind,bool over)
    {
        var expr="namespace-uri()=''";var members="";
        if(kind=="characters")expr=expr.PadRight(over?513:512);
        if(kind=="tokens"){var terms=Enumerable.Repeat("namespace-uri()=''",21).ToArray();for(int i=0;i<(over?4:3);i++)terms[i]="namespace-uri(.)=''";expr=string.Join(" and ",terms);Assert.True(expr.Length<=512);}
        if(kind=="depth")expr=new string('(',over?17:16)+expr+new string(')',over?17:16);
        members=string.Concat(Enumerable.Repeat(AssertXml(expr),kind=="assertions"?(over?9:8):1));
        var result=NativeDecimalSchema.Compile(Schema(members),new(256,256),maxStringCharacters:1024);Assert.Equal(over?NativeOutcome.Unsupported:NativeOutcome.Valid,result.Outcome);if(over)Assert.Equal(NativeDecimalUnsupportedReason.RepresentationCapacity,result.UnsupportedReason);
    }
    /// <summary>Zero-scalar empty children still consume the exact node and depth quotas.</summary>
    /// <param name="kind">Node/depth quota.</param><param name="over">One over the limit.</param>
    [Theory][InlineData("nodes",false)][InlineData("nodes",true)][InlineData("depth",false)][InlineData("depth",true)]
    public void EmptyNodeBudgets(string kind,bool over)
    {
        string members,xml;
        if(kind=="nodes")
        {members="<xs:sequence>";xml="<r>";for(int i=0;i<4;i++){int n=i==3&&!over?15:16;members+=$"<xs:element name='e{i}' maxOccurs='{n}'><xs:complexType>{AssertXml("namespace-uri()=''")}</xs:complexType></xs:element>";xml+=string.Concat(Enumerable.Repeat($"<e{i}/>",n));}members+="</xs:sequence>"+AssertXml("namespace-uri(.)=''");xml+="</r>";}
        else
        {members=AssertXml("namespace-uri()=''");xml="";for(int i=0;i<(over?4:3);i++){members=$"<xs:sequence><xs:element name='e{i}'><xs:complexType>{members}</xs:complexType></xs:element></xs:sequence>";xml=$"<e{i}>{xml}</e{i}>";}xml="<r>"+xml+"</r>";}
        var s=Schema(members);var compiled=NativeDecimalSchema.Compile(s,new(256,256),maxStringCharacters:1024);Assert.Equal(over?NativeOutcome.Unsupported:NativeOutcome.Valid,compiled.Outcome);if(over){Assert.Equal(NativeDecimalUnsupportedReason.RepresentationCapacity,compiled.UnsupportedReason);return;}
        using var f=new Fixture(s,xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Empty(f.Assessment.ScalarFacts);Assert.Equal(kind=="nodes"?64:4,f.Assessment.ElementFacts.Length);
    }
    /// <summary>URI strings retain case, relative spelling and percent escapes without normalization.</summary>
    /// <param name="actual">Instance URI.</param><param name="expected">Compared URI.</param><param name="valid">Expected assertion result.</param>
    [Theory][InlineData("urn:a%2f","urn:a%2f",true)][InlineData("urn:a%2f","urn:a%2F",false)][InlineData("relative/path","relative/path",true)][InlineData("urn:Case","urn:case",false)]
    public void ExactNamespaceStrings(string actual,string expected,bool valid)
    {var s=Schema(AssertXml($"namespace-uri(.)='{expected}'"),$"targetNamespace='{actual}'");using var f=new Fixture(s,$"<r xmlns='{actual}'/>");Assert.Equal(valid?SchemaAssessmentStatus.Valid:SchemaAssessmentStatus.Invalid,f.Assessment.Status);if(!valid){Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Assessment.Evidence);}}
    /// <summary>Original function bindings survive XML default namespace and local prefix shadowing.</summary>
    [Fact]
    public void StaticBindingsAreCaptured()
    {
        var s=Schema("""<xs:assert xmlns:f="urn:wrong" test="f:namespace-uri()=''"/>""");Assert.Equal(NativeOutcome.Unsupported,Compile(s));
        s=Schema("""<xs:assert xmlns:g="http://www.w3.org/2005/xpath-functions" xpathDefaultNamespace="urn:unrelated" test="g:namespace-uri()='' and namespace-uri(.)=''"/>""");using var f=new Fixture(s,"<r/>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var plan=NativeDecimalSchema.Compile(s,new(256,256),maxStringCharacters:1024).Value!;var c=Assert.Single(plan.StructuralAssertions);Assert.Equal("urn:unrelated",c.DefaultNamespace);Assert.Equal("http://www.w3.org/2005/xpath-functions",c.Namespaces["g"]);Assert.Equal("http://www.w3.org/XML/1998/namespace",c.Namespaces["xml"]);
    }
    /// <summary>Final type namespace differs from the actual unqualified instance node name.</summary>
    [Fact]
    public void NamedTypeNamespaceIsNotNodeNamespace()
    {var s="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:t='urn:types' targetNamespace='urn:types'><xs:complexType name='E'>"+AssertXml("namespace-uri(.)=''")+"</xs:complexType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='e' type='t:E' form='unqualified'/></xs:sequence>"+AssertXml("namespace-uri()='urn:types'")+"</xs:complexType></xs:element></xs:schema>";using var f=new Fixture(s,"<t:r xmlns:t='urn:types'><e/></t:r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var n=f.Element([0]);Assert.Equal(("urn:types","E"),n.SchemaTypeAnnotation);Assert.Equal("",n.NamespaceUri);Assert.Equal(XdmValueKind.Undefined,f.Run(op=>f.Evaluate(op,n)).Kind);}
    /// <summary>False root or child assertion publishes no partial original facts.</summary>
    /// <param name="child">False nested assertion instead of root.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void NestedFalseRollsBack(bool child)
    {var inner=AssertXml("namespace-uri()='"+(child?"wrong":"")+"'");var outer=AssertXml("namespace-uri()='"+(child?"":"wrong")+"'");var s=Schema("<xs:sequence><xs:element name='e'><xs:complexType>"+inner+"</xs:complexType></xs:element></xs:sequence>"+outer);using var f=new Fixture(s,"<r><e/></r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.Empty(f.Assessment.Facts);Assert.False(f.Input.IsAssessed);Assert.Null(f.Assessment.Evidence);}
    /// <summary>New structural grammar cannot silently broaden the application attribute profile.</summary>
    [Fact]
    public void ApplicationAttributeCombinationRefuses()
    {var s=Schema("<xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attribute name='a' type='xs:decimal'/>"+AssertXml("namespace-uri()=''") );Assert.Equal(NativeOutcome.Unsupported,Compile(s));}
}
