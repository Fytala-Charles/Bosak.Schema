// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Tests actual IEEE and URI provenance, disjoint equality and finite budgets.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 26-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Fixture=Bosak.Schema.Tests.NodeData.Fixture;
using Xunit;
namespace Bosak.Schema.Tests;
/// <summary>Verifies actual native IEEE and URI facts and value-space boundaries.</summary>
public sealed class NativeNumericUriProofTests
{
    /// <summary>Checks original nodes, actual named/member proof, revocation and foreign refusal.</summary>
    /// <param name="kind">Builtin family.</param><param name="named">Alias selector.</param><param name="union">Union selector.</param>
    [Theory]
    [InlineData("float",false,false)][InlineData("float",true,false)][InlineData("float",false,true)]
    [InlineData("double",false,false)][InlineData("double",true,false)][InlineData("double",false,true)]
    [InlineData("anyURI",false,false)][InlineData("anyURI",true,false)][InlineData("anyURI",false,true)]
    public void Provenance(string kind,bool named,bool union)
    {
        string schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='N'><xs:restriction base='xs:"+kind+"'/></xs:simpleType><xs:simpleType name='U'><xs:union memberTypes='N xs:string'/></xs:simpleType><xs:element name='r' type='"+(union?"U":named?"N":"xs:"+kind)+"'/></xs:schema>";
        string xml=kind=="anyURI"?"<r> ../a%xx </r>":"<r>-0</r>";
        using var f=new Fixture(schema,xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Same(f.Input.Root,fact.Node);
        var typed=fact.ReadTypedValue(default).Value;Assert.True(typed.TryGetSingleAtomic(out var cell));
        Assert.Equal(kind=="float"?XdmValueKind.Float:kind=="double"?XdmValueKind.Double:XdmValueKind.String,cell.Value.Kind);
        if(kind=="anyURI"){Assert.Equal("../a%xx",cell.Value.StringValue);Assert.Equal("anyURI",cell.Value.SchemaTypeName);}
        else Assert.Equal(long.MinValue,BitConverter.DoubleToInt64Bits(cell.Value.DoubleValue));
        Assert.Same(f.Provider.TypeComponent(union?fact.NativeType.UnionMembers[0]:fact.NativeType),cell.TypeEvidence.ActualType);
        f.Run(op=>{var atom=f.Evaluate(op,f.Input.Root);Assert.True(SchemaAtomics.TryGetProvenance(atom,out var proof));Assert.Same(cell.TypeEvidence.ActualType,proof!.ActualType);Assert.Same(f.Assessment.Origin,proof.OriginalAssessment);return true;});
        using var other=new Fixture(schema,xml);Assert.False(other.Session.Advisor!.OwnsTypedValue(typed,other.Input,null));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>typed.TryGetSingleAtomic(out _));
    }
    /// <summary>Checks schema equality uses disjoint primitive spaces and singleton-list equality.</summary>
    [Fact]
    public void DisjointEquality()
    {
        NativeElementState State(string kind,string text)
        {
            var schema=NativeDecimalSchema.Compile("<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:"+kind+"'/></xs:schema>",new(256,256),maxStringCharacters:256).Value!;
            return schema.Assess("<r>"+text+"</r>").Value!.Elements[0];
        }
        var f=State("float","1");var d=State("double","1");var dec=State("decimal","1");var uri=State("anyURI","1");var str=State("string","1");
        Assert.False(NativeCompoundEquality.Equal(f,d));Assert.False(NativeCompoundEquality.Equal(d,dec));Assert.False(NativeCompoundEquality.Equal(f,dec));Assert.False(NativeCompoundEquality.Equal(uri,str));
        Assert.True(NativeCompoundEquality.Equal(State("double","-0"),State("double","0")));Assert.True(NativeCompoundEquality.Equal(State("float","NaN"),State("float","NaN")));Assert.False(NativeCompoundEquality.Equal(State("float","NaN"),State("double","NaN")));
        Assert.True(NativeCompoundEquality.Equal(f,f with { Items=[f] }));Assert.False(NativeCompoundEquality.Equal(f,d with { Items=[d] }));
    }
    /// <summary>Rejects inconsistent inherited opposite endpoints before assessing any value.</summary>
    /// <param name="upper">Upper-side selector.</param><param name="exclusive">Exclusive base selector.</param>
    [Theory][InlineData(false,false)][InlineData(true,false)][InlineData(false,true)][InlineData(true,true)]
    public void OppositeInheritedEndpoints(bool upper,bool exclusive)
    {
        var baseline=(upper?"min":"max")+(exclusive?"Exclusive":"Inclusive");var derived=(upper?"max":"min")+"Exclusive";
        var xml="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='B'><xs:restriction base='xs:double'><xs:"+baseline+" value='1'/></xs:restriction></xs:simpleType><xs:simpleType name='T'><xs:restriction base='B'><xs:"+derived+" value='1'/></xs:restriction></xs:simpleType><xs:element name='r' type='T'/></xs:schema>";
        Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(xml,new(256,256),maxStringCharacters:256).Outcome);
    }
    /// <summary>Preserves canonical shared components and distinguishes complex governing and atomic types.</summary>
    /// <param name="kind">Primitive name.</param>
    [Theory][InlineData("float")][InlineData("double")][InlineData("anyURI")]
    public void Canonical(string kind)
    {
        var xml="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='T'><xs:restriction base='xs:"+kind+"'/></xs:simpleType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='T'/><xs:element name='b' type='T'/><xs:element name='c'><xs:simpleType><xs:restriction base='T'/></xs:simpleType></xs:element></xs:sequence></xs:complexType></xs:element></xs:schema>";
        using var f=new Fixture(xml,"<r><a>1</a><b>2</b><c>3</c></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var children=new List<NativeSessionNode>();foreach(var child in f.Input.Root.Children(XdmNodeKind.Element))children.Add((NativeSessionNode)child.NodeValue);
        var a=f.Input.FactsFor(children[0])!;var b=f.Input.FactsFor(children[1])!;var c=f.Input.FactsFor(children[2])!;
        Assert.Same(a.Type,b.Type);Assert.NotSame(a.Type,c.Type);Assert.Same(a.NativeType,c.NativeType.DerivationBase);
        using var complex=new Fixture("<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:simpleContent><xs:extension base='xs:"+kind+"'/></xs:simpleContent></xs:complexType></xs:element></xs:schema>","<r>1</r>");
        var fact=complex.Input.FactsFor((NativeSessionNode)complex.Input.Root)!;Assert.True(fact.ReadTypedValue(default).Value.TryGetSingleAtomic(out var item));Assert.NotSame(fact.Type,item.TypeEvidence.ActualType);Assert.Equal(kind,fact.NativeType.ContentType!.Name);
    }
    /// <summary>Checks finite pattern work, cancellation and successful recovery.</summary>
    [Fact]
    public void Budget()
    {
        var schema=NativeDecimalSchema.Compile("<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='T'><xs:restriction base='xs:double'><xs:pattern value='1+'/></xs:restriction></xs:simpleType><xs:element name='r' type='T'/></xs:schema>",new(256,256),maxStringCharacters:256).Value!;
        var facets=schema.Root.Type.FloatingFacets!;var v=NativeFloatingValue.Parse("111",NativeFloatingKind.Double,256);
        Assert.Equal(NativeOutcome.Unsupported,Assert.Throws<NativeProfileException>(()=>facets.Accepts(v,"111",new(default,1))).Outcome);
        using var c=new CancellationTokenSource();c.Cancel();Assert.ThrowsAny<OperationCanceledException>(()=>facets.Accepts(v,"111",new(c.Token)));Assert.True(facets.Accepts(v,"111",new(default)));
    }
}
