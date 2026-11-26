// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 November 2026
// PURPOSE              : Verifies all optionality, original identity, routing and independent source budgets.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 23-11-2026     | Verify absent root typed-value diagnostic and caught FOTY0012 |
//                      | Charles Korthout | 0.3   | 23-11-2026     | Align test responsibility header with its primary type |
//                      | Charles Korthout | 0.4 | 26-11-2026     | Reconcile original markup admissions without changing stimuli |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================



using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.NodeData;
/// <summary>Verifies optionality, identity, routing and independent source budgets.</summary>
public sealed class NativeAllAdditionalTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,1,256,1,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private static string Schema(string model,string suffix="",string extra="")=>$"<xs:schema xmlns:xs='{X}'>{extra}<xs:element name='r'><xs:complexType>{model}{suffix}</xs:complexType></xs:element></xs:schema>";
    /// <summary>Optional whole-group and member minima remain independent.</summary>
    /// <param name="outer">Group minimum.</param><param name="a">First member minimum.</param><param name="b">Second member minimum.</param><param name="body">Physical content.</param><param name="valid">Expected validity.</param>
    [Theory]
    [InlineData(0,1,1,"",true)][InlineData(1,0,0,"",true)][InlineData(1,1,0,"",false)]
    [InlineData(0,1,1,"<b>7</b>",false)][InlineData(0,1,1,"<b>7</b><a>x</a>",true)]
    [InlineData(1,1,0,"<a/>",true)][InlineData(0,0,0,"<b>7</b><a/>",true)]
    public void OptionalityNativeFacts(int outer,int a,int b,string body,bool valid)
    {
        var model=$"<xs:all minOccurs='{outer}'><xs:element name='a' type='xs:string' minOccurs='{a}'/><xs:element name='b' type='xs:int' minOccurs='{b}'/></xs:all>";
        using var f=new Fixture(Schema(model),"<r>"+body+"</r>");Assert.Equal(valid?SchemaAssessmentStatus.Valid:SchemaAssessmentStatus.Invalid,f.Assessment.Status);
        if(!valid){Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Assessment.Evidence);return;}
        var root=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.True(root.HasNoTypedValue);Assert.False(root.NativeType.EmptyContent);Assert.Equal(SchemaResultKind.Unsupported,root.ReadTypedValue(default).Kind);Assert.Equal("FOTY0012",root.ReadTypedValue(default).DiagnosticCode);var error=Assert.Throws<InvalidOperationException>(()=>f.Run(op=>f.Evaluate(op,f.Input.Root)));Assert.Contains("FOTY0012",error.Message,StringComparison.Ordinal);
        Assert.Equal(f.Input.ElementNodes.Length,f.Assessment.ElementFacts.Length);
        if(body.Length==0)Assert.Single(f.Assessment.ElementFacts);
    }
    /// <summary>All sums each member while retaining physical order on both compiler routes.</summary>
    /// <param name="nested">Select mixed-catalog nested routing.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void SumBudgetAndBothRoutes(bool nested)
    {
        var model="<xs:all>"+string.Concat(Enumerable.Range(0,16).Select(i=>$"<xs:element name='n{i}' type='xs:int'/>"))+"</xs:all>";
        var extra=nested?"<xs:element name='other'><xs:complexType><xs:sequence><xs:element name='box'><xs:complexType><xs:sequence><xs:element name='v' type='xs:int'/></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element>":"";
        using var f=new Fixture(Schema(model,extra:extra),"<r>"+string.Concat(Enumerable.Range(0,16).Reverse().Select(i=>$"<n{i}>{i}</n{i}>"))+"</r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Equal(17,f.Assessment.ElementFacts.Length);
        for(int i=0;i<16;i++){var n=f.Element([i]);var fact=f.Input.FactsFor((NativeSessionNode)n)!;Assert.Equal("n"+(15-i),fact.NativeDeclaration.Name);Assert.Equal(15-i,f.Read(n).IntegerValue);Assert.Same(fact,f.Assessment.ElementFacts[i+1]);}
    }
    /// <summary>Root attributes retain declaration order while element slots use physical order.</summary>
    /// <param name="named">Select named root complex type.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void RootAttributeReadPolicy(bool named)
    {
        const string model="<xs:all><xs:element name='a' type='xs:string'/><xs:element name='b' type='xs:int'/></xs:all>";
        const string suffix="<xs:attribute name='z' type='xs:string'/><xs:attribute name='id' type='xs:unsignedLong' use='required'/>";
        var schema=named?$"<xs:schema xmlns:xs='{X}'><xs:complexType name='C'>{model}{suffix}</xs:complexType><xs:element name='r' type='C'/></xs:schema>":Schema(model,suffix);
        const string xml="<r id='18446744073709551615' z='last'><b>7</b><a>x</a></r>";
        using(var s=NativeSchemaSession.Create(schema,Options).Value!){using var d=s.Assess(xml,Context).Value!;Assert.Equal(2,d.AttributeCount);Assert.Equal("z",d.ReadAttribute(0).LocalName);Assert.Equal("id",d.ReadAttribute(1).LocalName);Assert.True(d.ReadAttribute(1).OwnerElementPath.IsEmpty);Assert.Equal(7,d.ReadScalar(0).IntegerValue);}
        using var f=new Fixture(schema,xml);var a=f.Attribute([],"","id");var fact=f.Input.FactsFor((NativeSessionNode)a)!;Assert.True(fact.OwnerElementPath.IsEmpty);Assert.Equal("unsignedLong",fact.NativeType.Name);Assert.True(f.Session.Advisor!.OwnsNode(a,fact,f.Input,f.Assessment.Origin));
    }
    /// <summary>Unused all-root anonymous definitions consume the existing source-site budget.</summary>
    /// <param name="count">Total source sites.</param><param name="expected">Expected compile result.</param>
    [Theory][InlineData(16,"Valid")][InlineData(17,"Unsupported")]
    public void AnonymousSourceBudget(int count,string expected)
    {
        string Members(int n)=>string.Concat(Enumerable.Range(0,n).Select(i=>$"<xs:element name='n{i}'><xs:simpleType><xs:restriction base='xs:string'/></xs:simpleType></xs:element>"));
        var other="<xs:element name='other'><xs:complexType><xs:all>"+Members(count-8)+"</xs:all></xs:complexType></xs:element>";
        var c=NativeSchemaSession.Create(Schema("<xs:all>"+Members(8)+"</xs:all>",extra:other),Options);Assert.Equal(expected,c.Outcome.ToString());c.Value?.Dispose();
    }
    /// <summary>Excluded XML nodes refuse assessment without partial publication.</summary>
    /// <param name="content">Comment or processing instruction.</param>
    [Theory][InlineData("<!--comment-->")][InlineData("<?p value?>")]
    public void ExcludedNodes(string content)
    {
        using var f=new Fixture(Schema("<xs:all minOccurs='0'><xs:element name='a' type='xs:string' minOccurs='0'/></xs:all>"),"<r>"+content+"</r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.True(f.Input.IsAssessed);Assert.NotEmpty(f.Assessment.Facts);Assert.True(f.Input.FactsFor((NativeSessionNode)f.Input.Root)!.HasNoTypedValue);
    }
    /// <summary>Patterns on omitted anonymous members are compiled eagerly.</summary>
    /// <param name="pattern">Pattern source.</param><param name="expected">Expected compile outcome.</param>
    [Theory][InlineData("(","Invalid")][InlineData("\\p{L}","Unsupported")]
    public void EagerOmittedPattern(string pattern,string expected)
    {
        var model="<xs:all><xs:element name='a' type='xs:int'/><xs:element name='b' minOccurs='0'><xs:simpleType><xs:restriction base='xs:string'><xs:pattern value='"+pattern+"'/></xs:restriction></xs:simpleType></xs:element></xs:all>";
        var c=NativeSchemaSession.Create(Schema(model),Options);Assert.Equal(expected,c.Outcome.ToString());Assert.Null(c.Value);
    }
}
