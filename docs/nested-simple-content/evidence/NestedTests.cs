// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Probes nested original node facts through published Core APIs.
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
//                      | Charles Korthout | 0.2   | 19-11-2026     | Probe public original attribute facts, typed reads and ownership refusal                   |
//                      | Charles Korthout | 0.3   | 19-11-2026     | Keep synthetic attribute declaration identity distinct from type identities                |
//                      | Charles Korthout | 0.4   | 20-11-2026     | Qualify complex simple-content root facts with separate actual decimal types               |
//                      | Charles Korthout | 0.5   | 20-11-2026     | Probe nested navigation, container refusal and original leaf facts                         |
//                      | Charles Korthout | 0.6   | 20-11-2026     | Register exact original node/fact bindings in the nested transport issuer                  |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace AttributeTransportProbe;

/// <summary>Qualifies nested original facts with a synthetic issuer, not an XSD validator.</summary>
public sealed class NestedTests
{
    private static Fixture Open(string expression="data($node)")
    {
        var f=new Fixture(expression,true);var sharedType=new Handle(f.Provider);var firstDecl=new Handle(f.Provider);var secondDecl=new Handle(f.Provider);
        f.Provider.Extra.UnionWith([sharedType,firstDecl,secondDecl]);
        foreach(var node in f.Input.Nodes)
        {
            var type=node.IsComplexType?sharedType:f.Provider.Declared;
            var declaration=node.LocalName=="quantity"?secondDecl:firstDecl;
            ProbeValue? issued=node.HasNoTypedValue?null:f.Issue(node.LocalName switch
            {
                "amount"=>XdmValue.FromDecimal(new XdmExactDecimal(node.StringValue=="12.50"?1250:375,2,256,256)),
                "quantity"=>XdmValue.FromInteger(System.Numerics.BigInteger.Parse(node.StringValue)),
                _=>XdmValue.FromBoolean(false)
            });
            var facts=new ProbeFacts(type,declaration,f.Provider.Advisor!.Origin,issued);node.Facts=facts;f.Provider.Advisor.RegisterNode(node,facts);
        }
        return f;
    }
    private static XdmValue Evaluate(Fixture f,ProbeAttributeNode node)=>f.Session.Run([f.Snapshot],op=>op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(node))));

    /// <summary>Reads each nested leaf from its original issued facts, despite interleaved containers.</summary>
    /// <param name="index">Preorder node index.</param>
    /// <param name="lexical">Expected exact value.</param>
    [Theory][InlineData(2,"12.5")][InlineData(3,"2")][InlineData(5,"3.75")][InlineData(6,"1")][InlineData(7,"false")]
    public void NestedLeafData_PreservesPayloadAndOrigin(int index,string lexical)
    {
        using var f=Open();var node=f.Input.Nodes[index];var value=Evaluate(f,node);
        var actual=value.Kind==XdmValueKind.Decimal?value.ExactDecimalValue.ToInvariantString(256):value.Kind==XdmValueKind.Integer?value.BigIntegerValue.ToString():value.BooleanValue?"true":"false";
        Assert.Equal(lexical,actual);Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(f.Provider.Advisor!.Origin,evidence!.OriginalAssessment);
    }
    /// <summary>Each element-only container refuses atomization without losing leaf usability.</summary>
    /// <param name="index">Container preorder index.</param>
    [Theory][InlineData(0)][InlineData(1)][InlineData(4)]
    public void ContainerData_RefusesAndLeafRecovers(int index)
    {
        using var f=Open();var node=f.Input.Nodes[index];Assert.True(node.OriginalSchemaFacts.HasNoTypedValue);
        var error=Assert.Throws<InvalidOperationException>(()=>Evaluate(f,node));Assert.Contains("FOTY0012",error.Message);
        Assert.Equal("12.5",Evaluate(f,f.Input.Nodes[2]).ExactDecimalValue.ToInvariantString(256));
    }
    /// <summary>Parent/children traversal returns the original wrapped nodes and distinct local declarations.</summary>
    [Fact]
    public void Navigation_RetainsOriginalIdentityAndDeclarations()
    {
        using var f=Open();var first=f.Input.Nodes[1];var second=f.Input.Nodes[4];
        Assert.Same(f.Input.RootNode,first.Parent);Assert.Same(first,f.Input.Nodes[2].Parent);Assert.Same(second,f.Input.Nodes[5].Parent);
        Assert.Same(first.OriginalSchemaFacts.Type,second.OriginalSchemaFacts.Type);
        Assert.NotSame(f.Input.Nodes[2].OriginalSchemaFacts.Declaration,f.Input.Nodes[3].OriginalSchemaFacts.Declaration);
        var children=new List<IXdmNode>();foreach(var value in first.Children(XdmNodeKind.Element))children.Add(value.NodeValue);
        Assert.Same(f.Input.Nodes[2],children[0]);Assert.Same(f.Input.Nodes[3],children[1]);
    }
    /// <summary>Foreign nested nodes and replaced foreign facts cannot authenticate.</summary>
    [Fact]
    public void ForeignNestedNodeAndFacts_AreRefused()
    {
        using var a=Open();using var b=Open();Assert.Throws<ArgumentException>(()=>Evaluate(b,a.Input.Nodes[2]));
        b.Input.Nodes[2].Facts=a.Input.Nodes[2].Facts;Assert.Throws<ArgumentException>(()=>Evaluate(b,b.Input.Nodes[2]));
    }
    /// <summary>Metadata alone does not manufacture a validated nested leaf.</summary>
    [Fact]
    public void MissingNestedFacts_AreRefused()
    {
        using var f=Open();f.Input.Nodes[2].Facts=null;Assert.Throws<InvalidOperationException>(()=>Evaluate(f,f.Input.Nodes[2]));
    }
    /// <summary>Snapshot closure revokes nested provenance.</summary>
    [Fact]
    public void NestedEvidence_ExpiresWithSnapshot()
    {
        using var f=Open();var v=Evaluate(f,f.Input.Nodes[2]);Assert.True(SchemaAtomics.TryGetProvenance(v,out var evidence));f.Snapshot.Dispose();Assert.Throws<ObjectDisposedException>(()=>evidence!.ActualType);
    }
    /// <summary>Input closure rejects further nested reads.</summary>
    [Fact]
    public void ClosedNestedInput_IsRefused()
    {
        using var f=Open();f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>Evaluate(f,f.Input.Nodes[2]));
    }
}
