// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Checks original complex/decimal provenance on nested simple-content terminals.
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

using Bosak.XPath.Api;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Checks actual native complex simple-content provenance and depth-four attribute ownership.</summary>
public sealed class NativeNestedSimpleContentCoreTests
{
    private static string Schema(string type="xs:decimal")=>"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='Money'><xs:restriction base='xs:decimal'><xs:minInclusive value='0'/></xs:restriction></xs:simpleType><xs:complexType name='Price'><xs:simpleContent><xs:extension base='"+type+"'><xs:attribute name='currency' type='xs:string' use='required'/></xs:extension></xs:simpleContent></xs:complexType><xs:complexType name='Line'><xs:sequence><xs:element name='price' type='Price'/></xs:sequence></xs:complexType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='Line' maxOccurs='2'/></xs:sequence></xs:complexType></xs:element><xs:element name='price' type='Price'/></xs:schema>";
    private static NativeSchemaProvider Provider(string type="xs:decimal",string? schema=null)=>new(NativeDecimalSchema.Compile(schema??Schema(type),new(256,256),maxStringCharacters:1024).Value!);
    private static SchemaQuerySession Session(NativeSchemaProvider p)=>SchemaQuerySession.CompileXPath("$value",new(2,16,1,256,256),assessor:p).Value;
    private static NativeSchemaProvider.Assessment Assess(SchemaQuerySession s,NativeSchemaInput i)=>(NativeSchemaProvider.Assessment)s.Advisor!.Assess(new(i,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);
    private sealed record Episode:ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }
    /// <summary>Original governing complex type, actual decimal type and normalized lexical node values stay distinct.</summary>
    /// <param name="type">Primitive or named decimal content type.</param>
    /// <param name="lexical">XML text inside the terminal.</param>
    /// <param name="expected">Exact normalized value.</param>
    /// <param name="normalized">Schema-collapsed lexical node text.</param>
    [Theory][InlineData("xs:decimal","12.50","12.5","12.50")][InlineData("xs:decimal"," 	+0012.50&#xD; ","12.5","+0012.50")][InlineData("xs:decimal","-0.00","0","-0.00")][InlineData("Money","12.50","12.5","12.50")]
    public void OriginalComplexTerminal_PreservesTypeAndNormalization(string type,string lexical,string expected,string normalized)
    {
        var p=Provider(type);using var s=Session(p);using var input=NativeSchemaInput.Capture("<r><a><price currency='EUR'>"+lexical+"</price></a></r>").Value!;var r=Assess(s,input);Assert.Equal(SchemaAssessmentStatus.Valid,r.Status);Assert.Single(r.ScalarFacts);var f=r.ScalarFacts[0];Assert.Same(input.ElementNodes[2],f.Node);Assert.True(f.Node.IsComplexType);Assert.False(f.HasNoTypedValue);Assert.Null(f.Node.SchemaElementDeclaration);Assert.Equal(("","Price"),f.Node.SchemaTypeAnnotation!.Value);Assert.Equal(normalized,f.Node.StringValue);Assert.Equal(normalized,XPath31Expression.Compile("string(.)").Evaluate(f.Node).StringValue);Assert.Equal(expected,XPath31Expression.Compile("data(.)").Evaluate(f.Node).ExactDecimalValue.ToInvariantString(256));
        Assert.Contains("FOTY0012",Assert.Throws<InvalidOperationException>(()=>XPath31Expression.Compile("data(.)").Evaluate(input.ElementNodes[1])).Message);
        using var snapshot=s.OpenSnapshot(input,input,r.Origin).Value;var actualType=type=="Money"?p.ResolveType(new("","Money"),default).Value:p.DecimalType;
        new NativeSessionRunner(s).Run([snapshot],op=>{var issued=f.ReadTypedValue(default).Value;Assert.Same(f.Type,issued.GoverningType);var read=op.ReadTypedValue(snapshot,issued);var value=op.Evaluate(new Bosak.XPath.Runtime.Vm.EvaluationContext().WithVariable("value",read.Value));Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(f.Type,evidence!.DeclaredType);Assert.Same(actualType,evidence.ActualType);Assert.NotSame(evidence.DeclaredType,evidence.ActualType);Assert.Same(r.Origin,evidence.OriginalAssessment);return 0;});
    }
    /// <summary>Local and global declarations share a named complex type without sharing declaration facts.</summary>
    [Fact]
    public void RootAndLocalPriceType_PreserveDeclarationIdentity()
    {
        var p=Provider();using var s=Session(p);using var local=NativeSchemaInput.Capture("<r><a><price currency='EUR'>1</price></a></r>").Value!;using var global=NativeSchemaInput.Capture("<price currency='USD'>2</price>").Value!;var a=Assess(s,local);var b=Assess(s,global);Assert.Same(a.ScalarFacts[0].Type,b.ElementFacts[0].Type);Assert.NotSame(a.ScalarFacts[0].Declaration,b.ElementFacts[0].Declaration);Assert.Null(a.ScalarFacts[0].Node.SchemaElementDeclaration);Assert.Equal(("","price"),b.ElementFacts[0].Node.SchemaElementDeclaration!.Value);
    }
    /// <summary>A scalar-bearing owner at depth four keeps its original node and three-segment attribute path.</summary>
    [Fact]
    public void DepthFourOwner_MapsOriginalAttributeNode()
    {
        var schema=Schema().Replace("<xs:element name='price' type='Price'/></xs:sequence></xs:complexType>","<xs:element name='b'><xs:complexType><xs:sequence><xs:element name='price' type='Price'/></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType>",StringComparison.Ordinal);
        var p=Provider(schema:schema);using var s=Session(p);using var input=NativeSchemaInput.Capture("<r><a><b><price currency='EUR'>1</price></b></a></r>").Value!;var r=Assess(s,input);Assert.Equal(SchemaAssessmentStatus.Valid,r.Status);var f=Assert.Single(r.DescendantAttributeFacts);Assert.Equal(new[]{0,0,0},f.OwnerElementPath);Assert.Same(input.ElementNodes[3],f.Node.Parent);Assert.Same(input.ElementNodes[3],Assert.Single(r.ScalarFacts).Node);
    }
    /// <summary>Failure after valid attributes never publishes a partial original assessment.</summary>
    /// <param name="lexical">Invalid content supplied after a valid currency.</param>
    [Theory][InlineData("bad")][InlineData("<v/>")][InlineData("1<!--x-->")]
    public void DeepContentFailure_PublishesNoFacts(string lexical)
    {
        var p=Provider();using var s=Session(p);using var input=NativeSchemaInput.Capture("<r><a><price currency='EUR'>"+lexical+"</price></a></r>").Value!;var r=Assess(s,input);Assert.NotEqual(SchemaAssessmentStatus.Valid,r.Status);Assert.False(input.IsAssessed);Assert.Null(r.Evidence);Assert.Empty(r.Facts);
    }
}
